using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators;

[Generator]
public sealed class SettingsPopulatorGenerator : IIncrementalGenerator
{
    private const string SdSettingsAttrFqn       = "Cmpnnt.StreamDeckToolkit.Attributes.SdSettingsAttribute";
    private const string JsonPropNameAttrFqn      = "System.Text.Json.Serialization.JsonPropertyNameAttribute";
    private const string FilenameAttrFqn          = "Cmpnnt.StreamDeckToolkit.Attributes.FilenamePropertyAttribute";
    private const string JsonSerializableAttrFqn  = "System.Text.Json.Serialization.JsonSerializableAttribute";
    private const string JsonSerializerContextFqn = "System.Text.Json.Serialization.JsonSerializerContext";
    private const string FilenameNoFileString     = "No file...";

    private static readonly DiagnosticDescriptor Sd001 = new(
        id: "SD001",
        title: "Complex type not registered in JsonSerializerContext",
        messageFormat: "Property '{0}' of type '{1}' on settings class '{2}' requires a JsonSerializerContext registration. " +
                       "Add [JsonSerializable(typeof({1}))] to your context.",
        category: "SdTools.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDecls = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (n, _) => n is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
            transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node);

        var compilationAndClasses = context.CompilationProvider.Combine(classDecls.Collect());
        context.RegisterSourceOutput(compilationAndClasses, Execute);
    }

    // -------------------------------------------------------------------------
    // Execution
    // -------------------------------------------------------------------------

    private static void Execute(
        SourceProductionContext spc,
        (Compilation Compilation, ImmutableArray<ClassDeclarationSyntax> Classes) source)
    {
        var (compilation, classes) = source;

        var sdSettingsAttr       = compilation.GetTypeByMetadataName(SdSettingsAttrFqn);
        if (sdSettingsAttr == null)
        {
            return;
        }

        var jsonPropNameAttr     = compilation.GetTypeByMetadataName(JsonPropNameAttrFqn);
        var filenameAttr         = compilation.GetTypeByMetadataName(FilenameAttrFqn);
        var jsonSerializerCtxBase = compilation.GetTypeByMetadataName(JsonSerializerContextFqn);
        var jsonSerializableAttr  = compilation.GetTypeByMetadataName(JsonSerializableAttrFqn);

        // Build: fully-qualified type name → "Context.Default.TypePropertyName"
        var contextLookup = BuildContextLookup(
            compilation, jsonSerializerCtxBase, jsonSerializableAttr);

        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var classDecl in classes)
        {
            var model = compilation.GetSemanticModel(classDecl.SyntaxTree);
            if (model.GetDeclaredSymbol(classDecl) is not INamedTypeSymbol classSymbol)
            {
                continue;
            }

            bool tagged = classSymbol.GetAttributes()
                .Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, sdSettingsAttr));
            if (!tagged)
            {
                continue;
            }

            // De-duplicate partial class declarations
            string key = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (!seen.Add(key))
            {
                continue;
            }

            var properties = CollectProperties(
                classSymbol, jsonPropNameAttr, filenameAttr, contextLookup, spc);

            string sourceText = GenerateSource(classSymbol, properties);
            spc.AddSource(
                $"{classSymbol.Name}.PopulateFromJson.g.cs",
                SourceText.From(sourceText, Encoding.UTF8));
        }
    }

    // -------------------------------------------------------------------------
    // JsonSerializerContext lookup
    // -------------------------------------------------------------------------

    private static Dictionary<string, string> BuildContextLookup(
        Compilation compilation,
        INamedTypeSymbol? jsonSerializerCtxBase,
        INamedTypeSymbol? jsonSerializableAttr)
    {
        var lookup = new Dictionary<string, string>(StringComparer.Ordinal);
        if (jsonSerializerCtxBase == null || jsonSerializableAttr == null)
        {
            return lookup;
        }

        foreach (var type in AllNamedTypes(compilation.GlobalNamespace))
        {
            if (!InheritsFrom(type, jsonSerializerCtxBase))
            {
                continue;
            }

            string contextFqn = type
                .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                .Replace("global::", "");

            foreach (var attr in type.GetAttributes())
            {
                if (!SymbolEqualityComparer.Default.Equals(attr.AttributeClass, jsonSerializableAttr))
                {
                    continue;
                }
                if (attr.ConstructorArguments.Length == 0)
                {
                    continue;
                }
                if (attr.ConstructorArguments[0].Value is not INamedTypeSymbol registeredType)
                {
                    continue;
                }

                string registeredFqn = registeredType
                    .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                // The generated property on the context is the simple type name
                lookup[registeredFqn] = $"{contextFqn}.Default.{registeredType.Name}";
            }
        }

        return lookup;
    }

    private static bool InheritsFrom(INamedTypeSymbol type, INamedTypeSymbol baseType)
    {
        var cur = type.BaseType;
        while (cur != null)
        {
            if (SymbolEqualityComparer.Default.Equals(cur.OriginalDefinition, baseType.OriginalDefinition))
                return true;
            cur = cur.BaseType;
        }
        return false;
    }

    private static IEnumerable<INamedTypeSymbol> AllNamedTypes(INamespaceSymbol ns)
    {
        foreach (var t in ns.GetTypeMembers())
        {
            yield return t;
            foreach (var n in NestedTypes(t)) yield return n;
        }
        foreach (var child in ns.GetNamespaceMembers())
            foreach (var t in AllNamedTypes(child)) yield return t;
    }

    private static IEnumerable<INamedTypeSymbol> NestedTypes(INamedTypeSymbol type)
    {
        foreach (var nested in type.GetTypeMembers())
        {
            yield return nested;
            foreach (var n in NestedTypes(nested)) yield return n;
        }
    }

    // -------------------------------------------------------------------------
    // Property collection
    // -------------------------------------------------------------------------

    private enum PropKind
    {
        String,
        Bool, NullableBool,
        Int, NullableInt,
        Long, NullableLong,
        Float, NullableFloat,
        Double, NullableDouble,
        Decimal, NullableDecimal,
        Guid, NullableGuid,
        DateTime, NullableDateTime,
        DateTimeOffset, NullableDateTimeOffset,
        Enum, NullableEnum,
        Complex,
        Unknown
    }

    private sealed record SettingsProp(
        string CSharpName,
        string JsonKey,
        PropKind Kind,
        bool IsFilename,
        string? EnumFqn,      // PropKind.Enum / NullableEnum
        string? ContextExpr); // PropKind.Complex

    private static IReadOnlyList<SettingsProp> CollectProperties(
        INamedTypeSymbol classSymbol,
        INamedTypeSymbol? jsonPropNameAttr,
        INamedTypeSymbol? filenameAttr,
        IReadOnlyDictionary<string, string> contextLookup,
        SourceProductionContext spc)
    {
        var result = new List<SettingsProp>();

        foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
        {
            if (member.DeclaredAccessibility != Accessibility.Public)
            {
                continue;
            }
            if (member.IsStatic)
            {
                continue;
            }
            if (member.SetMethod == null)
            {
                continue;
            }

            string jsonKey = GetJsonKey(member, jsonPropNameAttr);

            bool isFilename = filenameAttr != null &&
                member.GetAttributes()
                    .Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, filenameAttr));

            var (kind, enumFqn, ctxExpr) = ClassifyType(
                member.Type, contextLookup, spc, member.Name, classSymbol.Name);

            if (kind == PropKind.Unknown)
            {
                continue;
            }

            result.Add(new SettingsProp(member.Name, jsonKey, kind, isFilename, enumFqn, ctxExpr));
        }

        return result;
    }

    private static string GetJsonKey(IPropertySymbol prop, INamedTypeSymbol? jsonPropNameAttr)
    {
        if (jsonPropNameAttr != null)
        {
            var attr = prop.GetAttributes()
                .FirstOrDefault(a => SymbolEqualityComparer.Default
                    .Equals(a.AttributeClass, jsonPropNameAttr));

            if (attr?.ConstructorArguments.Length > 0 &&
                attr.ConstructorArguments[0].Value is string name &&
                !string.IsNullOrEmpty(name))
            {
                return name;
            }
        }

        return JsonNamingPolicy.CamelCase.ConvertName(prop.Name);
    }

    private static (PropKind Kind, string? EnumFqn, string? CtxExpr) ClassifyType(
        ITypeSymbol typeSymbol,
        IReadOnlyDictionary<string, string> contextLookup,
        SourceProductionContext spc,
        string propName,
        string className)
    {
        // Unwrap Nullable<T>
        bool nullable = false;
        ITypeSymbol inner = typeSymbol;

        if (typeSymbol is INamedTypeSymbol named &&
            named.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
        {
            nullable = true;
            inner = named.TypeArguments[0];
        }

        switch (inner.SpecialType)
        {
            case SpecialType.System_String:   return (PropKind.String,  null, null);
            case SpecialType.System_Boolean:  return (nullable ? PropKind.NullableBool    : PropKind.Bool,    null, null);
            case SpecialType.System_Int32:    return (nullable ? PropKind.NullableInt     : PropKind.Int,     null, null);
            case SpecialType.System_Int64:    return (nullable ? PropKind.NullableLong    : PropKind.Long,    null, null);
            case SpecialType.System_Single:   return (nullable ? PropKind.NullableFloat   : PropKind.Float,   null, null);
            case SpecialType.System_Double:   return (nullable ? PropKind.NullableDouble  : PropKind.Double,  null, null);
            case SpecialType.System_Decimal:  return (nullable ? PropKind.NullableDecimal : PropKind.Decimal, null, null);
        }

        string innerFqn = inner.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        switch (innerFqn)
        {
            case "global::System.Guid":           return (nullable ? PropKind.NullableGuid           : PropKind.Guid,           null, null);
            case "global::System.DateTime":       return (nullable ? PropKind.NullableDateTime       : PropKind.DateTime,       null, null);
            case "global::System.DateTimeOffset": return (nullable ? PropKind.NullableDateTimeOffset : PropKind.DateTimeOffset, null, null);
        }

        if (inner.TypeKind == TypeKind.Enum)
        {
            string enumFqn = innerFqn.Replace("global::", "");
            return (nullable ? PropKind.NullableEnum : PropKind.Enum, enumFqn, null);
        }

        // Complex — try to resolve via a JsonSerializerContext
        string typeFqn = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        if (contextLookup.TryGetValue(typeFqn, out string? ctxExpr))
            return (PropKind.Complex, null, ctxExpr);

        spc.ReportDiagnostic(Diagnostic.Create(
            Sd001, Location.None, propName, inner.ToDisplayString(), className));
        return (PropKind.Unknown, null, null);
    }

    // -------------------------------------------------------------------------
    // Code generation
    // -------------------------------------------------------------------------

    private static string GenerateSource(
        INamedTypeSymbol classSymbol,
        IReadOnlyList<SettingsProp> properties)
    {
        string ns = classSymbol.ContainingNamespace?.IsGlobalNamespace == false
            ? classSymbol.ContainingNamespace.ToDisplayString()
            : "";

        string accessibility = classSymbol.DeclaredAccessibility switch
        {
            Accessibility.Public             => "public",
            Accessibility.Internal           => "internal",
            Accessibility.Private            => "private",
            Accessibility.Protected          => "protected",
            Accessibility.ProtectedAndInternal => "private protected",
            Accessibility.ProtectedOrInternal  => "protected internal",
            _                                => "internal"
        };

        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable disable");
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Text.Json;");
        sb.AppendLine("using Cmpnnt.StreamDeckToolkit;");
        sb.AppendLine();

        bool hasNs = !string.IsNullOrEmpty(ns);
        if (hasNs) { sb.AppendLine($"namespace {ns}"); sb.AppendLine("{"); }

        sb.AppendLine($"    {accessibility} partial class {classSymbol.Name} : ISettingsPopulatable");
        sb.AppendLine("    {");
        sb.AppendLine("        public int PopulateFromJson(JsonElement element)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (element.ValueKind != JsonValueKind.Object)");
        sb.AppendLine("                return 0;");
        sb.AppendLine();
        sb.AppendLine("            int count = 0;");
        sb.AppendLine("            foreach (JsonProperty prop in element.EnumerateObject())");
        sb.AppendLine("            {");
        sb.AppendLine("                switch (prop.Name)");
        sb.AppendLine("                {");

        foreach (var prop in properties)
            AppendCase(sb, prop);

        sb.AppendLine("                }");
        sb.AppendLine("            }");
        sb.AppendLine("            return count;");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        if (hasNs)
        {
            sb.AppendLine("}");
        }

        return sb.ToString();
    }

    private static void AppendCase(StringBuilder sb, SettingsProp prop)
    {
        sb.AppendLine($"                    case \"{Escape(prop.JsonKey)}\":");

        if (prop.IsFilename)
        {
            AppendFilenameCase(sb, prop.CSharpName);
        }
        else
        {
            AppendTypedCase(sb, prop);
        }

        sb.AppendLine("                        break;");
    }

    private static void AppendFilenameCase(StringBuilder sb, string name)
    {
        sb.AppendLine("                        if (prop.Value.ValueKind == JsonValueKind.String)");
        sb.AppendLine("                        {");
        sb.AppendLine("                            string _raw = prop.Value.GetString();");
        sb.AppendLine("                            if (_raw != null)");
        sb.AppendLine("                            {");
        sb.AppendLine($"                                {name} = _raw == \"{FilenameNoFileString}\"");
        sb.AppendLine("                                    ? string.Empty");
        sb.AppendLine("                                    : Uri.UnescapeDataString(_raw.Replace(\"C:\\\\fakepath\\\\\", \"\"));");
        sb.AppendLine("                                count++;");
        sb.AppendLine("                            }");
        sb.AppendLine("                        }");
    }

    private static void AppendTypedCase(StringBuilder sb, SettingsProp prop)
    {
        switch (prop.Kind)
        {
            case PropKind.String:
                sb.AppendLine("                        if (prop.Value.ValueKind == JsonValueKind.String || prop.Value.ValueKind == JsonValueKind.Null)");
                sb.AppendLine("                        {");
                sb.AppendLine($"                            {prop.CSharpName} = prop.Value.ValueKind == JsonValueKind.Null ? null : prop.Value.GetString();");
                sb.AppendLine("                            count++;");
                sb.AppendLine("                        }");
                break;

            case PropKind.Bool:
                sb.AppendLine("                        if (prop.Value.ValueKind == JsonValueKind.True || prop.Value.ValueKind == JsonValueKind.False)");
                sb.AppendLine("                        {");
                sb.AppendLine($"                            {prop.CSharpName} = prop.Value.GetBoolean();");
                sb.AppendLine("                            count++;");
                sb.AppendLine("                        }");
                break;

            case PropKind.NullableBool:
                sb.AppendLine("                        if (prop.Value.ValueKind == JsonValueKind.Null)");
                sb.AppendLine($"                        {{ {prop.CSharpName} = null; count++; }}");
                sb.AppendLine("                        else if (prop.Value.ValueKind == JsonValueKind.True || prop.Value.ValueKind == JsonValueKind.False)");
                sb.AppendLine($"                        {{ {prop.CSharpName} = prop.Value.GetBoolean(); count++; }}");
                break;

            case PropKind.Int:           AppendSimple(sb, prop.CSharpName, "JsonValueKind.Number", "GetInt32()");    break;
            case PropKind.NullableInt:   AppendNullable(sb, prop.CSharpName, "JsonValueKind.Number", "GetInt32()");  break;
            case PropKind.Long:          AppendSimple(sb, prop.CSharpName, "JsonValueKind.Number", "GetInt64()");    break;
            case PropKind.NullableLong:  AppendNullable(sb, prop.CSharpName, "JsonValueKind.Number", "GetInt64()");  break;
            case PropKind.Float:         AppendSimple(sb, prop.CSharpName, "JsonValueKind.Number", "GetSingle()");   break;
            case PropKind.NullableFloat: AppendNullable(sb, prop.CSharpName, "JsonValueKind.Number", "GetSingle()"); break;
            case PropKind.Double:        AppendSimple(sb, prop.CSharpName, "JsonValueKind.Number", "GetDouble()");   break;
            case PropKind.NullableDouble:AppendNullable(sb, prop.CSharpName, "JsonValueKind.Number", "GetDouble()"); break;
            case PropKind.Decimal:       AppendSimple(sb, prop.CSharpName, "JsonValueKind.Number", "GetDecimal()");  break;
            case PropKind.NullableDecimal: AppendNullable(sb, prop.CSharpName, "JsonValueKind.Number", "GetDecimal()"); break;

            case PropKind.Guid:              AppendSimple(sb, prop.CSharpName, "JsonValueKind.String", "GetGuid()");         break;
            case PropKind.NullableGuid:      AppendNullable(sb, prop.CSharpName, "JsonValueKind.String", "GetGuid()");       break;
            case PropKind.DateTime:          AppendSimple(sb, prop.CSharpName, "JsonValueKind.String", "GetDateTime()");     break;
            case PropKind.NullableDateTime:  AppendNullable(sb, prop.CSharpName, "JsonValueKind.String", "GetDateTime()");   break;
            case PropKind.DateTimeOffset:    AppendSimple(sb, prop.CSharpName, "JsonValueKind.String", "GetDateTimeOffset()"); break;
            case PropKind.NullableDateTimeOffset: AppendNullable(sb, prop.CSharpName, "JsonValueKind.String", "GetDateTimeOffset()"); break;

            case PropKind.Enum:        AppendEnum(sb, prop.CSharpName, prop.EnumFqn!, nullable: false); break;
            case PropKind.NullableEnum: AppendEnum(sb, prop.CSharpName, prop.EnumFqn!, nullable: true); break;

            case PropKind.Complex:
                sb.AppendLine($"                        {prop.CSharpName} = prop.Value.Deserialize({prop.ContextExpr});");
                sb.AppendLine("                        count++;");
                break;
        }
    }

    private static void AppendSimple(StringBuilder sb, string name, string kind, string getter)
    {
        sb.AppendLine($"                        if (prop.Value.ValueKind == {kind})");
        sb.AppendLine($"                        {{ {name} = prop.Value.{getter}; count++; }}");
    }

    private static void AppendNullable(StringBuilder sb, string name, string kind, string getter)
    {
        sb.AppendLine("                        if (prop.Value.ValueKind == JsonValueKind.Null)");
        sb.AppendLine($"                        {{ {name} = null; count++; }}");
        sb.AppendLine($"                        else if (prop.Value.ValueKind == {kind})");
        sb.AppendLine($"                        {{ {name} = prop.Value.{getter}; count++; }}");
    }

    private static void AppendEnum(StringBuilder sb, string name, string enumFqn, bool nullable)
    {
        if (nullable)
        {
            sb.AppendLine("                        if (prop.Value.ValueKind == JsonValueKind.Null)");
            sb.AppendLine($"                        {{ {name} = null; count++; }}");
            sb.AppendLine("                        else if (prop.Value.ValueKind == JsonValueKind.String)");
        }
        else
        {
            sb.AppendLine("                        if (prop.Value.ValueKind == JsonValueKind.String)");
        }
        sb.AppendLine("                        {");
        sb.AppendLine("                            string _eraw = prop.Value.GetString();");
        sb.AppendLine($"                            if (_eraw != null && Enum.TryParse<{enumFqn}>(_eraw, ignoreCase: true, out var _eval))");
        sb.AppendLine($"                            {{ {name} = _eval; count++; }}");
        sb.AppendLine("                        }");
        sb.AppendLine("                        else if (prop.Value.ValueKind == JsonValueKind.Number)");
        sb.AppendLine($"                        {{ {name} = ({enumFqn})prop.Value.GetInt32(); count++; }}");
    }

    private static string Escape(string s) =>
        s.Replace("\\", "\\\\").Replace("\"", "\\\"");
}

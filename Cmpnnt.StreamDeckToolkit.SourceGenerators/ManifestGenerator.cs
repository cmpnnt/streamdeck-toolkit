using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Templates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators;

[Generator]
public class ManifestModelSourceGenerator : IIncrementalGenerator
{
    private const string COMMON_PLUGIN_FUNCTIONS = "Cmpnnt.StreamDeckToolkit.Actions.ICommonPluginFunctions";
    private const string KEYPAD_PLUGIN = "Cmpnnt.StreamDeckToolkit.Actions.IKeypadPlugin";
    private const string ENCODER_PLUGIN = "Cmpnnt.StreamDeckToolkit.Actions.IEncoderPlugin";
    private const string MANIFEST_CONFIG_BASE = "Cmpnnt.StreamDeckToolkit.Manifest.ManifestConfigBase";
    private const string STREAM_DECK_PLUGIN_ATTR = "Cmpnnt.StreamDeckToolkit.Attributes.StreamDeckPluginAttribute";
    private const string STREAM_DECK_ACTION_ATTR = "Cmpnnt.StreamDeckToolkit.Attributes.StreamDeckActionAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var msbuildProps = context.AnalyzerConfigOptionsProvider.Select(
            static (opts, _) => GetMsbuildProps(opts));

        var compilationAndProps = context.CompilationProvider.Combine(msbuildProps);

        context.RegisterSourceOutput(compilationAndProps,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static MsbuildProps GetMsbuildProps(AnalyzerConfigOptionsProvider opts)
    {
        opts.GlobalOptions.TryGetValue("build_property.AssemblyName", out var assemblyName);
        opts.GlobalOptions.TryGetValue("build_property.Version", out var version);
        opts.GlobalOptions.TryGetValue("build_property.Authors", out var authors);
        opts.GlobalOptions.TryGetValue("build_property.Description", out var description);
        opts.GlobalOptions.TryGetValue("build_property.PackageProjectUrl", out var packageProjectUrl);
        // MSBuild returns "" for unset properties; normalise to null so the template can use ?? cleanly
        return new MsbuildProps(
            NullIfEmpty(assemblyName),
            NullIfEmpty(version),
            NullIfEmpty(authors),
            NullIfEmpty(description),
            NullIfEmpty(packageProjectUrl));
    }

    private static string? NullIfEmpty(string? s) => string.IsNullOrEmpty(s) ? null : s;

    private static void Execute(Compilation compilation, MsbuildProps msbuildProps, SourceProductionContext context)
    {
        INamedTypeSymbol? commonInterface = compilation.GetTypeByMetadataName(COMMON_PLUGIN_FUNCTIONS);
        INamedTypeSymbol? keypadInterface = compilation.GetTypeByMetadataName(KEYPAD_PLUGIN);
        INamedTypeSymbol? encoderInterface = compilation.GetTypeByMetadataName(ENCODER_PLUGIN);

        if (commonInterface == null) return;

        PluginAttrData? pluginAttr = GetPluginAttribute(compilation);
        List<ActionInfo> actions = GetActionInfos(compilation, commonInterface, keypadInterface, encoderInterface);

        if (actions.Count == 0) return;

        string? configClassName = GetConfigClassName(compilation);

        var input = new ManifestGeneratorInput(msbuildProps, pluginAttr, actions.ToImmutableArray(), configClassName);
        string source = ManifestProviderTemplate.Generate(input);
        context.AddSource("GeneratedManifestModel.g.cs", SourceText.From(source, Encoding.UTF8));
    }

    private static PluginAttrData? GetPluginAttribute(Compilation compilation)
    {
        // Check assembly-level attribute first
        AttributeData? attrData = FindAttribute(compilation.Assembly.GetAttributes(), STREAM_DECK_PLUGIN_ATTR);

        // Fall back to class-level attribute
        if (attrData == null)
        {
            foreach (SyntaxTree syntaxTree in compilation.SyntaxTrees)
            {
                SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
                foreach (ClassDeclarationSyntax classDecl in syntaxTree.GetRoot()
                             .DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    if (semanticModel.GetDeclaredSymbol(classDecl) is INamedTypeSymbol classSymbol)
                    {
                        attrData = FindAttribute(classSymbol.GetAttributes(), STREAM_DECK_PLUGIN_ATTR);
                        if (attrData != null) break;
                    }
                }
                if (attrData != null) break;
            }
        }

        return attrData != null ? ParsePluginAttrData(attrData) : null;
    }

    private static PluginAttrData ParsePluginAttrData(AttributeData attrData)
    {
        var args = attrData.NamedArguments.ToDictionary(kv => kv.Key, kv => kv.Value);

        string? GetString(string name) => args.TryGetValue(name, out var v) ? v.Value as string : null;
        int GetInt(string name, int defaultVal) =>
            args.TryGetValue(name, out var v) && v.Value is int i ? i : defaultVal;

        int softwareOrdinal = GetInt("SoftwareMinVersion", 0);
        string[] versionStrings = ["6.4", "6.5", "6.6", "6.7", "6.8", "6.9", "7.0", "7.1", "7.2", "7.3"];
        string softwareMinVersion = softwareOrdinal >= 0 && softwareOrdinal < versionStrings.Length
            ? versionStrings[softwareOrdinal] : "6.4";

        return new PluginAttrData(
            GetString("Name"),
            GetString("UUID"),
            GetString("Category"),
            GetString("CategoryIcon"),
            GetString("Icon"),
            GetString("SupportURL"),
            GetString("URL"),
            GetInt("SDKVersion", 2),
            softwareMinVersion,
            GetString("WindowsMinVersion"),
            GetString("MacMinVersion"),
            GetString("PropertyInspectorPath"),
            GetString("CodePathWin"),
            GetString("CodePathMac")
        );
    }

    private static List<ActionInfo> GetActionInfos(
        Compilation compilation,
        INamedTypeSymbol commonInterface,
        INamedTypeSymbol? keypadInterface,
        INamedTypeSymbol? encoderInterface)
    {
        var results = new List<ActionInfo>();

        foreach (SyntaxTree syntaxTree in compilation.SyntaxTrees)
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
            foreach (ClassDeclarationSyntax classDecl in syntaxTree.GetRoot()
                         .DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                if (semanticModel.GetDeclaredSymbol(classDecl) is not INamedTypeSymbol classSymbol)
                    continue;
                if (classSymbol.IsAbstract || classSymbol.IsStatic)
                    continue;
                if (!classSymbol.AllInterfaces.Any(i =>
                        SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, commonInterface.OriginalDefinition)))
                    continue;

                bool isKeypad = keypadInterface != null && classSymbol.AllInterfaces.Any(i =>
                    SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, keypadInterface.OriginalDefinition));
                bool isEncoder = encoderInterface != null && classSymbol.AllInterfaces.Any(i =>
                    SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, encoderInterface.OriginalDefinition));

                string fullClassName = classSymbol
                    .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                    .Replace("global::", "");

                AttributeData? actionAttr = FindAttribute(classSymbol.GetAttributes(), STREAM_DECK_ACTION_ATTR);

                results.Add(new ActionInfo(
                    fullClassName,
                    isKeypad,
                    isEncoder,
                    actionAttr != null ? ParseActionAttrData(actionAttr) : null));
            }
        }

        return results;
    }

    private static ActionAttrData ParseActionAttrData(AttributeData attrData)
    {
        var args = attrData.NamedArguments.ToDictionary(kv => kv.Key, kv => kv.Value);

        string? GetString(string name) => args.TryGetValue(name, out var v) ? v.Value as string : null;
        bool? GetBool(string name) => args.TryGetValue(name, out var v) && v.Value is bool b ? b : null;

        return new ActionAttrData(
            GetString("Name"),
            GetString("Tooltip"),
            GetString("Icon"),
            GetString("PropertyInspectorPath"),
            GetString("SupportURL"),
            GetBool("SupportedInMultiActions"),
            GetBool("SupportedInKeyLogicActions"),
            GetBool("DisableAutomaticStates"),
            GetBool("DisableCaching"),
            GetBool("UserTitleEnabled"),
            GetBool("VisibleInActionsList")
        );
    }

    private static string? GetConfigClassName(Compilation compilation)
    {
        foreach (SyntaxTree syntaxTree in compilation.SyntaxTrees)
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
            foreach (ClassDeclarationSyntax classDecl in syntaxTree.GetRoot()
                         .DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                if (semanticModel.GetDeclaredSymbol(classDecl) is not INamedTypeSymbol classSymbol)
                    continue;
                if (classSymbol.IsAbstract)
                    continue;

                INamedTypeSymbol? baseType = classSymbol.BaseType;
                while (baseType != null)
                {
                    if (baseType.ToDisplayString() == MANIFEST_CONFIG_BASE)
                    {
                        return classSymbol
                            .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                            .Replace("global::", "");
                    }
                    baseType = baseType.BaseType;
                }
            }
        }
        return null;
    }

    private static AttributeData? FindAttribute(ImmutableArray<AttributeData> attrs, string fullName)
    {
        foreach (AttributeData attr in attrs)
        {
            if (attr.AttributeClass?.ToDisplayString() == fullName)
                return attr;
        }
        return null;
    }
}

// Input records — internal so ManifestProviderTemplate can access them from the same assembly

internal sealed record MsbuildProps(
    string? AssemblyName,
    string? Version,
    string? Authors,
    string? Description,
    string? PackageProjectUrl);

internal sealed record PluginAttrData(
    string? Name,
    string? Uuid,
    string? Category,
    string? CategoryIcon,
    string? Icon,
    string? SupportUrl,
    string? Url,
    int SdkVersion,
    string SoftwareMinVersion,
    string? WindowsMinVersion,
    string? MacMinVersion,
    string? PropertyInspectorPath,
    string? CodePathWin,
    string? CodePathMac);

internal sealed record ActionAttrData(
    string? Name,
    string? Tooltip,
    string? Icon,
    string? PropertyInspectorPath,
    string? SupportUrl,
    bool? SupportedInMultiActions,
    bool? SupportedInKeyLogicActions,
    bool? DisableAutomaticStates,
    bool? DisableCaching,
    bool? UserTitleEnabled,
    bool? VisibleInActionsList);

internal sealed record ActionInfo(
    string FullClassName,
    bool IsKeypad,
    bool IsEncoder,
    ActionAttrData? AttrData)
{
    public string ClassName =>
        FullClassName.Contains('.')
            ? FullClassName.Substring(FullClassName.LastIndexOf('.') + 1)
            : FullClassName;

    public string ActionUuid => FullClassName.ToLowerInvariant();
}

internal sealed record ManifestGeneratorInput(
    MsbuildProps MsbuildProps,
    PluginAttrData? PluginAttr,
    ImmutableArray<ActionInfo> Actions,
    string? ConfigClassName);

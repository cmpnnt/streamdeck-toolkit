using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Templates;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi;

[Generator]
public class SdpiGenerator : IIncrementalGenerator
{
    private const string TEXT_AREA_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.TextArea";
    private const string TEXTFIELD_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Textfield";
    private const string CHECKBOX_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Checkbox";
    private const string CHECKBOX_LIST_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.CheckboxList";
    private const string BUTTON_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Button";
    private const string DATE_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Calendar.Date";
    private const string DATETIME_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Calendar.DateTime";
    private const string MONTH_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Calendar.Month";
    private const string TIME_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Calendar.Time";
    private const string WEEK_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Calendar.Week";
    private const string COLOR_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Color";
    private const string DELEGATE_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Delegate";
    private const string FILE_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.File";
    private const string PASSWORD_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Password";
    private const string RADIO_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Radio";
    private const string RANGE_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Range";
    private const string SELECT_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Components.Select";
    private const string SDPI_OUTPUT_ATTRIBUTE_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Attributes.SdpiOutputDirectoryAttribute";

    private record ClassInfo(
        string ClassName,
        string? OutputDirectory,
        ClassDeclarationSyntax ClassSyntax,
        Location? AttributeLocation);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find all classes decorated with SdpiOutputDirectoryAttribute
        IncrementalValuesProvider<ClassInfo> decoratedClasses = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                SDPI_OUTPUT_ATTRIBUTE_FULL_NAME,
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, ct) => GetClassInfo(ctx, ct))
            .Where(static m => m is not null)!;

        // Combine with compilation
        IncrementalValueProvider<(Compilation, ImmutableArray<ClassInfo>)> combined =
            context.CompilationProvider.Combine(decoratedClasses.Collect());

        // Register the source output step
        context.RegisterSourceOutput(combined,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    /// <summary>
    /// Extracts class information from a class decorated with SdpiOutputDirectoryAttribute.
    /// </summary>
    private static ClassInfo? GetClassInfo(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.TargetNode is not ClassDeclarationSyntax classSyntax)
            return null;

        if (context.TargetSymbol is not INamedTypeSymbol namedTypeSymbol)
            return null;

        string className = namedTypeSymbol.Name;
        string? outputDirectory = null;
        Location? attributeLocation = null;

        if (context.Attributes.Length > 0)
        {
            AttributeData attributeData = context.Attributes[0];
            attributeLocation = attributeData.ApplicationSyntaxReference?.GetSyntax(cancellationToken).GetLocation();

            if (attributeData.ConstructorArguments.Length > 0 &&
                attributeData.ConstructorArguments[0].Value is string directory)
            {
                outputDirectory = directory;
            }
        }

        return new ClassInfo(className, outputDirectory, classSyntax, attributeLocation);
    }
    
    private static void Execute(
        Compilation compilation,
        ImmutableArray<ClassInfo> classInfos,
        SourceProductionContext context)
    {
        if (classInfos.IsDefaultOrEmpty)
        {
            return;
        }

        var htmlOutputs = new List<(string ClassName, string HtmlContent, string OutputPath)>();

        foreach (ClassInfo classInfo in classInfos)
        {
            // Find all object creations within this class
            List<BaseObjectCreationExpressionSyntax> objectCreations = classInfo.ClassSyntax
                .DescendantNodes()
                .OfType<BaseObjectCreationExpressionSyntax>()
                .ToList();

            if (objectCreations.Count == 0)
            {
                continue;
            }

            SemanticModel semanticModel = compilation.GetSemanticModel(classInfo.ClassSyntax.SyntaxTree);
            List<string> htmlComponents = [];

            foreach (BaseObjectCreationExpressionSyntax objectCreation in objectCreations)
            {
                ISymbol? symbol = semanticModel.GetSymbolInfo(objectCreation, context.CancellationToken).Symbol;

                if (symbol is not IMethodSymbol methodSymbol)
                {
                    continue;
                }

                INamedTypeSymbol? typeSymbol = methodSymbol.ContainingType;
                if (typeSymbol is null)
                {
                    continue;
                }

                string fullName = typeSymbol.ToDisplayString();
                string? generatedComponent = null;

                // Check if it's one of the types we want to generate HTML for
                if (fullName is not TEXT_AREA_FULL_NAME and not TEXTFIELD_FULL_NAME and not CHECKBOX_FULL_NAME and not CHECKBOX_LIST_FULL_NAME and not BUTTON_FULL_NAME and not DATE_FULL_NAME and not DATETIME_FULL_NAME and not MONTH_FULL_NAME and not TIME_FULL_NAME and not WEEK_FULL_NAME and not COLOR_FULL_NAME and not DELEGATE_FULL_NAME and not FILE_FULL_NAME and not PASSWORD_FULL_NAME and not RADIO_FULL_NAME and not RANGE_FULL_NAME and not SELECT_FULL_NAME)
                    continue;

                // Extract property values from initializer
                Dictionary<string, object?> properties = PropertyExtractor.ExtractProperties(
                    objectCreation, semanticModel, context.CancellationToken);

                switch (fullName)
                {
                    case TEXT_AREA_FULL_NAME:
                    {
                        var model = new TextAreaModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Rows = properties.GetValueOrDefault<int?>("Rows"),
                            Placeholder = properties.GetValueOrDefault<string>("Placeholder"),
                            Required = properties.GetValueOrDefault<bool>("Required"),
                            Readonly = properties.GetValueOrDefault<bool>("Readonly"),
                            MaxLength = properties.GetValueOrDefault<int?>("MaxLength"),
                            ShowLength = properties.GetValueOrDefault<bool>("ShowLength"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };
                        generatedComponent = TextAreaTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case TEXTFIELD_FULL_NAME:
                    {
                        var model = new TextFieldModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Placeholder = properties.GetValueOrDefault<string>("Placeholder"),
                            Required = properties.GetValueOrDefault<bool>("Required"),
                            Pattern = properties.GetValueOrDefault<string>("Pattern"),
                            Readonly = properties.GetValueOrDefault<bool>("Readonly"),
                            MaxLength = properties.GetValueOrDefault<int?>("MaxLength"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };
                        generatedComponent = TextFieldTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case CHECKBOX_FULL_NAME:
                    {
                        var model = new CheckboxModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };
                        generatedComponent = CheckboxTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case CHECKBOX_LIST_FULL_NAME:
                    {
                        var model = new CheckboxListModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Columns = properties.GetValueOrDefault<int?>("Columns"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };

                        if (properties.TryGetValue("DataSourceSettings", out var dsObject)
                            && dsObject is Dictionary<string, object?> dsProps)
                        {
                            if (dsProps.TryGetValue("Options", out var optionsObject)
                                && optionsObject is List<object?> optionsList)
                            {
                                model.Options = new List<OptionSettingModel>();
                                foreach (var optionObject in optionsList)
                                {
                                    if (optionObject is Dictionary<string, object?> optionProps)
                                    {
                                        model.Options.Add(new OptionSettingModel
                                        {
                                            Label = optionProps.GetValueOrDefault<string>("Label"),
                                            Value = optionProps.GetValueOrDefault<string>("Value"),
                                            Group = optionProps.GetValueOrDefault<string>("Group")
                                        });
                                    }
                                }
                            }
                        }

                        generatedComponent = CheckboxListTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case BUTTON_FULL_NAME:
                    {
                        var model = new ButtonModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Value = properties.GetValueOrDefault<string>("Value"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };
                        generatedComponent = ButtonTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case DATE_FULL_NAME:
                    case DATETIME_FULL_NAME:
                    case MONTH_FULL_NAME:
                    case TIME_FULL_NAME:
                    case WEEK_FULL_NAME:
                    {
                        var model = new CalendarModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Max = properties.GetValueOrDefault<string>("Max"),
                            Min = properties.GetValueOrDefault<string>("Min"),
                            Step = properties.GetValueOrDefault<int?>("Step"),
                            Type = fullName switch {
                                DATE_FULL_NAME => "date",
                                DATETIME_FULL_NAME => "datetime-local",
                                MONTH_FULL_NAME => "month",
                                TIME_FULL_NAME => "time",
                                WEEK_FULL_NAME => "week",
                                _ => null
                            },
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };
                        generatedComponent = CalendarTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case COLOR_FULL_NAME:
                    {
                        var model = new ColorModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };
                        generatedComponent = ColorTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case DELEGATE_FULL_NAME:
                    {
                        var model = new DelegateModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Invoke = properties.GetValueOrDefault<string>("Invoke"),
                            FormatType = properties.GetValueOrDefault<bool>("FormatType"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };
                        generatedComponent = DelegateTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case FILE_FULL_NAME:
                    {
                        var model = new FileModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Accept = properties.GetValueOrDefault<string>("Accept"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };
                        generatedComponent = FileTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case PASSWORD_FULL_NAME:
                    {
                        var model = new PasswordModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Placeholder = properties.GetValueOrDefault<string>("Placeholder"),
                            MaxLength = properties.GetValueOrDefault<int?>("MaxLength"),
                            Required = properties.GetValueOrDefault<bool>("Required"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };
                        generatedComponent = PasswordTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case RADIO_FULL_NAME:
                    {
                        var model = new RadioModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Columns = properties.GetValueOrDefault<int?>("Columns"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };

                        if (properties.TryGetValue("DataSourceSettings", out var dsObject)
                            && dsObject is Dictionary<string, object?> dsProps)
                        {
                            if (dsProps.TryGetValue("Options", out var optionsObject)
                                && optionsObject is List<object?> optionsList)
                            {
                                model.Options = new List<OptionSettingModel>();
                                foreach (var optionObject in optionsList)
                                {
                                    if (optionObject is Dictionary<string, object?> optionProps)
                                    {
                                        model.Options.Add(new OptionSettingModel
                                        {
                                            Label = optionProps.GetValueOrDefault<string>("Label"),
                                            Value = optionProps.GetValueOrDefault<string>("Value"),
                                            Group = optionProps.GetValueOrDefault<string>("Group")
                                        });
                                    }
                                }
                            }
                        }

                        generatedComponent = RadioTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case RANGE_FULL_NAME:
                    {
                        var model = new RangeModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Setting = properties.GetValueOrDefault<string>("Setting"),
                            Min = properties.GetValueOrDefault<int?>("Min"),
                            Max = properties.GetValueOrDefault<int?>("Max"),
                            Step = properties.GetValueOrDefault<int?>("Step"),
                            ShowLabels = properties.GetValueOrDefault<bool>("ShowLabels"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };
                        generatedComponent = RangeTemplate.GenerateComponent(model, properties);
                        break;
                    }
                    case SELECT_FULL_NAME:
                    {
                        var model = new SelectModel
                        {
                            Label = properties.GetValueOrDefault<string>("Label"),
                            Placeholder = properties.GetValueOrDefault<string>("Placeholder"),
                            Disabled = properties.GetValueOrDefault<bool>("Disabled")
                        };

                        if (properties.TryGetValue("PersistenceSettings", out var psObject)
                            && psObject is Dictionary<string, object?> psProps)
                        {
                            model.Setting = psProps.GetValueOrDefault<string>("Setting");
                        }

                        if (properties.TryGetValue("DataSourceSettings", out var dsObject)
                            && dsObject is Dictionary<string, object?> dsProps)
                        {
                            if (dsProps.TryGetValue("Options", out var optionsObject)
                                && optionsObject is List<object?> optionsList)
                            {
                                model.Options = new List<OptionSettingModel>();
                                foreach (var optionObject in optionsList)
                                {
                                    if (optionObject is Dictionary<string, object?> optionProps)
                                    {
                                        model.Options.Add(new OptionSettingModel
                                        {
                                            Label = optionProps.GetValueOrDefault<string>("Label"),
                                            Value = optionProps.GetValueOrDefault<string>("Value"),
                                            Group = optionProps.GetValueOrDefault<string>("Group")
                                        });
                                    }
                                }
                            }
                        }

                        generatedComponent = SelectTemplate.GenerateComponent(model, properties);
                        break;
                    }
                }

                if (generatedComponent != null)
                {
                    htmlComponents.Add(generatedComponent);
                }
            }

            if (htmlComponents.Count == 0)
            {
                continue;
            }

            string fullHtml = HtmlTemplates.GenerateHtmlDocument(htmlComponents);

            // Calculate output path
            string? fullOutputPath = null;
            if (classInfo.OutputDirectory != null)
            {
                string normalizedDirectory = classInfo.OutputDirectory.TrimEnd('/', '\\');
                if (!string.IsNullOrEmpty(normalizedDirectory))
                {
                    normalizedDirectory += "/";
                }
                fullOutputPath = $"{normalizedDirectory}{classInfo.ClassName}.html";
            }
            else
            {
                fullOutputPath = $"{classInfo.ClassName}.html";
            }

            htmlOutputs.Add((classInfo.ClassName, fullHtml, fullOutputPath));

            // Report diagnostic
            if (classInfo.AttributeLocation != null)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "SDPI001",
                        "SdpiOutputDirectoryAttribute specified",
                        $"HTML content generated for class '{classInfo.ClassName}'. Output path: '{fullOutputPath}'.",
                        "SdpiGenerator",
                        DiagnosticSeverity.Info,
                        isEnabledByDefault: true),
                    classInfo.AttributeLocation));
            }
        }

        if (htmlOutputs.Count == 0)
        {
            return;
        }

        // Generate a C# file containing all HTML outputs
        string csharpOutput = GenerateMultipleHtmlOutputs(htmlOutputs);
        context.AddSource("GeneratedSdpiComponents.g.cs", SourceText.From(csharpOutput, Encoding.UTF8));
    }

    private static string GenerateMultipleHtmlOutputs(List<(string ClassName, string HtmlContent, string OutputPath)> htmlOutputs)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// Auto-generated code by SdpiGenerator");
        sb.AppendLine("namespace Sdpi.Generated");
        sb.AppendLine("{");
        sb.AppendLine("    public static class GeneratedSdpiContent");
        sb.AppendLine("    {");

        for (int i = 0; i < htmlOutputs.Count; i++)
        {
            var (className, htmlContent, outputPath) = htmlOutputs[i];

            sb.AppendLine($"        public static class {className}");
            sb.AppendLine("        {");
            sb.AppendLine($"            public const string OutputPath = @\"{outputPath}\";");
            sb.AppendLine("            public const string Html = @\"");
            sb.AppendLine(htmlContent.Replace("\"", "\"\""));
            sb.AppendLine("\";");
            sb.AppendLine("        }");

            if (i < htmlOutputs.Count - 1)
            {
                sb.AppendLine();
            }
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }
}

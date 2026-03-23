using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Templates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators;

[Generator]
public class PluginRegistrar : IIncrementalGenerator
{
    // All plugin actions should implement this interface
    private const string INTERFACE_FULL_NAME = "Cmpnnt.StreamDeckToolkit.Backend.ICommonPluginFunctions";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find all class declarations
        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax, // Find any class declaration
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node // Select the class node
            );

        // 2. Combine with compilation to get semantic models
        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses =
            context.CompilationProvider.Combine(classDeclarations.Collect());

        // 3. Filter classes and extract required data (now just the full class name)
        IncrementalValueProvider<ImmutableArray<string>> pluginClassNames =
            compilationAndClasses.Select((tuple, ct) =>
                GetPluginClassNames(tuple.Item1, tuple.Item2, ct)); // Renamed method for clarity

        // 4. Register the source output.
        context.RegisterSourceOutput(pluginClassNames, Generate);
    }
    
    private static ImmutableArray<string> GetPluginClassNames(
        Compilation compilation,
        ImmutableArray<ClassDeclarationSyntax> classes,
        CancellationToken ct)
    {
        ImmutableArray<string>.Builder results = ImmutableArray.CreateBuilder<string>();

        INamedTypeSymbol? interfaceSymbol = compilation.GetTypeByMetadataName(INTERFACE_FULL_NAME);

        if (interfaceSymbol == null)
        {
            // Interface not found in compilation, cannot proceed
            return results.ToImmutable();
        }

        foreach (ClassDeclarationSyntax? classDecl in classes)
        {
            ct.ThrowIfCancellationRequested();

            SemanticModel semanticModel = compilation.GetSemanticModel(classDecl.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(classDecl, ct) is not INamedTypeSymbol classSymbol)
            {
                continue; // Skip if symbol resolution fails
            }
            
            if (classSymbol.IsAbstract || classSymbol.IsStatic ||
                !classSymbol.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, interfaceSymbol.OriginalDefinition)))
            {
                continue; // Skip if abstract/static or doesn't implement the interface
            }
            

            // Get the fully qualified class name
            string className = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            className = className.Replace("global::", "");

            // Ensure valid class name
            if (!string.IsNullOrEmpty(className))
            {
                results.Add(className);
            }
        }

        return results.ToImmutable();
    }
    
    private static void Generate(SourceProductionContext context, ImmutableArray<string> pluginClassNames)
    {
        if (pluginClassNames.IsEmpty)
        {
            return;
        }
        
        SourceText sourceText = SourceText.From(PluginActionIdRegistryTemplate.CreateRegistrar(pluginClassNames), Encoding.UTF8);
        context.AddSource("PluginActionIdRegistry.g.cs", sourceText);
    }
}

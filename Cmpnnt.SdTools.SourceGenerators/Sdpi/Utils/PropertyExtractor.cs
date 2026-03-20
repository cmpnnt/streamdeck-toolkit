using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Utils;

/// <summary>
/// Extracts property values from object creation syntax.
/// </summary>
internal static class PropertyExtractor
{
    /// <summary>
    /// Extracts property values assigned in an object initializer.
    /// Supports both explicit (new TextArea()) and implicit (new()) syntax.
    /// </summary>
    public static Dictionary<string, object?> ExtractProperties(
        BaseObjectCreationExpressionSyntax objectCreation,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var properties = new Dictionary<string, object?>();

        if (objectCreation.Initializer == null)
        {
            return properties;
        }

        foreach (ExpressionSyntax expression in objectCreation.Initializer.Expressions)
        {
            if (expression is not AssignmentExpressionSyntax { Left: IdentifierNameSyntax identifier } assignment)
            {
                continue;
            }

            string propertyName = identifier.Identifier.Text;
            properties[propertyName] = ExtractValue(assignment.Right, semanticModel, cancellationToken);
        }
        
        return properties;
    }

    private static object? ExtractValue(
        ExpressionSyntax expression,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        // Try to get constant value first (for literals: string, bool, int)
        Optional<object?> constantValue = semanticModel.GetConstantValue(expression, cancellationToken);

        if (constantValue.HasValue)
        {
            return constantValue.Value;
        }

        return expression switch
        {
            // Handle nested object creation
            BaseObjectCreationExpressionSyntax nestedObjectCreation => ExtractProperties(nestedObjectCreation, semanticModel, cancellationToken),
            // Handle collection expression (e.g. [..])
            CollectionExpressionSyntax collectionExpression => collectionExpression.Elements
                .OfType<ExpressionElementSyntax>()
                .Select(element => ExtractValue(element.Expression, semanticModel, cancellationToken))
                .ToList(),
            // Handle collection initializer
            InitializerExpressionSyntax collectionInitializer when
                expression.IsKind(SyntaxKind.CollectionInitializerExpression) => collectionInitializer.Expressions
                    .Select(itemExpression => ExtractValue(itemExpression, semanticModel, cancellationToken))
                    .ToList(),
            _ => null
        };
    }


    /// <summary>
    /// Helper to safely get a value from the properties dictionary and cast it.
    /// </summary>
    public static T? GetValueOrDefault<T>(this Dictionary<string, object?> properties, string key)
    {
        if (properties.TryGetValue(key, out object? value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }
}

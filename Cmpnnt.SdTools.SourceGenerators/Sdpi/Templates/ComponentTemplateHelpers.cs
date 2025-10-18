using System.Linq;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Utils;
using Microsoft.CodeAnalysis;

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Provides shared helper methods for generating HTML components.
/// </summary>
internal static class ComponentTemplateHelpers
{
    /// <summary>
    /// Gets the HTML attribute name for a C# property, respecting SdpiPropertyNameAttribute.
    /// Defaults to converting PascalCase to kebab-case.
    /// </summary>
    public static string GetHtmlAttributeName(IPropertySymbol propertySymbol)
    {
        AttributeData? attributeData = propertySymbol.GetAttributes().FirstOrDefault(ad =>
            ad.AttributeClass?.ToDisplayString() == "Cmpnnt.SdTools.SourceGenerators.Sdpi.Attributes.SdpiPropertyNameAttribute");

        if (attributeData is { ConstructorArguments.Length: > 0 })
        {
            // Use the name from the attribute: [SdpiPropertyName("format-type")]
            return attributeData.ConstructorArguments[0].Value?.ToString() ?? StringUtils.ToKebabCase(propertySymbol.Name);
        }

        // Default conversion: "ShowLength" -> "show-length"
        return StringUtils.ToKebabCase(propertySymbol.Name);
    }
    
    /// <summary>
    /// Generates a single HTML attribute string (e.g., 'setting="value"' or 'required').
    /// Returns string.Empty if the attribute should not be rendered.
    /// </summary>
    /// <param name="name">The HTML attribute name (e.g., "setting", "required").</param>
    /// <param name="value">The C# value from the property.</param>
    public static string GenerateAttributeString(string name, object? value)
    {
        return value switch
        {
            null => string.Empty,
            // Handle boolean attributes: 'required', 'readonly', 'showlength'
            // If true, render the name. If false, render nothing.
            bool boolValue => boolValue ? $" {name}" : string.Empty,
            // Handle strings
            string stringValue => string.IsNullOrEmpty(stringValue)
                ? string.Empty : $" {name}=\"{Escape(stringValue)}\"",
            _ => $" {name}=\"{value}\""
        };
    }

    /// <summary>
    /// Wraps the provided HTML in an &lt;sdpi-item&gt; if the label is not empty.
    /// </summary>
    public static string WrapInSdpiItem(string? label, string innerHtml)
    {
        if (string.IsNullOrEmpty(label))
        {
            return innerHtml;
        }

        // Indent the inner HTML for readability
        string indentedInnerHtml = innerHtml.Replace("\n", "\n    ");
        return $"<sdpi-item label=\"{Escape(label)}\">\n    {indentedInnerHtml}\n</sdpi-item>";
    }

    /// <summary>
    /// Basic escaping for HTML attribute values.
    /// </summary>
    private static string Escape(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }
        
        return value.Replace("&", "&amp;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("\"", "&quot;")
                    .Replace("'", "&#39;");
    }
}

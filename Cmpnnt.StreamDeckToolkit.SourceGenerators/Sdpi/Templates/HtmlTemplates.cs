using System.Collections.Generic;
using System.Text;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates the final HTML document and the C# wrapper class.
/// </summary>
internal static class HtmlTemplates
{
    /// <summary>
    /// Generates the basic HTML document structure wrapping the provided body content.
    /// </summary>
    public static string GenerateHtmlDocument(IEnumerable<string> bodyContent)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html>");
        sb.AppendLine("    <head lang=\"en\">");
        sb.AppendLine("        <meta charset=\"utf-q8\" />");
        sb.AppendLine("        <script src=\"sdpi-components.js\"></script>");
        sb.AppendLine("    </head>");
        sb.AppendLine("    <body>");

        foreach (string? component in bodyContent)
        {
            // Add indentation for readability
            string indentedComponent = component.Replace("\n", "\n        ");
            sb.AppendLine($"        {indentedComponent}");
        }

        sb.AppendLine("    </body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }

     /// <summary>
    /// Generates a C# class containing the HTML content.
    /// </summary>
    public static string GenerateCSharpCodeContainingHtml(string htmlContent, 
         string className = "GeneratedSdpiContent", 
         string namespaceName = "Sdpi.Generated")
    {
        var sb = new StringBuilder();
        sb.AppendLine("// Auto-generated code by SdpiGenerator");
        sb.AppendLine($"namespace {namespaceName}");
        sb.AppendLine("{");
        sb.AppendLine($"    public static class {className}");
        sb.AppendLine("    {");
        sb.AppendLine("        public const string Html = @\"");
        sb.AppendLine(htmlContent.Replace("\"", "\"\"")); 
        sb.AppendLine("\";");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }
}

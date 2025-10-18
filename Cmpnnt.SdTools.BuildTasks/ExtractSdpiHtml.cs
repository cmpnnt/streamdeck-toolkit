using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Task = Microsoft.Build.Utilities.Task;

namespace Cmpnnt.SdTools.BuildTasks;

/// <summary>
/// Extracts HTML content from a source-generated C# file containing SDPI HTML constants.
/// Supports multiple HTML outputs from classes decorated with SdpiOutputDirectoryAttribute.
/// </summary>
public class ExtractSdpiHtml : Task
{
    /// <summary>
    /// The path to the generated C# file containing the HTML constants.
    /// </summary>
    [Required]
    public string GeneratedFile { get; set; }

    /// <summary>
    /// Output parameter containing the list of generated HTML files.
    /// </summary>
    [Output]
    public ITaskItem[] GeneratedHtmlFiles { get; set; }

    public override bool Execute()
    {
        if (string.IsNullOrEmpty(GeneratedFile))
        {
            Log.LogError("GeneratedFile property is required.");
            return false;
        }

        if (!File.Exists(GeneratedFile))
        {
            Log.LogError($"Generated file not found at '{GeneratedFile}'.");
            return false;
        }

        try
        {
            string generatedCsContent = File.ReadAllText(GeneratedFile);

            // Pattern to find nested classes within GeneratedSdpiContent
            // Each class should have OutputPath and Html constants
            var classPattern = new Regex(
                @"public\s+static\s+class\s+(\w+)\s*\{[^}]*?OutputPath\s*=\s*@""([^""]+)""[^}]*?Html\s*=\s*@""(.*?)""\s*;",
                RegexOptions.Singleline);

            MatchCollection matches = classPattern.Matches(generatedCsContent);

            if (matches.Count == 0)
            {
                Log.LogWarning($"SdpiGenerator: No HTML content found in '{GeneratedFile}'.");
                return true; // Not an error, just no content to extract
            }

            var extractedFiles = new List<string>();

            foreach (Match match in matches)
            {
                string className = match.Groups[1].Value;
                string outputPath = match.Groups[2].Value;
                string htmlContent = match.Groups[3].Value;

                // Unescape the double quotes back to single quotes
                string finalHtml = htmlContent.Replace("\"\"", "\"");

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Write the final HTML file
                File.WriteAllText(outputPath, finalHtml);
                extractedFiles.Add(outputPath);
                Log.LogMessage(MessageImportance.High,
                    $"SdpiGenerator: Successfully wrote HTML for '{className}' to '{outputPath}'");
            }

            Log.LogMessage(MessageImportance.High,
                $"SdpiGenerator: Extracted {extractedFiles.Count} HTML file(s).");

            // Set the output parameter with the list of generated files
            GeneratedHtmlFiles = extractedFiles.Select(f => new TaskItem(f) as ITaskItem).ToArray();

            return true;
        }
        catch (Exception ex)
        {
            Log.LogErrorFromException(ex, true, true, null);
            return false;
        }
    }
}

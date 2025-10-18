using System.Text.RegularExpressions;

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Utils;

internal static class StringUtils
{
    // Define the Regex instance using the older, compatible method
    private static readonly Regex _kebabCaseRegex = new Regex("(?<!^)([A-Z])", RegexOptions.Compiled);

    /// <summary>
    /// Converts a PascalCase string to kebab-case.
    /// Example: "ShowLength" -> "show-length"
    /// </summary>
    public static string ToKebabCase(string pascalCase)
    {
        if (string.IsNullOrEmpty(pascalCase))
        {
            return string.Empty;
        }
        
        return _kebabCaseRegex.Replace(pascalCase, "-$1").ToLowerInvariant();
    }
}

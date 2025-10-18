using System.Collections.Generic;
using System.Text;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Utils;

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-checkbox-list&gt; component.
/// </summary>
internal static class CheckboxListTemplate
{
    public static string GenerateComponent(CheckboxListModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-checkbox-list");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("columns", model.Columns));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append('>');

        if (model.Options != null)
        {
            sb.AppendLine();
            foreach (OptionSettingModel option in model.Options)
            {
                sb.Append($"        <option value=\"{Escape(option.Value)}\">{Escape(option.Label)}</option>");
                sb.AppendLine();
            }
            sb.Append("    ");
        }

        sb.Append("</sdpi-checkbox-list>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }

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

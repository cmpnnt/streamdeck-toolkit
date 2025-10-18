using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Utils;

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-select&gt; component.
/// </summary>
internal static class SelectTemplate
{
    public static string GenerateComponent(SelectModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-select");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("placeholder", model.Placeholder));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append('>');

        if (model.Options != null && model.Options.Any())
        {
            sb.AppendLine();

            List<IGrouping<string?, OptionSettingModel>> groupedOptions = model.Options
                .GroupBy(o => o.Group)
                .ToList();

            IGrouping<string?, OptionSettingModel>? optionsWithoutGroup = 
                groupedOptions.FirstOrDefault(g => string.IsNullOrEmpty(g.Key));
            
            if (optionsWithoutGroup != null)
            {
                foreach (OptionSettingModel option in optionsWithoutGroup)
                {
                    sb.AppendLine($"        {GenerateOptionHtml(option)}");
                }
            }

            IEnumerable<IGrouping<string?, OptionSettingModel>> optionsWithGroup = groupedOptions
                .Where(g => !string.IsNullOrEmpty(g.Key));
            
            foreach (IGrouping<string?, OptionSettingModel> group in optionsWithGroup)
            {
                sb.AppendLine($"        <optgroup label=\"{Escape(group.Key)}\">");
                foreach (OptionSettingModel option in group)
                {
                    sb.AppendLine($"            {GenerateOptionHtml(option)}");
                }
                sb.AppendLine("        </optgroup>");
            }
            sb.Append("    ");
        }

        sb.Append("</sdpi-select>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }

    private static string GenerateOptionHtml(OptionSettingModel option)
    {
        return $"<option value=\"{Escape(option.Value)}\">{Escape(option.Label)}</option>";
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

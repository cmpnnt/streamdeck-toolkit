using System.Collections.Generic;
using System.Text;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Utils;

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-radio&gt; component.
/// </summary>
internal static class RadioTemplate
{
    public static string GenerateComponent(RadioModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-radio");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("columns", model.Columns?.ToString()));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append('>');

        if (model.Options != null)
        {
            foreach (OptionSettingModel option in model.Options)
            {
                sb.Append("<option");
                sb.Append(ComponentTemplateHelpers.GenerateAttributeString("value", option.Value));

                sb.Append('>');
                sb.Append(option.Label);
                sb.Append("</option>");
            }
        }

        sb.Append("</sdpi-radio>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

using System.Collections.Generic;
using System.Text;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Utils;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-range&gt; component.
/// </summary>
internal static class RangeTemplate
{
    public static string GenerateComponent(RangeModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-range");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("min", model.Min?.ToString()));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("max", model.Max?.ToString()));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("step", model.Step?.ToString()));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("showlabels", model.ShowLabels));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append("></sdpi-range>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

using System.Collections.Generic;
using System.Text;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Utils;

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-calendar&gt; component.
/// </summary>
internal static class CalendarTemplate
{
    public static string GenerateComponent(CalendarModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-calendar");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("type", model.Type));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("max", model.Max));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("min", model.Min));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("step", model.Step?.ToString()));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append("></sdpi-calendar>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

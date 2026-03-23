using System.Collections.Generic;
using System.Text;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Utils;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-color&gt; component.
/// </summary>
internal static class ColorTemplate
{
    public static string GenerateComponent(ColorModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-color");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append("></sdpi-color>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

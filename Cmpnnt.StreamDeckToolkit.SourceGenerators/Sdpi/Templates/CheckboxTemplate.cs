using System.Collections.Generic;
using System.Text;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Utils;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-checkbox&gt; component.
/// </summary>
internal static class CheckboxTemplate
{
    public static string GenerateComponent(CheckboxModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-checkbox");
        if (model.Global) sb.Append(" global");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("label", model.Label));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append("></sdpi-checkbox>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

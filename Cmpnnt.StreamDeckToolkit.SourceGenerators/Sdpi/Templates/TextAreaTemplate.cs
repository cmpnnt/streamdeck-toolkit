using System.Collections.Generic;
using System.Text;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Utils;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-textarea&gt; component.
/// </summary>
internal static class TextAreaTemplate
{
    public static string GenerateComponent(TextAreaModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-textarea");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("rows", model.Rows));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("placeholder", model.Placeholder));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("maxlength", model.MaxLength));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("showlength", model.ShowLength));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("required", model.Required));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("readonly", model.Readonly));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append("></sdpi-textarea>");

        // Wrap in <sdpi-item> if a Label is provided
        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

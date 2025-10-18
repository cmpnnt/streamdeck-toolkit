using System.Collections.Generic;
using System.Text;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Utils;

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-textfield&gt; component.
/// </summary>
internal static class TextFieldTemplate
{
    public static string GenerateComponent(TextFieldModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-textfield");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("placeholder", model.Placeholder));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("pattern", model.Pattern));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("maxlength", model.MaxLength));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("required", model.Required));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("readonly", model.Readonly));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append("></sdpi-textfield>");

        // Wrap in <sdpi-item> if a Label is provided
        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

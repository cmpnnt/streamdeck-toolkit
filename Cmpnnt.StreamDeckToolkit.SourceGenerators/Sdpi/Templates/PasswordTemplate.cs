using System.Collections.Generic;
using System.Text;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Utils;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-password&gt; component.
/// </summary>
internal static class PasswordTemplate
{
    public static string GenerateComponent(PasswordModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-password");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("placeholder", model.Placeholder));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("maxlength", model.MaxLength?.ToString()));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("required", model.Required));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append("></sdpi-password>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

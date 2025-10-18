using System.Collections.Generic;
using System.Text;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Utils;

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-delegate&gt; component.
/// </summary>
internal static class DelegateTemplate
{
    public static string GenerateComponent(DelegateModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-delegate");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("invoke", model.Invoke));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("format-type", model.FormatType));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append("></sdpi-delegate>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

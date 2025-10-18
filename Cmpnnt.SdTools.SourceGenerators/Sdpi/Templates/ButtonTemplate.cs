using System.Collections.Generic;
using System.Text;
using Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-button&gt; component.
/// </summary>
internal static class ButtonTemplate
{
    public static string GenerateComponent(ButtonModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-button");
        if (model.Disabled)
        {
            sb.Append(" disabled");
        }
        sb.Append('>');
        sb.Append(model.Value);
        sb.Append("</sdpi-button>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

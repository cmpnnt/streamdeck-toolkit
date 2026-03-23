using System.Collections.Generic;
using System.Text;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Utils;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Templates;

/// <summary>
/// Generates HTML for the &lt;sdpi-file&gt; component.
/// </summary>
internal static class FileTemplate
{
    public static string GenerateComponent(FileModel model, Dictionary<string, object?> properties)
    {
        var sb = new StringBuilder();
        sb.Append("<sdpi-file");
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("setting", model.Setting));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("default", properties.GetValueOrDefault<string>("Default")));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("accept", model.Accept));
        sb.Append(ComponentTemplateHelpers.GenerateAttributeString("disabled", model.Disabled));
        sb.Append("></sdpi-file>");

        return ComponentTemplateHelpers.WrapInSdpiItem(model.Label, sb.ToString());
    }
}

using System.Text;
using Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Templates;

internal static class GroupStartTemplate
{
    public static string GenerateComponent(GroupStartModel model)
    {
        var sb = new StringBuilder();
        sb.Append("<details class=\"sdpi-group\"");
        if (model.Open) sb.Append(" open");
        sb.AppendLine(">");
        sb.Append($"    <summary>{model.Label}</summary>");
        return sb.ToString();
    }
}

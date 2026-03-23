using System.Collections.Generic;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a Select instantiation for template generation.
/// </summary>
internal class SelectModel
{
    public string? Label { get; set; }
    public string? Setting { get; set; }
    public string? Placeholder { get; set; }
    public List<OptionSettingModel>? Options { get; set; }
    public bool Disabled { get; set; }
}

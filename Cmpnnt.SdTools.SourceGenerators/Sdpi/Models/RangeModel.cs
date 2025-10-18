namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a Range instantiation for template generation.
/// </summary>
internal class RangeModel
{
    public string? Label { get; set; }
    public string? Setting { get; set; }
    public int? Max { get; set; }
    public int? Min { get; set; }
    public bool ShowLabels { get; set; }
    public int? Step { get; set; }
    public bool Disabled { get; set; }
}

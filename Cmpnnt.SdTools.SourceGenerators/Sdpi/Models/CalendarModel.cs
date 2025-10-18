namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a Calendar instantiation for template generation.
/// </summary>
internal class CalendarModel
{
    public string? Label { get; set; }
    public string? Setting { get; set; }
    public string? Max { get; set; }
    public string? Min { get; set; }
    public int? Step { get; set; }
    public string? Type { get; set; }
    public bool Disabled { get; set; }
}

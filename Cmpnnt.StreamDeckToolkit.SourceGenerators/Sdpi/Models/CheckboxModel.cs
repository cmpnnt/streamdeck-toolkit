namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a Checkbox instantiation for template generation.
/// </summary>
internal class CheckboxModel
{
    public string? Label { get; set; }
    public string? Setting { get; set; }
    public bool Global { get; set; }
    public bool Disabled { get; set; }
}

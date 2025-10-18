namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a Button instantiation for template generation.
/// </summary>
internal class ButtonModel
{
    public string? Label { get; set; }
    public string? Value { get; set; }
    public bool Disabled { get; set; }
}

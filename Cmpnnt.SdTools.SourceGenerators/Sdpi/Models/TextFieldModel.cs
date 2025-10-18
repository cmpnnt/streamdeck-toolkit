namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a Textfield instantiation for template generation.
/// </summary>
internal class TextFieldModel
{
    public string? Label { get; set; }
    public string? Setting { get; set; }
    public string? Placeholder { get; set; }
    public bool Required { get; set; }
    public string? Pattern { get; set; }
    public bool Readonly { get; set; }
    public int? MaxLength { get; set; }
    public bool Disabled { get; set; }
}
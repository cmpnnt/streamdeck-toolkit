namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a TextArea instantiation for template generation.
/// </summary>
internal class TextAreaModel
{
    public string? Label { get; set; }
    public string? Setting { get; set; }
    public int? Rows { get; set; }
    public string? Placeholder { get; set; }
    public bool Required { get; set; }
    public bool Readonly { get; set; }
    public int? MaxLength { get; set; }
    public bool ShowLength { get; set; }
    public bool Disabled { get; set; }
}

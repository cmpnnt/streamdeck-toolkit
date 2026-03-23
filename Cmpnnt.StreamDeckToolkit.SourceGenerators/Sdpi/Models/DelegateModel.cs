namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a Delegate instantiation for template generation.
/// </summary>
internal class DelegateModel
{
    public string? Label { get; set; }
    public string? Setting { get; set; }
    public bool FormatType { get; set; }
    public string? Invoke { get; set; }
    public bool Disabled { get; set; }
}

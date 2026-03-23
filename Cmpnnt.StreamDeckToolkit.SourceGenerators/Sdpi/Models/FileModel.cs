namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a File instantiation for template generation.
/// </summary>
internal class FileModel
{
    public string? Label { get; set; }
    public string? Setting { get; set; }
    public string? Accept { get; set; }
    public bool Disabled { get; set; }
}

namespace Cmpnnt.SdTools.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a Password instantiation for template generation.
/// </summary>
internal class PasswordModel
{
    public string? Label { get; set; }
    public string? Setting { get; set; }
    public int? MaxLength { get; set; }
    public string? Placeholder { get; set; }
    public bool Required { get; set; }
    public bool Disabled { get; set; }
}

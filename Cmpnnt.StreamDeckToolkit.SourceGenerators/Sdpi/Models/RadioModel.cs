using System;
using System.Collections.Generic;

namespace Cmpnnt.StreamDeckToolkit.SourceGenerators.Sdpi.Models;

/// <summary>
/// Represents the data extracted from a Radio instantiation for template generation.
/// </summary>
internal class RadioModel
{
    private int? _columns;

    public string? Label { get; set; }
    public string? Setting { get; set; }

    public int? Columns
    {
        get => _columns;
        set => _columns = value.HasValue ? Math.Clamp(value.Value, 1, 6) : null;
    }

    public List<OptionSettingModel>? Options { get; set; }
    public bool Disabled { get; set; }
}

namespace Cmpnnt.StreamDeckToolkit.Components.Settings;

public class OptionSetting
{
    /// <summary>
    /// The text displayed for the option.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// The underlying value for the option.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Optional: The label of the optgroup this option belongs to.
    /// If set, the source generator will use this to place this option under an &lt;optgroup&gt;.
    /// If no &lt;optgroup&gt; with this label exists, one will be created.
    /// </summary>
    #nullable enable
    public string? Group { get; set; } = null;
    #nullable disable
}

namespace Cmpnnt.StreamDeckToolkit.Components;

/// <summary>
/// Opens a collapsible group in the Property Inspector. Place this before the components
/// you want to group, and close it with a <see cref="GroupEnd"/> field.
/// </summary>
public class GroupStart
{
    /// <summary>
    /// The label shown in the group's header / toggle.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// When true, the group is expanded by default.
    /// </summary>
    public bool Open { get; set; }
}

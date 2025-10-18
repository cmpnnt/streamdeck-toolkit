using Cmpnnt.SdTools.Components.Settings;

namespace Cmpnnt.SdTools.Components;

/// <summary>
/// The &lt;sdpi-color&gt; component provides a styled wrapper of &lt;input type="color"&gt;.
/// </summary>
public class Color : BaseComponent
{
    // Example output:
    // <sdpi-item label="Color">
    //     <sdpi-color setting="selected_color"></sdpi-color>
    // </sdpi-item>
    
    /// <summary>
    /// Allows the value of the component to be automatically persisted to the Stream Deck.
    /// </summary>
    public PersistenceSettings PersistenceSettings { get; set; } = new();
    
    /// <summary>
    /// The default value; shown when the persisted value is undefined.
    /// </summary>
    public new string Default = "#FFFFFF";
}

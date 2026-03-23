using Cmpnnt.StreamDeckToolkit.Components.Settings;

namespace Cmpnnt.StreamDeckToolkit.Components;

/// <summary>
/// The &lt;sdpi-checkbox&gt; component provides a styled wrapper of &lt;input type="checkbox"&gt;.
/// </summary>
public class Checkbox : BaseComponent
{
    // Example output: 
    // <sdpi-item label="Checkbox">
    //  <sdpi-checkbox setting="is_okay" label="Is everything okay?"></sdpi-checkbox>
    // </sdpi-item>
    
    /// <summary>>
    /// Allows the value of the component to be automatically persisted to the Stream Deck.
    /// </summary>
    public PersistenceSettings PersistenceSettings { get; set; } = new();
}

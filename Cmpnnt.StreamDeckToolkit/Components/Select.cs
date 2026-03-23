using Cmpnnt.StreamDeckToolkit.Components.Settings;

namespace Cmpnnt.StreamDeckToolkit.Components;

/// <summary>
/// The &lt;sdpi-select&gt; component provides a styled wrapper of &lt;input type="select"&gt;.
/// </summary>
public class Select : BaseComponent
{
    // Example output:
    // <sdpi-item label="Select">
    //     <sdpi-select setting="color" placeholder="Please choose a color">
    //         <optgroup label="Primary Colors">
    //             <option value="#ff0000">Red</option>
    //             <option value="#00ff00">Green</option>
    //             <option value="#0000ff">Blue</option>
    //         </optgroup>
    //     <option value="#000000">Black</option>
    //     <option value="#ffffff">White</option>
    //     </sdpi-select>
    // </sdpi-item>
    
    /// <summary>
    /// Allows the value of the component to be automatically persisted to the Stream Deck.
    /// </summary>
    public PersistenceSettings PersistenceSettings { get; set; } = new();
    
    /// <summary>
    /// Allows the possible values of a setting to be populated from an external data source.
    /// </summary>
    public DataSourceSettings DataSourceSettings { get; set; } = new();
    
    /// <summary>
    /// Optional placeholder text shown in the input.
    /// </summary>
    public string Placeholder { get; set; } =   string.Empty;
}

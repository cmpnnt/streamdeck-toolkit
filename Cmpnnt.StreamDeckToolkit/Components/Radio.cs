using System;
using Cmpnnt.StreamDeckToolkit.Components.Settings;

namespace Cmpnnt.StreamDeckToolkit.Components;

/// <summary>
/// The &lt;sdpi-radio&gt; component provides a styled wrapper of &lt;input type="radio"&gt;.
/// </summary>
public class Radio : BaseComponent
{
    // Example output:
    // <sdpi-item label="Radio">
    //     <sdpi-radio setting="fav_number" columns="3">
    //         <option value="1">One</option>
    //         <option value="2">Two</option>
    //         <option value="3">Three</option>
    //         <option value="4">Four</option>
    //         <option value="5">Five</option>
    //     </sdpi-radio>
    // </sdpi-item>
    
    private int columns = 1;
    
    /// <summary>
    /// Allows the value of the component to be automatically persisted to the Stream Deck.
    /// </summary>
    public PersistenceSettings PersistenceSettings { get; set; } = new();
    
    /// <summary>
    /// Allows the possible values of a setting to be populated from an external data source.
    /// </summary>
    public DataSourceSettings DataSourceSettings { get; set; } = new();
    
    /// <summary>
    /// The number of columns to render the inputs in; valid values are 1-6.
    /// Values greater than 6 will be clamped to 6.
    /// </summary>
    public int Columns { 
        get => columns; 
        set => columns = Math.Clamp(value,1,6); 
    }
}

using Cmpnnt.SdTools.Components.Settings;

namespace Cmpnnt.SdTools.Components;

/// <summary>
/// The &lt;sdpi-range&gt; component provides a styled wrapper of &lt;input type="range"&gt;.
/// </summary>
public class Range : BaseComponent
{
    // Example output:
    //     <sdpi-item label="Range">
    //         <sdpi-range
    //             setting="brightness"
    //             min="0"
    //             max="100"
    //             step="5"
    //             showlabels>
    //         </sdpi-range>
    //     </sdpi-item>
    
    /// <summary>
    /// Allows the value of the component to be automatically persisted to the Stream Deck.
    /// </summary>
    public PersistenceSettings PersistenceSettings { get; set; } = new();
    
    /// <summary>
    /// Maximum possible value.
    /// </summary>
    public int Max { get; set; }
    
    /// <summary>
    /// Minimum possible value.
    /// </summary>
    public int Min { get; set; }
    
    /// <summary>
    /// When specified, the minimum and maximum labels are shown
    /// </summary>
    public bool ShowLabels { get; set; }
    
    /// <summary>
    /// Specifies the granularity that the value must adhere to.
    /// </summary>
    public int Step { get; set; }
}

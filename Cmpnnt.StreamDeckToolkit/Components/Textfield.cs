using Cmpnnt.StreamDeckToolkit.Components.Settings;

namespace Cmpnnt.StreamDeckToolkit.Components;

/// <summary>
/// The &lt;sdpi-text&gt; component provides a styled wrapper of &lt;input type="text"&gt;.
/// </summary>
public class Textfield : BaseComponent
{
    // Example output:
    // <sdpi-item label="Textfield">
    //     <sdpi-textfield
    //         setting="first_name"
    //         pattern="/^[a-z ,.'-]+$/i"
    //         placeholder="First name"
    //         required>
    //     </sdpi-textfield>
    // </sdpi-item>
    
    /// <summary>
    /// Allows the value of the component to be automatically persisted to the Stream Deck.
    /// </summary>
    public PersistenceSettings PersistenceSettings { get; set; } = new();
    
    /// <summary>
    /// Optional maximum length of the value.
    /// </summary>
    public int MaxLength { get; set; }
    
    /// <summary>
    /// Optional regular expression used to validate the input.
    /// </summary>
    public string Pattern { get; set; } =  string.Empty;
    
    /// <summary>
    /// Optional placeholder text shown in the input.
    /// </summary>
    public string Placeholder { get; set; } =  string.Empty;
    
    /// <summary>
    /// When present, an icon is shown in the input if the value is empty
    /// </summary>
    public bool Required { get; set; }
}

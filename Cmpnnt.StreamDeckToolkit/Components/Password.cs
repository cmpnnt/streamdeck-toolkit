using Cmpnnt.StreamDeckToolkit.Components.Settings;

namespace Cmpnnt.StreamDeckToolkit.Components;

/// <summary>
/// The &lt;sdpi-password&gt; component provides a styled wrapper of &lt;input type="password"&gt;.
/// </summary>
public class Password : BaseComponent
{
    // Example output:
    // <sdpi-item label="Password">    
    //     <sdpi-password setting="api_key"></sdpi-password>
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
    /// Optional placeholder text shown in the input.
    /// </summary>
    public string Placeholder { get; set; } = string.Empty;
    
    /// <summary>
    /// When present, an icon is shown in the input if the value is empty.
    /// </summary>
    public bool Required { get; set; }
}

using Cmpnnt.StreamDeckToolkit.Components.Settings;

namespace Cmpnnt.StreamDeckToolkit.Components;

/// <summary>
/// The &lt;sdpi-textarea&gt; component provides a styled wrapper of &lt;textarea&gt;.
/// </summary>
public class TextArea : BaseComponent
{
    // Example output:
    // <sdpi-item label="Textarea">
    //     <sdpi-textarea
    //         setting="short_description"
    //         maxlength="250"
    //         rows="3"
    //         showlength>
    //     </sdpi-textarea>
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
    /// Defines the size, in rows, of the text area.
    /// </summary>
    public int Rows { get; set; }
    
    /// <summary>
    /// When present, the current length and maximum length are displayed.
    /// </summary>
    public bool ShowLength { get; set; }
}

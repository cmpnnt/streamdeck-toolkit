using Cmpnnt.StreamDeckToolkit.Attributes;
using Cmpnnt.StreamDeckToolkit.Components.Settings;

namespace Cmpnnt.StreamDeckToolkit.Components;

/// <summary>
/// The &lt;sdpi-delegate&gt; component enables the invocation of a sendToPlugin event, allowing for
/// the persisted setting to be delegated to the plugin, and then rendered within this component.
/// </summary>
public class Delegate
{
    // Example output:
    // <sdpi-item label="Delegate">
    //     <sdpi-delegate
    //          setting="delegated_value"
    //          invoke="eventName"
    //          label="...">
    //     </sdpi-delegate>
    // </sdpi-item>
        
    /// <summary>
    /// Allows the value of the component to be automatically persisted to the Stream Deck.
    /// </summary>
    public PersistenceSettings PersistenceSettings { get; set; } = new();
    
    /// <summary>
    /// The default value; shown when the persisted value is undefined.
    /// </summary>
    public string Default { get; set; } = string.Empty;
    
    /// <summary>
    /// Determines whether the input is disabled.
    /// </summary>
    public bool Disabled { get; set; }
    
    /// <summary>
    /// When <c>true</c>, formats the value using the component's built-in type formatter
    /// before displaying it. Maps to the <c>format-type</c> HTML attribute.
    /// </summary>
    [SdpiPropertyName("format-type")]
    public bool FormatType { get; set; }

    /// <summary>
    /// The name of the <c>sendToPlugin</c> event that the property inspector fires when requesting
    /// the delegated value from the plugin. The plugin should respond via <c>sendToPropertyInspector</c>.
    /// </summary>
    public string Invoke { get; set; } = string.Empty;
    
    /// <summary>
    /// The label of the &lt;sdpi-component&gt; wrapper web component.
    /// </summary>
    public string Label { get; set; } = string.Empty;
    
    /// <summary>
    /// The value of the component, and the persisted setting. This will be translated to JSON as the value of <see cref="Setting" />
    /// </summary>
    public bool Value { get; set; }
    
    /// <summary>>
    /// The name of the setting. This will be translated to JSON as a key.
    /// </summary>
    public string Setting { get; set; } = string.Empty;
}

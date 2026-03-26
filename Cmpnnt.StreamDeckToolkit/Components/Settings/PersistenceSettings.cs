namespace Cmpnnt.StreamDeckToolkit.Components.Settings;

/// <summary>
/// Controls how and where a component's value is persisted in the Stream Deck settings.
/// When <see cref="Setting"/> is empty the parent component's <c>Setting</c> key is used.
/// </summary>
public class PersistenceSettings
{
    /// <summary>>
    /// When present, the value will be persisted to the global settings.
    /// </summary>
    public bool Global { get; set; }
    
    /// <summary>>
    /// The path of the property where the value should be persisted in the settings.
    /// A setting named foo.bar.prop would be stored in settings as: {"foo":{"bar":{"prop":"value"}}}.
    /// </summary>
    public string Setting { get; set; } = string.Empty;
}

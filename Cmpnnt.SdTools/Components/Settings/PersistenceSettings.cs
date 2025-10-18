namespace Cmpnnt.SdTools.Components.Settings;

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

namespace Cmpnnt.SdTools.Components;

public class BaseComponent
{
    /// <summary>
    /// The default value; shown when the persisted value is undefined.
    /// </summary>
    public string Default { get; set; } = string.Empty;
    
    /// <summary>
    /// The value of the component, and the persisted setting. This will be translated to JSON
    /// as the value of <see cref="Setting" />
    /// </summary>
    public string Value { get; set; } = string.Empty;
    
    /// <summary>
    /// The label of the &lt;sdpi-component&gt; wrapper web component.
    /// </summary>
    public string Label { get; set; } = string.Empty;
    
    /// <summary>>
    /// Determines whether the input is disabled.
    /// </summary>
    public bool Disabled { get; set; }
    
    /// <summary>>
    /// The name of the setting. This will be translated to JSON as a key.
    /// </summary>
    public string Setting { get; set; } = string.Empty;
}

namespace Cmpnnt.SdTools.Components;

/// <summary>
/// The &lt;sdpi-button&gt; component rendering of localized content.
/// </summary>
/// <remarks>This component requires manual configuration in the generated HTML file.
/// See the sdpi-components <see href="https://sdpi-components.dev/docs/components/button">documentation</see></remarks>
// Does not extend BaseComponent because it does not have a Setting.
public class Button
{
    // Example output:
    // <sdpi-item>
    //  <sdpi-button onclick="javascript:alert('Hello World')">
    //      Click me
    //  </sdpi-button>
    // </sdpi-item>
    
    /// <summary>>
    /// Determines whether the input is disabled.
    /// </summary>
    public bool Disabled { get; set; }
    
    /// <summary>
    /// The label of the &lt;sdpi-component&gt; wrapper web component.
    /// </summary>
    public string Label { get; set; } = string.Empty;
    
    /// <summary>
    /// The text of the button. This will be translated to JSON as the value of <see cref="BaseComponent.Setting" />
    /// </summary>
    public required string Value { get; set; }
    
}

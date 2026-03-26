namespace Cmpnnt.StreamDeckToolkit.Components;

/// <summary>
/// The &lt;sdpi-i18n&gt; component provides rendering of localized content.
/// </summary>
/// <remarks>
/// This is currently unimplemented and will not function.
/// </remarks>
public class LocalizedContent
{
    // Example output:
    // <sdpi-item>
    //     <sdpi-button>
    //         <sdpi-i18n key="click_prompt"></sdpi-i18n>
    //     </sdpi-button>
    // </sdpi-item>
    
    // TODO: This will not function.
    // See: https://sdpi-components.dev/docs/helpers/localization
    // See: https://sdpi-components.dev/docs/components/i18n
    
    /// <summary>
    /// The localization key used to look up the translated string from the plugin's locale files.
    /// Maps to the <c>key</c> attribute on <c>&lt;sdpi-i18n&gt;</c>.
    /// </summary>
    public bool Key { get; set; }
    
    /// <summary>
    /// The value of the component, and the persisted setting. This will be translated to JSON as the value of <see cref="Setting" />
    /// </summary>
    public required string Value { get; set; }
}

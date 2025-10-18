using Cmpnnt.SdTools.Components.Settings;

namespace Cmpnnt.SdTools.Components;

/// <summary>
/// The &lt;sdpi-file&gt; component provides a styled wrapper of &lt;input type="file"&gt;.
/// </summary>
public class File : BaseComponent
{
    // Example output:
    // <sdpi-item label="File">
    //      <sdpi-file setting="avatar" accept="image/png, image/jpeg"></sdpi-file>
    // </sdpi-item>
    
    /// <summary>
    /// Allows the value of the component to be automatically persisted to the Stream Deck.
    /// </summary>
    public PersistenceSettings PersistenceSettings { get; set; } = new();

    /// <summary>
    /// The types of files that can be selected; directly mapped to the input's accept attribute.
    /// </summary>
    public string Accept { get; set; } = string.Empty;
}

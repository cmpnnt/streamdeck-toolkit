using Cmpnnt.StreamDeckToolkit.Components.Settings;

namespace Cmpnnt.StreamDeckToolkit.Components.Calendar;

/// <summary>
/// The common properties for all calendar input types.
/// </summary>
public class BaseCalendar : BaseComponent
{
    /// <summary>>
    /// Allows the value of the component to be automatically persisted to the Stream Deck.
    /// </summary>
    public PersistenceSettings PersistenceSettings { get; set; } = new();
    
    /// <summary>>
    /// The latest acceptable date.
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/date#max"> the MSDN docs.</see>
    /// </summary>
    public string Max { get; set; } = string.Empty;
    
    /// <summary>>
    /// The earliest acceptable date.
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/date#min"> the MSDN docs.</see>
    /// </summary>
    public string Min { get; set; } = string.Empty;
    
    /// <summary>>
    /// Specifies the granularity that the value must adhere to.
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/date#step"> the MSDN docs.</see>
    /// </summary>
    public int Step { get; set; }
    
    /// <summary>>
    /// Defines the type of input; valid values are date, datetime-local, month, week, or time.
    /// See <see cref="TimeTypes"> TimeTypes.cs </see>
    /// </summary>
    public string Type { get; set; } = string.Empty;
}

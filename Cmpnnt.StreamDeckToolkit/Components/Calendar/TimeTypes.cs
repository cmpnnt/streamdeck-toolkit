namespace Cmpnnt.StreamDeckToolkit.Components.Calendar;

/// <summary>
/// Contains all the valid values for the "type" attribute of &lt;sdpi-calendar&gt; components.
/// </summary>
public class TimeTypes
{
    /// <summary>Calendar type for selecting a date (year, month, day).</summary>
    public const string DATE = "date";

    /// <summary>Calendar type for selecting a date and time (year, month, day, hour, minute).</summary>
    public const string DATETIME_LOCAL = "datetime-local";

    /// <summary>Calendar type for selecting a month and year.</summary>
    public const string MONTH = "month";

    /// <summary>Calendar type for selecting a week number and year.</summary>
    public const string WEEK = "week";

    /// <summary>Calendar type for selecting a time of day (hour, minute).</summary>
    public const string TIME = "time";
}
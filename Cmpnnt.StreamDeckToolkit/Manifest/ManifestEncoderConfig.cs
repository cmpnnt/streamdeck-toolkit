#nullable enable
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    /// <summary>
    /// Configures the Stream Deck+ encoder (dial) appearance for an action.
    /// Set via <see cref="ManifestConfigBase.DefaultEncoder"/> or per-action in the manifest.
    /// </summary>
    public class ManifestEncoderConfig
    {
        /// <summary>Path to the background image shown behind the dial (without extension). Omitted when <c>null</c>.</summary>
        [JsonPropertyName("background")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Background { get; set; }

        /// <summary>Path to the icon shown on the dial face (without extension). Omitted when <c>null</c>.</summary>
        [JsonPropertyName("Icon")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Icon { get; set; }

        /// <summary>Name of the built-in touchpad layout to use (e.g. <c>"$B1"</c>). Omitted when <c>null</c>.</summary>
        [JsonPropertyName("layout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Layout { get; set; }

        /// <summary>Background color of the dial stack as a hex string (e.g. <c>"#FF0000"</c>). Omitted when <c>null</c>.</summary>
        [JsonPropertyName("StackColor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StackColor { get; set; }

        /// <summary>Human-readable descriptions of the dial's trigger interactions. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("TriggerDescription")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ManifestTriggerDescription? TriggerDescription { get; set; }
    }

    /// <summary>
    /// Human-readable descriptions of the dial's trigger interactions shown in the Stream Deck UI.
    /// All fields are optional and omitted when <c>null</c>.
    /// </summary>
    public class ManifestTriggerDescription
    {
        /// <summary>Description of the long-touch interaction on the touchpad above the dial.</summary>
        [JsonPropertyName("LongTouch")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LongTouch { get; set; }

        /// <summary>Description of the dial press (push) interaction.</summary>
        [JsonPropertyName("Push")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Push { get; set; }

        /// <summary>Description of the dial rotation interaction.</summary>
        [JsonPropertyName("Rotate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Rotate { get; set; }

        /// <summary>Description of the short-touch interaction on the touchpad above the dial.</summary>
        [JsonPropertyName("Touch")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Touch { get; set; }
    }
}

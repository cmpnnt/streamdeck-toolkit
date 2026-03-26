#nullable enable
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    /// <summary>
    /// Represents a single action entry within a Stream Deck plugin manifest.
    /// Populated automatically by the manifest source generator from <c>[StreamDeckAction]</c> attributes.
    /// </summary>
    public class ManifestAction
    {
        /// <summary>
        /// Controller types supported by this action (e.g. <c>"Keypad"</c>, <c>"Encoder"</c>).
        /// Derived automatically from the interfaces implemented by the action class. Omitted when <c>null</c>.
        /// </summary>
        [JsonPropertyName("Controllers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Controllers { get; set; }

        /// <summary>When <c>true</c>, disables automatic state management for this action. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("DisableAutomaticStates")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableAutomaticStates { get; set; }

        /// <summary>When <c>true</c>, disables image caching for this action. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("DisableCaching")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableCaching { get; set; }

        /// <summary>Encoder (dial) configuration for Stream Deck+ actions. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("Encoder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ManifestEncoderConfig? Encoder { get; set; }

        /// <summary>Path to the action icon (without extension).</summary>
        [JsonPropertyName("Icon")]
        public string Icon { get; set; } = string.Empty;

        /// <summary>Display name of the action shown in Stream Deck.</summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Path to the property inspector HTML file for this action. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("PropertyInspectorPath")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PropertyInspectorPath { get; set; }

        /// <summary>The visual states of this action (e.g. on/off). Defaults to a single state.</summary>
        [JsonPropertyName("States")]
        public ManifestStateConfig[] States { get; set; } = [new ManifestStateConfig()];

        /// <summary>When <c>false</c>, prevents use in Stream Deck key logic actions. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("SupportedInKeyLogicActions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SupportedInKeyLogicActions { get; set; }

        /// <summary>When <c>false</c>, prevents use in multi-actions. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("SupportedInMultiActions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SupportedInMultiActions { get; set; }

        /// <summary>URL for the action's support page. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("SupportURL")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SupportURL { get; set; }

        /// <summary>Tooltip shown when hovering over the action in Stream Deck. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("Tooltip")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Tooltip { get; set; }

        /// <summary>When <c>false</c>, prevents the user from setting a custom title. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("UserTitleEnabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? UserTitleEnabled { get; set; }

        /// <summary>Unique action identifier in reverse-domain format (e.g. <c>"com.mycompany.myplugin.myaction"</c>).</summary>
        [JsonPropertyName("UUID")]
        public string UUID { get; set; } = string.Empty;

        /// <summary>When <c>false</c>, hides the action from the Stream Deck action list. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("VisibleInActionsList")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? VisibleInActionsList { get; set; }
    }
}

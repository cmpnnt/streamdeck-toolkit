#nullable enable
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    public class ManifestAction
    {
        [JsonPropertyName("Controllers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Controllers { get; set; }

        [JsonPropertyName("DisableAutomaticStates")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableAutomaticStates { get; set; }

        [JsonPropertyName("DisableCaching")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableCaching { get; set; }

        [JsonPropertyName("Encoder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ManifestEncoderConfig? Encoder { get; set; }

        [JsonPropertyName("Icon")]
        public string Icon { get; set; } = string.Empty;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("PropertyInspectorPath")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PropertyInspectorPath { get; set; }

        [JsonPropertyName("States")]
        public ManifestStateConfig[] States { get; set; } = [new ManifestStateConfig()];

        [JsonPropertyName("SupportedInKeyLogicActions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SupportedInKeyLogicActions { get; set; }

        [JsonPropertyName("SupportedInMultiActions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SupportedInMultiActions { get; set; }

        [JsonPropertyName("SupportURL")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SupportURL { get; set; }

        [JsonPropertyName("Tooltip")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Tooltip { get; set; }

        [JsonPropertyName("UserTitleEnabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? UserTitleEnabled { get; set; }

        [JsonPropertyName("UUID")]
        public string UUID { get; set; } = string.Empty;

        [JsonPropertyName("VisibleInActionsList")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? VisibleInActionsList { get; set; }
    }
}

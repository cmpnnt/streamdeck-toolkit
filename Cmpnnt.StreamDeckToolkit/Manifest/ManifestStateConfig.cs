#nullable enable
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    /// <summary>
    /// Configures the appearance of a single action state in the Stream Deck manifest.
    /// A state controls the image, title, and font displayed on the key for that state.
    /// </summary>
    public class ManifestStateConfig
    {
        /// <summary>Font family for the state title. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("FontFamily")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FontFamily { get; set; }

        /// <summary>Font size in points for the state title. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("FontSize")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? FontSize { get; set; }

        /// <summary>Font style for the state title (e.g. <c>"Bold"</c>, <c>"Italic"</c>). Omitted when <c>null</c>.</summary>
        [JsonPropertyName("FontStyle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FontStyle { get; set; }

        /// <summary>When <c>true</c>, the title is rendered with an underline. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("FontUnderline")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? FontUnderline { get; set; }

        /// <summary>Path to the key image (without extension). Defaults to <c>"Images/pluginAction"</c>.</summary>
        [JsonPropertyName("Image")]
        public string Image { get; set; } = "Images/pluginAction";

        /// <summary>Path to the image shown in multi-action mode (without extension). Omitted when <c>null</c>.</summary>
        [JsonPropertyName("MultiActionImage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MultiActionImage { get; set; }

        /// <summary>Display name for this state shown in the Stream Deck UI. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("Name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        /// <summary>When <c>false</c>, hides the user-configurable title overlay. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("ShowTitle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ShowTitle { get; set; }

        /// <summary>Default title text shown on the key. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("Title")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; set; }

        /// <summary>Vertical alignment of the title text: <c>"top"</c>, <c>"middle"</c>, or <c>"bottom"</c>. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("TitleAlignment")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TitleAlignment { get; set; }

        /// <summary>Title color as a hex string (e.g. <c>"#FFFFFF"</c>). Omitted when <c>null</c>.</summary>
        [JsonPropertyName("TitleColor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TitleColor { get; set; }
    }
}

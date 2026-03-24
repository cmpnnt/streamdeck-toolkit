#nullable enable
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    public class ManifestRoot
    {
        [JsonPropertyName("$schema")]
        public string Schema { get; set; } = "https://schemas.elgato.com/streamdeck/plugins/manifest.json";

        [JsonPropertyName("Actions")]
        public List<ManifestAction> Actions { get; set; } = new();

        [JsonPropertyName("ApplicationsToMonitor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ManifestApplicationsToMonitor? ApplicationsToMonitor { get; set; }

        [JsonPropertyName("Author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("Category")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Category { get; set; }

        [JsonPropertyName("CategoryIcon")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CategoryIcon { get; set; }

        [JsonPropertyName("CodePath")]
        public string CodePath { get; set; } = string.Empty;

        [JsonPropertyName("CodePathMac")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CodePathMac { get; set; }

        [JsonPropertyName("CodePathWin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CodePathWin { get; set; }

        [JsonPropertyName("DefaultWindowSize")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int[]? DefaultWindowSize { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("Icon")]
        public string Icon { get; set; } = string.Empty;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("OS")]
        public List<ManifestOS> OS { get; set; } = new();

        [JsonPropertyName("Profiles")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ManifestProfile>? Profiles { get; set; }

        [JsonPropertyName("PropertyInspectorPath")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PropertyInspectorPath { get; set; }

        [JsonPropertyName("SDKVersion")]
        public int SDKVersion { get; set; } = 2;

        [JsonPropertyName("Software")]
        public ManifestSoftware Software { get; set; } = new();

        [JsonPropertyName("SupportURL")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SupportURL { get; set; }

        [JsonPropertyName("URL")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? URL { get; set; }

        [JsonPropertyName("UUID")]
        public string UUID { get; set; } = string.Empty;

        [JsonPropertyName("Version")]
        public string Version { get; set; } = string.Empty;
    }

    public class ManifestOS
    {
        [JsonPropertyName("Platform")]
        public string Platform { get; set; } = string.Empty;

        [JsonPropertyName("MinimumVersion")]
        public string MinimumVersion { get; set; } = string.Empty;
    }

    public class ManifestSoftware
    {
        [JsonPropertyName("MinimumVersion")]
        public string MinimumVersion { get; set; } = "6.4";
    }
}

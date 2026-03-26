#nullable enable
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    /// <summary>
    /// Represents the root structure of a Stream Deck plugin manifest (<c>manifest.json</c>).
    /// Populated by the manifest source generator at build time; use <see cref="ManifestConfigBase"/>
    /// and the <c>[StreamDeckPlugin]</c>/<c>[StreamDeckAction]</c> attributes to control the output.
    /// </summary>
    public class ManifestRoot
    {
        /// <summary>JSON schema URL for the manifest.</summary>
        [JsonPropertyName("$schema")]
        public string Schema { get; set; } = "https://schemas.elgato.com/streamdeck/plugins/manifest.json";

        /// <summary>The list of actions provided by this plugin.</summary>
        [JsonPropertyName("Actions")]
        public List<ManifestAction> Actions { get; set; } = new();

        /// <summary>
        /// Applications whose launch and termination events the plugin wants to receive.
        /// Omitted from the manifest when <c>null</c>.
        /// </summary>
        [JsonPropertyName("ApplicationsToMonitor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ManifestApplicationsToMonitor? ApplicationsToMonitor { get; set; }

        /// <summary>The author of the plugin (maps to the MSBuild <c>Authors</c> property).</summary>
        [JsonPropertyName("Author")]
        public string Author { get; set; } = string.Empty;

        /// <summary>Category label shown in the Stream Deck action list. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("Category")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Category { get; set; }

        /// <summary>Path to the category icon (without extension). Omitted when <c>null</c>.</summary>
        [JsonPropertyName("CategoryIcon")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CategoryIcon { get; set; }

        /// <summary>Relative path to the plugin executable (e.g. <c>myplugin.exe</c> on Windows).</summary>
        [JsonPropertyName("CodePath")]
        public string CodePath { get; set; } = string.Empty;

        /// <summary>macOS-specific executable path override. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("CodePathMac")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CodePathMac { get; set; }

        /// <summary>Windows-specific executable path override. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("CodePathWin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CodePathWin { get; set; }

        /// <summary>Default size of the plugin window, as <c>[width, height]</c>. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("DefaultWindowSize")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int[]? DefaultWindowSize { get; set; }

        /// <summary>Plugin description (maps to the MSBuild <c>Description</c> property).</summary>
        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Path to the plugin icon (without extension).</summary>
        [JsonPropertyName("Icon")]
        public string Icon { get; set; } = string.Empty;

        /// <summary>Display name of the plugin shown in Stream Deck.</summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>List of supported operating systems and their minimum versions.</summary>
        [JsonPropertyName("OS")]
        public List<ManifestOS> OS { get; set; } = new();

        /// <summary>Stream Deck profiles bundled with the plugin. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("Profiles")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ManifestProfile>? Profiles { get; set; }

        /// <summary>Default property inspector HTML path for all actions. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("PropertyInspectorPath")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PropertyInspectorPath { get; set; }

        /// <summary>Stream Deck SDK version targeted by this plugin (must be 2 or 3).</summary>
        [JsonPropertyName("SDKVersion")]
        public int SDKVersion { get; set; } = 2;

        /// <summary>Minimum required Stream Deck software version.</summary>
        [JsonPropertyName("Software")]
        public ManifestSoftware Software { get; set; } = new();

        /// <summary>URL for the plugin's support page. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("SupportURL")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SupportURL { get; set; }

        /// <summary>URL for the plugin's website. Omitted when <c>null</c>.</summary>
        [JsonPropertyName("URL")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? URL { get; set; }

        /// <summary>Unique plugin identifier in reverse-domain format (e.g. <c>"com.mycompany.myplugin"</c>).</summary>
        [JsonPropertyName("UUID")]
        public string UUID { get; set; } = string.Empty;

        /// <summary>Plugin version string (maps to the MSBuild <c>Version</c> property).</summary>
        [JsonPropertyName("Version")]
        public string Version { get; set; } = string.Empty;
    }

    /// <summary>Specifies a supported OS platform and its minimum required version.</summary>
    public class ManifestOS
    {
        /// <summary>The OS platform name: <c>"windows"</c> or <c>"mac"</c>.</summary>
        [JsonPropertyName("Platform")]
        public string Platform { get; set; } = string.Empty;

        /// <summary>The minimum OS version string (e.g. <c>"10"</c> for Windows 10, <c>"12"</c> for macOS 12).</summary>
        [JsonPropertyName("MinimumVersion")]
        public string MinimumVersion { get; set; } = string.Empty;
    }

    /// <summary>Specifies the minimum required Stream Deck software version.</summary>
    public class ManifestSoftware
    {
        /// <summary>The minimum Stream Deck software version string (e.g. <c>"6.4"</c>).</summary>
        [JsonPropertyName("MinimumVersion")]
        public string MinimumVersion { get; set; } = "6.4";
    }
}

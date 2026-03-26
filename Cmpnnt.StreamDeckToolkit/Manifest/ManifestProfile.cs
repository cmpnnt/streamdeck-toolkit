#nullable enable
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    /// <summary>
    /// Describes a Stream Deck profile bundled with the plugin.
    /// Supply instances via <see cref="ManifestConfigBase.Profiles"/>.
    /// </summary>
    public class ManifestProfile
    {
        /// <summary>
        /// When <c>true</c>, the profile is automatically installed when the plugin is installed.
        /// Omitted when <c>null</c>.
        /// </summary>
        [JsonPropertyName("AutoInstall")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AutoInstall { get; set; }

        /// <summary>
        /// The numeric device type this profile targets.
        /// Corresponds to the <see cref="Cmpnnt.StreamDeckToolkit.Communication.Registration.DeviceType"/> enum values.
        /// </summary>
        [JsonPropertyName("DeviceType")]
        public int DeviceType { get; set; }

        /// <summary>
        /// When <c>true</c>, Stream Deck does not automatically switch to this profile after installation.
        /// Omitted when <c>null</c>.
        /// </summary>
        [JsonPropertyName("DontAutoSwitchWhenInstalled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DontAutoSwitchWhenInstalled { get; set; }

        /// <summary>The file name of the profile (without the <c>.streamDeckProfile</c> extension).</summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// When <c>true</c>, the user cannot modify this profile.
        /// Omitted when <c>null</c>.
        /// </summary>
        [JsonPropertyName("Readonly")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Readonly { get; set; }
    }
}

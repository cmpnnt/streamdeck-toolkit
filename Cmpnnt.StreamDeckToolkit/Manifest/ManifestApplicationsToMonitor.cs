#nullable enable
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    /// <summary>
    /// Specifies the applications whose launch and termination events the plugin wants to receive.
    /// Supply an instance via <see cref="ManifestConfigBase.ApplicationsToMonitor"/>.
    /// </summary>
    public class ManifestApplicationsToMonitor
    {
        /// <summary>
        /// Bundle identifiers of macOS applications to monitor (e.g. <c>"com.apple.safari"</c>).
        /// Omitted when <c>null</c>.
        /// </summary>
        [JsonPropertyName("mac")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Mac { get; set; }

        /// <summary>
        /// Executable names of Windows applications to monitor (e.g. <c>"Notepad.exe"</c>).
        /// Omitted when <c>null</c>.
        /// </summary>
        [JsonPropertyName("windows")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Windows { get; set; }
    }
}

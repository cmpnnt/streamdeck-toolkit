#nullable enable
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    public class ManifestApplicationsToMonitor
    {
        [JsonPropertyName("mac")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Mac { get; set; }

        [JsonPropertyName("windows")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Windows { get; set; }
    }
}

#nullable enable
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    public class ManifestProfile
    {
        [JsonPropertyName("AutoInstall")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AutoInstall { get; set; }

        [JsonPropertyName("DeviceType")]
        public int DeviceType { get; set; }

        [JsonPropertyName("DontAutoSwitchWhenInstalled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DontAutoSwitchWhenInstalled { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Readonly")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Readonly { get; set; }
    }
}

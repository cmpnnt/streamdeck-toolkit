using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Payloads
{
    /// <summary>
    /// Payload that holds all the settings in the ReceivedGlobalSettings event
    /// </summary>
    public class ReceivedGlobalSettingsPayload
    {
        /// <summary>
        /// Global settings object
        /// </summary>
        [JsonPropertyName("settings")]
        public JsonElement Settings { get; set; }

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public ReceivedGlobalSettingsPayload(JsonElement settings)
        {
            Settings = settings;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public ReceivedGlobalSettingsPayload() {}
    }
}

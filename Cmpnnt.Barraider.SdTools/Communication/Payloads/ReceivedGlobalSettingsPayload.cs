using System.Text.Json;
using System.Text.Json.Serialization;

namespace BarRaider.SdTools.Communication.Payloads
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

        [JsonConstructor]
        public ReceivedGlobalSettingsPayload(JsonElement settings)
        {
            Settings = settings;
        }

        public ReceivedGlobalSettingsPayload() {}
    }
}

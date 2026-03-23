using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Payloads
{
    /// <summary>
    /// Payload that holds all the settings in the ReceivedSettings event
    /// </summary>
    public class ReceivedSettingsPayload
    {
        /// <summary>
        /// Action's settings
        /// </summary>
        [JsonPropertyName("settings")]
        public JsonElement Settings { get; set; }

        /// <summary>
        /// Coordinates of the key pressed
        /// </summary>
        [JsonPropertyName("coordinates")]
        public KeyCoordinates Coordinates { get; set; }
        
        /// <summary>
        /// Coordinates of the key pressed
        /// </summary>
        [JsonPropertyName("state")]
        public int? State { get; set; }

        /// <summary>
        /// Is event part of a multiaction
        /// </summary>
        [JsonPropertyName("isInMultiAction")]
        public bool IsInMultiAction { get; set; }

        [JsonConstructor]
        public ReceivedSettingsPayload(JsonElement settings, KeyCoordinates coordinates, bool isInMultiAction, int? state)
        {
            Settings = settings;
            Coordinates = coordinates;
            IsInMultiAction = isInMultiAction;
            State = state;
        }

        public ReceivedSettingsPayload() {}
    }
}

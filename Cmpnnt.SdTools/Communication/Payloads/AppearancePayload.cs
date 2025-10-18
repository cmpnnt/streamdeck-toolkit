using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cmpnnt.SdTools.Communication.Payloads
{
    /// <summary>
    /// Payload for Appearance settings
    /// </summary>
    public class AppearancePayload
    {
        /// <summary>
        /// Additional settings
        /// </summary>
        [JsonPropertyName("settings")]
        public JsonElement Settings { get; set; }

        /// <summary>
        /// Coordinates of key pressed
        /// </summary>
        [JsonPropertyName("coordinates")]
        public KeyCoordinates Coordinates { get; set; }

        /// <summary>
        /// State of key
        /// </summary>
        [JsonPropertyName("state")]
        public uint? State { get; set; }

        /// <summary>
        /// Is action in MultiAction
        /// </summary>
        [JsonPropertyName("isInMultiAction")]
        public bool IsInMultiAction { get; set; }

        /// <summary>
        /// Controller which issued the event
        /// </summary>
        [JsonPropertyName("controller")]
        public string Controller { get; set; }

        [JsonConstructor]
        public AppearancePayload(JsonElement settings, KeyCoordinates coordinates, uint? state, bool isInMultiAction, string controller)
        {
            Settings = settings;
            Coordinates = coordinates;
            State = state;
            IsInMultiAction = isInMultiAction;
            Controller = controller;
        }

        public AppearancePayload() {}
    }
}

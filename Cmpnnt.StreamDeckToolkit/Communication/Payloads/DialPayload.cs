using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Payloads
{
    /// <summary>
    /// Payload received when a dial is down or up
    /// </summary>
    public class DialPayload
    {
        /// <summary>
        /// Controller which issued the event
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Current event settings
        /// </summary>
        public JsonElement? Settings { get; set; }

        /// <summary>
        /// Coordinates of key on the stream deck
        /// </summary>
        public KeyCoordinates Coordinates { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="settings"></param>
        /// <param name="controller"></param>
        public DialPayload(KeyCoordinates coordinates, JsonElement? settings, string controller)
        {
            Coordinates = coordinates;
            Settings = settings;
            Controller = controller;
        }
        
        [JsonConstructor]
        public DialPayload(string controller, JsonElement? settings, KeyCoordinates coordinates)
        {
            Controller = controller;
            Settings = settings;
            Coordinates = coordinates;
        }

        public DialPayload() {}
    }
}

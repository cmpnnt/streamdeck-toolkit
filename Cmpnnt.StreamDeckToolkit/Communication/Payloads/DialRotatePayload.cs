using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Payloads
{
    /// <summary>
    /// Payload received when a dial is rotated
    /// </summary>
    public class DialRotatePayload
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
        /// Number of ticks rotated. Positive is to the right, negative to the left
        /// </summary>
        public int Ticks { get; set; }

        /// <summary>
        /// Boolean whether the dial is currently pressed or not
        /// </summary>
        public bool Pressed { get; set; } //IsDialPressed
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="settings"></param>
        /// <param name="controller"></param>
        /// <param name="ticks"></param>
        /// <param name="pressed"></param>
        public DialRotatePayload(KeyCoordinates coordinates, JsonElement? settings, string controller, int ticks, bool pressed)
        {
            Coordinates = coordinates;
            Settings = settings;
            Controller = controller;
            Ticks = ticks;
            Pressed = pressed;
        }

        [JsonConstructor]
        public DialRotatePayload(string controller, JsonElement? settings, KeyCoordinates coordinates, int ticks, bool pressed)
        {
            Controller = controller;
            Settings = settings;
            Coordinates = coordinates;
            Ticks = ticks;
            Pressed = pressed;
        }

        public DialRotatePayload() {}
    }
}

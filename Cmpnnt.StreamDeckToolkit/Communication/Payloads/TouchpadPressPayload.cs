using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Payloads
{
    /// <summary>
    /// Payload received when the touchpad (above the dials) is pressed
    /// </summary>
    public class TouchpadPressPayload
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
        /// Boolean whether it was a long press or not
        /// </summary>
        public bool IsLongPress { get; set; }

        /// <summary>
        /// Position on touchpad which was pressed
        /// </summary>
        public int[] TapPos { get; set; } // tap position
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="settings"></param>
        /// <param name="controller"></param>
        /// <param name="isLongPress"></param>
        /// <param name="tapPos"></param>
        public TouchpadPressPayload(KeyCoordinates coordinates, JsonElement? settings, string controller, bool isLongPress, int[] tapPos)
        {
            Coordinates = coordinates;
            Settings = settings;
            Controller = controller;
            IsLongPress = isLongPress;
            TapPos = tapPos;
        }

        [JsonConstructor]
        public TouchpadPressPayload(string controller, JsonElement? settings, KeyCoordinates coordinates, bool isLongPress, int[] tapPos)
        {
            Controller = controller;
            Settings = settings;
            Coordinates = coordinates;
            IsLongPress = isLongPress;
            TapPos = tapPos;
        }

        public TouchpadPressPayload() {}
    }
}

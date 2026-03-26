using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Payloads
{
    /// <summary>
    /// Payload received when a key is pressed or released
    /// </summary>
    public class KeyPayload
    {
        /// <summary>
        /// Current event settings
        /// </summary>
        public JsonElement? Settings { get; set; }

        /// <summary>
        /// Coordinates of key on the stream deck
        /// </summary>
        public KeyCoordinates Coordinates { get; set; }

        /// <summary>
        /// Current key state
        /// </summary>
        public uint? State { get; set; }

        /// <summary>
        /// Desired state
        /// </summary>
        public uint UserDesiredState { get; set; }

        /// <summary>
        /// Is part of a multiAction
        /// </summary>
        public bool IsInMultiAction { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="settings"></param>
        /// <param name="state"></param>
        /// <param name="userDesiredState"></param>
        /// <param name="isInMultiAction"></param>
        [JsonConstructor]
        public KeyPayload(KeyCoordinates coordinates, JsonElement? settings, uint? state, uint userDesiredState, bool isInMultiAction)
        {
            Coordinates = coordinates;
            Settings = settings;
            State = state;
            UserDesiredState = userDesiredState;
            IsInMultiAction = isInMultiAction;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public KeyPayload() { }
    }
}

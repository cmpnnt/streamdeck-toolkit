using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for KeyUp event
    /// </summary>
    public class KeyUpEvent : BaseEvent
    {
        /// <summary>
        /// Action name
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Unique action UUID
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Stream Deck device UUID
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// Key settings
        /// </summary>
        public KeyPayload Payload { get; set; }

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public KeyUpEvent(string action, string context, string device, KeyPayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public KeyUpEvent() {}
    }
}

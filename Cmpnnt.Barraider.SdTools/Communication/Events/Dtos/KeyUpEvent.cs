using System.Text.Json.Serialization;
using BarRaider.SdTools.Communication.Payloads;

namespace BarRaider.SdTools.Communication.Events.Dtos
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

        [JsonConstructor]
        public KeyUpEvent(string action, string context, string device, KeyPayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        public KeyUpEvent() {}
    }
}

using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for KeyDown event
    /// </summary>
    public class KeyDownEvent : BaseEvent
    {
        /// <summary>
        /// Action Name
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Unique Action UUID
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Device UUID key was pressed on
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// Information on key
        /// </summary>
        public KeyPayload Payload { get; set; }

        [JsonConstructor]
        public KeyDownEvent(string action, string context, string device, KeyPayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        public KeyDownEvent() {}
    }
}

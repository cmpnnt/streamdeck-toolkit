using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for Dial up event
    /// </summary>
    public class DialUpEvent : BaseEvent
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
        /// Information on dial status
        /// </summary>
        public DialPayload Payload { get; set; }

        [JsonConstructor]
        public DialUpEvent(string action, string context, string device, DialPayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        public DialUpEvent()
        {
        }
    }
}
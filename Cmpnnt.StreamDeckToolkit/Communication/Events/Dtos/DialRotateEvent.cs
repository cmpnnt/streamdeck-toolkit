using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for dial rotation event
    /// </summary>
    public class DialRotateEvent : BaseEvent
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
        /// Information on dial rotation
        /// </summary>
        public DialRotatePayload Payload { get; set; }

        [JsonConstructor]
        public DialRotateEvent(string action, string context, string device, DialRotatePayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        public DialRotateEvent()
        {
        }
    }
}
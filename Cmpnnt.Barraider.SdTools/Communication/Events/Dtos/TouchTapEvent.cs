using System.Text.Json.Serialization;
using BarRaider.SdTools.Communication.Payloads;

namespace BarRaider.SdTools.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for touchpad press
    /// </summary>
    public class TouchTapEvent : BaseEvent
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
        /// Information on touchpad press
        /// </summary>
        public TouchpadPressPayload Payload { get; set; }

        [JsonConstructor]
        public TouchTapEvent(string action, string context, string device, TouchpadPressPayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        public TouchTapEvent() {}
    }
}

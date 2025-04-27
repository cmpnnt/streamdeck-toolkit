using System.Text.Json.Serialization;
using BarRaider.SdTools.Communication.Payloads;

namespace BarRaider.SdTools.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for WillDisappearEvent event
    /// </summary>
    public class WillDisappearEvent : BaseEvent
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
        /// Stream Deck device UUID
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// settings
        /// </summary>
        public AppearancePayload Payload { get; set; }

        [JsonConstructor]
        public WillDisappearEvent(string action, string context, string device, AppearancePayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        public WillDisappearEvent() {}
    }
}

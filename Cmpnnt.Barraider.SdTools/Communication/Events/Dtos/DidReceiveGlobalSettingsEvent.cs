using System.Text.Json.Serialization;
using BarRaider.SdTools.Communication.Payloads;

namespace BarRaider.SdTools.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for DidReceiveGlobalSettings Event
    /// </summary>
    public class DidReceiveGlobalSettingsEvent : BaseEvent
    {
        /// <summary>
        /// Global Settings payload
        /// </summary>
        public ReceivedGlobalSettingsPayload Payload { get; set; }

        [JsonConstructor]
        public DidReceiveGlobalSettingsEvent(ReceivedGlobalSettingsPayload payload)
        {
            Payload = payload;
        }

        public DidReceiveGlobalSettingsEvent() {}
    }
}

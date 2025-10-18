using System.Text.Json.Serialization;
using Cmpnnt.SdTools.Communication.Payloads;

namespace Cmpnnt.SdTools.Communication.Events.Dtos
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

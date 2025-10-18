using System.Text.Json.Serialization;
using Cmpnnt.SdTools.Communication.Payloads;

namespace Cmpnnt.SdTools.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for ApplicationDidTerminate Event
    /// </summary>
    public class ApplicationDidTerminateEvent : BaseEvent
    {
        /// <summary>
        /// Application payload
        /// </summary>
        public ApplicationPayload Payload { get; set; }

        [JsonConstructor]
        public ApplicationDidTerminateEvent(ApplicationPayload payload)
        {
            Payload = payload;
        }

        public ApplicationDidTerminateEvent()
        {
        }
    }
}
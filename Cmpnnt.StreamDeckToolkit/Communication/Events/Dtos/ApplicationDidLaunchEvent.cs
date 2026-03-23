using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for ApplicationDidLaunch event
    /// </summary>
    public class ApplicationDidLaunchEvent : BaseEvent
    {
        /// <summary>
        /// Application information
        /// </summary>
        public ApplicationPayload Payload { get; set; }

        [JsonConstructor]
        public ApplicationDidLaunchEvent(ApplicationPayload payload)
        {
            Payload = payload;
        }

        public ApplicationDidLaunchEvent()
        {
        }
    }
}
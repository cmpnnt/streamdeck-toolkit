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

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public ApplicationDidLaunchEvent(ApplicationPayload payload)
        {
            Payload = payload;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public ApplicationDidLaunchEvent()
        {
        }
    }
}
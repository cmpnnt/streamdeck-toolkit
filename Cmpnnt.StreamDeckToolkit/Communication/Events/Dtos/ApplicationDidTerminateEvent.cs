using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
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

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public ApplicationDidTerminateEvent(ApplicationPayload payload)
        {
            Payload = payload;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public ApplicationDidTerminateEvent()
        {
        }
    }
}
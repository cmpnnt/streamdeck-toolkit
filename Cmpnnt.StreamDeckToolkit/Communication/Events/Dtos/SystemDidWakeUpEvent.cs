using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for SystemDidWakeUp event
    /// </summary>
    public class SystemDidWakeUpEvent : BaseEvent
    {
        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public SystemDidWakeUpEvent() { }
    }
}

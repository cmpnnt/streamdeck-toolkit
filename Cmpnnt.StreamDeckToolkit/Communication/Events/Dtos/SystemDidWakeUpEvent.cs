using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for SystemDidWakeUp event
    /// </summary>
    public class SystemDidWakeUpEvent : BaseEvent
    {
        [JsonConstructor]
        public SystemDidWakeUpEvent() { }
    }
}

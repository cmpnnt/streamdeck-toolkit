using System.Text.Json.Serialization;

namespace BarRaider.SdTools.Communication.Events.Dtos
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

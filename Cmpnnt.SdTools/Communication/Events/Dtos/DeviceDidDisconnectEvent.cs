using System.Text.Json.Serialization;

namespace Cmpnnt.SdTools.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for DeviceDidDisconnect Event
    /// </summary>
    public class DeviceDidDisconnectEvent : BaseEvent
    {
        /// <summary>
        /// UUID of device that was disconnected
        /// </summary>
        public string Device { get; set; }

        [JsonConstructor]
        public DeviceDidDisconnectEvent(string device)
        {
            Device = device;
        }

        public DeviceDidDisconnectEvent()
        {
        }
    }
}
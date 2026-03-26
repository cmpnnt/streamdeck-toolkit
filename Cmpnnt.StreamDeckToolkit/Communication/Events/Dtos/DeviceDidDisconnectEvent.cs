using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
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

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public DeviceDidDisconnectEvent(string device)
        {
            Device = device;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public DeviceDidDisconnectEvent()
        {
        }
    }
}
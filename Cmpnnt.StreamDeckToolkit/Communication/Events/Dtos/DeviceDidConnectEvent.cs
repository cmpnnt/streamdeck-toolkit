using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Registration;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for DeviceDidConnect Event
    /// </summary>
    public class DeviceDidConnectEvent : BaseEvent
    {
        /// <summary>
        /// UUID of device
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// Information on the device connected
        /// </summary>
        public StreamDeckDeviceInfo DeviceInfo { get; set; }

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public DeviceDidConnectEvent(string device, StreamDeckDeviceInfo deviceInfo)
        {
            Device = device;
            DeviceInfo = deviceInfo;
        }

        /// <summary>Initializes the event with device info only.</summary>
        public DeviceDidConnectEvent(StreamDeckDeviceInfo deviceInfo)
        {
            DeviceInfo = deviceInfo;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public DeviceDidConnectEvent()
        {
        }
    }
}
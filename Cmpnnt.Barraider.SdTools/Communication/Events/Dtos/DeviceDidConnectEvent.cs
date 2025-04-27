using System.Text.Json.Serialization;
using BarRaider.SdTools.Communication.Registration;

namespace BarRaider.SdTools.Communication.Events.Dtos
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

        [JsonConstructor]
        public DeviceDidConnectEvent(string device, StreamDeckDeviceInfo deviceInfo)
        {
            Device = device;
            DeviceInfo = deviceInfo;
        }

        public DeviceDidConnectEvent(StreamDeckDeviceInfo deviceInfo)
        {
            DeviceInfo = deviceInfo;
        }
        
        public DeviceDidConnectEvent()
        {
        }
    }
}
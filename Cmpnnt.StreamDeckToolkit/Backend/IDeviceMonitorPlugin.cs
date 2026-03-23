using Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos;

namespace Cmpnnt.StreamDeckToolkit.Backend
{
    /// <summary>
    /// Optional interface for plugin actions that need to respond to Stream Deck device connect/disconnect events.
    /// Implement this on your plugin action class to receive these global events.
    /// </summary>
    public interface IDeviceMonitorPlugin
    {
        /// <summary>
        /// Called when a Stream Deck device is plugged into the computer.
        /// </summary>
        /// <param name="evt">The event containing device info for the newly connected device.</param>
        void OnDeviceDidConnect(DeviceDidConnectEvent evt);

        /// <summary>
        /// Called when a Stream Deck device is unplugged from the computer.
        /// </summary>
        /// <param name="deviceId">The opaque identifier of the device that disconnected.</param>
        void OnDeviceDidDisconnect(string deviceId);
    }
}

using System.Text.Json;
using Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;
using Cmpnnt.StreamDeckToolkit.Runtime;

namespace Cmpnnt.StreamDeckToolkit.Actions
{
    /// <summary>
    /// Main abstract class your plugin should derive from for keys (not dials).
    /// For dials use EncoderBase or KeyAndEncoderBase.
    /// </summary>
    public abstract class KeypadBase : IKeypadPlugin, IDeviceMonitorPlugin
    {
        /// <inheritdoc/>
        public abstract void KeyPressed(KeyPayload payload);

        /// <inheritdoc/>
        public virtual void KeyReleased(KeyPayload payload) { }

        /// <inheritdoc/>
        public virtual void ReceivedSettings(ReceivedSettingsPayload payload) { }

        /// <inheritdoc/>
        public virtual void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        /// <inheritdoc/>
        public virtual void OnTick() { }

        /// <inheritdoc/>
        public abstract void Dispose();

        /// <inheritdoc/>
        public virtual void OnSendToPlugin(JsonElement payload) { }

        /// <inheritdoc/>
        public virtual void OnTitleParametersDidChange(TitleParametersPayload payload) { }

        /// <inheritdoc/>
        public virtual void OnPropertyInspectorDidAppear() { }

        /// <inheritdoc/>
        public virtual void OnPropertyInspectorDidDisappear() { }
        
        /// <inheritdoc/>
        public virtual void OnDeviceDidConnect(DeviceDidConnectEvent evt) { }

        /// <inheritdoc/>
        public virtual void OnDeviceDidDisconnect(string deviceId) { }

        /// <summary>
        /// Internal dispose function
        /// </summary>
        public void Destroy()
        {
            Dispose();
            Connection?.Dispose();
        }

        /// <summary>
        /// Connection object which handles your communication with the Stream Deck app
        /// </summary>
        protected IOutboundConnection Connection { get; set; }

        /// <summary>
        /// Constructor for PluginBase. Receives the communication and plugin settings.
        /// Note that the settings object is not used by the base and should be consumed by the deriving class.
        /// </summary>
        public KeypadBase(IOutboundConnection connection, InitialPayload payload)
        {
            Connection = connection;
        }
    }
}

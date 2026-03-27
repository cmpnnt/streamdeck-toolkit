using System.Text.Json;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;
using Cmpnnt.StreamDeckToolkit.Runtime;

namespace Cmpnnt.StreamDeckToolkit.Actions
{
    /// <summary>
    /// Main abstract class your plugin should derive from for dials (not keys).
    /// For keys use KeypadBase or KeyAndEncoderBase.
    /// </summary>
    public abstract class EncoderBase : IEncoderPlugin
    {
        /// <inheritdoc/>
        public abstract void DialRotate(DialRotatePayload payload);

        /// <inheritdoc/>
        public abstract void DialDown(DialPayload payload);

        //// <inheritdoc/>
        public abstract void DialUp(DialPayload payload);

        /// <inheritdoc/>
        public abstract void TouchPress(TouchpadPressPayload payload);

        /// <inheritdoc/>
        public abstract void ReceivedSettings(ReceivedSettingsPayload payload);

        /// <inheritdoc/>
        public virtual void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        /// <inheritdoc/>
        public abstract void OnTick();

        /// <summary>
        /// Called when the plugin is disposed
        /// </summary>
        public abstract void Dispose();

        /// <inheritdoc/>
        public virtual void OnSendToPlugin(JsonElement payload) { }

        /// <inheritdoc/>
        public virtual void OnTitleParametersDidChange(TitleParametersPayload payload) { }

        /// <inheritdoc/>
        public virtual void OnPropertyInspectorDidAppear() { }

        /// <inheritdoc/>
        public virtual void OnPropertyInspectorDidDisappear() { }

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
        #pragma warning disable IDE0060 // Remove unused parameter
        public EncoderBase(IOutboundConnection connection, InitialPayload payload)
        #pragma warning restore IDE0060 // Remove unused parameter
        {
            Connection = connection;
        }
    }
}

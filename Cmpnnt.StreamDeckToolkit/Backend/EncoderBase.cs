using System.Text.Json;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Backend
{
    /// <summary>
    /// Main abstract class your plugin should derive from for dials (not keys).
    /// For keys use KeypadBase or KeyAndEncoderBase.
    /// </summary>
    public abstract class EncoderBase : IEncoderPlugin
    {
        /// <summary>
        /// Called when the dial is rotated
        /// </summary>
        public abstract void DialRotate(DialRotatePayload payload);

        /// <summary>
        /// Called when the dial is pressed
        /// </summary>
        public abstract void DialDown(DialPayload payload);

        /// <summary>
        /// Called when the dial is released
        /// </summary>
        public abstract void DialUp(DialPayload payload);

        /// <summary>
        /// Called when the touchpad (above the dials) is pressed
        /// </summary>
        public abstract void TouchPress(TouchpadPressPayload payload);

        /// <summary>
        /// Called when the PropertyInspector has new settings
        /// </summary>
        public abstract void ReceivedSettings(ReceivedSettingsPayload payload);

        /// <summary>
        /// Called when GetGlobalSettings is called.
        /// </summary>
        public abstract void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload);

        /// <summary>
        /// Called every second. Logic for displaying title/image can go here.
        /// </summary>
        public abstract void OnTick();

        /// <summary>
        /// Called when the plugin is disposed
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Called when the Property Inspector sends a payload to the plugin via sendToPlugin
        /// </summary>
        public virtual void OnSendToPlugin(JsonElement payload) { }

        /// <summary>
        /// Called when the user changes the title or title parameters
        /// </summary>
        public virtual void OnTitleParametersDidChange(TitleParametersPayload payload) { }

        /// <summary>
        /// Called when the Property Inspector appears in the Stream Deck software UI
        /// </summary>
        public virtual void OnPropertyInspectorDidAppear() { }

        /// <summary>
        /// Called when the Property Inspector for this instance is removed from the Stream Deck software UI
        /// </summary>
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
        protected ISdConnection Connection { get; set; }

        /// <summary>
        /// Constructor for PluginBase. Receives the communication and plugin settings.
        /// Note that the settings object is not used by the base and should be consumed by the deriving class.
        /// </summary>
        #pragma warning disable IDE0060 // Remove unused parameter
        public EncoderBase(ISdConnection connection, InitialPayload payload)
        #pragma warning restore IDE0060 // Remove unused parameter
        {
            Connection = connection;
        }
    }
}

using System;
using System.Text.Json;
using Cmpnnt.SdTools.Communication.Payloads;

namespace Cmpnnt.SdTools.Backend
{
    /// <summary>
    /// Common functions used by both keypad and dial plugins
    /// </summary>
    public interface ICommonPluginFunctions : IDisposable
    {
        /// <summary>
        /// Called when the PropertyInspector has new settings
        /// </summary>
        void ReceivedSettings(ReceivedSettingsPayload payload);

        /// <summary>
        /// Called when GetGlobalSettings is called.
        /// </summary>
        void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload);

        /// <summary>
        /// Called every second. Logic for displaying title/image can go here.
        /// </summary>
        void OnTick();

        /// <summary>
        /// Internal function used by StreamDeckTools to prevent memory leaks
        /// </summary>
        void Destroy();

        /// <summary>
        /// Called when the Property Inspector sends a payload to the plugin via sendToPlugin
        /// </summary>
        void OnSendToPlugin(JsonElement payload);

        /// <summary>
        /// Called when the user changes the title or title parameters
        /// </summary>
        void OnTitleParametersDidChange(TitleParametersPayload payload);

        /// <summary>
        /// Called when the Property Inspector appears in the Stream Deck software UI
        /// </summary>
        void OnPropertyInspectorDidAppear();

        /// <summary>
        /// Called when the Property Inspector for this instance is removed from the Stream Deck software UI
        /// </summary>
        void OnPropertyInspectorDidDisappear();
    }
}

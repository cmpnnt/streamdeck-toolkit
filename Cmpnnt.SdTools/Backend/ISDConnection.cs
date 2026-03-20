using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Cmpnnt.SdTools.Communication;
using Cmpnnt.SdTools.Communication.Registration;
using SkiaSharp;

namespace Cmpnnt.SdTools.Backend
{
    
    // TODO: Rename ISdConnection and SdConnection to IOutboundSdConnection and OutboundSdConnection
    // or something similar. The name should communicate that it's an outbound connection for the plugin actions
    // and not be named similarly to StreamDeckConnection.
    
    /// <summary>
    /// Interface for a Stream Deck connection. Provides outbound API methods to send
    /// commands to the Stream Deck app. Incoming events are delivered via interface
    /// methods on ICommonPluginFunctions and its extensions.
    /// </summary>
    public interface ISdConnection : IDisposable
    {
        /// <summary>
        /// Send settings to the PropertyInspector
        /// </summary>
        Task SendToPropertyInspectorAsync(JsonElement settings);

        /// <summary>
        /// Persists your plugin settings
        /// </summary>
        Task SetSettingsAsync(JsonElement settings);

        /// <summary>
        /// Persists your global plugin settings
        /// </summary>
        Task SetGlobalSettingsAsync(JsonElement settings, bool triggerDidReceiveGlobalSettings = true);

        /// <summary>
        /// Requests global plugin settings, triggering a ReceivedGlobalSettings call
        /// </summary>
        Task GetGlobalSettingsAsync();

        /// <summary>
        /// Sets an image on the StreamDeck key.
        /// </summary>
        Task SetImageAsync(string base64Image, int? state = null, bool forceSendToStreamdeck = false);

        /// <summary>
        /// Sets an image on the StreamDeck key
        /// </summary>
        Task SetImageAsync(SKData data, int? state = null, bool forceSendToStreamdeck = false);

        /// <summary>
        /// Sets the default image for this state, as configured in the manifest
        /// </summary>
        Task SetDefaultImageAsync();

        /// <summary>
        /// Sets a title on the StreamDeck key
        /// </summary>
        Task SetTitleAsync(string title, int? state = null);

        /// <summary>
        /// Switches to one of the plugin's built-in profiles
        /// </summary>
        Task SwitchProfileAsync(string profileName);

        /// <summary>
        /// Switches to one of the plugin's built-in profiles on a specific device
        /// </summary>
        Task SwitchProfileAsync(string profileName, string deviceId);

        /// <summary>
        /// Shows the Alert (Yellow Triangle) on the StreamDeck key
        /// </summary>
        Task ShowAlert();

        /// <summary>
        /// Shows the Success (Green checkmark) on the StreamDeck key
        /// </summary>
        Task ShowOk();

        /// <summary>
        /// Add a message to the Stream Deck log
        /// </summary>
        Task LogSdMessage(string message);

        /// <summary>
        /// Gets the Stream Deck device's info
        /// </summary>
        StreamDeckDeviceInfo DeviceInfo();

        /// <summary>
        /// Tells Stream Deck to return the current plugin settings via the ReceivedSettings function
        /// </summary>
        Task GetSettingsAsync();

        /// <summary>
        /// Opens a URI in the user's browser
        /// </summary>
        Task OpenUrlAsync(string uri);

        /// <summary>
        /// Opens a URI in the user's browser
        /// </summary>
        Task OpenUrlAsync(Uri uri);

        /// <summary>
        /// Sets the plugin to a specific state which is pre-configured in the manifest file
        /// </summary>
        Task SetStateAsync(uint? state);

        /// <summary>
        /// Sets the values of touchpad layouts items
        /// </summary>
        Task SetFeedbackAsync(Dictionary<string, string> dictKeyValue);

        /// <summary>
        /// Sets the value of a single touchpad layout item
        /// </summary>
        Task SetFeedbackAsync(string layoutItemKey, string value);

        /// <summary>
        /// Sets the values of touchpad layouts items using a preset JsonElement
        /// </summary>
        Task SetFeedbackAsync(JsonElement feedbackPayload);

        /// <summary>
        /// Changes the current Stream Deck+ touch display layout
        /// </summary>
        Task SetFeedbackLayoutAsync(string layout);

        /// <summary>
        /// An opaque value identifying the plugin instance. Received during the Registration procedure.
        /// </summary>
        [JsonIgnore]
        string ContextId { get; }

        /// <summary>
        /// An opaque value identifying the device the plugin is launched on.
        /// </summary>
        [JsonIgnore]
        string DeviceId { get; }
    }
}

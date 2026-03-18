using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Cmpnnt.SdTools.Communication;
using Cmpnnt.SdTools.Communication.Payloads;
using Cmpnnt.SdTools.Communication.Registration;
using Cmpnnt.SdTools.Utilities;
using SkiaSharp;

namespace Cmpnnt.SdTools.Backend
{
    /// <summary>
    /// Outbound API client for a single Stream Deck action instance.
    /// Use this to send commands (set title, set image, etc.) to the Stream Deck app.
    /// Incoming events are delivered via interface methods on ICommonPluginFunctions.
    /// </summary>
    public class SdConnection : ISdConnection
    {
        #region Private Members

        private string previousImageHash;

        [JsonIgnore] private readonly string actionId;
        [JsonIgnore] private readonly string pluginUuid;
        [JsonIgnore] private readonly RegistrationInfo deviceInfo;
        [JsonIgnore] private readonly StreamDeckConnection streamDeckConnection;

        #endregion

        #region Public Properties

        /// <summary>
        /// An opaque value identifying the plugin instance. Received during the Registration procedure.
        /// </summary>
        [JsonIgnore]
        public string ContextId { get; set; }

        /// <summary>
        /// An opaque value identifying the device the plugin is launched on.
        /// </summary>
        [JsonIgnore]
        public string DeviceId { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new SdConnection for a single action instance.
        /// </summary>
        /// <param name="connection">The underlying WebSocket connection to the Stream Deck app.</param>
        /// <param name="pluginUuid">The plugin's unique identifier.</param>
        /// <param name="deviceInfo">Registration info containing connected device details.</param>
        /// <param name="actionId">The action's UUID as defined in the manifest.</param>
        /// <param name="contextId">The opaque context identifier for this action instance.</param>
        /// <param name="deviceId">The identifier of the device this instance runs on.</param>
        public SdConnection(
            StreamDeckConnection connection,
            string pluginUuid,
            RegistrationInfo deviceInfo,
            string actionId,
            string contextId,
            string deviceId)
        {
            streamDeckConnection = connection;
            this.pluginUuid = pluginUuid;
            this.deviceInfo = deviceInfo;
            this.actionId = actionId;
            ContextId = contextId;
            DeviceId = deviceId;
        }

        /// <inheritdoc/>
        public void Dispose() { }

        /// <summary>
        /// Gets the Stream Deck device's info
        /// </summary>
        public StreamDeckDeviceInfo DeviceInfo()
        {
            if (deviceInfo != null && !string.IsNullOrEmpty(DeviceId))
            {
                return deviceInfo.Devices.FirstOrDefault(d => d.Id == DeviceId);
            }

            Logger.Instance.LogMessage(TracingLevel.Error,
                $"Could not get DeviceInfo for DeviceId: {DeviceId} Devices: {deviceInfo?.Devices?.Length}");
            return null;
        }

        #region Public Requests

        /// <inheritdoc/>
        public async Task SendToPropertyInspectorAsync(JsonElement settings)
        {
            if (streamDeckConnection != null && !string.IsNullOrEmpty(ContextId) && !string.IsNullOrEmpty(actionId))
            {
                await streamDeckConnection.SendToPropertyInspectorAsync(actionId, settings, ContextId);
            }
        }

        /// <inheritdoc/>
        public async Task SetSettingsAsync(JsonElement settings)
        {
            if (streamDeckConnection != null && !string.IsNullOrEmpty(ContextId) && !string.IsNullOrEmpty(actionId))
            {
                await streamDeckConnection.SetSettingsAsync(settings, ContextId);
            }
        }

        /// <inheritdoc/>
        public async Task SetGlobalSettingsAsync(JsonElement settings, bool triggerDidReceiveGlobalSettings = true)
        {
            if (streamDeckConnection != null)
            {
                await streamDeckConnection.SetGlobalSettingsAsync(settings);

                if (triggerDidReceiveGlobalSettings)
                {
                    await GetGlobalSettingsAsync();
                }
            }
        }

        /// <inheritdoc/>
        public async Task GetGlobalSettingsAsync()
        {
            if (streamDeckConnection != null)
            {
                await streamDeckConnection.GetGlobalSettingsAsync();
            }
        }

        /// <inheritdoc/>
        public async Task SetImageAsync(string base64Image, int? state = null, bool forceSendToStreamdeck = false)
        {
            string hash = Tools.StringToSha512(base64Image);
            if (forceSendToStreamdeck || hash != previousImageHash)
            {
                previousImageHash = hash;
                await streamDeckConnection.SetImageAsync(base64Image, ContextId, SdkTarget.HardwareAndSoftware, state);
            }
        }

        /// <inheritdoc/>
        public async Task SetImageAsync(SKData data, int? state = null, bool forceSendToStreamdeck = false)
        {
            string hash = Tools.ImageToSha512(data);
            if (forceSendToStreamdeck || hash != previousImageHash)
            {
                Logger.Instance.LogMessage(TracingLevel.Info, "sending to streamdeck");
                previousImageHash = hash;
                await streamDeckConnection.SetImageAsync(data, ContextId, SdkTarget.HardwareAndSoftware, state);
            }
        }

        /// <inheritdoc/>
        public async Task SetDefaultImageAsync()
        {
            await SetImageAsync((string)null);
        }

        /// <inheritdoc/>
        public async Task SetTitleAsync(string title, int? state = null)
        {
            await streamDeckConnection.SetTitleAsync(title, ContextId, SdkTarget.HardwareAndSoftware, state);
        }

        /// <inheritdoc/>
        public async Task SwitchProfileAsync(string profileName)
        {
            await streamDeckConnection.SwitchToProfileAsync(DeviceId, profileName, pluginUuid);
        }

        /// <inheritdoc/>
        public async Task SwitchProfileAsync(string profileName, string deviceId)
        {
            await streamDeckConnection.SwitchToProfileAsync(deviceId, profileName, pluginUuid);
        }

        /// <inheritdoc/>
        public async Task ShowAlert()
        {
            await streamDeckConnection.ShowAlertAsync(ContextId);
        }

        /// <inheritdoc/>
        public async Task ShowOk()
        {
            await streamDeckConnection.ShowOkAsync(ContextId);
        }

        /// <inheritdoc/>
        public async Task LogSdMessage(string message)
        {
            await streamDeckConnection.LogMessageAsync(message);
        }

        /// <inheritdoc/>
        public async Task GetSettingsAsync()
        {
            await streamDeckConnection.GetSettingsAsync(ContextId);
        }

        /// <inheritdoc/>
        public async Task OpenUrlAsync(string uri)
        {
            await streamDeckConnection.OpenUrlAsync(uri);
        }

        /// <inheritdoc/>
        public async Task OpenUrlAsync(Uri uri)
        {
            await streamDeckConnection.OpenUrlAsync(uri);
        }

        /// <inheritdoc/>
        public async Task SetStateAsync(uint? state)
        {
            await streamDeckConnection.SetStateAsync(state, ContextId);
        }

        /// <inheritdoc/>
        public async Task SetFeedbackAsync(Dictionary<string, string> dictKeyValues)
        {
            await streamDeckConnection.SetFeedbackAsync(dictKeyValues, ContextId);
        }

        /// <inheritdoc/>
        public async Task SetFeedbackAsync(string layoutItemKey, string value)
        {
            await streamDeckConnection.SetFeedbackAsync(new Dictionary<string, string>() { { layoutItemKey, value } }, ContextId);
        }

        /// <inheritdoc/>
        public async Task SetFeedbackAsync(JsonElement feedbackPayload)
        {
            await streamDeckConnection.SetFeedbackAsync(feedbackPayload, ContextId);
        }

        /// <inheritdoc/>
        public async Task SetFeedbackLayoutAsync(string layout)
        {
            await streamDeckConnection.SetFeedbackLayoutAsync(layout, ContextId);
        }

        #endregion
    }
}

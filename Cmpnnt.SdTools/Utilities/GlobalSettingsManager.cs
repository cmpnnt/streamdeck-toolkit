using System;
using System.Text.Json;
using System.Threading.Tasks;
using Cmpnnt.SdTools.Communication;
using Cmpnnt.SdTools.Communication.Payloads;

namespace Cmpnnt.SdTools.Utilities
{
    /// <summary>
    /// Helper class which allows fetching the GlobalSettings of a plugin
    /// </summary>
    public class GlobalSettingsManager
    {
        #region Private Static Members
        private static GlobalSettingsManager _instance;
        private static readonly object ObjLock = new();
        #endregion

        #region Private Members
        private const int GET_GLOBAL_SETTINGS_DELAY_MS = 300;
        private StreamDeckConnection streamDeckConnection;
        private readonly System.Timers.Timer tmrGetGlobalSettings = new();
        #endregion

        #region Constructor
        /// <summary>
        /// Returns singleton entry of GlobalSettingsManager
        /// </summary>
        public static GlobalSettingsManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (ObjLock)
                {
                    return _instance ??= new GlobalSettingsManager();
                }
            }
        }

        private GlobalSettingsManager()
        {
            tmrGetGlobalSettings.Interval = GET_GLOBAL_SETTINGS_DELAY_MS;
            tmrGetGlobalSettings.Elapsed += TmrGetGlobalSettings_Elapsed;
            tmrGetGlobalSettings.AutoReset = true;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Event triggered when Global Settings are received
        /// </summary>
        public event EventHandler<ReceivedGlobalSettingsPayload> OnReceivedGlobalSettings;
        
        /// <summary>
        /// Initializes the manager with the active WebSocket connection after plugin registration completes.
        /// </summary>
        /// <param name="connection">The active Stream Deck connection.</param>
        /// <param name="getGlobalSettingsDelayMs">Debounce delay in milliseconds for global settings requests.</param>
        internal void Initialize(StreamDeckConnection connection, int getGlobalSettingsDelayMs = GET_GLOBAL_SETTINGS_DELAY_MS)
        {
            this.streamDeckConnection = connection;
            tmrGetGlobalSettings.Stop();
            tmrGetGlobalSettings.Interval = getGlobalSettingsDelayMs;
        }

        /// <summary>
        /// Called by PluginContainer when global settings are received. Notifies subscribers.
        /// </summary>
        internal void OnGlobalSettingsReceived(ReceivedGlobalSettingsPayload payload)
        {
            OnReceivedGlobalSettings?.Invoke(this, payload);
        }

        /// <summary>
        /// Command to request the Global Settings. Use the OnDidReceiveGlobalSSettings callback function to receive the Global Settings.
        /// </summary>
        /// <returns></returns>
        public void RequestGlobalSettings()
        {
            if (streamDeckConnection == null)
            {
                Logger.Instance.LogMessage(TracingLevel.Error, "GlobalSettingsManager::RequestGlobalSettings called while streamDeckConnection is null");
                return;
            }
            
            tmrGetGlobalSettings.Start();
        }

        /// <summary>
        /// Sets the Global Settings for the plugin
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="triggerDidReceiveGlobalSettings"></param>
        /// <returns></returns>
        public async Task SetGlobalSettings(JsonElement settings, bool triggerDidReceiveGlobalSettings = true)
        {
            if (streamDeckConnection == null)
            {
                Logger.Instance.LogMessage(TracingLevel.Error, "GlobalSettingsManager::SetGlobalSettings called while streamDeckConnection is null");
                return;
            }
            
            Logger.Instance.LogMessage(TracingLevel.Info, "GlobalSettingsManager::SetGlobalSettings called");
            // END
            
            await streamDeckConnection.SetGlobalSettingsAsync(settings);

            if (triggerDidReceiveGlobalSettings)
            {
                tmrGetGlobalSettings.Start();
            }
        }
        #endregion

        #region Private Methods
        private async void TmrGetGlobalSettings_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmrGetGlobalSettings.Stop();
                
                await streamDeckConnection.GetGlobalSettingsAsync();
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Error, $"TmrGetGlobalSettings_Elapsed has crashed with exception: {ex.Message}");
            }

        }
        #endregion
    }
}

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Cmpnnt.SdTools.Attributes;
using Cmpnnt.SdTools.Backend;
using Cmpnnt.SdTools.Communication.Events.Dtos;
using Cmpnnt.SdTools.Communication.Payloads;
using Cmpnnt.SdTools.Utilities;
using Cmpnnt.SdTools.Wrappers;
using SkiaSharp;

namespace Cmpnnt.SdTools.SamplePlugin
{
    public partial class PluginAction2 : KeyAndEncoderBase
    {
        // TODO: Can the framework be refactored to have a standardized settings class?
        //   See: https://docs.elgato.com/streamdeck/sdk/guides/settings
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                var instance = new PluginSettings
                {
                    OutputFileName = string.Empty,
                    InputString = string.Empty
                };
                return instance;
            }

            [FilenameProperty]
            [JsonPropertyName("outputFileName")]
            public string OutputFileName { get; set; }
            
            [JsonPropertyName("inputString")]
            public string InputString { get; set; }
            
            public override string ToString()
            {
                return $"OutputFileName: {OutputFileName}, InputString: {InputString}";
            }
        }

        #region Private Members
        private readonly PluginSettings settings;
        #endregion
        
        public PluginAction2(ISdConnection connection, InitialPayload payload) : base(connection, payload)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            settings = (payload.Settings == null || !payload.Settings.HasValue) ? 
                PluginSettings.CreateDefaultSettings() : 
                payload.Settings.Value.Deserialize<PluginSettings>(options);
            
            Logger.Instance.LogMessage(TracingLevel.Info, $"Settings: {settings}");
            
            // TODO: Remove these event handlers and replace with method calls like those in the base class
            // They can be registered in the PluginContainer.Run method like those in the base classes
            // and invoked in StreamDeckConnection.ReceiveAsync
            Connection.OnApplicationDidLaunch += Connection_OnApplicationDidLaunch;
            Connection.OnApplicationDidTerminate += Connection_OnApplicationDidTerminate;
            Connection.OnDeviceDidConnect += Connection_OnDeviceDidConnect;
            Connection.OnDeviceDidDisconnect += Connection_OnDeviceDidDisconnect;
            Connection.OnPropertyInspectorDidAppear += Connection_OnPropertyInspectorDidAppear;
            Connection.OnPropertyInspectorDidDisappear += Connection_OnPropertyInspectorDidDisappear;
            Connection.OnSendToPlugin += Connection_OnSendToPlugin;
            Connection.OnTitleParametersDidChange += Connection_OnTitleParametersDidChange;
        }

        private void Connection_OnTitleParametersDidChange(object sender, SdEventReceivedEventArgs<TitleParametersDidChangeEvent> e)
        {
            // Your logic here. Feel free to remove this and the related event de/registrations if it's not needed
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: TitleParametersDidChangeEvent");
        }

        private void Connection_OnSendToPlugin(object sender, SdEventReceivedEventArgs<SendToPluginEvent> e)
        {
            // Your logic here. Feel free to remove this and the related event de/registrations if it's not needed
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: SendToPluginEvent");
        }

        private void Connection_OnPropertyInspectorDidDisappear(object sender, SdEventReceivedEventArgs<PropertyInspectorDidDisappearEvent> e)
        {
            // Your logic here. Feel free to remove this and the related event de/registrations if it's not needed
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: PropertyInspectorDidDisappearEvent");
        }

        private void Connection_OnPropertyInspectorDidAppear(object sender, SdEventReceivedEventArgs<PropertyInspectorDidAppearEvent> e)
        {
            // Your logic here. Feel free to remove this and the related event de/registrations if it's not needed
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: PropertyInspectorDidAppearEvent");
        }

        private void Connection_OnDeviceDidDisconnect(object sender, SdEventReceivedEventArgs<DeviceDidDisconnectEvent> e)
        {
            // Your logic here. Feel free to remove this and the related event de/registrations if it's not needed
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: DeviceDidDisconnectEvent");
        }

        private void Connection_OnDeviceDidConnect(object sender, SdEventReceivedEventArgs<DeviceDidConnectEvent> e)
        {
            // Your logic here. Feel free to remove this and the related event de/registrations if it's not needed
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: DeviceDidConnectEvent");
        }

        private void Connection_OnApplicationDidTerminate(object sender, SdEventReceivedEventArgs<ApplicationDidTerminateEvent> e)
        {
            // Your logic here. Feel free to remove this and the related event de/registrations if it's not needed
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: ApplicationDidTerminateEvent");
        }

        private void Connection_OnApplicationDidLaunch(object sender, SdEventReceivedEventArgs<ApplicationDidLaunchEvent> e)
        {
            // Your logic here. Feel free to remove this and the related event de/registrations if it's not needed
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: ApplicationDidLaunchEvent");
        }

        public override void Dispose()
        {
            Connection.OnApplicationDidLaunch -= Connection_OnApplicationDidLaunch;
            Connection.OnApplicationDidTerminate -= Connection_OnApplicationDidTerminate;
            Connection.OnDeviceDidConnect -= Connection_OnDeviceDidConnect;
            Connection.OnDeviceDidDisconnect -= Connection_OnDeviceDidDisconnect;
            Connection.OnPropertyInspectorDidAppear -= Connection_OnPropertyInspectorDidAppear;
            Connection.OnPropertyInspectorDidDisappear -= Connection_OnPropertyInspectorDidDisappear;
            Connection.OnSendToPlugin -= Connection_OnSendToPlugin;
            Connection.OnTitleParametersDidChange -= Connection_OnTitleParametersDidChange;
            Logger.Instance.LogMessage(TracingLevel.Info, $"Destructor called");
        }

        public override void DialRotate(DialRotatePayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Dial rotated");
        }

        public override void DialDown(DialPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Dial pressed");
        }

        public override void DialUp(DialPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Dial released");
        }

        public override void TouchPress(TouchpadPressPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Touchpad pressed");
        }

        public override async void KeyPressed(KeyPayload payload)
        {
            try
            {
                // Just some example busy work to do when the button is released
                var tp = new TitleParameters()
                {
                    FontFamily = SKTypeface.FromFamilyName("Arial"),
                    FontStyle = SKFontStyle.Bold,
                    FontSizeInPoints = 9f,
                    TitleColor = SKColors.Gray,
                };

                using (SKData data = Tools.GenerateKeyImage(tp, "Test", SKColors.White))
                {
                    await Connection.SetImageAsync(data);
                }

                Logger.Instance.LogMessage(TracingLevel.Info, "Key Pressed");
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Fatal, $"Plugin crashed with the following message: {ex.Message}");
            }
        }

        public override async void KeyReleased(KeyPayload payload)
        {
            try
            {
                var rand = RandomGenerator.Next(100).ToString();

                // Just some example busy work to do when the button is released
                var tp = new TitleParameters()
                {
                    FontFamily = SKTypeface.FromFamilyName("Arial"),
                    FontStyle = SKFontStyle.Bold,
                    FontSizeInPoints = 9f,
                    TitleColor = SKColors.White
                };

                using (SKData data = Tools.GenerateKeyImage(tp, rand, SKColors.Black))
                {
                    await Connection.SetImageAsync(data);
                }

                Logger.Instance.LogMessage(TracingLevel.Info, "Key Released");
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Fatal, $"Plugin crashed with the following message: {ex.Message}");
            }
        }

        public override void OnTick() { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Plugin action has received settings");
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        #region Private Methods
        private Task SaveSettings()
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Plugin action is saving settings");
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            using JsonDocument doc = JsonDocument.Parse(JsonSerializer.Serialize(settings, options));
            return Connection.SetSettingsAsync(doc.RootElement);
        }
        #endregion
    }
}

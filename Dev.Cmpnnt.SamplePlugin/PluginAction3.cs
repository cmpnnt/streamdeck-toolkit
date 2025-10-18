using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Cmpnnt.SdTools.Attributes;
using Cmpnnt.SdTools.Backend;
using Cmpnnt.SdTools.Communication.Events.Dtos;
using Cmpnnt.SdTools.Communication.Payloads;
using Cmpnnt.SdTools.Components;
using Cmpnnt.SdTools.Components.Settings;
using Cmpnnt.SdTools.Utilities;
using Cmpnnt.SdTools.Wrappers;
using SkiaSharp;

namespace Cmpnnt.SdTools.SamplePlugin
{
    // TODO: Generate the pi.html regardless of whether this attribute exists.
    // Default to /PropertyInspector/ActionClassName.html and use this if provided
    [SdpiOutputDirectory("PropertyInspector/")]
    public partial class PluginAction3 : KeyAndEncoderBase
    {
        public TextArea ta = new()
        {
            MaxLength = 250,
            Rows = 3,
            Label = "Textarea",
            ShowLength = true,
            Setting = "short_description",
            Value = "short_description",
            Disabled = true
        };
        
        public Select Select = new()
        {
            Label = "Select List",
            Setting = "fav_numbers",
            Default = "2",
            DataSourceSettings = new DataSourceSettings()
            {
                Options =
                [
                    new OptionSetting { Value = "1", Label = "One" },
                    new OptionSetting { Value = "2", Label = "Two" },
                    new OptionSetting { Value = "3", Label = "Three" },
                    new OptionSetting { Value = "4", Label = "Four" },
                    new OptionSetting { Value = "5", Label = "Five" }
                ]
            }
        };

        public CheckboxList cbl = new()
        {
            Label = "Checkbox List",
            Setting = "fav_numbers",
            Columns = 8,
            Default = "2",
            DataSourceSettings = new DataSourceSettings()
            {
                Options =
                [
                    new OptionSetting { Value = "1", Label = "One" },
                    new OptionSetting { Value = "2", Label = "Two" },
                    new OptionSetting { Value = "3", Label = "Three" },
                    new OptionSetting { Value = "4", Label = "Four" },
                    new OptionSetting { Value = "5", Label = "Five" }
                ]
            }
        };
        
        private class PluginSettings3
        {
            public static PluginSettings3 CreateDefaultSettings()
            {
                var instance = new PluginSettings3
                {
                    Name = string.Empty,
                    ShowName = false
                };
                return instance;
            }
            
            public string Name { get; set; }
            public bool ShowName { get; set; }

            public override string ToString()
            {
                return $"Name: {Name}, ShowName: {ShowName}";
            }
        }

        #region Private Members
        private readonly PluginSettings3 settings;
        #endregion
        
        public PluginAction3(ISdConnection connection, InitialPayload payload) : base(connection, payload)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            settings = (payload.Settings == null || !payload.Settings.HasValue) ? 
                PluginSettings3.CreateDefaultSettings() : 
                payload.Settings.Value.Deserialize<PluginSettings3>(options);
            
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
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        #region Private Methods
        private Task SaveSettings()
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Plugin action is saving settings");
            // var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            // using JsonDocument doc = JsonDocument.Parse(JsonSerializer.Serialize(settings, options));
            // return Connection.SetSettingsAsync(doc.RootElement);
            return Task.CompletedTask;
        }
        #endregion
    }
}

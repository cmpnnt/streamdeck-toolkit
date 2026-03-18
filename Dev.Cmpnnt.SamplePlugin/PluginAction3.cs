using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Cmpnnt.SdTools.Attributes;
using Cmpnnt.SdTools.Backend;
using Cmpnnt.SdTools.Communication.Payloads;
using Cmpnnt.SdTools.Components;
using Cmpnnt.SdTools.Components.Settings;
using Cmpnnt.SdTools.Utilities;
using Cmpnnt.SdTools.Wrappers;
using SkiaSharp;

namespace Cmpnnt.SdTools.SamplePlugin
{
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
        }

        public override void OnTitleParametersDidChange(TitleParametersPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: TitleParametersDidChangeEvent");
        }

        public override void OnSendToPlugin(JsonElement payload)
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: SendToPluginEvent");
        }

        public override void OnPropertyInspectorDidDisappear()
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: PropertyInspectorDidDisappearEvent");
        }

        public override void OnPropertyInspectorDidAppear()
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Event Triggered: PropertyInspectorDidAppearEvent");
        }

        public override void Dispose()
        {
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
            return Task.CompletedTask;
        }
        #endregion
    }
}

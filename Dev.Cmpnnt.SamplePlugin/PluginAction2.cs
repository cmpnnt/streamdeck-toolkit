using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Cmpnnt.SdTools.Attributes;
using Cmpnnt.SdTools.Backend;
using Cmpnnt.SdTools.Communication.Payloads;
using Cmpnnt.SdTools.Utilities;
using Cmpnnt.SdTools.Wrappers;
using SkiaSharp;

namespace Cmpnnt.SdTools.SamplePlugin
{
    public partial class PluginAction2 : KeyAndEncoderBase
    {
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

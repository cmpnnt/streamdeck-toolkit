using System;
using System.Text.Json;
using System.Threading.Tasks;
using Cmpnnt.StreamDeckToolkit.Actions;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;
using Cmpnnt.StreamDeckToolkit.Runtime;
using Cmpnnt.StreamDeckToolkit.Utilities;
using Cmpnnt.StreamDeckToolkit.Wrappers;
using SkiaSharp;

namespace Cmpnnt.StreamDeckToolkit.SamplePlugin
{
    public partial class PluginAction2 : KeyAndEncoderBase
    {
        #region Private Members
        private readonly PluginAction2Settings settings;
        #endregion

        public PluginAction2(IOutboundConnection connection, InitialPayload payload) : base(connection, payload)
        {
            settings = (payload.Settings == null || !payload.Settings.HasValue) ?
                PluginAction2Settings.CreateDefaultSettings() :
                payload.Settings.Value.Deserialize(SamplePluginSerializerContext.Default.PluginAction2Settings);

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
            settings.PopulateFromJson(payload.Settings);
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        #region Private Methods
        private Task SaveSettings()
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Plugin action is saving settings");
            using JsonDocument doc = JsonDocument.Parse(JsonSerializer.Serialize(settings, SamplePluginSerializerContext.Default.PluginAction2Settings));
            return Connection.SetSettingsAsync(doc.RootElement);
        }
        #endregion
    }
}

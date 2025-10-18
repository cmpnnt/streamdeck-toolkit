using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cmpnnt.SdTools.Communication.Registration;
using Cmpnnt.SdTools.Utilities;
using CommandLine;

namespace Cmpnnt.SdTools.Communication.Payloads
{
    /// <summary>
    /// Class holding all the information passed to the plugin when the program was launched
    /// </summary>
    public class StreamDeckOptions
    {
        private RegistrationInfo deviceInfo;

        /// <summary>
        /// Port to communicate with the StreamDeck app
        /// </summary>
        [Option("port", Required = true, HelpText = "The websocket port to connect to", SetName = "port")]
        public int Port { get; set; }

        /// <summary>
        /// UUID of the plugin
        /// </summary>
        [Option("pluginUUID", Required = true, HelpText = "The UUID of the plugin")]
        public string PluginUuid { get; set; }

        /// <summary>
        /// Name of the event we should pass to the StreamDeck app to register
        /// </summary>
        [Option("registerEvent", Required = true, HelpText = "The event triggered when the plugin is registered?")]
        public string RegisterEvent { get; set; }

        /// <summary>
        /// Raw information in JSON format which we will parse into the DeviceInfo property
        /// </summary>
        [Option("info", Required = true, HelpText = "Extra JSON launch data")]
        public string RawInfo { get; set; }

        /// <summary>
        /// Information regarding the StreamDeck app and StreamDeck hardware which was parsed from the RawInfo JSON field.
        /// </summary>
        public RegistrationInfo DeviceInfo
        {
            get
            {
                if (deviceInfo != null)
                {
                    return deviceInfo;
                }

                if (string.IsNullOrEmpty(RawInfo)) // Also check for empty string
                {
                    return null;
                }

                try
                {
                    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    deviceInfo = JsonSerializer.Deserialize<RegistrationInfo>(RawInfo, options);
                    return deviceInfo;
                }
                catch (JsonException ex)
                {
                    Logger.Instance.LogMessage(TracingLevel.Fatal, $"Error deserializing DeviceInfo: {ex.Message}");
                    return null;
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogMessage(TracingLevel.Fatal, $"Error deserializing DeviceInfo: {ex.Message}");
                    return null;
                }
            }
        }

        [JsonConstructor]
        public StreamDeckOptions(int port, string pluginUuid, string registerEvent, string rawInfo)
        {
            Port = port;
            PluginUuid = pluginUuid;
            RegisterEvent = registerEvent;
            RawInfo = rawInfo;
        }

        public StreamDeckOptions() {}
    }
}

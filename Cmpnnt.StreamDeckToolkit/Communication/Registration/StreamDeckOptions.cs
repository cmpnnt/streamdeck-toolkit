using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Utilities;

namespace Cmpnnt.StreamDeckToolkit.Communication.Registration
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
        public int Port { get; set; }

        /// <summary>
        /// UUID of the plugin
        /// </summary>
        public string PluginUuid { get; set; }

        /// <summary>
        /// Name of the event we should pass to the StreamDeck app to register
        /// </summary>
        public string RegisterEvent { get; set; }

        /// <summary>
        /// Raw information in JSON format which we will parse into the DeviceInfo property
        /// </summary>
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
                    deviceInfo = JsonSerializer.Deserialize(RawInfo, RegistrationSerializerContext.Default.RegistrationInfo);
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

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public StreamDeckOptions(int port, string pluginUuid, string registerEvent, string rawInfo)
        {
            Port = port;
            PluginUuid = pluginUuid;
            RegisterEvent = registerEvent;
            RawInfo = rawInfo;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public StreamDeckOptions() {}

        /// <summary>
        /// Parses Stream Deck launch arguments into a <see cref="StreamDeckOptions"/> instance.
        /// Handles both single-dash and double-dash prefixes and is case-insensitive.
        /// Unknown arguments are ignored.
        /// </summary>
        public static StreamDeckOptions Parse(string[] args)
        {
            var options = new StreamDeckOptions();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (!args[i].StartsWith('-')) continue;
                string key = args[i].TrimStart('-');
                string value = args[i + 1];
                i++;

                switch (key.ToLowerInvariant())
                {
                    case "port":
                        if (int.TryParse(value, out int port)) options.Port = port;
                        break;
                    case "pluginuuid":
                        options.PluginUuid = value;
                        break;
                    case "registerevent":
                        options.RegisterEvent = value;
                        break;
                    case "info":
                        options.RawInfo = value;
                        break;
                }
            }
            return options;
        }
    }
}

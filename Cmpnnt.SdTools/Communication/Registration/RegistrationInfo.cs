using System.Text;
using System.Text.Json.Serialization;

namespace Cmpnnt.SdTools.Communication.Registration
{
    /// <summary>
    /// Class which holds information on the StreamDeck app and StreamDeck hardware device that the plugin is communicating with
    /// </summary>
    public class RegistrationInfo
    {
        /// <summary>
        /// Information on the StreamDeck App which we're communicating with
        /// </summary>
        public StreamDeckApplicationInfo Application { get; set; }

        /// <summary>
        /// Information on the StreamDeck hardware device that the plugin is running on
        /// </summary>
        public StreamDeckDeviceInfo[] Devices { get; set; }

        /// <summary>
        /// Information on the Plugin we're currently running
        /// </summary>
        public StreamDeckPluginInfo Plugin { get; set; }

        /// <summary>
        /// Device pixel ratio
        /// </summary>
        public int DevicePixelRatio { get; set; }

        [JsonConstructor]
        public RegistrationInfo(StreamDeckApplicationInfo application, StreamDeckDeviceInfo[] devices, StreamDeckPluginInfo plugin)
        {
            Application = application;
            Devices = devices;
            Plugin = plugin;
        }

        public RegistrationInfo() { }

        /// <summary>
        /// Shows class information as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Devices != null)
            {
                sb.Append("Devices:\n");
                foreach (StreamDeckDeviceInfo sdi in Devices)
                {
                    if (sdi != null)
                    {
                        sb.Append($"[{sdi}]\n");
                    }
                }
            }

            if (Application != null) sb.Append($"ApplicationInfo: {Application}\n");
            if (Plugin != null) sb.Append($"PluginInfo: {Plugin}\n");
            
            return sb.ToString();
        }
    }
}

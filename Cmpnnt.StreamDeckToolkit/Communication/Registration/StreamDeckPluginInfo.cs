using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Registration
{
    /// <summary>
    /// Holds general information on the StreamDeck App we're communicating with
    /// </summary>
    public class StreamDeckPluginInfo
    {
        /// <summary>
        /// Current version of the plugin
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Shows class information as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Version: {Version}";
        }

        [JsonConstructor]
        public StreamDeckPluginInfo(string version)
        {
            Version = version;
        }

        public StreamDeckPluginInfo() { }
    }
}

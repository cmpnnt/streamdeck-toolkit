using System.Text.Json.Serialization;

namespace Cmpnnt.SdTools.Communication.Registration
{
    /// <summary>
    /// Holds general information on the StreamDeck App we're communicating with
    /// </summary>
    public class StreamDeckApplicationInfo
    {
        /// <summary>
        /// Current language of the StreamDeck app
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// OS Platform
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Current version of the StreamDeck app
        /// </summary>
        public string Version { get; set; }

        [JsonConstructor]
        public StreamDeckApplicationInfo(string language, string platform, string version)
        {
            Language = language;
            Platform = platform;
            Version = version;
        }

        public StreamDeckApplicationInfo() { }

        /// <summary>
        /// Shows class information as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Language: {Language} Platform: {Platform} Version: {Version}";
        }
    }
}

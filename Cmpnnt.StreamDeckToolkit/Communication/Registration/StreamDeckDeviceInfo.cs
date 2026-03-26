using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Registration
{
    /// <summary>
    /// Class which holds information on the StreamDeck hardware device
    /// </summary>
    public class StreamDeckDeviceInfo
    {
        /// <summary>
        /// Details on number of keys of the StreamDeck hardware device
        /// </summary>
        public StreamDeckDeviceSize Size { get; set; }

        /// <summary>
        /// Type of StreamDeck hardware device
        /// </summary>
        public DeviceType Type { get; set; }

        /// <summary>
        /// ID of the StreamDeck hardware device
        /// </summary>
        public string Id { get; set; } // deviceId

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        [JsonConstructor]
        public StreamDeckDeviceInfo(StreamDeckDeviceSize size, DeviceType type, string id)
        {
            Size = size;
            Type = type;
            Id = id;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public StreamDeckDeviceInfo() { }

        /// <summary>
        /// Shows class information as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Id: {Id} Type: {Type} Size: {Size}";
        }
    }
}

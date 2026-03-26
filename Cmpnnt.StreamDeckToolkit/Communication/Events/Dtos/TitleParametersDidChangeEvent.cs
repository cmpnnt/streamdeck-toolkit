using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for TitleParametersDidChangeEvent event
    /// </summary>
    public class TitleParametersDidChangeEvent : BaseEvent
    {
        /// <summary>
        /// Action Name
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Unique Action UUID
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Stream Deck device UUID
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// Title settings
        /// </summary>
        public TitleParametersPayload Payload { get; set; }

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public TitleParametersDidChangeEvent(string action, string context, string device, TitleParametersPayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public TitleParametersDidChangeEvent() {}
    }
}

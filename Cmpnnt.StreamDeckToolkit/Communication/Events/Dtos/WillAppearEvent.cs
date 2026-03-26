using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for WillAppearEvent event
    /// </summary>
    public class WillAppearEvent : BaseEvent
    {
        /// <summary>
        /// Action Name
        /// </summary>
        [JsonPropertyName("action")]
        public string Action { get; set; }

        /// <summary>
        /// Unique Action UUID
        /// </summary>
        [JsonPropertyName("context")]
        public string Context { get; set; }

        /// <summary>
        /// Stream Deck device UUID
        /// </summary>
        [JsonPropertyName("device")]
        public string Device { get; set; }

        /// <summary>
        /// Appearance settings
        /// </summary>
        [JsonPropertyName("payload")]
        public AppearancePayload Payload { get; set; }

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public WillAppearEvent(string action, string context, string device, AppearancePayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public WillAppearEvent() {}

        /// <summary>Returns a string representation of the event.</summary>
        public override string ToString()
        {
            return $"Action: {Action}, Context: {Context}, Device: {Device} Payload: {Payload}";
        }
    }
}

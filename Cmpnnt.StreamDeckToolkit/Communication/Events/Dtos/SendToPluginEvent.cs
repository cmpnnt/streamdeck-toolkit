using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for SendToPluginEvent event
    /// </summary>
    public class SendToPluginEvent : BaseEvent
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
        /// Payload
        /// </summary>
        public JsonElement Payload { get; set; }

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public SendToPluginEvent(string action, string context, JsonElement payload)
        {
            Action = action;
            Context = context;
            Payload = payload;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public SendToPluginEvent() {}
    }
}

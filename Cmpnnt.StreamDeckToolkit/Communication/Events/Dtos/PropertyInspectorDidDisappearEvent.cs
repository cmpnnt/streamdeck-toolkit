using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for PropertyInspectorDidDisappearEvent event
    /// </summary>
    public class PropertyInspectorDidDisappearEvent : BaseEvent
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

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public PropertyInspectorDidDisappearEvent(string action, string context, string device)
        {
            Action = action;
            Context = context;
            Device = device;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public PropertyInspectorDidDisappearEvent() {}
    }
}

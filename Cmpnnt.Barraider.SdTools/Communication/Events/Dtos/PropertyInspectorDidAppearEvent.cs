using System.Text.Json.Serialization;

namespace BarRaider.SdTools.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for PropertyInspectorDidAppearEvent event
    /// </summary>
    public class PropertyInspectorDidAppearEvent : BaseEvent
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

        [JsonConstructor]
        public PropertyInspectorDidAppearEvent(string action, string context, string device)
        {
            Action = action;
            Context = context;
            Device = device;
        }

        public PropertyInspectorDidAppearEvent() {}
    }
}

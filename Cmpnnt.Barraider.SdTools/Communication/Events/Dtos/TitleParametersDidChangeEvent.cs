using System.Text.Json.Serialization;
using BarRaider.SdTools.Communication.Payloads;

namespace BarRaider.SdTools.Communication.Events.Dtos
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

        [JsonConstructor]
        public TitleParametersDidChangeEvent(string action, string context, string device, TitleParametersPayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        public TitleParametersDidChangeEvent() {}
    }
}

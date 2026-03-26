using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos
{
    /// <summary>
    /// Payload for DidReceiveSettings Event
    /// </summary>
    public class DidReceiveSettingsEvent : BaseEvent
    {
        /// <summary>
        /// Action Name
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Context (unique action UUID)
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Device UUID action is on
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// Settings for action
        /// </summary>
        public ReceivedSettingsPayload Payload { get; set; }

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public DidReceiveSettingsEvent(string action, string context, string device, ReceivedSettingsPayload payload)
        {
            Action = action;
            Context = context;
            Device = device;
            Payload = payload;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public DidReceiveSettingsEvent() {}
    }
}

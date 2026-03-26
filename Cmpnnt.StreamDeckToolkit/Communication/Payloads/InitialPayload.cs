using System.Text.Json;
using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Registration;

namespace Cmpnnt.StreamDeckToolkit.Communication.Payloads
{
    /// <summary>
    /// Payload received during the plugin's constructor
    /// </summary>
    public class InitialPayload
    {
        /// <summary>
        /// Plugin instance's settings (set through Property Inspector)
        /// </summary>
        public JsonElement? Settings { get; set; }

        /// <summary>
        /// Plugin's physical location on the Stream Deck device
        /// </summary>
        public KeyCoordinates Coordinates { get; set; }

        /// <summary>
        /// Current plugin state
        /// </summary>
        public uint? State { get; set; }

        /// <summary>
        /// Is it in a Multiaction
        /// </summary>
        public bool IsInMultiAction { get; set; }

        /// <summary>
        /// The controller of the current action. Values include "Keypad" and "Encoder".
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Information regarding the Stream Deck hardware device
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RegistrationInfo DeviceInfo { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appearancePayload"></param>
        /// <param name="deviceInfo"></param>
        public InitialPayload(AppearancePayload appearancePayload, RegistrationInfo deviceInfo)
        {
            Coordinates = appearancePayload.Coordinates;
            Settings = appearancePayload.Settings;
            State = appearancePayload.State;
            IsInMultiAction = appearancePayload.IsInMultiAction;
            Controller = appearancePayload.Controller;
            DeviceInfo = deviceInfo;
        }

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public InitialPayload(JsonElement? settings, KeyCoordinates coordinates, uint? state, bool isInMultiAction, string controller, RegistrationInfo deviceInfo)
        {
            Settings = settings;
            Coordinates = coordinates;
            State = state;
            IsInMultiAction = isInMultiAction;
            Controller = controller;
            DeviceInfo = deviceInfo;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public InitialPayload() {}
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Wrappers;

namespace Cmpnnt.StreamDeckToolkit.Communication.Payloads
{
    /// <summary>
    /// Payload for TitleParametersDidChange Event
    /// </summary>
    public class TitleParametersPayload
    {
        private TitleParameters titleParameters;

        /// <summary>
        /// Settings JSON Object
        /// </summary>
        public JsonElement Settings { get; set; }

        /// <summary>
        /// Key Coordinates
        /// </summary>
        public KeyCoordinates Coordinates { get; set; }

        /// <summary>
        /// Key State
        /// </summary>
        public uint? State { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Title Parameters
        /// </summary>
        [JsonIgnore]
        public TitleParameters TitleParameters
        {
            get
            {
                if (titleParameters != null)
                {
                    return titleParameters;
                }

                if (TitleParametersRaw != null)
                {
                    titleParameters = new TitleParameters(TitleParametersRaw.FontFamily, TitleParametersRaw.FontSize, TitleParametersRaw.FontStyle, TitleParametersRaw.ShowTitle, TitleParametersRaw.TitleAlignment, TitleParametersRaw.TitleColor);
                }

                return titleParameters;
            }
            set => titleParameters = value;
        }

        /// <summary>
        /// Raw Title Parameters (not as proper object)
        /// </summary>
        [JsonPropertyName("titleParameters")]
        public TitleParametersRawPayload TitleParametersRaw { get; set; }
        
        /// <summary>Initializes the payload with strongly-typed title parameters.</summary>
        public TitleParametersPayload(JsonElement settings, KeyCoordinates coordinates, uint? state, string title, TitleParameters titleParameters)
        {
            Settings = settings;
            Coordinates = coordinates;
            State = state;
            Title = title;
            TitleParameters = titleParameters;
        }

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public TitleParametersPayload(JsonElement settings, KeyCoordinates coordinates, uint? state, string title, TitleParametersRawPayload titleParametersRaw)
        {
            Settings = settings;
            Coordinates = coordinates;
            State = state;
            Title = title;
            TitleParametersRaw = titleParametersRaw;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public TitleParametersPayload() {}
    }
}

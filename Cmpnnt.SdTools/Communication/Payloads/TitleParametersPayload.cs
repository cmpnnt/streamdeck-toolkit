using System.Text.Json;
using System.Text.Json.Serialization;
using Cmpnnt.SdTools.Wrappers;

namespace Cmpnnt.SdTools.Communication.Payloads
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
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="coordinates"></param>
        /// <param name="state"></param>
        /// <param name="title"></param>
        /// <param name="titleParameters"></param>
        public TitleParametersPayload(JsonElement settings, KeyCoordinates coordinates, uint? state, string title, TitleParameters titleParameters)
        {
            Settings = settings;
            Coordinates = coordinates;
            State = state;
            Title = title;
            TitleParameters = titleParameters;
        }

        [JsonConstructor]
        public TitleParametersPayload(JsonElement settings, KeyCoordinates coordinates, uint? state, string title, TitleParametersRawPayload titleParametersRaw)
        {
            Settings = settings;
            Coordinates = coordinates;
            State = state;
            Title = title;
            TitleParametersRaw = titleParametersRaw;
        }

        public TitleParametersPayload() {}
    }
}

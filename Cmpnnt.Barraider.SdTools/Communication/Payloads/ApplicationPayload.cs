using System.Text.Json.Serialization;

namespace BarRaider.SdTools.Communication.Payloads
{
    /// <summary>
    /// ApplicationPayload
    /// </summary>
    public class ApplicationPayload
    {
        /// <summary>
        /// Application Name
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="application"></param>
        [JsonConstructor]
        public ApplicationPayload(string application)
        {
            Application = application;
        }

        public ApplicationPayload() {}
    }
}

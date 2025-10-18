using System.Text.Json.Serialization;
using Cmpnnt.SdTools.Communication.Payloads;

namespace Cmpnnt.SdTools.Communication.Commands.Dtos
{
    internal class SetImageCommand(string base64Image, string context, SdkTarget target, int? state) : IMessage
    {
        public string Event => "setImage";

        public string Context { get; set; } = context;

        public SetImageCommandPayload Payload { get; set; } = new (base64Image, target, state);

        internal class SetImageCommandPayload(string image, SdkTarget target, int? state) : IPayload
        {
            #nullable enable
            public string? Image { get; set; } = image;
            
            public SdkTarget Target { get; set; } = target;
            
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public int? State { get; set; } = state;
        }
    }
}
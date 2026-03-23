using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Commands.Dtos
{
    internal class SetTitleCommand(string title, string context, SdkTarget target, int? state) : IMessage
    {
        public string Event => "setTitle";

        public string Context { get; set; } = context;

        public SetTitleCommandPayload Payload { get; set; } = new (title, target, state);

        internal class SetTitleCommandPayload(string title, SdkTarget target, int? state) : IPayload
        {
            public string Title { get; set; } = title;

            public SdkTarget Target { get; set; } = target;

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public int? State { get; set; } = state;
        }
    }
}
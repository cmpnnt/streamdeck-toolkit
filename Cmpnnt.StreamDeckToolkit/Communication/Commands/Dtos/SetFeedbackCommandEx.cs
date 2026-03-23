using System.Text.Json;

namespace Cmpnnt.StreamDeckToolkit.Communication.Commands.Dtos
{
    internal class SetFeedbackCommandEx(JsonElement payload, string pluginUuid) : IMessage
    {
        public string Event => "setFeedback";

        public string Context { get; set; } = pluginUuid;

        public JsonElement Payload { get; set; } = payload;
    }
}
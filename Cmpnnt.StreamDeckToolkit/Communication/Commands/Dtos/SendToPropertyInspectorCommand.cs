using System.Text.Json;

namespace Cmpnnt.StreamDeckToolkit.Communication.Commands.Dtos
{
    internal class SendToPropertyInspectorCommand(string action, JsonElement data, string context) : IMessage
    {
        public string Event => "sendToPropertyInspector";

        public string Context { get; set; } = context;

        public JsonElement Payload { get; set; } = data;

        public string Action { get; set; } = action;
    }
}
using System.Text.Json;

namespace Cmpnnt.SdTools.Communication.Commands.Dtos
{
    internal class SetSettingsCommand(JsonElement settings, string context) : IMessage
    {
        public string Event => "setSettings";

        public string Context { get; set; } = context;

        public JsonElement Payload { get; set; } = settings;
    }
}
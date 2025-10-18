using System.Text.Json;

namespace Cmpnnt.SdTools.Communication.Commands.Dtos
{
    internal class SetGlobalSettingsCommand(JsonElement settings, string pluginUuid) : IMessage
    {
        public string Event => "setGlobalSettings";

        public string Context { get; set; } = pluginUuid;

        public JsonElement Payload { get; set; } = settings;
    }
}
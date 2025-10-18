using System.Text.Json;

namespace Cmpnnt.SdTools.Communication.Commands.Dtos
{
    internal class SetSettingsCommand(JsonElement settings, string context) : IMessage
    {
        public string Event => "setSettings";

        public string Context { get; set; } = context;

        /* TODO: Does the type matter here? I think the overall set settings logic can be
           left as is. It just needs minor tweaks to align with the new classes and source generation */
        public JsonElement Payload { get; set; } = settings;
    }
}
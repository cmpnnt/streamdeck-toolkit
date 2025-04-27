namespace BarRaider.SdTools.Communication.Commands.Dtos
{
    internal class GetGlobalSettingsCommand(string pluginUuid) : IMessage
    {
        public string Event => "getGlobalSettings";

        public string Context { get; set; } = pluginUuid;
    }
}
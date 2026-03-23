namespace Cmpnnt.StreamDeckToolkit.Communication.Commands.Dtos
{
    internal class GetSettingsCommand(string context) : IMessage
    {
        public string Event => "getSettings";

        public string Context { get; set; } = context;
    }
}
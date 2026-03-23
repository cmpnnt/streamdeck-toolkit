namespace Cmpnnt.StreamDeckToolkit.Communication.Commands.Dtos
{
    internal class ShowAlertCommand(string context) : IMessage
    {
        public string Event => "showAlert";

        public string Context { get; set; } = context;
    }
}
namespace Cmpnnt.SdTools.Communication.Commands.Dtos
{
    internal class ShowOkCommand(string context) : IMessage
    {
        public string Event => "showOk";

        public string Context { get; set; } = context;
    }
}
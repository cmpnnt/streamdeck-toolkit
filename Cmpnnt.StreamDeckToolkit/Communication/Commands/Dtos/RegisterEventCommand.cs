namespace Cmpnnt.StreamDeckToolkit.Communication.Commands.Dtos
{
    internal class RegisterEventCommand : IMessage
    {
        public string Event { get; set; }
        
        public string Uuid { get; set; }
        
        public RegisterEventCommand(string eventName, string uuid)
        {
            Event = eventName;
            Uuid = uuid;
        }

        public RegisterEventCommand() { }
    }
}
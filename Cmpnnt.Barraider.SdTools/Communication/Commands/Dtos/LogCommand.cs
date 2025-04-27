using BarRaider.SdTools.Communication.Payloads;

namespace BarRaider.SdTools.Communication.Commands.Dtos
{
    internal class LogCommand(string message) : IMessage
    {
        public string Event => "logMessage";

        public LogCommandPayload Payload { get; set; } = new (message);

        internal class LogCommandPayload(string message) : IPayload
        {
            public string Message { get; set; } = message;
        }
    }
}
using BarRaider.SdTools.Communication.Payloads;

namespace BarRaider.SdTools.Communication.Commands.Dtos
{
    internal class SetStateCommand(uint? state, string context) : IMessage
    {
        public string Event => "setState";

        public string Context { get; set; } = context;

        public SetStateCommandPayload Payload { get; set; } = new (state);

        internal class SetStateCommandPayload(uint? state) : IPayload
        {
            public uint? State { get; set; } = state;
        }
    }
}
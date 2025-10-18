using Cmpnnt.SdTools.Communication.Payloads;

namespace Cmpnnt.SdTools.Communication.Commands.Dtos
{
    internal class SetFeedbackLayoutCommand(string layout, string context) : IMessage
    {
        public string Event => "setFeedbackLayout";

        public string Context { get; set; } = context;

        public SetFeedbackLayoutCommandPayload Payload { get; set; } = new (layout);

        internal class SetFeedbackLayoutCommandPayload(string layout) : IPayload
        {
            public string Layout { get; set; } = layout;
        }
    }
}
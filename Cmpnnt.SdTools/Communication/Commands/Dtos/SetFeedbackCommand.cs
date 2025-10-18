using System.Collections.Generic;

namespace Cmpnnt.SdTools.Communication.Commands.Dtos
{
    internal class SetFeedbackCommand(Dictionary<string, string> dictKeyValues, string pluginUuid) : IMessage
    {
        public string Event => "setFeedback";

        public string Context { get; set; } = pluginUuid;

        public Dictionary<string, string> DictKeyValues { get; set; } = dictKeyValues;
    }
}
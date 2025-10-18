using System;
using Cmpnnt.SdTools.Communication.Payloads;

namespace Cmpnnt.SdTools.Communication.Commands.Dtos
{
    internal class OpenUrlCommand(Uri uri) : IMessage
    {
        public string Event => "openUrl";

        public OpenUrlCommandPayload Payload { get; set; } = new (uri);

        internal class OpenUrlCommandPayload(Uri uri) : IPayload
        {
            public string Url { get; set; } = uri.ToString();
        }
    }
}
using System;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;

namespace Cmpnnt.StreamDeckToolkit.Communication.Commands.Dtos
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
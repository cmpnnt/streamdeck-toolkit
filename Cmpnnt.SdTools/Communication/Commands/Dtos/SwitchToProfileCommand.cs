using Cmpnnt.SdTools.Communication.Payloads;

namespace Cmpnnt.SdTools.Communication.Commands.Dtos
{
    internal class SwitchToProfileCommand(string device, string profileName, string pluginUuid) : IMessage
    {
        public string Event => "switchToProfile";

        public string Context { get; set; } = pluginUuid;

        public string Device { get; set; } = device;

        public SwitchToProfileCommandPayload Payload { get; set; } = !string.IsNullOrEmpty(profileName) ? new SwitchToProfileCommandPayload(profileName) : new SwitchToProfileCommandPayload();

        internal class SwitchToProfileCommandPayload : IPayload
        {
            public string Profile { get; set; }

            public SwitchToProfileCommandPayload(string profile)
            {
                Profile = profile;
            }
            public SwitchToProfileCommandPayload() { }
        }
    }
}
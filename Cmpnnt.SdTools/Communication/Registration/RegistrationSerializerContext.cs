using System.Text.Json.Serialization;

namespace Cmpnnt.SdTools.Communication.Registration;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = false)]
[JsonSerializable(typeof(RegistrationInfo))]
[JsonSerializable(typeof(StreamDeckApplicationInfo))]
[JsonSerializable(typeof(StreamDeckDeviceInfo))]
[JsonSerializable(typeof(StreamDeckDeviceInfo[]))]
[JsonSerializable(typeof(StreamDeckDeviceSize))]
[JsonSerializable(typeof(StreamDeckPluginInfo))]
[JsonSerializable(typeof(DeviceType))]
internal partial class RegistrationSerializerContext : JsonSerializerContext
{
}
using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events;

[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true, WriteIndented = false)]
[JsonSerializable(typeof(BaseEvent))]
[JsonSerializable(typeof(ApplicationDidLaunchEvent))]
[JsonSerializable(typeof(ApplicationDidTerminateEvent))]
[JsonSerializable(typeof(DeviceDidConnectEvent))]
[JsonSerializable(typeof(DeviceDidDisconnectEvent))]
[JsonSerializable(typeof(DialDownEvent))]
[JsonSerializable(typeof(DialRotateEvent))]
[JsonSerializable(typeof(DialUpEvent))]
[JsonSerializable(typeof(DidReceiveGlobalSettingsEvent))]
[JsonSerializable(typeof(DidReceiveSettingsEvent))]
[JsonSerializable(typeof(KeyDownEvent))]
[JsonSerializable(typeof(KeyUpEvent))]
[JsonSerializable(typeof(PropertyInspectorDidAppearEvent))]
[JsonSerializable(typeof(PropertyInspectorDidDisappearEvent))]
[JsonSerializable(typeof(SendToPluginEvent))]
[JsonSerializable(typeof(SystemDidWakeUpEvent))]
[JsonSerializable(typeof(TitleParametersDidChangeEvent))]
[JsonSerializable(typeof(TouchTapEvent))]
[JsonSerializable(typeof(WillAppearEvent))]
[JsonSerializable(typeof(WillDisappearEvent))]
internal partial class EventSerializerContext : JsonSerializerContext
{
    
}
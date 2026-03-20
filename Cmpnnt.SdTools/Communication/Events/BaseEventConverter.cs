#nullable enable
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cmpnnt.SdTools.Communication.Events;

internal sealed class BaseEventConverter : JsonConverter<BaseEvent>
{
    public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("event", out var eventProp))
            return null;

        return eventProp.GetString() switch
        {
            "applicationDidLaunch"        => root.Deserialize(EventSerializerContext.Default.ApplicationDidLaunchEvent),
            "applicationDidTerminate"     => root.Deserialize(EventSerializerContext.Default.ApplicationDidTerminateEvent),
            "deviceDidConnect"            => root.Deserialize(EventSerializerContext.Default.DeviceDidConnectEvent),
            "deviceDidDisconnect"         => root.Deserialize(EventSerializerContext.Default.DeviceDidDisconnectEvent),
            "dialDown"                    => root.Deserialize(EventSerializerContext.Default.DialDownEvent),
            "dialRotate"                  => root.Deserialize(EventSerializerContext.Default.DialRotateEvent),
            "dialUp"                      => root.Deserialize(EventSerializerContext.Default.DialUpEvent),
            "didReceiveGlobalSettings"    => root.Deserialize(EventSerializerContext.Default.DidReceiveGlobalSettingsEvent),
            "didReceiveSettings"          => root.Deserialize(EventSerializerContext.Default.DidReceiveSettingsEvent),
            "keyDown"                     => root.Deserialize(EventSerializerContext.Default.KeyDownEvent),
            "keyUp"                       => root.Deserialize(EventSerializerContext.Default.KeyUpEvent),
            "propertyInspectorDidAppear"  => root.Deserialize(EventSerializerContext.Default.PropertyInspectorDidAppearEvent),
            "propertyInspectorDidDisappear" => root.Deserialize(EventSerializerContext.Default.PropertyInspectorDidDisappearEvent),
            "sendToPlugin"                => root.Deserialize(EventSerializerContext.Default.SendToPluginEvent),
            "systemDidWakeUp"             => root.Deserialize(EventSerializerContext.Default.SystemDidWakeUpEvent),
            "titleParametersDidChange"    => root.Deserialize(EventSerializerContext.Default.TitleParametersDidChangeEvent),
            "touchTap"                    => root.Deserialize(EventSerializerContext.Default.TouchTapEvent),
            "willAppear"                  => root.Deserialize(EventSerializerContext.Default.WillAppearEvent),
            "willDisappear"               => root.Deserialize(EventSerializerContext.Default.WillDisappearEvent),
            _                             => null
        };
    }

    public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        => throw new NotSupportedException("BaseEvent serialization is not supported.");
}
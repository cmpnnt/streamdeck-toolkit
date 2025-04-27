using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using BarRaider.SdTools.Exceptions;
using BarRaider.SdTools.Utilities;

namespace BarRaider.SdTools.Communication.Events
{
    /// <summary>
    /// Base event that all the actual events derive from
    /// </summary>
    public abstract class BaseEvent
    {
        /// <summary>
        /// Name of the event raised
        /// </summary>
        public string Event { get; set; }

        #nullable enable
        internal static BaseEvent? Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;

            try
            {
                JsonNode? jsonNode = JsonNode.Parse(json);
                if (jsonNode is null) return null;

                var eventType = (string?)jsonNode["event"];

                return eventType switch
                {
                    EventTypes.APPLICATION_DID_LAUNCH => JsonSerializer.Deserialize(json, EventSerializerContext.Default.ApplicationDidLaunchEvent)!,
                    EventTypes.APPLICATION_DID_TERMINATE => JsonSerializer.Deserialize(json, EventSerializerContext.Default.ApplicationDidTerminateEvent)!,
                    EventTypes.DEVICE_DID_CONNECT => JsonSerializer.Deserialize(json, EventSerializerContext.Default.DeviceDidConnectEvent)!,
                    EventTypes.DEVICE_DID_DISCONNECT => JsonSerializer.Deserialize(json, EventSerializerContext.Default.DeviceDidDisconnectEvent)!,
                    EventTypes.DIAL_DOWN => JsonSerializer.Deserialize(json, EventSerializerContext.Default.DialDownEvent)!,
                    EventTypes.DIAL_ROTATE => JsonSerializer.Deserialize(json, EventSerializerContext.Default.DialRotateEvent)!,
                    EventTypes.DIAL_UP => JsonSerializer.Deserialize(json, EventSerializerContext.Default.DialUpEvent)!,
                    EventTypes.DID_RECEIVE_GLOBAL_SETTINGS => JsonSerializer.Deserialize(json, EventSerializerContext.Default.DidReceiveGlobalSettingsEvent)!,
                    EventTypes.DID_RECEIVE_SETTINGS => JsonSerializer.Deserialize(json, EventSerializerContext.Default.DidReceiveSettingsEvent)!,
                    EventTypes.KEY_DOWN => JsonSerializer.Deserialize(json, EventSerializerContext.Default.KeyDownEvent)!,
                    EventTypes.KEY_UP => JsonSerializer.Deserialize(json, EventSerializerContext.Default.KeyUpEvent)!,
                    EventTypes.PROPERTY_INSPECTOR_DID_APPEAR => JsonSerializer.Deserialize(json, EventSerializerContext.Default.PropertyInspectorDidAppearEvent)!,
                    EventTypes.PROPERTY_INSPECTOR_DID_DISAPPEAR => JsonSerializer.Deserialize(json, EventSerializerContext.Default.PropertyInspectorDidDisappearEvent)!,
                    EventTypes.SEND_TO_PLUGIN => JsonSerializer.Deserialize(json, EventSerializerContext.Default.SendToPluginEvent)!,
                    EventTypes.SYSTEM_DID_WAKE_UP => JsonSerializer.Deserialize(json, EventSerializerContext.Default.SystemDidWakeUpEvent)!,
                    EventTypes.TITLE_PARAMETERS_DID_CHANGE => JsonSerializer.Deserialize(json, EventSerializerContext.Default.TitleParametersDidChangeEvent)!,
                    EventTypes.TOUCHTAP => JsonSerializer.Deserialize(json, EventSerializerContext.Default.TouchTapEvent)!,
                    EventTypes.WILL_APPEAR => JsonSerializer.Deserialize(json, EventSerializerContext.Default.WillAppearEvent)!,
                    EventTypes.WILL_DISAPPEAR => JsonSerializer.Deserialize(json, EventSerializerContext.Default.WillDisappearEvent)!,
                    _ => throw new UnknownEventException(eventType, json)
                };
            }
            catch (JsonException ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Fatal, $"JSON parsing/deserialization failed: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Fatal,
                    $"An unexpected error occurred during event parsing: {ex.Message}");
                return null;
            }
        }
    }
}
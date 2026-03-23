using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Utilities;

namespace Cmpnnt.StreamDeckToolkit.Communication.Events
{
    /// <summary>
    /// Base event that all the actual events derive from
    /// </summary>
    [JsonConverter(typeof(BaseEventConverter))]
    public abstract class BaseEvent
    {
        #nullable enable
        internal static BaseEvent? Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;

            try
            {
                return JsonSerializer.Deserialize(json, EventSerializerContext.Default.BaseEvent);
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

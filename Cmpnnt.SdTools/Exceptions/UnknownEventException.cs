using System;

namespace Cmpnnt.SdTools.Exceptions;

public class UnknownEventException(string eventType, string jsonPayload)
    : Exception($"Unknown event type ${eventType} received.")
{
    #nullable enable
    public string? Event { get; } = eventType;
    public string JsonPayload { get; } = jsonPayload;
}
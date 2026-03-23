using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Communication.Commands.Dtos;

namespace Cmpnnt.StreamDeckToolkit.Communication.Commands
{
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = false)]
    [JsonSerializable(typeof(RegisterEventCommand))]
    [JsonSerializable(typeof(SetTitleCommand))]
    [JsonSerializable(typeof(SetTitleCommand.SetTitleCommandPayload))]
    [JsonSerializable(typeof(LogCommand))]
    [JsonSerializable(typeof(LogCommand.LogCommandPayload))]
    [JsonSerializable(typeof(SetImageCommand))]
    [JsonSerializable(typeof(SetImageCommand.SetImageCommandPayload))]
    [JsonSerializable(typeof(ShowAlertCommand))]
    [JsonSerializable(typeof(ShowOkCommand))]
    [JsonSerializable(typeof(SetGlobalSettingsCommand))]
    [JsonSerializable(typeof(GetGlobalSettingsCommand))]
    [JsonSerializable(typeof(SetSettingsCommand))]
    [JsonSerializable(typeof(GetSettingsCommand))]
    [JsonSerializable(typeof(SetStateCommand))]
    [JsonSerializable(typeof(SetStateCommand.SetStateCommandPayload))]
    [JsonSerializable(typeof(SendToPropertyInspectorCommand))]
    [JsonSerializable(typeof(SwitchToProfileCommand))]
    [JsonSerializable(typeof(SwitchToProfileCommand.SwitchToProfileCommandPayload))]
    [JsonSerializable(typeof(OpenUrlCommand))]
    [JsonSerializable(typeof(OpenUrlCommand.OpenUrlCommandPayload))]
    [JsonSerializable(typeof(SetFeedbackCommand))]
    [JsonSerializable(typeof(SetFeedbackCommandEx))]
    [JsonSerializable(typeof(SetFeedbackLayoutCommand))]
    [JsonSerializable(typeof(SetFeedbackLayoutCommand.SetFeedbackLayoutCommandPayload))]
    internal partial class CommandSerializerContext : JsonSerializerContext
    {
    }
}
using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Attributes;

namespace Cmpnnt.StreamDeckToolkit.SamplePlugin
{
    [SdSettings]
    internal partial class PluginActionSettings
    {
        public static PluginActionSettings CreateDefaultSettings() =>
            new() { Name = string.Empty, ShowName = false };

        public string Name { get; set; }
        public bool ShowName { get; set; }

        public override string ToString() => $"Name: {Name}, ShowName: {ShowName}";
    }

    [SdSettings]
    internal partial class PluginAction2Settings
    {
        public static PluginAction2Settings CreateDefaultSettings() =>
            new() { OutputFileName = string.Empty, InputString = string.Empty };

        [FilenameProperty]
        [JsonPropertyName("outputFileName")]
        public string OutputFileName { get; set; }

        [JsonPropertyName("inputString")]
        public string InputString { get; set; }

        public override string ToString() => $"OutputFileName: {OutputFileName}, InputString: {InputString}";
    }

    [SdSettings]
    internal partial class PluginAction3Settings
    {
        public static PluginAction3Settings CreateDefaultSettings() =>
            new() { Name = string.Empty, ShowName = false };

        public string Name { get; set; }
        public bool ShowName { get; set; }

        public override string ToString() => $"Name: {Name}, ShowName: {ShowName}";
    }

    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
    [JsonSerializable(typeof(PluginActionSettings))]
    [JsonSerializable(typeof(PluginAction2Settings))]
    [JsonSerializable(typeof(PluginAction3Settings))]
    internal partial class SamplePluginSerializerContext : JsonSerializerContext { }
}

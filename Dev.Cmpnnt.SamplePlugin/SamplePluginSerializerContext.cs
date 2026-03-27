using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Attributes;

namespace Cmpnnt.StreamDeckToolkit.SamplePlugin
{
    [SdSettings]
    internal partial class PluginActionSettings
    {
        public static PluginActionSettings CreateDefaultSettings()
        {
            return new PluginActionSettings { Name = string.Empty, ShowName = false };
        }

        public string Name { get; set; }
        public bool ShowName { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, ShowName: {ShowName}";
        }
    }

    [SdSettings]
    internal partial class PluginAction2Settings
    {
        public static PluginAction2Settings CreateDefaultSettings()
        {
            return new PluginAction2Settings { OutputFileName = string.Empty, InputString = string.Empty };
        }

        [FilenameProperty]
        [JsonPropertyName("outputFileName")]
        public string OutputFileName { get; set; }

        [JsonPropertyName("inputString")]
        public string InputString { get; set; }

        public override string ToString()
        {
            return $"OutputFileName: {OutputFileName}, InputString: {InputString}";
        }
    }

    [SdSettings]
    internal partial class PluginAction3Settings
    {
        public static PluginAction3Settings CreateDefaultSettings()
        {
            return new PluginAction3Settings { Name = string.Empty, ShowName = false };
        }

        public string Name { get; set; }
        public bool ShowName { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, ShowName: {ShowName}";
        }
    }

    // TODO: Document verbose logging and collapsable PI groups
    internal class SamplePluginGlobalSettings
    {
        public static SamplePluginGlobalSettings CreateDefaultSettings()
        {
            return new SamplePluginGlobalSettings { VerboseLogging = false };
        }

        public bool VerboseLogging { get; set; }
    }

    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
    [JsonSerializable(typeof(PluginActionSettings))]
    [JsonSerializable(typeof(PluginAction2Settings))]
    [JsonSerializable(typeof(PluginAction3Settings))]
    [JsonSerializable(typeof(SamplePluginGlobalSettings))]
    internal partial class SamplePluginSerializerContext : JsonSerializerContext { }
}

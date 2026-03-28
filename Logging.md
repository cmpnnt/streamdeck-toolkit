# Logging

This toolkit uses [Serilog](https://serilog.net/) for logging. By default, it is configured to log to a file named 
`pluginlog_<date>.log` in the plugin directory (your build directory), with a rolling interval of one day. 

The format is `[date] [timestamp]|[log level]|[thread ID]|[source context]: [message]`. For example:
```
2026-03-27 15:32:00.2180|INFORMATION|6|ReceiveAsync:
```

The log level is set to `Warning` by default, but you can allow your users to enable more verbose logging in the Global Settings. 
To do this, set the `EnableVerboseLogging` property in the `GlobalSettings` class to `true`. This will set the log level to `Verbose`, 
which will include all log messages. An example can be found in the sample plugin's [PluginAction3.cs](https://github.com/cmpnnt/streamdeck-toolkit/blob/main/Dev.Cmpnnt.SamplePlugin/PluginAction3.cs) 
file and in [SamplePluginGlobalSettings.cs](https://github.com/cmpnnt/streamdeck-toolkit/blob/main/Dev.Cmpnnt.SamplePlugin/SamplePluginSerializerContext.cs).

In PluginAction3.cs:
```csharp
        public GroupStart AdvancedSettingsGroup = new() { Label = "Advanced Settings" };

        public Checkbox VerboseLoggingCheckbox = new()
        {
            Label = "Verbose Logging",
            PersistenceSettings = new() { Global = true, Setting = "VerboseLogging" }
        };

        public GroupEnd AdvancedSettingsGroupEnd = new();
```

In SamplePluginGlobalSettings.cs:
```csharp
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
```
# Quick Start: Building a Stream Deck Plugin

This guide walks through creating a plugin from scratch using `streamdeck-toolkit`. The build system, manifest, and property inspector HTML are all generated automatically — you write C# and the tooling handles the rest.

---

## Prerequisites

- .NET 10 SDK
- Stream Deck software installed
- Stream Deck CLI (`streamdeck`) on your PATH

---

## 1. Project setup

Create a new console app and configure the `.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk" InitialTargets="BuildTasks">

  <!-- Load custom MSBuild tasks from the BuildTasks assembly -->
  <UsingTask TaskName="Cmpnnt.StreamDeckToolkit.BuildTasks.CloseStreamDeck"   TaskFactory="TaskHostFactory" AssemblyFile="$(TaskAssemblyPath)" />
  <UsingTask TaskName="Cmpnnt.StreamDeckToolkit.BuildTasks.GenerateManifest"  AssemblyFile="$(TaskAssemblyPath)" />
  <UsingTask TaskName="Cmpnnt.StreamDeckToolkit.BuildTasks.ExtractSdpiHtml"   TaskFactory="TaskHostFactory" AssemblyFile="$(TaskAssemblyPath)" />
  <UsingTask TaskName="Cmpnnt.StreamDeckToolkit.BuildTasks.LinkPlugin"        TaskFactory="TaskHostFactory" AssemblyFile="$(TaskAssemblyPath)" />
  <UsingTask TaskName="Cmpnnt.StreamDeckToolkit.BuildTasks.PackagePlugin"     TaskFactory="TaskHostFactory" AssemblyFile="$(TaskAssemblyPath)" />
  <UsingTask TaskName="Cmpnnt.StreamDeckToolkit.BuildTasks.OpenStreamDeck"    TaskFactory="TaskHostFactory" AssemblyFile="$(TaskAssemblyPath)" />

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>14</LangVersion>
    <PublishAot>true</PublishAot>
    <AssemblyName>com.mycompany.myplugin</AssemblyName>
    <Version>1.0.0</Version>
    <Authors>Your Name</Authors>
    <Description>A plugin that does something useful.</Description>
    <PackageProjectUrl>https://example.com/myplugin</PackageProjectUrl>
  </PropertyGroup>

  <!-- Derive output paths from the assembly name -->
  <PropertyGroup>
    <TaskAssemblyPath>path\to\Cmpnnt.StreamDeckToolkit.BuildTasks.dll</TaskAssemblyPath>
    <AssemblyNameLower>$([System.String]::Copy('$(AssemblyName)').ToLowerInvariant())</AssemblyNameLower>
    <OutDir>bin\$(Configuration)\$(Platform)\$(AssemblyNameLower).sdPlugin\</OutDir>
    <BaseDir>$(MSBuildThisFileDirectory)bin\$(Configuration)\$(Platform)</BaseDir>
  </PropertyGroup>

  <!-- Expose MSBuild properties to source generators -->
  <ItemGroup>
    <CompilerVisibleProperty Include="AssemblyName" />
    <CompilerVisibleProperty Include="Version" />
    <CompilerVisibleProperty Include="Authors" />
    <CompilerVisibleProperty Include="Description" />
    <CompilerVisibleProperty Include="PackageProjectUrl" />
  </ItemGroup>

  <!-- Copy images and property inspector files to output -->
  <ItemGroup>
    <Content Include="Images\**\*.png">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
    <Content Include="PropertyInspector\**\*.*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>

  <!-- Source generators run as an analyzer, not a compile-time reference -->
  <ItemGroup>
    <ProjectReference Include="..\..\Cmpnnt.StreamDeckToolkit.SourceGenerators\..."
                      ReferenceOutputAssembly="false" OutputItemType="Analyzer" />
    <ProjectReference Include="..\..\Cmpnnt.StreamDeckToolkit.BuildTasks\..."
                      ReferenceOutputAssembly="false" />
    <ProjectReference Include="..\..\Cmpnnt.StreamDeckToolkit\..." />
  </ItemGroup>

  <!-- Required for SDPI HTML extraction (see section 5) -->
  <PropertyGroup>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)Generated</CompilerGeneratedFilesOutputPath>
  </PropertyGroup>

  <!-- Build targets (see section 6) -->
  <Target Name="BuildTasks">
    <MSBuild Projects="path\to\Cmpnnt.StreamDeckToolkit.BuildTasks.csproj" />
  </Target>
  <Target Name="CloseStreamDeck" BeforeTargets="BeforeBuild" Condition="'$(DesignTimeBuild)' != 'true'">
    <Cmpnnt.StreamDeckToolkit.BuildTasks.CloseStreamDeck PluginName="$(AssemblyName)" />
  </Target>
  <Target Name="GenerateManifest" AfterTargets="Build" Condition="'$(DesignTimeBuild)' != 'true'">
    <Cmpnnt.StreamDeckToolkit.BuildTasks.GenerateManifest
        PluginAssemblyPath="$(BaseDir)\$(AssemblyNameLower).sdPlugin\$(AssemblyNameLower).dll"
        OutputManifestPath="$(BaseDir)\$(AssemblyNameLower).sdPlugin\manifest.json" />
  </Target>
  <Target Name="RenameOutputDirectory" AfterTargets="Build" DependsOnTargets="GenerateManifest" Condition="'$(DesignTimeBuild)' != 'true'">
    <Cmpnnt.StreamDeckToolkit.BuildTasks.LinkPlugin    Condition="'$(Configuration)' == 'Debug'"   BuildDir="$(BaseDir)\$(AssemblyNameLower).sdPlugin" PluginName="$(AssemblyNameLower)" />
    <Cmpnnt.StreamDeckToolkit.BuildTasks.PackagePlugin Condition="'$(Configuration)' == 'Release'" BuildDir="$(BaseDir)\$(AssemblyNameLower).sdPlugin" />
    <Cmpnnt.StreamDeckToolkit.BuildTasks.OpenStreamDeck />
  </Target>
  <Target Name="WriteSdpiHtmlOutput" AfterTargets="CoreCompile" Condition="'$(DesignTimeBuild)' != 'true'">
    <ItemGroup>
      <SdpiGeneratedFile Include="$(CompilerGeneratedFilesOutputPath)\**\GeneratedSdpiComponents.g.cs" />
    </ItemGroup>
    <Cmpnnt.StreamDeckToolkit.BuildTasks.ExtractSdpiHtml
        Condition="'@(SdpiGeneratedFile)' != ''"
        GeneratedFile="@(SdpiGeneratedFile)">
      <Output TaskParameter="GeneratedHtmlFiles" ItemName="SdpiHtmlFiles" />
    </Cmpnnt.StreamDeckToolkit.BuildTasks.ExtractSdpiHtml>
    <Copy SourceFiles="@(SdpiHtmlFiles)"
          DestinationFiles="@(SdpiHtmlFiles->'$(OutDir)%(Identity)')"
          SkipUnchangedFiles="true"
          Condition="'@(SdpiHtmlFiles)' != ''" />
  </Target>

</Project>
```

---

## 2. Entry point

In `Program.cs`, declare plugin-level metadata with `[StreamDeckPlugin]` and pass the source-generated `PluginActionIdRegistry` to `Toolkit.Run`. The registry is generated automatically — you never write it by hand.

```csharp
using Cmpnnt.StreamDeckToolkit.Attributes;
using Cmpnnt.StreamDeckToolkit.Enums;
using Cmpnnt.StreamDeckToolkit.Runtime;

[assembly: StreamDeckPlugin(
    Name             = "My Plugin",
    UUID             = "com.mycompany.myplugin",
    Category         = "My Category",
    CategoryIcon     = "Images/categoryIcon",
    Icon             = "Images/pluginIcon",
    SDKVersion       = 2,
    SoftwareMinVersion = SoftwareMinVersion.V6_5,
    WindowsMinVersion  = "10",
    MacMinVersion      = "12"
)]

class Program
{
    static void Main(string[] args) => Toolkit.Run(args, new PluginActionIdRegistry());
}
```

`PluginActionIdRegistry` is generated by the `PluginRegistrar` source generator. It scans all classes that implement `ICommonPluginFunctions` (i.e. subclass `KeypadBase`, `EncoderBase`, or `KeyAndEncoderBase`) and emits a `FrozenDictionary` of action-ID → constructor factory. See [SourceGenerators.md — PluginRegistrar](https://github.com/cmpnnt/streamdeck-toolkit/wiki/Source-Generators#1-pluginregistrar).

---

## 3. Create an action

Subclass one of the base action classes and apply `[StreamDeckAction]`:

| Base class | Controllers entry | Use when |
|---|---|---|
| `KeypadBase` | `["Keypad"]` | Key/button only |
| `EncoderBase` | `["Encoder"]` | Dial/touchpad only |
| `KeyAndEncoderBase` | `["Keypad", "Encoder"]` | Both |

```csharp
using Cmpnnt.StreamDeckToolkit.Actions;
using Cmpnnt.StreamDeckToolkit.Attributes;
using Cmpnnt.StreamDeckToolkit.Connections;
using Cmpnnt.StreamDeckToolkit.Payloads;

[StreamDeckAction(
    Name                  = "My Action",
    Tooltip               = "Does something useful",
    Icon                  = "Images/pluginAction",
    SupportedInMultiActions = true
)]
public partial class MyAction : KeypadBase
{
    private MyActionSettings settings;

    public MyAction(IOutboundConnection connection, InitialPayload payload)
        : base(connection, payload)
    {
        settings = payload.Settings?.Deserialize(MySerializerContext.Default.MyActionSettings)
                   ?? MyActionSettings.CreateDefaultSettings();
    }

    public override void KeyPressed(KeyPayload payload) { /* ... */ }
    public override void KeyReleased(KeyPayload payload) { /* ... */ }
    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        settings.PopulateFromJson(payload.Settings);
        SaveSettings();
    }
    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }
    public override void OnTick() { }
    public override void Dispose() { }
}
```

The action's UUID is derived automatically from its fully-qualified class name (lowercased). For the class above in namespace `MyCompany.MyPlugin`, the UUID would be `mycompany.myplugin.myaction`. You never set it manually.

---

## 4. Settings

Settings classes must be `partial` and carry `[SdSettings]`. The source generator implements `ISettingsPopulatable`, which provides an AOT-safe `PopulateFromJson(JsonElement)` method that merges only the properties present in the incoming JSON (patch semantics, not full replacement).

```csharp
using System.Text.Json.Serialization;
using Cmpnnt.StreamDeckToolkit.Attributes;

[SdSettings]
internal partial class MyActionSettings
{
    public static MyActionSettings CreateDefaultSettings() =>
        new() { Name = string.Empty, Enabled = false };

    public string Name    { get; set; }
    public bool   Enabled { get; set; }

    // Use [FilenameProperty] for file-picker inputs — strips the "C:\fakepath\" prefix
    [FilenameProperty]
    [JsonPropertyName("outputFile")]
    public string OutputFile { get; set; }
}
```

Register all settings types in a `JsonSerializerContext` for AOT-safe deserialization:

```csharp
using System.Text.Json.Serialization;

[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(MyActionSettings))]
internal partial class MySerializerContext : JsonSerializerContext { }
```

See [SourceGenerators.md — SettingsPopulatorGenerator](https://github.com/cmpnnt/streamdeck-toolkit/wiki/Source-Generators#3-settingspopulatorgenerator) for the full type-handling table and diagnostics.

---

## 5. Property inspector (SDPI)

Declare SDPI UI components as fields on your action class and apply `[SdpiOutputDirectory]`. The `SdpiGenerator` source generator emits the corresponding HTML document to `PropertyInspector/{ClassName}.html` automatically — no hand-authored HTML needed.

```csharp
using Cmpnnt.StreamDeckToolkit.Attributes;
using Cmpnnt.StreamDeckToolkit.Components;
using Cmpnnt.StreamDeckToolkit.Components.Models;

[SdpiOutputDirectory("PropertyInspector/")]
[StreamDeckAction(Name = "My Action", Icon = "Images/pluginAction")]
public partial class MyAction : KeypadBase
{
    public Textfield nameField = new()
    {
        Label   = "Name",
        Setting = "name",
    };

    public Select modeSelect = new()
    {
        Label   = "Mode",
        Setting = "mode",
        DataSourceSettings = new DataSourceSettings
        {
            Options =
            [
                new OptionSetting { Value = "fast",  Label = "Fast"  },
                new OptionSetting { Value = "slow",  Label = "Slow"  },
            ]
        }
    };

    // ... constructor, event handlers
}
```

The `ExtractSdpiHtml` build task reads the generated C# constants and writes the final `.html` files to your plugin output directory. The manifest generator uses the same `PropertyInspector/{ClassName}.html` convention by default, so the two are automatically consistent.

Supported components: `TextArea`, `Textfield`, `Password`, `Checkbox`, `CheckboxList`, `Radio`, `Select`, `Button`, `File`, `Color`, `Range`, `Date/Time/Month/Week`. See [SourceGenerators.md — SdpiGenerator](https://github.com/cmpnnt/streamdeck-toolkit/wiki/Source-Generators#4-sdpigenerator) for the full list.

---

## 6. Manifest generation

You never write `manifest.json` by hand. It is generated from three layers (highest priority first):

```
ManifestConfigBase  >  [StreamDeckPlugin] / [StreamDeckAction]  >  MSBuild  >  Convention
```

For complex nested values (`States`, `Encoder`, `ApplicationsToMonitor`, `Profiles`) that cannot be expressed as attribute arguments, subclass `ManifestConfigBase`:

```csharp
using Cmpnnt.StreamDeckToolkit.Manifest;

internal class PluginManifestConfig : ManifestConfigBase
{
    public override ManifestStateConfig[] DefaultStates =>
    [
        new ManifestStateConfig { Image = "Images/pluginAction", TitleAlignment = "middle" }
    ];

    public override ManifestEncoderConfig DefaultEncoder => new()
    {
        Layout = "$B1",
        TriggerDescription = new ManifestTriggerDescription
        {
            Rotate = "Adjust level",
            Push   = "Reset",
        }
    };

    public override ManifestApplicationsToMonitor ApplicationsToMonitor => new()
    {
        Windows = ["myapp.exe"],
        Mac     = ["com.example.myapp"],
    };
}
```

After the build completes, the `GenerateManifest` MSBuild task loads the compiled plugin DLL, calls the generated `ManifestProvider.GetManifestData()` via reflection, and serializes the result to `manifest.json` in the plugin output directory.

See [ManifestGeneration.md](ManifestGeneration.md) for the full attribute and property reference.

---

## 7. Build behaviour

The `.csproj` targets wire together a complete plugin development workflow:

| Phase | Task | What it does |
|---|---|---|
| `BeforeBuild` | `CloseStreamDeck` | Kills the Stream Deck app and your plugin process so their binaries can be overwritten |
| `CoreCompile` | Source generators | Emit `PluginActionIdRegistry.g.cs`, `GeneratedManifestModel.g.cs`, `*.PopulateFromJson.g.cs`, `GeneratedSdpiComponents.g.cs` |
| `AfterCoreCompile` | `ExtractSdpiHtml` | Reads `GeneratedSdpiComponents.g.cs`, writes `.html` files to the plugin output directory |
| `AfterBuild` | `GenerateManifest` | Loads the compiled DLL, calls `ManifestProvider.GetManifestData()`, writes `manifest.json` |
| `AfterBuild` (Debug) | `LinkPlugin` | Runs `streamdeck link` to symlink the build directory into the Stream Deck plugins folder — no manual copy needed on each rebuild |
| `AfterBuild` (Release) | `PackagePlugin` | Runs `streamdeck pack` to produce an installable `.sdPlugin` archive |
| `AfterBuild` | `OpenStreamDeck` | Restarts the Stream Deck app so it picks up the updated plugin |

On a Debug build you can simply hit **Build** in your IDE and Stream Deck will reload the plugin automatically.
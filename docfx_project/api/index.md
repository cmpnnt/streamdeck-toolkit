# Cmpnnt.StreamDeckToolkit API Reference

This section contains detailed API documentation for the `Cmpnnt.StreamDeckToolkit` library.

## Entry point

| Member | Description |
|---|---|
| `Toolkit.Run(args, registry)` | Starts the plugin and connects to the Stream Deck software |

## Action base classes

Derive from one of these abstract classes depending on the hardware your action targets.

| Class | Controllers | Use when |
|---|---|---|
| `KeypadBase` | `["Keypad"]` | Key/button actions only |
| `EncoderBase` | `["Encoder"]` | Dial/touchpad actions only |
| `KeyAndEncoderBase` | `["Keypad", "Encoder"]` | Actions that support both |

## Outbound connection (`IOutboundConnection`)

Methods available on the `Connection` property inside every action class.

### Settings
  * `SetSettingsAsync`
  * `GetSettingsAsync`
  * `SetGlobalSettingsAsync`
  * `GetGlobalSettingsAsync`

### UI
  * `SetTitleAsync`
  * `SetImageAsync`
  * `SetDefaultImageAsync`
  * `SetStateAsync`
  * `ShowAlert`
  * `ShowOk`

### Dial feedback
  * `SetFeedbackAsync`
  * `SetFeedbackLayoutAsync`

### Navigation
  * `SwitchProfileAsync`
  * `OpenUrlAsync`

### Property inspector
  * `SendToPropertyInspectorAsync`

### Diagnostics
  * `LogSdMessage`

## Attributes

| Attribute | Target | Purpose |
|---|---|---|
| `[StreamDeckPlugin]` | Assembly or class | Plugin-level metadata (name, UUID, icons, OS versions) |
| `[StreamDeckAction]` | Action class | Action-level metadata (name, tooltip, icon, capabilities) |
| `[SdSettings]` | Partial settings class | Generates AOT-safe `PopulateFromJson` implementation |
| `[FilenameProperty]` | `string` property | Strips the `C:\fakepath\` prefix added by Stream Deck |
| `[SdpiOutputDirectory]` | Action class | Tells the SDPI generator where to write the HTML file |

## Manifest (`ManifestConfigBase`)

Subclass `ManifestConfigBase` to supply values that cannot be expressed as attribute arguments.

  * `DefaultStates` — `ManifestStateConfig[]`
  * `DefaultEncoder` — `ManifestEncoderConfig`
  * `ApplicationsToMonitor` — `ManifestApplicationsToMonitor`
  * `Profiles` — `ManifestProfile[]`

## SDPI components

Declare these as fields on an action class decorated with `[SdpiOutputDirectory]` to generate property inspector HTML.

  * `Textfield`
  * `TextArea`
  * `Password`
  * `Select`
  * `Checkbox`
  * `CheckboxList`
  * `Radio`
  * `Range`
  * `Color`
  * `File`
  * `Button`
  * `Delegate`
  * `Date` / `Time` / `DateTime` / `Month` / `Week`

## Utilities

| Class | Purpose |
|---|---|
| `Logger.Instance` | Singleton logger (`LogMessage(TracingLevel, string)`) |
| `GlobalSettingsManager.Instance` | Read/write global plugin settings |
| `Tools` | Image encoding, key image generation, filename helpers, formatting, hashing |
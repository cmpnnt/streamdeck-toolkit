# Changes from StreamDeck Tools

There are a number of changes to Barraider's StreamDeck Tools. Some of the changes are stylistic, while others are focused on ease of use, performance and compatibility. There are also several breaking changes, meaning this library is not backwards compatible with the original.

The two biggest updates are **Native AOT** compatibility and **cross-platform** enhancements.

In no particular order:

- Native AOT compatibility
- Cross-platform compatibility (BarRaider added cross-platform support before I released this publicly).
- Automatic manifest generation.
- New MSBuild tasks to automate plugin installation during development and plugin packaging.
- Replaced `Newtonsoft.Json` with source-generated `System.Text.Json`.
- Replaced reflection-based plugin action autoloading with source generation.
- `System.Drawing` replaced with [Skia Sharp](https://github.com/mono/skiasharp) for cross-platform purposes (ditto cross-platform).
- Sample plugin updated to use Skia Sharp, and include dial press and encoder examples
- Refactored to use new language features.
- Removed code marked `deprecated` and `obsolete` by Barraider.
- Dropped legacy .NET Framework in favor of .NET 8 minimum.
- Replace Easy PI with [SDPI Components](https://sdpi-components.dev).
- TODO: Update to latest Stream Deck SDK and add new devices then remove this bullet point.
- Removed PluginActionId attribute in favor of automatic generation based on the class name.
  - This is part of a new source generation process that replaces reflection-based plugin autoloading.
  - The Plugin ID is now generated based on the class name and is automatically populated in manifest.json.
- TODO: Reversioning the changelog
  - The library no longer tracks with the Stream Deck SDK version. 

## Backlogged TODO:

In addition to the TODO comments in the code:

- Update documentation and wiki to reflect changes
- CI Pipeline
- Test suite
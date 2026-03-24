#nullable enable
using System;

namespace Cmpnnt.StreamDeckToolkit.Attributes
{
    /// <summary>
    /// Configures plugin-level manifest properties. Values set here override MSBuild properties
    /// but are overridden by <see cref="Cmpnnt.StreamDeckToolkit.Manifest.ManifestConfigBase"/>.
    /// Apply to the assembly (<c>[assembly: StreamDeckPlugin(...)]</c>) or to any class in the project.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class StreamDeckPluginAttribute : Attribute
    {
        /// <summary>Display name of the plugin shown in Stream Deck.</summary>
        public string? Name { get; set; }

        /// <summary>
        /// Plugin UUID in reverse-domain format (e.g. <c>"com.mycompany.myplugin"</c>).
        /// Defaults to the lowercase assembly name.
        /// </summary>
        public string? UUID { get; set; }

        /// <summary>Category label shown in the Stream Deck action list.</summary>
        public string? Category { get; set; }

        /// <summary>Path to the category icon (without extension).</summary>
        public string? CategoryIcon { get; set; }

        /// <summary>Path to the plugin icon (without extension).</summary>
        public string? Icon { get; set; }

        /// <summary>URL for the plugin's support page.</summary>
        public string? SupportURL { get; set; }

        /// <summary>URL for the plugin's website. Falls back to the MSBuild <c>PackageProjectUrl</c> property.</summary>
        public string? URL { get; set; }

        /// <summary>Stream Deck SDK version. Must be 2 or 3.</summary>
        public int SDKVersion { get; set; } = 2;

        /// <summary>Minimum required Stream Deck software version.</summary>
        public SoftwareMinVersion SoftwareMinVersion { get; set; } = SoftwareMinVersion.V6_4;

        /// <summary>Minimum Windows version required (e.g. <c>"10"</c>). Omits Windows from the OS list when not set.</summary>
        public string? WindowsMinVersion { get; set; }

        /// <summary>Minimum macOS version required (e.g. <c>"12"</c>). Omits macOS from the OS list when not set.</summary>
        public string? MacMinVersion { get; set; }

        /// <summary>Default property inspector HTML path for all actions. Individual actions can override this.</summary>
        public string? PropertyInspectorPath { get; set; }

        /// <summary>Windows-specific executable name override for <c>CodePathWin</c>. Defaults to <c>{assemblyname}.exe</c>.</summary>
        public string? CodePathWin { get; set; }

        /// <summary>macOS executable name for <c>CodePathMac</c>.</summary>
        public string? CodePathMac { get; set; }
    }

    /// <summary>Minimum Stream Deck software version required by the plugin.</summary>
    public enum SoftwareMinVersion
    {
        V6_4 = 0,
        V6_5 = 1,
        V6_6 = 2,
        V6_7 = 3,
        V6_8 = 4,
        V6_9 = 5,
        V7_0 = 6,
        V7_1 = 7,
        V7_2 = 8,
        V7_3 = 9,
    }
}

#nullable enable
namespace Cmpnnt.StreamDeckToolkit.Manifest
{
    /// <summary>
    /// Base class for providing manifest configuration that cannot be derived from code or MSBuild properties.
    /// Subclass this in your plugin project to supply default states, encoder configuration, application
    /// monitoring, and profile settings. Values from this class take highest priority — they override
    /// both <c>[StreamDeckPlugin]</c>/<c>[StreamDeckAction]</c> attributes and MSBuild properties.
    /// </summary>
    public abstract class ManifestConfigBase
    {
        /// <summary>
        /// Default states applied to all actions that do not define their own.
        /// Returns <c>null</c> to use the generator's built-in default (a single state with
        /// <c>Image = "Images/pluginAction"</c>).
        /// </summary>
        public virtual ManifestStateConfig[]? DefaultStates => null;

        /// <summary>
        /// Default encoder configuration applied to all encoder actions.
        /// Returns <c>null</c> to omit encoder configuration.
        /// </summary>
        public virtual ManifestEncoderConfig? DefaultEncoder => null;

        /// <summary>
        /// Applications whose launch/termination the plugin wants to be notified about.
        /// Returns <c>null</c> to omit <c>ApplicationsToMonitor</c> from the manifest.
        /// </summary>
        public virtual ManifestApplicationsToMonitor? ApplicationsToMonitor => null;

        /// <summary>
        /// Stream Deck profiles to install with the plugin.
        /// Returns <c>null</c> to omit <c>Profiles</c> from the manifest.
        /// </summary>
        public virtual ManifestProfile[]? Profiles => null;
    }
}

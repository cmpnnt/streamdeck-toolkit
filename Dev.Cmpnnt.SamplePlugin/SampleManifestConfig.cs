using Cmpnnt.StreamDeckToolkit.Manifest;

namespace Cmpnnt.StreamDeckToolkit.SamplePlugin
{
    /// <summary>
    /// Provides manifest configuration that cannot be expressed through attributes or MSBuild properties.
    /// Override properties here to supply states, encoder settings, monitored applications, and profiles.
    /// These values take highest priority and override attribute and MSBuild values.
    /// </summary>
    internal class SampleManifestConfig : ManifestConfigBase
    {
        public override ManifestStateConfig[] DefaultStates =>
        [
            new ManifestStateConfig
            {
                Image = "Images/pluginAction",
                TitleAlignment = "middle",
                FontSize = 12,
            }
        ];

        public override ManifestEncoderConfig DefaultEncoder => new()
        {
            Layout = "$B1",
            TriggerDescription = new ManifestTriggerDescription
            {
                Push = "Play / Pause",
                Rotate = "Adjust Volume",
                Touch = "Play / Pause",
                LongTouch = "Skip Track",
            }
        };

        public override ManifestApplicationsToMonitor ApplicationsToMonitor => new()
        {
            Windows = ["notepad.exe", "calc.exe"]
        };
    }
}

using Cmpnnt.SdTools.BuildTasks.Utilities;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace Cmpnnt.SdTools.BuildTasks;

public class CloseStreamDeck : Task
{
    
    /// <summary>
    /// The name of the Stream Deck plugin.
    /// </summary>
    [Required]
    public string PluginName { get; set; }
    
    public override bool Execute()
    {
        // This is a `BeforeBuild` task to stop the streamdeck and the plugin instance, because
        // the output can't be copied if it's running. It can poll a few times to ensure the SD
        // app and plugin isn't running, or throw an error if it doesn't stop. The task should
        // have an output property denoting whether the SD app and plugin were closed successfully.
        // First, it should check should that the SD CLI exists, so we don't bother killing the
        // SD application if we can't link the plugin. If neither the SD app nor plugin were running,
        // set `ClosedStreamDeck` to true. If either one was running and was successfully stopped,
        // set it to true. This will also run only in debug.

        ProcessUtilities pu = new(PluginName, this);

        Log.LogMessage(MessageImportance.High, "Preparing to build Stream Deck plugin...");

        if (!pu.FindCli())
        {
            Log.LogWarning("Stream Deck CLI not found. Plugin will not be automatically linked.");
            return true; // Don't fail the build, just skip the automation
        }

        if (!pu.StopPlugin())
        {
            return false;
        }

        if (!pu.StopStreamDeck())
        {
            return false;
        }

        Log.LogMessage(MessageImportance.High, "Stream Deck closed successfully.");

        return true;
    }
}
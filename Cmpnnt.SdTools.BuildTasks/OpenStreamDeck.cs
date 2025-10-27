using System;
using Cmpnnt.SdTools.BuildTasks.Utilities;
using Task = Microsoft.Build.Utilities.Task;

namespace Cmpnnt.SdTools.BuildTasks;

public class OpenStreamDeck : Task
{
    public override bool Execute()
    {
        // This is an `AfterBuild` task to restart the Stream Deck application after the plugin
        // has been successfully built and linked/packaged. This ensures the plugin is loaded
        // and ready for testing.

        string appPath = Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => @"C:\Program Files\Elgato\StreamDeck\StreamDeck.exe",
            PlatformID.MacOSX => "/Applications/Elgato Stream Deck.app",
            _ => throw new NotSupportedException() // Linux: Wishful thinking
        };

        Log.LogMessage(Microsoft.Build.Framework.MessageImportance.High, "Starting Stream Deck...");

        ProcessUtilities pu = new("", this);
        if (pu.StartStreamDeck(appPath))
        {
            Log.LogMessage(Microsoft.Build.Framework.MessageImportance.High, "Stream Deck started successfully.");
            return true;
        }

        Log.LogWarning("Failed to start Stream Deck. You may need to start it manually.");
        return true; // Don't fail the build
    }
}
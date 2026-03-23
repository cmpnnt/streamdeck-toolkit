using System;
using System.Runtime.InteropServices;
using Cmpnnt.SdTools.BuildTasks.Utilities;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace Cmpnnt.SdTools.BuildTasks;

public class OpenStreamDeck : Task
{
    public override bool Execute()
    {
        // This is an `AfterBuild` task to restart the Stream Deck application after the plugin
        // has been successfully built and linked/packaged. This ensures the plugin is loaded
        // and ready for testing.
        string appPath = CommandLineWrapper.GetOsPlatform() == OSPlatform.Windows ? 
            @"C:\Program Files\Elgato\StreamDeck\StreamDeck.exe" :
            "/Applications/Elgato Stream Deck.app";

        Log.LogMessage(MessageImportance.High, "Starting Stream Deck...");

        ProcessUtilities pu = new("", this);
        if (pu.StartStreamDeck(appPath))
        {
            Log.LogMessage(MessageImportance.High, "Stream Deck started successfully.");
            return true;
        }

        Log.LogWarning("Failed to start Stream Deck. You may need to start it manually.");
        return true; // Don't fail the build
    }
}
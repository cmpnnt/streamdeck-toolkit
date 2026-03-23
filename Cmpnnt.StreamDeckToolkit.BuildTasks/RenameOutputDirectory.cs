using System;
using System.IO;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace Cmpnnt.StreamDeckToolkit.BuildTasks;

public class RenameOutputDirectory : Task
{
    /// <summary>
    /// The path to be renamed.
    /// </summary>
    [Required]
    public string OldPath { get; set; }
        
    /// <summary>
    /// The path to change the old path to.
    /// </summary>
    [Required]
    public string NewPath { get; set; }

    public override bool Execute()
    {
        if(Directory.Exists(NewPath))
        {
            Directory.Delete(NewPath, true);
        }

        try
        {
            Directory.Move(OldPath, NewPath);
        }
        catch (Exception e)
        {
            Log.LogErrorFromException(e);
        }
            
        return true;
    }
}
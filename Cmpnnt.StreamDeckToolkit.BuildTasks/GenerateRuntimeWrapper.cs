using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace Cmpnnt.StreamDeckToolkit.BuildTasks;

/// <summary>
/// Generates a shell script wrapper that sets DOTNET_ROOT before launching the plugin binary.
/// This is needed when .NET is installed at a non-standard path (e.g. via mise) because
/// macOS GUI apps like Stream Deck do not inherit shell environment variables.
/// </summary>
public class GenerateRuntimeWrapper : Task
{
    [Required]
    public string BuildDir { get; set; }

    [Required]
    public string AssemblyName { get; set; }

    [Required]
    public string ManifestPath { get; set; }

    private static readonly string[] StandardDotnetPaths =
    [
        "/usr/local/share/dotnet",
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".dotnet")
    ];

    public override bool Execute()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Log.LogMessage(MessageImportance.Low, "Skipping runtime wrapper generation (not macOS).");
            return true;
        }

        string dotnetRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
        if (string.IsNullOrEmpty(dotnetRoot))
        {
            Log.LogMessage(MessageImportance.Normal, "DOTNET_ROOT is not set; no wrapper needed.");
            return true;
        }

        string normalizedRoot = Path.GetFullPath(dotnetRoot).TrimEnd('/');
        foreach (string standardPath in StandardDotnetPaths)
        {
            if (normalizedRoot.Equals(Path.GetFullPath(standardPath).TrimEnd('/'), StringComparison.Ordinal))
            {
                Log.LogMessage(MessageImportance.Normal,
                    $"DOTNET_ROOT points to standard location ({dotnetRoot}); no wrapper needed.");
                return true;
            }
        }

        Log.LogMessage(MessageImportance.High,
            $"DOTNET_ROOT is set to non-standard path: {dotnetRoot}");
        Log.LogMessage(MessageImportance.High,
            "Generating wrapper script so Stream Deck can find the .NET runtime.");

        string wrapperPath = Path.Combine(BuildDir, "run");
        string scriptContent =
            $"#!/bin/bash\n" +
            $"export DOTNET_ROOT=\"{dotnetRoot}\"\n" +
            $"exec \"$(dirname \"$0\")/{AssemblyName}\" \"$@\"\n";

        File.WriteAllText(wrapperPath, scriptContent);

        File.SetUnixFileMode(wrapperPath,
            UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
            UnixFileMode.GroupRead | UnixFileMode.GroupExecute |
            UnixFileMode.OtherRead | UnixFileMode.OtherExecute);

        Log.LogMessage(MessageImportance.Normal, $"Generated wrapper script: {wrapperPath}");

        if (!PatchManifestCodePath())
        {
            return true; // non-fatal — wrapper exists but manifest wasn't patched
        }

        Log.LogMessage(MessageImportance.High, "Updated manifest.json CodePath to 'run'.");
        return true;
    }

    private bool PatchManifestCodePath()
    {
        if (!File.Exists(ManifestPath))
        {
            Log.LogWarning($"Manifest not found at {ManifestPath}; cannot update CodePath.");
            return false;
        }

        try
        {
            string json = File.ReadAllText(ManifestPath);
            JsonNode manifest = JsonNode.Parse(json);
            if (manifest == null)
            {
                Log.LogWarning("Failed to parse manifest.json.");
                return false;
            }

            manifest["CodePath"] = "run";

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(ManifestPath, manifest.ToJsonString(options));
            return true;
        }
        catch (Exception ex)
        {
            Log.LogWarning($"Failed to patch manifest.json: {ex.Message}");
            return false;
        }
    }
}
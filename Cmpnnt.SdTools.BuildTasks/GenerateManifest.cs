using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace Cmpnnt.SdTools.BuildTasks;

public class GenerateManifest : Task
{
    /// <summary>
    /// Path to the compiled assembly of the main plugin project
    /// </summary>
    [Required]
    public string PluginAssemblyPath { get; set; }
    
    /// <summary>
    /// Output path for manifest.json
    /// </summary>
    [Required]
    public string OutputManifestPath { get; set; }
    
    /// <summary>
    /// Optional input: Namespace of the generated provider
    /// </summary>
    public string GeneratedProviderNamespace { get; set; } = "GeneratedManifest";
    
    /// <summary>
    /// Optional input: Class name of the generated provider
    /// </summary>
    public string GeneratedProviderClass { get; set; } = "ManifestProvider";
    
    /// <summary>
    /// Optional input: Method name to get the data object
    /// </summary>
    public string GeneratedProviderMethod { get; set; } = "GetManifestData";

    public override bool Execute()
    {
        if (string.IsNullOrEmpty(PluginAssemblyPath) || string.IsNullOrEmpty(OutputManifestPath))
        {
            Log.LogError("PluginAssemblyPath and OutputManifestPath properties are required.");
            return false;
        }

        if (!File.Exists(PluginAssemblyPath))
        {
            Log.LogWarning(
                $"Plugin assembly not found at '{PluginAssemblyPath}'. Skipping manifest generation. This might happen on initial builds before the assembly exists.");
            return true;
        }

        var alc = new AssemblyLoadContext(name: "ManifestGen", isCollectible: true);
        try
        {
            // The assembly and its dependencies must be loaded into the same context.
            // In this case, we assume that any dependencies for the plugin assembly are in the same directory.
            alc.Resolving += (context, assemblyName) =>
            {
                string assemblyPath = Path.Combine(Path.GetDirectoryName(PluginAssemblyPath) ?? string.Empty, $"{assemblyName.Name}.dll");
                
                return File.Exists(assemblyPath) ? context.LoadFromAssemblyPath(assemblyPath) : null;
            };

            Log.LogMessage(MessageImportance.Normal, $"Loading assembly: {PluginAssemblyPath}");
            /* Using LoadFromAssemblyPath here means we need to manually resolve dependencies,
             which is what's happening in the alc.Resolving block */
            Assembly assembly = alc.LoadFromAssemblyPath(PluginAssemblyPath);

            var providerFullName = $"{GeneratedProviderNamespace}.{GeneratedProviderClass}";
            Log.LogMessage(MessageImportance.Normal, $"Looking for type: {providerFullName}");
            Type providerType = assembly.GetType(providerFullName);

            if (providerType == null)
            {
                Log.LogError(
                    $"Could not find generated type '{providerFullName}' in assembly '{PluginAssemblyPath}'. Ensure the source generator ran successfully.");
                return false;
            }

            Log.LogMessage(MessageImportance.Normal, $"Looking for method: {GeneratedProviderMethod}");
            MethodInfo getDataMethod = providerType.GetMethod(GeneratedProviderMethod,
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (getDataMethod == null)
            {
                Log.LogError(
                    $"Could not find static method '{GeneratedProviderMethod}' on type '{providerFullName}'.");
                return false;
            }

            object manifestData = getDataMethod.Invoke(null, null);

            if (manifestData == null)
            {
                Log.LogError($"'{GeneratedProviderMethod}' returned null.");
                return false;
            }

            Log.LogMessage(MessageImportance.Normal, "Serializing manifest data to JSON...");
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                // Ensure encoder allows characters often found in paths/names if necessary
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonContent = JsonSerializer.Serialize(manifestData, options);

            // Ensure directory exists
            string outputDir = Path.GetDirectoryName(OutputManifestPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Log.LogMessage(MessageImportance.Normal, $"Creating directory: {outputDir}");
                Directory.CreateDirectory(outputDir);
            }

            Log.LogMessage(MessageImportance.High, $"Writing manifest file: {OutputManifestPath}");
            File.WriteAllText(OutputManifestPath, jsonContent);

            return true;
        }
        catch (Exception ex)
        {
            Log.LogErrorFromException(ex, true, true, null);
            return false;
        }
        finally
        {
            alc.Unload();
        }
    }
}

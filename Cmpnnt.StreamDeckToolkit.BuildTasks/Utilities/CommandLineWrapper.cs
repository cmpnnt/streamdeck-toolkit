using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cmpnnt.StreamDeckToolkit.BuildTasks.Utilities;

public static class CommandLineWrapper
{
    
    private const string CMD = "cmd.exe";
    private const string BASH = "/bin/zsh";
    
    ///  <summary>
    ///  Wraps the system command line and executes the command passed to it.
    ///  </summary>
    ///  <param name="command">The CLI command to be passed to command line</param>
    ///  <param name="args">The options for the command to run</param>
    ///  <param name="commandLine">Whether the command will run on the command line (true) or be invoked directly (false)</param>
    ///  <param name="ct">A cancellation token.</param>
    ///  <returns>A <c>Task&lt;(bool, string)&gt;</c> containing command output.</returns>
    public static async Task<(bool, string)> Execute(string command, string args, bool commandLine = false, CancellationToken ct = default)
    {
        
        string shell = GetShell();

         string arguments = commandLine switch
         {
             true => shell == CMD ? $"/C {command} {args}" : $"-c \"{command} {args}\"",
             false => string.IsNullOrEmpty(args) ? command : $"{command} {args}"
         };

        using var proc = new Process();
        proc.StartInfo.FileName = shell;
        proc.StartInfo.Arguments = arguments;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.RedirectStandardOutput = true;

        var output = new StringBuilder();
        var error = new StringBuilder();
        var exitCode = 0;

        proc.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
               output.AppendLine(e.Data.Trim());
            }
        };

        proc.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                error.AppendLine(e.Data.Trim());
            }
        };

        proc.Start();

        // Asynchronously read the standard output of the spawned process.
        // This raises OutputDataReceived events for each line of output.
        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();

        try
        {
            await proc.WaitForExitAsync(ct);
        }
        catch (OperationCanceledException)
        {
            // Process always throws this exception when the token is canceled
            // Assume if ct was passed in and the operation was canceled, it was via `ExecuteTimed()` below (therefore successful)
            error.Append("Command was canceled.");
        }
        catch (Exception)
        {
            error.Append("Command failed with an unexpected error.");
        }
        finally
        {
            exitCode = proc.ExitCode;
            proc.Close();
        }
        
        bool hasError = exitCode != 0 || error.Length > 0;
        if (hasError)
        {
            error.Append(output);
        }
        
        return !hasError ? (true, output.ToString()) : (false, error.ToString());
    }
    
    public static async Task<(bool, string)> Run(string command, string args = "", CancellationToken ct = default)
    {
        return await Execute(command, args, commandLine: false, ct: ct);
    }

    public static OSPlatform GetOsPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return OSPlatform.Windows;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return OSPlatform.OSX;
        throw new NotSupportedException("Unsupported operating system.");
    }
    
    private static string GetShell()
    {
        OSPlatform platform = GetOsPlatform();
        return platform == OSPlatform.Windows ? CMD : BASH;
    }
}
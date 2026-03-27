using Cmpnnt.StreamDeckToolkit.Attributes;
using Cmpnnt.StreamDeckToolkit.Runtime;
using Cmpnnt.StreamDeckToolkit.SourceGenerators;

[assembly: StreamDeckPlugin(
    Name = "SDTools Sample Plugin",
    Category = "SDTools Sample Plugin",
    CategoryIcon = "Images/categoryIcon",
    Icon = "Images/pluginIcon",
    SDKVersion = 2,
    SoftwareMinVersion = SoftwareMinVersion.V6_4,
    MacMinVersion = "12"
)]

namespace Cmpnnt.StreamDeckToolkit.SamplePlugin
{
    class Program
    {
        static void Main(string[] args)
        {
            // Uncomment this line of code to allow for debugging
            //while (!System.Diagnostics.Debugger.IsAttached) { System.Threading.Thread.Sleep(100); }
            Toolkit.Run(args, new PluginActionIdRegistry());
        }
    }
}

/*
 * TODO:
 * - Implement the file-writing for one of the sample actions.
 * - Demonstrate global settings. Maybe set a value and then dig that value out and prepend it to whatever
 *     is being written to the text file by the action.
 * - Test all of the possible manifest generation options on both platforms
 * - Update CommandLineWrapper.GetOsPlatform() methods that use it. GetOsPlatform needs to support Linux
 *     for the CI pipeline but the callers would then need to throw each time the platform isn't Mac or Windows
*/

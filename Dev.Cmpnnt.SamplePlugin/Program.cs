using Cmpnnt.StreamDeckToolkit.Attributes;
using Cmpnnt.StreamDeckToolkit.Runtime;
using Cmpnnt.StreamDeckToolkit.SourceGenerators;

[assembly: StreamDeckPlugin(
    Name = "SDTools Sample Plugin",
    UUID = "com.cmpnnt.streamdecktoolkit.sampleplugin",
    Category = "SDTools Sample Plugin",
    CategoryIcon = "Images/categoryIcon",
    Icon = "Images/pluginIcon",
    SDKVersion = 2,
    SoftwareMinVersion = SoftwareMinVersion.V6_4,
    WindowsMinVersion = "10"
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

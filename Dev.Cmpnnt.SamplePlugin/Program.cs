using Cmpnnt.SdTools.Backend;
using Cmpnnt.SdTools.SourceGenerators;

namespace Cmpnnt.SdTools.SamplePlugin
{
    class Program
    {
        static void Main(string[] args)
        {
            // Uncomment this line of code to allow for debugging
            //while (!System.Diagnostics.Debugger.IsAttached) { System.Threading.Thread.Sleep(100); }
            SdWrapper.Run(args, new PluginActionIdRegistry());
        }
    }
}

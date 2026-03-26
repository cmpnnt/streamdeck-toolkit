using System;
using System.Threading.Tasks;
using Cmpnnt.StreamDeckToolkit.Communication.Registration;
using Cmpnnt.StreamDeckToolkit.Utilities;

namespace Cmpnnt.StreamDeckToolkit.Runtime
{
    /// <summary>
    /// Main entry point for the Stream Deck plugin toolkit.
    /// Call <see cref="Run"/> from your plugin's <c>Main</c> method to connect to the Stream Deck app.
    /// </summary>
    public static class Toolkit
    {
        // Handles all the communication with the plugin
        private static PluginContainer _container;

        /// /************************************************************************
        /// Based on Barraider's C# StreamDeck tools, which in turn was based on:
        ///   * Initial configuration from TyrenDe's streamdeck-client-csharp example:
        ///     - https://github.com/TyrenDe/streamdeck-client-csharp
        ///   *  and SaviorXTanren's MixItUp.StreamDeckPlugin:
        ///     - https://github.com/SaviorXTanren/mixer-mixitup/
        /// *************************************************************************/
        /// <summary>
        /// Connects the plugin to the Stream Deck app and begins processing events.
        /// Call this from your plugin's <c>Main</c> method, passing the command-line arguments
        /// supplied by Stream Deck at launch.
        /// </summary>
        /// <param name="args">Command-line arguments passed by the Stream Deck app.</param>
        /// <param name="registry">Registry that maps action UUIDs to action instances.</param>
        /// <param name="updateHandler">Optional handler for plugin update notifications.</param>
        public static void Run(string[] args, IPluginActionRegistry registry, IUpdateHandler updateHandler = null)
        {
            Logger.Instance.LogMessage(TracingLevel.Info,
                $"Plugin [{Tools.GetExeName()}] Loading - {registry.PluginActionIDs().Count} Actions Found");
            
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            #if DEBUG
            Logger.Instance.LogMessage(TracingLevel.Debug, $"Plugin Loading - Args: {string.Join(" ", args)}");
            #endif

            StreamDeckOptions options = StreamDeckOptions.Parse(args);
            async void RunAsync()
            {
                try
                {
                    await RunPlugin(options, registry, updateHandler);
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogMessage(TracingLevel.Fatal,
                        $"Plugin crashed with the following message: {ex.Message}");
                }
            }
            RunAsync();
        }

        private static async Task RunPlugin(StreamDeckOptions options, IPluginActionRegistry registry, IUpdateHandler updateHandler)
        {
            _container = new PluginContainer(registry, updateHandler);
            await _container.Run(options);
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Instance.LogMessage(TracingLevel.Fatal, $"Unhandled Exception: {e.ExceptionObject}");
        }
    }
}
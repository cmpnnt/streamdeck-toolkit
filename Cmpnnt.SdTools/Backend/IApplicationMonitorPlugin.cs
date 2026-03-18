using Cmpnnt.SdTools.Communication.Payloads;

namespace Cmpnnt.SdTools.Backend
{
    /// <summary>
    /// Optional interface for plugin actions that need to monitor application launch/terminate events.
    /// Implement this on your plugin action class to receive these global events.
    /// </summary>
    public interface IApplicationMonitorPlugin
    {
        /// <summary>
        /// Called when a monitored application is launched.
        /// </summary>
        /// <param name="payload">Contains the name of the application that was launched.</param>
        void OnApplicationDidLaunch(ApplicationPayload payload);

        /// <summary>
        /// Called when a monitored application is terminated.
        /// </summary>
        /// <param name="payload">Contains the name of the application that was terminated.</param>
        void OnApplicationDidTerminate(ApplicationPayload payload);
    }
}

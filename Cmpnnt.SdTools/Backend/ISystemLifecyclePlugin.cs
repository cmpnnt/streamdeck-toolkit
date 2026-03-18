namespace Cmpnnt.SdTools.Backend
{
    /// <summary>
    /// Optional interface for plugin actions that need to respond to system wake events.
    /// Implement this on your plugin action class to receive these global events.
    /// </summary>
    public interface ISystemLifecyclePlugin
    {
        /// <summary>
        /// Called when the computer wakes up from sleep.
        /// </summary>
        void OnSystemDidWakeUp();
    }
}

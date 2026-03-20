using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cmpnnt.SdTools.Communication;
using Cmpnnt.SdTools.Communication.Events;
using Cmpnnt.SdTools.Communication.Events.Dtos;
using Cmpnnt.SdTools.Communication.Payloads;
using Cmpnnt.SdTools.Communication.Registration;
using Cmpnnt.SdTools.Utilities;

namespace Cmpnnt.SdTools.Backend
{
    internal class PluginContainer(IPluginActionRegistry actionRegistry, IUpdateHandler updateHandler)
    {
        private const int STREAMDECK_INITIAL_CONNECTION_TIMEOUT_SECONDS = 60;
        private StreamDeckConnection connection;
        private readonly ManualResetEvent connectEvent = new(false);
        private readonly ManualResetEvent disconnectEvent = new(false);
        private readonly SemaphoreSlim instancesLock = new(1);
        private string pluginUuid;
        private RegistrationInfo deviceInfo;
        private PluginUpdateInfo lastUpdateInfo;

        /// <summary>
        /// All current instances of plugin actions. Keyed on Action UUID. Action instances are added
        /// when the StreamDeck page containing the plugin is shown and removed when it is not visible.
        /// </summary>
        private static readonly Dictionary<string, ICommonPluginFunctions> Instances = new();

        public async Task Run(StreamDeckOptions options)
        {
            pluginUuid = options.PluginUuid;
            deviceInfo = options.DeviceInfo;
            connection = new StreamDeckConnection(options.Port, options.PluginUuid, options.RegisterEvent);

            connection.OnConnected += Connection_OnConnected;
            connection.OnDisconnected += Connection_OnDisconnected;
            connection.OnEventReceived += Connection_OnEventReceived;

            if (updateHandler != null)
            {
                updateHandler.OnUpdateStatusChanged += UpdateHandler_OnUpdateStatusChanged;
                updateHandler.SetPluginConfiguration(Tools.GetExeName(), deviceInfo.Plugin.Version);
                await Task.Run(updateHandler.CheckForUpdate);
            }

            connection.Run();
            Logger.Instance.LogMessage(TracingLevel.Debug, $"Plugin Loaded: UUID: {pluginUuid} Device Info: {deviceInfo}");
            Logger.Instance.LogMessage(TracingLevel.Info, $"Plugin version: {deviceInfo.Plugin.Version}");
            Logger.Instance.LogMessage(TracingLevel.Info, "Connecting to Stream Deck...");

            if (connectEvent.WaitOne(TimeSpan.FromSeconds(STREAMDECK_INITIAL_CONNECTION_TIMEOUT_SECONDS)))
            {
                Logger.Instance.LogMessage(TracingLevel.Info, "Connected to Stream Deck");

                GlobalSettingsManager.Instance.Initialize(connection);

                while (!disconnectEvent.WaitOne(TimeSpan.FromMilliseconds(1000)))
                {
                    await RunTick();
                }
            }

            Logger.Instance.LogMessage(TracingLevel.Info, "Plugin Disconnected - Exiting");
            await connection.DisposeAsync();
            Environment.Exit(0);
        }

        private async void Connection_OnEventReceived(object sender, BaseEvent evt)
        {
            try
            {
                switch (evt)
                {
                    case WillAppearEvent e:
                    {
                        var conn = new SdConnection(connection, pluginUuid, deviceInfo, e.Action, e.Context, e.Device);
                        await instancesLock.WaitAsync();
                        try
                        {
                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug,
                                $"Plugin OnWillAppear: Context: {e.Context} Action: {e.Action} Payload: {e.Payload?.ToStringEx()}");
                            #endif

                            if (!actionRegistry.PluginActionIDs().Contains(e.Action))
                            {
                                Logger.Instance.LogMessage(TracingLevel.Warn, $"No plugin found that matches action: {e.Action}");
                                return;
                            }

                            if (Instances.TryGetValue(e.Context, out ICommonPluginFunctions existing) && existing != null)
                            {
                                Logger.Instance.LogMessage(TracingLevel.Info,
                                    $"WillAppear called for already existing context {e.Context} (might be inside a multi-action)");
                                return;
                            }

                            var payload = new InitialPayload(e.Payload, deviceInfo);
                            Instances[e.Context] = actionRegistry.CreateAction(e.Action, conn, payload);

                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug, $"Instance count is now {Instances.Count}");
                            #endif
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.LogMessage(TracingLevel.Fatal,
                                $"Could not create instance of {e.Action} with context {e.Context} - This may be due to an Exception raised in the constructor, or the class does not inherit PluginBase with the same constructor {ex}");
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case WillDisappearEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug,
                                $"Plugin OnWillDisappear: Context: {e.Context} Action: {e.Action}");
                            #endif

                            if (!Instances.TryGetValue(e.Context, out ICommonPluginFunctions value))
                            {
                                return;
                            }
                            value.Destroy();
                            Instances.Remove(e.Context);

                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug, $"Instance count is now {Instances.Count}");
                            #endif
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case KeyDownEvent e:
                    {
                        if (updateHandler?.IsBlockingUpdate ?? false)
                        {
                            if (!string.IsNullOrEmpty(lastUpdateInfo?.UpdateUrl))
                            {
                                await connection.OpenUrlAsync(lastUpdateInfo.UpdateUrl);
                            }
                            return;
                        }
                        await instancesLock.WaitAsync();
                        try
                        {
                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug,
                                $"Plugin Keydown: Context: {e.Context} Action: {e.Action} Payload: {e.Payload?.ToStringEx()}");
                            #endif

                            if (!Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                            {
                                return;
                            }
                            if (instance is IKeypadPlugin plugin)
                            {
                                plugin.KeyPressed(e.Payload);
                            }
                            else
                            {
                                Logger.Instance.LogMessage(TracingLevel.Error,
                                    $"Keydown General Error: Could not convert {e.Context} to IKeypadPlugin");
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case KeyUpEvent e:
                    {
                        if (updateHandler?.IsBlockingUpdate ?? false)
                        {
                            return;
                        }
                        await instancesLock.WaitAsync();
                        try
                        {
                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug,
                                $"Plugin Keyup: Context: {e.Context} Action: {e.Action} Payload: {e.Payload?.ToStringEx()}");
                            #endif

                            if (!Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                            {
                                return;
                            }
                            if (instance is IKeypadPlugin plugin)
                            {
                                plugin.KeyReleased(e.Payload);
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case DialRotateEvent e:
                    {
                        if (updateHandler?.IsBlockingUpdate ?? false) return;
                        await instancesLock.WaitAsync();
                        try
                        {
                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug,
                                $"DialRotate: Context: {e.Context} Action: {e.Action} Payload: {e.Payload?.ToStringEx()}");
                            #endif

                            if (!Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                            {
                                return;
                            }
                            if (instance is IEncoderPlugin plugin)
                            {
                                plugin.DialRotate(e.Payload);
                            }
                            else
                            {
                                Logger.Instance.LogMessage(TracingLevel.Error,
                                    $"DialRotate General Error: Could not convert {e.Context} to IEncoderPlugin");
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case DialDownEvent e:
                    {
                        if (updateHandler?.IsBlockingUpdate ?? false)
                        {
                            if (!string.IsNullOrEmpty(lastUpdateInfo?.UpdateUrl))
                            {
                                await connection.OpenUrlAsync(lastUpdateInfo.UpdateUrl);
                            }
                            return;
                        }
                        await instancesLock.WaitAsync();
                        try
                        {
                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug,
                                $"DialPress: Context: {e.Context} Action: {e.Action} Payload: {e.Payload?.ToStringEx()}");
                            #endif

                            if (!Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                            {
                                return;
                            }
                            if (instance is IEncoderPlugin plugin)
                            {
                                plugin.DialDown(e.Payload);
                            }
                            else
                            {
                                Logger.Instance.LogMessage(TracingLevel.Error,
                                    $"DialDown General Error: Could not convert {e.Context} to IEncoderPlugin");
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case DialUpEvent e:
                    {
                        if (updateHandler?.IsBlockingUpdate ?? false) return;
                        await instancesLock.WaitAsync();
                        try
                        {
                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug,
                                $"DialPress: Context: {e.Context} Action: {e.Action} Payload: {e.Payload?.ToStringEx()}");
                            #endif

                            if (!Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                            {
                                return;
                            }
                            if (instance is IEncoderPlugin plugin)
                            {
                                plugin.DialUp(e.Payload);
                            }
                            else
                            {
                                Logger.Instance.LogMessage(TracingLevel.Error,
                                    $"DialDown General Error: Could not convert {e.Context} to IEncoderPlugin");
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case TouchTapEvent e:
                    {
                        if (updateHandler?.IsBlockingUpdate ?? false)
                        {
                            if (!string.IsNullOrEmpty(lastUpdateInfo?.UpdateUrl))
                            {
                                await connection.OpenUrlAsync(lastUpdateInfo.UpdateUrl);
                            }
                            return;
                        }
                        await instancesLock.WaitAsync();
                        try
                        {
                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug,
                                $"TouchpadPress: Context: {e.Context} Action: {e.Action} Payload: {e.Payload?.ToStringEx()}");
                            #endif

                            if (!Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                            {
                                return;
                            }
                            if (instance is IEncoderPlugin plugin)
                            {
                                plugin.TouchPress(e.Payload);
                            }
                            else
                            {
                                Logger.Instance.LogMessage(TracingLevel.Error,
                                    $"TouchpadPress General Error: Could not convert {e.Context} to IEncoderPlugin");
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case DidReceiveSettingsEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug,
                                $"Plugin OnDidReceiveSettings: Context: {e.Context} Action: {e.Action} Payload: {e.Payload?.ToStringEx()}");
                            #endif

                            if (Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                                instance.ReceivedSettings(e.Payload ?? new ReceivedSettingsPayload());
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case DidReceiveGlobalSettingsEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            #if DEBUG
                            Logger.Instance.LogMessage(TracingLevel.Debug,
                                $"Plugin OnDidReceiveGlobalSettings: Settings: {e.Payload?.ToStringEx()}");
                            #endif

                            ReceivedGlobalSettingsPayload globalSettings = e.Payload ?? new ReceivedGlobalSettingsPayload();
                            foreach (string key in Instances.Keys)
                            {
                                Instances[key].ReceivedGlobalSettings(globalSettings);
                            }

                            updateHandler?.SetGlobalSettings(globalSettings);
                            GlobalSettingsManager.Instance.OnGlobalSettingsReceived(globalSettings);
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case SendToPluginEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            if (Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                            {
                                instance.OnSendToPlugin(e.Payload);
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case TitleParametersDidChangeEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            if (Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                            {
                                instance.OnTitleParametersDidChange(e.Payload);
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case PropertyInspectorDidAppearEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            if (Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                            {
                                instance.OnPropertyInspectorDidAppear();
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case PropertyInspectorDidDisappearEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            if (Instances.TryGetValue(e.Context, out ICommonPluginFunctions instance))
                            {
                                instance.OnPropertyInspectorDidDisappear();
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    // Global events — broadcast to all instances that opt in via optional interfaces
                    case ApplicationDidLaunchEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            foreach (ICommonPluginFunctions instance in Instances.Values)
                            {
                                if (instance is IApplicationMonitorPlugin monitor)
                                {
                                    monitor.OnApplicationDidLaunch(e.Payload);
                                }
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case ApplicationDidTerminateEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            foreach (ICommonPluginFunctions instance in Instances.Values)
                            {
                                if (instance is IApplicationMonitorPlugin monitor)
                                {
                                    monitor.OnApplicationDidTerminate(e.Payload);
                                }
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case DeviceDidConnectEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            foreach (ICommonPluginFunctions instance in Instances.Values)
                            {
                                if (instance is IDeviceMonitorPlugin monitor)
                                {
                                    monitor.OnDeviceDidConnect(e);
                                }
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case DeviceDidDisconnectEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            foreach (ICommonPluginFunctions instance in Instances.Values)
                            {
                                if (instance is IDeviceMonitorPlugin monitor)
                                {
                                    monitor.OnDeviceDidDisconnect(e.Device);
                                }
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case SystemDidWakeUpEvent:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            foreach (ICommonPluginFunctions instance in Instances.Values)
                            {
                                if (instance is ISystemLifecyclePlugin lifecycle)
                                {
                                    lifecycle.OnSystemDidWakeUp();
                                }
                            }
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    default:
                        Logger.Instance.LogMessage(TracingLevel.Warn,
                            $"{GetType()} Unhandled event type: {evt.GetType().Name}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Fatal,
                    $"Plugin crashed with the following message: {ex.Message}");
            }
        }

        private async Task RunTick()
        {
            if (updateHandler?.IsBlockingUpdate ?? false) return;

            await instancesLock.WaitAsync();
            try
            {
                foreach (KeyValuePair<string, ICommonPluginFunctions> kvp in Instances.ToArray())
                {
                    kvp.Value.OnTick();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Fatal,
                    $"Plugin crashed with the following message: {ex.Message}");
            }
            finally
            {
                instancesLock.Release();
            }
        }

        private async void UpdateHandler_OnUpdateStatusChanged(object sender, PluginUpdateInfo e)
        {
            try
            {
                lastUpdateInfo = e;
                if (!string.IsNullOrEmpty(e.UpdateUrl))
                {
                    await connection.OpenUrlAsync(e.UpdateUrl);
                }

                if (!string.IsNullOrEmpty(e.UpdateImage))
                {
                    await Task.Run(async () =>
                    {
                        foreach (string contextId in Instances.Keys.ToList())
                        {
                            await connection.SetImageAsync(e.UpdateImage, contextId, SdkTarget.HardwareAndSoftware, null);
                            await connection.SetTitleAsync(null, contextId, SdkTarget.HardwareAndSoftware, null);
                        }
                    });
                }

                if (e.Status != PluginUpdateStatus.CriticalUpgrade) return;
                Logger.Instance.LogMessage(TracingLevel.Fatal, $"Critical update needed");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Fatal,
                    $"Plugin crashed with the following message: {ex.Message}");
            }
        }

        private void Connection_OnConnected(object sender, EventArgs e)
        {
            try
            {
                connectEvent.Set();
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Fatal,
                    $"Plugin crashed with the following message: {ex.Message}");
            }
        }

        private void Connection_OnDisconnected(object sender, EventArgs e)
        {
            Logger.Instance.LogMessage(TracingLevel.Info, "Disconnect event received");
            disconnectEvent.Set();
        }
    }
}

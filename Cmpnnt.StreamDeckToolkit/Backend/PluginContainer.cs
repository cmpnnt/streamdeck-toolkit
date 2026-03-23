using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cmpnnt.StreamDeckToolkit.Communication;
using Cmpnnt.StreamDeckToolkit.Communication.Events;
using Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos;
using Cmpnnt.StreamDeckToolkit.Communication.Payloads;
using Cmpnnt.StreamDeckToolkit.Communication.Registration;
using Cmpnnt.StreamDeckToolkit.Utilities;

namespace Cmpnnt.StreamDeckToolkit.Backend
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
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.LogMessage(TracingLevel.Fatal,
                                $"Could not create instance of {e.Action} with context {e.Context} - Check your constructor {ex}");
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case WillDisappearEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
                            if (!Instances.TryGetValue(e.Context, out ICommonPluginFunctions value)) return;
                            
                            value.Destroy();
                            Instances.Remove(e.Context);
                        }
                        finally { instancesLock.Release(); }
                        break;
                    }

                    case KeyDownEvent e:
                    {
                        if (await IsUpdateBlockingAsync(openUrl: true)) return;
                        await OnInstanceAsync<IKeypadPlugin>(e.Context, p => p.KeyPressed(e.Payload), "Keydown General");
                        break;
                    }

                    case KeyUpEvent e:
                    {
                        if (await IsUpdateBlockingAsync()) return;
                        await OnInstanceAsync<IKeypadPlugin>(e.Context, p => p.KeyReleased(e.Payload));
                        break;
                    }

                    case DialRotateEvent e:
                    {
                        if (await IsUpdateBlockingAsync())  return;
                        await OnInstanceAsync<IEncoderPlugin>(e.Context, p => p.DialRotate(e.Payload), "DialRotate General");
                        break;
                    }

                    case DialDownEvent e:
                    {
                        if (await IsUpdateBlockingAsync(openUrl: true)) return;
                        await OnInstanceAsync<IEncoderPlugin>(e.Context, p => p.DialDown(e.Payload), "DialDown General");
                        break;
                    }

                    case DialUpEvent e:
                    {
                        if (await IsUpdateBlockingAsync()) return;
                        await OnInstanceAsync<IEncoderPlugin>(e.Context, p => p.DialUp(e.Payload), "DialUp General");
                        break;
                    }

                    case TouchTapEvent e:
                    {
                        if (await IsUpdateBlockingAsync(openUrl: true)) return;
                        await OnInstanceAsync<IEncoderPlugin>(e.Context, p => p.TouchPress(e.Payload), "TouchpadPress General");
                        break;
                    }

                    case DidReceiveSettingsEvent e:
                    {
                        await OnInstanceAsync<ICommonPluginFunctions>(e.Context, i => i.ReceivedSettings(e.Payload ?? new ReceivedSettingsPayload()));
                        break;
                    }

                    case DidReceiveGlobalSettingsEvent e:
                    {
                        await instancesLock.WaitAsync();
                        try
                        {
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
                        await OnInstanceAsync<ICommonPluginFunctions>(e.Context, i => i.OnSendToPlugin(e.Payload));
                        break;
                    }

                    case TitleParametersDidChangeEvent e:
                    {
                        await OnInstanceAsync<ICommonPluginFunctions>(e.Context, i => i.OnTitleParametersDidChange(e.Payload));
                        break;
                    }

                    case PropertyInspectorDidAppearEvent e:
                    {
                        await OnInstanceAsync<ICommonPluginFunctions>(e.Context, i => i.OnPropertyInspectorDidAppear());
                        break;
                    }

                    case PropertyInspectorDidDisappearEvent e:
                    {
                        await OnInstanceAsync<ICommonPluginFunctions>(e.Context, i => i.OnPropertyInspectorDidDisappear());
                        break;
                    }

                    // Global events — broadcast to all instances that opt in via optional interfaces
                    case ApplicationDidLaunchEvent e:
                    {
                        await BroadcastAsync<IApplicationMonitorPlugin>(m => m.OnApplicationDidLaunch(e.Payload));
                        break;
                    }

                    case ApplicationDidTerminateEvent e:
                    {
                        await BroadcastAsync<IApplicationMonitorPlugin>(m => m.OnApplicationDidTerminate(e.Payload));
                        break;
                    }

                    case DeviceDidConnectEvent e:
                    {
                        await BroadcastAsync<IDeviceMonitorPlugin>(m => m.OnDeviceDidConnect(e));
                        break;
                    }

                    case DeviceDidDisconnectEvent e:
                    {
                        await BroadcastAsync<IDeviceMonitorPlugin>(m => m.OnDeviceDidDisconnect(e.Device));
                        break;
                    }

                    case SystemDidWakeUpEvent:
                    {
                        await BroadcastAsync<ISystemLifecyclePlugin>(l => l.OnSystemDidWakeUp());
                        break;
                    }

                    default:
                        Logger.Instance.LogMessage(TracingLevel.Warn, $"{GetType()} Unhandled event type: {evt.GetType().Name}");
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
            if (updateHandler?.IsBlockingUpdate ?? false)
            {
                return;
            }

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

        /// <summary>
        /// Returns true and optionally opens the update URL if an update is blocking input.
        /// </summary>
        private async Task<bool> IsUpdateBlockingAsync(bool openUrl = false)
        {
            if (!(updateHandler?.IsBlockingUpdate ?? false))
            {
                return false;
            }
            if (openUrl && !string.IsNullOrEmpty(lastUpdateInfo?.UpdateUrl))
            {
                await connection.OpenUrlAsync(lastUpdateInfo.UpdateUrl);
            }
            return true;
        }

        /// <summary>
        /// Acquires the instance lock, looks up the instance for <paramref name="context"/>, casts it
        /// to <typeparamref name="TPlugin"/>, and invokes <paramref name="action"/>. If the cast fails
        /// and <paramref name="errorName"/> is supplied, logs an error. Silently returns if the context
        /// is not found or if no <paramref name="errorName"/> is given and the cast fails.
        /// </summary>
        private async Task OnInstanceAsync<TPlugin>(
            string context,
            Action<TPlugin> action,
            string? errorName = null) where TPlugin : class
        {
            await instancesLock.WaitAsync();
            try
            {
                if (Instances.TryGetValue(context, out ICommonPluginFunctions instance))
                {
                    if (instance is TPlugin typed)
                    {
                        action(typed);
                    }
                    else if (errorName != null)
                    {
                        Logger.Instance.LogMessage(TracingLevel.Error,
                            $"{errorName} Error: Could not convert {context}");
                    }
                }
            }
            finally { instancesLock.Release(); }
        }

        /// <summary>
        /// Acquires the instance lock and invokes <paramref name="action"/> on every instance that
        /// implements <typeparamref name="TPlugin"/>. Instances that do not implement the interface
        /// are silently skipped.
        /// </summary>
        private async Task BroadcastAsync<TPlugin>(Action<TPlugin> action) where TPlugin : class
        {
            await instancesLock.WaitAsync();
            try
            {
                foreach (ICommonPluginFunctions instance in Instances.Values)
                {
                    if (instance is TPlugin typed)
                    {
                        action(typed);
                    }
                }
            }
            finally { instancesLock.Release(); }
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

                if (e.Status != PluginUpdateStatus.CriticalUpgrade)
                {
                    return;
                }
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

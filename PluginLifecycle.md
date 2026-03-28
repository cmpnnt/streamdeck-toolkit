# Plugin Execution Flow and Lifecycle

## Startup Sequence

When the Stream Deck software launches the plugin, events arrive in this order:

1. **Plugin process starts** — `PluginContainer.Run()` is called, the WebSocket connection to Stream Deck is established.
2. **`DeviceDidConnectEvent`** — fired immediately for each connected hardware device. At this point, the `Instances` dictionary in `PluginContainer` is **empty** — no action instances have been created yet. Any `BroadcastAsync<IDeviceMonitorPlugin>` call here reaches nothing.
3. **`WillAppearEvent`** — fired for each action visible on the current Stream Deck page. This is when `PluginContainer` calls `actionRegistry.CreateAction(...)` and adds the instance to `Instances`. The action constructor runs here.
4. **Tick loop** — after connection is established, `OnTick()` is called on all instances approximately once per second.

## Implications for `OnDeviceDidConnect`

Because `DeviceDidConnectEvent` arrives before `WillAppearEvent`, **`OnDeviceDidConnect` is never called on action instances during startup**. The instances do not exist yet when the broadcast fires.

`OnDeviceDidConnect` is only reliable for **hot-plug** scenarios — when a Stream Deck device is physically disconnected and reconnected while the plugin is already running. In that case, instances exist and the broadcast reaches them correctly.

## Where to Put Initialization Logic

| Scenario | Correct Handler |
|---|---|
| Initialize state when the action first appears | Constructor |
| Respond to device reconnection (hot-plug) | `OnDeviceDidConnect` |
| Periodic state sync | `OnTick` |
| Respond to property inspector opening | `OnPropertyInspectorDidAppear` |

For state that must be current when the button is first shown, initialize it in the **constructor** or rely on `OnTick` to sync it within the first tick interval (~1 second).

## Instance Lifecycle

Instances are created and destroyed based on page visibility:

- **Created** (`WillAppearEvent`) — when the Stream Deck page containing the action becomes visible.
- **Destroyed** (`WillDisappearEvent`) — when the page is navigated away from. `Destroy()` → `Dispose()` is called on the instance.

An action is only "alive" while its page is visible. If the user switches pages, the instance is torn down. When they switch back, a new instance is constructed.
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Cmpnnt.StreamDeckToolkit.Backend;
using Cmpnnt.StreamDeckToolkit.Communication.Commands;
using Cmpnnt.StreamDeckToolkit.Communication.Commands.Dtos;
using Cmpnnt.StreamDeckToolkit.Communication.Events;
using Cmpnnt.StreamDeckToolkit.Utilities;
using SkiaSharp;

namespace Cmpnnt.StreamDeckToolkit.Communication;

/// <summary>
/// Underlying object that communicates with the stream deck app
/// </summary>
public class StreamDeckConnection : IDisposable, IAsyncDisposable
{
    private const int BUFFER_SIZE = 1024 * 1024;

    private IClientWebSocket webSocket;
    private readonly SemaphoreSlim sendSocketSemaphore = new(1);
    private readonly CancellationTokenSource cancelTokenSource;
    private readonly string registerEvent;
    private bool disposed = false;

    /// <summary>
    /// The port used to connect to the StreamDeck websocket
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// This is the unique identifier used to communicate with the register StreamDeck plugin.
    /// </summary>
    public string Uuid { get; set; }

    #region Public Events

    /// <summary>
    /// Raised when plugin is connected to stream deck app
    /// </summary>
    public event EventHandler<EventArgs> OnConnected;

    /// <summary>
    /// Raised when plugin is disconnected from stream deck app
    /// </summary>
    public event EventHandler<EventArgs> OnDisconnected;

    /// <summary>
    /// Raised when any event is received from the Stream Deck app.
    /// Use pattern matching on the BaseEvent to handle specific event types.
    /// </summary>
    public event EventHandler<BaseEvent> OnEventReceived;

    #endregion

    internal StreamDeckConnection(int port, string uuid, string registerEvent)
    {
        Port = port;
        Uuid = uuid;
        this.registerEvent = registerEvent;
        cancelTokenSource = new CancellationTokenSource();
    }
    
    // primarily for testing
    internal StreamDeckConnection(int port, string uuid, string registerEvent, CancellationTokenSource cancellationTokenSource, IClientWebSocket webSocket = null)
    {
        Port = port;
        Uuid = uuid;
        this.registerEvent = registerEvent;
        this.webSocket = webSocket ?? new ClientWebSocketWrapper();
        cancelTokenSource = cancellationTokenSource;
    }

    internal void Run()
    {
        if (webSocket != null)
        {
            return;
        }
        webSocket = new ClientWebSocketWrapper();
        _ = RunAsync();
    }

    internal void Stop()
    {
        if (!cancelTokenSource.IsCancellationRequested)
        {
            cancelTokenSource.Cancel();
        }
    }

    private Task SendAsync(IMessage message)
    {
        try
        {
            string json = message switch
            {
                RegisterEventCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.RegisterEventCommand),
                SetTitleCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.SetTitleCommand),
                LogCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.LogCommand),
                SetImageCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.SetImageCommand),
                ShowAlertCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.ShowAlertCommand),
                ShowOkCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.ShowOkCommand),
                SetGlobalSettingsCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.SetGlobalSettingsCommand),
                GetGlobalSettingsCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.GetGlobalSettingsCommand),
                SetSettingsCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.SetSettingsCommand),
                GetSettingsCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.GetSettingsCommand),
                SetStateCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.SetStateCommand),
                SendToPropertyInspectorCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.SendToPropertyInspectorCommand),
                SwitchToProfileCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.SwitchToProfileCommand),
                OpenUrlCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.OpenUrlCommand),
                SetFeedbackCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.SetFeedbackCommand),
                SetFeedbackCommandEx m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.SetFeedbackCommandEx),
                SetFeedbackLayoutCommand m => JsonSerializer.Serialize(m, CommandSerializerContext.Default.SetFeedbackLayoutCommand),
                _ => throw new NotSupportedException($"Serialization for type {message.GetType().FullName} is not supported by the source generator.")
            };
            
            return SendAsync(json);
        }
        catch (Exception ex)
        {
            Logger.Instance.LogMessage(TracingLevel.Error, $"{GetType()} SDTools SendAsync Exception: {ex}");
        }
        return null;
    }

    #region Requests
    internal Task SetTitleAsync(string title, string context, SdkTarget target, int? state)
    {
        return SendAsync(new SetTitleCommand(title, context, target, state));
    }

    internal Task LogMessageAsync(string message)
    {
        return SendAsync(new LogCommand(message));
    }

    internal Task SetImageAsync(SKData data, string context, SdkTarget target, int? state)
    {
        try
        {
            byte[] imageBytes = data.ToArray();

            // Convert byte[] to Base64 String
            var base64String = $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
            return SetImageAsync(base64String, context, target, state);
        }
        catch (Exception ex)
        {
            Logger.Instance.LogMessage(TracingLevel.Error, $"{GetType()} SetImageAsync Exception: {ex}");
        }
        return null;
    }

    internal Task SetImageAsync(string base64Image, string context, SdkTarget target, int? state)
    {
        return SendAsync(new SetImageCommand(base64Image, context, target, state));
    }

    internal Task ShowAlertAsync(string context)
    {
        return SendAsync(new ShowAlertCommand(context));
    }

    internal Task ShowOkAsync(string context)
    {
        return SendAsync(new ShowOkCommand(context));
    }

    internal Task SetGlobalSettingsAsync(JsonElement settings)
    {
        return SendAsync(new SetGlobalSettingsCommand(settings, Uuid));
    }

    internal Task GetGlobalSettingsAsync()
    {
        return SendAsync(new GetGlobalSettingsCommand(Uuid));
    }

    internal Task SetSettingsAsync(JsonElement settings, string context)
    {
        return SendAsync(new SetSettingsCommand(settings, context));
    }

    internal Task GetSettingsAsync(string context)
    {
        return SendAsync(new GetSettingsCommand(context));
    }

    internal Task SetStateAsync(uint? state, string context)
    {
        return SendAsync(new SetStateCommand(state, context));
    }

    internal Task SendToPropertyInspectorAsync(string action, JsonElement data, string context)
    {
        return SendAsync(new SendToPropertyInspectorCommand(action, data, context));
    }

    internal Task SwitchToProfileAsync(string device, string profileName, string context)
    {
        return SendAsync(new SwitchToProfileCommand(device, profileName, context));
    }
    internal Task OpenUrlAsync(string uri)
    {
        return OpenUrlAsync(new Uri(uri));
    }

    internal Task OpenUrlAsync(Uri uri)
    {
        return SendAsync(new OpenUrlCommand(uri));
    }

    internal Task SetFeedbackAsync(Dictionary<string, string> dictKeyValues, string context)
    {
        return SendAsync(new SetFeedbackCommand(dictKeyValues, context));
    }

    internal Task SetFeedbackAsync(JsonElement feedbackPayload, string context)
    {
        return SendAsync(new SetFeedbackCommandEx(feedbackPayload, context));
    }

    internal Task SetFeedbackLayoutAsync(string layout, string context)
    {
        return SendAsync(new SetFeedbackLayoutCommand(layout, context));
    }
    #endregion

    #region Private Methods
    private async Task SendAsync(string text)
    {
        try
        {
            if (webSocket != null)
            {
                try
                {
                    await sendSocketSemaphore.WaitAsync();
                    byte[] buffer = Encoding.UTF8.GetBytes(text);
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancelTokenSource.Token);
                }
                finally
                {
                    sendSocketSemaphore.Release();
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Instance.LogMessage(TracingLevel.Fatal, $"{GetType()} SendAsync Exception: {ex}");
            await DisconnectAsync();
        }

    }

    internal async Task RunAsync()
    {
        try
        {
            #if DEBUG
            Logger.Instance.LogMessage(TracingLevel.Info, "RunAsync: Attempting to connect...");
            #endif
            
            await webSocket.ConnectAsync(new Uri($"ws://localhost:{Port}"), cancelTokenSource.Token);   
            if (webSocket.State != WebSocketState.Open)
            {
                Logger.Instance.LogMessage(TracingLevel.Fatal, $"{GetType()} RunAsync failed - Websocket not open {webSocket.State}");
                await DisconnectAsync();
                return;
            }
            
            #if DEBUG
            Logger.Instance.LogMessage(TracingLevel.Info, "RunAsync: WebSocket Connected. Sending registration...");
            Logger.Instance.LogMessage(TracingLevel.Info, $"RegisterEvent: {registerEvent}, uuid: {Uuid}");
            #endif
            
            await SendAsync(new RegisterEventCommand(registerEvent, Uuid));
            
            OnConnected?.Invoke(this, EventArgs.Empty);
            
            await ReceiveAsync();
        }
        catch (Exception ex)
        {
            Logger.Instance.LogMessage(TracingLevel.Fatal, $"{GetType()} ReceiveAsync Exception: {ex}");
        }
        finally
        {
            Logger.Instance.LogMessage(TracingLevel.Info, $"{GetType()} RunAsync completed, shutting down");
            await DisconnectAsync();
        }
    }

    internal async Task<WebSocketCloseStatus> ReceiveAsync()
    {
        var buffer = new byte[BUFFER_SIZE];
        var arrayBuffer = new ArraySegment<byte>(buffer);
        var textBuffer = new StringBuilder(BUFFER_SIZE);

        try
        {
            while (!cancelTokenSource.IsCancellationRequested && webSocket != null)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(arrayBuffer, cancelTokenSource.Token);

                if (result == null)
                {
                    continue;
                }

                if (result.MessageType == WebSocketMessageType.Close ||
                    (result.CloseStatus is not null && result.CloseStatus.Value != WebSocketCloseStatus.Empty))
                {
                    string closeStatus = (result.CloseStatus == null) ? "None" :
                        (result.CloseStatus.HasValue) ? result.CloseStatus.Value.ToString() : "None";

                    Logger.Instance.LogMessage(TracingLevel.Info,
                        $"{GetType()} Received websocket close message. MessageType: {result.MessageType} CloseStatus: {closeStatus}");
                    return result.CloseStatus.GetValueOrDefault();
                }

                if (result.MessageType != WebSocketMessageType.Text)
                {
                    continue;
                }
                textBuffer.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                if (!result.EndOfMessage)
                {
                    #if DEBUG
                    Logger.Instance.LogMessage(TracingLevel.Debug,
                        "ReceiveAsync: Message is fragmented, continuing to receive...");
                    #endif

                    continue;
                }

                #if DEBUG
                Logger.Instance.LogMessage(TracingLevel.Debug, $"Incoming Message: {textBuffer}");
                #endif

                var strBuffer = textBuffer.ToString();
                textBuffer.Clear();
                Logger.Instance.LogMessage(TracingLevel.Info, $"ReceiveAsync: Full message received: {strBuffer}");

                BaseEvent evt = BaseEvent.Deserialize(strBuffer);
                if (evt == null)
                {
                    Logger.Instance.LogMessage(TracingLevel.Warn,
                        $"{GetType()} Unknown event received from Stream Deck: {strBuffer}");
                    continue;
                }

                #if DEBUG
                Logger.Instance.LogMessage(TracingLevel.Info, $"ReceiveAsync: Parsed event '{evt.GetType().Name}'.");
                #endif

                try
                {
                    OnEventReceived?.Invoke(this, evt);
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogMessage(TracingLevel.Error,
                        $"{GetType()} Unhandled exception when handling {evt.GetType().Name} event. Exception: {ex}");
                }
            }
        }
        catch (WebSocketException ex)
        {
            Logger.Instance.LogMessage(TracingLevel.Info, $"{GetType()} ReceiveAsync Exception: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Logger.Instance.LogMessage(TracingLevel.Info, $"{GetType()} ReceiveAsync Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            Logger.Instance.LogMessage(TracingLevel.Fatal, $"{GetType()} ReceiveAsync Exception: {ex}");
        }

        Logger.Instance.LogMessage(TracingLevel.Info, $"{GetType()} ReceiveAsync ended with CancelToken: {cancelTokenSource.IsCancellationRequested} Websocket: {(webSocket == null ? "null" : "valid")}");
        
        return WebSocketCloseStatus.NormalClosure;
    }

    private async Task DisconnectAsync()
    {
        if (webSocket != null)
        {
            IClientWebSocket socket = webSocket;
            webSocket = null;

            try
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnecting", cancelTokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Error, $"{GetType()} DisconnectAsync failed to close connection. Exception: {ex}");
            }
            
            try
            {
                socket.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.Error, $"{GetType()} DisconnectAsync failed to dispose websocket. Exception: {ex}");
            }

            OnDisconnected?.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion
    
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }
        Stop();
        sendSocketSemaphore?.Dispose();
        cancelTokenSource?.Dispose();
        disposed = true;
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (disposed)
        {
            return;
        }

        if (!cancelTokenSource.IsCancellationRequested)
        {
            await cancelTokenSource.CancelAsync();
        }

        await DisconnectAsync();
        
        sendSocketSemaphore?.Dispose();
        cancelTokenSource?.Dispose();
        disposed = true;
        GC.SuppressFinalize(this);
    }
}

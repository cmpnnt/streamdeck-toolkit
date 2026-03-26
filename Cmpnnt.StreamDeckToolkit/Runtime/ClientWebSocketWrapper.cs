using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cmpnnt.StreamDeckToolkit.Runtime;

/// <summary>Default <see cref="IClientWebSocket"/> implementation backed by <see cref="System.Net.WebSockets.ClientWebSocket"/>.</summary>
public class ClientWebSocketWrapper : IClientWebSocket
{
    private readonly ClientWebSocket clientWebSocket = new();

    /// <summary>Initializes a new instance and sets a 30-second keep-alive interval.</summary>
    public ClientWebSocketWrapper()
    {
        clientWebSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(30);
    }

    /// <inheritdoc/>
    public WebSocketState State => clientWebSocket.State;

    /// <inheritdoc/>
    public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
    {
        return clientWebSocket.ConnectAsync(uri, cancellationToken);
    }

    /// <inheritdoc/>
    public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
    {
        return clientWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        return clientWebSocket.ReceiveAsync(buffer, cancellationToken);
    }

    /// <inheritdoc/>
    public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        return clientWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        clientWebSocket.Dispose();
    }
}

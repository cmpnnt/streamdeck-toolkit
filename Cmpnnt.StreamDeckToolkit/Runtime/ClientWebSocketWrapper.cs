using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cmpnnt.StreamDeckToolkit.Runtime;

public class ClientWebSocketWrapper : IClientWebSocket
{
    private readonly ClientWebSocket clientWebSocket = new();

    public ClientWebSocketWrapper()
    {
        clientWebSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(30);
    }

    public WebSocketState State => clientWebSocket.State;

    public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
    {
        return clientWebSocket.ConnectAsync(uri, cancellationToken);
    }

    public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
    {
        return clientWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
    }

    public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        return clientWebSocket.ReceiveAsync(buffer, cancellationToken);
    }

    public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        return clientWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
    }

    public void Dispose()
    {
        clientWebSocket.Dispose();
    }
}
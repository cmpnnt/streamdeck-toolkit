using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cmpnnt.StreamDeckToolkit.Runtime;

/// <summary>Abstraction over <see cref="System.Net.WebSockets.ClientWebSocket"/> to enable testing and injection.</summary>
public interface IClientWebSocket : IDisposable
{
    /// <summary>Gets the current state of the WebSocket connection.</summary>
    WebSocketState State { get; }

    /// <summary>Connects to the specified URI.</summary>
    Task ConnectAsync(Uri uri, CancellationToken cancellationToken);

    /// <summary>Sends data over the WebSocket connection.</summary>
    Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken);

    /// <summary>Receives data from the WebSocket connection.</summary>
    Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);

    /// <summary>Closes the WebSocket connection.</summary>
    Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);
}

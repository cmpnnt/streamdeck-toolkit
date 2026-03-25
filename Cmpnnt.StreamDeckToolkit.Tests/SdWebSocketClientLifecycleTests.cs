using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Cmpnnt.StreamDeckToolkit.Runtime;
using Cmpnnt.StreamDeckToolkit.Communication;
using NSubstitute;

namespace Cmpnnt.StreamDeckToolkit.Tests;

// TODO: Test more ClientWebSocket scenarios (closed, aborted, etc)
public class SdWebSocketClientLifecycleTests
{
    [Test]
    public async Task RunAsync_ShouldRaiseOnConnected_WhenWebSocketOpens()
    {
        // Arrange
        var mockWebSocket = Substitute.For<IClientWebSocket>();
        var cts = new CancellationTokenSource();

        mockWebSocket.State.Returns(WebSocketState.Open);
        mockWebSocket.ReceiveAsync(Arg.Any<ArraySegment<byte>>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                cts.Cancel();
                return Task.FromResult(new WebSocketReceiveResult(0, WebSocketMessageType.Text, true));
            });

        var connection = new SdWebSocketClient(1234, "test-uuid", "register-event", cts, mockWebSocket);

        var connected = false;
        connection.OnConnected += (_, _) => connected = true;

        // Act
        await connection.RunAsync();

        // Assert
        await Assert.That(connected).IsTrue();
    }

    [Test]
    public async Task RunAsync_ShouldRaiseOnDisconnected_WhenConnectionCloses()
    {
        // Arrange
        var mockWebSocket = Substitute.For<IClientWebSocket>();
        var cts = new CancellationTokenSource();

        mockWebSocket.State.Returns(WebSocketState.Open);
        mockWebSocket.ReceiveAsync(Arg.Any<ArraySegment<byte>>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                cts.Cancel();
                return Task.FromResult(new WebSocketReceiveResult(0, WebSocketMessageType.Text, true));
            });

        var connection = new SdWebSocketClient(1234, "test-uuid", "register-event", cts, mockWebSocket);

        var disconnected = false;
        connection.OnDisconnected += (_, _) => disconnected = true;

        // Act
        await connection.RunAsync();

        // Assert
        await Assert.That(disconnected).IsTrue();
    }
}

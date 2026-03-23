using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cmpnnt.StreamDeckToolkit.Backend;
using Cmpnnt.StreamDeckToolkit.Communication;
using Cmpnnt.StreamDeckToolkit.Communication.Events;
using Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos;
using NSubstitute;

namespace Cmpnnt.StreamDeckToolkit.Tests;

public class StreamDeckConnectionTests
{
    // TODO: Test more ClientWebSocket scenarios (closed, aborted, etc)
    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenKeyDownMessageIsReceived()
    {
        // Arrange
        var mockWebSocket = Substitute.For<IClientWebSocket>();
        var cts = new CancellationTokenSource();

        const string message = """{"event":"keyDown","action":"com.example.action","context":"some_context","device":"some_device","payload":{"settings":{},"coordinates":{"column":1,"row":1},"state":0,"userDesiredEncoding":0,"isInMultiAction":false}}""";
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        var messageSegment = new ArraySegment<byte>(messageBytes);

        mockWebSocket.State.Returns(WebSocketState.Open);

        mockWebSocket.ReceiveAsync(Arg.Any<ArraySegment<byte>>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var buffer = callInfo.Arg<ArraySegment<byte>>();
                messageSegment.CopyTo(buffer);

                var result = new WebSocketReceiveResult(messageSegment.Count, WebSocketMessageType.Text, true);

                cts.Cancel();

                return Task.FromResult(result);
            });

        var connection = new StreamDeckConnection(1234, "test-uuid", "register-event", cts, mockWebSocket);

        BaseEvent? receivedEvent = null;
        connection.OnEventReceived += (_, evt) =>
        {
            receivedEvent = evt;
        };

        // Act
        await connection.RunAsync();

        // Assert
        await Assert.That(receivedEvent).IsNotNull();
        await Assert.That(receivedEvent).IsAssignableTo<KeyDownEvent>();
        var keyDownEvent = (KeyDownEvent)receivedEvent!;
        await Assert.That(keyDownEvent.Action).IsEqualTo("com.example.action");
        await Assert.That(keyDownEvent.Context).IsEqualTo("some_context");
    }
}

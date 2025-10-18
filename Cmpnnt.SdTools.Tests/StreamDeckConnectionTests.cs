using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cmpnnt.SdTools.Communication;
using Cmpnnt.SdTools.Backend;
using Cmpnnt.SdTools.Communication.Events.Dtos;
using NSubstitute;

namespace Cmpnnt.SdTools.Tests;

public class StreamDeckConnectionTests
{
    // TODO: Test more ClientWebSocket scenarios (closed, aborted, etc)
    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnKeyDownEvent_WhenKeyDownMessageIsReceived()
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
                // This line copies the JSON message above from the messageSegment to the buffer passed into the ReceiveAsync method
                var buffer = callInfo.Arg<ArraySegment<byte>>();
                messageSegment.CopyTo(buffer);
                
                var result = new WebSocketReceiveResult(messageSegment.Count, WebSocketMessageType.Text, true);
                
                // End the infinite loop
                cts.Cancel();
                
                return Task.FromResult(result);
            });

        var connection = new StreamDeckConnection(1234, "test-uuid", "register-event", cts, mockWebSocket);

        KeyDownEvent? receivedEvent = null;
        connection.OnKeyDown += (_, args) =>
        {
            receivedEvent = args.Event;
        };
        
        // Act
        await connection.RunAsync();
        
        // Assert
        await Assert.That(receivedEvent).IsNotNull();
        await Assert.That(receivedEvent?.Action).IsEqualTo("com.example.action");
        await Assert.That(receivedEvent?.Context).IsEqualTo("some_context");
        
    }
}
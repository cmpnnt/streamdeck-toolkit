using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cmpnnt.StreamDeckToolkit.Runtime;
using Cmpnnt.StreamDeckToolkit.Communication;
using Cmpnnt.StreamDeckToolkit.Communication.Events;
using Cmpnnt.StreamDeckToolkit.Communication.Events.Dtos;
using NSubstitute;

namespace Cmpnnt.StreamDeckToolkit.Tests;

public class SdWebSocketClientEventTests
{
    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenKeyDownMessageIsReceived()
    {
        const string message = """{"event":"keyDown","action":"com.example.action","context":"some_context","device":"some_device","payload":{"settings":{},"coordinates":{"column":1,"row":1},"state":0,"userDesiredEncoding":0,"isInMultiAction":false}}""";
        var evt = await EventTest<KeyDownEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenKeyUpMessageIsReceived()
    {
        const string message = """{"event":"keyUp","action":"com.example.action","context":"some_context","device":"some_device","payload":{"settings":{},"coordinates":{"column":1,"row":1},"state":0,"userDesiredState":0,"isInMultiAction":false}}""";
        var evt = await EventTest<KeyUpEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenWillAppearMessageIsReceived()
    {
        const string message = """{"event":"willAppear","action":"com.example.action","context":"some_context","device":"some_device","payload":{"settings":{},"coordinates":{"column":1,"row":1},"state":0,"isInMultiAction":false,"controller":"Keypad"}}""";
        var evt = await EventTest<WillAppearEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenWillDisappearMessageIsReceived()
    {
        const string message = """{"event":"willDisappear","action":"com.example.action","context":"some_context","device":"some_device","payload":{"settings":{},"coordinates":{"column":1,"row":1},"state":0,"isInMultiAction":false,"controller":"Keypad"}}""";
        var evt = await EventTest<WillDisappearEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenTitleParametersDidChangeMessageIsReceived()
    {
        const string message = """{"event":"titleParametersDidChange","action":"com.example.action","context":"some_context","device":"some_device","payload":{"settings":{},"coordinates":{"column":1,"row":1},"state":0,"title":"Hello","titleParameters":{"fontFamily":"Arial","fontSize":12,"fontStyle":"Regular","fontUnderline":false,"showTitle":true,"titleAlignment":"middle","titleColor":"#ffffff"}}}""";
        var evt = await EventTest<TitleParametersDidChangeEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenDeviceDidConnectMessageIsReceived()
    {
        const string message = """{"event":"deviceDidConnect","device":"some_device","deviceInfo":{"size":{"rows":3,"columns":5},"type":0,"id":"some_device_id"}}""";
        var evt = await EventTest<DeviceDidConnectEvent>(message);
        await Assert.That(evt.Device).IsEqualTo("some_device");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenDeviceDidDisconnectMessageIsReceived()
    {
        const string message = """{"event":"deviceDidDisconnect","device":"some_device"}""";
        var evt = await EventTest<DeviceDidDisconnectEvent>(message);
        await Assert.That(evt.Device).IsEqualTo("some_device");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenApplicationDidLaunchMessageIsReceived()
    {
        const string message = """{"event":"applicationDidLaunch","payload":{"application":"com.example.app"}}""";
        var evt = await EventTest<ApplicationDidLaunchEvent>(message);
        await Assert.That(evt.Payload.Application).IsEqualTo("com.example.app");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenApplicationDidTerminateMessageIsReceived()
    {
        const string message = """{"event":"applicationDidTerminate","payload":{"application":"com.example.app"}}""";
        var evt = await EventTest<ApplicationDidTerminateEvent>(message);
        await Assert.That(evt.Payload.Application).IsEqualTo("com.example.app");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenSystemDidWakeUpMessageIsReceived()
    {
        const string message = """{"event":"systemDidWakeUp"}""";
        await EventTest<SystemDidWakeUpEvent>(message);
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenDidReceiveSettingsMessageIsReceived()
    {
        const string message = """{"event":"didReceiveSettings","action":"com.example.action","context":"some_context","device":"some_device","payload":{"settings":{},"coordinates":{"column":1,"row":1},"isInMultiAction":false,"state":0}}""";
        var evt = await EventTest<DidReceiveSettingsEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenDidReceiveGlobalSettingsMessageIsReceived()
    {
        const string message = """{"event":"didReceiveGlobalSettings","payload":{"settings":{}}}""";
        await EventTest<DidReceiveGlobalSettingsEvent>(message);
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenPropertyInspectorDidAppearMessageIsReceived()
    {
        const string message = """{"event":"propertyInspectorDidAppear","action":"com.example.action","context":"some_context","device":"some_device"}""";
        var evt = await EventTest<PropertyInspectorDidAppearEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenPropertyInspectorDidDisappearMessageIsReceived()
    {
        const string message = """{"event":"propertyInspectorDidDisappear","action":"com.example.action","context":"some_context","device":"some_device"}""";
        var evt = await EventTest<PropertyInspectorDidDisappearEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenSendToPluginMessageIsReceived()
    {
        const string message = """{"event":"sendToPlugin","action":"com.example.action","context":"some_context","payload":{}}""";
        var evt = await EventTest<SendToPluginEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenDialRotateMessageIsReceived()
    {
        const string message = """{"event":"dialRotate","action":"com.example.action","context":"some_context","device":"some_device","payload":{"controller":"Encoder","settings":{},"coordinates":{"column":0,"row":0},"ticks":3,"pressed":false}}""";
        var evt = await EventTest<DialRotateEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenDialDownMessageIsReceived()
    {
        const string message = """{"event":"dialDown","action":"com.example.action","context":"some_context","device":"some_device","payload":{"controller":"Encoder","settings":{},"coordinates":{"column":0,"row":0}}}""";
        var evt = await EventTest<DialDownEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenDialUpMessageIsReceived()
    {
        const string message = """{"event":"dialUp","action":"com.example.action","context":"some_context","device":"some_device","payload":{"controller":"Encoder","settings":{},"coordinates":{"column":0,"row":0}}}""";
        var evt = await EventTest<DialUpEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    [Test]
    public async Task ReceiveAsync_ShouldRaiseOnEventReceived_WhenTouchTapMessageIsReceived()
    {
        const string message = """{"event":"touchTap","action":"com.example.action","context":"some_context","device":"some_device","payload":{"controller":"Encoder","settings":{},"coordinates":{"column":0,"row":0},"isLongPress":false,"tapPos":[100,50]}}""";
        var evt = await EventTest<TouchTapEvent>(message);
        await Assert.That(evt.Action).IsEqualTo("com.example.action");
        await Assert.That(evt.Context).IsEqualTo("some_context");
    }

    private async Task<T> EventTest<T>(string message) where T : BaseEvent
    {
        // Arrange
        var mockWebSocket = Substitute.For<IClientWebSocket>();
        var cts = new CancellationTokenSource();

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

        var connection = new SdWebSocketClient(1234, "test-uuid", "register-event", cts, mockWebSocket);

        BaseEvent? receivedEvent = null;
        connection.OnEventReceived += (_, evt) =>
        {
            receivedEvent = evt;
        };

        // Act
        await connection.RunAsync();

        // Assert
        await Assert.That(receivedEvent).IsNotNull();
        await Assert.That(receivedEvent).IsAssignableTo<T>();
        return (T)receivedEvent!;
    }
}

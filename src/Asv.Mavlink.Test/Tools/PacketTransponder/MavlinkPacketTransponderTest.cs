using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Minimal;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Moq;
using R3;
using Xunit;

namespace Asv.Mavlink.Test;

public class MavlinkPacketTransponderTest
{
    private readonly FakeTimeProvider _time = new();
    private readonly Mock<IProtocolConnection> _connection = new();

    private MavlinkPacketTransponder<HeartbeatPacket> Create()
    {
        var core = new Mock<IMavlinkContext>();
        core.SetupGet(x => x.TimeProvider).Returns(_time);
        core.SetupGet(x => x.Connection).Returns(_connection.Object);
        core.SetupGet(x => x.LoggerFactory).Returns(NullLoggerFactory.Instance);
        core.SetupGet(x => x.Sequence).Returns(new PacketSequenceCalculator());
        return new MavlinkPacketTransponder<HeartbeatPacket>(new MavlinkIdentity(1, 2), core.Object);
    }

    [Fact]
    public void CompletedSend_KeepsQueuedPacketsIndependentUntilEncoding()
    {
        var queued = new List<HeartbeatPacket>();
        _connection.Setup(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()))
            .Callback<IProtocolMessage, CancellationToken>((packet, _) => queued.Add((HeartbeatPacket)packet))
            .Returns(ValueTask.CompletedTask);
        using var transponder = Create();
        transponder.Set(packet =>
        {
            packet.Payload.CustomMode = 11;
            packet.Tags["value"] = 11;
        });
        transponder.Start(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        _time.Advance(TimeSpan.FromSeconds(1));
        transponder.Set(packet =>
        {
            packet.Payload.CustomMode = 22;
            packet.Tags["value"] = 22;
        });
        _time.Advance(TimeSpan.FromSeconds(1));
        transponder.Set(packet =>
        {
            packet.Payload.CustomMode = 33;
            packet.Tags["value"] = 33;
        });

        Assert.Equal(2, queued.Count);
        Assert.NotSame(queued[0], queued[1]);
        Assert.Equal(11, queued[0].Tags["value"]);
        Assert.Equal(22, queued[1].Tags["value"]);
        // Physical connections encode queued objects after Send has already completed.
        var decoded = queued.ConvertAll(packet =>
        {
            ReadOnlySpan<byte> buffer = packet.EncodeToArray(DefaultProtocolEncoder.Instance);
            var result = new HeartbeatPacket();
            result.Deserialize(ref buffer);
            Assert.True(buffer.IsEmpty);
            return result;
        });
        Assert.Equal(new uint[] { 11, 22 }, decoded.ConvertAll(packet => packet.Payload.CustomMode));
        Assert.Equal(new byte[] { 1, 2 }, decoded.ConvertAll(packet => packet.Sequence));
    }

    [Fact]
    public async Task SlowSend_SkipsTicksAndKeepsPacketStableUntilCompletion()
    {
        var pending = new TaskCompletionSource();
        HeartbeatPacket? sent = null;
        var sequences = new List<byte>();
        _connection.Setup(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()))
            .Callback<IProtocolMessage, CancellationToken>((packet, _) =>
            {
                sent = (HeartbeatPacket)packet;
                sequences.Add(sent.Sequence);
            })
            .Returns<IProtocolMessage, CancellationToken>((_, cancel) => new ValueTask(pending.Task.WaitAsync(cancel)));
        await using var transponder = Create();
        transponder.Set(x =>
        {
            x.Payload.CustomMode = 42;
            x.CompatFlags = 4;
            x.IncompatFlags = 1;
            x.Signature.IsPresent = true;
            x.Signature.LinkId = 3;
            x.Signature.Timestamp = 123;
            x.Signature.Sign = 456;
            for (var i = 0; i < 10; i++) x.Tags[$"tag{i}"] = i;
        });
        transponder.Start(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        _time.Advance(TimeSpan.FromSeconds(10));
        _connection.Verify(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(PacketTransponderState.Skipped, transponder.State.CurrentValue);
        transponder.Set(x =>
        {
            x.Payload.CustomMode = 99;
            x.Tags["tag0"] = 99;
        });
        Assert.Equal(42u, sent!.Payload.CustomMode);
        Assert.Equal((byte)1, sent.Sequence);
        Assert.Equal(new byte[] { 1 }, sequences);
        Assert.Equal(0, sent.Tags["tag0"]);
        Assert.Equal((byte)1, sent.SystemId);
        Assert.Equal((byte)2, sent.ComponentId);
        Assert.Equal((byte)4, sent.CompatFlags);
        Assert.Equal((byte)1, sent.IncompatFlags);
        Assert.True(sent.Signature.IsPresent);
        Assert.Equal((byte)3, sent.Signature.LinkId);
        Assert.Equal(123ul, sent.Signature.Timestamp);
        Assert.Equal(456ul, sent.Signature.Sign);
        pending.SetResult();
        await WaitForState(transponder, PacketTransponderState.Ok);
        _time.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(99u, sent.Payload.CustomMode);
        Assert.Equal((byte)2, sent.Sequence);
        Assert.Equal(new byte[] { 1, 2 }, sequences);
        Assert.Equal(99, sent.Tags["tag0"]);
        _connection.Verify(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task AsynchronousFailure_IsObservedAndNextTickCanRetry()
    {
        var pending = new TaskCompletionSource();
        _connection.SetupSequence(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask(pending.Task)).Returns(ValueTask.CompletedTask);
        await using var transponder = Create();
        transponder.Start(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        _time.Advance(TimeSpan.FromSeconds(1));
        pending.SetException(new InvalidOperationException("Send failed"));
        await WaitForState(transponder, PacketTransponderState.ErrorToSend);
        _time.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(PacketTransponderState.Ok, transponder.State.CurrentValue);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Dispose_CancelsPendingSendAndRemovesTimer(bool asynchronous)
    {
        var cancelled = new TaskCompletionSource();
        _connection.Setup(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()))
            .Returns<IProtocolMessage, CancellationToken>((_, cancel) => new ValueTask(WaitForCancellation(cancel)));
        var transponder = Create();
        transponder.Start(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        _time.Advance(TimeSpan.FromSeconds(1));
        if (asynchronous) await transponder.DisposeAsync();
        else transponder.Dispose();
        await cancelled.Task.WaitAsync(TimeSpan.FromSeconds(10), Xunit.TestContext.Current.CancellationToken);
        Assert.False(transponder.IsStarted);
        _time.Advance(TimeSpan.FromSeconds(10));
        _connection.Verify(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Throws<ObjectDisposedException>(() => transponder.Start(TimeSpan.Zero, TimeSpan.FromSeconds(1)));
        return;

        async Task WaitForCancellation(CancellationToken cancel)
        {
            try { await Task.Delay(Timeout.InfiniteTimeSpan, cancel); }
            finally { cancelled.SetResult(); }
        }
    }

    [Fact]
    public async Task DisposeAsync_WaitsUntilTransportReleasesPacket()
    {
        var pending = new TaskCompletionSource();
        CancellationToken sendCancel = default;
        _connection.Setup(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()))
            .Callback<IProtocolMessage, CancellationToken>((_, cancel) => sendCancel = cancel)
            .Returns(() => new ValueTask(pending.Task));
        var transponder = Create();
        try
        {
            transponder.Start(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            _time.Advance(TimeSpan.FromSeconds(1));
            var disposing = transponder.DisposeAsync().AsTask();
            try
            {
                Assert.True(sendCancel.IsCancellationRequested);
                Assert.False(disposing.IsCompleted);
            }
            finally
            {
                pending.TrySetResult();
                await disposing.WaitAsync(TimeSpan.FromSeconds(10), Xunit.TestContext.Current.CancellationToken);
            }
            _time.Advance(TimeSpan.FromSeconds(5));
            _connection.Verify(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        finally
        {
            pending.TrySetResult();
            await transponder.DisposeAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(10), Xunit.TestContext.Current.CancellationToken);
        }
    }

    [Fact]
    public async Task StopAndRestart_DoesNotOverlapExistingSend()
    {
        var pending = new TaskCompletionSource();
        _connection.Setup(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()))
            .Returns<IProtocolMessage, CancellationToken>((_, cancel) => new ValueTask(pending.Task.WaitAsync(cancel)));
        await using var transponder = Create();
        transponder.Start(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        _time.Advance(TimeSpan.FromSeconds(1));
        transponder.Stop();
        Assert.False(transponder.IsStarted);
        _time.Advance(TimeSpan.FromSeconds(10));
        transponder.Start(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        _time.Advance(TimeSpan.FromSeconds(5));
        _connection.Verify(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        pending.SetResult();
        await WaitForState(transponder, PacketTransponderState.Ok);
        _time.Advance(TimeSpan.FromSeconds(1));
        _connection.Verify(x => x.Send(It.IsAny<IProtocolMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    private static async Task WaitForState(MavlinkPacketTransponder<HeartbeatPacket> transponder, PacketTransponderState state)
    {
        var changed = new TaskCompletionSource();
        using var subscription = transponder.State.Subscribe(value =>
        {
            if (value == state) changed.TrySetResult();
        });
        await changed.Task.WaitAsync(TimeSpan.FromSeconds(10), Xunit.TestContext.Current.CancellationToken);
    }
}

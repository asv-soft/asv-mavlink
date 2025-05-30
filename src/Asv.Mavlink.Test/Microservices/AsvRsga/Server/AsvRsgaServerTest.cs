using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.AsvRsga;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvRsgaServer))]
public class AsvRsgaServerTest : ServerTestBase<AsvRsgaServer>, IDisposable
{
    private readonly TaskCompletionSource<MavlinkMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public AsvRsgaServerTest(ITestOutputHelper output) : base(output)
    {
        _taskCompletionSource = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override AsvRsgaServer CreateServer(MavlinkIdentity identity, CoreServices core) => new(identity, core);

    [Fact]
    public async Task SendCompatibilityResponse_SendSinglePacket_Success()
    {
        // Arrange
        var payload = new AsvRsgaCompatibilityResponsePayload
        {
            RequestId = 1,
            Result = AsvRsgaRequestAck.AsvRsgaRequestAckOk,
        };

        using var sub = Link.Client.OnRxMessage.FilterByType<MavlinkMessage>().Subscribe(p => _taskCompletionSource.TrySetResult(p));
        // Act
        await Server.SendCompatibilityResponse(p => p.RequestId = 1, _cancellationTokenSource.Token);

        // Assert
        var result = await _taskCompletionSource.Task as AsvRsgaCompatibilityResponsePacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.True(payload.IsDeepEqual(result?.Payload));
    }

    [Theory]
    [InlineData(10)]
    [InlineData(200)]
    [InlineData(20000)]
    public async Task SendCompatibilityResponse_SendManyPackets_Success(int packetCount)
    {
        // Arrange
        var called = 0;
        var results = new List<AsvRsgaCompatibilityResponsePayload>();
        var serverResults = new List<AsvRsgaCompatibilityResponsePayload>();
        using var sub = Link.Client.OnRxMessage.FilterByType<MavlinkMessage>().Subscribe(p =>
        {
            called++;
            if (p is AsvRsgaCompatibilityResponsePacket packet)
            {
                results.Add(packet.Payload);
            }

            if (called >= packetCount)
            {
                _taskCompletionSource.TrySetResult(p);
            }
        });

        // Act
        for (var i = 0; i < packetCount; i++)
        {
            await Server.SendCompatibilityResponse(p => serverResults.Add(p), _cancellationTokenSource.Token);
        }

        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(packetCount, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(packetCount, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(packetCount, results.Count);
        Assert.Equal(serverResults.Count, results.Count);
        for (var i = 0; i < results.Count; i++)
        {
            Assert.True(results[i].IsDeepEqual(serverResults[i]));
        }
    }

    [Fact]
    public async Task SendCompatibilityResponse_ArgumentCanceledToken_ShouldThrowOperationCanceledException()
    {
        // Arrange
        using var sub = Link.Client.OnRxMessage.FilterByType<MavlinkMessage>().Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
        // Act
        await _cancellationTokenSource.CancelAsync();
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await Server.SendCompatibilityResponse(p => { }, _cancellationTokenSource.Token));
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.RxMessages);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}
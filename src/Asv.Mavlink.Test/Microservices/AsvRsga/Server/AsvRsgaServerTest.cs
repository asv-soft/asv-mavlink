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
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public AsvRsgaServerTest(ITestOutputHelper output) : base(output)
    {
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override AsvRsgaServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);

    [Fact]
    public async Task SendCompatibilityResponse_SendSinglePacket_Success()
    {
        // Arrange
        var payload = new AsvRsgaCompatibilityResponsePayload
        {
            RequestId = 1,
            Result = AsvRsgaRequestAck.AsvRsgaRequestAckOk,
            SupportedModes = new byte[32],
        };

        using var sub = Link.Client.OnRxMessage.Subscribe(p => _taskCompletionSource.TrySetResult(p));
        // Act
        await Server.SendCompatibilityResponse(p => p.RequestId = 1, _cancellationTokenSource.Token);

        // Assert
        var result = await _taskCompletionSource.Task as AsvRsgaCompatibilityResponsePacket;
        Assert.NotNull(result);
        Assert.Equal((uint)1, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.True(payload.IsDeepEqual(result?.Payload));
    }

    [Theory]
    [InlineData(10)]
    [InlineData(200)]
    [InlineData(20000)]
    public async Task SendCompatibilityResponse_SendManyPackets_Success(uint packetCount)
    {
        // Arrange
        var called = 0;
        var results = new List<AsvRsgaCompatibilityResponsePayload>();
        var serverResults = new List<AsvRsgaCompatibilityResponsePayload>();
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
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
        Assert.Equal(packetCount, Link.Server.Statistic.TxMessages);
        Assert.Equal(packetCount, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)packetCount, results.Count);
        Assert.Equal(serverResults.Count, results.Count);
        for (var i = 0; i < results.Count; i++)
        {
            Assert.True(results[i].IsDeepEqual(serverResults[i]));
        }
    }

    [Fact(Skip = "Cancellation doesn't work")] // TODO: FIX CANCELLATION
    public async Task SendCompatibilityResponse_WhenCanceled_ShouldThrowOperationCanceledException()
    {
        // Arrange
        using var sub = Link.Client.OnRxMessage.Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
        _cancellationTokenSource.Cancel();

        // Act
        var task = Server.SendCompatibilityResponse(p => { }, _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);

        Assert.Equal((uint)0, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
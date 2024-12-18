using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvGbsServer))]
public class AdsbVehicleServerTest : ServerTestBase<AdsbVehicleServer>, IDisposable
{
    private readonly TaskCompletionSource<MavlinkMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public AdsbVehicleServerTest(ITestOutputHelper output) : base(output)
    {
        _taskCompletionSource = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override AdsbVehicleServer CreateClient(MavlinkIdentity identity, CoreServices core) =>
        new(identity, core);

    [Fact]
    public async Task Send_SinglePacket_Success()
    {
        // Arrange
        var icao = (uint)123;
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
        // Act
        await Server.Send(p => p.IcaoAddress = icao, _cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task.ConfigureAwait(false) as AdsbVehiclePacket;
        Assert.NotNull(res);
        Assert.Equal(1, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(icao, res?.Payload.IcaoAddress);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(200)]
    [InlineData(20000)]
    public async Task Send_ManyPackets_Success(int packetsCount)
    {
        // Arrange
        var called = 0;
        var results = new List<AdsbVehiclePayload>();
        var serverResults = new List<AdsbVehiclePayload>();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
        {
            called++;
            if (p is AdsbVehiclePacket packet)
            {
                results.Add(packet.Payload);
            }

            if (called >= packetsCount)
            {
                _taskCompletionSource.TrySetResult(p);
            }
        });

        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Server.Send(payload => serverResults.Add(payload), default);
        }

        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(packetsCount, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(packetsCount, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(packetsCount, results.Count);
        Assert.Equal(serverResults.Count, results.Count);
        for (var i = 0; i < results.Count; i++)
        {
            Assert.True(results[i].IsDeepEqual(serverResults[i]));
        }
    }

    [Fact]
    public async Task Send_Canceled_Throws()
    {
        // Arrange
        AdsbVehiclePacket? packet = new AdsbVehiclePacket();
        OperationCanceledException? ex = null;
        
        var task = Task.Factory.StartNew(async () =>
        {
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    await Server.Send(p => p = packet.Payload, _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException e)
                {
                    ex = e;
                    throw;
                }
            }
        });
        // Act
        await _cancellationTokenSource.CancelAsync();
        task.Wait();
        // Assert
        Assert.NotNull(ex);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task Send_ArgumentCanceledToken_Throws()
    {
        // Arrange
        AdsbVehiclePacket? packet = new AdsbVehiclePacket();
        // Act
        await _cancellationTokenSource.CancelAsync();
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Server.Send(p => p = packet.Payload, _cancellationTokenSource.Token);
        });
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
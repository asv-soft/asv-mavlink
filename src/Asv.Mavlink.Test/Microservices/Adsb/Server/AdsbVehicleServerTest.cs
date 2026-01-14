using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using FluentAssertions;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvGbsServer))]
public class AdsbVehicleServerTest : ServerTestBase<AdsbVehicleServer>
{
    private readonly TaskCompletionSource<IProtocolMessage> _tcs;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public AdsbVehicleServerTest(ITestOutputHelper output) : base(output)
    {
        _tcs = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _tcs.TrySetCanceled());
    }

    [Theory]
    [InlineData(uint.MaxValue)]
    [InlineData(uint.MinValue)]
    public async Task Send_SinglePacket_Success(uint icao)
    {
        // Arrange
        var expectedPackets = 1;
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
            {
                _tcs.TrySetResult(p);
            }
        );
        using var sub2 = Link.Server.OnTxMessage.Subscribe(p =>
        {
            ServerTime.Advance(TimeSpan.FromMilliseconds(100));
        });
        
        // Act
        await Server.Send(p => p.IcaoAddress = icao, _cancellationTokenSource.Token);

        // Assert
        var res = await _tcs.Task;
        Assert.NotNull(res);
        Assert.IsType<AdsbVehiclePacket>(res);
        Assert.Equal(icao, res.As<AdsbVehiclePacket>().Payload.IcaoAddress);
        Assert.Equal(expectedPackets, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(0u, Link.Server.Statistic.RxMessages);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(200)]
    [InlineData(20000)]
    public async Task Send_ManyPackets_Success(int packetsCount)
    {
        // Arrange
        var called = 0;
        var receivedPackets = new List<AdsbVehiclePayload>();
        var packetsToSend = new List<AdsbVehiclePayload>();
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            if (p is AdsbVehiclePacket packet)
            {
                receivedPackets.Add(packet.Payload);
            }
            else
            {
                throw new Exception("Not AdsbVehiclePacket. Wrong packet was received");
            }

            if (called >= packetsCount)
            {
                _tcs.TrySetResult(p);
            }
        });
        using var sub2 = Link.Server.OnTxMessage.Subscribe(p =>
        {
            ServerTime.Advance(TimeSpan.FromMilliseconds(100));
        });

        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Server.Send(payload =>
            {
                payload.IcaoAddress = (uint)i;
                packetsToSend.Add(payload);
            }, 
                _cancellationTokenSource.Token
            );
        }

        // Assert
        await _tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(packetsToSend.Count, receivedPackets.Count);
        Assert.True(receivedPackets.IsDeepEqual(packetsToSend));
        Assert.Equal(packetsCount, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(packetsCount, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0u, Link.Server.Statistic.RxMessages);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task Send_Canceled_Throws()
    {
        // Arrange
        var icao = 10u;
        
        // Act
        await _cancellationTokenSource.CancelAsync();
        var task = Server.Send(p => p.IcaoAddress = icao, _cancellationTokenSource.Token);
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0u, Link.Client.Statistic.RxMessages);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
        Assert.Equal(0u, Link.Server.Statistic.RxMessages);
    }
    
    protected override AdsbVehicleServer CreateServer(MavlinkIdentity identity, CoreServices core) =>
        new(identity, core);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}
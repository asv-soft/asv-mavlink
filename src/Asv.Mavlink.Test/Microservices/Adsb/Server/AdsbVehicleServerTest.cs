using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Test.Utils;
using Asv.Mavlink.V2.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;


[TestSubject(typeof(AsvGbsServer))]
public class AdsbVehicleServerTest : ServerTestBase<AdsbVehicleServer>, IDisposable
{
    private readonly TaskCompletionSource<IPacketV2<IPayload>> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    public AdsbVehicleServerTest(ITestOutputHelper output) : base(output)
    {
        _taskCompletionSource = new TaskCompletionSource<IPacketV2<IPayload>>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override AdsbVehicleServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);
    
    [Fact]
    public async Task Send_SinglePacket_Success()
    {
        // Arrange
        var packet = new AdsbVehiclePacket
        {
            Payload =
            {
                IcaoAddress = 123456789,
                Callsign = "testTestT".ToCharArray(),
                Lat = 51,
                Lon = -8,
                Altitude = 10000,
                Heading = 90,
                EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo,
                AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric,
                Tslc = 60,
                HorVelocity = 500,
                VerVelocity = 50,
                Flags = AdsbFlags.AdsbFlagsValidCoords,
                Squawk = 7000
            }
        };
        using var sub = Link.Client.RxPipe.Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );

        // Act
        await Server.Send(p =>
        {
            p.IcaoAddress = 123456789;
            p.Callsign = "testTestT".ToCharArray();
            p.Lat = 51;
            p.Lon = -8;
            p.Altitude = 10000;
            p.Heading = 90;
            p.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
            p.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
            p.Tslc = 60;
            p.HorVelocity = 500;
            p.VerVelocity = 50;
            p.Flags = AdsbFlags.AdsbFlagsValidCoords;
            p.Squawk = 7000;
        }, _cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task.ConfigureAwait(false) as AdsbVehiclePacket;
        Assert.NotNull(res);
        Assert.Equal(1, Link.Client.RxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.True(AdsbVehiclePacketComparer.IsEqualPayload(packet.Payload, res.Payload));
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
        using var sub = Link.Client.RxPipe.Subscribe(p =>
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
        Assert.Equal(packetsCount, Link.Server.TxPackets);
        Assert.Equal(packetsCount, Link.Client.RxPackets);
        Assert.Equal(packetsCount, results.Count);
        Assert.Equal(serverResults.Count, results.Count);
        for (var i = 0; i < results.Count; i++)
        {
            Assert.True(AdsbVehiclePacketComparer.IsEqualPayload(results[i], serverResults[i]));
        }
    }
    
    [Fact(Skip = "Cancellation doesn't work")] // TODO: FIX CANCELLATION
    public async Task Send_Canceled_Throws()
    {
        // Arrange
        AdsbVehiclePacket? packet = null;
        using var sub = Link.Client.RxPipe.Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );
    
        // Act
        await _cancellationTokenSource.CancelAsync();
        var task = Server.Send(p => p = packet.Payload, _cancellationTokenSource.Token);
    
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, Link.Client.RxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}

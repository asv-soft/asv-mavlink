using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.AsvGbs;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvGbsServer))]
public class AsvGbsServerTest : ServerTestBase<AsvGbsServer>, IDisposable
{
    private readonly AsvGbsServerConfig _serverConfig = new()
    {
        StatusRateMs = 1000
    };
    
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IAsvGbsServer _server;

    public AsvGbsServerTest(ITestOutputHelper output) : base(output)
    {
        _server = Server;
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override AsvGbsServer CreateClient(MavlinkIdentity identity, CoreServices core) =>
        new(identity, _serverConfig, core);

    [Fact]
    public async Task SendDgps_SinglePacket_Success()
    {
        // Arrange
        GpsRtcmDataPacket? packet = null;
        using var sub = Link.Client.OnRxMessage.Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );

        // Act
        await _server.SendDgps(_ => packet = _, _cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task as GpsRtcmDataPacket;
        Assert.NotNull(res);
        Assert.NotNull(packet);
        Assert.Equal(1, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.True(packet.IsDeepEqual(res));
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(200)]
    [InlineData(20000)]
    public async Task SendDgps_ManyPackets_Success(int packetsCount)
    {
        // Arrange
        var called = 0;
        var results = new List<GpsRtcmDataPacket>();
        var serverResults = new List<GpsRtcmDataPacket>();
        var tcs = new TaskCompletionSource<IProtocolMessage>();
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(packetsCount), TimeProvider.System);
        
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
        {
            called++;
            if (p is GpsRtcmDataPacket gpsPacket)
            {
                results.Add(gpsPacket);
            }

            if (called >= packetsCount)
            {
                tcs.TrySetResult(p);
            }
        });

        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await _server.SendDgps(packet => serverResults.Add(packet), cancellationTokenSource.Token);
        }

        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, (int) Link.Server.Statistic.TxMessages);
        Assert.Equal(packetsCount, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(packetsCount, results.Count);
        Assert.True(serverResults.IsDeepEqual(results));
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Set_DifferentValues_Success(bool isMax)
    {
        // Arrange
        _server.Start();
        var intValue = isMax ? int.MaxValue : int.MinValue;
        var ushortValue = isMax ? ushort.MaxValue : ushort.MinValue;
        var byteValue = isMax ? byte.MaxValue : byte.MinValue;
        using var sub = Link.Client.OnRxMessage
            .Subscribe(p =>
            {
                _taskCompletionSource.TrySetResult(p);
            });
        
        // Act
        _server.Set(pld =>
        {
            pld.Accuracy = ushortValue;
            pld.Observation = ushortValue;
            pld.DgpsRate = ushortValue;
            pld.SatAll = byteValue;
            pld.SatGal = byteValue;
            pld.SatBdu = byteValue;
            pld.SatGlo = byteValue;
            pld.SatGps = byteValue;
            pld.SatQzs = byteValue;
            pld.SatSbs = byteValue;
            pld.SatIme = byteValue;
            pld.Lat = intValue;
            pld.Lng = intValue;
            pld.Alt = intValue;
        });
        
        ServerTime.Advance(TimeSpan.FromSeconds(10));
        
        // Assert
        var status = await _taskCompletionSource.Task as AsvGbsOutStatusPacket;
        Assert.NotNull(status);
        Assert.Equal(ushortValue, status?.Payload.Accuracy);
        Assert.Equal(ushortValue, status?.Payload.Observation);
        Assert.Equal(ushortValue, status?.Payload.DgpsRate);
        Assert.Equal(byteValue, status?.Payload.SatAll);
        Assert.Equal(byteValue, status?.Payload.SatGal);
        Assert.Equal(byteValue, status?.Payload.SatBdu);
        Assert.Equal(byteValue, status?.Payload.SatGlo);
        Assert.Equal(byteValue, status?.Payload.SatGps);
        Assert.Equal(byteValue, status?.Payload.SatQzs);
        Assert.Equal(byteValue, status?.Payload.SatSbs);
        Assert.Equal(byteValue, status?.Payload.SatIme);
        Assert.Equal(intValue, status?.Payload.Lat);            
        Assert.Equal(intValue, status?.Payload.Lng);
        Assert.Equal(intValue, status?.Payload.Alt);         
    }
    
    [Fact(Skip = "Cancellation doesn't work")] // TODO: FIX CANCELLATION
    public async Task SendDgps_Canceled_Throws()
    {
        // Arrange
        GpsRtcmDataPacket? packet = null;
        using var sub = Link.Client.OnRxMessage.Subscribe(
            p => _taskCompletionSource.TrySetResult(p)
        );

        // Act
        await _cancellationTokenSource.CancelAsync();
        var task = _server.SendDgps(_ => packet = _, _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
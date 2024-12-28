using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Ex;

[TestSubject(typeof(AsvGbsExServer))]
public class AsvGbsExServerTest : ServerTestBase<AsvGbsExServer>, IDisposable
{
    private readonly AsvGbsServerConfig _config = new()
    {
        StatusRateMs = 1000
    };
    private readonly MavlinkHeartbeatServerConfig _heartbeatConfig = new()
    {
        HeartbeatRateMs = 1000
    };
    
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IAsvGbsServerEx _server;

    public AsvGbsExServerTest(ITestOutputHelper output) : base(output)
    {
        _server = Server;
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override AsvGbsExServer CreateServer(MavlinkIdentity identity, CoreServices core)
    {
        return new AsvGbsExServer(
            new AsvGbsServer(identity, _config,core),
            new HeartbeatServer(identity, _heartbeatConfig,core),
            new CommandLongServerEx(
                new CommandServer(identity, core)
            )
        );
    }
    
    [Fact]
    public async Task SendRtcmData_SinglePacket_Success()
    {
        // Arrange
        var length = AsvGbsExServer.MaxDgpsMessageLength;
        var bytes = new byte[length];
        Random.Shared.NextBytes(bytes);
        using var sub = Link.Client.OnRxMessage
            .Subscribe(p => _taskCompletionSource.TrySetResult(p));

        // Act
        await _server.SendRtcmData(bytes, length, _cancellationTokenSource.Token);

        // Assert
        var res = await _taskCompletionSource.Task as GpsRtcmDataPacket;
        var data = res?.Payload.Data;
        Assert.NotNull(res);
        Assert.Equal(1, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.True(data?.SequenceEqual(bytes));
    }
    
    [Fact]
    public async Task SendRtcmData_SinglePacketWithZeroBytes_NoDataSend()
    {
        // Arrange
        var length = 0;
        var bytes = new byte[length];

        // Act
        await _server.SendRtcmData(bytes, length, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact] 
    public async Task SendRtcmData_Canceled_Throws()
    {
        // Arrange
        var length = AsvGbsExServer.MaxDgpsMessageLength;
        var bytes = new byte[length];

        // Act
        await _cancellationTokenSource.CancelAsync();
        var task = _server.SendRtcmData(bytes, length, _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task SendRtcmData_MaxRtcmMessageLength_Success()
    {
        // Arrange
        var length = AsvGbsExServer.MaxRtcmMessageLength;
        var bytes = new byte[length];
        Random.Shared.NextBytes(bytes);
        var results = new List<GpsRtcmDataPacket>();
        var called = 0;
        using var sub = Link.Client.OnRxMessage
            .Subscribe(p =>
            {
                called++;
                if (p is GpsRtcmDataPacket gpsPacket)
                {
                    results.Add(gpsPacket);
                }

                if (called >= AsvGbsExServer.MaxRtcmMessageLength 
                    / AsvGbsExServer.MaxDgpsMessageLength)
                {
                    _taskCompletionSource.TrySetResult(p);
                }
            });

        // Act
        await _server.SendRtcmData(bytes, length, _cancellationTokenSource.Token);

        // Assert
        await _taskCompletionSource.Task;
        var receivedBytes = results.SelectMany(x => x.Payload.Data).ToArray();
        
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.Equal(bytes, receivedBytes);
    }
    
    [Fact]
    public async Task SendRtcmData_MessageTooBig_NoDataSend()
    {
        // Arrange
        var length = AsvGbsExServer.MaxRtcmMessageLength + 1;
        var bytes = new byte[length];
        Random.Shared.NextBytes(bytes);

        // Act
        await _server.SendRtcmData(bytes, length, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(2000)]
    public async Task SendRtcmData_SeveralMessages_Success(int callsCount)
    {
        // Arrange
        var results = new List<GpsRtcmDataPacket>();
        var sendByteArrays = new List<byte[]>();
        var called = 0;
        using var sub = Link.Client.OnRxMessage
            .Subscribe(p =>
            {
                called++;
                if (p is GpsRtcmDataPacket gpsPacket)
                {
                    results.Add(gpsPacket);
                }

                if (called >= callsCount)
                {
                    _taskCompletionSource.TrySetResult(p);
                }
            });

        // Act
        for (var i = 0; i < callsCount; i++)
        {
            var length = Random.Shared.Next(1, AsvGbsExServer.MaxDgpsMessageLength);
            var bytes = new byte[length];
            Random.Shared.NextBytes(bytes);
            var extendedBytes = new byte[AsvGbsExServer.MaxDgpsMessageLength];
            Array.Copy(bytes, extendedBytes, bytes.Length);
            sendByteArrays.Add(extendedBytes);
            await Server.SendRtcmData(bytes, length, _cancellationTokenSource.Token);
        }

        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(called, (int) Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.Equal(results.Count, sendByteArrays.Count);
        for (var i = 0; i < results.Count; i++)
        {
            Assert.Equal(results[i].Payload.Data, sendByteArrays[i]);
        }
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
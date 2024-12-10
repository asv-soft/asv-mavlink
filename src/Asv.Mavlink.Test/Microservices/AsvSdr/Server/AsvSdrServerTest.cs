using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.AsvSdr;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrServer))]
public class AsvSdrServerTest : ServerTestBase<AsvSdrServer>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly AsvSdrServerConfig _config = new()
    {
        StatusRateMs = 1000
    };

    protected override AsvSdrServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, _config, core);

    public AsvSdrServerTest(ITestOutputHelper log) : base(log)
    {
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(25));
    }

    [Fact]
    public async Task SendRecord_WhenCalled_ShouldTransmitRecordSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        var name = "test".ToCharArray();
        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p => 
            tcs.TrySetResult(p));
        
        // Act
        await Server.SendRecord(p =>p.RecordName = name, _cancellationTokenSource.Token);
        
        // Assert
        var result = await tcs.Task as AsvSdrRecordPacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(name.ToString(), result?.Payload.RecordName.ToString());
    }

    [Fact]
    public async Task SendSignal_WhenCalled_ShouldTransmitSignalSuccessfully()
    {
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p => 
            tcs.TrySetResult(p));
        
        await Server.SendSignal(packet => {}, _cancellationTokenSource.Token);
        
        var result = await tcs.Task as AsvSdrSignalRawPacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Theory]
    [InlineData(AsvSdrRequestAck.AsvSdrRequestAckOk)]
    [InlineData(AsvSdrRequestAck.AsvSdrRequestAckInProgress)]
    [InlineData(AsvSdrRequestAck.AsvSdrRequestAckFail)]
    [InlineData(AsvSdrRequestAck.AsvSdrRequestAckNotSupported)]
    public async Task SendCalibrationAcc_WhenCalled_ShouldTransmitCalibrationAck(AsvSdrRequestAck ack)
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p => 
            tcs.TrySetResult(p));
        
        // Act
        await Server.SendCalibrationAcc(1, ack, _cancellationTokenSource.Token);
        
        // Assert
        var result = await tcs.Task as AsvSdrCalibAccPacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.True(result?.Payload.Result == ack);
    }
    
    [Fact]
    public async Task SendCalibrationTableReadResponse_WhenCalled_ShouldTransmitTableSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        var name = "test".ToCharArray();
        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p => 
            tcs.TrySetResult(p));
        
        // Act
        await Server.SendCalibrationTableReadResponse(p => p.TableName = name, _cancellationTokenSource.Token);
        
        // Assert
        var result = await tcs.Task as AsvSdrCalibTablePacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(name.ToString(), result?.Payload.TableName.ToString());
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(200)]
    [InlineData(20000)]
    public async Task SendRecord_WhenCalledManyTimes_ShouldTransmitAllRecordsSuccessfully(int packetCount)
    {
        // Arrange
        var called = 0;
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        var results = new List<AsvSdrRecordPayload>();
        var serverResults = new List<AsvSdrRecordPayload>();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
        {
            called++;
            if (p is AsvSdrRecordPacket packet)
            {
                results.Add(packet.Payload);
            }

            if (called >= packetCount)
            {
                tcs.TrySetResult(p);
            }
        });

        // Act
        for (var i = 0; i < packetCount; i++)
        {
            await Server.SendRecord(p =>
            {
                p.RecordName = $"packet{i}".ToCharArray();
                serverResults.Add(p);
            }, _cancellationTokenSource.Token);
        }

        // Assert
        await tcs.Task;
        Assert.Equal(packetCount, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(packetCount, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(packetCount, results.Count);
        Assert.Equal(serverResults.Count, results.Count);
        for (var i = 0; i < results.Count; i++)
        {
            Assert.True(results[i].IsDeepEqual(serverResults[i]));
        }
    }
    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
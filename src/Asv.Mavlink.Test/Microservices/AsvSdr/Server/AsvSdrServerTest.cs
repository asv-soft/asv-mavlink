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

    protected override AsvSdrServer CreateClient(MavlinkIdentity identity, CoreServices core) =>
        new(identity, _config, core);

    public AsvSdrServerTest(ITestOutputHelper log) : base(log)
    {
            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(25));
    }

    #region Record

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
        await Server.SendRecord(p => p.RecordName = name, _cancellationTokenSource.Token);

        // Assert
        var result = await tcs.Task as AsvSdrRecordPacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(name.ToString(), result?.Payload.RecordName.ToString());
    }
    
    [Fact]
    public async Task SendRecordResponse_WhenCalled_ShouldTransmitRecordResponseSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
            tcs.TrySetResult(p));

        // Act
        await Server.SendRecordResponse(p => { }, _cancellationTokenSource.Token);

        // Assert
        var result = await tcs.Task as AsvSdrRecordResponsePacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.True(result?.Payload.Result == AsvSdrRequestAck.AsvSdrRequestAckOk);
    }
    
    [Fact]
    public async Task SendRecordDeleteResponse_WhenCalled_ShouldTransmitDeleteResponseSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
            tcs.TrySetResult(p));

        // Act
        await Server.SendRecordDeleteResponse(p => p.Result = AsvSdrRequestAck.AsvSdrRequestAckOk, _cancellationTokenSource.Token);

        // Assert
        var result = await tcs.Task as AsvSdrRecordDeleteResponsePacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.True(result?.Payload.Result == AsvSdrRequestAck.AsvSdrRequestAckOk);
    }
    
    [Fact]
    public async Task SendRecordTagResponse_WhenCalled_ShouldTransmitTagResponseSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
            tcs.TrySetResult(p));

        // Act
        await Server.SendRecordTagResponse(p => p.Result = AsvSdrRequestAck.AsvSdrRequestAckOk, _cancellationTokenSource.Token);

        // Assert
        var result = await tcs.Task as AsvSdrRecordTagResponsePacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.True(result?.Payload.Result == AsvSdrRequestAck.AsvSdrRequestAckOk);
    }
    
    [Fact]
    public async Task SendRecordTag_WhenCalled_ShouldTransmitRecordTagSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        var tagName = "SampleTag".ToCharArray();
        var recordId = new byte[16];

        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
            tcs.TrySetResult(p));

        // Act
        await Server.SendRecordTag(p => { }, _cancellationTokenSource.Token);

        // Assert
        var result = await tcs.Task as AsvSdrRecordTagPacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(tagName.ToString(), result?.Payload.TagName.ToString());
        Assert.Equal(recordId, result?.Payload.RecordGuid);
    }

    [Fact]
    public async Task SendRecordTagDeleteResponse_WhenCalled_ShouldTransmitTagDeleteResponseSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();

        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
            tcs.TrySetResult(p));

        // Act
        await Server.SendRecordTagDeleteResponse(p => p.Result = AsvSdrRequestAck.AsvSdrRequestAckOk, _cancellationTokenSource.Token);

        // Assert
        var result = await tcs.Task as AsvSdrRecordTagDeleteResponsePacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.True(result?.Payload.Result == AsvSdrRequestAck.AsvSdrRequestAckOk);
    }
    
    [Fact]
    public async Task SendRecordDataResponse_WhenCalled_ShouldTransmitDataResponseSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        var dataId = (ushort)202;
        var dataCount = 2U;

        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
            tcs.TrySetResult(p));

        // Act
        await Server.SendRecordDataResponse(p =>
        {
            p.RequestId = dataId;
            p.ItemsCount = dataCount;
        }, _cancellationTokenSource.Token);

        // Assert
        var result = await tcs.Task as AsvSdrRecordDataResponsePacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(dataId, result?.Payload.RequestId);
        Assert.Equal(dataCount, result?.Payload.ItemsCount);
    }
    
    [Fact(Skip = "test fails in method")] //TODO: test fails in method
    public async Task SendRecordData_WhenCalled_ShouldTransmitRecordDataSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();

        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
            tcs.TrySetResult(p));

        // Act
        await Server.SendRecordData(AsvSdrCustomMode.AsvSdrCustomModeGp, p => {}, _cancellationTokenSource.Token);

        // Assert
        var result = await tcs.Task as AsvSdrRecordDataGpPacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    #endregion
    
    #region Signal
    
    [Fact] 
    public async Task SendSignal_WhenCalled_ShouldTransmitSignalSuccessfully()
    {
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
            tcs.TrySetResult(p));

        await Server.SendSignal(packet => { }, _cancellationTokenSource.Token);

        var result = await tcs.Task as AsvSdrSignalRawPacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    #endregion
    
    #region Calibration
    
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
    public async Task SendCalibrationTableReadResponse_WhenCalled_ShouldTransmitTableReadResponseSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        var tableIndex = (ushort)5;
        var rowCount = (ushort)20;

        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
            tcs.TrySetResult(p));

        // Act
        await Server.SendCalibrationTableReadResponse(p =>
        {
            p.TableIndex = tableIndex;
            p.RowCount = rowCount;
        }, _cancellationTokenSource.Token);

        // Assert
        var result = await tcs.Task as AsvSdrCalibTablePacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(tableIndex, result?.Payload.TableIndex);
        Assert.Equal(rowCount, result?.Payload.RowCount);
    }

    [Fact]
    public async Task SendCalibrationTableRowReadResponse_WhenCalled_ShouldTransmitTableRowReadResponseSuccessfully()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        var tableIndex = (ushort)2;
        var rowIndex = (ushort)7;

        Server.Start();
        using var sub = Link.Client.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
            tcs.TrySetResult(p));

        // Act
        await Server.SendCalibrationTableRowReadResponse(p =>
        {
            p.TableIndex = tableIndex;
            p.RowIndex = rowIndex;
        }, _cancellationTokenSource.Token);

        // Assert
        var result = await tcs.Task as AsvSdrCalibTableRowPacket;
        Assert.NotNull(result);
        Assert.Equal(1U, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(tableIndex, result?.Payload.TableIndex);
        Assert.Equal(rowIndex, result?.Payload.RowIndex);
    }
    
    #endregion
    
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
                var buffer = new char[28];
                var name = $"packet{i}".ToCharArray();
                Array.Copy(name, buffer, name.Length);
                p.RecordName = buffer;
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
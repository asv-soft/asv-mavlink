using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.AsvChart;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvChartServer))]
public class AsvChartServerTest : ServerTestBase<AsvChartServer>
{
    private readonly TaskCompletionSource<IProtocolMessage> _tcs;
    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly AsvChartServerConfig _config = new()
    {
        SendSignalDelayMs = 0, // zero because we want no delays in most cases
        SendCollectionUpdateMs = 100,
    };

    public AsvChartServerTest(ITestOutputHelper output) : base(output)
    {
        _tcs = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _tcs.TrySetCanceled());
    }

    [Fact]
    public async Task Send_SinglePacket_Success()
    {
        // Arrange
        var count = 0;
        var payload = new AsvChartInfoPayload();
        var axisX = new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10);
        var axisY = new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10);
        var info = new AsvChartInfo(
            1, 
            "TestChart", 
            axisX,
            axisY,
            AsvChartDataFormat.AsvChartDataFormatFloat
        );
        info.Fill(payload);
        var expectedPacketsCount = (int) Math.Ceiling(info.OneFrameByteSize / (double)AsvChartDataPayload.DataMaxItemsCount);
        var receivedPackets = new List<AsvChartDataPacket>();

        var data = new ReadOnlyMemory<float>(new float[info.OneFrameMeasureSize]);
        using var sub = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            count++;

            if (p is not AsvChartDataPacket dataPacket)
            {
                throw new InvalidCastException($"Packet should be of type {typeof(AsvChartDataPacket)}");
            }
            
            receivedPackets.Add(dataPacket);

            if (count == expectedPacketsCount)
            {
                _tcs.TrySetResult(p);
            }
        });
        using var sub1 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            ServerTime.Advance(TimeSpan.FromMilliseconds(100));
        });

        // Act
        await Server.Send(ServerTime.GetUtcNow().DateTime, data, info, _cancellationTokenSource.Token);

        // Assert
        Assert.True(receivedPackets.All(p => p.Payload.ChatInfoHash == payload.ChartInfoHash));
        Assert.Equal(expectedPacketsCount, count);
        Assert.Equal(count, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Server.Statistic.RxMessages);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task Send_SinglePacketWithWrongDatatype_ThrowsException()
    {
        // Arrange
        var axisX = new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10);
        var axisY = new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10);
        var info = new AsvChartInfo(
            1, 
            "TestChart",
            axisX,
            axisY,
            AsvChartDataFormat.AsvChartDataFormatFloat
        );
        var payload = new AsvChartInfoPayload();
        info.Fill(payload);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await Server.Send(
                ServerTime.GetUtcNow().DateTime, 
                new ReadOnlyMemory<float>(new float[info.OneFrameMeasureSize + 1]),
                info, 
                _cancellationTokenSource.Token
            );
        });
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await Server.Send(
                ServerTime.GetUtcNow().DateTime, 
                new ReadOnlyMemory<float>(new float[info.OneFrameByteSize + 1]), 
                info,
                _cancellationTokenSource.Token
            );
        });
    }

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task Send_ManyPacket_Success(int packetsCount)
    {
        // Arrange
        var called = 0;
        var info = new AsvChartInfo(1, "TestChart",
            new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, 0, 10, 10),
            new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, 0, 10, 10),
            AsvChartDataFormat.AsvChartDataFormatFloat);
        var data = new ReadOnlyMemory<float>(new float[info.OneFrameMeasureSize]);
        var receivedPackets = new List<AsvChartDataPacket>();
        var packetsSplitCount =
            (int)Math.Ceiling(info.OneFrameByteSize / (double)AsvChartDataPayload.DataMaxItemsCount);
        var expectedPacketsCount = packetsCount * packetsSplitCount;
        using var sub = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            called++;
            if (p is not AsvChartDataPacket infoPacket)
            {
                throw new InvalidCastException($"Packet should be of type {typeof(AsvChartDataPacket)}");
            }
            
            receivedPackets.Add(infoPacket);
            if (called == expectedPacketsCount)
            {
                _tcs.TrySetResult(p);
            }
        });
        using var sub1 = Link.Server.OnTxMessage.Synchronize().Subscribe(p =>
        {
            ServerTime.Advance(TimeSpan.FromMilliseconds(100));
        });

        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Server.Send(ServerTime.GetUtcNow().DateTime, data, info);
        }

        // Assert
        await _tcs.Task;
        Assert.True(receivedPackets.All(p => p.Payload.ChatInfoHash == info.InfoHash));
        Assert.Equal(expectedPacketsCount, called);
        Assert.Equal(called, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
        Assert.Equal(0u, Link.Server.Statistic.RxMessages);
    }
    
    protected override AsvChartServer CreateServer(MavlinkIdentity identity, CoreServices core) =>
        new(identity, _config, core);


    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}
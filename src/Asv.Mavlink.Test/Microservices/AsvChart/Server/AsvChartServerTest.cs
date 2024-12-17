using System;
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
public class AsvChartServerTest : ServerTestBase<AsvChartServer>, IDisposable
{
    private readonly TaskCompletionSource<IProtocolMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly AsvChartServerConfig _config = new()
    {
        SendSignalDelayMs = 30,
        SendCollectionUpdateMs = 100,
    };

    public AsvChartServerTest(ITestOutputHelper output) : base(output)
    {
        _taskCompletionSource = new TaskCompletionSource<IProtocolMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
    }


    protected override AsvChartServer CreateClient(MavlinkIdentity identity, CoreServices core) =>
        new(identity, _config, core);


    [Fact]
    public async Task Send_SendSinglePacket_Success()
    {
        // Arrange
        var count = 0;
        var payload = new AsvChartInfoPayload();

        var info = new AsvChartInfo(1, "TestChart",
            new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            AsvChartDataFormat.AsvChartDataFormatFloat);
        info.Fill(payload);

        var data = new ReadOnlyMemory<float>(new float[info.OneFrameMeasureSize]);
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
        {
            count++;
            _taskCompletionSource.TrySetResult(p);
        });

        // Act
        await Server.Send(DateTime.Now, data, info, _cancellationTokenSource.Token);

        // Assert
        var result = await _taskCompletionSource.Task as AsvChartDataPacket;
        Assert.NotNull(result);
        Assert.Equal(payload.ChartInfoHash, result?.Payload.ChatInfoHash);
        Assert.Equal(count, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task Send_SendSinglePacket_ThrowException()
    {
        // Arrange
        var info = new AsvChartInfo(1, "TestChart",
            new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            AsvChartDataFormat.AsvChartDataFormatFloat);
        var payload = new AsvChartInfoPayload();
        info.Fill(payload);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await Server.Send(DateTime.Now, new ReadOnlyMemory<float>(new float[info.OneFrameMeasureSize + 1]),
                info, _cancellationTokenSource.Token);
        });
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await Server.Send(DateTime.Now, new ReadOnlyMemory<float>(new float[info.OneFrameByteSize + 1]), info,
                _cancellationTokenSource.Token);
        });
    }

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task Send_SendManyPacket_Success(int packetsCount)
    {
        // Arrange
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        cancel.Token.Register(() => _taskCompletionSource.TrySetCanceled());

        var count = 0;
        var info = new AsvChartInfo(1, "TestChart",
            new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, 0, 10, 10),
            new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, 0, 10, 10),
            AsvChartDataFormat.AsvChartDataFormatFloat);
        var data = new ReadOnlyMemory<float>(new float[info.OneFrameMeasureSize]);
        using var sub = Link.Client.OnRxMessage.Subscribe(p =>
        {
            count++;
            _taskCompletionSource.TrySetResult(p);
        });

        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Server.Send(DateTime.Now, data, info, cancel.Token);
        }

        // Assert
        Assert.Equal(count, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }


    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
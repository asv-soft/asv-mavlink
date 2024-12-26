using System;
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

[TestSubject(typeof(AsvChartClient))]
[TestSubject(typeof(AsvChartServer))]
public class AsvChartComplexTest : ComplexTestBase<AsvChartClient, AsvChartServer>, IDisposable
{
    private readonly TaskCompletionSource<AsvChartInfo> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public AsvChartComplexTest(ITestOutputHelper log) : base(log)
    {
        _taskCompletionSource = new TaskCompletionSource<AsvChartInfo>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private readonly AsvChartServerConfig _serverConfig = new()
    {
        SendSignalDelayMs = 30,
        SendCollectionUpdateMs = 100,
    };

    private readonly AsvChartClientConfig _clientConfig = new()
    {
        MaxTimeToWaitForResponseForListMs = 1000,
    };

    protected override AsvChartServer CreateServer(MavlinkIdentity identity, IMavlinkContext core) =>
        new(identity, _serverConfig, core);

    protected override AsvChartClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core) =>
        new(identity, _clientConfig, core);

    [Fact]
    public async Task ReadAllInfo_WhenCalled_ShouldSynchronizeServerAndClientCharts()
    {
        // Arrange
        Server.Charts.Add(1, new AsvChartInfo(1, "TestChart",
            new AsvChartAxisInfo("dBm", AsvChartUnitType.AsvChartUnitTypeCustom, float.MinValue, 0f, int.MaxValue),
            new AsvChartAxisInfo("Hz", AsvChartUnitType.AsvChartUnitTypeCustom, 0f, float.MaxValue, int.MaxValue),
            AsvChartDataFormat.AsvChartDataFormatFloat));
        Server.Charts.Add(2, new AsvChartInfo(2, "TestChart",
            new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, float.MinValue, float.MaxValue, int.MaxValue),
            new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, float.MinValue, float.MaxValue, int.MaxValue),
            AsvChartDataFormat.AsvChartDataFormatFloat));
        using var sub = Client.OnChartInfo.Subscribe(
            p => _taskCompletionSource.TrySetResult(p));

        // Act
        var sync = await Client.ReadAllInfo(default, _cancellationTokenSource.Token);
        Assert.True(sync);

        // Assert
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(Server.Charts.Count, Client.Charts.Count);
    }

    [Fact]
    public async Task RequestStream_WhenSingleRequest_ShouldReturnStreamAndUpdateCharts()
    {
        // Arrange
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource.Token.Register(() => tcs.TrySetCanceled());

        var info = new AsvChartInfo(1, "TestChart1",
            new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit);
        var option = new AsvChartOptions(1, AsvChartDataTrigger.AsvChartDataTriggerPeriodic, 30);
        Server.Charts.Add(1, info);

        Random r = new Random();
        var array = Enumerable.Range(0, info.AxisX.Size).Select(_ => r.NextSingle()).ToArray();
        var data = new ReadOnlyMemory<float>(array);

        Server.OnDataRequest = (options, infoChart, cancel) =>
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    await Server.Send(DateTime.Now, data, info, _cancellationTokenSource.Token);
                }
            }, cancel);
            return Task.FromResult(new AsvChartOptions(infoChart.Id, AsvChartDataTrigger.AsvChartDataTriggerPeriodic,
                30));
        };
        using var sub = Link.Client.OnRxMessage.FilterByType<MavlinkMessage>().Subscribe(_ =>
        {
            if (_ is AsvChartDataResponsePacket)
            {
                tcs.TrySetResult(_);
            }
        });

        // Act
        var sync = await Client.ReadAllInfo(default, _cancellationTokenSource.Token);

        // Assert
        Assert.True(sync);
        Assert.Equal(Server.Charts.Count, Client.Charts.Count);

        // Act
        var stream = await Client.RequestStream(option, _cancellationTokenSource.Token);

        // Assert
        var res = await tcs.Task as AsvChartDataResponsePacket;
        Assert.NotNull(res);
        Assert.NotNull(stream);
        Assert.Equal(res?.Payload.ChartId, stream.ChartId);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task RequestStream_WhenMultipleRequests_ShouldHandleAllAndUpdateCharts()
    {
        // Arrange
        var tcsFirst = new TaskCompletionSource<AsvChartDataResponsePacket>();
        var tcsSecond = new TaskCompletionSource<AsvChartDataResponsePacket>();
        _cancellationTokenSource.Token.Register(() =>
        {
            tcsFirst.TrySetCanceled();
            tcsSecond.TrySetCanceled();
        });

        var infoFirst = new AsvChartInfo(1, "TestChart1",
            new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit);
        var infoSecond = new AsvChartInfo(2, "TestChart2",
            new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 100),
            AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit);
        var optionFirst = new AsvChartOptions(1, AsvChartDataTrigger.AsvChartDataTriggerPeriodic, 30);
        var optionSecond = new AsvChartOptions(2, AsvChartDataTrigger.AsvChartDataTriggerPeriodic, 30);

        Server.Charts.Add(1, infoFirst);
        Server.Charts.Add(2, infoSecond);

        Random random = new();
        var arrayFirst = Enumerable.Range(0, infoFirst.AxisX.Size).Select(_ => random.NextSingle()).ToArray();
        var dataFirst = new ReadOnlyMemory<float>(arrayFirst);
        var arraySecond = Enumerable.Range(0, infoSecond.AxisY.Size).Select(_ => random.NextSingle()).ToArray();
        var dataSecond = new ReadOnlyMemory<float>(arraySecond);

        Server.OnDataRequest = (options, infoChart, cancel) =>
        {
            _ = Task.Run(async () =>
            {
                while (!cancel.IsCancellationRequested)
                {
                    if (infoChart.Id == 1)
                    {
                        await Server.Send(DateTime.Now, dataFirst, infoFirst, cancel);
                    }
                    else if (infoChart.Id == 2)
                    {
                        await Server.Send(DateTime.Now, dataSecond, infoSecond, cancel);
                    }
                }
            }, cancel);

            return Task.FromResult(new AsvChartOptions(infoChart.Id, AsvChartDataTrigger.AsvChartDataTriggerPeriodic,
                30));
        };

        using var sub = Link.Client.OnRxMessage.Subscribe(packet =>
        {
            if (packet is AsvChartDataResponsePacket response)
            {
                if (response.Payload.ChartId == 1)
                {
                    tcsFirst.TrySetResult(response);
                }
                else if (response.Payload.ChartId == 2)
                {
                    tcsSecond.TrySetResult(response);
                }
            }
        });

        // Act
        var sync = await Client.ReadAllInfo(default, _cancellationTokenSource.Token);

        // Assert
        Assert.True(sync);
        Assert.Equal(Server.Charts.Count, Client.Charts.Count);

        // Act
        var firstStream = await Client.RequestStream(optionFirst, _cancellationTokenSource.Token);
        var secondStream = await Client.RequestStream(optionSecond, _cancellationTokenSource.Token);

        // Assert
        var resFirst = await tcsFirst.Task;
        var resSecond = await tcsSecond.Task;

        Assert.NotNull(firstStream);
        Assert.NotNull(secondStream);
        Assert.NotNull(resFirst);
        Assert.NotNull(resSecond);
        Assert.Equal(resFirst.Payload.ChartId, firstStream.ChartId);
        Assert.Equal(resSecond.Payload.ChartId, secondStream.ChartId);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
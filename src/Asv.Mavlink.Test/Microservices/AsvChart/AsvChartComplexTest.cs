using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.AsvChart;
using DeepEqual.Syntax;
using FluentAssertions;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvChartClient))]
[TestSubject(typeof(AsvChartServer))]
public class AsvChartComplexTest : ComplexTestBase<AsvChartClient, AsvChartServer>
{
    private readonly TaskCompletionSource _tcs;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    private readonly AsvChartServerConfig _serverConfig = new()
    {
        SendSignalDelayMs = 0,
        SendCollectionUpdateMs = 100,
    };

    private readonly AsvChartClientConfig _clientConfig = new()
    {
        MaxTimeToWaitForResponseForListMs = 1000,
    };

    public AsvChartComplexTest(ITestOutputHelper log) : base(log)
    {
        _tcs = new TaskCompletionSource();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _tcs.TrySetCanceled());
    }

    [Fact]
    public async Task ReadAllInfo_SynchronizeServerAndClientCharts_Success()
    {
        // Arrange
        const int expectedInfoPackets = 2;
        const int expectedPacketsFromServer = 4;
        const int expectedPacketsFromClient = 1;
        
        var totalPacketsReceivedFromServer = 0;
        var infoPacketsCount = 0;
        Server.Charts.Add(1, new AsvChartInfo(1, "TestChart",
            new AsvChartAxisInfo("dBm", AsvChartUnitType.AsvChartUnitTypeCustom, float.MinValue, 0f, 20),
            new AsvChartAxisInfo("Hz", AsvChartUnitType.AsvChartUnitTypeCustom, 0f, float.MaxValue, 50),
            AsvChartDataFormat.AsvChartDataFormatFloat));
        Server.Charts.Add(2, new AsvChartInfo(2, "TestChart",
            new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, float.MinValue, float.MaxValue, 24),
            new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, float.MinValue, float.MaxValue, 77),
            AsvChartDataFormat.AsvChartDataFormatFloat));
        using var sub = Client.OnChartInfo.Synchronize().Subscribe(
            _ => infoPacketsCount++);
        using var sub2 = Link.Client.OnRxMessage.Synchronize().Subscribe(p =>
        {
            totalPacketsReceivedFromServer++;
            Log.WriteLine("Packet on rx client {0}", p.Name);
            
            if (totalPacketsReceivedFromServer == expectedPacketsFromServer)
            {
                _tcs.TrySetResult();
            }
        });

        // Act
        var sync = await Client.ReadAllInfo(null, _cancellationTokenSource.Token);

        // Assert
        await _tcs.Task;
        Assert.True(sync);
        Assert.Equal(Server.Charts.Count, Client.Charts.Count);
        Server.Charts.Should().BeEquivalentTo(Client.Charts);
        Assert.Equal(expectedPacketsFromServer, totalPacketsReceivedFromServer);
        Assert.Equal(expectedInfoPackets, infoPacketsCount);
        Assert.Equal(expectedPacketsFromServer, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(expectedPacketsFromClient, (int) Link.Server.Statistic.RxMessages);
        Assert.Equal(expectedPacketsFromClient, (int) Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task RequestStream_WhenSingleRequest_ShouldReturnStreamAndUpdateCharts()
    {
        // Arrange
        const ushort id = 1;
        const string signalName = "TestChart1";
        const AsvChartDataFormat dataFormat = AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit;
        const AsvChartUnitType dataUnitType = AsvChartUnitType.AsvChartUnitTypeDbm;
        const string AxisXName = "X";
        const string AxisYName = "Y";
        
        var tcs = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource.Token.Register(() => tcs.TrySetCanceled());
        
        int count = 0;
        int expectedCount = 5;
        
        var info = new AsvChartInfo(
            id, 
            signalName,
            new AsvChartAxisInfo(AxisXName, dataUnitType, 0f, 10f, 10),
            new AsvChartAxisInfo(AxisYName, dataUnitType, 0f, 10f, 10),
            dataFormat
        );
        var option = new AsvChartOptions(1, AsvChartDataTrigger.AsvChartDataTriggerPeriodic, 30);
        Server.Charts.Add(1, info);
        var sync = await Client.ReadAllInfo(null, _cancellationTokenSource.Token);
        if (!sync)
        {
            throw new Exception("Can't sync charts");
        }
        
        var chartsToSend = new List<AsvChartInfo>
        {
            info,
            info,
            info,
            info,
            info,
        };
        
        var dataReceived = new List<AsvChartInfo>();

        Server.OnDataRequest = async (options, infoChart, cancel) =>
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < expectedCount; i++)
                {
                    var data = new ReadOnlyMemory<float>(new float[infoChart.OneFrameMeasureSize]);
                    await Server.Send(ServerTime.GetUtcNow().DateTime, data, infoChart, _cancellationTokenSource.Token);
                    ServerTime.Advance(TimeSpan.FromMilliseconds(_serverConfig.SendSignalDelayMs + 1));
                }
            }, cancel);
            
            return options;
        };

        Client.OnDataReceived = (time, memory, chartInfo) =>
        {
            dataReceived.Add(chartInfo);
            count++;
            if (count == expectedCount)
            {
                _tcs.TrySetResult();
            }
        };
        
        using var sub1 = Link.Client.OnRxMessage.FilterByType<MavlinkMessage>().Subscribe(_ =>
        {
            if (_ is AsvChartDataResponsePacket)
            {
                tcs.TrySetResult(_);
            }
        });

        // Act
        var finalOptions = await Client.RequestStream(option, _cancellationTokenSource.Token);

        // Assert
        await _tcs.Task;
        Assert.NotNull(finalOptions); 
        Assert.True(chartsToSend.IsDeepEqual(dataReceived));
        //  Assert.Equal(res.Payload.ChatId, finalOptions.ChartId);
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
                if (response.Payload.ChatId == 1)
                {
                    tcsFirst.TrySetResult(response);
                }
                else if (response.Payload.ChatId == 2)
                {
                    tcsSecond.TrySetResult(response);
                }
            }
        });

        // Act
        var sync = await Client.ReadAllInfo(null, _cancellationTokenSource.Token);

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
        Assert.Equal(resFirst.Payload.ChatId, firstStream.ChartId);
        Assert.Equal(resSecond.Payload.ChatId, secondStream.ChartId);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    protected override AsvChartServer CreateServer(MavlinkIdentity identity, IMavlinkContext core) =>
        new(identity, _serverConfig, core);

    protected override AsvChartClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core) =>
        new(identity, _clientConfig, core);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}
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
    public async Task RequestStream_TransferDataByStreamPeriodic_Success()
    {
        // Arrange
        const int min = 0;
        const int max = 10;
        const int expectedCount = 5;
        const AsvChartUnitType dataUnitType = AsvChartUnitType.AsvChartUnitTypeCustom;
        
        var count = 0;
        var dataToSend = new List<float[]>();
        var dataReceived = new List<float[]>();
        
        var info = new AsvChartInfo(
            1, 
            "TestChart1",
            new AsvChartAxisInfo("X", dataUnitType, min, max, 10),
            new AsvChartAxisInfo("Y", dataUnitType, min, max, 10),
            AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit
        );
        var options = new AsvChartOptions(1, AsvChartDataTrigger.AsvChartDataTriggerPeriodic, 0);
        Server.Charts.Add(1, info);
        var sync = await Client.ReadAllInfo(null, _cancellationTokenSource.Token);
        if (!sync)
        {
            throw new Exception("Can't sync charts");
        }

        Server.OnDataRequest = (opts, infoChart, cancel) =>
        {
            Task.Run(async () =>
            {
                for (int i = 0; i < expectedCount; i++)
                {
                    var array = Enumerable
                        .Range(0, infoChart.OneFrameMeasureSize)
                        .Select(_ => (float) Random.Shared.Next(min, max))
                        .ToArray();
                    var data = new ReadOnlyMemory<float>(array);
                    dataToSend.Add(data.Span.ToArray());
                    await Server.Send(ServerTime.GetUtcNow().DateTime, data, infoChart, cancel);
                    ServerTime.Advance(TimeSpan.FromMilliseconds(_serverConfig.SendSignalDelayMs + 1));
                }
            }, cancel);
            
            return Task.FromResult(opts);
        };

        Client.OnDataReceived = (time, memory, chartInfo) =>
        {
            dataReceived.Add(memory.Span.ToArray().Select(_ => MathF.Round(_)).ToArray());
            count++;
            if (count == expectedCount)
            {
                _tcs.TrySetResult();
            }
        };

        // Act
        var finalOptions = await Client.RequestStream(options, _cancellationTokenSource.Token);

        // Assert
        await _tcs.Task;
        Assert.NotNull(finalOptions); 
        finalOptions.Should().BeEquivalentTo(options);
        dataReceived.Should().BeEquivalentTo(dataToSend);
        Assert.Equal(expectedCount, count);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact(Skip = "Test doesn't work. Something is wrong with the crc of the packets")]
    public async Task RequestStream_TransferDataOfTwoChartsByStream_Success()
    {
        // Arrange
        const int expectedCount = 5;
        const AsvChartUnitType dataUnitType = AsvChartUnitType.AsvChartUnitTypeCustom;
        
        var tcs1 = new TaskCompletionSource();
        var tcs2 = new TaskCompletionSource();
        _cancellationTokenSource.Token.Register(() =>
        {
            tcs1.TrySetCanceled();
            tcs2.TrySetCanceled();
        });
        
        var dataToSend1 = new List<float[]>();
        var dataToSend2 = new List<float[]>();
        var dataReceived1 = new List<float[]>();
        var dataReceived2 = new List<float[]>();
        
        var info1 = new AsvChartInfo(
            1, 
            "TestChart1",
            new AsvChartAxisInfo("X", dataUnitType, 0f, 10f, 10),
            new AsvChartAxisInfo("Y", dataUnitType, 0f, 10f, 10),
            AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit
        );
        var info2 = new AsvChartInfo(
            2, 
            "TestChart2",
            new AsvChartAxisInfo("X", dataUnitType, 0f, 10f, 10),
            new AsvChartAxisInfo("Y", dataUnitType, 0f, 10f, 10),
            AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit
        );
        var options1 = new AsvChartOptions(1, AsvChartDataTrigger.AsvChartDataTriggerPeriodic, 0);
        var options2 = new AsvChartOptions(2, AsvChartDataTrigger.AsvChartDataTriggerPeriodic, 0);
        Server.Charts.Add(1, info1);
        Server.Charts.Add(2, info2);
        var sync = await Client.ReadAllInfo(null, _cancellationTokenSource.Token);
        if (!sync)
        {
            throw new Exception("Can't sync charts");
        }

        Server.OnDataRequest = (opts, infoChart, cancel) =>
        {
            Task.Run(async () =>
            {
                for (int i = 0; i < expectedCount; i++)
                {
                    if (opts.ChartId == 1 && infoChart.Id == 1)
                    {
                        var array = Enumerable
                            .Range(0, infoChart.OneFrameMeasureSize)
                            .Select(_ => (float) Random.Shared.Next(0, 10))
                            .ToArray();
                        var data = new ReadOnlyMemory<float>(array);
                        dataToSend1.Add(data.Span.ToArray());
                        await Server.Send(ServerTime.GetUtcNow().DateTime, data, infoChart, cancel);
                        // ServerTime.Advance(TimeSpan.FromMilliseconds(_serverConfig.SendSignalDelayMs + 1));
                    } else if (opts.ChartId == 2 && infoChart.Id == 2)
                    {
                        var array = Enumerable
                            .Range(0, infoChart.OneFrameMeasureSize)
                            .Select(_ => (float) Random.Shared.Next(0, 10))
                            .ToArray();
                        var data = new ReadOnlyMemory<float>(array);
                        dataToSend2.Add(data.Span.ToArray());
                        await Server.Send(ServerTime.GetUtcNow().DateTime, data, infoChart, cancel);
                        // ServerTime.Advance(TimeSpan.FromMilliseconds(_serverConfig.SendSignalDelayMs + 1));
                    }
                }
            }, cancel);
            
            return Task.FromResult(opts);
        };

        Client.OnDataReceived = (time, memory, chartInfo) =>
        {
            if (chartInfo.Id == 1) 
            {
                dataReceived1.Add(memory.Span.ToArray().Select(_ => MathF.Round(_)).ToArray());
                if (dataReceived1.Count == expectedCount)
                {
                    tcs1.TrySetResult();
                }
            } else if (chartInfo.Id == 2)
            {
                dataReceived2.Add(memory.Span.ToArray().Select(_ => MathF.Round(_)).ToArray());
                if (dataReceived2.Count == expectedCount)
                {
                    tcs2.TrySetResult();
                }
            }
        };

        // Act
        var finalOptions1 = await Client.RequestStream(options1, _cancellationTokenSource.Token);
        var finalOptions2 = await Client.RequestStream(options2, _cancellationTokenSource.Token);

        // Assert
        await tcs1.Task;
        await tcs2.Task;
        Assert.NotNull(finalOptions1); 
        Assert.NotNull(finalOptions2);
        finalOptions1.Should().BeEquivalentTo(options1);
        finalOptions2.Should().BeEquivalentTo(options2);
        dataReceived1.Should().BeEquivalentTo(dataToSend1);
        dataReceived2.Should().BeEquivalentTo(dataToSend2);
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
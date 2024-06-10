using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRfsa;
using DynamicData;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class AsvRfsaTest
{
    private readonly ITestOutputHelper _output;

    public AsvRfsaTest(ITestOutputHelper output)
    {
        _output = output;
    }
    private void SetupConnection(IEnumerable<SignalInfo> signals, out IRfsaClient clientEx, out IRfsaServer serverEx)
    {
        var link = new VirtualMavlinkConnection();
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var clientSeq = new PacketSequenceCalculator();
        clientEx = new RfsaClient(new RfsaClientConfig(), link.Client, clientId,clientSeq);
       
        
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        var serverSeq = new PacketSequenceCalculator();
        serverEx = new RfsaServer(new RfsaServerConfig(), signals, link.Server, serverId, serverSeq, Scheduler.Default);
        
    }
    
    [Fact]
    public async Task Client_request_stream_list()
    {
        var signals = new List<SignalInfo>
        {
            new(0, 1,-1,2,-2,1,100, AsvRfsaSignalFormat.AsvSdrSignalFormatFloat, "test0", "x0","y0"),
            new(1, 2,-2,3,-3,1,120, AsvRfsaSignalFormat.AsvSdrSignalFormatRangeFloat8bit, "test1", "x1","y1")
        };
        SetupConnection(signals, out var client, out var server);
        var src = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await client.ReadAllSignalInfo(cancel: src.Token);
        var list = new BindingList<SignalInfo>();
        using var c = client.Signals.Bind(list).Subscribe();
        
        Assert.Equal(signals[0].Format, list[0].Format);
        Assert.Equal(signals[0].Id, list[0].Id);
        Assert.Equal(signals[0].SignalName, list[0].SignalName);
        Assert.Equal(signals[0].AxesXName, list[0].AxesXName);
        Assert.Equal(signals[0].AxesYName, list[0].AxesYName);
        Assert.Equal(signals[0].MaxX, list[0].MaxX);
        Assert.Equal(signals[0].MinX, list[0].MinX);
        Assert.Equal(signals[0].MaxY, list[0].MaxY);
        Assert.Equal(signals[0].MinY, list[0].MinY);
        
        Assert.Equal(signals[1].Format, list[1].Format);
        Assert.Equal(signals[0].Id, list[0].Id);
        Assert.Equal(signals[0].SignalName, list[0].SignalName);
        Assert.Equal(signals[0].AxesXName, list[0].AxesXName);
        Assert.Equal(signals[0].AxesYName, list[0].AxesYName);
        Assert.Equal(signals[0].MaxX, list[0].MaxX);
        Assert.Equal(signals[0].MinX, list[0].MinX);
        Assert.Equal(signals[0].MaxY, list[0].MaxY);
        Assert.Equal(signals[0].MinY, list[0].MinY);
        
    }

    [Fact]
    public async Task Client_request_stream()
    {
        var signals = new List<SignalInfo>
        {
            new(0, 1, -1, 2, -2, 1, 100, AsvRfsaSignalFormat.AsvSdrSignalFormatFloat, "test0", "x0", "y0"),
            new(1, 2, -2, 3, -3, 1, 120, AsvRfsaSignalFormat.AsvSdrSignalFormatRangeFloat8bit, "test1", "x1", "y1")
        };
        SetupConnection(signals, out var client, out var server);
        var src = new CancellationTokenSource();
        await client.ReadAllSignalInfo(cancel: src.Token);
        server.OnStreamOptions = async (options, cancel) => options;
        var streamOptions = new StreamOptions(0, AsvRfsaStreamArg.AsvRfsaStreamArgOnce, 100);
        var result = await client.RequestStream(streamOptions, src.Token);
        Assert.Equal(streamOptions.EventType, result.EventType);
        Assert.Equal(streamOptions.Rate, result.Rate);
        Assert.Equal(streamOptions.SignalId, result.SignalId);
        
    }

    [Theory]
    [InlineData(1,10,-1,1,-2,2,AsvRfsaSignalFormat.AsvSdrSignalFormatRangeFloat8bit,3)]
    [InlineData(1,20,-1,1,-2,2,AsvRfsaSignalFormat.AsvSdrSignalFormatRangeFloat16bit,3)]
    [InlineData(1,100,-1,1,-2,2,AsvRfsaSignalFormat.AsvSdrSignalFormatFloat,5)]
    [InlineData(100,100,-10,10,-20,20,AsvRfsaSignalFormat.AsvSdrSignalFormatFloat,5)]
    public async Task Server_send_data(int xSize, int ySize, float minX,float maxX, float minY, float maxY,  AsvRfsaSignalFormat format,int equalPrecision)
    {
        var tcs = new TaskCompletionSource();
        var originTime = DateTime.Now;
        var signals = new List<SignalInfo>
        {
            new(0, maxX, minX, maxY, minY, xSize, ySize, format, "test0", "x0", "y0"),
        };
        SetupConnection(signals, out var client, out var server);
        
        var originData = new float[xSize * ySize];
        var delay = (maxX - minX) / originData.Length;
        for (var i = 0; i < originData.Length; i++)
        {
            originData[i] = minX + delay * i;
        }
        
        var src = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await client.ReadAllSignalInfo(cancel:src.Token);
        client.OnDataReceived = (time, data, info) =>
        {
            Assert.Equal(originTime,time, TimeSpan.FromMilliseconds(1));
            Assert.Equal(xSize * ySize, data.Length);
            for (var i = 0; i < data.Length; i++)
            {
                Assert.Equal(originData[i], data.Span[i], equalPrecision);
            }
            tcs.TrySetResult();
        };
        src = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await server.Send(originTime,originData,signals[0], src.Token);
        
        await tcs.Task;

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvChart;
using DynamicData;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class AsvChartTest
{
    private readonly ITestOutputHelper _output;

    public AsvChartTest(ITestOutputHelper output)
    {
        _output = output;
    }
    private void SetupConnection(out IAsvChartClient clientEx, out IAsvChartServer serverEx)
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
        clientEx = new AsvChartClient(new AsvChartClientConfig(), link.Client, clientId,clientSeq);
       
        
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        var serverSeq = new PacketSequenceCalculator();
        serverEx = new AsvChartServer(new AsvChartServerConfig
        {
            SendSignalDelayMs = 0,
        }, link.Server, serverId, serverSeq);
        
    }
    
    [Fact]
    public async Task Client_request_stream_list()
    {
        var signals = new List<AsvChartInfo>
        {
            new( 0, "test0", new AsvChartAxisInfo("x",  AsvChartUnitType.AsvChartUnitTypeCustom, -1,1,1), new AsvChartAxisInfo("y", AsvChartUnitType.AsvChartUnitTypeCustom,-2,2,100), AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit,null),
            new( 1, "test1", new AsvChartAxisInfo("x",  AsvChartUnitType.AsvChartUnitTypeCustom, -1,1,1), new AsvChartAxisInfo("y", AsvChartUnitType.AsvChartUnitTypeCustom,-2,2,100), AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit,null)    
        };
        SetupConnection(out var client, out var server);
        server.Charts.AddOrUpdate(signals);
        var src = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await client.ReadAllInfo(cancel: src.Token);
        var list = new BindingList<AsvChartInfo>();
        using var c = client.Charts.Bind(list).Subscribe();
        
        Assert.Equal(signals[0].Id, list[0].Id);
        Assert.Equal(signals[0].Format, list[0].Format);
        Assert.Equal(signals[0].SignalName, list[0].SignalName);
        Assert.Equal(signals[0].AxisX.Name, list[0].AxisX.Name);
        Assert.Equal(signals[0].AxisX.Min, list[0].AxisX.Min);
        Assert.Equal(signals[0].AxisX.Max, list[0].AxisX.Max);
        Assert.Equal(signals[0].AxisX.Size, list[0].AxisX.Size);
        Assert.Equal(signals[0].AxisX.Unit, list[0].AxisX.Unit);
        Assert.Equal(signals[0].AxisY.Name, list[0].AxisY.Name);
        Assert.Equal(signals[0].AxisY.Min, list[0].AxisY.Min);
        Assert.Equal(signals[0].AxisY.Max, list[0].AxisY.Max);
        Assert.Equal(signals[0].AxisY.Size, list[0].AxisY.Size);
        Assert.Equal(signals[0].AxisY.Unit, list[0].AxisY.Unit);
        
        Assert.Equal(signals[1].Id, list[1].Id);
        Assert.Equal(signals[1].Format, list[1].Format);
        Assert.Equal(signals[1].SignalName, list[1].SignalName);
        Assert.Equal(signals[1].AxisX.Name, list[1].AxisX.Name);
        Assert.Equal(signals[1].AxisX.Min, list[1].AxisX.Min);
        Assert.Equal(signals[1].AxisX.Max, list[1].AxisX.Max);
        Assert.Equal(signals[1].AxisX.Size, list[1].AxisX.Size);
        Assert.Equal(signals[1].AxisX.Unit, list[1].AxisX.Unit);
        Assert.Equal(signals[1].AxisY.Name, list[1].AxisY.Name);
        Assert.Equal(signals[1].AxisY.Min, list[1].AxisY.Min);
        Assert.Equal(signals[1].AxisY.Max, list[1].AxisY.Max);
        Assert.Equal(signals[1].AxisY.Size, list[1].AxisY.Size);
        Assert.Equal(signals[1].AxisY.Unit, list[1].AxisY.Unit);
        
    }

    [Fact]
    public async Task Client_request_stream()
    {
        var signals = new List<AsvChartInfo>
        {
            new( 0, "test0", new AsvChartAxisInfo("x",  AsvChartUnitType.AsvChartUnitTypeCustom, -1,1,1), new AsvChartAxisInfo("y", AsvChartUnitType.AsvChartUnitTypeCustom,-2,2,100), AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit,null),
            new( 1, "test1", new AsvChartAxisInfo("x",  AsvChartUnitType.AsvChartUnitTypeCustom, -1,1,1), new AsvChartAxisInfo("y", AsvChartUnitType.AsvChartUnitTypeCustom,-2,2,100), AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit,null)    
        };
        
        SetupConnection(out var client, out var server);
        var src = new CancellationTokenSource();
        server.Charts.AddOrUpdate(signals);
        await client.ReadAllInfo(cancel: src.Token);
        server.OnDataRequest = (options,info, cancel) => Task.FromResult(options);
        var streamOptions = new AsvChartOptions(0, AsvChartDataTrigger.AsvChartDataTriggerOnce, 100);
        var result = await client.RequestStream(streamOptions, src.Token);
        Assert.Equal(streamOptions.Trigger, result.Trigger);
        Assert.Equal(streamOptions.Rate, result.Rate);
        Assert.Equal(streamOptions.ChartId, result.ChartId);
        
    }

    [Theory]
    [InlineData(1,10,-1,1,-2,2,AsvChartDataFormat.AsvChartDataFormatRangeFloat8bit,1)]
    [InlineData(1,20,-1,1,-2,2,AsvChartDataFormat.AsvChartDataFormatRangeFloat16bit,3)]
    [InlineData(1,100,-1,1,-2,2,AsvChartDataFormat.AsvChartDataFormatFloat,5)]
    [InlineData(100,100,-10,10,-20,20,AsvChartDataFormat.AsvChartDataFormatFloat,5)]
    public async Task Server_send_data(int xSize, int ySize, float minX,float maxX, float minY, float maxY,  AsvChartDataFormat format,int equalPrecision)
    {
        var tcs = new TaskCompletionSource();
        var originTime = DateTime.Now;
        var signals = new List<AsvChartInfo>
        {
            new( 0, "test0", new AsvChartAxisInfo("x",  AsvChartUnitType.AsvChartUnitTypeCustom, minX,maxX,xSize), new AsvChartAxisInfo("y", AsvChartUnitType.AsvChartUnitTypeCustom,minY,maxY,ySize), format,null),
        };
        SetupConnection(out var client, out var server);
        server.Charts.AddOrUpdate(signals);
        var originData = new float[xSize * ySize];
        var delay = (maxX - minX) / originData.Length;
        for (var i = 0; i < originData.Length; i++)
        {
            originData[i] = minX + delay * i;
        }
        
        var src = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await client.ReadAllInfo(cancel:src.Token);
        client.OnDataReceived = (time, data, info) =>
        {
            try
            {
                Assert.Equal(originTime,time, TimeSpan.FromMilliseconds(1));
                Assert.Equal(xSize * ySize, data.Length);
                for (var i = 0; i < data.Length; i++)
                {
                    Assert.Equal(originData[i], data.Span[i], equalPrecision);
                }
                tcs.TrySetResult();
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
            
            
        };
        src = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await server.Send(originTime,originData,signals[0], src.Token);
        
        await tcs.Task;

    }
}
using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData.Binding;
using Xunit;

namespace Asv.Mavlink.Test;

public class MavlinkMissionExMicroserviceTest
{
    private void CreateClientServer(VirtualMavlinkConnection link, out IMissionServerEx serverEx, out IMissionClientEx client)
    {
        var serverSeq = new PacketSequenceCalculator();
        var serverId = new MavlinkIdentity(13,13);
        var statusServer = new StatusTextServer(link.Server, serverSeq,
            serverId, new StatusTextLoggerConfig(), Scheduler.Default);
        var server = new MissionServer(link.Server, serverId, serverSeq, TaskPoolScheduler.Default);
        serverEx = new MissionServerEx(server, statusServer, link.Server, serverId, serverSeq, Scheduler.Default);
        
        var clientSeq = new PacketSequenceCalculator();
        var clientId = new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13};
        var clientBase = new MissionClient(
            link.Client, 
            clientId, 
            clientSeq, new MissionClientConfig
            {
                AttemptToCallCount = 5,
                CommandTimeoutMs = 100
            });
        client = new MissionClientEx(clientBase);
    }
    
    [Fact]
    public async Task Upload_And_Download_Mission_Item()
    {
        var link = new VirtualMavlinkConnection();
        CreateClientServer(link, out var server, out var client);
        using var subscribe = server.Items.BindToObservableList(out var serverList).Subscribe();
        var item1 = client.Create();
        item1.Command.OnNext(MavCmd.MavCmdUser1);
        item1.Current.OnNext(true);
        item1.AutoContinue.OnNext(true);
        item1.Param1.OnNext(1);
        item1.Param2.OnNext(2);
        item1.Param3.OnNext(3);
        item1.Param4.OnNext(4);
        item1.Frame.OnNext(MavFrame.MavFrameGlobal);
        item1.MissionType.OnNext(MavMissionType.MavMissionTypeMission);
        item1.Location.OnNext(new GeoPoint(1, 2, 3));
        
        
        
        await client.Upload();
        Assert.Equal(1, serverList.Count);
        var item2 = serverList.Items.First();
        Assert.Equal(item1.Command.Value, item2.Command);
        Assert.Equal(item1.AutoContinue.Value, item2.Autocontinue != 0);
        Assert.Equal(item1.Param1.Value, item2.Param1);
        Assert.Equal(item1.Param2.Value, item2.Param2);
        Assert.Equal(item1.Param3.Value, item2.Param3);
        Assert.Equal(item1.Param4.Value, item2.Param4);
        Assert.Equal(item1.Frame.Value, item2.Frame);
        Assert.Equal(item1.MissionType.Value, item2.MissionType);
        var geoPoint = new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(item2.X),MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(item2.Y),item2.Z);
        Assert.Equal(item1.Location.Value, geoPoint);
        
        var downloadedItems = await client.Download(CancellationToken.None);

        Assert.Single(downloadedItems);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task Client_upload_and_download_list_of_missions(int maxItems)
    {
        var link = new VirtualMavlinkConnection();
        CreateClientServer(link, out var server, out var client);
        using var subscribe = server.Items.BindToObservableList(out var serverList).Subscribe();
        for (int i = 0; i < maxItems; i++)
        {
            var item1 = client.Create();
        }
        await client.Upload();
        Assert.Equal(maxItems, serverList.Count);
        var items = await client.Download(CancellationToken.None);
        Assert.Equal(maxItems, items.Length);
    }

    [Fact]
    public async Task Check_Client_MissionSetCurrent()
    {
        var link = new VirtualMavlinkConnection();
        CreateClientServer(link, out var server, out var client);
        using var subscribe = server.Items.BindToObservableList(out var serverList).Subscribe();
        client.Create();
        client.Create();
        client.Create();
        await client.Upload();
        
        Assert.Equal(3, serverList.Count);
        
        await client.Base.MissionSetCurrent(2);
        
        Assert.Equal(2, server.Current.Value);

        await client.SetCurrent(1);
        
        Assert.Equal(1, server.Current.Value);
        
        Assert.NotEqual(0, server.Current.Value);
        Assert.NotEqual(2, server.Current.Value);
        Assert.NotEqual(3, server.Current.Value);
    }

    [Fact]
    public async Task Check_Client_MissionReached()
    {
        var link = new VirtualMavlinkConnection();
        CreateClientServer(link, out var server, out var client);
        using var subscribe = server.Items.BindToObservableList(out var serverList).Subscribe();
        client.Create();
        client.Create();
        client.Create();
        await client.Upload();
        
        server.Reached.OnNext(serverList.Items.ElementAt(0).Seq);
        await Task.Delay(100);
        Assert.Equal(serverList.Items.ElementAt(0).Seq, client.Reached.Value);
        
        server.Reached.OnNext(serverList.Items.ElementAt(1).Seq);
        await Task.Delay(100);
        Assert.Equal(serverList.Items.ElementAt(1).Seq, client.Reached.Value);
        
        server.Reached.OnNext(serverList.Items.ElementAt(2).Seq);
        await Task.Delay(100);
        Assert.Equal(serverList.Items.ElementAt(2).Seq, client.Reached.Value);
    }

    [Fact]
    public async Task Check_Client_MissionRequestCount()
    {
        var link = new VirtualMavlinkConnection();
        CreateClientServer(link, out var server, out var client);
        using var subscribe = server.Items.BindToObservableList(out var serverList).Subscribe();
        client.Create();
        client.Create();
        client.Create();
        await client.Upload();

        var count = await client.Base.MissionRequestCount();
        
        Assert.Equal(serverList.Count, count);
    }

    [Fact]
    public async Task Check_Client_MissionRequestItem()
    {
        var link = new VirtualMavlinkConnection();
        CreateClientServer(link, out var server, out var client);
        using var subscribe = server.Items.BindToObservableList(out var serverList).Subscribe();
        client.Create();
        client.Create();
        client.Create();
        await client.Upload();

        var payload = await client.Base.MissionRequestItem(1);
        var serverSideData = serverList.Items.ElementAt(1);
        
        Assert.Equal(payload.Command, serverSideData.Command);
        Assert.Equal(payload.Autocontinue, serverSideData.Autocontinue);
        Assert.Equal(payload.MissionType, serverSideData.MissionType);
        Assert.Equal(payload.Param1, serverSideData.Param1);
        Assert.Equal(payload.Param2, serverSideData.Param2);
        Assert.Equal(payload.Param3, serverSideData.Param3);
        Assert.Equal(payload.Param4, serverSideData.Param4);
        Assert.Equal(payload.Frame, serverSideData.Frame);
        Assert.Equal(payload.Seq, serverSideData.Seq);
        Assert.Equal(payload.X, serverSideData.X);
        Assert.Equal(payload.Y, serverSideData.Y);
        Assert.Equal(payload.Z, serverSideData.Z);
    }

    [Fact]
    public async Task Check_Client_WriteMissionItem()
    {
        var link = new VirtualMavlinkConnection();
        CreateClientServer(link, out var server, out var client);
        using var subscribe = server.Items.BindToObservableList(out var serverList).Subscribe();
        client.Create();
        client.Create();
        client.Create();
        
        await client.Upload();
    }
}
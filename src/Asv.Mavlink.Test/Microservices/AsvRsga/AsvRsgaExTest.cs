using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRsga;
using Asv.Mavlink.V2.Common;
using DynamicData.Binding;
using Xunit;

namespace Asv.Mavlink.Test.AsvRsga;

public class AsvRsgaExTest
{
    private void CreateEx(out IAsvRsgaClientEx clientEx,  out IAsvRsgaServerEx serverEx)
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
        var commandClient = new CommandClient(link.Client, clientId, clientSeq, new CommandProtocolConfig());
        var rsga = new AsvRsgaClient(link.Client, clientId, clientSeq);
        clientEx = new AsvRsgaClientEx(rsga, commandClient);
        
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        var serverSeq = new PacketSequenceCalculator();
        var commandServer = new CommandServer(link.Server, serverSeq, serverId, Scheduler.Default);
        var commandLongServerEx = new CommandLongServerEx(commandServer);
        var status = new StatusTextServer(link.Server, serverSeq, serverId, new StatusTextLoggerConfig(), Scheduler.Default);
        
        var server = new AsvRsgaServer(link.Server, serverId, serverSeq, Scheduler.Default);
        serverEx = new AsvRsgaServerEx(server,status,commandLongServerEx);
    }
    
    [Fact]
    public async Task Client_request_compatibility_server_respond_it()
    {
        CreateEx(out var clientEx, out var serverEx);
        var origin = new HashSet<AsvRsgaCustomMode>(Enum.GetValues<AsvRsgaCustomMode>());
        serverEx.GetCompatibility = () => origin;
        var originFreq = uint.MaxValue;
        var originMode = AsvRsgaCustomMode.AsvRsgaCustomModeTxGp;
        serverEx.SetMode = async (mode, freq, cancel) =>
        {
            Assert.Equal(originMode,mode);
            Assert.Equal(originFreq,freq);
            return MavResult.MavResultAccepted;
        };
        await clientEx.RefreshInfo();
        using var c = clientEx.AvailableModes.BindToObservableList(out var list).Subscribe();
        Assert.Equal(origin.Count, list.Count);
        Assert.True(origin.All(x => list.Items.Contains(x)));

        await clientEx.SetMode(originMode, originFreq);
    }
}
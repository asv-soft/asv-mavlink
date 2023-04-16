using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class HeartbeatMicroserviceTest
{
    [Fact]
    public async Task TestHeartbeat()
    {
        var link = new VirtualLink();
        
        var serverHeartbeat = new HeartbeatServer(
            link.Server, 
            new PacketSequenceCalculator(), 
            new MavlinkServerIdentity{ComponentId = 13, SystemId = 13}, 
            new MavlinkHeartbeatServerConfig(), 
            TaskPoolScheduler.Default);
        
        var clientHeartbeat = new HeartbeatClient(
            link.Client, 
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default,
            new HeartbeatClientConfig());
        
        serverHeartbeat.Set(_=>
        {
            _.Autopilot = MavAutopilot.MavAutopilotGeneric;
            _.BaseMode = MavModeFlag.MavModeFlagManualInputEnabled;
            _.CustomMode = 123U;
            _.SystemStatus = MavState.MavStateActive;
            _.MavlinkVersion = 3;
        });
        serverHeartbeat.Start();
        await clientHeartbeat.Link.FirstAsync(_ => _ == LinkState.Connected);
        await Task.Delay(1000);
        Assert.Equal(MavAutopilot.MavAutopilotGeneric, clientHeartbeat.RawHeartbeat.Value.Autopilot);
        Assert.Equal(MavModeFlag.MavModeFlagManualInputEnabled, clientHeartbeat.RawHeartbeat.Value.BaseMode);
        Assert.Equal(123U, clientHeartbeat.RawHeartbeat.Value.CustomMode);
        Assert.Equal(MavState.MavStateActive, clientHeartbeat.RawHeartbeat.Value.SystemStatus);
        Assert.Equal(3, clientHeartbeat.RawHeartbeat.Value.MavlinkVersion);


    }
    
    
   
}
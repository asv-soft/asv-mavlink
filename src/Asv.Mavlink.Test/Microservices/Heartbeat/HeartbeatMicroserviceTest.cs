using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace Asv.Mavlink.Test;

public class HeartbeatMicroserviceTest
{
    [Fact]
    public async Task Server_Send_Heartbeat_Packet_And_Client_Catch_It()
    {
        var link = new VirtualMavlinkConnection();
        var fakeTime = new FakeTimeProvider();
        var serverHeartbeat = new HeartbeatServer(
            link.Server, 
            new PacketSequenceCalculator(), 
            new MavlinkIdentity(13,13), 
            new MavlinkHeartbeatServerConfig(), fakeTime);
        
        var clientHeartbeat = new HeartbeatClient(
            link.Client, 
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new PacketSequenceCalculator(),
            new HeartbeatClientConfig(),fakeTime);
        
        serverHeartbeat.Set(_=>
        {
            _.Autopilot = MavAutopilot.MavAutopilotGeneric;
            _.BaseMode = MavModeFlag.MavModeFlagManualInputEnabled;
            _.CustomMode = 123U;
            _.SystemStatus = MavState.MavStateActive;
            _.MavlinkVersion = 3;
        });
        serverHeartbeat.Start();
        fakeTime.Advance(TimeSpan.FromSeconds(1.1));
        await clientHeartbeat.Link.FirstAsync(_ => _ == LinkState.Connected);
        fakeTime.Advance(TimeSpan.FromSeconds(1.1));
        Assert.Equal(MavAutopilot.MavAutopilotGeneric, clientHeartbeat.RawHeartbeat.Value.Autopilot);
        Assert.Equal(MavModeFlag.MavModeFlagManualInputEnabled, clientHeartbeat.RawHeartbeat.Value.BaseMode);
        Assert.Equal(123U, clientHeartbeat.RawHeartbeat.Value.CustomMode);
        Assert.Equal(MavState.MavStateActive, clientHeartbeat.RawHeartbeat.Value.SystemStatus);
        Assert.Equal(3, clientHeartbeat.RawHeartbeat.Value.MavlinkVersion);
    }
}
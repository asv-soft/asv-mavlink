using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class HeartbeatTests(ITestOutputHelper output) : ComplexTestBase<HeartbeatClient, HeartbeatServer>(output)
{
    private readonly HeartbeatClientConfig _clientConfig = new();
    private readonly MavlinkHeartbeatServerConfig _serverConfig = new();

    protected override HeartbeatServer CreateServer(MavlinkIdentity identity, ICoreServices core) => new(identity,_serverConfig,core);
    protected override HeartbeatClient CreateClient(MavlinkClientIdentity identity, ICoreServices core) => new(identity, _clientConfig, core);

    [Fact]
    public async Task Server_Send_Heartbeat_Packet_And_Client_Catch_It()
    {
      Server.Set(p=>
        {
            p.Autopilot = MavAutopilot.MavAutopilotGeneric;
            p.BaseMode = MavModeFlag.MavModeFlagManualInputEnabled;
            p.CustomMode = 123U;
            p.SystemStatus = MavState.MavStateActive;
            p.MavlinkVersion = 3;
        });
        Server.Start();
        ServerTime.Advance(TimeSpan.FromSeconds(1.1));
        ClientTime.Advance(TimeSpan.FromSeconds(1.1));
        await Client.Link.FirstAsync(_ => _ == LinkState.Connected);
        
        Assert.NotNull(Client.RawHeartbeat.Value);
        Assert.Equal(MavAutopilot.MavAutopilotGeneric, Client.RawHeartbeat.Value.Autopilot);
        Assert.Equal(MavModeFlag.MavModeFlagManualInputEnabled, Client.RawHeartbeat.Value.BaseMode);
        Assert.Equal(123U, Client.RawHeartbeat.Value.CustomMode);
        Assert.Equal(MavState.MavStateActive, Client.RawHeartbeat.Value.SystemStatus);
        Assert.Equal(3, Client.RawHeartbeat.Value.MavlinkVersion);
    }

   
}
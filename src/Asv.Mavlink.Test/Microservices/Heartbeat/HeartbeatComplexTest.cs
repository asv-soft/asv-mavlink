using System;

using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class HeartbeatComplexTest(ITestOutputHelper output) : ComplexTestBase<HeartbeatClient, HeartbeatServer>(output)
{
    private readonly HeartbeatClientConfig _clientConfig = new();
    private readonly MavlinkHeartbeatServerConfig _serverConfig = new();

    protected override HeartbeatServer CreateServer(MavlinkIdentity identity, ICoreServices core) => new(identity,_serverConfig,core);
    protected override HeartbeatClient CreateClient(MavlinkClientIdentity identity, ICoreServices core) => new(identity, _clientConfig, core);

    [Fact]
    public async Task Server_Send_Heartbeat_Packet_And_Client_Catch_It()
    {
        var client = Client;
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
        await Client.Link.FirstAsync(x => x == LinkState.Connected);
        
        Assert.NotNull(Client.RawHeartbeat.CurrentValue);
        Assert.Equal(MavAutopilot.MavAutopilotGeneric, Client.RawHeartbeat.CurrentValue.Autopilot);
        Assert.Equal(MavModeFlag.MavModeFlagManualInputEnabled, Client.RawHeartbeat.CurrentValue.BaseMode);
        Assert.Equal(123U, Client.RawHeartbeat.CurrentValue.CustomMode);
        Assert.Equal(MavState.MavStateActive, Client.RawHeartbeat.CurrentValue.SystemStatus);
        Assert.Equal(3, Client.RawHeartbeat.CurrentValue.MavlinkVersion);
    }

   
}
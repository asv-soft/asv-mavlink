using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Minimal;
using Xunit;
using Xunit.Priority;

namespace Asv.Mavlink.Test.Router;


public class MavlinkRouterTest
{
    [Fact(Skip = "Bad test for CI")]
    public async Task Retrieves_Packets_All()
    {
        // Arrange
        var ip = "127.0.0.1";

        var client = CreateTcpRouter(ip, 5000, 0, false);
        
        var server = CreateTcpRouter(ip, 5000, 0, true);
        
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        
        var serverId = new MavlinkClientIdentity
        {
            SystemId = 3,
            ComponentId = 4,
            TargetSystemId = 1,
            TargetComponentId = 2
        };

        var clientDevice = new AbstractClientDevice(
            client,
            clientId,
            new ClientDeviceConfig(),
            new PacketSequenceCalculator());

        var serverDevice = new AbstractClientDevice(
            server,
            serverId,
            new ClientDeviceConfig(),
            new PacketSequenceCalculator());
        
        clientDevice.WaitUntilConnect();
        
        var packets = new List<HeartbeatPacket>
        {
            new(),
            new(),
            new(),
            new(),
            new(),
        };

        // Act
        foreach (var packet in packets)
        {
            await serverDevice.Connection.Send(packet, CancellationToken.None);
            await Task.Delay(500);
        }
        
        // Assert
        Assert.Equal(serverDevice.Connection.TxPackets, clientDevice.Connection.RxPackets);
    }
    
    [Fact, Priority(1)]
    public async Task Retrieves_Packets_NoneOrNotAll()
    {
        // Arrange
        var ip = "127.0.0.1";

        var client = CreateTcpRouter(ip, 5000, 100, false);
        
        var server = CreateTcpRouter(ip, 5000, 0, true);
        
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        
        var serverId = new MavlinkClientIdentity
        {
            SystemId = 3,
            ComponentId = 4,
            TargetSystemId = 1,
            TargetComponentId = 2
        };

        var clientDevice = new AbstractClientDevice(
            client,
            clientId,
            new ClientDeviceConfig(),
            new PacketSequenceCalculator());

        var serverDevice = new AbstractClientDevice(
            server,
            serverId,
            new ClientDeviceConfig(),
            new PacketSequenceCalculator());
        
        clientDevice.WaitUntilConnect();
        
        var packets = new List<HeartbeatPacket>
        {
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
        };

        // Act
        foreach (var packet in packets)
        {
            await serverDevice.Connection.Send(packet, CancellationToken.None);
            await Task.Delay(500);
        }
        
        // Assert
        Assert.Equal(0, clientDevice.Connection.RxPackets);
    }
    
    private MavlinkRouter CreateTcpRouter(string server, int portName, int packetLossChance, bool isSrv)
    {
        var mavlinkRouter =
            new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects);
        mavlinkRouter.WrapToV2ExtensionEnabled = true;
        
        var srv = isSrv ? "?srv=true" : "";
        
        var port = new MavlinkPortConfig
        {
            ConnectionString = $"tcp://{server}:{portName}{srv}",
            Name = isSrv ? "Server" : "Client",
            IsEnabled = true,
            PacketLossChance = packetLossChance,
        };

        mavlinkRouter.AddPort(port);

        return mavlinkRouter;
    }
}
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class BaseDevicesTest
{
    private readonly ITestOutputHelper _output;

    public BaseDevicesTest(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public Task ClientDeviceIdentityArgumentNullExceptionTest()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();
            
            var clientDevice = new AbstractClientDevice(link.Client,
                null,
                new ClientDeviceConfig(),
                new PacketSequenceCalculator(),
                Scheduler.Default);
        });
        return Task.CompletedTask;
    }
    
    [Fact]
    public Task ClientDeviceConfigArgumentNullExceptionTest()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();
            
            var clientId = new MavlinkClientIdentity
            {
                SystemId = 1,
                ComponentId = 2,
                TargetSystemId = 3,
                TargetComponentId = 4
            };
            
            var clientDevice = new AbstractClientDevice(link.Client,
                clientId,
                new ClientDeviceConfig
                {
                    Heartbeat = null
                },
                new PacketSequenceCalculator(),
                Scheduler.Default);
        });
        return Task.CompletedTask;
    }
    
    [Fact]
    public Task ClientDeviceVirtualLinkArgumentNullExceptionTest()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var clientDevice = new AbstractClientDevice(null,
                new MavlinkClientIdentity(),
                new ClientDeviceConfig
                {
                    Heartbeat = null
                },
                new PacketSequenceCalculator(),
                Scheduler.Default);
        });
        return Task.CompletedTask;
    }
    
    [Fact]
    public Task ServerDeviceVirtualLinkArgumentNullExceptionTest()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var serverDevice = new ServerDevice(null,
                new PacketSequenceCalculator(),
                new MavlinkIdentity(),
                new ServerDeviceConfig(),
                Scheduler.Default);
        });
        return Task.CompletedTask;
    }
    
    [Fact]
    public Task ServerDeviceSequenceArgumentNullExceptionTest()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();
            
            var serverDevice = new ServerDevice(link.Server,
                null,
                new MavlinkIdentity(),
                new ServerDeviceConfig(),
                Scheduler.Default);
        });
        return Task.CompletedTask;
    }
    
    
    
    // [Fact]
    // public async Task ServerDeviceConfigHeartbeatArgumentNullExceptionTest()
    // {
    //     Assert.Throws<ArgumentNullException>(() =>
    //     {
    //         var link = new VirtualMavlinkConnection();
    //         
    //         var serverDevice = new ServerDevice(link.Server,
    //             new PacketSequenceCalculator(),
    //             new MavlinkIdentity(),
    //             new ServerDeviceConfig
    //             {
    //                 Heartbeat = null
    //             },
    //             Scheduler.Default);
    //     });
    // }
    
    [Fact]
    public Task ServerDeviceConfigStatusTextArgumentNullExceptionTest()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();
            
            var serverDevice = new ServerDevice(link.Server,
                new PacketSequenceCalculator(),
                new MavlinkIdentity(),
                new ServerDeviceConfig
                {
                    StatusText = null
                },
                Scheduler.Default);
        });
        return Task.CompletedTask;
    }
    
    [Fact]
    public void ServerDeviceSchedulerArgumentNullExceptionTest()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();
            
            var serverDevice = new ServerDevice(link.Server,
                new PacketSequenceCalculator(),
                new MavlinkIdentity(),
                new ServerDeviceConfig(),
                null);
        });
        
    }
    
    [Fact]
    public void ClientDeviceSequenceArgumentNullExceptionTest()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();
            var clientId = new MavlinkClientIdentity
            {
                SystemId = 1,
                ComponentId = 2,
                TargetSystemId = 3,
                TargetComponentId = 4
            };
            var clientDevice = new AbstractClientDevice(link.Client,
                clientId,
                new ClientDeviceConfig(),
                null,
                Scheduler.Default);
        });
    }
    

    
    [Theory]
    [InlineData("The quick brown fox jumps over the lazy dog")]
    [InlineData("/><#$@!%^&*()-=+_?.,]{}[~`';:\"|^\\")]
    [InlineData("1234567890")]
    public async Task BaseDevicesStatusTextTest(string messageText)
    {
        var link = new VirtualMavlinkConnection();
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        var serverDevice = new ServerDevice(link.Server, new PacketSequenceCalculator(),
            serverId, new ServerDeviceConfig(), Scheduler.Default);
        
        var clientDevice = new AbstractClientDevice(link.Client, clientId,
            new ClientDeviceConfig(), new PacketSequenceCalculator(), Scheduler.Default);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        await Task.Delay(2000);
        
        serverDevice.StatusText.Log(MavSeverity.MavSeverityDebug, messageText);
        
        var message = await clientDevice.StatusText.OnMessage.FirstAsync();
        
        _output.WriteLine($"Client status message: {message.Text}");
        
        Assert.Equal(messageText, message.Text);
    }

    [Fact]
    public async Task BaseDevicesStatusTextOverloadTest()
    {
        var link = new VirtualMavlinkConnection();
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        
        var serverDevice = new ServerDevice(link.Server, new PacketSequenceCalculator(),
            serverId, new ServerDeviceConfig(), Scheduler.Default);
        
        var clientDevice = new AbstractClientDevice(link.Client, clientId,
            new ClientDeviceConfig(), new PacketSequenceCalculator(), Scheduler.Default);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        bool isOverloaded = false;

        int overloadIndex = 0;
        
        for (int i = 0; i < 1000; i++)
        {
            if(!serverDevice.StatusText.Debug($"Message {i}"))
            {
                isOverloaded = true;
                
                _output.WriteLine($"Breaked on: {i}");
                
                overloadIndex = i;
                
                break;
            }
        }
        
        Assert.True(isOverloaded);
        
        Assert.Equal(101, overloadIndex);
        
        var message = await clientDevice.StatusText.OnMessage.FirstAsync();
        
        _output.WriteLine($"Client status message: {message.Text}");
        
        Assert.Equal("Message 0", message.Text);
    }
    
    [Fact]
    public async Task BaseDevicesHeartbeatConnectionQualityTest()
    {
        //Need to lose half of all packets
        
        int packetsCount = 0;
        
        var link = new VirtualMavlinkConnection(_ => true, _ => ++packetsCount % 2 == 1);
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        
        var serverDevice = new ServerDevice(link.Server, new PacketSequenceCalculator(), 
            serverId,
            new ServerDeviceConfig()
            {
                Heartbeat = new MavlinkHeartbeatServerConfig()
                {
                    HeartbeatRateMs = 100
                }
            },
            Scheduler.Default);
        
        var clientDevice = new AbstractClientDevice(link.Client, clientId,
            new ClientDeviceConfig(), new PacketSequenceCalculator(), Scheduler.Default);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        await Task.Delay(3_100);

        var linkQuality = clientDevice.Heartbeat.LinkQuality.Value;
        
        _output.WriteLine($"Client connection quality: {linkQuality}");
        
        _output.WriteLine($"Rx packets: {clientDevice.Connection.RxPackets}");
        
        _output.WriteLine($"Tx packets: {serverDevice.Connection.TxPackets}");
        
        Assert.InRange(linkQuality,0.4,0.7); 
        _output.WriteLine($"Linq quality {linkQuality:F2}");
    }
    
    [Fact]
    public async Task BaseDevicesHeartbeatConnectionStateTest()
    {
        bool canSend = true;
        
        var link = new VirtualMavlinkConnection(_ => canSend, _ => canSend);
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        
        var serverDevice = new ServerDevice(link.Server, new PacketSequenceCalculator(),
            serverId, new ServerDeviceConfig(), Scheduler.Default);
        
        var clientDevice = new AbstractClientDevice(link.Client, clientId,
            new ClientDeviceConfig(), new PacketSequenceCalculator(), Scheduler.Default);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        await Task.Delay(4000);
        
        var linkStatus = await clientDevice.Heartbeat.Link.FirstAsync();
        
        Assert.Equal(LinkState.Connected, linkStatus);
        
        _output.WriteLine($"Client connection state: {linkStatus}");

        canSend = false;
        
        await Task.Delay(10000);
        
        linkStatus = await clientDevice.Heartbeat.Link.FirstAsync();
        
        Assert.Equal(LinkState.Disconnected, linkStatus);
        
        _output.WriteLine($"Client connection state: {linkStatus}");
        
        canSend = true;
        
        await Task.Delay(3000);
        
        linkStatus = await clientDevice.Heartbeat.Link.FirstAsync();
        
        Assert.Equal(LinkState.Connected, linkStatus);
        
        _output.WriteLine($"Client connection state: {linkStatus}");
        
        canSend = false;
        
        await Task.Delay(3000);
        
        linkStatus = await clientDevice.Heartbeat.Link.FirstAsync();
        
        Assert.Equal(LinkState.Downgrade, linkStatus);
        
        _output.WriteLine($"Client connection state: {linkStatus}");
    }

    [Fact]
    public async Task BaseDevicesCustomModeTest()
    {
        var link = new VirtualMavlinkConnection();
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        
        var mode = 23;
        
        var serverDevice = new ServerDevice(link.Server, new PacketSequenceCalculator(),
            serverId, new ServerDeviceConfig(), Scheduler.Default);
        
        serverDevice.Heartbeat.Set(_ => _.CustomMode = (uint)mode);
        
        var clientDevice = new AbstractClientDevice(link.Client, clientId,
            new ClientDeviceConfig(), new PacketSequenceCalculator(), Scheduler.Default);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        await Task.Delay(2000);
        
        var clientState = await clientDevice.Heartbeat.RawHeartbeat.FirstAsync();
        
        Assert.Equal((uint)mode, clientState.CustomMode);
        
        _output.WriteLine($"Server custom mode: {mode}");
        
        _output.WriteLine($"Client custom mode: {clientState.CustomMode}");
    }
    
    [Fact]
    public async Task BaseDevicesPacketsLossTest()
    {
        int packetsCount = 0;
        
        int lostPacketsCount = 4;
        
        var link = new VirtualMavlinkConnection(_ => true,_ => ++packetsCount > lostPacketsCount);
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);

        var serverDevice = new ServerDevice(link.Server, new PacketSequenceCalculator(),
            serverId, new ServerDeviceConfig(),
            Scheduler.Default);

        var clientDevice = new AbstractClientDevice(link.Client,
            clientId,
            new ClientDeviceConfig(), new PacketSequenceCalculator(), Scheduler.Default);

        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        await Task.Delay(5000);

        Assert.NotEqual(serverDevice.Connection.TxPackets, clientDevice.Connection.RxPackets);
        
        _output.WriteLine($"Total packets count: {packetsCount}");
        
        _output.WriteLine($"Server tx packets: {serverDevice.Connection.TxPackets}");
        
        _output.WriteLine($"Client rx packets: {clientDevice.Connection.RxPackets}");
    }
    
    private AbstractClientDevice CreateClientDevice(VirtualMavlinkConnection link, PacketSequenceCalculator seq, 
        MavlinkClientIdentity identity, ClientDeviceConfig cfg)
    {
        return new AbstractClientDevice(link.Client,
            identity, cfg, seq, Scheduler.Default);
    }
    
    private ServerDevice CreateServerDevice(VirtualMavlinkConnection link, PacketSequenceCalculator seq, 
        MavlinkIdentity identity, ServerDeviceConfig cfg)
    {
        return new ServerDevice(link.Server,
            seq, identity, cfg, Scheduler.Default);
    }

}

public class AbstractClientDevice : ClientDevice, IClientDevice
{
    public AbstractClientDevice(IMavlinkV2Connection connection, 
        MavlinkClientIdentity identity, 
        ClientDeviceConfig config, 
        IPacketSequenceCalculator seq, 
        IScheduler scheduler) 
        : base(connection, identity, config, seq, scheduler)
    {
        
    }

    protected override string DefaultName => "TEST";

    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }


    public override DeviceClass Class => DeviceClass.Unknown;
}


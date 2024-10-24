using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class BaseDevicesTest(ITestOutputHelper output)
{
    [Fact]
    public Task ClientDeviceIdentityArgumentNullExceptionTest()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();
            
            var clientDevice = new AbstractClientDevice(link.Client,
                null,
                new ClientDeviceBaseConfig(),
                new PacketSequenceCalculator());
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
                new ClientDeviceBaseConfig
                {
                    Heartbeat = null
                },
                new PacketSequenceCalculator());
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
                new ClientDeviceBaseConfig
                {
                    Heartbeat = null
                },
                new PacketSequenceCalculator());
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
                new ServerDeviceConfig());
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
                new ServerDeviceConfig());
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
                });
        });
        return Task.CompletedTask;
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
                new ClientDeviceBaseConfig(),
                null);
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
            serverId, new ServerDeviceConfig());
        
        var clientDevice = new AbstractClientDevice(link.Client, clientId,
            new ClientDeviceBaseConfig(), new PacketSequenceCalculator());
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        
        
        serverDevice.StatusText.Log(MavSeverity.MavSeverityDebug, messageText);
        
        var message = await clientDevice.StatusText.OnMessage.FirstAsync();
        
        output.WriteLine($"Client status message: {message.Text}");
        
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
            serverId, new ServerDeviceConfig());
        
        var clientDevice = new AbstractClientDevice(link.Client, clientId,
            new ClientDeviceBaseConfig(), new PacketSequenceCalculator());
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        bool isOverloaded = false;

        int overloadIndex = 0;
        
        for (int i = 0; i < 1000; i++)
        {
            if(!serverDevice.StatusText.Debug($"Message {i}"))
            {
                isOverloaded = true;
                
                output.WriteLine($"Breaked on: {i}");
                
                overloadIndex = i;
                
                break;
            }
        }
        
        Assert.True(isOverloaded);
        
        Assert.Equal(101, overloadIndex);
        
        var message = await clientDevice.StatusText.OnMessage.FirstAsync();
        
        output.WriteLine($"Client status message: {message.Text}");
        
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
        var fake = new FakeTimeProvider();
        var serverDevice = new ServerDevice(link.Server, new PacketSequenceCalculator(), 
            serverId,
            new ServerDeviceConfig()
            {
                Heartbeat = new MavlinkHeartbeatServerConfig()
                {
                    HeartbeatRateMs = 100
                }
            },fake);
        
        var clientDevice = new AbstractClientDevice(link.Client, clientId,
            new ClientDeviceBaseConfig(), new PacketSequenceCalculator(),fake);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        fake.Advance(TimeSpan.FromSeconds(3));

        var linkQuality = clientDevice.Heartbeat.LinkQuality.Value;
        
        output.WriteLine($"Client connection quality: {linkQuality}");
        
        output.WriteLine($"Rx packets: {clientDevice.Connection.RxPackets}");
        
        output.WriteLine($"Tx packets: {serverDevice.Connection.TxPackets}");
        
        Assert.InRange(linkQuality,0.4,0.7); 
        output.WriteLine($"Linq quality {linkQuality:F2}");
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
        var fake = new FakeTimeProvider();
        var serverDevice = new ServerDevice(link.Server, new PacketSequenceCalculator(),
            serverId, new ServerDeviceConfig(),fake);
        
        var clientDevice = new AbstractClientDevice(link.Client, clientId,
            new ClientDeviceBaseConfig(), new PacketSequenceCalculator(),fake);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        fake.Advance(TimeSpan.FromSeconds(5));
        
        var linkStatus = await clientDevice.Heartbeat.Link.FirstAsync();
        
        Assert.Equal(LinkState.Connected, linkStatus);
        
        output.WriteLine($"Client connection state: {linkStatus}");

        canSend = false;
        
        fake.Advance(TimeSpan.FromSeconds(11));
        
        linkStatus = await clientDevice.Heartbeat.Link.FirstAsync();
        
        Assert.Equal(LinkState.Disconnected, linkStatus);
        
        output.WriteLine($"Client connection state: {linkStatus}");
        
        canSend = true;
        
        fake.Advance(TimeSpan.FromSeconds(4));
        
        linkStatus = await clientDevice.Heartbeat.Link.FirstAsync();
        
        Assert.Equal(LinkState.Connected, linkStatus);
        
        output.WriteLine($"Client connection state: {linkStatus}");
        
        canSend = false;
        
        fake.Advance(TimeSpan.FromSeconds(2.5));
        
        linkStatus = await clientDevice.Heartbeat.Link.FirstAsync();
        
        Assert.Equal(LinkState.Downgrade, linkStatus);
        
        output.WriteLine($"Client connection state: {linkStatus}");
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
        var fake = new FakeTimeProvider();
        var serverDevice = new ServerDevice(link.Server, new PacketSequenceCalculator(),
            serverId, new ServerDeviceConfig(),fake);
        
        serverDevice.Heartbeat.Set(_ => _.CustomMode = (uint)mode);
        
        var clientDevice = new AbstractClientDevice(link.Client, clientId,
            new ClientDeviceBaseConfig(), new PacketSequenceCalculator(),fake);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        fake.Advance(TimeSpan.FromSeconds(3));
        
        var clientState = await clientDevice.Heartbeat.RawHeartbeat.FirstAsync();
        
        Assert.Equal((uint)mode, clientState.CustomMode);
        
        output.WriteLine($"Server custom mode: {mode}");
        
        output.WriteLine($"Client custom mode: {clientState.CustomMode}");
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

        var fake = new FakeTimeProvider();
        var serverDevice = new ServerDevice(link.Server, new PacketSequenceCalculator(),
            serverId, new ServerDeviceConfig(),fake);

        var clientDevice = new AbstractClientDevice(link.Client,
            clientId,
            new ClientDeviceBaseConfig(), new PacketSequenceCalculator(),fake);

        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        fake.Advance(TimeSpan.FromSeconds(4.1));

        Assert.NotEqual(serverDevice.Connection.TxPackets, clientDevice.Connection.RxPackets);
        
        output.WriteLine($"Total packets count: {packetsCount}");
        
        output.WriteLine($"Server tx packets: {serverDevice.Connection.TxPackets}");
        
        output.WriteLine($"Client rx packets: {clientDevice.Connection.RxPackets}");
    }
    
    private AbstractClientDevice CreateClientDevice(VirtualMavlinkConnection link, PacketSequenceCalculator seq, 
        MavlinkClientIdentity identity, ClientDeviceBaseConfig cfg)
    {
        return new AbstractClientDevice(link.Client,
            identity, cfg, seq);
    }
    
    private ServerDevice CreateServerDevice(VirtualMavlinkConnection link, PacketSequenceCalculator seq, 
        MavlinkIdentity identity, ServerDeviceConfig cfg)
    {
        return new ServerDevice(link.Server,
            seq, identity, cfg);
    }

}

public class AbstractClientDevice : ClientDevice, IClientDevice
{
    public AbstractClientDevice(IMavlinkV2Connection connection, 
        MavlinkClientIdentity identity, 
        ClientDeviceBaseConfig config, 
        IPacketSequenceCalculator seq,
        TimeProvider? timeProvider = null,
        IScheduler? rxScheduler = null,
        ILoggerFactory? logFactory = null) 
        : base(connection, identity, config, seq,timeProvider,rxScheduler,logFactory)
    {
        
    }

    protected override string DefaultName => "TEST";

    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }


    public override DeviceClass Class => DeviceClass.Unknown;
}


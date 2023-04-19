using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Xunit;
using Xunit.Abstractions;
using MavCmd = Asv.Mavlink.V2.AsvGbs.MavCmd;

namespace Asv.Mavlink.Test;

public class BaseDevicesTest
{
    private readonly ITestOutputHelper _output;

    public BaseDevicesTest(ITestOutputHelper output)
    {
        _output = output;
        
        // var link = new VirtualLink();
        //
        // var serverDevice = new ServerDevice(link.Server,
        //     new PacketSequenceCalculator(),
        //     new MavlinkServerIdentity(),
        //     new ServerDeviceConfig(),
        //     Scheduler.Default);
        //
        // var clientDevice = new AbstractClientDevice(link.Server,
        //     new MavlinkClientIdentity(),
        //     new ClientDeviceConfig(),
        //     new PacketSequenceCalculator(),
        //     Scheduler.Default);
    }

    [Theory]
    [InlineData("The quick brown fox jumps over the lazy dog")]
    [InlineData("Съешь же ещё этих мягких французских булок, да выпей чаю")]
    [InlineData("/><#$@!%^&*()-=+_?/.,][[~`';:\"|^№\\")]
    [InlineData("1234567890")]
    public async Task BaseDevicesSimpleStatusTextTest(string messageText)
    {
        var link = new VirtualLink();

        var serverDevice = new ServerDevice(link.Server,
            new PacketSequenceCalculator(),
            new MavlinkServerIdentity(),
            new ServerDeviceConfig(),
            Scheduler.Default);
        
        var clientDevice = new AbstractClientDevice(link.Client,
            new MavlinkClientIdentity(),
            new ClientDeviceConfig(),
            new PacketSequenceCalculator(),
            Scheduler.Default);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        await Task.Delay(2000);
        
        serverDevice.StatusText.Log(MavSeverity.MavSeverityDebug, messageText);
        
        var message = await clientDevice.StatusText.OnMessage.FirstAsync();
        
        _output.WriteLine($"Client status message: {message.Text}");
        
        Assert.Equal(messageText, message.Text);
    }

    
    [Fact]
    public async Task BaseDevicesHeartbeatPacketRateTest()
    {
        var link = new VirtualLink();

        var serverDevice = new ServerDevice(link.Server,
            new PacketSequenceCalculator(),
            new MavlinkServerIdentity(),
            new ServerDeviceConfig(),
            Scheduler.Default);
        
        var clientDevice = new AbstractClientDevice(link.Client,
            new MavlinkClientIdentity(),
            new ClientDeviceConfig(),
            new PacketSequenceCalculator(),
            Scheduler.Default);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        await Task.Delay(4000);
        
        var packetRate = await clientDevice.Heartbeat.PacketRateHz.FirstAsync();
        
        Assert.True(packetRate > 0);
        _output.WriteLine($"Client connection quality: {packetRate}");
    }
    
    [Fact]
    public async Task BaseDevicesHeartbeatConnectionQualityTest()
    {
        var link = new VirtualLink();

        var serverDevice = new ServerDevice(link.Server,
            new PacketSequenceCalculator(),
            new MavlinkServerIdentity(),
            new ServerDeviceConfig(),
            Scheduler.Default);
        
        var clientDevice = new AbstractClientDevice(link.Client,
            new MavlinkClientIdentity(),
            new ClientDeviceConfig(),
            new PacketSequenceCalculator(),
            Scheduler.Default);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        await Task.Delay(2000);
        
        var linkQuality = await clientDevice.Heartbeat.LinkQuality.FirstAsync();
        
        Assert.NotEqual(0, linkQuality);
        _output.WriteLine($"Client connection quality: {linkQuality}");
    }
    
    [Fact]
    public async Task BaseDevicesHeartbeatConnectionStateTest()
    {
        var link = new VirtualLink();

        var serverDevice = new ServerDevice(link.Server,
            new PacketSequenceCalculator(),
            new MavlinkServerIdentity(),
            new ServerDeviceConfig(),
            Scheduler.Default);
        
        var clientDevice = new AbstractClientDevice(link.Client,
            new MavlinkClientIdentity(),
            new ClientDeviceConfig(),
            new PacketSequenceCalculator(),
            Scheduler.Default);
        
        serverDevice.Start();
        
        clientDevice.WaitUntilConnect();
        
        await Task.Delay(2000);
        
        var linkStatus = await clientDevice.Heartbeat.Link.FirstAsync();
        
        Assert.Equal(LinkState.Connected, linkStatus);
        _output.WriteLine($"Client connection state: {linkStatus}");
    }

    [Fact]
    public async Task BaseDevicesCustomModeTest()
    {
        var link = new VirtualLink();
        var mode = 23;
        
        var serverDevice = new ServerDevice(link.Server,
            new PacketSequenceCalculator(),
            new MavlinkServerIdentity(),
            new ServerDeviceConfig(),
            Scheduler.Default);
        
        serverDevice.Heartbeat.Set(_ => _.CustomMode = (uint)mode);
        
        var clientDevice = new AbstractClientDevice(link.Client,
            new MavlinkClientIdentity(),
            new ClientDeviceConfig(),
            new PacketSequenceCalculator(),
            Scheduler.Default);
        
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
        
        var link = new VirtualLink(_ => true,_ => ++packetsCount > lostPacketsCount);

        var serverDevice = new ServerDevice(link.Server,
            new PacketSequenceCalculator(),
            new MavlinkServerIdentity { ComponentId = 13, SystemId = 13 },
            new ServerDeviceConfig(),
            Scheduler.Default);

        var clientDevice = new AbstractClientDevice(link.Client,
            new MavlinkClientIdentity { ComponentId = 1, SystemId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new ClientDeviceConfig(),
            new PacketSequenceCalculator(),
            Scheduler.Default);

        serverDevice.Start();
        clientDevice.WaitUntilConnect();
        await Task.Delay(5000);

        Assert.NotEqual(serverDevice.Connection.TxPackets, clientDevice.Connection.RxPackets);
        
        _output.WriteLine($"Total packets count: {packetsCount}");
        _output.WriteLine($"Server tx packets: {serverDevice.Connection.TxPackets}");
        _output.WriteLine($"Client rx packets: {clientDevice.Connection.RxPackets}");
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

    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }

    protected override Task<string> GetCustomName(CancellationToken cancel)
    {
        return Task.FromResult("TEST");
    }

    public override DeviceClass Class => DeviceClass.Unknown;
}


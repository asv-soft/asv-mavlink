using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class ServerDeviceTests(ITestOutputHelper log):ServerTestBase<ServerDevice>(log)
{
    protected override ServerDevice CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public void Ctor_WithDefaultArgs_Success()
    {
        var link = new VirtualMavlinkConnection();
        var seq = new PacketSequenceCalculator();
        var time = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        var core = new CoreServices(link.Server,seq,NullLoggerFactory.Instance, time, meter);
        var device = new ServerDevice(new MavlinkIdentity(3,4), new ServerDeviceConfig(),core);
        Assert.NotNull(device);
    }
    
    [Fact]
    public void Ctor_WithNullIdentity_ThrowException()
    {
        var link = new VirtualMavlinkConnection();
        var seq = new PacketSequenceCalculator();
        var time = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        var core = new CoreServices(link.Server,seq,NullLoggerFactory.Instance, time, meter);
        Assert.Throws<ArgumentNullException>(() => new ServerDevice(null!, new ServerDeviceConfig(),core));
        Assert.Throws<ArgumentNullException>(() => new ServerDevice(new MavlinkIdentity(3,4), null!,core));
        Assert.Throws<ArgumentNullException>(() => new ServerDevice(new MavlinkIdentity(3,4), new ServerDeviceConfig(),null!));
    }
    
    [Fact]
    public void Heartbeat_SendMessagesAfterStart_Success()
    {
        var link = new VirtualMavlinkConnection();
        var seq = new PacketSequenceCalculator();
        var time = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        var core = new CoreServices(link.Client,seq,NullLoggerFactory.Instance, time, meter);
        var device = new ServerDevice(new MavlinkIdentity(3,4), new ServerDeviceConfig
        {
            Heartbeat =
            {
                HeartbeatRateMs = 1000,
            }
        },core);
        var comId = 0;
        var sysId = 0;
        link.Server.Filter<HeartbeatPacket>().Subscribe(x =>
        {
            sysId = x.SystemId;
            comId = x.ComponentId;
        });
        device.Start();
        Assert.Equal(0,sysId);
        Assert.Equal(0,comId);
        time.Advance(TimeSpan.FromSeconds(1.1));
        Assert.Equal(3,sysId);
        Assert.Equal(4,comId);
        
    }

    
}
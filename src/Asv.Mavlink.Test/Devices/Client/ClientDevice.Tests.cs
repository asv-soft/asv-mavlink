using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace Asv.Mavlink.Test;

public class ClientDeviceTests
{
    [Fact]
    public void Ctor_WithDefaultArgs_Success()
    {
        var link = new VirtualMavlinkConnection();
        var seq = new PacketSequenceCalculator();
        var time = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        var core = new CoreServices(link.Client,seq,NullLoggerFactory.Instance, time, meter);
        var device = new ClientDevice(new MavlinkClientIdentity(1,2,3,4), new ClientDeviceBaseConfig(),core, DeviceClass.Copter);
        Assert.NotNull(device);
    }
    
    [Fact]
    public void Ctor_WithNullArgs_Fail()
    {
        var link = new VirtualMavlinkConnection();
        var seq = new PacketSequenceCalculator();
        var time = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        var core = new CoreServices(link.Client,seq,NullLoggerFactory.Instance, time, meter);
        Assert.Throws<ArgumentNullException>(() => new ClientDevice(null, new ClientDeviceBaseConfig(),core, DeviceClass.Copter));
        Assert.Throws<ArgumentNullException>(() => new ClientDevice(new MavlinkClientIdentity(1,2,3,4), null,core, DeviceClass.Copter));
        Assert.Throws<ArgumentNullException>(() => new ClientDevice(new MavlinkClientIdentity(1,2,3,4), new ClientDeviceBaseConfig(),null, DeviceClass.Copter));
    }
    
    
}
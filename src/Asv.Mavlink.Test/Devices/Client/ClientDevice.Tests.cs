using System;
using Asv.Common;
using Asv.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ClientDevice))]
public class ClientDeviceTests(ITestOutputHelper log) : ClientTestBase<ClientDevice>(log)
{
    readonly ClientDeviceConfig _config = new ClientDeviceConfig
    {
        Heartbeat = new HeartbeatClientConfig
        {
            HeartbeatTimeoutMs = 2000,
            LinkQualityWarningSkipCount = 3,
            RateMovingAverageFilter = 10
        }
    };
    
    protected override ClientDevice CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new(identity,_config ,core, DeviceClass.Copter);
    }
    [Fact]
    public void Ctor_WithDefaultArgs_Success()
    {
        Assert.NotNull(Client);
    }
    
    [Fact]
    public void Ctor_WithNullArgs_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => new ClientDevice(null, new ClientDeviceConfig(),Core, DeviceClass.Copter));
        Assert.Throws<ArgumentNullException>(() => new ClientDevice(new MavlinkClientIdentity(1,2,3,4), null,Core, DeviceClass.Copter));
        Assert.Throws<ArgumentNullException>(() => new ClientDevice(new MavlinkClientIdentity(1,2,3,4), new ClientDeviceConfig(),null, DeviceClass.Copter));
    }
    
    [Fact]
    public void Ctor_WithNullSubConfigArgs_Fail()
    {
        var protocol = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        });
        var link = protocol.CreateVirtualConnection();
        var seq = new PacketSequenceCalculator();
        var time = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        var core = new CoreServices(link.Client,seq,NullLoggerFactory.Instance, time, meter);
        
        Assert.Throws<ArgumentNullException>(() =>
        {
            var device = new ClientDevice(new MavlinkClientIdentity(1,2,3,4), new ClientDeviceConfig
            {
                Heartbeat = null!
            },core, DeviceClass.Copter);
        });
    }


    
}
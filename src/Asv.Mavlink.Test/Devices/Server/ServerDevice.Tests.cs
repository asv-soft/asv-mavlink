using System;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Minimal;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;
using R3;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(IServerDevice))]
public class ServerDeviceTests(ITestOutputHelper log) : ServerTestBase<IServerDevice>(log)
{
    protected override IServerDevice CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return ServerDevice.Create(identity,core, builder =>
        {
            builder.RegisterHeartbeat();
        }); 
    }

    [Fact]
    public void Ctor_WithDefaultArgs_Success()
    {
        var protocol = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        });
        var link = protocol.CreateVirtualConnection();
        var seq = new PacketSequenceCalculator();
        var time = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        var core = new CoreServices(link.Server, seq, NullLoggerFactory.Instance, time, meter);
        var device = ServerDevice.Create(new MavlinkIdentity(1,2) ,core, builder =>
        {
            builder.RegisterHeartbeat();
        }); 
        Assert.NotNull(device);
    }

    [Fact]
    public void Ctor_WithNullIdentity_ThrowException()
    {
        var protocol = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        });
        var link = protocol.CreateVirtualConnection();
        var seq = new PacketSequenceCalculator();
        var time = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        var core = new CoreServices(link.Server, seq, NullLoggerFactory.Instance, time, meter);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Assert.Throws<ArgumentNullException>(() => ServerDevice.Create(new MavlinkIdentity(1,2), (IMavlinkContext)null, _=>{}));
        Assert.Throws<ArgumentNullException>(() => ServerDevice.Create(new MavlinkIdentity(1,2), (IProtocolConnection)null, _=>{}));
        Assert.Throws<ArgumentNullException>(() => ServerDevice.Create(null, core, builder=>{}));
        Assert.Throws<ArgumentNullException>(() => ServerDevice.Create(new MavlinkIdentity(1,2), core, null));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        
    }

    [Fact]
    public void Heartbeat_SendMessagesAfterStart_Success()
    {
        var protocol = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        });
        var link = protocol.CreateVirtualConnection();
        var seq = new PacketSequenceCalculator();
        var time = new FakeTimeProvider();
        var meter = new DefaultMeterFactory();
        var core = new CoreServices(link.Client, seq, NullLoggerFactory.Instance, time, meter);
        var device = ServerDevice.Create(new MavlinkIdentity(3,4),core, builder =>
        {
            builder.RegisterHeartbeat(new MavlinkHeartbeatServerConfig{ HeartbeatRateMs = 1000});
        });
        device.Start();
        var comId = 0;
        var sysId = 0;
        link.Server.RxFilterByType<HeartbeatPacket>().Subscribe(x =>
        {
            sysId = x.SystemId;
            comId = x.ComponentId;
        });
        device.Start();
        Assert.Equal(0, sysId);
        Assert.Equal(0, comId);
        time.Advance(TimeSpan.FromSeconds(1.1));
        Assert.Equal(3, sysId);
        Assert.Equal(4, comId);
    }
    
}
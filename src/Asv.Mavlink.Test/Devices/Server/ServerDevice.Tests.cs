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

[TestSubject(typeof(ServerDevice))]
public class ServerDeviceTests(ITestOutputHelper log) : ServerTestBase<ServerDevice>(log)
{
    protected override ServerDevice CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new ServerDevice(identity,
            new ServerDeviceConfig
            {
                Heartbeat = new MavlinkHeartbeatServerConfig { HeartbeatRateMs = 1000 },
                StatusText = new StatusTextLoggerConfig { MaxQueueSize = 100, MaxSendRateHz = 10 }
            }, core);
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
        var device = new ServerDevice(new MavlinkIdentity(3, 4), new ServerDeviceConfig(), core);
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
        Assert.Throws<ArgumentNullException>(() => new ServerDevice(null!, new ServerDeviceConfig(), core));
        Assert.Throws<ArgumentNullException>(() => new ServerDevice(new MavlinkIdentity(3, 4), null!, core));
        Assert.Throws<ArgumentNullException>(() =>
            new ServerDevice(new MavlinkIdentity(3, 4), new ServerDeviceConfig(), null!));
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
        var device = new ServerDevice(new MavlinkIdentity(3, 4), new ServerDeviceConfig
        {
            Heartbeat =
            {
                HeartbeatRateMs = 1000,
            }
        }, core);
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
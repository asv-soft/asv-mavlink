using System;
using System.Collections.Immutable;
using Asv.Common;
using Asv.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MavlinkClientDevice))]
public class ClientDeviceTests(ITestOutputHelper log) : ClientTestBase<MavlinkClientDevice>(log)
{
    readonly MavlinkClientDeviceConfig _config = new()
    {
        Heartbeat = new HeartbeatClientConfig
        {
            HeartbeatTimeoutMs = 2000,
            LinkQualityWarningSkipCount = 3,
            RateMovingAverageFilter = 10
        }
    };
    
    protected override MavlinkClientDevice CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new(new MavlinkClientDeviceId("TEST", identity),_config , ImmutableArray<IClientDeviceExtender>.Empty,  core);
    }
    [Fact]
    public void Ctor_WithDefaultArgs_Success()
    {
        Assert.NotNull(Client);
    }
    
    [Fact]
    public void Ctor_WithNullArgs_Fail()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(() => new MavlinkClientDevice(null, new MavlinkClientDeviceConfig(),ImmutableArray<IClientDeviceExtender>.Empty,Context));
        // constructor does not throw ArgumentNullException when config argument is null
        Assert.Throws<NullReferenceException>(() => new MavlinkClientDevice(new MavlinkClientDeviceId("TEST", new MavlinkClientIdentity(1,2,3,4)), null,ImmutableArray<IClientDeviceExtender>.Empty,Context));
        Assert.Throws<ArgumentNullException>(() => new MavlinkClientDevice(new MavlinkClientDeviceId("TEST", new MavlinkClientIdentity(1,2,3,4)), new MavlinkClientDeviceConfig(),ImmutableArray<IClientDeviceExtender>.Empty,null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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
        
        Assert.Throws<ArgumentNullException>(() => new MavlinkClientDevice(new MavlinkClientDeviceId("TEST", new MavlinkClientIdentity(1,2,3,4)), new MavlinkClientDeviceConfig
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Heartbeat = null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        },ImmutableArray<IClientDeviceExtender>.Empty,Context));
    }


    
}
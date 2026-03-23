using System;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MavlinkClientDevice))]
public class MavlinkClientDeviceTest(ITestOutputHelper log) 
    : MavlinkClientDeviceTestBase<MavlinkClientDevice>(log)
{
    private readonly MavlinkClientDeviceConfig _config = new()
    {
        Heartbeat = new HeartbeatClientConfig
        {
            HeartbeatTimeoutMs = 2000,
            LinkQualityWarningSkipCount = 3,
            RateMovingAverageFilter = 10
        }
    };
    
    [Fact]
    public void Ctor_WithNullArguments_Fail()
    {
        var id = new MavlinkClientDeviceId(
            "TEST",
            new MavlinkClientIdentity(1, 2, 3, 4)
        );
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(() => 
            new MavlinkClientDevice(
                null, 
                new MavlinkClientDeviceConfig(),
                [],
                Context
            )
        );
        // constructor does not throw ArgumentNullException when config argument is null
        Assert.Throws<NullReferenceException>(() => 
            new MavlinkClientDevice(
                id, 
                null,
                [], 
                Context
            )
        );
        Assert.Throws<ArgumentNullException>(() => 
            new MavlinkClientDevice(
                id, 
                new MavlinkClientDeviceConfig(),
                [],
                null
            )
        );
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
    
    protected override MavlinkClientDevice CreateClient(
        MavlinkClientIdentity identity, 
        CoreServices core
    )
    {
        return new MavlinkClientDevice(
            new MavlinkClientDeviceId("TEST", identity),
            _config , 
            [],  
            core
        );
    }
}
using System;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(RadioClientDevice))]
public class RadioClientDeviceTest(ITestOutputHelper log) 
    : MavlinkClientDeviceTestBase<RadioClientDevice>(log)
{
    private readonly RadioClientDeviceConfig _cfg = new()
    {
        Command = new CommandProtocolConfig
        {
            CommandAttempt = 1, 
            CommandTimeoutMs = 1000,
        },
        Heartbeat = new HeartbeatClientConfig
        {
            HeartbeatTimeoutMs = 1000,
        },
        Params = new ParamsClientExConfig
        {
            ChunkUpdateBufferMs = 1000, 
            ReadAttemptCount = 1,
        },
    };

    [Fact]
    public void Ctor_WithNullConfig_Fail()
    {
        var id =
            new MavlinkClientDeviceId(RadioClientDevice.DeviceClass, new MavlinkClientIdentity(1,2,3,4));
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<NullReferenceException>(() =>
        {
            _ = new RadioClientDevice(id, null,[], Context);
        });
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
    
    protected override RadioClientDevice CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new RadioClientDevice(
            new MavlinkClientDeviceId(
                RadioClientDevice.DeviceClass,
                identity
            ),
            _cfg,
            [], 
            core
        );
    }
}
using System;
using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Test.Client;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class RadioDeviceClientTests : ClientTestBase<RadioClientDevice>
{
    private RadioClientDeviceConfig _cfg;

    public RadioDeviceClientTests(ITestOutputHelper log) : base(log)
    {
        _cfg = new RadioClientDeviceConfig()
        {
            Command = new CommandProtocolConfig() { CommandAttempt = 1, CommandTimeoutMs = 1000 },
            Heartbeat = new HeartbeatClientConfig { HeartbeatTimeoutMs = 1000 },
            Params = new ParamsClientExConfig() { ChunkUpdateBufferMs = 1000, ReadAttemptCount = 1 },
        };
    }

    [Fact]
    public void Client_CreateClient_Success()
    {
        var radio = CreateClient(Identity, Context);
        Assert.NotNull(radio);
    }

    [Fact]
    public void Ctor_TryPassEmptyValues_Fail()
    {
        var id =
            new MavlinkClientDeviceId(RadioClientDevice.DeviceClass, new MavlinkClientIdentity(1,2,3,4));
        Assert.Throws<ArgumentNullException>( () =>
        {
            var radio = new RadioClientDevice(null,_cfg, ImmutableArray<IClientDeviceExtender>.Empty,  Context);
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            var radio = new RadioClientDevice(id, null,ImmutableArray<IClientDeviceExtender>.Empty, Context);
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            var radio = new RadioClientDevice(id, _cfg,ImmutableArray<IClientDeviceExtender>.Empty, null);
        });
    }
    
    protected override RadioClientDevice CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        var cfg = _cfg ?? new RadioClientDeviceConfig();
        return new RadioClientDevice(new MavlinkClientDeviceId(RadioClientDevice.DeviceClass,identity), cfg,ImmutableArray<IClientDeviceExtender>.Empty, core);
    }
}
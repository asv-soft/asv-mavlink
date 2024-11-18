using System;
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
            SerialNumberParamName = "TEST_PARAM_NAME",
            RequestInitDataDelayAfterFailMs = 1000
        };
    }

    [Fact]
    public void Client_CreateClient_Success()
    {
        var radio = CreateClient(Identity, Core);
        Assert.NotNull(radio);
    }

    [Fact]
    public void Ctor_TryPassEmptyValues_Fail()
    {
        RadioClientDeviceConfig cfg = null;
        CoreServices? core = null;
        MavlinkClientIdentity? identity = null;
        Assert.Throws<ArgumentNullException>( () =>
        {
            var radio = new RadioClientDevice(identity,_cfg,Core);
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            var radio = new RadioClientDevice(Identity, cfg, Core);
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            var radio = new RadioClientDevice(Identity, _cfg, core);
        });
    }
    
    protected override RadioClientDevice CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        var cfg = _cfg ?? new RadioClientDeviceConfig();
        return new RadioClientDevice(identity, cfg, core);
    }
}
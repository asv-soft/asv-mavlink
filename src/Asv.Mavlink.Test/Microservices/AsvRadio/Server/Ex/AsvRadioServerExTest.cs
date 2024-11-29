using System.Collections.Generic;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvRadio;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Ex;

[TestSubject(typeof(AsvRadioServerEx))]
public class AsvRadioServerExTest(ITestOutputHelper log) : ServerTestBase<AsvRadioServerEx>(log)
{
    private readonly AsvRadioCapabilities _capabilities = AsvRadioCapabilities.Empty;
    private readonly IReadOnlySet<AsvAudioCodec> _codecs = new HashSet<AsvAudioCodec>();

    private readonly AsvRadioServerConfig _radioConfig = new()
    {
        StatusRateMs = 1000
    };

    private readonly MavlinkHeartbeatServerConfig _heartbeatConfig = new()
    {
        HeartbeatRateMs = 1000
    };

    private readonly StatusTextLoggerConfig _statusConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 100
    };

    protected override AsvRadioServerEx CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        var srv = new AsvRadioServer(identity, _radioConfig, core);
        var hb = new HeartbeatServer(identity, _heartbeatConfig, core);
        var cmd = new CommandLongServerEx(new CommandServer(identity, core));
        var status = new StatusTextServer(identity, _statusConfig, core);
        return new AsvRadioServerEx(_capabilities, _codecs, srv, hb, cmd, status);
    }

    [Fact]
    public void Ctor_ServerCreatesCorrect_Success()
    {
        //Arrange & Act
        var server = Server;

        //Assert
        Assert.NotNull(server);
        Assert.NotNull(server.Base);
        Assert.Equal(AsvRadioCustomMode.AsvRadioCustomModeIdle, server.CustomMode.CurrentValue);
    }
}
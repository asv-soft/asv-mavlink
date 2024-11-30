using System;
using System.Collections.Generic;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvRadio;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Ex;

[TestSubject(typeof(AsvRadioServerEx))]
public class AsvRadioServerExTest(ITestOutputHelper log) : ServerTestBase<AsvRadioServerEx>(log)
{
    private readonly AsvRadioCapabilities _capabilities = AsvRadioCapabilities.Empty;
    private readonly IReadOnlySet<AsvAudioCodec> _codecs = new HashSet<AsvAudioCodec>();
    private IDisposable? _dispose;

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
        var builder = Disposable.CreateBuilder();
        var srv = new AsvRadioServer(identity, _radioConfig, core).AddTo(ref builder);
        var hb = new HeartbeatServer(identity, _heartbeatConfig, core).AddTo(ref builder);
        var cmd = new CommandLongServerEx(new CommandServer(identity, core)).AddTo(ref builder);
        var status = new StatusTextServer(identity, _statusConfig, core).AddTo(ref builder);
        _dispose = builder.Build();
        return new AsvRadioServerEx(_capabilities, _codecs, srv, hb, cmd, status);
    }

    [Fact]
    public void Ctor_ServerCreatesCorrect_Success()
    {
        //Arrange & Act
        using var server = Server;
        Assert.NotNull(server);
        Assert.NotNull(server.Base);
        Assert.Equal(AsvRadioCustomMode.AsvRadioCustomModeIdle, server.CustomMode.CurrentValue);
        _dispose?.Dispose();
    }
    
  
}
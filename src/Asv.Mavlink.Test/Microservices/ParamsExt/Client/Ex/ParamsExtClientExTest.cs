using System;
using System.Collections.Generic;
using System.Threading;
using Asv.Mavlink.V2.Common;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsExtClientEx))]
public class ParamsExtClientExTest : ClientTestBase<ParamsExtClientEx>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private List<ParamDescription> _existDescription;
    private readonly IMavParamEncoding _encoding;
    private readonly ParamsExtClientEx _clientEx;
    private ParamsExtClient _client;
    
    private readonly ParamsExtClientExConfig _config = new()
    {
        ReadAttemptCount = 5,
        ReadTimeouMs = 500,
        ReadListTimeoutMs = 100,
        ChunkUpdateBufferMs = 100
    };

    private IEnumerable<ParamExtDescription> ParamDescription { get; } = new List<ParamExtDescription>
    {
        new() { Name = "WP_RADIUS", ParamExtType = MavParamExtType.MavParamExtTypeUint8 },
        new() { Name = "FS_GCS_ENABL", ParamExtType = MavParamExtType.MavParamExtTypeInt8 },
        new() { Name = "RC_MAP_THROTTLE", ParamExtType = MavParamExtType.MavParamExtTypeUint16 },
        new() { Name = "RC1_TRIM", ParamExtType = MavParamExtType.MavParamExtTypeInt16 },
        new() { Name = "ARMING_CHECK", ParamExtType = MavParamExtType.MavParamExtTypeUint32 },
        new() { Name = "SERVO1_MIN", ParamExtType = MavParamExtType.MavParamExtTypeInt32 },
        new() { Name = "INS_FAST_SAMPLE", ParamExtType = MavParamExtType.MavParamExtTypeUint64 },
        new() { Name = "SCHED_LOOP_RATE", ParamExtType = MavParamExtType.MavParamExtTypeInt64 },
        new() { Name = "PITCH_MAX", ParamExtType = MavParamExtType.MavParamExtTypeReal32 },
        new() { Name = "GND_ABS_PRESS", ParamExtType = MavParamExtType.MavParamExtTypeReal64 },
        new() { Name = "SERIAL1_PROTOCOL", ParamExtType = MavParamExtType.MavParamExtTypeCustom },
    };

    protected override ParamsExtClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        var client = new ParamsExtClient(identity, _config, core);
        return new ParamsExtClientEx(client, _config, ParamDescription, _encoding);
    }


    public ParamsExtClientExTest(ITestOutputHelper log) : base(log)
    {
        _encoding = new MavParamByteWiseEncoding();
        _clientEx = Client;
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
    }
    
    [Fact]
    public void Constructor_Null_Throws()
    {
        Assert.Throws<NullReferenceException>(() => new ParamsExtClientEx(null!, _config, ParamDescription, _encoding));
        Assert.Throws<ArgumentNullException>(() => new ParamsExtClientEx(_client, null!, ParamDescription, _encoding));
        Assert.Throws<ArgumentNullException>(() => new ParamsExtClientEx(_client, _config, null!, _encoding));
        Assert.Throws<ArgumentNullException>(() => new ParamsExtClientEx(_client, _config, ParamDescription, null!));
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
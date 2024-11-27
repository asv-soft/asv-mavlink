using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsClientEx))]
public class ParamsClientExTest : ClientTestBase<ParamsClientEx>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private List<ParamDescription> _existDescription;
    private readonly MavParamCStyleEncoding _encoding;
    private readonly ParamsClientEx _clientEx;
    private ParamsClient _client;

    private readonly ParamsClientExConfig _config = new()
    {
        ReadTimeouMs = 1000,
        ReadAttemptCount = 3,
        ChunkUpdateBufferMs = 100,
        ReadListTimeoutMs = 500,
    };

    public ParamsClientExTest(ITestOutputHelper log) : base(log)
    {
        _encoding = new MavParamCStyleEncoding();
        _clientEx = Client;
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
    }

    protected override ParamsClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        _client = new ParamsClient(identity, _config, core);
        _existDescription = new List<ParamDescription>
        {
            new() { Name = "BARO_PRIMARY", ParamType = MavParamType.MavParamTypeUint8 },
            new() { Name = "SIM_CAN_TYPE1", ParamType = MavParamType.MavParamTypeInt8 },
            new() { Name = "FENCE_ACTION", ParamType = MavParamType.MavParamTypeUint16 },
            new() { Name = "RC7_DZ", ParamType = MavParamType.MavParamTypeInt16 },
            new() { Name = "LOG_BITMASK", ParamType = MavParamType.MavParamTypeUint32 },
            new() { Name = "WPNAV_SPEED", ParamType = MavParamType.MavParamTypeInt32 },
            new() { Name = "BARO_ALT_OFFSET", ParamType = MavParamType.MavParamTypeReal32 },
        };
        return new ParamsClientEx(_client, _config, _encoding, _existDescription);
    }

    [Fact]
    public void Constructor_Null_Throws()
    {
        Assert.Throws<NullReferenceException>(() => new ParamsClientEx(null!, _config, _encoding, _existDescription));
        Assert.Throws<ArgumentNullException>(() => new ParamsClientEx(_client, null!, _encoding, _existDescription));
        Assert.Throws<ArgumentNullException>(() => new ParamsClientEx(_client, _config, null!, _existDescription));
        Assert.Throws<ArgumentNullException>(() => new ParamsClientEx(_client, _config, _encoding, null!));
    }
    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
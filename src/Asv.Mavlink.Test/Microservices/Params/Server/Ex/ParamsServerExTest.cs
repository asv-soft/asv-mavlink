using Asv.Cfg;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsServerEx))]
public class ParamsServerExTest(ITestOutputHelper log) : ServerTestBase<ParamsServerEx>(log)
{
    private readonly StatusTextLoggerConfig _statusConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 100
    };

    private readonly ParamsServerExConfig _config = new()
    {
        SendingParamItemDelayMs = 100,
        CfgPrefix = "MAV_"
    };

    protected override ParamsServerEx CreateServer(MavlinkIdentity identity, CoreServices core) 
        => new(new ParamsServer(identity, core), new StatusTextServer(identity, _statusConfig, core) , ParamDesc, Encoding,Configuration, _config);
    private IMavParamTypeMetadata[] ParamDesc { get; set; } = [];
    private IMavParamEncoding Encoding { get; set; } = MavParamHelper.ByteWiseEncoding;
    private InMemoryConfiguration Configuration { get; set; } = new();
}
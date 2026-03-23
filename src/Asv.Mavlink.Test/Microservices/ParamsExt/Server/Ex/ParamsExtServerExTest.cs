using Asv.Cfg;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsExtServerEx))]
public class ParamsExtServerExTest(ITestOutputHelper log)
    : ServerTestBase<ParamsExtServerEx>(log)
{
    private readonly StatusTextLoggerConfig _statusConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 100,
    };
    private readonly ParamsExtServerExConfig _config = new()
    {
        SendingParamItemDelayMs = 100,
        CfgPrefix = "MAV_",
    };
    private readonly InMemoryConfiguration _configuration = new();

    protected override ParamsExtServerEx CreateServer(MavlinkIdentity identity, CoreServices core) 
        => new(
            new ParamsExtServer(identity, core), 
            new StatusTextServer(identity, _statusConfig, core), 
            ParamsExtTestHelper.ServerParamsMeta,
            _configuration,
            _config);
}
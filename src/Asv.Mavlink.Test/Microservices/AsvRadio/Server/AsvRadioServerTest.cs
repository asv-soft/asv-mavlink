using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvRadioServer))]
public class AsvRadioServerTest(ITestOutputHelper log) : ServerTestBase<AsvRadioServer>(log)
{
    private readonly AsvRadioServerConfig _config = new()
    {
        StatusRateMs = 1000
    };

    protected override AsvRadioServer CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new AsvRadioServer(identity, _config, core);
    }
}
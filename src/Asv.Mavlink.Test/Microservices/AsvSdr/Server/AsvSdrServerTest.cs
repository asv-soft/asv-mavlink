using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrServer))]
public class AsvSdrServerTest(ITestOutputHelper log) : ServerTestBase<AsvSdrServer>(log)
{
    private readonly AsvSdrServerConfig _config = new()
    {
        StatusRateMs = 1000
    };

    protected override AsvSdrServer CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new AsvSdrServer(identity, _config, core);
    }
}
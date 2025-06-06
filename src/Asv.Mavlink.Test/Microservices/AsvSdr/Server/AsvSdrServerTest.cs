using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrServer))]
public class AsvSdrServerTest(ITestOutputHelper log) : ServerTestBase<AsvSdrServer>(log)
{
    private readonly AsvSdrServerConfig _config = new()
    {
        StatusRateMs = 1000
    };

    protected override AsvSdrServer CreateServer(MavlinkIdentity identity, CoreServices core) => new(identity, _config, core);
}
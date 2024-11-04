using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvChartServer))]
public class AsvChartServerTest(ITestOutputHelper log) : ServerTestBase<AsvChartServer>(log)
{
    private readonly AsvChartServerConfig _config = new()
    {
        SendSignalDelayMs = 100,
        SendCollectionUpdateMs = 1000
    };

    protected override AsvChartServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, _config, core);
}
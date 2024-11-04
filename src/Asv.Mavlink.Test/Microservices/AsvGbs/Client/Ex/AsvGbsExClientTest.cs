using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Ex;

[TestSubject(typeof(AsvGbsExClient))]
public class AsvGbsExClientTest(ITestOutputHelper log) : ClientTestBase<AsvGbsExClient>(log)
{
    private readonly CommandProtocolConfig _commandConfig = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };
    private readonly HeartbeatClientConfig _hbConfig = new()
    {
        HeartbeatTimeoutMs = 2000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 3,
        PrintStatisticsToLogDelayMs = 10000,
        PrintLinkStateToLog = true
    };

    protected override AsvGbsExClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new AsvGbsExClient(new AsvGbsClient(identity, core),new HeartbeatClient(identity,_hbConfig,core),new CommandClient(identity, _commandConfig, core));
    }
}
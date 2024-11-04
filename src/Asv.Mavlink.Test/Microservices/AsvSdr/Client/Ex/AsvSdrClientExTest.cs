using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrClientEx))]
public class AsvSdrClientExTest(ITestOutputHelper log) : ClientTestBase<AsvSdrClientEx>(log)
{
    private readonly HeartbeatClientConfig _configHeartbeat = new()
    {
        HeartbeatTimeoutMs = 2000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 3,
        PrintStatisticsToLogDelayMs = 10_000,
        PrintLinkStateToLog = true
    };
    private readonly CommandProtocolConfig _commandConfig = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };
    private readonly AsvSdrClientExConfig _sdrConfig = new()
    {
        MaxTimeToWaitForResponseForListMs = 1000
    };
    

    protected override AsvSdrClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new AsvSdrClientEx(new AsvSdrClient(identity, core),new HeartbeatClient(identity, _configHeartbeat, core), new CommandClient(identity, _commandConfig, core), _sdrConfig);
    }
}
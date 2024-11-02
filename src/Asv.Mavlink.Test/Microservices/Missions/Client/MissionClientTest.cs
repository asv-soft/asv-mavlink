using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MissionClient))]
public class MissionClientTest(ITestOutputHelper log) : ClientTestBase<MissionClient>(log)
{
    private readonly MissionClientConfig _config = new()
    {
        CommandTimeoutMs = 1000,
        AttemptToCallCount = 5
    };

    protected override MissionClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, _config, core);
}
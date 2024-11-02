using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Ex;

[TestSubject(typeof(MissionClientEx))]
public class MissionClientExTest(ITestOutputHelper log) : ClientTestBase<MissionClientEx>(log)
{
    private readonly MissionClientExConfig _config = new()
    {
        CommandTimeoutMs = 1000,
        AttemptToCallCount = 5,
        DeviceUploadTimeoutMs = 10_000
    };

    protected override MissionClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new MissionClientEx(new MissionClient(identity, _config, core), _config);
    }
}
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrClient))]
public class AsvSdrClientTest(ITestOutputHelper log) : ClientTestBase<AsvSdrClient>(log)
{
    protected override AsvSdrClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
}
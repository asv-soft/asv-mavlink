using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvRsgaServer))]
public class AsvRsgaServerTest(ITestOutputHelper log) : ServerTestBase<AsvRsgaServer>(log)
{
    protected override AsvRsgaServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);
}
using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Server;

[TestSubject(typeof(MissionServer))]
public class MissionServerTest(ITestOutputHelper log) : ServerTestBase<MissionServer>(log)
{
    protected override MissionServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);
}
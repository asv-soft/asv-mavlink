using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Position.Client;

[TestSubject(typeof(PositionClient))]
public class PositionClientTest(ITestOutputHelper log, VirtualMavlinkConnection? link = null)
    : ClientTestBase<PositionClient>(log, link)
{
    protected override PositionClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
}
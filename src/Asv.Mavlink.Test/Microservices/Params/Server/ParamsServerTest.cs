using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Server;

[TestSubject(typeof(ParamsServer))]
public class ParamsServerTest(ITestOutputHelper log) : ServerTestBase<ParamsServer>(log)
{
    protected override ParamsServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);
}
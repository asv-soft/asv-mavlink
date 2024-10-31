using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.ParamsExt.Server;

[TestSubject(typeof(ParamsExtServer))]
public class ParamsExtServerTest(ITestOutputHelper log)
    : ServerTestBase<ParamsExtServer>(log)
{
    protected override ParamsExtServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);
}
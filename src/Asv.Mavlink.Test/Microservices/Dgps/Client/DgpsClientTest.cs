using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Dgps.Client;

[TestSubject(typeof(DgpsClient))]
public class DgpsClientTest(ITestOutputHelper log) : ClientTestBase<DgpsClient>(log)
{
    protected override DgpsClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
}
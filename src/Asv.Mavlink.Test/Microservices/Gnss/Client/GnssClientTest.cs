using JetBrains.Annotations;

using Xunit;
namespace Asv.Mavlink.Test.Client;

[TestSubject(typeof(GnssClient))]
public class GnssClientTest(ITestOutputHelper log) : ClientTestBase<GnssClient>(log)
{
    protected override GnssClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
}
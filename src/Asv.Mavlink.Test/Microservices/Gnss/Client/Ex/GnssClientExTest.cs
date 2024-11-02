using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Client.Ex;

[TestSubject(typeof(GnssClientEx))]
public class GnssClientExTest(ITestOutputHelper log) : ClientTestBase<GnssClientEx>(log)
{
    protected override GnssClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(new GnssClient(identity, core));
}
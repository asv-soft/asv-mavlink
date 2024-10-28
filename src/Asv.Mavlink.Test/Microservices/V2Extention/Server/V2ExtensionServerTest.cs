using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(V2ExtensionServer))]
public class V2ExtensionServerTest(ITestOutputHelper log, VirtualMavlinkConnection? link = null)
    : ServerTestBase<V2ExtensionServer>(log, link)
{
    protected override V2ExtensionServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);
    
    
}
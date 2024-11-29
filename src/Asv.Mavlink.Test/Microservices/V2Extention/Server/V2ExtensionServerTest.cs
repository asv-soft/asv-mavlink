using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(V2ExtensionServer))]
public class V2ExtensionServerTest(ITestOutputHelper log)
    : ServerTestBase<V2ExtensionServer>(log)
{
    protected override V2ExtensionServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);
    
    
}
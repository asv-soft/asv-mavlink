using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Client;

[TestSubject(typeof(V2ExtensionClient))]
public class V2ExtensionClientTest(ITestOutputHelper log, VirtualMavlinkConnection? link = null)
    : ClientTestBase<V2ExtensionClient>(log, link)
{

    protected override V2ExtensionClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
    [Fact]
    public void METHOD()
    {
        
    }

    
}
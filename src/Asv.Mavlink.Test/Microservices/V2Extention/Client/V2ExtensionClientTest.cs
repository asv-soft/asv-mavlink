using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Client;

[TestSubject(typeof(V2ExtensionClient))]
public class V2ExtensionClientTest(ITestOutputHelper log)
    : ClientTestBase<V2ExtensionClient>(log)
{

    protected override V2ExtensionClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
   
    
}
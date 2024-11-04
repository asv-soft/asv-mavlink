using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvRsgaClient))]
public class AsvRsgaClientTest(ITestOutputHelper log) : ClientTestBase<AsvRsgaClient>(log)
{
    protected override AsvRsgaClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
}
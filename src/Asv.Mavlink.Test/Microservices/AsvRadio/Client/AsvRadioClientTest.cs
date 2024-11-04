using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Client;

[TestSubject(typeof(AsvRadioClient))]
public class AsvRadioClientTest(ITestOutputHelper log) : ClientTestBase<AsvRadioClient>(log)
{
    protected override AsvRadioClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
}
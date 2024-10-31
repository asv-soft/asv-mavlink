using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.ParamsExt.Client;

[TestSubject(typeof(ParamsExtClient))]
public class ParamsExtClientTest(ITestOutputHelper log) : ClientTestBase<ParamsExtClient>(log)
{
    private readonly ParamsExtClientConfig _config = new ParamsExtClientExConfig
    {
        ReadAttemptCount = 5,
        ReadTimeouMs = 1000,
        ReadListTimeoutMs = 1000
    };
    protected override ParamsExtClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, _config, core);
}
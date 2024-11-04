using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvRsgaClientEx))]
public class AsvRsgaClientExTest(ITestOutputHelper log) : ClientTestBase<AsvRsgaClientEx>(log)
{
    private readonly CommandProtocolConfig _commandCore = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };

    protected override AsvRsgaClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new AsvRsgaClientEx(new AsvRsgaClient(identity, core), new CommandClient(identity, _commandCore, core));
    }
}
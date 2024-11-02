using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Commands.Client;

[TestSubject(typeof(CommandClient))]
public class CommandClientTest(ITestOutputHelper log) : ClientTestBase<CommandClient>(log)
{
    private readonly CommandProtocolConfig _config = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };

    protected override CommandClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new CommandClient(identity, _config, core);
    }
}
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Commands.Server.Ex;

[TestSubject(typeof(CommandIntServerEx))]
public class CommandLongServerExTest(ITestOutputHelper log) : ServerTestBase<CommandLongServerEx>(log)
{
    protected override CommandLongServerEx CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new CommandLongServerEx(new CommandServer(identity, core));
    }
}
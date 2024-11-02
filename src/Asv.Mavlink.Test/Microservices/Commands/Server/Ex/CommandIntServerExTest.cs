using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Commands.Server.Ex;

[TestSubject(typeof(CommandIntServerEx))]
public class CommandIntServerExTest(ITestOutputHelper log) : ServerTestBase<CommandIntServerEx>(log)
{
    protected override CommandIntServerEx CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new CommandIntServerEx(new CommandServer(identity, core));
    }
}
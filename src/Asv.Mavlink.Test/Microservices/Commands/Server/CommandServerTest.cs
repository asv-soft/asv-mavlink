using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Commands.Server;

[TestSubject(typeof(CommandServer))]
public class CommandServerTest(ITestOutputHelper log) : ServerTestBase<CommandServer>(log)
{
    protected override CommandServer CreateClient(MavlinkIdentity identity, CoreServices core) => new(identity, core);
}
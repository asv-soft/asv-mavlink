using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Logging.Server;

[TestSubject(typeof(LoggingServer))]
public class LoggingServerTest(ITestOutputHelper log) : ServerTestBase<LoggingServer>(log)
{
    protected override LoggingServer CreateServer(MavlinkIdentity identity, CoreServices core) => new(identity, core);
}
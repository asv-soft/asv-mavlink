using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Logging.Client;

[TestSubject(typeof(LoggingClient))]
public class LoggingClientTest(ITestOutputHelper log) : ClientTestBase<LoggingClient>(log)
{
    protected override LoggingClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
}
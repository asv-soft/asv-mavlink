using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Server.Ex;

[TestSubject(typeof(MissionServerEx))]
public class MissionServerExTest(ITestOutputHelper log) : ServerTestBase<MissionServerEx>(log)
{
    private readonly StatusTextLoggerConfig _config = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 30
    };

    protected override MissionServerEx CreateClient(MavlinkIdentity identity, CoreServices core) => new(new MissionServer(identity, core), new StatusTextServer(identity, _config, core));
}
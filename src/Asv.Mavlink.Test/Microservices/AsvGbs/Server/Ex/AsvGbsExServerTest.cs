using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Ex;

[TestSubject(typeof(AsvGbsExServer))]
public class AsvGbsExServerTest(ITestOutputHelper log) : ServerTestBase<AsvGbsExServer>(log)
{
    private readonly AsvGbsServerConfig _config = new()
    {
        StatusRateMs = 1000
    };
    private readonly MavlinkHeartbeatServerConfig _heartbeatConfig = new()
    {
        HeartbeatRateMs = 1000
    };

    protected override AsvGbsExServer CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new(new AsvGbsServer(identity,_config,core),new HeartbeatServer(identity,_heartbeatConfig,core),new CommandLongServerEx(new CommandServer(identity, core)));
    }
}
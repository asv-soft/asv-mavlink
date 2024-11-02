using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class HeartbeatServerTest(ITestOutputHelper log) : ServerTestBase<HeartbeatServer>(log)
{
    private readonly MavlinkHeartbeatServerConfig _config = new()
    {
        HeartbeatRateMs = 1000,
    };

    protected override HeartbeatServer CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new HeartbeatServer(identity,_config,core);
    }
}
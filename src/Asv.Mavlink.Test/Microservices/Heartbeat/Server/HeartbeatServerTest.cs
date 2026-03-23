using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class HeartbeatServerTest(ITestOutputHelper log) : ServerTestBase<HeartbeatServer>(log)
{
    private readonly MavlinkHeartbeatServerConfig _config = new();

    protected override HeartbeatServer CreateServer(MavlinkIdentity identity, CoreServices core)
    {
        return new HeartbeatServer(identity, _config, core);
    }
}
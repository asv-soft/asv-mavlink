using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpServer))]
public class FtpServerTest(ITestOutputHelper log) : ServerTestBase<FtpServer>(log)
{
    private readonly MavlinkFtpServerConfig _config = new()
    {
        NetworkId = 0,
        BurstReadChunkDelayMs = 100
    };

    protected override FtpServer CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new FtpServer(identity, _config, core);
    }
}
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpClient))]
public class FtpClientTest(ITestOutputHelper log) : ClientTestBase<FtpClient>(log)
{
    private readonly MavlinkFtpClientConfig _config = new()
    {
        TimeoutMs = 1000,
        CommandAttemptCount = 5,
        TargetNetworkId = 0,
        BurstTimeoutMs = 1000
    };

    protected override FtpClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new FtpClient(identity, _config, core);
    }
}
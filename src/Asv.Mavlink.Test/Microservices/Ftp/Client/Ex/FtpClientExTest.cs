using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpClientEx))]
public class FtpClientExTest(ITestOutputHelper log) : ClientTestBase<FtpClientEx>(log)
{
    private readonly MavlinkFtpClientConfig _config = new ()
    {
        TimeoutMs = 1000,
        CommandAttemptCount = 5,
        TargetNetworkId = 0,
        BurstTimeoutMs = 100
    };

    protected override FtpClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(new FtpClient(identity, _config, core));
}
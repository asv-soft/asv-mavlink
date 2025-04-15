using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpClientEx))]
public class FtpClientExTest : ClientTestBase<FtpClientEx>
{
    private readonly TaskCompletionSource _tcs = new();

    private readonly MavlinkFtpClientConfig _clientExConfig = new ()
    {
        TimeoutMs = 1000,
        CommandAttemptCount = 5,
        TargetNetworkId = 0,
        BurstTimeoutMs = 100
    };

    public FtpClientExTest(ITestOutputHelper log) 
        : base(log)
    {
        var cts = new CancellationTokenSource();
        cts.Token.Register(() => _tcs.TrySetCanceled());
    }

    protected override FtpClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new FtpClientEx(new FtpClient(identity, _clientExConfig, core));
    }
}
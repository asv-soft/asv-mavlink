using System;
using System.Threading;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsExtClient))]
public class ParamsExtClientTest : ClientTestBase<ParamsExtClient>
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ParamsExtClientConfig _config = new ParamsExtClientExConfig
    {
        ReadAttemptCount = 5,
        ReadTimeoutMs = 1000,
    };

    public ParamsExtClientTest(ITestOutputHelper log) : base(log)
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public void Constructor_NullArguments_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ParamsExtClient(null!, _config, Context));
        Assert.Throws<ArgumentNullException>(() => new ParamsExtClient(Identity, null!, Context));
        Assert.Throws<ArgumentNullException>(() => new ParamsExtClient(Identity, _config, null!));
    }
    
    protected override ParamsExtClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
        => new(identity, _config, core);
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}
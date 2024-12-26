using System;
using System.Threading;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsClient))]
public class ParamsClientTest : ClientTestBase<ParamsClient>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ParamsClient _client;
    
    private readonly ParameterClientConfig _config = new()
    {
        ReadAttemptCount = 6,
        ReadTimeouMs = 1000,
    };
    
    public ParamsClientTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override ParamsClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, _config, core);
    
    [Fact]
    public void Constructor_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ParamsClient(null!, _config, Context));
        Assert.Throws<ArgumentNullException>(() => new ParamsClient(Identity, null!, Context));
        Assert.Throws<ArgumentNullException>(() => new ParamsClient(Identity, _config, null!));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}
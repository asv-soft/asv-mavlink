using System;
using System.Threading;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsExtClient))]
public class ParamsExtClientTest : ClientTestBase<ParamsExtClient>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ParamsExtClient _client;
    
    private readonly ParamsExtClientConfig _config = new ParamsExtClientExConfig
    {
        ReadAttemptCount = 5,
        ReadTimeouMs = 1000,
        ReadListTimeoutMs = 1000
    };
    protected override ParamsExtClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, _config, core);

    public ParamsExtClientTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
    }

    [Fact]
    public void Constructor_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ParamsExtClient(null!, _config, Context));
        Assert.Throws<ArgumentNullException>(() => new ParamsExtClient(Identity, null!, Context));
        Assert.Throws<ArgumentNullException>(() => new ParamsExtClient(Identity, _config, null!));
    }
    
    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
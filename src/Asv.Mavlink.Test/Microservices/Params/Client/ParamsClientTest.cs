using System;
using System.Threading;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsClient))]
public class ParamsClientTest : ClientTestBase<ParamsClient>
{
    private readonly ParameterClientConfig _config = new()
    {
        ReadAttemptCount = 6,
        ReadTimeouMs = 1000,
    };
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    public ParamsClientTest(ITestOutputHelper log) : base(log)
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    [Fact]
    public void Constructor_NullArguments_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ParamsClient(null!, _config, Context));
        Assert.Throws<ArgumentNullException>(() => new ParamsClient(Identity, null!, Context));
        Assert.Throws<ArgumentNullException>(() => new ParamsClient(Identity, _config, null!));
    }
    
    protected override ParamsClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => 
        new(identity, _config, core);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}
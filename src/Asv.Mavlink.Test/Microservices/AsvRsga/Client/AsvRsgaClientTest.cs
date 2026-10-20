using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvRsgaClient))]
public class AsvRsgaClientTest : ClientTestBase<AsvRsgaClient>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly AsvRsgaClient _client;

    public AsvRsgaClientTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override AsvRsgaClient CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(identity, core);
    
    [Fact]
    public void Ctor_ThrowsExceptions_ArgIsNullFail()
    {
        Assert.Throws<ArgumentNullException>(() => new AsvRsgaClient(null!, Context));
        Assert.Throws<ArgumentNullException>(() => new AsvRsgaClient(Identity, null!));
    }
    
    [Fact]
    public async Task ReadAllInfo_ShouldThrowTimeout_Exception()
    {
        // Arrange
        var attempts = (uint)5;
        var timeout = 1000;
        
        // Act / Assert
        await RetryTimeoutTestHelper.AssertTimeoutAsync(
            cancel => Client.GetCompatibilities(cancel),
            Link.Client,
            Time,
            (int)attempts,
            TimeSpan.FromMilliseconds(timeout)
        );
        Assert.Equal(attempts, Link.Client.Statistic.TxMessages);
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

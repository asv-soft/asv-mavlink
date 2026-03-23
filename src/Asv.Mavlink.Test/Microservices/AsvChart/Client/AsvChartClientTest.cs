using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvChartClient))]
public class AsvChartClientTest : ClientTestBase<AsvChartClient>
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly AsvChartClientConfig _config = new()
    {
        MaxAttempts = 5,
        MaxTimeToWaitForResponseForListMs = 1000
    };

    public AsvChartClientTest(ITestOutputHelper log) : base(log)
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public void Ctor_NullArguments_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new AsvChartClient(null!, _config, Context));
        Assert.Throws<NullReferenceException>(() => new AsvChartClient(Identity, null!, Context));
        Assert.Throws<ArgumentNullException>(() => new AsvChartClient(Identity, _config, null!));
    }
    
    [Fact]
    public async Task ReadAllInfo_Timeout_Throws()
    {
        // Arrange
        using var sub = Link.Client.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            Time.Advance(TimeSpan.FromMilliseconds(_config.MaxTimeToWaitForResponseForListMs));
        });
        
        // Act
        var task = Client.ReadAllInfo(null, _cancellationTokenSource.Token);
        
        //Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.MaxAttempts, (int)Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(0u, Link.Client.Statistic.RxMessages);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
    }
    
    protected override AsvChartClient CreateClient(MavlinkClientIdentity identity, CoreServices core) =>
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
using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Client;

[TestSubject(typeof(AsvChartClient))]
public class AsvChartClientTest : ClientTestBase<AsvChartClient>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    public AsvChartClientTest(ITestOutputHelper log) : base(log)
    {
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20), TimeProvider.System);
    }
    
    private readonly AsvChartClientConfig _config = new()
    {
        MaxTimeToWaitForResponseForListMs = 1000
    };

    protected override AsvChartClient CreateClient(MavlinkClientIdentity identity, CoreServices core) =>
        new(identity, _config, core);

    [Fact]
    public void Ctor_ThrowsExceptions_ArgIsNullFail()
    {
        Assert.Throws<ArgumentNullException>(() => new AsvChartClient(null!, _config, Core));
        Assert.Throws<NullReferenceException>(() => new AsvChartClient(Identity, null!, Core));
        Assert.Throws<ArgumentNullException>(() => new AsvChartClient(Identity, _config, null!));
    }
    
    [Fact]
    public async Task ReadAllInfo_ShouldThrowTimeout_Exception()
    {
        // Act
        var attempts = 5;
        var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.ReadAllInfo(default, _cancellationTokenSource.Token);
        });

        var t2 = Task.Factory.StartNew(() =>
        {
            Time.Advance(TimeSpan.FromMilliseconds((_config.MaxTimeToWaitForResponseForListMs * attempts) + 1));
        });
        
        //Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(attempts, Link.Client.TxPackets);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
using System;
using System.Threading;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AdsbVehicleClient))]
public class AdsbVehicleClientTest : ClientTestBase<AdsbVehicleClient>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly AdsbVehicleClientConfig _config = new()
    {
        TargetTimeoutMs = 10_000,
        CheckOldDevicesMs = 3_000,
    };

    public AdsbVehicleClientTest(ITestOutputHelper output) : base(output)
    {
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    }

    protected override AdsbVehicleClient CreateClient(MavlinkClientIdentity identity, CoreServices core) =>
        new(identity, _config, core);

    [Fact]
    public void Ctor_ThrowsExceptions_ArgIsNullFail()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new AdsbVehicleClient(null, _config, Core);
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new AdsbVehicleClient(Identity, _config, null);
        });
        Assert.Throws<NullReferenceException>(() =>
        {
            var client = new AdsbVehicleClient(Identity, null, Core);
        });
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
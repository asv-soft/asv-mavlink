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
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override AdsbVehicleClient CreateClient(MavlinkClientIdentity identity, CoreServices core) =>
        new(identity, _config, core);

    [Fact]
    public void Ctor_ThrowsExceptions_ArgIsNullFail()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var client = new AdsbVehicleClient(null, _config, Context);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var client = new AdsbVehicleClient(Identity, _config, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });
        Assert.Throws<NullReferenceException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var client = new AdsbVehicleClient(Identity, null, Context);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
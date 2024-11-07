using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Test.Utils;
using Asv.Mavlink.V2.Common;
using JetBrains.Annotations;
using ObservableCollections;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class AdsbVehicleComplexTest : ComplexTestBase<AdsbVehicleClient, AdsbVehicleServer>, IDisposable
{
    private readonly TaskCompletionSource<AdsbVehiclePayload> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly AdsbVehicleClientConfig _clientConfig = new()
    {
        TargetTimeoutMs = 10000,
        CheckOldDevicesMs = 3000,
    };

    public AdsbVehicleComplexTest(ITestOutputHelper output) : base(output)
    {
        _taskCompletionSource = new TaskCompletionSource<AdsbVehiclePayload>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override AdsbVehicleServer CreateServer(MavlinkIdentity identity, ICoreServices core) =>
        new(identity, core);

    protected override AdsbVehicleClient CreateClient(MavlinkClientIdentity identity, ICoreServices core) =>
        new(identity, _clientConfig, core);

    [Fact]
    public async Task UpdateTarget_AddSingleTarget_Success()
    {
        // Arrange
        int count = 0;
        var payload = new AdsbVehiclePayload
        {
            IcaoAddress = 987654321,
        };
        Client.OnTarget.Subscribe(p => { count++; });

        // Act
        await Server.Send(p => { p.IcaoAddress = 987654321; }, _cancellationTokenSource.Token);

        // Assert
        Assert.NotNull(Client.Targets.Values.FirstOrDefault());
        Assert.Equal(payload.IcaoAddress.ToString(), Client.Targets.Values.FirstOrDefault()?.IcaoAddress.ToString());
        Assert.Equal(count, Link.Client.RxPackets);
        Assert.Equal(count, Client.Targets.Count);
    }

    [Fact]
    public async Task DeleteTargets_DeleteWhenTimeout_Success()
    {
        // Arrange
        var count = 0;
        var payload = new AdsbVehiclePayload
        {
            IcaoAddress = 987654321,
        };
        using var sub = Client.OnTarget.Subscribe(p => { count++; });

        // Act
        await Server.Send(p => { p.IcaoAddress = 987654321; }, _cancellationTokenSource.Token);
        ClientTime.Advance(TimeSpan.FromMilliseconds(10001));

        // Assert
        Assert.NotEqual(count, Client.Targets.Count);
        Assert.False(Client.Targets.ContainsKey(payload.IcaoAddress));
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
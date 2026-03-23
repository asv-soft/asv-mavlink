using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class AdsbVehicleComplexTest : ComplexTestBase<AdsbVehicleClient, AdsbVehicleServer>
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TaskCompletionSource _tcs;

    private readonly AdsbVehicleClientConfig _clientConfig;

    public AdsbVehicleComplexTest(ITestOutputHelper output) 
        : base(output)
    {
        _clientConfig = new AdsbVehicleClientConfig
        {
            TargetTimeoutMs = 10000
        };
        _clientConfig.CheckOldDevicesMs = _clientConfig.TargetTimeoutMs/10;
        _tcs = new TaskCompletionSource();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _tcs.TrySetCanceled());
    }

    [Theory]
    [InlineData(uint.MinValue)]
    [InlineData(uint.MaxValue)]
    public async Task Send_AddSingleTarget_Success(uint icao)
    {
        // Arrange
        var expectedCount = 1;
        int count = 0;
        using var sub = Client.OnTarget.Synchronize().Subscribe(_ =>
        {
            count++;
            _tcs.TrySetResult();
        });
        using var sub2 = Server.Core.Connection.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.CheckOldDevicesMs/2));
        });

        // Act
        await Server.Send(p => p.IcaoAddress = icao, _cancellationTokenSource.Token);

        // Assert
        await _tcs.Task;
        Assert.NotEmpty(Client.Targets);
        Assert.NotNull(Client.Targets.Values.FirstOrDefault());
        Assert.True(Client.Targets.ContainsKey(icao));
        Assert.Equal(expectedCount, count);
        Assert.Equal(count, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
        Assert.Equal(0u, Link.Server.Statistic.RxMessages);
        Assert.Equal(count, Client.Targets.Count);
    }

    [Fact]
    public async Task Send_AddSingleTargetAndDeleteWhenTimeout_Success()
    {
        // Arrange
        var expectedCount = 1;
        var count = 0;
        var icao = 10u;
        using var sub = Client.OnTarget.Synchronize().Subscribe(_ =>
        {
            count++;
            ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.TargetTimeoutMs+1));
            _tcs.TrySetResult();
        });
        using var sub2 = Server.Core.Connection.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.CheckOldDevicesMs/2));
        });

        // Act
        await Server.Send(p => p.IcaoAddress = icao, _cancellationTokenSource.Token);
        
        // Assert
        await _tcs.Task;
        Assert.Empty(Client.Targets);
        Assert.NotEqual(count, Client.Targets.Count);
        Assert.False(Client.Targets.ContainsKey(icao));
        Assert.Equal(expectedCount, count);
        Assert.Equal(count, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
        Assert.Equal(0u, Link.Server.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task Send_ReceiveTwoEqualPacketsAndAddOnlyOneDevice_Success()
    {
        // Arrange
        var expectedCount = 2;
        int count = 0;
        uint icao = 10;
        using var sub = Client.OnTarget.Synchronize().Subscribe(_ =>
        {
            count++;

            if (count >= 2)
            {
                _tcs.TrySetResult();
            }
        });
        using var sub2 = Server.Core.Connection.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.CheckOldDevicesMs/4));
        });

        // Act
        await Server.Send(p => p.IcaoAddress = icao, _cancellationTokenSource.Token);
        await Server.Send(p => p.IcaoAddress = icao, _cancellationTokenSource.Token);

        // Assert
        await _tcs.Task;
        Assert.NotEmpty(Client.Targets);
        Assert.Single(Client.Targets);
        Assert.NotNull(Client.Targets.Values.FirstOrDefault());
        Assert.True(Client.Targets.ContainsKey(icao));
        Assert.Equal(icao, Client.Targets[icao].IcaoAddress);
        Assert.Equal(expectedCount, count);
        Assert.Equal(count, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
        Assert.Equal(0u, Link.Server.Statistic.RxMessages);
        Assert.NotEqual(count, Client.Targets.Count);
    }
    
    [Fact]
    public async Task Send_AddSeveralDifferentVehicles_Success()
    {
        // Arrange
        int count = 0;
        uint icao1 = 10;
        uint icao2 = 11;
        uint icao3 = 12;
        var expectedCount = 3;
        using var sub = Client.OnTarget.Synchronize().Subscribe(_ =>
        {
            count++;

            if (count >= 3)
            {
                _tcs.TrySetResult();
            }
        });
        using var sub2 = Server.Core.Connection.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.CheckOldDevicesMs/6));
        });

        // Act
        await Server.Send(p => p.IcaoAddress = icao1, _cancellationTokenSource.Token);
        await Server.Send(p => p.IcaoAddress = icao2, _cancellationTokenSource.Token);
        await Server.Send(p => p.IcaoAddress = icao3, _cancellationTokenSource.Token);

        // Assert
        await _tcs.Task;
        Assert.NotEmpty(Client.Targets);
        Assert.Equal(expectedCount, Client.Targets.Count);
        Assert.Equal(expectedCount, count);
        Assert.True(Client.Targets.ContainsKey(icao1));
        Assert.True(Client.Targets.ContainsKey(icao2));
        Assert.True(Client.Targets.ContainsKey(icao3));
        Assert.Equal(icao1, Client.Targets[icao1].IcaoAddress);
        Assert.Equal(icao2, Client.Targets[icao2].IcaoAddress);
        Assert.Equal(icao3, Client.Targets[icao3].IcaoAddress);
        Assert.Equal(count, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
        Assert.Equal(0u, Link.Server.Statistic.RxMessages);
    }

    [Fact]
    public async Task Send_AddTwoVehiclesAndRemoveOnlyOneByTimeout_Success()
    {
        // Arrange
        int count = 0;
        int sendCount = 0;
        uint icao1 = 10;
        uint icao2 = 11;
        var expectedCount = 2;
        var vehiclesBeforeFirstDeletion = new List<IAdsbVehicle>();
        using var sub = Client.OnTarget.Synchronize().Subscribe(p =>
        {
            count++;
            
            if (count == 2)
            {
                vehiclesBeforeFirstDeletion = Client.Targets.Values.ToList();
                ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.TargetTimeoutMs/2));
                _tcs.TrySetResult();
            }
        });
        using var sub2 = Server.Core.Connection.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            sendCount++;

            if (sendCount == 1)
            {
                ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.CheckOldDevicesMs/10));
                return;
            }

            if (sendCount == 2)
            {
                ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.TargetTimeoutMs/2));
            }
        });

        // Act
        await Server.Send(p => p.IcaoAddress = icao1, _cancellationTokenSource.Token);
        await Server.Send(p => p.IcaoAddress = icao2, _cancellationTokenSource.Token);

        // Assert
        await _tcs.Task;
        Assert.NotEmpty(Client.Targets);
        Assert.Single(Client.Targets);
        Assert.True(Client.Targets.ContainsKey(icao2));
        Assert.Equal(icao2, Client.Targets[icao2].IcaoAddress);
        Assert.Equal(expectedCount, vehiclesBeforeFirstDeletion.Count);
        Assert.Equal(icao1, vehiclesBeforeFirstDeletion[0].IcaoAddress);
        Assert.Equal(icao2, vehiclesBeforeFirstDeletion[1].IcaoAddress);
        Assert.Equal(expectedCount, count);
        Assert.Equal(count, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
        Assert.Equal(0u, Link.Server.Statistic.RxMessages);
    }
    
    protected override AdsbVehicleServer CreateServer(MavlinkIdentity identity, IMavlinkContext core) =>
        new(identity, core);

    protected override AdsbVehicleClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core) =>
        new(identity, _clientConfig,core);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}
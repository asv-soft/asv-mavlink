using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsExtClientEx))]
public class ParamsExtClientExTest : ClientTestBase<ParamsExtClientEx>
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ParamsExtClientExConfig _config = new()
    {
        ReadAttemptCount = 5,
        ReadTimeoutMs = 100,
        ChunkUpdateBufferMs = 100
    };
    
    public ParamsExtClientExTest(ITestOutputHelper log) : base(log)
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    [Fact]
    public void Constructor_NullArguments_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new ParamsExtClientEx(null!, _config, ParamsExtTestHelper.ClientParamsDescriptions));
        Assert.Throws<ArgumentNullException>(
            () => new ParamsExtClientEx(Client.Base, null!, ParamsExtTestHelper.ClientParamsDescriptions));
        Assert.Throws<ArgumentNullException>(
            () => new ParamsExtClientEx(Client.Base, _config, null!));
    }
    
    [Fact]
    public async Task ReadOnce_Timeout_Throws()
    {
        // Arrange
        var paramName = ParamsExtTestHelper.ClientParamsDescriptions.First().Name;
        
        var called = 0;
        using var s1 = Link.Client.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.ReadTimeoutMs + 1));
        });
        
        // Act
        var task = Client.ReadOnce(paramName, _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.ReadAttemptCount, called);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ReadAll_Timeout_Throws()
    {
        // Arrange
        var called = 0;
        using var s1 = Link.Client.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.ReadTimeoutMs * (_config.ReadAttemptCount + 1) + 1));
        });
        
        // Act
        var result = await Client.ReadAll(null, _cancellationTokenSource.Token);
        
        // Assert
        Assert.False(result);
        Assert.Equal(1, called);
        Assert.Equal(0, (int) Link.Server.Statistic.TxMessages);
        Assert.Equal(0, (int) Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task WriteOnce_Timeout_Throws()
    {
        // Arrange
        var paramName = ParamsExtTestHelper.ServerParamsMeta.First().Name;
        var paramValue = ParamsExtTestHelper.ServerParamsMeta.First().DefaultValue;
        
        var called = 0;
        using var s1 = Link.Client.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.ReadTimeoutMs + 1));
        });
        
        // Act
        var task = Client.WriteOnce(paramName, paramValue, _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.ReadAttemptCount, called);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(_config.ReadAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    protected override ParamsExtClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        var client = new ParamsExtClient(identity, _config, core);
        return new ParamsExtClientEx(client, _config, ParamsExtTestHelper.ClientParamsDescriptions);
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
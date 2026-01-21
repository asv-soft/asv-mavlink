using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ParamsClientEx))]
public class ParamsClientExTest : ClientTestBase<ParamsClientEx>
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ParamsClientExConfig _config = new()
    {
        ReadTimeouMs = 100,
        ReadAttemptCount = 3,
        ChunkUpdateBufferMs = 100,
    };
    
    public ParamsClientExTest(ITestOutputHelper log) : base(log)
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public void Constructor_NullArguments_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new ParamsClientEx(null!, _config, ParamsMeta.Encoding, ParamsMeta.ClientParamsDescriptions));
        Assert.Throws<ArgumentNullException>(
            () => new ParamsClientEx(Client.Base, null!, ParamsMeta.Encoding, ParamsMeta.ClientParamsDescriptions));
        Assert.Throws<ArgumentNullException>(
            () => new ParamsClientEx(Client.Base, _config, null!, ParamsMeta.ClientParamsDescriptions));
        Assert.Throws<ArgumentNullException>(
            () => new ParamsClientEx(Client.Base, _config, ParamsMeta.Encoding, null!));
    }
    
    [Fact]
    public async Task ReadOnce_Timeout_Throws()
    {
        // Arrange
        var paramName = ParamsMeta.ClientParamsDescriptions.First().Name;
        
        var called = 0;
        using var s1 = Link.Client.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.ReadTimeouMs + 1));
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
            Time.Advance(TimeSpan.FromMilliseconds(_config.ReadTimeouMs * (_config.ReadAttemptCount + 1) + 1));
        });
        
        // Act
        await Client.ReadAll(null, false, _cancellationTokenSource.Token);
        
        // Assert
        Assert.Equal(1, called);
        Assert.Equal(0, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(0, (int)Link.Client.Statistic.RxMessages);
        Assert.Equal(0, Client.LocalCount.CurrentValue);
        Assert.Empty(Client.Items);
    }
    
    [Fact]
    public async Task WriteOnce_Timeout_Throws()
    {
        // Arrange
        var paramName = ParamsMeta.ServerParamsMeta.First().Name;
        var paramValue = ParamsMeta.ServerParamsMeta.First().DefaultValue;
        
        var called = 0;
        using var s1 = Link.Client.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(_config.ReadTimeouMs + 1));
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
    
    protected override ParamsClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        var paramsClient = new ParamsClient(identity, _config, core);
        return new ParamsClientEx(paramsClient, _config, ParamsMeta.Encoding, ParamsMeta.ClientParamsDescriptions);
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
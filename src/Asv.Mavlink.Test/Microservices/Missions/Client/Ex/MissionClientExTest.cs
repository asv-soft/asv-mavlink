using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;
using ArgumentNullException = System.ArgumentNullException;

namespace Asv.Mavlink.Test.Ex;

[TestSubject(typeof(MissionClientEx))]
public class MissionClientExTest : ClientTestBase<MissionClientEx>
{
    private const int MaxCommandTimeoutMs = 1000;
    private const int MaxAttemptsToCallCount = 5;
    private const int MaxDeviceUploadTimeoutMs = 10_000;
    
    private readonly MissionClientExConfig _config = new()
    {
        CommandTimeoutMs = MaxCommandTimeoutMs,
        AttemptToCallCount = MaxAttemptsToCallCount,
        DeviceUploadTimeoutMs = MaxDeviceUploadTimeoutMs
    };
    private readonly CommandProtocolConfig _commandConfig = new();
    
    private readonly MissionClientEx _client;
    private CommandClient? _command;
    
    public MissionClientExTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
    }
    
    [Fact]
    public void Constructor_NullArguments_Throws()
    {
        if (_command == null)
        {
            throw new NullReferenceException("Command client is not initialized");
        }
        Assert.Throws<NullReferenceException>(() => new MissionClientEx(null!, _command, _config));
        Assert.Throws<ArgumentNullException>(() => new MissionClientEx(
                new MissionClient(Identity, _config, Context), 
                null!,
                _config
            )
        );
        Assert.Throws<ArgumentNullException>(() => new MissionClientEx(
            new MissionClient(Identity, _config, Context), 
            _command,
            null!
            )
        );
    }

    [Fact]
    public void Create_NoItemsInSource_Success()
    {
        // Arrange
        MissionItem? item;
        
        // Act
        item = _client.Create();

        // Assert
        var realItems = _client.MissionItems.ToList();
        Assert.Single(realItems);
        Assert.NotNull(item);
        Assert.True(realItems[0].IsDeepEqual(item));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(100)]
    [InlineData(4000)]
    public void Create_SeveralItems_Success(ushort itemsCount)
    {
        // Arrange
        var items = new List<MissionItem>();
        
        // Act
        for (var i = 0; i < itemsCount; i++)
        {
            items.Add(_client.Create());
        }
        
        // Assert
        var realItems = _client.MissionItems.ToList();
        Assert.NotEmpty(items);
        Assert.Equal(itemsCount, items.Count);
        Assert.True(realItems.IsDeepEqual(items));
    }

    [Fact]
    public void Remove_NonExistingItem_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _client.Remove(0));
    }
    
    [Fact]
    public void Remove_ExistingItem_Success()
    {
        // Arrange
        _client.Create();
        
        // Act
        _client.Remove(0);
        
        // Assert
        var item = _client.MissionItems.ToList();
        Assert.Empty(item);
    }
    
    [Fact]
    public void Remove_ExistingItemWithSeveralItemsInCollection_Success()
    {
        // Arrange
        const int expectedItemsCount = 2;
        var item0 = _client.Create();
        var item1 = _client.Create();
        var item2 = _client.Create();
        
        // Act
        _client.Remove(item1.Index);
        
        // Assert
        var items = _client.MissionItems.ToList();
        Assert.NotEmpty(items);
        Assert.Equal(expectedItemsCount, items.Count);
        Assert.Contains(item0, items);
        Assert.DoesNotContain(item1, items);
        Assert.Contains(item2, items);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(1000)]
    public void Remove_SeveralExistingItems_Success(ushort itemsCount)
    {
        // Arrange
        var items = new List<MissionItem>();
        for (var i = 0; i < itemsCount; i++)
        {
            items.Add(_client.Create());
        }
        
        // Act
        for (var i = 0; i < itemsCount; i++)
        {
            _client.Remove(items[i].Index);
        }
        
        // Assert
        var realItems = _client.MissionItems.ToList();
        Assert.Empty(realItems);
    }

    [Fact]
    public void ClearLocal_SingleItem_Success()
    {
        // Arrange
        _client.Create();
        
        // Act
        _client.ClearLocal();
        
        // Assert
        var item = _client.MissionItems.ToList();
        Assert.Empty(item);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(1000)]
    public void ClearLocal_SeveralExistingItems_Success(ushort itemsCount)
    {
        // Arrange
        for (var i = 0; i < itemsCount; i++)
        {
            _client.Create();
        }
        
        // Act
        _client.ClearLocal();
        
        // Assert
        var realItems = _client.MissionItems.ToList();
        Assert.Empty(realItems);
    }
    
    [Fact]
    public void ClearLocal_NoItems_Success()
    {
        // Act
        _client.ClearLocal();
        
        // Assert
        var realItems = _client.MissionItems.ToList();
        Assert.Empty(realItems);
    }

    [Fact]
    public void Init_ProperInput_Success()
    {
        _client.Init();
    }

    [Fact]
    public async Task SetCurrent_Timeout_Throws()
    {
        // Arrange
        var called = 0;
        var tcs = new TaskCompletionSource<ushort>();
        using var cancel = new CancellationTokenSource();
        cancel.Token.Register(() => tcs.TrySetCanceled());

        Link.SetServerToClientFilter(_ => false);
        using var s1 = Link.Client.OnTxMessage.Synchronize().Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(MaxCommandTimeoutMs + 1));
        });

        // Act
        var task = _client.SetCurrent(0, cancel.Token);

        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.AttemptToCallCount, called);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task Download_Timeout_Throws()
    {
        // Arrange
        using var cancel = new CancellationTokenSource();
        var called = 0;
        var progress = 0d;
        
        Link.SetServerToClientFilter(_ => false);
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(MaxCommandTimeoutMs + 1));
        });
        
        // Act
        var task = _client.Download(cancel.Token, pr => progress = pr);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.AttemptToCallCount, called);
        Assert.Equal(0, progress);
        Assert.False(_client.IsSynced.CurrentValue);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task ClearRemote_Timeout_Throws()
    {
        // Arrange
        using var cancel = new CancellationTokenSource();
        var called = 0;
        Link.SetServerToClientFilter(_ => false);
        using var s1 = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(MaxCommandTimeoutMs + 1));
        });
        
        // Act
        var task = _client.ClearRemote(cancel.Token);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async ()=>
        {
            await task;
        });
        Assert.Equal(_config.AttemptToCallCount, called);
        Assert.False(_client.IsSynced.CurrentValue);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task Upload_Timeout_Throws()
    {
        // Arrange
        using var cancel = new CancellationTokenSource();
        var progress = 0d;
        var called = 0;
        _client.Create();

        Link.SetServerToClientFilter(_ => true);
        using var sub = Link.Client.OnTxMessage.Subscribe(_ =>
        {
            called++;
            Time.Advance(TimeSpan.FromMilliseconds(MaxCommandTimeoutMs + 1));
        });
        
        // Act
        var task = _client.Upload(cancel.Token, p => progress = p);
        
        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_config.AttemptToCallCount, called);
        Assert.Equal(0, progress);
        Assert.Equal(0u, Link.Server.Statistic.TxMessages);
        Assert.Equal(0u, Link.Client.Statistic.RxMessages);
    }
    
    protected override MissionClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        _command = new CommandClient(Identity, _commandConfig, core);
        return new MissionClientEx(new MissionClient(identity, _config, core), _command, _config);
    }
}
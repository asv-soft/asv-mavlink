using System;
using System.Collections.Generic;
using System.Linq;
using DeepEqual.Syntax;
using JetBrains.Annotations;
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
    
    private readonly MissionClientEx _client;

    public MissionClientExTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
    }

    protected override MissionClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new MissionClientEx(new MissionClient(identity, _config, core), _config);
    }
    
    [Fact]
    public void Constructor_Null_Throws()
    {
        Assert.Throws<NullReferenceException>(() => new MissionClientEx(null!, _config));
        Assert.Throws<ArgumentNullException>(() => new MissionClientEx(
            new MissionClient(Identity, _config, Context), 
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
        var item0 = _client.Create();
        var item1 = _client.Create();
        var item2 = _client.Create();
        
        // Act
        _client.Remove(item1.Index);
        
        // Assert
        var items = _client.MissionItems.ToList();
        Assert.NotEmpty(items);
        Assert.Equal(2, items.Count);
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
}
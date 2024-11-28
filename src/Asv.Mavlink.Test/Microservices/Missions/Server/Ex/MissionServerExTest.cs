using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.V2.Common;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;
using NullReferenceException = System.NullReferenceException;

namespace Asv.Mavlink.Test.Server.Ex;

[TestSubject(typeof(MissionServerEx))]
public class MissionServerExTest : ServerTestBase<MissionServerEx>
{
    private const int MaxQueueSizeValue = 100;
    private const int MaxSendRateHzValue = 30;
    private readonly StatusTextLoggerConfig _config = new()
    {
        MaxQueueSize = MaxQueueSizeValue,
        MaxSendRateHz = MaxSendRateHzValue,
    };

    private readonly MissionServerEx _server;

    public MissionServerExTest(ITestOutputHelper log) : base(log)
    {
        _server = Server;
    }

    protected override MissionServerEx CreateClient(MavlinkIdentity identity, CoreServices core) 
        => new(new MissionServer(identity, core), new StatusTextServer(identity, _config, core));
    
    [Fact]
    public void Constructor_Null_Throws()
    {
        Assert.Throws<NullReferenceException>(
            () => new MissionServerEx(null!, new StatusTextServer(Identity, _config, Core))
        );
        Assert.Throws<ArgumentNullException>(
            () => new MissionServerEx(new MissionServer(Identity, Core), null!)
        );
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(9_000)]
    public void AddItems_DifferentItemsCount_Success(int itemsCount)
    {
        // Arrange
        var realItems = new List<ServerMissionItem>();

        for (var i = 0; i < itemsCount; i++)
        {
            realItems.Add(new ServerMissionItem()
            {
                Autocontinue = 1,
                Command = MavCmd.MavCmdAirframeConfiguration,
                Frame = MavFrame.MavFrameReserved18,
                MissionType = MavMissionType.MavMissionTypeMission,
                Param1 = 23,
                Param2 = 34,
                Param3 = 42,
                Param4 = 24.5f,
                Seq = 0,
                X = 10,
                Y = 32,
                Z = 24.5f,
            });
        }
        
        // Act
        _server.AddItems(realItems);
        
        // Assert
        var itemsFromServer = _server.Items.ToList();
        Assert.Equal(itemsCount, itemsFromServer.Count);
        Assert.True(realItems.IsDeepEqual(itemsFromServer));
    }
    
    [Fact]
    public void AddItems_AddNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _server.AddItems(null!));
    }
    
    [Fact]
    public void AddItems_AddArrayWithNullItems_Throws()
    {
        Assert.Throws<NullReferenceException>(() => _server.AddItems(new ServerMissionItem[10]));
    }
    
    [Fact]
    public void AddItems_AddArrayEmpty_Success()
    {
        // Arrange
        var realItems = Array.Empty<ServerMissionItem>();
        
        // Act
        _server.AddItems(realItems);
        
        // Assert
        var itemsFromServer = _server.Items.ToArray();
        Assert.Empty(itemsFromServer);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(9_000)]
    public void RemoveItems_DifferentItemsCount_Success(int itemsCount)
    {
        // Arrange
        var realItems = new List<ServerMissionItem>();

        for (var i = 0; i < itemsCount; i++)
        {
            realItems.Add(new ServerMissionItem());
        }
        _server.AddItems(realItems);
        
        // Act
        _server.RemoveItems(realItems);
        
        // Assert
        var itemsFromServer = _server.Items.ToList();
        Assert.Empty(itemsFromServer);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(ushort.MaxValue)]
    public void RemoveItems_NonExistingItem_Success(ushort seq)
    {
        // Arrange
        var itemsCount = 10;
        var realItems = new List<ServerMissionItem>();
        var itemToRemove = new ServerMissionItem()
        {
            Autocontinue = 1,
            Command = MavCmd.MavCmdAirframeConfiguration,
            Frame = MavFrame.MavFrameReserved18,
            MissionType = MavMissionType.MavMissionTypeMission,
            Param1 = 23,
            Param2 = 34,
            Param3 = 42,
            Param4 = 24.5f,
            Seq = seq,
            X = 10,
            Y = 32,
            Z = 24.5f,
        };

        for (var i = 0; i < itemsCount; i++)
        {
            realItems.Add(new ServerMissionItem());
        }
        _server.AddItems(realItems);
        
        // Act
        _server.RemoveItems([itemToRemove]);
        
        // Assert
        var itemsFromServer = _server.Items.ToList();
        Assert.NotEmpty(itemsFromServer);
        Assert.Equal(itemsCount, itemsFromServer.Count);
        Assert.True(realItems.IsDeepEqual(itemsFromServer));
    }
    
    [Fact]
    public void RemoveItems_NotAllItems_Success()
    {
        // Arrange
        var itemsCount = 10;
        var realItems = new List<ServerMissionItem>();

        for (var i = 0; i < itemsCount; i++)
        {
            realItems.Add(new ServerMissionItem());
        }
        _server.AddItems(realItems);
        
        // Act
        _server.RemoveItems(realItems.Take(realItems.Count/2));
        
        // Assert
        var itemsFromServer = _server.Items.ToList();
        Assert.NotEmpty(itemsFromServer);
        Assert.Equal(itemsCount/2, itemsFromServer.Count);
        Assert.True(realItems.Take(realItems.Count/2).IsDeepEqual(itemsFromServer));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(9_000)]
    public void GetItemsSnapshot_DifferentItemsCount_Success(int itemsCount)
    {
        // Arrange
        var realItems = new List<ServerMissionItem>();

        for (var i = 0; i < itemsCount; i++)
        {
            realItems.Add(new ServerMissionItem());
        }
        _server.AddItems(realItems);
        
        // Act
        var result = _server.GetItemsSnapshot();
        
        // Assert
        var itemsFromServer = _server.Items.ToImmutableArray();
        Assert.NotEmpty(itemsFromServer);
        Assert.NotEmpty(result);
        Assert.Equal(itemsCount, result.Length);
        for (var i = 0; i < itemsCount; i++)
        {
            Assert.True(itemsFromServer[i].IsDeepEqual(result[i]));
        }
    }
    
    [Fact]
    public void GetItemsSnapshot_NoItems_Success()
    {
        // Act
        var result = _server.GetItemsSnapshot();
        
        // Assert
        var itemsFromServer = _server.Items.ToImmutableArray();
        Assert.Empty(itemsFromServer);
        Assert.Empty(result);
    }

    [Fact]
    public void GetItemsSnapshot_NullItems_Throws()
    {
        // Arrange
        var realItems = Array.Empty<ServerMissionItem>();
        _server.AddItems(realItems);
        
        // Act
        var result = _server.GetItemsSnapshot();
        
        // Assert
        var itemsFromServer = _server.Items.ToImmutableArray();
        Assert.Empty(itemsFromServer);
        Assert.Empty(result);
    }
}
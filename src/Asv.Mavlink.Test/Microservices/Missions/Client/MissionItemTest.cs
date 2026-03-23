using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using FluentAssertions;
using JetBrains.Annotations;
using R3;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MissionItem))]
public class MissionItemTest : IDisposable
{
    private readonly TaskCompletionSource _tcs;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    public MissionItemTest()
    {
        _tcs = new TaskCompletionSource();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _tcs.TrySetCanceled());
    }
    
    [Fact]
    public void Constructor_NullPayload_Throws()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => new MissionItem(null!));
    }
    
    [Theory]
    [InlineData("tag")]
    [InlineData(null)]
    [InlineData($"{nameof(MissionItem.Tag)}")]
    [InlineData(56)]
    public void Tag_SetRandomTag_Success(object? tag)
    {
        // Arrange
        var item = new MissionItem(new MissionItemIntPayload());
        
        // Act
        item.Tag = tag;
        
        // Assert
        Assert.Equal(item.Tag, tag);
    }
    
    [Fact]
    public void Edit_EmptyEditCallback_Success()
    {
        // Arrange
        var called = 0;
        var payload = new MissionItemIntPayload();
        var item = new MissionItem(payload);
        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        item.Edit(_ => {});
        
        // Assert
        Assert.Equal(0, called);
        Assert.Equivalent(payload, item.Payload);
    }
    
    [Fact]
    public async Task Edit_ChangeFullPayload_Success()
    {
        // Arrange
        const int newX = 4;
        const int newY = 5;
        const float newZ = 6f;
        const byte newAutocontinue = 1;
        const MavCmd newCommand = MavCmd.MavCmdUser2;
        const byte newCurrent = 1;
        const MavFrame newFrame = MavFrame.MavFrameReserved13;
        const MavMissionType newMissionType = MavMissionType.MavMissionTypeFence;
        const int newParam1 = 10;
        const int newParam2 = 14;
        const int newParam3 = 144;
        const int newParam4 = 100;
        const byte newSeq = 2;
        const int expectedCalls = 1;
        
        var called = 0;
        
        var payload = new MissionItemIntPayload
        {
            X = 1,
            Y = 2,
            Z = 3,
            Autocontinue = 0,
            Command = MavCmd.MavCmdUser1,
            Current = 0,
            Frame = MavFrame.MavFrameGlobal,
            MissionType = MavMissionType.MavMissionTypeAll,
            Param1 = 1.1f,
            Param2 = 2.3f,
            Param3 = 3.3f,
            Param4 = 4.5f,
            Seq = 1
        };
        
        var oldItem = new MissionItem(payload);
        var item = new MissionItem(payload);

        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
            _tcs.TrySetResult();
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.X = newX;
            pld.Y = newY;
            pld.Z = newZ;
            pld.Autocontinue = newAutocontinue;
            pld.Command = newCommand;
            pld.Current = newCurrent;
            pld.Frame = newFrame;
            pld.MissionType = newMissionType;
            pld.Param1 = newParam1;
            pld.Param2 = newParam2;
            pld.Param3 = newParam3;
            pld.Param4 = newParam4;
            pld.Seq = newSeq;
        });
        
        // Assert
        await _tcs.Task;
        Assert.Equal(expectedCalls, called);
        item.Should().NotBeEquivalentTo(oldItem);
        Assert.Equal(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newX), item.Location.CurrentValue.Latitude);
        Assert.Equal(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newY), item.Location.CurrentValue.Longitude);
        Assert.Equal(newZ, item.Location.CurrentValue.Altitude);
        Assert.Equal(newAutocontinue != 0, item.AutoContinue.CurrentValue);
        Assert.Equal(newCommand, item.Command.CurrentValue);
        Assert.Equal(newCurrent != 0, item.Current.CurrentValue);
        Assert.Equal(newFrame, item.Frame.CurrentValue);
        Assert.Equal(newMissionType, item.MissionType.CurrentValue);
        Assert.Equal(newParam1, item.Param1.CurrentValue);
        Assert.Equal(newParam2, item.Param2.CurrentValue);
        Assert.Equal(newParam3, item.Param3.CurrentValue);
        Assert.Equal(newParam4, item.Param4.CurrentValue);
        Assert.Equal(newSeq, item.Index);
    }
    
    [Fact]
    public async Task Edit_ChangeHalfPayload_Success()
    {
        // Arrange
        const int newX = 4;
        const int newY = 5;
        const float newZ = 6f;
        const byte newAutocontinue = byte.MinValue;
        const int newParam2 = 14;
        const int newParam3 = 144;
        const int newParam4 = 100;
        const byte newSeq = 2;
        const int expectedCalls = 1;
        
        var called = 0;
        
        var payload = new MissionItemIntPayload
        {
            X = 1,
            Y = 2,
            Z = 3,
            Autocontinue = 1,
            Command = MavCmd.MavCmdUser1,
            Current = 1,
            Frame = MavFrame.MavFrameGlobal,
            MissionType = MavMissionType.MavMissionTypeAll,
            Param1 = 1.1f,
            Param2 = 2.3f,
            Param3 = 3.3f,
            Param4 = 4.5f,
            Seq = 1
        };
        
        var oldItem = new MissionItem(payload);
        var item = new MissionItem(payload);

        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
            _tcs.TrySetResult();
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.X = newX;
            pld.Y = newY;
            pld.Z = newZ;
            pld.Autocontinue = newAutocontinue;
            pld.Param2 = newParam2;
            pld.Param3 = newParam3;
            pld.Param4 = newParam4;
            pld.Seq = newSeq;
        });
        
        // Assert
        await _tcs.Task;
        Assert.Equal(expectedCalls, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newX), item.Location.CurrentValue.Latitude);
        Assert.Equal(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newY), item.Location.CurrentValue.Longitude);
        Assert.Equal(newZ, item.Location.CurrentValue.Altitude);
        Assert.Equal(newAutocontinue != 0, item.AutoContinue.CurrentValue);
        Assert.Equal(newParam2, item.Param2.CurrentValue);
        Assert.Equal(newParam3, item.Param3.CurrentValue);
        Assert.Equal(newParam4, item.Param4.CurrentValue);
        Assert.Equal(newSeq, item.Index);
    }
    
    [Theory]
    [InlineData(1, 2, 3, 4, 5, 6)]
    [InlineData(int.MinValue, int.MinValue, float.MinValue, int.MaxValue, int.MaxValue, float.MaxValue)]
    [InlineData(int.MaxValue, int.MaxValue, float.MaxValue, int.MinValue, int.MinValue, float.MinValue)]
    [InlineData(1, 2, 3, 0, 0, 0)]
    public async Task Edit_ChangeLocation_Success(int x, int y, float z, int newX, int newY, float newZ)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        var payload = new MissionItemIntPayload
        {
            X = x,
            Y = y,
            Z = z,
            Autocontinue = 1,
            Command = MavCmd.MavCmdUser1,
            Current = 1,
            Frame = MavFrame.MavFrameGlobal,
            MissionType = MavMissionType.MavMissionTypeAll,
            Param1 = 1.1f,
            Param2 = 2.3f,
            Param3 = 3.3f,
            Param4 = 4.5f,
            Seq = 1
        };
        var oldItem = new MissionItem(payload);
        var item = new MissionItem(payload);

        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
            _tcs.TrySetResult();
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.X = newX;
            pld.Y = newY;
            pld.Z = newZ;
        });
        
        // Assert
        await _tcs.Task;
        Assert.Equal(expectedCalls, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newX), item.Location.CurrentValue.Latitude);
        Assert.Equal(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newY), item.Location.CurrentValue.Longitude);
        Assert.Equal(newZ, item.Location.CurrentValue.Altitude);
    }
    
    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public async Task Edit_ChangeAutoContinue_Success(byte autoContinue, byte newAutoContinue)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        
        var payload = new MissionItemIntPayload
        {
            X = 1,
            Y = 2,
            Z = 3,
            Autocontinue = autoContinue,
            Command = MavCmd.MavCmdUser1,
            Current = 1,
            Frame = MavFrame.MavFrameGlobal,
            MissionType = MavMissionType.MavMissionTypeAll,
            Param1 = 1.1f,
            Param2 = 2.3f,
            Param3 = 3.3f,
            Param4 = 4.5f,
            Seq = 1
        };
        
        var oldItem = new MissionItem(payload);
        var item = new MissionItem(payload);

        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
            _tcs.TrySetResult();
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.Autocontinue = newAutoContinue;
        });
        
        // Assert
        await _tcs.Task;
        Assert.Equal(expectedCalls, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(newAutoContinue != 0, item.AutoContinue.CurrentValue);
    }
    
    [Theory]
    [InlineData(MavCmd.MavCmdNavWaypoint)]
    [InlineData(MavCmd.MavCmdUser3)]
    [InlineData(MavCmd.MavCmdCanForward)]
    public async Task Edit_ChangeCommand_Success(MavCmd newCommand)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        
        var payload = new MissionItemIntPayload
        {
            X = 1,
            Y = 2,
            Z = 3,
            Autocontinue = 1,
            Command = MavCmd.MavCmdUser1,
            Current = 1,
            Frame = MavFrame.MavFrameGlobal,
            MissionType = MavMissionType.MavMissionTypeAll,
            Param1 = 1.1f,
            Param2 = 2.3f,
            Param3 = 3.3f,
            Param4 = 4.5f,
            Seq = 1
        };
        
        var oldItem = new MissionItem(payload);
        var item = new MissionItem(payload);

        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
            _tcs.TrySetResult();
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.Command = newCommand;
        });
        
        // Assert
        await _tcs.Task;
        Assert.Equal(expectedCalls, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(newCommand, item.Command.CurrentValue);
    }
    
    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public async Task Edit_ChangeCurrent_Success(byte current, byte newCurrent)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        
        var payload = new MissionItemIntPayload
        {
            X = 1,
            Y = 2,
            Z = 3,
            Autocontinue = 1,
            Command = MavCmd.MavCmdUser1,
            Current = current,
            Frame = MavFrame.MavFrameGlobal,
            MissionType = MavMissionType.MavMissionTypeAll,
            Param1 = 1.1f,
            Param2 = 2.3f,
            Param3 = 3.3f,
            Param4 = 4.5f,
            Seq = 1
        };
        
        var oldItem = new MissionItem(payload);
        var item = new MissionItem(payload);

        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
            _tcs.TrySetResult();
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.Current = newCurrent;
        });
        
        // Assert
        await _tcs.Task;
        Assert.Equal(expectedCalls, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(newCurrent != 0, item.Current.CurrentValue);
    }
    
    [Theory]
    [InlineData(MavFrame.MavFrameGlobal)]
    [InlineData(MavFrame.MavFrameReserved13)]
    [InlineData(MavFrame.MavFrameLocalFlu)]
    public async Task Edit_ChangeFrame_Success(MavFrame newFrame)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        
        var payload = new MissionItemIntPayload
        {
            X = 1,
            Y = 2,
            Z = 3,
            Autocontinue = 1,
            Command = MavCmd.MavCmdUser1,
            Current = 1,
            Frame = MavFrame.MavFrameGlobalRelativeAltInt,
            MissionType = MavMissionType.MavMissionTypeAll,
            Param1 = 1.1f,
            Param2 = 2.3f,
            Param3 = 3.3f,
            Param4 = 4.5f,
            Seq = 1
        };
        
        var oldItem = new MissionItem(payload);
        var item = new MissionItem(payload);
        
        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
            _tcs.TrySetResult();
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.Frame = newFrame;
        });
        
        // Assert
        await _tcs.Task;
        Assert.Equal(expectedCalls, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(newFrame, item.Frame.CurrentValue);
    }
    
    [Theory]
    [InlineData(MavMissionType.MavMissionTypeMission)]
    [InlineData(MavMissionType.MavMissionTypeRally)]
    [InlineData(MavMissionType.MavMissionTypeFence)]
    public async Task Edit_ChangeMissionType_Success(MavMissionType newMissionType)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        
        var payload = new MissionItemIntPayload
        {
            X = 1,
            Y = 2,
            Z = 3,
            Autocontinue = 1,
            Command = MavCmd.MavCmdUser1,
            Current = 1,
            Frame = MavFrame.MavFrameGlobal,
            MissionType = MavMissionType.MavMissionTypeAll,
            Param1 = 1.1f,
            Param2 = 2.3f,
            Param3 = 3.3f,
            Param4 = 4.5f,
            Seq = 1
        };
        
        var oldItem = new MissionItem(payload);
        var item = new MissionItem(payload);

        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
            _tcs.TrySetResult();
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.MissionType = newMissionType;
        });
        
        // Assert
        await _tcs.Task;
        Assert.Equal(expectedCalls, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(item.MissionType.CurrentValue, newMissionType);
    }
    
    [Theory]
    [InlineData(1, float.MinValue)]
    [InlineData(1, float.MaxValue)]
    [InlineData(2, float.MinValue)]
    [InlineData(2, float.MaxValue)]
    [InlineData(3, float.MinValue)]
    [InlineData(3, float.MaxValue)]
    [InlineData(4, float.MinValue)]
    [InlineData(4, float.MaxValue)]
    [InlineData(1, float.NaN)]
    [InlineData(2, float.NaN)]
    [InlineData(3, float.NaN)]
    [InlineData(4, float.NaN)]
    public async Task Edit_ChangeParams_Success(int paramNumber, float newParam)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        
        var payload = new MissionItemIntPayload
        {
            X = 1,
            Y = 2,
            Z = 3,
            Autocontinue = 1,
            Command = MavCmd.MavCmdUser1,
            Current = 1,
            Frame = MavFrame.MavFrameGlobal,
            MissionType = MavMissionType.MavMissionTypeAll,
            Param1 = 1.1f,
            Param2 = 2.3f,
            Param3 = 3.3f,
            Param4 = 4.5f,
            Seq = 1
        };
        
        var oldItem = new MissionItem(payload);
        var item = new MissionItem(payload);

        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
            _tcs.TrySetResult();
        });
        
        // Act
        switch (paramNumber)
        {
            case 1:
                item.Edit(pld =>
                {
                    pld.Param1 = newParam;
                });
                break;
            case 2:
                item.Edit(pld =>
                {
                    pld.Param2 = newParam;
                });
                break;
            case 3:
                item.Edit(pld =>
                {
                    pld.Param3 = newParam;
                });
                break;
            default:
                item.Edit(pld =>
                {
                    pld.Param4 = newParam;
                });
                break;
        }
        
        // Assert
        await _tcs.Task;
        Assert.Equal(expectedCalls, called);
        Assert.False(item.IsDeepEqual(oldItem));
        switch (paramNumber)
        {
            case 1:
                Assert.Equal(item.Param1.CurrentValue, newParam);
                break;
            case 2:
                Assert.Equal(item.Param2.CurrentValue, newParam);
                break;
            case 3:
                Assert.Equal(item.Param3.CurrentValue, newParam);
                break;
            default:
                Assert.Equal(item.Param4.CurrentValue, newParam);
                break;
        }
    }
    
    [Theory]
    [InlineData(ushort.MinValue)]
    [InlineData(ushort.MaxValue)]
    public async Task Edit_ChangeSeq_Success(ushort newSeq)
    {
        // Arrange
        const int expectedCalls = 1;
        var called = 0;
        
        var payload = new MissionItemIntPayload
        {
            X = 1,
            Y = 2,
            Z = 3,
            Autocontinue = 1,
            Command = MavCmd.MavCmdUser1,
            Current = 1,
            Frame = MavFrame.MavFrameGlobal,
            MissionType = MavMissionType.MavMissionTypeAll,
            Param1 = 1.1f,
            Param2 = 2.3f,
            Param3 = 3.3f,
            Param4 = 4.5f,
            Seq = 1
        };
        
        var oldItem = new MissionItem(payload);
        var item = new MissionItem(payload);

        using var sub = item.OnChanged.Synchronize().Subscribe(_ =>
        {
            called++;
            _tcs.TrySetResult();
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.Seq = newSeq;
        });
        
        // Assert
        await _tcs.Task;
        Assert.Equal(expectedCalls, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(item.Index, newSeq);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        GC.SuppressFinalize(this);
    }
}
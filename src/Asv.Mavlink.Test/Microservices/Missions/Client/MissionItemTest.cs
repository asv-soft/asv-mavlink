using System;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MissionItem))]
public class MissionItemTest
{
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
        var item = new MissionItem(new MissionItemIntPayload());
        item.OnChanged.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        item.Edit(_ => {});
        
        // Assert
        Assert.Equal(0, called);
    }
    
    [Fact]
    public void Edit_ChangeFullPayload_Success()
    {
        // Arrange
        var called = 0;
        var newX = 4;
        var newY = 5;
        var newZ = 6f;
        var newAutocontinue = (byte) 1;
        var newCommand = MavCmd.MavCmdUser2;
        var newCurrent = (byte) 1;
        var newFrame = MavFrame.MavFrameReserved13;
        var newMissionType = MavMissionType.MavMissionTypeFence;
        var newParam1 = 10;
        var newParam2 = 14;
        var newParam3 = 144;
        var newParam4 = 100;
        var newSeq = (byte) 2;
        
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

        item.OnChanged.Subscribe(_ =>
        {
            called++;
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
        Assert.Equal(1, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(item.Location.CurrentValue.Latitude, MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newX));
        Assert.Equal(item.Location.CurrentValue.Longitude, MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newY));
        Assert.Equal(item.Location.CurrentValue.Altitude, newZ);
        Assert.Equal(item.AutoContinue.CurrentValue, newAutocontinue != 0);
        Assert.Equal(item.Command.CurrentValue, newCommand);
        Assert.Equal(item.Current.CurrentValue, newCurrent != 0);
        Assert.Equal(item.Frame.CurrentValue, newFrame);
        Assert.Equal(item.MissionType.CurrentValue, newMissionType);
        Assert.Equal(item.Param1.CurrentValue, newParam1);
        Assert.Equal(item.Param2.CurrentValue, newParam2);
        Assert.Equal(item.Param3.CurrentValue, newParam3);
        Assert.Equal(item.Param4.CurrentValue, newParam4);
        Assert.Equal(item.Index, newSeq);
    }
    
    [Fact]
    public void Edit_ChangeHalfPayload_Success()
    {
        // Arrange
        var called = 0;
        var newX = 4;
        var newY = 5;
        var newZ = 6f;
        var newAutocontinue = byte.MinValue;
        var newParam2 = 14;
        var newParam3 = 144;
        var newParam4 = 100;
        var newSeq = (byte) 2;
        
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

        item.OnChanged.Subscribe(_ =>
        {
            called++;
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
        Assert.Equal(1, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(item.Location.CurrentValue.Latitude, MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newX));
        Assert.Equal(item.Location.CurrentValue.Longitude, MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newY));
        Assert.Equal(item.Location.CurrentValue.Altitude, newZ);
        Assert.Equal(item.AutoContinue.CurrentValue, newAutocontinue != 0);
        Assert.Equal(item.Param2.CurrentValue, newParam2);
        Assert.Equal(item.Param3.CurrentValue, newParam3);
        Assert.Equal(item.Param4.CurrentValue, newParam4);
        Assert.Equal(item.Index, newSeq);
    }
    
    [Theory]
    [InlineData(1, 2, 3, 4, 5, 6)]
    [InlineData(1, 2, 3, 1, 5, 6)]
    [InlineData(1, 2, 3, 1, 2, 6)]
    [InlineData(1, 2, 3, 4, 2, 3)]
    [InlineData(1, 2, 3, 4, 2, 6)]
    [InlineData(1, 2, 3, 4, 5, 3)]
    [InlineData(int.MinValue, int.MinValue, float.MinValue, int.MaxValue, int.MaxValue, float.MaxValue)]
    [InlineData(int.MaxValue, int.MaxValue, float.MaxValue, int.MinValue, int.MinValue, float.MinValue)]
    [InlineData(1, 2, 3, 0, 0, 0)]
    public void Edit_ChangeLocation_Success(int x, int y, float z, int newX, int newY, float newZ)
    {
        // Arrange
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

        item.OnChanged.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.X = newX;
            pld.Y = newY;
            pld.Z = newZ;
        });
        
        // Assert
        Assert.Equal(1, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(item.Location.CurrentValue.Latitude, MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newX));
        Assert.Equal(item.Location.CurrentValue.Longitude, MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(newY));
        Assert.Equal(item.Location.CurrentValue.Altitude, newZ);
    }
    
    [Theory]
    
     [InlineData(1, 0)]
    [InlineData(0, 1)]
    
    public void Edit_ChangeAutoContinue_Success(byte autoContinue, byte newAutoContinue)
    {
        // Arrange
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

        item.OnChanged.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.Autocontinue = newAutoContinue;
        });
        
        // Assert
        Assert.Equal(1, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(item.AutoContinue.CurrentValue, newAutoContinue != 0);
    }
    
    [Theory]
    [InlineData(MavCmd.MavCmdUser2)]
    [InlineData(MavCmd.MavCmdArmAuthorizationRequest)]
    [InlineData(MavCmd.MavCmdUser3)]
    [InlineData(MavCmd.MavCmdNavTakeoff)]
    public void Edit_ChangeCommand_Success(MavCmd newCommand)
    {
        // Arrange
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

        item.OnChanged.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.Command = newCommand;
        });
        
        // Assert
        Assert.Equal(1, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(item.Command.CurrentValue, newCommand);
    }
    
    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public void Edit_ChangeCurrent_Success(byte current, byte newCurrent)
    {
        // Arrange
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

        item.OnChanged.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.Current = newCurrent;
        });
        
        // Assert
        Assert.Equal(1, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(item.Current.CurrentValue, newCurrent != 0);
    }
    
    [Theory]
    [InlineData(MavFrame.MavFrameGlobalInt)]
    [InlineData(MavFrame.MavFrameReserved13)]
    [InlineData(MavFrame.MavFrameLocalNed)]
    [InlineData(MavFrame.MavFrameLocalOffsetNed)]
    public void Edit_ChangeFrame_Success(MavFrame newFrame)
    {
        // Arrange
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

        item.OnChanged.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.Frame = newFrame;
        });
        
        // Assert
        Assert.Equal(1, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(item.Frame.CurrentValue, newFrame);
    }
    
    [Theory]
    [InlineData(MavMissionType.MavMissionTypeMission)]
    [InlineData(MavMissionType.MavMissionTypeRally)]
    [InlineData(MavMissionType.MavMissionTypeFence)]
    public void Edit_ChangeMissionType_Success(MavMissionType newMissionType)
    {
        // Arrange
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

        item.OnChanged.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.MissionType = newMissionType;
        });
        
        // Assert
        Assert.Equal(1, called);
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
    public void Edit_ChangeParams_Success(int paramNumber, float newParam)
    {
        // Arrange
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

        item.OnChanged.Subscribe(_ =>
        {
            called++;
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
        Assert.Equal(1, called);
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
    public void Edit_ChangeSeq_Success(ushort newSeq)
    {
        // Arrange
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

        item.OnChanged.Subscribe(_ =>
        {
            called++;
        });
        
        // Act
        item.Edit(pld =>
        {
            pld.Seq = newSeq;
        });
        
        // Assert
        Assert.Equal(1, called);
        Assert.False(item.IsDeepEqual(oldItem));
        Assert.Equal(item.Index, newSeq);
    }
}
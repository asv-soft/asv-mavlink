using System;
using System.IO;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DeepEqual.Syntax;
using Xunit;

namespace Asv.Mavlink.Test;

public class ListDataFileTest
{
    public static readonly ListDataFileFormat FileFormat1 = new()
    {
        Version = "1.0.0",
        Type = "TestFile1",
        MetadataMaxSize = 1024 * 4,
        ItemMaxSize = 256,
    };
    
    public static readonly ListDataFileFormat FileFormat2 = new()
    {
        Version = "1.0.0",
        Type = "TestFile2",
        MetadataMaxSize = 1024 * 4,
        ItemMaxSize = 256,
    };
    
    [Fact]
    public void Test_Null_Reference()
    {
        using var strm = new MemoryStream();
        Assert.Throws<ArgumentNullException>(() =>
        {
            using var file = new ListDataFile<AsvSdrRecordFileMetadata>(null,FileFormat1, false);    
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            using var file = new ListDataFile<AsvSdrRecordFileMetadata>(new MemoryStream(),null, false);    
        });
    }
    
    
    [Fact]
    public void Check_Metadata_Serialization()
    {
        using var strm = new MemoryStream();
        using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, AsvSdrHelper.FileFormat,false);
        var payload = new AsvSdrRecordPayload
        {
            Frequency = 1,
            Size = 1,
            CreatedUnixUs = MavlinkTypesHelper.ToUnixTimeSec(DateTime.Now),
            DataType = AsvSdrCustomMode.AsvSdrCustomModeGp,
            DataCount = 1,
            DurationSec = 321,
            TagCount = 1,
        };
        var name = "test";
        MavlinkTypesHelper.SetGuid(payload.RecordGuid,Guid.NewGuid());
        MavlinkTypesHelper.SetString(payload.RecordName,name);
       
   
        file.EditMetadata(_=>
        {
            payload.RecordGuid.CopyTo(_.Info.RecordGuid,0);
            payload.RecordName.CopyTo(_.Info.RecordName,0);
            _.Info.Frequency = payload.Frequency;
            _.Info.Size = payload.Size;
            _.Info.CreatedUnixUs = payload.CreatedUnixUs;
            _.Info.DataType = payload.DataType;
            _.Info.DataCount = payload.DataCount;
            _.Info.DurationSec = payload.DurationSec;
            _.Info.TagCount = payload.TagCount;
        });
        
        file.EditMetadata(_ =>
        {
            Assert.Equal(payload.RecordGuid, _.Info.RecordGuid);
            Assert.Equal(payload.RecordName, _.Info.RecordName);
            Assert.Equal(payload.Frequency, _.Info.Frequency);
            Assert.Equal(payload.Size, _.Info.Size);
            Assert.Equal(payload.CreatedUnixUs, _.Info.CreatedUnixUs);
            Assert.Equal(payload.DataType, _.Info.DataType);
            Assert.Equal(payload.DataCount, _.Info.DataCount);
            Assert.Equal(payload.DurationSec, _.Info.DurationSec);
            Assert.Equal(payload.TagCount, _.Info.TagCount);
        });
    }


    [Fact]
    public void Check_Data_Serialization()
    {
        using var strm = new MemoryStream();
        using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, FileFormat1,false);
        
        Assert.False(file.Exist(0));
        
        var ilsRead = new AsvSdrRecordDataLlzPayload();
        Assert.False(file.Read(0, ilsRead));
        
        var ils = new AsvSdrRecordDataLlzPayload
        {
            TimeUnixUsec = 15,
            TotalFreq = 2,
            DataIndex = 3,
            GnssLat = 4,
            GnssLon = 5,
            GnssAlt = 6,
            GnssAltEllipsoid = 7,
            GnssHAcc = 8,
            GnssVAcc = 9,
            GnssVelAcc = 10,
            Lat = 5566,
            Lon = 64654,
            Alt = 5465,
            RelativeAlt = 150,
            Roll = 30,
            Pitch = 10,
            Yaw = 70,
            CrsPower = 90.90f,
            CrsAm90 = 10.1f,
            CrsAm150 = 20.2f,
            ClrPower = 30.4f,
            ClrAm90 = 40.5f,
            ClrAm150 = 50.5f,
            TotalPower = 60.123f,
            TotalFieldStrength = 70,
            TotalAm90 = 80,
            TotalAm150 = 90,
            Phi90CrsVsClr = 10,
            Phi150CrsVsClr = 20,
            CodeIdAm1020 = 30,
            GnssEph = 40,
            GnssEpv = 50,
            GnssVel = 60,
            Vx = 70,
            Vy = 80,
            Vz = 90,
            Hdg = 10,
            CrsCarrierOffset = 10,
            CrsFreq90 = 20,
            CrsFreq150 = 0,
            ClrCarrierOffset = 0,
            ClrFreq90 = 0,
            ClrFreq150 = 0,
            TotalCarrierOffset = 0,
            TotalFreq90 = 0,
            TotalFreq150 = 0,
            CodeIdFreq1020 = 0,
            MeasureTime = 0,
            RecordGuid = new byte[16],
            GnssFixType = GpsFixType.GpsFixTypeNoGps,
            GnssSatellitesVisible = 0
        };
        file.Write(10, ils);
        file.Write(20, ils);
        
        var ilsRead2 = new AsvSdrRecordDataLlzPayload();
        Assert.True(file.Read(10, ilsRead2));
        ils.ShouldDeepEqual(ilsRead2);
        Assert.True(file.Read(20, ilsRead2));
        ils.ShouldDeepEqual(ilsRead2);
        
        Assert.False(file.Read(9, ilsRead2));
        Assert.False(file.Exist(5));
    }

}
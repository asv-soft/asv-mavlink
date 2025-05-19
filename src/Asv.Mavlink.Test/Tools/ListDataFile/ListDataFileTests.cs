using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.AsvSdr;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using Xunit;

namespace Asv.Mavlink.Test;

public class ListDataFileTests
{
    private static MockFileSystem _fileSystem = new();

    public static readonly ListDataFileFormat FileFormat1 = new()
    {
        Version = "1.0.0",
        Type = "TestFile1",
        MetadataMaxSize = 1024 * 4,
        ItemMaxSize = 256
    };

    public static readonly ListDataFileFormat FileFormat2 = new()
    {
        Version = "1.0.0",
        Type = "TestFile2",
        MetadataMaxSize = 1024 * 4,
        ItemMaxSize = 256
    };

    [Fact]
    public void Ctor_Null_Reference_Fail()
    {
        using var strm = new MemoryStream();
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            using var file = new ListDataFile<AsvSdrRecordFileMetadata>(null, FileFormat1, false, _fileSystem);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            using var file = new ListDataFile<AsvSdrRecordFileMetadata>(new MemoryStream(), null, false, _fileSystem);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });
    }

    [Fact]
    public static void Ctor_TryPassNullValueOfVersionToFileCtor_Fail()
    {
        using var strm = new MemoryStream();

        var format = new ListDataFileFormat()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Version = null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Type = "type",
            MetadataMaxSize = 1234,
            ItemMaxSize = 567
        };

        Assert.Throws<InvalidOperationException>(() =>
        {
            using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, format, false, _fileSystem);
        });
    }

    [Theory]
    [InlineData("1.0.0", "type", 0, 53)]
    [InlineData("1.0.0", "type", 1241, 0)]
    public static void Ctor_WrongSizesPass_Fail(string version, string type, ushort metadataMaxSize, ushort itemMaxSize)
    {
        using var strm = new MemoryStream();

        var format = new ListDataFileFormat()
        {
            Version = version,
            Type = type,
            MetadataMaxSize = metadataMaxSize,
            ItemMaxSize = itemMaxSize
        };

        Assert.Throws<InvalidOperationException>(() =>
        {
            using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, format, false, _fileSystem);
        });
    }

    [Fact]
    public void Metadata_AsvSdrRecordPayloadSerialization_Success()
    {
        using var strm = new MemoryStream();
        using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, AsvSdrHelper.FileFormat, false, _fileSystem);
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
        MavlinkTypesHelper.SetGuid(payload.RecordGuid, Guid.NewGuid());
        MavlinkTypesHelper.SetString(payload.RecordName, name);

        file.EditMetadata(_ =>
        {
            payload.RecordGuid.CopyTo(_.Info.RecordGuid, 0);
            payload.RecordName.CopyTo(_.Info.RecordName, 0);
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
    public void Data_AsvSdrRecordDataLlzSerialization_Success()
    {
        using var strm = new MemoryStream();
        using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, FileFormat1, false, _fileSystem);

        Assert.False(file.Exist(0));

        var payloadRead = new AsvSdrRecordDataLlzPayload();
        Assert.False(file.Read(0, payloadRead));

        var payload = new AsvSdrRecordDataLlzPayload()
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
            MeasureTime = 0,
            GnssFixType = GpsFixType.GpsFixTypeNoGps,
            GnssSatellitesVisible = 0
        };
        file.Write(10, payload);
        file.Write(20, payload);

        var payloadRead2 = new AsvSdrRecordDataLlzPayload();
        Assert.True(file.Read(10, payloadRead2));
        payload.IsDeepEqual(payloadRead2);
        Assert.True(file.Read(20, payloadRead2));
        payload.IsDeepEqual(payloadRead2);

        Assert.False(file.Read(9, payloadRead2));
        Assert.False(file.Exist(5));
    }

    [Fact]
    public void Data_AsvSdrRecordDataGpSerialization_Success()
    {
        using var strm = new MemoryStream();
        using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, FileFormat1, false, _fileSystem);

        Assert.False(file.Exist(0));

        var payloadRead = new AsvSdrRecordDataGpPayload();
        Assert.False(file.Read(0, payloadRead));

        var payload = new AsvSdrRecordDataGpPayload()
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
            MeasureTime = 0,
            
            GnssFixType = GpsFixType.GpsFixTypeNoGps,
            GnssSatellitesVisible = 0
        };
        file.Write(10, payload);
        file.Write(20, payload);

        var payload2 = new AsvSdrRecordDataGpPayload();
        Assert.True(file.Read(10, payload2));
        payload.IsDeepEqual(payload2);
        Assert.True(file.Read(20, payload2));
        payload.IsDeepEqual(payload2);

        Assert.False(file.Read(9, payload2));
        Assert.False(file.Exist(5));
    }

    [Fact]
    public void Data_AsvSdrRecordDataRequestSerialization_Success()
    {
        using var strm = new MemoryStream();
        using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, FileFormat1, false, _fileSystem);

        Assert.False(file.Exist(0));

        var payloadRead = new AsvSdrRecordDataRequestPayload();
        Assert.False(file.Read(0, payloadRead));

        var payload = new AsvSdrRecordDataRequestPayload()
        {
            
            Count = 2,
            Skip = 1,
            TargetComponent = 1,
            TargetSystem = 2
        };
        file.Write(10, payload);
        file.Write(20, payload);

        var payloadRead2 = new AsvSdrRecordDataRequestPayload();
        Assert.True(file.Read(10, payloadRead2));
        payload.IsDeepEqual(payloadRead2);
        Assert.True(file.Read(20, payloadRead2));
        payload.IsDeepEqual(payloadRead2);

        Assert.False(file.Read(9, payloadRead2));
        Assert.False(file.Exist(5));
    }

    [Fact]
    public void Data_AsvSdrRecordDataResponseSerialization_Success()
    {
        using var strm = new MemoryStream();
        using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, FileFormat1, false, _fileSystem);

        Assert.False(file.Exist(0));

        var payloadRead = new AsvSdrRecordDataResponsePayload();
        Assert.False(file.Read(0, payloadRead));

        var payload = new AsvSdrRecordDataResponsePayload()
        {
            
            DataType = AsvSdrCustomMode.AsvSdrCustomModeIdle,
            ItemsCount = 15,
            Result = AsvSdrRequestAck.AsvSdrRequestAckOk,
            RequestId = 15
        };
        file.Write(10, payload);
        file.Write(20, payload);

        var payloadRead2 = new AsvSdrRecordDataResponsePayload();
        Assert.True(file.Read(10, payloadRead2));
        payload.IsDeepEqual(payloadRead2);
        Assert.True(file.Read(20, payloadRead2));
        payload.IsDeepEqual(payloadRead2);

        Assert.False(file.Read(9, payloadRead2));
        Assert.False(file.Exist(5));
    }

    [Fact]
    public void Data_AsvSdrRecordDataVorSerialization_Success()
    {
        using var strm = new MemoryStream();
        using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, FileFormat1, false, _fileSystem);

        Assert.False(file.Exist(0));

        var payloadRead = new AsvSdrRecordDataVorPayload();
        Assert.False(file.Read(0, payloadRead));
        var payload = new AsvSdrRecordDataVorPayload()
        {
            
            Alt = 500,
            Am30 = (float)15.0,
            Am9960 = (float)80.0,
            Azimuth = 180,
            GnssAlt = 505,
            CarrierOffset = 1,
            CodeIdAm1020 = (float)100.0,
            DataIndex = 13,
            CodeIdFreq1020 = 13,
            Deviation = 10,
            FieldStrength = 100,
            Freq30 = 30,
            Freq9960 = 9960,
            GnssEph = 100,
            GnssLat = 10,
            GnssEpv = 100,
            GnssLon = 20,
            GnssVel = 1,
            GnssAltEllipsoid = 100,
            GnssFixType = GpsFixType.GpsFixTypeDgps,
            GnssHAcc = 1,
            GnssSatellitesVisible = 15,
            GnssVAcc = 1,
            GnssVelAcc = 12
        };
        file.Write(10, payload);
        file.Write(20, payload);

        var payloadRead2 = new AsvSdrRecordDataVorPayload();
        Assert.True(file.Read(10, payloadRead2));
        payload.IsDeepEqual(payloadRead2);
        Assert.True(file.Read(20, payloadRead2));
        payload.IsDeepEqual(payloadRead2);

        Assert.False(file.Read(9, payloadRead2));
        Assert.False(file.Exist(5));
    }

    [Fact]
    public void Data_TryToReWriteToSameIndex_Success()
    {
        using var strm = new MemoryStream();
        var payload = new AsvSdrRecordDataVorPayload()
        {
            
            Alt = 500,
            Am30 = (float)15.0,
            Am9960 = (float)80.0,
            Azimuth = 180,
            GnssAlt = 505,
            CarrierOffset = 1,
            CodeIdAm1020 = (float)100.0,
            DataIndex = 13,
            CodeIdFreq1020 = 13,
            Deviation = 10,
            FieldStrength = 100,
            Freq30 = 30,
            Freq9960 = 9960,
            GnssEph = 100,
            GnssLat = 10,
            GnssEpv = 100,
            GnssLon = 20,
            GnssVel = 1,
            GnssAltEllipsoid = 100,
            GnssFixType = GpsFixType.GpsFixTypeDgps,
            GnssHAcc = 1,
            GnssSatellitesVisible = 15,
            GnssVAcc = 1,
            GnssVelAcc = 12
        };
        var format = new ListDataFileFormat()
        {
            ItemMaxSize = (ushort)payload.GetByteSize(),
            MetadataMaxSize = (ushort)payload.GetByteSize(),
            Type = "TestFilef",
            Version = SemVersion.Parse("0.0.1")
        };
        var payloadRead = new AsvSdrRecordDataVorPayload();
        using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, format, false, _fileSystem);
        file.Write(0, payload);
        file.Write(0, payload);
        file.Read(0, payloadRead);
        Assert.True(payload.IsDeepEqual(payloadRead));
        file.Read(100, payloadRead);
    }

    [Fact]
    public async Task Data_ThreadSafeTestForEditMetadata_Success()
    {
        using var strm = new MemoryStream();
        var payload = new AsvSdrRecordDataVorPayload()
        {
            
            Alt = 500,
            Am30 = (float)15.0,
            Am9960 = (float)80.0,
            Azimuth = 180,
            GnssAlt = 505,
            CarrierOffset = 1,
            CodeIdAm1020 = (float)100.0,
            DataIndex = 13,
            CodeIdFreq1020 = 13,
            Deviation = 10,
            FieldStrength = 100,
            Freq30 = 30,
            Freq9960 = 9960,
            GnssEph = 100,
            GnssLat = 10,
            GnssEpv = 100,
            GnssLon = 20,
            GnssVel = 1,
            GnssAltEllipsoid = 100,
            GnssFixType = GpsFixType.GpsFixTypeDgps,
            GnssHAcc = 1,
            GnssSatellitesVisible = 15,
            GnssVAcc = 1,
            GnssVelAcc = 12
        };

        var format = new ListDataFileFormat()
        {
            ItemMaxSize = (ushort)payload.GetByteSize(),
            MetadataMaxSize = (ushort)payload.GetByteSize(),
            Type = "TestFilef",
            Version = SemVersion.Parse("0.0.1")
        };
        var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, format, false, _fileSystem);
        await Task.Run(() =>
        {
            for (var i = 0; i < 100; i++)
            {
                file.Write(payload);
            }
        });
        await Task.Run(() =>
        {
            file.EditMetadata(_ =>
            {
                _.Tags.Add(new AsvSdrRecordTagPayload()
                {
                    
                    TagType = AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64
                });
            });
        });
    }

    [Fact]
    public void Format_MetadataMaxSizeLessThanItemSize_Fail()
    {
        using var strm = new MemoryStream();
        var format = new ListDataFileFormat()
        {
            ItemMaxSize = 40,
            MetadataMaxSize = 39,
            Type = "TestFilef",
            Version = SemVersion.Parse("0.0.1")
        };
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, format, false, _fileSystem);
        });
    }


    [Fact]
    public static void Metadata_TryPassNullValue_Fail()
    {
        using var strm = new MemoryStream();
        IListDataFile<AsvSdrRecordFileMetadata> file =
            new ListDataFile<AsvSdrRecordFileMetadata>(strm, FileFormat1, false, _fileSystem);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<NullReferenceException>(() => { file.EditMetadata(null); });
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public static void Data_TryPassNullValueAsItem_Fail()
    {
        using var strm = new MemoryStream();
        IListDataFile<AsvSdrRecordFileMetadata> file =
            new ListDataFile<AsvSdrRecordFileMetadata>(strm, FileFormat1, false, _fileSystem);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<NullReferenceException>(() => { file.Write(0, null); });
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public static void Different_Headers_Read_Fail()
    {
        using var strm = new MemoryStream();

        var file1 = new ListDataFile<AsvSdrRecordFileMetadata>(strm, FileFormat1, false, _fileSystem);

        file1.Dispose();

        Assert.Throws<Exception>(() =>
        {
            var file2 = new ListDataFile<AsvSdrRecordFileMetadata>(strm, FileFormat2, false, _fileSystem);
        });
    }

    [Fact]
    public static void Format_WrongVersionHeader_Fail()
    {
        using var strm = new MemoryStream();
        Assert.Throws<ArgumentException>(() =>
        {
            var format = new ListDataFileFormat()
            {
                Version = "aboba",
                Type = "type",
                MetadataMaxSize = 1234,
                ItemMaxSize = 567
            };
        });
    }


    [Fact]
    public static void Format_StringEmptyTypeHeader_Fail()
    {
        using var strm = new MemoryStream();

        var format = new ListDataFileFormat()
        {
            Version = "1.0.0",
            Type = "",
            MetadataMaxSize = 1234,
            ItemMaxSize = 567
        };

        Assert.Throws<InvalidOperationException>(() =>
        {
            using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, format, false, _fileSystem);
        });
    }

    [Fact]
    public static void Format_NullValueTypeHeader_Fail()
    {
        using var strm = new MemoryStream();

        var format = new ListDataFileFormat()
        {
            Version = "1.0.0",
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Type = null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            MetadataMaxSize = 1234,
            ItemMaxSize = 567
        };

        Assert.Throws<InvalidOperationException>(() =>
        {
            using var file = new ListDataFile<AsvSdrRecordFileMetadata>(strm, format, false, _fileSystem);
        });
    }
}
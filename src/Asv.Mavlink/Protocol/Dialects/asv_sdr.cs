// MIT License
//
// Copyright (c) 2018 Alexey (https://github.com/asvol)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// This code was generate by tool Asv.Mavlink.Shell version 2.0.2

using System;
using System.Text;
using Asv.Mavlink.V2.Common;
using Asv.IO;

namespace Asv.Mavlink.V2.AsvSdr
{

    public static class AsvSdrHelper
    {
        public static void RegisterAsvSdrDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AsvSdrOutStatusPacket());
            src.Register(()=>new AsvSdrIlsRecordPacket());
            src.Register(()=>new AsvSdrVorRecordPacket());
        }
    }

#region Enums

    /// <summary>
    ///  MAV_TYPE
    /// </summary>
    public enum MavType:uint
    {
        /// <summary>
        /// Used to identify Software-defined radio payload in HEARTBEAT packet.
        /// MAV_TYPE_ASV_SDR_PAYLOAD
        /// </summary>
        MavTypeAsvSdrPayload = 251,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Start recording SDR data
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_START_RECORD
        /// </summary>
        MavCmdAsvSdrStartRecord = 13051,
    }

    /// <summary>
    /// A mapping of SDR payload modes for custom_mode field of heartbeat
    ///  ASV_SDR_CUSTOM_MODE
    /// </summary>
    public enum AsvSdrCustomMode:uint
    {
        /// <summary>
        /// ASV_SDR_CUSTOM_MODE_LOADING
        /// </summary>
        AsvSdrCustomModeLoading = 0,
        /// <summary>
        /// ASV_SDR_CUSTOM_MODE_IDLE
        /// </summary>
        AsvSdrCustomModeIdle = 1,
        /// <summary>
        /// ASV_SDR_CUSTOM_MODE_ERROR
        /// </summary>
        AsvSdrCustomModeError = 2,
        /// <summary>
        /// ASV_SDR_CUSTOM_MODE_ILS
        /// </summary>
        AsvSdrCustomModeIls = 3,
        /// <summary>
        /// ASV_SDR_CUSTOM_MODE_ILS_IN_PROGRESS
        /// </summary>
        AsvSdrCustomModeIlsInProgress = 4,
        /// <summary>
        /// ASV_SDR_CUSTOM_MODE_VOR
        /// </summary>
        AsvSdrCustomModeVor = 5,
        /// <summary>
        /// ASV_SDR_CUSTOM_MODE_VOR_IN_PROGRESS
        /// </summary>
        AsvSdrCustomModeVorInProgress = 6,
    }


#endregion

#region Messages

    /// <summary>
    /// SDR payload status message. Send with 1 Hz frequency.
    ///  ASV_SDR_OUT_STATUS
    /// </summary>
    public class AsvSdrOutStatusPacket: PacketV2<AsvSdrOutStatusPayload>
    {
	    public const int PacketMessageId = 13100;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 102;

        public override AsvSdrOutStatusPayload Payload { get; } = new AsvSdrOutStatusPayload();

        public override string Name => "ASV_SDR_OUT_STATUS";
    }

    /// <summary>
    ///  ASV_SDR_OUT_STATUS
    /// </summary>
    public class AsvSdrOutStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 0; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 0; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {

        }

        public void Serialize(ref Span<byte> buffer)
        {
            /* PayloadByteSize = 0 */;
        }



    }
    /// <summary>
    /// ILS reciever record data.
    ///  ASV_SDR_ILS_RECORD
    /// </summary>
    public class AsvSdrIlsRecordPacket: PacketV2<AsvSdrIlsRecordPayload>
    {
	    public const int PacketMessageId = 13101;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 123;

        public override AsvSdrIlsRecordPayload Payload { get; } = new AsvSdrIlsRecordPayload();

        public override string Name => "ASV_SDR_ILS_RECORD";
    }

    /// <summary>
    ///  ASV_SDR_ILS_RECORD
    /// </summary>
    public class AsvSdrIlsRecordPayload : IPayload
    {
        public byte GetMaxByteSize() => 156; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 156; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Index = BinSerialize.ReadUInt(ref buffer);
            GnssLat = BinSerialize.ReadInt(ref buffer);
            GnssLon = BinSerialize.ReadInt(ref buffer);
            GnssAlt = BinSerialize.ReadInt(ref buffer);
            GnssAltEllipsoid = BinSerialize.ReadInt(ref buffer);
            GnssHAcc = BinSerialize.ReadUInt(ref buffer);
            GnssVAcc = BinSerialize.ReadUInt(ref buffer);
            GnssVelAcc = BinSerialize.ReadUInt(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lon = BinSerialize.ReadInt(ref buffer);
            Alt = BinSerialize.ReadInt(ref buffer);
            RelativeAlt = BinSerialize.ReadInt(ref buffer);
            Roll = BinSerialize.ReadFloat(ref buffer);
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            CrsAm90 = BinSerialize.ReadFloat(ref buffer);
            CrsAm150 = BinSerialize.ReadFloat(ref buffer);
            ClrPower = BinSerialize.ReadFloat(ref buffer);
            ClrAm90 = BinSerialize.ReadFloat(ref buffer);
            ClrAm150 = BinSerialize.ReadFloat(ref buffer);
            TotalFreq = BinSerialize.ReadUInt(ref buffer);
            TotalPower = BinSerialize.ReadFloat(ref buffer);
            TotalFieldStrength = BinSerialize.ReadFloat(ref buffer);
            TotalAm90 = BinSerialize.ReadFloat(ref buffer);
            TotalAm150 = BinSerialize.ReadFloat(ref buffer);
            Phi90CrsVsClr = BinSerialize.ReadFloat(ref buffer);
            Phi150CrsVsClr = BinSerialize.ReadFloat(ref buffer);
            CodeIdAm1020 = BinSerialize.ReadFloat(ref buffer);
            GnssEph = BinSerialize.ReadUShort(ref buffer);
            GnssEpv = BinSerialize.ReadUShort(ref buffer);
            GnssVel = BinSerialize.ReadUShort(ref buffer);
            Vx = BinSerialize.ReadShort(ref buffer);
            Vy = BinSerialize.ReadShort(ref buffer);
            Vz = BinSerialize.ReadShort(ref buffer);
            Hdg = BinSerialize.ReadUShort(ref buffer);
            CrsPower = BinSerialize.ReadShort(ref buffer);
            CrsCarrierOffset = BinSerialize.ReadShort(ref buffer);
            CrsFreq90 = BinSerialize.ReadShort(ref buffer);
            CrsFreq150 = BinSerialize.ReadShort(ref buffer);
            ClrCarrierOffset = BinSerialize.ReadShort(ref buffer);
            ClrFreq90 = BinSerialize.ReadShort(ref buffer);
            ClrFreq150 = BinSerialize.ReadShort(ref buffer);
            TotalCarrierOffset = BinSerialize.ReadShort(ref buffer);
            TotalFreq90 = BinSerialize.ReadShort(ref buffer);
            TotalFreq150 = BinSerialize.ReadShort(ref buffer);
            CodeIdFreq1020 = BinSerialize.ReadShort(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            GnssFixType = (GpsFixType)BinSerialize.ReadByte(ref buffer);
            GnssSatellitesVisible = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/156 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            CodeId = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, CodeId.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,Index);
            BinSerialize.WriteInt(ref buffer,GnssLat);
            BinSerialize.WriteInt(ref buffer,GnssLon);
            BinSerialize.WriteInt(ref buffer,GnssAlt);
            BinSerialize.WriteInt(ref buffer,GnssAltEllipsoid);
            BinSerialize.WriteUInt(ref buffer,GnssHAcc);
            BinSerialize.WriteUInt(ref buffer,GnssVAcc);
            BinSerialize.WriteUInt(ref buffer,GnssVelAcc);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lon);
            BinSerialize.WriteInt(ref buffer,Alt);
            BinSerialize.WriteInt(ref buffer,RelativeAlt);
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteFloat(ref buffer,CrsAm90);
            BinSerialize.WriteFloat(ref buffer,CrsAm150);
            BinSerialize.WriteFloat(ref buffer,ClrPower);
            BinSerialize.WriteFloat(ref buffer,ClrAm90);
            BinSerialize.WriteFloat(ref buffer,ClrAm150);
            BinSerialize.WriteUInt(ref buffer,TotalFreq);
            BinSerialize.WriteFloat(ref buffer,TotalPower);
            BinSerialize.WriteFloat(ref buffer,TotalFieldStrength);
            BinSerialize.WriteFloat(ref buffer,TotalAm90);
            BinSerialize.WriteFloat(ref buffer,TotalAm150);
            BinSerialize.WriteFloat(ref buffer,Phi90CrsVsClr);
            BinSerialize.WriteFloat(ref buffer,Phi150CrsVsClr);
            BinSerialize.WriteFloat(ref buffer,CodeIdAm1020);
            BinSerialize.WriteUShort(ref buffer,GnssEph);
            BinSerialize.WriteUShort(ref buffer,GnssEpv);
            BinSerialize.WriteUShort(ref buffer,GnssVel);
            BinSerialize.WriteShort(ref buffer,Vx);
            BinSerialize.WriteShort(ref buffer,Vy);
            BinSerialize.WriteShort(ref buffer,Vz);
            BinSerialize.WriteUShort(ref buffer,Hdg);
            BinSerialize.WriteShort(ref buffer,CrsPower);
            BinSerialize.WriteShort(ref buffer,CrsCarrierOffset);
            BinSerialize.WriteShort(ref buffer,CrsFreq90);
            BinSerialize.WriteShort(ref buffer,CrsFreq150);
            BinSerialize.WriteShort(ref buffer,ClrCarrierOffset);
            BinSerialize.WriteShort(ref buffer,ClrFreq90);
            BinSerialize.WriteShort(ref buffer,ClrFreq150);
            BinSerialize.WriteShort(ref buffer,TotalCarrierOffset);
            BinSerialize.WriteShort(ref buffer,TotalFreq90);
            BinSerialize.WriteShort(ref buffer,TotalFreq150);
            BinSerialize.WriteShort(ref buffer,CodeIdFreq1020);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            BinSerialize.WriteByte(ref buffer,(byte)GnssFixType);
            BinSerialize.WriteByte(ref buffer,(byte)GnssSatellitesVisible);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId)
                {
                    Encoding.ASCII.GetBytes(charPointer, CodeId.Length, bytePointer, CodeId.Length);
                }
            }
            buffer = buffer.Slice(CodeId.Length);
            
            /* PayloadByteSize = 156 */;
        }



        /// <summary>
        /// Record index
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
        /// <summary>
        /// Latitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public int GnssLat { get; set; }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public int GnssLon { get; set; }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public int GnssAlt { get; set; }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public int GnssAltEllipsoid { get; set; }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssHAcc { get; set; }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssVAcc { get; set; }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssVelAcc { get; set; }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public int Lon { get; set; }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public int Alt { get; set; }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public int RelativeAlt { get; set; }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Aplitude modulation of 90Hz of course.
        /// OriginName: crs_am_90, Units: %, IsExtended: false
        /// </summary>
        public float CrsAm90 { get; set; }
        /// <summary>
        /// Aplitude modulation of 150Hz of course.
        /// OriginName: crs_am_150, Units: %, IsExtended: false
        /// </summary>
        public float CrsAm150 { get; set; }
        /// <summary>
        /// Input power of clearance.
        /// OriginName: clr_power, Units: dBm, IsExtended: false
        /// </summary>
        public float ClrPower { get; set; }
        /// <summary>
        /// Aplitude modulation of 90Hz of clearance.
        /// OriginName: clr_am_90, Units: %, IsExtended: false
        /// </summary>
        public float ClrAm90 { get; set; }
        /// <summary>
        /// Aplitude modulation of 150Hz of clearance.
        /// OriginName: clr_am_150, Units: % E2, IsExtended: false
        /// </summary>
        public float ClrAm150 { get; set; }
        /// <summary>
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public uint TotalFreq { get; set; }
        /// <summary>
        /// Total input power.
        /// OriginName: total_power, Units: dBm, IsExtended: false
        /// </summary>
        public float TotalPower { get; set; }
        /// <summary>
        /// Total field strength.
        /// OriginName: total_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public float TotalFieldStrength { get; set; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: total_am_90, Units: %, IsExtended: false
        /// </summary>
        public float TotalAm90 { get; set; }
        /// <summary>
        /// Total aplitude modulation of 150Hz.
        /// OriginName: total_am_150, Units: %, IsExtended: false
        /// </summary>
        public float TotalAm150 { get; set; }
        /// <summary>
        ///  Phase difference 90 Hz clearance and cource
        /// OriginName: phi_90_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public float Phi90CrsVsClr { get; set; }
        /// <summary>
        /// Phase difference 150 Hz clearance and cource.
        /// OriginName: phi_150_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public float Phi150CrsVsClr { get; set; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: code_id_am_1020, Units: %, IsExtended: false
        /// </summary>
        public float CodeIdAm1020 { get; set; }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public ushort GnssEph { get; set; }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public ushort GnssEpv { get; set; }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public ushort GnssVel { get; set; }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vx { get; set; }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vy { get; set; }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vz { get; set; }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public ushort Hdg { get; set; }
        /// <summary>
        /// Input power of course.
        /// OriginName: crs_power, Units: dBm, IsExtended: false
        /// </summary>
        public short CrsPower { get; set; }
        /// <summary>
        /// Carrier frequency offset of course.
        /// OriginName: crs_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short CrsCarrierOffset { get; set; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of course.
        /// OriginName: crs_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public short CrsFreq90 { get; set; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of course.
        /// OriginName: crs_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public short CrsFreq150 { get; set; }
        /// <summary>
        /// Carrier frequency offset of clearance.
        /// OriginName: clr_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short ClrCarrierOffset { get; set; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of clearance.
        /// OriginName: clr_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public short ClrFreq90 { get; set; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of clearance.
        /// OriginName: clr_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public short ClrFreq150 { get; set; }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: total_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short TotalCarrierOffset { get; set; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: total_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public short TotalFreq90 { get; set; }
        /// <summary>
        /// Total frequency offset of signal 150 Hz.
        /// OriginName: total_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public short TotalFreq150 { get; set; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: code_id_freq_1020, Units: Hz, IsExtended: false
        /// </summary>
        public short CodeIdFreq1020 { get; set; }
        /// <summary>
        /// Measure time.
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public short MeasureTime { get; set; }
        /// <summary>
        /// GPS fix type.
        /// OriginName: gnss_fix_type, Units: , IsExtended: false
        /// </summary>
        public GpsFixType GnssFixType { get; set; }
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public byte GnssSatellitesVisible { get; set; }
        /// <summary>
        /// Code identification
        /// OriginName: code_id, Units: Letters, IsExtended: false
        /// </summary>
        public char[] CodeId { get; set; } = new char[4];
        public byte GetCodeIdMaxItemsCount() => 4;
    }
    /// <summary>
    /// VOR reciever record data.
    ///  ASV_SDR_VOR_RECORD
    /// </summary>
    public class AsvSdrVorRecordPacket: PacketV2<AsvSdrVorRecordPayload>
    {
	    public const int PacketMessageId = 13102;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 61;

        public override AsvSdrVorRecordPayload Payload { get; } = new AsvSdrVorRecordPayload();

        public override string Name => "ASV_SDR_VOR_RECORD";
    }

    /// <summary>
    ///  ASV_SDR_VOR_RECORD
    /// </summary>
    public class AsvSdrVorRecordPayload : IPayload
    {
        public byte GetMaxByteSize() => 122; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 122; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Index = BinSerialize.ReadUInt(ref buffer);
            GnssLat = BinSerialize.ReadInt(ref buffer);
            GnssLon = BinSerialize.ReadInt(ref buffer);
            GnssAlt = BinSerialize.ReadInt(ref buffer);
            GnssAltEllipsoid = BinSerialize.ReadInt(ref buffer);
            GnssHAcc = BinSerialize.ReadUInt(ref buffer);
            GnssVAcc = BinSerialize.ReadUInt(ref buffer);
            GnssVelAcc = BinSerialize.ReadUInt(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lon = BinSerialize.ReadInt(ref buffer);
            Alt = BinSerialize.ReadInt(ref buffer);
            RelativeAlt = BinSerialize.ReadInt(ref buffer);
            Roll = BinSerialize.ReadFloat(ref buffer);
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            Freq = BinSerialize.ReadUInt(ref buffer);
            Azimuth = BinSerialize.ReadFloat(ref buffer);
            Power = BinSerialize.ReadFloat(ref buffer);
            FieldStrength = BinSerialize.ReadFloat(ref buffer);
            Am30 = BinSerialize.ReadFloat(ref buffer);
            Am9960 = BinSerialize.ReadFloat(ref buffer);
            Deviation = BinSerialize.ReadFloat(ref buffer);
            CodeIdAm1020 = BinSerialize.ReadFloat(ref buffer);
            GnssEph = BinSerialize.ReadUShort(ref buffer);
            GnssEpv = BinSerialize.ReadUShort(ref buffer);
            GnssVel = BinSerialize.ReadUShort(ref buffer);
            Vx = BinSerialize.ReadShort(ref buffer);
            Vy = BinSerialize.ReadShort(ref buffer);
            Vz = BinSerialize.ReadShort(ref buffer);
            Hdg = BinSerialize.ReadUShort(ref buffer);
            CarrierOffset = BinSerialize.ReadShort(ref buffer);
            Freq30 = BinSerialize.ReadShort(ref buffer);
            Freq9960 = BinSerialize.ReadShort(ref buffer);
            CodeIdFreq1020 = BinSerialize.ReadShort(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            GnssFixType = (GpsFixType)BinSerialize.ReadByte(ref buffer);
            GnssSatellitesVisible = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/122 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            CodeId = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, CodeId.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,Index);
            BinSerialize.WriteInt(ref buffer,GnssLat);
            BinSerialize.WriteInt(ref buffer,GnssLon);
            BinSerialize.WriteInt(ref buffer,GnssAlt);
            BinSerialize.WriteInt(ref buffer,GnssAltEllipsoid);
            BinSerialize.WriteUInt(ref buffer,GnssHAcc);
            BinSerialize.WriteUInt(ref buffer,GnssVAcc);
            BinSerialize.WriteUInt(ref buffer,GnssVelAcc);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lon);
            BinSerialize.WriteInt(ref buffer,Alt);
            BinSerialize.WriteInt(ref buffer,RelativeAlt);
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteUInt(ref buffer,Freq);
            BinSerialize.WriteFloat(ref buffer,Azimuth);
            BinSerialize.WriteFloat(ref buffer,Power);
            BinSerialize.WriteFloat(ref buffer,FieldStrength);
            BinSerialize.WriteFloat(ref buffer,Am30);
            BinSerialize.WriteFloat(ref buffer,Am9960);
            BinSerialize.WriteFloat(ref buffer,Deviation);
            BinSerialize.WriteFloat(ref buffer,CodeIdAm1020);
            BinSerialize.WriteUShort(ref buffer,GnssEph);
            BinSerialize.WriteUShort(ref buffer,GnssEpv);
            BinSerialize.WriteUShort(ref buffer,GnssVel);
            BinSerialize.WriteShort(ref buffer,Vx);
            BinSerialize.WriteShort(ref buffer,Vy);
            BinSerialize.WriteShort(ref buffer,Vz);
            BinSerialize.WriteUShort(ref buffer,Hdg);
            BinSerialize.WriteShort(ref buffer,CarrierOffset);
            BinSerialize.WriteShort(ref buffer,Freq30);
            BinSerialize.WriteShort(ref buffer,Freq9960);
            BinSerialize.WriteShort(ref buffer,CodeIdFreq1020);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            BinSerialize.WriteByte(ref buffer,(byte)GnssFixType);
            BinSerialize.WriteByte(ref buffer,(byte)GnssSatellitesVisible);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId)
                {
                    Encoding.ASCII.GetBytes(charPointer, CodeId.Length, bytePointer, CodeId.Length);
                }
            }
            buffer = buffer.Slice(CodeId.Length);
            
            /* PayloadByteSize = 122 */;
        }



        /// <summary>
        /// Record index
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
        /// <summary>
        /// Latitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public int GnssLat { get; set; }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public int GnssLon { get; set; }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public int GnssAlt { get; set; }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public int GnssAltEllipsoid { get; set; }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssHAcc { get; set; }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssVAcc { get; set; }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssVelAcc { get; set; }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public int Lon { get; set; }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public int Alt { get; set; }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public int RelativeAlt { get; set; }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Frequency.
        /// OriginName: freq, Units: Hz, IsExtended: false
        /// </summary>
        public uint Freq { get; set; }
        /// <summary>
        /// Measured azimuth.
        /// OriginName: azimuth, Units: deg, IsExtended: false
        /// </summary>
        public float Azimuth { get; set; }
        /// <summary>
        /// Total input power.
        /// OriginName: power, Units: dBm, IsExtended: false
        /// </summary>
        public float Power { get; set; }
        /// <summary>
        /// Total field strength.
        /// OriginName: field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public float FieldStrength { get; set; }
        /// <summary>
        /// Total aplitude modulation of 30 Hz.
        /// OriginName: am_30, Units: %, IsExtended: false
        /// </summary>
        public float Am30 { get; set; }
        /// <summary>
        /// Total aplitude modulation of 9960 Hz.
        /// OriginName: am_9960, Units: %, IsExtended: false
        /// </summary>
        public float Am9960 { get; set; }
        /// <summary>
        /// Deviation.
        /// OriginName: deviation, Units: , IsExtended: false
        /// </summary>
        public float Deviation { get; set; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: code_id_am_1020, Units: %, IsExtended: false
        /// </summary>
        public float CodeIdAm1020 { get; set; }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public ushort GnssEph { get; set; }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public ushort GnssEpv { get; set; }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public ushort GnssVel { get; set; }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vx { get; set; }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vy { get; set; }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vz { get; set; }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public ushort Hdg { get; set; }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short CarrierOffset { get; set; }
        /// <summary>
        /// Total frequency offset of signal 30 Hz.
        /// OriginName: freq_30, Units: Hz, IsExtended: false
        /// </summary>
        public short Freq30 { get; set; }
        /// <summary>
        /// Total frequency offset of signal 9960 Hz.
        /// OriginName: freq_9960, Units: Hz, IsExtended: false
        /// </summary>
        public short Freq9960 { get; set; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: code_id_freq_1020, Units: Hz, IsExtended: false
        /// </summary>
        public short CodeIdFreq1020 { get; set; }
        /// <summary>
        /// Measure time.
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public short MeasureTime { get; set; }
        /// <summary>
        /// GPS fix type.
        /// OriginName: gnss_fix_type, Units: , IsExtended: false
        /// </summary>
        public GpsFixType GnssFixType { get; set; }
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public byte GnssSatellitesVisible { get; set; }
        /// <summary>
        /// Code identification
        /// OriginName: code_id, Units: Letters, IsExtended: false
        /// </summary>
        public char[] CodeId { get; set; } = new char[4];
        public byte GetCodeIdMaxItemsCount() => 4;
    }


#endregion


}

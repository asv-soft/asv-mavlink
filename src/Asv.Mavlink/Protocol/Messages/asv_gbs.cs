// MIT License
//
// Copyright (c) 2025 asv-soft (https://github.com/asv-soft)
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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.15+3a942e4794bafbc9b7e025a76c610b9704955531 25-05-11.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.IO;

namespace Asv.Mavlink.AsvGbs
{

    public static class AsvGbsHelper
    {
        public static void RegisterAsvGbsDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(AsvGbsOutStatusPacket.MessageId, ()=>new AsvGbsOutStatusPacket());
        }
    }

#region Enums

    /// <summary>
    ///  MAV_TYPE
    /// </summary>
    public enum MavType:uint
    {
        /// <summary>
        /// Used to identify RTK ground base station in HEARTBEAT packet.
        /// MAV_TYPE_ASV_GBS
        /// </summary>
        MavTypeAsvGbs = 250,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Run in observation mode to determine the current position of GBS and start sending RTK corrections.
        /// Param 1 - Minimum observation time (seconds).
        /// Param 2 - Minimum position accuracy (m).
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_GBS_RUN_AUTO_MODE
        /// </summary>
        MavCmdAsvGbsRunAutoMode = 13001,
        /// <summary>
        /// Run in fixed mode with well known GBS position and start sending RTK corrections.
        /// Param 1 - Latitude (int32_t,degE7).
        /// Param 2 - Longitude (int32_t,degE7).
        /// Param 3 - Altitude (int32_t, mm).
        /// Param 4 - Position accuracy (m).
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_GBS_RUN_FIXED_MODE
        /// </summary>
        MavCmdAsvGbsRunFixedMode = 13002,
        /// <summary>
        /// Cancel all modes and switch to default state. Used to cancel all other modes and disable RTK sending.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_GBS_RUN_IDLE_MODE
        /// </summary>
        MavCmdAsvGbsRunIdleMode = 13003,
    }

    /// <summary>
    /// A mapping of GBS modes for custom_mode field of heartbeat.
    ///  ASV_GBS_CUSTOM_MODE
    /// </summary>
    public enum AsvGbsCustomMode:uint
    {
        /// <summary>
        /// ASV_GBS_CUSTOM_MODE_LOADING
        /// </summary>
        AsvGbsCustomModeLoading = 0,
        /// <summary>
        /// ASV_GBS_CUSTOM_MODE_IDLE
        /// </summary>
        AsvGbsCustomModeIdle = 1,
        /// <summary>
        /// ASV_GBS_CUSTOM_MODE_ERROR
        /// </summary>
        AsvGbsCustomModeError = 2,
        /// <summary>
        /// ASV_GBS_CUSTOM_MODE_AUTO_IN_PROGRESS
        /// </summary>
        AsvGbsCustomModeAutoInProgress = 3,
        /// <summary>
        /// ASV_GBS_CUSTOM_MODE_AUTO
        /// </summary>
        AsvGbsCustomModeAuto = 4,
        /// <summary>
        /// ASV_GBS_CUSTOM_MODE_FIXED_IN_PROGRESS
        /// </summary>
        AsvGbsCustomModeFixedInProgress = 5,
        /// <summary>
        /// ASV_GBS_CUSTOM_MODE_FIXED
        /// </summary>
        AsvGbsCustomModeFixed = 6,
    }


#endregion

#region Messages

    /// <summary>
    /// Ground base station status message. Send with 1 Hz frequency.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_GBS_OUT_STATUS
    /// </summary>
    public class AsvGbsOutStatusPacket : MavlinkV2Message<AsvGbsOutStatusPayload>
    {
        public const int MessageId = 13000;
        
        public const byte CrcExtra = 216;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvGbsOutStatusPayload Payload { get; } = new();

        public override string Name => "ASV_GBS_OUT_STATUS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "lat",
            "Latitude of GBS (value / 10000000D).",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(1,
            "lng",
            "Longitude of GBS (value / 10000000D).",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(2,
            "alt",
            "Altitude of GBS.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(3,
            "accuracy",
            "Current position accuracy (cm).",
            string.Empty, 
            @"cm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(4,
            "observation",
            "Observation time (seconds).",
            string.Empty, 
            @"s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(5,
            "dgps_rate",
            "Rate of GPS_RTCM_DATA data.",
            string.Empty, 
            @"bytes\seconds", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(6,
            "sat_all",
            "All GNSS satellite count.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(7,
            "sat_gps",
            "GPS satellite count.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(8,
            "sat_glo",
            "GLONASS satellite count.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(9,
            "sat_bdu",
            "BeiDou satellite count.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(10,
            "sat_gal",
            "Galileo satellite count.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(11,
            "sat_qzs",
            "QZSS satellite count.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(12,
            "sat_ime",
            "IMES satellite count.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(13,
            "sat_sbs",
            "SBAS satellite count.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_GBS_OUT_STATUS:"
        + "int32_t lat;"
        + "int32_t lng;"
        + "int32_t alt;"
        + "uint16_t accuracy;"
        + "uint16_t observation;"
        + "uint16_t dgps_rate;"
        + "uint8_t sat_all;"
        + "uint8_t sat_gps;"
        + "uint8_t sat_glo;"
        + "uint8_t sat_bdu;"
        + "uint8_t sat_gal;"
        + "uint8_t sat_qzs;"
        + "uint8_t sat_ime;"
        + "uint8_t sat_sbs;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Lat);
            writer.Write(StaticFields[1], Payload.Lng);
            writer.Write(StaticFields[2], Payload.Alt);
            writer.Write(StaticFields[3], Payload.Accuracy);
            writer.Write(StaticFields[4], Payload.Observation);
            writer.Write(StaticFields[5], Payload.DgpsRate);
            writer.Write(StaticFields[6], Payload.SatAll);
            writer.Write(StaticFields[7], Payload.SatGps);
            writer.Write(StaticFields[8], Payload.SatGlo);
            writer.Write(StaticFields[9], Payload.SatBdu);
            writer.Write(StaticFields[10], Payload.SatGal);
            writer.Write(StaticFields[11], Payload.SatQzs);
            writer.Write(StaticFields[12], Payload.SatIme);
            writer.Write(StaticFields[13], Payload.SatSbs);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Lat = reader.ReadInt(StaticFields[0]);
            Payload.Lng = reader.ReadInt(StaticFields[1]);
            Payload.Alt = reader.ReadInt(StaticFields[2]);
            Payload.Accuracy = reader.ReadUShort(StaticFields[3]);
            Payload.Observation = reader.ReadUShort(StaticFields[4]);
            Payload.DgpsRate = reader.ReadUShort(StaticFields[5]);
            Payload.SatAll = reader.ReadByte(StaticFields[6]);
            Payload.SatGps = reader.ReadByte(StaticFields[7]);
            Payload.SatGlo = reader.ReadByte(StaticFields[8]);
            Payload.SatBdu = reader.ReadByte(StaticFields[9]);
            Payload.SatGal = reader.ReadByte(StaticFields[10]);
            Payload.SatQzs = reader.ReadByte(StaticFields[11]);
            Payload.SatIme = reader.ReadByte(StaticFields[12]);
            Payload.SatSbs = reader.ReadByte(StaticFields[13]);
        
            
        }
    }

    /// <summary>
    ///  ASV_GBS_OUT_STATUS
    /// </summary>
    public class AsvGbsOutStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 26; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 26; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // int32_t lat
            +4 // int32_t lng
            +4 // int32_t alt
            +2 // uint16_t accuracy
            +2 // uint16_t observation
            +2 // uint16_t dgps_rate
            +1 // uint8_t sat_all
            +1 // uint8_t sat_gps
            +1 // uint8_t sat_glo
            +1 // uint8_t sat_bdu
            +1 // uint8_t sat_gal
            +1 // uint8_t sat_qzs
            +1 // uint8_t sat_ime
            +1 // uint8_t sat_sbs
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Lat = BinSerialize.ReadInt(ref buffer);
            Lng = BinSerialize.ReadInt(ref buffer);
            Alt = BinSerialize.ReadInt(ref buffer);
            Accuracy = BinSerialize.ReadUShort(ref buffer);
            Observation = BinSerialize.ReadUShort(ref buffer);
            DgpsRate = BinSerialize.ReadUShort(ref buffer);
            SatAll = (byte)BinSerialize.ReadByte(ref buffer);
            SatGps = (byte)BinSerialize.ReadByte(ref buffer);
            SatGlo = (byte)BinSerialize.ReadByte(ref buffer);
            SatBdu = (byte)BinSerialize.ReadByte(ref buffer);
            SatGal = (byte)BinSerialize.ReadByte(ref buffer);
            SatQzs = (byte)BinSerialize.ReadByte(ref buffer);
            SatIme = (byte)BinSerialize.ReadByte(ref buffer);
            SatSbs = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lng);
            BinSerialize.WriteInt(ref buffer,Alt);
            BinSerialize.WriteUShort(ref buffer,Accuracy);
            BinSerialize.WriteUShort(ref buffer,Observation);
            BinSerialize.WriteUShort(ref buffer,DgpsRate);
            BinSerialize.WriteByte(ref buffer,(byte)SatAll);
            BinSerialize.WriteByte(ref buffer,(byte)SatGps);
            BinSerialize.WriteByte(ref buffer,(byte)SatGlo);
            BinSerialize.WriteByte(ref buffer,(byte)SatBdu);
            BinSerialize.WriteByte(ref buffer,(byte)SatGal);
            BinSerialize.WriteByte(ref buffer,(byte)SatQzs);
            BinSerialize.WriteByte(ref buffer,(byte)SatIme);
            BinSerialize.WriteByte(ref buffer,(byte)SatSbs);
            /* PayloadByteSize = 26 */;
        }
        
        



        /// <summary>
        /// Latitude of GBS (value / 10000000D).
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Longitude of GBS (value / 10000000D).
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public int Lng { get; set; }
        /// <summary>
        /// Altitude of GBS.
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public int Alt { get; set; }
        /// <summary>
        /// Current position accuracy (cm).
        /// OriginName: accuracy, Units: cm, IsExtended: false
        /// </summary>
        public ushort Accuracy { get; set; }
        /// <summary>
        /// Observation time (seconds).
        /// OriginName: observation, Units: s, IsExtended: false
        /// </summary>
        public ushort Observation { get; set; }
        /// <summary>
        /// Rate of GPS_RTCM_DATA data.
        /// OriginName: dgps_rate, Units: bytes\seconds, IsExtended: false
        /// </summary>
        public ushort DgpsRate { get; set; }
        /// <summary>
        /// All GNSS satellite count.
        /// OriginName: sat_all, Units: , IsExtended: false
        /// </summary>
        public byte SatAll { get; set; }
        /// <summary>
        /// GPS satellite count.
        /// OriginName: sat_gps, Units: , IsExtended: false
        /// </summary>
        public byte SatGps { get; set; }
        /// <summary>
        /// GLONASS satellite count.
        /// OriginName: sat_glo, Units: , IsExtended: false
        /// </summary>
        public byte SatGlo { get; set; }
        /// <summary>
        /// BeiDou satellite count.
        /// OriginName: sat_bdu, Units: , IsExtended: false
        /// </summary>
        public byte SatBdu { get; set; }
        /// <summary>
        /// Galileo satellite count.
        /// OriginName: sat_gal, Units: , IsExtended: false
        /// </summary>
        public byte SatGal { get; set; }
        /// <summary>
        /// QZSS satellite count.
        /// OriginName: sat_qzs, Units: , IsExtended: false
        /// </summary>
        public byte SatQzs { get; set; }
        /// <summary>
        /// IMES satellite count.
        /// OriginName: sat_ime, Units: , IsExtended: false
        /// </summary>
        public byte SatIme { get; set; }
        /// <summary>
        /// SBAS satellite count.
        /// OriginName: sat_sbs, Units: , IsExtended: false
        /// </summary>
        public byte SatSbs { get; set; }
    }


#endregion


}

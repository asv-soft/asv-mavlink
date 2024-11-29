// MIT License
//
// Copyright (c) 2024 asv-soft (https://github.com/asv-soft)
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

// This code was generate by tool Asv.Mavlink.Shell version 3.10.4+1a2d7cd3ae509bbfa5f932af5791dfe12de59ff1

using System;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.IO;

namespace Asv.Mavlink.AsvGbs
{

    public static class AsvGbsHelper
    {
        public static void RegisterAsvGbsDialect(this ImmutableDictionary<ushort,Func<MavlinkMessage>>.Builder src)
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
    public class AsvGbsOutStatusPacket: MavlinkV2Message<AsvGbsOutStatusPayload>
    {
        public const int MessageId = 13000;
        
        public const byte CrcExtra = 216;
        
        public override ushort Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvGbsOutStatusPayload Payload { get; } = new();

        public override string Name => "ASV_GBS_OUT_STATUS";
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Lat
            sum+=4; //Lng
            sum+=4; //Alt
            sum+=2; //Accuracy
            sum+=2; //Observation
            sum+=2; //DgpsRate
            sum+=1; //SatAll
            sum+=1; //SatGps
            sum+=1; //SatGlo
            sum+=1; //SatBdu
            sum+=1; //SatGal
            sum+=1; //SatQzs
            sum+=1; //SatIme
            sum+=1; //SatSbs
            return (byte)sum;
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
        /// Current position accuracy (mm).
        /// OriginName: accuracy, Units: mm, IsExtended: false
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

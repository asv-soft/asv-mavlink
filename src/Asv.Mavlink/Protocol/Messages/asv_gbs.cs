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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.11+05423b76b208fe780abe1cef9f7beeacb19cba77 25-07-03.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.AsvAudio;
using System.Linq;
using System.Collections.Generic;
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
    public enum MavType : ulong
    {
        /// <summary>
        /// Used to identify RTK ground base station in HEARTBEAT packet.
        /// MAV_TYPE_ASV_GBS
        /// </summary>
        MavTypeAsvGbs = 250,
    }
    public static class MavTypeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(250);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(250),"MAV_TYPE_ASV_GBS");
        }
    }
    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd : ulong
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
    public static class MavCmdHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(13001);
            yield return converter(13002);
            yield return converter(13003);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(13001),"MAV_CMD_ASV_GBS_RUN_AUTO_MODE");
            yield return new EnumValue<T>(converter(13002),"MAV_CMD_ASV_GBS_RUN_FIXED_MODE");
            yield return new EnumValue<T>(converter(13003),"MAV_CMD_ASV_GBS_RUN_IDLE_MODE");
        }
    }
    /// <summary>
    /// A mapping of GBS modes for custom_mode field of heartbeat.
    ///  ASV_GBS_CUSTOM_MODE
    /// </summary>
    public enum AsvGbsCustomMode : ulong
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
    public static class AsvGbsCustomModeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
            yield return converter(4);
            yield return converter(5);
            yield return converter(6);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_GBS_CUSTOM_MODE_LOADING");
            yield return new EnumValue<T>(converter(1),"ASV_GBS_CUSTOM_MODE_IDLE");
            yield return new EnumValue<T>(converter(2),"ASV_GBS_CUSTOM_MODE_ERROR");
            yield return new EnumValue<T>(converter(3),"ASV_GBS_CUSTOM_MODE_AUTO_IN_PROGRESS");
            yield return new EnumValue<T>(converter(4),"ASV_GBS_CUSTOM_MODE_AUTO");
            yield return new EnumValue<T>(converter(5),"ASV_GBS_CUSTOM_MODE_FIXED_IN_PROGRESS");
            yield return new EnumValue<T>(converter(6),"ASV_GBS_CUSTOM_MODE_FIXED");
        }
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

        public void Accept(IVisitor visitor)
        {
            Int32Type.Accept(visitor,LatField, LatField.DataType, ref _lat);    
            Int32Type.Accept(visitor,LngField, LngField.DataType, ref _lng);    
            Int32Type.Accept(visitor,AltField, AltField.DataType, ref _alt);    
            UInt16Type.Accept(visitor,AccuracyField, AccuracyField.DataType, ref _accuracy);    
            UInt16Type.Accept(visitor,ObservationField, ObservationField.DataType, ref _observation);    
            UInt16Type.Accept(visitor,DgpsRateField, DgpsRateField.DataType, ref _dgpsRate);    
            UInt8Type.Accept(visitor,SatAllField, SatAllField.DataType, ref _satAll);    
            UInt8Type.Accept(visitor,SatGpsField, SatGpsField.DataType, ref _satGps);    
            UInt8Type.Accept(visitor,SatGloField, SatGloField.DataType, ref _satGlo);    
            UInt8Type.Accept(visitor,SatBduField, SatBduField.DataType, ref _satBdu);    
            UInt8Type.Accept(visitor,SatGalField, SatGalField.DataType, ref _satGal);    
            UInt8Type.Accept(visitor,SatQzsField, SatQzsField.DataType, ref _satQzs);    
            UInt8Type.Accept(visitor,SatImeField, SatImeField.DataType, ref _satIme);    
            UInt8Type.Accept(visitor,SatSbsField, SatSbsField.DataType, ref _satSbs);    

        }

        /// <summary>
        /// Latitude of GBS (value / 10000000D).
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Latitude of GBS (value / 10000000D).")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _lat;
        public int Lat { get => _lat; set => _lat = value; }
        /// <summary>
        /// Longitude of GBS (value / 10000000D).
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LngField = new Field.Builder()
            .Name(nameof(Lng))
            .Title("lng")
            .Description("Longitude of GBS (value / 10000000D).")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _lng;
        public int Lng { get => _lng; set => _lng = value; }
        /// <summary>
        /// Altitude of GBS.
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltField = new Field.Builder()
            .Name(nameof(Alt))
            .Title("alt")
            .Description("Altitude of GBS.")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _alt;
        public int Alt { get => _alt; set => _alt = value; }
        /// <summary>
        /// Current position accuracy (cm).
        /// OriginName: accuracy, Units: cm, IsExtended: false
        /// </summary>
        public static readonly Field AccuracyField = new Field.Builder()
            .Name(nameof(Accuracy))
            .Title("accuracy")
            .Description("Current position accuracy (cm).")
.Units(@"cm")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _accuracy;
        public ushort Accuracy { get => _accuracy; set => _accuracy = value; }
        /// <summary>
        /// Observation time (seconds).
        /// OriginName: observation, Units: s, IsExtended: false
        /// </summary>
        public static readonly Field ObservationField = new Field.Builder()
            .Name(nameof(Observation))
            .Title("observation")
            .Description("Observation time (seconds).")
.Units(@"s")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _observation;
        public ushort Observation { get => _observation; set => _observation = value; }
        /// <summary>
        /// Rate of GPS_RTCM_DATA data.
        /// OriginName: dgps_rate, Units: bytes\seconds, IsExtended: false
        /// </summary>
        public static readonly Field DgpsRateField = new Field.Builder()
            .Name(nameof(DgpsRate))
            .Title("dgps_rate")
            .Description("Rate of GPS_RTCM_DATA data.")
.Units(@"bytes\seconds")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _dgpsRate;
        public ushort DgpsRate { get => _dgpsRate; set => _dgpsRate = value; }
        /// <summary>
        /// All GNSS satellite count.
        /// OriginName: sat_all, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SatAllField = new Field.Builder()
            .Name(nameof(SatAll))
            .Title("sat_all")
            .Description("All GNSS satellite count.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _satAll;
        public byte SatAll { get => _satAll; set => _satAll = value; }
        /// <summary>
        /// GPS satellite count.
        /// OriginName: sat_gps, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SatGpsField = new Field.Builder()
            .Name(nameof(SatGps))
            .Title("sat_gps")
            .Description("GPS satellite count.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _satGps;
        public byte SatGps { get => _satGps; set => _satGps = value; }
        /// <summary>
        /// GLONASS satellite count.
        /// OriginName: sat_glo, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SatGloField = new Field.Builder()
            .Name(nameof(SatGlo))
            .Title("sat_glo")
            .Description("GLONASS satellite count.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _satGlo;
        public byte SatGlo { get => _satGlo; set => _satGlo = value; }
        /// <summary>
        /// BeiDou satellite count.
        /// OriginName: sat_bdu, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SatBduField = new Field.Builder()
            .Name(nameof(SatBdu))
            .Title("sat_bdu")
            .Description("BeiDou satellite count.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _satBdu;
        public byte SatBdu { get => _satBdu; set => _satBdu = value; }
        /// <summary>
        /// Galileo satellite count.
        /// OriginName: sat_gal, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SatGalField = new Field.Builder()
            .Name(nameof(SatGal))
            .Title("sat_gal")
            .Description("Galileo satellite count.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _satGal;
        public byte SatGal { get => _satGal; set => _satGal = value; }
        /// <summary>
        /// QZSS satellite count.
        /// OriginName: sat_qzs, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SatQzsField = new Field.Builder()
            .Name(nameof(SatQzs))
            .Title("sat_qzs")
            .Description("QZSS satellite count.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _satQzs;
        public byte SatQzs { get => _satQzs; set => _satQzs = value; }
        /// <summary>
        /// IMES satellite count.
        /// OriginName: sat_ime, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SatImeField = new Field.Builder()
            .Name(nameof(SatIme))
            .Title("sat_ime")
            .Description("IMES satellite count.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _satIme;
        public byte SatIme { get => _satIme; set => _satIme = value; }
        /// <summary>
        /// SBAS satellite count.
        /// OriginName: sat_sbs, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SatSbsField = new Field.Builder()
            .Name(nameof(SatSbs))
            .Title("sat_sbs")
            .Description("SBAS satellite count.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _satSbs;
        public byte SatSbs { get => _satSbs; set => _satSbs = value; }
    }




        


#endregion


}

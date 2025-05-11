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

namespace Asv.Mavlink.Uavionix
{

    public static class UavionixHelper
    {
        public static void RegisterUavionixDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(UavionixAdsbOutCfgPacket.MessageId, ()=>new UavionixAdsbOutCfgPacket());
            src.Add(UavionixAdsbOutDynamicPacket.MessageId, ()=>new UavionixAdsbOutDynamicPacket());
            src.Add(UavionixAdsbTransceiverHealthReportPacket.MessageId, ()=>new UavionixAdsbTransceiverHealthReportPacket());
        }
    }

#region Enums

    /// <summary>
    /// State flags for ADS-B transponder dynamic report
    ///  UAVIONIX_ADSB_OUT_DYNAMIC_STATE
    /// </summary>
    public enum UavionixAdsbOutDynamicState:uint
    {
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_STATE_INTENT_CHANGE
        /// </summary>
        UavionixAdsbOutDynamicStateIntentChange = 1,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_STATE_AUTOPILOT_ENABLED
        /// </summary>
        UavionixAdsbOutDynamicStateAutopilotEnabled = 2,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_STATE_NICBARO_CROSSCHECKED
        /// </summary>
        UavionixAdsbOutDynamicStateNicbaroCrosschecked = 4,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_STATE_ON_GROUND
        /// </summary>
        UavionixAdsbOutDynamicStateOnGround = 8,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_STATE_IDENT
        /// </summary>
        UavionixAdsbOutDynamicStateIdent = 16,
    }

    /// <summary>
    /// Transceiver RF control flags for ADS-B transponder dynamic reports
    ///  UAVIONIX_ADSB_OUT_RF_SELECT
    /// </summary>
    public enum UavionixAdsbOutRfSelect:uint
    {
        /// <summary>
        /// UAVIONIX_ADSB_OUT_RF_SELECT_STANDBY
        /// </summary>
        UavionixAdsbOutRfSelectStandby = 0,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_RF_SELECT_RX_ENABLED
        /// </summary>
        UavionixAdsbOutRfSelectRxEnabled = 1,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_RF_SELECT_TX_ENABLED
        /// </summary>
        UavionixAdsbOutRfSelectTxEnabled = 2,
    }

    /// <summary>
    /// Status for ADS-B transponder dynamic input
    ///  UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX
    /// </summary>
    public enum UavionixAdsbOutDynamicGpsFix:uint
    {
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_NONE_0
        /// </summary>
        UavionixAdsbOutDynamicGpsFixNone0 = 0,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_NONE_1
        /// </summary>
        UavionixAdsbOutDynamicGpsFixNone1 = 1,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_2D
        /// </summary>
        UavionixAdsbOutDynamicGpsFix2d = 2,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_3D
        /// </summary>
        UavionixAdsbOutDynamicGpsFix3d = 3,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_DGPS
        /// </summary>
        UavionixAdsbOutDynamicGpsFixDgps = 4,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_RTK
        /// </summary>
        UavionixAdsbOutDynamicGpsFixRtk = 5,
    }

    /// <summary>
    /// Status flags for ADS-B transponder dynamic output
    ///  UAVIONIX_ADSB_RF_HEALTH
    /// </summary>
    public enum UavionixAdsbRfHealth:uint
    {
        /// <summary>
        /// UAVIONIX_ADSB_RF_HEALTH_INITIALIZING
        /// </summary>
        UavionixAdsbRfHealthInitializing = 0,
        /// <summary>
        /// UAVIONIX_ADSB_RF_HEALTH_OK
        /// </summary>
        UavionixAdsbRfHealthOk = 1,
        /// <summary>
        /// UAVIONIX_ADSB_RF_HEALTH_FAIL_TX
        /// </summary>
        UavionixAdsbRfHealthFailTx = 2,
        /// <summary>
        /// UAVIONIX_ADSB_RF_HEALTH_FAIL_RX
        /// </summary>
        UavionixAdsbRfHealthFailRx = 16,
    }

    /// <summary>
    /// Definitions for aircraft size
    ///  UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE
    /// </summary>
    public enum UavionixAdsbOutCfgAircraftSize:uint
    {
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_NO_DATA
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeNoData = 0,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L15M_W23M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL15mW23m = 1,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L25M_W28P5M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL25mW28p5m = 2,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L25_34M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL2534m = 3,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L35_33M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL3533m = 4,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L35_38M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL3538m = 5,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L45_39P5M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL4539p5m = 6,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L45_45M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL4545m = 7,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L55_45M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL5545m = 8,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L55_52M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL5552m = 9,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L65_59P5M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL6559p5m = 10,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L65_67M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL6567m = 11,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L75_W72P5M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL75W72p5m = 12,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L75_W80M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL75W80m = 13,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L85_W80M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL85W80m = 14,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L85_W90M
        /// </summary>
        UavionixAdsbOutCfgAircraftSizeL85W90m = 15,
    }

    /// <summary>
    /// GPS lataral offset encoding
    ///  UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT
    /// </summary>
    public enum UavionixAdsbOutCfgGpsOffsetLat:uint
    {
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_NO_DATA
        /// </summary>
        UavionixAdsbOutCfgGpsOffsetLatNoData = 0,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_LEFT_2M
        /// </summary>
        UavionixAdsbOutCfgGpsOffsetLatLeft2m = 1,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_LEFT_4M
        /// </summary>
        UavionixAdsbOutCfgGpsOffsetLatLeft4m = 2,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_LEFT_6M
        /// </summary>
        UavionixAdsbOutCfgGpsOffsetLatLeft6m = 3,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_RIGHT_0M
        /// </summary>
        UavionixAdsbOutCfgGpsOffsetLatRight0m = 4,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_RIGHT_2M
        /// </summary>
        UavionixAdsbOutCfgGpsOffsetLatRight2m = 5,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_RIGHT_4M
        /// </summary>
        UavionixAdsbOutCfgGpsOffsetLatRight4m = 6,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_RIGHT_6M
        /// </summary>
        UavionixAdsbOutCfgGpsOffsetLatRight6m = 7,
    }

    /// <summary>
    /// GPS longitudinal offset encoding
    ///  UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LON
    /// </summary>
    public enum UavionixAdsbOutCfgGpsOffsetLon:uint
    {
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LON_NO_DATA
        /// </summary>
        UavionixAdsbOutCfgGpsOffsetLonNoData = 0,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LON_APPLIED_BY_SENSOR
        /// </summary>
        UavionixAdsbOutCfgGpsOffsetLonAppliedBySensor = 1,
    }

    /// <summary>
    /// Emergency status encoding
    ///  UAVIONIX_ADSB_EMERGENCY_STATUS
    /// </summary>
    public enum UavionixAdsbEmergencyStatus:uint
    {
        /// <summary>
        /// UAVIONIX_ADSB_OUT_NO_EMERGENCY
        /// </summary>
        UavionixAdsbOutNoEmergency = 0,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_GENERAL_EMERGENCY
        /// </summary>
        UavionixAdsbOutGeneralEmergency = 1,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_LIFEGUARD_EMERGENCY
        /// </summary>
        UavionixAdsbOutLifeguardEmergency = 2,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_MINIMUM_FUEL_EMERGENCY
        /// </summary>
        UavionixAdsbOutMinimumFuelEmergency = 3,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_NO_COMM_EMERGENCY
        /// </summary>
        UavionixAdsbOutNoCommEmergency = 4,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_UNLAWFUL_INTERFERANCE_EMERGENCY
        /// </summary>
        UavionixAdsbOutUnlawfulInterferanceEmergency = 5,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_DOWNED_AIRCRAFT_EMERGENCY
        /// </summary>
        UavionixAdsbOutDownedAircraftEmergency = 6,
        /// <summary>
        /// UAVIONIX_ADSB_OUT_RESERVED
        /// </summary>
        UavionixAdsbOutReserved = 7,
    }


#endregion

#region Messages

    /// <summary>
    /// Static data to configure the ADS-B transponder (send within 10 sec of a POR and every 10 sec thereafter)
    ///  UAVIONIX_ADSB_OUT_CFG
    /// </summary>
    public class UavionixAdsbOutCfgPacket : MavlinkV2Message<UavionixAdsbOutCfgPayload>
    {
        public const int MessageId = 10001;
        
        public const byte CrcExtra = 209;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override UavionixAdsbOutCfgPayload Payload { get; } = new();

        public override string Name => "UAVIONIX_ADSB_OUT_CFG";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "ICAO",
            "Vehicle address (24 bit)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "stallSpeed",
            "Aircraft stall speed in cm/s",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "callsign",
            "Vehicle identifier (8 characters, null terminated, valid characters are A-Z, 0-9, \" \" only)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            9, 
            false),
            new(3,
            "emitterType",
            "Transmitting vehicle type. See ADSB_EMITTER_TYPE enum",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "aircraftSize",
            "Aircraft length and width encoding (table 2-35 of DO-282B)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(5,
            "gpsOffsetLat",
            "GPS antenna lateral offset (table 2-36 of DO-282B)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(6,
            "gpsOffsetLon",
            "GPS antenna longitudinal offset from nose [if non-zero, take position (in meters) divide by 2 and add one] (table 2-37 DO-282B)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(7,
            "rfSelect",
            "ADS-B transponder receiver and transmit enable flags",
            string.Empty, 
            string.Empty, 
            "bitmask", 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "UAVIONIX_ADSB_OUT_CFG:"
        + "uint32_t ICAO;"
        + "uint16_t stallSpeed;"
        + "char[9] callsign;"
        + "uint8_t emitterType;"
        + "uint8_t aircraftSize;"
        + "uint8_t gpsOffsetLat;"
        + "uint8_t gpsOffsetLon;"
        + "uint8_t rfSelect;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Icao);
            writer.Write(StaticFields[1], Payload.Stallspeed);
            writer.Write(StaticFields[2], Payload.Callsign);
            writer.Write(StaticFields[3], Payload.Emittertype);
            writer.Write(StaticFields[4], Payload.Aircraftsize);
            writer.Write(StaticFields[5], Payload.Gpsoffsetlat);
            writer.Write(StaticFields[6], Payload.Gpsoffsetlon);
            writer.Write(StaticFields[7], Payload.Rfselect);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Icao = reader.ReadUInt(StaticFields[0]);
            Payload.Stallspeed = reader.ReadUShort(StaticFields[1]);
            reader.ReadCharArray(StaticFields[2], Payload.Callsign);
            Payload.Emittertype = (AdsbEmitterType)reader.ReadEnum(StaticFields[3]);
            Payload.Aircraftsize = (UavionixAdsbOutCfgAircraftSize)reader.ReadEnum(StaticFields[4]);
            Payload.Gpsoffsetlat = (UavionixAdsbOutCfgGpsOffsetLat)reader.ReadEnum(StaticFields[5]);
            Payload.Gpsoffsetlon = (UavionixAdsbOutCfgGpsOffsetLon)reader.ReadEnum(StaticFields[6]);
            Payload.Rfselect = (UavionixAdsbOutRfSelect)reader.ReadEnum(StaticFields[7]);
        
            
        }
    }

    /// <summary>
    ///  UAVIONIX_ADSB_OUT_CFG
    /// </summary>
    public class UavionixAdsbOutCfgPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t ICAO
            +2 // uint16_t stallSpeed
            +Callsign.Length // char[9] callsign
            + 1 // uint8_t emitterType
            + 1 // uint8_t aircraftSize
            + 1 // uint8_t gpsOffsetLat
            + 1 // uint8_t gpsOffsetLon
            + 1 // uint8_t rfSelect
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Icao = BinSerialize.ReadUInt(ref buffer);
            Stallspeed = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/9 - Math.Max(0,((/*PayloadByteSize*/20 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Callsign = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Callsign)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Callsign.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
            Emittertype = (AdsbEmitterType)BinSerialize.ReadByte(ref buffer);
            Aircraftsize = (UavionixAdsbOutCfgAircraftSize)BinSerialize.ReadByte(ref buffer);
            Gpsoffsetlat = (UavionixAdsbOutCfgGpsOffsetLat)BinSerialize.ReadByte(ref buffer);
            Gpsoffsetlon = (UavionixAdsbOutCfgGpsOffsetLon)BinSerialize.ReadByte(ref buffer);
            Rfselect = (UavionixAdsbOutRfSelect)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,Icao);
            BinSerialize.WriteUShort(ref buffer,Stallspeed);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Callsign)
                {
                    Encoding.ASCII.GetBytes(charPointer, Callsign.Length, bytePointer, Callsign.Length);
                }
            }
            buffer = buffer.Slice(Callsign.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)Emittertype);
            BinSerialize.WriteByte(ref buffer,(byte)Aircraftsize);
            BinSerialize.WriteByte(ref buffer,(byte)Gpsoffsetlat);
            BinSerialize.WriteByte(ref buffer,(byte)Gpsoffsetlon);
            BinSerialize.WriteByte(ref buffer,(byte)Rfselect);
            /* PayloadByteSize = 20 */;
        }
        
        



        /// <summary>
        /// Vehicle address (24 bit)
        /// OriginName: ICAO, Units: , IsExtended: false
        /// </summary>
        public uint Icao { get; set; }
        /// <summary>
        /// Aircraft stall speed in cm/s
        /// OriginName: stallSpeed, Units: cm/s, IsExtended: false
        /// </summary>
        public ushort Stallspeed { get; set; }
        /// <summary>
        /// Vehicle identifier (8 characters, null terminated, valid characters are A-Z, 0-9, " " only)
        /// OriginName: callsign, Units: , IsExtended: false
        /// </summary>
        public const int CallsignMaxItemsCount = 9;
        public char[] Callsign { get; set; } = new char[9];
        [Obsolete("This method is deprecated. Use GetCallsignMaxItemsCount instead.")]
        public byte GetCallsignMaxItemsCount() => 9;
        /// <summary>
        /// Transmitting vehicle type. See ADSB_EMITTER_TYPE enum
        /// OriginName: emitterType, Units: , IsExtended: false
        /// </summary>
        public AdsbEmitterType Emittertype { get; set; }
        /// <summary>
        /// Aircraft length and width encoding (table 2-35 of DO-282B)
        /// OriginName: aircraftSize, Units: , IsExtended: false
        /// </summary>
        public UavionixAdsbOutCfgAircraftSize Aircraftsize { get; set; }
        /// <summary>
        /// GPS antenna lateral offset (table 2-36 of DO-282B)
        /// OriginName: gpsOffsetLat, Units: , IsExtended: false
        /// </summary>
        public UavionixAdsbOutCfgGpsOffsetLat Gpsoffsetlat { get; set; }
        /// <summary>
        /// GPS antenna longitudinal offset from nose [if non-zero, take position (in meters) divide by 2 and add one] (table 2-37 DO-282B)
        /// OriginName: gpsOffsetLon, Units: , IsExtended: false
        /// </summary>
        public UavionixAdsbOutCfgGpsOffsetLon Gpsoffsetlon { get; set; }
        /// <summary>
        /// ADS-B transponder receiver and transmit enable flags
        /// OriginName: rfSelect, Units: , IsExtended: false
        /// </summary>
        public UavionixAdsbOutRfSelect Rfselect { get; set; }
    }
    /// <summary>
    /// Dynamic data used to generate ADS-B out transponder data (send at 5Hz)
    ///  UAVIONIX_ADSB_OUT_DYNAMIC
    /// </summary>
    public class UavionixAdsbOutDynamicPacket : MavlinkV2Message<UavionixAdsbOutDynamicPayload>
    {
        public const int MessageId = 10002;
        
        public const byte CrcExtra = 186;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override UavionixAdsbOutDynamicPayload Payload { get; } = new();

        public override string Name => "UAVIONIX_ADSB_OUT_DYNAMIC";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "utcTime",
            "UTC time in seconds since GPS epoch (Jan 6, 1980). If unknown set to UINT32_MAX",
            string.Empty, 
            @"s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "gpsLat",
            "Latitude WGS84 (deg * 1E7). If unknown set to INT32_MAX",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(2,
            "gpsLon",
            "Longitude WGS84 (deg * 1E7). If unknown set to INT32_MAX",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(3,
            "gpsAlt",
            "Altitude (WGS84). UP +ve. If unknown set to INT32_MAX",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(4,
            "baroAltMSL",
            "Barometric pressure altitude (MSL) relative to a standard atmosphere of 1013.2 mBar and NOT bar corrected altitude (m * 1E-3). (up +ve). If unknown set to INT32_MAX",
            string.Empty, 
            @"mbar", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(5,
            "accuracyHor",
            "Horizontal accuracy in mm (m * 1E-3). If unknown set to UINT32_MAX",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(6,
            "accuracyVert",
            "Vertical accuracy in cm. If unknown set to UINT16_MAX",
            string.Empty, 
            @"cm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(7,
            "accuracyVel",
            "Velocity accuracy in mm/s (m * 1E-3). If unknown set to UINT16_MAX",
            string.Empty, 
            @"mm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(8,
            "velVert",
            "GPS vertical speed in cm/s. If unknown set to INT16_MAX",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(9,
            "velNS",
            "North-South velocity over ground in cm/s North +ve. If unknown set to INT16_MAX",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(10,
            "VelEW",
            "East-West velocity over ground in cm/s East +ve. If unknown set to INT16_MAX",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(11,
            "state",
            "ADS-B transponder dynamic input state flags",
            string.Empty, 
            string.Empty, 
            "bitmask", 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(12,
            "squawk",
            "Mode A code (typically 1200 [0x04B0] for VFR)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(13,
            "gpsFix",
            "0-1: no fix, 2: 2D fix, 3: 3D fix, 4: DGPS, 5: RTK",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(14,
            "numSats",
            "Number of satellites visible. If unknown set to UINT8_MAX",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(15,
            "emergencyStatus",
            "Emergency status",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "UAVIONIX_ADSB_OUT_DYNAMIC:"
        + "uint32_t utcTime;"
        + "int32_t gpsLat;"
        + "int32_t gpsLon;"
        + "int32_t gpsAlt;"
        + "int32_t baroAltMSL;"
        + "uint32_t accuracyHor;"
        + "uint16_t accuracyVert;"
        + "uint16_t accuracyVel;"
        + "int16_t velVert;"
        + "int16_t velNS;"
        + "int16_t VelEW;"
        + "uint16_t state;"
        + "uint16_t squawk;"
        + "uint8_t gpsFix;"
        + "uint8_t numSats;"
        + "uint8_t emergencyStatus;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Utctime);
            writer.Write(StaticFields[1], Payload.Gpslat);
            writer.Write(StaticFields[2], Payload.Gpslon);
            writer.Write(StaticFields[3], Payload.Gpsalt);
            writer.Write(StaticFields[4], Payload.Baroaltmsl);
            writer.Write(StaticFields[5], Payload.Accuracyhor);
            writer.Write(StaticFields[6], Payload.Accuracyvert);
            writer.Write(StaticFields[7], Payload.Accuracyvel);
            writer.Write(StaticFields[8], Payload.Velvert);
            writer.Write(StaticFields[9], Payload.Velns);
            writer.Write(StaticFields[10], Payload.Velew);
            writer.Write(StaticFields[11], Payload.State);
            writer.Write(StaticFields[12], Payload.Squawk);
            writer.Write(StaticFields[13], Payload.Gpsfix);
            writer.Write(StaticFields[14], Payload.Numsats);
            writer.Write(StaticFields[15], Payload.Emergencystatus);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Utctime = reader.ReadUInt(StaticFields[0]);
            Payload.Gpslat = reader.ReadInt(StaticFields[1]);
            Payload.Gpslon = reader.ReadInt(StaticFields[2]);
            Payload.Gpsalt = reader.ReadInt(StaticFields[3]);
            Payload.Baroaltmsl = reader.ReadInt(StaticFields[4]);
            Payload.Accuracyhor = reader.ReadUInt(StaticFields[5]);
            Payload.Accuracyvert = reader.ReadUShort(StaticFields[6]);
            Payload.Accuracyvel = reader.ReadUShort(StaticFields[7]);
            Payload.Velvert = reader.ReadShort(StaticFields[8]);
            Payload.Velns = reader.ReadShort(StaticFields[9]);
            Payload.Velew = reader.ReadShort(StaticFields[10]);
            Payload.State = (UavionixAdsbOutDynamicState)reader.ReadEnum(StaticFields[11]);
            Payload.Squawk = reader.ReadUShort(StaticFields[12]);
            Payload.Gpsfix = (UavionixAdsbOutDynamicGpsFix)reader.ReadEnum(StaticFields[13]);
            Payload.Numsats = reader.ReadByte(StaticFields[14]);
            Payload.Emergencystatus = (UavionixAdsbEmergencyStatus)reader.ReadEnum(StaticFields[15]);
        
            
        }
    }

    /// <summary>
    ///  UAVIONIX_ADSB_OUT_DYNAMIC
    /// </summary>
    public class UavionixAdsbOutDynamicPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 41; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 41; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t utcTime
            +4 // int32_t gpsLat
            +4 // int32_t gpsLon
            +4 // int32_t gpsAlt
            +4 // int32_t baroAltMSL
            +4 // uint32_t accuracyHor
            +2 // uint16_t accuracyVert
            +2 // uint16_t accuracyVel
            +2 // int16_t velVert
            +2 // int16_t velNS
            +2 // int16_t VelEW
            + 2 // uint16_t state
            +2 // uint16_t squawk
            + 1 // uint8_t gpsFix
            +1 // uint8_t numSats
            + 1 // uint8_t emergencyStatus
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Utctime = BinSerialize.ReadUInt(ref buffer);
            Gpslat = BinSerialize.ReadInt(ref buffer);
            Gpslon = BinSerialize.ReadInt(ref buffer);
            Gpsalt = BinSerialize.ReadInt(ref buffer);
            Baroaltmsl = BinSerialize.ReadInt(ref buffer);
            Accuracyhor = BinSerialize.ReadUInt(ref buffer);
            Accuracyvert = BinSerialize.ReadUShort(ref buffer);
            Accuracyvel = BinSerialize.ReadUShort(ref buffer);
            Velvert = BinSerialize.ReadShort(ref buffer);
            Velns = BinSerialize.ReadShort(ref buffer);
            Velew = BinSerialize.ReadShort(ref buffer);
            State = (UavionixAdsbOutDynamicState)BinSerialize.ReadUShort(ref buffer);
            Squawk = BinSerialize.ReadUShort(ref buffer);
            Gpsfix = (UavionixAdsbOutDynamicGpsFix)BinSerialize.ReadByte(ref buffer);
            Numsats = (byte)BinSerialize.ReadByte(ref buffer);
            Emergencystatus = (UavionixAdsbEmergencyStatus)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,Utctime);
            BinSerialize.WriteInt(ref buffer,Gpslat);
            BinSerialize.WriteInt(ref buffer,Gpslon);
            BinSerialize.WriteInt(ref buffer,Gpsalt);
            BinSerialize.WriteInt(ref buffer,Baroaltmsl);
            BinSerialize.WriteUInt(ref buffer,Accuracyhor);
            BinSerialize.WriteUShort(ref buffer,Accuracyvert);
            BinSerialize.WriteUShort(ref buffer,Accuracyvel);
            BinSerialize.WriteShort(ref buffer,Velvert);
            BinSerialize.WriteShort(ref buffer,Velns);
            BinSerialize.WriteShort(ref buffer,Velew);
            BinSerialize.WriteUShort(ref buffer,(ushort)State);
            BinSerialize.WriteUShort(ref buffer,Squawk);
            BinSerialize.WriteByte(ref buffer,(byte)Gpsfix);
            BinSerialize.WriteByte(ref buffer,(byte)Numsats);
            BinSerialize.WriteByte(ref buffer,(byte)Emergencystatus);
            /* PayloadByteSize = 41 */;
        }
        
        



        /// <summary>
        /// UTC time in seconds since GPS epoch (Jan 6, 1980). If unknown set to UINT32_MAX
        /// OriginName: utcTime, Units: s, IsExtended: false
        /// </summary>
        public uint Utctime { get; set; }
        /// <summary>
        /// Latitude WGS84 (deg * 1E7). If unknown set to INT32_MAX
        /// OriginName: gpsLat, Units: degE7, IsExtended: false
        /// </summary>
        public int Gpslat { get; set; }
        /// <summary>
        /// Longitude WGS84 (deg * 1E7). If unknown set to INT32_MAX
        /// OriginName: gpsLon, Units: degE7, IsExtended: false
        /// </summary>
        public int Gpslon { get; set; }
        /// <summary>
        /// Altitude (WGS84). UP +ve. If unknown set to INT32_MAX
        /// OriginName: gpsAlt, Units: mm, IsExtended: false
        /// </summary>
        public int Gpsalt { get; set; }
        /// <summary>
        /// Barometric pressure altitude (MSL) relative to a standard atmosphere of 1013.2 mBar and NOT bar corrected altitude (m * 1E-3). (up +ve). If unknown set to INT32_MAX
        /// OriginName: baroAltMSL, Units: mbar, IsExtended: false
        /// </summary>
        public int Baroaltmsl { get; set; }
        /// <summary>
        /// Horizontal accuracy in mm (m * 1E-3). If unknown set to UINT32_MAX
        /// OriginName: accuracyHor, Units: mm, IsExtended: false
        /// </summary>
        public uint Accuracyhor { get; set; }
        /// <summary>
        /// Vertical accuracy in cm. If unknown set to UINT16_MAX
        /// OriginName: accuracyVert, Units: cm, IsExtended: false
        /// </summary>
        public ushort Accuracyvert { get; set; }
        /// <summary>
        /// Velocity accuracy in mm/s (m * 1E-3). If unknown set to UINT16_MAX
        /// OriginName: accuracyVel, Units: mm/s, IsExtended: false
        /// </summary>
        public ushort Accuracyvel { get; set; }
        /// <summary>
        /// GPS vertical speed in cm/s. If unknown set to INT16_MAX
        /// OriginName: velVert, Units: cm/s, IsExtended: false
        /// </summary>
        public short Velvert { get; set; }
        /// <summary>
        /// North-South velocity over ground in cm/s North +ve. If unknown set to INT16_MAX
        /// OriginName: velNS, Units: cm/s, IsExtended: false
        /// </summary>
        public short Velns { get; set; }
        /// <summary>
        /// East-West velocity over ground in cm/s East +ve. If unknown set to INT16_MAX
        /// OriginName: VelEW, Units: cm/s, IsExtended: false
        /// </summary>
        public short Velew { get; set; }
        /// <summary>
        /// ADS-B transponder dynamic input state flags
        /// OriginName: state, Units: , IsExtended: false
        /// </summary>
        public UavionixAdsbOutDynamicState State { get; set; }
        /// <summary>
        /// Mode A code (typically 1200 [0x04B0] for VFR)
        /// OriginName: squawk, Units: , IsExtended: false
        /// </summary>
        public ushort Squawk { get; set; }
        /// <summary>
        /// 0-1: no fix, 2: 2D fix, 3: 3D fix, 4: DGPS, 5: RTK
        /// OriginName: gpsFix, Units: , IsExtended: false
        /// </summary>
        public UavionixAdsbOutDynamicGpsFix Gpsfix { get; set; }
        /// <summary>
        /// Number of satellites visible. If unknown set to UINT8_MAX
        /// OriginName: numSats, Units: , IsExtended: false
        /// </summary>
        public byte Numsats { get; set; }
        /// <summary>
        /// Emergency status
        /// OriginName: emergencyStatus, Units: , IsExtended: false
        /// </summary>
        public UavionixAdsbEmergencyStatus Emergencystatus { get; set; }
    }
    /// <summary>
    /// Transceiver heartbeat with health report (updated every 10s)
    ///  UAVIONIX_ADSB_TRANSCEIVER_HEALTH_REPORT
    /// </summary>
    public class UavionixAdsbTransceiverHealthReportPacket : MavlinkV2Message<UavionixAdsbTransceiverHealthReportPayload>
    {
        public const int MessageId = 10003;
        
        public const byte CrcExtra = 4;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override UavionixAdsbTransceiverHealthReportPayload Payload { get; } = new();

        public override string Name => "UAVIONIX_ADSB_TRANSCEIVER_HEALTH_REPORT";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "rfHealth",
            "ADS-B transponder messages",
            string.Empty, 
            string.Empty, 
            "bitmask", 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "UAVIONIX_ADSB_TRANSCEIVER_HEALTH_REPORT:"
        + "uint8_t rfHealth;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Rfhealth);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Rfhealth = (UavionixAdsbRfHealth)reader.ReadEnum(StaticFields[0]);
        
            
        }
    }

    /// <summary>
    ///  UAVIONIX_ADSB_TRANSCEIVER_HEALTH_REPORT
    /// </summary>
    public class UavionixAdsbTransceiverHealthReportPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 1; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 1; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 1 // uint8_t rfHealth
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Rfhealth = (UavionixAdsbRfHealth)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Rfhealth);
            /* PayloadByteSize = 1 */;
        }
        
        



        /// <summary>
        /// ADS-B transponder messages
        /// OriginName: rfHealth, Units: , IsExtended: false
        /// </summary>
        public UavionixAdsbRfHealth Rfhealth { get; set; }
    }


#endregion


}

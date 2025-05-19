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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.16+a43ef88c0eb6d4725d650c062779442ee3bd78f6 25-05-19.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.AsvAudio;
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
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Callsign)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Callsign.Length);
                }
            }
            buffer = buffer[arraySize..];
           
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

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,IcaoField, ref _Icao);    
            UInt16Type.Accept(visitor,StallspeedField, ref _Stallspeed);    
            ArrayType.Accept(visitor,CallsignField, 9, (index,v) =>
            {
                var tmp = (byte)Callsign[index];
                UInt8Type.Accept(v,CallsignField, ref tmp);
                Callsign[index] = (char)tmp;
            });
            var tmpEmittertype = (byte)Emittertype;
            UInt8Type.Accept(visitor,EmittertypeField, ref tmpEmittertype);
            Emittertype = (AdsbEmitterType)tmpEmittertype;
            var tmpAircraftsize = (byte)Aircraftsize;
            UInt8Type.Accept(visitor,AircraftsizeField, ref tmpAircraftsize);
            Aircraftsize = (UavionixAdsbOutCfgAircraftSize)tmpAircraftsize;
            var tmpGpsoffsetlat = (byte)Gpsoffsetlat;
            UInt8Type.Accept(visitor,GpsoffsetlatField, ref tmpGpsoffsetlat);
            Gpsoffsetlat = (UavionixAdsbOutCfgGpsOffsetLat)tmpGpsoffsetlat;
            var tmpGpsoffsetlon = (byte)Gpsoffsetlon;
            UInt8Type.Accept(visitor,GpsoffsetlonField, ref tmpGpsoffsetlon);
            Gpsoffsetlon = (UavionixAdsbOutCfgGpsOffsetLon)tmpGpsoffsetlon;
            var tmpRfselect = (byte)Rfselect;
            UInt8Type.Accept(visitor,RfselectField, ref tmpRfselect);
            Rfselect = (UavionixAdsbOutRfSelect)tmpRfselect;

        }

        /// <summary>
        /// Vehicle address (24 bit)
        /// OriginName: ICAO, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IcaoField = new Field.Builder()
            .Name(nameof(Icao))
            .Title("ICAO")
            .Description("Vehicle address (24 bit)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Icao;
        public uint Icao { get => _Icao; set { _Icao = value; } }
        /// <summary>
        /// Aircraft stall speed in cm/s
        /// OriginName: stallSpeed, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field StallspeedField = new Field.Builder()
            .Name(nameof(Stallspeed))
            .Title("stallSpeed")
            .Description("Aircraft stall speed in cm/s")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Stallspeed;
        public ushort Stallspeed { get => _Stallspeed; set { _Stallspeed = value; } }
        /// <summary>
        /// Vehicle identifier (8 characters, null terminated, valid characters are A-Z, 0-9, " " only)
        /// OriginName: callsign, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CallsignField = new Field.Builder()
            .Name(nameof(Callsign))
            .Title("callsign")
            .Description("Vehicle identifier (8 characters, null terminated, valid characters are A-Z, 0-9, \" \" only)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,9))

            .Build();
        public const int CallsignMaxItemsCount = 9;
        public char[] Callsign { get; } = new char[9];
        [Obsolete("This method is deprecated. Use GetCallsignMaxItemsCount instead.")]
        public byte GetCallsignMaxItemsCount() => 9;
        /// <summary>
        /// Transmitting vehicle type. See ADSB_EMITTER_TYPE enum
        /// OriginName: emitterType, Units: , IsExtended: false
        /// </summary>
        public static readonly Field EmittertypeField = new Field.Builder()
            .Name(nameof(Emittertype))
            .Title("emitterType")
            .Description("Transmitting vehicle type. See ADSB_EMITTER_TYPE enum")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AdsbEmitterType _Emittertype;
        public AdsbEmitterType Emittertype { get => _Emittertype; set => _Emittertype = value; } 
        /// <summary>
        /// Aircraft length and width encoding (table 2-35 of DO-282B)
        /// OriginName: aircraftSize, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AircraftsizeField = new Field.Builder()
            .Name(nameof(Aircraftsize))
            .Title("aircraftSize")
            .Description("Aircraft length and width encoding (table 2-35 of DO-282B)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public UavionixAdsbOutCfgAircraftSize _Aircraftsize;
        public UavionixAdsbOutCfgAircraftSize Aircraftsize { get => _Aircraftsize; set => _Aircraftsize = value; } 
        /// <summary>
        /// GPS antenna lateral offset (table 2-36 of DO-282B)
        /// OriginName: gpsOffsetLat, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GpsoffsetlatField = new Field.Builder()
            .Name(nameof(Gpsoffsetlat))
            .Title("gpsOffsetLat")
            .Description("GPS antenna lateral offset (table 2-36 of DO-282B)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public UavionixAdsbOutCfgGpsOffsetLat _Gpsoffsetlat;
        public UavionixAdsbOutCfgGpsOffsetLat Gpsoffsetlat { get => _Gpsoffsetlat; set => _Gpsoffsetlat = value; } 
        /// <summary>
        /// GPS antenna longitudinal offset from nose [if non-zero, take position (in meters) divide by 2 and add one] (table 2-37 DO-282B)
        /// OriginName: gpsOffsetLon, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GpsoffsetlonField = new Field.Builder()
            .Name(nameof(Gpsoffsetlon))
            .Title("gpsOffsetLon")
            .Description("GPS antenna longitudinal offset from nose [if non-zero, take position (in meters) divide by 2 and add one] (table 2-37 DO-282B)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public UavionixAdsbOutCfgGpsOffsetLon _Gpsoffsetlon;
        public UavionixAdsbOutCfgGpsOffsetLon Gpsoffsetlon { get => _Gpsoffsetlon; set => _Gpsoffsetlon = value; } 
        /// <summary>
        /// ADS-B transponder receiver and transmit enable flags
        /// OriginName: rfSelect, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RfselectField = new Field.Builder()
            .Name(nameof(Rfselect))
            .Title("bitmask")
            .Description("ADS-B transponder receiver and transmit enable flags")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public UavionixAdsbOutRfSelect _Rfselect;
        public UavionixAdsbOutRfSelect Rfselect { get => _Rfselect; set => _Rfselect = value; } 
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

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,UtctimeField, ref _Utctime);    
            Int32Type.Accept(visitor,GpslatField, ref _Gpslat);    
            Int32Type.Accept(visitor,GpslonField, ref _Gpslon);    
            Int32Type.Accept(visitor,GpsaltField, ref _Gpsalt);    
            Int32Type.Accept(visitor,BaroaltmslField, ref _Baroaltmsl);    
            UInt32Type.Accept(visitor,AccuracyhorField, ref _Accuracyhor);    
            UInt16Type.Accept(visitor,AccuracyvertField, ref _Accuracyvert);    
            UInt16Type.Accept(visitor,AccuracyvelField, ref _Accuracyvel);    
            Int16Type.Accept(visitor,VelvertField, ref _Velvert);
            Int16Type.Accept(visitor,VelnsField, ref _Velns);
            Int16Type.Accept(visitor,VelewField, ref _Velew);
            var tmpState = (ushort)State;
            UInt16Type.Accept(visitor,StateField, ref tmpState);
            State = (UavionixAdsbOutDynamicState)tmpState;
            UInt16Type.Accept(visitor,SquawkField, ref _Squawk);    
            var tmpGpsfix = (byte)Gpsfix;
            UInt8Type.Accept(visitor,GpsfixField, ref tmpGpsfix);
            Gpsfix = (UavionixAdsbOutDynamicGpsFix)tmpGpsfix;
            UInt8Type.Accept(visitor,NumsatsField, ref _Numsats);    
            var tmpEmergencystatus = (byte)Emergencystatus;
            UInt8Type.Accept(visitor,EmergencystatusField, ref tmpEmergencystatus);
            Emergencystatus = (UavionixAdsbEmergencyStatus)tmpEmergencystatus;

        }

        /// <summary>
        /// UTC time in seconds since GPS epoch (Jan 6, 1980). If unknown set to UINT32_MAX
        /// OriginName: utcTime, Units: s, IsExtended: false
        /// </summary>
        public static readonly Field UtctimeField = new Field.Builder()
            .Name(nameof(Utctime))
            .Title("utcTime")
            .Description("UTC time in seconds since GPS epoch (Jan 6, 1980). If unknown set to UINT32_MAX")
            .FormatString(string.Empty)
            .Units(@"s")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Utctime;
        public uint Utctime { get => _Utctime; set { _Utctime = value; } }
        /// <summary>
        /// Latitude WGS84 (deg * 1E7). If unknown set to INT32_MAX
        /// OriginName: gpsLat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GpslatField = new Field.Builder()
            .Name(nameof(Gpslat))
            .Title("gpsLat")
            .Description("Latitude WGS84 (deg * 1E7). If unknown set to INT32_MAX")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Gpslat;
        public int Gpslat { get => _Gpslat; set { _Gpslat = value; } }
        /// <summary>
        /// Longitude WGS84 (deg * 1E7). If unknown set to INT32_MAX
        /// OriginName: gpsLon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GpslonField = new Field.Builder()
            .Name(nameof(Gpslon))
            .Title("gpsLon")
            .Description("Longitude WGS84 (deg * 1E7). If unknown set to INT32_MAX")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Gpslon;
        public int Gpslon { get => _Gpslon; set { _Gpslon = value; } }
        /// <summary>
        /// Altitude (WGS84). UP +ve. If unknown set to INT32_MAX
        /// OriginName: gpsAlt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GpsaltField = new Field.Builder()
            .Name(nameof(Gpsalt))
            .Title("gpsAlt")
            .Description("Altitude (WGS84). UP +ve. If unknown set to INT32_MAX")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _Gpsalt;
        public int Gpsalt { get => _Gpsalt; set { _Gpsalt = value; } }
        /// <summary>
        /// Barometric pressure altitude (MSL) relative to a standard atmosphere of 1013.2 mBar and NOT bar corrected altitude (m * 1E-3). (up +ve). If unknown set to INT32_MAX
        /// OriginName: baroAltMSL, Units: mbar, IsExtended: false
        /// </summary>
        public static readonly Field BaroaltmslField = new Field.Builder()
            .Name(nameof(Baroaltmsl))
            .Title("baroAltMSL")
            .Description("Barometric pressure altitude (MSL) relative to a standard atmosphere of 1013.2 mBar and NOT bar corrected altitude (m * 1E-3). (up +ve). If unknown set to INT32_MAX")
            .FormatString(string.Empty)
            .Units(@"mbar")
            .DataType(Int32Type.Default)

            .Build();
        private int _Baroaltmsl;
        public int Baroaltmsl { get => _Baroaltmsl; set { _Baroaltmsl = value; } }
        /// <summary>
        /// Horizontal accuracy in mm (m * 1E-3). If unknown set to UINT32_MAX
        /// OriginName: accuracyHor, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AccuracyhorField = new Field.Builder()
            .Name(nameof(Accuracyhor))
            .Title("accuracyHor")
            .Description("Horizontal accuracy in mm (m * 1E-3). If unknown set to UINT32_MAX")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Accuracyhor;
        public uint Accuracyhor { get => _Accuracyhor; set { _Accuracyhor = value; } }
        /// <summary>
        /// Vertical accuracy in cm. If unknown set to UINT16_MAX
        /// OriginName: accuracyVert, Units: cm, IsExtended: false
        /// </summary>
        public static readonly Field AccuracyvertField = new Field.Builder()
            .Name(nameof(Accuracyvert))
            .Title("accuracyVert")
            .Description("Vertical accuracy in cm. If unknown set to UINT16_MAX")
            .FormatString(string.Empty)
            .Units(@"cm")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Accuracyvert;
        public ushort Accuracyvert { get => _Accuracyvert; set { _Accuracyvert = value; } }
        /// <summary>
        /// Velocity accuracy in mm/s (m * 1E-3). If unknown set to UINT16_MAX
        /// OriginName: accuracyVel, Units: mm/s, IsExtended: false
        /// </summary>
        public static readonly Field AccuracyvelField = new Field.Builder()
            .Name(nameof(Accuracyvel))
            .Title("accuracyVel")
            .Description("Velocity accuracy in mm/s (m * 1E-3). If unknown set to UINT16_MAX")
            .FormatString(string.Empty)
            .Units(@"mm/s")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Accuracyvel;
        public ushort Accuracyvel { get => _Accuracyvel; set { _Accuracyvel = value; } }
        /// <summary>
        /// GPS vertical speed in cm/s. If unknown set to INT16_MAX
        /// OriginName: velVert, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VelvertField = new Field.Builder()
            .Name(nameof(Velvert))
            .Title("velVert")
            .Description("GPS vertical speed in cm/s. If unknown set to INT16_MAX")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Velvert;
        public short Velvert { get => _Velvert; set { _Velvert = value; } }
        /// <summary>
        /// North-South velocity over ground in cm/s North +ve. If unknown set to INT16_MAX
        /// OriginName: velNS, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VelnsField = new Field.Builder()
            .Name(nameof(Velns))
            .Title("velNS")
            .Description("North-South velocity over ground in cm/s North +ve. If unknown set to INT16_MAX")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Velns;
        public short Velns { get => _Velns; set { _Velns = value; } }
        /// <summary>
        /// East-West velocity over ground in cm/s East +ve. If unknown set to INT16_MAX
        /// OriginName: VelEW, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VelewField = new Field.Builder()
            .Name(nameof(Velew))
            .Title("VelEW")
            .Description("East-West velocity over ground in cm/s East +ve. If unknown set to INT16_MAX")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Velew;
        public short Velew { get => _Velew; set { _Velew = value; } }
        /// <summary>
        /// ADS-B transponder dynamic input state flags
        /// OriginName: state, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StateField = new Field.Builder()
            .Name(nameof(State))
            .Title("bitmask")
            .Description("ADS-B transponder dynamic input state flags")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        public UavionixAdsbOutDynamicState _State;
        public UavionixAdsbOutDynamicState State { get => _State; set => _State = value; } 
        /// <summary>
        /// Mode A code (typically 1200 [0x04B0] for VFR)
        /// OriginName: squawk, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SquawkField = new Field.Builder()
            .Name(nameof(Squawk))
            .Title("squawk")
            .Description("Mode A code (typically 1200 [0x04B0] for VFR)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Squawk;
        public ushort Squawk { get => _Squawk; set { _Squawk = value; } }
        /// <summary>
        /// 0-1: no fix, 2: 2D fix, 3: 3D fix, 4: DGPS, 5: RTK
        /// OriginName: gpsFix, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GpsfixField = new Field.Builder()
            .Name(nameof(Gpsfix))
            .Title("gpsFix")
            .Description("0-1: no fix, 2: 2D fix, 3: 3D fix, 4: DGPS, 5: RTK")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public UavionixAdsbOutDynamicGpsFix _Gpsfix;
        public UavionixAdsbOutDynamicGpsFix Gpsfix { get => _Gpsfix; set => _Gpsfix = value; } 
        /// <summary>
        /// Number of satellites visible. If unknown set to UINT8_MAX
        /// OriginName: numSats, Units: , IsExtended: false
        /// </summary>
        public static readonly Field NumsatsField = new Field.Builder()
            .Name(nameof(Numsats))
            .Title("numSats")
            .Description("Number of satellites visible. If unknown set to UINT8_MAX")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Numsats;
        public byte Numsats { get => _Numsats; set { _Numsats = value; } }
        /// <summary>
        /// Emergency status
        /// OriginName: emergencyStatus, Units: , IsExtended: false
        /// </summary>
        public static readonly Field EmergencystatusField = new Field.Builder()
            .Name(nameof(Emergencystatus))
            .Title("emergencyStatus")
            .Description("Emergency status")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public UavionixAdsbEmergencyStatus _Emergencystatus;
        public UavionixAdsbEmergencyStatus Emergencystatus { get => _Emergencystatus; set => _Emergencystatus = value; } 
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

        public void Visit(IVisitor visitor)
        {
            var tmpRfhealth = (byte)Rfhealth;
            UInt8Type.Accept(visitor,RfhealthField, ref tmpRfhealth);
            Rfhealth = (UavionixAdsbRfHealth)tmpRfhealth;

        }

        /// <summary>
        /// ADS-B transponder messages
        /// OriginName: rfHealth, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RfhealthField = new Field.Builder()
            .Name(nameof(Rfhealth))
            .Title("bitmask")
            .Description("ADS-B transponder messages")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public UavionixAdsbRfHealth _Rfhealth;
        public UavionixAdsbRfHealth Rfhealth { get => _Rfhealth; set => _Rfhealth = value; } 
    }




        


#endregion


}

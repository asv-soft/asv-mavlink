// MIT License
//
// Copyright (c) 2018 Alexey Voloshkevich Cursir ltd. (https://github.com/asvol)
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

// This code was generate by tool Asv.Mavlink.Shell version 1.0.0

using System;
using System.Text;
using Asv.IO;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.V2.Uavionix
{

    public static class UavionixHelper
    {
        public static void RegisterUavionixDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new UavionixAdsbOutCfgPacket());
            src.Register(()=>new UavionixAdsbOutDynamicPacket());
            src.Register(()=>new UavionixAdsbTransceiverHealthReportPacket());
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
    public class UavionixAdsbOutCfgPacket: PacketV2<UavionixAdsbOutCfgPayload>
    {
	    public const int PacketMessageId = 10001;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 209;

        public override UavionixAdsbOutCfgPayload Payload { get; } = new UavionixAdsbOutCfgPayload();

        public override string Name => "UAVIONIX_ADSB_OUT_CFG";
    }

    /// <summary>
    ///  UAVIONIX_ADSB_OUT_CFG
    /// </summary>
    public class UavionixAdsbOutCfgPayload : IPayload
    {
        public byte GetMaxByteSize() => 20; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Icao = BinSerialize.ReadUInt(ref buffer);index+=4;
            Stallspeed = BinSerialize.ReadUShort(ref buffer);index+=2;
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
            index+=arraySize;
            Emittertype = (AdsbEmitterType)BinSerialize.ReadByte(ref buffer);index+=1;
            Aircraftsize = (UavionixAdsbOutCfgAircraftSize)BinSerialize.ReadByte(ref buffer);index+=1;
            Gpsoffsetlat = (UavionixAdsbOutCfgGpsOffsetLat)BinSerialize.ReadByte(ref buffer);index+=1;
            Gpsoffsetlon = (UavionixAdsbOutCfgGpsOffsetLon)BinSerialize.ReadByte(ref buffer);index+=1;
            Rfselect = (UavionixAdsbOutRfSelect)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,Icao);index+=4;
            BinSerialize.WriteUShort(ref buffer,Stallspeed);index+=2;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Callsign)
                {
                    Encoding.ASCII.GetBytes(charPointer, Callsign.Length, bytePointer, Callsign.Length);
                }
            }
            buffer = buffer.Slice(Callsign.Length);
            index+=Callsign.Length;
            BinSerialize.WriteByte(ref buffer,(byte)Emittertype);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Aircraftsize);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Gpsoffsetlat);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Gpsoffsetlon);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Rfselect);index+=1;
            return index; // /*PayloadByteSize*/20;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Icao = BitConverter.ToUInt32(buffer,index);index+=4;
            Stallspeed = BitConverter.ToUInt16(buffer,index);index+=2;
            arraySize = /*ArrayLength*/9 - Math.Max(0,((/*PayloadByteSize*/20 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Callsign = new char[arraySize];
            Encoding.ASCII.GetChars(buffer, index,arraySize,Callsign,0);
            index+=arraySize;
            Emittertype = (AdsbEmitterType)buffer[index++];
            Aircraftsize = (UavionixAdsbOutCfgAircraftSize)buffer[index++];
            Gpsoffsetlat = (UavionixAdsbOutCfgGpsOffsetLat)buffer[index++];
            Gpsoffsetlon = (UavionixAdsbOutCfgGpsOffsetLon)buffer[index++];
            Rfselect = (UavionixAdsbOutRfSelect)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Icao).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Stallspeed).CopyTo(buffer, index);index+=2;
            index+=Encoding.ASCII.GetBytes(Callsign,0,Callsign.Length,buffer,index);
            buffer[index] = (byte)Emittertype;index+=1;
            buffer[index] = (byte)Aircraftsize;index+=1;
            buffer[index] = (byte)Gpsoffsetlat;index+=1;
            buffer[index] = (byte)Gpsoffsetlon;index+=1;
            buffer[index] = (byte)Rfselect;index+=1;
            return index - start; // /*PayloadByteSize*/20;
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
        public char[] Callsign { get; set; } = new char[9];
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
        /// ADS-B transponder reciever and transmit enable flags
        /// OriginName: rfSelect, Units: , IsExtended: false
        /// </summary>
        public UavionixAdsbOutRfSelect Rfselect { get; set; }
    }
    /// <summary>
    /// Dynamic data used to generate ADS-B out transponder data (send at 5Hz)
    ///  UAVIONIX_ADSB_OUT_DYNAMIC
    /// </summary>
    public class UavionixAdsbOutDynamicPacket: PacketV2<UavionixAdsbOutDynamicPayload>
    {
	    public const int PacketMessageId = 10002;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 186;

        public override UavionixAdsbOutDynamicPayload Payload { get; } = new UavionixAdsbOutDynamicPayload();

        public override string Name => "UAVIONIX_ADSB_OUT_DYNAMIC";
    }

    /// <summary>
    ///  UAVIONIX_ADSB_OUT_DYNAMIC
    /// </summary>
    public class UavionixAdsbOutDynamicPayload : IPayload
    {
        public byte GetMaxByteSize() => 41; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 41; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            Utctime = BinSerialize.ReadUInt(ref buffer);index+=4;
            Gpslat = BinSerialize.ReadInt(ref buffer);index+=4;
            Gpslon = BinSerialize.ReadInt(ref buffer);index+=4;
            Gpsalt = BinSerialize.ReadInt(ref buffer);index+=4;
            Baroaltmsl = BinSerialize.ReadInt(ref buffer);index+=4;
            Accuracyhor = BinSerialize.ReadUInt(ref buffer);index+=4;
            Accuracyvert = BinSerialize.ReadUShort(ref buffer);index+=2;
            Accuracyvel = BinSerialize.ReadUShort(ref buffer);index+=2;
            Velvert = BinSerialize.ReadShort(ref buffer);index+=2;
            Velns = BinSerialize.ReadShort(ref buffer);index+=2;
            Velew = BinSerialize.ReadShort(ref buffer);index+=2;
            State = (UavionixAdsbOutDynamicState)BinSerialize.ReadUShort(ref buffer);index+=2;
            Squawk = BinSerialize.ReadUShort(ref buffer);index+=2;
            Gpsfix = (UavionixAdsbOutDynamicGpsFix)BinSerialize.ReadByte(ref buffer);index+=1;
            Numsats = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Emergencystatus = (UavionixAdsbEmergencyStatus)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,Utctime);index+=4;
            BinSerialize.WriteInt(ref buffer,Gpslat);index+=4;
            BinSerialize.WriteInt(ref buffer,Gpslon);index+=4;
            BinSerialize.WriteInt(ref buffer,Gpsalt);index+=4;
            BinSerialize.WriteInt(ref buffer,Baroaltmsl);index+=4;
            BinSerialize.WriteUInt(ref buffer,Accuracyhor);index+=4;
            BinSerialize.WriteUShort(ref buffer,Accuracyvert);index+=2;
            BinSerialize.WriteUShort(ref buffer,Accuracyvel);index+=2;
            BinSerialize.WriteShort(ref buffer,Velvert);index+=2;
            BinSerialize.WriteShort(ref buffer,Velns);index+=2;
            BinSerialize.WriteShort(ref buffer,Velew);index+=2;
            BinSerialize.WriteUShort(ref buffer,(ushort)State);index+=2;
            BinSerialize.WriteUShort(ref buffer,Squawk);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)Gpsfix);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Numsats);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Emergencystatus);index+=1;
            return index; // /*PayloadByteSize*/41;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            Utctime = BitConverter.ToUInt32(buffer,index);index+=4;
            Gpslat = BitConverter.ToInt32(buffer,index);index+=4;
            Gpslon = BitConverter.ToInt32(buffer,index);index+=4;
            Gpsalt = BitConverter.ToInt32(buffer,index);index+=4;
            Baroaltmsl = BitConverter.ToInt32(buffer,index);index+=4;
            Accuracyhor = BitConverter.ToUInt32(buffer,index);index+=4;
            Accuracyvert = BitConverter.ToUInt16(buffer,index);index+=2;
            Accuracyvel = BitConverter.ToUInt16(buffer,index);index+=2;
            Velvert = BitConverter.ToInt16(buffer,index);index+=2;
            Velns = BitConverter.ToInt16(buffer,index);index+=2;
            Velew = BitConverter.ToInt16(buffer,index);index+=2;
            State = (UavionixAdsbOutDynamicState)BitConverter.ToUInt16(buffer,index);index+=2;
            Squawk = BitConverter.ToUInt16(buffer,index);index+=2;
            Gpsfix = (UavionixAdsbOutDynamicGpsFix)buffer[index++];
            Numsats = (byte)buffer[index++];
            Emergencystatus = (UavionixAdsbEmergencyStatus)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Utctime).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Gpslat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Gpslon).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Gpsalt).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Baroaltmsl).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Accuracyhor).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Accuracyvert).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Accuracyvel).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Velvert).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Velns).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Velew).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes((ushort)State).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Squawk).CopyTo(buffer, index);index+=2;
            buffer[index] = (byte)Gpsfix;index+=1;
            BitConverter.GetBytes(Numsats).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Emergencystatus;index+=1;
            return index - start; // /*PayloadByteSize*/41;
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
    public class UavionixAdsbTransceiverHealthReportPacket: PacketV2<UavionixAdsbTransceiverHealthReportPayload>
    {
	    public const int PacketMessageId = 10003;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 4;

        public override UavionixAdsbTransceiverHealthReportPayload Payload { get; } = new UavionixAdsbTransceiverHealthReportPayload();

        public override string Name => "UAVIONIX_ADSB_TRANSCEIVER_HEALTH_REPORT";
    }

    /// <summary>
    ///  UAVIONIX_ADSB_TRANSCEIVER_HEALTH_REPORT
    /// </summary>
    public class UavionixAdsbTransceiverHealthReportPayload : IPayload
    {
        public byte GetMaxByteSize() => 1; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 1; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            Rfhealth = (UavionixAdsbRfHealth)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)Rfhealth);index+=1;
            return index; // /*PayloadByteSize*/1;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            Rfhealth = (UavionixAdsbRfHealth)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            buffer[index] = (byte)Rfhealth;index+=1;
            return index - start; // /*PayloadByteSize*/1;
        }

        /// <summary>
        /// ADS-B transponder messages
        /// OriginName: rfHealth, Units: , IsExtended: false
        /// </summary>
        public UavionixAdsbRfHealth Rfhealth { get; set; }
    }


#endregion


}

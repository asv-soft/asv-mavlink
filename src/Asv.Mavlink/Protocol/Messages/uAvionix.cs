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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.17-dev.8+356100e330ee3351d1c0a76be38f09294117ae6a 25-09-26.

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
    public enum UavionixAdsbOutDynamicState : ulong
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
    public static class UavionixAdsbOutDynamicStateHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(4);
            yield return converter(8);
            yield return converter(16);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"UAVIONIX_ADSB_OUT_DYNAMIC_STATE_INTENT_CHANGE");
            yield return new EnumValue<T>(converter(2),"UAVIONIX_ADSB_OUT_DYNAMIC_STATE_AUTOPILOT_ENABLED");
            yield return new EnumValue<T>(converter(4),"UAVIONIX_ADSB_OUT_DYNAMIC_STATE_NICBARO_CROSSCHECKED");
            yield return new EnumValue<T>(converter(8),"UAVIONIX_ADSB_OUT_DYNAMIC_STATE_ON_GROUND");
            yield return new EnumValue<T>(converter(16),"UAVIONIX_ADSB_OUT_DYNAMIC_STATE_IDENT");
        }
    }
    /// <summary>
    /// Transceiver RF control flags for ADS-B transponder dynamic reports
    ///  UAVIONIX_ADSB_OUT_RF_SELECT
    /// </summary>
    public enum UavionixAdsbOutRfSelect : ulong
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
    public static class UavionixAdsbOutRfSelectHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"UAVIONIX_ADSB_OUT_RF_SELECT_STANDBY");
            yield return new EnumValue<T>(converter(1),"UAVIONIX_ADSB_OUT_RF_SELECT_RX_ENABLED");
            yield return new EnumValue<T>(converter(2),"UAVIONIX_ADSB_OUT_RF_SELECT_TX_ENABLED");
        }
    }
    /// <summary>
    /// Status for ADS-B transponder dynamic input
    ///  UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX
    /// </summary>
    public enum UavionixAdsbOutDynamicGpsFix : ulong
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
    public static class UavionixAdsbOutDynamicGpsFixHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
            yield return converter(4);
            yield return converter(5);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_NONE_0");
            yield return new EnumValue<T>(converter(1),"UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_NONE_1");
            yield return new EnumValue<T>(converter(2),"UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_2D");
            yield return new EnumValue<T>(converter(3),"UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_3D");
            yield return new EnumValue<T>(converter(4),"UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_DGPS");
            yield return new EnumValue<T>(converter(5),"UAVIONIX_ADSB_OUT_DYNAMIC_GPS_FIX_RTK");
        }
    }
    /// <summary>
    /// Status flags for ADS-B transponder dynamic output
    ///  UAVIONIX_ADSB_RF_HEALTH
    /// </summary>
    public enum UavionixAdsbRfHealth : ulong
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
    public static class UavionixAdsbRfHealthHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
            yield return converter(16);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"UAVIONIX_ADSB_RF_HEALTH_INITIALIZING");
            yield return new EnumValue<T>(converter(1),"UAVIONIX_ADSB_RF_HEALTH_OK");
            yield return new EnumValue<T>(converter(2),"UAVIONIX_ADSB_RF_HEALTH_FAIL_TX");
            yield return new EnumValue<T>(converter(16),"UAVIONIX_ADSB_RF_HEALTH_FAIL_RX");
        }
    }
    /// <summary>
    /// Definitions for aircraft size
    ///  UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE
    /// </summary>
    public enum UavionixAdsbOutCfgAircraftSize : ulong
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
    public static class UavionixAdsbOutCfgAircraftSizeHelper
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
            yield return converter(7);
            yield return converter(8);
            yield return converter(9);
            yield return converter(10);
            yield return converter(11);
            yield return converter(12);
            yield return converter(13);
            yield return converter(14);
            yield return converter(15);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_NO_DATA");
            yield return new EnumValue<T>(converter(1),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L15M_W23M");
            yield return new EnumValue<T>(converter(2),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L25M_W28P5M");
            yield return new EnumValue<T>(converter(3),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L25_34M");
            yield return new EnumValue<T>(converter(4),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L35_33M");
            yield return new EnumValue<T>(converter(5),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L35_38M");
            yield return new EnumValue<T>(converter(6),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L45_39P5M");
            yield return new EnumValue<T>(converter(7),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L45_45M");
            yield return new EnumValue<T>(converter(8),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L55_45M");
            yield return new EnumValue<T>(converter(9),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L55_52M");
            yield return new EnumValue<T>(converter(10),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L65_59P5M");
            yield return new EnumValue<T>(converter(11),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L65_67M");
            yield return new EnumValue<T>(converter(12),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L75_W72P5M");
            yield return new EnumValue<T>(converter(13),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L75_W80M");
            yield return new EnumValue<T>(converter(14),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L85_W80M");
            yield return new EnumValue<T>(converter(15),"UAVIONIX_ADSB_OUT_CFG_AIRCRAFT_SIZE_L85_W90M");
        }
    }
    /// <summary>
    /// GPS lataral offset encoding
    ///  UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT
    /// </summary>
    public enum UavionixAdsbOutCfgGpsOffsetLat : ulong
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
    public static class UavionixAdsbOutCfgGpsOffsetLatHelper
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
            yield return converter(7);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_NO_DATA");
            yield return new EnumValue<T>(converter(1),"UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_LEFT_2M");
            yield return new EnumValue<T>(converter(2),"UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_LEFT_4M");
            yield return new EnumValue<T>(converter(3),"UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_LEFT_6M");
            yield return new EnumValue<T>(converter(4),"UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_RIGHT_0M");
            yield return new EnumValue<T>(converter(5),"UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_RIGHT_2M");
            yield return new EnumValue<T>(converter(6),"UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_RIGHT_4M");
            yield return new EnumValue<T>(converter(7),"UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LAT_RIGHT_6M");
        }
    }
    /// <summary>
    /// GPS longitudinal offset encoding
    ///  UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LON
    /// </summary>
    public enum UavionixAdsbOutCfgGpsOffsetLon : ulong
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
    public static class UavionixAdsbOutCfgGpsOffsetLonHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LON_NO_DATA");
            yield return new EnumValue<T>(converter(1),"UAVIONIX_ADSB_OUT_CFG_GPS_OFFSET_LON_APPLIED_BY_SENSOR");
        }
    }
    /// <summary>
    /// Emergency status encoding
    ///  UAVIONIX_ADSB_EMERGENCY_STATUS
    /// </summary>
    public enum UavionixAdsbEmergencyStatus : ulong
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
    public static class UavionixAdsbEmergencyStatusHelper
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
            yield return converter(7);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"UAVIONIX_ADSB_OUT_NO_EMERGENCY");
            yield return new EnumValue<T>(converter(1),"UAVIONIX_ADSB_OUT_GENERAL_EMERGENCY");
            yield return new EnumValue<T>(converter(2),"UAVIONIX_ADSB_OUT_LIFEGUARD_EMERGENCY");
            yield return new EnumValue<T>(converter(3),"UAVIONIX_ADSB_OUT_MINIMUM_FUEL_EMERGENCY");
            yield return new EnumValue<T>(converter(4),"UAVIONIX_ADSB_OUT_NO_COMM_EMERGENCY");
            yield return new EnumValue<T>(converter(5),"UAVIONIX_ADSB_OUT_UNLAWFUL_INTERFERANCE_EMERGENCY");
            yield return new EnumValue<T>(converter(6),"UAVIONIX_ADSB_OUT_DOWNED_AIRCRAFT_EMERGENCY");
            yield return new EnumValue<T>(converter(7),"UAVIONIX_ADSB_OUT_RESERVED");
        }
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,IcaoField, ref _icao);    
            UInt16Type.Accept(visitor,StallspeedField, ref _stallspeed);    
            ArrayType.Accept(visitor,CallsignField,  
                (index, v, f, t) => CharType.Accept(v, f, t, ref Callsign[index]));
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

            .DataType(UInt32Type.Default)
        .Build();
        private uint _icao;
        public uint Icao { get => _icao; set => _icao = value; }
        /// <summary>
        /// Aircraft stall speed in cm/s
        /// OriginName: stallSpeed, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field StallspeedField = new Field.Builder()
            .Name(nameof(Stallspeed))
            .Title("stallSpeed")
            .Description("Aircraft stall speed in cm/s")
.Units(@"cm/s")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _stallspeed;
        public ushort Stallspeed { get => _stallspeed; set => _stallspeed = value; }
        /// <summary>
        /// Vehicle identifier (8 characters, null terminated, valid characters are A-Z, 0-9, " " only)
        /// OriginName: callsign, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CallsignField = new Field.Builder()
            .Name(nameof(Callsign))
            .Title("callsign")
            .Description("Vehicle identifier (8 characters, null terminated, valid characters are A-Z, 0-9, \" \" only)")

            .DataType(new ArrayType(CharType.Ascii,9))
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
            .DataType(new UInt8Type(AdsbEmitterTypeHelper.GetValues(x=>(byte)x).Min(),AdsbEmitterTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AdsbEmitterTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AdsbEmitterType _emittertype;
        public AdsbEmitterType Emittertype { get => _emittertype; set => _emittertype = value; } 
        /// <summary>
        /// Aircraft length and width encoding (table 2-35 of DO-282B)
        /// OriginName: aircraftSize, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AircraftsizeField = new Field.Builder()
            .Name(nameof(Aircraftsize))
            .Title("aircraftSize")
            .Description("Aircraft length and width encoding (table 2-35 of DO-282B)")
            .DataType(new UInt8Type(UavionixAdsbOutCfgAircraftSizeHelper.GetValues(x=>(byte)x).Min(),UavionixAdsbOutCfgAircraftSizeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(UavionixAdsbOutCfgAircraftSizeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private UavionixAdsbOutCfgAircraftSize _aircraftsize;
        public UavionixAdsbOutCfgAircraftSize Aircraftsize { get => _aircraftsize; set => _aircraftsize = value; } 
        /// <summary>
        /// GPS antenna lateral offset (table 2-36 of DO-282B)
        /// OriginName: gpsOffsetLat, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GpsoffsetlatField = new Field.Builder()
            .Name(nameof(Gpsoffsetlat))
            .Title("gpsOffsetLat")
            .Description("GPS antenna lateral offset (table 2-36 of DO-282B)")
            .DataType(new UInt8Type(UavionixAdsbOutCfgGpsOffsetLatHelper.GetValues(x=>(byte)x).Min(),UavionixAdsbOutCfgGpsOffsetLatHelper.GetValues(x=>(byte)x).Max()))
            .Enum(UavionixAdsbOutCfgGpsOffsetLatHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private UavionixAdsbOutCfgGpsOffsetLat _gpsoffsetlat;
        public UavionixAdsbOutCfgGpsOffsetLat Gpsoffsetlat { get => _gpsoffsetlat; set => _gpsoffsetlat = value; } 
        /// <summary>
        /// GPS antenna longitudinal offset from nose [if non-zero, take position (in meters) divide by 2 and add one] (table 2-37 DO-282B)
        /// OriginName: gpsOffsetLon, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GpsoffsetlonField = new Field.Builder()
            .Name(nameof(Gpsoffsetlon))
            .Title("gpsOffsetLon")
            .Description("GPS antenna longitudinal offset from nose [if non-zero, take position (in meters) divide by 2 and add one] (table 2-37 DO-282B)")
            .DataType(new UInt8Type(UavionixAdsbOutCfgGpsOffsetLonHelper.GetValues(x=>(byte)x).Min(),UavionixAdsbOutCfgGpsOffsetLonHelper.GetValues(x=>(byte)x).Max()))
            .Enum(UavionixAdsbOutCfgGpsOffsetLonHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private UavionixAdsbOutCfgGpsOffsetLon _gpsoffsetlon;
        public UavionixAdsbOutCfgGpsOffsetLon Gpsoffsetlon { get => _gpsoffsetlon; set => _gpsoffsetlon = value; } 
        /// <summary>
        /// ADS-B transponder receiver and transmit enable flags
        /// OriginName: rfSelect, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RfselectField = new Field.Builder()
            .Name(nameof(Rfselect))
            .Title("bitmask")
            .Description("ADS-B transponder receiver and transmit enable flags")
            .DataType(new UInt8Type(UavionixAdsbOutRfSelectHelper.GetValues(x=>(byte)x).Min(),UavionixAdsbOutRfSelectHelper.GetValues(x=>(byte)x).Max()))
            .Enum(UavionixAdsbOutRfSelectHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private UavionixAdsbOutRfSelect _rfselect;
        public UavionixAdsbOutRfSelect Rfselect { get => _rfselect; set => _rfselect = value; } 
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,UtctimeField, ref _utctime);    
            Int32Type.Accept(visitor,GpslatField, ref _gpslat);    
            Int32Type.Accept(visitor,GpslonField, ref _gpslon);    
            Int32Type.Accept(visitor,GpsaltField, ref _gpsalt);    
            Int32Type.Accept(visitor,BaroaltmslField, ref _baroaltmsl);    
            UInt32Type.Accept(visitor,AccuracyhorField, ref _accuracyhor);    
            UInt16Type.Accept(visitor,AccuracyvertField, ref _accuracyvert);    
            UInt16Type.Accept(visitor,AccuracyvelField, ref _accuracyvel);    
            Int16Type.Accept(visitor,VelvertField, ref _velvert);
            Int16Type.Accept(visitor,VelnsField, ref _velns);
            Int16Type.Accept(visitor,VelewField, ref _velew);
            var tmpState = (ushort)State;
            UInt16Type.Accept(visitor,StateField, ref tmpState);
            State = (UavionixAdsbOutDynamicState)tmpState;
            UInt16Type.Accept(visitor,SquawkField, ref _squawk);    
            var tmpGpsfix = (byte)Gpsfix;
            UInt8Type.Accept(visitor,GpsfixField, ref tmpGpsfix);
            Gpsfix = (UavionixAdsbOutDynamicGpsFix)tmpGpsfix;
            UInt8Type.Accept(visitor,NumsatsField, ref _numsats);    
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
.Units(@"s")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _utctime;
        public uint Utctime { get => _utctime; set => _utctime = value; }
        /// <summary>
        /// Latitude WGS84 (deg * 1E7). If unknown set to INT32_MAX
        /// OriginName: gpsLat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GpslatField = new Field.Builder()
            .Name(nameof(Gpslat))
            .Title("gpsLat")
            .Description("Latitude WGS84 (deg * 1E7). If unknown set to INT32_MAX")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _gpslat;
        public int Gpslat { get => _gpslat; set => _gpslat = value; }
        /// <summary>
        /// Longitude WGS84 (deg * 1E7). If unknown set to INT32_MAX
        /// OriginName: gpsLon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GpslonField = new Field.Builder()
            .Name(nameof(Gpslon))
            .Title("gpsLon")
            .Description("Longitude WGS84 (deg * 1E7). If unknown set to INT32_MAX")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _gpslon;
        public int Gpslon { get => _gpslon; set => _gpslon = value; }
        /// <summary>
        /// Altitude (WGS84). UP +ve. If unknown set to INT32_MAX
        /// OriginName: gpsAlt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GpsaltField = new Field.Builder()
            .Name(nameof(Gpsalt))
            .Title("gpsAlt")
            .Description("Altitude (WGS84). UP +ve. If unknown set to INT32_MAX")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _gpsalt;
        public int Gpsalt { get => _gpsalt; set => _gpsalt = value; }
        /// <summary>
        /// Barometric pressure altitude (MSL) relative to a standard atmosphere of 1013.2 mBar and NOT bar corrected altitude (m * 1E-3). (up +ve). If unknown set to INT32_MAX
        /// OriginName: baroAltMSL, Units: mbar, IsExtended: false
        /// </summary>
        public static readonly Field BaroaltmslField = new Field.Builder()
            .Name(nameof(Baroaltmsl))
            .Title("baroAltMSL")
            .Description("Barometric pressure altitude (MSL) relative to a standard atmosphere of 1013.2 mBar and NOT bar corrected altitude (m * 1E-3). (up +ve). If unknown set to INT32_MAX")
.Units(@"mbar")
            .DataType(Int32Type.Default)
        .Build();
        private int _baroaltmsl;
        public int Baroaltmsl { get => _baroaltmsl; set => _baroaltmsl = value; }
        /// <summary>
        /// Horizontal accuracy in mm (m * 1E-3). If unknown set to UINT32_MAX
        /// OriginName: accuracyHor, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AccuracyhorField = new Field.Builder()
            .Name(nameof(Accuracyhor))
            .Title("accuracyHor")
            .Description("Horizontal accuracy in mm (m * 1E-3). If unknown set to UINT32_MAX")
.Units(@"mm")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _accuracyhor;
        public uint Accuracyhor { get => _accuracyhor; set => _accuracyhor = value; }
        /// <summary>
        /// Vertical accuracy in cm. If unknown set to UINT16_MAX
        /// OriginName: accuracyVert, Units: cm, IsExtended: false
        /// </summary>
        public static readonly Field AccuracyvertField = new Field.Builder()
            .Name(nameof(Accuracyvert))
            .Title("accuracyVert")
            .Description("Vertical accuracy in cm. If unknown set to UINT16_MAX")
.Units(@"cm")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _accuracyvert;
        public ushort Accuracyvert { get => _accuracyvert; set => _accuracyvert = value; }
        /// <summary>
        /// Velocity accuracy in mm/s (m * 1E-3). If unknown set to UINT16_MAX
        /// OriginName: accuracyVel, Units: mm/s, IsExtended: false
        /// </summary>
        public static readonly Field AccuracyvelField = new Field.Builder()
            .Name(nameof(Accuracyvel))
            .Title("accuracyVel")
            .Description("Velocity accuracy in mm/s (m * 1E-3). If unknown set to UINT16_MAX")
.Units(@"mm/s")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _accuracyvel;
        public ushort Accuracyvel { get => _accuracyvel; set => _accuracyvel = value; }
        /// <summary>
        /// GPS vertical speed in cm/s. If unknown set to INT16_MAX
        /// OriginName: velVert, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VelvertField = new Field.Builder()
            .Name(nameof(Velvert))
            .Title("velVert")
            .Description("GPS vertical speed in cm/s. If unknown set to INT16_MAX")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _velvert;
        public short Velvert { get => _velvert; set => _velvert = value; }
        /// <summary>
        /// North-South velocity over ground in cm/s North +ve. If unknown set to INT16_MAX
        /// OriginName: velNS, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VelnsField = new Field.Builder()
            .Name(nameof(Velns))
            .Title("velNS")
            .Description("North-South velocity over ground in cm/s North +ve. If unknown set to INT16_MAX")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _velns;
        public short Velns { get => _velns; set => _velns = value; }
        /// <summary>
        /// East-West velocity over ground in cm/s East +ve. If unknown set to INT16_MAX
        /// OriginName: VelEW, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VelewField = new Field.Builder()
            .Name(nameof(Velew))
            .Title("VelEW")
            .Description("East-West velocity over ground in cm/s East +ve. If unknown set to INT16_MAX")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _velew;
        public short Velew { get => _velew; set => _velew = value; }
        /// <summary>
        /// ADS-B transponder dynamic input state flags
        /// OriginName: state, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StateField = new Field.Builder()
            .Name(nameof(State))
            .Title("bitmask")
            .Description("ADS-B transponder dynamic input state flags")
            .DataType(new UInt16Type(UavionixAdsbOutDynamicStateHelper.GetValues(x=>(ushort)x).Min(),UavionixAdsbOutDynamicStateHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(UavionixAdsbOutDynamicStateHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private UavionixAdsbOutDynamicState _state;
        public UavionixAdsbOutDynamicState State { get => _state; set => _state = value; } 
        /// <summary>
        /// Mode A code (typically 1200 [0x04B0] for VFR)
        /// OriginName: squawk, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SquawkField = new Field.Builder()
            .Name(nameof(Squawk))
            .Title("squawk")
            .Description("Mode A code (typically 1200 [0x04B0] for VFR)")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _squawk;
        public ushort Squawk { get => _squawk; set => _squawk = value; }
        /// <summary>
        /// 0-1: no fix, 2: 2D fix, 3: 3D fix, 4: DGPS, 5: RTK
        /// OriginName: gpsFix, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GpsfixField = new Field.Builder()
            .Name(nameof(Gpsfix))
            .Title("gpsFix")
            .Description("0-1: no fix, 2: 2D fix, 3: 3D fix, 4: DGPS, 5: RTK")
            .DataType(new UInt8Type(UavionixAdsbOutDynamicGpsFixHelper.GetValues(x=>(byte)x).Min(),UavionixAdsbOutDynamicGpsFixHelper.GetValues(x=>(byte)x).Max()))
            .Enum(UavionixAdsbOutDynamicGpsFixHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private UavionixAdsbOutDynamicGpsFix _gpsfix;
        public UavionixAdsbOutDynamicGpsFix Gpsfix { get => _gpsfix; set => _gpsfix = value; } 
        /// <summary>
        /// Number of satellites visible. If unknown set to UINT8_MAX
        /// OriginName: numSats, Units: , IsExtended: false
        /// </summary>
        public static readonly Field NumsatsField = new Field.Builder()
            .Name(nameof(Numsats))
            .Title("numSats")
            .Description("Number of satellites visible. If unknown set to UINT8_MAX")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _numsats;
        public byte Numsats { get => _numsats; set => _numsats = value; }
        /// <summary>
        /// Emergency status
        /// OriginName: emergencyStatus, Units: , IsExtended: false
        /// </summary>
        public static readonly Field EmergencystatusField = new Field.Builder()
            .Name(nameof(Emergencystatus))
            .Title("emergencyStatus")
            .Description("Emergency status")
            .DataType(new UInt8Type(UavionixAdsbEmergencyStatusHelper.GetValues(x=>(byte)x).Min(),UavionixAdsbEmergencyStatusHelper.GetValues(x=>(byte)x).Max()))
            .Enum(UavionixAdsbEmergencyStatusHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private UavionixAdsbEmergencyStatus _emergencystatus;
        public UavionixAdsbEmergencyStatus Emergencystatus { get => _emergencystatus; set => _emergencystatus = value; } 
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

        public void Accept(IVisitor visitor)
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
            .DataType(new UInt8Type(UavionixAdsbRfHealthHelper.GetValues(x=>(byte)x).Min(),UavionixAdsbRfHealthHelper.GetValues(x=>(byte)x).Max()))
            .Enum(UavionixAdsbRfHealthHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private UavionixAdsbRfHealth _rfhealth;
        public UavionixAdsbRfHealth Rfhealth { get => _rfhealth; set => _rfhealth = value; } 
    }




        


#endregion


}

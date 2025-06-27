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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.2+82bde669fa8b85517700c6d12362e9f17d819d33 25-06-27.

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

namespace Asv.Mavlink.AsvRsga
{

    public static class AsvRsgaHelper
    {
        public static void RegisterAsvRsgaDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(AsvRsgaCompatibilityRequestPacket.MessageId, ()=>new AsvRsgaCompatibilityRequestPacket());
            src.Add(AsvRsgaCompatibilityResponsePacket.MessageId, ()=>new AsvRsgaCompatibilityResponsePacket());
            src.Add(AsvRsgaRttGnssPacket.MessageId, ()=>new AsvRsgaRttGnssPacket());
            src.Add(AsvRsgaRttSpectrumPacket.MessageId, ()=>new AsvRsgaRttSpectrumPacket());
            src.Add(AsvRsgaRttTxLlzPacket.MessageId, ()=>new AsvRsgaRttTxLlzPacket());
            src.Add(AsvRsgaRttTxGpPacket.MessageId, ()=>new AsvRsgaRttTxGpPacket());
            src.Add(AsvRsgaRttTxVorPacket.MessageId, ()=>new AsvRsgaRttTxVorPacket());
            src.Add(AsvRsgaRttTxMarkerPacket.MessageId, ()=>new AsvRsgaRttTxMarkerPacket());
            src.Add(AsvRsgaRttDmeRepPacket.MessageId, ()=>new AsvRsgaRttDmeRepPacket());
            src.Add(AsvRsgaRttTxGbasPacket.MessageId, ()=>new AsvRsgaRttTxGbasPacket());
            src.Add(AsvRsgaRttAdsbReqPacket.MessageId, ()=>new AsvRsgaRttAdsbReqPacket());
            src.Add(AsvRsgaRttTxGnssPacket.MessageId, ()=>new AsvRsgaRttTxGnssPacket());
            src.Add(AsvRsgaRttDmeReqPacket.MessageId, ()=>new AsvRsgaRttDmeReqPacket());
            src.Add(AsvRsgaRttRxLlzPacket.MessageId, ()=>new AsvRsgaRttRxLlzPacket());
            src.Add(AsvRsgaRttRxGpPacket.MessageId, ()=>new AsvRsgaRttRxGpPacket());
            src.Add(AsvRsgaRttRxVorPacket.MessageId, ()=>new AsvRsgaRttRxVorPacket());
            src.Add(AsvRsgaRttRxMarkerPacket.MessageId, ()=>new AsvRsgaRttRxMarkerPacket());
            src.Add(AsvRsgaRttRxGbasPacket.MessageId, ()=>new AsvRsgaRttRxGbasPacket());
            src.Add(AsvRsgaRttAdsbRepPacket.MessageId, ()=>new AsvRsgaRttAdsbRepPacket());
            src.Add(AsvRsgaRttRxGnssPacket.MessageId, ()=>new AsvRsgaRttRxGnssPacket());
        }
 
    }

#region Enums

    /// <summary>
    ///  MAV_TYPE
    /// </summary>
    public enum MavType : ulong
    {
        /// <summary>
        /// Identifies the Radio Signal Generator and Analyzer (RSGA) payload in the HEARTBEAT message.
        /// MAV_TYPE_ASV_RSGA
        /// </summary>
        MavTypeAsvRsga = 254,
    }
    public static class MavTypeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(254);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(254),"MAV_TYPE_ASV_RSGA");
        }
    }
    /// <summary>
    /// Defines RSGA modes mapped to the custom_mode[0–7] bit field of the HEARTBEAT message. Maximum 255 values.
    ///  ASV_RSGA_CUSTOM_MODE
    /// </summary>
    public enum AsvRsgaCustomMode : ulong
    {
        /// <summary>
        /// Default mode. No operation performed.
        /// ASV_RSGA_CUSTOM_MODE_IDLE
        /// </summary>
        AsvRsgaCustomModeIdle = 0,
        /// <summary>
        /// Spectrum analysis mode.
        /// ASV_RSGA_CUSTOM_MODE_SPECTRUM
        /// </summary>
        AsvRsgaCustomModeSpectrum = 25,
        /// <summary>
        /// Localizer generator mode.
        /// ASV_RSGA_CUSTOM_MODE_TX_LLZ
        /// </summary>
        AsvRsgaCustomModeTxLlz = 50,
        /// <summary>
        /// Glide Path generator mode.
        /// ASV_RSGA_CUSTOM_MODE_TX_GP
        /// </summary>
        AsvRsgaCustomModeTxGp = 51,
        /// <summary>
        /// VOR generator mode.
        /// ASV_RSGA_CUSTOM_MODE_TX_VOR
        /// </summary>
        AsvRsgaCustomModeTxVor = 52,
        /// <summary>
        /// Marker generator mode.
        /// ASV_RSGA_CUSTOM_MODE_TX_MARKER
        /// </summary>
        AsvRsgaCustomModeTxMarker = 53,
        /// <summary>
        /// DME beacon (replier) mode.
        /// ASV_RSGA_CUSTOM_MODE_DME_REP
        /// </summary>
        AsvRsgaCustomModeDmeRep = 54,
        /// <summary>
        /// GBAS generator mode.
        /// ASV_RSGA_CUSTOM_MODE_TX_GBAS
        /// </summary>
        AsvRsgaCustomModeTxGbas = 55,
        /// <summary>
        /// ADSB beacon(interrogator) mode.
        /// ASV_RSGA_CUSTOM_MODE_ADSB_REQ
        /// </summary>
        AsvRsgaCustomModeAdsbReq = 56,
        /// <summary>
        /// GNSS generator(satellite) mode.
        /// ASV_RSGA_CUSTOM_MODE_TX_GNSS
        /// </summary>
        AsvRsgaCustomModeTxGnss = 57,
        /// <summary>
        /// DME air(interrogator) mode.
        /// ASV_RSGA_CUSTOM_MODE_DME_REQ
        /// </summary>
        AsvRsgaCustomModeDmeReq = 74,
        /// <summary>
        /// Localizer analyzer mode.
        /// ASV_RSGA_CUSTOM_MODE_RX_LLZ
        /// </summary>
        AsvRsgaCustomModeRxLlz = 75,
        /// <summary>
        /// Glide Path analyzer mode.
        /// ASV_RSGA_CUSTOM_MODE_RX_GP
        /// </summary>
        AsvRsgaCustomModeRxGp = 76,
        /// <summary>
        /// VOR analyzer mode.
        /// ASV_RSGA_CUSTOM_MODE_RX_VOR
        /// </summary>
        AsvRsgaCustomModeRxVor = 77,
        /// <summary>
        /// Marker analyzer mode.
        /// ASV_RSGA_CUSTOM_MODE_RX_MARKER
        /// </summary>
        AsvRsgaCustomModeRxMarker = 78,
        /// <summary>
        /// GBAS analyzer mode.
        /// ASV_RSGA_CUSTOM_MODE_RX_GBAS
        /// </summary>
        AsvRsgaCustomModeRxGbas = 79,
        /// <summary>
        /// ADSB air(replier) mode.
        /// ASV_RSGA_CUSTOM_MODE_ADSB_REP
        /// </summary>
        AsvRsgaCustomModeAdsbRep = 80,
        /// <summary>
        /// GNSS analyzer mode.
        /// ASV_RSGA_CUSTOM_MODE_RX_GNSS
        /// </summary>
        AsvRsgaCustomModeRxGnss = 81,
        /// <summary>
        /// RDF mode.
        /// ASV_RSGA_CUSTOM_MODE_RDF
        /// </summary>
        AsvRsgaCustomModeRdf = 82,
        /// <summary>
        /// Audio radio station mode.
        /// ASV_RSGA_CUSTOM_MODE_RADIO
        /// </summary>
        AsvRsgaCustomModeRadio = 100,
        /// <summary>
        /// Max available mode value (Reserved).
        /// ASV_RSGA_CUSTOM_MODE_RESERVED
        /// </summary>
        AsvRsgaCustomModeReserved = 255,
    }
    public static class AsvRsgaCustomModeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(25);
            yield return converter(50);
            yield return converter(51);
            yield return converter(52);
            yield return converter(53);
            yield return converter(54);
            yield return converter(55);
            yield return converter(56);
            yield return converter(57);
            yield return converter(74);
            yield return converter(75);
            yield return converter(76);
            yield return converter(77);
            yield return converter(78);
            yield return converter(79);
            yield return converter(80);
            yield return converter(81);
            yield return converter(82);
            yield return converter(100);
            yield return converter(255);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_RSGA_CUSTOM_MODE_IDLE");
            yield return new EnumValue<T>(converter(25),"ASV_RSGA_CUSTOM_MODE_SPECTRUM");
            yield return new EnumValue<T>(converter(50),"ASV_RSGA_CUSTOM_MODE_TX_LLZ");
            yield return new EnumValue<T>(converter(51),"ASV_RSGA_CUSTOM_MODE_TX_GP");
            yield return new EnumValue<T>(converter(52),"ASV_RSGA_CUSTOM_MODE_TX_VOR");
            yield return new EnumValue<T>(converter(53),"ASV_RSGA_CUSTOM_MODE_TX_MARKER");
            yield return new EnumValue<T>(converter(54),"ASV_RSGA_CUSTOM_MODE_DME_REP");
            yield return new EnumValue<T>(converter(55),"ASV_RSGA_CUSTOM_MODE_TX_GBAS");
            yield return new EnumValue<T>(converter(56),"ASV_RSGA_CUSTOM_MODE_ADSB_REQ");
            yield return new EnumValue<T>(converter(57),"ASV_RSGA_CUSTOM_MODE_TX_GNSS");
            yield return new EnumValue<T>(converter(74),"ASV_RSGA_CUSTOM_MODE_DME_REQ");
            yield return new EnumValue<T>(converter(75),"ASV_RSGA_CUSTOM_MODE_RX_LLZ");
            yield return new EnumValue<T>(converter(76),"ASV_RSGA_CUSTOM_MODE_RX_GP");
            yield return new EnumValue<T>(converter(77),"ASV_RSGA_CUSTOM_MODE_RX_VOR");
            yield return new EnumValue<T>(converter(78),"ASV_RSGA_CUSTOM_MODE_RX_MARKER");
            yield return new EnumValue<T>(converter(79),"ASV_RSGA_CUSTOM_MODE_RX_GBAS");
            yield return new EnumValue<T>(converter(80),"ASV_RSGA_CUSTOM_MODE_ADSB_REP");
            yield return new EnumValue<T>(converter(81),"ASV_RSGA_CUSTOM_MODE_RX_GNSS");
            yield return new EnumValue<T>(converter(82),"ASV_RSGA_CUSTOM_MODE_RDF");
            yield return new EnumValue<T>(converter(100),"ASV_RSGA_CUSTOM_MODE_RADIO");
            yield return new EnumValue<T>(converter(255),"ASV_RSGA_CUSTOM_MODE_RESERVED");
        }
    }
    /// <summary>
    /// A mapping of RSGA special sub modes for custom_mode[8-15] bits field of heartbeat.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RSGA_CUSTOM_SUB_MODE
    /// </summary>
    [Flags]
    public enum AsvRsgaCustomSubMode : ulong
    {
        /// <summary>
        /// Recording enabled.
        /// ASV_RSGA_CUSTOM_SUB_MODE_RECORD
        /// </summary>
        AsvRsgaCustomSubModeRecord = 1,
        /// <summary>
        /// Mission is started.
        /// ASV_RSGA_CUSTOM_SUB_MODE_MISSION
        /// </summary>
        AsvRsgaCustomSubModeMission = 2,
        /// <summary>
        /// Reserved 2.
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED2
        /// </summary>
        AsvRsgaCustomSubModeReserved2 = 4,
        /// <summary>
        /// Reserved 3.
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED3
        /// </summary>
        AsvRsgaCustomSubModeReserved3 = 8,
        /// <summary>
        /// Reserved 4.
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED4
        /// </summary>
        AsvRsgaCustomSubModeReserved4 = 16,
        /// <summary>
        /// Reserved 5.
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED5
        /// </summary>
        AsvRsgaCustomSubModeReserved5 = 32,
        /// <summary>
        /// Reserved 6.
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED6
        /// </summary>
        AsvRsgaCustomSubModeReserved6 = 64,
        /// <summary>
        /// Reserved 7.
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED7
        /// </summary>
        AsvRsgaCustomSubModeReserved7 = 128,
    }
    public static class AsvRsgaCustomSubModeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(4);
            yield return converter(8);
            yield return converter(16);
            yield return converter(32);
            yield return converter(64);
            yield return converter(128);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_RSGA_CUSTOM_SUB_MODE_RECORD");
            yield return new EnumValue<T>(converter(2),"ASV_RSGA_CUSTOM_SUB_MODE_MISSION");
            yield return new EnumValue<T>(converter(4),"ASV_RSGA_CUSTOM_SUB_MODE_RESERVED2");
            yield return new EnumValue<T>(converter(8),"ASV_RSGA_CUSTOM_SUB_MODE_RESERVED3");
            yield return new EnumValue<T>(converter(16),"ASV_RSGA_CUSTOM_SUB_MODE_RESERVED4");
            yield return new EnumValue<T>(converter(32),"ASV_RSGA_CUSTOM_SUB_MODE_RESERVED5");
            yield return new EnumValue<T>(converter(64),"ASV_RSGA_CUSTOM_SUB_MODE_RESERVED6");
            yield return new EnumValue<T>(converter(128),"ASV_RSGA_CUSTOM_SUB_MODE_RESERVED7");
        }
    }
    /// <summary>
    /// ACK / NACK / ERROR values as a result of ASV_RSGA_*_REQUEST commands.
    ///  ASV_RSGA_REQUEST_ACK
    /// </summary>
    public enum AsvRsgaRequestAck : ulong
    {
        /// <summary>
        /// Request is ok.
        /// ASV_RSGA_REQUEST_ACK_OK
        /// </summary>
        AsvRsgaRequestAckOk = 0,
        /// <summary>
        /// Already in progress.
        /// ASV_RSGA_REQUEST_ACK_IN_PROGRESS
        /// </summary>
        AsvRsgaRequestAckInProgress = 1,
        /// <summary>
        /// Internal error.
        /// ASV_RSGA_REQUEST_ACK_FAIL
        /// </summary>
        AsvRsgaRequestAckFail = 2,
        /// <summary>
        /// Not supported.
        /// ASV_RSGA_REQUEST_ACK_NOT_SUPPORTED
        /// </summary>
        AsvRsgaRequestAckNotSupported = 3,
        /// <summary>
        /// Not found.
        /// ASV_RSGA_REQUEST_ACK_NOT_FOUND
        /// </summary>
        AsvRsgaRequestAckNotFound = 4,
    }
    public static class AsvRsgaRequestAckHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
            yield return converter(4);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_RSGA_REQUEST_ACK_OK");
            yield return new EnumValue<T>(converter(1),"ASV_RSGA_REQUEST_ACK_IN_PROGRESS");
            yield return new EnumValue<T>(converter(2),"ASV_RSGA_REQUEST_ACK_FAIL");
            yield return new EnumValue<T>(converter(3),"ASV_RSGA_REQUEST_ACK_NOT_SUPPORTED");
            yield return new EnumValue<T>(converter(4),"ASV_RSGA_REQUEST_ACK_NOT_FOUND");
        }
    }
    /// <summary>
    /// Common status flags for all ASV_RSGA_RTT_* data.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RSGA_DATA_FLAGS
    /// </summary>
    [Flags]
    public enum AsvRsgaDataFlags : ulong
    {
        /// <summary>
        /// Is data valid.
        /// ASV_RSGA_DATA_FLAGS_VALID
        /// </summary>
        AsvRsgaDataFlagsValid = 1,
    }
    public static class AsvRsgaDataFlagsHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_RSGA_DATA_FLAGS_VALID");
        }
    }
    /// <summary>
    /// Common status flags for all ASV_RSGA_RTT_GNSS message.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RSGA_RTT_GNSS_FLAGS
    /// </summary>
    [Flags]
    public enum AsvRsgaRttGnssFlags : ulong
    {
        /// <summary>
        /// This flag is set when the vehicle is known to be on the ground.
        /// ASV_RSGA_RTT_GNSS_FLAGS_ON_THE_GROUND
        /// </summary>
        AsvRsgaRttGnssFlagsOnTheGround = 1,
        /// <summary>
        /// Reserved .
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED1
        /// </summary>
        AsvRsgaRttGnssFlagsReserved1 = 2,
        /// <summary>
        /// Reserved.
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED2
        /// </summary>
        AsvRsgaRttGnssFlagsReserved2 = 4,
        /// <summary>
        /// Reserved.
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED3
        /// </summary>
        AsvRsgaRttGnssFlagsReserved3 = 8,
        /// <summary>
        /// Reserved.
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED4
        /// </summary>
        AsvRsgaRttGnssFlagsReserved4 = 16,
        /// <summary>
        /// Reserved.
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED5
        /// </summary>
        AsvRsgaRttGnssFlagsReserved5 = 32,
        /// <summary>
        /// Reserved.
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED6
        /// </summary>
        AsvRsgaRttGnssFlagsReserved6 = 64,
        /// <summary>
        /// Reserved.
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED7
        /// </summary>
        AsvRsgaRttGnssFlagsReserved7 = 128,
    }
    public static class AsvRsgaRttGnssFlagsHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(4);
            yield return converter(8);
            yield return converter(16);
            yield return converter(32);
            yield return converter(64);
            yield return converter(128);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_RSGA_RTT_GNSS_FLAGS_ON_THE_GROUND");
            yield return new EnumValue<T>(converter(2),"ASV_RSGA_RTT_GNSS_FLAGS_RESERVED1");
            yield return new EnumValue<T>(converter(4),"ASV_RSGA_RTT_GNSS_FLAGS_RESERVED2");
            yield return new EnumValue<T>(converter(8),"ASV_RSGA_RTT_GNSS_FLAGS_RESERVED3");
            yield return new EnumValue<T>(converter(16),"ASV_RSGA_RTT_GNSS_FLAGS_RESERVED4");
            yield return new EnumValue<T>(converter(32),"ASV_RSGA_RTT_GNSS_FLAGS_RESERVED5");
            yield return new EnumValue<T>(converter(64),"ASV_RSGA_RTT_GNSS_FLAGS_RESERVED6");
            yield return new EnumValue<T>(converter(128),"ASV_RSGA_RTT_GNSS_FLAGS_RESERVED7");
        }
    }
    /// <summary>
    /// Type of GNSS receiver
    ///  ASV_RSGA_RTT_GNSS_TYPE
    /// </summary>
    public enum AsvRsgaRttGnssType : ulong
    {
        /// <summary>
        /// Virtual GNSS data.
        /// ASV_RSGA_RTT_GNSS_TYPE_VIRTUAL
        /// </summary>
        AsvRsgaRttGnssTypeVirtual = 0,
        /// <summary>
        /// GNSS data from receiver.
        /// ASV_RSGA_RTT_GNSS_TYPE_NMEA
        /// </summary>
        AsvRsgaRttGnssTypeNmea = 1,
        /// <summary>
        /// GNSS data from UAV.
        /// ASV_RSGA_RTT_GNSS_TYPE_UAV
        /// </summary>
        AsvRsgaRttGnssTypeUav = 2,
    }
    public static class AsvRsgaRttGnssTypeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_RSGA_RTT_GNSS_TYPE_VIRTUAL");
            yield return new EnumValue<T>(converter(1),"ASV_RSGA_RTT_GNSS_TYPE_NMEA");
            yield return new EnumValue<T>(converter(2),"ASV_RSGA_RTT_GNSS_TYPE_UAV");
        }
    }
    /// <summary>
    /// Mode S interrogation or uplink formats.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RSGA_RTT_ADSB_MSG_UF
    /// </summary>
    [Flags]
    public enum AsvRsgaRttAdsbMsgUf : ulong
    {
        /// <summary>
        /// [UF00] Short air-air surveillance (ACAS).
        /// ASV_RSGA_RTT_ADSB_MSG_UF_00
        /// </summary>
        AsvRsgaRttAdsbMsgUf00 = 1,
        /// <summary>
        /// [UF01] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_01
        /// </summary>
        AsvRsgaRttAdsbMsgUf01 = 2,
        /// <summary>
        /// [UF02] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_02
        /// </summary>
        AsvRsgaRttAdsbMsgUf02 = 4,
        /// <summary>
        /// [UF03] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_03
        /// </summary>
        AsvRsgaRttAdsbMsgUf03 = 8,
        /// <summary>
        /// [UF04] Surveillance, altitude request.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_04
        /// </summary>
        AsvRsgaRttAdsbMsgUf04 = 16,
        /// <summary>
        /// [UF05] Surveillance, identify request.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_05
        /// </summary>
        AsvRsgaRttAdsbMsgUf05 = 32,
        /// <summary>
        /// [UF06] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_06
        /// </summary>
        AsvRsgaRttAdsbMsgUf06 = 64,
        /// <summary>
        /// [UF07] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_07
        /// </summary>
        AsvRsgaRttAdsbMsgUf07 = 128,
        /// <summary>
        /// [UF08] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_08
        /// </summary>
        AsvRsgaRttAdsbMsgUf08 = 256,
        /// <summary>
        /// [UF09] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_09
        /// </summary>
        AsvRsgaRttAdsbMsgUf09 = 512,
        /// <summary>
        /// [UF10] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_10
        /// </summary>
        AsvRsgaRttAdsbMsgUf10 = 1024,
        /// <summary>
        /// [UF11] Mode S only all-call.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_11
        /// </summary>
        AsvRsgaRttAdsbMsgUf11 = 2048,
        /// <summary>
        /// [UF12] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_12
        /// </summary>
        AsvRsgaRttAdsbMsgUf12 = 4096,
        /// <summary>
        /// [UF13] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_13
        /// </summary>
        AsvRsgaRttAdsbMsgUf13 = 8192,
        /// <summary>
        /// [UF14] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_14
        /// </summary>
        AsvRsgaRttAdsbMsgUf14 = 16384,
        /// <summary>
        /// [UF15] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_15
        /// </summary>
        AsvRsgaRttAdsbMsgUf15 = 32768,
        /// <summary>
        /// [UF16] Long air-air surveillance (ACAS).
        /// ASV_RSGA_RTT_ADSB_MSG_UF_16
        /// </summary>
        AsvRsgaRttAdsbMsgUf16 = 65536,
        /// <summary>
        /// [UF17] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_17
        /// </summary>
        AsvRsgaRttAdsbMsgUf17 = 131072,
        /// <summary>
        /// [UF18] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_18
        /// </summary>
        AsvRsgaRttAdsbMsgUf18 = 262144,
        /// <summary>
        /// [UF19] Reserved for military use.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_19
        /// </summary>
        AsvRsgaRttAdsbMsgUf19 = 524288,
        /// <summary>
        /// [UF20] Comm-A, altitude request.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_20
        /// </summary>
        AsvRsgaRttAdsbMsgUf20 = 1048576,
        /// <summary>
        /// [UF21] Comm-A, identify request.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_21
        /// </summary>
        AsvRsgaRttAdsbMsgUf21 = 2097152,
        /// <summary>
        /// [UF22] Reserved for military use.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_22
        /// </summary>
        AsvRsgaRttAdsbMsgUf22 = 4194304,
        /// <summary>
        /// [UF23] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_UF_23
        /// </summary>
        AsvRsgaRttAdsbMsgUf23 = 8388608,
        /// <summary>
        /// [UF24] Comm-C (ELM).
        /// ASV_RSGA_RTT_ADSB_MSG_UF_24
        /// </summary>
        AsvRsgaRttAdsbMsgUf24 = 16777216,
    }
    public static class AsvRsgaRttAdsbMsgUfHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(4);
            yield return converter(8);
            yield return converter(16);
            yield return converter(32);
            yield return converter(64);
            yield return converter(128);
            yield return converter(256);
            yield return converter(512);
            yield return converter(1024);
            yield return converter(2048);
            yield return converter(4096);
            yield return converter(8192);
            yield return converter(16384);
            yield return converter(32768);
            yield return converter(65536);
            yield return converter(131072);
            yield return converter(262144);
            yield return converter(524288);
            yield return converter(1048576);
            yield return converter(2097152);
            yield return converter(4194304);
            yield return converter(8388608);
            yield return converter(16777216);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_RSGA_RTT_ADSB_MSG_UF_00");
            yield return new EnumValue<T>(converter(2),"ASV_RSGA_RTT_ADSB_MSG_UF_01");
            yield return new EnumValue<T>(converter(4),"ASV_RSGA_RTT_ADSB_MSG_UF_02");
            yield return new EnumValue<T>(converter(8),"ASV_RSGA_RTT_ADSB_MSG_UF_03");
            yield return new EnumValue<T>(converter(16),"ASV_RSGA_RTT_ADSB_MSG_UF_04");
            yield return new EnumValue<T>(converter(32),"ASV_RSGA_RTT_ADSB_MSG_UF_05");
            yield return new EnumValue<T>(converter(64),"ASV_RSGA_RTT_ADSB_MSG_UF_06");
            yield return new EnumValue<T>(converter(128),"ASV_RSGA_RTT_ADSB_MSG_UF_07");
            yield return new EnumValue<T>(converter(256),"ASV_RSGA_RTT_ADSB_MSG_UF_08");
            yield return new EnumValue<T>(converter(512),"ASV_RSGA_RTT_ADSB_MSG_UF_09");
            yield return new EnumValue<T>(converter(1024),"ASV_RSGA_RTT_ADSB_MSG_UF_10");
            yield return new EnumValue<T>(converter(2048),"ASV_RSGA_RTT_ADSB_MSG_UF_11");
            yield return new EnumValue<T>(converter(4096),"ASV_RSGA_RTT_ADSB_MSG_UF_12");
            yield return new EnumValue<T>(converter(8192),"ASV_RSGA_RTT_ADSB_MSG_UF_13");
            yield return new EnumValue<T>(converter(16384),"ASV_RSGA_RTT_ADSB_MSG_UF_14");
            yield return new EnumValue<T>(converter(32768),"ASV_RSGA_RTT_ADSB_MSG_UF_15");
            yield return new EnumValue<T>(converter(65536),"ASV_RSGA_RTT_ADSB_MSG_UF_16");
            yield return new EnumValue<T>(converter(131072),"ASV_RSGA_RTT_ADSB_MSG_UF_17");
            yield return new EnumValue<T>(converter(262144),"ASV_RSGA_RTT_ADSB_MSG_UF_18");
            yield return new EnumValue<T>(converter(524288),"ASV_RSGA_RTT_ADSB_MSG_UF_19");
            yield return new EnumValue<T>(converter(1048576),"ASV_RSGA_RTT_ADSB_MSG_UF_20");
            yield return new EnumValue<T>(converter(2097152),"ASV_RSGA_RTT_ADSB_MSG_UF_21");
            yield return new EnumValue<T>(converter(4194304),"ASV_RSGA_RTT_ADSB_MSG_UF_22");
            yield return new EnumValue<T>(converter(8388608),"ASV_RSGA_RTT_ADSB_MSG_UF_23");
            yield return new EnumValue<T>(converter(16777216),"ASV_RSGA_RTT_ADSB_MSG_UF_24");
        }
    }
    /// <summary>
    /// Mode S reply or downlink formats.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RSGA_RTT_ADSB_MSG_DF
    /// </summary>
    [Flags]
    public enum AsvRsgaRttAdsbMsgDf : ulong
    {
        /// <summary>
        /// [DF00] Short air-air surveillance (ACAS).
        /// ASV_RSGA_RTT_ADSB_MSG_DF_00
        /// </summary>
        AsvRsgaRttAdsbMsgDf00 = 1,
        /// <summary>
        /// [DF01] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_01
        /// </summary>
        AsvRsgaRttAdsbMsgDf01 = 2,
        /// <summary>
        /// [DF02] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_02
        /// </summary>
        AsvRsgaRttAdsbMsgDf02 = 4,
        /// <summary>
        /// [DF03] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_03
        /// </summary>
        AsvRsgaRttAdsbMsgDf03 = 8,
        /// <summary>
        /// [DF04] Surveillance, altitude reply.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_04
        /// </summary>
        AsvRsgaRttAdsbMsgDf04 = 16,
        /// <summary>
        /// [DF05] Surveillance, identify reply.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_05
        /// </summary>
        AsvRsgaRttAdsbMsgDf05 = 32,
        /// <summary>
        /// [DF06] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_06
        /// </summary>
        AsvRsgaRttAdsbMsgDf06 = 64,
        /// <summary>
        /// [DF07] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_07
        /// </summary>
        AsvRsgaRttAdsbMsgDf07 = 128,
        /// <summary>
        /// [DF08] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_08
        /// </summary>
        AsvRsgaRttAdsbMsgDf08 = 256,
        /// <summary>
        /// [DF09] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_09
        /// </summary>
        AsvRsgaRttAdsbMsgDf09 = 512,
        /// <summary>
        /// [DF10] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_10
        /// </summary>
        AsvRsgaRttAdsbMsgDf10 = 1024,
        /// <summary>
        /// [DF11] All-call reply.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_11
        /// </summary>
        AsvRsgaRttAdsbMsgDf11 = 2048,
        /// <summary>
        /// [DF12] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_12
        /// </summary>
        AsvRsgaRttAdsbMsgDf12 = 4096,
        /// <summary>
        /// [DF13] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_13
        /// </summary>
        AsvRsgaRttAdsbMsgDf13 = 8192,
        /// <summary>
        /// [DF14] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_14
        /// </summary>
        AsvRsgaRttAdsbMsgDf14 = 16384,
        /// <summary>
        /// [DF15] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_15
        /// </summary>
        AsvRsgaRttAdsbMsgDf15 = 32768,
        /// <summary>
        /// [DF16] Long air-air surveillance (ACAS).
        /// ASV_RSGA_RTT_ADSB_MSG_DF_16
        /// </summary>
        AsvRsgaRttAdsbMsgDf16 = 65536,
        /// <summary>
        /// [DF17] Extended squitter.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_17
        /// </summary>
        AsvRsgaRttAdsbMsgDf17 = 131072,
        /// <summary>
        /// [DF18] Extended squitter/non transponder.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_18
        /// </summary>
        AsvRsgaRttAdsbMsgDf18 = 262144,
        /// <summary>
        /// [DF19] Military extended squitter.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_19
        /// </summary>
        AsvRsgaRttAdsbMsgDf19 = 524288,
        /// <summary>
        /// [DF20] Comm-B, altitude reply.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_20
        /// </summary>
        AsvRsgaRttAdsbMsgDf20 = 1048576,
        /// <summary>
        /// [DF21] Comm-B, identify reply.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_21
        /// </summary>
        AsvRsgaRttAdsbMsgDf21 = 2097152,
        /// <summary>
        /// [DF22] Reserved for military use.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_22
        /// </summary>
        AsvRsgaRttAdsbMsgDf22 = 4194304,
        /// <summary>
        /// [DF23] Reserved.
        /// ASV_RSGA_RTT_ADSB_MSG_DF_23
        /// </summary>
        AsvRsgaRttAdsbMsgDf23 = 8388608,
        /// <summary>
        /// [DF24] Comm-D (ELM).
        /// ASV_RSGA_RTT_ADSB_MSG_DF_24
        /// </summary>
        AsvRsgaRttAdsbMsgDf24 = 16777216,
    }
    public static class AsvRsgaRttAdsbMsgDfHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(4);
            yield return converter(8);
            yield return converter(16);
            yield return converter(32);
            yield return converter(64);
            yield return converter(128);
            yield return converter(256);
            yield return converter(512);
            yield return converter(1024);
            yield return converter(2048);
            yield return converter(4096);
            yield return converter(8192);
            yield return converter(16384);
            yield return converter(32768);
            yield return converter(65536);
            yield return converter(131072);
            yield return converter(262144);
            yield return converter(524288);
            yield return converter(1048576);
            yield return converter(2097152);
            yield return converter(4194304);
            yield return converter(8388608);
            yield return converter(16777216);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_RSGA_RTT_ADSB_MSG_DF_00");
            yield return new EnumValue<T>(converter(2),"ASV_RSGA_RTT_ADSB_MSG_DF_01");
            yield return new EnumValue<T>(converter(4),"ASV_RSGA_RTT_ADSB_MSG_DF_02");
            yield return new EnumValue<T>(converter(8),"ASV_RSGA_RTT_ADSB_MSG_DF_03");
            yield return new EnumValue<T>(converter(16),"ASV_RSGA_RTT_ADSB_MSG_DF_04");
            yield return new EnumValue<T>(converter(32),"ASV_RSGA_RTT_ADSB_MSG_DF_05");
            yield return new EnumValue<T>(converter(64),"ASV_RSGA_RTT_ADSB_MSG_DF_06");
            yield return new EnumValue<T>(converter(128),"ASV_RSGA_RTT_ADSB_MSG_DF_07");
            yield return new EnumValue<T>(converter(256),"ASV_RSGA_RTT_ADSB_MSG_DF_08");
            yield return new EnumValue<T>(converter(512),"ASV_RSGA_RTT_ADSB_MSG_DF_09");
            yield return new EnumValue<T>(converter(1024),"ASV_RSGA_RTT_ADSB_MSG_DF_10");
            yield return new EnumValue<T>(converter(2048),"ASV_RSGA_RTT_ADSB_MSG_DF_11");
            yield return new EnumValue<T>(converter(4096),"ASV_RSGA_RTT_ADSB_MSG_DF_12");
            yield return new EnumValue<T>(converter(8192),"ASV_RSGA_RTT_ADSB_MSG_DF_13");
            yield return new EnumValue<T>(converter(16384),"ASV_RSGA_RTT_ADSB_MSG_DF_14");
            yield return new EnumValue<T>(converter(32768),"ASV_RSGA_RTT_ADSB_MSG_DF_15");
            yield return new EnumValue<T>(converter(65536),"ASV_RSGA_RTT_ADSB_MSG_DF_16");
            yield return new EnumValue<T>(converter(131072),"ASV_RSGA_RTT_ADSB_MSG_DF_17");
            yield return new EnumValue<T>(converter(262144),"ASV_RSGA_RTT_ADSB_MSG_DF_18");
            yield return new EnumValue<T>(converter(524288),"ASV_RSGA_RTT_ADSB_MSG_DF_19");
            yield return new EnumValue<T>(converter(1048576),"ASV_RSGA_RTT_ADSB_MSG_DF_20");
            yield return new EnumValue<T>(converter(2097152),"ASV_RSGA_RTT_ADSB_MSG_DF_21");
            yield return new EnumValue<T>(converter(4194304),"ASV_RSGA_RTT_ADSB_MSG_DF_22");
            yield return new EnumValue<T>(converter(8388608),"ASV_RSGA_RTT_ADSB_MSG_DF_23");
            yield return new EnumValue<T>(converter(16777216),"ASV_RSGA_RTT_ADSB_MSG_DF_24");
        }
    }
    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd : ulong
    {
        /// <summary>
        /// Set the operational mode.
        /// Param 1 - Mode (uint32_t, see ASV_RSGA_CUSTOM_MODE).
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_RSGA_SET_MODE
        /// </summary>
        MavCmdAsvRsgaSetMode = 13400,
        /// <summary>
        /// Start data recording with a unique name (maximum 28 characters). Can be used in mission protocol for RSGA payloads.
        /// Param 1 - Record unique name: 0-3 chars (char[4]).
        /// Param 2 - Record unique name: 4-7 chars (char[4]).
        /// Param 3 - Record unique name: 8-11 chars (char[4]).
        /// Param 4 - Record unique name: 12-15 chars (char[4]).
        /// Param 5 - Record unique name: 16-19 chars (char[4]).
        /// Param 6 - Record unique name: 20-23 chars (char[4]).
        /// Param 7 - Record unique name: 24-27 chars (char[4]).
        /// MAV_CMD_ASV_RSGA_START_RECORD
        /// </summary>
        MavCmdAsvRsgaStartRecord = 13401,
        /// <summary>
        /// Stop recording data. Can be used in the mission protocol for RSGA payloads.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_RSGA_STOP_RECORD
        /// </summary>
        MavCmdAsvRsgaStopRecord = 13402,
    }
    public static class MavCmdHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(13400);
            yield return converter(13401);
            yield return converter(13402);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(13400),"MAV_CMD_ASV_RSGA_SET_MODE");
            yield return new EnumValue<T>(converter(13401),"MAV_CMD_ASV_RSGA_START_RECORD");
            yield return new EnumValue<T>(converter(13402),"MAV_CMD_ASV_RSGA_STOP_RECORD");
        }
    }

#endregion

#region Messages

    /// <summary>
    /// Requests device COMPATIBILITY. Returns ASV_RSGA_COMPATIBILITY_RESPONSE. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_COMPATIBILITY_REQUEST
    /// </summary>
    public class AsvRsgaCompatibilityRequestPacket : MavlinkV2Message<AsvRsgaCompatibilityRequestPayload>
    {
        public const int MessageId = 13400;
        
        public const byte CrcExtra = 16;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaCompatibilityRequestPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_COMPATIBILITY_REQUEST";
    }

    /// <summary>
    ///  ASV_RSGA_COMPATIBILITY_REQUEST
    /// </summary>
    public class AsvRsgaCompatibilityRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 4; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 4; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 4 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    

        }

        /// <summary>
        /// Specifies a unique number for this request. This allows the response packet to be identified.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies a unique number for this request. This allows the response packet to be identified.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _requestId;
        public ushort RequestId { get => _requestId; set => _requestId = value; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetSystem;
        public byte TargetSystem { get => _targetSystem; set => _targetSystem = value; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetComponent;
        public byte TargetComponent { get => _targetComponent; set => _targetComponent = value; }
    }
    /// <summary>
    /// Responds to the ASV_RSGA_COMPATIBILITY_REQUEST. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_COMPATIBILITY_RESPONSE
    /// </summary>
    public class AsvRsgaCompatibilityResponsePacket : MavlinkV2Message<AsvRsgaCompatibilityResponsePayload>
    {
        public const int MessageId = 13401;
        
        public const byte CrcExtra = 196;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaCompatibilityResponsePayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_COMPATIBILITY_RESPONSE";
    }

    /// <summary>
    ///  ASV_RSGA_COMPATIBILITY_RESPONSE
    /// </summary>
    public class AsvRsgaCompatibilityResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 35; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 35; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            + 1 // uint8_t result
            +SupportedModes.Length // uint8_t[32] supported_modes
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvRsgaRequestAck)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/35 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                SupportedModes[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            for(var i=0;i<SupportedModes.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)SupportedModes[i]);
            }
            /* PayloadByteSize = 35 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ResultField.DataType, ref tmpResult);
            Result = (AsvRsgaRequestAck)tmpResult;
            ArrayType.Accept(visitor,SupportedModesField, SupportedModesField.DataType, 32,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref SupportedModes[index]));    

        }

        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request. This allows the response to be matched to the correct request.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _requestId;
        public ushort RequestId { get => _requestId; set => _requestId = value; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .DataType(new UInt8Type(AsvRsgaRequestAckHelper.GetValues(x=>(byte)x).Min(),AsvRsgaRequestAckHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvRsgaRequestAckHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvRsgaRequestAck _result;
        public AsvRsgaRequestAck Result { get => _result; set => _result = value; } 
        /// <summary>
        /// Supported modes. Each bit index represents an ASV_RSGA_CUSTOM_MODE value (256 bits). First (IDLE) bit always true.
        /// OriginName: supported_modes, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SupportedModesField = new Field.Builder()
            .Name(nameof(SupportedModes))
            .Title("supported_modes")
            .Description("Supported modes. Each bit index represents an ASV_RSGA_CUSTOM_MODE value (256 bits). First (IDLE) bit always true.")

            .DataType(new ArrayType(UInt8Type.Default,32))
        .Build();
        public const int SupportedModesMaxItemsCount = 32;
        public byte[] SupportedModes { get; } = new byte[32];
        [Obsolete("This method is deprecated. Use GetSupportedModesMaxItemsCount instead.")]
        public byte GetSupportedModesMaxItemsCount() => 32;
    }
    /// <summary>
    /// Global position from GNSS receiver. Can be transmitted for all supported modes. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_GNSS
    /// </summary>
    public class AsvRsgaRttGnssPacket : MavlinkV2Message<AsvRsgaRttGnssPayload>
    {
        public const int MessageId = 13450;
        
        public const byte CrcExtra = 15;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttGnssPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_GNSS";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_GNSS
    /// </summary>
    public class AsvRsgaRttGnssPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 64; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 64; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t data_index
            +4 // int32_t lat
            +4 // int32_t lat_err
            +4 // int32_t lon
            +4 // int32_t lon_err
            +4 // int32_t alt_msl
            +4 // int32_t alt_wgs
            +4 // int32_t alt_err
            +2 // uint16_t ref_id
            +2 // uint16_t hdop
            +2 // uint16_t vdop
            +2 // uint16_t sog
            +2 // uint16_t cog_true
            +2 // uint16_t cog_mag
            + 1 // uint8_t receiver_type
            + 1 // uint8_t gnss_flags
            +1 // uint8_t sat_cnt
            + 1 // uint8_t fix_type
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            DataIndex = BinSerialize.ReadUInt(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            LatErr = BinSerialize.ReadInt(ref buffer);
            Lon = BinSerialize.ReadInt(ref buffer);
            LonErr = BinSerialize.ReadInt(ref buffer);
            AltMsl = BinSerialize.ReadInt(ref buffer);
            AltWgs = BinSerialize.ReadInt(ref buffer);
            AltErr = BinSerialize.ReadInt(ref buffer);
            RefId = BinSerialize.ReadUShort(ref buffer);
            Hdop = BinSerialize.ReadUShort(ref buffer);
            Vdop = BinSerialize.ReadUShort(ref buffer);
            Sog = BinSerialize.ReadUShort(ref buffer);
            CogTrue = BinSerialize.ReadUShort(ref buffer);
            CogMag = BinSerialize.ReadUShort(ref buffer);
            ReceiverType = (AsvRsgaRttGnssType)BinSerialize.ReadByte(ref buffer);
            GnssFlags = (AsvRsgaRttGnssFlags)BinSerialize.ReadByte(ref buffer);
            SatCnt = (byte)BinSerialize.ReadByte(ref buffer);
            FixType = (GpsFixType)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,DataIndex);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,LatErr);
            BinSerialize.WriteInt(ref buffer,Lon);
            BinSerialize.WriteInt(ref buffer,LonErr);
            BinSerialize.WriteInt(ref buffer,AltMsl);
            BinSerialize.WriteInt(ref buffer,AltWgs);
            BinSerialize.WriteInt(ref buffer,AltErr);
            BinSerialize.WriteUShort(ref buffer,RefId);
            BinSerialize.WriteUShort(ref buffer,Hdop);
            BinSerialize.WriteUShort(ref buffer,Vdop);
            BinSerialize.WriteUShort(ref buffer,Sog);
            BinSerialize.WriteUShort(ref buffer,CogTrue);
            BinSerialize.WriteUShort(ref buffer,CogMag);
            BinSerialize.WriteByte(ref buffer,(byte)ReceiverType);
            BinSerialize.WriteByte(ref buffer,(byte)GnssFlags);
            BinSerialize.WriteByte(ref buffer,(byte)SatCnt);
            BinSerialize.WriteByte(ref buffer,(byte)FixType);
            /* PayloadByteSize = 64 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,DataIndexField, DataIndexField.DataType, ref _dataIndex);    
            Int32Type.Accept(visitor,LatField, LatField.DataType, ref _lat);    
            Int32Type.Accept(visitor,LatErrField, LatErrField.DataType, ref _latErr);    
            Int32Type.Accept(visitor,LonField, LonField.DataType, ref _lon);    
            Int32Type.Accept(visitor,LonErrField, LonErrField.DataType, ref _lonErr);    
            Int32Type.Accept(visitor,AltMslField, AltMslField.DataType, ref _altMsl);    
            Int32Type.Accept(visitor,AltWgsField, AltWgsField.DataType, ref _altWgs);    
            Int32Type.Accept(visitor,AltErrField, AltErrField.DataType, ref _altErr);    
            UInt16Type.Accept(visitor,RefIdField, RefIdField.DataType, ref _refId);    
            UInt16Type.Accept(visitor,HdopField, HdopField.DataType, ref _hdop);    
            UInt16Type.Accept(visitor,VdopField, VdopField.DataType, ref _vdop);    
            UInt16Type.Accept(visitor,SogField, SogField.DataType, ref _sog);    
            UInt16Type.Accept(visitor,CogTrueField, CogTrueField.DataType, ref _cogTrue);    
            UInt16Type.Accept(visitor,CogMagField, CogMagField.DataType, ref _cogMag);    
            var tmpReceiverType = (byte)ReceiverType;
            UInt8Type.Accept(visitor,ReceiverTypeField, ReceiverTypeField.DataType, ref tmpReceiverType);
            ReceiverType = (AsvRsgaRttGnssType)tmpReceiverType;
            var tmpGnssFlags = (byte)GnssFlags;
            UInt8Type.Accept(visitor,GnssFlagsField, GnssFlagsField.DataType, ref tmpGnssFlags);
            GnssFlags = (AsvRsgaRttGnssFlags)tmpGnssFlags;
            UInt8Type.Accept(visitor,SatCntField, SatCntField.DataType, ref _satCnt);    
            var tmpFixType = (byte)FixType;
            UInt8Type.Accept(visitor,FixTypeField, FixTypeField.DataType, ref tmpFixType);
            FixType = (GpsFixType)tmpFixType;

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: data_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataIndexField = new Field.Builder()
            .Name(nameof(DataIndex))
            .Title("data_index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _dataIndex;
        public uint DataIndex { get => _dataIndex; set => _dataIndex = value; }
        /// <summary>
        /// Latitude (WGS84, EGM96 ellipsoid)
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Latitude (WGS84, EGM96 ellipsoid)")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _lat;
        public int Lat { get => _lat; set => _lat = value; }
        /// <summary>
        /// Expected Error in Latitude (North) Direction
        /// OriginName: lat_err, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field LatErrField = new Field.Builder()
            .Name(nameof(LatErr))
            .Title("lat_err")
            .Description("Expected Error in Latitude (North) Direction")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _latErr;
        public int LatErr { get => _latErr; set => _latErr = value; }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LonField = new Field.Builder()
            .Name(nameof(Lon))
            .Title("lon")
            .Description("Longitude (WGS84, EGM96 ellipsoid)")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _lon;
        public int Lon { get => _lon; set => _lon = value; }
        /// <summary>
        /// Expected Error in Longitude (East) Direction
        /// OriginName: lon_err, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field LonErrField = new Field.Builder()
            .Name(nameof(LonErr))
            .Title("lon_err")
            .Description("Expected Error in Longitude (East) Direction")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _lonErr;
        public int LonErr { get => _lonErr; set => _lonErr = value; }
        /// <summary>
        /// Antenna altitude above/below mean sea level (geoid)
        /// OriginName: alt_msl, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltMslField = new Field.Builder()
            .Name(nameof(AltMsl))
            .Title("alt_msl")
            .Description("Antenna altitude above/below mean sea level (geoid)")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _altMsl;
        public int AltMsl { get => _altMsl; set => _altMsl = value; }
        /// <summary>
        /// Antenna altitude WGS-84 earth ellipsoid
        /// OriginName: alt_wgs, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltWgsField = new Field.Builder()
            .Name(nameof(AltWgs))
            .Title("alt_wgs")
            .Description("Antenna altitude WGS-84 earth ellipsoid")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _altWgs;
        public int AltWgs { get => _altWgs; set => _altWgs = value; }
        /// <summary>
        /// Expected Error in Altitude
        /// OriginName: alt_err, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltErrField = new Field.Builder()
            .Name(nameof(AltErr))
            .Title("alt_err")
            .Description("Expected Error in Altitude")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _altErr;
        public int AltErr { get => _altErr; set => _altErr = value; }
        /// <summary>
        /// GNSS reference station ID (used when GNSS is received from multiple sources).
        /// OriginName: ref_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RefIdField = new Field.Builder()
            .Name(nameof(RefId))
            .Title("ref_id")
            .Description("GNSS reference station ID (used when GNSS is received from multiple sources).")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _refId;
        public ushort RefId { get => _refId; set => _refId = value; }
        /// <summary>
        /// HDOP horizontal dilution of position
        /// OriginName: hdop, Units: , IsExtended: false
        /// </summary>
        public static readonly Field HdopField = new Field.Builder()
            .Name(nameof(Hdop))
            .Title("hdop")
            .Description("HDOP horizontal dilution of position")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _hdop;
        public ushort Hdop { get => _hdop; set => _hdop = value; }
        /// <summary>
        /// VDOP vertical dilution of position
        /// OriginName: vdop, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VdopField = new Field.Builder()
            .Name(nameof(Vdop))
            .Title("vdop")
            .Description("VDOP vertical dilution of position")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _vdop;
        public ushort Vdop { get => _vdop; set => _vdop = value; }
        /// <summary>
        /// Speed over ground
        /// OriginName: sog, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field SogField = new Field.Builder()
            .Name(nameof(Sog))
            .Title("sog")
            .Description("Speed over ground")
.Units(@"cm/s")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _sog;
        public ushort Sog { get => _sog; set => _sog = value; }
        /// <summary>
        /// Course over ground (true) (yaw angle). 0.0..359.99 degrees
        /// OriginName: cog_true, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field CogTrueField = new Field.Builder()
            .Name(nameof(CogTrue))
            .Title("cog_true")
            .Description("Course over ground (true) (yaw angle). 0.0..359.99 degrees")
.Units(@"cdeg")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _cogTrue;
        public ushort CogTrue { get => _cogTrue; set => _cogTrue = value; }
        /// <summary>
        /// Course over ground (magnetic) (yaw angle). 0.0..359.99 degrees
        /// OriginName: cog_mag, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field CogMagField = new Field.Builder()
            .Name(nameof(CogMag))
            .Title("cog_mag")
            .Description("Course over ground (magnetic) (yaw angle). 0.0..359.99 degrees")
.Units(@"cdeg")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _cogMag;
        public ushort CogMag { get => _cogMag; set => _cogMag = value; }
        /// <summary>
        /// GNSS receiver type.
        /// OriginName: receiver_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ReceiverTypeField = new Field.Builder()
            .Name(nameof(ReceiverType))
            .Title("receiver_type")
            .Description("GNSS receiver type.")
            .DataType(new UInt8Type(AsvRsgaRttGnssTypeHelper.GetValues(x=>(byte)x).Min(),AsvRsgaRttGnssTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvRsgaRttGnssTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvRsgaRttGnssType _receiverType;
        public AsvRsgaRttGnssType ReceiverType { get => _receiverType; set => _receiverType = value; } 
        /// <summary>
        /// GNSS special flags.
        /// OriginName: gnss_flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssFlagsField = new Field.Builder()
            .Name(nameof(GnssFlags))
            .Title("gnss_flags")
            .Description("GNSS special flags.")
            .DataType(new UInt8Type(AsvRsgaRttGnssFlagsHelper.GetValues(x=>(byte)x).Min(),AsvRsgaRttGnssFlagsHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvRsgaRttGnssFlagsHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvRsgaRttGnssFlags _gnssFlags;
        public AsvRsgaRttGnssFlags GnssFlags { get => _gnssFlags; set => _gnssFlags = value; } 
        /// <summary>
        /// Number of satellites in view
        /// OriginName: sat_cnt, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SatCntField = new Field.Builder()
            .Name(nameof(SatCnt))
            .Title("sat_cnt")
            .Description("Number of satellites in view")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _satCnt;
        public byte SatCnt { get => _satCnt; set => _satCnt = value; }
        /// <summary>
        /// GNSS fix type
        /// OriginName: fix_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FixTypeField = new Field.Builder()
            .Name(nameof(FixType))
            .Title("fix_type")
            .Description("GNSS fix type")
            .DataType(new UInt8Type(GpsFixTypeHelper.GetValues(x=>(byte)x).Min(),GpsFixTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(GpsFixTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private GpsFixType _fixType;
        public GpsFixType FixType { get => _fixType; set => _fixType = value; } 
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_SPECTRUM mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_SPECTRUM
    /// </summary>
    public class AsvRsgaRttSpectrumPacket : MavlinkV2Message<AsvRsgaRttSpectrumPayload>
    {
        public const int MessageId = 13451;
        
        public const byte CrcExtra = 255;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttSpectrumPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_SPECTRUM";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_SPECTRUM
    /// </summary>
    public class AsvRsgaRttSpectrumPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_TX_LLZ mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_TX_LLZ
    /// </summary>
    public class AsvRsgaRttTxLlzPacket : MavlinkV2Message<AsvRsgaRttTxLlzPayload>
    {
        public const int MessageId = 13452;
        
        public const byte CrcExtra = 184;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttTxLlzPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_TX_LLZ";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_TX_LLZ
    /// </summary>
    public class AsvRsgaRttTxLlzPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_TX_GP mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_TX_GP
    /// </summary>
    public class AsvRsgaRttTxGpPacket : MavlinkV2Message<AsvRsgaRttTxGpPayload>
    {
        public const int MessageId = 13453;
        
        public const byte CrcExtra = 219;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttTxGpPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_TX_GP";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_TX_GP
    /// </summary>
    public class AsvRsgaRttTxGpPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_TX_VOR mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_TX_VOR
    /// </summary>
    public class AsvRsgaRttTxVorPacket : MavlinkV2Message<AsvRsgaRttTxVorPayload>
    {
        public const int MessageId = 13454;
        
        public const byte CrcExtra = 29;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttTxVorPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_TX_VOR";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_TX_VOR
    /// </summary>
    public class AsvRsgaRttTxVorPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_TX_MARKER mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_TX_MARKER
    /// </summary>
    public class AsvRsgaRttTxMarkerPacket : MavlinkV2Message<AsvRsgaRttTxMarkerPayload>
    {
        public const int MessageId = 13455;
        
        public const byte CrcExtra = 69;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttTxMarkerPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_TX_MARKER";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_TX_MARKER
    /// </summary>
    public class AsvRsgaRttTxMarkerPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_DME_REP mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_DME_REP
    /// </summary>
    public class AsvRsgaRttDmeRepPacket : MavlinkV2Message<AsvRsgaRttDmeRepPayload>
    {
        public const int MessageId = 13456;
        
        public const byte CrcExtra = 44;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttDmeRepPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_DME_REP";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_DME_REP
    /// </summary>
    public class AsvRsgaRttDmeRepPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_TX_GBAS mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_TX_GBAS
    /// </summary>
    public class AsvRsgaRttTxGbasPacket : MavlinkV2Message<AsvRsgaRttTxGbasPayload>
    {
        public const int MessageId = 13457;
        
        public const byte CrcExtra = 123;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttTxGbasPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_TX_GBAS";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_TX_GBAS
    /// </summary>
    public class AsvRsgaRttTxGbasPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_ADSB_REQ mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_ADSB_REQ
    /// </summary>
    public class AsvRsgaRttAdsbReqPacket : MavlinkV2Message<AsvRsgaRttAdsbReqPayload>
    {
        public const int MessageId = 13458;
        
        public const byte CrcExtra = 98;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttAdsbReqPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_ADSB_REQ";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_ADSB_REQ
    /// </summary>
    public class AsvRsgaRttAdsbReqPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_TX_GNSS mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_TX_GNSS
    /// </summary>
    public class AsvRsgaRttTxGnssPacket : MavlinkV2Message<AsvRsgaRttTxGnssPayload>
    {
        public const int MessageId = 13459;
        
        public const byte CrcExtra = 198;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttTxGnssPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_TX_GNSS";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_TX_GNSS
    /// </summary>
    public class AsvRsgaRttTxGnssPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_DME_REQ mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_DME_REQ
    /// </summary>
    public class AsvRsgaRttDmeReqPacket : MavlinkV2Message<AsvRsgaRttDmeReqPayload>
    {
        public const int MessageId = 13460;
        
        public const byte CrcExtra = 187;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttDmeReqPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_DME_REQ";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_DME_REQ
    /// </summary>
    public class AsvRsgaRttDmeReqPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 89; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 89; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +8 // uint64_t tx_freq
            +8 // uint64_t rx_freq
            +4 // uint32_t index
            +4 // float tx_power
            +4 // float tx_gain
            +4 // float rx_power
            +4 // float rx_field_strength
            +4 // float rx_signal_overflow
            +4 // float rx_gain
            +4 // float distance
            +4 // float reply_efficiency
            +2 // int16_t rx_freq_offset
            +2 // uint16_t pulse_shape_rise
            +2 // uint16_t pulse_shape_duration
            +2 // uint16_t pulse_shape_decay
            +2 // uint16_t pulse_spacing
            +2 // uint16_t req_freq
            +2 // uint16_t hip_freq
            +2 // int16_t measure_time
            +1 // int8_t pulse_shape_amplitude
            +CodeId.Length // char[4] code_id
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            TxFreq = BinSerialize.ReadULong(ref buffer);
            RxFreq = BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);
            TxPower = BinSerialize.ReadFloat(ref buffer);
            TxGain = BinSerialize.ReadFloat(ref buffer);
            RxPower = BinSerialize.ReadFloat(ref buffer);
            RxFieldStrength = BinSerialize.ReadFloat(ref buffer);
            RxSignalOverflow = BinSerialize.ReadFloat(ref buffer);
            RxGain = BinSerialize.ReadFloat(ref buffer);
            Distance = BinSerialize.ReadFloat(ref buffer);
            ReplyEfficiency = BinSerialize.ReadFloat(ref buffer);
            RxFreqOffset = BinSerialize.ReadShort(ref buffer);
            PulseShapeRise = BinSerialize.ReadUShort(ref buffer);
            PulseShapeDuration = BinSerialize.ReadUShort(ref buffer);
            PulseShapeDecay = BinSerialize.ReadUShort(ref buffer);
            PulseSpacing = BinSerialize.ReadUShort(ref buffer);
            ReqFreq = BinSerialize.ReadUShort(ref buffer);
            HipFreq = BinSerialize.ReadUShort(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            PulseShapeAmplitude = (sbyte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/89 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, CodeId.Length);
                }
            }
            buffer = buffer[arraySize..];
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteULong(ref buffer,TxFreq);
            BinSerialize.WriteULong(ref buffer,RxFreq);
            BinSerialize.WriteUInt(ref buffer,Index);
            BinSerialize.WriteFloat(ref buffer,TxPower);
            BinSerialize.WriteFloat(ref buffer,TxGain);
            BinSerialize.WriteFloat(ref buffer,RxPower);
            BinSerialize.WriteFloat(ref buffer,RxFieldStrength);
            BinSerialize.WriteFloat(ref buffer,RxSignalOverflow);
            BinSerialize.WriteFloat(ref buffer,RxGain);
            BinSerialize.WriteFloat(ref buffer,Distance);
            BinSerialize.WriteFloat(ref buffer,ReplyEfficiency);
            BinSerialize.WriteShort(ref buffer,RxFreqOffset);
            BinSerialize.WriteUShort(ref buffer,PulseShapeRise);
            BinSerialize.WriteUShort(ref buffer,PulseShapeDuration);
            BinSerialize.WriteUShort(ref buffer,PulseShapeDecay);
            BinSerialize.WriteUShort(ref buffer,PulseSpacing);
            BinSerialize.WriteUShort(ref buffer,ReqFreq);
            BinSerialize.WriteUShort(ref buffer,HipFreq);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            BinSerialize.WriteByte(ref buffer,(byte)PulseShapeAmplitude);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId)
                {
                    Encoding.ASCII.GetBytes(charPointer, CodeId.Length, bytePointer, CodeId.Length);
                }
            }
            buffer = buffer.Slice(CodeId.Length);
            
            /* PayloadByteSize = 89 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt64Type.Accept(visitor,TxFreqField, TxFreqField.DataType, ref _txFreq);    
            UInt64Type.Accept(visitor,RxFreqField, RxFreqField.DataType, ref _rxFreq);    
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    
            FloatType.Accept(visitor,TxPowerField, TxPowerField.DataType, ref _txPower);    
            FloatType.Accept(visitor,TxGainField, TxGainField.DataType, ref _txGain);    
            FloatType.Accept(visitor,RxPowerField, RxPowerField.DataType, ref _rxPower);    
            FloatType.Accept(visitor,RxFieldStrengthField, RxFieldStrengthField.DataType, ref _rxFieldStrength);    
            FloatType.Accept(visitor,RxSignalOverflowField, RxSignalOverflowField.DataType, ref _rxSignalOverflow);    
            FloatType.Accept(visitor,RxGainField, RxGainField.DataType, ref _rxGain);    
            FloatType.Accept(visitor,DistanceField, DistanceField.DataType, ref _distance);    
            FloatType.Accept(visitor,ReplyEfficiencyField, ReplyEfficiencyField.DataType, ref _replyEfficiency);    
            Int16Type.Accept(visitor,RxFreqOffsetField, RxFreqOffsetField.DataType, ref _rxFreqOffset);
            UInt16Type.Accept(visitor,PulseShapeRiseField, PulseShapeRiseField.DataType, ref _pulseShapeRise);    
            UInt16Type.Accept(visitor,PulseShapeDurationField, PulseShapeDurationField.DataType, ref _pulseShapeDuration);    
            UInt16Type.Accept(visitor,PulseShapeDecayField, PulseShapeDecayField.DataType, ref _pulseShapeDecay);    
            UInt16Type.Accept(visitor,PulseSpacingField, PulseSpacingField.DataType, ref _pulseSpacing);    
            UInt16Type.Accept(visitor,ReqFreqField, ReqFreqField.DataType, ref _reqFreq);    
            UInt16Type.Accept(visitor,HipFreqField, HipFreqField.DataType, ref _hipFreq);    
            Int16Type.Accept(visitor,MeasureTimeField, MeasureTimeField.DataType, ref _measureTime);
            Int8Type.Accept(visitor,PulseShapeAmplitudeField, PulseShapeAmplitudeField.DataType, ref _pulseShapeAmplitude);                
            ArrayType.Accept(visitor,CodeIdField, CodeIdField.DataType, 4, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref CodeId[index]));

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// TX frequency
        /// OriginName: tx_freq, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TxFreqField = new Field.Builder()
            .Name(nameof(TxFreq))
            .Title("tx_freq")
            .Description("TX frequency")
.Units(@"Hz")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _txFreq;
        public ulong TxFreq { get => _txFreq; set => _txFreq = value; }
        /// <summary>
        /// RX frequency
        /// OriginName: rx_freq, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field RxFreqField = new Field.Builder()
            .Name(nameof(RxFreq))
            .Title("rx_freq")
            .Description("RX frequency")
.Units(@"Hz")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _rxFreq;
        public ulong RxFreq { get => _rxFreq; set => _rxFreq = value; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
        /// <summary>
        /// Output power
        /// OriginName: tx_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field TxPowerField = new Field.Builder()
            .Name(nameof(TxPower))
            .Title("tx_power")
            .Description("Output power")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _txPower;
        public float TxPower { get => _txPower; set => _txPower = value; }
        /// <summary>
        /// Percent of total TX gain level (0.0 - 1.0)
        /// OriginName: tx_gain, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TxGainField = new Field.Builder()
            .Name(nameof(TxGain))
            .Title("tx_gain")
            .Description("Percent of total TX gain level (0.0 - 1.0)")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _txGain;
        public float TxGain { get => _txGain; set => _txGain = value; }
        /// <summary>
        /// Receive power (peak)
        /// OriginName: rx_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field RxPowerField = new Field.Builder()
            .Name(nameof(RxPower))
            .Title("rx_power")
            .Description("Receive power (peak)")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _rxPower;
        public float RxPower { get => _rxPower; set => _rxPower = value; }
        /// <summary>
        /// Receive power field strength.
        /// OriginName: rx_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field RxFieldStrengthField = new Field.Builder()
            .Name(nameof(RxFieldStrength))
            .Title("rx_field_strength")
            .Description("Receive power field strength.")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _rxFieldStrength;
        public float RxFieldStrength { get => _rxFieldStrength; set => _rxFieldStrength = value; }
        /// <summary>
        /// Signal overflow indicator (≤0.2 — too weak, ≥0.8 — too strong).
        /// OriginName: rx_signal_overflow, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field RxSignalOverflowField = new Field.Builder()
            .Name(nameof(RxSignalOverflow))
            .Title("rx_signal_overflow")
            .Description("Signal overflow indicator (\u22640.2 \u2014 too weak, \u22650.8 \u2014 too strong).")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _rxSignalOverflow;
        public float RxSignalOverflow { get => _rxSignalOverflow; set => _rxSignalOverflow = value; }
        /// <summary>
        /// Percent of total RX gain level (0.0 - 1.0)
        /// OriginName: rx_gain, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field RxGainField = new Field.Builder()
            .Name(nameof(RxGain))
            .Title("rx_gain")
            .Description("Percent of total RX gain level (0.0 - 1.0)")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _rxGain;
        public float RxGain { get => _rxGain; set => _rxGain = value; }
        /// <summary>
        /// Measured distance
        /// OriginName: distance, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field DistanceField = new Field.Builder()
            .Name(nameof(Distance))
            .Title("distance")
            .Description("Measured distance")
.Units(@"m")
            .DataType(FloatType.Default)
        .Build();
        private float _distance;
        public float Distance { get => _distance; set => _distance = value; }
        /// <summary>
        /// Reply efficiency request\response (between 0% - 100%)
        /// OriginName: reply_efficiency, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field ReplyEfficiencyField = new Field.Builder()
            .Name(nameof(ReplyEfficiency))
            .Title("reply_efficiency")
            .Description("Reply efficiency request\\response (between 0% - 100%)")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _replyEfficiency;
        public float ReplyEfficiency { get => _replyEfficiency; set => _replyEfficiency = value; }
        /// <summary>
        /// RX frequency offset
        /// OriginName: rx_freq_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field RxFreqOffsetField = new Field.Builder()
            .Name(nameof(RxFreqOffset))
            .Title("rx_freq_offset")
            .Description("RX frequency offset")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _rxFreqOffset;
        public short RxFreqOffset { get => _rxFreqOffset; set => _rxFreqOffset = value; }
        /// <summary>
        /// Pulse shape: rise time (≤3 μs)
        /// OriginName: pulse_shape_rise, Units: ns, IsExtended: false
        /// </summary>
        public static readonly Field PulseShapeRiseField = new Field.Builder()
            .Name(nameof(PulseShapeRise))
            .Title("pulse_shape_rise")
            .Description("Pulse shape: rise time (\u22643 \u03BCs)")
.Units(@"ns")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _pulseShapeRise;
        public ushort PulseShapeRise { get => _pulseShapeRise; set => _pulseShapeRise = value; }
        /// <summary>
        /// Pulse shape: rise time (3.5 μs, ±0.5 μs)
        /// OriginName: pulse_shape_duration, Units: ns, IsExtended: false
        /// </summary>
        public static readonly Field PulseShapeDurationField = new Field.Builder()
            .Name(nameof(PulseShapeDuration))
            .Title("pulse_shape_duration")
            .Description("Pulse shape: rise time (3.5 \u03BCs, \u00B10.5 \u03BCs)")
.Units(@"ns")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _pulseShapeDuration;
        public ushort PulseShapeDuration { get => _pulseShapeDuration; set => _pulseShapeDuration = value; }
        /// <summary>
        /// Pulse shape: rise time (≤3.5 μs)
        /// OriginName: pulse_shape_decay, Units: ns, IsExtended: false
        /// </summary>
        public static readonly Field PulseShapeDecayField = new Field.Builder()
            .Name(nameof(PulseShapeDecay))
            .Title("pulse_shape_decay")
            .Description("Pulse shape: rise time (\u22643.5 \u03BCs)")
.Units(@"ns")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _pulseShapeDecay;
        public ushort PulseShapeDecay { get => _pulseShapeDecay; set => _pulseShapeDecay = value; }
        /// <summary>
        /// Pulse spacing (X channel 12 ±0.25 us, Y channel: 30 ±0.25 us)
        /// OriginName: pulse_spacing, Units: ns, IsExtended: false
        /// </summary>
        public static readonly Field PulseSpacingField = new Field.Builder()
            .Name(nameof(PulseSpacing))
            .Title("pulse_spacing")
            .Description("Pulse spacing (X channel 12 \u00B10.25 us, Y channel: 30 \u00B10.25 us)")
.Units(@"ns")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _pulseSpacing;
        public ushort PulseSpacing { get => _pulseSpacing; set => _pulseSpacing = value; }
        /// <summary>
        /// Number of our request
        /// OriginName: req_freq, Units: pps, IsExtended: false
        /// </summary>
        public static readonly Field ReqFreqField = new Field.Builder()
            .Name(nameof(ReqFreq))
            .Title("req_freq")
            .Description("Number of our request")
.Units(@"pps")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _reqFreq;
        public ushort ReqFreq { get => _reqFreq; set => _reqFreq = value; }
        /// <summary>
        /// Measured number of all replies, that was recognised as beacon HIP
        /// OriginName: hip_freq, Units: pps, IsExtended: false
        /// </summary>
        public static readonly Field HipFreqField = new Field.Builder()
            .Name(nameof(HipFreq))
            .Title("hip_freq")
            .Description("Measured number of all replies, that was recognised as beacon HIP")
.Units(@"pps")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _hipFreq;
        public ushort HipFreq { get => _hipFreq; set => _hipFreq = value; }
        /// <summary>
        /// Measure time.
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field MeasureTimeField = new Field.Builder()
            .Name(nameof(MeasureTime))
            .Title("measure_time")
            .Description("Measure time.")
.Units(@"ms")
            .DataType(Int16Type.Default)
        .Build();
        private short _measureTime;
        public short MeasureTime { get => _measureTime; set => _measureTime = value; }
        /// <summary>
        /// Pulse shape: amplitude (between 95% rise/fall amplitudes, ≥95% of maximum amplitude)
        /// OriginName: pulse_shape_amplitude, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field PulseShapeAmplitudeField = new Field.Builder()
            .Name(nameof(PulseShapeAmplitude))
            .Title("pulse_shape_amplitude")
            .Description("Pulse shape: amplitude (between 95% rise/fall amplitudes, \u226595% of maximum amplitude)")
.Units(@"%")
            .DataType(Int8Type.Default)
        .Build();
        private sbyte _pulseShapeAmplitude;
        public sbyte PulseShapeAmplitude { get => _pulseShapeAmplitude; set => _pulseShapeAmplitude = value; }
        /// <summary>
        /// Code identification
        /// OriginName: code_id, Units: Letters, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdField = new Field.Builder()
            .Name(nameof(CodeId))
            .Title("code_id")
            .Description("Code identification")
.Units(@"Letters")
            .DataType(new ArrayType(CharType.Ascii,4))
        .Build();
        public const int CodeIdMaxItemsCount = 4;
        public char[] CodeId { get; } = new char[4];
        [Obsolete("This method is deprecated. Use GetCodeIdMaxItemsCount instead.")]
        public byte GetCodeIdMaxItemsCount() => 4;
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_RX_LLZ mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_RX_LLZ
    /// </summary>
    public class AsvRsgaRttRxLlzPacket : MavlinkV2Message<AsvRsgaRttRxLlzPayload>
    {
        public const int MessageId = 13461;
        
        public const byte CrcExtra = 24;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttRxLlzPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_RX_LLZ";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_RX_LLZ
    /// </summary>
    public class AsvRsgaRttRxLlzPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_RX_GP mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_RX_GP
    /// </summary>
    public class AsvRsgaRttRxGpPacket : MavlinkV2Message<AsvRsgaRttRxGpPayload>
    {
        public const int MessageId = 13462;
        
        public const byte CrcExtra = 57;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttRxGpPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_RX_GP";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_RX_GP
    /// </summary>
    public class AsvRsgaRttRxGpPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_RX_VOR mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_RX_VOR
    /// </summary>
    public class AsvRsgaRttRxVorPacket : MavlinkV2Message<AsvRsgaRttRxVorPayload>
    {
        public const int MessageId = 13463;
        
        public const byte CrcExtra = 189;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttRxVorPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_RX_VOR";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_RX_VOR
    /// </summary>
    public class AsvRsgaRttRxVorPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_RX_MARKER mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_RX_MARKER
    /// </summary>
    public class AsvRsgaRttRxMarkerPacket : MavlinkV2Message<AsvRsgaRttRxMarkerPayload>
    {
        public const int MessageId = 13464;
        
        public const byte CrcExtra = 236;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttRxMarkerPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_RX_MARKER";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_RX_MARKER
    /// </summary>
    public class AsvRsgaRttRxMarkerPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_RX_GBAS mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_RX_GBAS
    /// </summary>
    public class AsvRsgaRttRxGbasPacket : MavlinkV2Message<AsvRsgaRttRxGbasPayload>
    {
        public const int MessageId = 13465;
        
        public const byte CrcExtra = 3;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttRxGbasPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_RX_GBAS";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_RX_GBAS
    /// </summary>
    public class AsvRsgaRttRxGbasPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_ADSB_REP mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_ADSB_REP
    /// </summary>
    public class AsvRsgaRttAdsbRepPacket : MavlinkV2Message<AsvRsgaRttAdsbRepPayload>
    {
        public const int MessageId = 13466;
        
        public const byte CrcExtra = 151;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttAdsbRepPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_ADSB_REP";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_ADSB_REP
    /// </summary>
    public class AsvRsgaRttAdsbRepPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 136; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 136; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +8 // uint64_t tx_freq
            +8 // uint64_t rx_freq
            +4 // uint32_t index
            +4 // float tx_power
            +4 // float tx_gain
            +4 // float rx_power
            +4 // float rx_field_strength
            +4 // float rx_signal_overflow
            +4 // float rx_gain
            +4 // uint32_t icao_address
            + 4 // uint32_t uf_counter_flag
            + 4 // uint32_t df_counter_present
            +2 // int16_t rx_freq_offset
            +2 // uint16_t ref_id
            +2 // uint16_t squawk
            +CallSign.Length // char[8] call_sign
            +UfCounter.Length // uint8_t[25] uf_counter
            +DfCounter.Length // uint8_t[25] df_counter
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            TxFreq = BinSerialize.ReadULong(ref buffer);
            RxFreq = BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);
            TxPower = BinSerialize.ReadFloat(ref buffer);
            TxGain = BinSerialize.ReadFloat(ref buffer);
            RxPower = BinSerialize.ReadFloat(ref buffer);
            RxFieldStrength = BinSerialize.ReadFloat(ref buffer);
            RxSignalOverflow = BinSerialize.ReadFloat(ref buffer);
            RxGain = BinSerialize.ReadFloat(ref buffer);
            IcaoAddress = BinSerialize.ReadUInt(ref buffer);
            UfCounterFlag = (AsvRsgaRttAdsbMsgUf)BinSerialize.ReadUInt(ref buffer);
            DfCounterPresent = (AsvRsgaRttAdsbMsgDf)BinSerialize.ReadUInt(ref buffer);
            RxFreqOffset = BinSerialize.ReadShort(ref buffer);
            RefId = BinSerialize.ReadUShort(ref buffer);
            Squawk = BinSerialize.ReadUShort(ref buffer);
            arraySize = 8;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CallSign)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, CallSign.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            arraySize = /*ArrayLength*/25 - Math.Max(0,((/*PayloadByteSize*/136 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                UfCounter[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 25;
            for(var i=0;i<arraySize;i++)
            {
                DfCounter[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteULong(ref buffer,TxFreq);
            BinSerialize.WriteULong(ref buffer,RxFreq);
            BinSerialize.WriteUInt(ref buffer,Index);
            BinSerialize.WriteFloat(ref buffer,TxPower);
            BinSerialize.WriteFloat(ref buffer,TxGain);
            BinSerialize.WriteFloat(ref buffer,RxPower);
            BinSerialize.WriteFloat(ref buffer,RxFieldStrength);
            BinSerialize.WriteFloat(ref buffer,RxSignalOverflow);
            BinSerialize.WriteFloat(ref buffer,RxGain);
            BinSerialize.WriteUInt(ref buffer,IcaoAddress);
            BinSerialize.WriteUInt(ref buffer,(uint)UfCounterFlag);
            BinSerialize.WriteUInt(ref buffer,(uint)DfCounterPresent);
            BinSerialize.WriteShort(ref buffer,RxFreqOffset);
            BinSerialize.WriteUShort(ref buffer,RefId);
            BinSerialize.WriteUShort(ref buffer,Squawk);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CallSign)
                {
                    Encoding.ASCII.GetBytes(charPointer, CallSign.Length, bytePointer, CallSign.Length);
                }
            }
            buffer = buffer.Slice(CallSign.Length);
            
            for(var i=0;i<UfCounter.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)UfCounter[i]);
            }
            for(var i=0;i<DfCounter.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)DfCounter[i]);
            }
            /* PayloadByteSize = 136 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt64Type.Accept(visitor,TxFreqField, TxFreqField.DataType, ref _txFreq);    
            UInt64Type.Accept(visitor,RxFreqField, RxFreqField.DataType, ref _rxFreq);    
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    
            FloatType.Accept(visitor,TxPowerField, TxPowerField.DataType, ref _txPower);    
            FloatType.Accept(visitor,TxGainField, TxGainField.DataType, ref _txGain);    
            FloatType.Accept(visitor,RxPowerField, RxPowerField.DataType, ref _rxPower);    
            FloatType.Accept(visitor,RxFieldStrengthField, RxFieldStrengthField.DataType, ref _rxFieldStrength);    
            FloatType.Accept(visitor,RxSignalOverflowField, RxSignalOverflowField.DataType, ref _rxSignalOverflow);    
            FloatType.Accept(visitor,RxGainField, RxGainField.DataType, ref _rxGain);    
            UInt32Type.Accept(visitor,IcaoAddressField, IcaoAddressField.DataType, ref _icaoAddress);    
            var tmpUfCounterFlag = (uint)UfCounterFlag;
            UInt32Type.Accept(visitor,UfCounterFlagField, UfCounterFlagField.DataType, ref tmpUfCounterFlag);
            UfCounterFlag = (AsvRsgaRttAdsbMsgUf)tmpUfCounterFlag;
            var tmpDfCounterPresent = (uint)DfCounterPresent;
            UInt32Type.Accept(visitor,DfCounterPresentField, DfCounterPresentField.DataType, ref tmpDfCounterPresent);
            DfCounterPresent = (AsvRsgaRttAdsbMsgDf)tmpDfCounterPresent;
            Int16Type.Accept(visitor,RxFreqOffsetField, RxFreqOffsetField.DataType, ref _rxFreqOffset);
            UInt16Type.Accept(visitor,RefIdField, RefIdField.DataType, ref _refId);    
            UInt16Type.Accept(visitor,SquawkField, SquawkField.DataType, ref _squawk);    
            ArrayType.Accept(visitor,CallSignField, CallSignField.DataType, 8, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref CallSign[index]));
            ArrayType.Accept(visitor,UfCounterField, UfCounterField.DataType, 25,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref UfCounter[index]));    
            ArrayType.Accept(visitor,DfCounterField, DfCounterField.DataType, 25,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref DfCounter[index]));    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// TX frequency
        /// OriginName: tx_freq, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TxFreqField = new Field.Builder()
            .Name(nameof(TxFreq))
            .Title("tx_freq")
            .Description("TX frequency")
.Units(@"Hz")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _txFreq;
        public ulong TxFreq { get => _txFreq; set => _txFreq = value; }
        /// <summary>
        /// RX frequency
        /// OriginName: rx_freq, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field RxFreqField = new Field.Builder()
            .Name(nameof(RxFreq))
            .Title("rx_freq")
            .Description("RX frequency")
.Units(@"Hz")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _rxFreq;
        public ulong RxFreq { get => _rxFreq; set => _rxFreq = value; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
        /// <summary>
        /// Output power
        /// OriginName: tx_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field TxPowerField = new Field.Builder()
            .Name(nameof(TxPower))
            .Title("tx_power")
            .Description("Output power")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _txPower;
        public float TxPower { get => _txPower; set => _txPower = value; }
        /// <summary>
        /// Percent of total TX gain level (0.0 - 1.0)
        /// OriginName: tx_gain, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TxGainField = new Field.Builder()
            .Name(nameof(TxGain))
            .Title("tx_gain")
            .Description("Percent of total TX gain level (0.0 - 1.0)")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _txGain;
        public float TxGain { get => _txGain; set => _txGain = value; }
        /// <summary>
        /// Receive power (peak)
        /// OriginName: rx_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field RxPowerField = new Field.Builder()
            .Name(nameof(RxPower))
            .Title("rx_power")
            .Description("Receive power (peak)")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _rxPower;
        public float RxPower { get => _rxPower; set => _rxPower = value; }
        /// <summary>
        /// Receive power field strength.
        /// OriginName: rx_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field RxFieldStrengthField = new Field.Builder()
            .Name(nameof(RxFieldStrength))
            .Title("rx_field_strength")
            .Description("Receive power field strength.")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _rxFieldStrength;
        public float RxFieldStrength { get => _rxFieldStrength; set => _rxFieldStrength = value; }
        /// <summary>
        /// Signal overflow indicator (≤0.2 — too weak, ≥0.8 — too strong).
        /// OriginName: rx_signal_overflow, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field RxSignalOverflowField = new Field.Builder()
            .Name(nameof(RxSignalOverflow))
            .Title("rx_signal_overflow")
            .Description("Signal overflow indicator (\u22640.2 \u2014 too weak, \u22650.8 \u2014 too strong).")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _rxSignalOverflow;
        public float RxSignalOverflow { get => _rxSignalOverflow; set => _rxSignalOverflow = value; }
        /// <summary>
        /// Percent of total RX gain level (0.0 - 1.0)
        /// OriginName: rx_gain, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field RxGainField = new Field.Builder()
            .Name(nameof(RxGain))
            .Title("rx_gain")
            .Description("Percent of total RX gain level (0.0 - 1.0)")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _rxGain;
        public float RxGain { get => _rxGain; set => _rxGain = value; }
        /// <summary>
        /// Vehicle ICAO address (24 bit)
        /// OriginName: icao_address, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IcaoAddressField = new Field.Builder()
            .Name(nameof(IcaoAddress))
            .Title("icao_address")
            .Description("Vehicle ICAO address (24 bit)")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _icaoAddress;
        public uint IcaoAddress { get => _icaoAddress; set => _icaoAddress = value; }
        /// <summary>
        /// UF counters present flag 
        /// OriginName: uf_counter_flag, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UfCounterFlagField = new Field.Builder()
            .Name(nameof(UfCounterFlag))
            .Title("uf_counter_flag")
            .Description("UF counters present flag ")
            .DataType(new UInt32Type(AsvRsgaRttAdsbMsgUfHelper.GetValues(x=>(uint)x).Min(),AsvRsgaRttAdsbMsgUfHelper.GetValues(x=>(uint)x).Max()))
            .Enum(AsvRsgaRttAdsbMsgUfHelper.GetEnumValues(x=>(uint)x))
            .Build();
        private AsvRsgaRttAdsbMsgUf _ufCounterFlag;
        public AsvRsgaRttAdsbMsgUf UfCounterFlag { get => _ufCounterFlag; set => _ufCounterFlag = value; } 
        /// <summary>
        /// UF counters present 
        /// OriginName: df_counter_present, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DfCounterPresentField = new Field.Builder()
            .Name(nameof(DfCounterPresent))
            .Title("df_counter_present")
            .Description("UF counters present ")
            .DataType(new UInt32Type(AsvRsgaRttAdsbMsgDfHelper.GetValues(x=>(uint)x).Min(),AsvRsgaRttAdsbMsgDfHelper.GetValues(x=>(uint)x).Max()))
            .Enum(AsvRsgaRttAdsbMsgDfHelper.GetEnumValues(x=>(uint)x))
            .Build();
        private AsvRsgaRttAdsbMsgDf _dfCounterPresent;
        public AsvRsgaRttAdsbMsgDf DfCounterPresent { get => _dfCounterPresent; set => _dfCounterPresent = value; } 
        /// <summary>
        /// RX frequency offset
        /// OriginName: rx_freq_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field RxFreqOffsetField = new Field.Builder()
            .Name(nameof(RxFreqOffset))
            .Title("rx_freq_offset")
            .Description("RX frequency offset")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _rxFreqOffset;
        public short RxFreqOffset { get => _rxFreqOffset; set => _rxFreqOffset = value; }
        /// <summary>
        /// GNSS reference station ID (used when GNSS is received from multiple sources).
        /// OriginName: ref_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RefIdField = new Field.Builder()
            .Name(nameof(RefId))
            .Title("ref_id")
            .Description("GNSS reference station ID (used when GNSS is received from multiple sources).")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _refId;
        public ushort RefId { get => _refId; set => _refId = value; }
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
        /// Vehicle identifier (8 characters, valid characters are A-Z, 0-9, " " only)
        /// OriginName: call_sign, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CallSignField = new Field.Builder()
            .Name(nameof(CallSign))
            .Title("call_sign")
            .Description("Vehicle identifier (8 characters, valid characters are A-Z, 0-9, \" \" only)")

            .DataType(new ArrayType(CharType.Ascii,8))
        .Build();
        public const int CallSignMaxItemsCount = 8;
        public char[] CallSign { get; } = new char[8];
        /// <summary>
        /// UF incremental counters for every 25 message 
        /// OriginName: uf_counter, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UfCounterField = new Field.Builder()
            .Name(nameof(UfCounter))
            .Title("uf_counter")
            .Description("UF incremental counters for every 25 message ")

            .DataType(new ArrayType(UInt8Type.Default,25))
        .Build();
        public const int UfCounterMaxItemsCount = 25;
        public byte[] UfCounter { get; } = new byte[25];
        [Obsolete("This method is deprecated. Use GetUfCounterMaxItemsCount instead.")]
        public byte GetUfCounterMaxItemsCount() => 25;
        /// <summary>
        /// DF incremental counters for every 25 message 
        /// OriginName: df_counter, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DfCounterField = new Field.Builder()
            .Name(nameof(DfCounter))
            .Title("df_counter")
            .Description("DF incremental counters for every 25 message ")

            .DataType(new ArrayType(UInt8Type.Default,25))
        .Build();
        public const int DfCounterMaxItemsCount = 25;
        public byte[] DfCounter { get; } = new byte[25];
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_RX_GNSS mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_RX_GNSS
    /// </summary>
    public class AsvRsgaRttRxGnssPacket : MavlinkV2Message<AsvRsgaRttRxGnssPayload>
    {
        public const int MessageId = 13467;
        
        public const byte CrcExtra = 190;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttRxGnssPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_RX_GNSS";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_RX_GNSS
    /// </summary>
    public class AsvRsgaRttRxGnssPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +4 // uint32_t index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteUInt(ref buffer,Index);
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags.")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IndexField = new Field.Builder()
            .Name(nameof(Index))
            .Title("index")
            .Description("Data index in record")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _index;
        public uint Index { get => _index; set => _index = value; }
    }




        


#endregion


}

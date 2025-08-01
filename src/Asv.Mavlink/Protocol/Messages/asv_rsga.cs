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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.11+05423b76b208fe780abe1cef9f7beeacb19cba77 25-07-04.

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
            src.Add(AsvRsgaRttRdfPacket.MessageId, ()=>new AsvRsgaRttRdfPacket());
        }
 
    }

#region Enums

    /// <summary>
    ///  MAV_TYPE
    /// </summary>
    public enum MavType : ulong
    {
        /// <summary>
        /// Identifies the Radio Signal Generator and Analyzer (RSGA) payload in the HEARTBEAT message
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
    /// Defines RSGA modes mapped to the custom_mode[0–7] bit field of the HEARTBEAT message. Maximum 255 values
    ///  ASV_RSGA_CUSTOM_MODE
    /// </summary>
    public enum AsvRsgaCustomMode : ulong
    {
        /// <summary>
        /// Default mode. No operation performed
        /// ASV_RSGA_CUSTOM_MODE_IDLE
        /// </summary>
        AsvRsgaCustomModeIdle = 0,
        /// <summary>
        /// Spectrum analysis mode
        /// ASV_RSGA_CUSTOM_MODE_SPECTRUM
        /// </summary>
        AsvRsgaCustomModeSpectrum = 25,
        /// <summary>
        /// Localizer generator mode
        /// ASV_RSGA_CUSTOM_MODE_TX_LLZ
        /// </summary>
        AsvRsgaCustomModeTxLlz = 50,
        /// <summary>
        /// Glide Path generator mode
        /// ASV_RSGA_CUSTOM_MODE_TX_GP
        /// </summary>
        AsvRsgaCustomModeTxGp = 51,
        /// <summary>
        /// VOR generator mode
        /// ASV_RSGA_CUSTOM_MODE_TX_VOR
        /// </summary>
        AsvRsgaCustomModeTxVor = 52,
        /// <summary>
        /// Marker generator mode
        /// ASV_RSGA_CUSTOM_MODE_TX_MARKER
        /// </summary>
        AsvRsgaCustomModeTxMarker = 53,
        /// <summary>
        /// DME beacon (replier) mode
        /// ASV_RSGA_CUSTOM_MODE_DME_REP
        /// </summary>
        AsvRsgaCustomModeDmeRep = 54,
        /// <summary>
        /// GBAS generator mode
        /// ASV_RSGA_CUSTOM_MODE_TX_GBAS
        /// </summary>
        AsvRsgaCustomModeTxGbas = 55,
        /// <summary>
        /// ADSB beacon(interrogator) mode
        /// ASV_RSGA_CUSTOM_MODE_ADSB_REQ
        /// </summary>
        AsvRsgaCustomModeAdsbReq = 56,
        /// <summary>
        /// GNSS generator(satellite) mode
        /// ASV_RSGA_CUSTOM_MODE_TX_GNSS
        /// </summary>
        AsvRsgaCustomModeTxGnss = 57,
        /// <summary>
        /// DME air(interrogator) mode
        /// ASV_RSGA_CUSTOM_MODE_DME_REQ
        /// </summary>
        AsvRsgaCustomModeDmeReq = 74,
        /// <summary>
        /// Localizer analyzer mode
        /// ASV_RSGA_CUSTOM_MODE_RX_LLZ
        /// </summary>
        AsvRsgaCustomModeRxLlz = 75,
        /// <summary>
        /// Glide Path analyzer mode
        /// ASV_RSGA_CUSTOM_MODE_RX_GP
        /// </summary>
        AsvRsgaCustomModeRxGp = 76,
        /// <summary>
        /// VOR analyzer mode
        /// ASV_RSGA_CUSTOM_MODE_RX_VOR
        /// </summary>
        AsvRsgaCustomModeRxVor = 77,
        /// <summary>
        /// Marker analyzer mode
        /// ASV_RSGA_CUSTOM_MODE_RX_MARKER
        /// </summary>
        AsvRsgaCustomModeRxMarker = 78,
        /// <summary>
        /// GBAS analyzer mode
        /// ASV_RSGA_CUSTOM_MODE_RX_GBAS
        /// </summary>
        AsvRsgaCustomModeRxGbas = 79,
        /// <summary>
        /// ADSB air(replier) mode
        /// ASV_RSGA_CUSTOM_MODE_ADSB_REP
        /// </summary>
        AsvRsgaCustomModeAdsbRep = 80,
        /// <summary>
        /// GNSS analyzer mode
        /// ASV_RSGA_CUSTOM_MODE_RX_GNSS
        /// </summary>
        AsvRsgaCustomModeRxGnss = 81,
        /// <summary>
        /// RDF mode
        /// ASV_RSGA_CUSTOM_MODE_RDF
        /// </summary>
        AsvRsgaCustomModeRdf = 82,
        /// <summary>
        /// Audio radio station mode
        /// ASV_RSGA_CUSTOM_MODE_RADIO
        /// </summary>
        AsvRsgaCustomModeRadio = 100,
        /// <summary>
        /// Max available mode value (Reserved)
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
        /// Recording enabled
        /// ASV_RSGA_CUSTOM_SUB_MODE_RECORD
        /// </summary>
        AsvRsgaCustomSubModeRecord = 1,
        /// <summary>
        /// Mission is started
        /// ASV_RSGA_CUSTOM_SUB_MODE_MISSION
        /// </summary>
        AsvRsgaCustomSubModeMission = 2,
        /// <summary>
        /// Reserved 2
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED2
        /// </summary>
        AsvRsgaCustomSubModeReserved2 = 4,
        /// <summary>
        /// Reserved 3
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED3
        /// </summary>
        AsvRsgaCustomSubModeReserved3 = 8,
        /// <summary>
        /// Reserved 4
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED4
        /// </summary>
        AsvRsgaCustomSubModeReserved4 = 16,
        /// <summary>
        /// Reserved 5
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED5
        /// </summary>
        AsvRsgaCustomSubModeReserved5 = 32,
        /// <summary>
        /// Reserved 6
        /// ASV_RSGA_CUSTOM_SUB_MODE_RESERVED6
        /// </summary>
        AsvRsgaCustomSubModeReserved6 = 64,
        /// <summary>
        /// Reserved 7
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
    /// ACK / NACK / ERROR values as a result of ASV_RSGA_*_REQUEST commands
    ///  ASV_RSGA_REQUEST_ACK
    /// </summary>
    public enum AsvRsgaRequestAck : ulong
    {
        /// <summary>
        /// Request is ok
        /// ASV_RSGA_REQUEST_ACK_OK
        /// </summary>
        AsvRsgaRequestAckOk = 0,
        /// <summary>
        /// Already in progress
        /// ASV_RSGA_REQUEST_ACK_IN_PROGRESS
        /// </summary>
        AsvRsgaRequestAckInProgress = 1,
        /// <summary>
        /// Internal error
        /// ASV_RSGA_REQUEST_ACK_FAIL
        /// </summary>
        AsvRsgaRequestAckFail = 2,
        /// <summary>
        /// Not supported
        /// ASV_RSGA_REQUEST_ACK_NOT_SUPPORTED
        /// </summary>
        AsvRsgaRequestAckNotSupported = 3,
        /// <summary>
        /// Not found
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
        /// Is data valid
        /// ASV_RSGA_DATA_FLAGS_IS_VALID
        /// </summary>
        AsvRsgaDataFlagsIsValid = 1,
        /// <summary>
        /// Signal level too high
        /// ASV_RSGA_DATA_FLAGS_SIGNAL_OVERFLOW
        /// </summary>
        AsvRsgaDataFlagsSignalOverflow = 2,
        /// <summary>
        /// Signal level too low
        /// ASV_RSGA_DATA_FLAGS_SIGNAL_LOW
        /// </summary>
        AsvRsgaDataFlagsSignalLow = 4,
    }
    public static class AsvRsgaDataFlagsHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(4);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_RSGA_DATA_FLAGS_IS_VALID");
            yield return new EnumValue<T>(converter(2),"ASV_RSGA_DATA_FLAGS_SIGNAL_OVERFLOW");
            yield return new EnumValue<T>(converter(4),"ASV_RSGA_DATA_FLAGS_SIGNAL_LOW");
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
        /// This flag is set when the vehicle is known to be on the ground
        /// ASV_RSGA_RTT_GNSS_FLAGS_ON_THE_GROUND
        /// </summary>
        AsvRsgaRttGnssFlagsOnTheGround = 1,
        /// <summary>
        /// Reserved 
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED1
        /// </summary>
        AsvRsgaRttGnssFlagsReserved1 = 2,
        /// <summary>
        /// Reserved
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED2
        /// </summary>
        AsvRsgaRttGnssFlagsReserved2 = 4,
        /// <summary>
        /// Reserved
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED3
        /// </summary>
        AsvRsgaRttGnssFlagsReserved3 = 8,
        /// <summary>
        /// Reserved
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED4
        /// </summary>
        AsvRsgaRttGnssFlagsReserved4 = 16,
        /// <summary>
        /// Reserved
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED5
        /// </summary>
        AsvRsgaRttGnssFlagsReserved5 = 32,
        /// <summary>
        /// Reserved
        /// ASV_RSGA_RTT_GNSS_FLAGS_RESERVED6
        /// </summary>
        AsvRsgaRttGnssFlagsReserved6 = 64,
        /// <summary>
        /// Reserved
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
        /// Virtual GNSS data
        /// ASV_RSGA_RTT_GNSS_TYPE_VIRTUAL
        /// </summary>
        AsvRsgaRttGnssTypeVirtual = 0,
        /// <summary>
        /// GNSS data from receiver
        /// ASV_RSGA_RTT_GNSS_TYPE_NMEA
        /// </summary>
        AsvRsgaRttGnssTypeNmea = 1,
        /// <summary>
        /// GNSS data from UAV
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
        /// [UF00] Short air-air surveillance (ACAS)
        /// ASV_RSGA_RTT_ADSB_MSG_UF_00
        /// </summary>
        AsvRsgaRttAdsbMsgUf00 = 1,
        /// <summary>
        /// [UF01] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_01
        /// </summary>
        AsvRsgaRttAdsbMsgUf01 = 2,
        /// <summary>
        /// [UF02] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_02
        /// </summary>
        AsvRsgaRttAdsbMsgUf02 = 4,
        /// <summary>
        /// [UF03] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_03
        /// </summary>
        AsvRsgaRttAdsbMsgUf03 = 8,
        /// <summary>
        /// [UF04] Surveillance, altitude request
        /// ASV_RSGA_RTT_ADSB_MSG_UF_04
        /// </summary>
        AsvRsgaRttAdsbMsgUf04 = 16,
        /// <summary>
        /// [UF05] Surveillance, identify request
        /// ASV_RSGA_RTT_ADSB_MSG_UF_05
        /// </summary>
        AsvRsgaRttAdsbMsgUf05 = 32,
        /// <summary>
        /// [UF06] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_06
        /// </summary>
        AsvRsgaRttAdsbMsgUf06 = 64,
        /// <summary>
        /// [UF07] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_07
        /// </summary>
        AsvRsgaRttAdsbMsgUf07 = 128,
        /// <summary>
        /// [UF08] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_08
        /// </summary>
        AsvRsgaRttAdsbMsgUf08 = 256,
        /// <summary>
        /// [UF09] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_09
        /// </summary>
        AsvRsgaRttAdsbMsgUf09 = 512,
        /// <summary>
        /// [UF10] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_10
        /// </summary>
        AsvRsgaRttAdsbMsgUf10 = 1024,
        /// <summary>
        /// [UF11] Mode S only all-call
        /// ASV_RSGA_RTT_ADSB_MSG_UF_11
        /// </summary>
        AsvRsgaRttAdsbMsgUf11 = 2048,
        /// <summary>
        /// [UF12] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_12
        /// </summary>
        AsvRsgaRttAdsbMsgUf12 = 4096,
        /// <summary>
        /// [UF13] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_13
        /// </summary>
        AsvRsgaRttAdsbMsgUf13 = 8192,
        /// <summary>
        /// [UF14] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_14
        /// </summary>
        AsvRsgaRttAdsbMsgUf14 = 16384,
        /// <summary>
        /// [UF15] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_15
        /// </summary>
        AsvRsgaRttAdsbMsgUf15 = 32768,
        /// <summary>
        /// [UF16] Long air-air surveillance (ACAS)
        /// ASV_RSGA_RTT_ADSB_MSG_UF_16
        /// </summary>
        AsvRsgaRttAdsbMsgUf16 = 65536,
        /// <summary>
        /// [UF17] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_17
        /// </summary>
        AsvRsgaRttAdsbMsgUf17 = 131072,
        /// <summary>
        /// [UF18] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_18
        /// </summary>
        AsvRsgaRttAdsbMsgUf18 = 262144,
        /// <summary>
        /// [UF19] Reserved for military use
        /// ASV_RSGA_RTT_ADSB_MSG_UF_19
        /// </summary>
        AsvRsgaRttAdsbMsgUf19 = 524288,
        /// <summary>
        /// [UF20] Comm-A, altitude request
        /// ASV_RSGA_RTT_ADSB_MSG_UF_20
        /// </summary>
        AsvRsgaRttAdsbMsgUf20 = 1048576,
        /// <summary>
        /// [UF21] Comm-A, identify request
        /// ASV_RSGA_RTT_ADSB_MSG_UF_21
        /// </summary>
        AsvRsgaRttAdsbMsgUf21 = 2097152,
        /// <summary>
        /// [UF22] Reserved for military use
        /// ASV_RSGA_RTT_ADSB_MSG_UF_22
        /// </summary>
        AsvRsgaRttAdsbMsgUf22 = 4194304,
        /// <summary>
        /// [UF23] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_UF_23
        /// </summary>
        AsvRsgaRttAdsbMsgUf23 = 8388608,
        /// <summary>
        /// [UF24] Comm-C (ELM)
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
        /// [DF00] Short air-air surveillance (ACAS)
        /// ASV_RSGA_RTT_ADSB_MSG_DF_00
        /// </summary>
        AsvRsgaRttAdsbMsgDf00 = 1,
        /// <summary>
        /// [DF01] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_01
        /// </summary>
        AsvRsgaRttAdsbMsgDf01 = 2,
        /// <summary>
        /// [DF02] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_02
        /// </summary>
        AsvRsgaRttAdsbMsgDf02 = 4,
        /// <summary>
        /// [DF03] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_03
        /// </summary>
        AsvRsgaRttAdsbMsgDf03 = 8,
        /// <summary>
        /// [DF04] Surveillance, altitude reply
        /// ASV_RSGA_RTT_ADSB_MSG_DF_04
        /// </summary>
        AsvRsgaRttAdsbMsgDf04 = 16,
        /// <summary>
        /// [DF05] Surveillance, identify reply
        /// ASV_RSGA_RTT_ADSB_MSG_DF_05
        /// </summary>
        AsvRsgaRttAdsbMsgDf05 = 32,
        /// <summary>
        /// [DF06] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_06
        /// </summary>
        AsvRsgaRttAdsbMsgDf06 = 64,
        /// <summary>
        /// [DF07] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_07
        /// </summary>
        AsvRsgaRttAdsbMsgDf07 = 128,
        /// <summary>
        /// [DF08] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_08
        /// </summary>
        AsvRsgaRttAdsbMsgDf08 = 256,
        /// <summary>
        /// [DF09] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_09
        /// </summary>
        AsvRsgaRttAdsbMsgDf09 = 512,
        /// <summary>
        /// [DF10] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_10
        /// </summary>
        AsvRsgaRttAdsbMsgDf10 = 1024,
        /// <summary>
        /// [DF11] All-call reply
        /// ASV_RSGA_RTT_ADSB_MSG_DF_11
        /// </summary>
        AsvRsgaRttAdsbMsgDf11 = 2048,
        /// <summary>
        /// [DF12] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_12
        /// </summary>
        AsvRsgaRttAdsbMsgDf12 = 4096,
        /// <summary>
        /// [DF13] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_13
        /// </summary>
        AsvRsgaRttAdsbMsgDf13 = 8192,
        /// <summary>
        /// [DF14] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_14
        /// </summary>
        AsvRsgaRttAdsbMsgDf14 = 16384,
        /// <summary>
        /// [DF15] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_15
        /// </summary>
        AsvRsgaRttAdsbMsgDf15 = 32768,
        /// <summary>
        /// [DF16] Long air-air surveillance (ACAS)
        /// ASV_RSGA_RTT_ADSB_MSG_DF_16
        /// </summary>
        AsvRsgaRttAdsbMsgDf16 = 65536,
        /// <summary>
        /// [DF17] Extended squitter
        /// ASV_RSGA_RTT_ADSB_MSG_DF_17
        /// </summary>
        AsvRsgaRttAdsbMsgDf17 = 131072,
        /// <summary>
        /// [DF18] Extended squitter/non transponder
        /// ASV_RSGA_RTT_ADSB_MSG_DF_18
        /// </summary>
        AsvRsgaRttAdsbMsgDf18 = 262144,
        /// <summary>
        /// [DF19] Military extended squitter
        /// ASV_RSGA_RTT_ADSB_MSG_DF_19
        /// </summary>
        AsvRsgaRttAdsbMsgDf19 = 524288,
        /// <summary>
        /// [DF20] Comm-B, altitude reply
        /// ASV_RSGA_RTT_ADSB_MSG_DF_20
        /// </summary>
        AsvRsgaRttAdsbMsgDf20 = 1048576,
        /// <summary>
        /// [DF21] Comm-B, identify reply
        /// ASV_RSGA_RTT_ADSB_MSG_DF_21
        /// </summary>
        AsvRsgaRttAdsbMsgDf21 = 2097152,
        /// <summary>
        /// [DF22] Reserved for military use
        /// ASV_RSGA_RTT_ADSB_MSG_DF_22
        /// </summary>
        AsvRsgaRttAdsbMsgDf22 = 4194304,
        /// <summary>
        /// [DF23] Reserved
        /// ASV_RSGA_RTT_ADSB_MSG_DF_23
        /// </summary>
        AsvRsgaRttAdsbMsgDf23 = 8388608,
        /// <summary>
        /// [DF24] Comm-D (ELM)
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
        /// Set the operational mode
        /// Param 1 - Mode (uint32_t, see ASV_RSGA_CUSTOM_MODE)
        /// Param 2 - Empty
        /// Param 3 - Empty
        /// Param 4 - Empty
        /// Param 5 - Empty
        /// Param 6 - Empty
        /// Param 7 - Empty
        /// MAV_CMD_ASV_RSGA_SET_MODE
        /// </summary>
        MavCmdAsvRsgaSetMode = 13400,
        /// <summary>
        /// Start data recording with a unique name (maximum 28 characters). Can be used in mission protocol for RSGA payloads
        /// Param 1 - Record unique name: 0-3 chars (char[4])
        /// Param 2 - Record unique name: 4-7 chars (char[4])
        /// Param 3 - Record unique name: 8-11 chars (char[4])
        /// Param 4 - Record unique name: 12-15 chars (char[4])
        /// Param 5 - Record unique name: 16-19 chars (char[4])
        /// Param 6 - Record unique name: 20-23 chars (char[4])
        /// Param 7 - Record unique name: 24-27 chars (char[4])
        /// MAV_CMD_ASV_RSGA_START_RECORD
        /// </summary>
        MavCmdAsvRsgaStartRecord = 13401,
        /// <summary>
        /// Stop recording data. Can be used in the mission protocol for RSGA payloads
        /// Param 1 - Empty
        /// Param 2 - Empty
        /// Param 3 - Empty
        /// Param 4 - Empty
        /// Param 5 - Empty
        /// Param 6 - Empty
        /// Param 7 - Empty
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
        /// Specifies a unique number for this request. This allows the response packet to be identified
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies a unique number for this request. This allows the response packet to be identified")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _requestId;
        public ushort RequestId { get => _requestId; set => _requestId = value; }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetSystem;
        public byte TargetSystem { get => _targetSystem; set => _targetSystem = value; }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")

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
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request. This allows the response to be matched to the correct request")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _requestId;
        public ushort RequestId { get => _requestId; set => _requestId = value; }
        /// <summary>
        /// Result code
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code")
            .DataType(new UInt8Type(AsvRsgaRequestAckHelper.GetValues(x=>(byte)x).Min(),AsvRsgaRequestAckHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvRsgaRequestAckHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvRsgaRequestAck _result;
        public AsvRsgaRequestAck Result { get => _result; set => _result = value; } 
        /// <summary>
        /// Supported modes. Each bit index represents an ASV_RSGA_CUSTOM_MODE value (256 bits). First (IDLE) bit always true
        /// OriginName: supported_modes, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SupportedModesField = new Field.Builder()
            .Name(nameof(SupportedModes))
            .Title("supported_modes")
            .Description("Supported modes. Each bit index represents an ASV_RSGA_CUSTOM_MODE value (256 bits). First (IDLE) bit always true")

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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// GNSS reference station ID (used when GNSS is received from multiple sources)
        /// OriginName: ref_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RefIdField = new Field.Builder()
            .Name(nameof(RefId))
            .Title("ref_id")
            .Description("GNSS reference station ID (used when GNSS is received from multiple sources)")

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
        /// GNSS receiver type
        /// OriginName: receiver_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ReceiverTypeField = new Field.Builder()
            .Name(nameof(ReceiverType))
            .Title("receiver_type")
            .Description("GNSS receiver type")
            .DataType(new UInt8Type(AsvRsgaRttGnssTypeHelper.GetValues(x=>(byte)x).Min(),AsvRsgaRttGnssTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvRsgaRttGnssTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvRsgaRttGnssType _receiverType;
        public AsvRsgaRttGnssType ReceiverType { get => _receiverType; set => _receiverType = value; } 
        /// <summary>
        /// GNSS special flags
        /// OriginName: gnss_flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssFlagsField = new Field.Builder()
            .Name(nameof(GnssFlags))
            .Title("gnss_flags")
            .Description("GNSS special flags")
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        
        public const byte CrcExtra = 243;
        
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
            +8 // uint64_t rx_freq
            +8 // uint64_t tx_freq
            +4 // uint32_t index
            +4 // float rx_power
            +4 // float rx_field_strength
            +4 // float rx_signal_overflow
            +4 // float rx_gain
            +4 // float tx_power
            +4 // float tx_gain
            +4 // float distance
            +4 // float reply_efficiency
            +2 // int16_t measure_time
            +2 // int16_t rx_freq_offset
            +2 // uint16_t pulse_shape_rise
            +2 // uint16_t pulse_shape_duration
            +2 // uint16_t pulse_shape_decay
            +2 // uint16_t pulse_spacing
            +2 // uint16_t req_freq
            +2 // uint16_t hip_freq
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
            RxFreq = BinSerialize.ReadULong(ref buffer);
            TxFreq = BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);
            RxPower = BinSerialize.ReadFloat(ref buffer);
            RxFieldStrength = BinSerialize.ReadFloat(ref buffer);
            RxSignalOverflow = BinSerialize.ReadFloat(ref buffer);
            RxGain = BinSerialize.ReadFloat(ref buffer);
            TxPower = BinSerialize.ReadFloat(ref buffer);
            TxGain = BinSerialize.ReadFloat(ref buffer);
            Distance = BinSerialize.ReadFloat(ref buffer);
            ReplyEfficiency = BinSerialize.ReadFloat(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            RxFreqOffset = BinSerialize.ReadShort(ref buffer);
            PulseShapeRise = BinSerialize.ReadUShort(ref buffer);
            PulseShapeDuration = BinSerialize.ReadUShort(ref buffer);
            PulseShapeDecay = BinSerialize.ReadUShort(ref buffer);
            PulseSpacing = BinSerialize.ReadUShort(ref buffer);
            ReqFreq = BinSerialize.ReadUShort(ref buffer);
            HipFreq = BinSerialize.ReadUShort(ref buffer);
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
            BinSerialize.WriteULong(ref buffer,RxFreq);
            BinSerialize.WriteULong(ref buffer,TxFreq);
            BinSerialize.WriteUInt(ref buffer,Index);
            BinSerialize.WriteFloat(ref buffer,RxPower);
            BinSerialize.WriteFloat(ref buffer,RxFieldStrength);
            BinSerialize.WriteFloat(ref buffer,RxSignalOverflow);
            BinSerialize.WriteFloat(ref buffer,RxGain);
            BinSerialize.WriteFloat(ref buffer,TxPower);
            BinSerialize.WriteFloat(ref buffer,TxGain);
            BinSerialize.WriteFloat(ref buffer,Distance);
            BinSerialize.WriteFloat(ref buffer,ReplyEfficiency);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            BinSerialize.WriteShort(ref buffer,RxFreqOffset);
            BinSerialize.WriteUShort(ref buffer,PulseShapeRise);
            BinSerialize.WriteUShort(ref buffer,PulseShapeDuration);
            BinSerialize.WriteUShort(ref buffer,PulseShapeDecay);
            BinSerialize.WriteUShort(ref buffer,PulseSpacing);
            BinSerialize.WriteUShort(ref buffer,ReqFreq);
            BinSerialize.WriteUShort(ref buffer,HipFreq);
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
            UInt64Type.Accept(visitor,RxFreqField, RxFreqField.DataType, ref _rxFreq);    
            UInt64Type.Accept(visitor,TxFreqField, TxFreqField.DataType, ref _txFreq);    
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    
            FloatType.Accept(visitor,RxPowerField, RxPowerField.DataType, ref _rxPower);    
            FloatType.Accept(visitor,RxFieldStrengthField, RxFieldStrengthField.DataType, ref _rxFieldStrength);    
            FloatType.Accept(visitor,RxSignalOverflowField, RxSignalOverflowField.DataType, ref _rxSignalOverflow);    
            FloatType.Accept(visitor,RxGainField, RxGainField.DataType, ref _rxGain);    
            FloatType.Accept(visitor,TxPowerField, TxPowerField.DataType, ref _txPower);    
            FloatType.Accept(visitor,TxGainField, TxGainField.DataType, ref _txGain);    
            FloatType.Accept(visitor,DistanceField, DistanceField.DataType, ref _distance);    
            FloatType.Accept(visitor,ReplyEfficiencyField, ReplyEfficiencyField.DataType, ref _replyEfficiency);    
            Int16Type.Accept(visitor,MeasureTimeField, MeasureTimeField.DataType, ref _measureTime);
            Int16Type.Accept(visitor,RxFreqOffsetField, RxFreqOffsetField.DataType, ref _rxFreqOffset);
            UInt16Type.Accept(visitor,PulseShapeRiseField, PulseShapeRiseField.DataType, ref _pulseShapeRise);    
            UInt16Type.Accept(visitor,PulseShapeDurationField, PulseShapeDurationField.DataType, ref _pulseShapeDuration);    
            UInt16Type.Accept(visitor,PulseShapeDecayField, PulseShapeDecayField.DataType, ref _pulseShapeDecay);    
            UInt16Type.Accept(visitor,PulseSpacingField, PulseSpacingField.DataType, ref _pulseSpacing);    
            UInt16Type.Accept(visitor,ReqFreqField, ReqFreqField.DataType, ref _reqFreq);    
            UInt16Type.Accept(visitor,HipFreqField, HipFreqField.DataType, ref _hipFreq);    
            Int8Type.Accept(visitor,PulseShapeAmplitudeField, PulseShapeAmplitudeField.DataType, ref _pulseShapeAmplitude);                
            ArrayType.Accept(visitor,CodeIdField, CodeIdField.DataType, 4, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref CodeId[index]));

        }

        /// <summary>
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
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
        /// Receive power field strength
        /// OriginName: rx_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field RxFieldStrengthField = new Field.Builder()
            .Name(nameof(RxFieldStrength))
            .Title("rx_field_strength")
            .Description("Receive power field strength")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _rxFieldStrength;
        public float RxFieldStrength { get => _rxFieldStrength; set => _rxFieldStrength = value; }
        /// <summary>
        /// Signal overflow indicator (≤0.2 — too low, ≥0.8 — too high)
        /// OriginName: rx_signal_overflow, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field RxSignalOverflowField = new Field.Builder()
            .Name(nameof(RxSignalOverflow))
            .Title("rx_signal_overflow")
            .Description("Signal overflow indicator (\u22640.2 \u2014 too low, \u22650.8 \u2014 too high)")
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
        /// Measure time
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field MeasureTimeField = new Field.Builder()
            .Name(nameof(MeasureTime))
            .Title("measure_time")
            .Description("Measure time")
.Units(@"ms")
            .DataType(Int16Type.Default)
        .Build();
        private short _measureTime;
        public short MeasureTime { get => _measureTime; set => _measureTime = value; }
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
        
        public const byte CrcExtra = 59;
        
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
        public byte GetMaxByteSize() => 130; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 130; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +8 // uint64_t rx_freq
            +4 // uint32_t index
            +4 // float rx_power
            +4 // float rx_field_strength
            +4 // float rx_signal_overflow
            +4 // float rx_gain
            +4 // float am_90
            +4 // float am_150
            +4 // float phi_90_low_vs_high
            +4 // float phi_150_low_vs_high
            +4 // float low_power
            +4 // float low_am_90
            +4 // float low_am_150
            +4 // float high_power
            +4 // float high_am_90
            +4 // float high_am_150
            +4 // float code_id_am_1020
            +4 // float code_id_am_min_1020
            +4 // float code_id_am_max_1020
            +2 // int16_t measure_time
            +2 // int16_t rx_freq_offset
            +2 // int16_t freq_90
            +2 // int16_t freq_150
            +2 // int16_t low_carrier_offset
            +2 // int16_t low_freq_90
            +2 // int16_t low_freq_150
            +2 // int16_t high_carrier_offset
            +2 // int16_t high_freq_90
            +2 // int16_t high_freq_150
            +2 // uint16_t code_id_dot_time
            +2 // uint16_t code_id_dash_time
            +2 // uint16_t code_id_symbol_pause
            +2 // uint16_t code_id_char_pause
            +2 // uint16_t code_id_delay
            +CodeId.Length // char[4] code_id
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            RxFreq = BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);
            RxPower = BinSerialize.ReadFloat(ref buffer);
            RxFieldStrength = BinSerialize.ReadFloat(ref buffer);
            RxSignalOverflow = BinSerialize.ReadFloat(ref buffer);
            RxGain = BinSerialize.ReadFloat(ref buffer);
            Am90 = BinSerialize.ReadFloat(ref buffer);
            Am150 = BinSerialize.ReadFloat(ref buffer);
            Phi90LowVsHigh = BinSerialize.ReadFloat(ref buffer);
            Phi150LowVsHigh = BinSerialize.ReadFloat(ref buffer);
            LowPower = BinSerialize.ReadFloat(ref buffer);
            LowAm90 = BinSerialize.ReadFloat(ref buffer);
            LowAm150 = BinSerialize.ReadFloat(ref buffer);
            HighPower = BinSerialize.ReadFloat(ref buffer);
            HighAm90 = BinSerialize.ReadFloat(ref buffer);
            HighAm150 = BinSerialize.ReadFloat(ref buffer);
            CodeIdAm1020 = BinSerialize.ReadFloat(ref buffer);
            CodeIdAmMin1020 = BinSerialize.ReadFloat(ref buffer);
            CodeIdAmMax1020 = BinSerialize.ReadFloat(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            RxFreqOffset = BinSerialize.ReadShort(ref buffer);
            Freq90 = BinSerialize.ReadShort(ref buffer);
            Freq150 = BinSerialize.ReadShort(ref buffer);
            LowCarrierOffset = BinSerialize.ReadShort(ref buffer);
            LowFreq90 = BinSerialize.ReadShort(ref buffer);
            LowFreq150 = BinSerialize.ReadShort(ref buffer);
            HighCarrierOffset = BinSerialize.ReadShort(ref buffer);
            HighFreq90 = BinSerialize.ReadShort(ref buffer);
            HighFreq150 = BinSerialize.ReadShort(ref buffer);
            CodeIdDotTime = BinSerialize.ReadUShort(ref buffer);
            CodeIdDashTime = BinSerialize.ReadUShort(ref buffer);
            CodeIdSymbolPause = BinSerialize.ReadUShort(ref buffer);
            CodeIdCharPause = BinSerialize.ReadUShort(ref buffer);
            CodeIdDelay = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/130 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
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
            BinSerialize.WriteULong(ref buffer,RxFreq);
            BinSerialize.WriteUInt(ref buffer,Index);
            BinSerialize.WriteFloat(ref buffer,RxPower);
            BinSerialize.WriteFloat(ref buffer,RxFieldStrength);
            BinSerialize.WriteFloat(ref buffer,RxSignalOverflow);
            BinSerialize.WriteFloat(ref buffer,RxGain);
            BinSerialize.WriteFloat(ref buffer,Am90);
            BinSerialize.WriteFloat(ref buffer,Am150);
            BinSerialize.WriteFloat(ref buffer,Phi90LowVsHigh);
            BinSerialize.WriteFloat(ref buffer,Phi150LowVsHigh);
            BinSerialize.WriteFloat(ref buffer,LowPower);
            BinSerialize.WriteFloat(ref buffer,LowAm90);
            BinSerialize.WriteFloat(ref buffer,LowAm150);
            BinSerialize.WriteFloat(ref buffer,HighPower);
            BinSerialize.WriteFloat(ref buffer,HighAm90);
            BinSerialize.WriteFloat(ref buffer,HighAm150);
            BinSerialize.WriteFloat(ref buffer,CodeIdAm1020);
            BinSerialize.WriteFloat(ref buffer,CodeIdAmMin1020);
            BinSerialize.WriteFloat(ref buffer,CodeIdAmMax1020);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            BinSerialize.WriteShort(ref buffer,RxFreqOffset);
            BinSerialize.WriteShort(ref buffer,Freq90);
            BinSerialize.WriteShort(ref buffer,Freq150);
            BinSerialize.WriteShort(ref buffer,LowCarrierOffset);
            BinSerialize.WriteShort(ref buffer,LowFreq90);
            BinSerialize.WriteShort(ref buffer,LowFreq150);
            BinSerialize.WriteShort(ref buffer,HighCarrierOffset);
            BinSerialize.WriteShort(ref buffer,HighFreq90);
            BinSerialize.WriteShort(ref buffer,HighFreq150);
            BinSerialize.WriteUShort(ref buffer,CodeIdDotTime);
            BinSerialize.WriteUShort(ref buffer,CodeIdDashTime);
            BinSerialize.WriteUShort(ref buffer,CodeIdSymbolPause);
            BinSerialize.WriteUShort(ref buffer,CodeIdCharPause);
            BinSerialize.WriteUShort(ref buffer,CodeIdDelay);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId)
                {
                    Encoding.ASCII.GetBytes(charPointer, CodeId.Length, bytePointer, CodeId.Length);
                }
            }
            buffer = buffer.Slice(CodeId.Length);
            
            /* PayloadByteSize = 130 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt64Type.Accept(visitor,RxFreqField, RxFreqField.DataType, ref _rxFreq);    
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    
            FloatType.Accept(visitor,RxPowerField, RxPowerField.DataType, ref _rxPower);    
            FloatType.Accept(visitor,RxFieldStrengthField, RxFieldStrengthField.DataType, ref _rxFieldStrength);    
            FloatType.Accept(visitor,RxSignalOverflowField, RxSignalOverflowField.DataType, ref _rxSignalOverflow);    
            FloatType.Accept(visitor,RxGainField, RxGainField.DataType, ref _rxGain);    
            FloatType.Accept(visitor,Am90Field, Am90Field.DataType, ref _am90);    
            FloatType.Accept(visitor,Am150Field, Am150Field.DataType, ref _am150);    
            FloatType.Accept(visitor,Phi90LowVsHighField, Phi90LowVsHighField.DataType, ref _phi90LowVsHigh);    
            FloatType.Accept(visitor,Phi150LowVsHighField, Phi150LowVsHighField.DataType, ref _phi150LowVsHigh);    
            FloatType.Accept(visitor,LowPowerField, LowPowerField.DataType, ref _lowPower);    
            FloatType.Accept(visitor,LowAm90Field, LowAm90Field.DataType, ref _lowAm90);    
            FloatType.Accept(visitor,LowAm150Field, LowAm150Field.DataType, ref _lowAm150);    
            FloatType.Accept(visitor,HighPowerField, HighPowerField.DataType, ref _highPower);    
            FloatType.Accept(visitor,HighAm90Field, HighAm90Field.DataType, ref _highAm90);    
            FloatType.Accept(visitor,HighAm150Field, HighAm150Field.DataType, ref _highAm150);    
            FloatType.Accept(visitor,CodeIdAm1020Field, CodeIdAm1020Field.DataType, ref _codeIdAm1020);    
            FloatType.Accept(visitor,CodeIdAmMin1020Field, CodeIdAmMin1020Field.DataType, ref _codeIdAmMin1020);    
            FloatType.Accept(visitor,CodeIdAmMax1020Field, CodeIdAmMax1020Field.DataType, ref _codeIdAmMax1020);    
            Int16Type.Accept(visitor,MeasureTimeField, MeasureTimeField.DataType, ref _measureTime);
            Int16Type.Accept(visitor,RxFreqOffsetField, RxFreqOffsetField.DataType, ref _rxFreqOffset);
            Int16Type.Accept(visitor,Freq90Field, Freq90Field.DataType, ref _freq90);
            Int16Type.Accept(visitor,Freq150Field, Freq150Field.DataType, ref _freq150);
            Int16Type.Accept(visitor,LowCarrierOffsetField, LowCarrierOffsetField.DataType, ref _lowCarrierOffset);
            Int16Type.Accept(visitor,LowFreq90Field, LowFreq90Field.DataType, ref _lowFreq90);
            Int16Type.Accept(visitor,LowFreq150Field, LowFreq150Field.DataType, ref _lowFreq150);
            Int16Type.Accept(visitor,HighCarrierOffsetField, HighCarrierOffsetField.DataType, ref _highCarrierOffset);
            Int16Type.Accept(visitor,HighFreq90Field, HighFreq90Field.DataType, ref _highFreq90);
            Int16Type.Accept(visitor,HighFreq150Field, HighFreq150Field.DataType, ref _highFreq150);
            UInt16Type.Accept(visitor,CodeIdDotTimeField, CodeIdDotTimeField.DataType, ref _codeIdDotTime);    
            UInt16Type.Accept(visitor,CodeIdDashTimeField, CodeIdDashTimeField.DataType, ref _codeIdDashTime);    
            UInt16Type.Accept(visitor,CodeIdSymbolPauseField, CodeIdSymbolPauseField.DataType, ref _codeIdSymbolPause);    
            UInt16Type.Accept(visitor,CodeIdCharPauseField, CodeIdCharPauseField.DataType, ref _codeIdCharPause);    
            UInt16Type.Accept(visitor,CodeIdDelayField, CodeIdDelayField.DataType, ref _codeIdDelay);    
            ArrayType.Accept(visitor,CodeIdField, CodeIdField.DataType, 4, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref CodeId[index]));

        }

        /// <summary>
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
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
        /// Receive power field strength
        /// OriginName: rx_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field RxFieldStrengthField = new Field.Builder()
            .Name(nameof(RxFieldStrength))
            .Title("rx_field_strength")
            .Description("Receive power field strength")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _rxFieldStrength;
        public float RxFieldStrength { get => _rxFieldStrength; set => _rxFieldStrength = value; }
        /// <summary>
        /// Signal overflow indicator (≤0.2 — too low, ≥0.8 — too high)
        /// OriginName: rx_signal_overflow, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field RxSignalOverflowField = new Field.Builder()
            .Name(nameof(RxSignalOverflow))
            .Title("rx_signal_overflow")
            .Description("Signal overflow indicator (\u22640.2 \u2014 too low, \u22650.8 \u2014 too high)")
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
        /// Total amplitude modulation of 90Hz
        /// OriginName: am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am90Field = new Field.Builder()
            .Name(nameof(Am90))
            .Title("am_90")
            .Description("Total amplitude modulation of 90Hz")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am90;
        public float Am90 { get => _am90; set => _am90 = value; }
        /// <summary>
        /// Total amplitude modulation of 150Hz
        /// OriginName: am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am150Field = new Field.Builder()
            .Name(nameof(Am150))
            .Title("am_150")
            .Description("Total amplitude modulation of 150Hz")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am150;
        public float Am150 { get => _am150; set => _am150 = value; }
        /// <summary>
        ///  Phase difference 90 Hz low and high freq channel
        /// OriginName: phi_90_low_vs_high, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi90LowVsHighField = new Field.Builder()
            .Name(nameof(Phi90LowVsHigh))
            .Title("phi_90_low_vs_high")
            .Description(" Phase difference 90 Hz low and high freq channel")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _phi90LowVsHigh;
        public float Phi90LowVsHigh { get => _phi90LowVsHigh; set => _phi90LowVsHigh = value; }
        /// <summary>
        /// Phase difference 150 Hz low and high freq channel
        /// OriginName: phi_150_low_vs_high, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi150LowVsHighField = new Field.Builder()
            .Name(nameof(Phi150LowVsHigh))
            .Title("phi_150_low_vs_high")
            .Description("Phase difference 150 Hz low and high freq channel")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _phi150LowVsHigh;
        public float Phi150LowVsHigh { get => _phi150LowVsHigh; set => _phi150LowVsHigh = value; }
        /// <summary>
        /// Input power of low freq channel
        /// OriginName: low_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field LowPowerField = new Field.Builder()
            .Name(nameof(LowPower))
            .Title("low_power")
            .Description("Input power of low freq channel")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _lowPower;
        public float LowPower { get => _lowPower; set => _lowPower = value; }
        /// <summary>
        /// Aplitude modulation of 90Hz of low freq channel
        /// OriginName: low_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field LowAm90Field = new Field.Builder()
            .Name(nameof(LowAm90))
            .Title("low_am_90")
            .Description("Aplitude modulation of 90Hz of low freq channel")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _lowAm90;
        public float LowAm90 { get => _lowAm90; set => _lowAm90 = value; }
        /// <summary>
        /// Aplitude modulation of 150Hz of low freq channel
        /// OriginName: low_am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field LowAm150Field = new Field.Builder()
            .Name(nameof(LowAm150))
            .Title("low_am_150")
            .Description("Aplitude modulation of 150Hz of low freq channel")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _lowAm150;
        public float LowAm150 { get => _lowAm150; set => _lowAm150 = value; }
        /// <summary>
        /// Input power of high freq channel
        /// OriginName: high_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field HighPowerField = new Field.Builder()
            .Name(nameof(HighPower))
            .Title("high_power")
            .Description("Input power of high freq channel")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _highPower;
        public float HighPower { get => _highPower; set => _highPower = value; }
        /// <summary>
        /// Aplitude modulation of 90Hz of high freq channel
        /// OriginName: high_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field HighAm90Field = new Field.Builder()
            .Name(nameof(HighAm90))
            .Title("high_am_90")
            .Description("Aplitude modulation of 90Hz of high freq channel")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _highAm90;
        public float HighAm90 { get => _highAm90; set => _highAm90 = value; }
        /// <summary>
        /// Aplitude modulation of 150Hz of high freq channel
        /// OriginName: high_am_150, Units: % E2, IsExtended: false
        /// </summary>
        public static readonly Field HighAm150Field = new Field.Builder()
            .Name(nameof(HighAm150))
            .Title("high_am_150")
            .Description("Aplitude modulation of 150Hz of high freq channel")
.Units(@"% E2")
            .DataType(FloatType.Default)
        .Build();
        private float _highAm150;
        public float HighAm150 { get => _highAm150; set => _highAm150 = value; }
        /// <summary>
        /// Current amplitude modulation of Code ID
        /// OriginName: code_id_am_1020, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdAm1020Field = new Field.Builder()
            .Name(nameof(CodeIdAm1020))
            .Title("code_id_am_1020")
            .Description("Current amplitude modulation of Code ID")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _codeIdAm1020;
        public float CodeIdAm1020 { get => _codeIdAm1020; set => _codeIdAm1020 = value; }
        /// <summary>
        /// Min amplitude modulation of Code ID
        /// OriginName: code_id_am_min_1020, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdAmMin1020Field = new Field.Builder()
            .Name(nameof(CodeIdAmMin1020))
            .Title("code_id_am_min_1020")
            .Description("Min amplitude modulation of Code ID")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _codeIdAmMin1020;
        public float CodeIdAmMin1020 { get => _codeIdAmMin1020; set => _codeIdAmMin1020 = value; }
        /// <summary>
        /// Max amplitude modulation of Code ID
        /// OriginName: code_id_am_max_1020, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdAmMax1020Field = new Field.Builder()
            .Name(nameof(CodeIdAmMax1020))
            .Title("code_id_am_max_1020")
            .Description("Max amplitude modulation of Code ID")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _codeIdAmMax1020;
        public float CodeIdAmMax1020 { get => _codeIdAmMax1020; set => _codeIdAmMax1020 = value; }
        /// <summary>
        /// Measure time
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field MeasureTimeField = new Field.Builder()
            .Name(nameof(MeasureTime))
            .Title("measure_time")
            .Description("Measure time")
.Units(@"ms")
            .DataType(Int16Type.Default)
        .Build();
        private short _measureTime;
        public short MeasureTime { get => _measureTime; set => _measureTime = value; }
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
        /// Total frequency offset of signal 90 Hz
        /// OriginName: freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq90Field = new Field.Builder()
            .Name(nameof(Freq90))
            .Title("freq_90")
            .Description("Total frequency offset of signal 90 Hz")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq90;
        public short Freq90 { get => _freq90; set => _freq90 = value; }
        /// <summary>
        /// Total frequency offset of signal 150 Hz
        /// OriginName: freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq150Field = new Field.Builder()
            .Name(nameof(Freq150))
            .Title("freq_150")
            .Description("Total frequency offset of signal 150 Hz")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq150;
        public short Freq150 { get => _freq150; set => _freq150 = value; }
        /// <summary>
        /// Carrier frequency offset of low freq channel
        /// OriginName: low_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field LowCarrierOffsetField = new Field.Builder()
            .Name(nameof(LowCarrierOffset))
            .Title("low_carrier_offset")
            .Description("Carrier frequency offset of low freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _lowCarrierOffset;
        public short LowCarrierOffset { get => _lowCarrierOffset; set => _lowCarrierOffset = value; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of low freq channel
        /// OriginName: low_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field LowFreq90Field = new Field.Builder()
            .Name(nameof(LowFreq90))
            .Title("low_freq_90")
            .Description("Frequency offset of signal 90 Hz of low freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _lowFreq90;
        public short LowFreq90 { get => _lowFreq90; set => _lowFreq90 = value; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of low freq channel
        /// OriginName: low_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field LowFreq150Field = new Field.Builder()
            .Name(nameof(LowFreq150))
            .Title("low_freq_150")
            .Description("Frequency offset of signal 150 Hz of low freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _lowFreq150;
        public short LowFreq150 { get => _lowFreq150; set => _lowFreq150 = value; }
        /// <summary>
        /// Carrier frequency offset of high freq channel
        /// OriginName: high_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field HighCarrierOffsetField = new Field.Builder()
            .Name(nameof(HighCarrierOffset))
            .Title("high_carrier_offset")
            .Description("Carrier frequency offset of high freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _highCarrierOffset;
        public short HighCarrierOffset { get => _highCarrierOffset; set => _highCarrierOffset = value; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of high freq channel
        /// OriginName: high_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field HighFreq90Field = new Field.Builder()
            .Name(nameof(HighFreq90))
            .Title("high_freq_90")
            .Description("Frequency offset of signal 90 Hz of high freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _highFreq90;
        public short HighFreq90 { get => _highFreq90; set => _highFreq90 = value; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of high freq channel
        /// OriginName: high_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field HighFreq150Field = new Field.Builder()
            .Name(nameof(HighFreq150))
            .Title("high_freq_150")
            .Description("Frequency offset of signal 150 Hz of high freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _highFreq150;
        public short HighFreq150 { get => _highFreq150; set => _highFreq150 = value; }
        /// <summary>
        /// Dot time
        /// OriginName: code_id_dot_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdDotTimeField = new Field.Builder()
            .Name(nameof(CodeIdDotTime))
            .Title("code_id_dot_time")
            .Description("Dot time")
.Units(@"ms")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _codeIdDotTime;
        public ushort CodeIdDotTime { get => _codeIdDotTime; set => _codeIdDotTime = value; }
        /// <summary>
        /// Dash time
        /// OriginName: code_id_dash_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdDashTimeField = new Field.Builder()
            .Name(nameof(CodeIdDashTime))
            .Title("code_id_dash_time")
            .Description("Dash time")
.Units(@"ms")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _codeIdDashTime;
        public ushort CodeIdDashTime { get => _codeIdDashTime; set => _codeIdDashTime = value; }
        /// <summary>
        /// Symbol pause time
        /// OriginName: code_id_symbol_pause, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdSymbolPauseField = new Field.Builder()
            .Name(nameof(CodeIdSymbolPause))
            .Title("code_id_symbol_pause")
            .Description("Symbol pause time")
.Units(@"ms")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _codeIdSymbolPause;
        public ushort CodeIdSymbolPause { get => _codeIdSymbolPause; set => _codeIdSymbolPause = value; }
        /// <summary>
        /// Char pause time
        /// OriginName: code_id_char_pause, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdCharPauseField = new Field.Builder()
            .Name(nameof(CodeIdCharPause))
            .Title("code_id_char_pause")
            .Description("Char pause time")
.Units(@"ms")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _codeIdCharPause;
        public ushort CodeIdCharPause { get => _codeIdCharPause; set => _codeIdCharPause = value; }
        /// <summary>
        /// Last update
        /// OriginName: code_id_delay, Units: s, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdDelayField = new Field.Builder()
            .Name(nameof(CodeIdDelay))
            .Title("code_id_delay")
            .Description("Last update")
.Units(@"s")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _codeIdDelay;
        public ushort CodeIdDelay { get => _codeIdDelay; set => _codeIdDelay = value; }
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
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_RX_GP mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_RX_GP
    /// </summary>
    public class AsvRsgaRttRxGpPacket : MavlinkV2Message<AsvRsgaRttRxGpPayload>
    {
        public const int MessageId = 13462;
        
        public const byte CrcExtra = 130;
        
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
        public byte GetMaxByteSize() => 104; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 104; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +8 // uint64_t rx_freq
            +4 // uint32_t index
            +4 // float rx_power
            +4 // float rx_field_strength
            +4 // float rx_signal_overflow
            +4 // float rx_gain
            +4 // float low_power
            +4 // float low_am_90
            +4 // float low_am_150
            +4 // float high_power
            +4 // float high_am_90
            +4 // float high_am_150
            +4 // float am_90
            +4 // float am_150
            +4 // float phi_90_low_vs_high
            +4 // float phi_150_low_vs_high
            +2 // int16_t measure_time
            +2 // int16_t rx_freq_offset
            +2 // int16_t low_carrier_offset
            +2 // int16_t low_freq_90
            +2 // int16_t low_freq_150
            +2 // int16_t high_carrier_offset
            +2 // int16_t high_freq_90
            +2 // int16_t high_freq_150
            +2 // int16_t freq_90
            +2 // int16_t freq_150
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            RxFreq = BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);
            RxPower = BinSerialize.ReadFloat(ref buffer);
            RxFieldStrength = BinSerialize.ReadFloat(ref buffer);
            RxSignalOverflow = BinSerialize.ReadFloat(ref buffer);
            RxGain = BinSerialize.ReadFloat(ref buffer);
            LowPower = BinSerialize.ReadFloat(ref buffer);
            LowAm90 = BinSerialize.ReadFloat(ref buffer);
            LowAm150 = BinSerialize.ReadFloat(ref buffer);
            HighPower = BinSerialize.ReadFloat(ref buffer);
            HighAm90 = BinSerialize.ReadFloat(ref buffer);
            HighAm150 = BinSerialize.ReadFloat(ref buffer);
            Am90 = BinSerialize.ReadFloat(ref buffer);
            Am150 = BinSerialize.ReadFloat(ref buffer);
            Phi90LowVsHigh = BinSerialize.ReadFloat(ref buffer);
            Phi150LowVsHigh = BinSerialize.ReadFloat(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            RxFreqOffset = BinSerialize.ReadShort(ref buffer);
            LowCarrierOffset = BinSerialize.ReadShort(ref buffer);
            LowFreq90 = BinSerialize.ReadShort(ref buffer);
            LowFreq150 = BinSerialize.ReadShort(ref buffer);
            HighCarrierOffset = BinSerialize.ReadShort(ref buffer);
            HighFreq90 = BinSerialize.ReadShort(ref buffer);
            HighFreq150 = BinSerialize.ReadShort(ref buffer);
            Freq90 = BinSerialize.ReadShort(ref buffer);
            Freq150 = BinSerialize.ReadShort(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteULong(ref buffer,RxFreq);
            BinSerialize.WriteUInt(ref buffer,Index);
            BinSerialize.WriteFloat(ref buffer,RxPower);
            BinSerialize.WriteFloat(ref buffer,RxFieldStrength);
            BinSerialize.WriteFloat(ref buffer,RxSignalOverflow);
            BinSerialize.WriteFloat(ref buffer,RxGain);
            BinSerialize.WriteFloat(ref buffer,LowPower);
            BinSerialize.WriteFloat(ref buffer,LowAm90);
            BinSerialize.WriteFloat(ref buffer,LowAm150);
            BinSerialize.WriteFloat(ref buffer,HighPower);
            BinSerialize.WriteFloat(ref buffer,HighAm90);
            BinSerialize.WriteFloat(ref buffer,HighAm150);
            BinSerialize.WriteFloat(ref buffer,Am90);
            BinSerialize.WriteFloat(ref buffer,Am150);
            BinSerialize.WriteFloat(ref buffer,Phi90LowVsHigh);
            BinSerialize.WriteFloat(ref buffer,Phi150LowVsHigh);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            BinSerialize.WriteShort(ref buffer,RxFreqOffset);
            BinSerialize.WriteShort(ref buffer,LowCarrierOffset);
            BinSerialize.WriteShort(ref buffer,LowFreq90);
            BinSerialize.WriteShort(ref buffer,LowFreq150);
            BinSerialize.WriteShort(ref buffer,HighCarrierOffset);
            BinSerialize.WriteShort(ref buffer,HighFreq90);
            BinSerialize.WriteShort(ref buffer,HighFreq150);
            BinSerialize.WriteShort(ref buffer,Freq90);
            BinSerialize.WriteShort(ref buffer,Freq150);
            /* PayloadByteSize = 104 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt64Type.Accept(visitor,RxFreqField, RxFreqField.DataType, ref _rxFreq);    
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    
            FloatType.Accept(visitor,RxPowerField, RxPowerField.DataType, ref _rxPower);    
            FloatType.Accept(visitor,RxFieldStrengthField, RxFieldStrengthField.DataType, ref _rxFieldStrength);    
            FloatType.Accept(visitor,RxSignalOverflowField, RxSignalOverflowField.DataType, ref _rxSignalOverflow);    
            FloatType.Accept(visitor,RxGainField, RxGainField.DataType, ref _rxGain);    
            FloatType.Accept(visitor,LowPowerField, LowPowerField.DataType, ref _lowPower);    
            FloatType.Accept(visitor,LowAm90Field, LowAm90Field.DataType, ref _lowAm90);    
            FloatType.Accept(visitor,LowAm150Field, LowAm150Field.DataType, ref _lowAm150);    
            FloatType.Accept(visitor,HighPowerField, HighPowerField.DataType, ref _highPower);    
            FloatType.Accept(visitor,HighAm90Field, HighAm90Field.DataType, ref _highAm90);    
            FloatType.Accept(visitor,HighAm150Field, HighAm150Field.DataType, ref _highAm150);    
            FloatType.Accept(visitor,Am90Field, Am90Field.DataType, ref _am90);    
            FloatType.Accept(visitor,Am150Field, Am150Field.DataType, ref _am150);    
            FloatType.Accept(visitor,Phi90LowVsHighField, Phi90LowVsHighField.DataType, ref _phi90LowVsHigh);    
            FloatType.Accept(visitor,Phi150LowVsHighField, Phi150LowVsHighField.DataType, ref _phi150LowVsHigh);    
            Int16Type.Accept(visitor,MeasureTimeField, MeasureTimeField.DataType, ref _measureTime);
            Int16Type.Accept(visitor,RxFreqOffsetField, RxFreqOffsetField.DataType, ref _rxFreqOffset);
            Int16Type.Accept(visitor,LowCarrierOffsetField, LowCarrierOffsetField.DataType, ref _lowCarrierOffset);
            Int16Type.Accept(visitor,LowFreq90Field, LowFreq90Field.DataType, ref _lowFreq90);
            Int16Type.Accept(visitor,LowFreq150Field, LowFreq150Field.DataType, ref _lowFreq150);
            Int16Type.Accept(visitor,HighCarrierOffsetField, HighCarrierOffsetField.DataType, ref _highCarrierOffset);
            Int16Type.Accept(visitor,HighFreq90Field, HighFreq90Field.DataType, ref _highFreq90);
            Int16Type.Accept(visitor,HighFreq150Field, HighFreq150Field.DataType, ref _highFreq150);
            Int16Type.Accept(visitor,Freq90Field, Freq90Field.DataType, ref _freq90);
            Int16Type.Accept(visitor,Freq150Field, Freq150Field.DataType, ref _freq150);

        }

        /// <summary>
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
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
        /// Receive power field strength
        /// OriginName: rx_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field RxFieldStrengthField = new Field.Builder()
            .Name(nameof(RxFieldStrength))
            .Title("rx_field_strength")
            .Description("Receive power field strength")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _rxFieldStrength;
        public float RxFieldStrength { get => _rxFieldStrength; set => _rxFieldStrength = value; }
        /// <summary>
        /// Signal overflow indicator (≤0.2 — too low, ≥0.8 — too high)
        /// OriginName: rx_signal_overflow, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field RxSignalOverflowField = new Field.Builder()
            .Name(nameof(RxSignalOverflow))
            .Title("rx_signal_overflow")
            .Description("Signal overflow indicator (\u22640.2 \u2014 too low, \u22650.8 \u2014 too high)")
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
        /// Input power of low freq channel
        /// OriginName: low_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field LowPowerField = new Field.Builder()
            .Name(nameof(LowPower))
            .Title("low_power")
            .Description("Input power of low freq channel")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _lowPower;
        public float LowPower { get => _lowPower; set => _lowPower = value; }
        /// <summary>
        /// Aplitude modulation of 90Hz of low freq channel
        /// OriginName: low_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field LowAm90Field = new Field.Builder()
            .Name(nameof(LowAm90))
            .Title("low_am_90")
            .Description("Aplitude modulation of 90Hz of low freq channel")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _lowAm90;
        public float LowAm90 { get => _lowAm90; set => _lowAm90 = value; }
        /// <summary>
        /// Aplitude modulation of 150Hz of low freq channel
        /// OriginName: low_am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field LowAm150Field = new Field.Builder()
            .Name(nameof(LowAm150))
            .Title("low_am_150")
            .Description("Aplitude modulation of 150Hz of low freq channel")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _lowAm150;
        public float LowAm150 { get => _lowAm150; set => _lowAm150 = value; }
        /// <summary>
        /// Input power of high freq channel
        /// OriginName: high_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field HighPowerField = new Field.Builder()
            .Name(nameof(HighPower))
            .Title("high_power")
            .Description("Input power of high freq channel")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _highPower;
        public float HighPower { get => _highPower; set => _highPower = value; }
        /// <summary>
        /// Aplitude modulation of 90Hz of high freq channel
        /// OriginName: high_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field HighAm90Field = new Field.Builder()
            .Name(nameof(HighAm90))
            .Title("high_am_90")
            .Description("Aplitude modulation of 90Hz of high freq channel")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _highAm90;
        public float HighAm90 { get => _highAm90; set => _highAm90 = value; }
        /// <summary>
        /// Aplitude modulation of 150Hz of high freq channel
        /// OriginName: high_am_150, Units: % E2, IsExtended: false
        /// </summary>
        public static readonly Field HighAm150Field = new Field.Builder()
            .Name(nameof(HighAm150))
            .Title("high_am_150")
            .Description("Aplitude modulation of 150Hz of high freq channel")
.Units(@"% E2")
            .DataType(FloatType.Default)
        .Build();
        private float _highAm150;
        public float HighAm150 { get => _highAm150; set => _highAm150 = value; }
        /// <summary>
        /// Total amplitude modulation of 90Hz
        /// OriginName: am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am90Field = new Field.Builder()
            .Name(nameof(Am90))
            .Title("am_90")
            .Description("Total amplitude modulation of 90Hz")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am90;
        public float Am90 { get => _am90; set => _am90 = value; }
        /// <summary>
        /// Total amplitude modulation of 150Hz
        /// OriginName: am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am150Field = new Field.Builder()
            .Name(nameof(Am150))
            .Title("am_150")
            .Description("Total amplitude modulation of 150Hz")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am150;
        public float Am150 { get => _am150; set => _am150 = value; }
        /// <summary>
        ///  Phase difference 90 Hz low and high freq channel
        /// OriginName: phi_90_low_vs_high, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi90LowVsHighField = new Field.Builder()
            .Name(nameof(Phi90LowVsHigh))
            .Title("phi_90_low_vs_high")
            .Description(" Phase difference 90 Hz low and high freq channel")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _phi90LowVsHigh;
        public float Phi90LowVsHigh { get => _phi90LowVsHigh; set => _phi90LowVsHigh = value; }
        /// <summary>
        /// Phase difference 150 Hz low and high freq channel
        /// OriginName: phi_150_low_vs_high, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi150LowVsHighField = new Field.Builder()
            .Name(nameof(Phi150LowVsHigh))
            .Title("phi_150_low_vs_high")
            .Description("Phase difference 150 Hz low and high freq channel")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _phi150LowVsHigh;
        public float Phi150LowVsHigh { get => _phi150LowVsHigh; set => _phi150LowVsHigh = value; }
        /// <summary>
        /// Measure time
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field MeasureTimeField = new Field.Builder()
            .Name(nameof(MeasureTime))
            .Title("measure_time")
            .Description("Measure time")
.Units(@"ms")
            .DataType(Int16Type.Default)
        .Build();
        private short _measureTime;
        public short MeasureTime { get => _measureTime; set => _measureTime = value; }
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
        /// Carrier frequency offset of low freq channel
        /// OriginName: low_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field LowCarrierOffsetField = new Field.Builder()
            .Name(nameof(LowCarrierOffset))
            .Title("low_carrier_offset")
            .Description("Carrier frequency offset of low freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _lowCarrierOffset;
        public short LowCarrierOffset { get => _lowCarrierOffset; set => _lowCarrierOffset = value; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of low freq channel
        /// OriginName: low_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field LowFreq90Field = new Field.Builder()
            .Name(nameof(LowFreq90))
            .Title("low_freq_90")
            .Description("Frequency offset of signal 90 Hz of low freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _lowFreq90;
        public short LowFreq90 { get => _lowFreq90; set => _lowFreq90 = value; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of low freq channel
        /// OriginName: low_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field LowFreq150Field = new Field.Builder()
            .Name(nameof(LowFreq150))
            .Title("low_freq_150")
            .Description("Frequency offset of signal 150 Hz of low freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _lowFreq150;
        public short LowFreq150 { get => _lowFreq150; set => _lowFreq150 = value; }
        /// <summary>
        /// Carrier frequency offset of high freq channel
        /// OriginName: high_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field HighCarrierOffsetField = new Field.Builder()
            .Name(nameof(HighCarrierOffset))
            .Title("high_carrier_offset")
            .Description("Carrier frequency offset of high freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _highCarrierOffset;
        public short HighCarrierOffset { get => _highCarrierOffset; set => _highCarrierOffset = value; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of high freq channel
        /// OriginName: high_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field HighFreq90Field = new Field.Builder()
            .Name(nameof(HighFreq90))
            .Title("high_freq_90")
            .Description("Frequency offset of signal 90 Hz of high freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _highFreq90;
        public short HighFreq90 { get => _highFreq90; set => _highFreq90 = value; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of high freq channel
        /// OriginName: high_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field HighFreq150Field = new Field.Builder()
            .Name(nameof(HighFreq150))
            .Title("high_freq_150")
            .Description("Frequency offset of signal 150 Hz of high freq channel")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _highFreq150;
        public short HighFreq150 { get => _highFreq150; set => _highFreq150 = value; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz
        /// OriginName: freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq90Field = new Field.Builder()
            .Name(nameof(Freq90))
            .Title("freq_90")
            .Description("Total frequency offset of signal 90 Hz")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq90;
        public short Freq90 { get => _freq90; set => _freq90 = value; }
        /// <summary>
        /// Total frequency offset of signal 150 Hz
        /// OriginName: freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq150Field = new Field.Builder()
            .Name(nameof(Freq150))
            .Title("freq_150")
            .Description("Total frequency offset of signal 150 Hz")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq150;
        public short Freq150 { get => _freq150; set => _freq150 = value; }
    }
    /// <summary>
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_RX_VOR mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_RX_VOR
    /// </summary>
    public class AsvRsgaRttRxVorPacket : MavlinkV2Message<AsvRsgaRttRxVorPayload>
    {
        public const int MessageId = 13463;
        
        public const byte CrcExtra = 94;
        
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
        public byte GetMaxByteSize() => 94; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 94; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +8 // uint64_t rx_freq
            +4 // uint32_t index
            +4 // float rx_power
            +4 // float rx_field_strength
            +4 // float rx_signal_overflow
            +4 // float rx_gain
            +4 // float azimuth
            +4 // float am_30
            +4 // float am_9960
            +4 // float deviation
            +4 // float code_id_am_1020
            +4 // float code_id_am_min_1020
            +4 // float code_id_am_max_1020
            +2 // int16_t measure_time
            +2 // int16_t rx_freq_offset
            +2 // int16_t freq_30
            +2 // int16_t freq_9960
            +2 // uint16_t code_id_dot_time
            +2 // uint16_t code_id_dash_time
            +2 // uint16_t code_id_symbol_pause
            +2 // uint16_t code_id_char_pause
            +2 // uint16_t code_id_delay
            +CodeId.Length // char[4] code_id
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            RxFreq = BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);
            RxPower = BinSerialize.ReadFloat(ref buffer);
            RxFieldStrength = BinSerialize.ReadFloat(ref buffer);
            RxSignalOverflow = BinSerialize.ReadFloat(ref buffer);
            RxGain = BinSerialize.ReadFloat(ref buffer);
            Azimuth = BinSerialize.ReadFloat(ref buffer);
            Am30 = BinSerialize.ReadFloat(ref buffer);
            Am9960 = BinSerialize.ReadFloat(ref buffer);
            Deviation = BinSerialize.ReadFloat(ref buffer);
            CodeIdAm1020 = BinSerialize.ReadFloat(ref buffer);
            CodeIdAmMin1020 = BinSerialize.ReadFloat(ref buffer);
            CodeIdAmMax1020 = BinSerialize.ReadFloat(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            RxFreqOffset = BinSerialize.ReadShort(ref buffer);
            Freq30 = BinSerialize.ReadShort(ref buffer);
            Freq9960 = BinSerialize.ReadShort(ref buffer);
            CodeIdDotTime = BinSerialize.ReadUShort(ref buffer);
            CodeIdDashTime = BinSerialize.ReadUShort(ref buffer);
            CodeIdSymbolPause = BinSerialize.ReadUShort(ref buffer);
            CodeIdCharPause = BinSerialize.ReadUShort(ref buffer);
            CodeIdDelay = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/94 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
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
            BinSerialize.WriteULong(ref buffer,RxFreq);
            BinSerialize.WriteUInt(ref buffer,Index);
            BinSerialize.WriteFloat(ref buffer,RxPower);
            BinSerialize.WriteFloat(ref buffer,RxFieldStrength);
            BinSerialize.WriteFloat(ref buffer,RxSignalOverflow);
            BinSerialize.WriteFloat(ref buffer,RxGain);
            BinSerialize.WriteFloat(ref buffer,Azimuth);
            BinSerialize.WriteFloat(ref buffer,Am30);
            BinSerialize.WriteFloat(ref buffer,Am9960);
            BinSerialize.WriteFloat(ref buffer,Deviation);
            BinSerialize.WriteFloat(ref buffer,CodeIdAm1020);
            BinSerialize.WriteFloat(ref buffer,CodeIdAmMin1020);
            BinSerialize.WriteFloat(ref buffer,CodeIdAmMax1020);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            BinSerialize.WriteShort(ref buffer,RxFreqOffset);
            BinSerialize.WriteShort(ref buffer,Freq30);
            BinSerialize.WriteShort(ref buffer,Freq9960);
            BinSerialize.WriteUShort(ref buffer,CodeIdDotTime);
            BinSerialize.WriteUShort(ref buffer,CodeIdDashTime);
            BinSerialize.WriteUShort(ref buffer,CodeIdSymbolPause);
            BinSerialize.WriteUShort(ref buffer,CodeIdCharPause);
            BinSerialize.WriteUShort(ref buffer,CodeIdDelay);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId)
                {
                    Encoding.ASCII.GetBytes(charPointer, CodeId.Length, bytePointer, CodeId.Length);
                }
            }
            buffer = buffer.Slice(CodeId.Length);
            
            /* PayloadByteSize = 94 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt64Type.Accept(visitor,RxFreqField, RxFreqField.DataType, ref _rxFreq);    
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    
            FloatType.Accept(visitor,RxPowerField, RxPowerField.DataType, ref _rxPower);    
            FloatType.Accept(visitor,RxFieldStrengthField, RxFieldStrengthField.DataType, ref _rxFieldStrength);    
            FloatType.Accept(visitor,RxSignalOverflowField, RxSignalOverflowField.DataType, ref _rxSignalOverflow);    
            FloatType.Accept(visitor,RxGainField, RxGainField.DataType, ref _rxGain);    
            FloatType.Accept(visitor,AzimuthField, AzimuthField.DataType, ref _azimuth);    
            FloatType.Accept(visitor,Am30Field, Am30Field.DataType, ref _am30);    
            FloatType.Accept(visitor,Am9960Field, Am9960Field.DataType, ref _am9960);    
            FloatType.Accept(visitor,DeviationField, DeviationField.DataType, ref _deviation);    
            FloatType.Accept(visitor,CodeIdAm1020Field, CodeIdAm1020Field.DataType, ref _codeIdAm1020);    
            FloatType.Accept(visitor,CodeIdAmMin1020Field, CodeIdAmMin1020Field.DataType, ref _codeIdAmMin1020);    
            FloatType.Accept(visitor,CodeIdAmMax1020Field, CodeIdAmMax1020Field.DataType, ref _codeIdAmMax1020);    
            Int16Type.Accept(visitor,MeasureTimeField, MeasureTimeField.DataType, ref _measureTime);
            Int16Type.Accept(visitor,RxFreqOffsetField, RxFreqOffsetField.DataType, ref _rxFreqOffset);
            Int16Type.Accept(visitor,Freq30Field, Freq30Field.DataType, ref _freq30);
            Int16Type.Accept(visitor,Freq9960Field, Freq9960Field.DataType, ref _freq9960);
            UInt16Type.Accept(visitor,CodeIdDotTimeField, CodeIdDotTimeField.DataType, ref _codeIdDotTime);    
            UInt16Type.Accept(visitor,CodeIdDashTimeField, CodeIdDashTimeField.DataType, ref _codeIdDashTime);    
            UInt16Type.Accept(visitor,CodeIdSymbolPauseField, CodeIdSymbolPauseField.DataType, ref _codeIdSymbolPause);    
            UInt16Type.Accept(visitor,CodeIdCharPauseField, CodeIdCharPauseField.DataType, ref _codeIdCharPause);    
            UInt16Type.Accept(visitor,CodeIdDelayField, CodeIdDelayField.DataType, ref _codeIdDelay);    
            ArrayType.Accept(visitor,CodeIdField, CodeIdField.DataType, 4, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref CodeId[index]));

        }

        /// <summary>
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
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
        /// Receive power field strength
        /// OriginName: rx_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field RxFieldStrengthField = new Field.Builder()
            .Name(nameof(RxFieldStrength))
            .Title("rx_field_strength")
            .Description("Receive power field strength")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _rxFieldStrength;
        public float RxFieldStrength { get => _rxFieldStrength; set => _rxFieldStrength = value; }
        /// <summary>
        /// Signal overflow indicator (≤0.2 — too low, ≥0.8 — too high)
        /// OriginName: rx_signal_overflow, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field RxSignalOverflowField = new Field.Builder()
            .Name(nameof(RxSignalOverflow))
            .Title("rx_signal_overflow")
            .Description("Signal overflow indicator (\u22640.2 \u2014 too low, \u22650.8 \u2014 too high)")
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
        /// Measured azimuth
        /// OriginName: azimuth, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field AzimuthField = new Field.Builder()
            .Name(nameof(Azimuth))
            .Title("azimuth")
            .Description("Measured azimuth")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _azimuth;
        public float Azimuth { get => _azimuth; set => _azimuth = value; }
        /// <summary>
        /// Total amplitude modulation of 30 Hz
        /// OriginName: am_30, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am30Field = new Field.Builder()
            .Name(nameof(Am30))
            .Title("am_30")
            .Description("Total amplitude modulation of 30 Hz")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am30;
        public float Am30 { get => _am30; set => _am30 = value; }
        /// <summary>
        /// Total amplitude modulation of 9960 Hz
        /// OriginName: am_9960, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am9960Field = new Field.Builder()
            .Name(nameof(Am9960))
            .Title("am_9960")
            .Description("Total amplitude modulation of 9960 Hz")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am9960;
        public float Am9960 { get => _am9960; set => _am9960 = value; }
        /// <summary>
        /// Deviation
        /// OriginName: deviation, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DeviationField = new Field.Builder()
            .Name(nameof(Deviation))
            .Title("deviation")
            .Description("Deviation")
.Units(@"")
            .DataType(FloatType.Default)
        .Build();
        private float _deviation;
        public float Deviation { get => _deviation; set => _deviation = value; }
        /// <summary>
        /// Current amplitude modulation of Code ID
        /// OriginName: code_id_am_1020, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdAm1020Field = new Field.Builder()
            .Name(nameof(CodeIdAm1020))
            .Title("code_id_am_1020")
            .Description("Current amplitude modulation of Code ID")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _codeIdAm1020;
        public float CodeIdAm1020 { get => _codeIdAm1020; set => _codeIdAm1020 = value; }
        /// <summary>
        /// Min amplitude modulation of Code ID
        /// OriginName: code_id_am_min_1020, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdAmMin1020Field = new Field.Builder()
            .Name(nameof(CodeIdAmMin1020))
            .Title("code_id_am_min_1020")
            .Description("Min amplitude modulation of Code ID")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _codeIdAmMin1020;
        public float CodeIdAmMin1020 { get => _codeIdAmMin1020; set => _codeIdAmMin1020 = value; }
        /// <summary>
        /// Max amplitude modulation of Code ID
        /// OriginName: code_id_am_max_1020, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdAmMax1020Field = new Field.Builder()
            .Name(nameof(CodeIdAmMax1020))
            .Title("code_id_am_max_1020")
            .Description("Max amplitude modulation of Code ID")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _codeIdAmMax1020;
        public float CodeIdAmMax1020 { get => _codeIdAmMax1020; set => _codeIdAmMax1020 = value; }
        /// <summary>
        /// Measure time
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field MeasureTimeField = new Field.Builder()
            .Name(nameof(MeasureTime))
            .Title("measure_time")
            .Description("Measure time")
.Units(@"ms")
            .DataType(Int16Type.Default)
        .Build();
        private short _measureTime;
        public short MeasureTime { get => _measureTime; set => _measureTime = value; }
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
        /// Total frequency offset of signal 30 Hz
        /// OriginName: freq_30, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq30Field = new Field.Builder()
            .Name(nameof(Freq30))
            .Title("freq_30")
            .Description("Total frequency offset of signal 30 Hz")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq30;
        public short Freq30 { get => _freq30; set => _freq30 = value; }
        /// <summary>
        /// Total frequency offset of signal 9960 Hz
        /// OriginName: freq_9960, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq9960Field = new Field.Builder()
            .Name(nameof(Freq9960))
            .Title("freq_9960")
            .Description("Total frequency offset of signal 9960 Hz")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq9960;
        public short Freq9960 { get => _freq9960; set => _freq9960 = value; }
        /// <summary>
        /// Dot time
        /// OriginName: code_id_dot_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdDotTimeField = new Field.Builder()
            .Name(nameof(CodeIdDotTime))
            .Title("code_id_dot_time")
            .Description("Dot time")
.Units(@"ms")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _codeIdDotTime;
        public ushort CodeIdDotTime { get => _codeIdDotTime; set => _codeIdDotTime = value; }
        /// <summary>
        /// Dash time
        /// OriginName: code_id_dash_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdDashTimeField = new Field.Builder()
            .Name(nameof(CodeIdDashTime))
            .Title("code_id_dash_time")
            .Description("Dash time")
.Units(@"ms")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _codeIdDashTime;
        public ushort CodeIdDashTime { get => _codeIdDashTime; set => _codeIdDashTime = value; }
        /// <summary>
        /// Symbol pause time
        /// OriginName: code_id_symbol_pause, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdSymbolPauseField = new Field.Builder()
            .Name(nameof(CodeIdSymbolPause))
            .Title("code_id_symbol_pause")
            .Description("Symbol pause time")
.Units(@"ms")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _codeIdSymbolPause;
        public ushort CodeIdSymbolPause { get => _codeIdSymbolPause; set => _codeIdSymbolPause = value; }
        /// <summary>
        /// Char pause time
        /// OriginName: code_id_char_pause, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdCharPauseField = new Field.Builder()
            .Name(nameof(CodeIdCharPause))
            .Title("code_id_char_pause")
            .Description("Char pause time")
.Units(@"ms")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _codeIdCharPause;
        public ushort CodeIdCharPause { get => _codeIdCharPause; set => _codeIdCharPause = value; }
        /// <summary>
        /// Last update
        /// OriginName: code_id_delay, Units: s, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdDelayField = new Field.Builder()
            .Name(nameof(CodeIdDelay))
            .Title("code_id_delay")
            .Description("Last update")
.Units(@"s")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _codeIdDelay;
        public ushort CodeIdDelay { get => _codeIdDelay; set => _codeIdDelay = value; }
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
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_RX_MARKER mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_RX_MARKER
    /// </summary>
    public class AsvRsgaRttRxMarkerPacket : MavlinkV2Message<AsvRsgaRttRxMarkerPayload>
    {
        public const int MessageId = 13464;
        
        public const byte CrcExtra = 237;
        
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
        public byte GetMaxByteSize() => 135; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 135; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            + 8 // uint64_t flags
            +8 // uint64_t rx_freq
            +4 // uint32_t index
            +4 // float rx_power
            +4 // float rx_field_strength
            +4 // float rx_signal_overflow
            +4 // float rx_gain
            +4 // float am_400
            +4 // float am_400_max
            +4 // float am_400_min
            +4 // float dot_400
            +4 // float dash_400
            +4 // float gap_400
            +4 // float am_1300
            +4 // float am_1300_max
            +4 // float am_1300_min
            +4 // float dot_1300
            +4 // float dash_1300
            +4 // float gap_1300
            +4 // float am_3000
            +4 // float am_3000_max
            +4 // float am_3000_min
            +4 // float dot_3000
            +4 // float dash_3000
            +4 // float gap_3000
            +2 // int16_t measure_time
            +2 // int16_t rx_freq_offset
            +2 // int16_t freq_400
            +2 // int16_t freq_1300
            +2 // int16_t freq_3000
            +CodeId400.Length // char[3] code_id_400
            +CodeId1300.Length // char[3] code_id_1300
            +CodeId3000.Length // char[3] code_id_3000
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Flags = (AsvRsgaDataFlags)BinSerialize.ReadULong(ref buffer);
            RxFreq = BinSerialize.ReadULong(ref buffer);
            Index = BinSerialize.ReadUInt(ref buffer);
            RxPower = BinSerialize.ReadFloat(ref buffer);
            RxFieldStrength = BinSerialize.ReadFloat(ref buffer);
            RxSignalOverflow = BinSerialize.ReadFloat(ref buffer);
            RxGain = BinSerialize.ReadFloat(ref buffer);
            Am400 = BinSerialize.ReadFloat(ref buffer);
            Am400Max = BinSerialize.ReadFloat(ref buffer);
            Am400Min = BinSerialize.ReadFloat(ref buffer);
            Dot400 = BinSerialize.ReadFloat(ref buffer);
            Dash400 = BinSerialize.ReadFloat(ref buffer);
            Gap400 = BinSerialize.ReadFloat(ref buffer);
            Am1300 = BinSerialize.ReadFloat(ref buffer);
            Am1300Max = BinSerialize.ReadFloat(ref buffer);
            Am1300Min = BinSerialize.ReadFloat(ref buffer);
            Dot1300 = BinSerialize.ReadFloat(ref buffer);
            Dash1300 = BinSerialize.ReadFloat(ref buffer);
            Gap1300 = BinSerialize.ReadFloat(ref buffer);
            Am3000 = BinSerialize.ReadFloat(ref buffer);
            Am3000Max = BinSerialize.ReadFloat(ref buffer);
            Am3000Min = BinSerialize.ReadFloat(ref buffer);
            Dot3000 = BinSerialize.ReadFloat(ref buffer);
            Dash3000 = BinSerialize.ReadFloat(ref buffer);
            Gap3000 = BinSerialize.ReadFloat(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            RxFreqOffset = BinSerialize.ReadShort(ref buffer);
            Freq400 = BinSerialize.ReadShort(ref buffer);
            Freq1300 = BinSerialize.ReadShort(ref buffer);
            Freq3000 = BinSerialize.ReadShort(ref buffer);
            arraySize = /*ArrayLength*/3 - Math.Max(0,((/*PayloadByteSize*/135 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId400)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, CodeId400.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            arraySize = 3;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId1300)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, CodeId1300.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            arraySize = 3;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId3000)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, CodeId3000.Length);
                }
            }
            buffer = buffer[arraySize..];
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,(ulong)Flags);
            BinSerialize.WriteULong(ref buffer,RxFreq);
            BinSerialize.WriteUInt(ref buffer,Index);
            BinSerialize.WriteFloat(ref buffer,RxPower);
            BinSerialize.WriteFloat(ref buffer,RxFieldStrength);
            BinSerialize.WriteFloat(ref buffer,RxSignalOverflow);
            BinSerialize.WriteFloat(ref buffer,RxGain);
            BinSerialize.WriteFloat(ref buffer,Am400);
            BinSerialize.WriteFloat(ref buffer,Am400Max);
            BinSerialize.WriteFloat(ref buffer,Am400Min);
            BinSerialize.WriteFloat(ref buffer,Dot400);
            BinSerialize.WriteFloat(ref buffer,Dash400);
            BinSerialize.WriteFloat(ref buffer,Gap400);
            BinSerialize.WriteFloat(ref buffer,Am1300);
            BinSerialize.WriteFloat(ref buffer,Am1300Max);
            BinSerialize.WriteFloat(ref buffer,Am1300Min);
            BinSerialize.WriteFloat(ref buffer,Dot1300);
            BinSerialize.WriteFloat(ref buffer,Dash1300);
            BinSerialize.WriteFloat(ref buffer,Gap1300);
            BinSerialize.WriteFloat(ref buffer,Am3000);
            BinSerialize.WriteFloat(ref buffer,Am3000Max);
            BinSerialize.WriteFloat(ref buffer,Am3000Min);
            BinSerialize.WriteFloat(ref buffer,Dot3000);
            BinSerialize.WriteFloat(ref buffer,Dash3000);
            BinSerialize.WriteFloat(ref buffer,Gap3000);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            BinSerialize.WriteShort(ref buffer,RxFreqOffset);
            BinSerialize.WriteShort(ref buffer,Freq400);
            BinSerialize.WriteShort(ref buffer,Freq1300);
            BinSerialize.WriteShort(ref buffer,Freq3000);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId400)
                {
                    Encoding.ASCII.GetBytes(charPointer, CodeId400.Length, bytePointer, CodeId400.Length);
                }
            }
            buffer = buffer.Slice(CodeId400.Length);
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId1300)
                {
                    Encoding.ASCII.GetBytes(charPointer, CodeId1300.Length, bytePointer, CodeId1300.Length);
                }
            }
            buffer = buffer.Slice(CodeId1300.Length);
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId3000)
                {
                    Encoding.ASCII.GetBytes(charPointer, CodeId3000.Length, bytePointer, CodeId3000.Length);
                }
            }
            buffer = buffer.Slice(CodeId3000.Length);
            
            /* PayloadByteSize = 135 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            var tmpFlags = (ulong)Flags;
            UInt64Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (AsvRsgaDataFlags)tmpFlags;
            UInt64Type.Accept(visitor,RxFreqField, RxFreqField.DataType, ref _rxFreq);    
            UInt32Type.Accept(visitor,IndexField, IndexField.DataType, ref _index);    
            FloatType.Accept(visitor,RxPowerField, RxPowerField.DataType, ref _rxPower);    
            FloatType.Accept(visitor,RxFieldStrengthField, RxFieldStrengthField.DataType, ref _rxFieldStrength);    
            FloatType.Accept(visitor,RxSignalOverflowField, RxSignalOverflowField.DataType, ref _rxSignalOverflow);    
            FloatType.Accept(visitor,RxGainField, RxGainField.DataType, ref _rxGain);    
            FloatType.Accept(visitor,Am400Field, Am400Field.DataType, ref _am400);    
            FloatType.Accept(visitor,Am400MaxField, Am400MaxField.DataType, ref _am400Max);    
            FloatType.Accept(visitor,Am400MinField, Am400MinField.DataType, ref _am400Min);    
            FloatType.Accept(visitor,Dot400Field, Dot400Field.DataType, ref _dot400);    
            FloatType.Accept(visitor,Dash400Field, Dash400Field.DataType, ref _dash400);    
            FloatType.Accept(visitor,Gap400Field, Gap400Field.DataType, ref _gap400);    
            FloatType.Accept(visitor,Am1300Field, Am1300Field.DataType, ref _am1300);    
            FloatType.Accept(visitor,Am1300MaxField, Am1300MaxField.DataType, ref _am1300Max);    
            FloatType.Accept(visitor,Am1300MinField, Am1300MinField.DataType, ref _am1300Min);    
            FloatType.Accept(visitor,Dot1300Field, Dot1300Field.DataType, ref _dot1300);    
            FloatType.Accept(visitor,Dash1300Field, Dash1300Field.DataType, ref _dash1300);    
            FloatType.Accept(visitor,Gap1300Field, Gap1300Field.DataType, ref _gap1300);    
            FloatType.Accept(visitor,Am3000Field, Am3000Field.DataType, ref _am3000);    
            FloatType.Accept(visitor,Am3000MaxField, Am3000MaxField.DataType, ref _am3000Max);    
            FloatType.Accept(visitor,Am3000MinField, Am3000MinField.DataType, ref _am3000Min);    
            FloatType.Accept(visitor,Dot3000Field, Dot3000Field.DataType, ref _dot3000);    
            FloatType.Accept(visitor,Dash3000Field, Dash3000Field.DataType, ref _dash3000);    
            FloatType.Accept(visitor,Gap3000Field, Gap3000Field.DataType, ref _gap3000);    
            Int16Type.Accept(visitor,MeasureTimeField, MeasureTimeField.DataType, ref _measureTime);
            Int16Type.Accept(visitor,RxFreqOffsetField, RxFreqOffsetField.DataType, ref _rxFreqOffset);
            Int16Type.Accept(visitor,Freq400Field, Freq400Field.DataType, ref _freq400);
            Int16Type.Accept(visitor,Freq1300Field, Freq1300Field.DataType, ref _freq1300);
            Int16Type.Accept(visitor,Freq3000Field, Freq3000Field.DataType, ref _freq3000);
            ArrayType.Accept(visitor,CodeId400Field, CodeId400Field.DataType, 3, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref CodeId400[index]));
            ArrayType.Accept(visitor,CodeId1300Field, CodeId1300Field.DataType, 3, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref CodeId1300[index]));
            ArrayType.Accept(visitor,CodeId3000Field, CodeId3000Field.DataType, 3, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref CodeId3000[index]));

        }

        /// <summary>
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
            .DataType(new UInt64Type(AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Min(),AsvRsgaDataFlagsHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvRsgaDataFlagsHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvRsgaDataFlags _flags;
        public AsvRsgaDataFlags Flags { get => _flags; set => _flags = value; } 
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
        /// Receive power field strength
        /// OriginName: rx_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field RxFieldStrengthField = new Field.Builder()
            .Name(nameof(RxFieldStrength))
            .Title("rx_field_strength")
            .Description("Receive power field strength")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _rxFieldStrength;
        public float RxFieldStrength { get => _rxFieldStrength; set => _rxFieldStrength = value; }
        /// <summary>
        /// Signal overflow indicator (≤0.2 — too low, ≥0.8 — too high)
        /// OriginName: rx_signal_overflow, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field RxSignalOverflowField = new Field.Builder()
            .Name(nameof(RxSignalOverflow))
            .Title("rx_signal_overflow")
            .Description("Signal overflow indicator (\u22640.2 \u2014 too low, \u22650.8 \u2014 too high)")
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
        /// Current AM modulation depth of the 400 Hz component
        /// OriginName: am_400, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am400Field = new Field.Builder()
            .Name(nameof(Am400))
            .Title("am_400")
            .Description("Current AM modulation depth of the 400 Hz component")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am400;
        public float Am400 { get => _am400; set => _am400 = value; }
        /// <summary>
        /// Max AM modulation depth of the 400 Hz component
        /// OriginName: am_400_max, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am400MaxField = new Field.Builder()
            .Name(nameof(Am400Max))
            .Title("am_400_max")
            .Description("Max AM modulation depth of the 400 Hz component")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am400Max;
        public float Am400Max { get => _am400Max; set => _am400Max = value; }
        /// <summary>
        /// Min AM modulation depth of the 400 Hz component
        /// OriginName: am_400_min, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am400MinField = new Field.Builder()
            .Name(nameof(Am400Min))
            .Title("am_400_min")
            .Description("Min AM modulation depth of the 400 Hz component")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am400Min;
        public float Am400Min { get => _am400Min; set => _am400Min = value; }
        /// <summary>
        /// Dot for the outer marker
        /// OriginName: dot_400, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field Dot400Field = new Field.Builder()
            .Name(nameof(Dot400))
            .Title("dot_400")
            .Description("Dot for the outer marker")
.Units(@"ms")
            .DataType(FloatType.Default)
        .Build();
        private float _dot400;
        public float Dot400 { get => _dot400; set => _dot400 = value; }
        /// <summary>
        /// Dash for the outer marker
        /// OriginName: dash_400, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field Dash400Field = new Field.Builder()
            .Name(nameof(Dash400))
            .Title("dash_400")
            .Description("Dash for the outer marker")
.Units(@"ms")
            .DataType(FloatType.Default)
        .Build();
        private float _dash400;
        public float Dash400 { get => _dash400; set => _dash400 = value; }
        /// <summary>
        /// Gap for the outer marker
        /// OriginName: gap_400, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field Gap400Field = new Field.Builder()
            .Name(nameof(Gap400))
            .Title("gap_400")
            .Description("Gap for the outer marker")
.Units(@"ms")
            .DataType(FloatType.Default)
        .Build();
        private float _gap400;
        public float Gap400 { get => _gap400; set => _gap400 = value; }
        /// <summary>
        /// AM modulation depth of the 1300 Hz component
        /// OriginName: am_1300, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am1300Field = new Field.Builder()
            .Name(nameof(Am1300))
            .Title("am_1300")
            .Description("AM modulation depth of the 1300 Hz component")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am1300;
        public float Am1300 { get => _am1300; set => _am1300 = value; }
        /// <summary>
        /// Max AM modulation depth of the 1300 Hz component
        /// OriginName: am_1300_max, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am1300MaxField = new Field.Builder()
            .Name(nameof(Am1300Max))
            .Title("am_1300_max")
            .Description("Max AM modulation depth of the 1300 Hz component")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am1300Max;
        public float Am1300Max { get => _am1300Max; set => _am1300Max = value; }
        /// <summary>
        /// Min AM modulation depth of the 1300 Hz component
        /// OriginName: am_1300_min, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am1300MinField = new Field.Builder()
            .Name(nameof(Am1300Min))
            .Title("am_1300_min")
            .Description("Min AM modulation depth of the 1300 Hz component")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am1300Min;
        public float Am1300Min { get => _am1300Min; set => _am1300Min = value; }
        /// <summary>
        /// Dot for the outer marker
        /// OriginName: dot_1300, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field Dot1300Field = new Field.Builder()
            .Name(nameof(Dot1300))
            .Title("dot_1300")
            .Description("Dot for the outer marker")
.Units(@"ms")
            .DataType(FloatType.Default)
        .Build();
        private float _dot1300;
        public float Dot1300 { get => _dot1300; set => _dot1300 = value; }
        /// <summary>
        /// Dash for the outer marker
        /// OriginName: dash_1300, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field Dash1300Field = new Field.Builder()
            .Name(nameof(Dash1300))
            .Title("dash_1300")
            .Description("Dash for the outer marker")
.Units(@"ms")
            .DataType(FloatType.Default)
        .Build();
        private float _dash1300;
        public float Dash1300 { get => _dash1300; set => _dash1300 = value; }
        /// <summary>
        /// Gap for the outer marker
        /// OriginName: gap_1300, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field Gap1300Field = new Field.Builder()
            .Name(nameof(Gap1300))
            .Title("gap_1300")
            .Description("Gap for the outer marker")
.Units(@"ms")
            .DataType(FloatType.Default)
        .Build();
        private float _gap1300;
        public float Gap1300 { get => _gap1300; set => _gap1300 = value; }
        /// <summary>
        /// AM modulation depth of the 3000 Hz component
        /// OriginName: am_3000, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am3000Field = new Field.Builder()
            .Name(nameof(Am3000))
            .Title("am_3000")
            .Description("AM modulation depth of the 3000 Hz component")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am3000;
        public float Am3000 { get => _am3000; set => _am3000 = value; }
        /// <summary>
        /// Max AM modulation depth of the 3000 Hz component
        /// OriginName: am_3000_max, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am3000MaxField = new Field.Builder()
            .Name(nameof(Am3000Max))
            .Title("am_3000_max")
            .Description("Max AM modulation depth of the 3000 Hz component")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am3000Max;
        public float Am3000Max { get => _am3000Max; set => _am3000Max = value; }
        /// <summary>
        /// Min AM modulation depth of the 3000 Hz component
        /// OriginName: am_3000_min, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am3000MinField = new Field.Builder()
            .Name(nameof(Am3000Min))
            .Title("am_3000_min")
            .Description("Min AM modulation depth of the 3000 Hz component")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am3000Min;
        public float Am3000Min { get => _am3000Min; set => _am3000Min = value; }
        /// <summary>
        /// Dot for the outer marker
        /// OriginName: dot_3000, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field Dot3000Field = new Field.Builder()
            .Name(nameof(Dot3000))
            .Title("dot_3000")
            .Description("Dot for the outer marker")
.Units(@"ms")
            .DataType(FloatType.Default)
        .Build();
        private float _dot3000;
        public float Dot3000 { get => _dot3000; set => _dot3000 = value; }
        /// <summary>
        /// Dash for the outer marker
        /// OriginName: dash_3000, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field Dash3000Field = new Field.Builder()
            .Name(nameof(Dash3000))
            .Title("dash_3000")
            .Description("Dash for the outer marker")
.Units(@"ms")
            .DataType(FloatType.Default)
        .Build();
        private float _dash3000;
        public float Dash3000 { get => _dash3000; set => _dash3000 = value; }
        /// <summary>
        /// Gap for the outer marker
        /// OriginName: gap_3000, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field Gap3000Field = new Field.Builder()
            .Name(nameof(Gap3000))
            .Title("gap_3000")
            .Description("Gap for the outer marker")
.Units(@"ms")
            .DataType(FloatType.Default)
        .Build();
        private float _gap3000;
        public float Gap3000 { get => _gap3000; set => _gap3000 = value; }
        /// <summary>
        /// Measure time
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field MeasureTimeField = new Field.Builder()
            .Name(nameof(MeasureTime))
            .Title("measure_time")
            .Description("Measure time")
.Units(@"ms")
            .DataType(Int16Type.Default)
        .Build();
        private short _measureTime;
        public short MeasureTime { get => _measureTime; set => _measureTime = value; }
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
        /// Frequency of the 400 Hz component
        /// OriginName: freq_400, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq400Field = new Field.Builder()
            .Name(nameof(Freq400))
            .Title("freq_400")
            .Description("Frequency of the 400 Hz component")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq400;
        public short Freq400 { get => _freq400; set => _freq400 = value; }
        /// <summary>
        /// Frequency of the 1300 Hz component
        /// OriginName: freq_1300, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq1300Field = new Field.Builder()
            .Name(nameof(Freq1300))
            .Title("freq_1300")
            .Description("Frequency of the 1300 Hz component")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq1300;
        public short Freq1300 { get => _freq1300; set => _freq1300 = value; }
        /// <summary>
        /// Frequency of the 3000 Hz component
        /// OriginName: freq_3000, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq3000Field = new Field.Builder()
            .Name(nameof(Freq3000))
            .Title("freq_3000")
            .Description("Frequency of the 3000 Hz component")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq3000;
        public short Freq3000 { get => _freq3000; set => _freq3000 = value; }
        /// <summary>
        /// Dash\dot id
        /// OriginName: code_id_400, Units: Symb, IsExtended: false
        /// </summary>
        public static readonly Field CodeId400Field = new Field.Builder()
            .Name(nameof(CodeId400))
            .Title("code_id_400")
            .Description("Dash\\dot id")
.Units(@"Symb")
            .DataType(new ArrayType(CharType.Ascii,3))
        .Build();
        public const int CodeId400MaxItemsCount = 3;
        public char[] CodeId400 { get; } = new char[3];
        [Obsolete("This method is deprecated. Use GetCodeId400MaxItemsCount instead.")]
        public byte GetCodeId400MaxItemsCount() => 3;
        /// <summary>
        /// Dash\dot id
        /// OriginName: code_id_1300, Units: Symb, IsExtended: false
        /// </summary>
        public static readonly Field CodeId1300Field = new Field.Builder()
            .Name(nameof(CodeId1300))
            .Title("code_id_1300")
            .Description("Dash\\dot id")
.Units(@"Symb")
            .DataType(new ArrayType(CharType.Ascii,3))
        .Build();
        public const int CodeId1300MaxItemsCount = 3;
        public char[] CodeId1300 { get; } = new char[3];
        /// <summary>
        /// Dash\dot id
        /// OriginName: code_id_3000, Units: Symb, IsExtended: false
        /// </summary>
        public static readonly Field CodeId3000Field = new Field.Builder()
            .Name(nameof(CodeId3000))
            .Title("code_id_3000")
            .Description("Dash\\dot id")
.Units(@"Symb")
            .DataType(new ArrayType(CharType.Ascii,3))
        .Build();
        public const int CodeId3000MaxItemsCount = 3;
        public char[] CodeId3000 { get; } = new char[3];
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
        /// Receive power field strength
        /// OriginName: rx_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field RxFieldStrengthField = new Field.Builder()
            .Name(nameof(RxFieldStrength))
            .Title("rx_field_strength")
            .Description("Receive power field strength")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _rxFieldStrength;
        public float RxFieldStrength { get => _rxFieldStrength; set => _rxFieldStrength = value; }
        /// <summary>
        /// Signal overflow indicator (≤0.2 — too weak, ≥0.8 — too strong)
        /// OriginName: rx_signal_overflow, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field RxSignalOverflowField = new Field.Builder()
            .Name(nameof(RxSignalOverflow))
            .Title("rx_signal_overflow")
            .Description("Signal overflow indicator (\u22640.2 \u2014 too weak, \u22650.8 \u2014 too strong)")
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
        /// GNSS reference station ID (used when GNSS is received from multiple sources)
        /// OriginName: ref_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RefIdField = new Field.Builder()
            .Name(nameof(RefId))
            .Title("ref_id")
            .Description("GNSS reference station ID (used when GNSS is received from multiple sources)")

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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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
    /// Real time telemetry (RTT) for ASV_RSGA_CUSTOM_MODE_RDF mode. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RSGA_RTT_RDF
    /// </summary>
    public class AsvRsgaRttRdfPacket : MavlinkV2Message<AsvRsgaRttRdfPayload>
    {
        public const int MessageId = 13468;
        
        public const byte CrcExtra = 120;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttRdfPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_RDF";
    }

    /// <summary>
    ///  ASV_RSGA_RTT_RDF
    /// </summary>
    public class AsvRsgaRttRdfPayload : IPayload
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
        /// Timestamp (UNIX epoch time)
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time)")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Data flags
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Data flags")
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

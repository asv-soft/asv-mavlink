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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.11+323463838958bbc455efbb799160b09e17080d75

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
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
    public enum MavType:uint
    {
        /// <summary>
        /// Identifies the Radio Signal Generator and Analyzer (RSGA) payload in the HEARTBEAT message.
        /// MAV_TYPE_ASV_RSGA
        /// </summary>
        MavTypeAsvRsga = 254,
    }

    /// <summary>
    /// Defines RSGA modes mapped to the custom_mode[0–7] bit field of the HEARTBEAT message. Maximum 255 values.
    ///  ASV_RSGA_CUSTOM_MODE
    /// </summary>
    public enum AsvRsgaCustomMode:uint
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

    /// <summary>
    /// A mapping of RSGA special sub modes for custom_mode[8-15] bits field of heartbeat.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RSGA_CUSTOM_SUB_MODE
    /// </summary>
    [Flags]
    public enum AsvRsgaCustomSubMode:uint
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

    /// <summary>
    /// ACK / NACK / ERROR values as a result of ASV_RSGA_*_REQUEST commands.
    ///  ASV_RSGA_REQUEST_ACK
    /// </summary>
    public enum AsvRsgaRequestAck:uint
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

    /// <summary>
    /// Common status flags for all ASV_RSGA_RTT_* data.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RSGA_DATA_FLAGS
    /// </summary>
    [Flags]
    public enum AsvRsgaDataFlags:uint
    {
        /// <summary>
        /// Is data valid.
        /// ASV_RSGA_DATA_VALID
        /// </summary>
        AsvRsgaDataValid = 1,
    }

    /// <summary>
    /// Common status flags for all ASV_RSGA_RTT_GNSS message.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RSGA_RTT_GNSS_FLAGS
    /// </summary>
    [Flags]
    public enum AsvRsgaRttGnssFlags:uint
    {
        /// <summary>
        /// This flag is set when the vehicle is known to be on the ground.
        /// ASV_RSGA_RTT_GNSS_FLAGS_ON_THE_GROUND
        /// </summary>
        AsvRsgaRttGnssFlagsOnTheGround = 1,
    }

    /// <summary>
    /// Mode S interrogation or uplink formats.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RSGA_RTT_ADSB_MSG_UF
    /// </summary>
    [Flags]
    public enum AsvRsgaRttAdsbMsgUf:uint
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

    /// <summary>
    /// Mode S reply or downlink formats.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RSGA_RTT_ADSB_MSG_DF
    /// </summary>
    [Flags]
    public enum AsvRsgaRttAdsbMsgDf:uint
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

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("request_id",
"Specifies a unique number for this request. This allows the response packet to be identified.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_COMPATIBILITY_REQUEST:"
        + "uint16_t request_id;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
        
        



        /// <summary>
        /// Specifies a unique number for this request. This allows the response packet to be identified.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("request_id",
"Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("result",
"Result code.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("supported_modes",
"Supported modes. Each bit index represents an ASV_RSGA_CUSTOM_MODE value (256 bits). First (IDLE) bit always true.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            32, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_COMPATIBILITY_RESPONSE:"
        + "uint16_t request_id;"
        + "uint8_t result;"
        + "uint8_t[32] supported_modes;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+= 1; // Result
            sum+=SupportedModes.Length; //SupportedModes
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvRsgaRequestAck)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/35 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            SupportedModes = new byte[arraySize];
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
        
        



        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaRequestAck Result { get; set; }
        /// <summary>
        /// Supported modes. Each bit index represents an ASV_RSGA_CUSTOM_MODE value (256 bits). First (IDLE) bit always true.
        /// OriginName: supported_modes, Units: , IsExtended: false
        /// </summary>
        public const int SupportedModesMaxItemsCount = 32;
        public byte[] SupportedModes { get; set; } = new byte[32];
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
        
        public const byte CrcExtra = 95;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRsgaRttGnssPayload Payload { get; } = new();

        public override string Name => "ASV_RSGA_RTT_GNSS";
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("data_index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("lat",
"Latitude (WGS84, EGM96 ellipsoid)",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("lat_err",
"Expected Error in Latitude (North) Direction",
string.Empty, 
@"mm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("lon",
"Longitude (WGS84, EGM96 ellipsoid)",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("lon_err",
"Expected Error in Longitude (East) Direction",
string.Empty, 
@"mm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("alt_msl",
"Antenna altitude above/below mean sea level (geoid)",
string.Empty, 
@"mm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("alt_wgs",
"Antenna altitude WGS-84 earth ellipsoid",
string.Empty, 
@"mm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("alt_err",
"Expected Error in Altitude",
string.Empty, 
@"mm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("ref_id",
"GNSS reference station ID (used when GNSS is received from multiple sources).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("hdop",
"HDOP horizontal dilution of position",
string.Empty, 
string.Empty, 
string.Empty, 
@"UINT16_MAX", 
            MessageFieldType.Uint16, 
            0, 
false),
            new("vdop",
"VDOP vertical dilution of position",
string.Empty, 
string.Empty, 
string.Empty, 
@"UINT16_MAX", 
            MessageFieldType.Uint16, 
            0, 
false),
            new("sog",
"Speed over ground",
string.Empty, 
@"cm/s", 
string.Empty, 
@"UINT16_MAX", 
            MessageFieldType.Uint16, 
            0, 
false),
            new("cog_true",
"Course over ground (true) (yaw angle). 0.0..359.99 degrees",
string.Empty, 
@"cdeg", 
string.Empty, 
@"UINT16_MAX", 
            MessageFieldType.Uint16, 
            0, 
false),
            new("cog_mag",
"Course over ground (magnetic) (yaw angle). 0.0..359.99 degrees",
string.Empty, 
@"cdeg", 
string.Empty, 
@"UINT16_MAX", 
            MessageFieldType.Uint16, 
            0, 
false),
            new("gnss_flags",
"GNSS special flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("sat_cnt",
"Number of satellites in view",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("fix_type",
"GNSS fix type",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_GNSS:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t data_index;"
        + "int32_t lat;"
        + "int32_t lat_err;"
        + "int32_t lon;"
        + "int32_t lon_err;"
        + "int32_t alt_msl;"
        + "int32_t alt_wgs;"
        + "int32_t alt_err;"
        + "uint16_t ref_id;"
        + "uint16_t hdop;"
        + "uint16_t vdop;"
        + "uint16_t sog;"
        + "uint16_t cog_true;"
        + "uint16_t cog_mag;"
        + "uint8_t gnss_flags;"
        + "uint8_t sat_cnt;"
        + "uint8_t fix_type;"
        ;
    }

    /// <summary>
    ///  ASV_RSGA_RTT_GNSS
    /// </summary>
    public class AsvRsgaRttGnssPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 63; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 63; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //DataIndex
            sum+=4; //Lat
            sum+=4; //LatErr
            sum+=4; //Lon
            sum+=4; //LonErr
            sum+=4; //AltMsl
            sum+=4; //AltWgs
            sum+=4; //AltErr
            sum+=2; //RefId
            sum+=2; //Hdop
            sum+=2; //Vdop
            sum+=2; //Sog
            sum+=2; //CogTrue
            sum+=2; //CogMag
            sum+= 1; // GnssFlags
            sum+=1; //SatCnt
            sum+= 1; // FixType
            return (byte)sum;
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
            BinSerialize.WriteByte(ref buffer,(byte)GnssFlags);
            BinSerialize.WriteByte(ref buffer,(byte)SatCnt);
            BinSerialize.WriteByte(ref buffer,(byte)FixType);
            /* PayloadByteSize = 63 */;
        }
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: data_index, Units: , IsExtended: false
        /// </summary>
        public uint DataIndex { get; set; }
        /// <summary>
        /// Latitude (WGS84, EGM96 ellipsoid)
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Expected Error in Latitude (North) Direction
        /// OriginName: lat_err, Units: mm, IsExtended: false
        /// </summary>
        public int LatErr { get; set; }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public int Lon { get; set; }
        /// <summary>
        /// Expected Error in Longitude (East) Direction
        /// OriginName: lon_err, Units: mm, IsExtended: false
        /// </summary>
        public int LonErr { get; set; }
        /// <summary>
        /// Antenna altitude above/below mean sea level (geoid)
        /// OriginName: alt_msl, Units: mm, IsExtended: false
        /// </summary>
        public int AltMsl { get; set; }
        /// <summary>
        /// Antenna altitude WGS-84 earth ellipsoid
        /// OriginName: alt_wgs, Units: mm, IsExtended: false
        /// </summary>
        public int AltWgs { get; set; }
        /// <summary>
        /// Expected Error in Altitude
        /// OriginName: alt_err, Units: mm, IsExtended: false
        /// </summary>
        public int AltErr { get; set; }
        /// <summary>
        /// GNSS reference station ID (used when GNSS is received from multiple sources).
        /// OriginName: ref_id, Units: , IsExtended: false
        /// </summary>
        public ushort RefId { get; set; }
        /// <summary>
        /// HDOP horizontal dilution of position
        /// OriginName: hdop, Units: , IsExtended: false
        /// </summary>
        public ushort Hdop { get; set; }
        /// <summary>
        /// VDOP vertical dilution of position
        /// OriginName: vdop, Units: , IsExtended: false
        /// </summary>
        public ushort Vdop { get; set; }
        /// <summary>
        /// Speed over ground
        /// OriginName: sog, Units: cm/s, IsExtended: false
        /// </summary>
        public ushort Sog { get; set; }
        /// <summary>
        /// Course over ground (true) (yaw angle). 0.0..359.99 degrees
        /// OriginName: cog_true, Units: cdeg, IsExtended: false
        /// </summary>
        public ushort CogTrue { get; set; }
        /// <summary>
        /// Course over ground (magnetic) (yaw angle). 0.0..359.99 degrees
        /// OriginName: cog_mag, Units: cdeg, IsExtended: false
        /// </summary>
        public ushort CogMag { get; set; }
        /// <summary>
        /// GNSS special flags.
        /// OriginName: gnss_flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaRttGnssFlags GnssFlags { get; set; }
        /// <summary>
        /// Number of satellites in view
        /// OriginName: sat_cnt, Units: , IsExtended: false
        /// </summary>
        public byte SatCnt { get; set; }
        /// <summary>
        /// GNSS fix type
        /// OriginName: fix_type, Units: , IsExtended: false
        /// </summary>
        public GpsFixType FixType { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_SPECTRUM:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_TX_LLZ:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_TX_GP:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_TX_VOR:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_TX_MARKER:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_DME_REP:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_TX_GBAS:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_ADSB_REQ:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_TX_GNSS:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("tx_freq",
"TX frequency",
string.Empty, 
@"Hz", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("rx_freq",
"RX frequency",
string.Empty, 
@"Hz", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("tx_power",
"Output power",
string.Empty, 
@"dBm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("tx_gain",
"Percent of total TX gain level (0.0 - 1.0)",
string.Empty, 
@"%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_power",
"Receive power (peak)",
string.Empty, 
@"dBm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_field_strength",
"Receive power field strength.",
string.Empty, 
@"uV/m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_signal_overflow",
"Signal overflow indicator (\u22640.2 \u2014 too weak, \u22650.8 \u2014 too strong).",
string.Empty, 
@"%", 
string.Empty, 
@"NaN", 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_gain",
"Percent of total RX gain level (0.0 - 1.0)",
string.Empty, 
@"%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("distance",
"Measured distance",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("reply_efficiency",
"Reply efficiency request\\response (between 0% - 100%)",
string.Empty, 
@"%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_freq_offset",
"RX frequency offset",
string.Empty, 
@"Hz", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("pulse_shape_rise",
"Pulse shape: rise time (\u22643 \u03BCs)",
string.Empty, 
@"ns", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("pulse_shape_duration",
"Pulse shape: rise time (3.5 \u03BCs, \u00B10.5 \u03BCs)",
string.Empty, 
@"ns", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("pulse_shape_decay",
"Pulse shape: rise time (\u22643.5 \u03BCs)",
string.Empty, 
@"ns", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("pulse_spacing",
"Pulse spacing (X channel 12 \u00B10.25 us, Y channel: 30 \u00B10.25 us)",
string.Empty, 
@"ns", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("req_freq",
"Number of our request",
string.Empty, 
@"pps", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("hip_freq",
"Measured number of all replies, that was recognised as beacon HIP",
string.Empty, 
@"pps", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("measure_time",
"Measure time.",
string.Empty, 
@"ms", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("pulse_shape_amplitude",
"Pulse shape: amplitude (between 95% rise/fall amplitudes, \u226595% of maximum amplitude)",
string.Empty, 
@"%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int8, 
            0, 
false),
            new("code_id",
"Code identification",
string.Empty, 
@"Letters", 
string.Empty, 
string.Empty, 
            MessageFieldType.Char, 
            4, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_DME_REQ:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint64_t tx_freq;"
        + "uint64_t rx_freq;"
        + "uint32_t index;"
        + "float tx_power;"
        + "float tx_gain;"
        + "float rx_power;"
        + "float rx_field_strength;"
        + "float rx_signal_overflow;"
        + "float rx_gain;"
        + "float distance;"
        + "float reply_efficiency;"
        + "int16_t rx_freq_offset;"
        + "uint16_t pulse_shape_rise;"
        + "uint16_t pulse_shape_duration;"
        + "uint16_t pulse_shape_decay;"
        + "uint16_t pulse_spacing;"
        + "uint16_t req_freq;"
        + "uint16_t hip_freq;"
        + "int16_t measure_time;"
        + "int8_t pulse_shape_amplitude;"
        + "char[4] code_id;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=8; //TxFreq
            sum+=8; //RxFreq
            sum+=4; //Index
            sum+=4; //TxPower
            sum+=4; //TxGain
            sum+=4; //RxPower
            sum+=4; //RxFieldStrength
            sum+=4; //RxSignalOverflow
            sum+=4; //RxGain
            sum+=4; //Distance
            sum+=4; //ReplyEfficiency
            sum+=2; //RxFreqOffset
            sum+=2; //PulseShapeRise
            sum+=2; //PulseShapeDuration
            sum+=2; //PulseShapeDecay
            sum+=2; //PulseSpacing
            sum+=2; //ReqFreq
            sum+=2; //HipFreq
            sum+=2; //MeasureTime
            sum+=1; //PulseShapeAmplitude
            sum+=CodeId.Length; //CodeId
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// TX frequency
        /// OriginName: tx_freq, Units: Hz, IsExtended: false
        /// </summary>
        public ulong TxFreq { get; set; }
        /// <summary>
        /// RX frequency
        /// OriginName: rx_freq, Units: Hz, IsExtended: false
        /// </summary>
        public ulong RxFreq { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
        /// <summary>
        /// Output power
        /// OriginName: tx_power, Units: dBm, IsExtended: false
        /// </summary>
        public float TxPower { get; set; }
        /// <summary>
        /// Percent of total TX gain level (0.0 - 1.0)
        /// OriginName: tx_gain, Units: %, IsExtended: false
        /// </summary>
        public float TxGain { get; set; }
        /// <summary>
        /// Receive power (peak)
        /// OriginName: rx_power, Units: dBm, IsExtended: false
        /// </summary>
        public float RxPower { get; set; }
        /// <summary>
        /// Receive power field strength.
        /// OriginName: rx_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public float RxFieldStrength { get; set; }
        /// <summary>
        /// Signal overflow indicator (≤0.2 — too weak, ≥0.8 — too strong).
        /// OriginName: rx_signal_overflow, Units: %, IsExtended: false
        /// </summary>
        public float RxSignalOverflow { get; set; }
        /// <summary>
        /// Percent of total RX gain level (0.0 - 1.0)
        /// OriginName: rx_gain, Units: %, IsExtended: false
        /// </summary>
        public float RxGain { get; set; }
        /// <summary>
        /// Measured distance
        /// OriginName: distance, Units: m, IsExtended: false
        /// </summary>
        public float Distance { get; set; }
        /// <summary>
        /// Reply efficiency request\response (between 0% - 100%)
        /// OriginName: reply_efficiency, Units: %, IsExtended: false
        /// </summary>
        public float ReplyEfficiency { get; set; }
        /// <summary>
        /// RX frequency offset
        /// OriginName: rx_freq_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short RxFreqOffset { get; set; }
        /// <summary>
        /// Pulse shape: rise time (≤3 μs)
        /// OriginName: pulse_shape_rise, Units: ns, IsExtended: false
        /// </summary>
        public ushort PulseShapeRise { get; set; }
        /// <summary>
        /// Pulse shape: rise time (3.5 μs, ±0.5 μs)
        /// OriginName: pulse_shape_duration, Units: ns, IsExtended: false
        /// </summary>
        public ushort PulseShapeDuration { get; set; }
        /// <summary>
        /// Pulse shape: rise time (≤3.5 μs)
        /// OriginName: pulse_shape_decay, Units: ns, IsExtended: false
        /// </summary>
        public ushort PulseShapeDecay { get; set; }
        /// <summary>
        /// Pulse spacing (X channel 12 ±0.25 us, Y channel: 30 ±0.25 us)
        /// OriginName: pulse_spacing, Units: ns, IsExtended: false
        /// </summary>
        public ushort PulseSpacing { get; set; }
        /// <summary>
        /// Number of our request
        /// OriginName: req_freq, Units: pps, IsExtended: false
        /// </summary>
        public ushort ReqFreq { get; set; }
        /// <summary>
        /// Measured number of all replies, that was recognised as beacon HIP
        /// OriginName: hip_freq, Units: pps, IsExtended: false
        /// </summary>
        public ushort HipFreq { get; set; }
        /// <summary>
        /// Measure time.
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public short MeasureTime { get; set; }
        /// <summary>
        /// Pulse shape: amplitude (between 95% rise/fall amplitudes, ≥95% of maximum amplitude)
        /// OriginName: pulse_shape_amplitude, Units: %, IsExtended: false
        /// </summary>
        public sbyte PulseShapeAmplitude { get; set; }
        /// <summary>
        /// Code identification
        /// OriginName: code_id, Units: Letters, IsExtended: false
        /// </summary>
        public const int CodeIdMaxItemsCount = 4;
        public char[] CodeId { get; set; } = new char[4];
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_RX_LLZ:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_RX_GP:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_RX_VOR:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_RX_MARKER:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_RX_GBAS:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("tx_freq",
"TX frequency",
string.Empty, 
@"Hz", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("rx_freq",
"RX frequency",
string.Empty, 
@"Hz", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("tx_power",
"Output power",
string.Empty, 
@"dBm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("tx_gain",
"Percent of total TX gain level (0.0 - 1.0)",
string.Empty, 
@"%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_power",
"Receive power (peak)",
string.Empty, 
@"dBm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_field_strength",
"Receive power field strength.",
string.Empty, 
@"uV/m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_signal_overflow",
"Signal overflow indicator (\u22640.2 \u2014 too weak, \u22650.8 \u2014 too strong).",
string.Empty, 
@"%", 
string.Empty, 
@"NaN", 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_gain",
"Percent of total RX gain level (0.0 - 1.0)",
string.Empty, 
@"%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("icao_address",
"Vehicle ICAO address (24 bit)",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("uf_counter_flag",
"UF counters present flag ",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("df_counter_present",
"UF counters present ",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("rx_freq_offset",
"RX frequency offset",
string.Empty, 
@"Hz", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("ref_id",
"GNSS reference station ID (used when GNSS is received from multiple sources).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("squawk",
"Mode A code (typically 1200 [0x04B0] for VFR)",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("call_sign",
"Vehicle identifier (8 characters, valid characters are A-Z, 0-9, \" \" only)",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Char, 
            8, 
false),
            new("uf_counter",
"UF incremental counters for every 25 message ",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            25, 
false),
            new("df_counter",
"DF incremental counters for every 25 message ",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            25, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_ADSB_REP:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint64_t tx_freq;"
        + "uint64_t rx_freq;"
        + "uint32_t index;"
        + "float tx_power;"
        + "float tx_gain;"
        + "float rx_power;"
        + "float rx_field_strength;"
        + "float rx_signal_overflow;"
        + "float rx_gain;"
        + "uint32_t icao_address;"
        + "uint32_t uf_counter_flag;"
        + "uint32_t df_counter_present;"
        + "int16_t rx_freq_offset;"
        + "uint16_t ref_id;"
        + "uint16_t squawk;"
        + "char[8] call_sign;"
        + "uint8_t[25] uf_counter;"
        + "uint8_t[25] df_counter;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=8; //TxFreq
            sum+=8; //RxFreq
            sum+=4; //Index
            sum+=4; //TxPower
            sum+=4; //TxGain
            sum+=4; //RxPower
            sum+=4; //RxFieldStrength
            sum+=4; //RxSignalOverflow
            sum+=4; //RxGain
            sum+=4; //IcaoAddress
            sum+= 4; // UfCounterFlag
            sum+= 4; // DfCounterPresent
            sum+=2; //RxFreqOffset
            sum+=2; //RefId
            sum+=2; //Squawk
            sum+=CallSign.Length; //CallSign
            sum+=UfCounter.Length; //UfCounter
            sum+=DfCounter.Length; //DfCounter
            return (byte)sum;
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
            buffer = buffer.Slice(arraySize);
           
            arraySize = /*ArrayLength*/25 - Math.Max(0,((/*PayloadByteSize*/136 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            UfCounter = new byte[arraySize];
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// TX frequency
        /// OriginName: tx_freq, Units: Hz, IsExtended: false
        /// </summary>
        public ulong TxFreq { get; set; }
        /// <summary>
        /// RX frequency
        /// OriginName: rx_freq, Units: Hz, IsExtended: false
        /// </summary>
        public ulong RxFreq { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
        /// <summary>
        /// Output power
        /// OriginName: tx_power, Units: dBm, IsExtended: false
        /// </summary>
        public float TxPower { get; set; }
        /// <summary>
        /// Percent of total TX gain level (0.0 - 1.0)
        /// OriginName: tx_gain, Units: %, IsExtended: false
        /// </summary>
        public float TxGain { get; set; }
        /// <summary>
        /// Receive power (peak)
        /// OriginName: rx_power, Units: dBm, IsExtended: false
        /// </summary>
        public float RxPower { get; set; }
        /// <summary>
        /// Receive power field strength.
        /// OriginName: rx_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public float RxFieldStrength { get; set; }
        /// <summary>
        /// Signal overflow indicator (≤0.2 — too weak, ≥0.8 — too strong).
        /// OriginName: rx_signal_overflow, Units: %, IsExtended: false
        /// </summary>
        public float RxSignalOverflow { get; set; }
        /// <summary>
        /// Percent of total RX gain level (0.0 - 1.0)
        /// OriginName: rx_gain, Units: %, IsExtended: false
        /// </summary>
        public float RxGain { get; set; }
        /// <summary>
        /// Vehicle ICAO address (24 bit)
        /// OriginName: icao_address, Units: , IsExtended: false
        /// </summary>
        public uint IcaoAddress { get; set; }
        /// <summary>
        /// UF counters present flag 
        /// OriginName: uf_counter_flag, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaRttAdsbMsgUf UfCounterFlag { get; set; }
        /// <summary>
        /// UF counters present 
        /// OriginName: df_counter_present, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaRttAdsbMsgDf DfCounterPresent { get; set; }
        /// <summary>
        /// RX frequency offset
        /// OriginName: rx_freq_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short RxFreqOffset { get; set; }
        /// <summary>
        /// GNSS reference station ID (used when GNSS is received from multiple sources).
        /// OriginName: ref_id, Units: , IsExtended: false
        /// </summary>
        public ushort RefId { get; set; }
        /// <summary>
        /// Mode A code (typically 1200 [0x04B0] for VFR)
        /// OriginName: squawk, Units: , IsExtended: false
        /// </summary>
        public ushort Squawk { get; set; }
        /// <summary>
        /// Vehicle identifier (8 characters, valid characters are A-Z, 0-9, " " only)
        /// OriginName: call_sign, Units: , IsExtended: false
        /// </summary>
        public const int CallSignMaxItemsCount = 8;
        public char[] CallSign { get; } = new char[8];
        /// <summary>
        /// UF incremental counters for every 25 message 
        /// OriginName: uf_counter, Units: , IsExtended: false
        /// </summary>
        public const int UfCounterMaxItemsCount = 25;
        public byte[] UfCounter { get; set; } = new byte[25];
        [Obsolete("This method is deprecated. Use GetUfCounterMaxItemsCount instead.")]
        public byte GetUfCounterMaxItemsCount() => 25;
        /// <summary>
        /// DF incremental counters for every 25 message 
        /// OriginName: df_counter, Units: , IsExtended: false
        /// </summary>
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_unix_usec",
"Timestamp (UNIX epoch time).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("flags",
"Data flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("index",
"Data index in record",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RSGA_RTT_RX_GNSS:"
        + "uint64_t time_unix_usec;"
        + "uint64_t flags;"
        + "uint32_t index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+= 8; // Flags
            sum+=4; //Index
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Data flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public AsvRsgaDataFlags Flags { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public uint Index { get; set; }
    }


#endregion


}

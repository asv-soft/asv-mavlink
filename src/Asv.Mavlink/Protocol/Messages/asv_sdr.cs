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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.2+82bde669fa8b85517700c6d12362e9f17d819d33 25-06-23.

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

namespace Asv.Mavlink.AsvSdr
{

    public static class AsvSdrHelper
    {
        public static void RegisterAsvSdrDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(AsvSdrOutStatusPacket.MessageId, ()=>new AsvSdrOutStatusPacket());
            src.Add(AsvSdrRecordRequestPacket.MessageId, ()=>new AsvSdrRecordRequestPacket());
            src.Add(AsvSdrRecordResponsePacket.MessageId, ()=>new AsvSdrRecordResponsePacket());
            src.Add(AsvSdrRecordPacket.MessageId, ()=>new AsvSdrRecordPacket());
            src.Add(AsvSdrRecordDeleteRequestPacket.MessageId, ()=>new AsvSdrRecordDeleteRequestPacket());
            src.Add(AsvSdrRecordDeleteResponsePacket.MessageId, ()=>new AsvSdrRecordDeleteResponsePacket());
            src.Add(AsvSdrRecordTagRequestPacket.MessageId, ()=>new AsvSdrRecordTagRequestPacket());
            src.Add(AsvSdrRecordTagResponsePacket.MessageId, ()=>new AsvSdrRecordTagResponsePacket());
            src.Add(AsvSdrRecordTagPacket.MessageId, ()=>new AsvSdrRecordTagPacket());
            src.Add(AsvSdrRecordTagDeleteRequestPacket.MessageId, ()=>new AsvSdrRecordTagDeleteRequestPacket());
            src.Add(AsvSdrRecordTagDeleteResponsePacket.MessageId, ()=>new AsvSdrRecordTagDeleteResponsePacket());
            src.Add(AsvSdrRecordDataRequestPacket.MessageId, ()=>new AsvSdrRecordDataRequestPacket());
            src.Add(AsvSdrRecordDataResponsePacket.MessageId, ()=>new AsvSdrRecordDataResponsePacket());
            src.Add(AsvSdrCalibAccPacket.MessageId, ()=>new AsvSdrCalibAccPacket());
            src.Add(AsvSdrCalibTableReadPacket.MessageId, ()=>new AsvSdrCalibTableReadPacket());
            src.Add(AsvSdrCalibTablePacket.MessageId, ()=>new AsvSdrCalibTablePacket());
            src.Add(AsvSdrCalibTableRowReadPacket.MessageId, ()=>new AsvSdrCalibTableRowReadPacket());
            src.Add(AsvSdrCalibTableRowPacket.MessageId, ()=>new AsvSdrCalibTableRowPacket());
            src.Add(AsvSdrCalibTableUploadStartPacket.MessageId, ()=>new AsvSdrCalibTableUploadStartPacket());
            src.Add(AsvSdrCalibTableUploadReadCallbackPacket.MessageId, ()=>new AsvSdrCalibTableUploadReadCallbackPacket());
            src.Add(AsvSdrSignalRawPacket.MessageId, ()=>new AsvSdrSignalRawPacket());
            src.Add(AsvSdrRecordDataLlzPacket.MessageId, ()=>new AsvSdrRecordDataLlzPacket());
            src.Add(AsvSdrRecordDataGpPacket.MessageId, ()=>new AsvSdrRecordDataGpPacket());
            src.Add(AsvSdrRecordDataVorPacket.MessageId, ()=>new AsvSdrRecordDataVorPacket());
        }
 
    }

#region Enums

    /// <summary>
    ///  MAV_TYPE
    /// </summary>
    public enum MavType : ulong
    {
        /// <summary>
        /// Used to identify Software-defined radio payload in HEARTBEAT packet.
        /// MAV_TYPE_ASV_SDR_PAYLOAD
        /// </summary>
        MavTypeAsvSdrPayload = 251,
    }
    public static class MavTypeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(251);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(251),"MAV_TYPE_ASV_SDR_PAYLOAD");
        }
    }
    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd : ulong
    {
        /// <summary>
        /// Start one of ASV_SDR_CUSTOM_MODE. Can be used in the mission protocol for SDR payloads.
        /// Param 1 - Mode (uint32_t, see ASV_SDR_CUSTOM_MODE).
        /// Param 2 - Frequency in Hz, 0-3 bytes of uint_64, ignored for IDLE mode (uint32).
        /// Param 3 - Frequency in Hz, 4-7 bytes of uint_64, ignored for IDLE mode (uint32).
        /// Param 4 - Record rate in Hz, ignored for IDLE mode (float).
        /// Param 5 - Sending data thinning ratio. Each specified amount of recorded data will be skipped before it is sent over the communication channel. (uint32).
        /// Param 6 - Estimated reference power in dBm. Needed to tune the internal amplifiers and filters (float).
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_SET_MODE
        /// </summary>
        MavCmdAsvSdrSetMode = 13100,
        /// <summary>
        /// Start recoring data with unique name (max 28 chars). Can be used in the mission protocol for SDR payloads.
        /// Param 1 - Record unique name: 0-3 chars (char[4]).
        /// Param 2 - Record unique name: 4-7 chars (char[4]).
        /// Param 3 - Record unique name: 8-11 chars (char[4]).
        /// Param 4 - Record unique name: 12-15 chars (char[4]).
        /// Param 5 - Record unique name: 16-19 chars (char[4]).
        /// Param 6 - Record unique name: 20-23 chars (char[4]).
        /// Param 7 - Record unique name: 24-27 chars (char[4]).
        /// MAV_CMD_ASV_SDR_START_RECORD
        /// </summary>
        MavCmdAsvSdrStartRecord = 13101,
        /// <summary>
        /// Stop recoring data. Can be used in the mission protocol for SDR payloads.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_STOP_RECORD
        /// </summary>
        MavCmdAsvSdrStopRecord = 13102,
        /// <summary>
        /// Set custom tag to current record. Can be used in the mission protocol for SDR payloads.
        /// Param 1 - ASV_SDR_RECORD_TAG_TYPE (uint32).
        /// Param 2 - Tag name: 0-3 chars (char[4]).
        /// Param 3 - Tag name: 4-7 chars (char[4]).
        /// Param 4 - Tag name: 8-11 chars (char[4]).
        /// Param 5 - Tag name: 12-15 chars (char[4]).
        /// Param 6 - Tag value data 0-3 bytes depends on the type of tag.
        /// Param 7 - Tag value data 4-7 bytes depends on the type of tag.
        /// MAV_CMD_ASV_SDR_SET_RECORD_TAG
        /// </summary>
        MavCmdAsvSdrSetRecordTag = 13103,
        /// <summary>
        /// Send shutdown or reboot command. Can't be used in the mission protocol for SDR payloads.
        /// Param 1 - ASV_SDR_SYSTEM_CONTROL_ACTION (uint32).
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_SYSTEM_CONTROL_ACTION
        /// </summary>
        MavCmdAsvSdrSystemControlAction = 13104,
        /// <summary>
        /// Waiting for a vehicle mission waypoint point. Only used in the mission protocol for SDR payloads.
        /// Param 1 - Waypoint index (uint32).
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_WAIT_VEHICLE_WAYPOINT
        /// </summary>
        MavCmdAsvSdrWaitVehicleWaypoint = 13105,
        /// <summary>
        /// Waiting a certain amount of time. Only used in the mission protocol for SDR payloads.
        /// Param 1 - Delay in ms (uint32).
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_DELAY
        /// </summary>
        MavCmdAsvSdrDelay = 13106,
        /// <summary>
        /// Starting mission, uploaded to SDR payload. Can't be used in the mission protocol for SDR payloads.
        /// Param 1 - Index of the task to start the mission (uint32).
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_START_MISSION
        /// </summary>
        MavCmdAsvSdrStartMission = 13107,
        /// <summary>
        /// Start mission, uploaded to SDR. Can't be used in the mission protocol for SDR payloads.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_STOP_MISSION
        /// </summary>
        MavCmdAsvSdrStopMission = 13108,
        /// <summary>
        /// Start calibration process. Can't be used in the mission protocol for SDR payloads.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_START_CALIBRATION
        /// </summary>
        MavCmdAsvSdrStartCalibration = 13109,
        /// <summary>
        /// Stop calibration process. Can't be used in the mission protocol for SDR payloads.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_STOP_CALIBRATION
        /// </summary>
        MavCmdAsvSdrStopCalibration = 13110,
    }
    public static class MavCmdHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(13100);
            yield return converter(13101);
            yield return converter(13102);
            yield return converter(13103);
            yield return converter(13104);
            yield return converter(13105);
            yield return converter(13106);
            yield return converter(13107);
            yield return converter(13108);
            yield return converter(13109);
            yield return converter(13110);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(13100),"MAV_CMD_ASV_SDR_SET_MODE");
            yield return new EnumValue<T>(converter(13101),"MAV_CMD_ASV_SDR_START_RECORD");
            yield return new EnumValue<T>(converter(13102),"MAV_CMD_ASV_SDR_STOP_RECORD");
            yield return new EnumValue<T>(converter(13103),"MAV_CMD_ASV_SDR_SET_RECORD_TAG");
            yield return new EnumValue<T>(converter(13104),"MAV_CMD_ASV_SDR_SYSTEM_CONTROL_ACTION");
            yield return new EnumValue<T>(converter(13105),"MAV_CMD_ASV_SDR_WAIT_VEHICLE_WAYPOINT");
            yield return new EnumValue<T>(converter(13106),"MAV_CMD_ASV_SDR_DELAY");
            yield return new EnumValue<T>(converter(13107),"MAV_CMD_ASV_SDR_START_MISSION");
            yield return new EnumValue<T>(converter(13108),"MAV_CMD_ASV_SDR_STOP_MISSION");
            yield return new EnumValue<T>(converter(13109),"MAV_CMD_ASV_SDR_START_CALIBRATION");
            yield return new EnumValue<T>(converter(13110),"MAV_CMD_ASV_SDR_STOP_CALIBRATION");
        }
    }
    /// <summary>
    /// State of the current mission (unit8_t).
    ///  ASV_SDR_MISSION_STATE
    /// </summary>
    public enum AsvSdrMissionState : ulong
    {
        /// <summary>
        /// Do nothing
        /// ASV_SDR_MISSION_STATE_IDLE
        /// </summary>
        AsvSdrMissionStateIdle = 1,
        /// <summary>
        /// Mission in progress
        /// ASV_SDR_MISSION_STATE_PROGRESS
        /// </summary>
        AsvSdrMissionStateProgress = 2,
        /// <summary>
        /// Mission failed
        /// ASV_SDR_MISSION_STATE_ERROR
        /// </summary>
        AsvSdrMissionStateError = 3,
    }
    public static class AsvSdrMissionStateHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_SDR_MISSION_STATE_IDLE");
            yield return new EnumValue<T>(converter(2),"ASV_SDR_MISSION_STATE_PROGRESS");
            yield return new EnumValue<T>(converter(3),"ASV_SDR_MISSION_STATE_ERROR");
        }
    }
    /// <summary>
    /// Specifies the datatype of a record tag (unit8_t).
    ///  ASV_SDR_RECORD_TAG_TYPE
    /// </summary>
    public enum AsvSdrRecordTagType : ulong
    {
        /// <summary>
        /// 64-bit unsigned integer
        /// ASV_SDR_RECORD_TAG_TYPE_UINT64
        /// </summary>
        AsvSdrRecordTagTypeUint64 = 1,
        /// <summary>
        /// 64-bit signed integer
        /// ASV_SDR_RECORD_TAG_TYPE_INT64
        /// </summary>
        AsvSdrRecordTagTypeInt64 = 2,
        /// <summary>
        /// 64-bit floating-point
        /// ASV_SDR_RECORD_TAG_TYPE_REAL64
        /// </summary>
        AsvSdrRecordTagTypeReal64 = 3,
        /// <summary>
        /// String type terminated by NULL if the length is less than 8 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 8 chars.
        /// ASV_SDR_RECORD_TAG_TYPE_STRING8
        /// </summary>
        AsvSdrRecordTagTypeString8 = 4,
    }
    public static class AsvSdrRecordTagTypeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
            yield return converter(4);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_SDR_RECORD_TAG_TYPE_UINT64");
            yield return new EnumValue<T>(converter(2),"ASV_SDR_RECORD_TAG_TYPE_INT64");
            yield return new EnumValue<T>(converter(3),"ASV_SDR_RECORD_TAG_TYPE_REAL64");
            yield return new EnumValue<T>(converter(4),"ASV_SDR_RECORD_TAG_TYPE_STRING8");
        }
    }
    /// <summary>
    /// A mapping of SDR payload modes for custom_mode field of heartbeat. Value of enum must be equal to message id from ASV_SDR_RECORD_DATA_* 13150-13199
    ///  ASV_SDR_CUSTOM_MODE
    /// </summary>
    public enum AsvSdrCustomMode : ulong
    {
        /// <summary>
        /// Default mode. Do nothing.
        /// ASV_SDR_CUSTOM_MODE_IDLE
        /// </summary>
        AsvSdrCustomModeIdle = 0,
        /// <summary>
        /// Localizer measure mode. In this mode should send and record ASV_SDR_RECORD_DATA_LLZ
        /// ASV_SDR_CUSTOM_MODE_LLZ
        /// </summary>
        AsvSdrCustomModeLlz = 13135,
        /// <summary>
        /// Glide Path measure mode. In this mode should send and record ASV_SDR_RECORD_DATA_GP
        /// ASV_SDR_CUSTOM_MODE_GP
        /// </summary>
        AsvSdrCustomModeGp = 13136,
        /// <summary>
        /// VOR measure mode. In this mode should send and record ASV_SDR_RECORD_DATA_VOR
        /// ASV_SDR_CUSTOM_MODE_VOR
        /// </summary>
        AsvSdrCustomModeVor = 13137,
    }
    public static class AsvSdrCustomModeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(13135);
            yield return converter(13136);
            yield return converter(13137);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_SDR_CUSTOM_MODE_IDLE");
            yield return new EnumValue<T>(converter(13135),"ASV_SDR_CUSTOM_MODE_LLZ");
            yield return new EnumValue<T>(converter(13136),"ASV_SDR_CUSTOM_MODE_GP");
            yield return new EnumValue<T>(converter(13137),"ASV_SDR_CUSTOM_MODE_VOR");
        }
    }
    /// <summary>
    /// These flags encode supported mode.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_SDR_CUSTOM_MODE_FLAG
    /// </summary>
    [Flags]
    public enum AsvSdrCustomModeFlag : ulong
    {
        /// <summary>
        /// ASV_SDR_CUSTOM_MODE_FLAG_LLZ
        /// </summary>
        AsvSdrCustomModeFlagLlz = 1,
        /// <summary>
        /// ASV_SDR_CUSTOM_MODE_FLAG_GP
        /// </summary>
        AsvSdrCustomModeFlagGp = 2,
        /// <summary>
        /// ASV_SDR_CUSTOM_MODE_FLAG_VOR
        /// </summary>
        AsvSdrCustomModeFlagVor = 4,
    }
    public static class AsvSdrCustomModeFlagHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(4);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_SDR_CUSTOM_MODE_FLAG_LLZ");
            yield return new EnumValue<T>(converter(2),"ASV_SDR_CUSTOM_MODE_FLAG_GP");
            yield return new EnumValue<T>(converter(4),"ASV_SDR_CUSTOM_MODE_FLAG_VOR");
        }
    }
    /// <summary>
    /// ACK / NACK / ERROR values as a result of ASV_SDR_*_REQUEST or ASV_SDR_*_DELETE commands.
    ///  ASV_SDR_REQUEST_ACK
    /// </summary>
    public enum AsvSdrRequestAck : ulong
    {
        /// <summary>
        /// Request is ok.
        /// ASV_SDR_REQUEST_ACK_OK
        /// </summary>
        AsvSdrRequestAckOk = 0,
        /// <summary>
        /// Command already in progress.
        /// ASV_SDR_REQUEST_ACK_IN_PROGRESS
        /// </summary>
        AsvSdrRequestAckInProgress = 1,
        /// <summary>
        /// Command error.
        /// ASV_SDR_REQUEST_ACK_FAIL
        /// </summary>
        AsvSdrRequestAckFail = 2,
        /// <summary>
        /// Not supported command.
        /// ASV_SDR_REQUEST_ACK_NOT_SUPPORTED
        /// </summary>
        AsvSdrRequestAckNotSupported = 3,
    }
    public static class AsvSdrRequestAckHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_SDR_REQUEST_ACK_OK");
            yield return new EnumValue<T>(converter(1),"ASV_SDR_REQUEST_ACK_IN_PROGRESS");
            yield return new EnumValue<T>(converter(2),"ASV_SDR_REQUEST_ACK_FAIL");
            yield return new EnumValue<T>(converter(3),"ASV_SDR_REQUEST_ACK_NOT_SUPPORTED");
        }
    }
    /// <summary>
    /// SDR system control actions [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_SYSTEM_CONTROL_ACTION
    /// </summary>
    public enum AsvSdrSystemControlAction : ulong
    {
        /// <summary>
        /// Request system reboot [!WRAP_TO_V2_EXTENSION_PACKET!]
        /// ASV_SDR_SYSTEM_CONTROL_ACTION_REBOOT
        /// </summary>
        AsvSdrSystemControlActionReboot = 0,
        /// <summary>
        /// Request system shutdown [!WRAP_TO_V2_EXTENSION_PACKET!]
        /// ASV_SDR_SYSTEM_CONTROL_ACTION_SHUTDOWN
        /// </summary>
        AsvSdrSystemControlActionShutdown = 1,
        /// <summary>
        /// Request software reboot [!WRAP_TO_V2_EXTENSION_PACKET!]
        /// ASV_SDR_SYSTEM_CONTROL_ACTION_RESTART
        /// </summary>
        AsvSdrSystemControlActionRestart = 2,
    }
    public static class AsvSdrSystemControlActionHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_SDR_SYSTEM_CONTROL_ACTION_REBOOT");
            yield return new EnumValue<T>(converter(1),"ASV_SDR_SYSTEM_CONTROL_ACTION_SHUTDOWN");
            yield return new EnumValue<T>(converter(2),"ASV_SDR_SYSTEM_CONTROL_ACTION_RESTART");
        }
    }
    /// <summary>
    /// Status of calibration process.
    ///  ASV_SDR_CALIB_STATE
    /// </summary>
    public enum AsvSdrCalibState : ulong
    {
        /// <summary>
        /// Calibration not supported by device. Commands MAV_CMD_ASV_SDR_START_CALIBRATION and MAV_CMD_ASV_SDR_STOP_CALIBRATION not supported.
        /// ASV_SDR_CALIB_STATE_NOT_SUPPORTED
        /// </summary>
        AsvSdrCalibStateNotSupported = 0,
        /// <summary>
        /// Normal measure mode. Calibration table USED for measures.
        /// ASV_SDR_CALIB_STATE_OK
        /// </summary>
        AsvSdrCalibStateOk = 1,
        /// <summary>
        /// Calibration progress started. Table NOT! used for calculating values. All measures send as raw values.
        /// ASV_SDR_CALIB_STATE_PROGRESS
        /// </summary>
        AsvSdrCalibStateProgress = 2,
    }
    public static class AsvSdrCalibStateHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_SDR_CALIB_STATE_NOT_SUPPORTED");
            yield return new EnumValue<T>(converter(1),"ASV_SDR_CALIB_STATE_OK");
            yield return new EnumValue<T>(converter(2),"ASV_SDR_CALIB_STATE_PROGRESS");
        }
    }
    /// <summary>
    /// SDR signal transmition data type
    ///  ASV_SDR_SIGNAL_FORMAT
    /// </summary>
    public enum AsvSdrSignalFormat : ulong
    {
        /// <summary>
        /// Write a value as a fraction between a given minimum and maximum. Uses 8 bits so we have '256' steps between min and max.
        /// ASV_SDR_SIGNAL_FORMAT_RANGE_FLOAT_8BIT
        /// </summary>
        AsvSdrSignalFormatRangeFloat8bit = 0,
        /// <summary>
        /// Write a value as a fraction between a given minimum and maximum. Uses 16 bits so we have '65535' steps between min and max.
        /// ASV_SDR_SIGNAL_FORMAT_RANGE_FLOAT_16BIT
        /// </summary>
        AsvSdrSignalFormatRangeFloat16bit = 1,
        /// <summary>
        /// Write a value as a float. Uses 32 bits.
        /// ASV_SDR_SIGNAL_FORMAT_FLOAT
        /// </summary>
        AsvSdrSignalFormatFloat = 2,
    }
    public static class AsvSdrSignalFormatHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_SDR_SIGNAL_FORMAT_RANGE_FLOAT_8BIT");
            yield return new EnumValue<T>(converter(1),"ASV_SDR_SIGNAL_FORMAT_RANGE_FLOAT_16BIT");
            yield return new EnumValue<T>(converter(2),"ASV_SDR_SIGNAL_FORMAT_FLOAT");
        }
    }

#endregion

#region Messages

    /// <summary>
    /// SDR payload status message. Send with 1 Hz frequency [!WRAP_TO_V2_EXTENSION_PACKET!].
    ///  ASV_SDR_OUT_STATUS
    /// </summary>
    public class AsvSdrOutStatusPacket : MavlinkV2Message<AsvSdrOutStatusPayload>
    {
        public const int MessageId = 13100;
        
        public const byte CrcExtra = 145;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrOutStatusPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_OUT_STATUS";
    }

    /// <summary>
    ///  ASV_SDR_OUT_STATUS
    /// </summary>
    public class AsvSdrOutStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 77; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 77; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 8 // uint64_t supported_modes
            +8 // uint64_t size
            +2 // uint16_t record_count
            +2 // uint16_t current_mission_index
            +CurrentRecordGuid.Length // uint8_t[16] current_record_guid
            + 1 // uint8_t current_record_mode
            +CurrentRecordName.Length // char[28] current_record_name
            + 1 // uint8_t mission_state
            + 1 // uint8_t calib_state
            +2 // uint16_t calib_table_count
            +4 // float ref_power
            +4 // float signal_overflow
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            SupportedModes = (AsvSdrCustomModeFlag)BinSerialize.ReadULong(ref buffer);
            Size = BinSerialize.ReadULong(ref buffer);
            RecordCount = BinSerialize.ReadUShort(ref buffer);
            CurrentMissionIndex = BinSerialize.ReadUShort(ref buffer);
            arraySize = 16;
            for(var i=0;i<arraySize;i++)
            {
                CurrentRecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            CurrentRecordMode = (AsvSdrCustomMode)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/28 - Math.Max(0,((/*PayloadByteSize*/77 - payloadSize - /*ExtendedFieldsLength*/11)/1 /*FieldTypeByteSize*/));
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CurrentRecordName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, CurrentRecordName.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            MissionState = (AsvSdrMissionState)BinSerialize.ReadByte(ref buffer);
            // extended field 'CalibState' can be empty
            if (buffer.IsEmpty) return;
            CalibState = (AsvSdrCalibState)BinSerialize.ReadByte(ref buffer);
            // extended field 'CalibTableCount' can be empty
            if (buffer.IsEmpty) return;
            CalibTableCount = BinSerialize.ReadUShort(ref buffer);
            // extended field 'RefPower' can be empty
            if (buffer.IsEmpty) return;
            RefPower = BinSerialize.ReadFloat(ref buffer);
            // extended field 'SignalOverflow' can be empty
            if (buffer.IsEmpty) return;
            SignalOverflow = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,(ulong)SupportedModes);
            BinSerialize.WriteULong(ref buffer,Size);
            BinSerialize.WriteUShort(ref buffer,RecordCount);
            BinSerialize.WriteUShort(ref buffer,CurrentMissionIndex);
            for(var i=0;i<CurrentRecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)CurrentRecordGuid[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)CurrentRecordMode);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CurrentRecordName)
                {
                    Encoding.ASCII.GetBytes(charPointer, CurrentRecordName.Length, bytePointer, CurrentRecordName.Length);
                }
            }
            buffer = buffer.Slice(CurrentRecordName.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)MissionState);
            BinSerialize.WriteByte(ref buffer,(byte)CalibState);
            BinSerialize.WriteUShort(ref buffer,CalibTableCount);
            BinSerialize.WriteFloat(ref buffer,RefPower);
            BinSerialize.WriteFloat(ref buffer,SignalOverflow);
            /* PayloadByteSize = 77 */;
        }

        public void Accept(IVisitor visitor)
        {
            var tmpSupportedModes = (ulong)SupportedModes;
            UInt64Type.Accept(visitor,SupportedModesField, SupportedModesField.DataType, ref tmpSupportedModes);
            SupportedModes = (AsvSdrCustomModeFlag)tmpSupportedModes;
            UInt64Type.Accept(visitor,SizeField, SizeField.DataType, ref _size);    
            UInt16Type.Accept(visitor,RecordCountField, RecordCountField.DataType, ref _recordCount);    
            UInt16Type.Accept(visitor,CurrentMissionIndexField, CurrentMissionIndexField.DataType, ref _currentMissionIndex);    
            ArrayType.Accept(visitor,CurrentRecordGuidField, CurrentRecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref CurrentRecordGuid[index]));    
            var tmpCurrentRecordMode = (byte)CurrentRecordMode;
            UInt8Type.Accept(visitor,CurrentRecordModeField, CurrentRecordModeField.DataType, ref tmpCurrentRecordMode);
            CurrentRecordMode = (AsvSdrCustomMode)tmpCurrentRecordMode;
            ArrayType.Accept(visitor,CurrentRecordNameField, CurrentRecordNameField.DataType, 28, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref CurrentRecordName[index]));
            var tmpMissionState = (byte)MissionState;
            UInt8Type.Accept(visitor,MissionStateField, MissionStateField.DataType, ref tmpMissionState);
            MissionState = (AsvSdrMissionState)tmpMissionState;
            var tmpCalibState = (byte)CalibState;
            UInt8Type.Accept(visitor,CalibStateField, CalibStateField.DataType, ref tmpCalibState);
            CalibState = (AsvSdrCalibState)tmpCalibState;
            UInt16Type.Accept(visitor,CalibTableCountField, CalibTableCountField.DataType, ref _calibTableCount);    
            FloatType.Accept(visitor,RefPowerField, RefPowerField.DataType, ref _refPower);    
            FloatType.Accept(visitor,SignalOverflowField, SignalOverflowField.DataType, ref _signalOverflow);    

        }

        /// <summary>
        /// Supported ASV_SDR_CUSTOM_MODE.
        /// OriginName: supported_modes, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SupportedModesField = new Field.Builder()
            .Name(nameof(SupportedModes))
            .Title("supported_modes")
            .Description("Supported ASV_SDR_CUSTOM_MODE.")
            .DataType(new UInt64Type(AsvSdrCustomModeFlagHelper.GetValues(x=>(ulong)x).Min(),AsvSdrCustomModeFlagHelper.GetValues(x=>(ulong)x).Max()))
            .Enum(AsvSdrCustomModeFlagHelper.GetEnumValues(x=>(ulong)x))
            .Build();
        private AsvSdrCustomModeFlag _supportedModes;
        public AsvSdrCustomModeFlag SupportedModes { get => _supportedModes; set => _supportedModes = value; } 
        /// <summary>
        /// Total storage size in bytes.
        /// OriginName: size, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field SizeField = new Field.Builder()
            .Name(nameof(Size))
            .Title("size")
            .Description("Total storage size in bytes.")
.Units(@"bytes")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _size;
        public ulong Size { get => _size; set => _size = value; }
        /// <summary>
        /// Number of records in storage.
        /// OriginName: record_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordCountField = new Field.Builder()
            .Name(nameof(RecordCount))
            .Title("record_count")
            .Description("Number of records in storage.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _recordCount;
        public ushort RecordCount { get => _recordCount; set => _recordCount = value; }
        /// <summary>
        /// Current mission index.
        /// OriginName: current_mission_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CurrentMissionIndexField = new Field.Builder()
            .Name(nameof(CurrentMissionIndex))
            .Title("current_mission_index")
            .Description("Current mission index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _currentMissionIndex;
        public ushort CurrentMissionIndex { get => _currentMissionIndex; set => _currentMissionIndex = value; }
        /// <summary>
        /// Record GUID. Also by this field we can understand if the data is currently being recorded (GUID!=0x00) or not (GUID==0x00).
        /// OriginName: current_record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CurrentRecordGuidField = new Field.Builder()
            .Name(nameof(CurrentRecordGuid))
            .Title("current_record_guid")
            .Description("Record GUID. Also by this field we can understand if the data is currently being recorded (GUID!=0x00) or not (GUID==0x00).")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int CurrentRecordGuidMaxItemsCount = 16;
        public byte[] CurrentRecordGuid { get; } = new byte[16];
        /// <summary>
        /// Current record mode (record data type).
        /// OriginName: current_record_mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CurrentRecordModeField = new Field.Builder()
            .Name(nameof(CurrentRecordMode))
            .Title("current_record_mode")
            .Description("Current record mode (record data type).")
            .DataType(new UInt8Type(AsvSdrCustomModeHelper.GetValues(x=>(byte)x).Min(),AsvSdrCustomModeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrCustomModeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrCustomMode _currentRecordMode;
        public AsvSdrCustomMode CurrentRecordMode { get => _currentRecordMode; set => _currentRecordMode = value; } 
        /// <summary>
        /// Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string. If the data is currently not being recorded, than return null; 
        /// OriginName: current_record_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CurrentRecordNameField = new Field.Builder()
            .Name(nameof(CurrentRecordName))
            .Title("current_record_name")
            .Description("Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string. If the data is currently not being recorded, than return null; ")

            .DataType(new ArrayType(CharType.Ascii,28))
        .Build();
        public const int CurrentRecordNameMaxItemsCount = 28;
        public char[] CurrentRecordName { get; } = new char[28];
        [Obsolete("This method is deprecated. Use GetCurrentRecordNameMaxItemsCount instead.")]
        public byte GetCurrentRecordNameMaxItemsCount() => 28;
        /// <summary>
        /// Mission state.
        /// OriginName: mission_state, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MissionStateField = new Field.Builder()
            .Name(nameof(MissionState))
            .Title("mission_state")
            .Description("Mission state.")
            .DataType(new UInt8Type(AsvSdrMissionStateHelper.GetValues(x=>(byte)x).Min(),AsvSdrMissionStateHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrMissionStateHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrMissionState _missionState;
        public AsvSdrMissionState MissionState { get => _missionState; set => _missionState = value; } 
        /// <summary>
        /// Calibration status.
        /// OriginName: calib_state, Units: , IsExtended: true
        /// </summary>
        public static readonly Field CalibStateField = new Field.Builder()
            .Name(nameof(CalibState))
            .Title("calib_state")
            .Description("Calibration status.")
            .DataType(new UInt8Type(AsvSdrCalibStateHelper.GetValues(x=>(byte)x).Min(),AsvSdrCalibStateHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrCalibStateHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrCalibState _calibState;
        public AsvSdrCalibState CalibState { get => _calibState; set => _calibState = value; } 
        /// <summary>
        /// Number of calibration tables.
        /// OriginName: calib_table_count, Units: , IsExtended: true
        /// </summary>
        public static readonly Field CalibTableCountField = new Field.Builder()
            .Name(nameof(CalibTableCount))
            .Title("calib_table_count")
            .Description("Number of calibration tables.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _calibTableCount;
        public ushort CalibTableCount { get => _calibTableCount; set => _calibTableCount = value; }
        /// <summary>
        /// Estimated reference power in dBm. Entered in MAV_CMD_ASV_SDR_SET_MODE command.
        /// OriginName: ref_power, Units: , IsExtended: true
        /// </summary>
        public static readonly Field RefPowerField = new Field.Builder()
            .Name(nameof(RefPower))
            .Title("ref_power")
            .Description("Estimated reference power in dBm. Entered in MAV_CMD_ASV_SDR_SET_MODE command.")

            .DataType(FloatType.Default)
        .Build();
        private float _refPower;
        public float RefPower { get => _refPower; set => _refPower = value; }
        /// <summary>
        /// Input path signal overflow indicator. Relative value from 0.0 to 1.0.
        /// OriginName: signal_overflow, Units: , IsExtended: true
        /// </summary>
        public static readonly Field SignalOverflowField = new Field.Builder()
            .Name(nameof(SignalOverflow))
            .Title("signal_overflow")
            .Description("Input path signal overflow indicator. Relative value from 0.0 to 1.0.")

            .DataType(FloatType.Default)
        .Build();
        private float _signalOverflow;
        public float SignalOverflow { get => _signalOverflow; set => _signalOverflow = value; }
    }
    /// <summary>
    /// Request list of ASV_SDR_RECORD from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_REQUEST
    /// </summary>
    public class AsvSdrRecordRequestPacket : MavlinkV2Message<AsvSdrRecordRequestPayload>
    {
        public const int MessageId = 13101;
        
        public const byte CrcExtra = 91;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordRequestPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_REQUEST
    /// </summary>
    public class AsvSdrRecordRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            +2 // uint16_t skip
            +2 // uint16_t count
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUShort(ref buffer);
            Skip = BinSerialize.ReadUShort(ref buffer);
            Count = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteUShort(ref buffer,Skip);
            BinSerialize.WriteUShort(ref buffer,Count);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 8 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt16Type.Accept(visitor,SkipField, SkipField.DataType, ref _skip);    
            UInt16Type.Accept(visitor,CountField, CountField.DataType, ref _count);    
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
        /// Specifies the start index of the records to be sent in the response.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SkipField = new Field.Builder()
            .Name(nameof(Skip))
            .Title("skip")
            .Description("Specifies the start index of the records to be sent in the response.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _skip;
        public ushort Skip { get => _skip; set => _skip = value; }
        /// <summary>
        /// Specifies the number of records to be sent in the response after the skip index.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Specifies the number of records to be sent in the response after the skip index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _count;
        public ushort Count { get => _count; set => _count = value; }
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
    /// Response for ASV_SDR_RECORD_REQUEST request. If success, device additional send ASV_SDR_RECORD items from start to stop index.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_RESPONSE
    /// </summary>
    public class AsvSdrRecordResponsePacket : MavlinkV2Message<AsvSdrRecordResponsePayload>
    {
        public const int MessageId = 13102;
        
        public const byte CrcExtra = 13;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordResponsePayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_RESPONSE
    /// </summary>
    public class AsvSdrRecordResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 5; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 5; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            +2 // uint16_t items_count
            + 1 // uint8_t result
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUShort(ref buffer);
            ItemsCount = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteUShort(ref buffer,ItemsCount);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 5 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt16Type.Accept(visitor,ItemsCountField, ItemsCountField.DataType, ref _itemsCount);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ResultField.DataType, ref tmpResult);
            Result = (AsvSdrRequestAck)tmpResult;

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
        /// Number of items ASV_SDR_RECORD for transmition after this request with success result code (depended from request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ItemsCountField = new Field.Builder()
            .Name(nameof(ItemsCount))
            .Title("items_count")
            .Description("Number of items ASV_SDR_RECORD for transmition after this request with success result code (depended from request).")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _itemsCount;
        public ushort ItemsCount { get => _itemsCount; set => _itemsCount = value; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .DataType(new UInt8Type(AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Min(),AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrRequestAckHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrRequestAck _result;
        public AsvSdrRequestAck Result { get => _result; set => _result = value; } 
    }
    /// <summary>
    /// SDR payload record info.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD
    /// </summary>
    public class AsvSdrRecordPacket : MavlinkV2Message<AsvSdrRecordPayload>
    {
        public const int MessageId = 13103;
        
        public const byte CrcExtra = 173;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD";
    }

    /// <summary>
    ///  ASV_SDR_RECORD
    /// </summary>
    public class AsvSdrRecordPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 78; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 78; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t frequency
            +8 // uint64_t created_unix_us
            + 4 // uint32_t data_type
            +4 // uint32_t duration_sec
            +4 // uint32_t data_count
            +4 // uint32_t size
            +2 // uint16_t tag_count
            +RecordGuid.Length // uint8_t[16] record_guid
            +RecordName.Length // char[28] record_name
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Frequency = BinSerialize.ReadULong(ref buffer);
            CreatedUnixUs = BinSerialize.ReadULong(ref buffer);
            DataType = (AsvSdrCustomMode)BinSerialize.ReadUInt(ref buffer);
            DurationSec = BinSerialize.ReadUInt(ref buffer);
            DataCount = BinSerialize.ReadUInt(ref buffer);
            Size = BinSerialize.ReadUInt(ref buffer);
            TagCount = BinSerialize.ReadUShort(ref buffer);
            arraySize = 16;
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = /*ArrayLength*/28 - Math.Max(0,((/*PayloadByteSize*/78 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = RecordName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, RecordName.Length);
                }
            }
            buffer = buffer[arraySize..];
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Frequency);
            BinSerialize.WriteULong(ref buffer,CreatedUnixUs);
            BinSerialize.WriteUInt(ref buffer,(uint)DataType);
            BinSerialize.WriteUInt(ref buffer,DurationSec);
            BinSerialize.WriteUInt(ref buffer,DataCount);
            BinSerialize.WriteUInt(ref buffer,Size);
            BinSerialize.WriteUShort(ref buffer,TagCount);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = RecordName)
                {
                    Encoding.ASCII.GetBytes(charPointer, RecordName.Length, bytePointer, RecordName.Length);
                }
            }
            buffer = buffer.Slice(RecordName.Length);
            
            /* PayloadByteSize = 78 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,FrequencyField, FrequencyField.DataType, ref _frequency);    
            UInt64Type.Accept(visitor,CreatedUnixUsField, CreatedUnixUsField.DataType, ref _createdUnixUs);    
            var tmpDataType = (uint)DataType;
            UInt32Type.Accept(visitor,DataTypeField, DataTypeField.DataType, ref tmpDataType);
            DataType = (AsvSdrCustomMode)tmpDataType;
            UInt32Type.Accept(visitor,DurationSecField, DurationSecField.DataType, ref _durationSec);    
            UInt32Type.Accept(visitor,DataCountField, DataCountField.DataType, ref _dataCount);    
            UInt32Type.Accept(visitor,SizeField, SizeField.DataType, ref _size);    
            UInt16Type.Accept(visitor,TagCountField, TagCountField.DataType, ref _tagCount);    
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    
            ArrayType.Accept(visitor,RecordNameField, RecordNameField.DataType, 28, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref RecordName[index]));

        }

        /// <summary>
        /// Reference frequency in Hz, specified by MAV_CMD_ASV_SDR_SET_MODE command.
        /// OriginName: frequency, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FrequencyField = new Field.Builder()
            .Name(nameof(Frequency))
            .Title("frequency")
            .Description("Reference frequency in Hz, specified by MAV_CMD_ASV_SDR_SET_MODE command.")

            .DataType(UInt64Type.Default)
        .Build();
        private ulong _frequency;
        public ulong Frequency { get => _frequency; set => _frequency = value; }
        /// <summary>
        /// Created timestamp (UNIX epoch time).
        /// OriginName: created_unix_us, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field CreatedUnixUsField = new Field.Builder()
            .Name(nameof(CreatedUnixUs))
            .Title("created_unix_us")
            .Description("Created timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _createdUnixUs;
        public ulong CreatedUnixUs { get => _createdUnixUs; set => _createdUnixUs = value; }
        /// <summary>
        /// Record data type(it is also possible to know the type of data inside the record by cast enum to int).
        /// OriginName: data_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataTypeField = new Field.Builder()
            .Name(nameof(DataType))
            .Title("data_type")
            .Description("Record data type(it is also possible to know the type of data inside the record by cast enum to int).")
            .DataType(new UInt32Type(AsvSdrCustomModeHelper.GetValues(x=>(uint)x).Min(),AsvSdrCustomModeHelper.GetValues(x=>(uint)x).Max()))
            .Enum(AsvSdrCustomModeHelper.GetEnumValues(x=>(uint)x))
            .Build();
        private AsvSdrCustomMode _dataType;
        public AsvSdrCustomMode DataType { get => _dataType; set => _dataType = value; } 
        /// <summary>
        /// Record duration in sec.
        /// OriginName: duration_sec, Units: sec, IsExtended: false
        /// </summary>
        public static readonly Field DurationSecField = new Field.Builder()
            .Name(nameof(DurationSec))
            .Title("duration_sec")
            .Description("Record duration in sec.")
.Units(@"sec")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _durationSec;
        public uint DurationSec { get => _durationSec; set => _durationSec = value; }
        /// <summary>
        /// Data items count.
        /// OriginName: data_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataCountField = new Field.Builder()
            .Name(nameof(DataCount))
            .Title("data_count")
            .Description("Data items count.")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _dataCount;
        public uint DataCount { get => _dataCount; set => _dataCount = value; }
        /// <summary>
        /// Total data size of record with all data items and tags.
        /// OriginName: size, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field SizeField = new Field.Builder()
            .Name(nameof(Size))
            .Title("size")
            .Description("Total data size of record with all data items and tags.")
.Units(@"bytes")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _size;
        public uint Size { get => _size; set => _size = value; }
        /// <summary>
        /// Tag items count.
        /// OriginName: tag_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TagCountField = new Field.Builder()
            .Name(nameof(TagCount))
            .Title("tag_count")
            .Description("Tag items count.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _tagCount;
        public ushort TagCount { get => _tagCount; set => _tagCount = value; }
        /// <summary>
        /// Record GUID. Generated by payload after the start of recording.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID. Generated by payload after the start of recording.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        /// <summary>
        /// Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.
        /// OriginName: record_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordNameField = new Field.Builder()
            .Name(nameof(RecordName))
            .Title("record_name")
            .Description("Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.")

            .DataType(new ArrayType(CharType.Ascii,28))
        .Build();
        public const int RecordNameMaxItemsCount = 28;
        public char[] RecordName { get; } = new char[28];
        [Obsolete("This method is deprecated. Use GetRecordNameMaxItemsCount instead.")]
        public byte GetRecordNameMaxItemsCount() => 28;
    }
    /// <summary>
    /// Request to delete ASV_SDR_RECORD items from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordDeleteRequestPacket : MavlinkV2Message<AsvSdrRecordDeleteRequestPayload>
    {
        public const int MessageId = 13104;
        
        public const byte CrcExtra = 181;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDeleteRequestPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_DELETE_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordDeleteRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +RecordGuid.Length // uint8_t[16] record_guid
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/20 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            /* PayloadByteSize = 20 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    

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
        /// <summary>
        /// Specifies GUID of the record to be deleted.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Specifies GUID of the record to be deleted.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
    }
    /// <summary>
    /// Response for ASV_SDR_RECORD_DELETE_REQUEST.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordDeleteResponsePacket : MavlinkV2Message<AsvSdrRecordDeleteResponsePayload>
    {
        public const int MessageId = 13105;
        
        public const byte CrcExtra = 62;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDeleteResponsePayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_DELETE_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordDeleteResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 19; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 19; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            + 1 // uint8_t result
            +RecordGuid.Length // uint8_t[16] record_guid
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/19 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            /* PayloadByteSize = 19 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ResultField.DataType, ref tmpResult);
            Result = (AsvSdrRequestAck)tmpResult;
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    

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
            .DataType(new UInt8Type(AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Min(),AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrRequestAckHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrRequestAck _result;
        public AsvSdrRequestAck Result { get => _result; set => _result = value; } 
        /// <summary>
        /// Specifies the GUID of the record that was deleted.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Specifies the GUID of the record that was deleted.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
    }
    /// <summary>
    /// Request list of ASV_SDR_RECORD_TAG from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_TAG_REQUEST
    /// </summary>
    public class AsvSdrRecordTagRequestPacket : MavlinkV2Message<AsvSdrRecordTagRequestPayload>
    {
        public const int MessageId = 13110;
        
        public const byte CrcExtra = 53;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordTagRequestPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_TAG_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_REQUEST
    /// </summary>
    public class AsvSdrRecordTagRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 24; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            +2 // uint16_t skip
            +2 // uint16_t count
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +RecordGuid.Length // uint8_t[16] record_guid
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUShort(ref buffer);
            Skip = BinSerialize.ReadUShort(ref buffer);
            Count = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/24 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteUShort(ref buffer,Skip);
            BinSerialize.WriteUShort(ref buffer,Count);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            /* PayloadByteSize = 24 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt16Type.Accept(visitor,SkipField, SkipField.DataType, ref _skip);    
            UInt16Type.Accept(visitor,CountField, CountField.DataType, ref _count);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    

        }

        /// <summary>
        /// Request unique number. This is to allow the response packet to be detected.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Request unique number. This is to allow the response packet to be detected.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _requestId;
        public ushort RequestId { get => _requestId; set => _requestId = value; }
        /// <summary>
        /// Specifies the start index of the tag to be sent in the response.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SkipField = new Field.Builder()
            .Name(nameof(Skip))
            .Title("skip")
            .Description("Specifies the start index of the tag to be sent in the response.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _skip;
        public ushort Skip { get => _skip; set => _skip = value; }
        /// <summary>
        /// Specifies the number of tag to be sent in the response after the skip index.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Specifies the number of tag to be sent in the response after the skip index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _count;
        public ushort Count { get => _count; set => _count = value; }
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
        /// <summary>
        /// Specifies the GUID of the record.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Specifies the GUID of the record.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
    }
    /// <summary>
    /// Response for ASV_SDR_RECORD_TAG_REQUEST. If success, device additional send ASV_SDR_RECORD_TAG items from start to stop index.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_TAG_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagResponsePacket : MavlinkV2Message<AsvSdrRecordTagResponsePayload>
    {
        public const int MessageId = 13111;
        
        public const byte CrcExtra = 187;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordTagResponsePayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_TAG_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 5; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 5; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            +2 // uint16_t items_count
            + 1 // uint8_t result
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUShort(ref buffer);
            ItemsCount = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteUShort(ref buffer,ItemsCount);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 5 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt16Type.Accept(visitor,ItemsCountField, ItemsCountField.DataType, ref _itemsCount);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ResultField.DataType, ref tmpResult);
            Result = (AsvSdrRequestAck)tmpResult;

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
        /// Number of items ASV_SDR_RECORD_TAG for transmition after this request with success result code (depended from request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ItemsCountField = new Field.Builder()
            .Name(nameof(ItemsCount))
            .Title("items_count")
            .Description("Number of items ASV_SDR_RECORD_TAG for transmition after this request with success result code (depended from request).")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _itemsCount;
        public ushort ItemsCount { get => _itemsCount; set => _itemsCount = value; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .DataType(new UInt8Type(AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Min(),AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrRequestAckHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrRequestAck _result;
        public AsvSdrRequestAck Result { get => _result; set => _result = value; } 
    }
    /// <summary>
    /// Request to read info with either tag_index and record_index from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_TAG
    /// </summary>
    public class AsvSdrRecordTagPacket : MavlinkV2Message<AsvSdrRecordTagPayload>
    {
        public const int MessageId = 13112;
        
        public const byte CrcExtra = 220;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordTagPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_TAG";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG
    /// </summary>
    public class AsvSdrRecordTagPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 57; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 57; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +RecordGuid.Length // uint8_t[16] record_guid
            +TagGuid.Length // uint8_t[16] tag_guid
            +TagName.Length // char[16] tag_name
            + 1 // uint8_t tag_type
            +TagValue.Length // uint8_t[8] tag_value
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/57 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 16;
            for(var i=0;i<arraySize;i++)
            {
                TagGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 16;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = TagName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, TagName.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            TagType = (AsvSdrRecordTagType)BinSerialize.ReadByte(ref buffer);
            arraySize = 8;
            for(var i=0;i<arraySize;i++)
            {
                TagValue[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            for(var i=0;i<TagGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)TagGuid[i]);
            }
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = TagName)
                {
                    Encoding.ASCII.GetBytes(charPointer, TagName.Length, bytePointer, TagName.Length);
                }
            }
            buffer = buffer.Slice(TagName.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)TagType);
            for(var i=0;i<TagValue.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)TagValue[i]);
            }
            /* PayloadByteSize = 57 */;
        }

        public void Accept(IVisitor visitor)
        {
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    
            ArrayType.Accept(visitor,TagGuidField, TagGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref TagGuid[index]));    
            ArrayType.Accept(visitor,TagNameField, TagNameField.DataType, 16, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref TagName[index]));
            var tmpTagType = (byte)TagType;
            UInt8Type.Accept(visitor,TagTypeField, TagTypeField.DataType, ref tmpTagType);
            TagType = (AsvSdrRecordTagType)tmpTagType;
            ArrayType.Accept(visitor,TagValueField, TagValueField.DataType, 8,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref TagValue[index]));    

        }

        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// Tag GUID.
        /// OriginName: tag_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TagGuidField = new Field.Builder()
            .Name(nameof(TagGuid))
            .Title("tag_guid")
            .Description("Tag GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int TagGuidMaxItemsCount = 16;
        public byte[] TagGuid { get; } = new byte[16];
        /// <summary>
        /// Tag name, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
        /// OriginName: tag_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TagNameField = new Field.Builder()
            .Name(nameof(TagName))
            .Title("tag_name")
            .Description("Tag name, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string")

            .DataType(new ArrayType(CharType.Ascii,16))
        .Build();
        public const int TagNameMaxItemsCount = 16;
        public char[] TagName { get; } = new char[16];
        /// <summary>
        /// Tag type.
        /// OriginName: tag_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TagTypeField = new Field.Builder()
            .Name(nameof(TagType))
            .Title("tag_type")
            .Description("Tag type.")
            .DataType(new UInt8Type(AsvSdrRecordTagTypeHelper.GetValues(x=>(byte)x).Min(),AsvSdrRecordTagTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrRecordTagTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrRecordTagType _tagType;
        public AsvSdrRecordTagType TagType { get => _tagType; set => _tagType = value; } 
        /// <summary>
        /// Tag value, depends on the type of tag.
        /// OriginName: tag_value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TagValueField = new Field.Builder()
            .Name(nameof(TagValue))
            .Title("tag_value")
            .Description("Tag value, depends on the type of tag.")

            .DataType(new ArrayType(UInt8Type.Default,8))
        .Build();
        public const int TagValueMaxItemsCount = 8;
        public byte[] TagValue { get; } = new byte[8];
    }
    /// <summary>
    /// Request to delete ASV_SDR_RECORD_TAG from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_TAG_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordTagDeleteRequestPacket : MavlinkV2Message<AsvSdrRecordTagDeleteRequestPayload>
    {
        public const int MessageId = 13113;
        
        public const byte CrcExtra = 233;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordTagDeleteRequestPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_TAG_DELETE_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordTagDeleteRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 36; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 36; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +RecordGuid.Length // uint8_t[16] record_guid
            +TagGuid.Length // uint8_t[16] tag_guid
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/36 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 16;
            for(var i=0;i<arraySize;i++)
            {
                TagGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            for(var i=0;i<TagGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)TagGuid[i]);
            }
            /* PayloadByteSize = 36 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    
            ArrayType.Accept(visitor,TagGuidField, TagGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref TagGuid[index]));    

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
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// Tag GUID.
        /// OriginName: tag_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TagGuidField = new Field.Builder()
            .Name(nameof(TagGuid))
            .Title("tag_guid")
            .Description("Tag GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int TagGuidMaxItemsCount = 16;
        public byte[] TagGuid { get; } = new byte[16];
    }
    /// <summary>
    /// Response for ASV_SDR_RECORD_TAG_DELETE_REQUEST.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_TAG_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagDeleteResponsePacket : MavlinkV2Message<AsvSdrRecordTagDeleteResponsePayload>
    {
        public const int MessageId = 13114;
        
        public const byte CrcExtra = 100;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordTagDeleteResponsePayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_TAG_DELETE_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagDeleteResponsePayload : IPayload
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
            +RecordGuid.Length // uint8_t[16] record_guid
            +TagGuid.Length // uint8_t[16] tag_guid
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/35 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 16;
            for(var i=0;i<arraySize;i++)
            {
                TagGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            for(var i=0;i<TagGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)TagGuid[i]);
            }
            /* PayloadByteSize = 35 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ResultField.DataType, ref tmpResult);
            Result = (AsvSdrRequestAck)tmpResult;
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    
            ArrayType.Accept(visitor,TagGuidField, TagGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref TagGuid[index]));    

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
            .DataType(new UInt8Type(AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Min(),AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrRequestAckHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrRequestAck _result;
        public AsvSdrRequestAck Result { get => _result; set => _result = value; } 
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// Tag GUID.
        /// OriginName: tag_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TagGuidField = new Field.Builder()
            .Name(nameof(TagGuid))
            .Title("tag_guid")
            .Description("Tag GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int TagGuidMaxItemsCount = 16;
        public byte[] TagGuid { get; } = new byte[16];
    }
    /// <summary>
    /// Request list of ASV_SDR_RECORD_DATA_* items from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DATA_REQUEST
    /// </summary>
    public class AsvSdrRecordDataRequestPacket : MavlinkV2Message<AsvSdrRecordDataRequestPayload>
    {
        public const int MessageId = 13120;
        
        public const byte CrcExtra = 101;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDataRequestPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_DATA_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_REQUEST
    /// </summary>
    public class AsvSdrRecordDataRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 28; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 28; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t skip
            +4 // uint32_t count
            +2 // uint16_t request_id
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +RecordGuid.Length // uint8_t[16] record_guid
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Skip = BinSerialize.ReadUInt(ref buffer);
            Count = BinSerialize.ReadUInt(ref buffer);
            RequestId = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/28 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,Skip);
            BinSerialize.WriteUInt(ref buffer,Count);
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            /* PayloadByteSize = 28 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,SkipField, SkipField.DataType, ref _skip);    
            UInt32Type.Accept(visitor,CountField, CountField.DataType, ref _count);    
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    

        }

        /// <summary>
        /// Specifies the start index of the tag to be sent in the response.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SkipField = new Field.Builder()
            .Name(nameof(Skip))
            .Title("skip")
            .Description("Specifies the start index of the tag to be sent in the response.")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _skip;
        public uint Skip { get => _skip; set => _skip = value; }
        /// <summary>
        /// Specifies the number of tag to be sent in the response after the skip index.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Specifies the number of tag to be sent in the response after the skip index.")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _count;
        public uint Count { get => _count; set => _count = value; }
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
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
    }
    /// <summary>
    /// Response for ASV_SDR_RECORD_DATA_REQUEST. If success, device additional send ASV_SDR_RECORD_DATA_* items from start to stop index.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DATA_RESPONSE
    /// </summary>
    public class AsvSdrRecordDataResponsePacket : MavlinkV2Message<AsvSdrRecordDataResponsePayload>
    {
        public const int MessageId = 13121;
        
        public const byte CrcExtra = 39;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDataResponsePayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_DATA_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_RESPONSE
    /// </summary>
    public class AsvSdrRecordDataResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 27; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 27; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 4 // uint32_t data_type
            +4 // uint32_t items_count
            +2 // uint16_t request_id
            + 1 // uint8_t result
            +RecordGuid.Length // uint8_t[16] record_guid
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            DataType = (AsvSdrCustomMode)BinSerialize.ReadUInt(ref buffer);
            ItemsCount = BinSerialize.ReadUInt(ref buffer);
            RequestId = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/27 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,(uint)DataType);
            BinSerialize.WriteUInt(ref buffer,ItemsCount);
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            /* PayloadByteSize = 27 */;
        }

        public void Accept(IVisitor visitor)
        {
            var tmpDataType = (uint)DataType;
            UInt32Type.Accept(visitor,DataTypeField, DataTypeField.DataType, ref tmpDataType);
            DataType = (AsvSdrCustomMode)tmpDataType;
            UInt32Type.Accept(visitor,ItemsCountField, ItemsCountField.DataType, ref _itemsCount);    
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ResultField.DataType, ref tmpResult);
            Result = (AsvSdrRequestAck)tmpResult;
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    

        }

        /// <summary>
        /// Record data type(it is also possible to know the type of data inside the record by cast enum to int).
        /// OriginName: data_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataTypeField = new Field.Builder()
            .Name(nameof(DataType))
            .Title("data_type")
            .Description("Record data type(it is also possible to know the type of data inside the record by cast enum to int).")
            .DataType(new UInt32Type(AsvSdrCustomModeHelper.GetValues(x=>(uint)x).Min(),AsvSdrCustomModeHelper.GetValues(x=>(uint)x).Max()))
            .Enum(AsvSdrCustomModeHelper.GetEnumValues(x=>(uint)x))
            .Build();
        private AsvSdrCustomMode _dataType;
        public AsvSdrCustomMode DataType { get => _dataType; set => _dataType = value; } 
        /// <summary>
        /// Number of items ASV_SDR_RECORD_DATA_* for transmition after this request with success result code (depended from request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ItemsCountField = new Field.Builder()
            .Name(nameof(ItemsCount))
            .Title("items_count")
            .Description("Number of items ASV_SDR_RECORD_DATA_* for transmition after this request with success result code (depended from request).")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _itemsCount;
        public uint ItemsCount { get => _itemsCount; set => _itemsCount = value; }
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
            .DataType(new UInt8Type(AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Min(),AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrRequestAckHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrRequestAck _result;
        public AsvSdrRequestAck Result { get => _result; set => _result = value; } 
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
    }
    /// <summary>
    /// Response for ASV_SDR_CALIB_* requests. Result from ASV_SDR_CALIB_TABLE_READ, ASV_SDR_CALIB_TABLE_ROW_READ, ASV_SDR_CALIB_TABLE_UPLOAD_START messages.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_CALIB_ACC
    /// </summary>
    public class AsvSdrCalibAccPacket : MavlinkV2Message<AsvSdrCalibAccPayload>
    {
        public const int MessageId = 13124;
        
        public const byte CrcExtra = 136;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibAccPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_CALIB_ACC";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_ACC
    /// </summary>
    public class AsvSdrCalibAccPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            + 1 // uint8_t result
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 3 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ResultField.DataType, ref tmpResult);
            Result = (AsvSdrRequestAck)tmpResult;

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
            .DataType(new UInt8Type(AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Min(),AsvSdrRequestAckHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrRequestAckHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrRequestAck _result;
        public AsvSdrRequestAck Result { get => _result; set => _result = value; } 
    }
    /// <summary>
    /// Request to read ASV_SDR_CALIB_TABLE from the system/component. If success, device send ASV_SDR_CALIB_TABLE or ASV_SDR_CALIB_ACC, when error occured.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_CALIB_TABLE_READ
    /// </summary>
    public class AsvSdrCalibTableReadPacket : MavlinkV2Message<AsvSdrCalibTableReadPayload>
    {
        public const int MessageId = 13125;
        
        public const byte CrcExtra = 8;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTableReadPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_CALIB_TABLE_READ";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE_READ
    /// </summary>
    public class AsvSdrCalibTableReadPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 6; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t table_index
            +2 // uint16_t request_id
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TableIndex = BinSerialize.ReadUShort(ref buffer);
            RequestId = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,TableIndex);
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 6 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,TableIndexField, TableIndexField.DataType, ref _tableIndex);    
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    

        }

        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _tableIndex;
        public ushort TableIndex { get => _tableIndex; set => _tableIndex = value; }
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
    /// Calibration table info.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_CALIB_TABLE
    /// </summary>
    public class AsvSdrCalibTablePacket : MavlinkV2Message<AsvSdrCalibTablePayload>
    {
        public const int MessageId = 13126;
        
        public const byte CrcExtra = 194;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTablePayload Payload { get; } = new();

        public override string Name => "ASV_SDR_CALIB_TABLE";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE
    /// </summary>
    public class AsvSdrCalibTablePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 40; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 40; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t created_unix_us
            +2 // uint16_t table_index
            +2 // uint16_t row_count
            +TableName.Length // char[28] table_name
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            CreatedUnixUs = BinSerialize.ReadULong(ref buffer);
            TableIndex = BinSerialize.ReadUShort(ref buffer);
            RowCount = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/28 - Math.Max(0,((/*PayloadByteSize*/40 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = TableName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, TableName.Length);
                }
            }
            buffer = buffer[arraySize..];
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,CreatedUnixUs);
            BinSerialize.WriteUShort(ref buffer,TableIndex);
            BinSerialize.WriteUShort(ref buffer,RowCount);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = TableName)
                {
                    Encoding.ASCII.GetBytes(charPointer, TableName.Length, bytePointer, TableName.Length);
                }
            }
            buffer = buffer.Slice(TableName.Length);
            
            /* PayloadByteSize = 40 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,CreatedUnixUsField, CreatedUnixUsField.DataType, ref _createdUnixUs);    
            UInt16Type.Accept(visitor,TableIndexField, TableIndexField.DataType, ref _tableIndex);    
            UInt16Type.Accept(visitor,RowCountField, RowCountField.DataType, ref _rowCount);    
            ArrayType.Accept(visitor,TableNameField, TableNameField.DataType, 28, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref TableName[index]));

        }

        /// <summary>
        /// Updated timestamp (UNIX epoch time).
        /// OriginName: created_unix_us, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field CreatedUnixUsField = new Field.Builder()
            .Name(nameof(CreatedUnixUs))
            .Title("created_unix_us")
            .Description("Updated timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _createdUnixUs;
        public ulong CreatedUnixUs { get => _createdUnixUs; set => _createdUnixUs = value; }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _tableIndex;
        public ushort TableIndex { get => _tableIndex; set => _tableIndex = value; }
        /// <summary>
        /// Specifies the number of ROWs in the table.
        /// OriginName: row_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RowCountField = new Field.Builder()
            .Name(nameof(RowCount))
            .Title("row_count")
            .Description("Specifies the number of ROWs in the table.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _rowCount;
        public ushort RowCount { get => _rowCount; set => _rowCount = value; }
        /// <summary>
        /// Table name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.
        /// OriginName: table_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableNameField = new Field.Builder()
            .Name(nameof(TableName))
            .Title("table_name")
            .Description("Table name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.")

            .DataType(new ArrayType(CharType.Ascii,28))
        .Build();
        public const int TableNameMaxItemsCount = 28;
        public char[] TableName { get; } = new char[28];
        [Obsolete("This method is deprecated. Use GetTableNameMaxItemsCount instead.")]
        public byte GetTableNameMaxItemsCount() => 28;
    }
    /// <summary>
    /// Request to read ASV_SDR_CALIB_TABLE_ROW from the system/component. If success, device send ASV_SDR_CALIB_TABLE_ROW or ASV_SDR_CALIB_ACC, when error occured.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_CALIB_TABLE_ROW_READ
    /// </summary>
    public class AsvSdrCalibTableRowReadPacket : MavlinkV2Message<AsvSdrCalibTableRowReadPayload>
    {
        public const int MessageId = 13127;
        
        public const byte CrcExtra = 2;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTableRowReadPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_CALIB_TABLE_ROW_READ";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE_ROW_READ
    /// </summary>
    public class AsvSdrCalibTableRowReadPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            +2 // uint16_t table_index
            +2 // uint16_t row_index
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUShort(ref buffer);
            TableIndex = BinSerialize.ReadUShort(ref buffer);
            RowIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteUShort(ref buffer,TableIndex);
            BinSerialize.WriteUShort(ref buffer,RowIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 8 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt16Type.Accept(visitor,TableIndexField, TableIndexField.DataType, ref _tableIndex);    
            UInt16Type.Accept(visitor,RowIndexField, RowIndexField.DataType, ref _rowIndex);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    

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
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _tableIndex;
        public ushort TableIndex { get => _tableIndex; set => _tableIndex = value; }
        /// <summary>
        /// ROW index.
        /// OriginName: row_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RowIndexField = new Field.Builder()
            .Name(nameof(RowIndex))
            .Title("row_index")
            .Description("ROW index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _rowIndex;
        public ushort RowIndex { get => _rowIndex; set => _rowIndex = value; }
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
    /// Calibration ROW content.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_CALIB_TABLE_ROW
    /// </summary>
    public class AsvSdrCalibTableRowPacket : MavlinkV2Message<AsvSdrCalibTableRowPayload>
    {
        public const int MessageId = 13128;
        
        public const byte CrcExtra = 179;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTableRowPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_CALIB_TABLE_ROW";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE_ROW
    /// </summary>
    public class AsvSdrCalibTableRowPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 26; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 26; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t ref_freq
            +4 // float ref_power
            +4 // float ref_value
            +4 // float adjustment
            +2 // uint16_t table_index
            +2 // uint16_t row_index
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RefFreq = BinSerialize.ReadULong(ref buffer);
            RefPower = BinSerialize.ReadFloat(ref buffer);
            RefValue = BinSerialize.ReadFloat(ref buffer);
            Adjustment = BinSerialize.ReadFloat(ref buffer);
            TableIndex = BinSerialize.ReadUShort(ref buffer);
            RowIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,RefFreq);
            BinSerialize.WriteFloat(ref buffer,RefPower);
            BinSerialize.WriteFloat(ref buffer,RefValue);
            BinSerialize.WriteFloat(ref buffer,Adjustment);
            BinSerialize.WriteUShort(ref buffer,TableIndex);
            BinSerialize.WriteUShort(ref buffer,RowIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 26 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,RefFreqField, RefFreqField.DataType, ref _refFreq);    
            FloatType.Accept(visitor,RefPowerField, RefPowerField.DataType, ref _refPower);    
            FloatType.Accept(visitor,RefValueField, RefValueField.DataType, ref _refValue);    
            FloatType.Accept(visitor,AdjustmentField, AdjustmentField.DataType, ref _adjustment);    
            UInt16Type.Accept(visitor,TableIndexField, TableIndexField.DataType, ref _tableIndex);    
            UInt16Type.Accept(visitor,RowIndexField, RowIndexField.DataType, ref _rowIndex);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    

        }

        /// <summary>
        /// Reference frequency in Hz.
        /// OriginName: ref_freq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RefFreqField = new Field.Builder()
            .Name(nameof(RefFreq))
            .Title("ref_freq")
            .Description("Reference frequency in Hz.")

            .DataType(UInt64Type.Default)
        .Build();
        private ulong _refFreq;
        public ulong RefFreq { get => _refFreq; set => _refFreq = value; }
        /// <summary>
        /// Reference power in dBm.
        /// OriginName: ref_power, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RefPowerField = new Field.Builder()
            .Name(nameof(RefPower))
            .Title("ref_power")
            .Description("Reference power in dBm.")

            .DataType(FloatType.Default)
        .Build();
        private float _refPower;
        public float RefPower { get => _refPower; set => _refPower = value; }
        /// <summary>
        /// Reference value.
        /// OriginName: ref_value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RefValueField = new Field.Builder()
            .Name(nameof(RefValue))
            .Title("ref_value")
            .Description("Reference value.")

            .DataType(FloatType.Default)
        .Build();
        private float _refValue;
        public float RefValue { get => _refValue; set => _refValue = value; }
        /// <summary>
        /// Adjustment for measured value (ref_value = measured_value + adjustment)
        /// OriginName: adjustment, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AdjustmentField = new Field.Builder()
            .Name(nameof(Adjustment))
            .Title("adjustment")
            .Description("Adjustment for measured value (ref_value = measured_value + adjustment)")

            .DataType(FloatType.Default)
        .Build();
        private float _adjustment;
        public float Adjustment { get => _adjustment; set => _adjustment = value; }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _tableIndex;
        public ushort TableIndex { get => _tableIndex; set => _tableIndex = value; }
        /// <summary>
        /// ROW index.
        /// OriginName: row_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RowIndexField = new Field.Builder()
            .Name(nameof(RowIndex))
            .Title("row_index")
            .Description("ROW index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _rowIndex;
        public ushort RowIndex { get => _rowIndex; set => _rowIndex = value; }
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
    /// Start uploading process. After that payload must send ASV_SDR_CALIB_TABLE_UPLOAD_READ_CALLBACK to client for reading table rows row_count times. Process end by payload with ASV_SDR_CALIB_ACC. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_CALIB_TABLE_UPLOAD_START
    /// </summary>
    public class AsvSdrCalibTableUploadStartPacket : MavlinkV2Message<AsvSdrCalibTableUploadStartPayload>
    {
        public const int MessageId = 13129;
        
        public const byte CrcExtra = 40;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTableUploadStartPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_CALIB_TABLE_UPLOAD_START";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE_UPLOAD_START
    /// </summary>
    public class AsvSdrCalibTableUploadStartPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 16; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t created_unix_us
            +2 // uint16_t table_index
            +2 // uint16_t request_id
            +2 // uint16_t row_count
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            CreatedUnixUs = BinSerialize.ReadULong(ref buffer);
            TableIndex = BinSerialize.ReadUShort(ref buffer);
            RequestId = BinSerialize.ReadUShort(ref buffer);
            RowCount = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,CreatedUnixUs);
            BinSerialize.WriteUShort(ref buffer,TableIndex);
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteUShort(ref buffer,RowCount);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 16 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,CreatedUnixUsField, CreatedUnixUsField.DataType, ref _createdUnixUs);    
            UInt16Type.Accept(visitor,TableIndexField, TableIndexField.DataType, ref _tableIndex);    
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt16Type.Accept(visitor,RowCountField, RowCountField.DataType, ref _rowCount);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    

        }

        /// <summary>
        /// Current timestamp (UNIX epoch time).
        /// OriginName: created_unix_us, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field CreatedUnixUsField = new Field.Builder()
            .Name(nameof(CreatedUnixUs))
            .Title("created_unix_us")
            .Description("Current timestamp (UNIX epoch time).")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _createdUnixUs;
        public ulong CreatedUnixUs { get => _createdUnixUs; set => _createdUnixUs = value; }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _tableIndex;
        public ushort TableIndex { get => _tableIndex; set => _tableIndex = value; }
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
        /// Specifies the number of ROWs in the table.
        /// OriginName: row_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RowCountField = new Field.Builder()
            .Name(nameof(RowCount))
            .Title("row_count")
            .Description("Specifies the number of ROWs in the table.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _rowCount;
        public ushort RowCount { get => _rowCount; set => _rowCount = value; }
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
    /// Read ASV_SDR_CALIB_TABLE_ROW callback from payload server to client. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_CALIB_TABLE_UPLOAD_READ_CALLBACK
    /// </summary>
    public class AsvSdrCalibTableUploadReadCallbackPacket : MavlinkV2Message<AsvSdrCalibTableUploadReadCallbackPayload>
    {
        public const int MessageId = 13130;
        
        public const byte CrcExtra = 156;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTableUploadReadCallbackPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_CALIB_TABLE_UPLOAD_READ_CALLBACK";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE_UPLOAD_READ_CALLBACK
    /// </summary>
    public class AsvSdrCalibTableUploadReadCallbackPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            +2 // uint16_t table_index
            +2 // uint16_t row_index
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUShort(ref buffer);
            TableIndex = BinSerialize.ReadUShort(ref buffer);
            RowIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteUShort(ref buffer,TableIndex);
            BinSerialize.WriteUShort(ref buffer,RowIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 8 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt16Type.Accept(visitor,TableIndexField, TableIndexField.DataType, ref _tableIndex);    
            UInt16Type.Accept(visitor,RowIndexField, RowIndexField.DataType, ref _rowIndex);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    

        }

        /// <summary>
        /// Specifies the unique number of the original request from ASV_SDR_CALIB_TABLE_UPLOAD_START. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request from ASV_SDR_CALIB_TABLE_UPLOAD_START. This allows the response to be matched to the correct request.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _requestId;
        public ushort RequestId { get => _requestId; set => _requestId = value; }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _tableIndex;
        public ushort TableIndex { get => _tableIndex; set => _tableIndex = value; }
        /// <summary>
        /// ROW index.
        /// OriginName: row_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RowIndexField = new Field.Builder()
            .Name(nameof(RowIndex))
            .Title("row_index")
            .Description("ROW index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _rowIndex;
        public ushort RowIndex { get => _rowIndex; set => _rowIndex = value; }
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
    /// Raw signal data for visualization.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_SIGNAL_RAW
    /// </summary>
    public class AsvSdrSignalRawPacket : MavlinkV2Message<AsvSdrSignalRawPayload>
    {
        public const int MessageId = 13134;
        
        public const byte CrcExtra = 27;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrSignalRawPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_SIGNAL_RAW";
    }

    /// <summary>
    ///  ASV_SDR_SIGNAL_RAW
    /// </summary>
    public class AsvSdrSignalRawPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 230; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 230; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            +4 // float min
            +4 // float max
            +2 // uint16_t start
            +2 // uint16_t total
            +SignalName.Length // char[8] signal_name
            + 1 // uint8_t format
            +1 // uint8_t count
            +Data.Length // uint8_t[200] data
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            Min = BinSerialize.ReadFloat(ref buffer);
            Max = BinSerialize.ReadFloat(ref buffer);
            Start = BinSerialize.ReadUShort(ref buffer);
            Total = BinSerialize.ReadUShort(ref buffer);
            arraySize = 8;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = SignalName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, SignalName.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            Format = (AsvSdrSignalFormat)BinSerialize.ReadByte(ref buffer);
            Count = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/200 - Math.Max(0,((/*PayloadByteSize*/230 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteFloat(ref buffer,Min);
            BinSerialize.WriteFloat(ref buffer,Max);
            BinSerialize.WriteUShort(ref buffer,Start);
            BinSerialize.WriteUShort(ref buffer,Total);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = SignalName)
                {
                    Encoding.ASCII.GetBytes(charPointer, SignalName.Length, bytePointer, SignalName.Length);
                }
            }
            buffer = buffer.Slice(SignalName.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)Format);
            BinSerialize.WriteByte(ref buffer,(byte)Count);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            /* PayloadByteSize = 230 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            FloatType.Accept(visitor,MinField, MinField.DataType, ref _min);    
            FloatType.Accept(visitor,MaxField, MaxField.DataType, ref _max);    
            UInt16Type.Accept(visitor,StartField, StartField.DataType, ref _start);    
            UInt16Type.Accept(visitor,TotalField, TotalField.DataType, ref _total);    
            ArrayType.Accept(visitor,SignalNameField, SignalNameField.DataType, 8, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref SignalName[index]));
            var tmpFormat = (byte)Format;
            UInt8Type.Accept(visitor,FormatField, FormatField.DataType, ref tmpFormat);
            Format = (AsvSdrSignalFormat)tmpFormat;
            UInt8Type.Accept(visitor,CountField, CountField.DataType, ref _count);    
            ArrayType.Accept(visitor,DataField, DataField.DataType, 200,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref Data[index]));    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time) for current set of measures.
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time) for current set of measures.")
.Units(@"us")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _timeUnixUsec;
        public ulong TimeUnixUsec { get => _timeUnixUsec; set => _timeUnixUsec = value; }
        /// <summary>
        /// Min value of set.
        /// OriginName: min, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MinField = new Field.Builder()
            .Name(nameof(Min))
            .Title("min")
            .Description("Min value of set.")

            .DataType(FloatType.Default)
        .Build();
        private float _min;
        public float Min { get => _min; set => _min = value; }
        /// <summary>
        /// Max value of set.
        /// OriginName: max, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MaxField = new Field.Builder()
            .Name(nameof(Max))
            .Title("max")
            .Description("Max value of set.")

            .DataType(FloatType.Default)
        .Build();
        private float _max;
        public float Max { get => _max; set => _max = value; }
        /// <summary>
        /// Start index of measure set.
        /// OriginName: start, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StartField = new Field.Builder()
            .Name(nameof(Start))
            .Title("start")
            .Description("Start index of measure set.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _start;
        public ushort Start { get => _start; set => _start = value; }
        /// <summary>
        /// Total points in set.
        /// OriginName: total, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TotalField = new Field.Builder()
            .Name(nameof(Total))
            .Title("total")
            .Description("Total points in set.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _total;
        public ushort Total { get => _total; set => _total = value; }
        /// <summary>
        /// Signal name, terminated by NULL if the length is less than 8 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 8+1 bytes storage if the ID is stored as string
        /// OriginName: signal_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SignalNameField = new Field.Builder()
            .Name(nameof(SignalName))
            .Title("signal_name")
            .Description("Signal name, terminated by NULL if the length is less than 8 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 8+1 bytes storage if the ID is stored as string")

            .DataType(new ArrayType(CharType.Ascii,8))
        .Build();
        public const int SignalNameMaxItemsCount = 8;
        public char[] SignalName { get; } = new char[8];
        /// <summary>
        /// Format of one measure.
        /// OriginName: format, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FormatField = new Field.Builder()
            .Name(nameof(Format))
            .Title("format")
            .Description("Format of one measure.")
            .DataType(new UInt8Type(AsvSdrSignalFormatHelper.GetValues(x=>(byte)x).Min(),AsvSdrSignalFormatHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvSdrSignalFormatHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvSdrSignalFormat _format;
        public AsvSdrSignalFormat Format { get => _format; set => _format = value; } 
        /// <summary>
        /// Measures count in this packet.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Measures count in this packet.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _count;
        public byte Count { get => _count; set => _count = value; }
        /// <summary>
        /// Data set of points.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Data set of points.")

            .DataType(new ArrayType(UInt8Type.Default,200))
        .Build();
        public const int DataMaxItemsCount = 200;
        public byte[] Data { get; } = new byte[200];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 200;
    }
    /// <summary>
    /// LLZ reciever record data.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DATA_LLZ
    /// </summary>
    public class AsvSdrRecordDataLlzPacket : MavlinkV2Message<AsvSdrRecordDataLlzPayload>
    {
        public const int MessageId = 13135;
        
        public const byte CrcExtra = 2;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDataLlzPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_DATA_LLZ";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_LLZ
    /// </summary>
    public class AsvSdrRecordDataLlzPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 186; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 186; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            +8 // uint64_t total_freq
            +4 // uint32_t data_index
            +4 // int32_t gnss_lat
            +4 // int32_t gnss_lon
            +4 // int32_t gnss_alt
            +4 // int32_t gnss_alt_ellipsoid
            +4 // uint32_t gnss_h_acc
            +4 // uint32_t gnss_v_acc
            +4 // uint32_t gnss_vel_acc
            +4 // int32_t lat
            +4 // int32_t lon
            +4 // int32_t alt
            +4 // int32_t relative_alt
            +4 // float roll
            +4 // float pitch
            +4 // float yaw
            +4 // float crs_power
            +4 // float crs_am_90
            +4 // float crs_am_150
            +4 // float clr_power
            +4 // float clr_am_90
            +4 // float clr_am_150
            +4 // float total_power
            +4 // float total_field_strength
            +4 // float total_am_90
            +4 // float total_am_150
            +4 // float phi_90_crs_vs_clr
            +4 // float phi_150_crs_vs_clr
            +4 // float code_id_am_1020
            +2 // uint16_t gnss_eph
            +2 // uint16_t gnss_epv
            +2 // uint16_t gnss_vel
            +2 // int16_t vx
            +2 // int16_t vy
            +2 // int16_t vz
            +2 // uint16_t hdg
            +2 // int16_t crs_carrier_offset
            +2 // int16_t crs_freq_90
            +2 // int16_t crs_freq_150
            +2 // int16_t clr_carrier_offset
            +2 // int16_t clr_freq_90
            +2 // int16_t clr_freq_150
            +2 // int16_t total_carrier_offset
            +2 // int16_t total_freq_90
            +2 // int16_t total_freq_150
            +2 // int16_t code_id_freq_1020
            +2 // int16_t measure_time
            +RecordGuid.Length // uint8_t[16] record_guid
            + 1 // uint8_t gnss_fix_type
            +1 // uint8_t gnss_satellites_visible
            +CodeId.Length // char[4] code_id
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            TotalFreq = BinSerialize.ReadULong(ref buffer);
            DataIndex = BinSerialize.ReadUInt(ref buffer);
            GnssLat = BinSerialize.ReadInt(ref buffer);
            GnssLon = BinSerialize.ReadInt(ref buffer);
            GnssAlt = BinSerialize.ReadInt(ref buffer);
            GnssAltEllipsoid = BinSerialize.ReadInt(ref buffer);
            GnssHAcc = BinSerialize.ReadUInt(ref buffer);
            GnssVAcc = BinSerialize.ReadUInt(ref buffer);
            GnssVelAcc = BinSerialize.ReadUInt(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lon = BinSerialize.ReadInt(ref buffer);
            Alt = BinSerialize.ReadInt(ref buffer);
            RelativeAlt = BinSerialize.ReadInt(ref buffer);
            Roll = BinSerialize.ReadFloat(ref buffer);
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            CrsPower = BinSerialize.ReadFloat(ref buffer);
            CrsAm90 = BinSerialize.ReadFloat(ref buffer);
            CrsAm150 = BinSerialize.ReadFloat(ref buffer);
            ClrPower = BinSerialize.ReadFloat(ref buffer);
            ClrAm90 = BinSerialize.ReadFloat(ref buffer);
            ClrAm150 = BinSerialize.ReadFloat(ref buffer);
            TotalPower = BinSerialize.ReadFloat(ref buffer);
            TotalFieldStrength = BinSerialize.ReadFloat(ref buffer);
            TotalAm90 = BinSerialize.ReadFloat(ref buffer);
            TotalAm150 = BinSerialize.ReadFloat(ref buffer);
            Phi90CrsVsClr = BinSerialize.ReadFloat(ref buffer);
            Phi150CrsVsClr = BinSerialize.ReadFloat(ref buffer);
            CodeIdAm1020 = BinSerialize.ReadFloat(ref buffer);
            GnssEph = BinSerialize.ReadUShort(ref buffer);
            GnssEpv = BinSerialize.ReadUShort(ref buffer);
            GnssVel = BinSerialize.ReadUShort(ref buffer);
            Vx = BinSerialize.ReadShort(ref buffer);
            Vy = BinSerialize.ReadShort(ref buffer);
            Vz = BinSerialize.ReadShort(ref buffer);
            Hdg = BinSerialize.ReadUShort(ref buffer);
            CrsCarrierOffset = BinSerialize.ReadShort(ref buffer);
            CrsFreq90 = BinSerialize.ReadShort(ref buffer);
            CrsFreq150 = BinSerialize.ReadShort(ref buffer);
            ClrCarrierOffset = BinSerialize.ReadShort(ref buffer);
            ClrFreq90 = BinSerialize.ReadShort(ref buffer);
            ClrFreq150 = BinSerialize.ReadShort(ref buffer);
            TotalCarrierOffset = BinSerialize.ReadShort(ref buffer);
            TotalFreq90 = BinSerialize.ReadShort(ref buffer);
            TotalFreq150 = BinSerialize.ReadShort(ref buffer);
            CodeIdFreq1020 = BinSerialize.ReadShort(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/186 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            GnssFixType = (GpsFixType)BinSerialize.ReadByte(ref buffer);
            GnssSatellitesVisible = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = 4;
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
            BinSerialize.WriteULong(ref buffer,TotalFreq);
            BinSerialize.WriteUInt(ref buffer,DataIndex);
            BinSerialize.WriteInt(ref buffer,GnssLat);
            BinSerialize.WriteInt(ref buffer,GnssLon);
            BinSerialize.WriteInt(ref buffer,GnssAlt);
            BinSerialize.WriteInt(ref buffer,GnssAltEllipsoid);
            BinSerialize.WriteUInt(ref buffer,GnssHAcc);
            BinSerialize.WriteUInt(ref buffer,GnssVAcc);
            BinSerialize.WriteUInt(ref buffer,GnssVelAcc);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lon);
            BinSerialize.WriteInt(ref buffer,Alt);
            BinSerialize.WriteInt(ref buffer,RelativeAlt);
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteFloat(ref buffer,CrsPower);
            BinSerialize.WriteFloat(ref buffer,CrsAm90);
            BinSerialize.WriteFloat(ref buffer,CrsAm150);
            BinSerialize.WriteFloat(ref buffer,ClrPower);
            BinSerialize.WriteFloat(ref buffer,ClrAm90);
            BinSerialize.WriteFloat(ref buffer,ClrAm150);
            BinSerialize.WriteFloat(ref buffer,TotalPower);
            BinSerialize.WriteFloat(ref buffer,TotalFieldStrength);
            BinSerialize.WriteFloat(ref buffer,TotalAm90);
            BinSerialize.WriteFloat(ref buffer,TotalAm150);
            BinSerialize.WriteFloat(ref buffer,Phi90CrsVsClr);
            BinSerialize.WriteFloat(ref buffer,Phi150CrsVsClr);
            BinSerialize.WriteFloat(ref buffer,CodeIdAm1020);
            BinSerialize.WriteUShort(ref buffer,GnssEph);
            BinSerialize.WriteUShort(ref buffer,GnssEpv);
            BinSerialize.WriteUShort(ref buffer,GnssVel);
            BinSerialize.WriteShort(ref buffer,Vx);
            BinSerialize.WriteShort(ref buffer,Vy);
            BinSerialize.WriteShort(ref buffer,Vz);
            BinSerialize.WriteUShort(ref buffer,Hdg);
            BinSerialize.WriteShort(ref buffer,CrsCarrierOffset);
            BinSerialize.WriteShort(ref buffer,CrsFreq90);
            BinSerialize.WriteShort(ref buffer,CrsFreq150);
            BinSerialize.WriteShort(ref buffer,ClrCarrierOffset);
            BinSerialize.WriteShort(ref buffer,ClrFreq90);
            BinSerialize.WriteShort(ref buffer,ClrFreq150);
            BinSerialize.WriteShort(ref buffer,TotalCarrierOffset);
            BinSerialize.WriteShort(ref buffer,TotalFreq90);
            BinSerialize.WriteShort(ref buffer,TotalFreq150);
            BinSerialize.WriteShort(ref buffer,CodeIdFreq1020);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)GnssFixType);
            BinSerialize.WriteByte(ref buffer,(byte)GnssSatellitesVisible);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId)
                {
                    Encoding.ASCII.GetBytes(charPointer, CodeId.Length, bytePointer, CodeId.Length);
                }
            }
            buffer = buffer.Slice(CodeId.Length);
            
            /* PayloadByteSize = 186 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            UInt64Type.Accept(visitor,TotalFreqField, TotalFreqField.DataType, ref _totalFreq);    
            UInt32Type.Accept(visitor,DataIndexField, DataIndexField.DataType, ref _dataIndex);    
            Int32Type.Accept(visitor,GnssLatField, GnssLatField.DataType, ref _gnssLat);    
            Int32Type.Accept(visitor,GnssLonField, GnssLonField.DataType, ref _gnssLon);    
            Int32Type.Accept(visitor,GnssAltField, GnssAltField.DataType, ref _gnssAlt);    
            Int32Type.Accept(visitor,GnssAltEllipsoidField, GnssAltEllipsoidField.DataType, ref _gnssAltEllipsoid);    
            UInt32Type.Accept(visitor,GnssHAccField, GnssHAccField.DataType, ref _gnssHAcc);    
            UInt32Type.Accept(visitor,GnssVAccField, GnssVAccField.DataType, ref _gnssVAcc);    
            UInt32Type.Accept(visitor,GnssVelAccField, GnssVelAccField.DataType, ref _gnssVelAcc);    
            Int32Type.Accept(visitor,LatField, LatField.DataType, ref _lat);    
            Int32Type.Accept(visitor,LonField, LonField.DataType, ref _lon);    
            Int32Type.Accept(visitor,AltField, AltField.DataType, ref _alt);    
            Int32Type.Accept(visitor,RelativeAltField, RelativeAltField.DataType, ref _relativeAlt);    
            FloatType.Accept(visitor,RollField, RollField.DataType, ref _roll);    
            FloatType.Accept(visitor,PitchField, PitchField.DataType, ref _pitch);    
            FloatType.Accept(visitor,YawField, YawField.DataType, ref _yaw);    
            FloatType.Accept(visitor,CrsPowerField, CrsPowerField.DataType, ref _crsPower);    
            FloatType.Accept(visitor,CrsAm90Field, CrsAm90Field.DataType, ref _crsAm90);    
            FloatType.Accept(visitor,CrsAm150Field, CrsAm150Field.DataType, ref _crsAm150);    
            FloatType.Accept(visitor,ClrPowerField, ClrPowerField.DataType, ref _clrPower);    
            FloatType.Accept(visitor,ClrAm90Field, ClrAm90Field.DataType, ref _clrAm90);    
            FloatType.Accept(visitor,ClrAm150Field, ClrAm150Field.DataType, ref _clrAm150);    
            FloatType.Accept(visitor,TotalPowerField, TotalPowerField.DataType, ref _totalPower);    
            FloatType.Accept(visitor,TotalFieldStrengthField, TotalFieldStrengthField.DataType, ref _totalFieldStrength);    
            FloatType.Accept(visitor,TotalAm90Field, TotalAm90Field.DataType, ref _totalAm90);    
            FloatType.Accept(visitor,TotalAm150Field, TotalAm150Field.DataType, ref _totalAm150);    
            FloatType.Accept(visitor,Phi90CrsVsClrField, Phi90CrsVsClrField.DataType, ref _phi90CrsVsClr);    
            FloatType.Accept(visitor,Phi150CrsVsClrField, Phi150CrsVsClrField.DataType, ref _phi150CrsVsClr);    
            FloatType.Accept(visitor,CodeIdAm1020Field, CodeIdAm1020Field.DataType, ref _codeIdAm1020);    
            UInt16Type.Accept(visitor,GnssEphField, GnssEphField.DataType, ref _gnssEph);    
            UInt16Type.Accept(visitor,GnssEpvField, GnssEpvField.DataType, ref _gnssEpv);    
            UInt16Type.Accept(visitor,GnssVelField, GnssVelField.DataType, ref _gnssVel);    
            Int16Type.Accept(visitor,VxField, VxField.DataType, ref _vx);
            Int16Type.Accept(visitor,VyField, VyField.DataType, ref _vy);
            Int16Type.Accept(visitor,VzField, VzField.DataType, ref _vz);
            UInt16Type.Accept(visitor,HdgField, HdgField.DataType, ref _hdg);    
            Int16Type.Accept(visitor,CrsCarrierOffsetField, CrsCarrierOffsetField.DataType, ref _crsCarrierOffset);
            Int16Type.Accept(visitor,CrsFreq90Field, CrsFreq90Field.DataType, ref _crsFreq90);
            Int16Type.Accept(visitor,CrsFreq150Field, CrsFreq150Field.DataType, ref _crsFreq150);
            Int16Type.Accept(visitor,ClrCarrierOffsetField, ClrCarrierOffsetField.DataType, ref _clrCarrierOffset);
            Int16Type.Accept(visitor,ClrFreq90Field, ClrFreq90Field.DataType, ref _clrFreq90);
            Int16Type.Accept(visitor,ClrFreq150Field, ClrFreq150Field.DataType, ref _clrFreq150);
            Int16Type.Accept(visitor,TotalCarrierOffsetField, TotalCarrierOffsetField.DataType, ref _totalCarrierOffset);
            Int16Type.Accept(visitor,TotalFreq90Field, TotalFreq90Field.DataType, ref _totalFreq90);
            Int16Type.Accept(visitor,TotalFreq150Field, TotalFreq150Field.DataType, ref _totalFreq150);
            Int16Type.Accept(visitor,CodeIdFreq1020Field, CodeIdFreq1020Field.DataType, ref _codeIdFreq1020);
            Int16Type.Accept(visitor,MeasureTimeField, MeasureTimeField.DataType, ref _measureTime);
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    
            var tmpGnssFixType = (byte)GnssFixType;
            UInt8Type.Accept(visitor,GnssFixTypeField, GnssFixTypeField.DataType, ref tmpGnssFixType);
            GnssFixType = (GpsFixType)tmpGnssFixType;
            UInt8Type.Accept(visitor,GnssSatellitesVisibleField, GnssSatellitesVisibleField.DataType, ref _gnssSatellitesVisible);    
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
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreqField = new Field.Builder()
            .Name(nameof(TotalFreq))
            .Title("total_freq")
            .Description("Measured frequency.")
.Units(@"Hz")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _totalFreq;
        public ulong TotalFreq { get => _totalFreq; set => _totalFreq = value; }
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
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLatField = new Field.Builder()
            .Name(nameof(GnssLat))
            .Title("gnss_lat")
            .Description("Latitude (WGS84, EGM96 ellipsoid)")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssLat;
        public int GnssLat { get => _gnssLat; set => _gnssLat = value; }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLonField = new Field.Builder()
            .Name(nameof(GnssLon))
            .Title("gnss_lon")
            .Description("Longitude (WGS84, EGM96 ellipsoid)")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssLon;
        public int GnssLon { get => _gnssLon; set => _gnssLon = value; }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltField = new Field.Builder()
            .Name(nameof(GnssAlt))
            .Title("gnss_alt")
            .Description("Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssAlt;
        public int GnssAlt { get => _gnssAlt; set => _gnssAlt = value; }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltEllipsoidField = new Field.Builder()
            .Name(nameof(GnssAltEllipsoid))
            .Title("gnss_alt_ellipsoid")
            .Description("Altitude (above WGS84, EGM96 ellipsoid). Positive for up.")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssAltEllipsoid;
        public int GnssAltEllipsoid { get => _gnssAltEllipsoid; set => _gnssAltEllipsoid = value; }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssHAccField = new Field.Builder()
            .Name(nameof(GnssHAcc))
            .Title("gnss_h_acc")
            .Description("Position uncertainty. Positive for up.")
.Units(@"mm")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _gnssHAcc;
        public uint GnssHAcc { get => _gnssHAcc; set => _gnssHAcc = value; }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVAccField = new Field.Builder()
            .Name(nameof(GnssVAcc))
            .Title("gnss_v_acc")
            .Description("Altitude uncertainty. Positive for up.")
.Units(@"mm")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _gnssVAcc;
        public uint GnssVAcc { get => _gnssVAcc; set => _gnssVAcc = value; }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelAccField = new Field.Builder()
            .Name(nameof(GnssVelAcc))
            .Title("gnss_vel_acc")
            .Description("Speed uncertainty. Positive for up.")
.Units(@"mm")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _gnssVelAcc;
        public uint GnssVelAcc { get => _gnssVelAcc; set => _gnssVelAcc = value; }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Filtered global position latitude, expressed")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _lat;
        public int Lat { get => _lat; set => _lat = value; }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LonField = new Field.Builder()
            .Name(nameof(Lon))
            .Title("lon")
            .Description("Filtered global position longitude, expressed")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _lon;
        public int Lon { get => _lon; set => _lon = value; }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltField = new Field.Builder()
            .Name(nameof(Alt))
            .Title("alt")
            .Description("Filtered global position altitude (MSL).")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _alt;
        public int Alt { get => _alt; set => _alt = value; }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field RelativeAltField = new Field.Builder()
            .Name(nameof(RelativeAlt))
            .Title("relative_alt")
            .Description("Altitude above ground")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _relativeAlt;
        public int RelativeAlt { get => _relativeAlt; set => _relativeAlt = value; }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle (-pi..+pi)")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _roll;
        public float Roll { get => _roll; set => _roll = value; }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch angle (-pi..+pi)")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _pitch;
        public float Pitch { get => _pitch; set => _pitch = value; }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw angle (-pi..+pi)")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _yaw;
        public float Yaw { get => _yaw; set => _yaw = value; }
        /// <summary>
        /// Input power of course.
        /// OriginName: crs_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field CrsPowerField = new Field.Builder()
            .Name(nameof(CrsPower))
            .Title("crs_power")
            .Description("Input power of course.")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _crsPower;
        public float CrsPower { get => _crsPower; set => _crsPower = value; }
        /// <summary>
        /// Aplitude modulation of 90Hz of course.
        /// OriginName: crs_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CrsAm90Field = new Field.Builder()
            .Name(nameof(CrsAm90))
            .Title("crs_am_90")
            .Description("Aplitude modulation of 90Hz of course.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _crsAm90;
        public float CrsAm90 { get => _crsAm90; set => _crsAm90 = value; }
        /// <summary>
        /// Aplitude modulation of 150Hz of course.
        /// OriginName: crs_am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CrsAm150Field = new Field.Builder()
            .Name(nameof(CrsAm150))
            .Title("crs_am_150")
            .Description("Aplitude modulation of 150Hz of course.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _crsAm150;
        public float CrsAm150 { get => _crsAm150; set => _crsAm150 = value; }
        /// <summary>
        /// Input power of clearance.
        /// OriginName: clr_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field ClrPowerField = new Field.Builder()
            .Name(nameof(ClrPower))
            .Title("clr_power")
            .Description("Input power of clearance.")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _clrPower;
        public float ClrPower { get => _clrPower; set => _clrPower = value; }
        /// <summary>
        /// Aplitude modulation of 90Hz of clearance.
        /// OriginName: clr_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field ClrAm90Field = new Field.Builder()
            .Name(nameof(ClrAm90))
            .Title("clr_am_90")
            .Description("Aplitude modulation of 90Hz of clearance.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _clrAm90;
        public float ClrAm90 { get => _clrAm90; set => _clrAm90 = value; }
        /// <summary>
        /// Aplitude modulation of 150Hz of clearance.
        /// OriginName: clr_am_150, Units: % E2, IsExtended: false
        /// </summary>
        public static readonly Field ClrAm150Field = new Field.Builder()
            .Name(nameof(ClrAm150))
            .Title("clr_am_150")
            .Description("Aplitude modulation of 150Hz of clearance.")
.Units(@"% E2")
            .DataType(FloatType.Default)
        .Build();
        private float _clrAm150;
        public float ClrAm150 { get => _clrAm150; set => _clrAm150 = value; }
        /// <summary>
        /// Total input power.
        /// OriginName: total_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field TotalPowerField = new Field.Builder()
            .Name(nameof(TotalPower))
            .Title("total_power")
            .Description("Total input power.")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _totalPower;
        public float TotalPower { get => _totalPower; set => _totalPower = value; }
        /// <summary>
        /// Total field strength.
        /// OriginName: total_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field TotalFieldStrengthField = new Field.Builder()
            .Name(nameof(TotalFieldStrength))
            .Title("total_field_strength")
            .Description("Total field strength.")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _totalFieldStrength;
        public float TotalFieldStrength { get => _totalFieldStrength; set => _totalFieldStrength = value; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: total_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TotalAm90Field = new Field.Builder()
            .Name(nameof(TotalAm90))
            .Title("total_am_90")
            .Description("Total aplitude modulation of 90Hz.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _totalAm90;
        public float TotalAm90 { get => _totalAm90; set => _totalAm90 = value; }
        /// <summary>
        /// Total aplitude modulation of 150Hz.
        /// OriginName: total_am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TotalAm150Field = new Field.Builder()
            .Name(nameof(TotalAm150))
            .Title("total_am_150")
            .Description("Total aplitude modulation of 150Hz.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _totalAm150;
        public float TotalAm150 { get => _totalAm150; set => _totalAm150 = value; }
        /// <summary>
        ///  Phase difference 90 Hz clearance and cource
        /// OriginName: phi_90_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi90CrsVsClrField = new Field.Builder()
            .Name(nameof(Phi90CrsVsClr))
            .Title("phi_90_crs_vs_clr")
            .Description(" Phase difference 90 Hz clearance and cource")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _phi90CrsVsClr;
        public float Phi90CrsVsClr { get => _phi90CrsVsClr; set => _phi90CrsVsClr = value; }
        /// <summary>
        /// Phase difference 150 Hz clearance and cource.
        /// OriginName: phi_150_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi150CrsVsClrField = new Field.Builder()
            .Name(nameof(Phi150CrsVsClr))
            .Title("phi_150_crs_vs_clr")
            .Description("Phase difference 150 Hz clearance and cource.")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _phi150CrsVsClr;
        public float Phi150CrsVsClr { get => _phi150CrsVsClr; set => _phi150CrsVsClr = value; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: code_id_am_1020, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdAm1020Field = new Field.Builder()
            .Name(nameof(CodeIdAm1020))
            .Title("code_id_am_1020")
            .Description("Total aplitude modulation of 90Hz.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _codeIdAm1020;
        public float CodeIdAm1020 { get => _codeIdAm1020; set => _codeIdAm1020 = value; }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEphField = new Field.Builder()
            .Name(nameof(GnssEph))
            .Title("gnss_eph")
            .Description("GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _gnssEph;
        public ushort GnssEph { get => _gnssEph; set => _gnssEph = value; }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEpvField = new Field.Builder()
            .Name(nameof(GnssEpv))
            .Title("gnss_epv")
            .Description("GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _gnssEpv;
        public ushort GnssEpv { get => _gnssEpv; set => _gnssEpv = value; }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelField = new Field.Builder()
            .Name(nameof(GnssVel))
            .Title("gnss_vel")
            .Description("GPS ground speed. If unknown, set to: UINT16_MAX")
.Units(@"cm/s")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _gnssVel;
        public ushort GnssVel { get => _gnssVel; set => _gnssVel = value; }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VxField = new Field.Builder()
            .Name(nameof(Vx))
            .Title("vx")
            .Description("Ground X Speed (Latitude, positive north)")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _vx;
        public short Vx { get => _vx; set => _vx = value; }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VyField = new Field.Builder()
            .Name(nameof(Vy))
            .Title("vy")
            .Description("Ground Y Speed (Longitude, positive east)")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _vy;
        public short Vy { get => _vy; set => _vy = value; }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VzField = new Field.Builder()
            .Name(nameof(Vz))
            .Title("vz")
            .Description("Ground Z Speed (Altitude, positive down)")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _vz;
        public short Vz { get => _vz; set => _vz = value; }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field HdgField = new Field.Builder()
            .Name(nameof(Hdg))
            .Title("hdg")
            .Description("Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX")
.Units(@"cdeg")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _hdg;
        public ushort Hdg { get => _hdg; set => _hdg = value; }
        /// <summary>
        /// Carrier frequency offset of course.
        /// OriginName: crs_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsCarrierOffsetField = new Field.Builder()
            .Name(nameof(CrsCarrierOffset))
            .Title("crs_carrier_offset")
            .Description("Carrier frequency offset of course.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _crsCarrierOffset;
        public short CrsCarrierOffset { get => _crsCarrierOffset; set => _crsCarrierOffset = value; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of course.
        /// OriginName: crs_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsFreq90Field = new Field.Builder()
            .Name(nameof(CrsFreq90))
            .Title("crs_freq_90")
            .Description("Frequency offset of signal 90 Hz of course.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _crsFreq90;
        public short CrsFreq90 { get => _crsFreq90; set => _crsFreq90 = value; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of course.
        /// OriginName: crs_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsFreq150Field = new Field.Builder()
            .Name(nameof(CrsFreq150))
            .Title("crs_freq_150")
            .Description("Frequency offset of signal 150 Hz of course.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _crsFreq150;
        public short CrsFreq150 { get => _crsFreq150; set => _crsFreq150 = value; }
        /// <summary>
        /// Carrier frequency offset of clearance.
        /// OriginName: clr_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrCarrierOffsetField = new Field.Builder()
            .Name(nameof(ClrCarrierOffset))
            .Title("clr_carrier_offset")
            .Description("Carrier frequency offset of clearance.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _clrCarrierOffset;
        public short ClrCarrierOffset { get => _clrCarrierOffset; set => _clrCarrierOffset = value; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of clearance.
        /// OriginName: clr_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrFreq90Field = new Field.Builder()
            .Name(nameof(ClrFreq90))
            .Title("clr_freq_90")
            .Description("Frequency offset of signal 90 Hz of clearance.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _clrFreq90;
        public short ClrFreq90 { get => _clrFreq90; set => _clrFreq90 = value; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of clearance.
        /// OriginName: clr_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrFreq150Field = new Field.Builder()
            .Name(nameof(ClrFreq150))
            .Title("clr_freq_150")
            .Description("Frequency offset of signal 150 Hz of clearance.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _clrFreq150;
        public short ClrFreq150 { get => _clrFreq150; set => _clrFreq150 = value; }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: total_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalCarrierOffsetField = new Field.Builder()
            .Name(nameof(TotalCarrierOffset))
            .Title("total_carrier_offset")
            .Description("Total carrier frequency offset.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _totalCarrierOffset;
        public short TotalCarrierOffset { get => _totalCarrierOffset; set => _totalCarrierOffset = value; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: total_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreq90Field = new Field.Builder()
            .Name(nameof(TotalFreq90))
            .Title("total_freq_90")
            .Description("Total frequency offset of signal 90 Hz.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _totalFreq90;
        public short TotalFreq90 { get => _totalFreq90; set => _totalFreq90 = value; }
        /// <summary>
        /// Total frequency offset of signal 150 Hz.
        /// OriginName: total_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreq150Field = new Field.Builder()
            .Name(nameof(TotalFreq150))
            .Title("total_freq_150")
            .Description("Total frequency offset of signal 150 Hz.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _totalFreq150;
        public short TotalFreq150 { get => _totalFreq150; set => _totalFreq150 = value; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: code_id_freq_1020, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdFreq1020Field = new Field.Builder()
            .Name(nameof(CodeIdFreq1020))
            .Title("code_id_freq_1020")
            .Description("Total frequency offset of signal 90 Hz.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _codeIdFreq1020;
        public short CodeIdFreq1020 { get => _codeIdFreq1020; set => _codeIdFreq1020 = value; }
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
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// GPS fix type.
        /// OriginName: gnss_fix_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssFixTypeField = new Field.Builder()
            .Name(nameof(GnssFixType))
            .Title("gnss_fix_type")
            .Description("GPS fix type.")
            .DataType(new UInt8Type(GpsFixTypeHelper.GetValues(x=>(byte)x).Min(),GpsFixTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(GpsFixTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private GpsFixType _gnssFixType;
        public GpsFixType GnssFixType { get => _gnssFixType; set => _gnssFixType = value; } 
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssSatellitesVisibleField = new Field.Builder()
            .Name(nameof(GnssSatellitesVisible))
            .Title("gnss_satellites_visible")
            .Description("Number of satellites visible. If unknown, set to 255")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _gnssSatellitesVisible;
        public byte GnssSatellitesVisible { get => _gnssSatellitesVisible; set => _gnssSatellitesVisible = value; }
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
    }
    /// <summary>
    /// GP reciever record data.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DATA_GP
    /// </summary>
    public class AsvSdrRecordDataGpPacket : MavlinkV2Message<AsvSdrRecordDataGpPayload>
    {
        public const int MessageId = 13136;
        
        public const byte CrcExtra = 233;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDataGpPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_DATA_GP";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_GP
    /// </summary>
    public class AsvSdrRecordDataGpPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 176; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 176; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            +8 // uint64_t total_freq
            +4 // uint32_t data_index
            +4 // int32_t gnss_lat
            +4 // int32_t gnss_lon
            +4 // int32_t gnss_alt
            +4 // int32_t gnss_alt_ellipsoid
            +4 // uint32_t gnss_h_acc
            +4 // uint32_t gnss_v_acc
            +4 // uint32_t gnss_vel_acc
            +4 // int32_t lat
            +4 // int32_t lon
            +4 // int32_t alt
            +4 // int32_t relative_alt
            +4 // float roll
            +4 // float pitch
            +4 // float yaw
            +4 // float crs_power
            +4 // float crs_am_90
            +4 // float crs_am_150
            +4 // float clr_power
            +4 // float clr_am_90
            +4 // float clr_am_150
            +4 // float total_power
            +4 // float total_field_strength
            +4 // float total_am_90
            +4 // float total_am_150
            +4 // float phi_90_crs_vs_clr
            +4 // float phi_150_crs_vs_clr
            +2 // uint16_t gnss_eph
            +2 // uint16_t gnss_epv
            +2 // uint16_t gnss_vel
            +2 // int16_t vx
            +2 // int16_t vy
            +2 // int16_t vz
            +2 // uint16_t hdg
            +2 // int16_t crs_carrier_offset
            +2 // int16_t crs_freq_90
            +2 // int16_t crs_freq_150
            +2 // int16_t clr_carrier_offset
            +2 // int16_t clr_freq_90
            +2 // int16_t clr_freq_150
            +2 // int16_t total_carrier_offset
            +2 // int16_t total_freq_90
            +2 // int16_t total_freq_150
            +2 // int16_t measure_time
            +RecordGuid.Length // uint8_t[16] record_guid
            + 1 // uint8_t gnss_fix_type
            +1 // uint8_t gnss_satellites_visible
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            TotalFreq = BinSerialize.ReadULong(ref buffer);
            DataIndex = BinSerialize.ReadUInt(ref buffer);
            GnssLat = BinSerialize.ReadInt(ref buffer);
            GnssLon = BinSerialize.ReadInt(ref buffer);
            GnssAlt = BinSerialize.ReadInt(ref buffer);
            GnssAltEllipsoid = BinSerialize.ReadInt(ref buffer);
            GnssHAcc = BinSerialize.ReadUInt(ref buffer);
            GnssVAcc = BinSerialize.ReadUInt(ref buffer);
            GnssVelAcc = BinSerialize.ReadUInt(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lon = BinSerialize.ReadInt(ref buffer);
            Alt = BinSerialize.ReadInt(ref buffer);
            RelativeAlt = BinSerialize.ReadInt(ref buffer);
            Roll = BinSerialize.ReadFloat(ref buffer);
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            CrsPower = BinSerialize.ReadFloat(ref buffer);
            CrsAm90 = BinSerialize.ReadFloat(ref buffer);
            CrsAm150 = BinSerialize.ReadFloat(ref buffer);
            ClrPower = BinSerialize.ReadFloat(ref buffer);
            ClrAm90 = BinSerialize.ReadFloat(ref buffer);
            ClrAm150 = BinSerialize.ReadFloat(ref buffer);
            TotalPower = BinSerialize.ReadFloat(ref buffer);
            TotalFieldStrength = BinSerialize.ReadFloat(ref buffer);
            TotalAm90 = BinSerialize.ReadFloat(ref buffer);
            TotalAm150 = BinSerialize.ReadFloat(ref buffer);
            Phi90CrsVsClr = BinSerialize.ReadFloat(ref buffer);
            Phi150CrsVsClr = BinSerialize.ReadFloat(ref buffer);
            GnssEph = BinSerialize.ReadUShort(ref buffer);
            GnssEpv = BinSerialize.ReadUShort(ref buffer);
            GnssVel = BinSerialize.ReadUShort(ref buffer);
            Vx = BinSerialize.ReadShort(ref buffer);
            Vy = BinSerialize.ReadShort(ref buffer);
            Vz = BinSerialize.ReadShort(ref buffer);
            Hdg = BinSerialize.ReadUShort(ref buffer);
            CrsCarrierOffset = BinSerialize.ReadShort(ref buffer);
            CrsFreq90 = BinSerialize.ReadShort(ref buffer);
            CrsFreq150 = BinSerialize.ReadShort(ref buffer);
            ClrCarrierOffset = BinSerialize.ReadShort(ref buffer);
            ClrFreq90 = BinSerialize.ReadShort(ref buffer);
            ClrFreq150 = BinSerialize.ReadShort(ref buffer);
            TotalCarrierOffset = BinSerialize.ReadShort(ref buffer);
            TotalFreq90 = BinSerialize.ReadShort(ref buffer);
            TotalFreq150 = BinSerialize.ReadShort(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/176 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            GnssFixType = (GpsFixType)BinSerialize.ReadByte(ref buffer);
            GnssSatellitesVisible = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteULong(ref buffer,TotalFreq);
            BinSerialize.WriteUInt(ref buffer,DataIndex);
            BinSerialize.WriteInt(ref buffer,GnssLat);
            BinSerialize.WriteInt(ref buffer,GnssLon);
            BinSerialize.WriteInt(ref buffer,GnssAlt);
            BinSerialize.WriteInt(ref buffer,GnssAltEllipsoid);
            BinSerialize.WriteUInt(ref buffer,GnssHAcc);
            BinSerialize.WriteUInt(ref buffer,GnssVAcc);
            BinSerialize.WriteUInt(ref buffer,GnssVelAcc);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lon);
            BinSerialize.WriteInt(ref buffer,Alt);
            BinSerialize.WriteInt(ref buffer,RelativeAlt);
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteFloat(ref buffer,CrsPower);
            BinSerialize.WriteFloat(ref buffer,CrsAm90);
            BinSerialize.WriteFloat(ref buffer,CrsAm150);
            BinSerialize.WriteFloat(ref buffer,ClrPower);
            BinSerialize.WriteFloat(ref buffer,ClrAm90);
            BinSerialize.WriteFloat(ref buffer,ClrAm150);
            BinSerialize.WriteFloat(ref buffer,TotalPower);
            BinSerialize.WriteFloat(ref buffer,TotalFieldStrength);
            BinSerialize.WriteFloat(ref buffer,TotalAm90);
            BinSerialize.WriteFloat(ref buffer,TotalAm150);
            BinSerialize.WriteFloat(ref buffer,Phi90CrsVsClr);
            BinSerialize.WriteFloat(ref buffer,Phi150CrsVsClr);
            BinSerialize.WriteUShort(ref buffer,GnssEph);
            BinSerialize.WriteUShort(ref buffer,GnssEpv);
            BinSerialize.WriteUShort(ref buffer,GnssVel);
            BinSerialize.WriteShort(ref buffer,Vx);
            BinSerialize.WriteShort(ref buffer,Vy);
            BinSerialize.WriteShort(ref buffer,Vz);
            BinSerialize.WriteUShort(ref buffer,Hdg);
            BinSerialize.WriteShort(ref buffer,CrsCarrierOffset);
            BinSerialize.WriteShort(ref buffer,CrsFreq90);
            BinSerialize.WriteShort(ref buffer,CrsFreq150);
            BinSerialize.WriteShort(ref buffer,ClrCarrierOffset);
            BinSerialize.WriteShort(ref buffer,ClrFreq90);
            BinSerialize.WriteShort(ref buffer,ClrFreq150);
            BinSerialize.WriteShort(ref buffer,TotalCarrierOffset);
            BinSerialize.WriteShort(ref buffer,TotalFreq90);
            BinSerialize.WriteShort(ref buffer,TotalFreq150);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)GnssFixType);
            BinSerialize.WriteByte(ref buffer,(byte)GnssSatellitesVisible);
            /* PayloadByteSize = 176 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            UInt64Type.Accept(visitor,TotalFreqField, TotalFreqField.DataType, ref _totalFreq);    
            UInt32Type.Accept(visitor,DataIndexField, DataIndexField.DataType, ref _dataIndex);    
            Int32Type.Accept(visitor,GnssLatField, GnssLatField.DataType, ref _gnssLat);    
            Int32Type.Accept(visitor,GnssLonField, GnssLonField.DataType, ref _gnssLon);    
            Int32Type.Accept(visitor,GnssAltField, GnssAltField.DataType, ref _gnssAlt);    
            Int32Type.Accept(visitor,GnssAltEllipsoidField, GnssAltEllipsoidField.DataType, ref _gnssAltEllipsoid);    
            UInt32Type.Accept(visitor,GnssHAccField, GnssHAccField.DataType, ref _gnssHAcc);    
            UInt32Type.Accept(visitor,GnssVAccField, GnssVAccField.DataType, ref _gnssVAcc);    
            UInt32Type.Accept(visitor,GnssVelAccField, GnssVelAccField.DataType, ref _gnssVelAcc);    
            Int32Type.Accept(visitor,LatField, LatField.DataType, ref _lat);    
            Int32Type.Accept(visitor,LonField, LonField.DataType, ref _lon);    
            Int32Type.Accept(visitor,AltField, AltField.DataType, ref _alt);    
            Int32Type.Accept(visitor,RelativeAltField, RelativeAltField.DataType, ref _relativeAlt);    
            FloatType.Accept(visitor,RollField, RollField.DataType, ref _roll);    
            FloatType.Accept(visitor,PitchField, PitchField.DataType, ref _pitch);    
            FloatType.Accept(visitor,YawField, YawField.DataType, ref _yaw);    
            FloatType.Accept(visitor,CrsPowerField, CrsPowerField.DataType, ref _crsPower);    
            FloatType.Accept(visitor,CrsAm90Field, CrsAm90Field.DataType, ref _crsAm90);    
            FloatType.Accept(visitor,CrsAm150Field, CrsAm150Field.DataType, ref _crsAm150);    
            FloatType.Accept(visitor,ClrPowerField, ClrPowerField.DataType, ref _clrPower);    
            FloatType.Accept(visitor,ClrAm90Field, ClrAm90Field.DataType, ref _clrAm90);    
            FloatType.Accept(visitor,ClrAm150Field, ClrAm150Field.DataType, ref _clrAm150);    
            FloatType.Accept(visitor,TotalPowerField, TotalPowerField.DataType, ref _totalPower);    
            FloatType.Accept(visitor,TotalFieldStrengthField, TotalFieldStrengthField.DataType, ref _totalFieldStrength);    
            FloatType.Accept(visitor,TotalAm90Field, TotalAm90Field.DataType, ref _totalAm90);    
            FloatType.Accept(visitor,TotalAm150Field, TotalAm150Field.DataType, ref _totalAm150);    
            FloatType.Accept(visitor,Phi90CrsVsClrField, Phi90CrsVsClrField.DataType, ref _phi90CrsVsClr);    
            FloatType.Accept(visitor,Phi150CrsVsClrField, Phi150CrsVsClrField.DataType, ref _phi150CrsVsClr);    
            UInt16Type.Accept(visitor,GnssEphField, GnssEphField.DataType, ref _gnssEph);    
            UInt16Type.Accept(visitor,GnssEpvField, GnssEpvField.DataType, ref _gnssEpv);    
            UInt16Type.Accept(visitor,GnssVelField, GnssVelField.DataType, ref _gnssVel);    
            Int16Type.Accept(visitor,VxField, VxField.DataType, ref _vx);
            Int16Type.Accept(visitor,VyField, VyField.DataType, ref _vy);
            Int16Type.Accept(visitor,VzField, VzField.DataType, ref _vz);
            UInt16Type.Accept(visitor,HdgField, HdgField.DataType, ref _hdg);    
            Int16Type.Accept(visitor,CrsCarrierOffsetField, CrsCarrierOffsetField.DataType, ref _crsCarrierOffset);
            Int16Type.Accept(visitor,CrsFreq90Field, CrsFreq90Field.DataType, ref _crsFreq90);
            Int16Type.Accept(visitor,CrsFreq150Field, CrsFreq150Field.DataType, ref _crsFreq150);
            Int16Type.Accept(visitor,ClrCarrierOffsetField, ClrCarrierOffsetField.DataType, ref _clrCarrierOffset);
            Int16Type.Accept(visitor,ClrFreq90Field, ClrFreq90Field.DataType, ref _clrFreq90);
            Int16Type.Accept(visitor,ClrFreq150Field, ClrFreq150Field.DataType, ref _clrFreq150);
            Int16Type.Accept(visitor,TotalCarrierOffsetField, TotalCarrierOffsetField.DataType, ref _totalCarrierOffset);
            Int16Type.Accept(visitor,TotalFreq90Field, TotalFreq90Field.DataType, ref _totalFreq90);
            Int16Type.Accept(visitor,TotalFreq150Field, TotalFreq150Field.DataType, ref _totalFreq150);
            Int16Type.Accept(visitor,MeasureTimeField, MeasureTimeField.DataType, ref _measureTime);
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    
            var tmpGnssFixType = (byte)GnssFixType;
            UInt8Type.Accept(visitor,GnssFixTypeField, GnssFixTypeField.DataType, ref tmpGnssFixType);
            GnssFixType = (GpsFixType)tmpGnssFixType;
            UInt8Type.Accept(visitor,GnssSatellitesVisibleField, GnssSatellitesVisibleField.DataType, ref _gnssSatellitesVisible);    

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
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreqField = new Field.Builder()
            .Name(nameof(TotalFreq))
            .Title("total_freq")
            .Description("Measured frequency.")
.Units(@"Hz")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _totalFreq;
        public ulong TotalFreq { get => _totalFreq; set => _totalFreq = value; }
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
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLatField = new Field.Builder()
            .Name(nameof(GnssLat))
            .Title("gnss_lat")
            .Description("Latitude (WGS84, EGM96 ellipsoid)")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssLat;
        public int GnssLat { get => _gnssLat; set => _gnssLat = value; }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLonField = new Field.Builder()
            .Name(nameof(GnssLon))
            .Title("gnss_lon")
            .Description("Longitude (WGS84, EGM96 ellipsoid)")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssLon;
        public int GnssLon { get => _gnssLon; set => _gnssLon = value; }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltField = new Field.Builder()
            .Name(nameof(GnssAlt))
            .Title("gnss_alt")
            .Description("Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssAlt;
        public int GnssAlt { get => _gnssAlt; set => _gnssAlt = value; }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltEllipsoidField = new Field.Builder()
            .Name(nameof(GnssAltEllipsoid))
            .Title("gnss_alt_ellipsoid")
            .Description("Altitude (above WGS84, EGM96 ellipsoid). Positive for up.")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssAltEllipsoid;
        public int GnssAltEllipsoid { get => _gnssAltEllipsoid; set => _gnssAltEllipsoid = value; }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssHAccField = new Field.Builder()
            .Name(nameof(GnssHAcc))
            .Title("gnss_h_acc")
            .Description("Position uncertainty. Positive for up.")
.Units(@"mm")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _gnssHAcc;
        public uint GnssHAcc { get => _gnssHAcc; set => _gnssHAcc = value; }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVAccField = new Field.Builder()
            .Name(nameof(GnssVAcc))
            .Title("gnss_v_acc")
            .Description("Altitude uncertainty. Positive for up.")
.Units(@"mm")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _gnssVAcc;
        public uint GnssVAcc { get => _gnssVAcc; set => _gnssVAcc = value; }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelAccField = new Field.Builder()
            .Name(nameof(GnssVelAcc))
            .Title("gnss_vel_acc")
            .Description("Speed uncertainty. Positive for up.")
.Units(@"mm")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _gnssVelAcc;
        public uint GnssVelAcc { get => _gnssVelAcc; set => _gnssVelAcc = value; }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Filtered global position latitude, expressed")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _lat;
        public int Lat { get => _lat; set => _lat = value; }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LonField = new Field.Builder()
            .Name(nameof(Lon))
            .Title("lon")
            .Description("Filtered global position longitude, expressed")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _lon;
        public int Lon { get => _lon; set => _lon = value; }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltField = new Field.Builder()
            .Name(nameof(Alt))
            .Title("alt")
            .Description("Filtered global position altitude (MSL).")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _alt;
        public int Alt { get => _alt; set => _alt = value; }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field RelativeAltField = new Field.Builder()
            .Name(nameof(RelativeAlt))
            .Title("relative_alt")
            .Description("Altitude above ground")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _relativeAlt;
        public int RelativeAlt { get => _relativeAlt; set => _relativeAlt = value; }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle (-pi..+pi)")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _roll;
        public float Roll { get => _roll; set => _roll = value; }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch angle (-pi..+pi)")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _pitch;
        public float Pitch { get => _pitch; set => _pitch = value; }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw angle (-pi..+pi)")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _yaw;
        public float Yaw { get => _yaw; set => _yaw = value; }
        /// <summary>
        /// Input power of course.
        /// OriginName: crs_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field CrsPowerField = new Field.Builder()
            .Name(nameof(CrsPower))
            .Title("crs_power")
            .Description("Input power of course.")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _crsPower;
        public float CrsPower { get => _crsPower; set => _crsPower = value; }
        /// <summary>
        /// Aplitude modulation of 90Hz of course.
        /// OriginName: crs_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CrsAm90Field = new Field.Builder()
            .Name(nameof(CrsAm90))
            .Title("crs_am_90")
            .Description("Aplitude modulation of 90Hz of course.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _crsAm90;
        public float CrsAm90 { get => _crsAm90; set => _crsAm90 = value; }
        /// <summary>
        /// Aplitude modulation of 150Hz of course.
        /// OriginName: crs_am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CrsAm150Field = new Field.Builder()
            .Name(nameof(CrsAm150))
            .Title("crs_am_150")
            .Description("Aplitude modulation of 150Hz of course.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _crsAm150;
        public float CrsAm150 { get => _crsAm150; set => _crsAm150 = value; }
        /// <summary>
        /// Input power of clearance.
        /// OriginName: clr_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field ClrPowerField = new Field.Builder()
            .Name(nameof(ClrPower))
            .Title("clr_power")
            .Description("Input power of clearance.")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _clrPower;
        public float ClrPower { get => _clrPower; set => _clrPower = value; }
        /// <summary>
        /// Aplitude modulation of 90Hz of clearance.
        /// OriginName: clr_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field ClrAm90Field = new Field.Builder()
            .Name(nameof(ClrAm90))
            .Title("clr_am_90")
            .Description("Aplitude modulation of 90Hz of clearance.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _clrAm90;
        public float ClrAm90 { get => _clrAm90; set => _clrAm90 = value; }
        /// <summary>
        /// Aplitude modulation of 150Hz of clearance.
        /// OriginName: clr_am_150, Units: % E2, IsExtended: false
        /// </summary>
        public static readonly Field ClrAm150Field = new Field.Builder()
            .Name(nameof(ClrAm150))
            .Title("clr_am_150")
            .Description("Aplitude modulation of 150Hz of clearance.")
.Units(@"% E2")
            .DataType(FloatType.Default)
        .Build();
        private float _clrAm150;
        public float ClrAm150 { get => _clrAm150; set => _clrAm150 = value; }
        /// <summary>
        /// Total input power.
        /// OriginName: total_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field TotalPowerField = new Field.Builder()
            .Name(nameof(TotalPower))
            .Title("total_power")
            .Description("Total input power.")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _totalPower;
        public float TotalPower { get => _totalPower; set => _totalPower = value; }
        /// <summary>
        /// Total field strength.
        /// OriginName: total_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field TotalFieldStrengthField = new Field.Builder()
            .Name(nameof(TotalFieldStrength))
            .Title("total_field_strength")
            .Description("Total field strength.")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _totalFieldStrength;
        public float TotalFieldStrength { get => _totalFieldStrength; set => _totalFieldStrength = value; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: total_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TotalAm90Field = new Field.Builder()
            .Name(nameof(TotalAm90))
            .Title("total_am_90")
            .Description("Total aplitude modulation of 90Hz.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _totalAm90;
        public float TotalAm90 { get => _totalAm90; set => _totalAm90 = value; }
        /// <summary>
        /// Total aplitude modulation of 150Hz.
        /// OriginName: total_am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TotalAm150Field = new Field.Builder()
            .Name(nameof(TotalAm150))
            .Title("total_am_150")
            .Description("Total aplitude modulation of 150Hz.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _totalAm150;
        public float TotalAm150 { get => _totalAm150; set => _totalAm150 = value; }
        /// <summary>
        ///  Phase difference 90 Hz clearance and cource
        /// OriginName: phi_90_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi90CrsVsClrField = new Field.Builder()
            .Name(nameof(Phi90CrsVsClr))
            .Title("phi_90_crs_vs_clr")
            .Description(" Phase difference 90 Hz clearance and cource")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _phi90CrsVsClr;
        public float Phi90CrsVsClr { get => _phi90CrsVsClr; set => _phi90CrsVsClr = value; }
        /// <summary>
        /// Phase difference 150 Hz clearance and cource.
        /// OriginName: phi_150_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi150CrsVsClrField = new Field.Builder()
            .Name(nameof(Phi150CrsVsClr))
            .Title("phi_150_crs_vs_clr")
            .Description("Phase difference 150 Hz clearance and cource.")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _phi150CrsVsClr;
        public float Phi150CrsVsClr { get => _phi150CrsVsClr; set => _phi150CrsVsClr = value; }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEphField = new Field.Builder()
            .Name(nameof(GnssEph))
            .Title("gnss_eph")
            .Description("GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _gnssEph;
        public ushort GnssEph { get => _gnssEph; set => _gnssEph = value; }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEpvField = new Field.Builder()
            .Name(nameof(GnssEpv))
            .Title("gnss_epv")
            .Description("GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _gnssEpv;
        public ushort GnssEpv { get => _gnssEpv; set => _gnssEpv = value; }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelField = new Field.Builder()
            .Name(nameof(GnssVel))
            .Title("gnss_vel")
            .Description("GPS ground speed. If unknown, set to: UINT16_MAX")
.Units(@"cm/s")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _gnssVel;
        public ushort GnssVel { get => _gnssVel; set => _gnssVel = value; }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VxField = new Field.Builder()
            .Name(nameof(Vx))
            .Title("vx")
            .Description("Ground X Speed (Latitude, positive north)")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _vx;
        public short Vx { get => _vx; set => _vx = value; }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VyField = new Field.Builder()
            .Name(nameof(Vy))
            .Title("vy")
            .Description("Ground Y Speed (Longitude, positive east)")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _vy;
        public short Vy { get => _vy; set => _vy = value; }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VzField = new Field.Builder()
            .Name(nameof(Vz))
            .Title("vz")
            .Description("Ground Z Speed (Altitude, positive down)")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _vz;
        public short Vz { get => _vz; set => _vz = value; }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field HdgField = new Field.Builder()
            .Name(nameof(Hdg))
            .Title("hdg")
            .Description("Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX")
.Units(@"cdeg")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _hdg;
        public ushort Hdg { get => _hdg; set => _hdg = value; }
        /// <summary>
        /// Carrier frequency offset of course.
        /// OriginName: crs_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsCarrierOffsetField = new Field.Builder()
            .Name(nameof(CrsCarrierOffset))
            .Title("crs_carrier_offset")
            .Description("Carrier frequency offset of course.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _crsCarrierOffset;
        public short CrsCarrierOffset { get => _crsCarrierOffset; set => _crsCarrierOffset = value; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of course.
        /// OriginName: crs_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsFreq90Field = new Field.Builder()
            .Name(nameof(CrsFreq90))
            .Title("crs_freq_90")
            .Description("Frequency offset of signal 90 Hz of course.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _crsFreq90;
        public short CrsFreq90 { get => _crsFreq90; set => _crsFreq90 = value; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of course.
        /// OriginName: crs_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsFreq150Field = new Field.Builder()
            .Name(nameof(CrsFreq150))
            .Title("crs_freq_150")
            .Description("Frequency offset of signal 150 Hz of course.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _crsFreq150;
        public short CrsFreq150 { get => _crsFreq150; set => _crsFreq150 = value; }
        /// <summary>
        /// Carrier frequency offset of clearance.
        /// OriginName: clr_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrCarrierOffsetField = new Field.Builder()
            .Name(nameof(ClrCarrierOffset))
            .Title("clr_carrier_offset")
            .Description("Carrier frequency offset of clearance.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _clrCarrierOffset;
        public short ClrCarrierOffset { get => _clrCarrierOffset; set => _clrCarrierOffset = value; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of clearance.
        /// OriginName: clr_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrFreq90Field = new Field.Builder()
            .Name(nameof(ClrFreq90))
            .Title("clr_freq_90")
            .Description("Frequency offset of signal 90 Hz of clearance.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _clrFreq90;
        public short ClrFreq90 { get => _clrFreq90; set => _clrFreq90 = value; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of clearance.
        /// OriginName: clr_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrFreq150Field = new Field.Builder()
            .Name(nameof(ClrFreq150))
            .Title("clr_freq_150")
            .Description("Frequency offset of signal 150 Hz of clearance.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _clrFreq150;
        public short ClrFreq150 { get => _clrFreq150; set => _clrFreq150 = value; }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: total_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalCarrierOffsetField = new Field.Builder()
            .Name(nameof(TotalCarrierOffset))
            .Title("total_carrier_offset")
            .Description("Total carrier frequency offset.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _totalCarrierOffset;
        public short TotalCarrierOffset { get => _totalCarrierOffset; set => _totalCarrierOffset = value; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: total_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreq90Field = new Field.Builder()
            .Name(nameof(TotalFreq90))
            .Title("total_freq_90")
            .Description("Total frequency offset of signal 90 Hz.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _totalFreq90;
        public short TotalFreq90 { get => _totalFreq90; set => _totalFreq90 = value; }
        /// <summary>
        /// Total frequency offset of signal 150 Hz.
        /// OriginName: total_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreq150Field = new Field.Builder()
            .Name(nameof(TotalFreq150))
            .Title("total_freq_150")
            .Description("Total frequency offset of signal 150 Hz.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _totalFreq150;
        public short TotalFreq150 { get => _totalFreq150; set => _totalFreq150 = value; }
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
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// GPS fix type.
        /// OriginName: gnss_fix_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssFixTypeField = new Field.Builder()
            .Name(nameof(GnssFixType))
            .Title("gnss_fix_type")
            .Description("GPS fix type.")
            .DataType(new UInt8Type(GpsFixTypeHelper.GetValues(x=>(byte)x).Min(),GpsFixTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(GpsFixTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private GpsFixType _gnssFixType;
        public GpsFixType GnssFixType { get => _gnssFixType; set => _gnssFixType = value; } 
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssSatellitesVisibleField = new Field.Builder()
            .Name(nameof(GnssSatellitesVisible))
            .Title("gnss_satellites_visible")
            .Description("Number of satellites visible. If unknown, set to 255")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _gnssSatellitesVisible;
        public byte GnssSatellitesVisible { get => _gnssSatellitesVisible; set => _gnssSatellitesVisible = value; }
    }
    /// <summary>
    /// VOR reciever record data.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DATA_VOR
    /// </summary>
    public class AsvSdrRecordDataVorPacket : MavlinkV2Message<AsvSdrRecordDataVorPayload>
    {
        public const int MessageId = 13137;
        
        public const byte CrcExtra = 250;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDataVorPayload Payload { get; } = new();

        public override string Name => "ASV_SDR_RECORD_DATA_VOR";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_VOR
    /// </summary>
    public class AsvSdrRecordDataVorPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 150; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 150; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            +8 // uint64_t total_freq
            +4 // uint32_t data_index
            +4 // int32_t gnss_lat
            +4 // int32_t gnss_lon
            +4 // int32_t gnss_alt
            +4 // int32_t gnss_alt_ellipsoid
            +4 // uint32_t gnss_h_acc
            +4 // uint32_t gnss_v_acc
            +4 // uint32_t gnss_vel_acc
            +4 // int32_t lat
            +4 // int32_t lon
            +4 // int32_t alt
            +4 // int32_t relative_alt
            +4 // float roll
            +4 // float pitch
            +4 // float yaw
            +4 // float azimuth
            +4 // float power
            +4 // float field_strength
            +4 // float am_30
            +4 // float am_9960
            +4 // float deviation
            +4 // float code_id_am_1020
            +2 // uint16_t gnss_eph
            +2 // uint16_t gnss_epv
            +2 // uint16_t gnss_vel
            +2 // int16_t vx
            +2 // int16_t vy
            +2 // int16_t vz
            +2 // uint16_t hdg
            +2 // int16_t carrier_offset
            +2 // int16_t freq_30
            +2 // int16_t freq_9960
            +2 // int16_t code_id_freq_1020
            +2 // int16_t measure_time
            +RecordGuid.Length // uint8_t[16] record_guid
            + 1 // uint8_t gnss_fix_type
            +1 // uint8_t gnss_satellites_visible
            +CodeId.Length // char[4] code_id
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            TotalFreq = BinSerialize.ReadULong(ref buffer);
            DataIndex = BinSerialize.ReadUInt(ref buffer);
            GnssLat = BinSerialize.ReadInt(ref buffer);
            GnssLon = BinSerialize.ReadInt(ref buffer);
            GnssAlt = BinSerialize.ReadInt(ref buffer);
            GnssAltEllipsoid = BinSerialize.ReadInt(ref buffer);
            GnssHAcc = BinSerialize.ReadUInt(ref buffer);
            GnssVAcc = BinSerialize.ReadUInt(ref buffer);
            GnssVelAcc = BinSerialize.ReadUInt(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lon = BinSerialize.ReadInt(ref buffer);
            Alt = BinSerialize.ReadInt(ref buffer);
            RelativeAlt = BinSerialize.ReadInt(ref buffer);
            Roll = BinSerialize.ReadFloat(ref buffer);
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            Azimuth = BinSerialize.ReadFloat(ref buffer);
            Power = BinSerialize.ReadFloat(ref buffer);
            FieldStrength = BinSerialize.ReadFloat(ref buffer);
            Am30 = BinSerialize.ReadFloat(ref buffer);
            Am9960 = BinSerialize.ReadFloat(ref buffer);
            Deviation = BinSerialize.ReadFloat(ref buffer);
            CodeIdAm1020 = BinSerialize.ReadFloat(ref buffer);
            GnssEph = BinSerialize.ReadUShort(ref buffer);
            GnssEpv = BinSerialize.ReadUShort(ref buffer);
            GnssVel = BinSerialize.ReadUShort(ref buffer);
            Vx = BinSerialize.ReadShort(ref buffer);
            Vy = BinSerialize.ReadShort(ref buffer);
            Vz = BinSerialize.ReadShort(ref buffer);
            Hdg = BinSerialize.ReadUShort(ref buffer);
            CarrierOffset = BinSerialize.ReadShort(ref buffer);
            Freq30 = BinSerialize.ReadShort(ref buffer);
            Freq9960 = BinSerialize.ReadShort(ref buffer);
            CodeIdFreq1020 = BinSerialize.ReadShort(ref buffer);
            MeasureTime = BinSerialize.ReadShort(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/150 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                RecordGuid[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            GnssFixType = (GpsFixType)BinSerialize.ReadByte(ref buffer);
            GnssSatellitesVisible = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = 4;
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
            BinSerialize.WriteULong(ref buffer,TotalFreq);
            BinSerialize.WriteUInt(ref buffer,DataIndex);
            BinSerialize.WriteInt(ref buffer,GnssLat);
            BinSerialize.WriteInt(ref buffer,GnssLon);
            BinSerialize.WriteInt(ref buffer,GnssAlt);
            BinSerialize.WriteInt(ref buffer,GnssAltEllipsoid);
            BinSerialize.WriteUInt(ref buffer,GnssHAcc);
            BinSerialize.WriteUInt(ref buffer,GnssVAcc);
            BinSerialize.WriteUInt(ref buffer,GnssVelAcc);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lon);
            BinSerialize.WriteInt(ref buffer,Alt);
            BinSerialize.WriteInt(ref buffer,RelativeAlt);
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteFloat(ref buffer,Azimuth);
            BinSerialize.WriteFloat(ref buffer,Power);
            BinSerialize.WriteFloat(ref buffer,FieldStrength);
            BinSerialize.WriteFloat(ref buffer,Am30);
            BinSerialize.WriteFloat(ref buffer,Am9960);
            BinSerialize.WriteFloat(ref buffer,Deviation);
            BinSerialize.WriteFloat(ref buffer,CodeIdAm1020);
            BinSerialize.WriteUShort(ref buffer,GnssEph);
            BinSerialize.WriteUShort(ref buffer,GnssEpv);
            BinSerialize.WriteUShort(ref buffer,GnssVel);
            BinSerialize.WriteShort(ref buffer,Vx);
            BinSerialize.WriteShort(ref buffer,Vy);
            BinSerialize.WriteShort(ref buffer,Vz);
            BinSerialize.WriteUShort(ref buffer,Hdg);
            BinSerialize.WriteShort(ref buffer,CarrierOffset);
            BinSerialize.WriteShort(ref buffer,Freq30);
            BinSerialize.WriteShort(ref buffer,Freq9960);
            BinSerialize.WriteShort(ref buffer,CodeIdFreq1020);
            BinSerialize.WriteShort(ref buffer,MeasureTime);
            for(var i=0;i<RecordGuid.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RecordGuid[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)GnssFixType);
            BinSerialize.WriteByte(ref buffer,(byte)GnssSatellitesVisible);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CodeId)
                {
                    Encoding.ASCII.GetBytes(charPointer, CodeId.Length, bytePointer, CodeId.Length);
                }
            }
            buffer = buffer.Slice(CodeId.Length);
            
            /* PayloadByteSize = 150 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            UInt64Type.Accept(visitor,TotalFreqField, TotalFreqField.DataType, ref _totalFreq);    
            UInt32Type.Accept(visitor,DataIndexField, DataIndexField.DataType, ref _dataIndex);    
            Int32Type.Accept(visitor,GnssLatField, GnssLatField.DataType, ref _gnssLat);    
            Int32Type.Accept(visitor,GnssLonField, GnssLonField.DataType, ref _gnssLon);    
            Int32Type.Accept(visitor,GnssAltField, GnssAltField.DataType, ref _gnssAlt);    
            Int32Type.Accept(visitor,GnssAltEllipsoidField, GnssAltEllipsoidField.DataType, ref _gnssAltEllipsoid);    
            UInt32Type.Accept(visitor,GnssHAccField, GnssHAccField.DataType, ref _gnssHAcc);    
            UInt32Type.Accept(visitor,GnssVAccField, GnssVAccField.DataType, ref _gnssVAcc);    
            UInt32Type.Accept(visitor,GnssVelAccField, GnssVelAccField.DataType, ref _gnssVelAcc);    
            Int32Type.Accept(visitor,LatField, LatField.DataType, ref _lat);    
            Int32Type.Accept(visitor,LonField, LonField.DataType, ref _lon);    
            Int32Type.Accept(visitor,AltField, AltField.DataType, ref _alt);    
            Int32Type.Accept(visitor,RelativeAltField, RelativeAltField.DataType, ref _relativeAlt);    
            FloatType.Accept(visitor,RollField, RollField.DataType, ref _roll);    
            FloatType.Accept(visitor,PitchField, PitchField.DataType, ref _pitch);    
            FloatType.Accept(visitor,YawField, YawField.DataType, ref _yaw);    
            FloatType.Accept(visitor,AzimuthField, AzimuthField.DataType, ref _azimuth);    
            FloatType.Accept(visitor,PowerField, PowerField.DataType, ref _power);    
            FloatType.Accept(visitor,FieldStrengthField, FieldStrengthField.DataType, ref _fieldStrength);    
            FloatType.Accept(visitor,Am30Field, Am30Field.DataType, ref _am30);    
            FloatType.Accept(visitor,Am9960Field, Am9960Field.DataType, ref _am9960);    
            FloatType.Accept(visitor,DeviationField, DeviationField.DataType, ref _deviation);    
            FloatType.Accept(visitor,CodeIdAm1020Field, CodeIdAm1020Field.DataType, ref _codeIdAm1020);    
            UInt16Type.Accept(visitor,GnssEphField, GnssEphField.DataType, ref _gnssEph);    
            UInt16Type.Accept(visitor,GnssEpvField, GnssEpvField.DataType, ref _gnssEpv);    
            UInt16Type.Accept(visitor,GnssVelField, GnssVelField.DataType, ref _gnssVel);    
            Int16Type.Accept(visitor,VxField, VxField.DataType, ref _vx);
            Int16Type.Accept(visitor,VyField, VyField.DataType, ref _vy);
            Int16Type.Accept(visitor,VzField, VzField.DataType, ref _vz);
            UInt16Type.Accept(visitor,HdgField, HdgField.DataType, ref _hdg);    
            Int16Type.Accept(visitor,CarrierOffsetField, CarrierOffsetField.DataType, ref _carrierOffset);
            Int16Type.Accept(visitor,Freq30Field, Freq30Field.DataType, ref _freq30);
            Int16Type.Accept(visitor,Freq9960Field, Freq9960Field.DataType, ref _freq9960);
            Int16Type.Accept(visitor,CodeIdFreq1020Field, CodeIdFreq1020Field.DataType, ref _codeIdFreq1020);
            Int16Type.Accept(visitor,MeasureTimeField, MeasureTimeField.DataType, ref _measureTime);
            ArrayType.Accept(visitor,RecordGuidField, RecordGuidField.DataType, 16,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RecordGuid[index]));    
            var tmpGnssFixType = (byte)GnssFixType;
            UInt8Type.Accept(visitor,GnssFixTypeField, GnssFixTypeField.DataType, ref tmpGnssFixType);
            GnssFixType = (GpsFixType)tmpGnssFixType;
            UInt8Type.Accept(visitor,GnssSatellitesVisibleField, GnssSatellitesVisibleField.DataType, ref _gnssSatellitesVisible);    
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
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreqField = new Field.Builder()
            .Name(nameof(TotalFreq))
            .Title("total_freq")
            .Description("Measured frequency.")
.Units(@"Hz")
            .DataType(UInt64Type.Default)
        .Build();
        private ulong _totalFreq;
        public ulong TotalFreq { get => _totalFreq; set => _totalFreq = value; }
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
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLatField = new Field.Builder()
            .Name(nameof(GnssLat))
            .Title("gnss_lat")
            .Description("Latitude (WGS84, EGM96 ellipsoid)")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssLat;
        public int GnssLat { get => _gnssLat; set => _gnssLat = value; }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLonField = new Field.Builder()
            .Name(nameof(GnssLon))
            .Title("gnss_lon")
            .Description("Longitude (WGS84, EGM96 ellipsoid)")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssLon;
        public int GnssLon { get => _gnssLon; set => _gnssLon = value; }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltField = new Field.Builder()
            .Name(nameof(GnssAlt))
            .Title("gnss_alt")
            .Description("Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssAlt;
        public int GnssAlt { get => _gnssAlt; set => _gnssAlt = value; }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltEllipsoidField = new Field.Builder()
            .Name(nameof(GnssAltEllipsoid))
            .Title("gnss_alt_ellipsoid")
            .Description("Altitude (above WGS84, EGM96 ellipsoid). Positive for up.")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _gnssAltEllipsoid;
        public int GnssAltEllipsoid { get => _gnssAltEllipsoid; set => _gnssAltEllipsoid = value; }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssHAccField = new Field.Builder()
            .Name(nameof(GnssHAcc))
            .Title("gnss_h_acc")
            .Description("Position uncertainty. Positive for up.")
.Units(@"mm")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _gnssHAcc;
        public uint GnssHAcc { get => _gnssHAcc; set => _gnssHAcc = value; }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVAccField = new Field.Builder()
            .Name(nameof(GnssVAcc))
            .Title("gnss_v_acc")
            .Description("Altitude uncertainty. Positive for up.")
.Units(@"mm")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _gnssVAcc;
        public uint GnssVAcc { get => _gnssVAcc; set => _gnssVAcc = value; }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelAccField = new Field.Builder()
            .Name(nameof(GnssVelAcc))
            .Title("gnss_vel_acc")
            .Description("Speed uncertainty. Positive for up.")
.Units(@"mm")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _gnssVelAcc;
        public uint GnssVelAcc { get => _gnssVelAcc; set => _gnssVelAcc = value; }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Filtered global position latitude, expressed")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _lat;
        public int Lat { get => _lat; set => _lat = value; }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LonField = new Field.Builder()
            .Name(nameof(Lon))
            .Title("lon")
            .Description("Filtered global position longitude, expressed")
.Units(@"degE7")
            .DataType(Int32Type.Default)
        .Build();
        private int _lon;
        public int Lon { get => _lon; set => _lon = value; }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltField = new Field.Builder()
            .Name(nameof(Alt))
            .Title("alt")
            .Description("Filtered global position altitude (MSL).")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _alt;
        public int Alt { get => _alt; set => _alt = value; }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field RelativeAltField = new Field.Builder()
            .Name(nameof(RelativeAlt))
            .Title("relative_alt")
            .Description("Altitude above ground")
.Units(@"mm")
            .DataType(Int32Type.Default)
        .Build();
        private int _relativeAlt;
        public int RelativeAlt { get => _relativeAlt; set => _relativeAlt = value; }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle (-pi..+pi)")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _roll;
        public float Roll { get => _roll; set => _roll = value; }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch angle (-pi..+pi)")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _pitch;
        public float Pitch { get => _pitch; set => _pitch = value; }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw angle (-pi..+pi)")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _yaw;
        public float Yaw { get => _yaw; set => _yaw = value; }
        /// <summary>
        /// Measured azimuth.
        /// OriginName: azimuth, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field AzimuthField = new Field.Builder()
            .Name(nameof(Azimuth))
            .Title("azimuth")
            .Description("Measured azimuth.")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _azimuth;
        public float Azimuth { get => _azimuth; set => _azimuth = value; }
        /// <summary>
        /// Total input power.
        /// OriginName: power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field PowerField = new Field.Builder()
            .Name(nameof(Power))
            .Title("power")
            .Description("Total input power.")
.Units(@"dBm")
            .DataType(FloatType.Default)
        .Build();
        private float _power;
        public float Power { get => _power; set => _power = value; }
        /// <summary>
        /// Total field strength.
        /// OriginName: field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field FieldStrengthField = new Field.Builder()
            .Name(nameof(FieldStrength))
            .Title("field_strength")
            .Description("Total field strength.")
.Units(@"uV/m")
            .DataType(FloatType.Default)
        .Build();
        private float _fieldStrength;
        public float FieldStrength { get => _fieldStrength; set => _fieldStrength = value; }
        /// <summary>
        /// Total aplitude modulation of 30 Hz.
        /// OriginName: am_30, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am30Field = new Field.Builder()
            .Name(nameof(Am30))
            .Title("am_30")
            .Description("Total aplitude modulation of 30 Hz.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am30;
        public float Am30 { get => _am30; set => _am30 = value; }
        /// <summary>
        /// Total aplitude modulation of 9960 Hz.
        /// OriginName: am_9960, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am9960Field = new Field.Builder()
            .Name(nameof(Am9960))
            .Title("am_9960")
            .Description("Total aplitude modulation of 9960 Hz.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _am9960;
        public float Am9960 { get => _am9960; set => _am9960 = value; }
        /// <summary>
        /// Deviation.
        /// OriginName: deviation, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DeviationField = new Field.Builder()
            .Name(nameof(Deviation))
            .Title("deviation")
            .Description("Deviation.")
.Units(@"")
            .DataType(FloatType.Default)
        .Build();
        private float _deviation;
        public float Deviation { get => _deviation; set => _deviation = value; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: code_id_am_1020, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdAm1020Field = new Field.Builder()
            .Name(nameof(CodeIdAm1020))
            .Title("code_id_am_1020")
            .Description("Total aplitude modulation of 90Hz.")
.Units(@"%")
            .DataType(FloatType.Default)
        .Build();
        private float _codeIdAm1020;
        public float CodeIdAm1020 { get => _codeIdAm1020; set => _codeIdAm1020 = value; }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEphField = new Field.Builder()
            .Name(nameof(GnssEph))
            .Title("gnss_eph")
            .Description("GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _gnssEph;
        public ushort GnssEph { get => _gnssEph; set => _gnssEph = value; }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEpvField = new Field.Builder()
            .Name(nameof(GnssEpv))
            .Title("gnss_epv")
            .Description("GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _gnssEpv;
        public ushort GnssEpv { get => _gnssEpv; set => _gnssEpv = value; }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelField = new Field.Builder()
            .Name(nameof(GnssVel))
            .Title("gnss_vel")
            .Description("GPS ground speed. If unknown, set to: UINT16_MAX")
.Units(@"cm/s")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _gnssVel;
        public ushort GnssVel { get => _gnssVel; set => _gnssVel = value; }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VxField = new Field.Builder()
            .Name(nameof(Vx))
            .Title("vx")
            .Description("Ground X Speed (Latitude, positive north)")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _vx;
        public short Vx { get => _vx; set => _vx = value; }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VyField = new Field.Builder()
            .Name(nameof(Vy))
            .Title("vy")
            .Description("Ground Y Speed (Longitude, positive east)")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _vy;
        public short Vy { get => _vy; set => _vy = value; }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VzField = new Field.Builder()
            .Name(nameof(Vz))
            .Title("vz")
            .Description("Ground Z Speed (Altitude, positive down)")
.Units(@"cm/s")
            .DataType(Int16Type.Default)
        .Build();
        private short _vz;
        public short Vz { get => _vz; set => _vz = value; }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field HdgField = new Field.Builder()
            .Name(nameof(Hdg))
            .Title("hdg")
            .Description("Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX")
.Units(@"cdeg")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _hdg;
        public ushort Hdg { get => _hdg; set => _hdg = value; }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CarrierOffsetField = new Field.Builder()
            .Name(nameof(CarrierOffset))
            .Title("carrier_offset")
            .Description("Total carrier frequency offset.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _carrierOffset;
        public short CarrierOffset { get => _carrierOffset; set => _carrierOffset = value; }
        /// <summary>
        /// Total frequency offset of signal 30 Hz.
        /// OriginName: freq_30, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq30Field = new Field.Builder()
            .Name(nameof(Freq30))
            .Title("freq_30")
            .Description("Total frequency offset of signal 30 Hz.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq30;
        public short Freq30 { get => _freq30; set => _freq30 = value; }
        /// <summary>
        /// Total frequency offset of signal 9960 Hz.
        /// OriginName: freq_9960, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq9960Field = new Field.Builder()
            .Name(nameof(Freq9960))
            .Title("freq_9960")
            .Description("Total frequency offset of signal 9960 Hz.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _freq9960;
        public short Freq9960 { get => _freq9960; set => _freq9960 = value; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: code_id_freq_1020, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdFreq1020Field = new Field.Builder()
            .Name(nameof(CodeIdFreq1020))
            .Title("code_id_freq_1020")
            .Description("Total frequency offset of signal 90 Hz.")
.Units(@"Hz")
            .DataType(Int16Type.Default)
        .Build();
        private short _codeIdFreq1020;
        public short CodeIdFreq1020 { get => _codeIdFreq1020; set => _codeIdFreq1020 = value; }
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
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")

            .DataType(new ArrayType(UInt8Type.Default,16))
        .Build();
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// GPS fix type.
        /// OriginName: gnss_fix_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssFixTypeField = new Field.Builder()
            .Name(nameof(GnssFixType))
            .Title("gnss_fix_type")
            .Description("GPS fix type.")
            .DataType(new UInt8Type(GpsFixTypeHelper.GetValues(x=>(byte)x).Min(),GpsFixTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(GpsFixTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private GpsFixType _gnssFixType;
        public GpsFixType GnssFixType { get => _gnssFixType; set => _gnssFixType = value; } 
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssSatellitesVisibleField = new Field.Builder()
            .Name(nameof(GnssSatellitesVisible))
            .Title("gnss_satellites_visible")
            .Description("Number of satellites visible. If unknown, set to 255")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _gnssSatellitesVisible;
        public byte GnssSatellitesVisible { get => _gnssSatellitesVisible; set => _gnssSatellitesVisible = value; }
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
    }




        


#endregion


}

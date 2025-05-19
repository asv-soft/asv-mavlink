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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.16+8bb2f8865168bf54d58a112cb63c6bf098479247 25-05-12.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
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
    public enum MavType:uint
    {
        /// <summary>
        /// Used to identify Software-defined radio payload in HEARTBEAT packet.
        /// MAV_TYPE_ASV_SDR_PAYLOAD
        /// </summary>
        MavTypeAsvSdrPayload = 251,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
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

    /// <summary>
    /// State of the current mission (unit8_t).
    ///  ASV_SDR_MISSION_STATE
    /// </summary>
    public enum AsvSdrMissionState:uint
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

    /// <summary>
    /// Specifies the datatype of a record tag (unit8_t).
    ///  ASV_SDR_RECORD_TAG_TYPE
    /// </summary>
    public enum AsvSdrRecordTagType:uint
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

    /// <summary>
    /// A mapping of SDR payload modes for custom_mode field of heartbeat. Value of enum must be equal to message id from ASV_SDR_RECORD_DATA_* 13150-13199
    ///  ASV_SDR_CUSTOM_MODE
    /// </summary>
    public enum AsvSdrCustomMode:uint
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

    /// <summary>
    /// These flags encode supported mode.[!THIS_IS_ENUM_FLAG!]
    ///  ASV_SDR_CUSTOM_MODE_FLAG
    /// </summary>
    [Flags]
    public enum AsvSdrCustomModeFlag:uint
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

    /// <summary>
    /// ACK / NACK / ERROR values as a result of ASV_SDR_*_REQUEST or ASV_SDR_*_DELETE commands.
    ///  ASV_SDR_REQUEST_ACK
    /// </summary>
    public enum AsvSdrRequestAck:uint
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

    /// <summary>
    /// SDR system control actions [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_SYSTEM_CONTROL_ACTION
    /// </summary>
    public enum AsvSdrSystemControlAction:uint
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

    /// <summary>
    /// Status of calibration process.
    ///  ASV_SDR_CALIB_STATE
    /// </summary>
    public enum AsvSdrCalibState:uint
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

    /// <summary>
    /// SDR signal transmition data type
    ///  ASV_SDR_SIGNAL_FORMAT
    /// </summary>
    public enum AsvSdrSignalFormat:uint
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "supported_modes",
            "Supported ASV_SDR_CUSTOM_MODE.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(1,
            "size",
            "Total storage size in bytes.",
            string.Empty, 
            @"bytes", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(2,
            "record_count",
            "Number of records in storage.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "current_mission_index",
            "Current mission index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(4,
            "current_record_guid",
            "Record GUID. Also by this field we can understand if the data is currently being recorded (GUID!=0x00) or not (GUID==0x00).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
            new(5,
            "current_record_mode",
            "Current record mode (record data type).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(6,
            "current_record_name",
            "Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string. If the data is currently not being recorded, than return null; ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            28, 
            false),
            new(7,
            "mission_state",
            "Mission state.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(8,
            "calib_state",
            "Calibration status.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            true),
            new(9,
            "calib_table_count",
            "Number of calibration tables.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            true),
            new(10,
            "ref_power",
            "Estimated reference power in dBm. Entered in MAV_CMD_ASV_SDR_SET_MODE command.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            true),
            new(11,
            "signal_overflow",
            "Input path signal overflow indicator. Relative value from 0.0 to 1.0.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            true),
        ];
        public const string FormatMessage = "ASV_SDR_OUT_STATUS:"
        + "uint64_t supported_modes;"
        + "uint64_t size;"
        + "uint16_t record_count;"
        + "uint16_t current_mission_index;"
        + "uint8_t[16] current_record_guid;"
        + "uint8_t current_record_mode;"
        + "char[28] current_record_name;"
        + "uint8_t mission_state;"
        + "uint8_t calib_state;"
        + "uint16_t calib_table_count;"
        + "float ref_power;"
        + "float signal_overflow;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], (ulong)Payload.SupportedModes);
            writer.Write(StaticFields[1], Payload.Size);
            writer.Write(StaticFields[2], Payload.RecordCount);
            writer.Write(StaticFields[3], Payload.CurrentMissionIndex);
            writer.Write(StaticFields[4], Payload.CurrentRecordGuid);
            writer.Write(StaticFields[5], (ulong)Payload.CurrentRecordMode);
            writer.Write(StaticFields[6], Payload.CurrentRecordName);
            writer.Write(StaticFields[7], (ulong)Payload.MissionState);
            writer.Write(StaticFields[8], (ulong)Payload.CalibState);
            writer.Write(StaticFields[9], Payload.CalibTableCount);
            writer.Write(StaticFields[10], Payload.RefPower);
            writer.Write(StaticFields[11], Payload.SignalOverflow);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.SupportedModes = (AsvSdrCustomModeFlag)reader.ReadULong(StaticFields[0]);
            Payload.Size = reader.ReadULong(StaticFields[1]);
            Payload.RecordCount = reader.ReadUShort(StaticFields[2]);
            Payload.CurrentMissionIndex = reader.ReadUShort(StaticFields[3]);
            reader.ReadByteArray(StaticFields[4], Payload.CurrentRecordGuid);
            Payload.CurrentRecordMode = (AsvSdrCustomMode)reader.ReadByte(StaticFields[5]);
            reader.ReadCharArray(StaticFields[6], Payload.CurrentRecordName);
            Payload.MissionState = (AsvSdrMissionState)reader.ReadByte(StaticFields[7]);
            Payload.CalibState = (AsvSdrCalibState)reader.ReadByte(StaticFields[8]);
            Payload.CalibTableCount = reader.ReadUShort(StaticFields[9]);
            Payload.RefPower = reader.ReadFloat(StaticFields[10]);
            Payload.SignalOverflow = reader.ReadFloat(StaticFields[11]);
        
            
        }
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
            CurrentRecordName = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = CurrentRecordName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, CurrentRecordName.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
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
        
        



        /// <summary>
        /// Supported ASV_SDR_CUSTOM_MODE.
        /// OriginName: supported_modes, Units: , IsExtended: false
        /// </summary>
        public AsvSdrCustomModeFlag SupportedModes { get; set; }
        /// <summary>
        /// Total storage size in bytes.
        /// OriginName: size, Units: bytes, IsExtended: false
        /// </summary>
        public ulong Size { get; set; }
        /// <summary>
        /// Number of records in storage.
        /// OriginName: record_count, Units: , IsExtended: false
        /// </summary>
        public ushort RecordCount { get; set; }
        /// <summary>
        /// Current mission index.
        /// OriginName: current_mission_index, Units: , IsExtended: false
        /// </summary>
        public ushort CurrentMissionIndex { get; set; }
        /// <summary>
        /// Record GUID. Also by this field we can understand if the data is currently being recorded (GUID!=0x00) or not (GUID==0x00).
        /// OriginName: current_record_guid, Units: , IsExtended: false
        /// </summary>
        public const int CurrentRecordGuidMaxItemsCount = 16;
        public byte[] CurrentRecordGuid { get; } = new byte[16];
        /// <summary>
        /// Current record mode (record data type).
        /// OriginName: current_record_mode, Units: , IsExtended: false
        /// </summary>
        public AsvSdrCustomMode CurrentRecordMode { get; set; }
        /// <summary>
        /// Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string. If the data is currently not being recorded, than return null; 
        /// OriginName: current_record_name, Units: , IsExtended: false
        /// </summary>
        public const int CurrentRecordNameMaxItemsCount = 28;
        public char[] CurrentRecordName { get; set; } = new char[28];
        [Obsolete("This method is deprecated. Use GetCurrentRecordNameMaxItemsCount instead.")]
        public byte GetCurrentRecordNameMaxItemsCount() => 28;
        /// <summary>
        /// Mission state.
        /// OriginName: mission_state, Units: , IsExtended: false
        /// </summary>
        public AsvSdrMissionState MissionState { get; set; }
        /// <summary>
        /// Calibration status.
        /// OriginName: calib_state, Units: , IsExtended: true
        /// </summary>
        public AsvSdrCalibState CalibState { get; set; }
        /// <summary>
        /// Number of calibration tables.
        /// OriginName: calib_table_count, Units: , IsExtended: true
        /// </summary>
        public ushort CalibTableCount { get; set; }
        /// <summary>
        /// Estimated reference power in dBm. Entered in MAV_CMD_ASV_SDR_SET_MODE command.
        /// OriginName: ref_power, Units: , IsExtended: true
        /// </summary>
        public float RefPower { get; set; }
        /// <summary>
        /// Input path signal overflow indicator. Relative value from 0.0 to 1.0.
        /// OriginName: signal_overflow, Units: , IsExtended: true
        /// </summary>
        public float SignalOverflow { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Specifies a unique number for this request. This allows the response packet to be identified.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "skip",
            "Specifies the start index of the records to be sent in the response.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "count",
            "Specifies the number of records to be sent in the response after the skip index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "target_system",
            "System ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "target_component",
            "Component ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_REQUEST:"
        + "uint16_t request_id;"
        + "uint16_t skip;"
        + "uint16_t count;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], Payload.Skip);
            writer.Write(StaticFields[2], Payload.Count);
            writer.Write(StaticFields[3], Payload.TargetSystem);
            writer.Write(StaticFields[4], Payload.TargetComponent);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.Skip = reader.ReadUShort(StaticFields[1]);
            Payload.Count = reader.ReadUShort(StaticFields[2]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[3]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[4]);
        
            
        }
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
        
        



        /// <summary>
        /// Specifies a unique number for this request. This allows the response packet to be identified.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Specifies the start index of the records to be sent in the response.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public ushort Skip { get; set; }
        /// <summary>
        /// Specifies the number of records to be sent in the response after the skip index.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public ushort Count { get; set; }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "items_count",
            "Number of items ASV_SDR_RECORD for transmition after this request with success result code (depended from request).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "result",
            "Result code.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_RESPONSE:"
        + "uint16_t request_id;"
        + "uint16_t items_count;"
        + "uint8_t result;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], Payload.ItemsCount);
            writer.Write(StaticFields[2], (ulong)Payload.Result);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.ItemsCount = reader.ReadUShort(StaticFields[1]);
            Payload.Result = (AsvSdrRequestAck)reader.ReadByte(StaticFields[2]);
        
            
        }
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
        
        



        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Number of items ASV_SDR_RECORD for transmition after this request with success result code (depended from request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public ushort ItemsCount { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRequestAck Result { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "frequency",
            "Reference frequency in Hz, specified by MAV_CMD_ASV_SDR_SET_MODE command.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(1,
            "created_unix_us",
            "Created timestamp (UNIX epoch time).",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(2,
            "data_type",
            "Record data type(it is also possible to know the type of data inside the record by cast enum to int).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(3,
            "duration_sec",
            "Record duration in sec.",
            string.Empty, 
            @"sec", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(4,
            "data_count",
            "Data items count.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(5,
            "size",
            "Total data size of record with all data items and tags.",
            string.Empty, 
            @"bytes", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(6,
            "tag_count",
            "Tag items count.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(7,
            "record_guid",
            "Record GUID. Generated by payload after the start of recording.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
            new(8,
            "record_name",
            "Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            28, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD:"
        + "uint64_t frequency;"
        + "uint64_t created_unix_us;"
        + "uint32_t data_type;"
        + "uint32_t duration_sec;"
        + "uint32_t data_count;"
        + "uint32_t size;"
        + "uint16_t tag_count;"
        + "uint8_t[16] record_guid;"
        + "char[28] record_name;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Frequency);
            writer.Write(StaticFields[1], Payload.CreatedUnixUs);
            writer.Write(StaticFields[2], (ulong)Payload.DataType);
            writer.Write(StaticFields[3], Payload.DurationSec);
            writer.Write(StaticFields[4], Payload.DataCount);
            writer.Write(StaticFields[5], Payload.Size);
            writer.Write(StaticFields[6], Payload.TagCount);
            writer.Write(StaticFields[7], Payload.RecordGuid);
            writer.Write(StaticFields[8], Payload.RecordName);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Frequency = reader.ReadULong(StaticFields[0]);
            Payload.CreatedUnixUs = reader.ReadULong(StaticFields[1]);
            Payload.DataType = (AsvSdrCustomMode)reader.ReadUInt(StaticFields[2]);
            Payload.DurationSec = reader.ReadUInt(StaticFields[3]);
            Payload.DataCount = reader.ReadUInt(StaticFields[4]);
            Payload.Size = reader.ReadUInt(StaticFields[5]);
            Payload.TagCount = reader.ReadUShort(StaticFields[6]);
            reader.ReadByteArray(StaticFields[7], Payload.RecordGuid);
            reader.ReadCharArray(StaticFields[8], Payload.RecordName);
        
            
        }
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
            RecordName = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = RecordName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, RecordName.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           

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
        
        



        /// <summary>
        /// Reference frequency in Hz, specified by MAV_CMD_ASV_SDR_SET_MODE command.
        /// OriginName: frequency, Units: , IsExtended: false
        /// </summary>
        public ulong Frequency { get; set; }
        /// <summary>
        /// Created timestamp (UNIX epoch time).
        /// OriginName: created_unix_us, Units: us, IsExtended: false
        /// </summary>
        public ulong CreatedUnixUs { get; set; }
        /// <summary>
        /// Record data type(it is also possible to know the type of data inside the record by cast enum to int).
        /// OriginName: data_type, Units: , IsExtended: false
        /// </summary>
        public AsvSdrCustomMode DataType { get; set; }
        /// <summary>
        /// Record duration in sec.
        /// OriginName: duration_sec, Units: sec, IsExtended: false
        /// </summary>
        public uint DurationSec { get; set; }
        /// <summary>
        /// Data items count.
        /// OriginName: data_count, Units: , IsExtended: false
        /// </summary>
        public uint DataCount { get; set; }
        /// <summary>
        /// Total data size of record with all data items and tags.
        /// OriginName: size, Units: bytes, IsExtended: false
        /// </summary>
        public uint Size { get; set; }
        /// <summary>
        /// Tag items count.
        /// OriginName: tag_count, Units: , IsExtended: false
        /// </summary>
        public ushort TagCount { get; set; }
        /// <summary>
        /// Record GUID. Generated by payload after the start of recording.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; } = new byte[16];
        /// <summary>
        /// Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.
        /// OriginName: record_name, Units: , IsExtended: false
        /// </summary>
        public const int RecordNameMaxItemsCount = 28;
        public char[] RecordName { get; set; } = new char[28];
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Specifies a unique number for this request. This allows the response packet to be identified.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "target_system",
            "System ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(2,
            "target_component",
            "Component ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(3,
            "record_guid",
            "Specifies GUID of the record to be deleted.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_DELETE_REQUEST:"
        + "uint16_t request_id;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t[16] record_guid;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], Payload.TargetSystem);
            writer.Write(StaticFields[2], Payload.TargetComponent);
            writer.Write(StaticFields[3], Payload.RecordGuid);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[1]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[2]);
            reader.ReadByteArray(StaticFields[3], Payload.RecordGuid);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
        /// <summary>
        /// Specifies GUID of the record to be deleted.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "result",
            "Result code.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(2,
            "record_guid",
            "Specifies the GUID of the record that was deleted.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_DELETE_RESPONSE:"
        + "uint16_t request_id;"
        + "uint8_t result;"
        + "uint8_t[16] record_guid;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], (ulong)Payload.Result);
            writer.Write(StaticFields[2], Payload.RecordGuid);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.Result = (AsvSdrRequestAck)reader.ReadByte(StaticFields[1]);
            reader.ReadByteArray(StaticFields[2], Payload.RecordGuid);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
        
        



        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRequestAck Result { get; set; }
        /// <summary>
        /// Specifies the GUID of the record that was deleted.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Request unique number. This is to allow the response packet to be detected.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "skip",
            "Specifies the start index of the tag to be sent in the response.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "count",
            "Specifies the number of tag to be sent in the response after the skip index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "target_system",
            "System ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "target_component",
            "Component ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(5,
            "record_guid",
            "Specifies the GUID of the record.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_TAG_REQUEST:"
        + "uint16_t request_id;"
        + "uint16_t skip;"
        + "uint16_t count;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t[16] record_guid;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], Payload.Skip);
            writer.Write(StaticFields[2], Payload.Count);
            writer.Write(StaticFields[3], Payload.TargetSystem);
            writer.Write(StaticFields[4], Payload.TargetComponent);
            writer.Write(StaticFields[5], Payload.RecordGuid);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.Skip = reader.ReadUShort(StaticFields[1]);
            Payload.Count = reader.ReadUShort(StaticFields[2]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[3]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[4]);
            reader.ReadByteArray(StaticFields[5], Payload.RecordGuid);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
        
        



        /// <summary>
        /// Request unique number. This is to allow the response packet to be detected.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Specifies the start index of the tag to be sent in the response.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public ushort Skip { get; set; }
        /// <summary>
        /// Specifies the number of tag to be sent in the response after the skip index.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public ushort Count { get; set; }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Specifies the GUID of the record.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "items_count",
            "Number of items ASV_SDR_RECORD_TAG for transmition after this request with success result code (depended from request).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "result",
            "Result code.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_TAG_RESPONSE:"
        + "uint16_t request_id;"
        + "uint16_t items_count;"
        + "uint8_t result;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], Payload.ItemsCount);
            writer.Write(StaticFields[2], (ulong)Payload.Result);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.ItemsCount = reader.ReadUShort(StaticFields[1]);
            Payload.Result = (AsvSdrRequestAck)reader.ReadByte(StaticFields[2]);
        
            
        }
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
        
        



        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Number of items ASV_SDR_RECORD_TAG for transmition after this request with success result code (depended from request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public ushort ItemsCount { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRequestAck Result { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "record_guid",
            "Record GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
            new(1,
            "tag_guid",
            "Tag GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
            new(2,
            "tag_name",
            "Tag name, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            16, 
            false),
            new(3,
            "tag_type",
            "Tag type.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "tag_value",
            "Tag value, depends on the type of tag.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            8, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_TAG:"
        + "uint8_t[16] record_guid;"
        + "uint8_t[16] tag_guid;"
        + "char[16] tag_name;"
        + "uint8_t tag_type;"
        + "uint8_t[8] tag_value;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RecordGuid);
            writer.Write(StaticFields[1], Payload.TagGuid);
            writer.Write(StaticFields[2], Payload.TagName);
            writer.Write(StaticFields[3], (ulong)Payload.TagType);
            writer.Write(StaticFields[4], Payload.TagValue);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            reader.ReadByteArray(StaticFields[0], Payload.RecordGuid);
            reader.ReadByteArray(StaticFields[1], Payload.TagGuid);
            reader.ReadCharArray(StaticFields[2], Payload.TagName);
            Payload.TagType = (AsvSdrRecordTagType)reader.ReadByte(StaticFields[3]);
            reader.ReadByteArray(StaticFields[4], Payload.TagValue);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
            buffer = buffer.Slice(arraySize);
           
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
        
        



        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// Tag GUID.
        /// OriginName: tag_guid, Units: , IsExtended: false
        /// </summary>
        public const int TagGuidMaxItemsCount = 16;
        public byte[] TagGuid { get; } = new byte[16];
        /// <summary>
        /// Tag name, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
        /// OriginName: tag_name, Units: , IsExtended: false
        /// </summary>
        public const int TagNameMaxItemsCount = 16;
        public char[] TagName { get; } = new char[16];
        /// <summary>
        /// Tag type.
        /// OriginName: tag_type, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRecordTagType TagType { get; set; }
        /// <summary>
        /// Tag value, depends on the type of tag.
        /// OriginName: tag_value, Units: , IsExtended: false
        /// </summary>
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "target_system",
            "System ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(2,
            "target_component",
            "Component ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(3,
            "record_guid",
            "Record GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
            new(4,
            "tag_guid",
            "Tag GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_TAG_DELETE_REQUEST:"
        + "uint16_t request_id;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t[16] record_guid;"
        + "uint8_t[16] tag_guid;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], Payload.TargetSystem);
            writer.Write(StaticFields[2], Payload.TargetComponent);
            writer.Write(StaticFields[3], Payload.RecordGuid);
            writer.Write(StaticFields[4], Payload.TagGuid);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[1]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[2]);
            reader.ReadByteArray(StaticFields[3], Payload.RecordGuid);
            reader.ReadByteArray(StaticFields[4], Payload.TagGuid);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
        
        



        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
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
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// Tag GUID.
        /// OriginName: tag_guid, Units: , IsExtended: false
        /// </summary>
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "result",
            "Result code.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(2,
            "record_guid",
            "Record GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
            new(3,
            "tag_guid",
            "Tag GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_TAG_DELETE_RESPONSE:"
        + "uint16_t request_id;"
        + "uint8_t result;"
        + "uint8_t[16] record_guid;"
        + "uint8_t[16] tag_guid;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], (ulong)Payload.Result);
            writer.Write(StaticFields[2], Payload.RecordGuid);
            writer.Write(StaticFields[3], Payload.TagGuid);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.Result = (AsvSdrRequestAck)reader.ReadByte(StaticFields[1]);
            reader.ReadByteArray(StaticFields[2], Payload.RecordGuid);
            reader.ReadByteArray(StaticFields[3], Payload.TagGuid);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
        
        



        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRequestAck Result { get; set; }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// Tag GUID.
        /// OriginName: tag_guid, Units: , IsExtended: false
        /// </summary>
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "skip",
            "Specifies the start index of the tag to be sent in the response.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "count",
            "Specifies the number of tag to be sent in the response after the skip index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(2,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "target_system",
            "System ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "target_component",
            "Component ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(5,
            "record_guid",
            "Record GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_DATA_REQUEST:"
        + "uint32_t skip;"
        + "uint32_t count;"
        + "uint16_t request_id;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t[16] record_guid;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Skip);
            writer.Write(StaticFields[1], Payload.Count);
            writer.Write(StaticFields[2], Payload.RequestId);
            writer.Write(StaticFields[3], Payload.TargetSystem);
            writer.Write(StaticFields[4], Payload.TargetComponent);
            writer.Write(StaticFields[5], Payload.RecordGuid);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Skip = reader.ReadUInt(StaticFields[0]);
            Payload.Count = reader.ReadUInt(StaticFields[1]);
            Payload.RequestId = reader.ReadUShort(StaticFields[2]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[3]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[4]);
            reader.ReadByteArray(StaticFields[5], Payload.RecordGuid);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
        
        



        /// <summary>
        /// Specifies the start index of the tag to be sent in the response.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public uint Skip { get; set; }
        /// <summary>
        /// Specifies the number of tag to be sent in the response after the skip index.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public uint Count { get; set; }
        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "data_type",
            "Record data type(it is also possible to know the type of data inside the record by cast enum to int).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "items_count",
            "Number of items ASV_SDR_RECORD_DATA_* for transmition after this request with success result code (depended from request).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(2,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "result",
            "Result code.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "record_guid",
            "Record GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_DATA_RESPONSE:"
        + "uint32_t data_type;"
        + "uint32_t items_count;"
        + "uint16_t request_id;"
        + "uint8_t result;"
        + "uint8_t[16] record_guid;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], (ulong)Payload.DataType);
            writer.Write(StaticFields[1], Payload.ItemsCount);
            writer.Write(StaticFields[2], Payload.RequestId);
            writer.Write(StaticFields[3], (ulong)Payload.Result);
            writer.Write(StaticFields[4], Payload.RecordGuid);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.DataType = (AsvSdrCustomMode)reader.ReadUInt(StaticFields[0]);
            Payload.ItemsCount = reader.ReadUInt(StaticFields[1]);
            Payload.RequestId = reader.ReadUShort(StaticFields[2]);
            Payload.Result = (AsvSdrRequestAck)reader.ReadByte(StaticFields[3]);
            reader.ReadByteArray(StaticFields[4], Payload.RecordGuid);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
        
        



        /// <summary>
        /// Record data type(it is also possible to know the type of data inside the record by cast enum to int).
        /// OriginName: data_type, Units: , IsExtended: false
        /// </summary>
        public AsvSdrCustomMode DataType { get; set; }
        /// <summary>
        /// Number of items ASV_SDR_RECORD_DATA_* for transmition after this request with success result code (depended from request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public uint ItemsCount { get; set; }
        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRequestAck Result { get; set; }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "result",
            "Result code.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_CALIB_ACC:"
        + "uint16_t request_id;"
        + "uint8_t result;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], (ulong)Payload.Result);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.Result = (AsvSdrRequestAck)reader.ReadByte(StaticFields[1]);
        
            
        }
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
        
        



        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRequestAck Result { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "table_index",
            "Table index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "target_system",
            "System ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(3,
            "target_component",
            "Component ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_CALIB_TABLE_READ:"
        + "uint16_t table_index;"
        + "uint16_t request_id;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TableIndex);
            writer.Write(StaticFields[1], Payload.RequestId);
            writer.Write(StaticFields[2], Payload.TargetSystem);
            writer.Write(StaticFields[3], Payload.TargetComponent);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TableIndex = reader.ReadUShort(StaticFields[0]);
            Payload.RequestId = reader.ReadUShort(StaticFields[1]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[2]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[3]);
        
            
        }
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
        
        



        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public ushort TableIndex { get; set; }
        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "created_unix_us",
            "Updated timestamp (UNIX epoch time).",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(1,
            "table_index",
            "Table index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "row_count",
            "Specifies the number of ROWs in the table.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "table_name",
            "Table name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            28, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_CALIB_TABLE:"
        + "uint64_t created_unix_us;"
        + "uint16_t table_index;"
        + "uint16_t row_count;"
        + "char[28] table_name;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.CreatedUnixUs);
            writer.Write(StaticFields[1], Payload.TableIndex);
            writer.Write(StaticFields[2], Payload.RowCount);
            writer.Write(StaticFields[3], Payload.TableName);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.CreatedUnixUs = reader.ReadULong(StaticFields[0]);
            Payload.TableIndex = reader.ReadUShort(StaticFields[1]);
            Payload.RowCount = reader.ReadUShort(StaticFields[2]);
            reader.ReadCharArray(StaticFields[3], Payload.TableName);
        
            
        }
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
            TableName = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = TableName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, TableName.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           

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
        
        



        /// <summary>
        /// Updated timestamp (UNIX epoch time).
        /// OriginName: created_unix_us, Units: us, IsExtended: false
        /// </summary>
        public ulong CreatedUnixUs { get; set; }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public ushort TableIndex { get; set; }
        /// <summary>
        /// Specifies the number of ROWs in the table.
        /// OriginName: row_count, Units: , IsExtended: false
        /// </summary>
        public ushort RowCount { get; set; }
        /// <summary>
        /// Table name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.
        /// OriginName: table_name, Units: , IsExtended: false
        /// </summary>
        public const int TableNameMaxItemsCount = 28;
        public char[] TableName { get; set; } = new char[28];
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "table_index",
            "Table index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "row_index",
            "ROW index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "target_system",
            "System ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "target_component",
            "Component ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_CALIB_TABLE_ROW_READ:"
        + "uint16_t request_id;"
        + "uint16_t table_index;"
        + "uint16_t row_index;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], Payload.TableIndex);
            writer.Write(StaticFields[2], Payload.RowIndex);
            writer.Write(StaticFields[3], Payload.TargetSystem);
            writer.Write(StaticFields[4], Payload.TargetComponent);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.TableIndex = reader.ReadUShort(StaticFields[1]);
            Payload.RowIndex = reader.ReadUShort(StaticFields[2]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[3]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[4]);
        
            
        }
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
        
        



        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public ushort TableIndex { get; set; }
        /// <summary>
        /// ROW index.
        /// OriginName: row_index, Units: , IsExtended: false
        /// </summary>
        public ushort RowIndex { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "ref_freq",
            "Reference frequency in Hz.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(1,
            "ref_power",
            "Reference power in dBm.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(2,
            "ref_value",
            "Reference value.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(3,
            "adjustment",
            "Adjustment for measured value (ref_value = measured_value + adjustment)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(4,
            "table_index",
            "Table index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(5,
            "row_index",
            "ROW index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(6,
            "target_system",
            "System ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(7,
            "target_component",
            "Component ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_CALIB_TABLE_ROW:"
        + "uint64_t ref_freq;"
        + "float ref_power;"
        + "float ref_value;"
        + "float adjustment;"
        + "uint16_t table_index;"
        + "uint16_t row_index;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RefFreq);
            writer.Write(StaticFields[1], Payload.RefPower);
            writer.Write(StaticFields[2], Payload.RefValue);
            writer.Write(StaticFields[3], Payload.Adjustment);
            writer.Write(StaticFields[4], Payload.TableIndex);
            writer.Write(StaticFields[5], Payload.RowIndex);
            writer.Write(StaticFields[6], Payload.TargetSystem);
            writer.Write(StaticFields[7], Payload.TargetComponent);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RefFreq = reader.ReadULong(StaticFields[0]);
            Payload.RefPower = reader.ReadFloat(StaticFields[1]);
            Payload.RefValue = reader.ReadFloat(StaticFields[2]);
            Payload.Adjustment = reader.ReadFloat(StaticFields[3]);
            Payload.TableIndex = reader.ReadUShort(StaticFields[4]);
            Payload.RowIndex = reader.ReadUShort(StaticFields[5]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[6]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[7]);
        
            
        }
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
        
        



        /// <summary>
        /// Reference frequency in Hz.
        /// OriginName: ref_freq, Units: , IsExtended: false
        /// </summary>
        public ulong RefFreq { get; set; }
        /// <summary>
        /// Reference power in dBm.
        /// OriginName: ref_power, Units: , IsExtended: false
        /// </summary>
        public float RefPower { get; set; }
        /// <summary>
        /// Reference value.
        /// OriginName: ref_value, Units: , IsExtended: false
        /// </summary>
        public float RefValue { get; set; }
        /// <summary>
        /// Adjustment for measured value (ref_value = measured_value + adjustment)
        /// OriginName: adjustment, Units: , IsExtended: false
        /// </summary>
        public float Adjustment { get; set; }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public ushort TableIndex { get; set; }
        /// <summary>
        /// ROW index.
        /// OriginName: row_index, Units: , IsExtended: false
        /// </summary>
        public ushort RowIndex { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "created_unix_us",
            "Current timestamp (UNIX epoch time).",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(1,
            "table_index",
            "Table index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "request_id",
            "Specifies the unique number of the original request. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "row_count",
            "Specifies the number of ROWs in the table.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(4,
            "target_system",
            "System ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(5,
            "target_component",
            "Component ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_CALIB_TABLE_UPLOAD_START:"
        + "uint64_t created_unix_us;"
        + "uint16_t table_index;"
        + "uint16_t request_id;"
        + "uint16_t row_count;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.CreatedUnixUs);
            writer.Write(StaticFields[1], Payload.TableIndex);
            writer.Write(StaticFields[2], Payload.RequestId);
            writer.Write(StaticFields[3], Payload.RowCount);
            writer.Write(StaticFields[4], Payload.TargetSystem);
            writer.Write(StaticFields[5], Payload.TargetComponent);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.CreatedUnixUs = reader.ReadULong(StaticFields[0]);
            Payload.TableIndex = reader.ReadUShort(StaticFields[1]);
            Payload.RequestId = reader.ReadUShort(StaticFields[2]);
            Payload.RowCount = reader.ReadUShort(StaticFields[3]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[4]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[5]);
        
            
        }
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
        
        



        /// <summary>
        /// Current timestamp (UNIX epoch time).
        /// OriginName: created_unix_us, Units: us, IsExtended: false
        /// </summary>
        public ulong CreatedUnixUs { get; set; }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public ushort TableIndex { get; set; }
        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Specifies the number of ROWs in the table.
        /// OriginName: row_count, Units: , IsExtended: false
        /// </summary>
        public ushort RowCount { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "request_id",
            "Specifies the unique number of the original request from ASV_SDR_CALIB_TABLE_UPLOAD_START. This allows the response to be matched to the correct request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "table_index",
            "Table index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "row_index",
            "ROW index.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "target_system",
            "System ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "target_component",
            "Component ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_CALIB_TABLE_UPLOAD_READ_CALLBACK:"
        + "uint16_t request_id;"
        + "uint16_t table_index;"
        + "uint16_t row_index;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], Payload.TableIndex);
            writer.Write(StaticFields[2], Payload.RowIndex);
            writer.Write(StaticFields[3], Payload.TargetSystem);
            writer.Write(StaticFields[4], Payload.TargetComponent);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.TableIndex = reader.ReadUShort(StaticFields[1]);
            Payload.RowIndex = reader.ReadUShort(StaticFields[2]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[3]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[4]);
        
            
        }
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
        
        



        /// <summary>
        /// Specifies the unique number of the original request from ASV_SDR_CALIB_TABLE_UPLOAD_START. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public ushort TableIndex { get; set; }
        /// <summary>
        /// ROW index.
        /// OriginName: row_index, Units: , IsExtended: false
        /// </summary>
        public ushort RowIndex { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "time_unix_usec",
            "Timestamp (UNIX epoch time) for current set of measures.",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(1,
            "min",
            "Min value of set.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(2,
            "max",
            "Max value of set.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(3,
            "start",
            "Start index of measure set.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(4,
            "total",
            "Total points in set.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(5,
            "signal_name",
            "Signal name, terminated by NULL if the length is less than 8 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 8+1 bytes storage if the ID is stored as string",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            8, 
            false),
            new(6,
            "format",
            "Format of one measure.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(7,
            "count",
            "Measures count in this packet.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(8,
            "data",
            "Data set of points.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            200, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_SIGNAL_RAW:"
        + "uint64_t time_unix_usec;"
        + "float min;"
        + "float max;"
        + "uint16_t start;"
        + "uint16_t total;"
        + "char[8] signal_name;"
        + "uint8_t format;"
        + "uint8_t count;"
        + "uint8_t[200] data;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TimeUnixUsec);
            writer.Write(StaticFields[1], Payload.Min);
            writer.Write(StaticFields[2], Payload.Max);
            writer.Write(StaticFields[3], Payload.Start);
            writer.Write(StaticFields[4], Payload.Total);
            writer.Write(StaticFields[5], Payload.SignalName);
            writer.Write(StaticFields[6], (ulong)Payload.Format);
            writer.Write(StaticFields[7], Payload.Count);
            writer.Write(StaticFields[8], Payload.Data);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TimeUnixUsec = reader.ReadULong(StaticFields[0]);
            Payload.Min = reader.ReadFloat(StaticFields[1]);
            Payload.Max = reader.ReadFloat(StaticFields[2]);
            Payload.Start = reader.ReadUShort(StaticFields[3]);
            Payload.Total = reader.ReadUShort(StaticFields[4]);
            reader.ReadCharArray(StaticFields[5], Payload.SignalName);
            Payload.Format = (AsvSdrSignalFormat)reader.ReadByte(StaticFields[6]);
            Payload.Count = reader.ReadByte(StaticFields[7]);
            reader.ReadByteArray(StaticFields[8], Payload.Data);
        
            
        }
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
            buffer = buffer.Slice(arraySize);
           
            Format = (AsvSdrSignalFormat)BinSerialize.ReadByte(ref buffer);
            Count = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/200 - Math.Max(0,((/*PayloadByteSize*/230 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time) for current set of measures.
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Min value of set.
        /// OriginName: min, Units: , IsExtended: false
        /// </summary>
        public float Min { get; set; }
        /// <summary>
        /// Max value of set.
        /// OriginName: max, Units: , IsExtended: false
        /// </summary>
        public float Max { get; set; }
        /// <summary>
        /// Start index of measure set.
        /// OriginName: start, Units: , IsExtended: false
        /// </summary>
        public ushort Start { get; set; }
        /// <summary>
        /// Total points in set.
        /// OriginName: total, Units: , IsExtended: false
        /// </summary>
        public ushort Total { get; set; }
        /// <summary>
        /// Signal name, terminated by NULL if the length is less than 8 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 8+1 bytes storage if the ID is stored as string
        /// OriginName: signal_name, Units: , IsExtended: false
        /// </summary>
        public const int SignalNameMaxItemsCount = 8;
        public char[] SignalName { get; } = new char[8];
        /// <summary>
        /// Format of one measure.
        /// OriginName: format, Units: , IsExtended: false
        /// </summary>
        public AsvSdrSignalFormat Format { get; set; }
        /// <summary>
        /// Measures count in this packet.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public byte Count { get; set; }
        /// <summary>
        /// Data set of points.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public const int DataMaxItemsCount = 200;
        public byte[] Data { get; set; } = new byte[200];
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "time_unix_usec",
            "Timestamp (UNIX epoch time).",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(1,
            "total_freq",
            "Measured frequency.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(2,
            "data_index",
            "Data index in record",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(3,
            "gnss_lat",
            "Latitude (WGS84, EGM96 ellipsoid)",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(4,
            "gnss_lon",
            "Longitude (WGS84, EGM96 ellipsoid)",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(5,
            "gnss_alt",
            "Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(6,
            "gnss_alt_ellipsoid",
            "Altitude (above WGS84, EGM96 ellipsoid). Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(7,
            "gnss_h_acc",
            "Position uncertainty. Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(8,
            "gnss_v_acc",
            "Altitude uncertainty. Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(9,
            "gnss_vel_acc",
            "Speed uncertainty. Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(10,
            "lat",
            "Filtered global position latitude, expressed",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(11,
            "lon",
            "Filtered global position longitude, expressed",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(12,
            "alt",
            "Filtered global position altitude (MSL).",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(13,
            "relative_alt",
            "Altitude above ground",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(14,
            "roll",
            "Roll angle (-pi..+pi)",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(15,
            "pitch",
            "Pitch angle (-pi..+pi)",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(16,
            "yaw",
            "Yaw angle (-pi..+pi)",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(17,
            "crs_power",
            "Input power of course.",
            string.Empty, 
            @"dBm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(18,
            "crs_am_90",
            "Aplitude modulation of 90Hz of course.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(19,
            "crs_am_150",
            "Aplitude modulation of 150Hz of course.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(20,
            "clr_power",
            "Input power of clearance.",
            string.Empty, 
            @"dBm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(21,
            "clr_am_90",
            "Aplitude modulation of 90Hz of clearance.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(22,
            "clr_am_150",
            "Aplitude modulation of 150Hz of clearance.",
            string.Empty, 
            @"% E2", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(23,
            "total_power",
            "Total input power.",
            string.Empty, 
            @"dBm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(24,
            "total_field_strength",
            "Total field strength.",
            string.Empty, 
            @"uV/m", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(25,
            "total_am_90",
            "Total aplitude modulation of 90Hz.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(26,
            "total_am_150",
            "Total aplitude modulation of 150Hz.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(27,
            "phi_90_crs_vs_clr",
            " Phase difference 90 Hz clearance and cource",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(28,
            "phi_150_crs_vs_clr",
            "Phase difference 150 Hz clearance and cource.",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(29,
            "code_id_am_1020",
            "Total aplitude modulation of 90Hz.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(30,
            "gnss_eph",
            "GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(31,
            "gnss_epv",
            "GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(32,
            "gnss_vel",
            "GPS ground speed. If unknown, set to: UINT16_MAX",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(33,
            "vx",
            "Ground X Speed (Latitude, positive north)",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(34,
            "vy",
            "Ground Y Speed (Longitude, positive east)",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(35,
            "vz",
            "Ground Z Speed (Altitude, positive down)",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(36,
            "hdg",
            "Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX",
            string.Empty, 
            @"cdeg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(37,
            "crs_carrier_offset",
            "Carrier frequency offset of course.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(38,
            "crs_freq_90",
            "Frequency offset of signal 90 Hz of course.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(39,
            "crs_freq_150",
            "Frequency offset of signal 150 Hz of course.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(40,
            "clr_carrier_offset",
            "Carrier frequency offset of clearance.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(41,
            "clr_freq_90",
            "Frequency offset of signal 90 Hz of clearance.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(42,
            "clr_freq_150",
            "Frequency offset of signal 150 Hz of clearance.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(43,
            "total_carrier_offset",
            "Total carrier frequency offset.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(44,
            "total_freq_90",
            "Total frequency offset of signal 90 Hz.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(45,
            "total_freq_150",
            "Total frequency offset of signal 150 Hz.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(46,
            "code_id_freq_1020",
            "Total frequency offset of signal 90 Hz.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(47,
            "measure_time",
            "Measure time.",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(48,
            "record_guid",
            "Record GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
            new(49,
            "gnss_fix_type",
            "GPS fix type.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(50,
            "gnss_satellites_visible",
            "Number of satellites visible. If unknown, set to 255",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(51,
            "code_id",
            "Code identification",
            string.Empty, 
            @"Letters", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            4, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_DATA_LLZ:"
        + "uint64_t time_unix_usec;"
        + "uint64_t total_freq;"
        + "uint32_t data_index;"
        + "int32_t gnss_lat;"
        + "int32_t gnss_lon;"
        + "int32_t gnss_alt;"
        + "int32_t gnss_alt_ellipsoid;"
        + "uint32_t gnss_h_acc;"
        + "uint32_t gnss_v_acc;"
        + "uint32_t gnss_vel_acc;"
        + "int32_t lat;"
        + "int32_t lon;"
        + "int32_t alt;"
        + "int32_t relative_alt;"
        + "float roll;"
        + "float pitch;"
        + "float yaw;"
        + "float crs_power;"
        + "float crs_am_90;"
        + "float crs_am_150;"
        + "float clr_power;"
        + "float clr_am_90;"
        + "float clr_am_150;"
        + "float total_power;"
        + "float total_field_strength;"
        + "float total_am_90;"
        + "float total_am_150;"
        + "float phi_90_crs_vs_clr;"
        + "float phi_150_crs_vs_clr;"
        + "float code_id_am_1020;"
        + "uint16_t gnss_eph;"
        + "uint16_t gnss_epv;"
        + "uint16_t gnss_vel;"
        + "int16_t vx;"
        + "int16_t vy;"
        + "int16_t vz;"
        + "uint16_t hdg;"
        + "int16_t crs_carrier_offset;"
        + "int16_t crs_freq_90;"
        + "int16_t crs_freq_150;"
        + "int16_t clr_carrier_offset;"
        + "int16_t clr_freq_90;"
        + "int16_t clr_freq_150;"
        + "int16_t total_carrier_offset;"
        + "int16_t total_freq_90;"
        + "int16_t total_freq_150;"
        + "int16_t code_id_freq_1020;"
        + "int16_t measure_time;"
        + "uint8_t[16] record_guid;"
        + "uint8_t gnss_fix_type;"
        + "uint8_t gnss_satellites_visible;"
        + "char[4] code_id;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TimeUnixUsec);
            writer.Write(StaticFields[1], Payload.TotalFreq);
            writer.Write(StaticFields[2], Payload.DataIndex);
            writer.Write(StaticFields[3], Payload.GnssLat);
            writer.Write(StaticFields[4], Payload.GnssLon);
            writer.Write(StaticFields[5], Payload.GnssAlt);
            writer.Write(StaticFields[6], Payload.GnssAltEllipsoid);
            writer.Write(StaticFields[7], Payload.GnssHAcc);
            writer.Write(StaticFields[8], Payload.GnssVAcc);
            writer.Write(StaticFields[9], Payload.GnssVelAcc);
            writer.Write(StaticFields[10], Payload.Lat);
            writer.Write(StaticFields[11], Payload.Lon);
            writer.Write(StaticFields[12], Payload.Alt);
            writer.Write(StaticFields[13], Payload.RelativeAlt);
            writer.Write(StaticFields[14], Payload.Roll);
            writer.Write(StaticFields[15], Payload.Pitch);
            writer.Write(StaticFields[16], Payload.Yaw);
            writer.Write(StaticFields[17], Payload.CrsPower);
            writer.Write(StaticFields[18], Payload.CrsAm90);
            writer.Write(StaticFields[19], Payload.CrsAm150);
            writer.Write(StaticFields[20], Payload.ClrPower);
            writer.Write(StaticFields[21], Payload.ClrAm90);
            writer.Write(StaticFields[22], Payload.ClrAm150);
            writer.Write(StaticFields[23], Payload.TotalPower);
            writer.Write(StaticFields[24], Payload.TotalFieldStrength);
            writer.Write(StaticFields[25], Payload.TotalAm90);
            writer.Write(StaticFields[26], Payload.TotalAm150);
            writer.Write(StaticFields[27], Payload.Phi90CrsVsClr);
            writer.Write(StaticFields[28], Payload.Phi150CrsVsClr);
            writer.Write(StaticFields[29], Payload.CodeIdAm1020);
            writer.Write(StaticFields[30], Payload.GnssEph);
            writer.Write(StaticFields[31], Payload.GnssEpv);
            writer.Write(StaticFields[32], Payload.GnssVel);
            writer.Write(StaticFields[33], Payload.Vx);
            writer.Write(StaticFields[34], Payload.Vy);
            writer.Write(StaticFields[35], Payload.Vz);
            writer.Write(StaticFields[36], Payload.Hdg);
            writer.Write(StaticFields[37], Payload.CrsCarrierOffset);
            writer.Write(StaticFields[38], Payload.CrsFreq90);
            writer.Write(StaticFields[39], Payload.CrsFreq150);
            writer.Write(StaticFields[40], Payload.ClrCarrierOffset);
            writer.Write(StaticFields[41], Payload.ClrFreq90);
            writer.Write(StaticFields[42], Payload.ClrFreq150);
            writer.Write(StaticFields[43], Payload.TotalCarrierOffset);
            writer.Write(StaticFields[44], Payload.TotalFreq90);
            writer.Write(StaticFields[45], Payload.TotalFreq150);
            writer.Write(StaticFields[46], Payload.CodeIdFreq1020);
            writer.Write(StaticFields[47], Payload.MeasureTime);
            writer.Write(StaticFields[48], Payload.RecordGuid);
            writer.Write(StaticFields[49], (ulong)Payload.GnssFixType);
            writer.Write(StaticFields[50], Payload.GnssSatellitesVisible);
            writer.Write(StaticFields[51], Payload.CodeId);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TimeUnixUsec = reader.ReadULong(StaticFields[0]);
            Payload.TotalFreq = reader.ReadULong(StaticFields[1]);
            Payload.DataIndex = reader.ReadUInt(StaticFields[2]);
            Payload.GnssLat = reader.ReadInt(StaticFields[3]);
            Payload.GnssLon = reader.ReadInt(StaticFields[4]);
            Payload.GnssAlt = reader.ReadInt(StaticFields[5]);
            Payload.GnssAltEllipsoid = reader.ReadInt(StaticFields[6]);
            Payload.GnssHAcc = reader.ReadUInt(StaticFields[7]);
            Payload.GnssVAcc = reader.ReadUInt(StaticFields[8]);
            Payload.GnssVelAcc = reader.ReadUInt(StaticFields[9]);
            Payload.Lat = reader.ReadInt(StaticFields[10]);
            Payload.Lon = reader.ReadInt(StaticFields[11]);
            Payload.Alt = reader.ReadInt(StaticFields[12]);
            Payload.RelativeAlt = reader.ReadInt(StaticFields[13]);
            Payload.Roll = reader.ReadFloat(StaticFields[14]);
            Payload.Pitch = reader.ReadFloat(StaticFields[15]);
            Payload.Yaw = reader.ReadFloat(StaticFields[16]);
            Payload.CrsPower = reader.ReadFloat(StaticFields[17]);
            Payload.CrsAm90 = reader.ReadFloat(StaticFields[18]);
            Payload.CrsAm150 = reader.ReadFloat(StaticFields[19]);
            Payload.ClrPower = reader.ReadFloat(StaticFields[20]);
            Payload.ClrAm90 = reader.ReadFloat(StaticFields[21]);
            Payload.ClrAm150 = reader.ReadFloat(StaticFields[22]);
            Payload.TotalPower = reader.ReadFloat(StaticFields[23]);
            Payload.TotalFieldStrength = reader.ReadFloat(StaticFields[24]);
            Payload.TotalAm90 = reader.ReadFloat(StaticFields[25]);
            Payload.TotalAm150 = reader.ReadFloat(StaticFields[26]);
            Payload.Phi90CrsVsClr = reader.ReadFloat(StaticFields[27]);
            Payload.Phi150CrsVsClr = reader.ReadFloat(StaticFields[28]);
            Payload.CodeIdAm1020 = reader.ReadFloat(StaticFields[29]);
            Payload.GnssEph = reader.ReadUShort(StaticFields[30]);
            Payload.GnssEpv = reader.ReadUShort(StaticFields[31]);
            Payload.GnssVel = reader.ReadUShort(StaticFields[32]);
            Payload.Vx = reader.ReadShort(StaticFields[33]);
            Payload.Vy = reader.ReadShort(StaticFields[34]);
            Payload.Vz = reader.ReadShort(StaticFields[35]);
            Payload.Hdg = reader.ReadUShort(StaticFields[36]);
            Payload.CrsCarrierOffset = reader.ReadShort(StaticFields[37]);
            Payload.CrsFreq90 = reader.ReadShort(StaticFields[38]);
            Payload.CrsFreq150 = reader.ReadShort(StaticFields[39]);
            Payload.ClrCarrierOffset = reader.ReadShort(StaticFields[40]);
            Payload.ClrFreq90 = reader.ReadShort(StaticFields[41]);
            Payload.ClrFreq150 = reader.ReadShort(StaticFields[42]);
            Payload.TotalCarrierOffset = reader.ReadShort(StaticFields[43]);
            Payload.TotalFreq90 = reader.ReadShort(StaticFields[44]);
            Payload.TotalFreq150 = reader.ReadShort(StaticFields[45]);
            Payload.CodeIdFreq1020 = reader.ReadShort(StaticFields[46]);
            Payload.MeasureTime = reader.ReadShort(StaticFields[47]);
            reader.ReadByteArray(StaticFields[48], Payload.RecordGuid);
            Payload.GnssFixType = (GpsFixType)reader.ReadByte(StaticFields[49]);
            Payload.GnssSatellitesVisible = reader.ReadByte(StaticFields[50]);
            reader.ReadCharArray(StaticFields[51], Payload.CodeId);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
            buffer = buffer.Slice(arraySize);
           

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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public ulong TotalFreq { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: data_index, Units: , IsExtended: false
        /// </summary>
        public uint DataIndex { get; set; }
        /// <summary>
        /// Latitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public int GnssLat { get; set; }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public int GnssLon { get; set; }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public int GnssAlt { get; set; }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public int GnssAltEllipsoid { get; set; }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssHAcc { get; set; }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssVAcc { get; set; }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssVelAcc { get; set; }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public int Lon { get; set; }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public int Alt { get; set; }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public int RelativeAlt { get; set; }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Input power of course.
        /// OriginName: crs_power, Units: dBm, IsExtended: false
        /// </summary>
        public float CrsPower { get; set; }
        /// <summary>
        /// Aplitude modulation of 90Hz of course.
        /// OriginName: crs_am_90, Units: %, IsExtended: false
        /// </summary>
        public float CrsAm90 { get; set; }
        /// <summary>
        /// Aplitude modulation of 150Hz of course.
        /// OriginName: crs_am_150, Units: %, IsExtended: false
        /// </summary>
        public float CrsAm150 { get; set; }
        /// <summary>
        /// Input power of clearance.
        /// OriginName: clr_power, Units: dBm, IsExtended: false
        /// </summary>
        public float ClrPower { get; set; }
        /// <summary>
        /// Aplitude modulation of 90Hz of clearance.
        /// OriginName: clr_am_90, Units: %, IsExtended: false
        /// </summary>
        public float ClrAm90 { get; set; }
        /// <summary>
        /// Aplitude modulation of 150Hz of clearance.
        /// OriginName: clr_am_150, Units: % E2, IsExtended: false
        /// </summary>
        public float ClrAm150 { get; set; }
        /// <summary>
        /// Total input power.
        /// OriginName: total_power, Units: dBm, IsExtended: false
        /// </summary>
        public float TotalPower { get; set; }
        /// <summary>
        /// Total field strength.
        /// OriginName: total_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public float TotalFieldStrength { get; set; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: total_am_90, Units: %, IsExtended: false
        /// </summary>
        public float TotalAm90 { get; set; }
        /// <summary>
        /// Total aplitude modulation of 150Hz.
        /// OriginName: total_am_150, Units: %, IsExtended: false
        /// </summary>
        public float TotalAm150 { get; set; }
        /// <summary>
        ///  Phase difference 90 Hz clearance and cource
        /// OriginName: phi_90_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public float Phi90CrsVsClr { get; set; }
        /// <summary>
        /// Phase difference 150 Hz clearance and cource.
        /// OriginName: phi_150_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public float Phi150CrsVsClr { get; set; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: code_id_am_1020, Units: %, IsExtended: false
        /// </summary>
        public float CodeIdAm1020 { get; set; }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public ushort GnssEph { get; set; }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public ushort GnssEpv { get; set; }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public ushort GnssVel { get; set; }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vx { get; set; }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vy { get; set; }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vz { get; set; }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public ushort Hdg { get; set; }
        /// <summary>
        /// Carrier frequency offset of course.
        /// OriginName: crs_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short CrsCarrierOffset { get; set; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of course.
        /// OriginName: crs_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public short CrsFreq90 { get; set; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of course.
        /// OriginName: crs_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public short CrsFreq150 { get; set; }
        /// <summary>
        /// Carrier frequency offset of clearance.
        /// OriginName: clr_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short ClrCarrierOffset { get; set; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of clearance.
        /// OriginName: clr_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public short ClrFreq90 { get; set; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of clearance.
        /// OriginName: clr_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public short ClrFreq150 { get; set; }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: total_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short TotalCarrierOffset { get; set; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: total_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public short TotalFreq90 { get; set; }
        /// <summary>
        /// Total frequency offset of signal 150 Hz.
        /// OriginName: total_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public short TotalFreq150 { get; set; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: code_id_freq_1020, Units: Hz, IsExtended: false
        /// </summary>
        public short CodeIdFreq1020 { get; set; }
        /// <summary>
        /// Measure time.
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public short MeasureTime { get; set; }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// GPS fix type.
        /// OriginName: gnss_fix_type, Units: , IsExtended: false
        /// </summary>
        public GpsFixType GnssFixType { get; set; }
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public byte GnssSatellitesVisible { get; set; }
        /// <summary>
        /// Code identification
        /// OriginName: code_id, Units: Letters, IsExtended: false
        /// </summary>
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "time_unix_usec",
            "Timestamp (UNIX epoch time).",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(1,
            "total_freq",
            "Measured frequency.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(2,
            "data_index",
            "Data index in record",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(3,
            "gnss_lat",
            "Latitude (WGS84, EGM96 ellipsoid)",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(4,
            "gnss_lon",
            "Longitude (WGS84, EGM96 ellipsoid)",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(5,
            "gnss_alt",
            "Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(6,
            "gnss_alt_ellipsoid",
            "Altitude (above WGS84, EGM96 ellipsoid). Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(7,
            "gnss_h_acc",
            "Position uncertainty. Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(8,
            "gnss_v_acc",
            "Altitude uncertainty. Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(9,
            "gnss_vel_acc",
            "Speed uncertainty. Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(10,
            "lat",
            "Filtered global position latitude, expressed",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(11,
            "lon",
            "Filtered global position longitude, expressed",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(12,
            "alt",
            "Filtered global position altitude (MSL).",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(13,
            "relative_alt",
            "Altitude above ground",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(14,
            "roll",
            "Roll angle (-pi..+pi)",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(15,
            "pitch",
            "Pitch angle (-pi..+pi)",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(16,
            "yaw",
            "Yaw angle (-pi..+pi)",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(17,
            "crs_power",
            "Input power of course.",
            string.Empty, 
            @"dBm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(18,
            "crs_am_90",
            "Aplitude modulation of 90Hz of course.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(19,
            "crs_am_150",
            "Aplitude modulation of 150Hz of course.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(20,
            "clr_power",
            "Input power of clearance.",
            string.Empty, 
            @"dBm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(21,
            "clr_am_90",
            "Aplitude modulation of 90Hz of clearance.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(22,
            "clr_am_150",
            "Aplitude modulation of 150Hz of clearance.",
            string.Empty, 
            @"% E2", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(23,
            "total_power",
            "Total input power.",
            string.Empty, 
            @"dBm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(24,
            "total_field_strength",
            "Total field strength.",
            string.Empty, 
            @"uV/m", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(25,
            "total_am_90",
            "Total aplitude modulation of 90Hz.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(26,
            "total_am_150",
            "Total aplitude modulation of 150Hz.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(27,
            "phi_90_crs_vs_clr",
            " Phase difference 90 Hz clearance and cource",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(28,
            "phi_150_crs_vs_clr",
            "Phase difference 150 Hz clearance and cource.",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(29,
            "gnss_eph",
            "GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(30,
            "gnss_epv",
            "GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(31,
            "gnss_vel",
            "GPS ground speed. If unknown, set to: UINT16_MAX",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(32,
            "vx",
            "Ground X Speed (Latitude, positive north)",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(33,
            "vy",
            "Ground Y Speed (Longitude, positive east)",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(34,
            "vz",
            "Ground Z Speed (Altitude, positive down)",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(35,
            "hdg",
            "Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX",
            string.Empty, 
            @"cdeg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(36,
            "crs_carrier_offset",
            "Carrier frequency offset of course.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(37,
            "crs_freq_90",
            "Frequency offset of signal 90 Hz of course.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(38,
            "crs_freq_150",
            "Frequency offset of signal 150 Hz of course.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(39,
            "clr_carrier_offset",
            "Carrier frequency offset of clearance.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(40,
            "clr_freq_90",
            "Frequency offset of signal 90 Hz of clearance.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(41,
            "clr_freq_150",
            "Frequency offset of signal 150 Hz of clearance.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(42,
            "total_carrier_offset",
            "Total carrier frequency offset.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(43,
            "total_freq_90",
            "Total frequency offset of signal 90 Hz.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(44,
            "total_freq_150",
            "Total frequency offset of signal 150 Hz.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(45,
            "measure_time",
            "Measure time.",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(46,
            "record_guid",
            "Record GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
            new(47,
            "gnss_fix_type",
            "GPS fix type.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(48,
            "gnss_satellites_visible",
            "Number of satellites visible. If unknown, set to 255",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_DATA_GP:"
        + "uint64_t time_unix_usec;"
        + "uint64_t total_freq;"
        + "uint32_t data_index;"
        + "int32_t gnss_lat;"
        + "int32_t gnss_lon;"
        + "int32_t gnss_alt;"
        + "int32_t gnss_alt_ellipsoid;"
        + "uint32_t gnss_h_acc;"
        + "uint32_t gnss_v_acc;"
        + "uint32_t gnss_vel_acc;"
        + "int32_t lat;"
        + "int32_t lon;"
        + "int32_t alt;"
        + "int32_t relative_alt;"
        + "float roll;"
        + "float pitch;"
        + "float yaw;"
        + "float crs_power;"
        + "float crs_am_90;"
        + "float crs_am_150;"
        + "float clr_power;"
        + "float clr_am_90;"
        + "float clr_am_150;"
        + "float total_power;"
        + "float total_field_strength;"
        + "float total_am_90;"
        + "float total_am_150;"
        + "float phi_90_crs_vs_clr;"
        + "float phi_150_crs_vs_clr;"
        + "uint16_t gnss_eph;"
        + "uint16_t gnss_epv;"
        + "uint16_t gnss_vel;"
        + "int16_t vx;"
        + "int16_t vy;"
        + "int16_t vz;"
        + "uint16_t hdg;"
        + "int16_t crs_carrier_offset;"
        + "int16_t crs_freq_90;"
        + "int16_t crs_freq_150;"
        + "int16_t clr_carrier_offset;"
        + "int16_t clr_freq_90;"
        + "int16_t clr_freq_150;"
        + "int16_t total_carrier_offset;"
        + "int16_t total_freq_90;"
        + "int16_t total_freq_150;"
        + "int16_t measure_time;"
        + "uint8_t[16] record_guid;"
        + "uint8_t gnss_fix_type;"
        + "uint8_t gnss_satellites_visible;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TimeUnixUsec);
            writer.Write(StaticFields[1], Payload.TotalFreq);
            writer.Write(StaticFields[2], Payload.DataIndex);
            writer.Write(StaticFields[3], Payload.GnssLat);
            writer.Write(StaticFields[4], Payload.GnssLon);
            writer.Write(StaticFields[5], Payload.GnssAlt);
            writer.Write(StaticFields[6], Payload.GnssAltEllipsoid);
            writer.Write(StaticFields[7], Payload.GnssHAcc);
            writer.Write(StaticFields[8], Payload.GnssVAcc);
            writer.Write(StaticFields[9], Payload.GnssVelAcc);
            writer.Write(StaticFields[10], Payload.Lat);
            writer.Write(StaticFields[11], Payload.Lon);
            writer.Write(StaticFields[12], Payload.Alt);
            writer.Write(StaticFields[13], Payload.RelativeAlt);
            writer.Write(StaticFields[14], Payload.Roll);
            writer.Write(StaticFields[15], Payload.Pitch);
            writer.Write(StaticFields[16], Payload.Yaw);
            writer.Write(StaticFields[17], Payload.CrsPower);
            writer.Write(StaticFields[18], Payload.CrsAm90);
            writer.Write(StaticFields[19], Payload.CrsAm150);
            writer.Write(StaticFields[20], Payload.ClrPower);
            writer.Write(StaticFields[21], Payload.ClrAm90);
            writer.Write(StaticFields[22], Payload.ClrAm150);
            writer.Write(StaticFields[23], Payload.TotalPower);
            writer.Write(StaticFields[24], Payload.TotalFieldStrength);
            writer.Write(StaticFields[25], Payload.TotalAm90);
            writer.Write(StaticFields[26], Payload.TotalAm150);
            writer.Write(StaticFields[27], Payload.Phi90CrsVsClr);
            writer.Write(StaticFields[28], Payload.Phi150CrsVsClr);
            writer.Write(StaticFields[29], Payload.GnssEph);
            writer.Write(StaticFields[30], Payload.GnssEpv);
            writer.Write(StaticFields[31], Payload.GnssVel);
            writer.Write(StaticFields[32], Payload.Vx);
            writer.Write(StaticFields[33], Payload.Vy);
            writer.Write(StaticFields[34], Payload.Vz);
            writer.Write(StaticFields[35], Payload.Hdg);
            writer.Write(StaticFields[36], Payload.CrsCarrierOffset);
            writer.Write(StaticFields[37], Payload.CrsFreq90);
            writer.Write(StaticFields[38], Payload.CrsFreq150);
            writer.Write(StaticFields[39], Payload.ClrCarrierOffset);
            writer.Write(StaticFields[40], Payload.ClrFreq90);
            writer.Write(StaticFields[41], Payload.ClrFreq150);
            writer.Write(StaticFields[42], Payload.TotalCarrierOffset);
            writer.Write(StaticFields[43], Payload.TotalFreq90);
            writer.Write(StaticFields[44], Payload.TotalFreq150);
            writer.Write(StaticFields[45], Payload.MeasureTime);
            writer.Write(StaticFields[46], Payload.RecordGuid);
            writer.Write(StaticFields[47], (ulong)Payload.GnssFixType);
            writer.Write(StaticFields[48], Payload.GnssSatellitesVisible);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TimeUnixUsec = reader.ReadULong(StaticFields[0]);
            Payload.TotalFreq = reader.ReadULong(StaticFields[1]);
            Payload.DataIndex = reader.ReadUInt(StaticFields[2]);
            Payload.GnssLat = reader.ReadInt(StaticFields[3]);
            Payload.GnssLon = reader.ReadInt(StaticFields[4]);
            Payload.GnssAlt = reader.ReadInt(StaticFields[5]);
            Payload.GnssAltEllipsoid = reader.ReadInt(StaticFields[6]);
            Payload.GnssHAcc = reader.ReadUInt(StaticFields[7]);
            Payload.GnssVAcc = reader.ReadUInt(StaticFields[8]);
            Payload.GnssVelAcc = reader.ReadUInt(StaticFields[9]);
            Payload.Lat = reader.ReadInt(StaticFields[10]);
            Payload.Lon = reader.ReadInt(StaticFields[11]);
            Payload.Alt = reader.ReadInt(StaticFields[12]);
            Payload.RelativeAlt = reader.ReadInt(StaticFields[13]);
            Payload.Roll = reader.ReadFloat(StaticFields[14]);
            Payload.Pitch = reader.ReadFloat(StaticFields[15]);
            Payload.Yaw = reader.ReadFloat(StaticFields[16]);
            Payload.CrsPower = reader.ReadFloat(StaticFields[17]);
            Payload.CrsAm90 = reader.ReadFloat(StaticFields[18]);
            Payload.CrsAm150 = reader.ReadFloat(StaticFields[19]);
            Payload.ClrPower = reader.ReadFloat(StaticFields[20]);
            Payload.ClrAm90 = reader.ReadFloat(StaticFields[21]);
            Payload.ClrAm150 = reader.ReadFloat(StaticFields[22]);
            Payload.TotalPower = reader.ReadFloat(StaticFields[23]);
            Payload.TotalFieldStrength = reader.ReadFloat(StaticFields[24]);
            Payload.TotalAm90 = reader.ReadFloat(StaticFields[25]);
            Payload.TotalAm150 = reader.ReadFloat(StaticFields[26]);
            Payload.Phi90CrsVsClr = reader.ReadFloat(StaticFields[27]);
            Payload.Phi150CrsVsClr = reader.ReadFloat(StaticFields[28]);
            Payload.GnssEph = reader.ReadUShort(StaticFields[29]);
            Payload.GnssEpv = reader.ReadUShort(StaticFields[30]);
            Payload.GnssVel = reader.ReadUShort(StaticFields[31]);
            Payload.Vx = reader.ReadShort(StaticFields[32]);
            Payload.Vy = reader.ReadShort(StaticFields[33]);
            Payload.Vz = reader.ReadShort(StaticFields[34]);
            Payload.Hdg = reader.ReadUShort(StaticFields[35]);
            Payload.CrsCarrierOffset = reader.ReadShort(StaticFields[36]);
            Payload.CrsFreq90 = reader.ReadShort(StaticFields[37]);
            Payload.CrsFreq150 = reader.ReadShort(StaticFields[38]);
            Payload.ClrCarrierOffset = reader.ReadShort(StaticFields[39]);
            Payload.ClrFreq90 = reader.ReadShort(StaticFields[40]);
            Payload.ClrFreq150 = reader.ReadShort(StaticFields[41]);
            Payload.TotalCarrierOffset = reader.ReadShort(StaticFields[42]);
            Payload.TotalFreq90 = reader.ReadShort(StaticFields[43]);
            Payload.TotalFreq150 = reader.ReadShort(StaticFields[44]);
            Payload.MeasureTime = reader.ReadShort(StaticFields[45]);
            reader.ReadByteArray(StaticFields[46], Payload.RecordGuid);
            Payload.GnssFixType = (GpsFixType)reader.ReadByte(StaticFields[47]);
            Payload.GnssSatellitesVisible = reader.ReadByte(StaticFields[48]);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public ulong TotalFreq { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: data_index, Units: , IsExtended: false
        /// </summary>
        public uint DataIndex { get; set; }
        /// <summary>
        /// Latitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public int GnssLat { get; set; }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public int GnssLon { get; set; }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public int GnssAlt { get; set; }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public int GnssAltEllipsoid { get; set; }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssHAcc { get; set; }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssVAcc { get; set; }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssVelAcc { get; set; }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public int Lon { get; set; }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public int Alt { get; set; }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public int RelativeAlt { get; set; }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Input power of course.
        /// OriginName: crs_power, Units: dBm, IsExtended: false
        /// </summary>
        public float CrsPower { get; set; }
        /// <summary>
        /// Aplitude modulation of 90Hz of course.
        /// OriginName: crs_am_90, Units: %, IsExtended: false
        /// </summary>
        public float CrsAm90 { get; set; }
        /// <summary>
        /// Aplitude modulation of 150Hz of course.
        /// OriginName: crs_am_150, Units: %, IsExtended: false
        /// </summary>
        public float CrsAm150 { get; set; }
        /// <summary>
        /// Input power of clearance.
        /// OriginName: clr_power, Units: dBm, IsExtended: false
        /// </summary>
        public float ClrPower { get; set; }
        /// <summary>
        /// Aplitude modulation of 90Hz of clearance.
        /// OriginName: clr_am_90, Units: %, IsExtended: false
        /// </summary>
        public float ClrAm90 { get; set; }
        /// <summary>
        /// Aplitude modulation of 150Hz of clearance.
        /// OriginName: clr_am_150, Units: % E2, IsExtended: false
        /// </summary>
        public float ClrAm150 { get; set; }
        /// <summary>
        /// Total input power.
        /// OriginName: total_power, Units: dBm, IsExtended: false
        /// </summary>
        public float TotalPower { get; set; }
        /// <summary>
        /// Total field strength.
        /// OriginName: total_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public float TotalFieldStrength { get; set; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: total_am_90, Units: %, IsExtended: false
        /// </summary>
        public float TotalAm90 { get; set; }
        /// <summary>
        /// Total aplitude modulation of 150Hz.
        /// OriginName: total_am_150, Units: %, IsExtended: false
        /// </summary>
        public float TotalAm150 { get; set; }
        /// <summary>
        ///  Phase difference 90 Hz clearance and cource
        /// OriginName: phi_90_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public float Phi90CrsVsClr { get; set; }
        /// <summary>
        /// Phase difference 150 Hz clearance and cource.
        /// OriginName: phi_150_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public float Phi150CrsVsClr { get; set; }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public ushort GnssEph { get; set; }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public ushort GnssEpv { get; set; }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public ushort GnssVel { get; set; }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vx { get; set; }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vy { get; set; }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vz { get; set; }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public ushort Hdg { get; set; }
        /// <summary>
        /// Carrier frequency offset of course.
        /// OriginName: crs_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short CrsCarrierOffset { get; set; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of course.
        /// OriginName: crs_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public short CrsFreq90 { get; set; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of course.
        /// OriginName: crs_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public short CrsFreq150 { get; set; }
        /// <summary>
        /// Carrier frequency offset of clearance.
        /// OriginName: clr_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short ClrCarrierOffset { get; set; }
        /// <summary>
        /// Frequency offset of signal 90 Hz of clearance.
        /// OriginName: clr_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public short ClrFreq90 { get; set; }
        /// <summary>
        /// Frequency offset of signal 150 Hz of clearance.
        /// OriginName: clr_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public short ClrFreq150 { get; set; }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: total_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short TotalCarrierOffset { get; set; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: total_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public short TotalFreq90 { get; set; }
        /// <summary>
        /// Total frequency offset of signal 150 Hz.
        /// OriginName: total_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public short TotalFreq150 { get; set; }
        /// <summary>
        /// Measure time.
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public short MeasureTime { get; set; }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// GPS fix type.
        /// OriginName: gnss_fix_type, Units: , IsExtended: false
        /// </summary>
        public GpsFixType GnssFixType { get; set; }
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public byte GnssSatellitesVisible { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "time_unix_usec",
            "Timestamp (UNIX epoch time).",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(1,
            "total_freq",
            "Measured frequency.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new(2,
            "data_index",
            "Data index in record",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(3,
            "gnss_lat",
            "Latitude (WGS84, EGM96 ellipsoid)",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(4,
            "gnss_lon",
            "Longitude (WGS84, EGM96 ellipsoid)",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(5,
            "gnss_alt",
            "Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(6,
            "gnss_alt_ellipsoid",
            "Altitude (above WGS84, EGM96 ellipsoid). Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(7,
            "gnss_h_acc",
            "Position uncertainty. Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(8,
            "gnss_v_acc",
            "Altitude uncertainty. Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(9,
            "gnss_vel_acc",
            "Speed uncertainty. Positive for up.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(10,
            "lat",
            "Filtered global position latitude, expressed",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(11,
            "lon",
            "Filtered global position longitude, expressed",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(12,
            "alt",
            "Filtered global position altitude (MSL).",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(13,
            "relative_alt",
            "Altitude above ground",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(14,
            "roll",
            "Roll angle (-pi..+pi)",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(15,
            "pitch",
            "Pitch angle (-pi..+pi)",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(16,
            "yaw",
            "Yaw angle (-pi..+pi)",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(17,
            "azimuth",
            "Measured azimuth.",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(18,
            "power",
            "Total input power.",
            string.Empty, 
            @"dBm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(19,
            "field_strength",
            "Total field strength.",
            string.Empty, 
            @"uV/m", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(20,
            "am_30",
            "Total aplitude modulation of 30 Hz.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(21,
            "am_9960",
            "Total aplitude modulation of 9960 Hz.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(22,
            "deviation",
            "Deviation.",
            string.Empty, 
            @"", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(23,
            "code_id_am_1020",
            "Total aplitude modulation of 90Hz.",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(24,
            "gnss_eph",
            "GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(25,
            "gnss_epv",
            "GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(26,
            "gnss_vel",
            "GPS ground speed. If unknown, set to: UINT16_MAX",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(27,
            "vx",
            "Ground X Speed (Latitude, positive north)",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(28,
            "vy",
            "Ground Y Speed (Longitude, positive east)",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(29,
            "vz",
            "Ground Z Speed (Altitude, positive down)",
            string.Empty, 
            @"cm/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(30,
            "hdg",
            "Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX",
            string.Empty, 
            @"cdeg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(31,
            "carrier_offset",
            "Total carrier frequency offset.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(32,
            "freq_30",
            "Total frequency offset of signal 30 Hz.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(33,
            "freq_9960",
            "Total frequency offset of signal 9960 Hz.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(34,
            "code_id_freq_1020",
            "Total frequency offset of signal 90 Hz.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(35,
            "measure_time",
            "Measure time.",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(36,
            "record_guid",
            "Record GUID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            16, 
            false),
            new(37,
            "gnss_fix_type",
            "GPS fix type.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(38,
            "gnss_satellites_visible",
            "Number of satellites visible. If unknown, set to 255",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(39,
            "code_id",
            "Code identification",
            string.Empty, 
            @"Letters", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            4, 
            false),
        ];
        public const string FormatMessage = "ASV_SDR_RECORD_DATA_VOR:"
        + "uint64_t time_unix_usec;"
        + "uint64_t total_freq;"
        + "uint32_t data_index;"
        + "int32_t gnss_lat;"
        + "int32_t gnss_lon;"
        + "int32_t gnss_alt;"
        + "int32_t gnss_alt_ellipsoid;"
        + "uint32_t gnss_h_acc;"
        + "uint32_t gnss_v_acc;"
        + "uint32_t gnss_vel_acc;"
        + "int32_t lat;"
        + "int32_t lon;"
        + "int32_t alt;"
        + "int32_t relative_alt;"
        + "float roll;"
        + "float pitch;"
        + "float yaw;"
        + "float azimuth;"
        + "float power;"
        + "float field_strength;"
        + "float am_30;"
        + "float am_9960;"
        + "float deviation;"
        + "float code_id_am_1020;"
        + "uint16_t gnss_eph;"
        + "uint16_t gnss_epv;"
        + "uint16_t gnss_vel;"
        + "int16_t vx;"
        + "int16_t vy;"
        + "int16_t vz;"
        + "uint16_t hdg;"
        + "int16_t carrier_offset;"
        + "int16_t freq_30;"
        + "int16_t freq_9960;"
        + "int16_t code_id_freq_1020;"
        + "int16_t measure_time;"
        + "uint8_t[16] record_guid;"
        + "uint8_t gnss_fix_type;"
        + "uint8_t gnss_satellites_visible;"
        + "char[4] code_id;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TimeUnixUsec);
            writer.Write(StaticFields[1], Payload.TotalFreq);
            writer.Write(StaticFields[2], Payload.DataIndex);
            writer.Write(StaticFields[3], Payload.GnssLat);
            writer.Write(StaticFields[4], Payload.GnssLon);
            writer.Write(StaticFields[5], Payload.GnssAlt);
            writer.Write(StaticFields[6], Payload.GnssAltEllipsoid);
            writer.Write(StaticFields[7], Payload.GnssHAcc);
            writer.Write(StaticFields[8], Payload.GnssVAcc);
            writer.Write(StaticFields[9], Payload.GnssVelAcc);
            writer.Write(StaticFields[10], Payload.Lat);
            writer.Write(StaticFields[11], Payload.Lon);
            writer.Write(StaticFields[12], Payload.Alt);
            writer.Write(StaticFields[13], Payload.RelativeAlt);
            writer.Write(StaticFields[14], Payload.Roll);
            writer.Write(StaticFields[15], Payload.Pitch);
            writer.Write(StaticFields[16], Payload.Yaw);
            writer.Write(StaticFields[17], Payload.Azimuth);
            writer.Write(StaticFields[18], Payload.Power);
            writer.Write(StaticFields[19], Payload.FieldStrength);
            writer.Write(StaticFields[20], Payload.Am30);
            writer.Write(StaticFields[21], Payload.Am9960);
            writer.Write(StaticFields[22], Payload.Deviation);
            writer.Write(StaticFields[23], Payload.CodeIdAm1020);
            writer.Write(StaticFields[24], Payload.GnssEph);
            writer.Write(StaticFields[25], Payload.GnssEpv);
            writer.Write(StaticFields[26], Payload.GnssVel);
            writer.Write(StaticFields[27], Payload.Vx);
            writer.Write(StaticFields[28], Payload.Vy);
            writer.Write(StaticFields[29], Payload.Vz);
            writer.Write(StaticFields[30], Payload.Hdg);
            writer.Write(StaticFields[31], Payload.CarrierOffset);
            writer.Write(StaticFields[32], Payload.Freq30);
            writer.Write(StaticFields[33], Payload.Freq9960);
            writer.Write(StaticFields[34], Payload.CodeIdFreq1020);
            writer.Write(StaticFields[35], Payload.MeasureTime);
            writer.Write(StaticFields[36], Payload.RecordGuid);
            writer.Write(StaticFields[37], (ulong)Payload.GnssFixType);
            writer.Write(StaticFields[38], Payload.GnssSatellitesVisible);
            writer.Write(StaticFields[39], Payload.CodeId);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TimeUnixUsec = reader.ReadULong(StaticFields[0]);
            Payload.TotalFreq = reader.ReadULong(StaticFields[1]);
            Payload.DataIndex = reader.ReadUInt(StaticFields[2]);
            Payload.GnssLat = reader.ReadInt(StaticFields[3]);
            Payload.GnssLon = reader.ReadInt(StaticFields[4]);
            Payload.GnssAlt = reader.ReadInt(StaticFields[5]);
            Payload.GnssAltEllipsoid = reader.ReadInt(StaticFields[6]);
            Payload.GnssHAcc = reader.ReadUInt(StaticFields[7]);
            Payload.GnssVAcc = reader.ReadUInt(StaticFields[8]);
            Payload.GnssVelAcc = reader.ReadUInt(StaticFields[9]);
            Payload.Lat = reader.ReadInt(StaticFields[10]);
            Payload.Lon = reader.ReadInt(StaticFields[11]);
            Payload.Alt = reader.ReadInt(StaticFields[12]);
            Payload.RelativeAlt = reader.ReadInt(StaticFields[13]);
            Payload.Roll = reader.ReadFloat(StaticFields[14]);
            Payload.Pitch = reader.ReadFloat(StaticFields[15]);
            Payload.Yaw = reader.ReadFloat(StaticFields[16]);
            Payload.Azimuth = reader.ReadFloat(StaticFields[17]);
            Payload.Power = reader.ReadFloat(StaticFields[18]);
            Payload.FieldStrength = reader.ReadFloat(StaticFields[19]);
            Payload.Am30 = reader.ReadFloat(StaticFields[20]);
            Payload.Am9960 = reader.ReadFloat(StaticFields[21]);
            Payload.Deviation = reader.ReadFloat(StaticFields[22]);
            Payload.CodeIdAm1020 = reader.ReadFloat(StaticFields[23]);
            Payload.GnssEph = reader.ReadUShort(StaticFields[24]);
            Payload.GnssEpv = reader.ReadUShort(StaticFields[25]);
            Payload.GnssVel = reader.ReadUShort(StaticFields[26]);
            Payload.Vx = reader.ReadShort(StaticFields[27]);
            Payload.Vy = reader.ReadShort(StaticFields[28]);
            Payload.Vz = reader.ReadShort(StaticFields[29]);
            Payload.Hdg = reader.ReadUShort(StaticFields[30]);
            Payload.CarrierOffset = reader.ReadShort(StaticFields[31]);
            Payload.Freq30 = reader.ReadShort(StaticFields[32]);
            Payload.Freq9960 = reader.ReadShort(StaticFields[33]);
            Payload.CodeIdFreq1020 = reader.ReadShort(StaticFields[34]);
            Payload.MeasureTime = reader.ReadShort(StaticFields[35]);
            reader.ReadByteArray(StaticFields[36], Payload.RecordGuid);
            Payload.GnssFixType = (GpsFixType)reader.ReadByte(StaticFields[37]);
            Payload.GnssSatellitesVisible = reader.ReadByte(StaticFields[38]);
            reader.ReadCharArray(StaticFields[39], Payload.CodeId);
        
            
        }
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
            RecordGuid = new byte[arraySize];
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
            buffer = buffer.Slice(arraySize);
           

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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public ulong TotalFreq { get; set; }
        /// <summary>
        /// Data index in record
        /// OriginName: data_index, Units: , IsExtended: false
        /// </summary>
        public uint DataIndex { get; set; }
        /// <summary>
        /// Latitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public int GnssLat { get; set; }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public int GnssLon { get; set; }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public int GnssAlt { get; set; }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public int GnssAltEllipsoid { get; set; }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssHAcc { get; set; }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssVAcc { get; set; }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public uint GnssVelAcc { get; set; }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public int Lon { get; set; }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public int Alt { get; set; }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public int RelativeAlt { get; set; }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Measured azimuth.
        /// OriginName: azimuth, Units: deg, IsExtended: false
        /// </summary>
        public float Azimuth { get; set; }
        /// <summary>
        /// Total input power.
        /// OriginName: power, Units: dBm, IsExtended: false
        /// </summary>
        public float Power { get; set; }
        /// <summary>
        /// Total field strength.
        /// OriginName: field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public float FieldStrength { get; set; }
        /// <summary>
        /// Total aplitude modulation of 30 Hz.
        /// OriginName: am_30, Units: %, IsExtended: false
        /// </summary>
        public float Am30 { get; set; }
        /// <summary>
        /// Total aplitude modulation of 9960 Hz.
        /// OriginName: am_9960, Units: %, IsExtended: false
        /// </summary>
        public float Am9960 { get; set; }
        /// <summary>
        /// Deviation.
        /// OriginName: deviation, Units: , IsExtended: false
        /// </summary>
        public float Deviation { get; set; }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: code_id_am_1020, Units: %, IsExtended: false
        /// </summary>
        public float CodeIdAm1020 { get; set; }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public ushort GnssEph { get; set; }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public ushort GnssEpv { get; set; }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public ushort GnssVel { get; set; }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vx { get; set; }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vy { get; set; }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public short Vz { get; set; }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public ushort Hdg { get; set; }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public short CarrierOffset { get; set; }
        /// <summary>
        /// Total frequency offset of signal 30 Hz.
        /// OriginName: freq_30, Units: Hz, IsExtended: false
        /// </summary>
        public short Freq30 { get; set; }
        /// <summary>
        /// Total frequency offset of signal 9960 Hz.
        /// OriginName: freq_9960, Units: Hz, IsExtended: false
        /// </summary>
        public short Freq9960 { get; set; }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: code_id_freq_1020, Units: Hz, IsExtended: false
        /// </summary>
        public short CodeIdFreq1020 { get; set; }
        /// <summary>
        /// Measure time.
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public short MeasureTime { get; set; }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public const int RecordGuidMaxItemsCount = 16;
        public byte[] RecordGuid { get; set; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetRecordGuidMaxItemsCount instead.")]
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// GPS fix type.
        /// OriginName: gnss_fix_type, Units: , IsExtended: false
        /// </summary>
        public GpsFixType GnssFixType { get; set; }
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public byte GnssSatellitesVisible { get; set; }
        /// <summary>
        /// Code identification
        /// OriginName: code_id, Units: Letters, IsExtended: false
        /// </summary>
        public const int CodeIdMaxItemsCount = 4;
        public char[] CodeId { get; } = new char[4];
    }


#endregion


}

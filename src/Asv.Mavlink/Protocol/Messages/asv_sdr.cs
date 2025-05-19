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

        public void Visit(IVisitor visitor)
        {
            var tmpSupportedModes = (ulong)SupportedModes;
            UInt64Type.Accept(visitor,SupportedModesField, ref tmpSupportedModes);
            SupportedModes = (AsvSdrCustomModeFlag)tmpSupportedModes;
            UInt64Type.Accept(visitor,SizeField, ref _Size);    
            UInt16Type.Accept(visitor,RecordCountField, ref _RecordCount);    
            UInt16Type.Accept(visitor,CurrentMissionIndexField, ref _CurrentMissionIndex);    
            ArrayType.Accept(visitor,CurrentRecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, CurrentRecordGuidField, ref CurrentRecordGuid[index]));    
            var tmpCurrentRecordMode = (byte)CurrentRecordMode;
            UInt8Type.Accept(visitor,CurrentRecordModeField, ref tmpCurrentRecordMode);
            CurrentRecordMode = (AsvSdrCustomMode)tmpCurrentRecordMode;
            ArrayType.Accept(visitor,CurrentRecordNameField, 28, (index,v) =>
            {
                var tmp = (byte)CurrentRecordName[index];
                UInt8Type.Accept(v,CurrentRecordNameField, ref tmp);
                CurrentRecordName[index] = (char)tmp;
            });
            var tmpMissionState = (byte)MissionState;
            UInt8Type.Accept(visitor,MissionStateField, ref tmpMissionState);
            MissionState = (AsvSdrMissionState)tmpMissionState;
            var tmpCalibState = (byte)CalibState;
            UInt8Type.Accept(visitor,CalibStateField, ref tmpCalibState);
            CalibState = (AsvSdrCalibState)tmpCalibState;
            UInt16Type.Accept(visitor,CalibTableCountField, ref _CalibTableCount);    
            FloatType.Accept(visitor,RefPowerField, ref _RefPower);    
            FloatType.Accept(visitor,SignalOverflowField, ref _SignalOverflow);    

        }

        /// <summary>
        /// Supported ASV_SDR_CUSTOM_MODE.
        /// OriginName: supported_modes, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SupportedModesField = new Field.Builder()
            .Name(nameof(SupportedModes))
            .Title("supported_modes")
            .Description("Supported ASV_SDR_CUSTOM_MODE.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt64Type.Default)

            .Build();
        public AsvSdrCustomModeFlag _SupportedModes;
        public AsvSdrCustomModeFlag SupportedModes { get => _SupportedModes; set => _SupportedModes = value; } 
        /// <summary>
        /// Total storage size in bytes.
        /// OriginName: size, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field SizeField = new Field.Builder()
            .Name(nameof(Size))
            .Title("size")
            .Description("Total storage size in bytes.")
            .FormatString(string.Empty)
            .Units(@"bytes")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Size;
        public ulong Size { get => _Size; set { _Size = value; } }
        /// <summary>
        /// Number of records in storage.
        /// OriginName: record_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordCountField = new Field.Builder()
            .Name(nameof(RecordCount))
            .Title("record_count")
            .Description("Number of records in storage.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RecordCount;
        public ushort RecordCount { get => _RecordCount; set { _RecordCount = value; } }
        /// <summary>
        /// Current mission index.
        /// OriginName: current_mission_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CurrentMissionIndexField = new Field.Builder()
            .Name(nameof(CurrentMissionIndex))
            .Title("current_mission_index")
            .Description("Current mission index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _CurrentMissionIndex;
        public ushort CurrentMissionIndex { get => _CurrentMissionIndex; set { _CurrentMissionIndex = value; } }
        /// <summary>
        /// Record GUID. Also by this field we can understand if the data is currently being recorded (GUID!=0x00) or not (GUID==0x00).
        /// OriginName: current_record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CurrentRecordGuidField = new Field.Builder()
            .Name(nameof(CurrentRecordGuid))
            .Title("current_record_guid")
            .Description("Record GUID. Also by this field we can understand if the data is currently being recorded (GUID!=0x00) or not (GUID==0x00).")
            .FormatString(string.Empty)
            .Units(string.Empty)
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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrCustomMode _CurrentRecordMode;
        public AsvSdrCustomMode CurrentRecordMode { get => _CurrentRecordMode; set => _CurrentRecordMode = value; } 
        /// <summary>
        /// Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string. If the data is currently not being recorded, than return null; 
        /// OriginName: current_record_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CurrentRecordNameField = new Field.Builder()
            .Name(nameof(CurrentRecordName))
            .Title("current_record_name")
            .Description("Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string. If the data is currently not being recorded, than return null; ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,28))

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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrMissionState _MissionState;
        public AsvSdrMissionState MissionState { get => _MissionState; set => _MissionState = value; } 
        /// <summary>
        /// Calibration status.
        /// OriginName: calib_state, Units: , IsExtended: true
        /// </summary>
        public static readonly Field CalibStateField = new Field.Builder()
            .Name(nameof(CalibState))
            .Title("calib_state")
            .Description("Calibration status.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrCalibState _CalibState;
        public AsvSdrCalibState CalibState { get => _CalibState; set => _CalibState = value; } 
        /// <summary>
        /// Number of calibration tables.
        /// OriginName: calib_table_count, Units: , IsExtended: true
        /// </summary>
        public static readonly Field CalibTableCountField = new Field.Builder()
            .Name(nameof(CalibTableCount))
            .Title("calib_table_count")
            .Description("Number of calibration tables.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _CalibTableCount;
        public ushort CalibTableCount { get => _CalibTableCount; set { _CalibTableCount = value; } }
        /// <summary>
        /// Estimated reference power in dBm. Entered in MAV_CMD_ASV_SDR_SET_MODE command.
        /// OriginName: ref_power, Units: , IsExtended: true
        /// </summary>
        public static readonly Field RefPowerField = new Field.Builder()
            .Name(nameof(RefPower))
            .Title("ref_power")
            .Description("Estimated reference power in dBm. Entered in MAV_CMD_ASV_SDR_SET_MODE command.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _RefPower;
        public float RefPower { get => _RefPower; set { _RefPower = value; } }
        /// <summary>
        /// Input path signal overflow indicator. Relative value from 0.0 to 1.0.
        /// OriginName: signal_overflow, Units: , IsExtended: true
        /// </summary>
        public static readonly Field SignalOverflowField = new Field.Builder()
            .Name(nameof(SignalOverflow))
            .Title("signal_overflow")
            .Description("Input path signal overflow indicator. Relative value from 0.0 to 1.0.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _SignalOverflow;
        public float SignalOverflow { get => _SignalOverflow; set { _SignalOverflow = value; } }
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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt16Type.Accept(visitor,SkipField, ref _Skip);    
            UInt16Type.Accept(visitor,CountField, ref _Count);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// Specifies a unique number for this request. This allows the response packet to be identified.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies a unique number for this request. This allows the response packet to be identified.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Specifies the start index of the records to be sent in the response.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SkipField = new Field.Builder()
            .Name(nameof(Skip))
            .Title("skip")
            .Description("Specifies the start index of the records to be sent in the response.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Skip;
        public ushort Skip { get => _Skip; set { _Skip = value; } }
        /// <summary>
        /// Specifies the number of records to be sent in the response after the skip index.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Specifies the number of records to be sent in the response after the skip index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Count;
        public ushort Count { get => _Count; set { _Count = value; } }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt16Type.Accept(visitor,ItemsCountField, ref _ItemsCount);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ref tmpResult);
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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Number of items ASV_SDR_RECORD for transmition after this request with success result code (depended from request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ItemsCountField = new Field.Builder()
            .Name(nameof(ItemsCount))
            .Title("items_count")
            .Description("Number of items ASV_SDR_RECORD for transmition after this request with success result code (depended from request).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ItemsCount;
        public ushort ItemsCount { get => _ItemsCount; set { _ItemsCount = value; } }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrRequestAck _Result;
        public AsvSdrRequestAck Result { get => _Result; set => _Result = value; } 
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,FrequencyField, ref _Frequency);    
            UInt64Type.Accept(visitor,CreatedUnixUsField, ref _CreatedUnixUs);    
            var tmpDataType = (uint)DataType;
            UInt32Type.Accept(visitor,DataTypeField, ref tmpDataType);
            DataType = (AsvSdrCustomMode)tmpDataType;
            UInt32Type.Accept(visitor,DurationSecField, ref _DurationSec);    
            UInt32Type.Accept(visitor,DataCountField, ref _DataCount);    
            UInt32Type.Accept(visitor,SizeField, ref _Size);    
            UInt16Type.Accept(visitor,TagCountField, ref _TagCount);    
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    
            ArrayType.Accept(visitor,RecordNameField, 28, (index,v) =>
            {
                var tmp = (byte)RecordName[index];
                UInt8Type.Accept(v,RecordNameField, ref tmp);
                RecordName[index] = (char)tmp;
            });

        }

        /// <summary>
        /// Reference frequency in Hz, specified by MAV_CMD_ASV_SDR_SET_MODE command.
        /// OriginName: frequency, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FrequencyField = new Field.Builder()
            .Name(nameof(Frequency))
            .Title("frequency")
            .Description("Reference frequency in Hz, specified by MAV_CMD_ASV_SDR_SET_MODE command.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Frequency;
        public ulong Frequency { get => _Frequency; set { _Frequency = value; } }
        /// <summary>
        /// Created timestamp (UNIX epoch time).
        /// OriginName: created_unix_us, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field CreatedUnixUsField = new Field.Builder()
            .Name(nameof(CreatedUnixUs))
            .Title("created_unix_us")
            .Description("Created timestamp (UNIX epoch time).")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _CreatedUnixUs;
        public ulong CreatedUnixUs { get => _CreatedUnixUs; set { _CreatedUnixUs = value; } }
        /// <summary>
        /// Record data type(it is also possible to know the type of data inside the record by cast enum to int).
        /// OriginName: data_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataTypeField = new Field.Builder()
            .Name(nameof(DataType))
            .Title("data_type")
            .Description("Record data type(it is also possible to know the type of data inside the record by cast enum to int).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        public AsvSdrCustomMode _DataType;
        public AsvSdrCustomMode DataType { get => _DataType; set => _DataType = value; } 
        /// <summary>
        /// Record duration in sec.
        /// OriginName: duration_sec, Units: sec, IsExtended: false
        /// </summary>
        public static readonly Field DurationSecField = new Field.Builder()
            .Name(nameof(DurationSec))
            .Title("duration_sec")
            .Description("Record duration in sec.")
            .FormatString(string.Empty)
            .Units(@"sec")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _DurationSec;
        public uint DurationSec { get => _DurationSec; set { _DurationSec = value; } }
        /// <summary>
        /// Data items count.
        /// OriginName: data_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataCountField = new Field.Builder()
            .Name(nameof(DataCount))
            .Title("data_count")
            .Description("Data items count.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _DataCount;
        public uint DataCount { get => _DataCount; set { _DataCount = value; } }
        /// <summary>
        /// Total data size of record with all data items and tags.
        /// OriginName: size, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field SizeField = new Field.Builder()
            .Name(nameof(Size))
            .Title("size")
            .Description("Total data size of record with all data items and tags.")
            .FormatString(string.Empty)
            .Units(@"bytes")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Size;
        public uint Size { get => _Size; set { _Size = value; } }
        /// <summary>
        /// Tag items count.
        /// OriginName: tag_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TagCountField = new Field.Builder()
            .Name(nameof(TagCount))
            .Title("tag_count")
            .Description("Tag items count.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _TagCount;
        public ushort TagCount { get => _TagCount; set { _TagCount = value; } }
        /// <summary>
        /// Record GUID. Generated by payload after the start of recording.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID. Generated by payload after the start of recording.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,28))

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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    

        }

        /// <summary>
        /// Specifies a unique number for this request. This allows the response packet to be identified.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies a unique number for this request. This allows the response packet to be identified.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Specifies GUID of the record to be deleted.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Specifies GUID of the record to be deleted.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ref tmpResult);
            Result = (AsvSdrRequestAck)tmpResult;
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    

        }

        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request. This allows the response to be matched to the correct request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrRequestAck _Result;
        public AsvSdrRequestAck Result { get => _Result; set => _Result = value; } 
        /// <summary>
        /// Specifies the GUID of the record that was deleted.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Specifies the GUID of the record that was deleted.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt16Type.Accept(visitor,SkipField, ref _Skip);    
            UInt16Type.Accept(visitor,CountField, ref _Count);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    

        }

        /// <summary>
        /// Request unique number. This is to allow the response packet to be detected.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Request unique number. This is to allow the response packet to be detected.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Specifies the start index of the tag to be sent in the response.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SkipField = new Field.Builder()
            .Name(nameof(Skip))
            .Title("skip")
            .Description("Specifies the start index of the tag to be sent in the response.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Skip;
        public ushort Skip { get => _Skip; set { _Skip = value; } }
        /// <summary>
        /// Specifies the number of tag to be sent in the response after the skip index.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Specifies the number of tag to be sent in the response after the skip index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Count;
        public ushort Count { get => _Count; set { _Count = value; } }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Specifies the GUID of the record.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Specifies the GUID of the record.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt16Type.Accept(visitor,ItemsCountField, ref _ItemsCount);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ref tmpResult);
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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Number of items ASV_SDR_RECORD_TAG for transmition after this request with success result code (depended from request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ItemsCountField = new Field.Builder()
            .Name(nameof(ItemsCount))
            .Title("items_count")
            .Description("Number of items ASV_SDR_RECORD_TAG for transmition after this request with success result code (depended from request).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ItemsCount;
        public ushort ItemsCount { get => _ItemsCount; set { _ItemsCount = value; } }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrRequestAck _Result;
        public AsvSdrRequestAck Result { get => _Result; set => _Result = value; } 
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

        public void Visit(IVisitor visitor)
        {
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    
            ArrayType.Accept(visitor,TagGuidField, 16,
                (index,v) => UInt8Type.Accept(v, TagGuidField, ref TagGuid[index]));    
            ArrayType.Accept(visitor,TagNameField, 16, (index,v) =>
            {
                var tmp = (byte)TagName[index];
                UInt8Type.Accept(v,TagNameField, ref tmp);
                TagName[index] = (char)tmp;
            });
            var tmpTagType = (byte)TagType;
            UInt8Type.Accept(visitor,TagTypeField, ref tmpTagType);
            TagType = (AsvSdrRecordTagType)tmpTagType;
            ArrayType.Accept(visitor,TagValueField, 8,
                (index,v) => UInt8Type.Accept(v, TagValueField, ref TagValue[index]));    

        }

        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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
            .FormatString(string.Empty)
            .Units(string.Empty)
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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,16))

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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrRecordTagType _TagType;
        public AsvSdrRecordTagType TagType { get => _TagType; set => _TagType = value; } 
        /// <summary>
        /// Tag value, depends on the type of tag.
        /// OriginName: tag_value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TagValueField = new Field.Builder()
            .Name(nameof(TagValue))
            .Title("tag_value")
            .Description("Tag value, depends on the type of tag.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    
            ArrayType.Accept(visitor,TagGuidField, 16,
                (index,v) => UInt8Type.Accept(v, TagGuidField, ref TagGuid[index]));    

        }

        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request. This allows the response to be matched to the correct request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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
            .FormatString(string.Empty)
            .Units(string.Empty)
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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ref tmpResult);
            Result = (AsvSdrRequestAck)tmpResult;
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    
            ArrayType.Accept(visitor,TagGuidField, 16,
                (index,v) => UInt8Type.Accept(v, TagGuidField, ref TagGuid[index]));    

        }

        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request. This allows the response to be matched to the correct request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrRequestAck _Result;
        public AsvSdrRequestAck Result { get => _Result; set => _Result = value; } 
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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
            .FormatString(string.Empty)
            .Units(string.Empty)
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

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,SkipField, ref _Skip);    
            UInt32Type.Accept(visitor,CountField, ref _Count);    
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    

        }

        /// <summary>
        /// Specifies the start index of the tag to be sent in the response.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SkipField = new Field.Builder()
            .Name(nameof(Skip))
            .Title("skip")
            .Description("Specifies the start index of the tag to be sent in the response.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Skip;
        public uint Skip { get => _Skip; set { _Skip = value; } }
        /// <summary>
        /// Specifies the number of tag to be sent in the response after the skip index.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Specifies the number of tag to be sent in the response after the skip index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Count;
        public uint Count { get => _Count; set { _Count = value; } }
        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request. This allows the response to be matched to the correct request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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

        public void Visit(IVisitor visitor)
        {
            var tmpDataType = (uint)DataType;
            UInt32Type.Accept(visitor,DataTypeField, ref tmpDataType);
            DataType = (AsvSdrCustomMode)tmpDataType;
            UInt32Type.Accept(visitor,ItemsCountField, ref _ItemsCount);    
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ref tmpResult);
            Result = (AsvSdrRequestAck)tmpResult;
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    

        }

        /// <summary>
        /// Record data type(it is also possible to know the type of data inside the record by cast enum to int).
        /// OriginName: data_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataTypeField = new Field.Builder()
            .Name(nameof(DataType))
            .Title("data_type")
            .Description("Record data type(it is also possible to know the type of data inside the record by cast enum to int).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        public AsvSdrCustomMode _DataType;
        public AsvSdrCustomMode DataType { get => _DataType; set => _DataType = value; } 
        /// <summary>
        /// Number of items ASV_SDR_RECORD_DATA_* for transmition after this request with success result code (depended from request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ItemsCountField = new Field.Builder()
            .Name(nameof(ItemsCount))
            .Title("items_count")
            .Description("Number of items ASV_SDR_RECORD_DATA_* for transmition after this request with success result code (depended from request).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _ItemsCount;
        public uint ItemsCount { get => _ItemsCount; set { _ItemsCount = value; } }
        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request. This allows the response to be matched to the correct request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrRequestAck _Result;
        public AsvSdrRequestAck Result { get => _Result; set => _Result = value; } 
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ref tmpResult);
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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrRequestAck _Result;
        public AsvSdrRequestAck Result { get => _Result; set => _Result = value; } 
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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,TableIndexField, ref _TableIndex);    
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _TableIndex;
        public ushort TableIndex { get => _TableIndex; set { _TableIndex = value; } }
        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request. This allows the response to be matched to the correct request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,CreatedUnixUsField, ref _CreatedUnixUs);    
            UInt16Type.Accept(visitor,TableIndexField, ref _TableIndex);    
            UInt16Type.Accept(visitor,RowCountField, ref _RowCount);    
            ArrayType.Accept(visitor,TableNameField, 28, (index,v) =>
            {
                var tmp = (byte)TableName[index];
                UInt8Type.Accept(v,TableNameField, ref tmp);
                TableName[index] = (char)tmp;
            });

        }

        /// <summary>
        /// Updated timestamp (UNIX epoch time).
        /// OriginName: created_unix_us, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field CreatedUnixUsField = new Field.Builder()
            .Name(nameof(CreatedUnixUs))
            .Title("created_unix_us")
            .Description("Updated timestamp (UNIX epoch time).")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _CreatedUnixUs;
        public ulong CreatedUnixUs { get => _CreatedUnixUs; set { _CreatedUnixUs = value; } }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _TableIndex;
        public ushort TableIndex { get => _TableIndex; set { _TableIndex = value; } }
        /// <summary>
        /// Specifies the number of ROWs in the table.
        /// OriginName: row_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RowCountField = new Field.Builder()
            .Name(nameof(RowCount))
            .Title("row_count")
            .Description("Specifies the number of ROWs in the table.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RowCount;
        public ushort RowCount { get => _RowCount; set { _RowCount = value; } }
        /// <summary>
        /// Table name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.
        /// OriginName: table_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableNameField = new Field.Builder()
            .Name(nameof(TableName))
            .Title("table_name")
            .Description("Table name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,28))

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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt16Type.Accept(visitor,TableIndexField, ref _TableIndex);    
            UInt16Type.Accept(visitor,RowIndexField, ref _RowIndex);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request. This allows the response to be matched to the correct request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _TableIndex;
        public ushort TableIndex { get => _TableIndex; set { _TableIndex = value; } }
        /// <summary>
        /// ROW index.
        /// OriginName: row_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RowIndexField = new Field.Builder()
            .Name(nameof(RowIndex))
            .Title("row_index")
            .Description("ROW index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RowIndex;
        public ushort RowIndex { get => _RowIndex; set { _RowIndex = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,RefFreqField, ref _RefFreq);    
            FloatType.Accept(visitor,RefPowerField, ref _RefPower);    
            FloatType.Accept(visitor,RefValueField, ref _RefValue);    
            FloatType.Accept(visitor,AdjustmentField, ref _Adjustment);    
            UInt16Type.Accept(visitor,TableIndexField, ref _TableIndex);    
            UInt16Type.Accept(visitor,RowIndexField, ref _RowIndex);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// Reference frequency in Hz.
        /// OriginName: ref_freq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RefFreqField = new Field.Builder()
            .Name(nameof(RefFreq))
            .Title("ref_freq")
            .Description("Reference frequency in Hz.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _RefFreq;
        public ulong RefFreq { get => _RefFreq; set { _RefFreq = value; } }
        /// <summary>
        /// Reference power in dBm.
        /// OriginName: ref_power, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RefPowerField = new Field.Builder()
            .Name(nameof(RefPower))
            .Title("ref_power")
            .Description("Reference power in dBm.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _RefPower;
        public float RefPower { get => _RefPower; set { _RefPower = value; } }
        /// <summary>
        /// Reference value.
        /// OriginName: ref_value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RefValueField = new Field.Builder()
            .Name(nameof(RefValue))
            .Title("ref_value")
            .Description("Reference value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _RefValue;
        public float RefValue { get => _RefValue; set { _RefValue = value; } }
        /// <summary>
        /// Adjustment for measured value (ref_value = measured_value + adjustment)
        /// OriginName: adjustment, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AdjustmentField = new Field.Builder()
            .Name(nameof(Adjustment))
            .Title("adjustment")
            .Description("Adjustment for measured value (ref_value = measured_value + adjustment)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Adjustment;
        public float Adjustment { get => _Adjustment; set { _Adjustment = value; } }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _TableIndex;
        public ushort TableIndex { get => _TableIndex; set { _TableIndex = value; } }
        /// <summary>
        /// ROW index.
        /// OriginName: row_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RowIndexField = new Field.Builder()
            .Name(nameof(RowIndex))
            .Title("row_index")
            .Description("ROW index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RowIndex;
        public ushort RowIndex { get => _RowIndex; set { _RowIndex = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,CreatedUnixUsField, ref _CreatedUnixUs);    
            UInt16Type.Accept(visitor,TableIndexField, ref _TableIndex);    
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt16Type.Accept(visitor,RowCountField, ref _RowCount);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// Current timestamp (UNIX epoch time).
        /// OriginName: created_unix_us, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field CreatedUnixUsField = new Field.Builder()
            .Name(nameof(CreatedUnixUs))
            .Title("created_unix_us")
            .Description("Current timestamp (UNIX epoch time).")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _CreatedUnixUs;
        public ulong CreatedUnixUs { get => _CreatedUnixUs; set { _CreatedUnixUs = value; } }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _TableIndex;
        public ushort TableIndex { get => _TableIndex; set { _TableIndex = value; } }
        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request. This allows the response to be matched to the correct request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Specifies the number of ROWs in the table.
        /// OriginName: row_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RowCountField = new Field.Builder()
            .Name(nameof(RowCount))
            .Title("row_count")
            .Description("Specifies the number of ROWs in the table.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RowCount;
        public ushort RowCount { get => _RowCount; set { _RowCount = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
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

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt16Type.Accept(visitor,TableIndexField, ref _TableIndex);    
            UInt16Type.Accept(visitor,RowIndexField, ref _RowIndex);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// Specifies the unique number of the original request from ASV_SDR_CALIB_TABLE_UPLOAD_START. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Specifies the unique number of the original request from ASV_SDR_CALIB_TABLE_UPLOAD_START. This allows the response to be matched to the correct request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RequestId;
        public ushort RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Table index.
        /// OriginName: table_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TableIndexField = new Field.Builder()
            .Name(nameof(TableIndex))
            .Title("table_index")
            .Description("Table index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _TableIndex;
        public ushort TableIndex { get => _TableIndex; set { _TableIndex = value; } }
        /// <summary>
        /// ROW index.
        /// OriginName: row_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RowIndexField = new Field.Builder()
            .Name(nameof(RowIndex))
            .Title("row_index")
            .Description("ROW index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _RowIndex;
        public ushort RowIndex { get => _RowIndex; set { _RowIndex = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, ref _TimeUnixUsec);    
            FloatType.Accept(visitor,MinField, ref _Min);    
            FloatType.Accept(visitor,MaxField, ref _Max);    
            UInt16Type.Accept(visitor,StartField, ref _Start);    
            UInt16Type.Accept(visitor,TotalField, ref _Total);    
            ArrayType.Accept(visitor,SignalNameField, 8, (index,v) =>
            {
                var tmp = (byte)SignalName[index];
                UInt8Type.Accept(v,SignalNameField, ref tmp);
                SignalName[index] = (char)tmp;
            });
            var tmpFormat = (byte)Format;
            UInt8Type.Accept(visitor,FormatField, ref tmpFormat);
            Format = (AsvSdrSignalFormat)tmpFormat;
            UInt8Type.Accept(visitor,CountField, ref _Count);    
            ArrayType.Accept(visitor,DataField, 200,
                (index,v) => UInt8Type.Accept(v, DataField, ref Data[index]));    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time) for current set of measures.
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time) for current set of measures.")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TimeUnixUsec;
        public ulong TimeUnixUsec { get => _TimeUnixUsec; set { _TimeUnixUsec = value; } }
        /// <summary>
        /// Min value of set.
        /// OriginName: min, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MinField = new Field.Builder()
            .Name(nameof(Min))
            .Title("min")
            .Description("Min value of set.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Min;
        public float Min { get => _Min; set { _Min = value; } }
        /// <summary>
        /// Max value of set.
        /// OriginName: max, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MaxField = new Field.Builder()
            .Name(nameof(Max))
            .Title("max")
            .Description("Max value of set.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Max;
        public float Max { get => _Max; set { _Max = value; } }
        /// <summary>
        /// Start index of measure set.
        /// OriginName: start, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StartField = new Field.Builder()
            .Name(nameof(Start))
            .Title("start")
            .Description("Start index of measure set.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Start;
        public ushort Start { get => _Start; set { _Start = value; } }
        /// <summary>
        /// Total points in set.
        /// OriginName: total, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TotalField = new Field.Builder()
            .Name(nameof(Total))
            .Title("total")
            .Description("Total points in set.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Total;
        public ushort Total { get => _Total; set { _Total = value; } }
        /// <summary>
        /// Signal name, terminated by NULL if the length is less than 8 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 8+1 bytes storage if the ID is stored as string
        /// OriginName: signal_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SignalNameField = new Field.Builder()
            .Name(nameof(SignalName))
            .Title("signal_name")
            .Description("Signal name, terminated by NULL if the length is less than 8 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 8+1 bytes storage if the ID is stored as string")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,8))

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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvSdrSignalFormat _Format;
        public AsvSdrSignalFormat Format { get => _Format; set => _Format = value; } 
        /// <summary>
        /// Measures count in this packet.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Measures count in this packet.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Count;
        public byte Count { get => _Count; set { _Count = value; } }
        /// <summary>
        /// Data set of points.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Data set of points.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, ref _TimeUnixUsec);    
            UInt64Type.Accept(visitor,TotalFreqField, ref _TotalFreq);    
            UInt32Type.Accept(visitor,DataIndexField, ref _DataIndex);    
            Int32Type.Accept(visitor,GnssLatField, ref _GnssLat);    
            Int32Type.Accept(visitor,GnssLonField, ref _GnssLon);    
            Int32Type.Accept(visitor,GnssAltField, ref _GnssAlt);    
            Int32Type.Accept(visitor,GnssAltEllipsoidField, ref _GnssAltEllipsoid);    
            UInt32Type.Accept(visitor,GnssHAccField, ref _GnssHAcc);    
            UInt32Type.Accept(visitor,GnssVAccField, ref _GnssVAcc);    
            UInt32Type.Accept(visitor,GnssVelAccField, ref _GnssVelAcc);    
            Int32Type.Accept(visitor,LatField, ref _Lat);    
            Int32Type.Accept(visitor,LonField, ref _Lon);    
            Int32Type.Accept(visitor,AltField, ref _Alt);    
            Int32Type.Accept(visitor,RelativeAltField, ref _RelativeAlt);    
            FloatType.Accept(visitor,RollField, ref _Roll);    
            FloatType.Accept(visitor,PitchField, ref _Pitch);    
            FloatType.Accept(visitor,YawField, ref _Yaw);    
            FloatType.Accept(visitor,CrsPowerField, ref _CrsPower);    
            FloatType.Accept(visitor,CrsAm90Field, ref _CrsAm90);    
            FloatType.Accept(visitor,CrsAm150Field, ref _CrsAm150);    
            FloatType.Accept(visitor,ClrPowerField, ref _ClrPower);    
            FloatType.Accept(visitor,ClrAm90Field, ref _ClrAm90);    
            FloatType.Accept(visitor,ClrAm150Field, ref _ClrAm150);    
            FloatType.Accept(visitor,TotalPowerField, ref _TotalPower);    
            FloatType.Accept(visitor,TotalFieldStrengthField, ref _TotalFieldStrength);    
            FloatType.Accept(visitor,TotalAm90Field, ref _TotalAm90);    
            FloatType.Accept(visitor,TotalAm150Field, ref _TotalAm150);    
            FloatType.Accept(visitor,Phi90CrsVsClrField, ref _Phi90CrsVsClr);    
            FloatType.Accept(visitor,Phi150CrsVsClrField, ref _Phi150CrsVsClr);    
            FloatType.Accept(visitor,CodeIdAm1020Field, ref _CodeIdAm1020);    
            UInt16Type.Accept(visitor,GnssEphField, ref _GnssEph);    
            UInt16Type.Accept(visitor,GnssEpvField, ref _GnssEpv);    
            UInt16Type.Accept(visitor,GnssVelField, ref _GnssVel);    
            Int16Type.Accept(visitor,VxField, ref _Vx);
            Int16Type.Accept(visitor,VyField, ref _Vy);
            Int16Type.Accept(visitor,VzField, ref _Vz);
            UInt16Type.Accept(visitor,HdgField, ref _Hdg);    
            Int16Type.Accept(visitor,CrsCarrierOffsetField, ref _CrsCarrierOffset);
            Int16Type.Accept(visitor,CrsFreq90Field, ref _CrsFreq90);
            Int16Type.Accept(visitor,CrsFreq150Field, ref _CrsFreq150);
            Int16Type.Accept(visitor,ClrCarrierOffsetField, ref _ClrCarrierOffset);
            Int16Type.Accept(visitor,ClrFreq90Field, ref _ClrFreq90);
            Int16Type.Accept(visitor,ClrFreq150Field, ref _ClrFreq150);
            Int16Type.Accept(visitor,TotalCarrierOffsetField, ref _TotalCarrierOffset);
            Int16Type.Accept(visitor,TotalFreq90Field, ref _TotalFreq90);
            Int16Type.Accept(visitor,TotalFreq150Field, ref _TotalFreq150);
            Int16Type.Accept(visitor,CodeIdFreq1020Field, ref _CodeIdFreq1020);
            Int16Type.Accept(visitor,MeasureTimeField, ref _MeasureTime);
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    
            var tmpGnssFixType = (byte)GnssFixType;
            UInt8Type.Accept(visitor,GnssFixTypeField, ref tmpGnssFixType);
            GnssFixType = (GpsFixType)tmpGnssFixType;
            UInt8Type.Accept(visitor,GnssSatellitesVisibleField, ref _GnssSatellitesVisible);    
            ArrayType.Accept(visitor,CodeIdField, 4, (index,v) =>
            {
                var tmp = (byte)CodeId[index];
                UInt8Type.Accept(v,CodeIdField, ref tmp);
                CodeId[index] = (char)tmp;
            });

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TimeUnixUsec;
        public ulong TimeUnixUsec { get => _TimeUnixUsec; set { _TimeUnixUsec = value; } }
        /// <summary>
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreqField = new Field.Builder()
            .Name(nameof(TotalFreq))
            .Title("total_freq")
            .Description("Measured frequency.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TotalFreq;
        public ulong TotalFreq { get => _TotalFreq; set { _TotalFreq = value; } }
        /// <summary>
        /// Data index in record
        /// OriginName: data_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataIndexField = new Field.Builder()
            .Name(nameof(DataIndex))
            .Title("data_index")
            .Description("Data index in record")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _DataIndex;
        public uint DataIndex { get => _DataIndex; set { _DataIndex = value; } }
        /// <summary>
        /// Latitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLatField = new Field.Builder()
            .Name(nameof(GnssLat))
            .Title("gnss_lat")
            .Description("Latitude (WGS84, EGM96 ellipsoid)")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssLat;
        public int GnssLat { get => _GnssLat; set { _GnssLat = value; } }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLonField = new Field.Builder()
            .Name(nameof(GnssLon))
            .Title("gnss_lon")
            .Description("Longitude (WGS84, EGM96 ellipsoid)")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssLon;
        public int GnssLon { get => _GnssLon; set { _GnssLon = value; } }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltField = new Field.Builder()
            .Name(nameof(GnssAlt))
            .Title("gnss_alt")
            .Description("Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssAlt;
        public int GnssAlt { get => _GnssAlt; set { _GnssAlt = value; } }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltEllipsoidField = new Field.Builder()
            .Name(nameof(GnssAltEllipsoid))
            .Title("gnss_alt_ellipsoid")
            .Description("Altitude (above WGS84, EGM96 ellipsoid). Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssAltEllipsoid;
        public int GnssAltEllipsoid { get => _GnssAltEllipsoid; set { _GnssAltEllipsoid = value; } }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssHAccField = new Field.Builder()
            .Name(nameof(GnssHAcc))
            .Title("gnss_h_acc")
            .Description("Position uncertainty. Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _GnssHAcc;
        public uint GnssHAcc { get => _GnssHAcc; set { _GnssHAcc = value; } }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVAccField = new Field.Builder()
            .Name(nameof(GnssVAcc))
            .Title("gnss_v_acc")
            .Description("Altitude uncertainty. Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _GnssVAcc;
        public uint GnssVAcc { get => _GnssVAcc; set { _GnssVAcc = value; } }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelAccField = new Field.Builder()
            .Name(nameof(GnssVelAcc))
            .Title("gnss_vel_acc")
            .Description("Speed uncertainty. Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _GnssVelAcc;
        public uint GnssVelAcc { get => _GnssVelAcc; set { _GnssVelAcc = value; } }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Filtered global position latitude, expressed")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lat;
        public int Lat { get => _Lat; set { _Lat = value; } }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LonField = new Field.Builder()
            .Name(nameof(Lon))
            .Title("lon")
            .Description("Filtered global position longitude, expressed")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lon;
        public int Lon { get => _Lon; set { _Lon = value; } }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltField = new Field.Builder()
            .Name(nameof(Alt))
            .Title("alt")
            .Description("Filtered global position altitude (MSL).")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _Alt;
        public int Alt { get => _Alt; set { _Alt = value; } }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field RelativeAltField = new Field.Builder()
            .Name(nameof(RelativeAlt))
            .Title("relative_alt")
            .Description("Altitude above ground")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _RelativeAlt;
        public int RelativeAlt { get => _RelativeAlt; set { _RelativeAlt = value; } }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle (-pi..+pi)")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Roll;
        public float Roll { get => _Roll; set { _Roll = value; } }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch angle (-pi..+pi)")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Pitch;
        public float Pitch { get => _Pitch; set { _Pitch = value; } }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw angle (-pi..+pi)")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Yaw;
        public float Yaw { get => _Yaw; set { _Yaw = value; } }
        /// <summary>
        /// Input power of course.
        /// OriginName: crs_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field CrsPowerField = new Field.Builder()
            .Name(nameof(CrsPower))
            .Title("crs_power")
            .Description("Input power of course.")
            .FormatString(string.Empty)
            .Units(@"dBm")
            .DataType(FloatType.Default)

            .Build();
        private float _CrsPower;
        public float CrsPower { get => _CrsPower; set { _CrsPower = value; } }
        /// <summary>
        /// Aplitude modulation of 90Hz of course.
        /// OriginName: crs_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CrsAm90Field = new Field.Builder()
            .Name(nameof(CrsAm90))
            .Title("crs_am_90")
            .Description("Aplitude modulation of 90Hz of course.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _CrsAm90;
        public float CrsAm90 { get => _CrsAm90; set { _CrsAm90 = value; } }
        /// <summary>
        /// Aplitude modulation of 150Hz of course.
        /// OriginName: crs_am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CrsAm150Field = new Field.Builder()
            .Name(nameof(CrsAm150))
            .Title("crs_am_150")
            .Description("Aplitude modulation of 150Hz of course.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _CrsAm150;
        public float CrsAm150 { get => _CrsAm150; set { _CrsAm150 = value; } }
        /// <summary>
        /// Input power of clearance.
        /// OriginName: clr_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field ClrPowerField = new Field.Builder()
            .Name(nameof(ClrPower))
            .Title("clr_power")
            .Description("Input power of clearance.")
            .FormatString(string.Empty)
            .Units(@"dBm")
            .DataType(FloatType.Default)

            .Build();
        private float _ClrPower;
        public float ClrPower { get => _ClrPower; set { _ClrPower = value; } }
        /// <summary>
        /// Aplitude modulation of 90Hz of clearance.
        /// OriginName: clr_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field ClrAm90Field = new Field.Builder()
            .Name(nameof(ClrAm90))
            .Title("clr_am_90")
            .Description("Aplitude modulation of 90Hz of clearance.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _ClrAm90;
        public float ClrAm90 { get => _ClrAm90; set { _ClrAm90 = value; } }
        /// <summary>
        /// Aplitude modulation of 150Hz of clearance.
        /// OriginName: clr_am_150, Units: % E2, IsExtended: false
        /// </summary>
        public static readonly Field ClrAm150Field = new Field.Builder()
            .Name(nameof(ClrAm150))
            .Title("clr_am_150")
            .Description("Aplitude modulation of 150Hz of clearance.")
            .FormatString(string.Empty)
            .Units(@"% E2")
            .DataType(FloatType.Default)

            .Build();
        private float _ClrAm150;
        public float ClrAm150 { get => _ClrAm150; set { _ClrAm150 = value; } }
        /// <summary>
        /// Total input power.
        /// OriginName: total_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field TotalPowerField = new Field.Builder()
            .Name(nameof(TotalPower))
            .Title("total_power")
            .Description("Total input power.")
            .FormatString(string.Empty)
            .Units(@"dBm")
            .DataType(FloatType.Default)

            .Build();
        private float _TotalPower;
        public float TotalPower { get => _TotalPower; set { _TotalPower = value; } }
        /// <summary>
        /// Total field strength.
        /// OriginName: total_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field TotalFieldStrengthField = new Field.Builder()
            .Name(nameof(TotalFieldStrength))
            .Title("total_field_strength")
            .Description("Total field strength.")
            .FormatString(string.Empty)
            .Units(@"uV/m")
            .DataType(FloatType.Default)

            .Build();
        private float _TotalFieldStrength;
        public float TotalFieldStrength { get => _TotalFieldStrength; set { _TotalFieldStrength = value; } }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: total_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TotalAm90Field = new Field.Builder()
            .Name(nameof(TotalAm90))
            .Title("total_am_90")
            .Description("Total aplitude modulation of 90Hz.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _TotalAm90;
        public float TotalAm90 { get => _TotalAm90; set { _TotalAm90 = value; } }
        /// <summary>
        /// Total aplitude modulation of 150Hz.
        /// OriginName: total_am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TotalAm150Field = new Field.Builder()
            .Name(nameof(TotalAm150))
            .Title("total_am_150")
            .Description("Total aplitude modulation of 150Hz.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _TotalAm150;
        public float TotalAm150 { get => _TotalAm150; set { _TotalAm150 = value; } }
        /// <summary>
        ///  Phase difference 90 Hz clearance and cource
        /// OriginName: phi_90_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi90CrsVsClrField = new Field.Builder()
            .Name(nameof(Phi90CrsVsClr))
            .Title("phi_90_crs_vs_clr")
            .Description(" Phase difference 90 Hz clearance and cource")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Phi90CrsVsClr;
        public float Phi90CrsVsClr { get => _Phi90CrsVsClr; set { _Phi90CrsVsClr = value; } }
        /// <summary>
        /// Phase difference 150 Hz clearance and cource.
        /// OriginName: phi_150_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi150CrsVsClrField = new Field.Builder()
            .Name(nameof(Phi150CrsVsClr))
            .Title("phi_150_crs_vs_clr")
            .Description("Phase difference 150 Hz clearance and cource.")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Phi150CrsVsClr;
        public float Phi150CrsVsClr { get => _Phi150CrsVsClr; set { _Phi150CrsVsClr = value; } }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: code_id_am_1020, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdAm1020Field = new Field.Builder()
            .Name(nameof(CodeIdAm1020))
            .Title("code_id_am_1020")
            .Description("Total aplitude modulation of 90Hz.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _CodeIdAm1020;
        public float CodeIdAm1020 { get => _CodeIdAm1020; set { _CodeIdAm1020 = value; } }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEphField = new Field.Builder()
            .Name(nameof(GnssEph))
            .Title("gnss_eph")
            .Description("GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _GnssEph;
        public ushort GnssEph { get => _GnssEph; set { _GnssEph = value; } }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEpvField = new Field.Builder()
            .Name(nameof(GnssEpv))
            .Title("gnss_epv")
            .Description("GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _GnssEpv;
        public ushort GnssEpv { get => _GnssEpv; set { _GnssEpv = value; } }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelField = new Field.Builder()
            .Name(nameof(GnssVel))
            .Title("gnss_vel")
            .Description("GPS ground speed. If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _GnssVel;
        public ushort GnssVel { get => _GnssVel; set { _GnssVel = value; } }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VxField = new Field.Builder()
            .Name(nameof(Vx))
            .Title("vx")
            .Description("Ground X Speed (Latitude, positive north)")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Vx;
        public short Vx { get => _Vx; set { _Vx = value; } }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VyField = new Field.Builder()
            .Name(nameof(Vy))
            .Title("vy")
            .Description("Ground Y Speed (Longitude, positive east)")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Vy;
        public short Vy { get => _Vy; set { _Vy = value; } }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VzField = new Field.Builder()
            .Name(nameof(Vz))
            .Title("vz")
            .Description("Ground Z Speed (Altitude, positive down)")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Vz;
        public short Vz { get => _Vz; set { _Vz = value; } }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field HdgField = new Field.Builder()
            .Name(nameof(Hdg))
            .Title("hdg")
            .Description("Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(@"cdeg")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Hdg;
        public ushort Hdg { get => _Hdg; set { _Hdg = value; } }
        /// <summary>
        /// Carrier frequency offset of course.
        /// OriginName: crs_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsCarrierOffsetField = new Field.Builder()
            .Name(nameof(CrsCarrierOffset))
            .Title("crs_carrier_offset")
            .Description("Carrier frequency offset of course.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _CrsCarrierOffset;
        public short CrsCarrierOffset { get => _CrsCarrierOffset; set { _CrsCarrierOffset = value; } }
        /// <summary>
        /// Frequency offset of signal 90 Hz of course.
        /// OriginName: crs_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsFreq90Field = new Field.Builder()
            .Name(nameof(CrsFreq90))
            .Title("crs_freq_90")
            .Description("Frequency offset of signal 90 Hz of course.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _CrsFreq90;
        public short CrsFreq90 { get => _CrsFreq90; set { _CrsFreq90 = value; } }
        /// <summary>
        /// Frequency offset of signal 150 Hz of course.
        /// OriginName: crs_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsFreq150Field = new Field.Builder()
            .Name(nameof(CrsFreq150))
            .Title("crs_freq_150")
            .Description("Frequency offset of signal 150 Hz of course.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _CrsFreq150;
        public short CrsFreq150 { get => _CrsFreq150; set { _CrsFreq150 = value; } }
        /// <summary>
        /// Carrier frequency offset of clearance.
        /// OriginName: clr_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrCarrierOffsetField = new Field.Builder()
            .Name(nameof(ClrCarrierOffset))
            .Title("clr_carrier_offset")
            .Description("Carrier frequency offset of clearance.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _ClrCarrierOffset;
        public short ClrCarrierOffset { get => _ClrCarrierOffset; set { _ClrCarrierOffset = value; } }
        /// <summary>
        /// Frequency offset of signal 90 Hz of clearance.
        /// OriginName: clr_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrFreq90Field = new Field.Builder()
            .Name(nameof(ClrFreq90))
            .Title("clr_freq_90")
            .Description("Frequency offset of signal 90 Hz of clearance.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _ClrFreq90;
        public short ClrFreq90 { get => _ClrFreq90; set { _ClrFreq90 = value; } }
        /// <summary>
        /// Frequency offset of signal 150 Hz of clearance.
        /// OriginName: clr_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrFreq150Field = new Field.Builder()
            .Name(nameof(ClrFreq150))
            .Title("clr_freq_150")
            .Description("Frequency offset of signal 150 Hz of clearance.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _ClrFreq150;
        public short ClrFreq150 { get => _ClrFreq150; set { _ClrFreq150 = value; } }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: total_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalCarrierOffsetField = new Field.Builder()
            .Name(nameof(TotalCarrierOffset))
            .Title("total_carrier_offset")
            .Description("Total carrier frequency offset.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _TotalCarrierOffset;
        public short TotalCarrierOffset { get => _TotalCarrierOffset; set { _TotalCarrierOffset = value; } }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: total_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreq90Field = new Field.Builder()
            .Name(nameof(TotalFreq90))
            .Title("total_freq_90")
            .Description("Total frequency offset of signal 90 Hz.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _TotalFreq90;
        public short TotalFreq90 { get => _TotalFreq90; set { _TotalFreq90 = value; } }
        /// <summary>
        /// Total frequency offset of signal 150 Hz.
        /// OriginName: total_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreq150Field = new Field.Builder()
            .Name(nameof(TotalFreq150))
            .Title("total_freq_150")
            .Description("Total frequency offset of signal 150 Hz.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _TotalFreq150;
        public short TotalFreq150 { get => _TotalFreq150; set { _TotalFreq150 = value; } }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: code_id_freq_1020, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdFreq1020Field = new Field.Builder()
            .Name(nameof(CodeIdFreq1020))
            .Title("code_id_freq_1020")
            .Description("Total frequency offset of signal 90 Hz.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _CodeIdFreq1020;
        public short CodeIdFreq1020 { get => _CodeIdFreq1020; set { _CodeIdFreq1020 = value; } }
        /// <summary>
        /// Measure time.
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field MeasureTimeField = new Field.Builder()
            .Name(nameof(MeasureTime))
            .Title("measure_time")
            .Description("Measure time.")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(Int16Type.Default)

            .Build();
        private short _MeasureTime;
        public short MeasureTime { get => _MeasureTime; set { _MeasureTime = value; } }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GpsFixType _GnssFixType;
        public GpsFixType GnssFixType { get => _GnssFixType; set => _GnssFixType = value; } 
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssSatellitesVisibleField = new Field.Builder()
            .Name(nameof(GnssSatellitesVisible))
            .Title("gnss_satellites_visible")
            .Description("Number of satellites visible. If unknown, set to 255")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _GnssSatellitesVisible;
        public byte GnssSatellitesVisible { get => _GnssSatellitesVisible; set { _GnssSatellitesVisible = value; } }
        /// <summary>
        /// Code identification
        /// OriginName: code_id, Units: Letters, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdField = new Field.Builder()
            .Name(nameof(CodeId))
            .Title("code_id")
            .Description("Code identification")
            .FormatString(string.Empty)
            .Units(@"Letters")
            .DataType(new ArrayType(UInt8Type.Default,4))

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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, ref _TimeUnixUsec);    
            UInt64Type.Accept(visitor,TotalFreqField, ref _TotalFreq);    
            UInt32Type.Accept(visitor,DataIndexField, ref _DataIndex);    
            Int32Type.Accept(visitor,GnssLatField, ref _GnssLat);    
            Int32Type.Accept(visitor,GnssLonField, ref _GnssLon);    
            Int32Type.Accept(visitor,GnssAltField, ref _GnssAlt);    
            Int32Type.Accept(visitor,GnssAltEllipsoidField, ref _GnssAltEllipsoid);    
            UInt32Type.Accept(visitor,GnssHAccField, ref _GnssHAcc);    
            UInt32Type.Accept(visitor,GnssVAccField, ref _GnssVAcc);    
            UInt32Type.Accept(visitor,GnssVelAccField, ref _GnssVelAcc);    
            Int32Type.Accept(visitor,LatField, ref _Lat);    
            Int32Type.Accept(visitor,LonField, ref _Lon);    
            Int32Type.Accept(visitor,AltField, ref _Alt);    
            Int32Type.Accept(visitor,RelativeAltField, ref _RelativeAlt);    
            FloatType.Accept(visitor,RollField, ref _Roll);    
            FloatType.Accept(visitor,PitchField, ref _Pitch);    
            FloatType.Accept(visitor,YawField, ref _Yaw);    
            FloatType.Accept(visitor,CrsPowerField, ref _CrsPower);    
            FloatType.Accept(visitor,CrsAm90Field, ref _CrsAm90);    
            FloatType.Accept(visitor,CrsAm150Field, ref _CrsAm150);    
            FloatType.Accept(visitor,ClrPowerField, ref _ClrPower);    
            FloatType.Accept(visitor,ClrAm90Field, ref _ClrAm90);    
            FloatType.Accept(visitor,ClrAm150Field, ref _ClrAm150);    
            FloatType.Accept(visitor,TotalPowerField, ref _TotalPower);    
            FloatType.Accept(visitor,TotalFieldStrengthField, ref _TotalFieldStrength);    
            FloatType.Accept(visitor,TotalAm90Field, ref _TotalAm90);    
            FloatType.Accept(visitor,TotalAm150Field, ref _TotalAm150);    
            FloatType.Accept(visitor,Phi90CrsVsClrField, ref _Phi90CrsVsClr);    
            FloatType.Accept(visitor,Phi150CrsVsClrField, ref _Phi150CrsVsClr);    
            UInt16Type.Accept(visitor,GnssEphField, ref _GnssEph);    
            UInt16Type.Accept(visitor,GnssEpvField, ref _GnssEpv);    
            UInt16Type.Accept(visitor,GnssVelField, ref _GnssVel);    
            Int16Type.Accept(visitor,VxField, ref _Vx);
            Int16Type.Accept(visitor,VyField, ref _Vy);
            Int16Type.Accept(visitor,VzField, ref _Vz);
            UInt16Type.Accept(visitor,HdgField, ref _Hdg);    
            Int16Type.Accept(visitor,CrsCarrierOffsetField, ref _CrsCarrierOffset);
            Int16Type.Accept(visitor,CrsFreq90Field, ref _CrsFreq90);
            Int16Type.Accept(visitor,CrsFreq150Field, ref _CrsFreq150);
            Int16Type.Accept(visitor,ClrCarrierOffsetField, ref _ClrCarrierOffset);
            Int16Type.Accept(visitor,ClrFreq90Field, ref _ClrFreq90);
            Int16Type.Accept(visitor,ClrFreq150Field, ref _ClrFreq150);
            Int16Type.Accept(visitor,TotalCarrierOffsetField, ref _TotalCarrierOffset);
            Int16Type.Accept(visitor,TotalFreq90Field, ref _TotalFreq90);
            Int16Type.Accept(visitor,TotalFreq150Field, ref _TotalFreq150);
            Int16Type.Accept(visitor,MeasureTimeField, ref _MeasureTime);
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    
            var tmpGnssFixType = (byte)GnssFixType;
            UInt8Type.Accept(visitor,GnssFixTypeField, ref tmpGnssFixType);
            GnssFixType = (GpsFixType)tmpGnssFixType;
            UInt8Type.Accept(visitor,GnssSatellitesVisibleField, ref _GnssSatellitesVisible);    

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TimeUnixUsec;
        public ulong TimeUnixUsec { get => _TimeUnixUsec; set { _TimeUnixUsec = value; } }
        /// <summary>
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreqField = new Field.Builder()
            .Name(nameof(TotalFreq))
            .Title("total_freq")
            .Description("Measured frequency.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TotalFreq;
        public ulong TotalFreq { get => _TotalFreq; set { _TotalFreq = value; } }
        /// <summary>
        /// Data index in record
        /// OriginName: data_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataIndexField = new Field.Builder()
            .Name(nameof(DataIndex))
            .Title("data_index")
            .Description("Data index in record")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _DataIndex;
        public uint DataIndex { get => _DataIndex; set { _DataIndex = value; } }
        /// <summary>
        /// Latitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLatField = new Field.Builder()
            .Name(nameof(GnssLat))
            .Title("gnss_lat")
            .Description("Latitude (WGS84, EGM96 ellipsoid)")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssLat;
        public int GnssLat { get => _GnssLat; set { _GnssLat = value; } }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLonField = new Field.Builder()
            .Name(nameof(GnssLon))
            .Title("gnss_lon")
            .Description("Longitude (WGS84, EGM96 ellipsoid)")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssLon;
        public int GnssLon { get => _GnssLon; set { _GnssLon = value; } }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltField = new Field.Builder()
            .Name(nameof(GnssAlt))
            .Title("gnss_alt")
            .Description("Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssAlt;
        public int GnssAlt { get => _GnssAlt; set { _GnssAlt = value; } }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltEllipsoidField = new Field.Builder()
            .Name(nameof(GnssAltEllipsoid))
            .Title("gnss_alt_ellipsoid")
            .Description("Altitude (above WGS84, EGM96 ellipsoid). Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssAltEllipsoid;
        public int GnssAltEllipsoid { get => _GnssAltEllipsoid; set { _GnssAltEllipsoid = value; } }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssHAccField = new Field.Builder()
            .Name(nameof(GnssHAcc))
            .Title("gnss_h_acc")
            .Description("Position uncertainty. Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _GnssHAcc;
        public uint GnssHAcc { get => _GnssHAcc; set { _GnssHAcc = value; } }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVAccField = new Field.Builder()
            .Name(nameof(GnssVAcc))
            .Title("gnss_v_acc")
            .Description("Altitude uncertainty. Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _GnssVAcc;
        public uint GnssVAcc { get => _GnssVAcc; set { _GnssVAcc = value; } }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelAccField = new Field.Builder()
            .Name(nameof(GnssVelAcc))
            .Title("gnss_vel_acc")
            .Description("Speed uncertainty. Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _GnssVelAcc;
        public uint GnssVelAcc { get => _GnssVelAcc; set { _GnssVelAcc = value; } }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Filtered global position latitude, expressed")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lat;
        public int Lat { get => _Lat; set { _Lat = value; } }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LonField = new Field.Builder()
            .Name(nameof(Lon))
            .Title("lon")
            .Description("Filtered global position longitude, expressed")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lon;
        public int Lon { get => _Lon; set { _Lon = value; } }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltField = new Field.Builder()
            .Name(nameof(Alt))
            .Title("alt")
            .Description("Filtered global position altitude (MSL).")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _Alt;
        public int Alt { get => _Alt; set { _Alt = value; } }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field RelativeAltField = new Field.Builder()
            .Name(nameof(RelativeAlt))
            .Title("relative_alt")
            .Description("Altitude above ground")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _RelativeAlt;
        public int RelativeAlt { get => _RelativeAlt; set { _RelativeAlt = value; } }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle (-pi..+pi)")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Roll;
        public float Roll { get => _Roll; set { _Roll = value; } }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch angle (-pi..+pi)")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Pitch;
        public float Pitch { get => _Pitch; set { _Pitch = value; } }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw angle (-pi..+pi)")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Yaw;
        public float Yaw { get => _Yaw; set { _Yaw = value; } }
        /// <summary>
        /// Input power of course.
        /// OriginName: crs_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field CrsPowerField = new Field.Builder()
            .Name(nameof(CrsPower))
            .Title("crs_power")
            .Description("Input power of course.")
            .FormatString(string.Empty)
            .Units(@"dBm")
            .DataType(FloatType.Default)

            .Build();
        private float _CrsPower;
        public float CrsPower { get => _CrsPower; set { _CrsPower = value; } }
        /// <summary>
        /// Aplitude modulation of 90Hz of course.
        /// OriginName: crs_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CrsAm90Field = new Field.Builder()
            .Name(nameof(CrsAm90))
            .Title("crs_am_90")
            .Description("Aplitude modulation of 90Hz of course.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _CrsAm90;
        public float CrsAm90 { get => _CrsAm90; set { _CrsAm90 = value; } }
        /// <summary>
        /// Aplitude modulation of 150Hz of course.
        /// OriginName: crs_am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CrsAm150Field = new Field.Builder()
            .Name(nameof(CrsAm150))
            .Title("crs_am_150")
            .Description("Aplitude modulation of 150Hz of course.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _CrsAm150;
        public float CrsAm150 { get => _CrsAm150; set { _CrsAm150 = value; } }
        /// <summary>
        /// Input power of clearance.
        /// OriginName: clr_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field ClrPowerField = new Field.Builder()
            .Name(nameof(ClrPower))
            .Title("clr_power")
            .Description("Input power of clearance.")
            .FormatString(string.Empty)
            .Units(@"dBm")
            .DataType(FloatType.Default)

            .Build();
        private float _ClrPower;
        public float ClrPower { get => _ClrPower; set { _ClrPower = value; } }
        /// <summary>
        /// Aplitude modulation of 90Hz of clearance.
        /// OriginName: clr_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field ClrAm90Field = new Field.Builder()
            .Name(nameof(ClrAm90))
            .Title("clr_am_90")
            .Description("Aplitude modulation of 90Hz of clearance.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _ClrAm90;
        public float ClrAm90 { get => _ClrAm90; set { _ClrAm90 = value; } }
        /// <summary>
        /// Aplitude modulation of 150Hz of clearance.
        /// OriginName: clr_am_150, Units: % E2, IsExtended: false
        /// </summary>
        public static readonly Field ClrAm150Field = new Field.Builder()
            .Name(nameof(ClrAm150))
            .Title("clr_am_150")
            .Description("Aplitude modulation of 150Hz of clearance.")
            .FormatString(string.Empty)
            .Units(@"% E2")
            .DataType(FloatType.Default)

            .Build();
        private float _ClrAm150;
        public float ClrAm150 { get => _ClrAm150; set { _ClrAm150 = value; } }
        /// <summary>
        /// Total input power.
        /// OriginName: total_power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field TotalPowerField = new Field.Builder()
            .Name(nameof(TotalPower))
            .Title("total_power")
            .Description("Total input power.")
            .FormatString(string.Empty)
            .Units(@"dBm")
            .DataType(FloatType.Default)

            .Build();
        private float _TotalPower;
        public float TotalPower { get => _TotalPower; set { _TotalPower = value; } }
        /// <summary>
        /// Total field strength.
        /// OriginName: total_field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field TotalFieldStrengthField = new Field.Builder()
            .Name(nameof(TotalFieldStrength))
            .Title("total_field_strength")
            .Description("Total field strength.")
            .FormatString(string.Empty)
            .Units(@"uV/m")
            .DataType(FloatType.Default)

            .Build();
        private float _TotalFieldStrength;
        public float TotalFieldStrength { get => _TotalFieldStrength; set { _TotalFieldStrength = value; } }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: total_am_90, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TotalAm90Field = new Field.Builder()
            .Name(nameof(TotalAm90))
            .Title("total_am_90")
            .Description("Total aplitude modulation of 90Hz.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _TotalAm90;
        public float TotalAm90 { get => _TotalAm90; set { _TotalAm90 = value; } }
        /// <summary>
        /// Total aplitude modulation of 150Hz.
        /// OriginName: total_am_150, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TotalAm150Field = new Field.Builder()
            .Name(nameof(TotalAm150))
            .Title("total_am_150")
            .Description("Total aplitude modulation of 150Hz.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _TotalAm150;
        public float TotalAm150 { get => _TotalAm150; set { _TotalAm150 = value; } }
        /// <summary>
        ///  Phase difference 90 Hz clearance and cource
        /// OriginName: phi_90_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi90CrsVsClrField = new Field.Builder()
            .Name(nameof(Phi90CrsVsClr))
            .Title("phi_90_crs_vs_clr")
            .Description(" Phase difference 90 Hz clearance and cource")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Phi90CrsVsClr;
        public float Phi90CrsVsClr { get => _Phi90CrsVsClr; set { _Phi90CrsVsClr = value; } }
        /// <summary>
        /// Phase difference 150 Hz clearance and cource.
        /// OriginName: phi_150_crs_vs_clr, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Phi150CrsVsClrField = new Field.Builder()
            .Name(nameof(Phi150CrsVsClr))
            .Title("phi_150_crs_vs_clr")
            .Description("Phase difference 150 Hz clearance and cource.")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Phi150CrsVsClr;
        public float Phi150CrsVsClr { get => _Phi150CrsVsClr; set { _Phi150CrsVsClr = value; } }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEphField = new Field.Builder()
            .Name(nameof(GnssEph))
            .Title("gnss_eph")
            .Description("GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _GnssEph;
        public ushort GnssEph { get => _GnssEph; set { _GnssEph = value; } }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEpvField = new Field.Builder()
            .Name(nameof(GnssEpv))
            .Title("gnss_epv")
            .Description("GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _GnssEpv;
        public ushort GnssEpv { get => _GnssEpv; set { _GnssEpv = value; } }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelField = new Field.Builder()
            .Name(nameof(GnssVel))
            .Title("gnss_vel")
            .Description("GPS ground speed. If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _GnssVel;
        public ushort GnssVel { get => _GnssVel; set { _GnssVel = value; } }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VxField = new Field.Builder()
            .Name(nameof(Vx))
            .Title("vx")
            .Description("Ground X Speed (Latitude, positive north)")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Vx;
        public short Vx { get => _Vx; set { _Vx = value; } }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VyField = new Field.Builder()
            .Name(nameof(Vy))
            .Title("vy")
            .Description("Ground Y Speed (Longitude, positive east)")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Vy;
        public short Vy { get => _Vy; set { _Vy = value; } }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VzField = new Field.Builder()
            .Name(nameof(Vz))
            .Title("vz")
            .Description("Ground Z Speed (Altitude, positive down)")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Vz;
        public short Vz { get => _Vz; set { _Vz = value; } }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field HdgField = new Field.Builder()
            .Name(nameof(Hdg))
            .Title("hdg")
            .Description("Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(@"cdeg")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Hdg;
        public ushort Hdg { get => _Hdg; set { _Hdg = value; } }
        /// <summary>
        /// Carrier frequency offset of course.
        /// OriginName: crs_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsCarrierOffsetField = new Field.Builder()
            .Name(nameof(CrsCarrierOffset))
            .Title("crs_carrier_offset")
            .Description("Carrier frequency offset of course.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _CrsCarrierOffset;
        public short CrsCarrierOffset { get => _CrsCarrierOffset; set { _CrsCarrierOffset = value; } }
        /// <summary>
        /// Frequency offset of signal 90 Hz of course.
        /// OriginName: crs_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsFreq90Field = new Field.Builder()
            .Name(nameof(CrsFreq90))
            .Title("crs_freq_90")
            .Description("Frequency offset of signal 90 Hz of course.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _CrsFreq90;
        public short CrsFreq90 { get => _CrsFreq90; set { _CrsFreq90 = value; } }
        /// <summary>
        /// Frequency offset of signal 150 Hz of course.
        /// OriginName: crs_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CrsFreq150Field = new Field.Builder()
            .Name(nameof(CrsFreq150))
            .Title("crs_freq_150")
            .Description("Frequency offset of signal 150 Hz of course.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _CrsFreq150;
        public short CrsFreq150 { get => _CrsFreq150; set { _CrsFreq150 = value; } }
        /// <summary>
        /// Carrier frequency offset of clearance.
        /// OriginName: clr_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrCarrierOffsetField = new Field.Builder()
            .Name(nameof(ClrCarrierOffset))
            .Title("clr_carrier_offset")
            .Description("Carrier frequency offset of clearance.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _ClrCarrierOffset;
        public short ClrCarrierOffset { get => _ClrCarrierOffset; set { _ClrCarrierOffset = value; } }
        /// <summary>
        /// Frequency offset of signal 90 Hz of clearance.
        /// OriginName: clr_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrFreq90Field = new Field.Builder()
            .Name(nameof(ClrFreq90))
            .Title("clr_freq_90")
            .Description("Frequency offset of signal 90 Hz of clearance.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _ClrFreq90;
        public short ClrFreq90 { get => _ClrFreq90; set { _ClrFreq90 = value; } }
        /// <summary>
        /// Frequency offset of signal 150 Hz of clearance.
        /// OriginName: clr_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field ClrFreq150Field = new Field.Builder()
            .Name(nameof(ClrFreq150))
            .Title("clr_freq_150")
            .Description("Frequency offset of signal 150 Hz of clearance.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _ClrFreq150;
        public short ClrFreq150 { get => _ClrFreq150; set { _ClrFreq150 = value; } }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: total_carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalCarrierOffsetField = new Field.Builder()
            .Name(nameof(TotalCarrierOffset))
            .Title("total_carrier_offset")
            .Description("Total carrier frequency offset.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _TotalCarrierOffset;
        public short TotalCarrierOffset { get => _TotalCarrierOffset; set { _TotalCarrierOffset = value; } }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: total_freq_90, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreq90Field = new Field.Builder()
            .Name(nameof(TotalFreq90))
            .Title("total_freq_90")
            .Description("Total frequency offset of signal 90 Hz.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _TotalFreq90;
        public short TotalFreq90 { get => _TotalFreq90; set { _TotalFreq90 = value; } }
        /// <summary>
        /// Total frequency offset of signal 150 Hz.
        /// OriginName: total_freq_150, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreq150Field = new Field.Builder()
            .Name(nameof(TotalFreq150))
            .Title("total_freq_150")
            .Description("Total frequency offset of signal 150 Hz.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _TotalFreq150;
        public short TotalFreq150 { get => _TotalFreq150; set { _TotalFreq150 = value; } }
        /// <summary>
        /// Measure time.
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field MeasureTimeField = new Field.Builder()
            .Name(nameof(MeasureTime))
            .Title("measure_time")
            .Description("Measure time.")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(Int16Type.Default)

            .Build();
        private short _MeasureTime;
        public short MeasureTime { get => _MeasureTime; set { _MeasureTime = value; } }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GpsFixType _GnssFixType;
        public GpsFixType GnssFixType { get => _GnssFixType; set => _GnssFixType = value; } 
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssSatellitesVisibleField = new Field.Builder()
            .Name(nameof(GnssSatellitesVisible))
            .Title("gnss_satellites_visible")
            .Description("Number of satellites visible. If unknown, set to 255")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _GnssSatellitesVisible;
        public byte GnssSatellitesVisible { get => _GnssSatellitesVisible; set { _GnssSatellitesVisible = value; } }
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, ref _TimeUnixUsec);    
            UInt64Type.Accept(visitor,TotalFreqField, ref _TotalFreq);    
            UInt32Type.Accept(visitor,DataIndexField, ref _DataIndex);    
            Int32Type.Accept(visitor,GnssLatField, ref _GnssLat);    
            Int32Type.Accept(visitor,GnssLonField, ref _GnssLon);    
            Int32Type.Accept(visitor,GnssAltField, ref _GnssAlt);    
            Int32Type.Accept(visitor,GnssAltEllipsoidField, ref _GnssAltEllipsoid);    
            UInt32Type.Accept(visitor,GnssHAccField, ref _GnssHAcc);    
            UInt32Type.Accept(visitor,GnssVAccField, ref _GnssVAcc);    
            UInt32Type.Accept(visitor,GnssVelAccField, ref _GnssVelAcc);    
            Int32Type.Accept(visitor,LatField, ref _Lat);    
            Int32Type.Accept(visitor,LonField, ref _Lon);    
            Int32Type.Accept(visitor,AltField, ref _Alt);    
            Int32Type.Accept(visitor,RelativeAltField, ref _RelativeAlt);    
            FloatType.Accept(visitor,RollField, ref _Roll);    
            FloatType.Accept(visitor,PitchField, ref _Pitch);    
            FloatType.Accept(visitor,YawField, ref _Yaw);    
            FloatType.Accept(visitor,AzimuthField, ref _Azimuth);    
            FloatType.Accept(visitor,PowerField, ref _Power);    
            FloatType.Accept(visitor,FieldStrengthField, ref _FieldStrength);    
            FloatType.Accept(visitor,Am30Field, ref _Am30);    
            FloatType.Accept(visitor,Am9960Field, ref _Am9960);    
            FloatType.Accept(visitor,DeviationField, ref _Deviation);    
            FloatType.Accept(visitor,CodeIdAm1020Field, ref _CodeIdAm1020);    
            UInt16Type.Accept(visitor,GnssEphField, ref _GnssEph);    
            UInt16Type.Accept(visitor,GnssEpvField, ref _GnssEpv);    
            UInt16Type.Accept(visitor,GnssVelField, ref _GnssVel);    
            Int16Type.Accept(visitor,VxField, ref _Vx);
            Int16Type.Accept(visitor,VyField, ref _Vy);
            Int16Type.Accept(visitor,VzField, ref _Vz);
            UInt16Type.Accept(visitor,HdgField, ref _Hdg);    
            Int16Type.Accept(visitor,CarrierOffsetField, ref _CarrierOffset);
            Int16Type.Accept(visitor,Freq30Field, ref _Freq30);
            Int16Type.Accept(visitor,Freq9960Field, ref _Freq9960);
            Int16Type.Accept(visitor,CodeIdFreq1020Field, ref _CodeIdFreq1020);
            Int16Type.Accept(visitor,MeasureTimeField, ref _MeasureTime);
            ArrayType.Accept(visitor,RecordGuidField, 16,
                (index,v) => UInt8Type.Accept(v, RecordGuidField, ref RecordGuid[index]));    
            var tmpGnssFixType = (byte)GnssFixType;
            UInt8Type.Accept(visitor,GnssFixTypeField, ref tmpGnssFixType);
            GnssFixType = (GpsFixType)tmpGnssFixType;
            UInt8Type.Accept(visitor,GnssSatellitesVisibleField, ref _GnssSatellitesVisible);    
            ArrayType.Accept(visitor,CodeIdField, 4, (index,v) =>
            {
                var tmp = (byte)CodeId[index];
                UInt8Type.Accept(v,CodeIdField, ref tmp);
                CodeId[index] = (char)tmp;
            });

        }

        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUnixUsecField = new Field.Builder()
            .Name(nameof(TimeUnixUsec))
            .Title("time_unix_usec")
            .Description("Timestamp (UNIX epoch time).")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TimeUnixUsec;
        public ulong TimeUnixUsec { get => _TimeUnixUsec; set { _TimeUnixUsec = value; } }
        /// <summary>
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field TotalFreqField = new Field.Builder()
            .Name(nameof(TotalFreq))
            .Title("total_freq")
            .Description("Measured frequency.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TotalFreq;
        public ulong TotalFreq { get => _TotalFreq; set { _TotalFreq = value; } }
        /// <summary>
        /// Data index in record
        /// OriginName: data_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataIndexField = new Field.Builder()
            .Name(nameof(DataIndex))
            .Title("data_index")
            .Description("Data index in record")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _DataIndex;
        public uint DataIndex { get => _DataIndex; set { _DataIndex = value; } }
        /// <summary>
        /// Latitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLatField = new Field.Builder()
            .Name(nameof(GnssLat))
            .Title("gnss_lat")
            .Description("Latitude (WGS84, EGM96 ellipsoid)")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssLat;
        public int GnssLat { get => _GnssLat; set { _GnssLat = value; } }
        /// <summary>
        /// Longitude (WGS84, EGM96 ellipsoid)
        /// OriginName: gnss_lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field GnssLonField = new Field.Builder()
            .Name(nameof(GnssLon))
            .Title("gnss_lon")
            .Description("Longitude (WGS84, EGM96 ellipsoid)")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssLon;
        public int GnssLon { get => _GnssLon; set { _GnssLon = value; } }
        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// OriginName: gnss_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltField = new Field.Builder()
            .Name(nameof(GnssAlt))
            .Title("gnss_alt")
            .Description("Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssAlt;
        public int GnssAlt { get => _GnssAlt; set { _GnssAlt = value; } }
        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// OriginName: gnss_alt_ellipsoid, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssAltEllipsoidField = new Field.Builder()
            .Name(nameof(GnssAltEllipsoid))
            .Title("gnss_alt_ellipsoid")
            .Description("Altitude (above WGS84, EGM96 ellipsoid). Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _GnssAltEllipsoid;
        public int GnssAltEllipsoid { get => _GnssAltEllipsoid; set { _GnssAltEllipsoid = value; } }
        /// <summary>
        /// Position uncertainty. Positive for up.
        /// OriginName: gnss_h_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssHAccField = new Field.Builder()
            .Name(nameof(GnssHAcc))
            .Title("gnss_h_acc")
            .Description("Position uncertainty. Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _GnssHAcc;
        public uint GnssHAcc { get => _GnssHAcc; set { _GnssHAcc = value; } }
        /// <summary>
        /// Altitude uncertainty. Positive for up.
        /// OriginName: gnss_v_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVAccField = new Field.Builder()
            .Name(nameof(GnssVAcc))
            .Title("gnss_v_acc")
            .Description("Altitude uncertainty. Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _GnssVAcc;
        public uint GnssVAcc { get => _GnssVAcc; set { _GnssVAcc = value; } }
        /// <summary>
        /// Speed uncertainty. Positive for up.
        /// OriginName: gnss_vel_acc, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelAccField = new Field.Builder()
            .Name(nameof(GnssVelAcc))
            .Title("gnss_vel_acc")
            .Description("Speed uncertainty. Positive for up.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _GnssVelAcc;
        public uint GnssVelAcc { get => _GnssVelAcc; set { _GnssVelAcc = value; } }
        /// <summary>
        /// Filtered global position latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Filtered global position latitude, expressed")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lat;
        public int Lat { get => _Lat; set { _Lat = value; } }
        /// <summary>
        /// Filtered global position longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LonField = new Field.Builder()
            .Name(nameof(Lon))
            .Title("lon")
            .Description("Filtered global position longitude, expressed")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lon;
        public int Lon { get => _Lon; set { _Lon = value; } }
        /// <summary>
        /// Filtered global position altitude (MSL).
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltField = new Field.Builder()
            .Name(nameof(Alt))
            .Title("alt")
            .Description("Filtered global position altitude (MSL).")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _Alt;
        public int Alt { get => _Alt; set { _Alt = value; } }
        /// <summary>
        /// Altitude above ground
        /// OriginName: relative_alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field RelativeAltField = new Field.Builder()
            .Name(nameof(RelativeAlt))
            .Title("relative_alt")
            .Description("Altitude above ground")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _RelativeAlt;
        public int RelativeAlt { get => _RelativeAlt; set { _RelativeAlt = value; } }
        /// <summary>
        /// Roll angle (-pi..+pi)
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle (-pi..+pi)")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Roll;
        public float Roll { get => _Roll; set { _Roll = value; } }
        /// <summary>
        /// Pitch angle (-pi..+pi)
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch angle (-pi..+pi)")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Pitch;
        public float Pitch { get => _Pitch; set { _Pitch = value; } }
        /// <summary>
        /// Yaw angle (-pi..+pi)
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw angle (-pi..+pi)")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Yaw;
        public float Yaw { get => _Yaw; set { _Yaw = value; } }
        /// <summary>
        /// Measured azimuth.
        /// OriginName: azimuth, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field AzimuthField = new Field.Builder()
            .Name(nameof(Azimuth))
            .Title("azimuth")
            .Description("Measured azimuth.")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Azimuth;
        public float Azimuth { get => _Azimuth; set { _Azimuth = value; } }
        /// <summary>
        /// Total input power.
        /// OriginName: power, Units: dBm, IsExtended: false
        /// </summary>
        public static readonly Field PowerField = new Field.Builder()
            .Name(nameof(Power))
            .Title("power")
            .Description("Total input power.")
            .FormatString(string.Empty)
            .Units(@"dBm")
            .DataType(FloatType.Default)

            .Build();
        private float _Power;
        public float Power { get => _Power; set { _Power = value; } }
        /// <summary>
        /// Total field strength.
        /// OriginName: field_strength, Units: uV/m, IsExtended: false
        /// </summary>
        public static readonly Field FieldStrengthField = new Field.Builder()
            .Name(nameof(FieldStrength))
            .Title("field_strength")
            .Description("Total field strength.")
            .FormatString(string.Empty)
            .Units(@"uV/m")
            .DataType(FloatType.Default)

            .Build();
        private float _FieldStrength;
        public float FieldStrength { get => _FieldStrength; set { _FieldStrength = value; } }
        /// <summary>
        /// Total aplitude modulation of 30 Hz.
        /// OriginName: am_30, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am30Field = new Field.Builder()
            .Name(nameof(Am30))
            .Title("am_30")
            .Description("Total aplitude modulation of 30 Hz.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _Am30;
        public float Am30 { get => _Am30; set { _Am30 = value; } }
        /// <summary>
        /// Total aplitude modulation of 9960 Hz.
        /// OriginName: am_9960, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field Am9960Field = new Field.Builder()
            .Name(nameof(Am9960))
            .Title("am_9960")
            .Description("Total aplitude modulation of 9960 Hz.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _Am9960;
        public float Am9960 { get => _Am9960; set { _Am9960 = value; } }
        /// <summary>
        /// Deviation.
        /// OriginName: deviation, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DeviationField = new Field.Builder()
            .Name(nameof(Deviation))
            .Title("deviation")
            .Description("Deviation.")
            .FormatString(string.Empty)
            .Units(@"")
            .DataType(FloatType.Default)

            .Build();
        private float _Deviation;
        public float Deviation { get => _Deviation; set { _Deviation = value; } }
        /// <summary>
        /// Total aplitude modulation of 90Hz.
        /// OriginName: code_id_am_1020, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdAm1020Field = new Field.Builder()
            .Name(nameof(CodeIdAm1020))
            .Title("code_id_am_1020")
            .Description("Total aplitude modulation of 90Hz.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _CodeIdAm1020;
        public float CodeIdAm1020 { get => _CodeIdAm1020; set { _CodeIdAm1020 = value; } }
        /// <summary>
        /// GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_eph, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEphField = new Field.Builder()
            .Name(nameof(GnssEph))
            .Title("gnss_eph")
            .Description("GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _GnssEph;
        public ushort GnssEph { get => _GnssEph; set { _GnssEph = value; } }
        /// <summary>
        /// GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        /// OriginName: gnss_epv, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssEpvField = new Field.Builder()
            .Name(nameof(GnssEpv))
            .Title("gnss_epv")
            .Description("GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _GnssEpv;
        public ushort GnssEpv { get => _GnssEpv; set { _GnssEpv = value; } }
        /// <summary>
        /// GPS ground speed. If unknown, set to: UINT16_MAX
        /// OriginName: gnss_vel, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field GnssVelField = new Field.Builder()
            .Name(nameof(GnssVel))
            .Title("gnss_vel")
            .Description("GPS ground speed. If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _GnssVel;
        public ushort GnssVel { get => _GnssVel; set { _GnssVel = value; } }
        /// <summary>
        /// Ground X Speed (Latitude, positive north)
        /// OriginName: vx, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VxField = new Field.Builder()
            .Name(nameof(Vx))
            .Title("vx")
            .Description("Ground X Speed (Latitude, positive north)")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Vx;
        public short Vx { get => _Vx; set { _Vx = value; } }
        /// <summary>
        /// Ground Y Speed (Longitude, positive east)
        /// OriginName: vy, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VyField = new Field.Builder()
            .Name(nameof(Vy))
            .Title("vy")
            .Description("Ground Y Speed (Longitude, positive east)")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Vy;
        public short Vy { get => _Vy; set { _Vy = value; } }
        /// <summary>
        /// Ground Z Speed (Altitude, positive down)
        /// OriginName: vz, Units: cm/s, IsExtended: false
        /// </summary>
        public static readonly Field VzField = new Field.Builder()
            .Name(nameof(Vz))
            .Title("vz")
            .Description("Ground Z Speed (Altitude, positive down)")
            .FormatString(string.Empty)
            .Units(@"cm/s")
            .DataType(Int16Type.Default)

            .Build();
        private short _Vz;
        public short Vz { get => _Vz; set { _Vz = value; } }
        /// <summary>
        /// Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        /// OriginName: hdg, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field HdgField = new Field.Builder()
            .Name(nameof(Hdg))
            .Title("hdg")
            .Description("Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX")
            .FormatString(string.Empty)
            .Units(@"cdeg")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Hdg;
        public ushort Hdg { get => _Hdg; set { _Hdg = value; } }
        /// <summary>
        /// Total carrier frequency offset.
        /// OriginName: carrier_offset, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CarrierOffsetField = new Field.Builder()
            .Name(nameof(CarrierOffset))
            .Title("carrier_offset")
            .Description("Total carrier frequency offset.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _CarrierOffset;
        public short CarrierOffset { get => _CarrierOffset; set { _CarrierOffset = value; } }
        /// <summary>
        /// Total frequency offset of signal 30 Hz.
        /// OriginName: freq_30, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq30Field = new Field.Builder()
            .Name(nameof(Freq30))
            .Title("freq_30")
            .Description("Total frequency offset of signal 30 Hz.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _Freq30;
        public short Freq30 { get => _Freq30; set { _Freq30 = value; } }
        /// <summary>
        /// Total frequency offset of signal 9960 Hz.
        /// OriginName: freq_9960, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field Freq9960Field = new Field.Builder()
            .Name(nameof(Freq9960))
            .Title("freq_9960")
            .Description("Total frequency offset of signal 9960 Hz.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _Freq9960;
        public short Freq9960 { get => _Freq9960; set { _Freq9960 = value; } }
        /// <summary>
        /// Total frequency offset of signal 90 Hz.
        /// OriginName: code_id_freq_1020, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdFreq1020Field = new Field.Builder()
            .Name(nameof(CodeIdFreq1020))
            .Title("code_id_freq_1020")
            .Description("Total frequency offset of signal 90 Hz.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(Int16Type.Default)

            .Build();
        private short _CodeIdFreq1020;
        public short CodeIdFreq1020 { get => _CodeIdFreq1020; set { _CodeIdFreq1020 = value; } }
        /// <summary>
        /// Measure time.
        /// OriginName: measure_time, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field MeasureTimeField = new Field.Builder()
            .Name(nameof(MeasureTime))
            .Title("measure_time")
            .Description("Measure time.")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(Int16Type.Default)

            .Build();
        private short _MeasureTime;
        public short MeasureTime { get => _MeasureTime; set { _MeasureTime = value; } }
        /// <summary>
        /// Record GUID.
        /// OriginName: record_guid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordGuidField = new Field.Builder()
            .Name(nameof(RecordGuid))
            .Title("record_guid")
            .Description("Record GUID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GpsFixType _GnssFixType;
        public GpsFixType GnssFixType { get => _GnssFixType; set => _GnssFixType = value; } 
        /// <summary>
        /// Number of satellites visible. If unknown, set to 255
        /// OriginName: gnss_satellites_visible, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GnssSatellitesVisibleField = new Field.Builder()
            .Name(nameof(GnssSatellitesVisible))
            .Title("gnss_satellites_visible")
            .Description("Number of satellites visible. If unknown, set to 255")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _GnssSatellitesVisible;
        public byte GnssSatellitesVisible { get => _GnssSatellitesVisible; set { _GnssSatellitesVisible = value; } }
        /// <summary>
        /// Code identification
        /// OriginName: code_id, Units: Letters, IsExtended: false
        /// </summary>
        public static readonly Field CodeIdField = new Field.Builder()
            .Name(nameof(CodeId))
            .Title("code_id")
            .Description("Code identification")
            .FormatString(string.Empty)
            .Units(@"Letters")
            .DataType(new ArrayType(UInt8Type.Default,4))

            .Build();
        public const int CodeIdMaxItemsCount = 4;
        public char[] CodeId { get; } = new char[4];
    }




        


#endregion


}

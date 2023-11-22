// MIT License
//
// Copyright (c) 2023 asv-soft (https://github.com/asv-soft)
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

// This code was generate by tool Asv.Mavlink.Shell version 3.2.5-alpha-11

using System;
using System.Text;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Asv.IO;

namespace Asv.Mavlink.V2.AsvSdr
{

    public static class AsvSdrHelper
    {
        public static void RegisterAsvSdrDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AsvSdrOutStatusPacket());
            src.Register(()=>new AsvSdrRecordRequestPacket());
            src.Register(()=>new AsvSdrRecordResponsePacket());
            src.Register(()=>new AsvSdrRecordPacket());
            src.Register(()=>new AsvSdrRecordDeleteRequestPacket());
            src.Register(()=>new AsvSdrRecordDeleteResponsePacket());
            src.Register(()=>new AsvSdrRecordTagRequestPacket());
            src.Register(()=>new AsvSdrRecordTagResponsePacket());
            src.Register(()=>new AsvSdrRecordTagPacket());
            src.Register(()=>new AsvSdrRecordTagDeleteRequestPacket());
            src.Register(()=>new AsvSdrRecordTagDeleteResponsePacket());
            src.Register(()=>new AsvSdrRecordDataRequestPacket());
            src.Register(()=>new AsvSdrRecordDataResponsePacket());
            src.Register(()=>new AsvSdrCalibAccPacket());
            src.Register(()=>new AsvSdrCalibTableReadPacket());
            src.Register(()=>new AsvSdrCalibTablePacket());
            src.Register(()=>new AsvSdrCalibTableRowReadPacket());
            src.Register(()=>new AsvSdrCalibTableRowPacket());
            src.Register(()=>new AsvSdrCalibTableUploadStartPacket());
            src.Register(()=>new AsvSdrCalibTableUploadReadCallbackPacket());
            src.Register(()=>new AsvSdrSignalRawPacket());
            src.Register(()=>new AsvSdrRecordDataLlzPacket());
            src.Register(()=>new AsvSdrRecordDataGpPacket());
            src.Register(()=>new AsvSdrRecordDataVorPacket());
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
    /// These flags encode supported mode.
    ///  ASV_SDR_CUSTOM_MODE_FLAG
    /// </summary>
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
    public class AsvSdrOutStatusPacket: PacketV2<AsvSdrOutStatusPayload>
    {
	    public const int PacketMessageId = 13100;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 145;
        public override bool WrapToV2Extension => true;

        public override AsvSdrOutStatusPayload Payload { get; } = new AsvSdrOutStatusPayload();

        public override string Name => "ASV_SDR_OUT_STATUS";
    }

    /// <summary>
    ///  ASV_SDR_OUT_STATUS
    /// </summary>
    public class AsvSdrOutStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 69; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 69; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 8; // SupportedModes
            sum+=8; //Size
            sum+=2; //RecordCount
            sum+=2; //CurrentMissionIndex
            sum+=CurrentRecordGuid.Length; //CurrentRecordGuid
            sum+= 1; // CurrentRecordMode
            sum+=CurrentRecordName.Length; //CurrentRecordName
            sum+= 1; // MissionState
            sum+= 1; // CalibState
            sum+=2; //CalibTableCount
            return (byte)sum;
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
            arraySize = /*ArrayLength*/28 - Math.Max(0,((/*PayloadByteSize*/69 - payloadSize - /*ExtendedFieldsLength*/3)/1 /*FieldTypeByteSize*/));
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
            /* PayloadByteSize = 69 */;
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
        public char[] CurrentRecordName { get; set; } = new char[28];
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
    }
    /// <summary>
    /// Request list of ASV_SDR_RECORD from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_REQUEST
    /// </summary>
    public class AsvSdrRecordRequestPacket: PacketV2<AsvSdrRecordRequestPayload>
    {
	    public const int PacketMessageId = 13101;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 91;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordRequestPayload Payload { get; } = new AsvSdrRecordRequestPayload();

        public override string Name => "ASV_SDR_RECORD_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_REQUEST
    /// </summary>
    public class AsvSdrRecordRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+=2; //Skip
            sum+=2; //Count
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
    public class AsvSdrRecordResponsePacket: PacketV2<AsvSdrRecordResponsePayload>
    {
	    public const int PacketMessageId = 13102;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 13;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordResponsePayload Payload { get; } = new AsvSdrRecordResponsePayload();

        public override string Name => "ASV_SDR_RECORD_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_RESPONSE
    /// </summary>
    public class AsvSdrRecordResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 5; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 5; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+=2; //ItemsCount
            sum+= 1; // Result
            return (byte)sum;
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
    public class AsvSdrRecordPacket: PacketV2<AsvSdrRecordPayload>
    {
	    public const int PacketMessageId = 13103;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 173;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordPayload Payload { get; } = new AsvSdrRecordPayload();

        public override string Name => "ASV_SDR_RECORD";
    }

    /// <summary>
    ///  ASV_SDR_RECORD
    /// </summary>
    public class AsvSdrRecordPayload : IPayload
    {
        public byte GetMaxByteSize() => 78; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 78; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Frequency
            sum+=8; //CreatedUnixUs
            sum+= 4; // DataType
            sum+=4; //DurationSec
            sum+=4; //DataCount
            sum+=4; //Size
            sum+=2; //TagCount
            sum+=RecordGuid.Length; //RecordGuid
            sum+=RecordName.Length; //RecordName
            return (byte)sum;
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
        public byte[] RecordGuid { get; } = new byte[16];
        /// <summary>
        /// Record name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.
        /// OriginName: record_name, Units: , IsExtended: false
        /// </summary>
        public char[] RecordName { get; set; } = new char[28];
        public byte GetRecordNameMaxItemsCount() => 28;
    }
    /// <summary>
    /// Request to delete ASV_SDR_RECORD items from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordDeleteRequestPacket: PacketV2<AsvSdrRecordDeleteRequestPayload>
    {
	    public const int PacketMessageId = 13104;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 181;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDeleteRequestPayload Payload { get; } = new AsvSdrRecordDeleteRequestPayload();

        public override string Name => "ASV_SDR_RECORD_DELETE_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordDeleteRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=RecordGuid.Length; //RecordGuid
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
        public byte GetRecordGuidMaxItemsCount() => 16;
    }
    /// <summary>
    /// Response for ASV_SDR_RECORD_DELETE_REQUEST.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordDeleteResponsePacket: PacketV2<AsvSdrRecordDeleteResponsePayload>
    {
	    public const int PacketMessageId = 13105;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 62;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDeleteResponsePayload Payload { get; } = new AsvSdrRecordDeleteResponsePayload();

        public override string Name => "ASV_SDR_RECORD_DELETE_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordDeleteResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 19; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 19; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+= 1; // Result
            sum+=RecordGuid.Length; //RecordGuid
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
        public byte GetRecordGuidMaxItemsCount() => 16;
    }
    /// <summary>
    /// Request list of ASV_SDR_RECORD_TAG from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_TAG_REQUEST
    /// </summary>
    public class AsvSdrRecordTagRequestPacket: PacketV2<AsvSdrRecordTagRequestPayload>
    {
	    public const int PacketMessageId = 13110;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 53;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordTagRequestPayload Payload { get; } = new AsvSdrRecordTagRequestPayload();

        public override string Name => "ASV_SDR_RECORD_TAG_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_REQUEST
    /// </summary>
    public class AsvSdrRecordTagRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 24; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+=2; //Skip
            sum+=2; //Count
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=RecordGuid.Length; //RecordGuid
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
        public byte GetRecordGuidMaxItemsCount() => 16;
    }
    /// <summary>
    /// Response for ASV_SDR_RECORD_TAG_REQUEST. If success, device additional send ASV_SDR_RECORD_TAG items from start to stop index.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_TAG_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagResponsePacket: PacketV2<AsvSdrRecordTagResponsePayload>
    {
	    public const int PacketMessageId = 13111;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 187;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordTagResponsePayload Payload { get; } = new AsvSdrRecordTagResponsePayload();

        public override string Name => "ASV_SDR_RECORD_TAG_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 5; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 5; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+=2; //ItemsCount
            sum+= 1; // Result
            return (byte)sum;
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
    public class AsvSdrRecordTagPacket: PacketV2<AsvSdrRecordTagPayload>
    {
	    public const int PacketMessageId = 13112;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 220;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordTagPayload Payload { get; } = new AsvSdrRecordTagPayload();

        public override string Name => "ASV_SDR_RECORD_TAG";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG
    /// </summary>
    public class AsvSdrRecordTagPayload : IPayload
    {
        public byte GetMaxByteSize() => 57; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 57; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=RecordGuid.Length; //RecordGuid
            sum+=TagGuid.Length; //TagGuid
            sum+=TagName.Length; //TagName
            sum+= 1; // TagType
            sum+=TagValue.Length; //TagValue
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// Tag GUID.
        /// OriginName: tag_guid, Units: , IsExtended: false
        /// </summary>
        public byte[] TagGuid { get; } = new byte[16];
        /// <summary>
        /// Tag name, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
        /// OriginName: tag_name, Units: , IsExtended: false
        /// </summary>
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
        public byte[] TagValue { get; } = new byte[8];
    }
    /// <summary>
    /// Request to delete ASV_SDR_RECORD_TAG from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_TAG_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordTagDeleteRequestPacket: PacketV2<AsvSdrRecordTagDeleteRequestPayload>
    {
	    public const int PacketMessageId = 13113;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 233;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordTagDeleteRequestPayload Payload { get; } = new AsvSdrRecordTagDeleteRequestPayload();

        public override string Name => "ASV_SDR_RECORD_TAG_DELETE_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordTagDeleteRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 36; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 36; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=RecordGuid.Length; //RecordGuid
            sum+=TagGuid.Length; //TagGuid
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// Tag GUID.
        /// OriginName: tag_guid, Units: , IsExtended: false
        /// </summary>
        public byte[] TagGuid { get; } = new byte[16];
    }
    /// <summary>
    /// Response for ASV_SDR_RECORD_TAG_DELETE_REQUEST.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_TAG_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagDeleteResponsePacket: PacketV2<AsvSdrRecordTagDeleteResponsePayload>
    {
	    public const int PacketMessageId = 13114;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 100;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordTagDeleteResponsePayload Payload { get; } = new AsvSdrRecordTagDeleteResponsePayload();

        public override string Name => "ASV_SDR_RECORD_TAG_DELETE_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagDeleteResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 35; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 35; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+= 1; // Result
            sum+=RecordGuid.Length; //RecordGuid
            sum+=TagGuid.Length; //TagGuid
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
        public byte GetRecordGuidMaxItemsCount() => 16;
        /// <summary>
        /// Tag GUID.
        /// OriginName: tag_guid, Units: , IsExtended: false
        /// </summary>
        public byte[] TagGuid { get; } = new byte[16];
    }
    /// <summary>
    /// Request list of ASV_SDR_RECORD_DATA_* items from the system/component.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DATA_REQUEST
    /// </summary>
    public class AsvSdrRecordDataRequestPacket: PacketV2<AsvSdrRecordDataRequestPayload>
    {
	    public const int PacketMessageId = 13120;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 101;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDataRequestPayload Payload { get; } = new AsvSdrRecordDataRequestPayload();

        public override string Name => "ASV_SDR_RECORD_DATA_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_REQUEST
    /// </summary>
    public class AsvSdrRecordDataRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 28; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 28; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Skip
            sum+=4; //Count
            sum+=2; //RequestId
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=RecordGuid.Length; //RecordGuid
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
        public byte GetRecordGuidMaxItemsCount() => 16;
    }
    /// <summary>
    /// Response for ASV_SDR_RECORD_DATA_REQUEST. If success, device additional send ASV_SDR_RECORD_DATA_* items from start to stop index.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DATA_RESPONSE
    /// </summary>
    public class AsvSdrRecordDataResponsePacket: PacketV2<AsvSdrRecordDataResponsePayload>
    {
	    public const int PacketMessageId = 13121;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 39;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDataResponsePayload Payload { get; } = new AsvSdrRecordDataResponsePayload();

        public override string Name => "ASV_SDR_RECORD_DATA_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_RESPONSE
    /// </summary>
    public class AsvSdrRecordDataResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 27; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 27; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 4; // DataType
            sum+=4; //ItemsCount
            sum+=2; //RequestId
            sum+= 1; // Result
            sum+=RecordGuid.Length; //RecordGuid
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
        public byte GetRecordGuidMaxItemsCount() => 16;
    }
    /// <summary>
    /// Response for ASV_SDR_CALIB_* requests. Result from ASV_SDR_CALIB_TABLE_READ, ASV_SDR_CALIB_TABLE_ROW_READ, ASV_SDR_CALIB_TABLE_UPLOAD_START messages.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_CALIB_ACC
    /// </summary>
    public class AsvSdrCalibAccPacket: PacketV2<AsvSdrCalibAccPayload>
    {
	    public const int PacketMessageId = 13124;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 136;
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibAccPayload Payload { get; } = new AsvSdrCalibAccPayload();

        public override string Name => "ASV_SDR_CALIB_ACC";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_ACC
    /// </summary>
    public class AsvSdrCalibAccPayload : IPayload
    {
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+= 1; // Result
            return (byte)sum;
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
    public class AsvSdrCalibTableReadPacket: PacketV2<AsvSdrCalibTableReadPayload>
    {
	    public const int PacketMessageId = 13125;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 8;
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTableReadPayload Payload { get; } = new AsvSdrCalibTableReadPayload();

        public override string Name => "ASV_SDR_CALIB_TABLE_READ";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE_READ
    /// </summary>
    public class AsvSdrCalibTableReadPayload : IPayload
    {
        public byte GetMaxByteSize() => 6; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //TableIndex
            sum+=2; //RequestId
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
    public class AsvSdrCalibTablePacket: PacketV2<AsvSdrCalibTablePayload>
    {
	    public const int PacketMessageId = 13126;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 194;
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTablePayload Payload { get; } = new AsvSdrCalibTablePayload();

        public override string Name => "ASV_SDR_CALIB_TABLE";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE
    /// </summary>
    public class AsvSdrCalibTablePayload : IPayload
    {
        public byte GetMaxByteSize() => 40; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 40; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //CreatedUnixUs
            sum+=2; //TableIndex
            sum+=2; //RowCount
            sum+=TableName.Length; //TableName
            return (byte)sum;
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
        public char[] TableName { get; set; } = new char[28];
        public byte GetTableNameMaxItemsCount() => 28;
    }
    /// <summary>
    /// Request to read ASV_SDR_CALIB_TABLE_ROW from the system/component. If success, device send ASV_SDR_CALIB_TABLE_ROW or ASV_SDR_CALIB_ACC, when error occured.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_CALIB_TABLE_ROW_READ
    /// </summary>
    public class AsvSdrCalibTableRowReadPacket: PacketV2<AsvSdrCalibTableRowReadPayload>
    {
	    public const int PacketMessageId = 13127;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 2;
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTableRowReadPayload Payload { get; } = new AsvSdrCalibTableRowReadPayload();

        public override string Name => "ASV_SDR_CALIB_TABLE_ROW_READ";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE_ROW_READ
    /// </summary>
    public class AsvSdrCalibTableRowReadPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+=2; //TableIndex
            sum+=2; //RowIndex
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
    public class AsvSdrCalibTableRowPacket: PacketV2<AsvSdrCalibTableRowPayload>
    {
	    public const int PacketMessageId = 13128;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 179;
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTableRowPayload Payload { get; } = new AsvSdrCalibTableRowPayload();

        public override string Name => "ASV_SDR_CALIB_TABLE_ROW";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE_ROW
    /// </summary>
    public class AsvSdrCalibTableRowPayload : IPayload
    {
        public byte GetMaxByteSize() => 26; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 26; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //RefFreq
            sum+=4; //RefPower
            sum+=4; //RefValue
            sum+=4; //Adjustment
            sum+=2; //TableIndex
            sum+=2; //RowIndex
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
    public class AsvSdrCalibTableUploadStartPacket: PacketV2<AsvSdrCalibTableUploadStartPayload>
    {
	    public const int PacketMessageId = 13129;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 40;
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTableUploadStartPayload Payload { get; } = new AsvSdrCalibTableUploadStartPayload();

        public override string Name => "ASV_SDR_CALIB_TABLE_UPLOAD_START";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE_UPLOAD_START
    /// </summary>
    public class AsvSdrCalibTableUploadStartPayload : IPayload
    {
        public byte GetMaxByteSize() => 16; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //CreatedUnixUs
            sum+=2; //TableIndex
            sum+=2; //RequestId
            sum+=2; //RowCount
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
    public class AsvSdrCalibTableUploadReadCallbackPacket: PacketV2<AsvSdrCalibTableUploadReadCallbackPayload>
    {
	    public const int PacketMessageId = 13130;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 156;
        public override bool WrapToV2Extension => true;

        public override AsvSdrCalibTableUploadReadCallbackPayload Payload { get; } = new AsvSdrCalibTableUploadReadCallbackPayload();

        public override string Name => "ASV_SDR_CALIB_TABLE_UPLOAD_READ_CALLBACK";
    }

    /// <summary>
    ///  ASV_SDR_CALIB_TABLE_UPLOAD_READ_CALLBACK
    /// </summary>
    public class AsvSdrCalibTableUploadReadCallbackPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+=2; //TableIndex
            sum+=2; //RowIndex
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
    public class AsvSdrSignalRawPacket: PacketV2<AsvSdrSignalRawPayload>
    {
	    public const int PacketMessageId = 13134;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 27;
        public override bool WrapToV2Extension => true;

        public override AsvSdrSignalRawPayload Payload { get; } = new AsvSdrSignalRawPayload();

        public override string Name => "ASV_SDR_SIGNAL_RAW";
    }

    /// <summary>
    ///  ASV_SDR_SIGNAL_RAW
    /// </summary>
    public class AsvSdrSignalRawPayload : IPayload
    {
        public byte GetMaxByteSize() => 230; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 230; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+=4; //Min
            sum+=4; //Max
            sum+=2; //Start
            sum+=2; //Total
            sum+=SignalName.Length; //SignalName
            sum+= 1; // Format
            sum+=1; //Count
            sum+=Data.Length; //Data
            return (byte)sum;
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
        public byte[] Data { get; set; } = new byte[200];
        public byte GetDataMaxItemsCount() => 200;
    }
    /// <summary>
    /// LLZ reciever record data.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DATA_LLZ
    /// </summary>
    public class AsvSdrRecordDataLlzPacket: PacketV2<AsvSdrRecordDataLlzPayload>
    {
	    public const int PacketMessageId = 13135;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 2;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDataLlzPayload Payload { get; } = new AsvSdrRecordDataLlzPayload();

        public override string Name => "ASV_SDR_RECORD_DATA_LLZ";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_LLZ
    /// </summary>
    public class AsvSdrRecordDataLlzPayload : IPayload
    {
        public byte GetMaxByteSize() => 186; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 186; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+=8; //TotalFreq
            sum+=4; //DataIndex
            sum+=4; //GnssLat
            sum+=4; //GnssLon
            sum+=4; //GnssAlt
            sum+=4; //GnssAltEllipsoid
            sum+=4; //GnssHAcc
            sum+=4; //GnssVAcc
            sum+=4; //GnssVelAcc
            sum+=4; //Lat
            sum+=4; //Lon
            sum+=4; //Alt
            sum+=4; //RelativeAlt
            sum+=4; //Roll
            sum+=4; //Pitch
            sum+=4; //Yaw
            sum+=4; //CrsPower
            sum+=4; //CrsAm90
            sum+=4; //CrsAm150
            sum+=4; //ClrPower
            sum+=4; //ClrAm90
            sum+=4; //ClrAm150
            sum+=4; //TotalPower
            sum+=4; //TotalFieldStrength
            sum+=4; //TotalAm90
            sum+=4; //TotalAm150
            sum+=4; //Phi90CrsVsClr
            sum+=4; //Phi150CrsVsClr
            sum+=4; //CodeIdAm1020
            sum+=2; //GnssEph
            sum+=2; //GnssEpv
            sum+=2; //GnssVel
            sum+=2; //Vx
            sum+=2; //Vy
            sum+=2; //Vz
            sum+=2; //Hdg
            sum+=2; //CrsCarrierOffset
            sum+=2; //CrsFreq90
            sum+=2; //CrsFreq150
            sum+=2; //ClrCarrierOffset
            sum+=2; //ClrFreq90
            sum+=2; //ClrFreq150
            sum+=2; //TotalCarrierOffset
            sum+=2; //TotalFreq90
            sum+=2; //TotalFreq150
            sum+=2; //CodeIdFreq1020
            sum+=2; //MeasureTime
            sum+=RecordGuid.Length; //RecordGuid
            sum+= 1; // GnssFixType
            sum+=1; //GnssSatellitesVisible
            sum+=CodeId.Length; //CodeId
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
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
        public char[] CodeId { get; } = new char[4];
    }
    /// <summary>
    /// GP reciever record data.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_SDR_RECORD_DATA_GP
    /// </summary>
    public class AsvSdrRecordDataGpPacket: PacketV2<AsvSdrRecordDataGpPayload>
    {
	    public const int PacketMessageId = 13136;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 233;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDataGpPayload Payload { get; } = new AsvSdrRecordDataGpPayload();

        public override string Name => "ASV_SDR_RECORD_DATA_GP";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_GP
    /// </summary>
    public class AsvSdrRecordDataGpPayload : IPayload
    {
        public byte GetMaxByteSize() => 176; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 176; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+=8; //TotalFreq
            sum+=4; //DataIndex
            sum+=4; //GnssLat
            sum+=4; //GnssLon
            sum+=4; //GnssAlt
            sum+=4; //GnssAltEllipsoid
            sum+=4; //GnssHAcc
            sum+=4; //GnssVAcc
            sum+=4; //GnssVelAcc
            sum+=4; //Lat
            sum+=4; //Lon
            sum+=4; //Alt
            sum+=4; //RelativeAlt
            sum+=4; //Roll
            sum+=4; //Pitch
            sum+=4; //Yaw
            sum+=4; //CrsPower
            sum+=4; //CrsAm90
            sum+=4; //CrsAm150
            sum+=4; //ClrPower
            sum+=4; //ClrAm90
            sum+=4; //ClrAm150
            sum+=4; //TotalPower
            sum+=4; //TotalFieldStrength
            sum+=4; //TotalAm90
            sum+=4; //TotalAm150
            sum+=4; //Phi90CrsVsClr
            sum+=4; //Phi150CrsVsClr
            sum+=2; //GnssEph
            sum+=2; //GnssEpv
            sum+=2; //GnssVel
            sum+=2; //Vx
            sum+=2; //Vy
            sum+=2; //Vz
            sum+=2; //Hdg
            sum+=2; //CrsCarrierOffset
            sum+=2; //CrsFreq90
            sum+=2; //CrsFreq150
            sum+=2; //ClrCarrierOffset
            sum+=2; //ClrFreq90
            sum+=2; //ClrFreq150
            sum+=2; //TotalCarrierOffset
            sum+=2; //TotalFreq90
            sum+=2; //TotalFreq150
            sum+=2; //MeasureTime
            sum+=RecordGuid.Length; //RecordGuid
            sum+= 1; // GnssFixType
            sum+=1; //GnssSatellitesVisible
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
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
    public class AsvSdrRecordDataVorPacket: PacketV2<AsvSdrRecordDataVorPayload>
    {
	    public const int PacketMessageId = 13137;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 250;
        public override bool WrapToV2Extension => true;

        public override AsvSdrRecordDataVorPayload Payload { get; } = new AsvSdrRecordDataVorPayload();

        public override string Name => "ASV_SDR_RECORD_DATA_VOR";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_VOR
    /// </summary>
    public class AsvSdrRecordDataVorPayload : IPayload
    {
        public byte GetMaxByteSize() => 150; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 150; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+=8; //TotalFreq
            sum+=4; //DataIndex
            sum+=4; //GnssLat
            sum+=4; //GnssLon
            sum+=4; //GnssAlt
            sum+=4; //GnssAltEllipsoid
            sum+=4; //GnssHAcc
            sum+=4; //GnssVAcc
            sum+=4; //GnssVelAcc
            sum+=4; //Lat
            sum+=4; //Lon
            sum+=4; //Alt
            sum+=4; //RelativeAlt
            sum+=4; //Roll
            sum+=4; //Pitch
            sum+=4; //Yaw
            sum+=4; //Azimuth
            sum+=4; //Power
            sum+=4; //FieldStrength
            sum+=4; //Am30
            sum+=4; //Am9960
            sum+=4; //Deviation
            sum+=4; //CodeIdAm1020
            sum+=2; //GnssEph
            sum+=2; //GnssEpv
            sum+=2; //GnssVel
            sum+=2; //Vx
            sum+=2; //Vy
            sum+=2; //Vz
            sum+=2; //Hdg
            sum+=2; //CarrierOffset
            sum+=2; //Freq30
            sum+=2; //Freq9960
            sum+=2; //CodeIdFreq1020
            sum+=2; //MeasureTime
            sum+=RecordGuid.Length; //RecordGuid
            sum+= 1; // GnssFixType
            sum+=1; //GnssSatellitesVisible
            sum+=CodeId.Length; //CodeId
            return (byte)sum;
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
        public byte[] RecordGuid { get; set; } = new byte[16];
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
        public char[] CodeId { get; } = new char[4];
    }


#endregion


}

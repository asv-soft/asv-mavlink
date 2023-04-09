// MIT License
//
// Copyright (c) 2018 Alexey (https://github.com/asvol)
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

// This code was generate by tool Asv.Mavlink.Shell version 2.0.2

using System;
using System.Text;
using Asv.Mavlink.V2.Common;
using Asv.IO;

namespace Asv.Mavlink.V2.AsvSdr
{

    public static class AsvSdrHelper
    {
        public static void RegisterAsvSdrDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AsvSdrOutStatusPacket());
            src.Register(()=>new AsvSdrRecordListRequestPacket());
            src.Register(()=>new AsvSdrRecordListResponsePacket());
            src.Register(()=>new AsvSdrRecordReadRequestPacket());
            src.Register(()=>new AsvSdrRecordPacket());
            src.Register(()=>new AsvSdrRecordDeleteRequestPacket());
            src.Register(()=>new AsvSdrRecordDeleteResponsePacket());
            src.Register(()=>new AsvSdrRecordTagListRequestPacket());
            src.Register(()=>new AsvSdrRecordTagListResponsePacket());
            src.Register(()=>new AsvSdrRecordTagReadRequestPacket());
            src.Register(()=>new AsvSdrRecordTagPacket());
            src.Register(()=>new AsvSdrRecordTagDeleteRequestPacket());
            src.Register(()=>new AsvSdrRecordTagDeleteResponsePacket());
            src.Register(()=>new AsvSdrRecordDataListRequestPacket());
            src.Register(()=>new AsvSdrRecordDataListResponsePacket());
            src.Register(()=>new AsvSdrRecordDataReadRequestPacket());
            src.Register(()=>new AsvSdrRecordDataDeleteRequestPacket());
            src.Register(()=>new AsvSdrRecordDataDeleteResponsePacket());
            src.Register(()=>new AsvSdrRecordDataIlsPacket());
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
        /// Start one of ASV_SDR_CUSTOM_MODE
        /// Param 1 - Mode (uint32_t, see ASV_SDR_CUSTOM_MODE).
        /// Param 2 - Frequency in Hz, 0-3 bytes of uint_64, ignored for IDLE mode (uint32).
        /// Param 3 - Frequency in Hz, 4-7 bytes of uint_64, ignored for IDLE mode (uint32).
        /// Param 4 - Stream rate for sending record data in Hz, ignored for IDLE mode (float).
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_SDR_SET_MODE
        /// </summary>
        MavCmdAsvSdrSetMode = 13100,
        /// <summary>
        /// Start recoring data with unique name (max 28 chars)
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
        /// Stop recoring data
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
        /// Set custom tag to record. Used to store additional data for records.
        /// Param 1 - Record index (uint16) , ASV_SDR_RECORD_TAG_FLAG (uint8) and ASV_SDR_RECORD_TAG_TYPE (uint8).
        /// Param 2 - Tag name: 0-3 chars (char[4]).
        /// Param 3 - Tag name: 4-7 chars (char[4]).
        /// Param 4 - Tag name: 8-11 chars (char[4]).
        /// Param 5 - Tag name: 12-15 chars (char[4]).
        /// Param 6 - Tag value data 0-3 bytes depends on the type of tag.
        /// Param 7 - Tag value data 4-7 bytes depends on the type of tag.
        /// MAV_CMD_ASV_SDR_SET_RECORD_TAG
        /// </summary>
        MavCmdAsvSdrSetRecordTag = 13103,
    }

    /// <summary>
    /// Specifies the flags of a record tag in MAV_CMD_ASV_SDR_SET_RECORD_TAG command (unit8_t).
    ///  ASV_SDR_RECORD_TAG_FLAG
    /// </summary>
    public enum AsvSdrRecordTagFlag:uint
    {
        /// <summary>
        /// Default value(no flags)
        /// ASV_SDR_RECORD_TAG_FLAG_NONE
        /// </summary>
        AsvSdrRecordTagFlagNone = 0,
        /// <summary>
        /// If the flag is set, the tag will be set for the current record. Record index in MAV_CMD_ASV_SDR_SET_RECORD_TAG will be ignored.
        /// ASV_SDR_RECORD_TAG_FLAG_FOR_CURRENT
        /// </summary>
        AsvSdrRecordTagFlagForCurrent = 1,
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
        /// ASV_SDR_RECORD_TAG_TYPE_STRING
        /// </summary>
        AsvSdrRecordTagTypeString = 4,
    }

    /// <summary>
    /// A mapping of SDR payload modes for custom_mode field of heartbeat
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
        /// Localizer measure mode. In this mode shuld send and record ASV_SDR_RECORD_DATA_ILS
        /// ASV_SDR_CUSTOM_MODE_LLZ
        /// </summary>
        AsvSdrCustomModeLlz = 1,
        /// <summary>
        /// Glide Path measure mode. In this mode shuld send and record ASV_SDR_RECORD_DATA_ILS
        /// ASV_SDR_CUSTOM_MODE_GP
        /// </summary>
        AsvSdrCustomModeGp = 2,
        /// <summary>
        /// VOR measure mode. In this mode shuld send and record ASV_SDR_RECORD_DATA_VOR
        /// ASV_SDR_CUSTOM_MODE_VOR
        /// </summary>
        AsvSdrCustomModeVor = 3,
    }

    /// <summary>
    /// These flags encode supported mode.
    ///  ASV_SDR_SUPPORT_MODE_FLAG
    /// </summary>
    public enum AsvSdrSupportModeFlag:uint
    {
        /// <summary>
        /// ASV_SDR_SUPPORT_MODE_LLZ
        /// </summary>
        AsvSdrSupportModeLlz = 1,
        /// <summary>
        /// ASV_SDR_SUPPORT_MODE_GP
        /// </summary>
        AsvSdrSupportModeGp = 2,
        /// <summary>
        /// ASV_SDR_SUPPORT_MODE_VOR
        /// </summary>
        AsvSdrSupportModeVor = 4,
    }

    /// <summary>
    /// Specifies the flags of a record (unit64_t).
    ///  ASV_SDR_RECORD_STATE_FLAG
    /// </summary>
    public enum AsvSdrRecordStateFlag:uint
    {
        /// <summary>
        /// Specify recording now is started.
        /// ASV_SDR_RECORD_FLAG_STARTED
        /// </summary>
        AsvSdrRecordFlagStarted = 1,
    }

    /// <summary>
    /// ACK / NACK / ERROR values as a result of ASV_SDR_*_REQUEST_LIST or ASV_SDR_*_DELETE commands.
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
    }


#endregion

#region Messages

    /// <summary>
    /// SDR payload status message. Send with 1 Hz frequency.
    ///  ASV_SDR_OUT_STATUS
    /// </summary>
    public class AsvSdrOutStatusPacket: PacketV2<AsvSdrOutStatusPayload>
    {
	    public const int PacketMessageId = 13100;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 184;

        public override AsvSdrOutStatusPayload Payload { get; } = new AsvSdrOutStatusPayload();

        public override string Name => "ASV_SDR_OUT_STATUS";
    }

    /// <summary>
    ///  ASV_SDR_OUT_STATUS
    /// </summary>
    public class AsvSdrOutStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 18; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 18; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            SupportedModes = (AsvSdrSupportModeFlag)BinSerialize.ReadULong(ref buffer);
            Size = BinSerialize.ReadULong(ref buffer);
            RecordCount = BinSerialize.ReadUShort(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,(ulong)SupportedModes);
            BinSerialize.WriteULong(ref buffer,Size);
            BinSerialize.WriteUShort(ref buffer,RecordCount);
            /* PayloadByteSize = 18 */;
        }



        /// <summary>
        /// Supported ASV_SDR_CUSTOM_MODE.
        /// OriginName: supported_modes, Units: , IsExtended: false
        /// </summary>
        public AsvSdrSupportModeFlag SupportedModes { get; set; }
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
    }
    /// <summary>
    /// Request the overall list of ASV_SDR_RECORD info items from the system/component.
    ///  ASV_SDR_RECORD_LIST_REQUEST
    /// </summary>
    public class AsvSdrRecordListRequestPacket: PacketV2<AsvSdrRecordListRequestPayload>
    {
	    public const int PacketMessageId = 13101;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 25;

        public override AsvSdrRecordListRequestPayload Payload { get; } = new AsvSdrRecordListRequestPayload();

        public override string Name => "ASV_SDR_RECORD_LIST_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_LIST_REQUEST
    /// </summary>
    public class AsvSdrRecordListRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 2; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 2; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 2 */;
        }



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
    /// Response for ASV_SDR_RECORD_LIST_REQUEST request.
    ///  ASV_SDR_RECORD_LIST_RESPONSE
    /// </summary>
    public class AsvSdrRecordListResponsePacket: PacketV2<AsvSdrRecordListResponsePayload>
    {
	    public const int PacketMessageId = 13102;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 0;

        public override AsvSdrRecordListResponsePayload Payload { get; } = new AsvSdrRecordListResponsePayload();

        public override string Name => "ASV_SDR_RECORD_LIST_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_LIST_RESPONSE
    /// </summary>
    public class AsvSdrRecordListResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            ItemsCount = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,ItemsCount);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 3 */;
        }



        /// <summary>
        /// Number of items for transmition.
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
    /// Request to read ASV_SDR_RECORD with either record_index from the system/component.
    ///  ASV_SDR_RECORD_READ_REQUEST
    /// </summary>
    public class AsvSdrRecordReadRequestPacket: PacketV2<AsvSdrRecordReadRequestPayload>
    {
	    public const int PacketMessageId = 13103;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 12;

        public override AsvSdrRecordReadRequestPayload Payload { get; } = new AsvSdrRecordReadRequestPayload();

        public override string Name => "ASV_SDR_RECORD_READ_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_READ_REQUEST
    /// </summary>
    public class AsvSdrRecordReadRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 4; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 4; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 4 */;
        }



        /// <summary>
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
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
    /// SDR payload record info.
    ///  ASV_SDR_RECORD
    /// </summary>
    public class AsvSdrRecordPacket: PacketV2<AsvSdrRecordPayload>
    {
	    public const int PacketMessageId = 13104;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 177;

        public override AsvSdrRecordPayload Payload { get; } = new AsvSdrRecordPayload();

        public override string Name => "ASV_SDR_RECORD";
    }

    /// <summary>
    ///  ASV_SDR_RECORD
    /// </summary>
    public class AsvSdrRecordPayload : IPayload
    {
        public byte GetMaxByteSize() => 74; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 74; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Frequency = BinSerialize.ReadULong(ref buffer);
            State = (AsvSdrRecordStateFlag)BinSerialize.ReadULong(ref buffer);
            CreatedUnixUs = BinSerialize.ReadULong(ref buffer);
            RecordMode = (AsvSdrCustomMode)BinSerialize.ReadUInt(ref buffer);
            DurationSec = BinSerialize.ReadUInt(ref buffer);
            DataCount = BinSerialize.ReadUInt(ref buffer);
            Size = BinSerialize.ReadUInt(ref buffer);
            Index = BinSerialize.ReadUShort(ref buffer);
            DataMessageId = BinSerialize.ReadUShort(ref buffer);
            TagCount = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/28 - Math.Max(0,((/*PayloadByteSize*/74 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Name = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Name)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Name.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Frequency);
            BinSerialize.WriteULong(ref buffer,(ulong)State);
            BinSerialize.WriteULong(ref buffer,CreatedUnixUs);
            BinSerialize.WriteUInt(ref buffer,(uint)RecordMode);
            BinSerialize.WriteUInt(ref buffer,DurationSec);
            BinSerialize.WriteUInt(ref buffer,DataCount);
            BinSerialize.WriteUInt(ref buffer,Size);
            BinSerialize.WriteUShort(ref buffer,Index);
            BinSerialize.WriteUShort(ref buffer,DataMessageId);
            BinSerialize.WriteUShort(ref buffer,TagCount);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Name)
                {
                    Encoding.ASCII.GetBytes(charPointer, Name.Length, bytePointer, Name.Length);
                }
            }
            buffer = buffer.Slice(Name.Length);
            
            /* PayloadByteSize = 74 */;
        }



        /// <summary>
        /// Frequency in Hz.
        /// OriginName: frequency, Units: , IsExtended: false
        /// </summary>
        public ulong Frequency { get; set; }
        /// <summary>
        /// Current state.
        /// OriginName: state, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRecordStateFlag State { get; set; }
        /// <summary>
        /// Created timestamp (UNIX epoch time).
        /// OriginName: created_unix_us, Units: us, IsExtended: false
        /// </summary>
        public ulong CreatedUnixUs { get; set; }
        /// <summary>
        /// Record mode.
        /// OriginName: record_mode, Units: , IsExtended: false
        /// </summary>
        public AsvSdrCustomMode RecordMode { get; set; }
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
        /// Record index in storage.
        /// OriginName: index, Units: , IsExtended: false
        /// </summary>
        public ushort Index { get; set; }
        /// <summary>
        /// Record data type (message id, e.g. 13120 for ASV_SDR_RECORD_DATA_ILS).
        /// OriginName: data_message_id, Units: , IsExtended: false
        /// </summary>
        public ushort DataMessageId { get; set; }
        /// <summary>
        /// Tag items count.
        /// OriginName: tag_count, Units: , IsExtended: false
        /// </summary>
        public ushort TagCount { get; set; }
        /// <summary>
        /// Record unique name, terminated by NULL if the length is less than 28 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 28 chars - applications have to provide 28+1 bytes storage if the name is stored as string.
        /// OriginName: name, Units: , IsExtended: false
        /// </summary>
        public char[] Name { get; set; } = new char[28];
        public byte GetNameMaxItemsCount() => 28;
    }
    /// <summary>
    /// Request to delete ASV_SDR_RECORD with either record_index from the system/component.
    ///  ASV_SDR_RECORD_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordDeleteRequestPacket: PacketV2<AsvSdrRecordDeleteRequestPayload>
    {
	    public const int PacketMessageId = 13105;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 134;

        public override AsvSdrRecordDeleteRequestPayload Payload { get; } = new AsvSdrRecordDeleteRequestPayload();

        public override string Name => "ASV_SDR_RECORD_DELETE_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordDeleteRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 6; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RecordStartIndex = BinSerialize.ReadUShort(ref buffer);
            RecordStopIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RecordStartIndex);
            BinSerialize.WriteUShort(ref buffer,RecordStopIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 6 */;
        }



        /// <summary>
        /// Record start index in record.
        /// OriginName: record_start_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordStartIndex { get; set; }
        /// <summary>
        /// Record stop index in record.
        /// OriginName: record_stop_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordStopIndex { get; set; }
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
    /// Request to delete ASV_SDR_RECORD with either record_index from the system/component.
    ///  ASV_SDR_RECORD_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordDeleteResponsePacket: PacketV2<AsvSdrRecordDeleteResponsePayload>
    {
	    public const int PacketMessageId = 13106;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 13;

        public override AsvSdrRecordDeleteResponsePayload Payload { get; } = new AsvSdrRecordDeleteResponsePayload();

        public override string Name => "ASV_SDR_RECORD_DELETE_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordDeleteResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 7; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RecordStartIndex = BinSerialize.ReadUShort(ref buffer);
            RecordStopIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RecordStartIndex);
            BinSerialize.WriteUShort(ref buffer,RecordStopIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 7 */;
        }



        /// <summary>
        /// Record start index in record.
        /// OriginName: record_start_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordStartIndex { get; set; }
        /// <summary>
        /// Record stop index in record.
        /// OriginName: record_stop_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordStopIndex { get; set; }
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
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRequestAck Result { get; set; }
    }
    /// <summary>
    /// Request the overall list of ASV_SDR_RECORD_TAG items from the system/component.
    ///  ASV_SDR_RECORD_TAG_LIST_REQUEST
    /// </summary>
    public class AsvSdrRecordTagListRequestPacket: PacketV2<AsvSdrRecordTagListRequestPayload>
    {
	    public const int PacketMessageId = 13110;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 40;

        public override AsvSdrRecordTagListRequestPayload Payload { get; } = new AsvSdrRecordTagListRequestPayload();

        public override string Name => "ASV_SDR_RECORD_TAG_LIST_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_LIST_REQUEST
    /// </summary>
    public class AsvSdrRecordTagListRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 4; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 4; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 4 */;
        }



        /// <summary>
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
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
    /// Response for ASV_SDR_RECORD_TAG_LIST_REQUEST.
    ///  ASV_SDR_RECORD_TAG_LIST_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagListResponsePacket: PacketV2<AsvSdrRecordTagListResponsePayload>
    {
	    public const int PacketMessageId = 13111;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 236;

        public override AsvSdrRecordTagListResponsePayload Payload { get; } = new AsvSdrRecordTagListResponsePayload();

        public override string Name => "ASV_SDR_RECORD_TAG_LIST_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_LIST_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagListResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            ItemsCount = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,ItemsCount);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 3 */;
        }



        /// <summary>
        /// Number of items for transmition.
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
    /// Request to read ASV_SDR_RECORD_TAG with either record_index and tag_index from the system/component.
    ///  ASV_SDR_RECORD_TAG_READ_REQUEST
    /// </summary>
    public class AsvSdrRecordTagReadRequestPacket: PacketV2<AsvSdrRecordTagReadRequestPayload>
    {
	    public const int PacketMessageId = 13112;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 177;

        public override AsvSdrRecordTagReadRequestPayload Payload { get; } = new AsvSdrRecordTagReadRequestPayload();

        public override string Name => "ASV_SDR_RECORD_TAG_READ_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_READ_REQUEST
    /// </summary>
    public class AsvSdrRecordTagReadRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 6; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            TagIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
            BinSerialize.WriteUShort(ref buffer,TagIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 6 */;
        }



        /// <summary>
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
        /// <summary>
        /// Tag index.
        /// OriginName: tag_index, Units: , IsExtended: false
        /// </summary>
        public ushort TagIndex { get; set; }
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
    /// Request to read info with either tag_index and record_index from the system/component.
    ///  ASV_SDR_RECORD_TAG
    /// </summary>
    public class AsvSdrRecordTagPacket: PacketV2<AsvSdrRecordTagPayload>
    {
	    public const int PacketMessageId = 13113;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 161;

        public override AsvSdrRecordTagPayload Payload { get; } = new AsvSdrRecordTagPayload();

        public override string Name => "ASV_SDR_RECORD_TAG";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG
    /// </summary>
    public class AsvSdrRecordTagPayload : IPayload
    {
        public byte GetMaxByteSize() => 29; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 29; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TagIndex = BinSerialize.ReadUShort(ref buffer);
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/29 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            TagName = new char[arraySize];
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
            BinSerialize.WriteUShort(ref buffer,TagIndex);
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
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
            /* PayloadByteSize = 29 */;
        }



        /// <summary>
        /// Record tag items count
        /// OriginName: tag_index, Units: , IsExtended: false
        /// </summary>
        public ushort TagIndex { get; set; }
        /// <summary>
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
        /// <summary>
        /// Tag name, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
        /// OriginName: tag_name, Units: , IsExtended: false
        /// </summary>
        public char[] TagName { get; set; } = new char[16];
        public byte GetTagNameMaxItemsCount() => 16;
        /// <summary>
        /// Record index in storage.
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
    /// Request to delete ASV_SDR_RECORD_TAG with either record_index and tag_index from the system/component.
    ///  ASV_SDR_RECORD_TAG_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordTagDeleteRequestPacket: PacketV2<AsvSdrRecordTagDeleteRequestPayload>
    {
	    public const int PacketMessageId = 13114;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 201;

        public override AsvSdrRecordTagDeleteRequestPayload Payload { get; } = new AsvSdrRecordTagDeleteRequestPayload();

        public override string Name => "ASV_SDR_RECORD_TAG_DELETE_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordTagDeleteRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            TagStartIndex = BinSerialize.ReadUShort(ref buffer);
            TagStopIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
            BinSerialize.WriteUShort(ref buffer,TagStartIndex);
            BinSerialize.WriteUShort(ref buffer,TagStopIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 8 */;
        }



        /// <summary>
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
        /// <summary>
        /// Tag start index.
        /// OriginName: tag_start_index, Units: , IsExtended: false
        /// </summary>
        public ushort TagStartIndex { get; set; }
        /// <summary>
        /// Tag stop index.
        /// OriginName: tag_stop_index, Units: , IsExtended: false
        /// </summary>
        public ushort TagStopIndex { get; set; }
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
    /// Response for ASV_SDR_RECORD_TAG_DELETE_REQUEST.
    ///  ASV_SDR_RECORD_TAG_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagDeleteResponsePacket: PacketV2<AsvSdrRecordTagDeleteResponsePayload>
    {
	    public const int PacketMessageId = 13115;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 228;

        public override AsvSdrRecordTagDeleteResponsePayload Payload { get; } = new AsvSdrRecordTagDeleteResponsePayload();

        public override string Name => "ASV_SDR_RECORD_TAG_DELETE_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_TAG_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordTagDeleteResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 7; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            TagStartIndex = BinSerialize.ReadUShort(ref buffer);
            TagStopIndex = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
            BinSerialize.WriteUShort(ref buffer,TagStartIndex);
            BinSerialize.WriteUShort(ref buffer,TagStopIndex);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 7 */;
        }



        /// <summary>
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
        /// <summary>
        /// Tag start index.
        /// OriginName: tag_start_index, Units: , IsExtended: false
        /// </summary>
        public ushort TagStartIndex { get; set; }
        /// <summary>
        /// Tag stop index.
        /// OriginName: tag_stop_index, Units: , IsExtended: false
        /// </summary>
        public ushort TagStopIndex { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRequestAck Result { get; set; }
    }
    /// <summary>
    /// Request the overall list of ASV_SDR_RECORD_DATA_* items from the system/component.
    ///  ASV_SDR_RECORD_DATA_LIST_REQUEST
    /// </summary>
    public class AsvSdrRecordDataListRequestPacket: PacketV2<AsvSdrRecordDataListRequestPayload>
    {
	    public const int PacketMessageId = 13120;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 235;

        public override AsvSdrRecordDataListRequestPayload Payload { get; } = new AsvSdrRecordDataListRequestPayload();

        public override string Name => "ASV_SDR_RECORD_DATA_LIST_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_LIST_REQUEST
    /// </summary>
    public class AsvSdrRecordDataListRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 12; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 12; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            DataStartIndex = BinSerialize.ReadUInt(ref buffer);
            DataStopIndex = BinSerialize.ReadUInt(ref buffer);
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,DataStartIndex);
            BinSerialize.WriteUInt(ref buffer,DataStopIndex);
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 12 */;
        }



        /// <summary>
        /// Data start index in record.
        /// OriginName: data_start_index, Units: , IsExtended: false
        /// </summary>
        public uint DataStartIndex { get; set; }
        /// <summary>
        /// Data stop index in record.
        /// OriginName: data_stop_index, Units: , IsExtended: false
        /// </summary>
        public uint DataStopIndex { get; set; }
        /// <summary>
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
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
    /// Response for ASV_SDR_RECORD_DATA_LIST_REQUEST.
    ///  ASV_SDR_RECORD_DATA_LIST_RESPONSE
    /// </summary>
    public class AsvSdrRecordDataListResponsePacket: PacketV2<AsvSdrRecordDataListResponsePayload>
    {
	    public const int PacketMessageId = 13121;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 59;

        public override AsvSdrRecordDataListResponsePayload Payload { get; } = new AsvSdrRecordDataListResponsePayload();

        public override string Name => "ASV_SDR_RECORD_DATA_LIST_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_LIST_RESPONSE
    /// </summary>
    public class AsvSdrRecordDataListResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 7; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            ItemsCount = BinSerialize.ReadUInt(ref buffer);
            DataType = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,ItemsCount);
            BinSerialize.WriteUShort(ref buffer,DataType);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 7 */;
        }



        /// <summary>
        /// Number of items for transmition.
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public uint ItemsCount { get; set; }
        /// <summary>
        /// Record data type (message id, e.g. 13120 for ASV_SDR_RECORD_DATA_ILS).
        /// OriginName: data_type, Units: , IsExtended: false
        /// </summary>
        public ushort DataType { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRequestAck Result { get; set; }
    }
    /// <summary>
    /// Request to read data with either record_index and data_index from the system/component.
    ///  ASV_SDR_RECORD_DATA_READ_REQUEST
    /// </summary>
    public class AsvSdrRecordDataReadRequestPacket: PacketV2<AsvSdrRecordDataReadRequestPayload>
    {
	    public const int PacketMessageId = 13122;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 67;

        public override AsvSdrRecordDataReadRequestPayload Payload { get; } = new AsvSdrRecordDataReadRequestPayload();

        public override string Name => "ASV_SDR_RECORD_DATA_READ_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_READ_REQUEST
    /// </summary>
    public class AsvSdrRecordDataReadRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            DataIndex = BinSerialize.ReadUInt(ref buffer);
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,DataIndex);
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 8 */;
        }



        /// <summary>
        /// Data index.
        /// OriginName: data_index, Units: , IsExtended: false
        /// </summary>
        public uint DataIndex { get; set; }
        /// <summary>
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
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
    /// Request to delete data with either record_index and data_index from the system/component.
    ///  ASV_SDR_RECORD_DATA_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordDataDeleteRequestPacket: PacketV2<AsvSdrRecordDataDeleteRequestPayload>
    {
	    public const int PacketMessageId = 13123;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 222;

        public override AsvSdrRecordDataDeleteRequestPayload Payload { get; } = new AsvSdrRecordDataDeleteRequestPayload();

        public override string Name => "ASV_SDR_RECORD_DATA_DELETE_REQUEST";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_DELETE_REQUEST
    /// </summary>
    public class AsvSdrRecordDataDeleteRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 12; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 12; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            StartDataIndex = BinSerialize.ReadUInt(ref buffer);
            StopDataIndex = BinSerialize.ReadUInt(ref buffer);
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,StartDataIndex);
            BinSerialize.WriteUInt(ref buffer,StopDataIndex);
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 12 */;
        }



        /// <summary>
        /// Start data index.
        /// OriginName: start_data_index, Units: , IsExtended: false
        /// </summary>
        public uint StartDataIndex { get; set; }
        /// <summary>
        /// Stop data index.
        /// OriginName: stop_data_index, Units: , IsExtended: false
        /// </summary>
        public uint StopDataIndex { get; set; }
        /// <summary>
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
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
    /// Response for ASV_SDR_RECORD_DATA_DELETE_REQUEST.
    ///  ASV_SDR_RECORD_DATA_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordDataDeleteResponsePacket: PacketV2<AsvSdrRecordDataDeleteResponsePayload>
    {
	    public const int PacketMessageId = 13124;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 36;

        public override AsvSdrRecordDataDeleteResponsePayload Payload { get; } = new AsvSdrRecordDataDeleteResponsePayload();

        public override string Name => "ASV_SDR_RECORD_DATA_DELETE_RESPONSE";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_DELETE_RESPONSE
    /// </summary>
    public class AsvSdrRecordDataDeleteResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 11; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 11; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            StartDataIndex = BinSerialize.ReadUInt(ref buffer);
            StopDataIndex = BinSerialize.ReadUInt(ref buffer);
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvSdrRequestAck)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,StartDataIndex);
            BinSerialize.WriteUInt(ref buffer,StopDataIndex);
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 11 */;
        }



        /// <summary>
        /// Start data index.
        /// OriginName: start_data_index, Units: , IsExtended: false
        /// </summary>
        public uint StartDataIndex { get; set; }
        /// <summary>
        /// Stop data index.
        /// OriginName: stop_data_index, Units: , IsExtended: false
        /// </summary>
        public uint StopDataIndex { get; set; }
        /// <summary>
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvSdrRequestAck Result { get; set; }
    }
    /// <summary>
    /// ILS reciever record data.
    ///  ASV_SDR_RECORD_DATA_ILS
    /// </summary>
    public class AsvSdrRecordDataIlsPacket: PacketV2<AsvSdrRecordDataIlsPayload>
    {
	    public const int PacketMessageId = 13150;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 45;

        public override AsvSdrRecordDataIlsPayload Payload { get; } = new AsvSdrRecordDataIlsPayload();

        public override string Name => "ASV_SDR_RECORD_DATA_ILS";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_ILS
    /// </summary>
    public class AsvSdrRecordDataIlsPayload : IPayload
    {
        public byte GetMaxByteSize() => 166; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 166; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
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
            CrsAm90 = BinSerialize.ReadFloat(ref buffer);
            CrsAm150 = BinSerialize.ReadFloat(ref buffer);
            ClrPower = BinSerialize.ReadFloat(ref buffer);
            ClrAm90 = BinSerialize.ReadFloat(ref buffer);
            ClrAm150 = BinSerialize.ReadFloat(ref buffer);
            TotalFreq = BinSerialize.ReadUInt(ref buffer);
            TotalPower = BinSerialize.ReadFloat(ref buffer);
            TotalFieldStrength = BinSerialize.ReadFloat(ref buffer);
            TotalAm90 = BinSerialize.ReadFloat(ref buffer);
            TotalAm150 = BinSerialize.ReadFloat(ref buffer);
            Phi90CrsVsClr = BinSerialize.ReadFloat(ref buffer);
            Phi150CrsVsClr = BinSerialize.ReadFloat(ref buffer);
            CodeIdAm1020 = BinSerialize.ReadFloat(ref buffer);
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
            GnssEph = BinSerialize.ReadUShort(ref buffer);
            GnssEpv = BinSerialize.ReadUShort(ref buffer);
            GnssVel = BinSerialize.ReadUShort(ref buffer);
            Vx = BinSerialize.ReadShort(ref buffer);
            Vy = BinSerialize.ReadShort(ref buffer);
            Vz = BinSerialize.ReadShort(ref buffer);
            Hdg = BinSerialize.ReadUShort(ref buffer);
            CrsPower = BinSerialize.ReadShort(ref buffer);
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
            GnssFixType = (GpsFixType)BinSerialize.ReadByte(ref buffer);
            GnssSatellitesVisible = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/166 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
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
            BinSerialize.WriteFloat(ref buffer,CrsAm90);
            BinSerialize.WriteFloat(ref buffer,CrsAm150);
            BinSerialize.WriteFloat(ref buffer,ClrPower);
            BinSerialize.WriteFloat(ref buffer,ClrAm90);
            BinSerialize.WriteFloat(ref buffer,ClrAm150);
            BinSerialize.WriteUInt(ref buffer,TotalFreq);
            BinSerialize.WriteFloat(ref buffer,TotalPower);
            BinSerialize.WriteFloat(ref buffer,TotalFieldStrength);
            BinSerialize.WriteFloat(ref buffer,TotalAm90);
            BinSerialize.WriteFloat(ref buffer,TotalAm150);
            BinSerialize.WriteFloat(ref buffer,Phi90CrsVsClr);
            BinSerialize.WriteFloat(ref buffer,Phi150CrsVsClr);
            BinSerialize.WriteFloat(ref buffer,CodeIdAm1020);
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
            BinSerialize.WriteUShort(ref buffer,GnssEph);
            BinSerialize.WriteUShort(ref buffer,GnssEpv);
            BinSerialize.WriteUShort(ref buffer,GnssVel);
            BinSerialize.WriteShort(ref buffer,Vx);
            BinSerialize.WriteShort(ref buffer,Vy);
            BinSerialize.WriteShort(ref buffer,Vz);
            BinSerialize.WriteUShort(ref buffer,Hdg);
            BinSerialize.WriteShort(ref buffer,CrsPower);
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
            
            /* PayloadByteSize = 166 */;
        }



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
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
        /// Measured frequency.
        /// OriginName: total_freq, Units: Hz, IsExtended: false
        /// </summary>
        public uint TotalFreq { get; set; }
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
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
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
        /// Input power of course.
        /// OriginName: crs_power, Units: dBm, IsExtended: false
        /// </summary>
        public short CrsPower { get; set; }
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
        public char[] CodeId { get; set; } = new char[4];
        public byte GetCodeIdMaxItemsCount() => 4;
    }
    /// <summary>
    /// VOR reciever record data.
    ///  ASV_SDR_RECORD_DATA_VOR
    /// </summary>
    public class AsvSdrRecordDataVorPacket: PacketV2<AsvSdrRecordDataVorPayload>
    {
	    public const int PacketMessageId = 13151;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 230;

        public override AsvSdrRecordDataVorPayload Payload { get; } = new AsvSdrRecordDataVorPayload();

        public override string Name => "ASV_SDR_RECORD_DATA_VOR";
    }

    /// <summary>
    ///  ASV_SDR_RECORD_DATA_VOR
    /// </summary>
    public class AsvSdrRecordDataVorPayload : IPayload
    {
        public byte GetMaxByteSize() => 132; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 132; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
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
            Freq = BinSerialize.ReadUInt(ref buffer);
            Azimuth = BinSerialize.ReadFloat(ref buffer);
            Power = BinSerialize.ReadFloat(ref buffer);
            FieldStrength = BinSerialize.ReadFloat(ref buffer);
            Am30 = BinSerialize.ReadFloat(ref buffer);
            Am9960 = BinSerialize.ReadFloat(ref buffer);
            Deviation = BinSerialize.ReadFloat(ref buffer);
            CodeIdAm1020 = BinSerialize.ReadFloat(ref buffer);
            RecordIndex = BinSerialize.ReadUShort(ref buffer);
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
            GnssFixType = (GpsFixType)BinSerialize.ReadByte(ref buffer);
            GnssSatellitesVisible = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/132 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
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
            BinSerialize.WriteUInt(ref buffer,Freq);
            BinSerialize.WriteFloat(ref buffer,Azimuth);
            BinSerialize.WriteFloat(ref buffer,Power);
            BinSerialize.WriteFloat(ref buffer,FieldStrength);
            BinSerialize.WriteFloat(ref buffer,Am30);
            BinSerialize.WriteFloat(ref buffer,Am9960);
            BinSerialize.WriteFloat(ref buffer,Deviation);
            BinSerialize.WriteFloat(ref buffer,CodeIdAm1020);
            BinSerialize.WriteUShort(ref buffer,RecordIndex);
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
            
            /* PayloadByteSize = 132 */;
        }



        /// <summary>
        /// Timestamp (UNIX epoch time).
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
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
        /// Frequency.
        /// OriginName: freq, Units: Hz, IsExtended: false
        /// </summary>
        public uint Freq { get; set; }
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
        /// Record index in storage.
        /// OriginName: record_index, Units: , IsExtended: false
        /// </summary>
        public ushort RecordIndex { get; set; }
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
        public char[] CodeId { get; set; } = new char[4];
        public byte GetCodeIdMaxItemsCount() => 4;
    }


#endregion


}

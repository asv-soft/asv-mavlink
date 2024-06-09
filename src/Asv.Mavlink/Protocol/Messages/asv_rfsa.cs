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

// This code was generate by tool Asv.Mavlink.Shell version 3.7.1+4106fec092ad8e5c656389a6225b57600d851309

using System;
using System.Text;
using System.ComponentModel;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Asv.IO;

namespace Asv.Mavlink.V2.AsvRfsa
{

    public static class AsvRfsaHelper
    {
        public static void RegisterAsvRfsaDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AsvRfsaSignalRequestPacket());
            src.Register(()=>new AsvRfsaSignalResponsePacket());
            src.Register(()=>new AsvRfsaSignalInfoPacket());
            src.Register(()=>new AsvRfsaSignalDataPacket());
        }
    }

#region Enums

    /// <summary>
    /// A mapping of RFSA modes for custom_mode field of heartbeat.
    ///  ASV_RFSA_CUSTOM_MODE
    /// </summary>
    public enum AsvRfsaCustomMode:uint
    {
        /// <summary>
        /// ASV_RFSA_CUSTOM_MODE_IDLE
        /// </summary>
        AsvRfsaCustomModeIdle = 0,
        /// <summary>
        /// ASV_RFSA_CUSTOM_MODE_MEASURE
        /// </summary>
        AsvRfsaCustomModeMeasure = 1,
    }

    /// <summary>
    ///  MAV_TYPE
    /// </summary>
    public enum MavType:uint
    {
        /// <summary>
        /// Used to identify RF spectrum analyzer payload in HEARTBEAT packet.
        /// MAV_TYPE_ASV_RFSA
        /// </summary>
        MavTypeAsvRfsa = 253,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Enable RF analyzer. Change mode to ASV_RFSA_CUSTOM_MODE_MEASURE
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_RFSA_ON
        /// </summary>
        MavCmdAsvRfsaOn = 13300,
        /// <summary>
        /// Disable analyzer. Change mode to ASV_RFSA_CUSTOM_MODE_IDLE
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_RFSA_OFF
        /// </summary>
        MavCmdAsvRfsaOff = 13301,
    }

    /// <summary>
    /// ACK / NACK / ERROR values as a result of ASV_RFSA_*_REQUEST commands.
    ///  ASV_RFSA_REQUEST_ACK
    /// </summary>
    public enum AsvRfsaRequestAck:uint
    {
        /// <summary>
        /// Request is ok.
        /// ASV_RFSA_REQUEST_ACK_OK
        /// </summary>
        AsvRfsaRequestAckOk = 0,
        /// <summary>
        /// Command already in progress.
        /// ASV_RFSA_REQUEST_ACK_IN_PROGRESS
        /// </summary>
        AsvRfsaRequestAckInProgress = 1,
        /// <summary>
        /// Command error.
        /// ASV_RFSA_REQUEST_ACK_FAIL
        /// </summary>
        AsvRfsaRequestAckFail = 2,
        /// <summary>
        /// Not supported command.
        /// ASV_RFSA_REQUEST_ACK_NOT_SUPPORTED
        /// </summary>
        AsvRfsaRequestAckNotSupported = 3,
    }

    /// <summary>
    /// SDR signal transmition data type
    ///  ASV_RFSA_SIGNAL_FORMAT
    /// </summary>
    public enum AsvRfsaSignalFormat:uint
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
    /// Request available signals for visualization.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RFSA_SIGNAL_REQUEST
    /// </summary>
    public class AsvRfsaSignalRequestPacket: PacketV2<AsvRfsaSignalRequestPayload>
    {
	    public const int PacketMessageId = 13300;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 244;
        public override bool WrapToV2Extension => true;

        public override AsvRfsaSignalRequestPayload Payload { get; } = new AsvRfsaSignalRequestPayload();

        public override string Name => "ASV_RFSA_SIGNAL_REQUEST";
    }

    /// <summary>
    ///  ASV_RFSA_SIGNAL_REQUEST
    /// </summary>
    public class AsvRfsaSignalRequestPayload : IPayload
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
    /// Request available signals for visualization.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RFSA_SIGNAL_RESPONSE
    /// </summary>
    public class AsvRfsaSignalResponsePacket: PacketV2<AsvRfsaSignalResponsePayload>
    {
	    public const int PacketMessageId = 13301;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 182;
        public override bool WrapToV2Extension => true;

        public override AsvRfsaSignalResponsePayload Payload { get; } = new AsvRfsaSignalResponsePayload();

        public override string Name => "ASV_RFSA_SIGNAL_RESPONSE";
    }

    /// <summary>
    ///  ASV_RFSA_SIGNAL_RESPONSE
    /// </summary>
    public class AsvRfsaSignalResponsePayload : IPayload
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
            Result = (AsvRfsaRequestAck)BinSerialize.ReadByte(ref buffer);

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
        /// Number of items ASV_RFSA_SIGNAL_INFO for transmition after this request with success result code (depended from request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public ushort ItemsCount { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvRfsaRequestAck Result { get; set; }
    }
    /// <summary>
    /// Response available signals for visualization with params.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RFSA_SIGNAL_INFO
    /// </summary>
    public class AsvRfsaSignalInfoPacket: PacketV2<AsvRfsaSignalInfoPayload>
    {
	    public const int PacketMessageId = 13302;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 242;
        public override bool WrapToV2Extension => true;

        public override AsvRfsaSignalInfoPayload Payload { get; } = new AsvRfsaSignalInfoPayload();

        public override string Name => "ASV_RFSA_SIGNAL_INFO";
    }

    /// <summary>
    ///  ASV_RFSA_SIGNAL_INFO
    /// </summary>
    public class AsvRfsaSignalInfoPayload : IPayload
    {
        public byte GetMaxByteSize() => 71; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 71; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //AxesXMin
            sum+=4; //AxesXMax
            sum+=4; //AxesYMin
            sum+=4; //AxesYMax
            sum+=2; //SignalId
            sum+=2; //AxesXCount
            sum+=2; //AxesYCount
            sum+=SignalName.Length; //SignalName
            sum+=AxesXName.Length; //AxesXName
            sum+=AxesYName.Length; //AxesYName
            sum+= 1; // Format
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            AxesXMin = BinSerialize.ReadFloat(ref buffer);
            AxesXMax = BinSerialize.ReadFloat(ref buffer);
            AxesYMin = BinSerialize.ReadFloat(ref buffer);
            AxesYMax = BinSerialize.ReadFloat(ref buffer);
            SignalId = BinSerialize.ReadUShort(ref buffer);
            AxesXCount = BinSerialize.ReadUShort(ref buffer);
            AxesYCount = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/71 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            SignalName = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = SignalName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, SignalName.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
            arraySize = 16;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = AxesXName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, AxesXName.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
            arraySize = 16;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = AxesYName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, AxesYName.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
            Format = (AsvRfsaSignalFormat)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,AxesXMin);
            BinSerialize.WriteFloat(ref buffer,AxesXMax);
            BinSerialize.WriteFloat(ref buffer,AxesYMin);
            BinSerialize.WriteFloat(ref buffer,AxesYMax);
            BinSerialize.WriteUShort(ref buffer,SignalId);
            BinSerialize.WriteUShort(ref buffer,AxesXCount);
            BinSerialize.WriteUShort(ref buffer,AxesYCount);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = SignalName)
                {
                    Encoding.ASCII.GetBytes(charPointer, SignalName.Length, bytePointer, SignalName.Length);
                }
            }
            buffer = buffer.Slice(SignalName.Length);
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = AxesXName)
                {
                    Encoding.ASCII.GetBytes(charPointer, AxesXName.Length, bytePointer, AxesXName.Length);
                }
            }
            buffer = buffer.Slice(AxesXName.Length);
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = AxesYName)
                {
                    Encoding.ASCII.GetBytes(charPointer, AxesYName.Length, bytePointer, AxesYName.Length);
                }
            }
            buffer = buffer.Slice(AxesYName.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)Format);
            /* PayloadByteSize = 71 */;
        }
        
        



        /// <summary>
        /// Minimum value of Axis X.
        /// OriginName: axes_x_min, Units: , IsExtended: false
        /// </summary>
        public float AxesXMin { get; set; }
        /// <summary>
        /// Maximum value of Axis X.
        /// OriginName: axes_x_max, Units: , IsExtended: false
        /// </summary>
        public float AxesXMax { get; set; }
        /// <summary>
        /// Minimum value of Axis Y.
        /// OriginName: axes_y_min, Units: , IsExtended: false
        /// </summary>
        public float AxesYMin { get; set; }
        /// <summary>
        /// Maximum value of Axis Y.
        /// OriginName: axes_y_max, Units: , IsExtended: false
        /// </summary>
        public float AxesYMax { get; set; }
        /// <summary>
        /// Signal id.
        /// OriginName: signal_id, Units: , IsExtended: false
        /// </summary>
        public ushort SignalId { get; set; }
        /// <summary>
        /// Total measure points for X axes (1 measure for simple plot signal, more for heatmap signal).
        /// OriginName: axes_x_count, Units: , IsExtended: false
        /// </summary>
        public ushort AxesXCount { get; set; }
        /// <summary>
        /// Total measure count for Y.
        /// OriginName: axes_y_count, Units: , IsExtended: false
        /// </summary>
        public ushort AxesYCount { get; set; }
        /// <summary>
        /// Signal name, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 8+1 bytes storage if the ID is stored as string
        /// OriginName: signal_name, Units: , IsExtended: false
        /// </summary>
        public const int SignalNameMaxItemsCount = 16;
        public char[] SignalName { get; set; } = new char[16];
        [Obsolete("This method is deprecated. Use GetSignalNameMaxItemsCount instead.")]
        public byte GetSignalNameMaxItemsCount() => 16;
        /// <summary>
        /// Axis X name, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 8+1 bytes storage if the ID is stored as string
        /// OriginName: axes_x_name, Units: , IsExtended: false
        /// </summary>
        public const int AxesXNameMaxItemsCount = 16;
        public char[] AxesXName { get; } = new char[16];
        /// <summary>
        /// Axis Y name, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 8+1 bytes storage if the ID is stored as string
        /// OriginName: axes_y_name, Units: , IsExtended: false
        /// </summary>
        public const int AxesYNameMaxItemsCount = 16;
        public char[] AxesYName { get; } = new char[16];
        /// <summary>
        /// Format of one measure.
        /// OriginName: format, Units: , IsExtended: false
        /// </summary>
        public AsvRfsaSignalFormat Format { get; set; }
    }
    /// <summary>
    /// Raw signal data for visualization.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RFSA_SIGNAL_DATA
    /// </summary>
    public class AsvRfsaSignalDataPacket: PacketV2<AsvRfsaSignalDataPayload>
    {
	    public const int PacketMessageId = 13303;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 211;
        public override bool WrapToV2Extension => true;

        public override AsvRfsaSignalDataPayload Payload { get; } = new AsvRfsaSignalDataPayload();

        public override string Name => "ASV_RFSA_SIGNAL_DATA";
    }

    /// <summary>
    ///  ASV_RFSA_SIGNAL_DATA
    /// </summary>
    public class AsvRfsaSignalDataPayload : IPayload
    {
        public byte GetMaxByteSize() => 233; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 233; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+=2; //SignalId
            sum+=1; //PktInFrame
            sum+=1; //PktSeq
            sum+=1; //DataSize
            sum+=Data.Length; //Data
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            SignalId = BinSerialize.ReadUShort(ref buffer);
            PktInFrame = (byte)BinSerialize.ReadByte(ref buffer);
            PktSeq = (byte)BinSerialize.ReadByte(ref buffer);
            DataSize = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/220 - Math.Max(0,((/*PayloadByteSize*/233 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteUShort(ref buffer,SignalId);
            BinSerialize.WriteByte(ref buffer,(byte)PktInFrame);
            BinSerialize.WriteByte(ref buffer,(byte)PktSeq);
            BinSerialize.WriteByte(ref buffer,(byte)DataSize);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            /* PayloadByteSize = 233 */;
        }
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time) for current set of measures.
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Signal id.
        /// OriginName: signal_id, Units: , IsExtended: false
        /// </summary>
        public ushort SignalId { get; set; }
        /// <summary>
        /// Number of packets for one frame.
        /// OriginName: pkt_in_frame, Units: , IsExtended: false
        /// </summary>
        public byte PktInFrame { get; set; }
        /// <summary>
        /// Packet sequence number (starting with 0 on every encoded frame).
        /// OriginName: pkt_seq, Units: , IsExtended: false
        /// </summary>
        public byte PktSeq { get; set; }
        /// <summary>
        /// Size of data array.
        /// OriginName: data_size, Units: , IsExtended: false
        /// </summary>
        public byte DataSize { get; set; }
        /// <summary>
        /// Signal data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public const int DataMaxItemsCount = 220;
        public byte[] Data { get; set; } = new byte[220];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 220;
    }


#endregion


}

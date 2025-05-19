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

namespace Asv.Mavlink.AsvChart
{

    public static class AsvChartHelper
    {
        public static void RegisterAsvChartDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(AsvChartInfoRequestPacket.MessageId, ()=>new AsvChartInfoRequestPacket());
            src.Add(AsvChartInfoResponsePacket.MessageId, ()=>new AsvChartInfoResponsePacket());
            src.Add(AsvChartInfoUpdatedEventPacket.MessageId, ()=>new AsvChartInfoUpdatedEventPacket());
            src.Add(AsvChartInfoPacket.MessageId, ()=>new AsvChartInfoPacket());
            src.Add(AsvChartDataRequestPacket.MessageId, ()=>new AsvChartDataRequestPacket());
            src.Add(AsvChartDataResponsePacket.MessageId, ()=>new AsvChartDataResponsePacket());
            src.Add(AsvChartDataPacket.MessageId, ()=>new AsvChartDataPacket());
        }
    }

#region Enums

    /// <summary>
    /// ACK / NACK / ERROR values as a result of ASV_CHART_*_REQUEST commands.
    ///  ASV_CHART_REQUEST_ACK
    /// </summary>
    public enum AsvChartRequestAck:uint
    {
        /// <summary>
        /// Request is ok.
        /// ASV_CHART_REQUEST_ACK_OK
        /// </summary>
        AsvChartRequestAckOk = 0,
        /// <summary>
        /// Already in progress.
        /// ASV_CHART_REQUEST_ACK_IN_PROGRESS
        /// </summary>
        AsvChartRequestAckInProgress = 1,
        /// <summary>
        /// Internal error.
        /// ASV_CHART_REQUEST_ACK_FAIL
        /// </summary>
        AsvChartRequestAckFail = 2,
        /// <summary>
        /// Not supported.
        /// ASV_CHART_REQUEST_ACK_NOT_SUPPORTED
        /// </summary>
        AsvChartRequestAckNotSupported = 3,
        /// <summary>
        /// Element not found.
        /// ASV_CHART_REQUEST_ACK_NOT_FOUND
        /// </summary>
        AsvChartRequestAckNotFound = 4,
    }

    /// <summary>
    /// Chart data transmission data type
    ///  ASV_CHART_DATA_FORMAT
    /// </summary>
    public enum AsvChartDataFormat:uint
    {
        /// <summary>
        /// Write a value as a fraction between a given minimum and maximum. Uses 8 bits so we have '256' steps between min and max.
        /// ASV_CHART_DATA_FORMAT_RANGE_FLOAT_8BIT
        /// </summary>
        AsvChartDataFormatRangeFloat8bit = 0,
        /// <summary>
        /// Write a value as a fraction between a given minimum and maximum. Uses 16 bits so we have '65535' steps between min and max.
        /// ASV_CHART_DATA_FORMAT_RANGE_FLOAT_16BIT
        /// </summary>
        AsvChartDataFormatRangeFloat16bit = 1,
        /// <summary>
        /// Write a value as a float. Uses 32 bits.
        /// ASV_CHART_DATA_FORMAT_FLOAT
        /// </summary>
        AsvChartDataFormatFloat = 2,
    }

    /// <summary>
    /// Args for requested stream
    ///  ASV_CHART_DATA_TRIGGER
    /// </summary>
    public enum AsvChartDataTrigger:uint
    {
        /// <summary>
        /// Disable stream.
        /// ASV_CHART_DATA_TRIGGER_DISABLE
        /// </summary>
        AsvChartDataTriggerDisable = 0,
        /// <summary>
        /// Update once.
        /// ASV_CHART_DATA_TRIGGER_ONCE
        /// </summary>
        AsvChartDataTriggerOnce = 1,
        /// <summary>
        /// Update only on changed.
        /// ASV_CHART_DATA_TRIGGER_ON_CHANGED
        /// </summary>
        AsvChartDataTriggerOnChanged = 2,
        /// <summary>
        /// Periodic update.
        /// ASV_CHART_DATA_TRIGGER_PERIODIC
        /// </summary>
        AsvChartDataTriggerPeriodic = 3,
    }

    /// <summary>
    /// Chart type
    ///  ASV_CHART_TYPE
    /// </summary>
    public enum AsvChartType:uint
    {
        /// <summary>
        /// Simple plot.
        /// ASV_CHART_TYPE_SIMPLE
        /// </summary>
        AsvChartTypeSimple = 0,
        /// <summary>
        /// Heatmap plot.
        /// ASV_CHART_TYPE_HEATMAP
        /// </summary>
        AsvChartTypeHeatmap = 1,
    }

    /// <summary>
    /// Chart type
    ///  ASV_CHART_UNIT_TYPE
    /// </summary>
    public enum AsvChartUnitType:uint
    {
        /// <summary>
        /// Custom unit.
        /// ASV_CHART_UNIT_TYPE_CUSTOM
        /// </summary>
        AsvChartUnitTypeCustom = 0,
        /// <summary>
        /// dBm.
        /// ASV_CHART_UNIT_TYPE_DBM
        /// </summary>
        AsvChartUnitTypeDbm = 1,
    }


#endregion

#region Messages

    /// <summary>
    /// Requests available charts for visualization. Returns ASV_CHART_INFO_RESPONSE and then items with ASV_CHART_INFO. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_CHART_INFO_REQUEST
    /// </summary>
    public class AsvChartInfoRequestPacket : MavlinkV2Message<AsvChartInfoRequestPayload>
    {
        public const int MessageId = 13350;
        
        public const byte CrcExtra = 131;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvChartInfoRequestPayload Payload { get; } = new();

        public override string Name => "ASV_CHART_INFO_REQUEST";
    }

    /// <summary>
    ///  ASV_CHART_INFO_REQUEST
    /// </summary>
    public class AsvChartInfoRequestPayload : IPayload
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
    /// Responds to the request for available charts for visualization. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_CHART_INFO_RESPONSE
    /// </summary>
    public class AsvChartInfoResponsePacket : MavlinkV2Message<AsvChartInfoResponsePayload>
    {
        public const int MessageId = 13351;
        
        public const byte CrcExtra = 109;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvChartInfoResponsePayload Payload { get; } = new();

        public override string Name => "ASV_CHART_INFO_RESPONSE";
    }

    /// <summary>
    ///  ASV_CHART_INFO_RESPONSE
    /// </summary>
    public class AsvChartInfoResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 7; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t request_id
            +2 // uint16_t items_count
            +2 // uint16_t chat_list_hash
            + 1 // uint8_t result
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUShort(ref buffer);
            ItemsCount = BinSerialize.ReadUShort(ref buffer);
            ChatListHash = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvChartRequestAck)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,RequestId);
            BinSerialize.WriteUShort(ref buffer,ItemsCount);
            BinSerialize.WriteUShort(ref buffer,ChatListHash);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 7 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt16Type.Accept(visitor,ItemsCountField, ref _ItemsCount);    
            UInt16Type.Accept(visitor,ChatListHashField, ref _ChatListHash);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ref tmpResult);
            Result = (AsvChartRequestAck)tmpResult;

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
        /// Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ItemsCountField = new Field.Builder()
            .Name(nameof(ItemsCount))
            .Title("items_count")
            .Description("Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ItemsCount;
        public ushort ItemsCount { get => _ItemsCount; set { _ItemsCount = value; } }
        /// <summary>
        /// Hash of the all ASV_CHART_INFO.
        /// OriginName: chat_list_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatListHashField = new Field.Builder()
            .Name(nameof(ChatListHash))
            .Title("chat_list_hash")
            .Description("Hash of the all ASV_CHART_INFO.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChatListHash;
        public ushort ChatListHash { get => _ChatListHash; set { _ChatListHash = value; } }
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
        public AsvChartRequestAck _Result;
        public AsvChartRequestAck Result { get => _Result; set => _Result = value; } 
    }
    /// <summary>
    /// Event about chart collection or it's element changed. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_CHART_INFO_UPDATED_EVENT
    /// </summary>
    public class AsvChartInfoUpdatedEventPacket : MavlinkV2Message<AsvChartInfoUpdatedEventPayload>
    {
        public const int MessageId = 13352;
        
        public const byte CrcExtra = 37;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvChartInfoUpdatedEventPayload Payload { get; } = new();

        public override string Name => "ASV_CHART_INFO_UPDATED_EVENT";
    }

    /// <summary>
    ///  ASV_CHART_INFO_UPDATED_EVENT
    /// </summary>
    public class AsvChartInfoUpdatedEventPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 4; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 4; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t chart_count
            +2 // uint16_t chat_list_hash
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            ChartCount = BinSerialize.ReadUShort(ref buffer);
            ChatListHash = BinSerialize.ReadUShort(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,ChartCount);
            BinSerialize.WriteUShort(ref buffer,ChatListHash);
            /* PayloadByteSize = 4 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,ChartCountField, ref _ChartCount);    
            UInt16Type.Accept(visitor,ChatListHashField, ref _ChatListHash);    

        }

        /// <summary>
        /// Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).
        /// OriginName: chart_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChartCountField = new Field.Builder()
            .Name(nameof(ChartCount))
            .Title("chart_count")
            .Description("Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChartCount;
        public ushort ChartCount { get => _ChartCount; set { _ChartCount = value; } }
        /// <summary>
        /// Hash of the all ASV_CHART_INFO.
        /// OriginName: chat_list_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatListHashField = new Field.Builder()
            .Name(nameof(ChatListHash))
            .Title("chat_list_hash")
            .Description("Hash of the all ASV_CHART_INFO.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChatListHash;
        public ushort ChatListHash { get => _ChatListHash; set { _ChatListHash = value; } }
    }
    /// <summary>
    /// Contains chart info. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_CHART_INFO
    /// </summary>
    public class AsvChartInfoPacket : MavlinkV2Message<AsvChartInfoPayload>
    {
        public const int MessageId = 13353;
        
        public const byte CrcExtra = 159;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvChartInfoPayload Payload { get; } = new();

        public override string Name => "ASV_CHART_INFO";
    }

    /// <summary>
    ///  ASV_CHART_INFO
    /// </summary>
    public class AsvChartInfoPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 78; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 78; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float axes_x_min
            +4 // float axes_x_max
            +4 // float axes_y_min
            +4 // float axes_y_max
            +2 // uint16_t chart_id
            +2 // uint16_t chart_info_hash
            + 2 // uint16_t axes_x_unit
            +2 // uint16_t axes_x_count
            + 2 // uint16_t axes_y_unit
            +2 // uint16_t axes_y_count
            +ChartName.Length // char[16] chart_name
            + 1 // uint8_t chart_type
            +AxesXName.Length // char[16] axes_x_name
            +AxesYName.Length // char[16] axes_y_name
            + 1 // uint8_t format
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            AxesXMin = BinSerialize.ReadFloat(ref buffer);
            AxesXMax = BinSerialize.ReadFloat(ref buffer);
            AxesYMin = BinSerialize.ReadFloat(ref buffer);
            AxesYMax = BinSerialize.ReadFloat(ref buffer);
            ChartId = BinSerialize.ReadUShort(ref buffer);
            ChartInfoHash = BinSerialize.ReadUShort(ref buffer);
            AxesXUnit = (AsvChartUnitType)BinSerialize.ReadUShort(ref buffer);
            AxesXCount = BinSerialize.ReadUShort(ref buffer);
            AxesYUnit = (AsvChartUnitType)BinSerialize.ReadUShort(ref buffer);
            AxesYCount = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/78 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ChartName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, ChartName.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            ChartType = (AsvChartType)BinSerialize.ReadByte(ref buffer);
            arraySize = 16;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = AxesXName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, AxesXName.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            arraySize = 16;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = AxesYName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, AxesYName.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            Format = (AsvChartDataFormat)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,AxesXMin);
            BinSerialize.WriteFloat(ref buffer,AxesXMax);
            BinSerialize.WriteFloat(ref buffer,AxesYMin);
            BinSerialize.WriteFloat(ref buffer,AxesYMax);
            BinSerialize.WriteUShort(ref buffer,ChartId);
            BinSerialize.WriteUShort(ref buffer,ChartInfoHash);
            BinSerialize.WriteUShort(ref buffer,(ushort)AxesXUnit);
            BinSerialize.WriteUShort(ref buffer,AxesXCount);
            BinSerialize.WriteUShort(ref buffer,(ushort)AxesYUnit);
            BinSerialize.WriteUShort(ref buffer,AxesYCount);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ChartName)
                {
                    Encoding.ASCII.GetBytes(charPointer, ChartName.Length, bytePointer, ChartName.Length);
                }
            }
            buffer = buffer.Slice(ChartName.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)ChartType);
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
            /* PayloadByteSize = 78 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,AxesXMinField, ref _AxesXMin);    
            FloatType.Accept(visitor,AxesXMaxField, ref _AxesXMax);    
            FloatType.Accept(visitor,AxesYMinField, ref _AxesYMin);    
            FloatType.Accept(visitor,AxesYMaxField, ref _AxesYMax);    
            UInt16Type.Accept(visitor,ChartIdField, ref _ChartId);    
            UInt16Type.Accept(visitor,ChartInfoHashField, ref _ChartInfoHash);    
            var tmpAxesXUnit = (ushort)AxesXUnit;
            UInt16Type.Accept(visitor,AxesXUnitField, ref tmpAxesXUnit);
            AxesXUnit = (AsvChartUnitType)tmpAxesXUnit;
            UInt16Type.Accept(visitor,AxesXCountField, ref _AxesXCount);    
            var tmpAxesYUnit = (ushort)AxesYUnit;
            UInt16Type.Accept(visitor,AxesYUnitField, ref tmpAxesYUnit);
            AxesYUnit = (AsvChartUnitType)tmpAxesYUnit;
            UInt16Type.Accept(visitor,AxesYCountField, ref _AxesYCount);    
            ArrayType.Accept(visitor,ChartNameField, 16, (index,v) =>
            {
                var tmp = (byte)ChartName[index];
                UInt8Type.Accept(v,ChartNameField, ref tmp);
                ChartName[index] = (char)tmp;
            });
            var tmpChartType = (byte)ChartType;
            UInt8Type.Accept(visitor,ChartTypeField, ref tmpChartType);
            ChartType = (AsvChartType)tmpChartType;
            ArrayType.Accept(visitor,AxesXNameField, 16, (index,v) =>
            {
                var tmp = (byte)AxesXName[index];
                UInt8Type.Accept(v,AxesXNameField, ref tmp);
                AxesXName[index] = (char)tmp;
            });
            ArrayType.Accept(visitor,AxesYNameField, 16, (index,v) =>
            {
                var tmp = (byte)AxesYName[index];
                UInt8Type.Accept(v,AxesYNameField, ref tmp);
                AxesYName[index] = (char)tmp;
            });
            var tmpFormat = (byte)Format;
            UInt8Type.Accept(visitor,FormatField, ref tmpFormat);
            Format = (AsvChartDataFormat)tmpFormat;

        }

        /// <summary>
        /// Minimum value of Axis X.
        /// OriginName: axes_x_min, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesXMinField = new Field.Builder()
            .Name(nameof(AxesXMin))
            .Title("axes_x_min")
            .Description("Minimum value of Axis X.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _AxesXMin;
        public float AxesXMin { get => _AxesXMin; set { _AxesXMin = value; } }
        /// <summary>
        /// Maximum value of Axis X.
        /// OriginName: axes_x_max, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesXMaxField = new Field.Builder()
            .Name(nameof(AxesXMax))
            .Title("axes_x_max")
            .Description("Maximum value of Axis X.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _AxesXMax;
        public float AxesXMax { get => _AxesXMax; set { _AxesXMax = value; } }
        /// <summary>
        /// Minimum value of Axis Y.
        /// OriginName: axes_y_min, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesYMinField = new Field.Builder()
            .Name(nameof(AxesYMin))
            .Title("axes_y_min")
            .Description("Minimum value of Axis Y.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _AxesYMin;
        public float AxesYMin { get => _AxesYMin; set { _AxesYMin = value; } }
        /// <summary>
        /// Maximum value of Axis Y.
        /// OriginName: axes_y_max, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesYMaxField = new Field.Builder()
            .Name(nameof(AxesYMax))
            .Title("axes_y_max")
            .Description("Maximum value of Axis Y.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _AxesYMax;
        public float AxesYMax { get => _AxesYMax; set { _AxesYMax = value; } }
        /// <summary>
        /// Chart ID.
        /// OriginName: chart_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChartIdField = new Field.Builder()
            .Name(nameof(ChartId))
            .Title("chart_id")
            .Description("Chart ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChartId;
        public ushort ChartId { get => _ChartId; set { _ChartId = value; } }
        /// <summary>
        /// Hash of the chart info.
        /// OriginName: chart_info_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChartInfoHashField = new Field.Builder()
            .Name(nameof(ChartInfoHash))
            .Title("chart_info_hash")
            .Description("Hash of the chart info.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChartInfoHash;
        public ushort ChartInfoHash { get => _ChartInfoHash; set { _ChartInfoHash = value; } }
        /// <summary>
        /// Axis X unit.
        /// OriginName: axes_x_unit, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesXUnitField = new Field.Builder()
            .Name(nameof(AxesXUnit))
            .Title("axes_x_unit")
            .Description("Axis X unit.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        public AsvChartUnitType _AxesXUnit;
        public AsvChartUnitType AxesXUnit { get => _AxesXUnit; set => _AxesXUnit = value; } 
        /// <summary>
        /// Total measure points for Axis X. Dependent on chart type (1 measure for simple plot signal, more for heatmap signal).
        /// OriginName: axes_x_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesXCountField = new Field.Builder()
            .Name(nameof(AxesXCount))
            .Title("axes_x_count")
            .Description("Total measure points for Axis X. Dependent on chart type (1 measure for simple plot signal, more for heatmap signal).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _AxesXCount;
        public ushort AxesXCount { get => _AxesXCount; set { _AxesXCount = value; } }
        /// <summary>
        /// Axis Y unit.
        /// OriginName: axes_y_unit, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesYUnitField = new Field.Builder()
            .Name(nameof(AxesYUnit))
            .Title("axes_y_unit")
            .Description("Axis Y unit.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        public AsvChartUnitType _AxesYUnit;
        public AsvChartUnitType AxesYUnit { get => _AxesYUnit; set => _AxesYUnit = value; } 
        /// <summary>
        /// Total measure points for Axis Y.
        /// OriginName: axes_y_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesYCountField = new Field.Builder()
            .Name(nameof(AxesYCount))
            .Title("axes_y_count")
            .Description("Total measure points for Axis Y.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _AxesYCount;
        public ushort AxesYCount { get => _AxesYCount; set { _AxesYCount = value; } }
        /// <summary>
        /// Chart name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.
        /// OriginName: chart_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChartNameField = new Field.Builder()
            .Name(nameof(ChartName))
            .Title("chart_name")
            .Description("Chart name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,16))

            .Build();
        public const int ChartNameMaxItemsCount = 16;
        public char[] ChartName { get; } = new char[16];
        [Obsolete("This method is deprecated. Use GetChartNameMaxItemsCount instead.")]
        public byte GetChartNameMaxItemsCount() => 16;
        /// <summary>
        /// Type of chart.
        /// OriginName: chart_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChartTypeField = new Field.Builder()
            .Name(nameof(ChartType))
            .Title("chart_type")
            .Description("Type of chart.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvChartType _ChartType;
        public AsvChartType ChartType { get => _ChartType; set => _ChartType = value; } 
        /// <summary>
        /// Axis X name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.
        /// OriginName: axes_x_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesXNameField = new Field.Builder()
            .Name(nameof(AxesXName))
            .Title("axes_x_name")
            .Description("Axis X name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,16))

            .Build();
        public const int AxesXNameMaxItemsCount = 16;
        public char[] AxesXName { get; } = new char[16];
        /// <summary>
        /// Axis Y name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.
        /// OriginName: axes_y_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesYNameField = new Field.Builder()
            .Name(nameof(AxesYName))
            .Title("axes_y_name")
            .Description("Axis Y name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,16))

            .Build();
        public const int AxesYNameMaxItemsCount = 16;
        public char[] AxesYName { get; } = new char[16];
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
        public AsvChartDataFormat _Format;
        public AsvChartDataFormat Format { get => _Format; set => _Format = value; } 
    }
    /// <summary>
    /// Request for chart data stream.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_CHART_DATA_REQUEST
    /// </summary>
    public class AsvChartDataRequestPacket : MavlinkV2Message<AsvChartDataRequestPayload>
    {
        public const int MessageId = 13354;
        
        public const byte CrcExtra = 4;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvChartDataRequestPayload Payload { get; } = new();

        public override string Name => "ASV_CHART_DATA_REQUEST";
    }

    /// <summary>
    ///  ASV_CHART_DATA_REQUEST
    /// </summary>
    public class AsvChartDataRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 11; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 11; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float data_rate
            +2 // uint16_t chat_id
            +2 // uint16_t chat_info_hash
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            + 1 // uint8_t data_trigger
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            DataRate = BinSerialize.ReadFloat(ref buffer);
            ChatId = BinSerialize.ReadUShort(ref buffer);
            ChatInfoHash = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            DataTrigger = (AsvChartDataTrigger)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,DataRate);
            BinSerialize.WriteUShort(ref buffer,ChatId);
            BinSerialize.WriteUShort(ref buffer,ChatInfoHash);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)DataTrigger);
            /* PayloadByteSize = 11 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,DataRateField, ref _DataRate);    
            UInt16Type.Accept(visitor,ChatIdField, ref _ChatId);    
            UInt16Type.Accept(visitor,ChatInfoHashField, ref _ChatInfoHash);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            var tmpDataTrigger = (byte)DataTrigger;
            UInt8Type.Accept(visitor,DataTriggerField, ref tmpDataTrigger);
            DataTrigger = (AsvChartDataTrigger)tmpDataTrigger;

        }

        /// <summary>
        /// The requested message rate (delay in ms)
        /// OriginName: data_rate, Units: Ms, IsExtended: false
        /// </summary>
        public static readonly Field DataRateField = new Field.Builder()
            .Name(nameof(DataRate))
            .Title("data_rate")
            .Description("The requested message rate (delay in ms)")
            .FormatString(string.Empty)
            .Units(@"Ms")
            .DataType(FloatType.Default)

            .Build();
        private float _DataRate;
        public float DataRate { get => _DataRate; set { _DataRate = value; } }
        /// <summary>
        /// The ID of the requested chart
        /// OriginName: chat_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatIdField = new Field.Builder()
            .Name(nameof(ChatId))
            .Title("chat_id")
            .Description("The ID of the requested chart")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChatId;
        public ushort ChatId { get => _ChatId; set { _ChatId = value; } }
        /// <summary>
        /// Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.
        /// OriginName: chat_info_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatInfoHashField = new Field.Builder()
            .Name(nameof(ChatInfoHash))
            .Title("chat_info_hash")
            .Description("Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChatInfoHash;
        public ushort ChatInfoHash { get => _ChatInfoHash; set { _ChatInfoHash = value; } }
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
        /// Additional argument for stream request.
        /// OriginName: data_trigger, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataTriggerField = new Field.Builder()
            .Name(nameof(DataTrigger))
            .Title("data_trigger")
            .Description("Additional argument for stream request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvChartDataTrigger _DataTrigger;
        public AsvChartDataTrigger DataTrigger { get => _DataTrigger; set => _DataTrigger = value; } 
    }
    /// <summary>
    /// Response for ASV_CHART_STREAM_REQUEST.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_CHART_DATA_RESPONSE
    /// </summary>
    public class AsvChartDataResponsePacket : MavlinkV2Message<AsvChartDataResponsePayload>
    {
        public const int MessageId = 13355;
        
        public const byte CrcExtra = 185;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvChartDataResponsePayload Payload { get; } = new();

        public override string Name => "ASV_CHART_DATA_RESPONSE";
    }

    /// <summary>
    ///  ASV_CHART_DATA_RESPONSE
    /// </summary>
    public class AsvChartDataResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 10; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 10; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float data_rate
            +2 // uint16_t chat_id
            +2 // uint16_t chat_info_hash
            + 1 // uint8_t result
            + 1 // uint8_t data_trigger
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            DataRate = BinSerialize.ReadFloat(ref buffer);
            ChatId = BinSerialize.ReadUShort(ref buffer);
            ChatInfoHash = BinSerialize.ReadUShort(ref buffer);
            Result = (AsvChartRequestAck)BinSerialize.ReadByte(ref buffer);
            DataTrigger = (AsvChartDataTrigger)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,DataRate);
            BinSerialize.WriteUShort(ref buffer,ChatId);
            BinSerialize.WriteUShort(ref buffer,ChatInfoHash);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            BinSerialize.WriteByte(ref buffer,(byte)DataTrigger);
            /* PayloadByteSize = 10 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,DataRateField, ref _DataRate);    
            UInt16Type.Accept(visitor,ChatIdField, ref _ChatId);    
            UInt16Type.Accept(visitor,ChatInfoHashField, ref _ChatInfoHash);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ref tmpResult);
            Result = (AsvChartRequestAck)tmpResult;
            var tmpDataTrigger = (byte)DataTrigger;
            UInt8Type.Accept(visitor,DataTriggerField, ref tmpDataTrigger);
            DataTrigger = (AsvChartDataTrigger)tmpDataTrigger;

        }

        /// <summary>
        /// The requested message rate (delay in ms).
        /// OriginName: data_rate, Units: Ms, IsExtended: false
        /// </summary>
        public static readonly Field DataRateField = new Field.Builder()
            .Name(nameof(DataRate))
            .Title("data_rate")
            .Description("The requested message rate (delay in ms).")
            .FormatString(string.Empty)
            .Units(@"Ms")
            .DataType(FloatType.Default)

            .Build();
        private float _DataRate;
        public float DataRate { get => _DataRate; set { _DataRate = value; } }
        /// <summary>
        /// The ID of the requested chart
        /// OriginName: chat_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatIdField = new Field.Builder()
            .Name(nameof(ChatId))
            .Title("chat_id")
            .Description("The ID of the requested chart")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChatId;
        public ushort ChatId { get => _ChatId; set { _ChatId = value; } }
        /// <summary>
        /// Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.
        /// OriginName: chat_info_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatInfoHashField = new Field.Builder()
            .Name(nameof(ChatInfoHash))
            .Title("chat_info_hash")
            .Description("Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChatInfoHash;
        public ushort ChatInfoHash { get => _ChatInfoHash; set { _ChatInfoHash = value; } }
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
        public AsvChartRequestAck _Result;
        public AsvChartRequestAck Result { get => _Result; set => _Result = value; } 
        /// <summary>
        /// Additional argument for stream request.
        /// OriginName: data_trigger, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataTriggerField = new Field.Builder()
            .Name(nameof(DataTrigger))
            .Title("data_trigger")
            .Description("Additional argument for stream request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public AsvChartDataTrigger _DataTrigger;
        public AsvChartDataTrigger DataTrigger { get => _DataTrigger; set => _DataTrigger = value; } 
    }
    /// <summary>
    /// Raw chart data for visualization.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_CHART_DATA
    /// </summary>
    public class AsvChartDataPacket : MavlinkV2Message<AsvChartDataPayload>
    {
        public const int MessageId = 13360;
        
        public const byte CrcExtra = 66;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvChartDataPayload Payload { get; } = new();

        public override string Name => "ASV_CHART_DATA";
    }

    /// <summary>
    ///  ASV_CHART_DATA
    /// </summary>
    public class AsvChartDataPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 237; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 237; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_unix_usec
            +2 // uint16_t chat_id
            +2 // uint16_t chat_info_hash
            +2 // uint16_t pkt_in_frame
            +2 // uint16_t pkt_seq
            +1 // uint8_t data_size
            +Data.Length // uint8_t[220] data
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUnixUsec = BinSerialize.ReadULong(ref buffer);
            ChatId = BinSerialize.ReadUShort(ref buffer);
            ChatInfoHash = BinSerialize.ReadUShort(ref buffer);
            PktInFrame = BinSerialize.ReadUShort(ref buffer);
            PktSeq = BinSerialize.ReadUShort(ref buffer);
            DataSize = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/220 - Math.Max(0,((/*PayloadByteSize*/237 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUnixUsec);
            BinSerialize.WriteUShort(ref buffer,ChatId);
            BinSerialize.WriteUShort(ref buffer,ChatInfoHash);
            BinSerialize.WriteUShort(ref buffer,PktInFrame);
            BinSerialize.WriteUShort(ref buffer,PktSeq);
            BinSerialize.WriteByte(ref buffer,(byte)DataSize);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            /* PayloadByteSize = 237 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, ref _TimeUnixUsec);    
            UInt16Type.Accept(visitor,ChatIdField, ref _ChatId);    
            UInt16Type.Accept(visitor,ChatInfoHashField, ref _ChatInfoHash);    
            UInt16Type.Accept(visitor,PktInFrameField, ref _PktInFrame);    
            UInt16Type.Accept(visitor,PktSeqField, ref _PktSeq);    
            UInt8Type.Accept(visitor,DataSizeField, ref _DataSize);    
            ArrayType.Accept(visitor,DataField, 220,
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
        /// Chart id.
        /// OriginName: chat_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatIdField = new Field.Builder()
            .Name(nameof(ChatId))
            .Title("chat_id")
            .Description("Chart id.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChatId;
        public ushort ChatId { get => _ChatId; set { _ChatId = value; } }
        /// <summary>
        /// Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.
        /// OriginName: chat_info_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatInfoHashField = new Field.Builder()
            .Name(nameof(ChatInfoHash))
            .Title("chat_info_hash")
            .Description("Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ChatInfoHash;
        public ushort ChatInfoHash { get => _ChatInfoHash; set { _ChatInfoHash = value; } }
        /// <summary>
        /// Number of packets for one frame.
        /// OriginName: pkt_in_frame, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PktInFrameField = new Field.Builder()
            .Name(nameof(PktInFrame))
            .Title("pkt_in_frame")
            .Description("Number of packets for one frame.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _PktInFrame;
        public ushort PktInFrame { get => _PktInFrame; set { _PktInFrame = value; } }
        /// <summary>
        /// Packet sequence number (starting with 0 on every encoded frame).
        /// OriginName: pkt_seq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PktSeqField = new Field.Builder()
            .Name(nameof(PktSeq))
            .Title("pkt_seq")
            .Description("Packet sequence number (starting with 0 on every encoded frame).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _PktSeq;
        public ushort PktSeq { get => _PktSeq; set { _PktSeq = value; } }
        /// <summary>
        /// Size of data array.
        /// OriginName: data_size, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataSizeField = new Field.Builder()
            .Name(nameof(DataSize))
            .Title("data_size")
            .Description("Size of data array.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _DataSize;
        public byte DataSize { get => _DataSize; set { _DataSize = value; } }
        /// <summary>
        /// Chart data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Chart data.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,220))

            .Build();
        public const int DataMaxItemsCount = 220;
        public byte[] Data { get; } = new byte[220];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 220;
    }




        


#endregion


}

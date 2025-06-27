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
    public enum AsvChartRequestAck : ulong
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
    public static class AsvChartRequestAckHelper
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
            yield return new EnumValue<T>(converter(0),"ASV_CHART_REQUEST_ACK_OK");
            yield return new EnumValue<T>(converter(1),"ASV_CHART_REQUEST_ACK_IN_PROGRESS");
            yield return new EnumValue<T>(converter(2),"ASV_CHART_REQUEST_ACK_FAIL");
            yield return new EnumValue<T>(converter(3),"ASV_CHART_REQUEST_ACK_NOT_SUPPORTED");
            yield return new EnumValue<T>(converter(4),"ASV_CHART_REQUEST_ACK_NOT_FOUND");
        }
    }
    /// <summary>
    /// Chart data transmission data type
    ///  ASV_CHART_DATA_FORMAT
    /// </summary>
    public enum AsvChartDataFormat : ulong
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
    public static class AsvChartDataFormatHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_CHART_DATA_FORMAT_RANGE_FLOAT_8BIT");
            yield return new EnumValue<T>(converter(1),"ASV_CHART_DATA_FORMAT_RANGE_FLOAT_16BIT");
            yield return new EnumValue<T>(converter(2),"ASV_CHART_DATA_FORMAT_FLOAT");
        }
    }
    /// <summary>
    /// Args for requested stream
    ///  ASV_CHART_DATA_TRIGGER
    /// </summary>
    public enum AsvChartDataTrigger : ulong
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
    public static class AsvChartDataTriggerHelper
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
            yield return new EnumValue<T>(converter(0),"ASV_CHART_DATA_TRIGGER_DISABLE");
            yield return new EnumValue<T>(converter(1),"ASV_CHART_DATA_TRIGGER_ONCE");
            yield return new EnumValue<T>(converter(2),"ASV_CHART_DATA_TRIGGER_ON_CHANGED");
            yield return new EnumValue<T>(converter(3),"ASV_CHART_DATA_TRIGGER_PERIODIC");
        }
    }
    /// <summary>
    /// Chart type
    ///  ASV_CHART_TYPE
    /// </summary>
    public enum AsvChartType : ulong
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
    public static class AsvChartTypeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_CHART_TYPE_SIMPLE");
            yield return new EnumValue<T>(converter(1),"ASV_CHART_TYPE_HEATMAP");
        }
    }
    /// <summary>
    /// Chart type
    ///  ASV_CHART_UNIT_TYPE
    /// </summary>
    public enum AsvChartUnitType : ulong
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
    public static class AsvChartUnitTypeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_CHART_UNIT_TYPE_CUSTOM");
            yield return new EnumValue<T>(converter(1),"ASV_CHART_UNIT_TYPE_DBM");
        }
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

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RequestIdField, RequestIdField.DataType, ref _requestId);    
            UInt16Type.Accept(visitor,ItemsCountField, ItemsCountField.DataType, ref _itemsCount);    
            UInt16Type.Accept(visitor,ChatListHashField, ChatListHashField.DataType, ref _chatListHash);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ResultField.DataType, ref tmpResult);
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

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _requestId;
        public ushort RequestId { get => _requestId; set => _requestId = value; }
        /// <summary>
        /// Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ItemsCountField = new Field.Builder()
            .Name(nameof(ItemsCount))
            .Title("items_count")
            .Description("Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _itemsCount;
        public ushort ItemsCount { get => _itemsCount; set => _itemsCount = value; }
        /// <summary>
        /// Hash of the all ASV_CHART_INFO.
        /// OriginName: chat_list_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatListHashField = new Field.Builder()
            .Name(nameof(ChatListHash))
            .Title("chat_list_hash")
            .Description("Hash of the all ASV_CHART_INFO.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chatListHash;
        public ushort ChatListHash { get => _chatListHash; set => _chatListHash = value; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .DataType(new UInt8Type(AsvChartRequestAckHelper.GetValues(x=>(byte)x).Min(),AsvChartRequestAckHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvChartRequestAckHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvChartRequestAck _result;
        public AsvChartRequestAck Result { get => _result; set => _result = value; } 
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

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,ChartCountField, ChartCountField.DataType, ref _chartCount);    
            UInt16Type.Accept(visitor,ChatListHashField, ChatListHashField.DataType, ref _chatListHash);    

        }

        /// <summary>
        /// Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).
        /// OriginName: chart_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChartCountField = new Field.Builder()
            .Name(nameof(ChartCount))
            .Title("chart_count")
            .Description("Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chartCount;
        public ushort ChartCount { get => _chartCount; set => _chartCount = value; }
        /// <summary>
        /// Hash of the all ASV_CHART_INFO.
        /// OriginName: chat_list_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatListHashField = new Field.Builder()
            .Name(nameof(ChatListHash))
            .Title("chat_list_hash")
            .Description("Hash of the all ASV_CHART_INFO.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chatListHash;
        public ushort ChatListHash { get => _chatListHash; set => _chatListHash = value; }
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

        public void Accept(IVisitor visitor)
        {
            FloatType.Accept(visitor,AxesXMinField, AxesXMinField.DataType, ref _axesXMin);    
            FloatType.Accept(visitor,AxesXMaxField, AxesXMaxField.DataType, ref _axesXMax);    
            FloatType.Accept(visitor,AxesYMinField, AxesYMinField.DataType, ref _axesYMin);    
            FloatType.Accept(visitor,AxesYMaxField, AxesYMaxField.DataType, ref _axesYMax);    
            UInt16Type.Accept(visitor,ChartIdField, ChartIdField.DataType, ref _chartId);    
            UInt16Type.Accept(visitor,ChartInfoHashField, ChartInfoHashField.DataType, ref _chartInfoHash);    
            var tmpAxesXUnit = (ushort)AxesXUnit;
            UInt16Type.Accept(visitor,AxesXUnitField, AxesXUnitField.DataType, ref tmpAxesXUnit);
            AxesXUnit = (AsvChartUnitType)tmpAxesXUnit;
            UInt16Type.Accept(visitor,AxesXCountField, AxesXCountField.DataType, ref _axesXCount);    
            var tmpAxesYUnit = (ushort)AxesYUnit;
            UInt16Type.Accept(visitor,AxesYUnitField, AxesYUnitField.DataType, ref tmpAxesYUnit);
            AxesYUnit = (AsvChartUnitType)tmpAxesYUnit;
            UInt16Type.Accept(visitor,AxesYCountField, AxesYCountField.DataType, ref _axesYCount);    
            ArrayType.Accept(visitor,ChartNameField, ChartNameField.DataType, 16, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref ChartName[index]));
            var tmpChartType = (byte)ChartType;
            UInt8Type.Accept(visitor,ChartTypeField, ChartTypeField.DataType, ref tmpChartType);
            ChartType = (AsvChartType)tmpChartType;
            ArrayType.Accept(visitor,AxesXNameField, AxesXNameField.DataType, 16, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref AxesXName[index]));
            ArrayType.Accept(visitor,AxesYNameField, AxesYNameField.DataType, 16, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref AxesYName[index]));
            var tmpFormat = (byte)Format;
            UInt8Type.Accept(visitor,FormatField, FormatField.DataType, ref tmpFormat);
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

            .DataType(FloatType.Default)
        .Build();
        private float _axesXMin;
        public float AxesXMin { get => _axesXMin; set => _axesXMin = value; }
        /// <summary>
        /// Maximum value of Axis X.
        /// OriginName: axes_x_max, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesXMaxField = new Field.Builder()
            .Name(nameof(AxesXMax))
            .Title("axes_x_max")
            .Description("Maximum value of Axis X.")

            .DataType(FloatType.Default)
        .Build();
        private float _axesXMax;
        public float AxesXMax { get => _axesXMax; set => _axesXMax = value; }
        /// <summary>
        /// Minimum value of Axis Y.
        /// OriginName: axes_y_min, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesYMinField = new Field.Builder()
            .Name(nameof(AxesYMin))
            .Title("axes_y_min")
            .Description("Minimum value of Axis Y.")

            .DataType(FloatType.Default)
        .Build();
        private float _axesYMin;
        public float AxesYMin { get => _axesYMin; set => _axesYMin = value; }
        /// <summary>
        /// Maximum value of Axis Y.
        /// OriginName: axes_y_max, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesYMaxField = new Field.Builder()
            .Name(nameof(AxesYMax))
            .Title("axes_y_max")
            .Description("Maximum value of Axis Y.")

            .DataType(FloatType.Default)
        .Build();
        private float _axesYMax;
        public float AxesYMax { get => _axesYMax; set => _axesYMax = value; }
        /// <summary>
        /// Chart ID.
        /// OriginName: chart_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChartIdField = new Field.Builder()
            .Name(nameof(ChartId))
            .Title("chart_id")
            .Description("Chart ID.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chartId;
        public ushort ChartId { get => _chartId; set => _chartId = value; }
        /// <summary>
        /// Hash of the chart info.
        /// OriginName: chart_info_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChartInfoHashField = new Field.Builder()
            .Name(nameof(ChartInfoHash))
            .Title("chart_info_hash")
            .Description("Hash of the chart info.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chartInfoHash;
        public ushort ChartInfoHash { get => _chartInfoHash; set => _chartInfoHash = value; }
        /// <summary>
        /// Axis X unit.
        /// OriginName: axes_x_unit, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesXUnitField = new Field.Builder()
            .Name(nameof(AxesXUnit))
            .Title("axes_x_unit")
            .Description("Axis X unit.")
            .DataType(new UInt16Type(AsvChartUnitTypeHelper.GetValues(x=>(ushort)x).Min(),AsvChartUnitTypeHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(AsvChartUnitTypeHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private AsvChartUnitType _axesXUnit;
        public AsvChartUnitType AxesXUnit { get => _axesXUnit; set => _axesXUnit = value; } 
        /// <summary>
        /// Total measure points for Axis X. Dependent on chart type (1 measure for simple plot signal, more for heatmap signal).
        /// OriginName: axes_x_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesXCountField = new Field.Builder()
            .Name(nameof(AxesXCount))
            .Title("axes_x_count")
            .Description("Total measure points for Axis X. Dependent on chart type (1 measure for simple plot signal, more for heatmap signal).")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _axesXCount;
        public ushort AxesXCount { get => _axesXCount; set => _axesXCount = value; }
        /// <summary>
        /// Axis Y unit.
        /// OriginName: axes_y_unit, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesYUnitField = new Field.Builder()
            .Name(nameof(AxesYUnit))
            .Title("axes_y_unit")
            .Description("Axis Y unit.")
            .DataType(new UInt16Type(AsvChartUnitTypeHelper.GetValues(x=>(ushort)x).Min(),AsvChartUnitTypeHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(AsvChartUnitTypeHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private AsvChartUnitType _axesYUnit;
        public AsvChartUnitType AxesYUnit { get => _axesYUnit; set => _axesYUnit = value; } 
        /// <summary>
        /// Total measure points for Axis Y.
        /// OriginName: axes_y_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesYCountField = new Field.Builder()
            .Name(nameof(AxesYCount))
            .Title("axes_y_count")
            .Description("Total measure points for Axis Y.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _axesYCount;
        public ushort AxesYCount { get => _axesYCount; set => _axesYCount = value; }
        /// <summary>
        /// Chart name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.
        /// OriginName: chart_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChartNameField = new Field.Builder()
            .Name(nameof(ChartName))
            .Title("chart_name")
            .Description("Chart name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.")

            .DataType(new ArrayType(CharType.Ascii,16))
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
            .DataType(new UInt8Type(AsvChartTypeHelper.GetValues(x=>(byte)x).Min(),AsvChartTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvChartTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvChartType _chartType;
        public AsvChartType ChartType { get => _chartType; set => _chartType = value; } 
        /// <summary>
        /// Axis X name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.
        /// OriginName: axes_x_name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxesXNameField = new Field.Builder()
            .Name(nameof(AxesXName))
            .Title("axes_x_name")
            .Description("Axis X name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.")

            .DataType(new ArrayType(CharType.Ascii,16))
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

            .DataType(new ArrayType(CharType.Ascii,16))
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
            .DataType(new UInt8Type(AsvChartDataFormatHelper.GetValues(x=>(byte)x).Min(),AsvChartDataFormatHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvChartDataFormatHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvChartDataFormat _format;
        public AsvChartDataFormat Format { get => _format; set => _format = value; } 
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

        public void Accept(IVisitor visitor)
        {
            FloatType.Accept(visitor,DataRateField, DataRateField.DataType, ref _dataRate);    
            UInt16Type.Accept(visitor,ChatIdField, ChatIdField.DataType, ref _chatId);    
            UInt16Type.Accept(visitor,ChatInfoHashField, ChatInfoHashField.DataType, ref _chatInfoHash);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    
            var tmpDataTrigger = (byte)DataTrigger;
            UInt8Type.Accept(visitor,DataTriggerField, DataTriggerField.DataType, ref tmpDataTrigger);
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
.Units(@"Ms")
            .DataType(FloatType.Default)
        .Build();
        private float _dataRate;
        public float DataRate { get => _dataRate; set => _dataRate = value; }
        /// <summary>
        /// The ID of the requested chart
        /// OriginName: chat_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatIdField = new Field.Builder()
            .Name(nameof(ChatId))
            .Title("chat_id")
            .Description("The ID of the requested chart")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chatId;
        public ushort ChatId { get => _chatId; set => _chatId = value; }
        /// <summary>
        /// Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.
        /// OriginName: chat_info_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatInfoHashField = new Field.Builder()
            .Name(nameof(ChatInfoHash))
            .Title("chat_info_hash")
            .Description("Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chatInfoHash;
        public ushort ChatInfoHash { get => _chatInfoHash; set => _chatInfoHash = value; }
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
        /// Additional argument for stream request.
        /// OriginName: data_trigger, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataTriggerField = new Field.Builder()
            .Name(nameof(DataTrigger))
            .Title("data_trigger")
            .Description("Additional argument for stream request.")
            .DataType(new UInt8Type(AsvChartDataTriggerHelper.GetValues(x=>(byte)x).Min(),AsvChartDataTriggerHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvChartDataTriggerHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvChartDataTrigger _dataTrigger;
        public AsvChartDataTrigger DataTrigger { get => _dataTrigger; set => _dataTrigger = value; } 
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

        public void Accept(IVisitor visitor)
        {
            FloatType.Accept(visitor,DataRateField, DataRateField.DataType, ref _dataRate);    
            UInt16Type.Accept(visitor,ChatIdField, ChatIdField.DataType, ref _chatId);    
            UInt16Type.Accept(visitor,ChatInfoHashField, ChatInfoHashField.DataType, ref _chatInfoHash);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ResultField.DataType, ref tmpResult);
            Result = (AsvChartRequestAck)tmpResult;
            var tmpDataTrigger = (byte)DataTrigger;
            UInt8Type.Accept(visitor,DataTriggerField, DataTriggerField.DataType, ref tmpDataTrigger);
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
.Units(@"Ms")
            .DataType(FloatType.Default)
        .Build();
        private float _dataRate;
        public float DataRate { get => _dataRate; set => _dataRate = value; }
        /// <summary>
        /// The ID of the requested chart
        /// OriginName: chat_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatIdField = new Field.Builder()
            .Name(nameof(ChatId))
            .Title("chat_id")
            .Description("The ID of the requested chart")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chatId;
        public ushort ChatId { get => _chatId; set => _chatId = value; }
        /// <summary>
        /// Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.
        /// OriginName: chat_info_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatInfoHashField = new Field.Builder()
            .Name(nameof(ChatInfoHash))
            .Title("chat_info_hash")
            .Description("Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chatInfoHash;
        public ushort ChatInfoHash { get => _chatInfoHash; set => _chatInfoHash = value; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Result code.")
            .DataType(new UInt8Type(AsvChartRequestAckHelper.GetValues(x=>(byte)x).Min(),AsvChartRequestAckHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvChartRequestAckHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvChartRequestAck _result;
        public AsvChartRequestAck Result { get => _result; set => _result = value; } 
        /// <summary>
        /// Additional argument for stream request.
        /// OriginName: data_trigger, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataTriggerField = new Field.Builder()
            .Name(nameof(DataTrigger))
            .Title("data_trigger")
            .Description("Additional argument for stream request.")
            .DataType(new UInt8Type(AsvChartDataTriggerHelper.GetValues(x=>(byte)x).Min(),AsvChartDataTriggerHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvChartDataTriggerHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvChartDataTrigger _dataTrigger;
        public AsvChartDataTrigger DataTrigger { get => _dataTrigger; set => _dataTrigger = value; } 
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

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUnixUsecField, TimeUnixUsecField.DataType, ref _timeUnixUsec);    
            UInt16Type.Accept(visitor,ChatIdField, ChatIdField.DataType, ref _chatId);    
            UInt16Type.Accept(visitor,ChatInfoHashField, ChatInfoHashField.DataType, ref _chatInfoHash);    
            UInt16Type.Accept(visitor,PktInFrameField, PktInFrameField.DataType, ref _pktInFrame);    
            UInt16Type.Accept(visitor,PktSeqField, PktSeqField.DataType, ref _pktSeq);    
            UInt8Type.Accept(visitor,DataSizeField, DataSizeField.DataType, ref _dataSize);    
            ArrayType.Accept(visitor,DataField, DataField.DataType, 220,
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
        /// Chart id.
        /// OriginName: chat_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatIdField = new Field.Builder()
            .Name(nameof(ChatId))
            .Title("chat_id")
            .Description("Chart id.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chatId;
        public ushort ChatId { get => _chatId; set => _chatId = value; }
        /// <summary>
        /// Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.
        /// OriginName: chat_info_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChatInfoHashField = new Field.Builder()
            .Name(nameof(ChatInfoHash))
            .Title("chat_info_hash")
            .Description("Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _chatInfoHash;
        public ushort ChatInfoHash { get => _chatInfoHash; set => _chatInfoHash = value; }
        /// <summary>
        /// Number of packets for one frame.
        /// OriginName: pkt_in_frame, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PktInFrameField = new Field.Builder()
            .Name(nameof(PktInFrame))
            .Title("pkt_in_frame")
            .Description("Number of packets for one frame.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _pktInFrame;
        public ushort PktInFrame { get => _pktInFrame; set => _pktInFrame = value; }
        /// <summary>
        /// Packet sequence number (starting with 0 on every encoded frame).
        /// OriginName: pkt_seq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PktSeqField = new Field.Builder()
            .Name(nameof(PktSeq))
            .Title("pkt_seq")
            .Description("Packet sequence number (starting with 0 on every encoded frame).")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _pktSeq;
        public ushort PktSeq { get => _pktSeq; set => _pktSeq = value; }
        /// <summary>
        /// Size of data array.
        /// OriginName: data_size, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataSizeField = new Field.Builder()
            .Name(nameof(DataSize))
            .Title("data_size")
            .Description("Size of data array.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _dataSize;
        public byte DataSize { get => _dataSize; set => _dataSize = value; }
        /// <summary>
        /// Chart data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Chart data.")

            .DataType(new ArrayType(UInt8Type.Default,220))
        .Build();
        public const int DataMaxItemsCount = 220;
        public byte[] Data { get; } = new byte[220];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 220;
    }




        


#endregion


}

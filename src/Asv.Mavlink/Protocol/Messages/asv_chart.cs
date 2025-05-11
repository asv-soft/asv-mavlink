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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.15+a2f1de3777820636a46d83925144e965a9eb2291 25-05-11.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
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
        public const string FormatMessage = "ASV_CHART_INFO_REQUEST:"
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
            "Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "chat_list_hash",
            "Hash of the all ASV_CHART_INFO.",
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
        ];
        public const string FormatMessage = "ASV_CHART_INFO_RESPONSE:"
        + "uint16_t request_id;"
        + "uint16_t items_count;"
        + "uint16_t chat_list_hash;"
        + "uint8_t result;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RequestId);
            writer.Write(StaticFields[1], Payload.ItemsCount);
            writer.Write(StaticFields[2], Payload.ChatListHash);
            writer.Write(StaticFields[3], Payload.Result);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RequestId = reader.ReadUShort(StaticFields[0]);
            Payload.ItemsCount = reader.ReadUShort(StaticFields[1]);
            Payload.ChatListHash = reader.ReadUShort(StaticFields[2]);
            Payload.Result = (AsvChartRequestAck)reader.ReadByte(StaticFields[3]);
        
            
        }
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
        
        



        /// <summary>
        /// Specifies the unique number of the original request. This allows the response to be matched to the correct request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public ushort RequestId { get; set; }
        /// <summary>
        /// Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).
        /// OriginName: items_count, Units: , IsExtended: false
        /// </summary>
        public ushort ItemsCount { get; set; }
        /// <summary>
        /// Hash of the all ASV_CHART_INFO.
        /// OriginName: chat_list_hash, Units: , IsExtended: false
        /// </summary>
        public ushort ChatListHash { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvChartRequestAck Result { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "chart_count",
            "Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "chat_list_hash",
            "Hash of the all ASV_CHART_INFO.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_CHART_INFO_UPDATED_EVENT:"
        + "uint16_t chart_count;"
        + "uint16_t chat_list_hash;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.ChartCount);
            writer.Write(StaticFields[1], Payload.ChatListHash);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.ChartCount = reader.ReadUShort(StaticFields[0]);
            Payload.ChatListHash = reader.ReadUShort(StaticFields[1]);
        
            
        }
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
        
        



        /// <summary>
        /// Number of ASV_CHART_INFO items to be transmitted after this response with a success result code (dependent on the request).
        /// OriginName: chart_count, Units: , IsExtended: false
        /// </summary>
        public ushort ChartCount { get; set; }
        /// <summary>
        /// Hash of the all ASV_CHART_INFO.
        /// OriginName: chat_list_hash, Units: , IsExtended: false
        /// </summary>
        public ushort ChatListHash { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "axes_x_min",
            "Minimum value of Axis X.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(1,
            "axes_x_max",
            "Maximum value of Axis X.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(2,
            "axes_y_min",
            "Minimum value of Axis Y.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(3,
            "axes_y_max",
            "Maximum value of Axis Y.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(4,
            "chart_id",
            "Chart ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(5,
            "chart_info_hash",
            "Hash of the chart info.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(6,
            "axes_x_unit",
            "Axis X unit.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(7,
            "axes_x_count",
            "Total measure points for Axis X. Dependent on chart type (1 measure for simple plot signal, more for heatmap signal).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(8,
            "axes_y_unit",
            "Axis Y unit.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(9,
            "axes_y_count",
            "Total measure points for Axis Y.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(10,
            "chart_name",
            "Chart name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            16, 
            false),
            new(11,
            "chart_type",
            "Type of chart.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(12,
            "axes_x_name",
            "Axis X name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            16, 
            false),
            new(13,
            "axes_y_name",
            "Axis Y name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            16, 
            false),
            new(14,
            "format",
            "Format of one measure.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_CHART_INFO:"
        + "float axes_x_min;"
        + "float axes_x_max;"
        + "float axes_y_min;"
        + "float axes_y_max;"
        + "uint16_t chart_id;"
        + "uint16_t chart_info_hash;"
        + "uint16_t axes_x_unit;"
        + "uint16_t axes_x_count;"
        + "uint16_t axes_y_unit;"
        + "uint16_t axes_y_count;"
        + "char[16] chart_name;"
        + "uint8_t chart_type;"
        + "char[16] axes_x_name;"
        + "char[16] axes_y_name;"
        + "uint8_t format;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.AxesXMin);
            writer.Write(StaticFields[1], Payload.AxesXMax);
            writer.Write(StaticFields[2], Payload.AxesYMin);
            writer.Write(StaticFields[3], Payload.AxesYMax);
            writer.Write(StaticFields[4], Payload.ChartId);
            writer.Write(StaticFields[5], Payload.ChartInfoHash);
            writer.Write(StaticFields[6], Payload.AxesXUnit);
            writer.Write(StaticFields[7], Payload.AxesXCount);
            writer.Write(StaticFields[8], Payload.AxesYUnit);
            writer.Write(StaticFields[9], Payload.AxesYCount);
            writer.Write(StaticFields[10], Payload.ChartName);
            writer.Write(StaticFields[11], Payload.ChartType);
            writer.Write(StaticFields[12], Payload.AxesXName);
            writer.Write(StaticFields[13], Payload.AxesYName);
            writer.Write(StaticFields[14], Payload.Format);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.AxesXMin = reader.ReadFloat(StaticFields[0]);
            Payload.AxesXMax = reader.ReadFloat(StaticFields[1]);
            Payload.AxesYMin = reader.ReadFloat(StaticFields[2]);
            Payload.AxesYMax = reader.ReadFloat(StaticFields[3]);
            Payload.ChartId = reader.ReadUShort(StaticFields[4]);
            Payload.ChartInfoHash = reader.ReadUShort(StaticFields[5]);
            Payload.AxesXUnit = (AsvChartUnitType)reader.ReadUShort(StaticFields[6]);
            Payload.AxesXCount = reader.ReadUShort(StaticFields[7]);
            Payload.AxesYUnit = (AsvChartUnitType)reader.ReadUShort(StaticFields[8]);
            Payload.AxesYCount = reader.ReadUShort(StaticFields[9]);
            reader.ReadCharArray(StaticFields[10], Payload.ChartName);
            Payload.ChartType = (AsvChartType)reader.ReadByte(StaticFields[11]);
            reader.ReadCharArray(StaticFields[12], Payload.AxesXName);
            reader.ReadCharArray(StaticFields[13], Payload.AxesYName);
            Payload.Format = (AsvChartDataFormat)reader.ReadByte(StaticFields[14]);
        
            
        }
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
            ChartName = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ChartName)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, ChartName.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
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
        /// Chart ID.
        /// OriginName: chart_id, Units: , IsExtended: false
        /// </summary>
        public ushort ChartId { get; set; }
        /// <summary>
        /// Hash of the chart info.
        /// OriginName: chart_info_hash, Units: , IsExtended: false
        /// </summary>
        public ushort ChartInfoHash { get; set; }
        /// <summary>
        /// Axis X unit.
        /// OriginName: axes_x_unit, Units: , IsExtended: false
        /// </summary>
        public AsvChartUnitType AxesXUnit { get; set; }
        /// <summary>
        /// Total measure points for Axis X. Dependent on chart type (1 measure for simple plot signal, more for heatmap signal).
        /// OriginName: axes_x_count, Units: , IsExtended: false
        /// </summary>
        public ushort AxesXCount { get; set; }
        /// <summary>
        /// Axis Y unit.
        /// OriginName: axes_y_unit, Units: , IsExtended: false
        /// </summary>
        public AsvChartUnitType AxesYUnit { get; set; }
        /// <summary>
        /// Total measure points for Axis Y.
        /// OriginName: axes_y_count, Units: , IsExtended: false
        /// </summary>
        public ushort AxesYCount { get; set; }
        /// <summary>
        /// Chart name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.
        /// OriginName: chart_name, Units: , IsExtended: false
        /// </summary>
        public const int ChartNameMaxItemsCount = 16;
        public char[] ChartName { get; set; } = new char[16];
        [Obsolete("This method is deprecated. Use GetChartNameMaxItemsCount instead.")]
        public byte GetChartNameMaxItemsCount() => 16;
        /// <summary>
        /// Type of chart.
        /// OriginName: chart_type, Units: , IsExtended: false
        /// </summary>
        public AsvChartType ChartType { get; set; }
        /// <summary>
        /// Axis X name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.
        /// OriginName: axes_x_name, Units: , IsExtended: false
        /// </summary>
        public const int AxesXNameMaxItemsCount = 16;
        public char[] AxesXName { get; } = new char[16];
        /// <summary>
        /// Axis Y name, terminated by NULL if the length is less than 16 human-readable chars, and WITHOUT null termination (NULL) byte if the length is exactly 16 chars. Applications have to provide 8+1 bytes storage if the ID is stored as a string.
        /// OriginName: axes_y_name, Units: , IsExtended: false
        /// </summary>
        public const int AxesYNameMaxItemsCount = 16;
        public char[] AxesYName { get; } = new char[16];
        /// <summary>
        /// Format of one measure.
        /// OriginName: format, Units: , IsExtended: false
        /// </summary>
        public AsvChartDataFormat Format { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "data_rate",
            "The requested message rate (delay in ms)",
            string.Empty, 
            @"Ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(1,
            "chat_id",
            "The ID of the requested chart",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "chat_info_hash",
            "Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.",
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
            "data_trigger",
            "Additional argument for stream request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_CHART_DATA_REQUEST:"
        + "float data_rate;"
        + "uint16_t chat_id;"
        + "uint16_t chat_info_hash;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t data_trigger;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.DataRate);
            writer.Write(StaticFields[1], Payload.ChatId);
            writer.Write(StaticFields[2], Payload.ChatInfoHash);
            writer.Write(StaticFields[3], Payload.TargetSystem);
            writer.Write(StaticFields[4], Payload.TargetComponent);
            writer.Write(StaticFields[5], Payload.DataTrigger);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.DataRate = reader.ReadFloat(StaticFields[0]);
            Payload.ChatId = reader.ReadUShort(StaticFields[1]);
            Payload.ChatInfoHash = reader.ReadUShort(StaticFields[2]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[3]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[4]);
            Payload.DataTrigger = (AsvChartDataTrigger)reader.ReadByte(StaticFields[5]);
        
            
        }
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
        
        



        /// <summary>
        /// The requested message rate (delay in ms)
        /// OriginName: data_rate, Units: Ms, IsExtended: false
        /// </summary>
        public float DataRate { get; set; }
        /// <summary>
        /// The ID of the requested chart
        /// OriginName: chat_id, Units: , IsExtended: false
        /// </summary>
        public ushort ChatId { get; set; }
        /// <summary>
        /// Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.
        /// OriginName: chat_info_hash, Units: , IsExtended: false
        /// </summary>
        public ushort ChatInfoHash { get; set; }
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
        /// Additional argument for stream request.
        /// OriginName: data_trigger, Units: , IsExtended: false
        /// </summary>
        public AsvChartDataTrigger DataTrigger { get; set; }
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
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "data_rate",
            "The requested message rate (delay in ms).",
            string.Empty, 
            @"Ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(1,
            "chat_id",
            "The ID of the requested chart",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "chat_info_hash",
            "Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.",
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
            "data_trigger",
            "Additional argument for stream request.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASV_CHART_DATA_RESPONSE:"
        + "float data_rate;"
        + "uint16_t chat_id;"
        + "uint16_t chat_info_hash;"
        + "uint8_t result;"
        + "uint8_t data_trigger;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.DataRate);
            writer.Write(StaticFields[1], Payload.ChatId);
            writer.Write(StaticFields[2], Payload.ChatInfoHash);
            writer.Write(StaticFields[3], Payload.Result);
            writer.Write(StaticFields[4], Payload.DataTrigger);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.DataRate = reader.ReadFloat(StaticFields[0]);
            Payload.ChatId = reader.ReadUShort(StaticFields[1]);
            Payload.ChatInfoHash = reader.ReadUShort(StaticFields[2]);
            Payload.Result = (AsvChartRequestAck)reader.ReadByte(StaticFields[3]);
            Payload.DataTrigger = (AsvChartDataTrigger)reader.ReadByte(StaticFields[4]);
        
            
        }
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
        
        



        /// <summary>
        /// The requested message rate (delay in ms).
        /// OriginName: data_rate, Units: Ms, IsExtended: false
        /// </summary>
        public float DataRate { get; set; }
        /// <summary>
        /// The ID of the requested chart
        /// OriginName: chat_id, Units: , IsExtended: false
        /// </summary>
        public ushort ChatId { get; set; }
        /// <summary>
        /// Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.
        /// OriginName: chat_info_hash, Units: , IsExtended: false
        /// </summary>
        public ushort ChatInfoHash { get; set; }
        /// <summary>
        /// Result code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public AsvChartRequestAck Result { get; set; }
        /// <summary>
        /// Additional argument for stream request.
        /// OriginName: data_trigger, Units: , IsExtended: false
        /// </summary>
        public AsvChartDataTrigger DataTrigger { get; set; }
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
            "chat_id",
            "Chart id.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "chat_info_hash",
            "Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "pkt_in_frame",
            "Number of packets for one frame.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(4,
            "pkt_seq",
            "Packet sequence number (starting with 0 on every encoded frame).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(5,
            "data_size",
            "Size of data array.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(6,
            "data",
            "Chart data.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            220, 
            false),
        ];
        public const string FormatMessage = "ASV_CHART_DATA:"
        + "uint64_t time_unix_usec;"
        + "uint16_t chat_id;"
        + "uint16_t chat_info_hash;"
        + "uint16_t pkt_in_frame;"
        + "uint16_t pkt_seq;"
        + "uint8_t data_size;"
        + "uint8_t[220] data;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TimeUnixUsec);
            writer.Write(StaticFields[1], Payload.ChatId);
            writer.Write(StaticFields[2], Payload.ChatInfoHash);
            writer.Write(StaticFields[3], Payload.PktInFrame);
            writer.Write(StaticFields[4], Payload.PktSeq);
            writer.Write(StaticFields[5], Payload.DataSize);
            writer.Write(StaticFields[6], Payload.Data);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TimeUnixUsec = reader.ReadULong(StaticFields[0]);
            Payload.ChatId = reader.ReadUShort(StaticFields[1]);
            Payload.ChatInfoHash = reader.ReadUShort(StaticFields[2]);
            Payload.PktInFrame = reader.ReadUShort(StaticFields[3]);
            Payload.PktSeq = reader.ReadUShort(StaticFields[4]);
            Payload.DataSize = reader.ReadByte(StaticFields[5]);
            reader.ReadByteArray(StaticFields[6], Payload.Data);
        
            
        }
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
            Data = new byte[arraySize];
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
        
        



        /// <summary>
        /// Timestamp (UNIX epoch time) for current set of measures.
        /// OriginName: time_unix_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUnixUsec { get; set; }
        /// <summary>
        /// Chart id.
        /// OriginName: chat_id, Units: , IsExtended: false
        /// </summary>
        public ushort ChatId { get; set; }
        /// <summary>
        /// Hash of the chart ASV_CHART_INFO to ensure that all settings are synchronized.
        /// OriginName: chat_info_hash, Units: , IsExtended: false
        /// </summary>
        public ushort ChatInfoHash { get; set; }
        /// <summary>
        /// Number of packets for one frame.
        /// OriginName: pkt_in_frame, Units: , IsExtended: false
        /// </summary>
        public ushort PktInFrame { get; set; }
        /// <summary>
        /// Packet sequence number (starting with 0 on every encoded frame).
        /// OriginName: pkt_seq, Units: , IsExtended: false
        /// </summary>
        public ushort PktSeq { get; set; }
        /// <summary>
        /// Size of data array.
        /// OriginName: data_size, Units: , IsExtended: false
        /// </summary>
        public byte DataSize { get; set; }
        /// <summary>
        /// Chart data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public const int DataMaxItemsCount = 220;
        public byte[] Data { get; set; } = new byte[220];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 220;
    }


#endregion


}

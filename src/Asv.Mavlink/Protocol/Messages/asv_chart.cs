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

// This code was generate by tool Asv.Mavlink.Shell version 3.10.4+c1002429a625f2cf26c5bd2680700906e0b44d76

using System;
using System.Text;
using System.ComponentModel;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Asv.IO;

namespace Asv.Mavlink.V2.AsvChart
{

    public static class AsvChartHelper
    {
        public static void RegisterAsvChartDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AsvChartInfoRequestPacket());
            src.Register(()=>new AsvChartInfoResponsePacket());
            src.Register(()=>new AsvChartInfoUpdatedEventPacket());
            src.Register(()=>new AsvChartInfoPacket());
            src.Register(()=>new AsvChartDataRequestPacket());
            src.Register(()=>new AsvChartDataResponsePacket());
            src.Register(()=>new AsvChartDataPacket());
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
    public class AsvChartInfoRequestPacket: PacketV2<AsvChartInfoRequestPayload>
    {
	    public const int PacketMessageId = 13350;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 131;
        public override bool WrapToV2Extension => true;

        public override AsvChartInfoRequestPayload Payload { get; } = new AsvChartInfoRequestPayload();

        public override string Name => "ASV_CHART_INFO_REQUEST";
    }

    /// <summary>
    ///  ASV_CHART_INFO_REQUEST
    /// </summary>
    public class AsvChartInfoRequestPayload : IPayload
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
    public class AsvChartInfoResponsePacket: PacketV2<AsvChartInfoResponsePayload>
    {
	    public const int PacketMessageId = 13351;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 109;
        public override bool WrapToV2Extension => true;

        public override AsvChartInfoResponsePayload Payload { get; } = new AsvChartInfoResponsePayload();

        public override string Name => "ASV_CHART_INFO_RESPONSE";
    }

    /// <summary>
    ///  ASV_CHART_INFO_RESPONSE
    /// </summary>
    public class AsvChartInfoResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 7; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RequestId
            sum+=2; //ItemsCount
            sum+=2; //ChatListHash
            sum+= 1; // Result
            return (byte)sum;
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
    public class AsvChartInfoUpdatedEventPacket: PacketV2<AsvChartInfoUpdatedEventPayload>
    {
	    public const int PacketMessageId = 13352;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 37;
        public override bool WrapToV2Extension => true;

        public override AsvChartInfoUpdatedEventPayload Payload { get; } = new AsvChartInfoUpdatedEventPayload();

        public override string Name => "ASV_CHART_INFO_UPDATED_EVENT";
    }

    /// <summary>
    ///  ASV_CHART_INFO_UPDATED_EVENT
    /// </summary>
    public class AsvChartInfoUpdatedEventPayload : IPayload
    {
        public byte GetMaxByteSize() => 4; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 4; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //ChartCount
            sum+=2; //ChatListHash
            return (byte)sum;
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
    public class AsvChartInfoPacket: PacketV2<AsvChartInfoPayload>
    {
	    public const int PacketMessageId = 13353;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 159;
        public override bool WrapToV2Extension => true;

        public override AsvChartInfoPayload Payload { get; } = new AsvChartInfoPayload();

        public override string Name => "ASV_CHART_INFO";
    }

    /// <summary>
    ///  ASV_CHART_INFO
    /// </summary>
    public class AsvChartInfoPayload : IPayload
    {
        public byte GetMaxByteSize() => 78; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 78; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //AxesXMin
            sum+=4; //AxesXMax
            sum+=4; //AxesYMin
            sum+=4; //AxesYMax
            sum+=2; //ChartId
            sum+=2; //ChartInfoHash
            sum+= 2; // AxesXUnit
            sum+=2; //AxesXCount
            sum+= 2; // AxesYUnit
            sum+=2; //AxesYCount
            sum+=ChartName.Length; //ChartName
            sum+= 1; // ChartType
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
    public class AsvChartDataRequestPacket: PacketV2<AsvChartDataRequestPayload>
    {
	    public const int PacketMessageId = 13354;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 4;
        public override bool WrapToV2Extension => true;

        public override AsvChartDataRequestPayload Payload { get; } = new AsvChartDataRequestPayload();

        public override string Name => "ASV_CHART_DATA_REQUEST";
    }

    /// <summary>
    ///  ASV_CHART_DATA_REQUEST
    /// </summary>
    public class AsvChartDataRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 11; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 11; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //DataRate
            sum+=2; //ChatId
            sum+=2; //ChatInfoHash
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+= 1; // DataTrigger
            return (byte)sum;
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
    public class AsvChartDataResponsePacket: PacketV2<AsvChartDataResponsePayload>
    {
	    public const int PacketMessageId = 13355;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 185;
        public override bool WrapToV2Extension => true;

        public override AsvChartDataResponsePayload Payload { get; } = new AsvChartDataResponsePayload();

        public override string Name => "ASV_CHART_DATA_RESPONSE";
    }

    /// <summary>
    ///  ASV_CHART_DATA_RESPONSE
    /// </summary>
    public class AsvChartDataResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 10; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 10; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //DataRate
            sum+=2; //ChatId
            sum+=2; //ChatInfoHash
            sum+= 1; // Result
            sum+= 1; // DataTrigger
            return (byte)sum;
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
    public class AsvChartDataPacket: PacketV2<AsvChartDataPayload>
    {
	    public const int PacketMessageId = 13360;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 66;
        public override bool WrapToV2Extension => true;

        public override AsvChartDataPayload Payload { get; } = new AsvChartDataPayload();

        public override string Name => "ASV_CHART_DATA";
    }

    /// <summary>
    ///  ASV_CHART_DATA
    /// </summary>
    public class AsvChartDataPayload : IPayload
    {
        public byte GetMaxByteSize() => 237; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 237; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUnixUsec
            sum+=2; //ChatId
            sum+=2; //ChatInfoHash
            sum+=2; //PktInFrame
            sum+=2; //PktSeq
            sum+=1; //DataSize
            sum+=Data.Length; //Data
            return (byte)sum;
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

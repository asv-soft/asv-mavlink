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

namespace Asv.Mavlink.Cubepilot
{

    public static class CubepilotHelper
    {
        public static void RegisterCubepilotDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(CubepilotRawRcPacket.MessageId, ()=>new CubepilotRawRcPacket());
            src.Add(HerelinkVideoStreamInformationPacket.MessageId, ()=>new HerelinkVideoStreamInformationPacket());
            src.Add(HerelinkTelemPacket.MessageId, ()=>new HerelinkTelemPacket());
            src.Add(CubepilotFirmwareUpdateStartPacket.MessageId, ()=>new CubepilotFirmwareUpdateStartPacket());
            src.Add(CubepilotFirmwareUpdateRespPacket.MessageId, ()=>new CubepilotFirmwareUpdateRespPacket());
        }
    }

#region Enums


#endregion

#region Messages

    /// <summary>
    /// Raw RC Data
    ///  CUBEPILOT_RAW_RC
    /// </summary>
    public class CubepilotRawRcPacket : MavlinkV2Message<CubepilotRawRcPayload>
    {
        public const int MessageId = 50001;
        
        public const byte CrcExtra = 246;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override CubepilotRawRcPayload Payload { get; } = new();

        public override string Name => "CUBEPILOT_RAW_RC";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "rc_raw",
            "",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            32, 
            false),
        ];
        public const string FormatMessage = "CUBEPILOT_RAW_RC:"
        + "uint8_t[32] rc_raw;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RcRaw);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            reader.ReadByteArray(StaticFields[0], Payload.RcRaw);
        
            
        }
    }

    /// <summary>
    ///  CUBEPILOT_RAW_RC
    /// </summary>
    public class CubepilotRawRcPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 32; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 32; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +RcRaw.Length // uint8_t[32] rc_raw
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/32 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            RcRaw = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                RcRaw[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<RcRaw.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)RcRaw[i]);
            }
            /* PayloadByteSize = 32 */;
        }
        
        



        /// <summary>
        /// 
        /// OriginName: rc_raw, Units: , IsExtended: false
        /// </summary>
        public const int RcRawMaxItemsCount = 32;
        public byte[] RcRaw { get; set; } = new byte[32];
        [Obsolete("This method is deprecated. Use GetRcRawMaxItemsCount instead.")]
        public byte GetRcRawMaxItemsCount() => 32;
    }
    /// <summary>
    /// Information about video stream
    ///  HERELINK_VIDEO_STREAM_INFORMATION
    /// </summary>
    public class HerelinkVideoStreamInformationPacket : MavlinkV2Message<HerelinkVideoStreamInformationPayload>
    {
        public const int MessageId = 50002;
        
        public const byte CrcExtra = 181;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override HerelinkVideoStreamInformationPayload Payload { get; } = new();

        public override string Name => "HERELINK_VIDEO_STREAM_INFORMATION";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "framerate",
            "Frame rate.",
            string.Empty, 
            @"Hz", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(1,
            "bitrate",
            "Bit rate.",
            string.Empty, 
            @"bits/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(2,
            "resolution_h",
            "Horizontal resolution.",
            string.Empty, 
            @"pix", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "resolution_v",
            "Vertical resolution.",
            string.Empty, 
            @"pix", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(4,
            "rotation",
            "Video image rotation clockwise.",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(5,
            "camera_id",
            "Video Stream ID (1 for first, 2 for second, etc.)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(6,
            "status",
            "Number of streams available.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(7,
            "uri",
            "Video stream URI (TCP or RTSP URI ground station should connect to) or port number (UDP port ground station should listen to).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            230, 
            false),
        ];
        public const string FormatMessage = "HERELINK_VIDEO_STREAM_INFORMATION:"
        + "float framerate;"
        + "uint32_t bitrate;"
        + "uint16_t resolution_h;"
        + "uint16_t resolution_v;"
        + "uint16_t rotation;"
        + "uint8_t camera_id;"
        + "uint8_t status;"
        + "char[230] uri;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Framerate);
            writer.Write(StaticFields[1], Payload.Bitrate);
            writer.Write(StaticFields[2], Payload.ResolutionH);
            writer.Write(StaticFields[3], Payload.ResolutionV);
            writer.Write(StaticFields[4], Payload.Rotation);
            writer.Write(StaticFields[5], Payload.CameraId);
            writer.Write(StaticFields[6], Payload.Status);
            writer.Write(StaticFields[7], Payload.Uri);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Framerate = reader.ReadFloat(StaticFields[0]);
            Payload.Bitrate = reader.ReadUInt(StaticFields[1]);
            Payload.ResolutionH = reader.ReadUShort(StaticFields[2]);
            Payload.ResolutionV = reader.ReadUShort(StaticFields[3]);
            Payload.Rotation = reader.ReadUShort(StaticFields[4]);
            Payload.CameraId = reader.ReadByte(StaticFields[5]);
            Payload.Status = reader.ReadByte(StaticFields[6]);
            reader.ReadCharArray(StaticFields[7], Payload.Uri);
        
            
        }
    }

    /// <summary>
    ///  HERELINK_VIDEO_STREAM_INFORMATION
    /// </summary>
    public class HerelinkVideoStreamInformationPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 246; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 246; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float framerate
            +4 // uint32_t bitrate
            +2 // uint16_t resolution_h
            +2 // uint16_t resolution_v
            +2 // uint16_t rotation
            +1 // uint8_t camera_id
            +1 // uint8_t status
            +Uri.Length // char[230] uri
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Framerate = BinSerialize.ReadFloat(ref buffer);
            Bitrate = BinSerialize.ReadUInt(ref buffer);
            ResolutionH = BinSerialize.ReadUShort(ref buffer);
            ResolutionV = BinSerialize.ReadUShort(ref buffer);
            Rotation = BinSerialize.ReadUShort(ref buffer);
            CameraId = (byte)BinSerialize.ReadByte(ref buffer);
            Status = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/230 - Math.Max(0,((/*PayloadByteSize*/246 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Uri = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Uri)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Uri.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Framerate);
            BinSerialize.WriteUInt(ref buffer,Bitrate);
            BinSerialize.WriteUShort(ref buffer,ResolutionH);
            BinSerialize.WriteUShort(ref buffer,ResolutionV);
            BinSerialize.WriteUShort(ref buffer,Rotation);
            BinSerialize.WriteByte(ref buffer,(byte)CameraId);
            BinSerialize.WriteByte(ref buffer,(byte)Status);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Uri)
                {
                    Encoding.ASCII.GetBytes(charPointer, Uri.Length, bytePointer, Uri.Length);
                }
            }
            buffer = buffer.Slice(Uri.Length);
            
            /* PayloadByteSize = 246 */;
        }
        
        



        /// <summary>
        /// Frame rate.
        /// OriginName: framerate, Units: Hz, IsExtended: false
        /// </summary>
        public float Framerate { get; set; }
        /// <summary>
        /// Bit rate.
        /// OriginName: bitrate, Units: bits/s, IsExtended: false
        /// </summary>
        public uint Bitrate { get; set; }
        /// <summary>
        /// Horizontal resolution.
        /// OriginName: resolution_h, Units: pix, IsExtended: false
        /// </summary>
        public ushort ResolutionH { get; set; }
        /// <summary>
        /// Vertical resolution.
        /// OriginName: resolution_v, Units: pix, IsExtended: false
        /// </summary>
        public ushort ResolutionV { get; set; }
        /// <summary>
        /// Video image rotation clockwise.
        /// OriginName: rotation, Units: deg, IsExtended: false
        /// </summary>
        public ushort Rotation { get; set; }
        /// <summary>
        /// Video Stream ID (1 for first, 2 for second, etc.)
        /// OriginName: camera_id, Units: , IsExtended: false
        /// </summary>
        public byte CameraId { get; set; }
        /// <summary>
        /// Number of streams available.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public byte Status { get; set; }
        /// <summary>
        /// Video stream URI (TCP or RTSP URI ground station should connect to) or port number (UDP port ground station should listen to).
        /// OriginName: uri, Units: , IsExtended: false
        /// </summary>
        public const int UriMaxItemsCount = 230;
        public char[] Uri { get; set; } = new char[230];
        [Obsolete("This method is deprecated. Use GetUriMaxItemsCount instead.")]
        public byte GetUriMaxItemsCount() => 230;
    }
    /// <summary>
    /// Herelink Telemetry
    ///  HERELINK_TELEM
    /// </summary>
    public class HerelinkTelemPacket : MavlinkV2Message<HerelinkTelemPayload>
    {
        public const int MessageId = 50003;
        
        public const byte CrcExtra = 62;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override HerelinkTelemPayload Payload { get; } = new();

        public override string Name => "HERELINK_TELEM";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "rf_freq",
            "",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "link_bw",
            "",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(2,
            "link_rate",
            "",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(3,
            "snr",
            "",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(4,
            "cpu_temp",
            "",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(5,
            "board_temp",
            "",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new(6,
            "rssi",
            "",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "HERELINK_TELEM:"
        + "uint32_t rf_freq;"
        + "uint32_t link_bw;"
        + "uint32_t link_rate;"
        + "int16_t snr;"
        + "int16_t cpu_temp;"
        + "int16_t board_temp;"
        + "uint8_t rssi;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.RfFreq);
            writer.Write(StaticFields[1], Payload.LinkBw);
            writer.Write(StaticFields[2], Payload.LinkRate);
            writer.Write(StaticFields[3], Payload.Snr);
            writer.Write(StaticFields[4], Payload.CpuTemp);
            writer.Write(StaticFields[5], Payload.BoardTemp);
            writer.Write(StaticFields[6], Payload.Rssi);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.RfFreq = reader.ReadUInt(StaticFields[0]);
            Payload.LinkBw = reader.ReadUInt(StaticFields[1]);
            Payload.LinkRate = reader.ReadUInt(StaticFields[2]);
            Payload.Snr = reader.ReadShort(StaticFields[3]);
            Payload.CpuTemp = reader.ReadShort(StaticFields[4]);
            Payload.BoardTemp = reader.ReadShort(StaticFields[5]);
            Payload.Rssi = reader.ReadByte(StaticFields[6]);
        
            
        }
    }

    /// <summary>
    ///  HERELINK_TELEM
    /// </summary>
    public class HerelinkTelemPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 19; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 19; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t rf_freq
            +4 // uint32_t link_bw
            +4 // uint32_t link_rate
            +2 // int16_t snr
            +2 // int16_t cpu_temp
            +2 // int16_t board_temp
            +1 // uint8_t rssi
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RfFreq = BinSerialize.ReadUInt(ref buffer);
            LinkBw = BinSerialize.ReadUInt(ref buffer);
            LinkRate = BinSerialize.ReadUInt(ref buffer);
            Snr = BinSerialize.ReadShort(ref buffer);
            CpuTemp = BinSerialize.ReadShort(ref buffer);
            BoardTemp = BinSerialize.ReadShort(ref buffer);
            Rssi = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,RfFreq);
            BinSerialize.WriteUInt(ref buffer,LinkBw);
            BinSerialize.WriteUInt(ref buffer,LinkRate);
            BinSerialize.WriteShort(ref buffer,Snr);
            BinSerialize.WriteShort(ref buffer,CpuTemp);
            BinSerialize.WriteShort(ref buffer,BoardTemp);
            BinSerialize.WriteByte(ref buffer,(byte)Rssi);
            /* PayloadByteSize = 19 */;
        }
        
        



        /// <summary>
        /// 
        /// OriginName: rf_freq, Units: , IsExtended: false
        /// </summary>
        public uint RfFreq { get; set; }
        /// <summary>
        /// 
        /// OriginName: link_bw, Units: , IsExtended: false
        /// </summary>
        public uint LinkBw { get; set; }
        /// <summary>
        /// 
        /// OriginName: link_rate, Units: , IsExtended: false
        /// </summary>
        public uint LinkRate { get; set; }
        /// <summary>
        /// 
        /// OriginName: snr, Units: , IsExtended: false
        /// </summary>
        public short Snr { get; set; }
        /// <summary>
        /// 
        /// OriginName: cpu_temp, Units: , IsExtended: false
        /// </summary>
        public short CpuTemp { get; set; }
        /// <summary>
        /// 
        /// OriginName: board_temp, Units: , IsExtended: false
        /// </summary>
        public short BoardTemp { get; set; }
        /// <summary>
        /// 
        /// OriginName: rssi, Units: , IsExtended: false
        /// </summary>
        public byte Rssi { get; set; }
    }
    /// <summary>
    /// Start firmware update with encapsulated data.
    ///  CUBEPILOT_FIRMWARE_UPDATE_START
    /// </summary>
    public class CubepilotFirmwareUpdateStartPacket : MavlinkV2Message<CubepilotFirmwareUpdateStartPayload>
    {
        public const int MessageId = 50004;
        
        public const byte CrcExtra = 240;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override CubepilotFirmwareUpdateStartPayload Payload { get; } = new();

        public override string Name => "CUBEPILOT_FIRMWARE_UPDATE_START";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "size",
            "FW Size.",
            string.Empty, 
            @"bytes", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "crc",
            "FW CRC.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(2,
            "target_system",
            "System ID.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(3,
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
        public const string FormatMessage = "CUBEPILOT_FIRMWARE_UPDATE_START:"
        + "uint32_t size;"
        + "uint32_t crc;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Size);
            writer.Write(StaticFields[1], Payload.Crc);
            writer.Write(StaticFields[2], Payload.TargetSystem);
            writer.Write(StaticFields[3], Payload.TargetComponent);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Size = reader.ReadUInt(StaticFields[0]);
            Payload.Crc = reader.ReadUInt(StaticFields[1]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[2]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[3]);
        
            
        }
    }

    /// <summary>
    ///  CUBEPILOT_FIRMWARE_UPDATE_START
    /// </summary>
    public class CubepilotFirmwareUpdateStartPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 10; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 10; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t size
            +4 // uint32_t crc
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Size = BinSerialize.ReadUInt(ref buffer);
            Crc = BinSerialize.ReadUInt(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,Size);
            BinSerialize.WriteUInt(ref buffer,Crc);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 10 */;
        }
        
        



        /// <summary>
        /// FW Size.
        /// OriginName: size, Units: bytes, IsExtended: false
        /// </summary>
        public uint Size { get; set; }
        /// <summary>
        /// FW CRC.
        /// OriginName: crc, Units: , IsExtended: false
        /// </summary>
        public uint Crc { get; set; }
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
    /// offset response to encapsulated data.
    ///  CUBEPILOT_FIRMWARE_UPDATE_RESP
    /// </summary>
    public class CubepilotFirmwareUpdateRespPacket : MavlinkV2Message<CubepilotFirmwareUpdateRespPayload>
    {
        public const int MessageId = 50005;
        
        public const byte CrcExtra = 152;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override CubepilotFirmwareUpdateRespPayload Payload { get; } = new();

        public override string Name => "CUBEPILOT_FIRMWARE_UPDATE_RESP";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "offset",
            "FW Offset.",
            string.Empty, 
            @"bytes", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
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
        ];
        public const string FormatMessage = "CUBEPILOT_FIRMWARE_UPDATE_RESP:"
        + "uint32_t offset;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Offset);
            writer.Write(StaticFields[1], Payload.TargetSystem);
            writer.Write(StaticFields[2], Payload.TargetComponent);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Offset = reader.ReadUInt(StaticFields[0]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[1]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[2]);
        
            
        }
    }

    /// <summary>
    ///  CUBEPILOT_FIRMWARE_UPDATE_RESP
    /// </summary>
    public class CubepilotFirmwareUpdateRespPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 6; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t offset
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Offset = BinSerialize.ReadUInt(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,Offset);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 6 */;
        }
        
        



        /// <summary>
        /// FW Offset.
        /// OriginName: offset, Units: bytes, IsExtended: false
        /// </summary>
        public uint Offset { get; set; }
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


#endregion


}

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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.11+05423b76b208fe780abe1cef9f7beeacb19cba77 25-07-03.

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

        public void Accept(IVisitor visitor)
        {
            ArrayType.Accept(visitor,RcRawField, RcRawField.DataType, 32,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref RcRaw[index]));    

        }

        /// <summary>
        /// 
        /// OriginName: rc_raw, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RcRawField = new Field.Builder()
            .Name(nameof(RcRaw))
            .Title("rc_raw")
            .Description("")

            .DataType(new ArrayType(UInt8Type.Default,32))
        .Build();
        public const int RcRawMaxItemsCount = 32;
        public byte[] RcRaw { get; } = new byte[32];
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
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Uri)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Uri.Length);
                }
            }
            buffer = buffer[arraySize..];
           

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

        public void Accept(IVisitor visitor)
        {
            FloatType.Accept(visitor,FramerateField, FramerateField.DataType, ref _framerate);    
            UInt32Type.Accept(visitor,BitrateField, BitrateField.DataType, ref _bitrate);    
            UInt16Type.Accept(visitor,ResolutionHField, ResolutionHField.DataType, ref _resolutionH);    
            UInt16Type.Accept(visitor,ResolutionVField, ResolutionVField.DataType, ref _resolutionV);    
            UInt16Type.Accept(visitor,RotationField, RotationField.DataType, ref _rotation);    
            UInt8Type.Accept(visitor,CameraIdField, CameraIdField.DataType, ref _cameraId);    
            UInt8Type.Accept(visitor,StatusField, StatusField.DataType, ref _status);    
            ArrayType.Accept(visitor,UriField, UriField.DataType, 230, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref Uri[index]));

        }

        /// <summary>
        /// Frame rate.
        /// OriginName: framerate, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field FramerateField = new Field.Builder()
            .Name(nameof(Framerate))
            .Title("framerate")
            .Description("Frame rate.")
.Units(@"Hz")
            .DataType(FloatType.Default)
        .Build();
        private float _framerate;
        public float Framerate { get => _framerate; set => _framerate = value; }
        /// <summary>
        /// Bit rate.
        /// OriginName: bitrate, Units: bits/s, IsExtended: false
        /// </summary>
        public static readonly Field BitrateField = new Field.Builder()
            .Name(nameof(Bitrate))
            .Title("bitrate")
            .Description("Bit rate.")
.Units(@"bits/s")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _bitrate;
        public uint Bitrate { get => _bitrate; set => _bitrate = value; }
        /// <summary>
        /// Horizontal resolution.
        /// OriginName: resolution_h, Units: pix, IsExtended: false
        /// </summary>
        public static readonly Field ResolutionHField = new Field.Builder()
            .Name(nameof(ResolutionH))
            .Title("resolution_h")
            .Description("Horizontal resolution.")
.Units(@"pix")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _resolutionH;
        public ushort ResolutionH { get => _resolutionH; set => _resolutionH = value; }
        /// <summary>
        /// Vertical resolution.
        /// OriginName: resolution_v, Units: pix, IsExtended: false
        /// </summary>
        public static readonly Field ResolutionVField = new Field.Builder()
            .Name(nameof(ResolutionV))
            .Title("resolution_v")
            .Description("Vertical resolution.")
.Units(@"pix")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _resolutionV;
        public ushort ResolutionV { get => _resolutionV; set => _resolutionV = value; }
        /// <summary>
        /// Video image rotation clockwise.
        /// OriginName: rotation, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field RotationField = new Field.Builder()
            .Name(nameof(Rotation))
            .Title("rotation")
            .Description("Video image rotation clockwise.")
.Units(@"deg")
            .DataType(UInt16Type.Default)
        .Build();
        private ushort _rotation;
        public ushort Rotation { get => _rotation; set => _rotation = value; }
        /// <summary>
        /// Video Stream ID (1 for first, 2 for second, etc.)
        /// OriginName: camera_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CameraIdField = new Field.Builder()
            .Name(nameof(CameraId))
            .Title("camera_id")
            .Description("Video Stream ID (1 for first, 2 for second, etc.)")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _cameraId;
        public byte CameraId { get => _cameraId; set => _cameraId = value; }
        /// <summary>
        /// Number of streams available.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StatusField = new Field.Builder()
            .Name(nameof(Status))
            .Title("status")
            .Description("Number of streams available.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _status;
        public byte Status { get => _status; set => _status = value; }
        /// <summary>
        /// Video stream URI (TCP or RTSP URI ground station should connect to) or port number (UDP port ground station should listen to).
        /// OriginName: uri, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UriField = new Field.Builder()
            .Name(nameof(Uri))
            .Title("uri")
            .Description("Video stream URI (TCP or RTSP URI ground station should connect to) or port number (UDP port ground station should listen to).")

            .DataType(new ArrayType(CharType.Ascii,230))
        .Build();
        public const int UriMaxItemsCount = 230;
        public char[] Uri { get; } = new char[230];
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,RfFreqField, RfFreqField.DataType, ref _rfFreq);    
            UInt32Type.Accept(visitor,LinkBwField, LinkBwField.DataType, ref _linkBw);    
            UInt32Type.Accept(visitor,LinkRateField, LinkRateField.DataType, ref _linkRate);    
            Int16Type.Accept(visitor,SnrField, SnrField.DataType, ref _snr);
            Int16Type.Accept(visitor,CpuTempField, CpuTempField.DataType, ref _cpuTemp);
            Int16Type.Accept(visitor,BoardTempField, BoardTempField.DataType, ref _boardTemp);
            UInt8Type.Accept(visitor,RssiField, RssiField.DataType, ref _rssi);    

        }

        /// <summary>
        /// 
        /// OriginName: rf_freq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RfFreqField = new Field.Builder()
            .Name(nameof(RfFreq))
            .Title("rf_freq")
            .Description("")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _rfFreq;
        public uint RfFreq { get => _rfFreq; set => _rfFreq = value; }
        /// <summary>
        /// 
        /// OriginName: link_bw, Units: , IsExtended: false
        /// </summary>
        public static readonly Field LinkBwField = new Field.Builder()
            .Name(nameof(LinkBw))
            .Title("link_bw")
            .Description("")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _linkBw;
        public uint LinkBw { get => _linkBw; set => _linkBw = value; }
        /// <summary>
        /// 
        /// OriginName: link_rate, Units: , IsExtended: false
        /// </summary>
        public static readonly Field LinkRateField = new Field.Builder()
            .Name(nameof(LinkRate))
            .Title("link_rate")
            .Description("")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _linkRate;
        public uint LinkRate { get => _linkRate; set => _linkRate = value; }
        /// <summary>
        /// 
        /// OriginName: snr, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SnrField = new Field.Builder()
            .Name(nameof(Snr))
            .Title("snr")
            .Description("")

            .DataType(Int16Type.Default)
        .Build();
        private short _snr;
        public short Snr { get => _snr; set => _snr = value; }
        /// <summary>
        /// 
        /// OriginName: cpu_temp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CpuTempField = new Field.Builder()
            .Name(nameof(CpuTemp))
            .Title("cpu_temp")
            .Description("")

            .DataType(Int16Type.Default)
        .Build();
        private short _cpuTemp;
        public short CpuTemp { get => _cpuTemp; set => _cpuTemp = value; }
        /// <summary>
        /// 
        /// OriginName: board_temp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BoardTempField = new Field.Builder()
            .Name(nameof(BoardTemp))
            .Title("board_temp")
            .Description("")

            .DataType(Int16Type.Default)
        .Build();
        private short _boardTemp;
        public short BoardTemp { get => _boardTemp; set => _boardTemp = value; }
        /// <summary>
        /// 
        /// OriginName: rssi, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RssiField = new Field.Builder()
            .Name(nameof(Rssi))
            .Title("rssi")
            .Description("")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _rssi;
        public byte Rssi { get => _rssi; set => _rssi = value; }
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,SizeField, SizeField.DataType, ref _size);    
            UInt32Type.Accept(visitor,CrcField, CrcField.DataType, ref _crc);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    

        }

        /// <summary>
        /// FW Size.
        /// OriginName: size, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field SizeField = new Field.Builder()
            .Name(nameof(Size))
            .Title("size")
            .Description("FW Size.")
.Units(@"bytes")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _size;
        public uint Size { get => _size; set => _size = value; }
        /// <summary>
        /// FW CRC.
        /// OriginName: crc, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CrcField = new Field.Builder()
            .Name(nameof(Crc))
            .Title("crc")
            .Description("FW CRC.")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _crc;
        public uint Crc { get => _crc; set => _crc = value; }
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,OffsetField, OffsetField.DataType, ref _offset);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    

        }

        /// <summary>
        /// FW Offset.
        /// OriginName: offset, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field OffsetField = new Field.Builder()
            .Name(nameof(Offset))
            .Title("offset")
            .Description("FW Offset.")
.Units(@"bytes")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _offset;
        public uint Offset { get => _offset; set => _offset = value; }
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




        


#endregion


}

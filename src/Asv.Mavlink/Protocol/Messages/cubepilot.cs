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

        public void Visit(IVisitor visitor)
        {
            ArrayType.Accept(visitor,RcRawField, 32,
                (index,v) => UInt8Type.Accept(v, RcRawField, ref RcRaw[index]));    

        }

        /// <summary>
        /// 
        /// OriginName: rc_raw, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RcRawField = new Field.Builder()
            .Name(nameof(RcRaw))
            .Title("rc_raw")
            .Description("")
            .FormatString(string.Empty)
            .Units(string.Empty)
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

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,FramerateField, ref _Framerate);    
            UInt32Type.Accept(visitor,BitrateField, ref _Bitrate);    
            UInt16Type.Accept(visitor,ResolutionHField, ref _ResolutionH);    
            UInt16Type.Accept(visitor,ResolutionVField, ref _ResolutionV);    
            UInt16Type.Accept(visitor,RotationField, ref _Rotation);    
            UInt8Type.Accept(visitor,CameraIdField, ref _CameraId);    
            UInt8Type.Accept(visitor,StatusField, ref _Status);    
            ArrayType.Accept(visitor,UriField, 230, (index,v) =>
            {
                var tmp = (byte)Uri[index];
                UInt8Type.Accept(v,UriField, ref tmp);
                Uri[index] = (char)tmp;
            });

        }

        /// <summary>
        /// Frame rate.
        /// OriginName: framerate, Units: Hz, IsExtended: false
        /// </summary>
        public static readonly Field FramerateField = new Field.Builder()
            .Name(nameof(Framerate))
            .Title("framerate")
            .Description("Frame rate.")
            .FormatString(string.Empty)
            .Units(@"Hz")
            .DataType(FloatType.Default)

            .Build();
        private float _Framerate;
        public float Framerate { get => _Framerate; set { _Framerate = value; } }
        /// <summary>
        /// Bit rate.
        /// OriginName: bitrate, Units: bits/s, IsExtended: false
        /// </summary>
        public static readonly Field BitrateField = new Field.Builder()
            .Name(nameof(Bitrate))
            .Title("bitrate")
            .Description("Bit rate.")
            .FormatString(string.Empty)
            .Units(@"bits/s")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Bitrate;
        public uint Bitrate { get => _Bitrate; set { _Bitrate = value; } }
        /// <summary>
        /// Horizontal resolution.
        /// OriginName: resolution_h, Units: pix, IsExtended: false
        /// </summary>
        public static readonly Field ResolutionHField = new Field.Builder()
            .Name(nameof(ResolutionH))
            .Title("resolution_h")
            .Description("Horizontal resolution.")
            .FormatString(string.Empty)
            .Units(@"pix")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ResolutionH;
        public ushort ResolutionH { get => _ResolutionH; set { _ResolutionH = value; } }
        /// <summary>
        /// Vertical resolution.
        /// OriginName: resolution_v, Units: pix, IsExtended: false
        /// </summary>
        public static readonly Field ResolutionVField = new Field.Builder()
            .Name(nameof(ResolutionV))
            .Title("resolution_v")
            .Description("Vertical resolution.")
            .FormatString(string.Empty)
            .Units(@"pix")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ResolutionV;
        public ushort ResolutionV { get => _ResolutionV; set { _ResolutionV = value; } }
        /// <summary>
        /// Video image rotation clockwise.
        /// OriginName: rotation, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field RotationField = new Field.Builder()
            .Name(nameof(Rotation))
            .Title("rotation")
            .Description("Video image rotation clockwise.")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Rotation;
        public ushort Rotation { get => _Rotation; set { _Rotation = value; } }
        /// <summary>
        /// Video Stream ID (1 for first, 2 for second, etc.)
        /// OriginName: camera_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CameraIdField = new Field.Builder()
            .Name(nameof(CameraId))
            .Title("camera_id")
            .Description("Video Stream ID (1 for first, 2 for second, etc.)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _CameraId;
        public byte CameraId { get => _CameraId; set { _CameraId = value; } }
        /// <summary>
        /// Number of streams available.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StatusField = new Field.Builder()
            .Name(nameof(Status))
            .Title("status")
            .Description("Number of streams available.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Status;
        public byte Status { get => _Status; set { _Status = value; } }
        /// <summary>
        /// Video stream URI (TCP or RTSP URI ground station should connect to) or port number (UDP port ground station should listen to).
        /// OriginName: uri, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UriField = new Field.Builder()
            .Name(nameof(Uri))
            .Title("uri")
            .Description("Video stream URI (TCP or RTSP URI ground station should connect to) or port number (UDP port ground station should listen to).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,230))

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

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,RfFreqField, ref _RfFreq);    
            UInt32Type.Accept(visitor,LinkBwField, ref _LinkBw);    
            UInt32Type.Accept(visitor,LinkRateField, ref _LinkRate);    
            Int16Type.Accept(visitor,SnrField, ref _Snr);
            Int16Type.Accept(visitor,CpuTempField, ref _CpuTemp);
            Int16Type.Accept(visitor,BoardTempField, ref _BoardTemp);
            UInt8Type.Accept(visitor,RssiField, ref _Rssi);    

        }

        /// <summary>
        /// 
        /// OriginName: rf_freq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RfFreqField = new Field.Builder()
            .Name(nameof(RfFreq))
            .Title("rf_freq")
            .Description("")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _RfFreq;
        public uint RfFreq { get => _RfFreq; set { _RfFreq = value; } }
        /// <summary>
        /// 
        /// OriginName: link_bw, Units: , IsExtended: false
        /// </summary>
        public static readonly Field LinkBwField = new Field.Builder()
            .Name(nameof(LinkBw))
            .Title("link_bw")
            .Description("")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _LinkBw;
        public uint LinkBw { get => _LinkBw; set { _LinkBw = value; } }
        /// <summary>
        /// 
        /// OriginName: link_rate, Units: , IsExtended: false
        /// </summary>
        public static readonly Field LinkRateField = new Field.Builder()
            .Name(nameof(LinkRate))
            .Title("link_rate")
            .Description("")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _LinkRate;
        public uint LinkRate { get => _LinkRate; set { _LinkRate = value; } }
        /// <summary>
        /// 
        /// OriginName: snr, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SnrField = new Field.Builder()
            .Name(nameof(Snr))
            .Title("snr")
            .Description("")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _Snr;
        public short Snr { get => _Snr; set { _Snr = value; } }
        /// <summary>
        /// 
        /// OriginName: cpu_temp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CpuTempField = new Field.Builder()
            .Name(nameof(CpuTemp))
            .Title("cpu_temp")
            .Description("")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _CpuTemp;
        public short CpuTemp { get => _CpuTemp; set { _CpuTemp = value; } }
        /// <summary>
        /// 
        /// OriginName: board_temp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BoardTempField = new Field.Builder()
            .Name(nameof(BoardTemp))
            .Title("board_temp")
            .Description("")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _BoardTemp;
        public short BoardTemp { get => _BoardTemp; set { _BoardTemp = value; } }
        /// <summary>
        /// 
        /// OriginName: rssi, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RssiField = new Field.Builder()
            .Name(nameof(Rssi))
            .Title("rssi")
            .Description("")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Rssi;
        public byte Rssi { get => _Rssi; set { _Rssi = value; } }
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

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,SizeField, ref _Size);    
            UInt32Type.Accept(visitor,CrcField, ref _Crc);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// FW Size.
        /// OriginName: size, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field SizeField = new Field.Builder()
            .Name(nameof(Size))
            .Title("size")
            .Description("FW Size.")
            .FormatString(string.Empty)
            .Units(@"bytes")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Size;
        public uint Size { get => _Size; set { _Size = value; } }
        /// <summary>
        /// FW CRC.
        /// OriginName: crc, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CrcField = new Field.Builder()
            .Name(nameof(Crc))
            .Title("crc")
            .Description("FW CRC.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Crc;
        public uint Crc { get => _Crc; set { _Crc = value; } }
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

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,OffsetField, ref _Offset);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// FW Offset.
        /// OriginName: offset, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field OffsetField = new Field.Builder()
            .Name(nameof(Offset))
            .Title("offset")
            .Description("FW Offset.")
            .FormatString(string.Empty)
            .Units(@"bytes")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Offset;
        public uint Offset { get => _Offset; set { _Offset = value; } }
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




        


#endregion


}

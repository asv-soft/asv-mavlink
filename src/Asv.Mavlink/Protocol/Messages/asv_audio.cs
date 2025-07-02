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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.8+aedf0e45cecf4e3648d310da2728457ab10b401a 25-07-02.

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

namespace Asv.Mavlink.AsvAudio
{

    public static class AsvAudioHelper
    {
        public static void RegisterAsvAudioDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(AsvAudioOnlinePacket.MessageId, ()=>new AsvAudioOnlinePacket());
            src.Add(AsvAudioStreamPacket.MessageId, ()=>new AsvAudioStreamPacket());
        }
 
    }

#region Enums

    /// <summary>
    /// Device capabilities flags (uint8_t).[!THIS_IS_ENUM_FLAG!]
    ///  ASV_AUDIO_MODE_FLAG
    /// </summary>
    [Flags]
    public enum AsvAudioModeFlag : ulong
    {
        /// <summary>
        /// The device can play input audio stream (e.g., speaker is on).
        /// ASV_AUDIO_MODE_FLAG_SPEAKER_ON
        /// </summary>
        AsvAudioModeFlagSpeakerOn = 1,
        /// <summary>
        /// The device generate audio stream (e.g., microphone is on).
        /// ASV_AUDIO_MODE_FLAG_MIC_ON
        /// </summary>
        AsvAudioModeFlagMicOn = 2,
    }
    public static class AsvAudioModeFlagHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_AUDIO_MODE_FLAG_SPEAKER_ON");
            yield return new EnumValue<T>(converter(2),"ASV_AUDIO_MODE_FLAG_MIC_ON");
        }
    }
    /// <summary>
    /// Audio codec and audio format (uint8_t).
    ///  ASV_AUDIO_CODEC
    /// </summary>
    public enum AsvAudioCodec : ulong
    {
        /// <summary>
        /// Unknown codec[!METADATA!]
        /// ASV_AUDIO_CODEC_UNKNOWN
        /// </summary>
        [Description("Unknown codec")]
        AsvAudioCodecUnknown = 0,
        /// <summary>
        /// Reserverd[!METADATA!]
        /// ASV_AUDIO_CODEC_RESERVED_255
        /// </summary>
        [Description("Reserverd")]
        AsvAudioCodecReserved255 = 255,
        /// <summary>
        /// PCM 8k MONO[!METADATA!]
        /// ASV_AUDIO_CODEC_RAW_8000_MONO
        /// </summary>
        [Description("PCM 8k MONO")]
        AsvAudioCodecRaw8000Mono = 256,
        /// <summary>
        /// OPUS 8k MONO[!METADATA!]
        /// ASV_AUDIO_CODEC_OPUS_8000_MONO
        /// </summary>
        [Description("OPUS 8k MONO")]
        AsvAudioCodecOpus8000Mono = 512,
        /// <summary>
        /// Advanced Audio Coding (AAC).
        /// ASV_AUDIO_CODEC_AAC
        /// </summary>
        AsvAudioCodecAac = 768,
        /// <summary>
        /// G.711 μ-law (PCMU).
        /// ASV_AUDIO_CODEC_PCMU
        /// </summary>
        AsvAudioCodecPcmu = 1024,
        /// <summary>
        /// G.711 A-law (PCMA).
        /// ASV_AUDIO_CODEC_PCMA
        /// </summary>
        AsvAudioCodecPcma = 1280,
        /// <summary>
        /// Speex.
        /// ASV_AUDIO_CODEC_SPEEX
        /// </summary>
        AsvAudioCodecSpeex = 1536,
        /// <summary>
        /// Internet Low Bitrate Codec (iLBC).
        /// ASV_AUDIO_CODEC_ILBC
        /// </summary>
        AsvAudioCodecIlbc = 1792,
        /// <summary>
        /// G.722.
        /// ASV_AUDIO_CODEC_G722
        /// </summary>
        AsvAudioCodecG722 = 2048,
        /// <summary>
        /// Linear Pulse Code Modulation (L16).
        /// ASV_AUDIO_CODEC_L16
        /// </summary>
        AsvAudioCodecL16 = 2304,
        /// <summary>
        /// Reserved
        /// ASV_AUDIO_CODEC_RESERVED
        /// </summary>
        AsvAudioCodecReserved = 65535,
    }
    public static class AsvAudioCodecHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(255);
            yield return converter(256);
            yield return converter(512);
            yield return converter(768);
            yield return converter(1024);
            yield return converter(1280);
            yield return converter(1536);
            yield return converter(1792);
            yield return converter(2048);
            yield return converter(2304);
            yield return converter(65535);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_AUDIO_CODEC_UNKNOWN");
            yield return new EnumValue<T>(converter(255),"ASV_AUDIO_CODEC_RESERVED_255");
            yield return new EnumValue<T>(converter(256),"ASV_AUDIO_CODEC_RAW_8000_MONO");
            yield return new EnumValue<T>(converter(512),"ASV_AUDIO_CODEC_OPUS_8000_MONO");
            yield return new EnumValue<T>(converter(768),"ASV_AUDIO_CODEC_AAC");
            yield return new EnumValue<T>(converter(1024),"ASV_AUDIO_CODEC_PCMU");
            yield return new EnumValue<T>(converter(1280),"ASV_AUDIO_CODEC_PCMA");
            yield return new EnumValue<T>(converter(1536),"ASV_AUDIO_CODEC_SPEEX");
            yield return new EnumValue<T>(converter(1792),"ASV_AUDIO_CODEC_ILBC");
            yield return new EnumValue<T>(converter(2048),"ASV_AUDIO_CODEC_G722");
            yield return new EnumValue<T>(converter(2304),"ASV_AUDIO_CODEC_L16");
            yield return new EnumValue<T>(converter(65535),"ASV_AUDIO_CODEC_RESERVED");
        }
    }

#endregion

#region Messages

    /// <summary>
    /// Every device that wants to be visible at voice chat and can talk to the others sends this packet at 1 Hz.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_AUDIO_ONLINE
    /// </summary>
    public class AsvAudioOnlinePacket : MavlinkV2Message<AsvAudioOnlinePayload>
    {
        public const int MessageId = 13200;
        
        public const byte CrcExtra = 142;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvAudioOnlinePayload Payload { get; } = new();

        public override string Name => "ASV_AUDIO_ONLINE";
    }

    /// <summary>
    ///  ASV_AUDIO_ONLINE
    /// </summary>
    public class AsvAudioOnlinePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 19; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 19; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 2 // uint16_t codec
            + 1 // uint8_t mode
            +Name.Length // char[16] name
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Codec = (AsvAudioCodec)BinSerialize.ReadUShort(ref buffer);
            Mode = (AsvAudioModeFlag)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/19 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Name)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Name.Length);
                }
            }
            buffer = buffer[arraySize..];
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,(ushort)Codec);
            BinSerialize.WriteByte(ref buffer,(byte)Mode);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Name)
                {
                    Encoding.ASCII.GetBytes(charPointer, Name.Length, bytePointer, Name.Length);
                }
            }
            buffer = buffer.Slice(Name.Length);
            
            /* PayloadByteSize = 19 */;
        }

        public void Accept(IVisitor visitor)
        {
            var tmpCodec = (ushort)Codec;
            UInt16Type.Accept(visitor,CodecField, CodecField.DataType, ref tmpCodec);
            Codec = (AsvAudioCodec)tmpCodec;
            var tmpMode = (byte)Mode;
            UInt8Type.Accept(visitor,ModeField, ModeField.DataType, ref tmpMode);
            Mode = (AsvAudioModeFlag)tmpMode;
            ArrayType.Accept(visitor,NameField, NameField.DataType, 16, 
                (index, v, f, t) => CharType.Accept(v, f, t, ref Name[index]));

        }

        /// <summary>
        /// Audio codec used by this device.
        /// OriginName: codec, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CodecField = new Field.Builder()
            .Name(nameof(Codec))
            .Title("codec")
            .Description("Audio codec used by this device.")
            .DataType(new UInt16Type(AsvAudioCodecHelper.GetValues(x=>(ushort)x).Min(),AsvAudioCodecHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(AsvAudioCodecHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private AsvAudioCodec _codec;
        public AsvAudioCodec Codec { get => _codec; set => _codec = value; } 
        /// <summary>
        /// Device current work mode.
        /// OriginName: mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ModeField = new Field.Builder()
            .Name(nameof(Mode))
            .Title("mode")
            .Description("Device current work mode.")
            .DataType(new UInt8Type(AsvAudioModeFlagHelper.GetValues(x=>(byte)x).Min(),AsvAudioModeFlagHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvAudioModeFlagHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvAudioModeFlag _mode;
        public AsvAudioModeFlag Mode { get => _mode; set => _mode = value; } 
        /// <summary>
        /// Audio device name in voice chat.
        /// OriginName: name, Units: , IsExtended: false
        /// </summary>
        public static readonly Field NameField = new Field.Builder()
            .Name(nameof(Name))
            .Title("name")
            .Description("Audio device name in voice chat.")

            .DataType(new ArrayType(CharType.Ascii,16))
        .Build();
        public const int NameMaxItemsCount = 16;
        public char[] Name { get; } = new char[16];
        [Obsolete("This method is deprecated. Use GetNameMaxItemsCount instead.")]
        public byte GetNameMaxItemsCount() => 16;
    }
    /// <summary>
    /// Message containing encoded audio data. If, after audio-encoding, one frame exceeds one packet size, multiple packets are used for frame transmitting.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_AUDIO_STREAM
    /// </summary>
    public class AsvAudioStreamPacket : MavlinkV2Message<AsvAudioStreamPayload>
    {
        public const int MessageId = 13201;
        
        public const byte CrcExtra = 152;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvAudioStreamPayload Payload { get; } = new();

        public override string Name => "ASV_AUDIO_STREAM";
    }

    /// <summary>
    ///  ASV_AUDIO_STREAM
    /// </summary>
    public class AsvAudioStreamPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 236; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 236; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t frame_seq
            +1 // uint8_t pkt_in_frame
            +1 // uint8_t pkt_seq
            +1 // uint8_t data_size
            +Data.Length // uint8_t[230] data
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            FrameSeq = (byte)BinSerialize.ReadByte(ref buffer);
            PktInFrame = (byte)BinSerialize.ReadByte(ref buffer);
            PktSeq = (byte)BinSerialize.ReadByte(ref buffer);
            DataSize = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/230 - Math.Max(0,((/*PayloadByteSize*/236 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)FrameSeq);
            BinSerialize.WriteByte(ref buffer,(byte)PktInFrame);
            BinSerialize.WriteByte(ref buffer,(byte)PktSeq);
            BinSerialize.WriteByte(ref buffer,(byte)DataSize);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            /* PayloadByteSize = 236 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    
            UInt8Type.Accept(visitor,FrameSeqField, FrameSeqField.DataType, ref _frameSeq);    
            UInt8Type.Accept(visitor,PktInFrameField, PktInFrameField.DataType, ref _pktInFrame);    
            UInt8Type.Accept(visitor,PktSeqField, PktSeqField.DataType, ref _pktSeq);    
            UInt8Type.Accept(visitor,DataSizeField, DataSizeField.DataType, ref _dataSize);    
            ArrayType.Accept(visitor,DataField, DataField.DataType, 230,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref Data[index]));    

        }

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
        /// <summary>
        /// Frame sequence number.
        /// OriginName: frame_seq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FrameSeqField = new Field.Builder()
            .Name(nameof(FrameSeq))
            .Title("frame_seq")
            .Description("Frame sequence number.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _frameSeq;
        public byte FrameSeq { get => _frameSeq; set => _frameSeq = value; }
        /// <summary>
        /// Number of packets for one encoded audio frame.
        /// OriginName: pkt_in_frame, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PktInFrameField = new Field.Builder()
            .Name(nameof(PktInFrame))
            .Title("pkt_in_frame")
            .Description("Number of packets for one encoded audio frame.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _pktInFrame;
        public byte PktInFrame { get => _pktInFrame; set => _pktInFrame = value; }
        /// <summary>
        /// Packet sequence number (starting with 0 on every encoded frame).
        /// OriginName: pkt_seq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PktSeqField = new Field.Builder()
            .Name(nameof(PktSeq))
            .Title("pkt_seq")
            .Description("Packet sequence number (starting with 0 on every encoded frame).")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _pktSeq;
        public byte PktSeq { get => _pktSeq; set => _pktSeq = value; }
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
        /// Audio data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Audio data.")

            .DataType(new ArrayType(UInt8Type.Default,230))
        .Build();
        public const int DataMaxItemsCount = 230;
        public byte[] Data { get; } = new byte[230];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 230;
    }




        


#endregion


}

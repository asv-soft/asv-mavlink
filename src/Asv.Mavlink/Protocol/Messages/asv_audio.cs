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

// This code was generate by tool Asv.Mavlink.Shell version 3.7.1+c33f656317a21f32a7734da0bfc10db26a371663

using System;
using System.Text;
using System.ComponentModel;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Asv.IO;

namespace Asv.Mavlink.V2.AsvAudio
{

    public static class AsvAudioHelper
    {
        public static void RegisterAsvAudioDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AsvAudioOnlinePacket());
            src.Register(()=>new AsvAudioStreamPacket());
        }
    }

#region Enums

    /// <summary>
    /// Device capabilities flags (uint8_t).[!THIS_IS_ENUM_FLAG!]
    ///  ASV_AUDIO_MODE_FLAG
    /// </summary>
    [Flags]
    public enum AsvAudioModeFlag:uint
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

    /// <summary>
    /// Audio codec and audio format (uint8_t).
    ///  ASV_AUDIO_CODEC
    /// </summary>
    public enum AsvAudioCodec:uint
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


#endregion

#region Messages

    /// <summary>
    /// Every device that wants to be visible at voice chat and can talk to the others sends this packet at 1 Hz.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_AUDIO_ONLINE
    /// </summary>
    public class AsvAudioOnlinePacket: PacketV2<AsvAudioOnlinePayload>
    {
	    public const int PacketMessageId = 13200;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 142;
        public override bool WrapToV2Extension => true;

        public override AsvAudioOnlinePayload Payload { get; } = new AsvAudioOnlinePayload();

        public override string Name => "ASV_AUDIO_ONLINE";
    }

    /// <summary>
    ///  ASV_AUDIO_ONLINE
    /// </summary>
    public class AsvAudioOnlinePayload : IPayload
    {
        public byte GetMaxByteSize() => 19; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 19; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 2; // Codec
            sum+= 1; // Mode
            sum+=Name.Length; //Name
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Codec = (AsvAudioCodec)BinSerialize.ReadUShort(ref buffer);
            Mode = (AsvAudioModeFlag)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/19 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
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
        
        



        /// <summary>
        /// Audio codec used by this device.
        /// OriginName: codec, Units: , IsExtended: false
        /// </summary>
        public AsvAudioCodec Codec { get; set; }
        /// <summary>
        /// Device current work mode.
        /// OriginName: mode, Units: , IsExtended: false
        /// </summary>
        public AsvAudioModeFlag Mode { get; set; }
        /// <summary>
        /// Audio device name in voice chat.
        /// OriginName: name, Units: , IsExtended: false
        /// </summary>
        public const int NameMaxItemsCount = 16;
        public char[] Name { get; set; } = new char[16];
        [Obsolete("This method is deprecated. Use GetNameMaxItemsCount instead.")]
        public byte GetNameMaxItemsCount() => 16;
    }
    /// <summary>
    /// Message containing encoded audio data. If, after audio-encoding, one frame exceeds one packet size, multiple packets are used for frame transmitting.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_AUDIO_STREAM
    /// </summary>
    public class AsvAudioStreamPacket: PacketV2<AsvAudioStreamPayload>
    {
	    public const int PacketMessageId = 13201;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 152;
        public override bool WrapToV2Extension => true;

        public override AsvAudioStreamPayload Payload { get; } = new AsvAudioStreamPayload();

        public override string Name => "ASV_AUDIO_STREAM";
    }

    /// <summary>
    ///  ASV_AUDIO_STREAM
    /// </summary>
    public class AsvAudioStreamPayload : IPayload
    {
        public byte GetMaxByteSize() => 236; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 236; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //FrameSeq
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
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            FrameSeq = (byte)BinSerialize.ReadByte(ref buffer);
            PktInFrame = (byte)BinSerialize.ReadByte(ref buffer);
            PktSeq = (byte)BinSerialize.ReadByte(ref buffer);
            DataSize = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/230 - Math.Max(0,((/*PayloadByteSize*/236 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
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
        /// Frame sequence number.
        /// OriginName: frame_seq, Units: , IsExtended: false
        /// </summary>
        public byte FrameSeq { get; set; }
        /// <summary>
        /// Number of packets for one encoded audio frame.
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
        /// Audio data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public const int DataMaxItemsCount = 230;
        public byte[] Data { get; set; } = new byte[230];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 230;
    }


#endregion


}

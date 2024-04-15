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
    ///  ASV_AUDIO_SAMPLE_RATE
    /// </summary>
    public enum AsvAudioSampleRate:uint
    {
        /// <summary>
        /// Unknown\unset sample rate
        /// ASV_AUDIO_SAMPLE_RATE_UNKNOWN
        /// </summary>
        AsvAudioSampleRateUnknown = 0,
        /// <summary>
        /// Adequate for human speech but without sibilance. Used in telephone/walkie-talkie.
        /// ASV_AUDIO_SAMPLE_RATE_8_000_HZ
        /// </summary>
        AsvAudioSampleRate8000Hz = 1,
        /// <summary>
        /// Used for lower-quality PCM, MPEG audio and for audio analysis of subwoofer bandpasses.
        /// ASV_AUDIO_SAMPLE_RATE_11_025_HZ
        /// </summary>
        AsvAudioSampleRate11025Hz = 2,
        /// <summary>
        /// 12 000 kHz.
        /// ASV_AUDIO_SAMPLE_RATE_12_000_HZ
        /// </summary>
        AsvAudioSampleRate12000Hz = 3,
        /// <summary>
        /// Used in most VoIP and VVoIP, extension of telephone narrowband.
        /// ASV_AUDIO_SAMPLE_RATE_16_000_HZ
        /// </summary>
        AsvAudioSampleRate16000Hz = 4,
        /// <summary>
        /// Used for lower-quality PCM and MPEG audio and for audio analysis of low frequency energy.
        /// ASV_AUDIO_SAMPLE_RATE_22_050_HZ
        /// </summary>
        AsvAudioSampleRate22050Hz = 5,
        /// <summary>
        /// Used for lower-quality PCM and MPEG audio and for audio analysis of low frequency energy.
        /// ASV_AUDIO_SAMPLE_RATE_24_000_HZ
        /// </summary>
        AsvAudioSampleRate24000Hz = 6,
        /// <summary>
        /// Audio CD, most commonly used rate with MPEG-1 audio (VCD, SVCD, MP3). Covers the 20 kHz bandwidth.
        /// ASV_AUDIO_SAMPLE_RATE_44_100_HZ
        /// </summary>
        AsvAudioSampleRate44100Hz = 7,
        /// <summary>
        /// Standard sampling rate used by professional digital video equipment, could reconstruct frequencies up to 22 kHz.
        /// ASV_AUDIO_SAMPLE_RATE_48_000_HZ
        /// </summary>
        AsvAudioSampleRate48000Hz = 8,
        /// <summary>
        /// Used by some professional recording equipment when the destination is CD, such as mixers, EQs, compressors, reverb, crossovers and recording devices.
        /// ASV_AUDIO_SAMPLE_RATE_88_200_HZ
        /// </summary>
        AsvAudioSampleRate88200Hz = 9,
        /// <summary>
        /// DVD-Audio, LPCM DVD tracks, Blu-ray audio tracks, HD DVD audio tracks.
        /// ASV_AUDIO_SAMPLE_RATE_96_000_HZ
        /// </summary>
        AsvAudioSampleRate96000Hz = 10,
        /// <summary>
        /// Used in HDCD recorders and other professional applications for CD production.
        /// ASV_AUDIO_SAMPLE_RATE_176_400_HZ
        /// </summary>
        AsvAudioSampleRate176400Hz = 11,
        /// <summary>
        /// Used with audio on professional video equipment. DVD-Audio, LPCM DVD tracks, Blu-ray audio tracks, HD DVD audio tracks.
        /// ASV_AUDIO_SAMPLE_RATE_192_000_HZ
        /// </summary>
        AsvAudioSampleRate192000Hz = 12,
        /// <summary>
        /// Digital eXtreme Definition. Used for recording and editing Super Audio CDs.
        /// ASV_AUDIO_SAMPLE_RATE_328_800_HZ
        /// </summary>
        AsvAudioSampleRate328800Hz = 13,
        /// <summary>
        /// Highest sample rate available for common software. Allows for precise peak detection.
        /// ASV_AUDIO_SAMPLE_RATE_384_000_HZ
        /// </summary>
        AsvAudioSampleRate384000Hz = 14,
        /// <summary>
        /// Last element in enum.
        /// ASV_AUDIO_SAMPLE_RATE_RESERVED
        /// </summary>
        AsvAudioSampleRateReserved = 255,
    }

    /// <summary>
    ///  ASV_AUDIO_CHANNEL
    /// </summary>
    public enum AsvAudioChannel:uint
    {
        /// <summary>
        /// Mono, 1 channel.
        /// ASV_AUDIO_CHANNEL_MONO
        /// </summary>
        AsvAudioChannelMono = 1,
        /// <summary>
        /// Stereo, 2 channel.
        /// ASV_AUDIO_CHANNEL_STEREO
        /// </summary>
        AsvAudioChannelStereo = 2,
    }

    /// <summary>
    /// Count of bit per sample for decoded.
    ///  ASV_AUDIO_PCM_FORMAT
    /// </summary>
    public enum AsvAudioPcmFormat:uint
    {
        /// <summary>
        /// 8 bit per sample.
        /// ASV_AUDIO_PCM_FORMAT_INT8
        /// </summary>
        AsvAudioPcmFormatInt8 = 1,
        /// <summary>
        /// 16 bit per sample.
        /// ASV_AUDIO_PCM_FORMAT_INT16
        /// </summary>
        AsvAudioPcmFormatInt16 = 2,
    }

    /// <summary>
    /// Audio codec (uint8_t).
    ///  ASV_AUDIO_CODEC
    /// </summary>
    public enum AsvAudioCodec:uint
    {
        /// <summary>
        /// Unknown codec.
        /// ASV_AUDIO_CODEC_UNKNOWN
        /// </summary>
        AsvAudioCodecUnknown = 0,
        /// <summary>
        /// Raw uncompressed audio (PCM).
        /// ASV_AUDIO_CODEC_RAW
        /// </summary>
        AsvAudioCodecRaw = 1,
        /// <summary>
        /// Opus (RFC 6716) – based on SILK vocoder and CELT codec.
        /// ASV_AUDIO_CODEC_OPUS
        /// </summary>
        AsvAudioCodecOpus = 2,
        /// <summary>
        /// Advanced Audio Coding (AAC).
        /// ASV_AUDIO_CODEC_AAC
        /// </summary>
        AsvAudioCodecAac = 3,
        /// <summary>
        /// G.711 μ-law (PCMU).
        /// ASV_AUDIO_CODEC_PCMU
        /// </summary>
        AsvAudioCodecPcmu = 4,
        /// <summary>
        /// G.711 A-law (PCMA).
        /// ASV_AUDIO_CODEC_PCMA
        /// </summary>
        AsvAudioCodecPcma = 5,
        /// <summary>
        /// Speex.
        /// ASV_AUDIO_CODEC_SPEEX
        /// </summary>
        AsvAudioCodecSpeex = 6,
        /// <summary>
        /// Internet Low Bitrate Codec (iLBC).
        /// ASV_AUDIO_CODEC_ILBC
        /// </summary>
        AsvAudioCodecIlbc = 7,
        /// <summary>
        /// G.722.
        /// ASV_AUDIO_CODEC_G722
        /// </summary>
        AsvAudioCodecG722 = 8,
        /// <summary>
        /// Linear Pulse Code Modulation (L16).
        /// ASV_AUDIO_CODEC_L16
        /// </summary>
        AsvAudioCodecL16 = 9,
        /// <summary>
        /// Reserved
        /// ASV_AUDIO_CODEC_RESERVED
        /// </summary>
        AsvAudioCodecReserved = 255,
    }

    /// <summary>
    /// Additional params for ASV_AUDIO_CODEC_RAW codec (uint8_t).
    ///  ASV_AUDIO_CODEC_RAW_CFG
    /// </summary>
    public enum AsvAudioCodecRawCfg:uint
    {
        /// <summary>
        /// Raw uncompressed audio (PCM) with 8 000 Hz smaple rate and 1 channel (mono).
        /// ASV_AUDIO_CODEC_RAW_CFG_8000_MONO
        /// </summary>
        AsvAudioCodecRawCfg8000Mono = 0,
    }

    /// <summary>
    /// Params for ASV_AUDIO_CODEC_OPUS codec (uint8_t).
    ///  ASV_AUDIO_CODEC_OPUS_CFG
    /// </summary>
    public enum AsvAudioCodecOpusCfg:uint
    {
        /// <summary>
        /// Raw uncompressed audio (PCM).
        /// ASV_AUDIO_CODEC_OPUS_CFG_8000_MONO
        /// </summary>
        AsvAudioCodecOpusCfg8000Mono = 0,
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
        public override byte GetCrcEtra() => 245;
        public override bool WrapToV2Extension => true;

        public override AsvAudioOnlinePayload Payload { get; } = new AsvAudioOnlinePayload();

        public override string Name => "ASV_AUDIO_ONLINE";
    }

    /// <summary>
    ///  ASV_AUDIO_ONLINE
    /// </summary>
    public class AsvAudioOnlinePayload : IPayload
    {
        public byte GetMaxByteSize() => 22; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 22; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 1; // Mode
            sum+= 1; // SampleRate
            sum+= 1; // Channels
            sum+= 1; // Format
            sum+= 1; // Codec
            sum+=1; //CodecCfg
            sum+=Name.Length; //Name
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Mode = (AsvAudioModeFlag)BinSerialize.ReadByte(ref buffer);
            SampleRate = (AsvAudioSampleRate)BinSerialize.ReadByte(ref buffer);
            Channels = (AsvAudioChannel)BinSerialize.ReadByte(ref buffer);
            Format = (AsvAudioPcmFormat)BinSerialize.ReadByte(ref buffer);
            Codec = (AsvAudioCodec)BinSerialize.ReadByte(ref buffer);
            CodecCfg = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/22 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
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
            BinSerialize.WriteByte(ref buffer,(byte)Mode);
            BinSerialize.WriteByte(ref buffer,(byte)SampleRate);
            BinSerialize.WriteByte(ref buffer,(byte)Channels);
            BinSerialize.WriteByte(ref buffer,(byte)Format);
            BinSerialize.WriteByte(ref buffer,(byte)Codec);
            BinSerialize.WriteByte(ref buffer,(byte)CodecCfg);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Name)
                {
                    Encoding.ASCII.GetBytes(charPointer, Name.Length, bytePointer, Name.Length);
                }
            }
            buffer = buffer.Slice(Name.Length);
            
            /* PayloadByteSize = 22 */;
        }
        
        



        /// <summary>
        /// Device current work mode.
        /// OriginName: mode, Units: , IsExtended: false
        /// </summary>
        public AsvAudioModeFlag Mode { get; set; }
        /// <summary>
        /// Decoded audio sample rate.
        /// OriginName: sample_rate, Units: , IsExtended: false
        /// </summary>
        public AsvAudioSampleRate SampleRate { get; set; }
        /// <summary>
        /// Channels count.
        /// OriginName: channels, Units: , IsExtended: false
        /// </summary>
        public AsvAudioChannel Channels { get; set; }
        /// <summary>
        /// PCM format.
        /// OriginName: format, Units: , IsExtended: false
        /// </summary>
        public AsvAudioPcmFormat Format { get; set; }
        /// <summary>
        /// Audio codec used by this device.
        /// OriginName: codec, Units: , IsExtended: false
        /// </summary>
        public AsvAudioCodec Codec { get; set; }
        /// <summary>
        /// Additional params for specified codec.
        /// OriginName: codec_cfg, Units: , IsExtended: false
        /// </summary>
        public byte CodecCfg { get; set; }
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
        /// System ID. If equal 0 - broadcast.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID. If equal 0 - broadcast.
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

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

// This code was generate by tool Asv.Mavlink.Shell version 3.2.5-alpha-11+3aea76daf22e6bd6e485f835f817413e247d3d85

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
    /// Device capabilities flags (uint8_t).
    ///  ASV_AUDIO_MODE_FLAG
    /// </summary>
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
    /// Audio codec (uint8_t).
    ///  ASV_AUDIO_CODEC
    /// </summary>
    public enum AsvAudioCodec:uint
    {
        /// <summary>
        /// Raw uncompressed audio (PCM).
        /// ASV_AUDIO_CODEC_RAW
        /// </summary>
        AsvAudioCodecRaw = 0,
        /// <summary>
        /// Opus (RFC 6716) – based on SILK vocoder and CELT codec.
        /// ASV_AUDIO_CODEC_OPUS
        /// </summary>
        AsvAudioCodecOpus = 1,
        /// <summary>
        /// Advanced Audio Coding (AAC).
        /// ASV_AUDIO_CODEC_AAC
        /// </summary>
        AsvAudioCodecAac = 2,
        /// <summary>
        /// G.711 μ-law (PCMU).
        /// ASV_AUDIO_CODEC_PCMU
        /// </summary>
        AsvAudioCodecPcmu = 3,
        /// <summary>
        /// G.711 A-law (PCMA).
        /// ASV_AUDIO_CODEC_PCMA
        /// </summary>
        AsvAudioCodecPcma = 4,
        /// <summary>
        /// Speex.
        /// ASV_AUDIO_CODEC_SPEEX
        /// </summary>
        AsvAudioCodecSpeex = 5,
        /// <summary>
        /// Internet Low Bitrate Codec (iLBC).
        /// ASV_AUDIO_CODEC_ILBC
        /// </summary>
        AsvAudioCodecIlbc = 6,
        /// <summary>
        /// G.722.
        /// ASV_AUDIO_CODEC_G722
        /// </summary>
        AsvAudioCodecG722 = 7,
        /// <summary>
        /// Linear Pulse Code Modulation (L16).
        /// ASV_AUDIO_CODEC_L16
        /// </summary>
        AsvAudioCodecL16 = 8,
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
        public override byte GetCrcEtra() => 212;
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
            sum+= 1; // Mode
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
            Codec = (AsvAudioCodec)BinSerialize.ReadByte(ref buffer);
            CodecCfg = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/19 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Name = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Name[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Mode);
            BinSerialize.WriteByte(ref buffer,(byte)Codec);
            BinSerialize.WriteByte(ref buffer,(byte)CodecCfg);
            for(var i=0;i<Name.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Name[i]);
            }
            /* PayloadByteSize = 19 */;
        }
        
        



        /// <summary>
        /// Device current work mode.
        /// OriginName: mode, Units: , IsExtended: false
        /// </summary>
        public AsvAudioModeFlag Mode { get; set; }
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
        public byte[] Name { get; set; } = new byte[16];
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
        public override byte GetCrcEtra() => 131;
        public override bool WrapToV2Extension => true;

        public override AsvAudioStreamPayload Payload { get; } = new AsvAudioStreamPayload();

        public override string Name => "ASV_AUDIO_STREAM";
    }

    /// <summary>
    ///  ASV_AUDIO_STREAM
    /// </summary>
    public class AsvAudioStreamPayload : IPayload
    {
        public byte GetMaxByteSize() => 235; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 235; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //PacketsInFrame
            sum+=1; //SequenceNumber
            sum+=1; //DataSzie
            sum+=Data.Length; //Data
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            PacketsInFrame = (byte)BinSerialize.ReadByte(ref buffer);
            SequenceNumber = (byte)BinSerialize.ReadByte(ref buffer);
            DataSzie = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/230 - Math.Max(0,((/*PayloadByteSize*/235 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
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
            BinSerialize.WriteByte(ref buffer,(byte)PacketsInFrame);
            BinSerialize.WriteByte(ref buffer,(byte)SequenceNumber);
            BinSerialize.WriteByte(ref buffer,(byte)DataSzie);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            /* PayloadByteSize = 235 */;
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
        /// Number of packets for one encoded audio frame.
        /// OriginName: packets_in_frame, Units: , IsExtended: false
        /// </summary>
        public byte PacketsInFrame { get; set; }
        /// <summary>
        /// Sequence number (starting with 0 on every encoded frame).
        /// OriginName: sequence_number, Units: , IsExtended: false
        /// </summary>
        public byte SequenceNumber { get; set; }
        /// <summary>
        /// Size of data array.
        /// OriginName: data_szie, Units: , IsExtended: false
        /// </summary>
        public byte DataSzie { get; set; }
        /// <summary>
        /// Audio data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public byte[] Data { get; set; } = new byte[230];
        public byte GetDataMaxItemsCount() => 230;
    }


#endregion


}

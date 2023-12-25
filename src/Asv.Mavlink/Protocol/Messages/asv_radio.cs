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

// This code was generate by tool Asv.Mavlink.Shell version 3.2.5-alpha-11+6881e692bec5d36a0fe50f4b69f669d0f2f2847f

using System;
using System.Text;
using System.ComponentModel;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Asv.IO;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink.V2.AsvRadio
{

    public static class AsvRadioHelper
    {
        public static void RegisterAsvRadioDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AsvRadioStatusPacket());
            src.Register(()=>new AsvRadioCapabilitiesRequestPacket());
            src.Register(()=>new AsvRadioCapabilitiesResponsePacket());
            src.Register(()=>new AsvRadioCodecCfgRequestPacket());
            src.Register(()=>new AsvRadioCodecCfgResponsePacket());
        }
    }

#region Enums

    /// <summary>
    /// A mapping of RADIO modes for custom_mode field of heartbeat.
    ///  ASV_RADIO_CUSTOM_MODE
    /// </summary>
    public enum AsvRadioCustomMode:uint
    {
        /// <summary>
        /// ASV_RADIO_CUSTOM_MODE_IDLE
        /// </summary>
        AsvRadioCustomModeIdle = 1,
        /// <summary>
        /// ASV_RADIO_CUSTOM_MODE_ONAIR
        /// </summary>
        AsvRadioCustomModeOnair = 2,
    }

    /// <summary>
    ///  MAV_TYPE
    /// </summary>
    public enum MavType:uint
    {
        /// <summary>
        /// Used to identify radio payload in HEARTBEAT packet.
        /// MAV_TYPE_ASV_RADIO
        /// </summary>
        MavTypeAsvRadio = 252,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Enable radio transmiiter. Change mode to ASV_RADIO_CUSTOM_MODE_ONAIR
        /// Param 1 - Reference frequency in Hz (unit32_t).
        /// Param 2 - RF modulation type, see ASV_RADIO_MODULATION (uint32_t).
        /// Param 3 - Set RX reciever or\and TX tranmitter enabled, see ASV_RADIO_RF_FLAG (uint32_t).
        /// Param 4 - Estimated RX reference power in dBm. Needed to tune the internal amplifiers and filters (float).
        /// Param 5 - TX power in dBm (float).
        /// Param 6 - Digital audio codec, see ASV_AUDIO_CODEC (unit32_t).
        /// Param 7 - Digital audio codec options, see ASV_AUDIO_CODEC_[*]_CFG (uint32_t).
        /// MAV_CMD_ASV_RADIO_ON
        /// </summary>
        MavCmdAsvRadioOn = 13250,
        /// <summary>
        /// Disable radio transmiiter. Change mode to ASV_RADIO_CUSTOM_MODE_IDLE
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_RADIO_OFF
        /// </summary>
        MavCmdAsvRadioOff = 13251,
    }

    /// <summary>
    /// RF modulation (uint8_t).
    ///  ASV_RADIO_MODULATION
    /// </summary>
    public enum AsvRadioModulation:uint
    {
        /// <summary>
        /// AM modulation.
        /// ASV_RADIO_MODULATION_AM
        /// </summary>
        AsvRadioModulationAm = 0,
        /// <summary>
        /// FM modulation.
        /// ASV_RADIO_MODULATION_FM
        /// </summary>
        AsvRadioModulationFm = 1,
    }

    /// <summary>
    /// RF device mode falgs (uint8_t).
    ///  ASV_RADIO_RF_FLAG
    /// </summary>
    public enum AsvRadioRfFlag:uint
    {
        /// <summary>
        /// RX channel for listen RF audio is enabled.
        /// ASV_RADIO_RF_FLAG_RX_ENABLED
        /// </summary>
        AsvRadioRfFlagRxEnabled = 1,
        /// <summary>
        /// TX channgel for transmitting RF audio is enabled.
        /// ASV_RADIO_RF_FLAG_TX_ENABLED
        /// </summary>
        AsvRadioRfFlagTxEnabled = 2,
    }


#endregion

#region Messages

    /// <summary>
    /// Status of radio device. [!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RADIO_STATUS
    /// </summary>
    public class AsvRadioStatusPacket: PacketV2<AsvRadioStatusPayload>
    {
	    public const int PacketMessageId = 13250;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 178;
        public override bool WrapToV2Extension => true;

        public override AsvRadioStatusPayload Payload { get; } = new AsvRadioStatusPayload();

        public override string Name => "ASV_RADIO_STATUS";
    }

    /// <summary>
    ///  ASV_RADIO_STATUS
    /// </summary>
    public class AsvRadioStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 1; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 1; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 1; // CurrentCodec
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            CurrentCodec = (AsvAudioCodec)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)CurrentCodec);
            /* PayloadByteSize = 1 */;
        }
        
        



        /// <summary>
        /// Current audio codecs.
        /// OriginName: current_codec, Units: , IsExtended: false
        /// </summary>
        public AsvAudioCodec CurrentCodec { get; set; }
    }
    /// <summary>
    /// Request for device capabilities. Devices must reply ASV_RADIO_CAPABILITIES_RESPONSE message.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RADIO_CAPABILITIES_REQUEST
    /// </summary>
    public class AsvRadioCapabilitiesRequestPacket: PacketV2<AsvRadioCapabilitiesRequestPayload>
    {
	    public const int PacketMessageId = 13251;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 10;
        public override bool WrapToV2Extension => true;

        public override AsvRadioCapabilitiesRequestPayload Payload { get; } = new AsvRadioCapabilitiesRequestPayload();

        public override string Name => "ASV_RADIO_CAPABILITIES_REQUEST";
    }

    /// <summary>
    ///  ASV_RADIO_CAPABILITIES_REQUEST
    /// </summary>
    public class AsvRadioCapabilitiesRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 2; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 2; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 2 */;
        }
        
        



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
    }
    /// <summary>
    /// Device capabilities. This is response for ASV_RADIO_CAPABILITIES_REQUEST message.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RADIO_CAPABILITIES_RESPONSE
    /// </summary>
    public class AsvRadioCapabilitiesResponsePacket: PacketV2<AsvRadioCapabilitiesResponsePayload>
    {
	    public const int PacketMessageId = 13252;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 138;
        public override bool WrapToV2Extension => true;

        public override AsvRadioCapabilitiesResponsePayload Payload { get; } = new AsvRadioCapabilitiesResponsePayload();

        public override string Name => "ASV_RADIO_CAPABILITIES_RESPONSE";
    }

    /// <summary>
    ///  ASV_RADIO_CAPABILITIES_RESPONSE
    /// </summary>
    public class AsvRadioCapabilitiesResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 64; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 64; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=SupportedModulation.Length; //SupportedModulation
            sum+=Codecs.Length; //Codecs
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/64 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            SupportedModulation = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                SupportedModulation[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 32;
            for(var i=0;i<arraySize;i++)
            {
                Codecs[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<SupportedModulation.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)SupportedModulation[i]);
            }
            for(var i=0;i<Codecs.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Codecs[i]);
            }
            /* PayloadByteSize = 64 */;
        }
        
        



        /// <summary>
        /// Supported RF modulations. Each bit of array is flag from ASV_RADIO_MODULATION(max 255 items) enum.
        /// OriginName: supported_modulation, Units: , IsExtended: false
        /// </summary>
        public const int SupportedModulationMaxItemsCount = 32;
        public byte[] SupportedModulation { get; set; } = new byte[32];
        [Obsolete("This method is deprecated. Use GetSupportedModulationMaxItemsCount instead.")]
        public byte GetSupportedModulationMaxItemsCount() => 32;
        /// <summary>
        /// Supported codecs. Each bit of array is flag from ASV_RADIO_CODEC(max 255 items) enum .
        /// OriginName: codecs, Units: , IsExtended: false
        /// </summary>
        public const int CodecsMaxItemsCount = 32;
        public byte[] Codecs { get; } = new byte[32];
    }
    /// <summary>
    /// Request supported additional params for target codec. Devices must reply ASV_RADIO_CODEC_CFG_RESPONSE message.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RADIO_CODEC_CFG_REQUEST
    /// </summary>
    public class AsvRadioCodecCfgRequestPacket: PacketV2<AsvRadioCodecCfgRequestPayload>
    {
	    public const int PacketMessageId = 13253;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 112;
        public override bool WrapToV2Extension => true;

        public override AsvRadioCodecCfgRequestPayload Payload { get; } = new AsvRadioCodecCfgRequestPayload();

        public override string Name => "ASV_RADIO_CODEC_CFG_REQUEST";
    }

    /// <summary>
    ///  ASV_RADIO_CODEC_CFG_REQUEST
    /// </summary>
    public class AsvRadioCodecCfgRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+= 1; // TargetCodec
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            TargetCodec = (AsvAudioCodec)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)TargetCodec);
            /* PayloadByteSize = 3 */;
        }
        
        



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
        /// Target audio codec.
        /// OriginName: target_codec, Units: , IsExtended: false
        /// </summary>
        public AsvAudioCodec TargetCodec { get; set; }
    }
    /// <summary>
    /// Request supported additional params for target codec. Devices must reply ASV_RADIO_CODEC_CFG_REQUEST message.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RADIO_CODEC_CFG_RESPONSE
    /// </summary>
    public class AsvRadioCodecCfgResponsePacket: PacketV2<AsvRadioCodecCfgResponsePayload>
    {
	    public const int PacketMessageId = 13254;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 129;
        public override bool WrapToV2Extension => true;

        public override AsvRadioCodecCfgResponsePayload Payload { get; } = new AsvRadioCodecCfgResponsePayload();

        public override string Name => "ASV_RADIO_CODEC_CFG_RESPONSE";
    }

    /// <summary>
    ///  ASV_RADIO_CODEC_CFG_RESPONSE
    /// </summary>
    public class AsvRadioCodecCfgResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 33; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 33; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 1; // TargetCodec
            sum+=SupportedCfg.Length; //SupportedCfg
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TargetCodec = (AsvAudioCodec)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/33 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            SupportedCfg = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                SupportedCfg[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetCodec);
            for(var i=0;i<SupportedCfg.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)SupportedCfg[i]);
            }
            /* PayloadByteSize = 33 */;
        }
        
        



        /// <summary>
        /// Selected audio codec.
        /// OriginName: target_codec, Units: , IsExtended: false
        /// </summary>
        public AsvAudioCodec TargetCodec { get; set; }
        /// <summary>
        /// Supported additional params for target codec. Each bit of array is flag from ASV_AUDIO_CODEC_[*]_CFG(max 255 items) enum .
        /// OriginName: supported_cfg, Units: , IsExtended: false
        /// </summary>
        public const int SupportedCfgMaxItemsCount = 32;
        public byte[] SupportedCfg { get; set; } = new byte[32];
        [Obsolete("This method is deprecated. Use GetSupportedCfgMaxItemsCount instead.")]
        public byte GetSupportedCfgMaxItemsCount() => 32;
    }


#endregion


}

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
            src.Register(()=>new AsvRadioCodecCapabilitiesRequestPacket());
            src.Register(()=>new AsvRadioCodecCapabilitiesResponsePacket());
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
        AsvRadioCustomModeIdle = 0,
        /// <summary>
        /// ASV_RADIO_CUSTOM_MODE_ONAIR
        /// </summary>
        AsvRadioCustomModeOnair = 1,
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
    /// RF modulation (uint8_t).
    ///  ASV_RADIO_MODULATION
    /// </summary>
    public enum AsvRadioModulation:uint
    {
        /// <summary>
        /// Not set modulation.
        /// ASV_RADIO_MODULATION_UNKNOWN
        /// </summary>
        AsvRadioModulationUnknown = 0,
        /// <summary>
        /// AM modulation.
        /// ASV_RADIO_MODULATION_AM
        /// </summary>
        AsvRadioModulationAm = 1,
        /// <summary>
        /// FM modulation.
        /// ASV_RADIO_MODULATION_FM
        /// </summary>
        AsvRadioModulationFm = 2,
    }

    /// <summary>
    /// RF device mode falgs (uint8_t).[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RADIO_RF_MODE_FLAG
    /// </summary>
    [Flags]
    public enum AsvRadioRfModeFlag:uint
    {
        /// <summary>
        /// RX channel found RF signal.
        /// ASV_RADIO_RF_MODE_FLAG_RX_ON_AIR
        /// </summary>
        AsvRadioRfModeFlagRxOnAir = 1,
        /// <summary>
        /// TX channel transmitting RF signal.
        /// ASV_RADIO_RF_MODE_FLAG_TX_ON_AIR
        /// </summary>
        AsvRadioRfModeFlagTxOnAir = 2,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Enable radio transmiiter. Change mode to ASV_RADIO_CUSTOM_MODE_ONAIR
        /// Param 1 - Reference frequency in Hz (unit32_t).
        /// Param 2 - RF modulation type, see ASV_RADIO_MODULATION (uint8_t).
        /// Param 3 - Estimated RX reference power in dBm. May be needed to tune the internal amplifiers and filters. NaN for auto-gain (float).
        /// Param 4 - TX power in dBm (float).
        /// Param 5 - Digital audio codec, see ASV_AUDIO_CODEC (uint16_t).
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ASV_RADIO_ON
        /// </summary>
        MavCmdAsvRadioOn = 13250,
        /// <summary>
        /// Disable radio. Change mode to ASV_RADIO_CUSTOM_MODE_IDLE
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
        public override byte GetCrcEtra() => 154;
        public override bool WrapToV2Extension => true;

        public override AsvRadioStatusPayload Payload { get; } = new AsvRadioStatusPayload();

        public override string Name => "ASV_RADIO_STATUS";
    }

    /// <summary>
    ///  ASV_RADIO_STATUS
    /// </summary>
    public class AsvRadioStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 21; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 21; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Freq
            sum+=4; //TxLevel
            sum+=4; //RxLevel
            sum+=4; //RxEstimatedLevel
            sum+= 4; // RfMode
            sum+= 1; // Modulation
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Freq = BinSerialize.ReadFloat(ref buffer);
            TxLevel = BinSerialize.ReadFloat(ref buffer);
            RxLevel = BinSerialize.ReadFloat(ref buffer);
            RxEstimatedLevel = BinSerialize.ReadFloat(ref buffer);
            RfMode = (AsvRadioRfModeFlag)BinSerialize.ReadUInt(ref buffer);
            Modulation = (AsvRadioModulation)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Freq);
            BinSerialize.WriteFloat(ref buffer,TxLevel);
            BinSerialize.WriteFloat(ref buffer,RxLevel);
            BinSerialize.WriteFloat(ref buffer,RxEstimatedLevel);
            BinSerialize.WriteUInt(ref buffer,(uint)RfMode);
            BinSerialize.WriteByte(ref buffer,(byte)Modulation);
            /* PayloadByteSize = 21 */;
        }
        
        



        /// <summary>
        /// RF frequency.
        /// OriginName: freq, Units: , IsExtended: false
        /// </summary>
        public float Freq { get; set; }
        /// <summary>
        /// Current TX power in dBm.
        /// OriginName: tx_level, Units: , IsExtended: false
        /// </summary>
        public float TxLevel { get; set; }
        /// <summary>
        /// Measured RX power in dBm.
        /// OriginName: rx_level, Units: , IsExtended: false
        /// </summary>
        public float RxLevel { get; set; }
        /// <summary>
        /// Estimated RX reference power in dBm.
        /// OriginName: rx_estimated_level, Units: , IsExtended: false
        /// </summary>
        public float RxEstimatedLevel { get; set; }
        /// <summary>
        /// RF mode.
        /// OriginName: rf_mode, Units: , IsExtended: false
        /// </summary>
        public AsvRadioRfModeFlag RfMode { get; set; }
        /// <summary>
        /// Current RF modulation.
        /// OriginName: modulation, Units: , IsExtended: false
        /// </summary>
        public AsvRadioModulation Modulation { get; set; }
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
        public override byte GetCrcEtra() => 62;
        public override bool WrapToV2Extension => true;

        public override AsvRadioCapabilitiesResponsePayload Payload { get; } = new AsvRadioCapabilitiesResponsePayload();

        public override string Name => "ASV_RADIO_CAPABILITIES_RESPONSE";
    }

    /// <summary>
    ///  ASV_RADIO_CAPABILITIES_RESPONSE
    /// </summary>
    public class AsvRadioCapabilitiesResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 56; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 56; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //MaxRfFreq
            sum+=4; //MinRfFreq
            sum+=4; //MaxTxPower
            sum+=4; //MinTxPower
            sum+=4; //MaxRxPower
            sum+=4; //MinRxPower
            sum+=SupportedModulation.Length; //SupportedModulation
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            MaxRfFreq = BinSerialize.ReadUInt(ref buffer);
            MinRfFreq = BinSerialize.ReadUInt(ref buffer);
            MaxTxPower = BinSerialize.ReadFloat(ref buffer);
            MinTxPower = BinSerialize.ReadFloat(ref buffer);
            MaxRxPower = BinSerialize.ReadFloat(ref buffer);
            MinRxPower = BinSerialize.ReadFloat(ref buffer);
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/56 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            SupportedModulation = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                SupportedModulation[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,MaxRfFreq);
            BinSerialize.WriteUInt(ref buffer,MinRfFreq);
            BinSerialize.WriteFloat(ref buffer,MaxTxPower);
            BinSerialize.WriteFloat(ref buffer,MinTxPower);
            BinSerialize.WriteFloat(ref buffer,MaxRxPower);
            BinSerialize.WriteFloat(ref buffer,MinRxPower);
            for(var i=0;i<SupportedModulation.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)SupportedModulation[i]);
            }
            /* PayloadByteSize = 56 */;
        }
        
        



        /// <summary>
        /// Max RF frequency in Hz.
        /// OriginName: max_rf_freq, Units: , IsExtended: false
        /// </summary>
        public uint MaxRfFreq { get; set; }
        /// <summary>
        /// Min RF frequency in Hz.
        /// OriginName: min_rf_freq, Units: , IsExtended: false
        /// </summary>
        public uint MinRfFreq { get; set; }
        /// <summary>
        /// Max TX power in dBm.
        /// OriginName: max_tx_power, Units: , IsExtended: false
        /// </summary>
        public float MaxTxPower { get; set; }
        /// <summary>
        /// Min TX power in dBm.
        /// OriginName: min_tx_power, Units: , IsExtended: false
        /// </summary>
        public float MinTxPower { get; set; }
        /// <summary>
        /// Max estimated RX power in dBm.
        /// OriginName: max_rx_power, Units: , IsExtended: false
        /// </summary>
        public float MaxRxPower { get; set; }
        /// <summary>
        /// Min estimated RX power in dBm.
        /// OriginName: min_rx_power, Units: , IsExtended: false
        /// </summary>
        public float MinRxPower { get; set; }
        /// <summary>
        /// Supported RF modulations. Each bit of array is flag from ASV_RADIO_MODULATION(max 255 items) enum.
        /// OriginName: supported_modulation, Units: , IsExtended: false
        /// </summary>
        public const int SupportedModulationMaxItemsCount = 32;
        public byte[] SupportedModulation { get; set; } = new byte[32];
        [Obsolete("This method is deprecated. Use GetSupportedModulationMaxItemsCount instead.")]
        public byte GetSupportedModulationMaxItemsCount() => 32;
    }
    /// <summary>
    /// Request supported target codecs. Devices must reply ASV_RADIO_CODEC_CAPABILITIES_RESPONSE message.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RADIO_CODEC_CAPABILITIES_REQUEST
    /// </summary>
    public class AsvRadioCodecCapabilitiesRequestPacket: PacketV2<AsvRadioCodecCapabilitiesRequestPayload>
    {
	    public const int PacketMessageId = 13253;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 205;
        public override bool WrapToV2Extension => true;

        public override AsvRadioCodecCapabilitiesRequestPayload Payload { get; } = new AsvRadioCodecCapabilitiesRequestPayload();

        public override string Name => "ASV_RADIO_CODEC_CAPABILITIES_REQUEST";
    }

    /// <summary>
    ///  ASV_RADIO_CODEC_CAPABILITIES_REQUEST
    /// </summary>
    public class AsvRadioCodecCapabilitiesRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 5; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 5; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //Skip
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //Count
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Skip = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Count = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,Skip);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Count);
            /* PayloadByteSize = 5 */;
        }
        
        



        /// <summary>
        /// Skip index.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public ushort Skip { get; set; }
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
        /// Codec count at ASV_RADIO_CODEC_CAPABILITIES_RESPONSE.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public byte Count { get; set; }
    }
    /// <summary>
    /// Request supported additional params for target codec. Devices must reply ASV_RADIO_CODEC_CAPABILITIES_REQUEST message.[!WRAP_TO_V2_EXTENSION_PACKET!]
    ///  ASV_RADIO_CODEC_CAPABILITIES_RESPONSE
    /// </summary>
    public class AsvRadioCodecCapabilitiesResponsePacket: PacketV2<AsvRadioCodecCapabilitiesResponsePayload>
    {
	    public const int PacketMessageId = 13254;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 228;
        public override bool WrapToV2Extension => true;

        public override AsvRadioCodecCapabilitiesResponsePayload Payload { get; } = new AsvRadioCodecCapabilitiesResponsePayload();

        public override string Name => "ASV_RADIO_CODEC_CAPABILITIES_RESPONSE";
    }

    /// <summary>
    ///  ASV_RADIO_CODEC_CAPABILITIES_RESPONSE
    /// </summary>
    public class AsvRadioCodecCapabilitiesResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 205; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 205; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //All
            sum+=2; //Skip
            sum+= Codecs.Length * 2; // Codecs
            
            sum+=1; //Count
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            All = BinSerialize.ReadUShort(ref buffer);
            Skip = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/100 - Math.Max(0,((/*PayloadByteSize*/205 - payloadSize - /*ExtendedFieldsLength*/0)/*FieldTypeByteSize*/ /2));
            Codecs = new AsvAudioCodec[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Codecs[i] = (AsvAudioCodec)BinSerialize.ReadUShort(ref buffer);
            }

            Count = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,All);
            BinSerialize.WriteUShort(ref buffer,Skip);
            for(var i=0;i<Codecs.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,(ushort)Codecs[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)Count);
            /* PayloadByteSize = 205 */;
        }
        
        



        /// <summary>
        /// All codec codecs.
        /// OriginName: all, Units: , IsExtended: false
        /// </summary>
        public ushort All { get; set; }
        /// <summary>
        /// Skip index codec.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public ushort Skip { get; set; }
        /// <summary>
        /// Supported codec array.
        /// OriginName: codecs, Units: , IsExtended: false
        /// </summary>
        public const int CodecsMaxItemsCount = 100;    
        public AsvAudioCodec[] Codecs { get; set; } = new AsvAudioCodec[100];
        /// <summary>
        /// Array size.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public byte Count { get; set; }
    }


#endregion


}

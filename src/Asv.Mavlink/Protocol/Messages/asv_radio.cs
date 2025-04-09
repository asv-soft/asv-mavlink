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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.11+22841a669900eb4c494a7e77e2d4b5fee4e474db

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.IO;
using Asv.Mavlink.AsvAudio;

namespace Asv.Mavlink.AsvRadio
{

    public static class AsvRadioHelper
    {
        public static void RegisterAsvRadioDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(AsvRadioStatusPacket.MessageId, ()=>new AsvRadioStatusPacket());
            src.Add(AsvRadioCapabilitiesRequestPacket.MessageId, ()=>new AsvRadioCapabilitiesRequestPacket());
            src.Add(AsvRadioCapabilitiesResponsePacket.MessageId, ()=>new AsvRadioCapabilitiesResponsePacket());
            src.Add(AsvRadioCodecCapabilitiesRequestPacket.MessageId, ()=>new AsvRadioCodecCapabilitiesRequestPacket());
            src.Add(AsvRadioCodecCapabilitiesResponsePacket.MessageId, ()=>new AsvRadioCodecCapabilitiesResponsePacket());
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
    public class AsvRadioStatusPacket : MavlinkV2Message<AsvRadioStatusPayload>
    {
        public const int MessageId = 13250;
        
        public const byte CrcExtra = 154;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRadioStatusPayload Payload { get; } = new();

        public override string Name => "ASV_RADIO_STATUS";
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("freq",
"RF frequency.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("tx_level",
"Current TX power in dBm.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_level",
"Measured RX power in dBm.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rx_estimated_level",
"Estimated RX reference power in dBm.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rf_mode",
"RF mode.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("modulation",
"Current RF modulation.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RADIO_STATUS:"
        + "float freq;"
        + "float tx_level;"
        + "float rx_level;"
        + "float rx_estimated_level;"
        + "uint32_t rf_mode;"
        + "uint8_t modulation;"
        ;
    }

    /// <summary>
    ///  ASV_RADIO_STATUS
    /// </summary>
    public class AsvRadioStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 21; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    public class AsvRadioCapabilitiesRequestPacket : MavlinkV2Message<AsvRadioCapabilitiesRequestPayload>
    {
        public const int MessageId = 13251;
        
        public const byte CrcExtra = 10;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRadioCapabilitiesRequestPayload Payload { get; } = new();

        public override string Name => "ASV_RADIO_CAPABILITIES_REQUEST";
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("target_system",
"System ID",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RADIO_CAPABILITIES_REQUEST:"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
    }

    /// <summary>
    ///  ASV_RADIO_CAPABILITIES_REQUEST
    /// </summary>
    public class AsvRadioCapabilitiesRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 2; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    public class AsvRadioCapabilitiesResponsePacket : MavlinkV2Message<AsvRadioCapabilitiesResponsePayload>
    {
        public const int MessageId = 13252;
        
        public const byte CrcExtra = 62;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRadioCapabilitiesResponsePayload Payload { get; } = new();

        public override string Name => "ASV_RADIO_CAPABILITIES_RESPONSE";
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("max_rf_freq",
"Max RF frequency in Hz.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("min_rf_freq",
"Min RF frequency in Hz.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("max_tx_power",
"Max TX power in dBm.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("min_tx_power",
"Min TX power in dBm.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("max_rx_power",
"Max estimated RX power in dBm.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("min_rx_power",
"Min estimated RX power in dBm.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("supported_modulation",
"Supported RF modulations. Each bit of array is flag from ASV_RADIO_MODULATION(max 255 items) enum.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            32, 
false),
        ];
        public const string FormatMessage = "ASV_RADIO_CAPABILITIES_RESPONSE:"
        + "uint32_t max_rf_freq;"
        + "uint32_t min_rf_freq;"
        + "float max_tx_power;"
        + "float min_tx_power;"
        + "float max_rx_power;"
        + "float min_rx_power;"
        + "uint8_t[32] supported_modulation;"
        ;
    }

    /// <summary>
    ///  ASV_RADIO_CAPABILITIES_RESPONSE
    /// </summary>
    public class AsvRadioCapabilitiesResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 56; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    public class AsvRadioCodecCapabilitiesRequestPacket : MavlinkV2Message<AsvRadioCodecCapabilitiesRequestPayload>
    {
        public const int MessageId = 13253;
        
        public const byte CrcExtra = 205;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRadioCodecCapabilitiesRequestPayload Payload { get; } = new();

        public override string Name => "ASV_RADIO_CODEC_CAPABILITIES_REQUEST";
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("skip",
"Skip index.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("target_system",
"System ID",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("count",
"Codec count at ASV_RADIO_CODEC_CAPABILITIES_RESPONSE.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RADIO_CODEC_CAPABILITIES_REQUEST:"
        + "uint16_t skip;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t count;"
        ;
    }

    /// <summary>
    ///  ASV_RADIO_CODEC_CAPABILITIES_REQUEST
    /// </summary>
    public class AsvRadioCodecCapabilitiesRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 5; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    public class AsvRadioCodecCapabilitiesResponsePacket : MavlinkV2Message<AsvRadioCodecCapabilitiesResponsePayload>
    {
        public const int MessageId = 13254;
        
        public const byte CrcExtra = 228;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => true;

        public override AsvRadioCodecCapabilitiesResponsePayload Payload { get; } = new();

        public override string Name => "ASV_RADIO_CODEC_CAPABILITIES_RESPONSE";
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("all",
"All codec codecs.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("skip",
"Skip index codec.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("codecs",
"Supported codec array.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            100, 
false),
            new("count",
"Array size.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "ASV_RADIO_CODEC_CAPABILITIES_RESPONSE:"
        + "uint16_t all;"
        + "uint16_t skip;"
        + "uint16_t[100] codecs;"
        + "uint8_t count;"
        ;
    }

    /// <summary>
    ///  ASV_RADIO_CODEC_CAPABILITIES_RESPONSE
    /// </summary>
    public class AsvRadioCodecCapabilitiesResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 205; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

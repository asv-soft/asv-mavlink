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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.2+82bde669fa8b85517700c6d12362e9f17d819d33 25-06-23.

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
    public enum AsvRadioCustomMode : ulong
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
    public static class AsvRadioCustomModeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_RADIO_CUSTOM_MODE_IDLE");
            yield return new EnumValue<T>(converter(1),"ASV_RADIO_CUSTOM_MODE_ONAIR");
        }
    }
    /// <summary>
    ///  MAV_TYPE
    /// </summary>
    public enum MavType : ulong
    {
        /// <summary>
        /// Used to identify radio payload in HEARTBEAT packet.
        /// MAV_TYPE_ASV_RADIO
        /// </summary>
        MavTypeAsvRadio = 252,
    }
    public static class MavTypeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(252);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(252),"MAV_TYPE_ASV_RADIO");
        }
    }
    /// <summary>
    /// RF modulation (uint8_t).
    ///  ASV_RADIO_MODULATION
    /// </summary>
    public enum AsvRadioModulation : ulong
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
    public static class AsvRadioModulationHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ASV_RADIO_MODULATION_UNKNOWN");
            yield return new EnumValue<T>(converter(1),"ASV_RADIO_MODULATION_AM");
            yield return new EnumValue<T>(converter(2),"ASV_RADIO_MODULATION_FM");
        }
    }
    /// <summary>
    /// RF device mode falgs (uint8_t).[!THIS_IS_ENUM_FLAG!]
    ///  ASV_RADIO_RF_MODE_FLAG
    /// </summary>
    [Flags]
    public enum AsvRadioRfModeFlag : ulong
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
    public static class AsvRadioRfModeFlagHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"ASV_RADIO_RF_MODE_FLAG_RX_ON_AIR");
            yield return new EnumValue<T>(converter(2),"ASV_RADIO_RF_MODE_FLAG_TX_ON_AIR");
        }
    }
    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd : ulong
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
    public static class MavCmdHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(13250);
            yield return converter(13251);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(13250),"MAV_CMD_ASV_RADIO_ON");
            yield return new EnumValue<T>(converter(13251),"MAV_CMD_ASV_RADIO_OFF");
        }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float freq
            +4 // float tx_level
            +4 // float rx_level
            +4 // float rx_estimated_level
            + 4 // uint32_t rf_mode
            + 1 // uint8_t modulation
            );
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

        public void Accept(IVisitor visitor)
        {
            FloatType.Accept(visitor,FreqField, FreqField.DataType, ref _freq);    
            FloatType.Accept(visitor,TxLevelField, TxLevelField.DataType, ref _txLevel);    
            FloatType.Accept(visitor,RxLevelField, RxLevelField.DataType, ref _rxLevel);    
            FloatType.Accept(visitor,RxEstimatedLevelField, RxEstimatedLevelField.DataType, ref _rxEstimatedLevel);    
            var tmpRfMode = (uint)RfMode;
            UInt32Type.Accept(visitor,RfModeField, RfModeField.DataType, ref tmpRfMode);
            RfMode = (AsvRadioRfModeFlag)tmpRfMode;
            var tmpModulation = (byte)Modulation;
            UInt8Type.Accept(visitor,ModulationField, ModulationField.DataType, ref tmpModulation);
            Modulation = (AsvRadioModulation)tmpModulation;

        }

        /// <summary>
        /// RF frequency.
        /// OriginName: freq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FreqField = new Field.Builder()
            .Name(nameof(Freq))
            .Title("freq")
            .Description("RF frequency.")

            .DataType(FloatType.Default)
        .Build();
        private float _freq;
        public float Freq { get => _freq; set => _freq = value; }
        /// <summary>
        /// Current TX power in dBm.
        /// OriginName: tx_level, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TxLevelField = new Field.Builder()
            .Name(nameof(TxLevel))
            .Title("tx_level")
            .Description("Current TX power in dBm.")

            .DataType(FloatType.Default)
        .Build();
        private float _txLevel;
        public float TxLevel { get => _txLevel; set => _txLevel = value; }
        /// <summary>
        /// Measured RX power in dBm.
        /// OriginName: rx_level, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RxLevelField = new Field.Builder()
            .Name(nameof(RxLevel))
            .Title("rx_level")
            .Description("Measured RX power in dBm.")

            .DataType(FloatType.Default)
        .Build();
        private float _rxLevel;
        public float RxLevel { get => _rxLevel; set => _rxLevel = value; }
        /// <summary>
        /// Estimated RX reference power in dBm.
        /// OriginName: rx_estimated_level, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RxEstimatedLevelField = new Field.Builder()
            .Name(nameof(RxEstimatedLevel))
            .Title("rx_estimated_level")
            .Description("Estimated RX reference power in dBm.")

            .DataType(FloatType.Default)
        .Build();
        private float _rxEstimatedLevel;
        public float RxEstimatedLevel { get => _rxEstimatedLevel; set => _rxEstimatedLevel = value; }
        /// <summary>
        /// RF mode.
        /// OriginName: rf_mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RfModeField = new Field.Builder()
            .Name(nameof(RfMode))
            .Title("rf_mode")
            .Description("RF mode.")
            .DataType(new UInt32Type(AsvRadioRfModeFlagHelper.GetValues(x=>(uint)x).Min(),AsvRadioRfModeFlagHelper.GetValues(x=>(uint)x).Max()))
            .Enum(AsvRadioRfModeFlagHelper.GetEnumValues(x=>(uint)x))
            .Build();
        private AsvRadioRfModeFlag _rfMode;
        public AsvRadioRfModeFlag RfMode { get => _rfMode; set => _rfMode = value; } 
        /// <summary>
        /// Current RF modulation.
        /// OriginName: modulation, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ModulationField = new Field.Builder()
            .Name(nameof(Modulation))
            .Title("modulation")
            .Description("Current RF modulation.")
            .DataType(new UInt8Type(AsvRadioModulationHelper.GetValues(x=>(byte)x).Min(),AsvRadioModulationHelper.GetValues(x=>(byte)x).Max()))
            .Enum(AsvRadioModulationHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private AsvRadioModulation _modulation;
        public AsvRadioModulation Modulation { get => _modulation; set => _modulation = value; } 
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
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

        public void Accept(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    

        }

        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetSystem;
        public byte TargetSystem { get => _targetSystem; set => _targetSystem = value; }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetComponent;
        public byte TargetComponent { get => _targetComponent; set => _targetComponent = value; }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t max_rf_freq
            +4 // uint32_t min_rf_freq
            +4 // float max_tx_power
            +4 // float min_tx_power
            +4 // float max_rx_power
            +4 // float min_rx_power
            +SupportedModulation.Length // uint8_t[32] supported_modulation
            );
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,MaxRfFreqField, MaxRfFreqField.DataType, ref _maxRfFreq);    
            UInt32Type.Accept(visitor,MinRfFreqField, MinRfFreqField.DataType, ref _minRfFreq);    
            FloatType.Accept(visitor,MaxTxPowerField, MaxTxPowerField.DataType, ref _maxTxPower);    
            FloatType.Accept(visitor,MinTxPowerField, MinTxPowerField.DataType, ref _minTxPower);    
            FloatType.Accept(visitor,MaxRxPowerField, MaxRxPowerField.DataType, ref _maxRxPower);    
            FloatType.Accept(visitor,MinRxPowerField, MinRxPowerField.DataType, ref _minRxPower);    
            ArrayType.Accept(visitor,SupportedModulationField, SupportedModulationField.DataType, 32,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref SupportedModulation[index]));    

        }

        /// <summary>
        /// Max RF frequency in Hz.
        /// OriginName: max_rf_freq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MaxRfFreqField = new Field.Builder()
            .Name(nameof(MaxRfFreq))
            .Title("max_rf_freq")
            .Description("Max RF frequency in Hz.")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _maxRfFreq;
        public uint MaxRfFreq { get => _maxRfFreq; set => _maxRfFreq = value; }
        /// <summary>
        /// Min RF frequency in Hz.
        /// OriginName: min_rf_freq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MinRfFreqField = new Field.Builder()
            .Name(nameof(MinRfFreq))
            .Title("min_rf_freq")
            .Description("Min RF frequency in Hz.")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _minRfFreq;
        public uint MinRfFreq { get => _minRfFreq; set => _minRfFreq = value; }
        /// <summary>
        /// Max TX power in dBm.
        /// OriginName: max_tx_power, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MaxTxPowerField = new Field.Builder()
            .Name(nameof(MaxTxPower))
            .Title("max_tx_power")
            .Description("Max TX power in dBm.")

            .DataType(FloatType.Default)
        .Build();
        private float _maxTxPower;
        public float MaxTxPower { get => _maxTxPower; set => _maxTxPower = value; }
        /// <summary>
        /// Min TX power in dBm.
        /// OriginName: min_tx_power, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MinTxPowerField = new Field.Builder()
            .Name(nameof(MinTxPower))
            .Title("min_tx_power")
            .Description("Min TX power in dBm.")

            .DataType(FloatType.Default)
        .Build();
        private float _minTxPower;
        public float MinTxPower { get => _minTxPower; set => _minTxPower = value; }
        /// <summary>
        /// Max estimated RX power in dBm.
        /// OriginName: max_rx_power, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MaxRxPowerField = new Field.Builder()
            .Name(nameof(MaxRxPower))
            .Title("max_rx_power")
            .Description("Max estimated RX power in dBm.")

            .DataType(FloatType.Default)
        .Build();
        private float _maxRxPower;
        public float MaxRxPower { get => _maxRxPower; set => _maxRxPower = value; }
        /// <summary>
        /// Min estimated RX power in dBm.
        /// OriginName: min_rx_power, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MinRxPowerField = new Field.Builder()
            .Name(nameof(MinRxPower))
            .Title("min_rx_power")
            .Description("Min estimated RX power in dBm.")

            .DataType(FloatType.Default)
        .Build();
        private float _minRxPower;
        public float MinRxPower { get => _minRxPower; set => _minRxPower = value; }
        /// <summary>
        /// Supported RF modulations. Each bit of array is flag from ASV_RADIO_MODULATION(max 255 items) enum.
        /// OriginName: supported_modulation, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SupportedModulationField = new Field.Builder()
            .Name(nameof(SupportedModulation))
            .Title("supported_modulation")
            .Description("Supported RF modulations. Each bit of array is flag from ASV_RADIO_MODULATION(max 255 items) enum.")

            .DataType(new ArrayType(UInt8Type.Default,32))
        .Build();
        public const int SupportedModulationMaxItemsCount = 32;
        public byte[] SupportedModulation { get; } = new byte[32];
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t skip
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t count
            );
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

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,SkipField, SkipField.DataType, ref _skip);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    
            UInt8Type.Accept(visitor,CountField, CountField.DataType, ref _count);    

        }

        /// <summary>
        /// Skip index.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SkipField = new Field.Builder()
            .Name(nameof(Skip))
            .Title("skip")
            .Description("Skip index.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _skip;
        public ushort Skip { get => _skip; set => _skip = value; }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetSystem;
        public byte TargetSystem { get => _targetSystem; set => _targetSystem = value; }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetComponent;
        public byte TargetComponent { get => _targetComponent; set => _targetComponent = value; }
        /// <summary>
        /// Codec count at ASV_RADIO_CODEC_CAPABILITIES_RESPONSE.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Codec count at ASV_RADIO_CODEC_CAPABILITIES_RESPONSE.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _count;
        public byte Count { get => _count; set => _count = value; }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t all
            +2 // uint16_t skip
            + Codecs.Length * 2 // uint16_t[100] codecs
            
            +1 // uint8_t count
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            All = BinSerialize.ReadUShort(ref buffer);
            Skip = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/100 - Math.Max(0,((/*PayloadByteSize*/205 - payloadSize - /*ExtendedFieldsLength*/0)/*FieldTypeByteSize*/ /2));
            
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

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,AllField, AllField.DataType, ref _all);    
            UInt16Type.Accept(visitor,SkipField, SkipField.DataType, ref _skip);    
            ArrayType.Accept(visitor,CodecsField, CodecsField.DataType, 100, (index, v, f, t) =>
            {
                var tmp = (ushort)Codecs[index];
                UInt16Type.Accept(v, f, t, ref tmp);
                Codecs[index] = (AsvAudioCodec)tmp;
            });
            UInt8Type.Accept(visitor,CountField, CountField.DataType, ref _count);    

        }

        /// <summary>
        /// All codec codecs.
        /// OriginName: all, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AllField = new Field.Builder()
            .Name(nameof(All))
            .Title("all")
            .Description("All codec codecs.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _all;
        public ushort All { get => _all; set => _all = value; }
        /// <summary>
        /// Skip index codec.
        /// OriginName: skip, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SkipField = new Field.Builder()
            .Name(nameof(Skip))
            .Title("skip")
            .Description("Skip index codec.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _skip;
        public ushort Skip { get => _skip; set => _skip = value; }
        /// <summary>
        /// Supported codec array.
        /// OriginName: codecs, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CodecsField = new Field.Builder()
            .Name(nameof(Codecs))
            .Title("codecs")
            .Description("Supported codec array.")
            .DataType(new ArrayType(new UInt16Type(AsvAudioCodecHelper.GetValues(x=>(ushort)x).Min(),AsvAudioCodecHelper.GetValues(x=>(ushort)x).Max()),100))
            .Enum(AsvAudioCodecHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        public const int CodecsMaxItemsCount = 100;    
        public AsvAudioCodec[] Codecs { get; } = new AsvAudioCodec[100];
        /// <summary>
        /// Array size.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Array size.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _count;
        public byte Count { get => _count; set => _count = value; }
    }




        


#endregion


}

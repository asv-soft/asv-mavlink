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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.14+613eac956231b473246c80e7d407c06ce1728417 25-04-26.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.IO;

namespace Asv.Mavlink.Asluav
{

    public static class AsluavHelper
    {
        public static void RegisterAsluavDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(CommandIntStampedPacket.MessageId, ()=>new CommandIntStampedPacket());
            src.Add(CommandLongStampedPacket.MessageId, ()=>new CommandLongStampedPacket());
            src.Add(SensPowerPacket.MessageId, ()=>new SensPowerPacket());
            src.Add(SensMpptPacket.MessageId, ()=>new SensMpptPacket());
            src.Add(AslctrlDataPacket.MessageId, ()=>new AslctrlDataPacket());
            src.Add(AslctrlDebugPacket.MessageId, ()=>new AslctrlDebugPacket());
            src.Add(AsluavStatusPacket.MessageId, ()=>new AsluavStatusPacket());
            src.Add(EkfExtPacket.MessageId, ()=>new EkfExtPacket());
            src.Add(AslObctrlPacket.MessageId, ()=>new AslObctrlPacket());
            src.Add(SensAtmosPacket.MessageId, ()=>new SensAtmosPacket());
            src.Add(SensBatmonPacket.MessageId, ()=>new SensBatmonPacket());
            src.Add(FwSoaringDataPacket.MessageId, ()=>new FwSoaringDataPacket());
            src.Add(SensorpodStatusPacket.MessageId, ()=>new SensorpodStatusPacket());
            src.Add(SensPowerBoardPacket.MessageId, ()=>new SensPowerBoardPacket());
            src.Add(GsmLinkStatusPacket.MessageId, ()=>new GsmLinkStatusPacket());
            src.Add(SatcomLinkStatusPacket.MessageId, ()=>new SatcomLinkStatusPacket());
            src.Add(SensorAirflowAnglesPacket.MessageId, ()=>new SensorAirflowAnglesPacket());
        }
    }

#region Enums

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Mission command to reset Maximum Power Point Tracker (MPPT)
        /// Param 1 - MPPT number
        /// Param 2 - Empty
        /// Param 3 - Empty
        /// Param 4 - Empty
        /// Param 5 - Empty
        /// Param 6 - Empty
        /// Param 7 - Empty
        /// MAV_CMD_RESET_MPPT
        /// </summary>
        MavCmdResetMppt = 40001,
        /// <summary>
        /// Mission command to perform a power cycle on payload
        /// Param 1 - Complete power cycle
        /// Param 2 - VISensor power cycle
        /// Param 3 - Empty
        /// Param 4 - Empty
        /// Param 5 - Empty
        /// Param 6 - Empty
        /// Param 7 - Empty
        /// MAV_CMD_PAYLOAD_CONTROL
        /// </summary>
        MavCmdPayloadControl = 40002,
    }

    /// <summary>
    ///  GSM_LINK_TYPE
    /// </summary>
    public enum GsmLinkType:uint
    {
        /// <summary>
        /// no service
        /// GSM_LINK_TYPE_NONE
        /// </summary>
        GsmLinkTypeNone = 0,
        /// <summary>
        /// link type unknown
        /// GSM_LINK_TYPE_UNKNOWN
        /// </summary>
        GsmLinkTypeUnknown = 1,
        /// <summary>
        /// 2G (GSM/GRPS/EDGE) link
        /// GSM_LINK_TYPE_2G
        /// </summary>
        GsmLinkType2g = 2,
        /// <summary>
        /// 3G link (WCDMA/HSDPA/HSPA) 
        /// GSM_LINK_TYPE_3G
        /// </summary>
        GsmLinkType3g = 3,
        /// <summary>
        /// 4G link (LTE)
        /// GSM_LINK_TYPE_4G
        /// </summary>
        GsmLinkType4g = 4,
    }

    /// <summary>
    ///  GSM_MODEM_TYPE
    /// </summary>
    public enum GsmModemType:uint
    {
        /// <summary>
        /// not specified
        /// GSM_MODEM_TYPE_UNKNOWN
        /// </summary>
        GsmModemTypeUnknown = 0,
        /// <summary>
        /// HUAWEI LTE USB Stick E3372
        /// GSM_MODEM_TYPE_HUAWEI_E3372
        /// </summary>
        GsmModemTypeHuaweiE3372 = 1,
    }


#endregion

#region Messages

    /// <summary>
    /// Message encoding a command with parameters as scaled integers and additional metadata. Scaling depends on the actual command value.
    ///  COMMAND_INT_STAMPED
    /// </summary>
    public class CommandIntStampedPacket : MavlinkV2Message<CommandIntStampedPayload>
    {
        public const int MessageId = 223;
        
        public const byte CrcExtra = 119;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override CommandIntStampedPayload Payload { get; } = new();

        public override string Name => "COMMAND_INT_STAMPED";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("vehicle_timestamp",
            "Microseconds elapsed since vehicle boot",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("utc_time",
            "UTC time, seconds elapsed since 01.01.1970",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new("param1",
            "PARAM1, see MAV_CMD enum",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("param2",
            "PARAM2, see MAV_CMD enum",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("param3",
            "PARAM3, see MAV_CMD enum",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("param4",
            "PARAM4, see MAV_CMD enum",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("x",
            "PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new("y",
            "PARAM6 / local: y position in meters * 1e4, global: longitude in degrees * 10^7",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new("z",
            "PARAM7 / z position: global: altitude in meters (MSL, WGS84, AGL or relative to home - depending on frame).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("command",
            "The scheduled action for the mission item, as defined by MAV_CMD enum",
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
            new("frame",
            "The coordinate system of the COMMAND, as defined by MAV_FRAME enum",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("current",
            "false:0, true:1",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("autocontinue",
            "autocontinue to next wp",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "COMMAND_INT_STAMPED:"
        + "uint64_t vehicle_timestamp;"
        + "uint32_t utc_time;"
        + "float param1;"
        + "float param2;"
        + "float param3;"
        + "float param4;"
        + "int32_t x;"
        + "int32_t y;"
        + "float z;"
        + "uint16_t command;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t frame;"
        + "uint8_t current;"
        + "uint8_t autocontinue;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  COMMAND_INT_STAMPED
    /// </summary>
    public class CommandIntStampedPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 47; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 47; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //VehicleTimestamp
            sum+=4; //UtcTime
            sum+=4; //Param1
            sum+=4; //Param2
            sum+=4; //Param3
            sum+=4; //Param4
            sum+=4; //X
            sum+=4; //Y
            sum+=4; //Z
            sum+= 2; // Command
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+= 1; // Frame
            sum+=1; //Current
            sum+=1; //Autocontinue
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            VehicleTimestamp = BinSerialize.ReadULong(ref buffer);
            UtcTime = BinSerialize.ReadUInt(ref buffer);
            Param1 = BinSerialize.ReadFloat(ref buffer);
            Param2 = BinSerialize.ReadFloat(ref buffer);
            Param3 = BinSerialize.ReadFloat(ref buffer);
            Param4 = BinSerialize.ReadFloat(ref buffer);
            X = BinSerialize.ReadInt(ref buffer);
            Y = BinSerialize.ReadInt(ref buffer);
            Z = BinSerialize.ReadFloat(ref buffer);
            Command = (MavCmd)BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Frame = (MavFrame)BinSerialize.ReadByte(ref buffer);
            Current = (byte)BinSerialize.ReadByte(ref buffer);
            Autocontinue = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,VehicleTimestamp);
            BinSerialize.WriteUInt(ref buffer,UtcTime);
            BinSerialize.WriteFloat(ref buffer,Param1);
            BinSerialize.WriteFloat(ref buffer,Param2);
            BinSerialize.WriteFloat(ref buffer,Param3);
            BinSerialize.WriteFloat(ref buffer,Param4);
            BinSerialize.WriteInt(ref buffer,X);
            BinSerialize.WriteInt(ref buffer,Y);
            BinSerialize.WriteFloat(ref buffer,Z);
            BinSerialize.WriteUShort(ref buffer,(ushort)Command);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Frame);
            BinSerialize.WriteByte(ref buffer,(byte)Current);
            BinSerialize.WriteByte(ref buffer,(byte)Autocontinue);
            /* PayloadByteSize = 47 */;
        }
        
        



        /// <summary>
        /// Microseconds elapsed since vehicle boot
        /// OriginName: vehicle_timestamp, Units: , IsExtended: false
        /// </summary>
        public ulong VehicleTimestamp { get; set; }
        /// <summary>
        /// UTC time, seconds elapsed since 01.01.1970
        /// OriginName: utc_time, Units: , IsExtended: false
        /// </summary>
        public uint UtcTime { get; set; }
        /// <summary>
        /// PARAM1, see MAV_CMD enum
        /// OriginName: param1, Units: , IsExtended: false
        /// </summary>
        public float Param1 { get; set; }
        /// <summary>
        /// PARAM2, see MAV_CMD enum
        /// OriginName: param2, Units: , IsExtended: false
        /// </summary>
        public float Param2 { get; set; }
        /// <summary>
        /// PARAM3, see MAV_CMD enum
        /// OriginName: param3, Units: , IsExtended: false
        /// </summary>
        public float Param3 { get; set; }
        /// <summary>
        /// PARAM4, see MAV_CMD enum
        /// OriginName: param4, Units: , IsExtended: false
        /// </summary>
        public float Param4 { get; set; }
        /// <summary>
        /// PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7
        /// OriginName: x, Units: , IsExtended: false
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// PARAM6 / local: y position in meters * 1e4, global: longitude in degrees * 10^7
        /// OriginName: y, Units: , IsExtended: false
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// PARAM7 / z position: global: altitude in meters (MSL, WGS84, AGL or relative to home - depending on frame).
        /// OriginName: z, Units: , IsExtended: false
        /// </summary>
        public float Z { get; set; }
        /// <summary>
        /// The scheduled action for the mission item, as defined by MAV_CMD enum
        /// OriginName: command, Units: , IsExtended: false
        /// </summary>
        public MavCmd Command { get; set; }
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
        /// The coordinate system of the COMMAND, as defined by MAV_FRAME enum
        /// OriginName: frame, Units: , IsExtended: false
        /// </summary>
        public MavFrame Frame { get; set; }
        /// <summary>
        /// false:0, true:1
        /// OriginName: current, Units: , IsExtended: false
        /// </summary>
        public byte Current { get; set; }
        /// <summary>
        /// autocontinue to next wp
        /// OriginName: autocontinue, Units: , IsExtended: false
        /// </summary>
        public byte Autocontinue { get; set; }
    }
    /// <summary>
    /// Send a command with up to seven parameters to the MAV and additional metadata
    ///  COMMAND_LONG_STAMPED
    /// </summary>
    public class CommandLongStampedPacket : MavlinkV2Message<CommandLongStampedPayload>
    {
        public const int MessageId = 224;
        
        public const byte CrcExtra = 102;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override CommandLongStampedPayload Payload { get; } = new();

        public override string Name => "COMMAND_LONG_STAMPED";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("vehicle_timestamp",
            "Microseconds elapsed since vehicle boot",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("utc_time",
            "UTC time, seconds elapsed since 01.01.1970",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new("param1",
            "Parameter 1, as defined by MAV_CMD enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("param2",
            "Parameter 2, as defined by MAV_CMD enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("param3",
            "Parameter 3, as defined by MAV_CMD enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("param4",
            "Parameter 4, as defined by MAV_CMD enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("param5",
            "Parameter 5, as defined by MAV_CMD enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("param6",
            "Parameter 6, as defined by MAV_CMD enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("param7",
            "Parameter 7, as defined by MAV_CMD enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("command",
            "Command ID, as defined by MAV_CMD enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("target_system",
            "System which should execute the command",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("target_component",
            "Component which should execute the command, 0 for all components",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("confirmation",
            "0: First transmission of this command. 1-255: Confirmation transmissions (e.g. for kill command)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "COMMAND_LONG_STAMPED:"
        + "uint64_t vehicle_timestamp;"
        + "uint32_t utc_time;"
        + "float param1;"
        + "float param2;"
        + "float param3;"
        + "float param4;"
        + "float param5;"
        + "float param6;"
        + "float param7;"
        + "uint16_t command;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t confirmation;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  COMMAND_LONG_STAMPED
    /// </summary>
    public class CommandLongStampedPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 45; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 45; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //VehicleTimestamp
            sum+=4; //UtcTime
            sum+=4; //Param1
            sum+=4; //Param2
            sum+=4; //Param3
            sum+=4; //Param4
            sum+=4; //Param5
            sum+=4; //Param6
            sum+=4; //Param7
            sum+= 2; // Command
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //Confirmation
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            VehicleTimestamp = BinSerialize.ReadULong(ref buffer);
            UtcTime = BinSerialize.ReadUInt(ref buffer);
            Param1 = BinSerialize.ReadFloat(ref buffer);
            Param2 = BinSerialize.ReadFloat(ref buffer);
            Param3 = BinSerialize.ReadFloat(ref buffer);
            Param4 = BinSerialize.ReadFloat(ref buffer);
            Param5 = BinSerialize.ReadFloat(ref buffer);
            Param6 = BinSerialize.ReadFloat(ref buffer);
            Param7 = BinSerialize.ReadFloat(ref buffer);
            Command = (MavCmd)BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Confirmation = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,VehicleTimestamp);
            BinSerialize.WriteUInt(ref buffer,UtcTime);
            BinSerialize.WriteFloat(ref buffer,Param1);
            BinSerialize.WriteFloat(ref buffer,Param2);
            BinSerialize.WriteFloat(ref buffer,Param3);
            BinSerialize.WriteFloat(ref buffer,Param4);
            BinSerialize.WriteFloat(ref buffer,Param5);
            BinSerialize.WriteFloat(ref buffer,Param6);
            BinSerialize.WriteFloat(ref buffer,Param7);
            BinSerialize.WriteUShort(ref buffer,(ushort)Command);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Confirmation);
            /* PayloadByteSize = 45 */;
        }
        
        



        /// <summary>
        /// Microseconds elapsed since vehicle boot
        /// OriginName: vehicle_timestamp, Units: , IsExtended: false
        /// </summary>
        public ulong VehicleTimestamp { get; set; }
        /// <summary>
        /// UTC time, seconds elapsed since 01.01.1970
        /// OriginName: utc_time, Units: , IsExtended: false
        /// </summary>
        public uint UtcTime { get; set; }
        /// <summary>
        /// Parameter 1, as defined by MAV_CMD enum.
        /// OriginName: param1, Units: , IsExtended: false
        /// </summary>
        public float Param1 { get; set; }
        /// <summary>
        /// Parameter 2, as defined by MAV_CMD enum.
        /// OriginName: param2, Units: , IsExtended: false
        /// </summary>
        public float Param2 { get; set; }
        /// <summary>
        /// Parameter 3, as defined by MAV_CMD enum.
        /// OriginName: param3, Units: , IsExtended: false
        /// </summary>
        public float Param3 { get; set; }
        /// <summary>
        /// Parameter 4, as defined by MAV_CMD enum.
        /// OriginName: param4, Units: , IsExtended: false
        /// </summary>
        public float Param4 { get; set; }
        /// <summary>
        /// Parameter 5, as defined by MAV_CMD enum.
        /// OriginName: param5, Units: , IsExtended: false
        /// </summary>
        public float Param5 { get; set; }
        /// <summary>
        /// Parameter 6, as defined by MAV_CMD enum.
        /// OriginName: param6, Units: , IsExtended: false
        /// </summary>
        public float Param6 { get; set; }
        /// <summary>
        /// Parameter 7, as defined by MAV_CMD enum.
        /// OriginName: param7, Units: , IsExtended: false
        /// </summary>
        public float Param7 { get; set; }
        /// <summary>
        /// Command ID, as defined by MAV_CMD enum.
        /// OriginName: command, Units: , IsExtended: false
        /// </summary>
        public MavCmd Command { get; set; }
        /// <summary>
        /// System which should execute the command
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component which should execute the command, 0 for all components
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// 0: First transmission of this command. 1-255: Confirmation transmissions (e.g. for kill command)
        /// OriginName: confirmation, Units: , IsExtended: false
        /// </summary>
        public byte Confirmation { get; set; }
    }
    /// <summary>
    /// Voltage and current sensor data
    ///  SENS_POWER
    /// </summary>
    public class SensPowerPacket : MavlinkV2Message<SensPowerPayload>
    {
        public const int MessageId = 8002;
        
        public const byte CrcExtra = 218;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SensPowerPayload Payload { get; } = new();

        public override string Name => "SENS_POWER";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("adc121_vspb_volt",
            " Power board voltage sensor reading",
            string.Empty, 
            @"V", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("adc121_cspb_amp",
            " Power board current sensor reading",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("adc121_cs1_amp",
            " Board current sensor 1 reading",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("adc121_cs2_amp",
            " Board current sensor 2 reading",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
        ];
        public const string FormatMessage = "SENS_POWER:"
        + "float adc121_vspb_volt;"
        + "float adc121_cspb_amp;"
        + "float adc121_cs1_amp;"
        + "float adc121_cs2_amp;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  SENS_POWER
    /// </summary>
    public class SensPowerPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 16; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Adc121VspbVolt
            sum+=4; //Adc121CspbAmp
            sum+=4; //Adc121Cs1Amp
            sum+=4; //Adc121Cs2Amp
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Adc121VspbVolt = BinSerialize.ReadFloat(ref buffer);
            Adc121CspbAmp = BinSerialize.ReadFloat(ref buffer);
            Adc121Cs1Amp = BinSerialize.ReadFloat(ref buffer);
            Adc121Cs2Amp = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Adc121VspbVolt);
            BinSerialize.WriteFloat(ref buffer,Adc121CspbAmp);
            BinSerialize.WriteFloat(ref buffer,Adc121Cs1Amp);
            BinSerialize.WriteFloat(ref buffer,Adc121Cs2Amp);
            /* PayloadByteSize = 16 */;
        }
        
        



        /// <summary>
        ///  Power board voltage sensor reading
        /// OriginName: adc121_vspb_volt, Units: V, IsExtended: false
        /// </summary>
        public float Adc121VspbVolt { get; set; }
        /// <summary>
        ///  Power board current sensor reading
        /// OriginName: adc121_cspb_amp, Units: A, IsExtended: false
        /// </summary>
        public float Adc121CspbAmp { get; set; }
        /// <summary>
        ///  Board current sensor 1 reading
        /// OriginName: adc121_cs1_amp, Units: A, IsExtended: false
        /// </summary>
        public float Adc121Cs1Amp { get; set; }
        /// <summary>
        ///  Board current sensor 2 reading
        /// OriginName: adc121_cs2_amp, Units: A, IsExtended: false
        /// </summary>
        public float Adc121Cs2Amp { get; set; }
    }
    /// <summary>
    /// Maximum Power Point Tracker (MPPT) sensor data for solar module power performance tracking
    ///  SENS_MPPT
    /// </summary>
    public class SensMpptPacket : MavlinkV2Message<SensMpptPayload>
    {
        public const int MessageId = 8003;
        
        public const byte CrcExtra = 231;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SensMpptPayload Payload { get; } = new();

        public override string Name => "SENS_MPPT";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("mppt_timestamp",
            " MPPT last timestamp ",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("mppt1_volt",
            " MPPT1 voltage ",
            string.Empty, 
            @"V", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("mppt1_amp",
            " MPPT1 current ",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("mppt2_volt",
            " MPPT2 voltage ",
            string.Empty, 
            @"V", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("mppt2_amp",
            " MPPT2 current ",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("mppt3_volt",
            "MPPT3 voltage ",
            string.Empty, 
            @"V", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("mppt3_amp",
            " MPPT3 current ",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("mppt1_pwm",
            " MPPT1 pwm ",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("mppt2_pwm",
            " MPPT2 pwm ",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("mppt3_pwm",
            " MPPT3 pwm ",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("mppt1_status",
            " MPPT1 status ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("mppt2_status",
            " MPPT2 status ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("mppt3_status",
            " MPPT3 status ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "SENS_MPPT:"
        + "uint64_t mppt_timestamp;"
        + "float mppt1_volt;"
        + "float mppt1_amp;"
        + "float mppt2_volt;"
        + "float mppt2_amp;"
        + "float mppt3_volt;"
        + "float mppt3_amp;"
        + "uint16_t mppt1_pwm;"
        + "uint16_t mppt2_pwm;"
        + "uint16_t mppt3_pwm;"
        + "uint8_t mppt1_status;"
        + "uint8_t mppt2_status;"
        + "uint8_t mppt3_status;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  SENS_MPPT
    /// </summary>
    public class SensMpptPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 41; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 41; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //MpptTimestamp
            sum+=4; //Mppt1Volt
            sum+=4; //Mppt1Amp
            sum+=4; //Mppt2Volt
            sum+=4; //Mppt2Amp
            sum+=4; //Mppt3Volt
            sum+=4; //Mppt3Amp
            sum+=2; //Mppt1Pwm
            sum+=2; //Mppt2Pwm
            sum+=2; //Mppt3Pwm
            sum+=1; //Mppt1Status
            sum+=1; //Mppt2Status
            sum+=1; //Mppt3Status
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            MpptTimestamp = BinSerialize.ReadULong(ref buffer);
            Mppt1Volt = BinSerialize.ReadFloat(ref buffer);
            Mppt1Amp = BinSerialize.ReadFloat(ref buffer);
            Mppt2Volt = BinSerialize.ReadFloat(ref buffer);
            Mppt2Amp = BinSerialize.ReadFloat(ref buffer);
            Mppt3Volt = BinSerialize.ReadFloat(ref buffer);
            Mppt3Amp = BinSerialize.ReadFloat(ref buffer);
            Mppt1Pwm = BinSerialize.ReadUShort(ref buffer);
            Mppt2Pwm = BinSerialize.ReadUShort(ref buffer);
            Mppt3Pwm = BinSerialize.ReadUShort(ref buffer);
            Mppt1Status = (byte)BinSerialize.ReadByte(ref buffer);
            Mppt2Status = (byte)BinSerialize.ReadByte(ref buffer);
            Mppt3Status = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,MpptTimestamp);
            BinSerialize.WriteFloat(ref buffer,Mppt1Volt);
            BinSerialize.WriteFloat(ref buffer,Mppt1Amp);
            BinSerialize.WriteFloat(ref buffer,Mppt2Volt);
            BinSerialize.WriteFloat(ref buffer,Mppt2Amp);
            BinSerialize.WriteFloat(ref buffer,Mppt3Volt);
            BinSerialize.WriteFloat(ref buffer,Mppt3Amp);
            BinSerialize.WriteUShort(ref buffer,Mppt1Pwm);
            BinSerialize.WriteUShort(ref buffer,Mppt2Pwm);
            BinSerialize.WriteUShort(ref buffer,Mppt3Pwm);
            BinSerialize.WriteByte(ref buffer,(byte)Mppt1Status);
            BinSerialize.WriteByte(ref buffer,(byte)Mppt2Status);
            BinSerialize.WriteByte(ref buffer,(byte)Mppt3Status);
            /* PayloadByteSize = 41 */;
        }
        
        



        /// <summary>
        ///  MPPT last timestamp 
        /// OriginName: mppt_timestamp, Units: us, IsExtended: false
        /// </summary>
        public ulong MpptTimestamp { get; set; }
        /// <summary>
        ///  MPPT1 voltage 
        /// OriginName: mppt1_volt, Units: V, IsExtended: false
        /// </summary>
        public float Mppt1Volt { get; set; }
        /// <summary>
        ///  MPPT1 current 
        /// OriginName: mppt1_amp, Units: A, IsExtended: false
        /// </summary>
        public float Mppt1Amp { get; set; }
        /// <summary>
        ///  MPPT2 voltage 
        /// OriginName: mppt2_volt, Units: V, IsExtended: false
        /// </summary>
        public float Mppt2Volt { get; set; }
        /// <summary>
        ///  MPPT2 current 
        /// OriginName: mppt2_amp, Units: A, IsExtended: false
        /// </summary>
        public float Mppt2Amp { get; set; }
        /// <summary>
        /// MPPT3 voltage 
        /// OriginName: mppt3_volt, Units: V, IsExtended: false
        /// </summary>
        public float Mppt3Volt { get; set; }
        /// <summary>
        ///  MPPT3 current 
        /// OriginName: mppt3_amp, Units: A, IsExtended: false
        /// </summary>
        public float Mppt3Amp { get; set; }
        /// <summary>
        ///  MPPT1 pwm 
        /// OriginName: mppt1_pwm, Units: us, IsExtended: false
        /// </summary>
        public ushort Mppt1Pwm { get; set; }
        /// <summary>
        ///  MPPT2 pwm 
        /// OriginName: mppt2_pwm, Units: us, IsExtended: false
        /// </summary>
        public ushort Mppt2Pwm { get; set; }
        /// <summary>
        ///  MPPT3 pwm 
        /// OriginName: mppt3_pwm, Units: us, IsExtended: false
        /// </summary>
        public ushort Mppt3Pwm { get; set; }
        /// <summary>
        ///  MPPT1 status 
        /// OriginName: mppt1_status, Units: , IsExtended: false
        /// </summary>
        public byte Mppt1Status { get; set; }
        /// <summary>
        ///  MPPT2 status 
        /// OriginName: mppt2_status, Units: , IsExtended: false
        /// </summary>
        public byte Mppt2Status { get; set; }
        /// <summary>
        ///  MPPT3 status 
        /// OriginName: mppt3_status, Units: , IsExtended: false
        /// </summary>
        public byte Mppt3Status { get; set; }
    }
    /// <summary>
    /// ASL-fixed-wing controller data
    ///  ASLCTRL_DATA
    /// </summary>
    public class AslctrlDataPacket : MavlinkV2Message<AslctrlDataPayload>
    {
        public const int MessageId = 8004;
        
        public const byte CrcExtra = 172;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AslctrlDataPayload Payload { get; } = new();

        public override string Name => "ASLCTRL_DATA";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("timestamp",
            " Timestamp",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("h",
            " See sourcecode for a description of these values... ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("hRef",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("hRef_t",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("PitchAngle",
            "Pitch angle",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("PitchAngleRef",
            "Pitch angle reference",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("q",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("qRef",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("uElev",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("uThrot",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("uThrot2",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("nZ",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("AirspeedRef",
            "Airspeed reference",
            string.Empty, 
            @"m/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("YawAngle",
            "Yaw angle",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("YawAngleRef",
            "Yaw angle reference",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("RollAngle",
            "Roll angle",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("RollAngleRef",
            "Roll angle reference",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("p",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("pRef",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("r",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("rRef",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("uAil",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("uRud",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("aslctrl_mode",
            " ASLCTRL control-mode (manual, stabilized, auto, etc...)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("SpoilersEngaged",
            " ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASLCTRL_DATA:"
        + "uint64_t timestamp;"
        + "float h;"
        + "float hRef;"
        + "float hRef_t;"
        + "float PitchAngle;"
        + "float PitchAngleRef;"
        + "float q;"
        + "float qRef;"
        + "float uElev;"
        + "float uThrot;"
        + "float uThrot2;"
        + "float nZ;"
        + "float AirspeedRef;"
        + "float YawAngle;"
        + "float YawAngleRef;"
        + "float RollAngle;"
        + "float RollAngleRef;"
        + "float p;"
        + "float pRef;"
        + "float r;"
        + "float rRef;"
        + "float uAil;"
        + "float uRud;"
        + "uint8_t aslctrl_mode;"
        + "uint8_t SpoilersEngaged;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  ASLCTRL_DATA
    /// </summary>
    public class AslctrlDataPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 98; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 98; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Timestamp
            sum+=4; //H
            sum+=4; //Href
            sum+=4; //HrefT
            sum+=4; //Pitchangle
            sum+=4; //Pitchangleref
            sum+=4; //Q
            sum+=4; //Qref
            sum+=4; //Uelev
            sum+=4; //Uthrot
            sum+=4; //Uthrot2
            sum+=4; //Nz
            sum+=4; //Airspeedref
            sum+=4; //Yawangle
            sum+=4; //Yawangleref
            sum+=4; //Rollangle
            sum+=4; //Rollangleref
            sum+=4; //P
            sum+=4; //Pref
            sum+=4; //R
            sum+=4; //Rref
            sum+=4; //Uail
            sum+=4; //Urud
            sum+=1; //AslctrlMode
            sum+=1; //Spoilersengaged
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Timestamp = BinSerialize.ReadULong(ref buffer);
            H = BinSerialize.ReadFloat(ref buffer);
            Href = BinSerialize.ReadFloat(ref buffer);
            HrefT = BinSerialize.ReadFloat(ref buffer);
            Pitchangle = BinSerialize.ReadFloat(ref buffer);
            Pitchangleref = BinSerialize.ReadFloat(ref buffer);
            Q = BinSerialize.ReadFloat(ref buffer);
            Qref = BinSerialize.ReadFloat(ref buffer);
            Uelev = BinSerialize.ReadFloat(ref buffer);
            Uthrot = BinSerialize.ReadFloat(ref buffer);
            Uthrot2 = BinSerialize.ReadFloat(ref buffer);
            Nz = BinSerialize.ReadFloat(ref buffer);
            Airspeedref = BinSerialize.ReadFloat(ref buffer);
            Yawangle = BinSerialize.ReadFloat(ref buffer);
            Yawangleref = BinSerialize.ReadFloat(ref buffer);
            Rollangle = BinSerialize.ReadFloat(ref buffer);
            Rollangleref = BinSerialize.ReadFloat(ref buffer);
            P = BinSerialize.ReadFloat(ref buffer);
            Pref = BinSerialize.ReadFloat(ref buffer);
            R = BinSerialize.ReadFloat(ref buffer);
            Rref = BinSerialize.ReadFloat(ref buffer);
            Uail = BinSerialize.ReadFloat(ref buffer);
            Urud = BinSerialize.ReadFloat(ref buffer);
            AslctrlMode = (byte)BinSerialize.ReadByte(ref buffer);
            Spoilersengaged = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Timestamp);
            BinSerialize.WriteFloat(ref buffer,H);
            BinSerialize.WriteFloat(ref buffer,Href);
            BinSerialize.WriteFloat(ref buffer,HrefT);
            BinSerialize.WriteFloat(ref buffer,Pitchangle);
            BinSerialize.WriteFloat(ref buffer,Pitchangleref);
            BinSerialize.WriteFloat(ref buffer,Q);
            BinSerialize.WriteFloat(ref buffer,Qref);
            BinSerialize.WriteFloat(ref buffer,Uelev);
            BinSerialize.WriteFloat(ref buffer,Uthrot);
            BinSerialize.WriteFloat(ref buffer,Uthrot2);
            BinSerialize.WriteFloat(ref buffer,Nz);
            BinSerialize.WriteFloat(ref buffer,Airspeedref);
            BinSerialize.WriteFloat(ref buffer,Yawangle);
            BinSerialize.WriteFloat(ref buffer,Yawangleref);
            BinSerialize.WriteFloat(ref buffer,Rollangle);
            BinSerialize.WriteFloat(ref buffer,Rollangleref);
            BinSerialize.WriteFloat(ref buffer,P);
            BinSerialize.WriteFloat(ref buffer,Pref);
            BinSerialize.WriteFloat(ref buffer,R);
            BinSerialize.WriteFloat(ref buffer,Rref);
            BinSerialize.WriteFloat(ref buffer,Uail);
            BinSerialize.WriteFloat(ref buffer,Urud);
            BinSerialize.WriteByte(ref buffer,(byte)AslctrlMode);
            BinSerialize.WriteByte(ref buffer,(byte)Spoilersengaged);
            /* PayloadByteSize = 98 */;
        }
        
        



        /// <summary>
        ///  Timestamp
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public ulong Timestamp { get; set; }
        /// <summary>
        ///  See sourcecode for a description of these values... 
        /// OriginName: h, Units: , IsExtended: false
        /// </summary>
        public float H { get; set; }
        /// <summary>
        ///  
        /// OriginName: hRef, Units: , IsExtended: false
        /// </summary>
        public float Href { get; set; }
        /// <summary>
        ///  
        /// OriginName: hRef_t, Units: , IsExtended: false
        /// </summary>
        public float HrefT { get; set; }
        /// <summary>
        /// Pitch angle
        /// OriginName: PitchAngle, Units: deg, IsExtended: false
        /// </summary>
        public float Pitchangle { get; set; }
        /// <summary>
        /// Pitch angle reference
        /// OriginName: PitchAngleRef, Units: deg, IsExtended: false
        /// </summary>
        public float Pitchangleref { get; set; }
        /// <summary>
        ///  
        /// OriginName: q, Units: , IsExtended: false
        /// </summary>
        public float Q { get; set; }
        /// <summary>
        ///  
        /// OriginName: qRef, Units: , IsExtended: false
        /// </summary>
        public float Qref { get; set; }
        /// <summary>
        ///  
        /// OriginName: uElev, Units: , IsExtended: false
        /// </summary>
        public float Uelev { get; set; }
        /// <summary>
        ///  
        /// OriginName: uThrot, Units: , IsExtended: false
        /// </summary>
        public float Uthrot { get; set; }
        /// <summary>
        ///  
        /// OriginName: uThrot2, Units: , IsExtended: false
        /// </summary>
        public float Uthrot2 { get; set; }
        /// <summary>
        ///  
        /// OriginName: nZ, Units: , IsExtended: false
        /// </summary>
        public float Nz { get; set; }
        /// <summary>
        /// Airspeed reference
        /// OriginName: AirspeedRef, Units: m/s, IsExtended: false
        /// </summary>
        public float Airspeedref { get; set; }
        /// <summary>
        /// Yaw angle
        /// OriginName: YawAngle, Units: deg, IsExtended: false
        /// </summary>
        public float Yawangle { get; set; }
        /// <summary>
        /// Yaw angle reference
        /// OriginName: YawAngleRef, Units: deg, IsExtended: false
        /// </summary>
        public float Yawangleref { get; set; }
        /// <summary>
        /// Roll angle
        /// OriginName: RollAngle, Units: deg, IsExtended: false
        /// </summary>
        public float Rollangle { get; set; }
        /// <summary>
        /// Roll angle reference
        /// OriginName: RollAngleRef, Units: deg, IsExtended: false
        /// </summary>
        public float Rollangleref { get; set; }
        /// <summary>
        ///  
        /// OriginName: p, Units: , IsExtended: false
        /// </summary>
        public float P { get; set; }
        /// <summary>
        ///  
        /// OriginName: pRef, Units: , IsExtended: false
        /// </summary>
        public float Pref { get; set; }
        /// <summary>
        ///  
        /// OriginName: r, Units: , IsExtended: false
        /// </summary>
        public float R { get; set; }
        /// <summary>
        ///  
        /// OriginName: rRef, Units: , IsExtended: false
        /// </summary>
        public float Rref { get; set; }
        /// <summary>
        ///  
        /// OriginName: uAil, Units: , IsExtended: false
        /// </summary>
        public float Uail { get; set; }
        /// <summary>
        ///  
        /// OriginName: uRud, Units: , IsExtended: false
        /// </summary>
        public float Urud { get; set; }
        /// <summary>
        ///  ASLCTRL control-mode (manual, stabilized, auto, etc...)
        /// OriginName: aslctrl_mode, Units: , IsExtended: false
        /// </summary>
        public byte AslctrlMode { get; set; }
        /// <summary>
        ///  
        /// OriginName: SpoilersEngaged, Units: , IsExtended: false
        /// </summary>
        public byte Spoilersengaged { get; set; }
    }
    /// <summary>
    /// ASL-fixed-wing controller debug data
    ///  ASLCTRL_DEBUG
    /// </summary>
    public class AslctrlDebugPacket : MavlinkV2Message<AslctrlDebugPayload>
    {
        public const int MessageId = 8005;
        
        public const byte CrcExtra = 251;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AslctrlDebugPayload Payload { get; } = new();

        public override string Name => "ASLCTRL_DEBUG";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("i32_1",
            " Debug data",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new("f_1",
            " Debug data ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("f_2",
            " Debug data",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("f_3",
            " Debug data",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("f_4",
            " Debug data",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("f_5",
            " Debug data",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("f_6",
            " Debug data",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("f_7",
            " Debug data",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("f_8",
            " Debug data",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("i8_1",
            " Debug data",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("i8_2",
            " Debug data",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASLCTRL_DEBUG:"
        + "uint32_t i32_1;"
        + "float f_1;"
        + "float f_2;"
        + "float f_3;"
        + "float f_4;"
        + "float f_5;"
        + "float f_6;"
        + "float f_7;"
        + "float f_8;"
        + "uint8_t i8_1;"
        + "uint8_t i8_2;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  ASLCTRL_DEBUG
    /// </summary>
    public class AslctrlDebugPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 38; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 38; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //I321
            sum+=4; //F1
            sum+=4; //F2
            sum+=4; //F3
            sum+=4; //F4
            sum+=4; //F5
            sum+=4; //F6
            sum+=4; //F7
            sum+=4; //F8
            sum+=1; //I81
            sum+=1; //I82
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            I321 = BinSerialize.ReadUInt(ref buffer);
            F1 = BinSerialize.ReadFloat(ref buffer);
            F2 = BinSerialize.ReadFloat(ref buffer);
            F3 = BinSerialize.ReadFloat(ref buffer);
            F4 = BinSerialize.ReadFloat(ref buffer);
            F5 = BinSerialize.ReadFloat(ref buffer);
            F6 = BinSerialize.ReadFloat(ref buffer);
            F7 = BinSerialize.ReadFloat(ref buffer);
            F8 = BinSerialize.ReadFloat(ref buffer);
            I81 = (byte)BinSerialize.ReadByte(ref buffer);
            I82 = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,I321);
            BinSerialize.WriteFloat(ref buffer,F1);
            BinSerialize.WriteFloat(ref buffer,F2);
            BinSerialize.WriteFloat(ref buffer,F3);
            BinSerialize.WriteFloat(ref buffer,F4);
            BinSerialize.WriteFloat(ref buffer,F5);
            BinSerialize.WriteFloat(ref buffer,F6);
            BinSerialize.WriteFloat(ref buffer,F7);
            BinSerialize.WriteFloat(ref buffer,F8);
            BinSerialize.WriteByte(ref buffer,(byte)I81);
            BinSerialize.WriteByte(ref buffer,(byte)I82);
            /* PayloadByteSize = 38 */;
        }
        
        



        /// <summary>
        ///  Debug data
        /// OriginName: i32_1, Units: , IsExtended: false
        /// </summary>
        public uint I321 { get; set; }
        /// <summary>
        ///  Debug data 
        /// OriginName: f_1, Units: , IsExtended: false
        /// </summary>
        public float F1 { get; set; }
        /// <summary>
        ///  Debug data
        /// OriginName: f_2, Units: , IsExtended: false
        /// </summary>
        public float F2 { get; set; }
        /// <summary>
        ///  Debug data
        /// OriginName: f_3, Units: , IsExtended: false
        /// </summary>
        public float F3 { get; set; }
        /// <summary>
        ///  Debug data
        /// OriginName: f_4, Units: , IsExtended: false
        /// </summary>
        public float F4 { get; set; }
        /// <summary>
        ///  Debug data
        /// OriginName: f_5, Units: , IsExtended: false
        /// </summary>
        public float F5 { get; set; }
        /// <summary>
        ///  Debug data
        /// OriginName: f_6, Units: , IsExtended: false
        /// </summary>
        public float F6 { get; set; }
        /// <summary>
        ///  Debug data
        /// OriginName: f_7, Units: , IsExtended: false
        /// </summary>
        public float F7 { get; set; }
        /// <summary>
        ///  Debug data
        /// OriginName: f_8, Units: , IsExtended: false
        /// </summary>
        public float F8 { get; set; }
        /// <summary>
        ///  Debug data
        /// OriginName: i8_1, Units: , IsExtended: false
        /// </summary>
        public byte I81 { get; set; }
        /// <summary>
        ///  Debug data
        /// OriginName: i8_2, Units: , IsExtended: false
        /// </summary>
        public byte I82 { get; set; }
    }
    /// <summary>
    /// Extended state information for ASLUAVs
    ///  ASLUAV_STATUS
    /// </summary>
    public class AsluavStatusPacket : MavlinkV2Message<AsluavStatusPayload>
    {
        public const int MessageId = 8006;
        
        public const byte CrcExtra = 97;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AsluavStatusPayload Payload { get; } = new();

        public override string Name => "ASLUAV_STATUS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("Motor_rpm",
            " Motor RPM ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("LED_status",
            " Status of the position-indicator LEDs",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("SATCOM_status",
            " Status of the IRIDIUM satellite communication system",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("Servo_status",
            " Status vector for up to 8 servos",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            8, 
            false),
        ];
        public const string FormatMessage = "ASLUAV_STATUS:"
        + "float Motor_rpm;"
        + "uint8_t LED_status;"
        + "uint8_t SATCOM_status;"
        + "uint8_t[8] Servo_status;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  ASLUAV_STATUS
    /// </summary>
    public class AsluavStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 14; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 14; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //MotorRpm
            sum+=1; //LedStatus
            sum+=1; //SatcomStatus
            sum+=ServoStatus.Length; //ServoStatus
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            MotorRpm = BinSerialize.ReadFloat(ref buffer);
            LedStatus = (byte)BinSerialize.ReadByte(ref buffer);
            SatcomStatus = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/8 - Math.Max(0,((/*PayloadByteSize*/14 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            ServoStatus = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                ServoStatus[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,MotorRpm);
            BinSerialize.WriteByte(ref buffer,(byte)LedStatus);
            BinSerialize.WriteByte(ref buffer,(byte)SatcomStatus);
            for(var i=0;i<ServoStatus.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)ServoStatus[i]);
            }
            /* PayloadByteSize = 14 */;
        }
        
        



        /// <summary>
        ///  Motor RPM 
        /// OriginName: Motor_rpm, Units: , IsExtended: false
        /// </summary>
        public float MotorRpm { get; set; }
        /// <summary>
        ///  Status of the position-indicator LEDs
        /// OriginName: LED_status, Units: , IsExtended: false
        /// </summary>
        public byte LedStatus { get; set; }
        /// <summary>
        ///  Status of the IRIDIUM satellite communication system
        /// OriginName: SATCOM_status, Units: , IsExtended: false
        /// </summary>
        public byte SatcomStatus { get; set; }
        /// <summary>
        ///  Status vector for up to 8 servos
        /// OriginName: Servo_status, Units: , IsExtended: false
        /// </summary>
        public const int ServoStatusMaxItemsCount = 8;
        public byte[] ServoStatus { get; set; } = new byte[8];
        [Obsolete("This method is deprecated. Use GetServoStatusMaxItemsCount instead.")]
        public byte GetServoStatusMaxItemsCount() => 8;
    }
    /// <summary>
    /// Extended EKF state estimates for ASLUAVs
    ///  EKF_EXT
    /// </summary>
    public class EkfExtPacket : MavlinkV2Message<EkfExtPayload>
    {
        public const int MessageId = 8007;
        
        public const byte CrcExtra = 64;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override EkfExtPayload Payload { get; } = new();

        public override string Name => "EKF_EXT";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("timestamp",
            " Time since system start",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("Windspeed",
            " Magnitude of wind velocity (in lateral inertial plane)",
            string.Empty, 
            @"m/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("WindDir",
            " Wind heading angle from North",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("WindZ",
            " Z (Down) component of inertial wind velocity",
            string.Empty, 
            @"m/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("Airspeed",
            " Magnitude of air velocity",
            string.Empty, 
            @"m/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("beta",
            " Sideslip angle",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("alpha",
            " Angle of attack",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
        ];
        public const string FormatMessage = "EKF_EXT:"
        + "uint64_t timestamp;"
        + "float Windspeed;"
        + "float WindDir;"
        + "float WindZ;"
        + "float Airspeed;"
        + "float beta;"
        + "float alpha;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  EKF_EXT
    /// </summary>
    public class EkfExtPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 32; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 32; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Timestamp
            sum+=4; //Windspeed
            sum+=4; //Winddir
            sum+=4; //Windz
            sum+=4; //Airspeed
            sum+=4; //Beta
            sum+=4; //Alpha
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Timestamp = BinSerialize.ReadULong(ref buffer);
            Windspeed = BinSerialize.ReadFloat(ref buffer);
            Winddir = BinSerialize.ReadFloat(ref buffer);
            Windz = BinSerialize.ReadFloat(ref buffer);
            Airspeed = BinSerialize.ReadFloat(ref buffer);
            Beta = BinSerialize.ReadFloat(ref buffer);
            Alpha = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Timestamp);
            BinSerialize.WriteFloat(ref buffer,Windspeed);
            BinSerialize.WriteFloat(ref buffer,Winddir);
            BinSerialize.WriteFloat(ref buffer,Windz);
            BinSerialize.WriteFloat(ref buffer,Airspeed);
            BinSerialize.WriteFloat(ref buffer,Beta);
            BinSerialize.WriteFloat(ref buffer,Alpha);
            /* PayloadByteSize = 32 */;
        }
        
        



        /// <summary>
        ///  Time since system start
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public ulong Timestamp { get; set; }
        /// <summary>
        ///  Magnitude of wind velocity (in lateral inertial plane)
        /// OriginName: Windspeed, Units: m/s, IsExtended: false
        /// </summary>
        public float Windspeed { get; set; }
        /// <summary>
        ///  Wind heading angle from North
        /// OriginName: WindDir, Units: rad, IsExtended: false
        /// </summary>
        public float Winddir { get; set; }
        /// <summary>
        ///  Z (Down) component of inertial wind velocity
        /// OriginName: WindZ, Units: m/s, IsExtended: false
        /// </summary>
        public float Windz { get; set; }
        /// <summary>
        ///  Magnitude of air velocity
        /// OriginName: Airspeed, Units: m/s, IsExtended: false
        /// </summary>
        public float Airspeed { get; set; }
        /// <summary>
        ///  Sideslip angle
        /// OriginName: beta, Units: rad, IsExtended: false
        /// </summary>
        public float Beta { get; set; }
        /// <summary>
        ///  Angle of attack
        /// OriginName: alpha, Units: rad, IsExtended: false
        /// </summary>
        public float Alpha { get; set; }
    }
    /// <summary>
    /// Off-board controls/commands for ASLUAVs
    ///  ASL_OBCTRL
    /// </summary>
    public class AslObctrlPacket : MavlinkV2Message<AslObctrlPayload>
    {
        public const int MessageId = 8008;
        
        public const byte CrcExtra = 234;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AslObctrlPayload Payload { get; } = new();

        public override string Name => "ASL_OBCTRL";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("timestamp",
            " Time since system start",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("uElev",
            " Elevator command [~]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("uThrot",
            " Throttle command [~]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("uThrot2",
            " Throttle 2 command [~]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("uAilL",
            " Left aileron command [~]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("uAilR",
            " Right aileron command [~]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("uRud",
            " Rudder command [~]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("obctrl_status",
            " Off-board computer status",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ASL_OBCTRL:"
        + "uint64_t timestamp;"
        + "float uElev;"
        + "float uThrot;"
        + "float uThrot2;"
        + "float uAilL;"
        + "float uAilR;"
        + "float uRud;"
        + "uint8_t obctrl_status;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  ASL_OBCTRL
    /// </summary>
    public class AslObctrlPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 33; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 33; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Timestamp
            sum+=4; //Uelev
            sum+=4; //Uthrot
            sum+=4; //Uthrot2
            sum+=4; //Uaill
            sum+=4; //Uailr
            sum+=4; //Urud
            sum+=1; //ObctrlStatus
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Timestamp = BinSerialize.ReadULong(ref buffer);
            Uelev = BinSerialize.ReadFloat(ref buffer);
            Uthrot = BinSerialize.ReadFloat(ref buffer);
            Uthrot2 = BinSerialize.ReadFloat(ref buffer);
            Uaill = BinSerialize.ReadFloat(ref buffer);
            Uailr = BinSerialize.ReadFloat(ref buffer);
            Urud = BinSerialize.ReadFloat(ref buffer);
            ObctrlStatus = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Timestamp);
            BinSerialize.WriteFloat(ref buffer,Uelev);
            BinSerialize.WriteFloat(ref buffer,Uthrot);
            BinSerialize.WriteFloat(ref buffer,Uthrot2);
            BinSerialize.WriteFloat(ref buffer,Uaill);
            BinSerialize.WriteFloat(ref buffer,Uailr);
            BinSerialize.WriteFloat(ref buffer,Urud);
            BinSerialize.WriteByte(ref buffer,(byte)ObctrlStatus);
            /* PayloadByteSize = 33 */;
        }
        
        



        /// <summary>
        ///  Time since system start
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public ulong Timestamp { get; set; }
        /// <summary>
        ///  Elevator command [~]
        /// OriginName: uElev, Units: , IsExtended: false
        /// </summary>
        public float Uelev { get; set; }
        /// <summary>
        ///  Throttle command [~]
        /// OriginName: uThrot, Units: , IsExtended: false
        /// </summary>
        public float Uthrot { get; set; }
        /// <summary>
        ///  Throttle 2 command [~]
        /// OriginName: uThrot2, Units: , IsExtended: false
        /// </summary>
        public float Uthrot2 { get; set; }
        /// <summary>
        ///  Left aileron command [~]
        /// OriginName: uAilL, Units: , IsExtended: false
        /// </summary>
        public float Uaill { get; set; }
        /// <summary>
        ///  Right aileron command [~]
        /// OriginName: uAilR, Units: , IsExtended: false
        /// </summary>
        public float Uailr { get; set; }
        /// <summary>
        ///  Rudder command [~]
        /// OriginName: uRud, Units: , IsExtended: false
        /// </summary>
        public float Urud { get; set; }
        /// <summary>
        ///  Off-board computer status
        /// OriginName: obctrl_status, Units: , IsExtended: false
        /// </summary>
        public byte ObctrlStatus { get; set; }
    }
    /// <summary>
    /// Atmospheric sensors (temperature, humidity, ...) 
    ///  SENS_ATMOS
    /// </summary>
    public class SensAtmosPacket : MavlinkV2Message<SensAtmosPayload>
    {
        public const int MessageId = 8009;
        
        public const byte CrcExtra = 144;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SensAtmosPayload Payload { get; } = new();

        public override string Name => "SENS_ATMOS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("timestamp",
            "Time since system boot",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("TempAmbient",
            " Ambient temperature",
            string.Empty, 
            @"degC", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("Humidity",
            " Relative humidity",
            string.Empty, 
            @"%", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
        ];
        public const string FormatMessage = "SENS_ATMOS:"
        + "uint64_t timestamp;"
        + "float TempAmbient;"
        + "float Humidity;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  SENS_ATMOS
    /// </summary>
    public class SensAtmosPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 16; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Timestamp
            sum+=4; //Tempambient
            sum+=4; //Humidity
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Timestamp = BinSerialize.ReadULong(ref buffer);
            Tempambient = BinSerialize.ReadFloat(ref buffer);
            Humidity = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Timestamp);
            BinSerialize.WriteFloat(ref buffer,Tempambient);
            BinSerialize.WriteFloat(ref buffer,Humidity);
            /* PayloadByteSize = 16 */;
        }
        
        



        /// <summary>
        /// Time since system boot
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public ulong Timestamp { get; set; }
        /// <summary>
        ///  Ambient temperature
        /// OriginName: TempAmbient, Units: degC, IsExtended: false
        /// </summary>
        public float Tempambient { get; set; }
        /// <summary>
        ///  Relative humidity
        /// OriginName: Humidity, Units: %, IsExtended: false
        /// </summary>
        public float Humidity { get; set; }
    }
    /// <summary>
    /// Battery pack monitoring data for Li-Ion batteries
    ///  SENS_BATMON
    /// </summary>
    public class SensBatmonPacket : MavlinkV2Message<SensBatmonPayload>
    {
        public const int MessageId = 8010;
        
        public const byte CrcExtra = 155;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SensBatmonPayload Payload { get; } = new();

        public override string Name => "SENS_BATMON";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("batmon_timestamp",
            "Time since system start",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("temperature",
            "Battery pack temperature",
            string.Empty, 
            @"degC", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("safetystatus",
            "Battery monitor safetystatus report bits in Hex",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new("operationstatus",
            "Battery monitor operation status report bits in Hex",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new("voltage",
            "Battery pack voltage",
            string.Empty, 
            @"mV", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("current",
            "Battery pack current",
            string.Empty, 
            @"mA", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new("batterystatus",
            "Battery monitor status report bits in Hex",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("serialnumber",
            "Battery monitor serial number in Hex",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("cellvoltage1",
            "Battery pack cell 1 voltage",
            string.Empty, 
            @"mV", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("cellvoltage2",
            "Battery pack cell 2 voltage",
            string.Empty, 
            @"mV", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("cellvoltage3",
            "Battery pack cell 3 voltage",
            string.Empty, 
            @"mV", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("cellvoltage4",
            "Battery pack cell 4 voltage",
            string.Empty, 
            @"mV", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("cellvoltage5",
            "Battery pack cell 5 voltage",
            string.Empty, 
            @"mV", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("cellvoltage6",
            "Battery pack cell 6 voltage",
            string.Empty, 
            @"mV", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("SoC",
            "Battery pack state-of-charge",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "SENS_BATMON:"
        + "uint64_t batmon_timestamp;"
        + "float temperature;"
        + "uint32_t safetystatus;"
        + "uint32_t operationstatus;"
        + "uint16_t voltage;"
        + "int16_t current;"
        + "uint16_t batterystatus;"
        + "uint16_t serialnumber;"
        + "uint16_t cellvoltage1;"
        + "uint16_t cellvoltage2;"
        + "uint16_t cellvoltage3;"
        + "uint16_t cellvoltage4;"
        + "uint16_t cellvoltage5;"
        + "uint16_t cellvoltage6;"
        + "uint8_t SoC;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  SENS_BATMON
    /// </summary>
    public class SensBatmonPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 41; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 41; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //BatmonTimestamp
            sum+=4; //Temperature
            sum+=4; //Safetystatus
            sum+=4; //Operationstatus
            sum+=2; //Voltage
            sum+=2; //Current
            sum+=2; //Batterystatus
            sum+=2; //Serialnumber
            sum+=2; //Cellvoltage1
            sum+=2; //Cellvoltage2
            sum+=2; //Cellvoltage3
            sum+=2; //Cellvoltage4
            sum+=2; //Cellvoltage5
            sum+=2; //Cellvoltage6
            sum+=1; //Soc
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            BatmonTimestamp = BinSerialize.ReadULong(ref buffer);
            Temperature = BinSerialize.ReadFloat(ref buffer);
            Safetystatus = BinSerialize.ReadUInt(ref buffer);
            Operationstatus = BinSerialize.ReadUInt(ref buffer);
            Voltage = BinSerialize.ReadUShort(ref buffer);
            Current = BinSerialize.ReadShort(ref buffer);
            Batterystatus = BinSerialize.ReadUShort(ref buffer);
            Serialnumber = BinSerialize.ReadUShort(ref buffer);
            Cellvoltage1 = BinSerialize.ReadUShort(ref buffer);
            Cellvoltage2 = BinSerialize.ReadUShort(ref buffer);
            Cellvoltage3 = BinSerialize.ReadUShort(ref buffer);
            Cellvoltage4 = BinSerialize.ReadUShort(ref buffer);
            Cellvoltage5 = BinSerialize.ReadUShort(ref buffer);
            Cellvoltage6 = BinSerialize.ReadUShort(ref buffer);
            Soc = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,BatmonTimestamp);
            BinSerialize.WriteFloat(ref buffer,Temperature);
            BinSerialize.WriteUInt(ref buffer,Safetystatus);
            BinSerialize.WriteUInt(ref buffer,Operationstatus);
            BinSerialize.WriteUShort(ref buffer,Voltage);
            BinSerialize.WriteShort(ref buffer,Current);
            BinSerialize.WriteUShort(ref buffer,Batterystatus);
            BinSerialize.WriteUShort(ref buffer,Serialnumber);
            BinSerialize.WriteUShort(ref buffer,Cellvoltage1);
            BinSerialize.WriteUShort(ref buffer,Cellvoltage2);
            BinSerialize.WriteUShort(ref buffer,Cellvoltage3);
            BinSerialize.WriteUShort(ref buffer,Cellvoltage4);
            BinSerialize.WriteUShort(ref buffer,Cellvoltage5);
            BinSerialize.WriteUShort(ref buffer,Cellvoltage6);
            BinSerialize.WriteByte(ref buffer,(byte)Soc);
            /* PayloadByteSize = 41 */;
        }
        
        



        /// <summary>
        /// Time since system start
        /// OriginName: batmon_timestamp, Units: us, IsExtended: false
        /// </summary>
        public ulong BatmonTimestamp { get; set; }
        /// <summary>
        /// Battery pack temperature
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
        public float Temperature { get; set; }
        /// <summary>
        /// Battery monitor safetystatus report bits in Hex
        /// OriginName: safetystatus, Units: , IsExtended: false
        /// </summary>
        public uint Safetystatus { get; set; }
        /// <summary>
        /// Battery monitor operation status report bits in Hex
        /// OriginName: operationstatus, Units: , IsExtended: false
        /// </summary>
        public uint Operationstatus { get; set; }
        /// <summary>
        /// Battery pack voltage
        /// OriginName: voltage, Units: mV, IsExtended: false
        /// </summary>
        public ushort Voltage { get; set; }
        /// <summary>
        /// Battery pack current
        /// OriginName: current, Units: mA, IsExtended: false
        /// </summary>
        public short Current { get; set; }
        /// <summary>
        /// Battery monitor status report bits in Hex
        /// OriginName: batterystatus, Units: , IsExtended: false
        /// </summary>
        public ushort Batterystatus { get; set; }
        /// <summary>
        /// Battery monitor serial number in Hex
        /// OriginName: serialnumber, Units: , IsExtended: false
        /// </summary>
        public ushort Serialnumber { get; set; }
        /// <summary>
        /// Battery pack cell 1 voltage
        /// OriginName: cellvoltage1, Units: mV, IsExtended: false
        /// </summary>
        public ushort Cellvoltage1 { get; set; }
        /// <summary>
        /// Battery pack cell 2 voltage
        /// OriginName: cellvoltage2, Units: mV, IsExtended: false
        /// </summary>
        public ushort Cellvoltage2 { get; set; }
        /// <summary>
        /// Battery pack cell 3 voltage
        /// OriginName: cellvoltage3, Units: mV, IsExtended: false
        /// </summary>
        public ushort Cellvoltage3 { get; set; }
        /// <summary>
        /// Battery pack cell 4 voltage
        /// OriginName: cellvoltage4, Units: mV, IsExtended: false
        /// </summary>
        public ushort Cellvoltage4 { get; set; }
        /// <summary>
        /// Battery pack cell 5 voltage
        /// OriginName: cellvoltage5, Units: mV, IsExtended: false
        /// </summary>
        public ushort Cellvoltage5 { get; set; }
        /// <summary>
        /// Battery pack cell 6 voltage
        /// OriginName: cellvoltage6, Units: mV, IsExtended: false
        /// </summary>
        public ushort Cellvoltage6 { get; set; }
        /// <summary>
        /// Battery pack state-of-charge
        /// OriginName: SoC, Units: , IsExtended: false
        /// </summary>
        public byte Soc { get; set; }
    }
    /// <summary>
    /// Fixed-wing soaring (i.e. thermal seeking) data
    ///  FW_SOARING_DATA
    /// </summary>
    public class FwSoaringDataPacket : MavlinkV2Message<FwSoaringDataPayload>
    {
        public const int MessageId = 8011;
        
        public const byte CrcExtra = 20;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override FwSoaringDataPayload Payload { get; } = new();

        public override string Name => "FW_SOARING_DATA";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("timestamp",
            "Timestamp",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("timestampModeChanged",
            "Timestamp since last mode change",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("xW",
            "Thermal core updraft strength",
            string.Empty, 
            @"m/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("xR",
            "Thermal radius",
            string.Empty, 
            @"m", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("xLat",
            "Thermal center latitude",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("xLon",
            "Thermal center longitude",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("VarW",
            "Variance W",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("VarR",
            "Variance R",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("VarLat",
            "Variance Lat",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("VarLon",
            "Variance Lon ",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("LoiterRadius",
            "Suggested loiter radius",
            string.Empty, 
            @"m", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("LoiterDirection",
            "Suggested loiter direction",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("DistToSoarPoint",
            "Distance to soar point",
            string.Empty, 
            @"m", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("vSinkExp",
            "Expected sink rate at current airspeed, roll and throttle",
            string.Empty, 
            @"m/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("z1_LocalUpdraftSpeed",
            "Measurement / updraft speed at current/local airplane position",
            string.Empty, 
            @"m/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("z2_DeltaRoll",
            "Measurement / roll angle tracking error",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("z1_exp",
            "Expected measurement 1",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("z2_exp",
            "Expected measurement 2",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("ThermalGSNorth",
            "Thermal drift (from estimator prediction step only)",
            string.Empty, 
            @"m/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("ThermalGSEast",
            "Thermal drift (from estimator prediction step only)",
            string.Empty, 
            @"m/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("TSE_dot",
            " Total specific energy change (filtered)",
            string.Empty, 
            @"m/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("DebugVar1",
            " Debug variable 1",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("DebugVar2",
            " Debug variable 2",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("ControlMode",
            "Control Mode [-]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("valid",
            "Data valid [-]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "FW_SOARING_DATA:"
        + "uint64_t timestamp;"
        + "uint64_t timestampModeChanged;"
        + "float xW;"
        + "float xR;"
        + "float xLat;"
        + "float xLon;"
        + "float VarW;"
        + "float VarR;"
        + "float VarLat;"
        + "float VarLon;"
        + "float LoiterRadius;"
        + "float LoiterDirection;"
        + "float DistToSoarPoint;"
        + "float vSinkExp;"
        + "float z1_LocalUpdraftSpeed;"
        + "float z2_DeltaRoll;"
        + "float z1_exp;"
        + "float z2_exp;"
        + "float ThermalGSNorth;"
        + "float ThermalGSEast;"
        + "float TSE_dot;"
        + "float DebugVar1;"
        + "float DebugVar2;"
        + "uint8_t ControlMode;"
        + "uint8_t valid;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  FW_SOARING_DATA
    /// </summary>
    public class FwSoaringDataPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 102; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 102; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Timestamp
            sum+=8; //Timestampmodechanged
            sum+=4; //Xw
            sum+=4; //Xr
            sum+=4; //Xlat
            sum+=4; //Xlon
            sum+=4; //Varw
            sum+=4; //Varr
            sum+=4; //Varlat
            sum+=4; //Varlon
            sum+=4; //Loiterradius
            sum+=4; //Loiterdirection
            sum+=4; //Disttosoarpoint
            sum+=4; //Vsinkexp
            sum+=4; //Z1Localupdraftspeed
            sum+=4; //Z2Deltaroll
            sum+=4; //Z1Exp
            sum+=4; //Z2Exp
            sum+=4; //Thermalgsnorth
            sum+=4; //Thermalgseast
            sum+=4; //TseDot
            sum+=4; //Debugvar1
            sum+=4; //Debugvar2
            sum+=1; //Controlmode
            sum+=1; //Valid
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Timestamp = BinSerialize.ReadULong(ref buffer);
            Timestampmodechanged = BinSerialize.ReadULong(ref buffer);
            Xw = BinSerialize.ReadFloat(ref buffer);
            Xr = BinSerialize.ReadFloat(ref buffer);
            Xlat = BinSerialize.ReadFloat(ref buffer);
            Xlon = BinSerialize.ReadFloat(ref buffer);
            Varw = BinSerialize.ReadFloat(ref buffer);
            Varr = BinSerialize.ReadFloat(ref buffer);
            Varlat = BinSerialize.ReadFloat(ref buffer);
            Varlon = BinSerialize.ReadFloat(ref buffer);
            Loiterradius = BinSerialize.ReadFloat(ref buffer);
            Loiterdirection = BinSerialize.ReadFloat(ref buffer);
            Disttosoarpoint = BinSerialize.ReadFloat(ref buffer);
            Vsinkexp = BinSerialize.ReadFloat(ref buffer);
            Z1Localupdraftspeed = BinSerialize.ReadFloat(ref buffer);
            Z2Deltaroll = BinSerialize.ReadFloat(ref buffer);
            Z1Exp = BinSerialize.ReadFloat(ref buffer);
            Z2Exp = BinSerialize.ReadFloat(ref buffer);
            Thermalgsnorth = BinSerialize.ReadFloat(ref buffer);
            Thermalgseast = BinSerialize.ReadFloat(ref buffer);
            TseDot = BinSerialize.ReadFloat(ref buffer);
            Debugvar1 = BinSerialize.ReadFloat(ref buffer);
            Debugvar2 = BinSerialize.ReadFloat(ref buffer);
            Controlmode = (byte)BinSerialize.ReadByte(ref buffer);
            Valid = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Timestamp);
            BinSerialize.WriteULong(ref buffer,Timestampmodechanged);
            BinSerialize.WriteFloat(ref buffer,Xw);
            BinSerialize.WriteFloat(ref buffer,Xr);
            BinSerialize.WriteFloat(ref buffer,Xlat);
            BinSerialize.WriteFloat(ref buffer,Xlon);
            BinSerialize.WriteFloat(ref buffer,Varw);
            BinSerialize.WriteFloat(ref buffer,Varr);
            BinSerialize.WriteFloat(ref buffer,Varlat);
            BinSerialize.WriteFloat(ref buffer,Varlon);
            BinSerialize.WriteFloat(ref buffer,Loiterradius);
            BinSerialize.WriteFloat(ref buffer,Loiterdirection);
            BinSerialize.WriteFloat(ref buffer,Disttosoarpoint);
            BinSerialize.WriteFloat(ref buffer,Vsinkexp);
            BinSerialize.WriteFloat(ref buffer,Z1Localupdraftspeed);
            BinSerialize.WriteFloat(ref buffer,Z2Deltaroll);
            BinSerialize.WriteFloat(ref buffer,Z1Exp);
            BinSerialize.WriteFloat(ref buffer,Z2Exp);
            BinSerialize.WriteFloat(ref buffer,Thermalgsnorth);
            BinSerialize.WriteFloat(ref buffer,Thermalgseast);
            BinSerialize.WriteFloat(ref buffer,TseDot);
            BinSerialize.WriteFloat(ref buffer,Debugvar1);
            BinSerialize.WriteFloat(ref buffer,Debugvar2);
            BinSerialize.WriteByte(ref buffer,(byte)Controlmode);
            BinSerialize.WriteByte(ref buffer,(byte)Valid);
            /* PayloadByteSize = 102 */;
        }
        
        



        /// <summary>
        /// Timestamp
        /// OriginName: timestamp, Units: ms, IsExtended: false
        /// </summary>
        public ulong Timestamp { get; set; }
        /// <summary>
        /// Timestamp since last mode change
        /// OriginName: timestampModeChanged, Units: ms, IsExtended: false
        /// </summary>
        public ulong Timestampmodechanged { get; set; }
        /// <summary>
        /// Thermal core updraft strength
        /// OriginName: xW, Units: m/s, IsExtended: false
        /// </summary>
        public float Xw { get; set; }
        /// <summary>
        /// Thermal radius
        /// OriginName: xR, Units: m, IsExtended: false
        /// </summary>
        public float Xr { get; set; }
        /// <summary>
        /// Thermal center latitude
        /// OriginName: xLat, Units: deg, IsExtended: false
        /// </summary>
        public float Xlat { get; set; }
        /// <summary>
        /// Thermal center longitude
        /// OriginName: xLon, Units: deg, IsExtended: false
        /// </summary>
        public float Xlon { get; set; }
        /// <summary>
        /// Variance W
        /// OriginName: VarW, Units: , IsExtended: false
        /// </summary>
        public float Varw { get; set; }
        /// <summary>
        /// Variance R
        /// OriginName: VarR, Units: , IsExtended: false
        /// </summary>
        public float Varr { get; set; }
        /// <summary>
        /// Variance Lat
        /// OriginName: VarLat, Units: , IsExtended: false
        /// </summary>
        public float Varlat { get; set; }
        /// <summary>
        /// Variance Lon 
        /// OriginName: VarLon, Units: , IsExtended: false
        /// </summary>
        public float Varlon { get; set; }
        /// <summary>
        /// Suggested loiter radius
        /// OriginName: LoiterRadius, Units: m, IsExtended: false
        /// </summary>
        public float Loiterradius { get; set; }
        /// <summary>
        /// Suggested loiter direction
        /// OriginName: LoiterDirection, Units: , IsExtended: false
        /// </summary>
        public float Loiterdirection { get; set; }
        /// <summary>
        /// Distance to soar point
        /// OriginName: DistToSoarPoint, Units: m, IsExtended: false
        /// </summary>
        public float Disttosoarpoint { get; set; }
        /// <summary>
        /// Expected sink rate at current airspeed, roll and throttle
        /// OriginName: vSinkExp, Units: m/s, IsExtended: false
        /// </summary>
        public float Vsinkexp { get; set; }
        /// <summary>
        /// Measurement / updraft speed at current/local airplane position
        /// OriginName: z1_LocalUpdraftSpeed, Units: m/s, IsExtended: false
        /// </summary>
        public float Z1Localupdraftspeed { get; set; }
        /// <summary>
        /// Measurement / roll angle tracking error
        /// OriginName: z2_DeltaRoll, Units: deg, IsExtended: false
        /// </summary>
        public float Z2Deltaroll { get; set; }
        /// <summary>
        /// Expected measurement 1
        /// OriginName: z1_exp, Units: , IsExtended: false
        /// </summary>
        public float Z1Exp { get; set; }
        /// <summary>
        /// Expected measurement 2
        /// OriginName: z2_exp, Units: , IsExtended: false
        /// </summary>
        public float Z2Exp { get; set; }
        /// <summary>
        /// Thermal drift (from estimator prediction step only)
        /// OriginName: ThermalGSNorth, Units: m/s, IsExtended: false
        /// </summary>
        public float Thermalgsnorth { get; set; }
        /// <summary>
        /// Thermal drift (from estimator prediction step only)
        /// OriginName: ThermalGSEast, Units: m/s, IsExtended: false
        /// </summary>
        public float Thermalgseast { get; set; }
        /// <summary>
        ///  Total specific energy change (filtered)
        /// OriginName: TSE_dot, Units: m/s, IsExtended: false
        /// </summary>
        public float TseDot { get; set; }
        /// <summary>
        ///  Debug variable 1
        /// OriginName: DebugVar1, Units: , IsExtended: false
        /// </summary>
        public float Debugvar1 { get; set; }
        /// <summary>
        ///  Debug variable 2
        /// OriginName: DebugVar2, Units: , IsExtended: false
        /// </summary>
        public float Debugvar2 { get; set; }
        /// <summary>
        /// Control Mode [-]
        /// OriginName: ControlMode, Units: , IsExtended: false
        /// </summary>
        public byte Controlmode { get; set; }
        /// <summary>
        /// Data valid [-]
        /// OriginName: valid, Units: , IsExtended: false
        /// </summary>
        public byte Valid { get; set; }
    }
    /// <summary>
    /// Monitoring of sensorpod status
    ///  SENSORPOD_STATUS
    /// </summary>
    public class SensorpodStatusPacket : MavlinkV2Message<SensorpodStatusPayload>
    {
        public const int MessageId = 8012;
        
        public const byte CrcExtra = 54;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SensorpodStatusPayload Payload { get; } = new();

        public override string Name => "SENSORPOD_STATUS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("timestamp",
            "Timestamp in linuxtime (since 1.1.1970)",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("free_space",
            "Free space available in recordings directory in [Gb] * 1e2",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("visensor_rate_1",
            "Rate of ROS topic 1",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("visensor_rate_2",
            "Rate of ROS topic 2",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("visensor_rate_3",
            "Rate of ROS topic 3",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("visensor_rate_4",
            "Rate of ROS topic 4",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("recording_nodes_count",
            "Number of recording nodes",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("cpu_temp",
            "Temperature of sensorpod CPU in",
            string.Empty, 
            @"degC", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "SENSORPOD_STATUS:"
        + "uint64_t timestamp;"
        + "uint16_t free_space;"
        + "uint8_t visensor_rate_1;"
        + "uint8_t visensor_rate_2;"
        + "uint8_t visensor_rate_3;"
        + "uint8_t visensor_rate_4;"
        + "uint8_t recording_nodes_count;"
        + "uint8_t cpu_temp;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  SENSORPOD_STATUS
    /// </summary>
    public class SensorpodStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 16; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Timestamp
            sum+=2; //FreeSpace
            sum+=1; //VisensorRate1
            sum+=1; //VisensorRate2
            sum+=1; //VisensorRate3
            sum+=1; //VisensorRate4
            sum+=1; //RecordingNodesCount
            sum+=1; //CpuTemp
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Timestamp = BinSerialize.ReadULong(ref buffer);
            FreeSpace = BinSerialize.ReadUShort(ref buffer);
            VisensorRate1 = (byte)BinSerialize.ReadByte(ref buffer);
            VisensorRate2 = (byte)BinSerialize.ReadByte(ref buffer);
            VisensorRate3 = (byte)BinSerialize.ReadByte(ref buffer);
            VisensorRate4 = (byte)BinSerialize.ReadByte(ref buffer);
            RecordingNodesCount = (byte)BinSerialize.ReadByte(ref buffer);
            CpuTemp = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Timestamp);
            BinSerialize.WriteUShort(ref buffer,FreeSpace);
            BinSerialize.WriteByte(ref buffer,(byte)VisensorRate1);
            BinSerialize.WriteByte(ref buffer,(byte)VisensorRate2);
            BinSerialize.WriteByte(ref buffer,(byte)VisensorRate3);
            BinSerialize.WriteByte(ref buffer,(byte)VisensorRate4);
            BinSerialize.WriteByte(ref buffer,(byte)RecordingNodesCount);
            BinSerialize.WriteByte(ref buffer,(byte)CpuTemp);
            /* PayloadByteSize = 16 */;
        }
        
        



        /// <summary>
        /// Timestamp in linuxtime (since 1.1.1970)
        /// OriginName: timestamp, Units: ms, IsExtended: false
        /// </summary>
        public ulong Timestamp { get; set; }
        /// <summary>
        /// Free space available in recordings directory in [Gb] * 1e2
        /// OriginName: free_space, Units: , IsExtended: false
        /// </summary>
        public ushort FreeSpace { get; set; }
        /// <summary>
        /// Rate of ROS topic 1
        /// OriginName: visensor_rate_1, Units: , IsExtended: false
        /// </summary>
        public byte VisensorRate1 { get; set; }
        /// <summary>
        /// Rate of ROS topic 2
        /// OriginName: visensor_rate_2, Units: , IsExtended: false
        /// </summary>
        public byte VisensorRate2 { get; set; }
        /// <summary>
        /// Rate of ROS topic 3
        /// OriginName: visensor_rate_3, Units: , IsExtended: false
        /// </summary>
        public byte VisensorRate3 { get; set; }
        /// <summary>
        /// Rate of ROS topic 4
        /// OriginName: visensor_rate_4, Units: , IsExtended: false
        /// </summary>
        public byte VisensorRate4 { get; set; }
        /// <summary>
        /// Number of recording nodes
        /// OriginName: recording_nodes_count, Units: , IsExtended: false
        /// </summary>
        public byte RecordingNodesCount { get; set; }
        /// <summary>
        /// Temperature of sensorpod CPU in
        /// OriginName: cpu_temp, Units: degC, IsExtended: false
        /// </summary>
        public byte CpuTemp { get; set; }
    }
    /// <summary>
    /// Monitoring of power board status
    ///  SENS_POWER_BOARD
    /// </summary>
    public class SensPowerBoardPacket : MavlinkV2Message<SensPowerBoardPayload>
    {
        public const int MessageId = 8013;
        
        public const byte CrcExtra = 222;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SensPowerBoardPayload Payload { get; } = new();

        public override string Name => "SENS_POWER_BOARD";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("timestamp",
            "Timestamp",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("pwr_brd_system_volt",
            "Power board system voltage",
            string.Empty, 
            @"V", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("pwr_brd_servo_volt",
            "Power board servo voltage",
            string.Empty, 
            @"V", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("pwr_brd_digital_volt",
            "Power board digital voltage",
            string.Empty, 
            @"V", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("pwr_brd_mot_l_amp",
            "Power board left motor current sensor",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("pwr_brd_mot_r_amp",
            "Power board right motor current sensor",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("pwr_brd_analog_amp",
            "Power board analog current sensor",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("pwr_brd_digital_amp",
            "Power board digital current sensor",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("pwr_brd_ext_amp",
            "Power board extension current sensor",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("pwr_brd_aux_amp",
            "Power board aux current sensor",
            string.Empty, 
            @"A", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("pwr_brd_status",
            "Power board status register",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("pwr_brd_led_status",
            "Power board leds status",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "SENS_POWER_BOARD:"
        + "uint64_t timestamp;"
        + "float pwr_brd_system_volt;"
        + "float pwr_brd_servo_volt;"
        + "float pwr_brd_digital_volt;"
        + "float pwr_brd_mot_l_amp;"
        + "float pwr_brd_mot_r_amp;"
        + "float pwr_brd_analog_amp;"
        + "float pwr_brd_digital_amp;"
        + "float pwr_brd_ext_amp;"
        + "float pwr_brd_aux_amp;"
        + "uint8_t pwr_brd_status;"
        + "uint8_t pwr_brd_led_status;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  SENS_POWER_BOARD
    /// </summary>
    public class SensPowerBoardPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 46; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 46; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Timestamp
            sum+=4; //PwrBrdSystemVolt
            sum+=4; //PwrBrdServoVolt
            sum+=4; //PwrBrdDigitalVolt
            sum+=4; //PwrBrdMotLAmp
            sum+=4; //PwrBrdMotRAmp
            sum+=4; //PwrBrdAnalogAmp
            sum+=4; //PwrBrdDigitalAmp
            sum+=4; //PwrBrdExtAmp
            sum+=4; //PwrBrdAuxAmp
            sum+=1; //PwrBrdStatus
            sum+=1; //PwrBrdLedStatus
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Timestamp = BinSerialize.ReadULong(ref buffer);
            PwrBrdSystemVolt = BinSerialize.ReadFloat(ref buffer);
            PwrBrdServoVolt = BinSerialize.ReadFloat(ref buffer);
            PwrBrdDigitalVolt = BinSerialize.ReadFloat(ref buffer);
            PwrBrdMotLAmp = BinSerialize.ReadFloat(ref buffer);
            PwrBrdMotRAmp = BinSerialize.ReadFloat(ref buffer);
            PwrBrdAnalogAmp = BinSerialize.ReadFloat(ref buffer);
            PwrBrdDigitalAmp = BinSerialize.ReadFloat(ref buffer);
            PwrBrdExtAmp = BinSerialize.ReadFloat(ref buffer);
            PwrBrdAuxAmp = BinSerialize.ReadFloat(ref buffer);
            PwrBrdStatus = (byte)BinSerialize.ReadByte(ref buffer);
            PwrBrdLedStatus = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Timestamp);
            BinSerialize.WriteFloat(ref buffer,PwrBrdSystemVolt);
            BinSerialize.WriteFloat(ref buffer,PwrBrdServoVolt);
            BinSerialize.WriteFloat(ref buffer,PwrBrdDigitalVolt);
            BinSerialize.WriteFloat(ref buffer,PwrBrdMotLAmp);
            BinSerialize.WriteFloat(ref buffer,PwrBrdMotRAmp);
            BinSerialize.WriteFloat(ref buffer,PwrBrdAnalogAmp);
            BinSerialize.WriteFloat(ref buffer,PwrBrdDigitalAmp);
            BinSerialize.WriteFloat(ref buffer,PwrBrdExtAmp);
            BinSerialize.WriteFloat(ref buffer,PwrBrdAuxAmp);
            BinSerialize.WriteByte(ref buffer,(byte)PwrBrdStatus);
            BinSerialize.WriteByte(ref buffer,(byte)PwrBrdLedStatus);
            /* PayloadByteSize = 46 */;
        }
        
        



        /// <summary>
        /// Timestamp
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public ulong Timestamp { get; set; }
        /// <summary>
        /// Power board system voltage
        /// OriginName: pwr_brd_system_volt, Units: V, IsExtended: false
        /// </summary>
        public float PwrBrdSystemVolt { get; set; }
        /// <summary>
        /// Power board servo voltage
        /// OriginName: pwr_brd_servo_volt, Units: V, IsExtended: false
        /// </summary>
        public float PwrBrdServoVolt { get; set; }
        /// <summary>
        /// Power board digital voltage
        /// OriginName: pwr_brd_digital_volt, Units: V, IsExtended: false
        /// </summary>
        public float PwrBrdDigitalVolt { get; set; }
        /// <summary>
        /// Power board left motor current sensor
        /// OriginName: pwr_brd_mot_l_amp, Units: A, IsExtended: false
        /// </summary>
        public float PwrBrdMotLAmp { get; set; }
        /// <summary>
        /// Power board right motor current sensor
        /// OriginName: pwr_brd_mot_r_amp, Units: A, IsExtended: false
        /// </summary>
        public float PwrBrdMotRAmp { get; set; }
        /// <summary>
        /// Power board analog current sensor
        /// OriginName: pwr_brd_analog_amp, Units: A, IsExtended: false
        /// </summary>
        public float PwrBrdAnalogAmp { get; set; }
        /// <summary>
        /// Power board digital current sensor
        /// OriginName: pwr_brd_digital_amp, Units: A, IsExtended: false
        /// </summary>
        public float PwrBrdDigitalAmp { get; set; }
        /// <summary>
        /// Power board extension current sensor
        /// OriginName: pwr_brd_ext_amp, Units: A, IsExtended: false
        /// </summary>
        public float PwrBrdExtAmp { get; set; }
        /// <summary>
        /// Power board aux current sensor
        /// OriginName: pwr_brd_aux_amp, Units: A, IsExtended: false
        /// </summary>
        public float PwrBrdAuxAmp { get; set; }
        /// <summary>
        /// Power board status register
        /// OriginName: pwr_brd_status, Units: , IsExtended: false
        /// </summary>
        public byte PwrBrdStatus { get; set; }
        /// <summary>
        /// Power board leds status
        /// OriginName: pwr_brd_led_status, Units: , IsExtended: false
        /// </summary>
        public byte PwrBrdLedStatus { get; set; }
    }
    /// <summary>
    /// Status of GSM modem (connected to onboard computer)
    ///  GSM_LINK_STATUS
    /// </summary>
    public class GsmLinkStatusPacket : MavlinkV2Message<GsmLinkStatusPayload>
    {
        public const int MessageId = 8014;
        
        public const byte CrcExtra = 200;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override GsmLinkStatusPayload Payload { get; } = new();

        public override string Name => "GSM_LINK_STATUS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("timestamp",
            "Timestamp (of OBC)",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("gsm_modem_type",
            "GSM modem used",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("gsm_link_type",
            "GSM link type",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("rssi",
            "RSSI as reported by modem (unconverted)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("rsrp_rscp",
            "RSRP (LTE) or RSCP (WCDMA) as reported by modem (unconverted)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("sinr_ecio",
            "SINR (LTE) or ECIO (WCDMA) as reported by modem (unconverted)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("rsrq",
            "RSRQ (LTE only) as reported by modem (unconverted)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "GSM_LINK_STATUS:"
        + "uint64_t timestamp;"
        + "uint8_t gsm_modem_type;"
        + "uint8_t gsm_link_type;"
        + "uint8_t rssi;"
        + "uint8_t rsrp_rscp;"
        + "uint8_t sinr_ecio;"
        + "uint8_t rsrq;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  GSM_LINK_STATUS
    /// </summary>
    public class GsmLinkStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 14; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 14; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Timestamp
            sum+= 1; // GsmModemType
            sum+= 1; // GsmLinkType
            sum+=1; //Rssi
            sum+=1; //RsrpRscp
            sum+=1; //SinrEcio
            sum+=1; //Rsrq
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Timestamp = BinSerialize.ReadULong(ref buffer);
            GsmModemType = (GsmModemType)BinSerialize.ReadByte(ref buffer);
            GsmLinkType = (GsmLinkType)BinSerialize.ReadByte(ref buffer);
            Rssi = (byte)BinSerialize.ReadByte(ref buffer);
            RsrpRscp = (byte)BinSerialize.ReadByte(ref buffer);
            SinrEcio = (byte)BinSerialize.ReadByte(ref buffer);
            Rsrq = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Timestamp);
            BinSerialize.WriteByte(ref buffer,(byte)GsmModemType);
            BinSerialize.WriteByte(ref buffer,(byte)GsmLinkType);
            BinSerialize.WriteByte(ref buffer,(byte)Rssi);
            BinSerialize.WriteByte(ref buffer,(byte)RsrpRscp);
            BinSerialize.WriteByte(ref buffer,(byte)SinrEcio);
            BinSerialize.WriteByte(ref buffer,(byte)Rsrq);
            /* PayloadByteSize = 14 */;
        }
        
        



        /// <summary>
        /// Timestamp (of OBC)
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public ulong Timestamp { get; set; }
        /// <summary>
        /// GSM modem used
        /// OriginName: gsm_modem_type, Units: , IsExtended: false
        /// </summary>
        public GsmModemType GsmModemType { get; set; }
        /// <summary>
        /// GSM link type
        /// OriginName: gsm_link_type, Units: , IsExtended: false
        /// </summary>
        public GsmLinkType GsmLinkType { get; set; }
        /// <summary>
        /// RSSI as reported by modem (unconverted)
        /// OriginName: rssi, Units: , IsExtended: false
        /// </summary>
        public byte Rssi { get; set; }
        /// <summary>
        /// RSRP (LTE) or RSCP (WCDMA) as reported by modem (unconverted)
        /// OriginName: rsrp_rscp, Units: , IsExtended: false
        /// </summary>
        public byte RsrpRscp { get; set; }
        /// <summary>
        /// SINR (LTE) or ECIO (WCDMA) as reported by modem (unconverted)
        /// OriginName: sinr_ecio, Units: , IsExtended: false
        /// </summary>
        public byte SinrEcio { get; set; }
        /// <summary>
        /// RSRQ (LTE only) as reported by modem (unconverted)
        /// OriginName: rsrq, Units: , IsExtended: false
        /// </summary>
        public byte Rsrq { get; set; }
    }
    /// <summary>
    /// Status of the SatCom link
    ///  SATCOM_LINK_STATUS
    /// </summary>
    public class SatcomLinkStatusPacket : MavlinkV2Message<SatcomLinkStatusPayload>
    {
        public const int MessageId = 8015;
        
        public const byte CrcExtra = 23;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SatcomLinkStatusPayload Payload { get; } = new();

        public override string Name => "SATCOM_LINK_STATUS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("timestamp",
            "Timestamp",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("last_heartbeat",
            "Timestamp of the last successful sbd session",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("failed_sessions",
            "Number of failed sessions",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("successful_sessions",
            "Number of successful sessions",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("signal_quality",
            "Signal quality",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("ring_pending",
            "Ring call pending",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("tx_session_pending",
            "Transmission session pending",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("rx_session_pending",
            "Receiving session pending",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "SATCOM_LINK_STATUS:"
        + "uint64_t timestamp;"
        + "uint64_t last_heartbeat;"
        + "uint16_t failed_sessions;"
        + "uint16_t successful_sessions;"
        + "uint8_t signal_quality;"
        + "uint8_t ring_pending;"
        + "uint8_t tx_session_pending;"
        + "uint8_t rx_session_pending;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  SATCOM_LINK_STATUS
    /// </summary>
    public class SatcomLinkStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 24; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Timestamp
            sum+=8; //LastHeartbeat
            sum+=2; //FailedSessions
            sum+=2; //SuccessfulSessions
            sum+=1; //SignalQuality
            sum+=1; //RingPending
            sum+=1; //TxSessionPending
            sum+=1; //RxSessionPending
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Timestamp = BinSerialize.ReadULong(ref buffer);
            LastHeartbeat = BinSerialize.ReadULong(ref buffer);
            FailedSessions = BinSerialize.ReadUShort(ref buffer);
            SuccessfulSessions = BinSerialize.ReadUShort(ref buffer);
            SignalQuality = (byte)BinSerialize.ReadByte(ref buffer);
            RingPending = (byte)BinSerialize.ReadByte(ref buffer);
            TxSessionPending = (byte)BinSerialize.ReadByte(ref buffer);
            RxSessionPending = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Timestamp);
            BinSerialize.WriteULong(ref buffer,LastHeartbeat);
            BinSerialize.WriteUShort(ref buffer,FailedSessions);
            BinSerialize.WriteUShort(ref buffer,SuccessfulSessions);
            BinSerialize.WriteByte(ref buffer,(byte)SignalQuality);
            BinSerialize.WriteByte(ref buffer,(byte)RingPending);
            BinSerialize.WriteByte(ref buffer,(byte)TxSessionPending);
            BinSerialize.WriteByte(ref buffer,(byte)RxSessionPending);
            /* PayloadByteSize = 24 */;
        }
        
        



        /// <summary>
        /// Timestamp
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public ulong Timestamp { get; set; }
        /// <summary>
        /// Timestamp of the last successful sbd session
        /// OriginName: last_heartbeat, Units: us, IsExtended: false
        /// </summary>
        public ulong LastHeartbeat { get; set; }
        /// <summary>
        /// Number of failed sessions
        /// OriginName: failed_sessions, Units: , IsExtended: false
        /// </summary>
        public ushort FailedSessions { get; set; }
        /// <summary>
        /// Number of successful sessions
        /// OriginName: successful_sessions, Units: , IsExtended: false
        /// </summary>
        public ushort SuccessfulSessions { get; set; }
        /// <summary>
        /// Signal quality
        /// OriginName: signal_quality, Units: , IsExtended: false
        /// </summary>
        public byte SignalQuality { get; set; }
        /// <summary>
        /// Ring call pending
        /// OriginName: ring_pending, Units: , IsExtended: false
        /// </summary>
        public byte RingPending { get; set; }
        /// <summary>
        /// Transmission session pending
        /// OriginName: tx_session_pending, Units: , IsExtended: false
        /// </summary>
        public byte TxSessionPending { get; set; }
        /// <summary>
        /// Receiving session pending
        /// OriginName: rx_session_pending, Units: , IsExtended: false
        /// </summary>
        public byte RxSessionPending { get; set; }
    }
    /// <summary>
    /// Calibrated airflow angle measurements
    ///  SENSOR_AIRFLOW_ANGLES
    /// </summary>
    public class SensorAirflowAnglesPacket : MavlinkV2Message<SensorAirflowAnglesPayload>
    {
        public const int MessageId = 8016;
        
        public const byte CrcExtra = 149;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SensorAirflowAnglesPayload Payload { get; } = new();

        public override string Name => "SENSOR_AIRFLOW_ANGLES";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("timestamp",
            "Timestamp",
            string.Empty, 
            @"us", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("angleofattack",
            "Angle of attack",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("sideslip",
            "Sideslip angle",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("angleofattack_valid",
            "Angle of attack measurement valid",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("sideslip_valid",
            "Sideslip angle measurement valid",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "SENSOR_AIRFLOW_ANGLES:"
        + "uint64_t timestamp;"
        + "float angleofattack;"
        + "float sideslip;"
        + "uint8_t angleofattack_valid;"
        + "uint8_t sideslip_valid;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  SENSOR_AIRFLOW_ANGLES
    /// </summary>
    public class SensorAirflowAnglesPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 18; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 18; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Timestamp
            sum+=4; //Angleofattack
            sum+=4; //Sideslip
            sum+=1; //AngleofattackValid
            sum+=1; //SideslipValid
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Timestamp = BinSerialize.ReadULong(ref buffer);
            Angleofattack = BinSerialize.ReadFloat(ref buffer);
            Sideslip = BinSerialize.ReadFloat(ref buffer);
            AngleofattackValid = (byte)BinSerialize.ReadByte(ref buffer);
            SideslipValid = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Timestamp);
            BinSerialize.WriteFloat(ref buffer,Angleofattack);
            BinSerialize.WriteFloat(ref buffer,Sideslip);
            BinSerialize.WriteByte(ref buffer,(byte)AngleofattackValid);
            BinSerialize.WriteByte(ref buffer,(byte)SideslipValid);
            /* PayloadByteSize = 18 */;
        }
        
        



        /// <summary>
        /// Timestamp
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public ulong Timestamp { get; set; }
        /// <summary>
        /// Angle of attack
        /// OriginName: angleofattack, Units: deg, IsExtended: false
        /// </summary>
        public float Angleofattack { get; set; }
        /// <summary>
        /// Sideslip angle
        /// OriginName: sideslip, Units: deg, IsExtended: false
        /// </summary>
        public float Sideslip { get; set; }
        /// <summary>
        /// Angle of attack measurement valid
        /// OriginName: angleofattack_valid, Units: , IsExtended: false
        /// </summary>
        public byte AngleofattackValid { get; set; }
        /// <summary>
        /// Sideslip angle measurement valid
        /// OriginName: sideslip_valid, Units: , IsExtended: false
        /// </summary>
        public byte SideslipValid { get; set; }
    }


#endregion


}

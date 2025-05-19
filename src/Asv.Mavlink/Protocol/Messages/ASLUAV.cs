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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t vehicle_timestamp
            +4 // uint32_t utc_time
            +4 // float param1
            +4 // float param2
            +4 // float param3
            +4 // float param4
            +4 // int32_t x
            +4 // int32_t y
            +4 // float z
            + 2 // uint16_t command
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            + 1 // uint8_t frame
            +1 // uint8_t current
            +1 // uint8_t autocontinue
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,VehicleTimestampField, ref _VehicleTimestamp);    
            UInt32Type.Accept(visitor,UtcTimeField, ref _UtcTime);    
            FloatType.Accept(visitor,Param1Field, ref _Param1);    
            FloatType.Accept(visitor,Param2Field, ref _Param2);    
            FloatType.Accept(visitor,Param3Field, ref _Param3);    
            FloatType.Accept(visitor,Param4Field, ref _Param4);    
            Int32Type.Accept(visitor,XField, ref _X);    
            Int32Type.Accept(visitor,YField, ref _Y);    
            FloatType.Accept(visitor,ZField, ref _Z);    
            var tmpCommand = (ushort)Command;
            UInt16Type.Accept(visitor,CommandField, ref tmpCommand);
            Command = (MavCmd)tmpCommand;
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            var tmpFrame = (byte)Frame;
            UInt8Type.Accept(visitor,FrameField, ref tmpFrame);
            Frame = (MavFrame)tmpFrame;
            UInt8Type.Accept(visitor,CurrentField, ref _Current);    
            UInt8Type.Accept(visitor,AutocontinueField, ref _Autocontinue);    

        }

        /// <summary>
        /// Microseconds elapsed since vehicle boot
        /// OriginName: vehicle_timestamp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VehicleTimestampField = new Field.Builder()
            .Name(nameof(VehicleTimestamp))
            .Title("vehicle_timestamp")
            .Description("Microseconds elapsed since vehicle boot")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _VehicleTimestamp;
        public ulong VehicleTimestamp { get => _VehicleTimestamp; set { _VehicleTimestamp = value; } }
        /// <summary>
        /// UTC time, seconds elapsed since 01.01.1970
        /// OriginName: utc_time, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UtcTimeField = new Field.Builder()
            .Name(nameof(UtcTime))
            .Title("utc_time")
            .Description("UTC time, seconds elapsed since 01.01.1970")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _UtcTime;
        public uint UtcTime { get => _UtcTime; set { _UtcTime = value; } }
        /// <summary>
        /// PARAM1, see MAV_CMD enum
        /// OriginName: param1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param1Field = new Field.Builder()
            .Name(nameof(Param1))
            .Title("param1")
            .Description("PARAM1, see MAV_CMD enum")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param1;
        public float Param1 { get => _Param1; set { _Param1 = value; } }
        /// <summary>
        /// PARAM2, see MAV_CMD enum
        /// OriginName: param2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param2Field = new Field.Builder()
            .Name(nameof(Param2))
            .Title("param2")
            .Description("PARAM2, see MAV_CMD enum")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param2;
        public float Param2 { get => _Param2; set { _Param2 = value; } }
        /// <summary>
        /// PARAM3, see MAV_CMD enum
        /// OriginName: param3, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param3Field = new Field.Builder()
            .Name(nameof(Param3))
            .Title("param3")
            .Description("PARAM3, see MAV_CMD enum")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param3;
        public float Param3 { get => _Param3; set { _Param3 = value; } }
        /// <summary>
        /// PARAM4, see MAV_CMD enum
        /// OriginName: param4, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param4Field = new Field.Builder()
            .Name(nameof(Param4))
            .Title("param4")
            .Description("PARAM4, see MAV_CMD enum")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param4;
        public float Param4 { get => _Param4; set { _Param4 = value; } }
        /// <summary>
        /// PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7
        /// OriginName: x, Units: , IsExtended: false
        /// </summary>
        public static readonly Field XField = new Field.Builder()
            .Name(nameof(X))
            .Title("x")
            .Description("PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int32Type.Default)

            .Build();
        private int _X;
        public int X { get => _X; set { _X = value; } }
        /// <summary>
        /// PARAM6 / local: y position in meters * 1e4, global: longitude in degrees * 10^7
        /// OriginName: y, Units: , IsExtended: false
        /// </summary>
        public static readonly Field YField = new Field.Builder()
            .Name(nameof(Y))
            .Title("y")
            .Description("PARAM6 / local: y position in meters * 1e4, global: longitude in degrees * 10^7")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int32Type.Default)

            .Build();
        private int _Y;
        public int Y { get => _Y; set { _Y = value; } }
        /// <summary>
        /// PARAM7 / z position: global: altitude in meters (MSL, WGS84, AGL or relative to home - depending on frame).
        /// OriginName: z, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ZField = new Field.Builder()
            .Name(nameof(Z))
            .Title("z")
            .Description("PARAM7 / z position: global: altitude in meters (MSL, WGS84, AGL or relative to home - depending on frame).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Z;
        public float Z { get => _Z; set { _Z = value; } }
        /// <summary>
        /// The scheduled action for the mission item, as defined by MAV_CMD enum
        /// OriginName: command, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CommandField = new Field.Builder()
            .Name(nameof(Command))
            .Title("command")
            .Description("The scheduled action for the mission item, as defined by MAV_CMD enum")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        public MavCmd _Command;
        public MavCmd Command { get => _Command; set => _Command = value; } 
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// The coordinate system of the COMMAND, as defined by MAV_FRAME enum
        /// OriginName: frame, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FrameField = new Field.Builder()
            .Name(nameof(Frame))
            .Title("frame")
            .Description("The coordinate system of the COMMAND, as defined by MAV_FRAME enum")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public MavFrame _Frame;
        public MavFrame Frame { get => _Frame; set => _Frame = value; } 
        /// <summary>
        /// false:0, true:1
        /// OriginName: current, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CurrentField = new Field.Builder()
            .Name(nameof(Current))
            .Title("current")
            .Description("false:0, true:1")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Current;
        public byte Current { get => _Current; set { _Current = value; } }
        /// <summary>
        /// autocontinue to next wp
        /// OriginName: autocontinue, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AutocontinueField = new Field.Builder()
            .Name(nameof(Autocontinue))
            .Title("autocontinue")
            .Description("autocontinue to next wp")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Autocontinue;
        public byte Autocontinue { get => _Autocontinue; set { _Autocontinue = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t vehicle_timestamp
            +4 // uint32_t utc_time
            +4 // float param1
            +4 // float param2
            +4 // float param3
            +4 // float param4
            +4 // float param5
            +4 // float param6
            +4 // float param7
            + 2 // uint16_t command
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t confirmation
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,VehicleTimestampField, ref _VehicleTimestamp);    
            UInt32Type.Accept(visitor,UtcTimeField, ref _UtcTime);    
            FloatType.Accept(visitor,Param1Field, ref _Param1);    
            FloatType.Accept(visitor,Param2Field, ref _Param2);    
            FloatType.Accept(visitor,Param3Field, ref _Param3);    
            FloatType.Accept(visitor,Param4Field, ref _Param4);    
            FloatType.Accept(visitor,Param5Field, ref _Param5);    
            FloatType.Accept(visitor,Param6Field, ref _Param6);    
            FloatType.Accept(visitor,Param7Field, ref _Param7);    
            var tmpCommand = (ushort)Command;
            UInt16Type.Accept(visitor,CommandField, ref tmpCommand);
            Command = (MavCmd)tmpCommand;
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,ConfirmationField, ref _Confirmation);    

        }

        /// <summary>
        /// Microseconds elapsed since vehicle boot
        /// OriginName: vehicle_timestamp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VehicleTimestampField = new Field.Builder()
            .Name(nameof(VehicleTimestamp))
            .Title("vehicle_timestamp")
            .Description("Microseconds elapsed since vehicle boot")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _VehicleTimestamp;
        public ulong VehicleTimestamp { get => _VehicleTimestamp; set { _VehicleTimestamp = value; } }
        /// <summary>
        /// UTC time, seconds elapsed since 01.01.1970
        /// OriginName: utc_time, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UtcTimeField = new Field.Builder()
            .Name(nameof(UtcTime))
            .Title("utc_time")
            .Description("UTC time, seconds elapsed since 01.01.1970")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _UtcTime;
        public uint UtcTime { get => _UtcTime; set { _UtcTime = value; } }
        /// <summary>
        /// Parameter 1, as defined by MAV_CMD enum.
        /// OriginName: param1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param1Field = new Field.Builder()
            .Name(nameof(Param1))
            .Title("param1")
            .Description("Parameter 1, as defined by MAV_CMD enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param1;
        public float Param1 { get => _Param1; set { _Param1 = value; } }
        /// <summary>
        /// Parameter 2, as defined by MAV_CMD enum.
        /// OriginName: param2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param2Field = new Field.Builder()
            .Name(nameof(Param2))
            .Title("param2")
            .Description("Parameter 2, as defined by MAV_CMD enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param2;
        public float Param2 { get => _Param2; set { _Param2 = value; } }
        /// <summary>
        /// Parameter 3, as defined by MAV_CMD enum.
        /// OriginName: param3, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param3Field = new Field.Builder()
            .Name(nameof(Param3))
            .Title("param3")
            .Description("Parameter 3, as defined by MAV_CMD enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param3;
        public float Param3 { get => _Param3; set { _Param3 = value; } }
        /// <summary>
        /// Parameter 4, as defined by MAV_CMD enum.
        /// OriginName: param4, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param4Field = new Field.Builder()
            .Name(nameof(Param4))
            .Title("param4")
            .Description("Parameter 4, as defined by MAV_CMD enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param4;
        public float Param4 { get => _Param4; set { _Param4 = value; } }
        /// <summary>
        /// Parameter 5, as defined by MAV_CMD enum.
        /// OriginName: param5, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param5Field = new Field.Builder()
            .Name(nameof(Param5))
            .Title("param5")
            .Description("Parameter 5, as defined by MAV_CMD enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param5;
        public float Param5 { get => _Param5; set { _Param5 = value; } }
        /// <summary>
        /// Parameter 6, as defined by MAV_CMD enum.
        /// OriginName: param6, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param6Field = new Field.Builder()
            .Name(nameof(Param6))
            .Title("param6")
            .Description("Parameter 6, as defined by MAV_CMD enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param6;
        public float Param6 { get => _Param6; set { _Param6 = value; } }
        /// <summary>
        /// Parameter 7, as defined by MAV_CMD enum.
        /// OriginName: param7, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Param7Field = new Field.Builder()
            .Name(nameof(Param7))
            .Title("param7")
            .Description("Parameter 7, as defined by MAV_CMD enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Param7;
        public float Param7 { get => _Param7; set { _Param7 = value; } }
        /// <summary>
        /// Command ID, as defined by MAV_CMD enum.
        /// OriginName: command, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CommandField = new Field.Builder()
            .Name(nameof(Command))
            .Title("command")
            .Description("Command ID, as defined by MAV_CMD enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        public MavCmd _Command;
        public MavCmd Command { get => _Command; set => _Command = value; } 
        /// <summary>
        /// System which should execute the command
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System which should execute the command")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component which should execute the command, 0 for all components
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component which should execute the command, 0 for all components")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// 0: First transmission of this command. 1-255: Confirmation transmissions (e.g. for kill command)
        /// OriginName: confirmation, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ConfirmationField = new Field.Builder()
            .Name(nameof(Confirmation))
            .Title("confirmation")
            .Description("0: First transmission of this command. 1-255: Confirmation transmissions (e.g. for kill command)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Confirmation;
        public byte Confirmation { get => _Confirmation; set { _Confirmation = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float adc121_vspb_volt
            +4 // float adc121_cspb_amp
            +4 // float adc121_cs1_amp
            +4 // float adc121_cs2_amp
            );
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

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,Adc121VspbVoltField, ref _Adc121VspbVolt);    
            FloatType.Accept(visitor,Adc121CspbAmpField, ref _Adc121CspbAmp);    
            FloatType.Accept(visitor,Adc121Cs1AmpField, ref _Adc121Cs1Amp);    
            FloatType.Accept(visitor,Adc121Cs2AmpField, ref _Adc121Cs2Amp);    

        }

        /// <summary>
        ///  Power board voltage sensor reading
        /// OriginName: adc121_vspb_volt, Units: V, IsExtended: false
        /// </summary>
        public static readonly Field Adc121VspbVoltField = new Field.Builder()
            .Name(nameof(Adc121VspbVolt))
            .Title("adc121_vspb_volt")
            .Description(" Power board voltage sensor reading")
            .FormatString(string.Empty)
            .Units(@"V")
            .DataType(FloatType.Default)

            .Build();
        private float _Adc121VspbVolt;
        public float Adc121VspbVolt { get => _Adc121VspbVolt; set { _Adc121VspbVolt = value; } }
        /// <summary>
        ///  Power board current sensor reading
        /// OriginName: adc121_cspb_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field Adc121CspbAmpField = new Field.Builder()
            .Name(nameof(Adc121CspbAmp))
            .Title("adc121_cspb_amp")
            .Description(" Power board current sensor reading")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _Adc121CspbAmp;
        public float Adc121CspbAmp { get => _Adc121CspbAmp; set { _Adc121CspbAmp = value; } }
        /// <summary>
        ///  Board current sensor 1 reading
        /// OriginName: adc121_cs1_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field Adc121Cs1AmpField = new Field.Builder()
            .Name(nameof(Adc121Cs1Amp))
            .Title("adc121_cs1_amp")
            .Description(" Board current sensor 1 reading")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _Adc121Cs1Amp;
        public float Adc121Cs1Amp { get => _Adc121Cs1Amp; set { _Adc121Cs1Amp = value; } }
        /// <summary>
        ///  Board current sensor 2 reading
        /// OriginName: adc121_cs2_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field Adc121Cs2AmpField = new Field.Builder()
            .Name(nameof(Adc121Cs2Amp))
            .Title("adc121_cs2_amp")
            .Description(" Board current sensor 2 reading")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _Adc121Cs2Amp;
        public float Adc121Cs2Amp { get => _Adc121Cs2Amp; set { _Adc121Cs2Amp = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t mppt_timestamp
            +4 // float mppt1_volt
            +4 // float mppt1_amp
            +4 // float mppt2_volt
            +4 // float mppt2_amp
            +4 // float mppt3_volt
            +4 // float mppt3_amp
            +2 // uint16_t mppt1_pwm
            +2 // uint16_t mppt2_pwm
            +2 // uint16_t mppt3_pwm
            +1 // uint8_t mppt1_status
            +1 // uint8_t mppt2_status
            +1 // uint8_t mppt3_status
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,MpptTimestampField, ref _MpptTimestamp);    
            FloatType.Accept(visitor,Mppt1VoltField, ref _Mppt1Volt);    
            FloatType.Accept(visitor,Mppt1AmpField, ref _Mppt1Amp);    
            FloatType.Accept(visitor,Mppt2VoltField, ref _Mppt2Volt);    
            FloatType.Accept(visitor,Mppt2AmpField, ref _Mppt2Amp);    
            FloatType.Accept(visitor,Mppt3VoltField, ref _Mppt3Volt);    
            FloatType.Accept(visitor,Mppt3AmpField, ref _Mppt3Amp);    
            UInt16Type.Accept(visitor,Mppt1PwmField, ref _Mppt1Pwm);    
            UInt16Type.Accept(visitor,Mppt2PwmField, ref _Mppt2Pwm);    
            UInt16Type.Accept(visitor,Mppt3PwmField, ref _Mppt3Pwm);    
            UInt8Type.Accept(visitor,Mppt1StatusField, ref _Mppt1Status);    
            UInt8Type.Accept(visitor,Mppt2StatusField, ref _Mppt2Status);    
            UInt8Type.Accept(visitor,Mppt3StatusField, ref _Mppt3Status);    

        }

        /// <summary>
        ///  MPPT last timestamp 
        /// OriginName: mppt_timestamp, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field MpptTimestampField = new Field.Builder()
            .Name(nameof(MpptTimestamp))
            .Title("mppt_timestamp")
            .Description(" MPPT last timestamp ")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _MpptTimestamp;
        public ulong MpptTimestamp { get => _MpptTimestamp; set { _MpptTimestamp = value; } }
        /// <summary>
        ///  MPPT1 voltage 
        /// OriginName: mppt1_volt, Units: V, IsExtended: false
        /// </summary>
        public static readonly Field Mppt1VoltField = new Field.Builder()
            .Name(nameof(Mppt1Volt))
            .Title("mppt1_volt")
            .Description(" MPPT1 voltage ")
            .FormatString(string.Empty)
            .Units(@"V")
            .DataType(FloatType.Default)

            .Build();
        private float _Mppt1Volt;
        public float Mppt1Volt { get => _Mppt1Volt; set { _Mppt1Volt = value; } }
        /// <summary>
        ///  MPPT1 current 
        /// OriginName: mppt1_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field Mppt1AmpField = new Field.Builder()
            .Name(nameof(Mppt1Amp))
            .Title("mppt1_amp")
            .Description(" MPPT1 current ")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _Mppt1Amp;
        public float Mppt1Amp { get => _Mppt1Amp; set { _Mppt1Amp = value; } }
        /// <summary>
        ///  MPPT2 voltage 
        /// OriginName: mppt2_volt, Units: V, IsExtended: false
        /// </summary>
        public static readonly Field Mppt2VoltField = new Field.Builder()
            .Name(nameof(Mppt2Volt))
            .Title("mppt2_volt")
            .Description(" MPPT2 voltage ")
            .FormatString(string.Empty)
            .Units(@"V")
            .DataType(FloatType.Default)

            .Build();
        private float _Mppt2Volt;
        public float Mppt2Volt { get => _Mppt2Volt; set { _Mppt2Volt = value; } }
        /// <summary>
        ///  MPPT2 current 
        /// OriginName: mppt2_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field Mppt2AmpField = new Field.Builder()
            .Name(nameof(Mppt2Amp))
            .Title("mppt2_amp")
            .Description(" MPPT2 current ")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _Mppt2Amp;
        public float Mppt2Amp { get => _Mppt2Amp; set { _Mppt2Amp = value; } }
        /// <summary>
        /// MPPT3 voltage 
        /// OriginName: mppt3_volt, Units: V, IsExtended: false
        /// </summary>
        public static readonly Field Mppt3VoltField = new Field.Builder()
            .Name(nameof(Mppt3Volt))
            .Title("mppt3_volt")
            .Description("MPPT3 voltage ")
            .FormatString(string.Empty)
            .Units(@"V")
            .DataType(FloatType.Default)

            .Build();
        private float _Mppt3Volt;
        public float Mppt3Volt { get => _Mppt3Volt; set { _Mppt3Volt = value; } }
        /// <summary>
        ///  MPPT3 current 
        /// OriginName: mppt3_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field Mppt3AmpField = new Field.Builder()
            .Name(nameof(Mppt3Amp))
            .Title("mppt3_amp")
            .Description(" MPPT3 current ")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _Mppt3Amp;
        public float Mppt3Amp { get => _Mppt3Amp; set { _Mppt3Amp = value; } }
        /// <summary>
        ///  MPPT1 pwm 
        /// OriginName: mppt1_pwm, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field Mppt1PwmField = new Field.Builder()
            .Name(nameof(Mppt1Pwm))
            .Title("mppt1_pwm")
            .Description(" MPPT1 pwm ")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Mppt1Pwm;
        public ushort Mppt1Pwm { get => _Mppt1Pwm; set { _Mppt1Pwm = value; } }
        /// <summary>
        ///  MPPT2 pwm 
        /// OriginName: mppt2_pwm, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field Mppt2PwmField = new Field.Builder()
            .Name(nameof(Mppt2Pwm))
            .Title("mppt2_pwm")
            .Description(" MPPT2 pwm ")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Mppt2Pwm;
        public ushort Mppt2Pwm { get => _Mppt2Pwm; set { _Mppt2Pwm = value; } }
        /// <summary>
        ///  MPPT3 pwm 
        /// OriginName: mppt3_pwm, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field Mppt3PwmField = new Field.Builder()
            .Name(nameof(Mppt3Pwm))
            .Title("mppt3_pwm")
            .Description(" MPPT3 pwm ")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Mppt3Pwm;
        public ushort Mppt3Pwm { get => _Mppt3Pwm; set { _Mppt3Pwm = value; } }
        /// <summary>
        ///  MPPT1 status 
        /// OriginName: mppt1_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Mppt1StatusField = new Field.Builder()
            .Name(nameof(Mppt1Status))
            .Title("mppt1_status")
            .Description(" MPPT1 status ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Mppt1Status;
        public byte Mppt1Status { get => _Mppt1Status; set { _Mppt1Status = value; } }
        /// <summary>
        ///  MPPT2 status 
        /// OriginName: mppt2_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Mppt2StatusField = new Field.Builder()
            .Name(nameof(Mppt2Status))
            .Title("mppt2_status")
            .Description(" MPPT2 status ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Mppt2Status;
        public byte Mppt2Status { get => _Mppt2Status; set { _Mppt2Status = value; } }
        /// <summary>
        ///  MPPT3 status 
        /// OriginName: mppt3_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Mppt3StatusField = new Field.Builder()
            .Name(nameof(Mppt3Status))
            .Title("mppt3_status")
            .Description(" MPPT3 status ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Mppt3Status;
        public byte Mppt3Status { get => _Mppt3Status; set { _Mppt3Status = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t timestamp
            +4 // float h
            +4 // float hRef
            +4 // float hRef_t
            +4 // float PitchAngle
            +4 // float PitchAngleRef
            +4 // float q
            +4 // float qRef
            +4 // float uElev
            +4 // float uThrot
            +4 // float uThrot2
            +4 // float nZ
            +4 // float AirspeedRef
            +4 // float YawAngle
            +4 // float YawAngleRef
            +4 // float RollAngle
            +4 // float RollAngleRef
            +4 // float p
            +4 // float pRef
            +4 // float r
            +4 // float rRef
            +4 // float uAil
            +4 // float uRud
            +1 // uint8_t aslctrl_mode
            +1 // uint8_t SpoilersEngaged
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimestampField, ref _Timestamp);    
            FloatType.Accept(visitor,HField, ref _H);    
            FloatType.Accept(visitor,HrefField, ref _Href);    
            FloatType.Accept(visitor,HrefTField, ref _HrefT);    
            FloatType.Accept(visitor,PitchangleField, ref _Pitchangle);    
            FloatType.Accept(visitor,PitchanglerefField, ref _Pitchangleref);    
            FloatType.Accept(visitor,QField, ref _Q);    
            FloatType.Accept(visitor,QrefField, ref _Qref);    
            FloatType.Accept(visitor,UelevField, ref _Uelev);    
            FloatType.Accept(visitor,UthrotField, ref _Uthrot);    
            FloatType.Accept(visitor,Uthrot2Field, ref _Uthrot2);    
            FloatType.Accept(visitor,NzField, ref _Nz);    
            FloatType.Accept(visitor,AirspeedrefField, ref _Airspeedref);    
            FloatType.Accept(visitor,YawangleField, ref _Yawangle);    
            FloatType.Accept(visitor,YawanglerefField, ref _Yawangleref);    
            FloatType.Accept(visitor,RollangleField, ref _Rollangle);    
            FloatType.Accept(visitor,RollanglerefField, ref _Rollangleref);    
            FloatType.Accept(visitor,PField, ref _P);    
            FloatType.Accept(visitor,PrefField, ref _Pref);    
            FloatType.Accept(visitor,RField, ref _R);    
            FloatType.Accept(visitor,RrefField, ref _Rref);    
            FloatType.Accept(visitor,UailField, ref _Uail);    
            FloatType.Accept(visitor,UrudField, ref _Urud);    
            UInt8Type.Accept(visitor,AslctrlModeField, ref _AslctrlMode);    
            UInt8Type.Accept(visitor,SpoilersengagedField, ref _Spoilersengaged);    

        }

        /// <summary>
        ///  Timestamp
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimestampField = new Field.Builder()
            .Name(nameof(Timestamp))
            .Title("timestamp")
            .Description(" Timestamp")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestamp;
        public ulong Timestamp { get => _Timestamp; set { _Timestamp = value; } }
        /// <summary>
        ///  See sourcecode for a description of these values... 
        /// OriginName: h, Units: , IsExtended: false
        /// </summary>
        public static readonly Field HField = new Field.Builder()
            .Name(nameof(H))
            .Title("h")
            .Description(" See sourcecode for a description of these values... ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _H;
        public float H { get => _H; set { _H = value; } }
        /// <summary>
        ///  
        /// OriginName: hRef, Units: , IsExtended: false
        /// </summary>
        public static readonly Field HrefField = new Field.Builder()
            .Name(nameof(Href))
            .Title("hRef")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Href;
        public float Href { get => _Href; set { _Href = value; } }
        /// <summary>
        ///  
        /// OriginName: hRef_t, Units: , IsExtended: false
        /// </summary>
        public static readonly Field HrefTField = new Field.Builder()
            .Name(nameof(HrefT))
            .Title("hRef_t")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _HrefT;
        public float HrefT { get => _HrefT; set { _HrefT = value; } }
        /// <summary>
        /// Pitch angle
        /// OriginName: PitchAngle, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field PitchangleField = new Field.Builder()
            .Name(nameof(Pitchangle))
            .Title("PitchAngle")
            .Description("Pitch angle")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Pitchangle;
        public float Pitchangle { get => _Pitchangle; set { _Pitchangle = value; } }
        /// <summary>
        /// Pitch angle reference
        /// OriginName: PitchAngleRef, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field PitchanglerefField = new Field.Builder()
            .Name(nameof(Pitchangleref))
            .Title("PitchAngleRef")
            .Description("Pitch angle reference")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Pitchangleref;
        public float Pitchangleref { get => _Pitchangleref; set { _Pitchangleref = value; } }
        /// <summary>
        ///  
        /// OriginName: q, Units: , IsExtended: false
        /// </summary>
        public static readonly Field QField = new Field.Builder()
            .Name(nameof(Q))
            .Title("q")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Q;
        public float Q { get => _Q; set { _Q = value; } }
        /// <summary>
        ///  
        /// OriginName: qRef, Units: , IsExtended: false
        /// </summary>
        public static readonly Field QrefField = new Field.Builder()
            .Name(nameof(Qref))
            .Title("qRef")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Qref;
        public float Qref { get => _Qref; set { _Qref = value; } }
        /// <summary>
        ///  
        /// OriginName: uElev, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UelevField = new Field.Builder()
            .Name(nameof(Uelev))
            .Title("uElev")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Uelev;
        public float Uelev { get => _Uelev; set { _Uelev = value; } }
        /// <summary>
        ///  
        /// OriginName: uThrot, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UthrotField = new Field.Builder()
            .Name(nameof(Uthrot))
            .Title("uThrot")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Uthrot;
        public float Uthrot { get => _Uthrot; set { _Uthrot = value; } }
        /// <summary>
        ///  
        /// OriginName: uThrot2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Uthrot2Field = new Field.Builder()
            .Name(nameof(Uthrot2))
            .Title("uThrot2")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Uthrot2;
        public float Uthrot2 { get => _Uthrot2; set { _Uthrot2 = value; } }
        /// <summary>
        ///  
        /// OriginName: nZ, Units: , IsExtended: false
        /// </summary>
        public static readonly Field NzField = new Field.Builder()
            .Name(nameof(Nz))
            .Title("nZ")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Nz;
        public float Nz { get => _Nz; set { _Nz = value; } }
        /// <summary>
        /// Airspeed reference
        /// OriginName: AirspeedRef, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field AirspeedrefField = new Field.Builder()
            .Name(nameof(Airspeedref))
            .Title("AirspeedRef")
            .Description("Airspeed reference")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Airspeedref;
        public float Airspeedref { get => _Airspeedref; set { _Airspeedref = value; } }
        /// <summary>
        /// Yaw angle
        /// OriginName: YawAngle, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field YawangleField = new Field.Builder()
            .Name(nameof(Yawangle))
            .Title("YawAngle")
            .Description("Yaw angle")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Yawangle;
        public float Yawangle { get => _Yawangle; set { _Yawangle = value; } }
        /// <summary>
        /// Yaw angle reference
        /// OriginName: YawAngleRef, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field YawanglerefField = new Field.Builder()
            .Name(nameof(Yawangleref))
            .Title("YawAngleRef")
            .Description("Yaw angle reference")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Yawangleref;
        public float Yawangleref { get => _Yawangleref; set { _Yawangleref = value; } }
        /// <summary>
        /// Roll angle
        /// OriginName: RollAngle, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field RollangleField = new Field.Builder()
            .Name(nameof(Rollangle))
            .Title("RollAngle")
            .Description("Roll angle")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Rollangle;
        public float Rollangle { get => _Rollangle; set { _Rollangle = value; } }
        /// <summary>
        /// Roll angle reference
        /// OriginName: RollAngleRef, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field RollanglerefField = new Field.Builder()
            .Name(nameof(Rollangleref))
            .Title("RollAngleRef")
            .Description("Roll angle reference")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Rollangleref;
        public float Rollangleref { get => _Rollangleref; set { _Rollangleref = value; } }
        /// <summary>
        ///  
        /// OriginName: p, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PField = new Field.Builder()
            .Name(nameof(P))
            .Title("p")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _P;
        public float P { get => _P; set { _P = value; } }
        /// <summary>
        ///  
        /// OriginName: pRef, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PrefField = new Field.Builder()
            .Name(nameof(Pref))
            .Title("pRef")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Pref;
        public float Pref { get => _Pref; set { _Pref = value; } }
        /// <summary>
        ///  
        /// OriginName: r, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RField = new Field.Builder()
            .Name(nameof(R))
            .Title("r")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _R;
        public float R { get => _R; set { _R = value; } }
        /// <summary>
        ///  
        /// OriginName: rRef, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RrefField = new Field.Builder()
            .Name(nameof(Rref))
            .Title("rRef")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Rref;
        public float Rref { get => _Rref; set { _Rref = value; } }
        /// <summary>
        ///  
        /// OriginName: uAil, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UailField = new Field.Builder()
            .Name(nameof(Uail))
            .Title("uAil")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Uail;
        public float Uail { get => _Uail; set { _Uail = value; } }
        /// <summary>
        ///  
        /// OriginName: uRud, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UrudField = new Field.Builder()
            .Name(nameof(Urud))
            .Title("uRud")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Urud;
        public float Urud { get => _Urud; set { _Urud = value; } }
        /// <summary>
        ///  ASLCTRL control-mode (manual, stabilized, auto, etc...)
        /// OriginName: aslctrl_mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AslctrlModeField = new Field.Builder()
            .Name(nameof(AslctrlMode))
            .Title("aslctrl_mode")
            .Description(" ASLCTRL control-mode (manual, stabilized, auto, etc...)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _AslctrlMode;
        public byte AslctrlMode { get => _AslctrlMode; set { _AslctrlMode = value; } }
        /// <summary>
        ///  
        /// OriginName: SpoilersEngaged, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SpoilersengagedField = new Field.Builder()
            .Name(nameof(Spoilersengaged))
            .Title("SpoilersEngaged")
            .Description(" ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Spoilersengaged;
        public byte Spoilersengaged { get => _Spoilersengaged; set { _Spoilersengaged = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t i32_1
            +4 // float f_1
            +4 // float f_2
            +4 // float f_3
            +4 // float f_4
            +4 // float f_5
            +4 // float f_6
            +4 // float f_7
            +4 // float f_8
            +1 // uint8_t i8_1
            +1 // uint8_t i8_2
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,I321Field, ref _I321);    
            FloatType.Accept(visitor,F1Field, ref _F1);    
            FloatType.Accept(visitor,F2Field, ref _F2);    
            FloatType.Accept(visitor,F3Field, ref _F3);    
            FloatType.Accept(visitor,F4Field, ref _F4);    
            FloatType.Accept(visitor,F5Field, ref _F5);    
            FloatType.Accept(visitor,F6Field, ref _F6);    
            FloatType.Accept(visitor,F7Field, ref _F7);    
            FloatType.Accept(visitor,F8Field, ref _F8);    
            UInt8Type.Accept(visitor,I81Field, ref _I81);    
            UInt8Type.Accept(visitor,I82Field, ref _I82);    

        }

        /// <summary>
        ///  Debug data
        /// OriginName: i32_1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field I321Field = new Field.Builder()
            .Name(nameof(I321))
            .Title("i32_1")
            .Description(" Debug data")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _I321;
        public uint I321 { get => _I321; set { _I321 = value; } }
        /// <summary>
        ///  Debug data 
        /// OriginName: f_1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field F1Field = new Field.Builder()
            .Name(nameof(F1))
            .Title("f_1")
            .Description(" Debug data ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _F1;
        public float F1 { get => _F1; set { _F1 = value; } }
        /// <summary>
        ///  Debug data
        /// OriginName: f_2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field F2Field = new Field.Builder()
            .Name(nameof(F2))
            .Title("f_2")
            .Description(" Debug data")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _F2;
        public float F2 { get => _F2; set { _F2 = value; } }
        /// <summary>
        ///  Debug data
        /// OriginName: f_3, Units: , IsExtended: false
        /// </summary>
        public static readonly Field F3Field = new Field.Builder()
            .Name(nameof(F3))
            .Title("f_3")
            .Description(" Debug data")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _F3;
        public float F3 { get => _F3; set { _F3 = value; } }
        /// <summary>
        ///  Debug data
        /// OriginName: f_4, Units: , IsExtended: false
        /// </summary>
        public static readonly Field F4Field = new Field.Builder()
            .Name(nameof(F4))
            .Title("f_4")
            .Description(" Debug data")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _F4;
        public float F4 { get => _F4; set { _F4 = value; } }
        /// <summary>
        ///  Debug data
        /// OriginName: f_5, Units: , IsExtended: false
        /// </summary>
        public static readonly Field F5Field = new Field.Builder()
            .Name(nameof(F5))
            .Title("f_5")
            .Description(" Debug data")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _F5;
        public float F5 { get => _F5; set { _F5 = value; } }
        /// <summary>
        ///  Debug data
        /// OriginName: f_6, Units: , IsExtended: false
        /// </summary>
        public static readonly Field F6Field = new Field.Builder()
            .Name(nameof(F6))
            .Title("f_6")
            .Description(" Debug data")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _F6;
        public float F6 { get => _F6; set { _F6 = value; } }
        /// <summary>
        ///  Debug data
        /// OriginName: f_7, Units: , IsExtended: false
        /// </summary>
        public static readonly Field F7Field = new Field.Builder()
            .Name(nameof(F7))
            .Title("f_7")
            .Description(" Debug data")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _F7;
        public float F7 { get => _F7; set { _F7 = value; } }
        /// <summary>
        ///  Debug data
        /// OriginName: f_8, Units: , IsExtended: false
        /// </summary>
        public static readonly Field F8Field = new Field.Builder()
            .Name(nameof(F8))
            .Title("f_8")
            .Description(" Debug data")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _F8;
        public float F8 { get => _F8; set { _F8 = value; } }
        /// <summary>
        ///  Debug data
        /// OriginName: i8_1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field I81Field = new Field.Builder()
            .Name(nameof(I81))
            .Title("i8_1")
            .Description(" Debug data")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _I81;
        public byte I81 { get => _I81; set { _I81 = value; } }
        /// <summary>
        ///  Debug data
        /// OriginName: i8_2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field I82Field = new Field.Builder()
            .Name(nameof(I82))
            .Title("i8_2")
            .Description(" Debug data")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _I82;
        public byte I82 { get => _I82; set { _I82 = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float Motor_rpm
            +1 // uint8_t LED_status
            +1 // uint8_t SATCOM_status
            +ServoStatus.Length // uint8_t[8] Servo_status
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            MotorRpm = BinSerialize.ReadFloat(ref buffer);
            LedStatus = (byte)BinSerialize.ReadByte(ref buffer);
            SatcomStatus = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/8 - Math.Max(0,((/*PayloadByteSize*/14 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
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

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,MotorRpmField, ref _MotorRpm);    
            UInt8Type.Accept(visitor,LedStatusField, ref _LedStatus);    
            UInt8Type.Accept(visitor,SatcomStatusField, ref _SatcomStatus);    
            ArrayType.Accept(visitor,ServoStatusField, 8,
                (index,v) => UInt8Type.Accept(v, ServoStatusField, ref ServoStatus[index]));    

        }

        /// <summary>
        ///  Motor RPM 
        /// OriginName: Motor_rpm, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MotorRpmField = new Field.Builder()
            .Name(nameof(MotorRpm))
            .Title("Motor_rpm")
            .Description(" Motor RPM ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _MotorRpm;
        public float MotorRpm { get => _MotorRpm; set { _MotorRpm = value; } }
        /// <summary>
        ///  Status of the position-indicator LEDs
        /// OriginName: LED_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field LedStatusField = new Field.Builder()
            .Name(nameof(LedStatus))
            .Title("LED_status")
            .Description(" Status of the position-indicator LEDs")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _LedStatus;
        public byte LedStatus { get => _LedStatus; set { _LedStatus = value; } }
        /// <summary>
        ///  Status of the IRIDIUM satellite communication system
        /// OriginName: SATCOM_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SatcomStatusField = new Field.Builder()
            .Name(nameof(SatcomStatus))
            .Title("SATCOM_status")
            .Description(" Status of the IRIDIUM satellite communication system")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _SatcomStatus;
        public byte SatcomStatus { get => _SatcomStatus; set { _SatcomStatus = value; } }
        /// <summary>
        ///  Status vector for up to 8 servos
        /// OriginName: Servo_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ServoStatusField = new Field.Builder()
            .Name(nameof(ServoStatus))
            .Title("Servo_status")
            .Description(" Status vector for up to 8 servos")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,8))

            .Build();
        public const int ServoStatusMaxItemsCount = 8;
        public byte[] ServoStatus { get; } = new byte[8];
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t timestamp
            +4 // float Windspeed
            +4 // float WindDir
            +4 // float WindZ
            +4 // float Airspeed
            +4 // float beta
            +4 // float alpha
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimestampField, ref _Timestamp);    
            FloatType.Accept(visitor,WindspeedField, ref _Windspeed);    
            FloatType.Accept(visitor,WinddirField, ref _Winddir);    
            FloatType.Accept(visitor,WindzField, ref _Windz);    
            FloatType.Accept(visitor,AirspeedField, ref _Airspeed);    
            FloatType.Accept(visitor,BetaField, ref _Beta);    
            FloatType.Accept(visitor,AlphaField, ref _Alpha);    

        }

        /// <summary>
        ///  Time since system start
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimestampField = new Field.Builder()
            .Name(nameof(Timestamp))
            .Title("timestamp")
            .Description(" Time since system start")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestamp;
        public ulong Timestamp { get => _Timestamp; set { _Timestamp = value; } }
        /// <summary>
        ///  Magnitude of wind velocity (in lateral inertial plane)
        /// OriginName: Windspeed, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field WindspeedField = new Field.Builder()
            .Name(nameof(Windspeed))
            .Title("Windspeed")
            .Description(" Magnitude of wind velocity (in lateral inertial plane)")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Windspeed;
        public float Windspeed { get => _Windspeed; set { _Windspeed = value; } }
        /// <summary>
        ///  Wind heading angle from North
        /// OriginName: WindDir, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field WinddirField = new Field.Builder()
            .Name(nameof(Winddir))
            .Title("WindDir")
            .Description(" Wind heading angle from North")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Winddir;
        public float Winddir { get => _Winddir; set { _Winddir = value; } }
        /// <summary>
        ///  Z (Down) component of inertial wind velocity
        /// OriginName: WindZ, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field WindzField = new Field.Builder()
            .Name(nameof(Windz))
            .Title("WindZ")
            .Description(" Z (Down) component of inertial wind velocity")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Windz;
        public float Windz { get => _Windz; set { _Windz = value; } }
        /// <summary>
        ///  Magnitude of air velocity
        /// OriginName: Airspeed, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field AirspeedField = new Field.Builder()
            .Name(nameof(Airspeed))
            .Title("Airspeed")
            .Description(" Magnitude of air velocity")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Airspeed;
        public float Airspeed { get => _Airspeed; set { _Airspeed = value; } }
        /// <summary>
        ///  Sideslip angle
        /// OriginName: beta, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field BetaField = new Field.Builder()
            .Name(nameof(Beta))
            .Title("beta")
            .Description(" Sideslip angle")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Beta;
        public float Beta { get => _Beta; set { _Beta = value; } }
        /// <summary>
        ///  Angle of attack
        /// OriginName: alpha, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field AlphaField = new Field.Builder()
            .Name(nameof(Alpha))
            .Title("alpha")
            .Description(" Angle of attack")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Alpha;
        public float Alpha { get => _Alpha; set { _Alpha = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t timestamp
            +4 // float uElev
            +4 // float uThrot
            +4 // float uThrot2
            +4 // float uAilL
            +4 // float uAilR
            +4 // float uRud
            +1 // uint8_t obctrl_status
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimestampField, ref _Timestamp);    
            FloatType.Accept(visitor,UelevField, ref _Uelev);    
            FloatType.Accept(visitor,UthrotField, ref _Uthrot);    
            FloatType.Accept(visitor,Uthrot2Field, ref _Uthrot2);    
            FloatType.Accept(visitor,UaillField, ref _Uaill);    
            FloatType.Accept(visitor,UailrField, ref _Uailr);    
            FloatType.Accept(visitor,UrudField, ref _Urud);    
            UInt8Type.Accept(visitor,ObctrlStatusField, ref _ObctrlStatus);    

        }

        /// <summary>
        ///  Time since system start
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimestampField = new Field.Builder()
            .Name(nameof(Timestamp))
            .Title("timestamp")
            .Description(" Time since system start")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestamp;
        public ulong Timestamp { get => _Timestamp; set { _Timestamp = value; } }
        /// <summary>
        ///  Elevator command [~]
        /// OriginName: uElev, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UelevField = new Field.Builder()
            .Name(nameof(Uelev))
            .Title("uElev")
            .Description(" Elevator command [~]")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Uelev;
        public float Uelev { get => _Uelev; set { _Uelev = value; } }
        /// <summary>
        ///  Throttle command [~]
        /// OriginName: uThrot, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UthrotField = new Field.Builder()
            .Name(nameof(Uthrot))
            .Title("uThrot")
            .Description(" Throttle command [~]")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Uthrot;
        public float Uthrot { get => _Uthrot; set { _Uthrot = value; } }
        /// <summary>
        ///  Throttle 2 command [~]
        /// OriginName: uThrot2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Uthrot2Field = new Field.Builder()
            .Name(nameof(Uthrot2))
            .Title("uThrot2")
            .Description(" Throttle 2 command [~]")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Uthrot2;
        public float Uthrot2 { get => _Uthrot2; set { _Uthrot2 = value; } }
        /// <summary>
        ///  Left aileron command [~]
        /// OriginName: uAilL, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UaillField = new Field.Builder()
            .Name(nameof(Uaill))
            .Title("uAilL")
            .Description(" Left aileron command [~]")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Uaill;
        public float Uaill { get => _Uaill; set { _Uaill = value; } }
        /// <summary>
        ///  Right aileron command [~]
        /// OriginName: uAilR, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UailrField = new Field.Builder()
            .Name(nameof(Uailr))
            .Title("uAilR")
            .Description(" Right aileron command [~]")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Uailr;
        public float Uailr { get => _Uailr; set { _Uailr = value; } }
        /// <summary>
        ///  Rudder command [~]
        /// OriginName: uRud, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UrudField = new Field.Builder()
            .Name(nameof(Urud))
            .Title("uRud")
            .Description(" Rudder command [~]")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Urud;
        public float Urud { get => _Urud; set { _Urud = value; } }
        /// <summary>
        ///  Off-board computer status
        /// OriginName: obctrl_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ObctrlStatusField = new Field.Builder()
            .Name(nameof(ObctrlStatus))
            .Title("obctrl_status")
            .Description(" Off-board computer status")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _ObctrlStatus;
        public byte ObctrlStatus { get => _ObctrlStatus; set { _ObctrlStatus = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t timestamp
            +4 // float TempAmbient
            +4 // float Humidity
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimestampField, ref _Timestamp);    
            FloatType.Accept(visitor,TempambientField, ref _Tempambient);    
            FloatType.Accept(visitor,HumidityField, ref _Humidity);    

        }

        /// <summary>
        /// Time since system boot
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimestampField = new Field.Builder()
            .Name(nameof(Timestamp))
            .Title("timestamp")
            .Description("Time since system boot")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestamp;
        public ulong Timestamp { get => _Timestamp; set { _Timestamp = value; } }
        /// <summary>
        ///  Ambient temperature
        /// OriginName: TempAmbient, Units: degC, IsExtended: false
        /// </summary>
        public static readonly Field TempambientField = new Field.Builder()
            .Name(nameof(Tempambient))
            .Title("TempAmbient")
            .Description(" Ambient temperature")
            .FormatString(string.Empty)
            .Units(@"degC")
            .DataType(FloatType.Default)

            .Build();
        private float _Tempambient;
        public float Tempambient { get => _Tempambient; set { _Tempambient = value; } }
        /// <summary>
        ///  Relative humidity
        /// OriginName: Humidity, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field HumidityField = new Field.Builder()
            .Name(nameof(Humidity))
            .Title("Humidity")
            .Description(" Relative humidity")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _Humidity;
        public float Humidity { get => _Humidity; set { _Humidity = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t batmon_timestamp
            +4 // float temperature
            +4 // uint32_t safetystatus
            +4 // uint32_t operationstatus
            +2 // uint16_t voltage
            +2 // int16_t current
            +2 // uint16_t batterystatus
            +2 // uint16_t serialnumber
            +2 // uint16_t cellvoltage1
            +2 // uint16_t cellvoltage2
            +2 // uint16_t cellvoltage3
            +2 // uint16_t cellvoltage4
            +2 // uint16_t cellvoltage5
            +2 // uint16_t cellvoltage6
            +1 // uint8_t SoC
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,BatmonTimestampField, ref _BatmonTimestamp);    
            FloatType.Accept(visitor,TemperatureField, ref _Temperature);    
            UInt32Type.Accept(visitor,SafetystatusField, ref _Safetystatus);    
            UInt32Type.Accept(visitor,OperationstatusField, ref _Operationstatus);    
            UInt16Type.Accept(visitor,VoltageField, ref _Voltage);    
            Int16Type.Accept(visitor,CurrentField, ref _Current);
            UInt16Type.Accept(visitor,BatterystatusField, ref _Batterystatus);    
            UInt16Type.Accept(visitor,SerialnumberField, ref _Serialnumber);    
            UInt16Type.Accept(visitor,Cellvoltage1Field, ref _Cellvoltage1);    
            UInt16Type.Accept(visitor,Cellvoltage2Field, ref _Cellvoltage2);    
            UInt16Type.Accept(visitor,Cellvoltage3Field, ref _Cellvoltage3);    
            UInt16Type.Accept(visitor,Cellvoltage4Field, ref _Cellvoltage4);    
            UInt16Type.Accept(visitor,Cellvoltage5Field, ref _Cellvoltage5);    
            UInt16Type.Accept(visitor,Cellvoltage6Field, ref _Cellvoltage6);    
            UInt8Type.Accept(visitor,SocField, ref _Soc);    

        }

        /// <summary>
        /// Time since system start
        /// OriginName: batmon_timestamp, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field BatmonTimestampField = new Field.Builder()
            .Name(nameof(BatmonTimestamp))
            .Title("batmon_timestamp")
            .Description("Time since system start")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _BatmonTimestamp;
        public ulong BatmonTimestamp { get => _BatmonTimestamp; set { _BatmonTimestamp = value; } }
        /// <summary>
        /// Battery pack temperature
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
        public static readonly Field TemperatureField = new Field.Builder()
            .Name(nameof(Temperature))
            .Title("temperature")
            .Description("Battery pack temperature")
            .FormatString(string.Empty)
            .Units(@"degC")
            .DataType(FloatType.Default)

            .Build();
        private float _Temperature;
        public float Temperature { get => _Temperature; set { _Temperature = value; } }
        /// <summary>
        /// Battery monitor safetystatus report bits in Hex
        /// OriginName: safetystatus, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SafetystatusField = new Field.Builder()
            .Name(nameof(Safetystatus))
            .Title("safetystatus")
            .Description("Battery monitor safetystatus report bits in Hex")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Safetystatus;
        public uint Safetystatus { get => _Safetystatus; set { _Safetystatus = value; } }
        /// <summary>
        /// Battery monitor operation status report bits in Hex
        /// OriginName: operationstatus, Units: , IsExtended: false
        /// </summary>
        public static readonly Field OperationstatusField = new Field.Builder()
            .Name(nameof(Operationstatus))
            .Title("operationstatus")
            .Description("Battery monitor operation status report bits in Hex")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Operationstatus;
        public uint Operationstatus { get => _Operationstatus; set { _Operationstatus = value; } }
        /// <summary>
        /// Battery pack voltage
        /// OriginName: voltage, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field VoltageField = new Field.Builder()
            .Name(nameof(Voltage))
            .Title("voltage")
            .Description("Battery pack voltage")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Voltage;
        public ushort Voltage { get => _Voltage; set { _Voltage = value; } }
        /// <summary>
        /// Battery pack current
        /// OriginName: current, Units: mA, IsExtended: false
        /// </summary>
        public static readonly Field CurrentField = new Field.Builder()
            .Name(nameof(Current))
            .Title("current")
            .Description("Battery pack current")
            .FormatString(string.Empty)
            .Units(@"mA")
            .DataType(Int16Type.Default)

            .Build();
        private short _Current;
        public short Current { get => _Current; set { _Current = value; } }
        /// <summary>
        /// Battery monitor status report bits in Hex
        /// OriginName: batterystatus, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BatterystatusField = new Field.Builder()
            .Name(nameof(Batterystatus))
            .Title("batterystatus")
            .Description("Battery monitor status report bits in Hex")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Batterystatus;
        public ushort Batterystatus { get => _Batterystatus; set { _Batterystatus = value; } }
        /// <summary>
        /// Battery monitor serial number in Hex
        /// OriginName: serialnumber, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SerialnumberField = new Field.Builder()
            .Name(nameof(Serialnumber))
            .Title("serialnumber")
            .Description("Battery monitor serial number in Hex")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Serialnumber;
        public ushort Serialnumber { get => _Serialnumber; set { _Serialnumber = value; } }
        /// <summary>
        /// Battery pack cell 1 voltage
        /// OriginName: cellvoltage1, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field Cellvoltage1Field = new Field.Builder()
            .Name(nameof(Cellvoltage1))
            .Title("cellvoltage1")
            .Description("Battery pack cell 1 voltage")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Cellvoltage1;
        public ushort Cellvoltage1 { get => _Cellvoltage1; set { _Cellvoltage1 = value; } }
        /// <summary>
        /// Battery pack cell 2 voltage
        /// OriginName: cellvoltage2, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field Cellvoltage2Field = new Field.Builder()
            .Name(nameof(Cellvoltage2))
            .Title("cellvoltage2")
            .Description("Battery pack cell 2 voltage")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Cellvoltage2;
        public ushort Cellvoltage2 { get => _Cellvoltage2; set { _Cellvoltage2 = value; } }
        /// <summary>
        /// Battery pack cell 3 voltage
        /// OriginName: cellvoltage3, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field Cellvoltage3Field = new Field.Builder()
            .Name(nameof(Cellvoltage3))
            .Title("cellvoltage3")
            .Description("Battery pack cell 3 voltage")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Cellvoltage3;
        public ushort Cellvoltage3 { get => _Cellvoltage3; set { _Cellvoltage3 = value; } }
        /// <summary>
        /// Battery pack cell 4 voltage
        /// OriginName: cellvoltage4, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field Cellvoltage4Field = new Field.Builder()
            .Name(nameof(Cellvoltage4))
            .Title("cellvoltage4")
            .Description("Battery pack cell 4 voltage")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Cellvoltage4;
        public ushort Cellvoltage4 { get => _Cellvoltage4; set { _Cellvoltage4 = value; } }
        /// <summary>
        /// Battery pack cell 5 voltage
        /// OriginName: cellvoltage5, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field Cellvoltage5Field = new Field.Builder()
            .Name(nameof(Cellvoltage5))
            .Title("cellvoltage5")
            .Description("Battery pack cell 5 voltage")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Cellvoltage5;
        public ushort Cellvoltage5 { get => _Cellvoltage5; set { _Cellvoltage5 = value; } }
        /// <summary>
        /// Battery pack cell 6 voltage
        /// OriginName: cellvoltage6, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field Cellvoltage6Field = new Field.Builder()
            .Name(nameof(Cellvoltage6))
            .Title("cellvoltage6")
            .Description("Battery pack cell 6 voltage")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Cellvoltage6;
        public ushort Cellvoltage6 { get => _Cellvoltage6; set { _Cellvoltage6 = value; } }
        /// <summary>
        /// Battery pack state-of-charge
        /// OriginName: SoC, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SocField = new Field.Builder()
            .Name(nameof(Soc))
            .Title("SoC")
            .Description("Battery pack state-of-charge")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Soc;
        public byte Soc { get => _Soc; set { _Soc = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t timestamp
            +8 // uint64_t timestampModeChanged
            +4 // float xW
            +4 // float xR
            +4 // float xLat
            +4 // float xLon
            +4 // float VarW
            +4 // float VarR
            +4 // float VarLat
            +4 // float VarLon
            +4 // float LoiterRadius
            +4 // float LoiterDirection
            +4 // float DistToSoarPoint
            +4 // float vSinkExp
            +4 // float z1_LocalUpdraftSpeed
            +4 // float z2_DeltaRoll
            +4 // float z1_exp
            +4 // float z2_exp
            +4 // float ThermalGSNorth
            +4 // float ThermalGSEast
            +4 // float TSE_dot
            +4 // float DebugVar1
            +4 // float DebugVar2
            +1 // uint8_t ControlMode
            +1 // uint8_t valid
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimestampField, ref _Timestamp);    
            UInt64Type.Accept(visitor,TimestampmodechangedField, ref _Timestampmodechanged);    
            FloatType.Accept(visitor,XwField, ref _Xw);    
            FloatType.Accept(visitor,XrField, ref _Xr);    
            FloatType.Accept(visitor,XlatField, ref _Xlat);    
            FloatType.Accept(visitor,XlonField, ref _Xlon);    
            FloatType.Accept(visitor,VarwField, ref _Varw);    
            FloatType.Accept(visitor,VarrField, ref _Varr);    
            FloatType.Accept(visitor,VarlatField, ref _Varlat);    
            FloatType.Accept(visitor,VarlonField, ref _Varlon);    
            FloatType.Accept(visitor,LoiterradiusField, ref _Loiterradius);    
            FloatType.Accept(visitor,LoiterdirectionField, ref _Loiterdirection);    
            FloatType.Accept(visitor,DisttosoarpointField, ref _Disttosoarpoint);    
            FloatType.Accept(visitor,VsinkexpField, ref _Vsinkexp);    
            FloatType.Accept(visitor,Z1LocalupdraftspeedField, ref _Z1Localupdraftspeed);    
            FloatType.Accept(visitor,Z2DeltarollField, ref _Z2Deltaroll);    
            FloatType.Accept(visitor,Z1ExpField, ref _Z1Exp);    
            FloatType.Accept(visitor,Z2ExpField, ref _Z2Exp);    
            FloatType.Accept(visitor,ThermalgsnorthField, ref _Thermalgsnorth);    
            FloatType.Accept(visitor,ThermalgseastField, ref _Thermalgseast);    
            FloatType.Accept(visitor,TseDotField, ref _TseDot);    
            FloatType.Accept(visitor,Debugvar1Field, ref _Debugvar1);    
            FloatType.Accept(visitor,Debugvar2Field, ref _Debugvar2);    
            UInt8Type.Accept(visitor,ControlmodeField, ref _Controlmode);    
            UInt8Type.Accept(visitor,ValidField, ref _Valid);    

        }

        /// <summary>
        /// Timestamp
        /// OriginName: timestamp, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field TimestampField = new Field.Builder()
            .Name(nameof(Timestamp))
            .Title("timestamp")
            .Description("Timestamp")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestamp;
        public ulong Timestamp { get => _Timestamp; set { _Timestamp = value; } }
        /// <summary>
        /// Timestamp since last mode change
        /// OriginName: timestampModeChanged, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field TimestampmodechangedField = new Field.Builder()
            .Name(nameof(Timestampmodechanged))
            .Title("timestampModeChanged")
            .Description("Timestamp since last mode change")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestampmodechanged;
        public ulong Timestampmodechanged { get => _Timestampmodechanged; set { _Timestampmodechanged = value; } }
        /// <summary>
        /// Thermal core updraft strength
        /// OriginName: xW, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field XwField = new Field.Builder()
            .Name(nameof(Xw))
            .Title("xW")
            .Description("Thermal core updraft strength")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Xw;
        public float Xw { get => _Xw; set { _Xw = value; } }
        /// <summary>
        /// Thermal radius
        /// OriginName: xR, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field XrField = new Field.Builder()
            .Name(nameof(Xr))
            .Title("xR")
            .Description("Thermal radius")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Xr;
        public float Xr { get => _Xr; set { _Xr = value; } }
        /// <summary>
        /// Thermal center latitude
        /// OriginName: xLat, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field XlatField = new Field.Builder()
            .Name(nameof(Xlat))
            .Title("xLat")
            .Description("Thermal center latitude")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Xlat;
        public float Xlat { get => _Xlat; set { _Xlat = value; } }
        /// <summary>
        /// Thermal center longitude
        /// OriginName: xLon, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field XlonField = new Field.Builder()
            .Name(nameof(Xlon))
            .Title("xLon")
            .Description("Thermal center longitude")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Xlon;
        public float Xlon { get => _Xlon; set { _Xlon = value; } }
        /// <summary>
        /// Variance W
        /// OriginName: VarW, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VarwField = new Field.Builder()
            .Name(nameof(Varw))
            .Title("VarW")
            .Description("Variance W")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Varw;
        public float Varw { get => _Varw; set { _Varw = value; } }
        /// <summary>
        /// Variance R
        /// OriginName: VarR, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VarrField = new Field.Builder()
            .Name(nameof(Varr))
            .Title("VarR")
            .Description("Variance R")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Varr;
        public float Varr { get => _Varr; set { _Varr = value; } }
        /// <summary>
        /// Variance Lat
        /// OriginName: VarLat, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VarlatField = new Field.Builder()
            .Name(nameof(Varlat))
            .Title("VarLat")
            .Description("Variance Lat")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Varlat;
        public float Varlat { get => _Varlat; set { _Varlat = value; } }
        /// <summary>
        /// Variance Lon 
        /// OriginName: VarLon, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VarlonField = new Field.Builder()
            .Name(nameof(Varlon))
            .Title("VarLon")
            .Description("Variance Lon ")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Varlon;
        public float Varlon { get => _Varlon; set { _Varlon = value; } }
        /// <summary>
        /// Suggested loiter radius
        /// OriginName: LoiterRadius, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field LoiterradiusField = new Field.Builder()
            .Name(nameof(Loiterradius))
            .Title("LoiterRadius")
            .Description("Suggested loiter radius")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Loiterradius;
        public float Loiterradius { get => _Loiterradius; set { _Loiterradius = value; } }
        /// <summary>
        /// Suggested loiter direction
        /// OriginName: LoiterDirection, Units: , IsExtended: false
        /// </summary>
        public static readonly Field LoiterdirectionField = new Field.Builder()
            .Name(nameof(Loiterdirection))
            .Title("LoiterDirection")
            .Description("Suggested loiter direction")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Loiterdirection;
        public float Loiterdirection { get => _Loiterdirection; set { _Loiterdirection = value; } }
        /// <summary>
        /// Distance to soar point
        /// OriginName: DistToSoarPoint, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field DisttosoarpointField = new Field.Builder()
            .Name(nameof(Disttosoarpoint))
            .Title("DistToSoarPoint")
            .Description("Distance to soar point")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Disttosoarpoint;
        public float Disttosoarpoint { get => _Disttosoarpoint; set { _Disttosoarpoint = value; } }
        /// <summary>
        /// Expected sink rate at current airspeed, roll and throttle
        /// OriginName: vSinkExp, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field VsinkexpField = new Field.Builder()
            .Name(nameof(Vsinkexp))
            .Title("vSinkExp")
            .Description("Expected sink rate at current airspeed, roll and throttle")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Vsinkexp;
        public float Vsinkexp { get => _Vsinkexp; set { _Vsinkexp = value; } }
        /// <summary>
        /// Measurement / updraft speed at current/local airplane position
        /// OriginName: z1_LocalUpdraftSpeed, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field Z1LocalupdraftspeedField = new Field.Builder()
            .Name(nameof(Z1Localupdraftspeed))
            .Title("z1_LocalUpdraftSpeed")
            .Description("Measurement / updraft speed at current/local airplane position")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Z1Localupdraftspeed;
        public float Z1Localupdraftspeed { get => _Z1Localupdraftspeed; set { _Z1Localupdraftspeed = value; } }
        /// <summary>
        /// Measurement / roll angle tracking error
        /// OriginName: z2_DeltaRoll, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Z2DeltarollField = new Field.Builder()
            .Name(nameof(Z2Deltaroll))
            .Title("z2_DeltaRoll")
            .Description("Measurement / roll angle tracking error")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Z2Deltaroll;
        public float Z2Deltaroll { get => _Z2Deltaroll; set { _Z2Deltaroll = value; } }
        /// <summary>
        /// Expected measurement 1
        /// OriginName: z1_exp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Z1ExpField = new Field.Builder()
            .Name(nameof(Z1Exp))
            .Title("z1_exp")
            .Description("Expected measurement 1")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Z1Exp;
        public float Z1Exp { get => _Z1Exp; set { _Z1Exp = value; } }
        /// <summary>
        /// Expected measurement 2
        /// OriginName: z2_exp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Z2ExpField = new Field.Builder()
            .Name(nameof(Z2Exp))
            .Title("z2_exp")
            .Description("Expected measurement 2")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Z2Exp;
        public float Z2Exp { get => _Z2Exp; set { _Z2Exp = value; } }
        /// <summary>
        /// Thermal drift (from estimator prediction step only)
        /// OriginName: ThermalGSNorth, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field ThermalgsnorthField = new Field.Builder()
            .Name(nameof(Thermalgsnorth))
            .Title("ThermalGSNorth")
            .Description("Thermal drift (from estimator prediction step only)")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Thermalgsnorth;
        public float Thermalgsnorth { get => _Thermalgsnorth; set { _Thermalgsnorth = value; } }
        /// <summary>
        /// Thermal drift (from estimator prediction step only)
        /// OriginName: ThermalGSEast, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field ThermalgseastField = new Field.Builder()
            .Name(nameof(Thermalgseast))
            .Title("ThermalGSEast")
            .Description("Thermal drift (from estimator prediction step only)")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Thermalgseast;
        public float Thermalgseast { get => _Thermalgseast; set { _Thermalgseast = value; } }
        /// <summary>
        ///  Total specific energy change (filtered)
        /// OriginName: TSE_dot, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field TseDotField = new Field.Builder()
            .Name(nameof(TseDot))
            .Title("TSE_dot")
            .Description(" Total specific energy change (filtered)")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _TseDot;
        public float TseDot { get => _TseDot; set { _TseDot = value; } }
        /// <summary>
        ///  Debug variable 1
        /// OriginName: DebugVar1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Debugvar1Field = new Field.Builder()
            .Name(nameof(Debugvar1))
            .Title("DebugVar1")
            .Description(" Debug variable 1")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Debugvar1;
        public float Debugvar1 { get => _Debugvar1; set { _Debugvar1 = value; } }
        /// <summary>
        ///  Debug variable 2
        /// OriginName: DebugVar2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Debugvar2Field = new Field.Builder()
            .Name(nameof(Debugvar2))
            .Title("DebugVar2")
            .Description(" Debug variable 2")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Debugvar2;
        public float Debugvar2 { get => _Debugvar2; set { _Debugvar2 = value; } }
        /// <summary>
        /// Control Mode [-]
        /// OriginName: ControlMode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ControlmodeField = new Field.Builder()
            .Name(nameof(Controlmode))
            .Title("ControlMode")
            .Description("Control Mode [-]")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Controlmode;
        public byte Controlmode { get => _Controlmode; set { _Controlmode = value; } }
        /// <summary>
        /// Data valid [-]
        /// OriginName: valid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ValidField = new Field.Builder()
            .Name(nameof(Valid))
            .Title("valid")
            .Description("Data valid [-]")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Valid;
        public byte Valid { get => _Valid; set { _Valid = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t timestamp
            +2 // uint16_t free_space
            +1 // uint8_t visensor_rate_1
            +1 // uint8_t visensor_rate_2
            +1 // uint8_t visensor_rate_3
            +1 // uint8_t visensor_rate_4
            +1 // uint8_t recording_nodes_count
            +1 // uint8_t cpu_temp
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimestampField, ref _Timestamp);    
            UInt16Type.Accept(visitor,FreeSpaceField, ref _FreeSpace);    
            UInt8Type.Accept(visitor,VisensorRate1Field, ref _VisensorRate1);    
            UInt8Type.Accept(visitor,VisensorRate2Field, ref _VisensorRate2);    
            UInt8Type.Accept(visitor,VisensorRate3Field, ref _VisensorRate3);    
            UInt8Type.Accept(visitor,VisensorRate4Field, ref _VisensorRate4);    
            UInt8Type.Accept(visitor,RecordingNodesCountField, ref _RecordingNodesCount);    
            UInt8Type.Accept(visitor,CpuTempField, ref _CpuTemp);    

        }

        /// <summary>
        /// Timestamp in linuxtime (since 1.1.1970)
        /// OriginName: timestamp, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field TimestampField = new Field.Builder()
            .Name(nameof(Timestamp))
            .Title("timestamp")
            .Description("Timestamp in linuxtime (since 1.1.1970)")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestamp;
        public ulong Timestamp { get => _Timestamp; set { _Timestamp = value; } }
        /// <summary>
        /// Free space available in recordings directory in [Gb] * 1e2
        /// OriginName: free_space, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FreeSpaceField = new Field.Builder()
            .Name(nameof(FreeSpace))
            .Title("free_space")
            .Description("Free space available in recordings directory in [Gb] * 1e2")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _FreeSpace;
        public ushort FreeSpace { get => _FreeSpace; set { _FreeSpace = value; } }
        /// <summary>
        /// Rate of ROS topic 1
        /// OriginName: visensor_rate_1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VisensorRate1Field = new Field.Builder()
            .Name(nameof(VisensorRate1))
            .Title("visensor_rate_1")
            .Description("Rate of ROS topic 1")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _VisensorRate1;
        public byte VisensorRate1 { get => _VisensorRate1; set { _VisensorRate1 = value; } }
        /// <summary>
        /// Rate of ROS topic 2
        /// OriginName: visensor_rate_2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VisensorRate2Field = new Field.Builder()
            .Name(nameof(VisensorRate2))
            .Title("visensor_rate_2")
            .Description("Rate of ROS topic 2")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _VisensorRate2;
        public byte VisensorRate2 { get => _VisensorRate2; set { _VisensorRate2 = value; } }
        /// <summary>
        /// Rate of ROS topic 3
        /// OriginName: visensor_rate_3, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VisensorRate3Field = new Field.Builder()
            .Name(nameof(VisensorRate3))
            .Title("visensor_rate_3")
            .Description("Rate of ROS topic 3")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _VisensorRate3;
        public byte VisensorRate3 { get => _VisensorRate3; set { _VisensorRate3 = value; } }
        /// <summary>
        /// Rate of ROS topic 4
        /// OriginName: visensor_rate_4, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VisensorRate4Field = new Field.Builder()
            .Name(nameof(VisensorRate4))
            .Title("visensor_rate_4")
            .Description("Rate of ROS topic 4")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _VisensorRate4;
        public byte VisensorRate4 { get => _VisensorRate4; set { _VisensorRate4 = value; } }
        /// <summary>
        /// Number of recording nodes
        /// OriginName: recording_nodes_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RecordingNodesCountField = new Field.Builder()
            .Name(nameof(RecordingNodesCount))
            .Title("recording_nodes_count")
            .Description("Number of recording nodes")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _RecordingNodesCount;
        public byte RecordingNodesCount { get => _RecordingNodesCount; set { _RecordingNodesCount = value; } }
        /// <summary>
        /// Temperature of sensorpod CPU in
        /// OriginName: cpu_temp, Units: degC, IsExtended: false
        /// </summary>
        public static readonly Field CpuTempField = new Field.Builder()
            .Name(nameof(CpuTemp))
            .Title("cpu_temp")
            .Description("Temperature of sensorpod CPU in")
            .FormatString(string.Empty)
            .Units(@"degC")
            .DataType(UInt8Type.Default)

            .Build();
        private byte _CpuTemp;
        public byte CpuTemp { get => _CpuTemp; set { _CpuTemp = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t timestamp
            +4 // float pwr_brd_system_volt
            +4 // float pwr_brd_servo_volt
            +4 // float pwr_brd_digital_volt
            +4 // float pwr_brd_mot_l_amp
            +4 // float pwr_brd_mot_r_amp
            +4 // float pwr_brd_analog_amp
            +4 // float pwr_brd_digital_amp
            +4 // float pwr_brd_ext_amp
            +4 // float pwr_brd_aux_amp
            +1 // uint8_t pwr_brd_status
            +1 // uint8_t pwr_brd_led_status
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimestampField, ref _Timestamp);    
            FloatType.Accept(visitor,PwrBrdSystemVoltField, ref _PwrBrdSystemVolt);    
            FloatType.Accept(visitor,PwrBrdServoVoltField, ref _PwrBrdServoVolt);    
            FloatType.Accept(visitor,PwrBrdDigitalVoltField, ref _PwrBrdDigitalVolt);    
            FloatType.Accept(visitor,PwrBrdMotLAmpField, ref _PwrBrdMotLAmp);    
            FloatType.Accept(visitor,PwrBrdMotRAmpField, ref _PwrBrdMotRAmp);    
            FloatType.Accept(visitor,PwrBrdAnalogAmpField, ref _PwrBrdAnalogAmp);    
            FloatType.Accept(visitor,PwrBrdDigitalAmpField, ref _PwrBrdDigitalAmp);    
            FloatType.Accept(visitor,PwrBrdExtAmpField, ref _PwrBrdExtAmp);    
            FloatType.Accept(visitor,PwrBrdAuxAmpField, ref _PwrBrdAuxAmp);    
            UInt8Type.Accept(visitor,PwrBrdStatusField, ref _PwrBrdStatus);    
            UInt8Type.Accept(visitor,PwrBrdLedStatusField, ref _PwrBrdLedStatus);    

        }

        /// <summary>
        /// Timestamp
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimestampField = new Field.Builder()
            .Name(nameof(Timestamp))
            .Title("timestamp")
            .Description("Timestamp")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestamp;
        public ulong Timestamp { get => _Timestamp; set { _Timestamp = value; } }
        /// <summary>
        /// Power board system voltage
        /// OriginName: pwr_brd_system_volt, Units: V, IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdSystemVoltField = new Field.Builder()
            .Name(nameof(PwrBrdSystemVolt))
            .Title("pwr_brd_system_volt")
            .Description("Power board system voltage")
            .FormatString(string.Empty)
            .Units(@"V")
            .DataType(FloatType.Default)

            .Build();
        private float _PwrBrdSystemVolt;
        public float PwrBrdSystemVolt { get => _PwrBrdSystemVolt; set { _PwrBrdSystemVolt = value; } }
        /// <summary>
        /// Power board servo voltage
        /// OriginName: pwr_brd_servo_volt, Units: V, IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdServoVoltField = new Field.Builder()
            .Name(nameof(PwrBrdServoVolt))
            .Title("pwr_brd_servo_volt")
            .Description("Power board servo voltage")
            .FormatString(string.Empty)
            .Units(@"V")
            .DataType(FloatType.Default)

            .Build();
        private float _PwrBrdServoVolt;
        public float PwrBrdServoVolt { get => _PwrBrdServoVolt; set { _PwrBrdServoVolt = value; } }
        /// <summary>
        /// Power board digital voltage
        /// OriginName: pwr_brd_digital_volt, Units: V, IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdDigitalVoltField = new Field.Builder()
            .Name(nameof(PwrBrdDigitalVolt))
            .Title("pwr_brd_digital_volt")
            .Description("Power board digital voltage")
            .FormatString(string.Empty)
            .Units(@"V")
            .DataType(FloatType.Default)

            .Build();
        private float _PwrBrdDigitalVolt;
        public float PwrBrdDigitalVolt { get => _PwrBrdDigitalVolt; set { _PwrBrdDigitalVolt = value; } }
        /// <summary>
        /// Power board left motor current sensor
        /// OriginName: pwr_brd_mot_l_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdMotLAmpField = new Field.Builder()
            .Name(nameof(PwrBrdMotLAmp))
            .Title("pwr_brd_mot_l_amp")
            .Description("Power board left motor current sensor")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _PwrBrdMotLAmp;
        public float PwrBrdMotLAmp { get => _PwrBrdMotLAmp; set { _PwrBrdMotLAmp = value; } }
        /// <summary>
        /// Power board right motor current sensor
        /// OriginName: pwr_brd_mot_r_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdMotRAmpField = new Field.Builder()
            .Name(nameof(PwrBrdMotRAmp))
            .Title("pwr_brd_mot_r_amp")
            .Description("Power board right motor current sensor")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _PwrBrdMotRAmp;
        public float PwrBrdMotRAmp { get => _PwrBrdMotRAmp; set { _PwrBrdMotRAmp = value; } }
        /// <summary>
        /// Power board analog current sensor
        /// OriginName: pwr_brd_analog_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdAnalogAmpField = new Field.Builder()
            .Name(nameof(PwrBrdAnalogAmp))
            .Title("pwr_brd_analog_amp")
            .Description("Power board analog current sensor")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _PwrBrdAnalogAmp;
        public float PwrBrdAnalogAmp { get => _PwrBrdAnalogAmp; set { _PwrBrdAnalogAmp = value; } }
        /// <summary>
        /// Power board digital current sensor
        /// OriginName: pwr_brd_digital_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdDigitalAmpField = new Field.Builder()
            .Name(nameof(PwrBrdDigitalAmp))
            .Title("pwr_brd_digital_amp")
            .Description("Power board digital current sensor")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _PwrBrdDigitalAmp;
        public float PwrBrdDigitalAmp { get => _PwrBrdDigitalAmp; set { _PwrBrdDigitalAmp = value; } }
        /// <summary>
        /// Power board extension current sensor
        /// OriginName: pwr_brd_ext_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdExtAmpField = new Field.Builder()
            .Name(nameof(PwrBrdExtAmp))
            .Title("pwr_brd_ext_amp")
            .Description("Power board extension current sensor")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _PwrBrdExtAmp;
        public float PwrBrdExtAmp { get => _PwrBrdExtAmp; set { _PwrBrdExtAmp = value; } }
        /// <summary>
        /// Power board aux current sensor
        /// OriginName: pwr_brd_aux_amp, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdAuxAmpField = new Field.Builder()
            .Name(nameof(PwrBrdAuxAmp))
            .Title("pwr_brd_aux_amp")
            .Description("Power board aux current sensor")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _PwrBrdAuxAmp;
        public float PwrBrdAuxAmp { get => _PwrBrdAuxAmp; set { _PwrBrdAuxAmp = value; } }
        /// <summary>
        /// Power board status register
        /// OriginName: pwr_brd_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdStatusField = new Field.Builder()
            .Name(nameof(PwrBrdStatus))
            .Title("pwr_brd_status")
            .Description("Power board status register")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _PwrBrdStatus;
        public byte PwrBrdStatus { get => _PwrBrdStatus; set { _PwrBrdStatus = value; } }
        /// <summary>
        /// Power board leds status
        /// OriginName: pwr_brd_led_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PwrBrdLedStatusField = new Field.Builder()
            .Name(nameof(PwrBrdLedStatus))
            .Title("pwr_brd_led_status")
            .Description("Power board leds status")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _PwrBrdLedStatus;
        public byte PwrBrdLedStatus { get => _PwrBrdLedStatus; set { _PwrBrdLedStatus = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t timestamp
            + 1 // uint8_t gsm_modem_type
            + 1 // uint8_t gsm_link_type
            +1 // uint8_t rssi
            +1 // uint8_t rsrp_rscp
            +1 // uint8_t sinr_ecio
            +1 // uint8_t rsrq
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimestampField, ref _Timestamp);    
            var tmpGsmModemType = (byte)GsmModemType;
            UInt8Type.Accept(visitor,GsmModemTypeField, ref tmpGsmModemType);
            GsmModemType = (GsmModemType)tmpGsmModemType;
            var tmpGsmLinkType = (byte)GsmLinkType;
            UInt8Type.Accept(visitor,GsmLinkTypeField, ref tmpGsmLinkType);
            GsmLinkType = (GsmLinkType)tmpGsmLinkType;
            UInt8Type.Accept(visitor,RssiField, ref _Rssi);    
            UInt8Type.Accept(visitor,RsrpRscpField, ref _RsrpRscp);    
            UInt8Type.Accept(visitor,SinrEcioField, ref _SinrEcio);    
            UInt8Type.Accept(visitor,RsrqField, ref _Rsrq);    

        }

        /// <summary>
        /// Timestamp (of OBC)
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimestampField = new Field.Builder()
            .Name(nameof(Timestamp))
            .Title("timestamp")
            .Description("Timestamp (of OBC)")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestamp;
        public ulong Timestamp { get => _Timestamp; set { _Timestamp = value; } }
        /// <summary>
        /// GSM modem used
        /// OriginName: gsm_modem_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GsmModemTypeField = new Field.Builder()
            .Name(nameof(GsmModemType))
            .Title("gsm_modem_type")
            .Description("GSM modem used")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GsmModemType _GsmModemType;
        public GsmModemType GsmModemType { get => _GsmModemType; set => _GsmModemType = value; } 
        /// <summary>
        /// GSM link type
        /// OriginName: gsm_link_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GsmLinkTypeField = new Field.Builder()
            .Name(nameof(GsmLinkType))
            .Title("gsm_link_type")
            .Description("GSM link type")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GsmLinkType _GsmLinkType;
        public GsmLinkType GsmLinkType { get => _GsmLinkType; set => _GsmLinkType = value; } 
        /// <summary>
        /// RSSI as reported by modem (unconverted)
        /// OriginName: rssi, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RssiField = new Field.Builder()
            .Name(nameof(Rssi))
            .Title("rssi")
            .Description("RSSI as reported by modem (unconverted)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Rssi;
        public byte Rssi { get => _Rssi; set { _Rssi = value; } }
        /// <summary>
        /// RSRP (LTE) or RSCP (WCDMA) as reported by modem (unconverted)
        /// OriginName: rsrp_rscp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RsrpRscpField = new Field.Builder()
            .Name(nameof(RsrpRscp))
            .Title("rsrp_rscp")
            .Description("RSRP (LTE) or RSCP (WCDMA) as reported by modem (unconverted)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _RsrpRscp;
        public byte RsrpRscp { get => _RsrpRscp; set { _RsrpRscp = value; } }
        /// <summary>
        /// SINR (LTE) or ECIO (WCDMA) as reported by modem (unconverted)
        /// OriginName: sinr_ecio, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SinrEcioField = new Field.Builder()
            .Name(nameof(SinrEcio))
            .Title("sinr_ecio")
            .Description("SINR (LTE) or ECIO (WCDMA) as reported by modem (unconverted)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _SinrEcio;
        public byte SinrEcio { get => _SinrEcio; set { _SinrEcio = value; } }
        /// <summary>
        /// RSRQ (LTE only) as reported by modem (unconverted)
        /// OriginName: rsrq, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RsrqField = new Field.Builder()
            .Name(nameof(Rsrq))
            .Title("rsrq")
            .Description("RSRQ (LTE only) as reported by modem (unconverted)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Rsrq;
        public byte Rsrq { get => _Rsrq; set { _Rsrq = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t timestamp
            +8 // uint64_t last_heartbeat
            +2 // uint16_t failed_sessions
            +2 // uint16_t successful_sessions
            +1 // uint8_t signal_quality
            +1 // uint8_t ring_pending
            +1 // uint8_t tx_session_pending
            +1 // uint8_t rx_session_pending
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimestampField, ref _Timestamp);    
            UInt64Type.Accept(visitor,LastHeartbeatField, ref _LastHeartbeat);    
            UInt16Type.Accept(visitor,FailedSessionsField, ref _FailedSessions);    
            UInt16Type.Accept(visitor,SuccessfulSessionsField, ref _SuccessfulSessions);    
            UInt8Type.Accept(visitor,SignalQualityField, ref _SignalQuality);    
            UInt8Type.Accept(visitor,RingPendingField, ref _RingPending);    
            UInt8Type.Accept(visitor,TxSessionPendingField, ref _TxSessionPending);    
            UInt8Type.Accept(visitor,RxSessionPendingField, ref _RxSessionPending);    

        }

        /// <summary>
        /// Timestamp
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimestampField = new Field.Builder()
            .Name(nameof(Timestamp))
            .Title("timestamp")
            .Description("Timestamp")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestamp;
        public ulong Timestamp { get => _Timestamp; set { _Timestamp = value; } }
        /// <summary>
        /// Timestamp of the last successful sbd session
        /// OriginName: last_heartbeat, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field LastHeartbeatField = new Field.Builder()
            .Name(nameof(LastHeartbeat))
            .Title("last_heartbeat")
            .Description("Timestamp of the last successful sbd session")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _LastHeartbeat;
        public ulong LastHeartbeat { get => _LastHeartbeat; set { _LastHeartbeat = value; } }
        /// <summary>
        /// Number of failed sessions
        /// OriginName: failed_sessions, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FailedSessionsField = new Field.Builder()
            .Name(nameof(FailedSessions))
            .Title("failed_sessions")
            .Description("Number of failed sessions")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _FailedSessions;
        public ushort FailedSessions { get => _FailedSessions; set { _FailedSessions = value; } }
        /// <summary>
        /// Number of successful sessions
        /// OriginName: successful_sessions, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SuccessfulSessionsField = new Field.Builder()
            .Name(nameof(SuccessfulSessions))
            .Title("successful_sessions")
            .Description("Number of successful sessions")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _SuccessfulSessions;
        public ushort SuccessfulSessions { get => _SuccessfulSessions; set { _SuccessfulSessions = value; } }
        /// <summary>
        /// Signal quality
        /// OriginName: signal_quality, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SignalQualityField = new Field.Builder()
            .Name(nameof(SignalQuality))
            .Title("signal_quality")
            .Description("Signal quality")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _SignalQuality;
        public byte SignalQuality { get => _SignalQuality; set { _SignalQuality = value; } }
        /// <summary>
        /// Ring call pending
        /// OriginName: ring_pending, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RingPendingField = new Field.Builder()
            .Name(nameof(RingPending))
            .Title("ring_pending")
            .Description("Ring call pending")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _RingPending;
        public byte RingPending { get => _RingPending; set { _RingPending = value; } }
        /// <summary>
        /// Transmission session pending
        /// OriginName: tx_session_pending, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TxSessionPendingField = new Field.Builder()
            .Name(nameof(TxSessionPending))
            .Title("tx_session_pending")
            .Description("Transmission session pending")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TxSessionPending;
        public byte TxSessionPending { get => _TxSessionPending; set { _TxSessionPending = value; } }
        /// <summary>
        /// Receiving session pending
        /// OriginName: rx_session_pending, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RxSessionPendingField = new Field.Builder()
            .Name(nameof(RxSessionPending))
            .Title("rx_session_pending")
            .Description("Receiving session pending")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _RxSessionPending;
        public byte RxSessionPending { get => _RxSessionPending; set { _RxSessionPending = value; } }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t timestamp
            +4 // float angleofattack
            +4 // float sideslip
            +1 // uint8_t angleofattack_valid
            +1 // uint8_t sideslip_valid
            );
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

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimestampField, ref _Timestamp);    
            FloatType.Accept(visitor,AngleofattackField, ref _Angleofattack);    
            FloatType.Accept(visitor,SideslipField, ref _Sideslip);    
            UInt8Type.Accept(visitor,AngleofattackValidField, ref _AngleofattackValid);    
            UInt8Type.Accept(visitor,SideslipValidField, ref _SideslipValid);    

        }

        /// <summary>
        /// Timestamp
        /// OriginName: timestamp, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimestampField = new Field.Builder()
            .Name(nameof(Timestamp))
            .Title("timestamp")
            .Description("Timestamp")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _Timestamp;
        public ulong Timestamp { get => _Timestamp; set { _Timestamp = value; } }
        /// <summary>
        /// Angle of attack
        /// OriginName: angleofattack, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field AngleofattackField = new Field.Builder()
            .Name(nameof(Angleofattack))
            .Title("angleofattack")
            .Description("Angle of attack")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Angleofattack;
        public float Angleofattack { get => _Angleofattack; set { _Angleofattack = value; } }
        /// <summary>
        /// Sideslip angle
        /// OriginName: sideslip, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field SideslipField = new Field.Builder()
            .Name(nameof(Sideslip))
            .Title("sideslip")
            .Description("Sideslip angle")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Sideslip;
        public float Sideslip { get => _Sideslip; set { _Sideslip = value; } }
        /// <summary>
        /// Angle of attack measurement valid
        /// OriginName: angleofattack_valid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AngleofattackValidField = new Field.Builder()
            .Name(nameof(AngleofattackValid))
            .Title("angleofattack_valid")
            .Description("Angle of attack measurement valid")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _AngleofattackValid;
        public byte AngleofattackValid { get => _AngleofattackValid; set { _AngleofattackValid = value; } }
        /// <summary>
        /// Sideslip angle measurement valid
        /// OriginName: sideslip_valid, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SideslipValidField = new Field.Builder()
            .Name(nameof(SideslipValid))
            .Title("sideslip_valid")
            .Description("Sideslip angle measurement valid")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _SideslipValid;
        public byte SideslipValid { get => _SideslipValid; set { _SideslipValid = value; } }
    }




        


#endregion


}

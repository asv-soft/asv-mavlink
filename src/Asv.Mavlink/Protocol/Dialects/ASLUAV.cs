// MIT License
//
// Copyright (c) 2018 Alexey (https://github.com/asvol)
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

// This code was generate by tool Asv.Mavlink.Shell version 1.1.1

using System;
using System.Text;
using Asv.Mavlink.V2.Common;
using Asv.IO;

namespace Asv.Mavlink.V2.Asluav
{

    public static class AsluavHelper
    {
        public static void RegisterAsluavDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new CommandIntStampedPacket());
            src.Register(()=>new CommandLongStampedPacket());
            src.Register(()=>new SensPowerPacket());
            src.Register(()=>new SensMpptPacket());
            src.Register(()=>new AslctrlDataPacket());
            src.Register(()=>new AslctrlDebugPacket());
            src.Register(()=>new AsluavStatusPacket());
            src.Register(()=>new EkfExtPacket());
            src.Register(()=>new AslObctrlPacket());
            src.Register(()=>new SensAtmosPacket());
            src.Register(()=>new SensBatmonPacket());
            src.Register(()=>new FwSoaringDataPacket());
            src.Register(()=>new SensorpodStatusPacket());
            src.Register(()=>new SensPowerBoardPacket());
            src.Register(()=>new GsmLinkStatusPacket());
            src.Register(()=>new SatcomLinkStatusPacket());
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
    public class CommandIntStampedPacket: PacketV2<CommandIntStampedPayload>
    {
	    public const int PacketMessageId = 78;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 119;

        public override CommandIntStampedPayload Payload { get; } = new CommandIntStampedPayload();

        public override string Name => "COMMAND_INT_STAMPED";
    }

    /// <summary>
    ///  COMMAND_INT_STAMPED
    /// </summary>
    public class CommandIntStampedPayload : IPayload
    {
        public byte GetMaxByteSize() => 47; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 47; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            VehicleTimestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            UtcTime = BinSerialize.ReadUInt(ref buffer);index+=4;
            Param1 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Param2 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Param3 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Param4 = BinSerialize.ReadFloat(ref buffer);index+=4;
            X = BinSerialize.ReadInt(ref buffer);index+=4;
            Y = BinSerialize.ReadInt(ref buffer);index+=4;
            Z = BinSerialize.ReadFloat(ref buffer);index+=4;
            Command = (MavCmd)BinSerialize.ReadUShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Frame = (MavFrame)BinSerialize.ReadByte(ref buffer);index+=1;
            Current = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Autocontinue = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,VehicleTimestamp);index+=8;
            BinSerialize.WriteUInt(ref buffer,UtcTime);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param1);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param2);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param3);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param4);index+=4;
            BinSerialize.WriteInt(ref buffer,X);index+=4;
            BinSerialize.WriteInt(ref buffer,Y);index+=4;
            BinSerialize.WriteFloat(ref buffer,Z);index+=4;
            BinSerialize.WriteUShort(ref buffer,(ushort)Command);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Frame);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Current);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Autocontinue);index+=1;
            return index; // /*PayloadByteSize*/47;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            VehicleTimestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            UtcTime = BitConverter.ToUInt32(buffer,index);index+=4;
            Param1 = BitConverter.ToSingle(buffer, index);index+=4;
            Param2 = BitConverter.ToSingle(buffer, index);index+=4;
            Param3 = BitConverter.ToSingle(buffer, index);index+=4;
            Param4 = BitConverter.ToSingle(buffer, index);index+=4;
            X = BitConverter.ToInt32(buffer,index);index+=4;
            Y = BitConverter.ToInt32(buffer,index);index+=4;
            Z = BitConverter.ToSingle(buffer, index);index+=4;
            Command = (MavCmd)BitConverter.ToUInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Frame = (MavFrame)buffer[index++];
            Current = (byte)buffer[index++];
            Autocontinue = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(VehicleTimestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(UtcTime).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param1).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param2).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param3).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param4).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(X).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Y).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Z).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes((ushort)Command).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Frame;index+=1;
            BitConverter.GetBytes(Current).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Autocontinue).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/47;
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
    public class CommandLongStampedPacket: PacketV2<CommandLongStampedPayload>
    {
	    public const int PacketMessageId = 79;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 102;

        public override CommandLongStampedPayload Payload { get; } = new CommandLongStampedPayload();

        public override string Name => "COMMAND_LONG_STAMPED";
    }

    /// <summary>
    ///  COMMAND_LONG_STAMPED
    /// </summary>
    public class CommandLongStampedPayload : IPayload
    {
        public byte GetMaxByteSize() => 45; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 45; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            VehicleTimestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            UtcTime = BinSerialize.ReadUInt(ref buffer);index+=4;
            Param1 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Param2 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Param3 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Param4 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Param5 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Param6 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Param7 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Command = (MavCmd)BinSerialize.ReadUShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Confirmation = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,VehicleTimestamp);index+=8;
            BinSerialize.WriteUInt(ref buffer,UtcTime);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param1);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param2);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param3);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param4);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param5);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param6);index+=4;
            BinSerialize.WriteFloat(ref buffer,Param7);index+=4;
            BinSerialize.WriteUShort(ref buffer,(ushort)Command);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Confirmation);index+=1;
            return index; // /*PayloadByteSize*/45;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            VehicleTimestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            UtcTime = BitConverter.ToUInt32(buffer,index);index+=4;
            Param1 = BitConverter.ToSingle(buffer, index);index+=4;
            Param2 = BitConverter.ToSingle(buffer, index);index+=4;
            Param3 = BitConverter.ToSingle(buffer, index);index+=4;
            Param4 = BitConverter.ToSingle(buffer, index);index+=4;
            Param5 = BitConverter.ToSingle(buffer, index);index+=4;
            Param6 = BitConverter.ToSingle(buffer, index);index+=4;
            Param7 = BitConverter.ToSingle(buffer, index);index+=4;
            Command = (MavCmd)BitConverter.ToUInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Confirmation = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(VehicleTimestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(UtcTime).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param1).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param2).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param3).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param4).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param5).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param6).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Param7).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes((ushort)Command).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Confirmation).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/45;
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
    public class SensPowerPacket: PacketV2<SensPowerPayload>
    {
	    public const int PacketMessageId = 201;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 218;

        public override SensPowerPayload Payload { get; } = new SensPowerPayload();

        public override string Name => "SENS_POWER";
    }

    /// <summary>
    ///  SENS_POWER
    /// </summary>
    public class SensPowerPayload : IPayload
    {
        public byte GetMaxByteSize() => 16; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Adc121VspbVolt = BinSerialize.ReadFloat(ref buffer);index+=4;
            Adc121CspbAmp = BinSerialize.ReadFloat(ref buffer);index+=4;
            Adc121Cs1Amp = BinSerialize.ReadFloat(ref buffer);index+=4;
            Adc121Cs2Amp = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Adc121VspbVolt);index+=4;
            BinSerialize.WriteFloat(ref buffer,Adc121CspbAmp);index+=4;
            BinSerialize.WriteFloat(ref buffer,Adc121Cs1Amp);index+=4;
            BinSerialize.WriteFloat(ref buffer,Adc121Cs2Amp);index+=4;
            return index; // /*PayloadByteSize*/16;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Adc121VspbVolt = BitConverter.ToSingle(buffer, index);index+=4;
            Adc121CspbAmp = BitConverter.ToSingle(buffer, index);index+=4;
            Adc121Cs1Amp = BitConverter.ToSingle(buffer, index);index+=4;
            Adc121Cs2Amp = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Adc121VspbVolt).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Adc121CspbAmp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Adc121Cs1Amp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Adc121Cs2Amp).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/16;
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
    public class SensMpptPacket: PacketV2<SensMpptPayload>
    {
	    public const int PacketMessageId = 202;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 231;

        public override SensMpptPayload Payload { get; } = new SensMpptPayload();

        public override string Name => "SENS_MPPT";
    }

    /// <summary>
    ///  SENS_MPPT
    /// </summary>
    public class SensMpptPayload : IPayload
    {
        public byte GetMaxByteSize() => 41; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 41; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            MpptTimestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            Mppt1Volt = BinSerialize.ReadFloat(ref buffer);index+=4;
            Mppt1Amp = BinSerialize.ReadFloat(ref buffer);index+=4;
            Mppt2Volt = BinSerialize.ReadFloat(ref buffer);index+=4;
            Mppt2Amp = BinSerialize.ReadFloat(ref buffer);index+=4;
            Mppt3Volt = BinSerialize.ReadFloat(ref buffer);index+=4;
            Mppt3Amp = BinSerialize.ReadFloat(ref buffer);index+=4;
            Mppt1Pwm = BinSerialize.ReadUShort(ref buffer);index+=2;
            Mppt2Pwm = BinSerialize.ReadUShort(ref buffer);index+=2;
            Mppt3Pwm = BinSerialize.ReadUShort(ref buffer);index+=2;
            Mppt1Status = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Mppt2Status = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Mppt3Status = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,MpptTimestamp);index+=8;
            BinSerialize.WriteFloat(ref buffer,Mppt1Volt);index+=4;
            BinSerialize.WriteFloat(ref buffer,Mppt1Amp);index+=4;
            BinSerialize.WriteFloat(ref buffer,Mppt2Volt);index+=4;
            BinSerialize.WriteFloat(ref buffer,Mppt2Amp);index+=4;
            BinSerialize.WriteFloat(ref buffer,Mppt3Volt);index+=4;
            BinSerialize.WriteFloat(ref buffer,Mppt3Amp);index+=4;
            BinSerialize.WriteUShort(ref buffer,Mppt1Pwm);index+=2;
            BinSerialize.WriteUShort(ref buffer,Mppt2Pwm);index+=2;
            BinSerialize.WriteUShort(ref buffer,Mppt3Pwm);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)Mppt1Status);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Mppt2Status);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Mppt3Status);index+=1;
            return index; // /*PayloadByteSize*/41;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            MpptTimestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            Mppt1Volt = BitConverter.ToSingle(buffer, index);index+=4;
            Mppt1Amp = BitConverter.ToSingle(buffer, index);index+=4;
            Mppt2Volt = BitConverter.ToSingle(buffer, index);index+=4;
            Mppt2Amp = BitConverter.ToSingle(buffer, index);index+=4;
            Mppt3Volt = BitConverter.ToSingle(buffer, index);index+=4;
            Mppt3Amp = BitConverter.ToSingle(buffer, index);index+=4;
            Mppt1Pwm = BitConverter.ToUInt16(buffer,index);index+=2;
            Mppt2Pwm = BitConverter.ToUInt16(buffer,index);index+=2;
            Mppt3Pwm = BitConverter.ToUInt16(buffer,index);index+=2;
            Mppt1Status = (byte)buffer[index++];
            Mppt2Status = (byte)buffer[index++];
            Mppt3Status = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(MpptTimestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(Mppt1Volt).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Mppt1Amp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Mppt2Volt).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Mppt2Amp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Mppt3Volt).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Mppt3Amp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Mppt1Pwm).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Mppt2Pwm).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Mppt3Pwm).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Mppt1Status).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Mppt2Status).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Mppt3Status).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/41;
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
    public class AslctrlDataPacket: PacketV2<AslctrlDataPayload>
    {
	    public const int PacketMessageId = 203;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 172;

        public override AslctrlDataPayload Payload { get; } = new AslctrlDataPayload();

        public override string Name => "ASLCTRL_DATA";
    }

    /// <summary>
    ///  ASLCTRL_DATA
    /// </summary>
    public class AslctrlDataPayload : IPayload
    {
        public byte GetMaxByteSize() => 98; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 98; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Timestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            H = BinSerialize.ReadFloat(ref buffer);index+=4;
            Href = BinSerialize.ReadFloat(ref buffer);index+=4;
            HrefT = BinSerialize.ReadFloat(ref buffer);index+=4;
            Pitchangle = BinSerialize.ReadFloat(ref buffer);index+=4;
            Pitchangleref = BinSerialize.ReadFloat(ref buffer);index+=4;
            Q = BinSerialize.ReadFloat(ref buffer);index+=4;
            Qref = BinSerialize.ReadFloat(ref buffer);index+=4;
            Uelev = BinSerialize.ReadFloat(ref buffer);index+=4;
            Uthrot = BinSerialize.ReadFloat(ref buffer);index+=4;
            Uthrot2 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Nz = BinSerialize.ReadFloat(ref buffer);index+=4;
            Airspeedref = BinSerialize.ReadFloat(ref buffer);index+=4;
            Yawangle = BinSerialize.ReadFloat(ref buffer);index+=4;
            Yawangleref = BinSerialize.ReadFloat(ref buffer);index+=4;
            Rollangle = BinSerialize.ReadFloat(ref buffer);index+=4;
            Rollangleref = BinSerialize.ReadFloat(ref buffer);index+=4;
            P = BinSerialize.ReadFloat(ref buffer);index+=4;
            Pref = BinSerialize.ReadFloat(ref buffer);index+=4;
            R = BinSerialize.ReadFloat(ref buffer);index+=4;
            Rref = BinSerialize.ReadFloat(ref buffer);index+=4;
            Uail = BinSerialize.ReadFloat(ref buffer);index+=4;
            Urud = BinSerialize.ReadFloat(ref buffer);index+=4;
            AslctrlMode = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Spoilersengaged = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,Timestamp);index+=8;
            BinSerialize.WriteFloat(ref buffer,H);index+=4;
            BinSerialize.WriteFloat(ref buffer,Href);index+=4;
            BinSerialize.WriteFloat(ref buffer,HrefT);index+=4;
            BinSerialize.WriteFloat(ref buffer,Pitchangle);index+=4;
            BinSerialize.WriteFloat(ref buffer,Pitchangleref);index+=4;
            BinSerialize.WriteFloat(ref buffer,Q);index+=4;
            BinSerialize.WriteFloat(ref buffer,Qref);index+=4;
            BinSerialize.WriteFloat(ref buffer,Uelev);index+=4;
            BinSerialize.WriteFloat(ref buffer,Uthrot);index+=4;
            BinSerialize.WriteFloat(ref buffer,Uthrot2);index+=4;
            BinSerialize.WriteFloat(ref buffer,Nz);index+=4;
            BinSerialize.WriteFloat(ref buffer,Airspeedref);index+=4;
            BinSerialize.WriteFloat(ref buffer,Yawangle);index+=4;
            BinSerialize.WriteFloat(ref buffer,Yawangleref);index+=4;
            BinSerialize.WriteFloat(ref buffer,Rollangle);index+=4;
            BinSerialize.WriteFloat(ref buffer,Rollangleref);index+=4;
            BinSerialize.WriteFloat(ref buffer,P);index+=4;
            BinSerialize.WriteFloat(ref buffer,Pref);index+=4;
            BinSerialize.WriteFloat(ref buffer,R);index+=4;
            BinSerialize.WriteFloat(ref buffer,Rref);index+=4;
            BinSerialize.WriteFloat(ref buffer,Uail);index+=4;
            BinSerialize.WriteFloat(ref buffer,Urud);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)AslctrlMode);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Spoilersengaged);index+=1;
            return index; // /*PayloadByteSize*/98;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Timestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            H = BitConverter.ToSingle(buffer, index);index+=4;
            Href = BitConverter.ToSingle(buffer, index);index+=4;
            HrefT = BitConverter.ToSingle(buffer, index);index+=4;
            Pitchangle = BitConverter.ToSingle(buffer, index);index+=4;
            Pitchangleref = BitConverter.ToSingle(buffer, index);index+=4;
            Q = BitConverter.ToSingle(buffer, index);index+=4;
            Qref = BitConverter.ToSingle(buffer, index);index+=4;
            Uelev = BitConverter.ToSingle(buffer, index);index+=4;
            Uthrot = BitConverter.ToSingle(buffer, index);index+=4;
            Uthrot2 = BitConverter.ToSingle(buffer, index);index+=4;
            Nz = BitConverter.ToSingle(buffer, index);index+=4;
            Airspeedref = BitConverter.ToSingle(buffer, index);index+=4;
            Yawangle = BitConverter.ToSingle(buffer, index);index+=4;
            Yawangleref = BitConverter.ToSingle(buffer, index);index+=4;
            Rollangle = BitConverter.ToSingle(buffer, index);index+=4;
            Rollangleref = BitConverter.ToSingle(buffer, index);index+=4;
            P = BitConverter.ToSingle(buffer, index);index+=4;
            Pref = BitConverter.ToSingle(buffer, index);index+=4;
            R = BitConverter.ToSingle(buffer, index);index+=4;
            Rref = BitConverter.ToSingle(buffer, index);index+=4;
            Uail = BitConverter.ToSingle(buffer, index);index+=4;
            Urud = BitConverter.ToSingle(buffer, index);index+=4;
            AslctrlMode = (byte)buffer[index++];
            Spoilersengaged = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Timestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(H).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Href).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(HrefT).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Pitchangle).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Pitchangleref).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Q).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Qref).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Uelev).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Uthrot).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Uthrot2).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Nz).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Airspeedref).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Yawangle).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Yawangleref).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Rollangle).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Rollangleref).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(P).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Pref).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(R).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Rref).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Uail).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Urud).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AslctrlMode).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Spoilersengaged).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/98;
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
    public class AslctrlDebugPacket: PacketV2<AslctrlDebugPayload>
    {
	    public const int PacketMessageId = 204;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 251;

        public override AslctrlDebugPayload Payload { get; } = new AslctrlDebugPayload();

        public override string Name => "ASLCTRL_DEBUG";
    }

    /// <summary>
    ///  ASLCTRL_DEBUG
    /// </summary>
    public class AslctrlDebugPayload : IPayload
    {
        public byte GetMaxByteSize() => 38; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 38; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            I321 = BinSerialize.ReadUInt(ref buffer);index+=4;
            F1 = BinSerialize.ReadFloat(ref buffer);index+=4;
            F2 = BinSerialize.ReadFloat(ref buffer);index+=4;
            F3 = BinSerialize.ReadFloat(ref buffer);index+=4;
            F4 = BinSerialize.ReadFloat(ref buffer);index+=4;
            F5 = BinSerialize.ReadFloat(ref buffer);index+=4;
            F6 = BinSerialize.ReadFloat(ref buffer);index+=4;
            F7 = BinSerialize.ReadFloat(ref buffer);index+=4;
            F8 = BinSerialize.ReadFloat(ref buffer);index+=4;
            I81 = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            I82 = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,I321);index+=4;
            BinSerialize.WriteFloat(ref buffer,F1);index+=4;
            BinSerialize.WriteFloat(ref buffer,F2);index+=4;
            BinSerialize.WriteFloat(ref buffer,F3);index+=4;
            BinSerialize.WriteFloat(ref buffer,F4);index+=4;
            BinSerialize.WriteFloat(ref buffer,F5);index+=4;
            BinSerialize.WriteFloat(ref buffer,F6);index+=4;
            BinSerialize.WriteFloat(ref buffer,F7);index+=4;
            BinSerialize.WriteFloat(ref buffer,F8);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)I81);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)I82);index+=1;
            return index; // /*PayloadByteSize*/38;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            I321 = BitConverter.ToUInt32(buffer,index);index+=4;
            F1 = BitConverter.ToSingle(buffer, index);index+=4;
            F2 = BitConverter.ToSingle(buffer, index);index+=4;
            F3 = BitConverter.ToSingle(buffer, index);index+=4;
            F4 = BitConverter.ToSingle(buffer, index);index+=4;
            F5 = BitConverter.ToSingle(buffer, index);index+=4;
            F6 = BitConverter.ToSingle(buffer, index);index+=4;
            F7 = BitConverter.ToSingle(buffer, index);index+=4;
            F8 = BitConverter.ToSingle(buffer, index);index+=4;
            I81 = (byte)buffer[index++];
            I82 = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(I321).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(F1).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(F2).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(F3).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(F4).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(F5).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(F6).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(F7).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(F8).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(I81).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(I82).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/38;
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
    public class AsluavStatusPacket: PacketV2<AsluavStatusPayload>
    {
	    public const int PacketMessageId = 205;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 97;

        public override AsluavStatusPayload Payload { get; } = new AsluavStatusPayload();

        public override string Name => "ASLUAV_STATUS";
    }

    /// <summary>
    ///  ASLUAV_STATUS
    /// </summary>
    public class AsluavStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 14; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 14; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            MotorRpm = BinSerialize.ReadFloat(ref buffer);index+=4;
            LedStatus = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            SatcomStatus = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/8 - Math.Max(0,((/*PayloadByteSize*/14 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            ServoStatus = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                ServoStatus[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,MotorRpm);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)LedStatus);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)SatcomStatus);index+=1;
            for(var i=0;i<ServoStatus.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)ServoStatus[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/14;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            MotorRpm = BitConverter.ToSingle(buffer, index);index+=4;
            LedStatus = (byte)buffer[index++];
            SatcomStatus = (byte)buffer[index++];
            arraySize = /*ArrayLength*/8 - Math.Max(0,((/*PayloadByteSize*/14 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            ServoStatus = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                ServoStatus[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(MotorRpm).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(LedStatus).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(SatcomStatus).CopyTo(buffer, index);index+=1;
            for(var i=0;i<ServoStatus.Length;i++)
            {
                buffer[index] = (byte)ServoStatus[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/14;
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
        public byte[] ServoStatus { get; set; } = new byte[8];
        public byte GetServoStatusMaxItemsCount() => 8;
    }
    /// <summary>
    /// Extended EKF state estimates for ASLUAVs
    ///  EKF_EXT
    /// </summary>
    public class EkfExtPacket: PacketV2<EkfExtPayload>
    {
	    public const int PacketMessageId = 206;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 64;

        public override EkfExtPayload Payload { get; } = new EkfExtPayload();

        public override string Name => "EKF_EXT";
    }

    /// <summary>
    ///  EKF_EXT
    /// </summary>
    public class EkfExtPayload : IPayload
    {
        public byte GetMaxByteSize() => 32; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 32; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Timestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            Windspeed = BinSerialize.ReadFloat(ref buffer);index+=4;
            Winddir = BinSerialize.ReadFloat(ref buffer);index+=4;
            Windz = BinSerialize.ReadFloat(ref buffer);index+=4;
            Airspeed = BinSerialize.ReadFloat(ref buffer);index+=4;
            Beta = BinSerialize.ReadFloat(ref buffer);index+=4;
            Alpha = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,Timestamp);index+=8;
            BinSerialize.WriteFloat(ref buffer,Windspeed);index+=4;
            BinSerialize.WriteFloat(ref buffer,Winddir);index+=4;
            BinSerialize.WriteFloat(ref buffer,Windz);index+=4;
            BinSerialize.WriteFloat(ref buffer,Airspeed);index+=4;
            BinSerialize.WriteFloat(ref buffer,Beta);index+=4;
            BinSerialize.WriteFloat(ref buffer,Alpha);index+=4;
            return index; // /*PayloadByteSize*/32;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Timestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            Windspeed = BitConverter.ToSingle(buffer, index);index+=4;
            Winddir = BitConverter.ToSingle(buffer, index);index+=4;
            Windz = BitConverter.ToSingle(buffer, index);index+=4;
            Airspeed = BitConverter.ToSingle(buffer, index);index+=4;
            Beta = BitConverter.ToSingle(buffer, index);index+=4;
            Alpha = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Timestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(Windspeed).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Winddir).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Windz).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Airspeed).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Beta).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Alpha).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/32;
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
    public class AslObctrlPacket: PacketV2<AslObctrlPayload>
    {
	    public const int PacketMessageId = 207;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 234;

        public override AslObctrlPayload Payload { get; } = new AslObctrlPayload();

        public override string Name => "ASL_OBCTRL";
    }

    /// <summary>
    ///  ASL_OBCTRL
    /// </summary>
    public class AslObctrlPayload : IPayload
    {
        public byte GetMaxByteSize() => 33; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 33; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Timestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            Uelev = BinSerialize.ReadFloat(ref buffer);index+=4;
            Uthrot = BinSerialize.ReadFloat(ref buffer);index+=4;
            Uthrot2 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Uaill = BinSerialize.ReadFloat(ref buffer);index+=4;
            Uailr = BinSerialize.ReadFloat(ref buffer);index+=4;
            Urud = BinSerialize.ReadFloat(ref buffer);index+=4;
            ObctrlStatus = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,Timestamp);index+=8;
            BinSerialize.WriteFloat(ref buffer,Uelev);index+=4;
            BinSerialize.WriteFloat(ref buffer,Uthrot);index+=4;
            BinSerialize.WriteFloat(ref buffer,Uthrot2);index+=4;
            BinSerialize.WriteFloat(ref buffer,Uaill);index+=4;
            BinSerialize.WriteFloat(ref buffer,Uailr);index+=4;
            BinSerialize.WriteFloat(ref buffer,Urud);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)ObctrlStatus);index+=1;
            return index; // /*PayloadByteSize*/33;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Timestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            Uelev = BitConverter.ToSingle(buffer, index);index+=4;
            Uthrot = BitConverter.ToSingle(buffer, index);index+=4;
            Uthrot2 = BitConverter.ToSingle(buffer, index);index+=4;
            Uaill = BitConverter.ToSingle(buffer, index);index+=4;
            Uailr = BitConverter.ToSingle(buffer, index);index+=4;
            Urud = BitConverter.ToSingle(buffer, index);index+=4;
            ObctrlStatus = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Timestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(Uelev).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Uthrot).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Uthrot2).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Uaill).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Uailr).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Urud).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(ObctrlStatus).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/33;
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
    public class SensAtmosPacket: PacketV2<SensAtmosPayload>
    {
	    public const int PacketMessageId = 208;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 144;

        public override SensAtmosPayload Payload { get; } = new SensAtmosPayload();

        public override string Name => "SENS_ATMOS";
    }

    /// <summary>
    ///  SENS_ATMOS
    /// </summary>
    public class SensAtmosPayload : IPayload
    {
        public byte GetMaxByteSize() => 16; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Timestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            Tempambient = BinSerialize.ReadFloat(ref buffer);index+=4;
            Humidity = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,Timestamp);index+=8;
            BinSerialize.WriteFloat(ref buffer,Tempambient);index+=4;
            BinSerialize.WriteFloat(ref buffer,Humidity);index+=4;
            return index; // /*PayloadByteSize*/16;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Timestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            Tempambient = BitConverter.ToSingle(buffer, index);index+=4;
            Humidity = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Timestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(Tempambient).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Humidity).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/16;
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
    public class SensBatmonPacket: PacketV2<SensBatmonPayload>
    {
	    public const int PacketMessageId = 209;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 155;

        public override SensBatmonPayload Payload { get; } = new SensBatmonPayload();

        public override string Name => "SENS_BATMON";
    }

    /// <summary>
    ///  SENS_BATMON
    /// </summary>
    public class SensBatmonPayload : IPayload
    {
        public byte GetMaxByteSize() => 41; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 41; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            BatmonTimestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            Temperature = BinSerialize.ReadFloat(ref buffer);index+=4;
            Safetystatus = BinSerialize.ReadUInt(ref buffer);index+=4;
            Operationstatus = BinSerialize.ReadUInt(ref buffer);index+=4;
            Voltage = BinSerialize.ReadUShort(ref buffer);index+=2;
            Current = BinSerialize.ReadShort(ref buffer);index+=2;
            Batterystatus = BinSerialize.ReadUShort(ref buffer);index+=2;
            Serialnumber = BinSerialize.ReadUShort(ref buffer);index+=2;
            Cellvoltage1 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Cellvoltage2 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Cellvoltage3 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Cellvoltage4 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Cellvoltage5 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Cellvoltage6 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Soc = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,BatmonTimestamp);index+=8;
            BinSerialize.WriteFloat(ref buffer,Temperature);index+=4;
            BinSerialize.WriteUInt(ref buffer,Safetystatus);index+=4;
            BinSerialize.WriteUInt(ref buffer,Operationstatus);index+=4;
            BinSerialize.WriteUShort(ref buffer,Voltage);index+=2;
            BinSerialize.WriteShort(ref buffer,Current);index+=2;
            BinSerialize.WriteUShort(ref buffer,Batterystatus);index+=2;
            BinSerialize.WriteUShort(ref buffer,Serialnumber);index+=2;
            BinSerialize.WriteUShort(ref buffer,Cellvoltage1);index+=2;
            BinSerialize.WriteUShort(ref buffer,Cellvoltage2);index+=2;
            BinSerialize.WriteUShort(ref buffer,Cellvoltage3);index+=2;
            BinSerialize.WriteUShort(ref buffer,Cellvoltage4);index+=2;
            BinSerialize.WriteUShort(ref buffer,Cellvoltage5);index+=2;
            BinSerialize.WriteUShort(ref buffer,Cellvoltage6);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)Soc);index+=1;
            return index; // /*PayloadByteSize*/41;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            BatmonTimestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            Temperature = BitConverter.ToSingle(buffer, index);index+=4;
            Safetystatus = BitConverter.ToUInt32(buffer,index);index+=4;
            Operationstatus = BitConverter.ToUInt32(buffer,index);index+=4;
            Voltage = BitConverter.ToUInt16(buffer,index);index+=2;
            Current = BitConverter.ToInt16(buffer,index);index+=2;
            Batterystatus = BitConverter.ToUInt16(buffer,index);index+=2;
            Serialnumber = BitConverter.ToUInt16(buffer,index);index+=2;
            Cellvoltage1 = BitConverter.ToUInt16(buffer,index);index+=2;
            Cellvoltage2 = BitConverter.ToUInt16(buffer,index);index+=2;
            Cellvoltage3 = BitConverter.ToUInt16(buffer,index);index+=2;
            Cellvoltage4 = BitConverter.ToUInt16(buffer,index);index+=2;
            Cellvoltage5 = BitConverter.ToUInt16(buffer,index);index+=2;
            Cellvoltage6 = BitConverter.ToUInt16(buffer,index);index+=2;
            Soc = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(BatmonTimestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(Temperature).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Safetystatus).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Operationstatus).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Voltage).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Current).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Batterystatus).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Serialnumber).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Cellvoltage1).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Cellvoltage2).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Cellvoltage3).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Cellvoltage4).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Cellvoltage5).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Cellvoltage6).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Soc).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/41;
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
    public class FwSoaringDataPacket: PacketV2<FwSoaringDataPayload>
    {
	    public const int PacketMessageId = 210;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 20;

        public override FwSoaringDataPayload Payload { get; } = new FwSoaringDataPayload();

        public override string Name => "FW_SOARING_DATA";
    }

    /// <summary>
    ///  FW_SOARING_DATA
    /// </summary>
    public class FwSoaringDataPayload : IPayload
    {
        public byte GetMaxByteSize() => 102; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 102; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Timestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            Timestampmodechanged = BinSerialize.ReadULong(ref buffer);index+=8;
            Xw = BinSerialize.ReadFloat(ref buffer);index+=4;
            Xr = BinSerialize.ReadFloat(ref buffer);index+=4;
            Xlat = BinSerialize.ReadFloat(ref buffer);index+=4;
            Xlon = BinSerialize.ReadFloat(ref buffer);index+=4;
            Varw = BinSerialize.ReadFloat(ref buffer);index+=4;
            Varr = BinSerialize.ReadFloat(ref buffer);index+=4;
            Varlat = BinSerialize.ReadFloat(ref buffer);index+=4;
            Varlon = BinSerialize.ReadFloat(ref buffer);index+=4;
            Loiterradius = BinSerialize.ReadFloat(ref buffer);index+=4;
            Loiterdirection = BinSerialize.ReadFloat(ref buffer);index+=4;
            Disttosoarpoint = BinSerialize.ReadFloat(ref buffer);index+=4;
            Vsinkexp = BinSerialize.ReadFloat(ref buffer);index+=4;
            Z1Localupdraftspeed = BinSerialize.ReadFloat(ref buffer);index+=4;
            Z2Deltaroll = BinSerialize.ReadFloat(ref buffer);index+=4;
            Z1Exp = BinSerialize.ReadFloat(ref buffer);index+=4;
            Z2Exp = BinSerialize.ReadFloat(ref buffer);index+=4;
            Thermalgsnorth = BinSerialize.ReadFloat(ref buffer);index+=4;
            Thermalgseast = BinSerialize.ReadFloat(ref buffer);index+=4;
            TseDot = BinSerialize.ReadFloat(ref buffer);index+=4;
            Debugvar1 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Debugvar2 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Controlmode = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Valid = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,Timestamp);index+=8;
            BinSerialize.WriteULong(ref buffer,Timestampmodechanged);index+=8;
            BinSerialize.WriteFloat(ref buffer,Xw);index+=4;
            BinSerialize.WriteFloat(ref buffer,Xr);index+=4;
            BinSerialize.WriteFloat(ref buffer,Xlat);index+=4;
            BinSerialize.WriteFloat(ref buffer,Xlon);index+=4;
            BinSerialize.WriteFloat(ref buffer,Varw);index+=4;
            BinSerialize.WriteFloat(ref buffer,Varr);index+=4;
            BinSerialize.WriteFloat(ref buffer,Varlat);index+=4;
            BinSerialize.WriteFloat(ref buffer,Varlon);index+=4;
            BinSerialize.WriteFloat(ref buffer,Loiterradius);index+=4;
            BinSerialize.WriteFloat(ref buffer,Loiterdirection);index+=4;
            BinSerialize.WriteFloat(ref buffer,Disttosoarpoint);index+=4;
            BinSerialize.WriteFloat(ref buffer,Vsinkexp);index+=4;
            BinSerialize.WriteFloat(ref buffer,Z1Localupdraftspeed);index+=4;
            BinSerialize.WriteFloat(ref buffer,Z2Deltaroll);index+=4;
            BinSerialize.WriteFloat(ref buffer,Z1Exp);index+=4;
            BinSerialize.WriteFloat(ref buffer,Z2Exp);index+=4;
            BinSerialize.WriteFloat(ref buffer,Thermalgsnorth);index+=4;
            BinSerialize.WriteFloat(ref buffer,Thermalgseast);index+=4;
            BinSerialize.WriteFloat(ref buffer,TseDot);index+=4;
            BinSerialize.WriteFloat(ref buffer,Debugvar1);index+=4;
            BinSerialize.WriteFloat(ref buffer,Debugvar2);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)Controlmode);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Valid);index+=1;
            return index; // /*PayloadByteSize*/102;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Timestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            Timestampmodechanged = BitConverter.ToUInt64(buffer,index);index+=8;
            Xw = BitConverter.ToSingle(buffer, index);index+=4;
            Xr = BitConverter.ToSingle(buffer, index);index+=4;
            Xlat = BitConverter.ToSingle(buffer, index);index+=4;
            Xlon = BitConverter.ToSingle(buffer, index);index+=4;
            Varw = BitConverter.ToSingle(buffer, index);index+=4;
            Varr = BitConverter.ToSingle(buffer, index);index+=4;
            Varlat = BitConverter.ToSingle(buffer, index);index+=4;
            Varlon = BitConverter.ToSingle(buffer, index);index+=4;
            Loiterradius = BitConverter.ToSingle(buffer, index);index+=4;
            Loiterdirection = BitConverter.ToSingle(buffer, index);index+=4;
            Disttosoarpoint = BitConverter.ToSingle(buffer, index);index+=4;
            Vsinkexp = BitConverter.ToSingle(buffer, index);index+=4;
            Z1Localupdraftspeed = BitConverter.ToSingle(buffer, index);index+=4;
            Z2Deltaroll = BitConverter.ToSingle(buffer, index);index+=4;
            Z1Exp = BitConverter.ToSingle(buffer, index);index+=4;
            Z2Exp = BitConverter.ToSingle(buffer, index);index+=4;
            Thermalgsnorth = BitConverter.ToSingle(buffer, index);index+=4;
            Thermalgseast = BitConverter.ToSingle(buffer, index);index+=4;
            TseDot = BitConverter.ToSingle(buffer, index);index+=4;
            Debugvar1 = BitConverter.ToSingle(buffer, index);index+=4;
            Debugvar2 = BitConverter.ToSingle(buffer, index);index+=4;
            Controlmode = (byte)buffer[index++];
            Valid = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Timestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(Timestampmodechanged).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(Xw).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Xr).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Xlat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Xlon).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Varw).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Varr).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Varlat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Varlon).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Loiterradius).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Loiterdirection).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Disttosoarpoint).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Vsinkexp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Z1Localupdraftspeed).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Z2Deltaroll).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Z1Exp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Z2Exp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Thermalgsnorth).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Thermalgseast).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TseDot).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Debugvar1).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Debugvar2).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Controlmode).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Valid).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/102;
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
    public class SensorpodStatusPacket: PacketV2<SensorpodStatusPayload>
    {
	    public const int PacketMessageId = 211;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 54;

        public override SensorpodStatusPayload Payload { get; } = new SensorpodStatusPayload();

        public override string Name => "SENSORPOD_STATUS";
    }

    /// <summary>
    ///  SENSORPOD_STATUS
    /// </summary>
    public class SensorpodStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 16; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Timestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            FreeSpace = BinSerialize.ReadUShort(ref buffer);index+=2;
            VisensorRate1 = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            VisensorRate2 = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            VisensorRate3 = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            VisensorRate4 = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            RecordingNodesCount = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CpuTemp = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,Timestamp);index+=8;
            BinSerialize.WriteUShort(ref buffer,FreeSpace);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)VisensorRate1);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)VisensorRate2);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)VisensorRate3);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)VisensorRate4);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)RecordingNodesCount);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CpuTemp);index+=1;
            return index; // /*PayloadByteSize*/16;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Timestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            FreeSpace = BitConverter.ToUInt16(buffer,index);index+=2;
            VisensorRate1 = (byte)buffer[index++];
            VisensorRate2 = (byte)buffer[index++];
            VisensorRate3 = (byte)buffer[index++];
            VisensorRate4 = (byte)buffer[index++];
            RecordingNodesCount = (byte)buffer[index++];
            CpuTemp = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Timestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(FreeSpace).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(VisensorRate1).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(VisensorRate2).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(VisensorRate3).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(VisensorRate4).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(RecordingNodesCount).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(CpuTemp).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/16;
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
    public class SensPowerBoardPacket: PacketV2<SensPowerBoardPayload>
    {
	    public const int PacketMessageId = 212;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 222;

        public override SensPowerBoardPayload Payload { get; } = new SensPowerBoardPayload();

        public override string Name => "SENS_POWER_BOARD";
    }

    /// <summary>
    ///  SENS_POWER_BOARD
    /// </summary>
    public class SensPowerBoardPayload : IPayload
    {
        public byte GetMaxByteSize() => 46; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 46; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Timestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            PwrBrdSystemVolt = BinSerialize.ReadFloat(ref buffer);index+=4;
            PwrBrdServoVolt = BinSerialize.ReadFloat(ref buffer);index+=4;
            PwrBrdDigitalVolt = BinSerialize.ReadFloat(ref buffer);index+=4;
            PwrBrdMotLAmp = BinSerialize.ReadFloat(ref buffer);index+=4;
            PwrBrdMotRAmp = BinSerialize.ReadFloat(ref buffer);index+=4;
            PwrBrdAnalogAmp = BinSerialize.ReadFloat(ref buffer);index+=4;
            PwrBrdDigitalAmp = BinSerialize.ReadFloat(ref buffer);index+=4;
            PwrBrdExtAmp = BinSerialize.ReadFloat(ref buffer);index+=4;
            PwrBrdAuxAmp = BinSerialize.ReadFloat(ref buffer);index+=4;
            PwrBrdStatus = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            PwrBrdLedStatus = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,Timestamp);index+=8;
            BinSerialize.WriteFloat(ref buffer,PwrBrdSystemVolt);index+=4;
            BinSerialize.WriteFloat(ref buffer,PwrBrdServoVolt);index+=4;
            BinSerialize.WriteFloat(ref buffer,PwrBrdDigitalVolt);index+=4;
            BinSerialize.WriteFloat(ref buffer,PwrBrdMotLAmp);index+=4;
            BinSerialize.WriteFloat(ref buffer,PwrBrdMotRAmp);index+=4;
            BinSerialize.WriteFloat(ref buffer,PwrBrdAnalogAmp);index+=4;
            BinSerialize.WriteFloat(ref buffer,PwrBrdDigitalAmp);index+=4;
            BinSerialize.WriteFloat(ref buffer,PwrBrdExtAmp);index+=4;
            BinSerialize.WriteFloat(ref buffer,PwrBrdAuxAmp);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)PwrBrdStatus);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)PwrBrdLedStatus);index+=1;
            return index; // /*PayloadByteSize*/46;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Timestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            PwrBrdSystemVolt = BitConverter.ToSingle(buffer, index);index+=4;
            PwrBrdServoVolt = BitConverter.ToSingle(buffer, index);index+=4;
            PwrBrdDigitalVolt = BitConverter.ToSingle(buffer, index);index+=4;
            PwrBrdMotLAmp = BitConverter.ToSingle(buffer, index);index+=4;
            PwrBrdMotRAmp = BitConverter.ToSingle(buffer, index);index+=4;
            PwrBrdAnalogAmp = BitConverter.ToSingle(buffer, index);index+=4;
            PwrBrdDigitalAmp = BitConverter.ToSingle(buffer, index);index+=4;
            PwrBrdExtAmp = BitConverter.ToSingle(buffer, index);index+=4;
            PwrBrdAuxAmp = BitConverter.ToSingle(buffer, index);index+=4;
            PwrBrdStatus = (byte)buffer[index++];
            PwrBrdLedStatus = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Timestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(PwrBrdSystemVolt).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PwrBrdServoVolt).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PwrBrdDigitalVolt).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PwrBrdMotLAmp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PwrBrdMotRAmp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PwrBrdAnalogAmp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PwrBrdDigitalAmp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PwrBrdExtAmp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PwrBrdAuxAmp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PwrBrdStatus).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(PwrBrdLedStatus).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/46;
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
    public class GsmLinkStatusPacket: PacketV2<GsmLinkStatusPayload>
    {
	    public const int PacketMessageId = 213;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 200;

        public override GsmLinkStatusPayload Payload { get; } = new GsmLinkStatusPayload();

        public override string Name => "GSM_LINK_STATUS";
    }

    /// <summary>
    ///  GSM_LINK_STATUS
    /// </summary>
    public class GsmLinkStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 14; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 14; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Timestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            GsmModemType = (GsmModemType)BinSerialize.ReadByte(ref buffer);index+=1;
            GsmLinkType = (GsmLinkType)BinSerialize.ReadByte(ref buffer);index+=1;
            Rssi = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            RsrpRscp = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            SinrEcio = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Rsrq = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,Timestamp);index+=8;
            BinSerialize.WriteByte(ref buffer,(byte)GsmModemType);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)GsmLinkType);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Rssi);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)RsrpRscp);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)SinrEcio);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Rsrq);index+=1;
            return index; // /*PayloadByteSize*/14;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Timestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            GsmModemType = (GsmModemType)buffer[index++];
            GsmLinkType = (GsmLinkType)buffer[index++];
            Rssi = (byte)buffer[index++];
            RsrpRscp = (byte)buffer[index++];
            SinrEcio = (byte)buffer[index++];
            Rsrq = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Timestamp).CopyTo(buffer, index);index+=8;
            buffer[index] = (byte)GsmModemType;index+=1;
            buffer[index] = (byte)GsmLinkType;index+=1;
            BitConverter.GetBytes(Rssi).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(RsrpRscp).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(SinrEcio).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Rsrq).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/14;
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
    public class SatcomLinkStatusPacket: PacketV2<SatcomLinkStatusPayload>
    {
	    public const int PacketMessageId = 214;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 23;

        public override SatcomLinkStatusPayload Payload { get; } = new SatcomLinkStatusPayload();

        public override string Name => "SATCOM_LINK_STATUS";
    }

    /// <summary>
    ///  SATCOM_LINK_STATUS
    /// </summary>
    public class SatcomLinkStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 24; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Timestamp = BinSerialize.ReadULong(ref buffer);index+=8;
            LastHeartbeat = BinSerialize.ReadULong(ref buffer);index+=8;
            FailedSessions = BinSerialize.ReadUShort(ref buffer);index+=2;
            SuccessfulSessions = BinSerialize.ReadUShort(ref buffer);index+=2;
            SignalQuality = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            RingPending = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TxSessionPending = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            RxSessionPending = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,Timestamp);index+=8;
            BinSerialize.WriteULong(ref buffer,LastHeartbeat);index+=8;
            BinSerialize.WriteUShort(ref buffer,FailedSessions);index+=2;
            BinSerialize.WriteUShort(ref buffer,SuccessfulSessions);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)SignalQuality);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)RingPending);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TxSessionPending);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)RxSessionPending);index+=1;
            return index; // /*PayloadByteSize*/24;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Timestamp = BitConverter.ToUInt64(buffer,index);index+=8;
            LastHeartbeat = BitConverter.ToUInt64(buffer,index);index+=8;
            FailedSessions = BitConverter.ToUInt16(buffer,index);index+=2;
            SuccessfulSessions = BitConverter.ToUInt16(buffer,index);index+=2;
            SignalQuality = (byte)buffer[index++];
            RingPending = (byte)buffer[index++];
            TxSessionPending = (byte)buffer[index++];
            RxSessionPending = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Timestamp).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(LastHeartbeat).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(FailedSessions).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(SuccessfulSessions).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(SignalQuality).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(RingPending).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TxSessionPending).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(RxSessionPending).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/24;
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


#endregion


}

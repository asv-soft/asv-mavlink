// MIT License
//
// Copyright (c) 2018 Alexey Voloshkevich Cursir ltd. (https://github.com/asvol)
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

// This code was generate by tool Asv.Mavlink.Shell version 1.0.0

using System;
using Asv.IO;

namespace Asv.Mavlink.V2.Avssuas
{

    public static class AvssuasHelper
    {
        public static void RegisterAvssuasDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AvssPrsSysStatusPacket());
        }
    }

#region Enums

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// AVSS defined command. Set PRS arm statuses.
        /// Param 1 - PRS arm statuses
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_SET_ARM
        /// </summary>
        MavCmdPrsSetArm = 60050,
        /// <summary>
        /// AVSS defined command. Gets PRS arm statuses
        /// Param 1 - User defined
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_GET_ARM
        /// </summary>
        MavCmdPrsGetArm = 60051,
        /// <summary>
        /// AVSS defined command.  Get the PRS battery voltage in millivolts
        /// Param 1 - User defined
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_GET_BATTERY
        /// </summary>
        MavCmdPrsGetBattery = 60052,
        /// <summary>
        /// AVSS defined command. Get the PRS error statuses.
        /// Param 1 - User defined
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_GET_ERR
        /// </summary>
        MavCmdPrsGetErr = 60053,
        /// <summary>
        /// AVSS defined command. Set the ATS arming altitude in meters.
        /// Param 1 - ATS arming altitude
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_SET_ARM_ALTI
        /// </summary>
        MavCmdPrsSetArmAlti = 60070,
        /// <summary>
        /// AVSS defined command. Get the ATS arming altitude in meters.
        /// Param 1 - User defined
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_GET_ARM_ALTI
        /// </summary>
        MavCmdPrsGetArmAlti = 60071,
        /// <summary>
        /// AVSS defined command. Shuts down the PRS system.
        /// Param 1 - User defined
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_SHUTDOWN
        /// </summary>
        MavCmdPrsShutdown = 60072,
        /// <summary>
        /// AVSS defined command. Set the threshold to charge from outside in millivolts
        /// Param 1 - Charge Threshold
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_SET_CHARGE_MV
        /// </summary>
        MavCmdPrsSetChargeMv = 60073,
        /// <summary>
        /// AVSS defined command. Get the threshold to charge from outside in millivolts.
        /// Param 1 - User defined
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_GET_CHARGE_MV
        /// </summary>
        MavCmdPrsGetChargeMv = 60074,
        /// <summary>
        /// AVSS defined command. Set the timeout between FTS request and deploying the chute.
        /// Param 1 - User defined
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_SET_TIMEOUT
        /// </summary>
        MavCmdPrsSetTimeout = 60075,
        /// <summary>
        /// AVSS defined command. Get the timeout between FTS request and deploying the chute.
        /// Param 1 - User defined
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_GET_TIMEOUT
        /// </summary>
        MavCmdPrsGetTimeout = 60076,
        /// <summary>
        /// AVSS defined command. Set up the PRS to connect to the drone..
        /// Param 1 - User defined
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_SET_FTS_CONNECT
        /// </summary>
        MavCmdPrsSetFtsConnect = 60077,
        /// <summary>
        /// AVSS defined command. Get the connection status of PRS and drone.
        /// Param 1 - User defined
        /// Param 2 - User defined
        /// Param 3 - User defined
        /// Param 4 - User defined
        /// Param 5 - User defined
        /// Param 6 - User defined
        /// Param 7 - User defined
        /// MAV_CMD_PRS_GET_FTS_CONNECT
        /// </summary>
        MavCmdPrsGetFtsConnect = 60078,
    }

    /// <summary>
    ///  MAV_AVSS_COMMAND_FAILURE_REASON
    /// </summary>
    public enum MavAvssCommandFailureReason:uint
    {
        /// <summary>
        /// AVSS defined command failure reason. PRS not steady.
        /// PRS_NOT_STEADY
        /// </summary>
        PrsNotSteady = 1,
        /// <summary>
        /// AVSS defined command failure reason. PRS DTM not armed.
        /// PRS_DTM_NOT_ARMED
        /// </summary>
        PrsDtmNotArmed = 2,
        /// <summary>
        /// AVSS defined command failure reason. PRS OTM not armed.
        /// PRS_OTM_NOT_ARMED
        /// </summary>
        PrsOtmNotArmed = 3,
    }


#endregion

#region Messages

    /// <summary>
    ///  AVSS PRS system status.
    ///  AVSS_PRS_SYS_STATUS
    /// </summary>
    public class AvssPrsSysStatusPacket: PacketV2<AvssPrsSysStatusPayload>
    {
	    public const int PacketMessageId = 60050;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 153;

        public override AvssPrsSysStatusPayload Payload { get; } = new AvssPrsSysStatusPayload();

        public override string Name => "AVSS_PRS_SYS_STATUS";
    }

    /// <summary>
    ///  AVSS_PRS_SYS_STATUS
    /// </summary>
    public class AvssPrsSysStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 12; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 12; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            ErrorStatus = BinSerialize.ReadUInt(ref buffer);index+=4;
            TimeBootMs = BinSerialize.ReadUInt(ref buffer);index+=4;
            BatteryStatus = BinSerialize.ReadUShort(ref buffer);index+=2;
            ArmStatus = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            ChangeStatus = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,ErrorStatus);index+=4;
            BinSerialize.WriteUInt(ref buffer,TimeBootMs);index+=4;
            BinSerialize.WriteUShort(ref buffer,BatteryStatus);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)ArmStatus);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)ChangeStatus);index+=1;
            return index; // /*PayloadByteSize*/12;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            ErrorStatus = BitConverter.ToUInt32(buffer,index);index+=4;
            TimeBootMs = BitConverter.ToUInt32(buffer,index);index+=4;
            BatteryStatus = BitConverter.ToUInt16(buffer,index);index+=2;
            ArmStatus = (byte)buffer[index++];
            ChangeStatus = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(ErrorStatus).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TimeBootMs).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(BatteryStatus).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(ArmStatus).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(ChangeStatus).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/12;
        }

        /// <summary>
        /// PRS error statuses
        /// OriginName: error_status, Units: , IsExtended: false
        /// </summary>
        public uint ErrorStatus { get; set; }
        /// <summary>
        /// Time since PRS system boot
        /// OriginName: time_boot_ms, Units: , IsExtended: false
        /// </summary>
        public uint TimeBootMs { get; set; }
        /// <summary>
        /// Estimated battery run-time without a remote connection and PRS battery voltage
        /// OriginName: battery_status, Units: , IsExtended: false
        /// </summary>
        public ushort BatteryStatus { get; set; }
        /// <summary>
        /// PRS arm statuses
        /// OriginName: arm_status, Units: , IsExtended: false
        /// </summary>
        public byte ArmStatus { get; set; }
        /// <summary>
        /// PRS battery change statuses
        /// OriginName: change_status, Units: , IsExtended: false
        /// </summary>
        public byte ChangeStatus { get; set; }
    }


#endregion


}

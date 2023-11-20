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

// This code was generate by tool Asv.Mavlink.Shell version 3.2.5-alpha-11

using System;
using Asv.IO;

namespace Asv.Mavlink.V2.Avssuas
{

    public static class AvssuasHelper
    {
        public static void RegisterAvssuasDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new AvssPrsSysStatusPacket());
            src.Register(()=>new AvssDronePositionPacket());
            src.Register(()=>new AvssDroneImuPacket());
            src.Register(()=>new AvssDroneOperationModePacket());
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

    /// <summary>
    ///  AVSS_M300_OPERATION_MODE
    /// </summary>
    public enum AvssM300OperationMode:uint
    {
        /// <summary>
        /// In manual control mode
        /// MODE_M300_MANUAL_CTRL
        /// </summary>
        ModeM300ManualCtrl = 0,
        /// <summary>
        /// In attitude mode 
        /// MODE_M300_ATTITUDE
        /// </summary>
        ModeM300Attitude = 1,
        /// <summary>
        /// In GPS mode
        /// MODE_M300_P_GPS
        /// </summary>
        ModeM300PGps = 6,
        /// <summary>
        /// In hotpoint mode 
        /// MODE_M300_HOTPOINT_MODE
        /// </summary>
        ModeM300HotpointMode = 9,
        /// <summary>
        /// In assisted takeoff mode
        /// MODE_M300_ASSISTED_TAKEOFF
        /// </summary>
        ModeM300AssistedTakeoff = 10,
        /// <summary>
        /// In auto takeoff mode
        /// MODE_M300_AUTO_TAKEOFF
        /// </summary>
        ModeM300AutoTakeoff = 11,
        /// <summary>
        /// In auto landing mode
        /// MODE_M300_AUTO_LANDING
        /// </summary>
        ModeM300AutoLanding = 12,
        /// <summary>
        /// In go home mode
        /// MODE_M300_NAVI_GO_HOME
        /// </summary>
        ModeM300NaviGoHome = 15,
        /// <summary>
        /// In sdk control mode
        /// MODE_M300_NAVI_SDK_CTRL
        /// </summary>
        ModeM300NaviSdkCtrl = 17,
        /// <summary>
        /// In sport mode
        /// MODE_M300_S_SPORT
        /// </summary>
        ModeM300SSport = 31,
        /// <summary>
        /// In force auto landing mode
        /// MODE_M300_FORCE_AUTO_LANDING
        /// </summary>
        ModeM300ForceAutoLanding = 33,
        /// <summary>
        /// In tripod mode
        /// MODE_M300_T_TRIPOD
        /// </summary>
        ModeM300TTripod = 38,
        /// <summary>
        /// In search mode
        /// MODE_M300_SEARCH_MODE
        /// </summary>
        ModeM300SearchMode = 40,
        /// <summary>
        /// In engine mode
        /// MODE_M300_ENGINE_START
        /// </summary>
        ModeM300EngineStart = 41,
    }

    /// <summary>
    ///  AVSS_HORSEFLY_OPERATION_MODE
    /// </summary>
    public enum AvssHorseflyOperationMode:uint
    {
        /// <summary>
        /// In manual control mode
        /// MODE_HORSEFLY_MANUAL_CTRL
        /// </summary>
        ModeHorseflyManualCtrl = 0,
        /// <summary>
        /// In auto takeoff mode
        /// MODE_HORSEFLY_AUTO_TAKEOFF
        /// </summary>
        ModeHorseflyAutoTakeoff = 1,
        /// <summary>
        /// In auto landing mode
        /// MODE_HORSEFLY_AUTO_LANDING
        /// </summary>
        ModeHorseflyAutoLanding = 2,
        /// <summary>
        /// In go home mode
        /// MODE_HORSEFLY_NAVI_GO_HOME
        /// </summary>
        ModeHorseflyNaviGoHome = 3,
        /// <summary>
        /// In drop mode
        /// MODE_HORSEFLY_DROP
        /// </summary>
        ModeHorseflyDrop = 4,
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
        public override byte GetCrcEtra() => 220;
        public override bool WrapToV2Extension => false;

        public override AvssPrsSysStatusPayload Payload { get; } = new AvssPrsSysStatusPayload();

        public override string Name => "AVSS_PRS_SYS_STATUS";
    }

    /// <summary>
    ///  AVSS_PRS_SYS_STATUS
    /// </summary>
    public class AvssPrsSysStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 14; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 14; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //TimeBootMs
            sum+=4; //ErrorStatus
            sum+=4; //BatteryStatus
            sum+=1; //ArmStatus
            sum+=1; //ChargeStatus
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeBootMs = BinSerialize.ReadUInt(ref buffer);
            ErrorStatus = BinSerialize.ReadUInt(ref buffer);
            BatteryStatus = BinSerialize.ReadUInt(ref buffer);
            ArmStatus = (byte)BinSerialize.ReadByte(ref buffer);
            ChargeStatus = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,TimeBootMs);
            BinSerialize.WriteUInt(ref buffer,ErrorStatus);
            BinSerialize.WriteUInt(ref buffer,BatteryStatus);
            BinSerialize.WriteByte(ref buffer,(byte)ArmStatus);
            BinSerialize.WriteByte(ref buffer,(byte)ChargeStatus);
            /* PayloadByteSize = 14 */;
        }
        
        



        /// <summary>
        /// Timestamp (time since PRS boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public uint TimeBootMs { get; set; }
        /// <summary>
        /// PRS error statuses
        /// OriginName: error_status, Units: , IsExtended: false
        /// </summary>
        public uint ErrorStatus { get; set; }
        /// <summary>
        /// Estimated battery run-time without a remote connection and PRS battery voltage
        /// OriginName: battery_status, Units: , IsExtended: false
        /// </summary>
        public uint BatteryStatus { get; set; }
        /// <summary>
        /// PRS arm statuses
        /// OriginName: arm_status, Units: , IsExtended: false
        /// </summary>
        public byte ArmStatus { get; set; }
        /// <summary>
        /// PRS battery charge statuses
        /// OriginName: charge_status, Units: , IsExtended: false
        /// </summary>
        public byte ChargeStatus { get; set; }
    }
    /// <summary>
    ///  Drone position.
    ///  AVSS_DRONE_POSITION
    /// </summary>
    public class AvssDronePositionPacket: PacketV2<AvssDronePositionPayload>
    {
	    public const int PacketMessageId = 60051;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 245;
        public override bool WrapToV2Extension => false;

        public override AvssDronePositionPayload Payload { get; } = new AvssDronePositionPayload();

        public override string Name => "AVSS_DRONE_POSITION";
    }

    /// <summary>
    ///  AVSS_DRONE_POSITION
    /// </summary>
    public class AvssDronePositionPayload : IPayload
    {
        public byte GetMaxByteSize() => 24; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //TimeBootMs
            sum+=4; //Lat
            sum+=4; //Lon
            sum+=4; //Alt
            sum+=4; //GroundAlt
            sum+=4; //BarometerAlt
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeBootMs = BinSerialize.ReadUInt(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lon = BinSerialize.ReadInt(ref buffer);
            Alt = BinSerialize.ReadInt(ref buffer);
            GroundAlt = BinSerialize.ReadFloat(ref buffer);
            BarometerAlt = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,TimeBootMs);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lon);
            BinSerialize.WriteInt(ref buffer,Alt);
            BinSerialize.WriteFloat(ref buffer,GroundAlt);
            BinSerialize.WriteFloat(ref buffer,BarometerAlt);
            /* PayloadByteSize = 24 */;
        }
        
        



        /// <summary>
        /// Timestamp (time since FC boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public uint TimeBootMs { get; set; }
        /// <summary>
        /// Latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public int Lon { get; set; }
        /// <summary>
        /// Altitude (MSL). Note that virtually all GPS modules provide both WGS84 and MSL.
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public int Alt { get; set; }
        /// <summary>
        /// Altitude above ground, This altitude is measured by a ultrasound, Laser rangefinder or millimeter-wave radar
        /// OriginName: ground_alt, Units: m, IsExtended: false
        /// </summary>
        public float GroundAlt { get; set; }
        /// <summary>
        /// This altitude is measured by a barometer
        /// OriginName: barometer_alt, Units: m, IsExtended: false
        /// </summary>
        public float BarometerAlt { get; set; }
    }
    /// <summary>
    ///  Drone IMU data. Quaternion order is w, x, y, z and a zero rotation would be expressed as (1 0 0 0).
    ///  AVSS_DRONE_IMU
    /// </summary>
    public class AvssDroneImuPacket: PacketV2<AvssDroneImuPayload>
    {
	    public const int PacketMessageId = 60052;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 101;
        public override bool WrapToV2Extension => false;

        public override AvssDroneImuPayload Payload { get; } = new AvssDroneImuPayload();

        public override string Name => "AVSS_DRONE_IMU";
    }

    /// <summary>
    ///  AVSS_DRONE_IMU
    /// </summary>
    public class AvssDroneImuPayload : IPayload
    {
        public byte GetMaxByteSize() => 44; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //TimeBootMs
            sum+=4; //Q1
            sum+=4; //Q2
            sum+=4; //Q3
            sum+=4; //Q4
            sum+=4; //Xacc
            sum+=4; //Yacc
            sum+=4; //Zacc
            sum+=4; //Xgyro
            sum+=4; //Ygyro
            sum+=4; //Zgyro
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeBootMs = BinSerialize.ReadUInt(ref buffer);
            Q1 = BinSerialize.ReadFloat(ref buffer);
            Q2 = BinSerialize.ReadFloat(ref buffer);
            Q3 = BinSerialize.ReadFloat(ref buffer);
            Q4 = BinSerialize.ReadFloat(ref buffer);
            Xacc = BinSerialize.ReadFloat(ref buffer);
            Yacc = BinSerialize.ReadFloat(ref buffer);
            Zacc = BinSerialize.ReadFloat(ref buffer);
            Xgyro = BinSerialize.ReadFloat(ref buffer);
            Ygyro = BinSerialize.ReadFloat(ref buffer);
            Zgyro = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,TimeBootMs);
            BinSerialize.WriteFloat(ref buffer,Q1);
            BinSerialize.WriteFloat(ref buffer,Q2);
            BinSerialize.WriteFloat(ref buffer,Q3);
            BinSerialize.WriteFloat(ref buffer,Q4);
            BinSerialize.WriteFloat(ref buffer,Xacc);
            BinSerialize.WriteFloat(ref buffer,Yacc);
            BinSerialize.WriteFloat(ref buffer,Zacc);
            BinSerialize.WriteFloat(ref buffer,Xgyro);
            BinSerialize.WriteFloat(ref buffer,Ygyro);
            BinSerialize.WriteFloat(ref buffer,Zgyro);
            /* PayloadByteSize = 44 */;
        }
        
        



        /// <summary>
        /// Timestamp (time since FC boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public uint TimeBootMs { get; set; }
        /// <summary>
        /// Quaternion component 1, w (1 in null-rotation)
        /// OriginName: q1, Units: , IsExtended: false
        /// </summary>
        public float Q1 { get; set; }
        /// <summary>
        /// Quaternion component 2, x (0 in null-rotation)
        /// OriginName: q2, Units: , IsExtended: false
        /// </summary>
        public float Q2 { get; set; }
        /// <summary>
        /// Quaternion component 3, y (0 in null-rotation)
        /// OriginName: q3, Units: , IsExtended: false
        /// </summary>
        public float Q3 { get; set; }
        /// <summary>
        /// Quaternion component 4, z (0 in null-rotation)
        /// OriginName: q4, Units: , IsExtended: false
        /// </summary>
        public float Q4 { get; set; }
        /// <summary>
        /// X acceleration
        /// OriginName: xacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public float Xacc { get; set; }
        /// <summary>
        /// Y acceleration
        /// OriginName: yacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public float Yacc { get; set; }
        /// <summary>
        /// Z acceleration
        /// OriginName: zacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public float Zacc { get; set; }
        /// <summary>
        /// Angular speed around X axis
        /// OriginName: xgyro, Units: rad/s, IsExtended: false
        /// </summary>
        public float Xgyro { get; set; }
        /// <summary>
        /// Angular speed around Y axis
        /// OriginName: ygyro, Units: rad/s, IsExtended: false
        /// </summary>
        public float Ygyro { get; set; }
        /// <summary>
        /// Angular speed around Z axis
        /// OriginName: zgyro, Units: rad/s, IsExtended: false
        /// </summary>
        public float Zgyro { get; set; }
    }
    /// <summary>
    ///  Drone operation mode.
    ///  AVSS_DRONE_OPERATION_MODE
    /// </summary>
    public class AvssDroneOperationModePacket: PacketV2<AvssDroneOperationModePayload>
    {
	    public const int PacketMessageId = 60053;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 45;
        public override bool WrapToV2Extension => false;

        public override AvssDroneOperationModePayload Payload { get; } = new AvssDroneOperationModePayload();

        public override string Name => "AVSS_DRONE_OPERATION_MODE";
    }

    /// <summary>
    ///  AVSS_DRONE_OPERATION_MODE
    /// </summary>
    public class AvssDroneOperationModePayload : IPayload
    {
        public byte GetMaxByteSize() => 6; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //TimeBootMs
            sum+=1; //M300OperationMode
            sum+=1; //HorseflyOperationMode
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeBootMs = BinSerialize.ReadUInt(ref buffer);
            M300OperationMode = (byte)BinSerialize.ReadByte(ref buffer);
            HorseflyOperationMode = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,TimeBootMs);
            BinSerialize.WriteByte(ref buffer,(byte)M300OperationMode);
            BinSerialize.WriteByte(ref buffer,(byte)HorseflyOperationMode);
            /* PayloadByteSize = 6 */;
        }
        
        



        /// <summary>
        /// Timestamp (time since FC boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public uint TimeBootMs { get; set; }
        /// <summary>
        /// DJI M300 operation mode
        /// OriginName: M300_operation_mode, Units: , IsExtended: false
        /// </summary>
        public byte M300OperationMode { get; set; }
        /// <summary>
        /// horsefly operation mode
        /// OriginName: horsefly_operation_mode, Units: , IsExtended: false
        /// </summary>
        public byte HorseflyOperationMode { get; set; }
    }


#endregion


}

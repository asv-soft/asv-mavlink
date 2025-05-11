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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.15+a2f1de3777820636a46d83925144e965a9eb2291 25-05-11.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.IO;

namespace Asv.Mavlink.Avssuas
{

    public static class AvssuasHelper
    {
        public static void RegisterAvssuasDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(AvssPrsSysStatusPacket.MessageId, ()=>new AvssPrsSysStatusPacket());
            src.Add(AvssDronePositionPacket.MessageId, ()=>new AvssDronePositionPacket());
            src.Add(AvssDroneImuPacket.MessageId, ()=>new AvssDroneImuPacket());
            src.Add(AvssDroneOperationModePacket.MessageId, ()=>new AvssDroneOperationModePacket());
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
    public class AvssPrsSysStatusPacket : MavlinkV2Message<AvssPrsSysStatusPayload>
    {
        public const int MessageId = 60050;
        
        public const byte CrcExtra = 220;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AvssPrsSysStatusPayload Payload { get; } = new();

        public override string Name => "AVSS_PRS_SYS_STATUS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "time_boot_ms",
            "Timestamp (time since PRS boot).",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "error_status",
            "PRS error statuses",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(2,
            "battery_status",
            "Estimated battery run-time without a remote connection and PRS battery voltage",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(3,
            "arm_status",
            "PRS arm statuses",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "charge_status",
            "PRS battery charge statuses",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "AVSS_PRS_SYS_STATUS:"
        + "uint32_t time_boot_ms;"
        + "uint32_t error_status;"
        + "uint32_t battery_status;"
        + "uint8_t arm_status;"
        + "uint8_t charge_status;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TimeBootMs);
            writer.Write(StaticFields[1], Payload.ErrorStatus);
            writer.Write(StaticFields[2], Payload.BatteryStatus);
            writer.Write(StaticFields[3], Payload.ArmStatus);
            writer.Write(StaticFields[4], Payload.ChargeStatus);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TimeBootMs = reader.ReadUInt(StaticFields[0]);
            Payload.ErrorStatus = reader.ReadUInt(StaticFields[1]);
            Payload.BatteryStatus = reader.ReadUInt(StaticFields[2]);
            Payload.ArmStatus = reader.ReadByte(StaticFields[3]);
            Payload.ChargeStatus = reader.ReadByte(StaticFields[4]);
        
            
        }
    }

    /// <summary>
    ///  AVSS_PRS_SYS_STATUS
    /// </summary>
    public class AvssPrsSysStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 14; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 14; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t time_boot_ms
            +4 // uint32_t error_status
            +4 // uint32_t battery_status
            +1 // uint8_t arm_status
            +1 // uint8_t charge_status
            );
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
    public class AvssDronePositionPacket : MavlinkV2Message<AvssDronePositionPayload>
    {
        public const int MessageId = 60051;
        
        public const byte CrcExtra = 245;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AvssDronePositionPayload Payload { get; } = new();

        public override string Name => "AVSS_DRONE_POSITION";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "time_boot_ms",
            "Timestamp (time since FC boot).",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "lat",
            "Latitude, expressed",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(2,
            "lon",
            "Longitude, expressed",
            string.Empty, 
            @"degE7", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(3,
            "alt",
            "Altitude (MSL). Note that virtually all GPS modules provide both WGS84 and MSL.",
            string.Empty, 
            @"mm", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new(4,
            "ground_alt",
            "Altitude above ground, This altitude is measured by a ultrasound, Laser rangefinder or millimeter-wave radar",
            string.Empty, 
            @"m", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(5,
            "barometer_alt",
            "This altitude is measured by a barometer",
            string.Empty, 
            @"m", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
        ];
        public const string FormatMessage = "AVSS_DRONE_POSITION:"
        + "uint32_t time_boot_ms;"
        + "int32_t lat;"
        + "int32_t lon;"
        + "int32_t alt;"
        + "float ground_alt;"
        + "float barometer_alt;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TimeBootMs);
            writer.Write(StaticFields[1], Payload.Lat);
            writer.Write(StaticFields[2], Payload.Lon);
            writer.Write(StaticFields[3], Payload.Alt);
            writer.Write(StaticFields[4], Payload.GroundAlt);
            writer.Write(StaticFields[5], Payload.BarometerAlt);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TimeBootMs = reader.ReadUInt(StaticFields[0]);
            Payload.Lat = reader.ReadInt(StaticFields[1]);
            Payload.Lon = reader.ReadInt(StaticFields[2]);
            Payload.Alt = reader.ReadInt(StaticFields[3]);
            Payload.GroundAlt = reader.ReadFloat(StaticFields[4]);
            Payload.BarometerAlt = reader.ReadFloat(StaticFields[5]);
        
            
        }
    }

    /// <summary>
    ///  AVSS_DRONE_POSITION
    /// </summary>
    public class AvssDronePositionPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 24; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t time_boot_ms
            +4 // int32_t lat
            +4 // int32_t lon
            +4 // int32_t alt
            +4 // float ground_alt
            +4 // float barometer_alt
            );
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
    public class AvssDroneImuPacket : MavlinkV2Message<AvssDroneImuPayload>
    {
        public const int MessageId = 60052;
        
        public const byte CrcExtra = 101;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AvssDroneImuPayload Payload { get; } = new();

        public override string Name => "AVSS_DRONE_IMU";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "time_boot_ms",
            "Timestamp (time since FC boot).",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "q1",
            "Quaternion component 1, w (1 in null-rotation)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(2,
            "q2",
            "Quaternion component 2, x (0 in null-rotation)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(3,
            "q3",
            "Quaternion component 3, y (0 in null-rotation)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(4,
            "q4",
            "Quaternion component 4, z (0 in null-rotation)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(5,
            "xacc",
            "X acceleration",
            string.Empty, 
            @"m/s/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(6,
            "yacc",
            "Y acceleration",
            string.Empty, 
            @"m/s/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(7,
            "zacc",
            "Z acceleration",
            string.Empty, 
            @"m/s/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(8,
            "xgyro",
            "Angular speed around X axis",
            string.Empty, 
            @"rad/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(9,
            "ygyro",
            "Angular speed around Y axis",
            string.Empty, 
            @"rad/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(10,
            "zgyro",
            "Angular speed around Z axis",
            string.Empty, 
            @"rad/s", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
        ];
        public const string FormatMessage = "AVSS_DRONE_IMU:"
        + "uint32_t time_boot_ms;"
        + "float q1;"
        + "float q2;"
        + "float q3;"
        + "float q4;"
        + "float xacc;"
        + "float yacc;"
        + "float zacc;"
        + "float xgyro;"
        + "float ygyro;"
        + "float zgyro;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TimeBootMs);
            writer.Write(StaticFields[1], Payload.Q1);
            writer.Write(StaticFields[2], Payload.Q2);
            writer.Write(StaticFields[3], Payload.Q3);
            writer.Write(StaticFields[4], Payload.Q4);
            writer.Write(StaticFields[5], Payload.Xacc);
            writer.Write(StaticFields[6], Payload.Yacc);
            writer.Write(StaticFields[7], Payload.Zacc);
            writer.Write(StaticFields[8], Payload.Xgyro);
            writer.Write(StaticFields[9], Payload.Ygyro);
            writer.Write(StaticFields[10], Payload.Zgyro);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TimeBootMs = reader.ReadUInt(StaticFields[0]);
            Payload.Q1 = reader.ReadFloat(StaticFields[1]);
            Payload.Q2 = reader.ReadFloat(StaticFields[2]);
            Payload.Q3 = reader.ReadFloat(StaticFields[3]);
            Payload.Q4 = reader.ReadFloat(StaticFields[4]);
            Payload.Xacc = reader.ReadFloat(StaticFields[5]);
            Payload.Yacc = reader.ReadFloat(StaticFields[6]);
            Payload.Zacc = reader.ReadFloat(StaticFields[7]);
            Payload.Xgyro = reader.ReadFloat(StaticFields[8]);
            Payload.Ygyro = reader.ReadFloat(StaticFields[9]);
            Payload.Zgyro = reader.ReadFloat(StaticFields[10]);
        
            
        }
    }

    /// <summary>
    ///  AVSS_DRONE_IMU
    /// </summary>
    public class AvssDroneImuPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 44; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t time_boot_ms
            +4 // float q1
            +4 // float q2
            +4 // float q3
            +4 // float q4
            +4 // float xacc
            +4 // float yacc
            +4 // float zacc
            +4 // float xgyro
            +4 // float ygyro
            +4 // float zgyro
            );
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
    public class AvssDroneOperationModePacket : MavlinkV2Message<AvssDroneOperationModePayload>
    {
        public const int MessageId = 60053;
        
        public const byte CrcExtra = 45;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AvssDroneOperationModePayload Payload { get; } = new();

        public override string Name => "AVSS_DRONE_OPERATION_MODE";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "time_boot_ms",
            "Timestamp (time since FC boot).",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "M300_operation_mode",
            "DJI M300 operation mode",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(2,
            "horsefly_operation_mode",
            "horsefly operation mode",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "AVSS_DRONE_OPERATION_MODE:"
        + "uint32_t time_boot_ms;"
        + "uint8_t M300_operation_mode;"
        + "uint8_t horsefly_operation_mode;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TimeBootMs);
            writer.Write(StaticFields[1], Payload.M300OperationMode);
            writer.Write(StaticFields[2], Payload.HorseflyOperationMode);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TimeBootMs = reader.ReadUInt(StaticFields[0]);
            Payload.M300OperationMode = reader.ReadByte(StaticFields[1]);
            Payload.HorseflyOperationMode = reader.ReadByte(StaticFields[2]);
        
            
        }
    }

    /// <summary>
    ///  AVSS_DRONE_OPERATION_MODE
    /// </summary>
    public class AvssDroneOperationModePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 6; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t time_boot_ms
            +1 // uint8_t M300_operation_mode
            +1 // uint8_t horsefly_operation_mode
            );
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

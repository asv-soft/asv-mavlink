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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0+9a2f8045d50788270a91c641f703bfc105fe5697 25-05-20.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.AsvAudio;
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,TimeBootMsField, ref _timeBootMs);    
            UInt32Type.Accept(visitor,ErrorStatusField, ref _errorStatus);    
            UInt32Type.Accept(visitor,BatteryStatusField, ref _batteryStatus);    
            UInt8Type.Accept(visitor,ArmStatusField, ref _armStatus);    
            UInt8Type.Accept(visitor,ChargeStatusField, ref _chargeStatus);    

        }

        /// <summary>
        /// Timestamp (time since PRS boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field TimeBootMsField = new Field.Builder()
            .Name(nameof(TimeBootMs))
            .Title("time_boot_ms")
            .Description("Timestamp (time since PRS boot).")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _timeBootMs;
        public uint TimeBootMs { get => _timeBootMs; set => _timeBootMs = value; }
        /// <summary>
        /// PRS error statuses
        /// OriginName: error_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ErrorStatusField = new Field.Builder()
            .Name(nameof(ErrorStatus))
            .Title("error_status")
            .Description("PRS error statuses")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _errorStatus;
        public uint ErrorStatus { get => _errorStatus; set => _errorStatus = value; }
        /// <summary>
        /// Estimated battery run-time without a remote connection and PRS battery voltage
        /// OriginName: battery_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BatteryStatusField = new Field.Builder()
            .Name(nameof(BatteryStatus))
            .Title("battery_status")
            .Description("Estimated battery run-time without a remote connection and PRS battery voltage")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _batteryStatus;
        public uint BatteryStatus { get => _batteryStatus; set => _batteryStatus = value; }
        /// <summary>
        /// PRS arm statuses
        /// OriginName: arm_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ArmStatusField = new Field.Builder()
            .Name(nameof(ArmStatus))
            .Title("arm_status")
            .Description("PRS arm statuses")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _armStatus;
        public byte ArmStatus { get => _armStatus; set => _armStatus = value; }
        /// <summary>
        /// PRS battery charge statuses
        /// OriginName: charge_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ChargeStatusField = new Field.Builder()
            .Name(nameof(ChargeStatus))
            .Title("charge_status")
            .Description("PRS battery charge statuses")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _chargeStatus;
        public byte ChargeStatus { get => _chargeStatus; set => _chargeStatus = value; }
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,TimeBootMsField, ref _timeBootMs);    
            Int32Type.Accept(visitor,LatField, ref _lat);    
            Int32Type.Accept(visitor,LonField, ref _lon);    
            Int32Type.Accept(visitor,AltField, ref _alt);    
            FloatType.Accept(visitor,GroundAltField, ref _groundAlt);    
            FloatType.Accept(visitor,BarometerAltField, ref _barometerAlt);    

        }

        /// <summary>
        /// Timestamp (time since FC boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field TimeBootMsField = new Field.Builder()
            .Name(nameof(TimeBootMs))
            .Title("time_boot_ms")
            .Description("Timestamp (time since FC boot).")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _timeBootMs;
        public uint TimeBootMs { get => _timeBootMs; set => _timeBootMs = value; }
        /// <summary>
        /// Latitude, expressed
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Latitude, expressed")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _lat;
        public int Lat { get => _lat; set => _lat = value; }
        /// <summary>
        /// Longitude, expressed
        /// OriginName: lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LonField = new Field.Builder()
            .Name(nameof(Lon))
            .Title("lon")
            .Description("Longitude, expressed")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _lon;
        public int Lon { get => _lon; set => _lon = value; }
        /// <summary>
        /// Altitude (MSL). Note that virtually all GPS modules provide both WGS84 and MSL.
        /// OriginName: alt, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field AltField = new Field.Builder()
            .Name(nameof(Alt))
            .Title("alt")
            .Description("Altitude (MSL). Note that virtually all GPS modules provide both WGS84 and MSL.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(Int32Type.Default)

            .Build();
        private int _alt;
        public int Alt { get => _alt; set => _alt = value; }
        /// <summary>
        /// Altitude above ground, This altitude is measured by a ultrasound, Laser rangefinder or millimeter-wave radar
        /// OriginName: ground_alt, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field GroundAltField = new Field.Builder()
            .Name(nameof(GroundAlt))
            .Title("ground_alt")
            .Description("Altitude above ground, This altitude is measured by a ultrasound, Laser rangefinder or millimeter-wave radar")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _groundAlt;
        public float GroundAlt { get => _groundAlt; set => _groundAlt = value; }
        /// <summary>
        /// This altitude is measured by a barometer
        /// OriginName: barometer_alt, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field BarometerAltField = new Field.Builder()
            .Name(nameof(BarometerAlt))
            .Title("barometer_alt")
            .Description("This altitude is measured by a barometer")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _barometerAlt;
        public float BarometerAlt { get => _barometerAlt; set => _barometerAlt = value; }
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,TimeBootMsField, ref _timeBootMs);    
            FloatType.Accept(visitor,Q1Field, ref _q1);    
            FloatType.Accept(visitor,Q2Field, ref _q2);    
            FloatType.Accept(visitor,Q3Field, ref _q3);    
            FloatType.Accept(visitor,Q4Field, ref _q4);    
            FloatType.Accept(visitor,XaccField, ref _xacc);    
            FloatType.Accept(visitor,YaccField, ref _yacc);    
            FloatType.Accept(visitor,ZaccField, ref _zacc);    
            FloatType.Accept(visitor,XgyroField, ref _xgyro);    
            FloatType.Accept(visitor,YgyroField, ref _ygyro);    
            FloatType.Accept(visitor,ZgyroField, ref _zgyro);    

        }

        /// <summary>
        /// Timestamp (time since FC boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field TimeBootMsField = new Field.Builder()
            .Name(nameof(TimeBootMs))
            .Title("time_boot_ms")
            .Description("Timestamp (time since FC boot).")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _timeBootMs;
        public uint TimeBootMs { get => _timeBootMs; set => _timeBootMs = value; }
        /// <summary>
        /// Quaternion component 1, w (1 in null-rotation)
        /// OriginName: q1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Q1Field = new Field.Builder()
            .Name(nameof(Q1))
            .Title("q1")
            .Description("Quaternion component 1, w (1 in null-rotation)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _q1;
        public float Q1 { get => _q1; set => _q1 = value; }
        /// <summary>
        /// Quaternion component 2, x (0 in null-rotation)
        /// OriginName: q2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Q2Field = new Field.Builder()
            .Name(nameof(Q2))
            .Title("q2")
            .Description("Quaternion component 2, x (0 in null-rotation)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _q2;
        public float Q2 { get => _q2; set => _q2 = value; }
        /// <summary>
        /// Quaternion component 3, y (0 in null-rotation)
        /// OriginName: q3, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Q3Field = new Field.Builder()
            .Name(nameof(Q3))
            .Title("q3")
            .Description("Quaternion component 3, y (0 in null-rotation)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _q3;
        public float Q3 { get => _q3; set => _q3 = value; }
        /// <summary>
        /// Quaternion component 4, z (0 in null-rotation)
        /// OriginName: q4, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Q4Field = new Field.Builder()
            .Name(nameof(Q4))
            .Title("q4")
            .Description("Quaternion component 4, z (0 in null-rotation)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _q4;
        public float Q4 { get => _q4; set => _q4 = value; }
        /// <summary>
        /// X acceleration
        /// OriginName: xacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public static readonly Field XaccField = new Field.Builder()
            .Name(nameof(Xacc))
            .Title("xacc")
            .Description("X acceleration")
            .FormatString(string.Empty)
            .Units(@"m/s/s")
            .DataType(FloatType.Default)

            .Build();
        private float _xacc;
        public float Xacc { get => _xacc; set => _xacc = value; }
        /// <summary>
        /// Y acceleration
        /// OriginName: yacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public static readonly Field YaccField = new Field.Builder()
            .Name(nameof(Yacc))
            .Title("yacc")
            .Description("Y acceleration")
            .FormatString(string.Empty)
            .Units(@"m/s/s")
            .DataType(FloatType.Default)

            .Build();
        private float _yacc;
        public float Yacc { get => _yacc; set => _yacc = value; }
        /// <summary>
        /// Z acceleration
        /// OriginName: zacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public static readonly Field ZaccField = new Field.Builder()
            .Name(nameof(Zacc))
            .Title("zacc")
            .Description("Z acceleration")
            .FormatString(string.Empty)
            .Units(@"m/s/s")
            .DataType(FloatType.Default)

            .Build();
        private float _zacc;
        public float Zacc { get => _zacc; set => _zacc = value; }
        /// <summary>
        /// Angular speed around X axis
        /// OriginName: xgyro, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field XgyroField = new Field.Builder()
            .Name(nameof(Xgyro))
            .Title("xgyro")
            .Description("Angular speed around X axis")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _xgyro;
        public float Xgyro { get => _xgyro; set => _xgyro = value; }
        /// <summary>
        /// Angular speed around Y axis
        /// OriginName: ygyro, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field YgyroField = new Field.Builder()
            .Name(nameof(Ygyro))
            .Title("ygyro")
            .Description("Angular speed around Y axis")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _ygyro;
        public float Ygyro { get => _ygyro; set => _ygyro = value; }
        /// <summary>
        /// Angular speed around Z axis
        /// OriginName: zgyro, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field ZgyroField = new Field.Builder()
            .Name(nameof(Zgyro))
            .Title("zgyro")
            .Description("Angular speed around Z axis")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _zgyro;
        public float Zgyro { get => _zgyro; set => _zgyro = value; }
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,TimeBootMsField, ref _timeBootMs);    
            UInt8Type.Accept(visitor,M300OperationModeField, ref _m300OperationMode);    
            UInt8Type.Accept(visitor,HorseflyOperationModeField, ref _horseflyOperationMode);    

        }

        /// <summary>
        /// Timestamp (time since FC boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field TimeBootMsField = new Field.Builder()
            .Name(nameof(TimeBootMs))
            .Title("time_boot_ms")
            .Description("Timestamp (time since FC boot).")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _timeBootMs;
        public uint TimeBootMs { get => _timeBootMs; set => _timeBootMs = value; }
        /// <summary>
        /// DJI M300 operation mode
        /// OriginName: M300_operation_mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field M300OperationModeField = new Field.Builder()
            .Name(nameof(M300OperationMode))
            .Title("M300_operation_mode")
            .Description("DJI M300 operation mode")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _m300OperationMode;
        public byte M300OperationMode { get => _m300OperationMode; set => _m300OperationMode = value; }
        /// <summary>
        /// horsefly operation mode
        /// OriginName: horsefly_operation_mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field HorseflyOperationModeField = new Field.Builder()
            .Name(nameof(HorseflyOperationMode))
            .Title("horsefly_operation_mode")
            .Description("horsefly operation mode")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _horseflyOperationMode;
        public byte HorseflyOperationMode { get => _horseflyOperationMode; set => _horseflyOperationMode = value; }
    }




        


#endregion


}

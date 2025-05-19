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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.16+8bb2f8865168bf54d58a112cb63c6bf098479247 25-05-12.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.IO;

namespace Asv.Mavlink.Storm32
{

    public static class Storm32Helper
    {
        public static void RegisterStorm32Dialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(Storm32GimbalManagerInformationPacket.MessageId, ()=>new Storm32GimbalManagerInformationPacket());
            src.Add(Storm32GimbalManagerStatusPacket.MessageId, ()=>new Storm32GimbalManagerStatusPacket());
            src.Add(Storm32GimbalManagerControlPacket.MessageId, ()=>new Storm32GimbalManagerControlPacket());
            src.Add(Storm32GimbalManagerControlPitchyawPacket.MessageId, ()=>new Storm32GimbalManagerControlPitchyawPacket());
            src.Add(Storm32GimbalManagerCorrectRollPacket.MessageId, ()=>new Storm32GimbalManagerCorrectRollPacket());
            src.Add(QshotStatusPacket.MessageId, ()=>new QshotStatusPacket());
            src.Add(RadioRcChannelsPacket.MessageId, ()=>new RadioRcChannelsPacket());
            src.Add(RadioLinkStatsPacket.MessageId, ()=>new RadioLinkStatsPacket());
            src.Add(FrskyPassthroughArrayPacket.MessageId, ()=>new FrskyPassthroughArrayPacket());
            src.Add(ParamValueArrayPacket.MessageId, ()=>new ParamValueArrayPacket());
        }
    }

#region Enums

    /// <summary>
    ///  MAV_STORM32_TUNNEL_PAYLOAD_TYPE
    /// </summary>
    public enum MavStorm32TunnelPayloadType:uint
    {
        /// <summary>
        /// Registered for STorM32 gimbal controller. For communication with gimbal or camera.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH1_IN
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch1In = 200,
        /// <summary>
        /// Registered for STorM32 gimbal controller. For communication with gimbal or camera.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH1_OUT
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch1Out = 201,
        /// <summary>
        /// Registered for STorM32 gimbal controller. For communication with gimbal.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH2_IN
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch2In = 202,
        /// <summary>
        /// Registered for STorM32 gimbal controller. For communication with gimbal.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH2_OUT
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch2Out = 203,
        /// <summary>
        /// Registered for STorM32 gimbal controller. For communication with camera.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH3_IN
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch3In = 204,
        /// <summary>
        /// Registered for STorM32 gimbal controller. For communication with camera.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH3_OUT
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch3Out = 205,
    }

    /// <summary>
    /// STorM32 gimbal prearm check flags.
    ///  MAV_STORM32_GIMBAL_PREARM_FLAGS
    /// </summary>
    public enum MavStorm32GimbalPrearmFlags:uint
    {
        /// <summary>
        /// STorM32 gimbal is in normal state.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_IS_NORMAL
        /// </summary>
        MavStorm32GimbalPrearmFlagsIsNormal = 1,
        /// <summary>
        /// The IMUs are healthy and working normally.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_IMUS_WORKING
        /// </summary>
        MavStorm32GimbalPrearmFlagsImusWorking = 2,
        /// <summary>
        /// The motors are active and working normally.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_MOTORS_WORKING
        /// </summary>
        MavStorm32GimbalPrearmFlagsMotorsWorking = 4,
        /// <summary>
        /// The encoders are healthy and working normally.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_ENCODERS_WORKING
        /// </summary>
        MavStorm32GimbalPrearmFlagsEncodersWorking = 8,
        /// <summary>
        /// A battery voltage is applied and is in range.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_VOLTAGE_OK
        /// </summary>
        MavStorm32GimbalPrearmFlagsVoltageOk = 16,
        /// <summary>
        /// Virtual input channels are receiving data.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_VIRTUALCHANNELS_RECEIVING
        /// </summary>
        MavStorm32GimbalPrearmFlagsVirtualchannelsReceiving = 32,
        /// <summary>
        /// Mavlink messages are being received.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_MAVLINK_RECEIVING
        /// </summary>
        MavStorm32GimbalPrearmFlagsMavlinkReceiving = 64,
        /// <summary>
        /// The STorM32Link data indicates QFix.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_STORM32LINK_QFIX
        /// </summary>
        MavStorm32GimbalPrearmFlagsStorm32linkQfix = 128,
        /// <summary>
        /// The STorM32Link is working.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_STORM32LINK_WORKING
        /// </summary>
        MavStorm32GimbalPrearmFlagsStorm32linkWorking = 256,
        /// <summary>
        /// The camera has been found and is connected.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_CAMERA_CONNECTED
        /// </summary>
        MavStorm32GimbalPrearmFlagsCameraConnected = 512,
        /// <summary>
        /// The signal on the AUX0 input pin is low.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_AUX0_LOW
        /// </summary>
        MavStorm32GimbalPrearmFlagsAux0Low = 1024,
        /// <summary>
        /// The signal on the AUX1 input pin is low.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_AUX1_LOW
        /// </summary>
        MavStorm32GimbalPrearmFlagsAux1Low = 2048,
        /// <summary>
        /// The NTLogger is working normally.
        /// MAV_STORM32_GIMBAL_PREARM_FLAGS_NTLOGGER_WORKING
        /// </summary>
        MavStorm32GimbalPrearmFlagsNtloggerWorking = 4096,
    }

    /// <summary>
    /// STorM32 camera prearm check flags.
    ///  MAV_STORM32_CAMERA_PREARM_FLAGS
    /// </summary>
    public enum MavStorm32CameraPrearmFlags:uint
    {
        /// <summary>
        /// The camera has been found and is connected.
        /// MAV_STORM32_CAMERA_PREARM_FLAGS_CONNECTED
        /// </summary>
        MavStorm32CameraPrearmFlagsConnected = 1,
    }

    /// <summary>
    /// Gimbal manager capability flags.
    ///  MAV_STORM32_GIMBAL_MANAGER_CAP_FLAGS
    /// </summary>
    public enum MavStorm32GimbalManagerCapFlags:uint
    {
        /// <summary>
        /// The gimbal manager supports several profiles.
        /// MAV_STORM32_GIMBAL_MANAGER_CAP_FLAGS_HAS_PROFILES
        /// </summary>
        MavStorm32GimbalManagerCapFlagsHasProfiles = 1,
    }

    /// <summary>
    /// Flags for gimbal manager operation. Used for setting and reporting, unless specified otherwise. If a setting has been accepted by the gimbal manager is reported in the STORM32_GIMBAL_MANAGER_STATUS message.
    ///  MAV_STORM32_GIMBAL_MANAGER_FLAGS
    /// </summary>
    public enum MavStorm32GimbalManagerFlags:uint
    {
        /// <summary>
        /// 0 = ignore.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_NONE
        /// </summary>
        MavStorm32GimbalManagerFlagsNone = 0,
        /// <summary>
        /// Request to set RC input to active, or report RC input is active. Implies RC mixed. RC exclusive is achieved by setting all clients to inactive.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_RC_ACTIVE
        /// </summary>
        MavStorm32GimbalManagerFlagsRcActive = 1,
        /// <summary>
        /// Request to set onboard/companion computer client to active, or report this client is active.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_ONBOARD_ACTIVE
        /// </summary>
        MavStorm32GimbalManagerFlagsClientOnboardActive = 2,
        /// <summary>
        /// Request to set autopliot client to active, or report this client is active.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_AUTOPILOT_ACTIVE
        /// </summary>
        MavStorm32GimbalManagerFlagsClientAutopilotActive = 4,
        /// <summary>
        /// Request to set GCS client to active, or report this client is active.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_GCS_ACTIVE
        /// </summary>
        MavStorm32GimbalManagerFlagsClientGcsActive = 8,
        /// <summary>
        /// Request to set camera client to active, or report this client is active.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_CAMERA_ACTIVE
        /// </summary>
        MavStorm32GimbalManagerFlagsClientCameraActive = 16,
        /// <summary>
        /// Request to set GCS2 client to active, or report this client is active.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_GCS2_ACTIVE
        /// </summary>
        MavStorm32GimbalManagerFlagsClientGcs2Active = 32,
        /// <summary>
        /// Request to set camera2 client to active, or report this client is active.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_CAMERA2_ACTIVE
        /// </summary>
        MavStorm32GimbalManagerFlagsClientCamera2Active = 64,
        /// <summary>
        /// Request to set custom client to active, or report this client is active.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_CUSTOM_ACTIVE
        /// </summary>
        MavStorm32GimbalManagerFlagsClientCustomActive = 128,
        /// <summary>
        /// Request to set custom2 client to active, or report this client is active.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_CUSTOM2_ACTIVE
        /// </summary>
        MavStorm32GimbalManagerFlagsClientCustom2Active = 256,
        /// <summary>
        /// Request supervision. This flag is only for setting, it is not reported.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_SET_SUPERVISON
        /// </summary>
        MavStorm32GimbalManagerFlagsSetSupervison = 512,
        /// <summary>
        /// Release supervision. This flag is only for setting, it is not reported.
        /// MAV_STORM32_GIMBAL_MANAGER_FLAGS_SET_RELEASE
        /// </summary>
        MavStorm32GimbalManagerFlagsSetRelease = 1024,
    }

    /// <summary>
    /// Gimbal manager client ID. In a prioritizing profile, the priorities are determined by the implementation; they could e.g. be custom1 > onboard > GCS > autopilot/camera > GCS2 > custom2.
    ///  MAV_STORM32_GIMBAL_MANAGER_CLIENT
    /// </summary>
    public enum MavStorm32GimbalManagerClient:uint
    {
        /// <summary>
        /// For convenience.
        /// MAV_STORM32_GIMBAL_MANAGER_CLIENT_NONE
        /// </summary>
        MavStorm32GimbalManagerClientNone = 0,
        /// <summary>
        /// This is the onboard/companion computer client.
        /// MAV_STORM32_GIMBAL_MANAGER_CLIENT_ONBOARD
        /// </summary>
        MavStorm32GimbalManagerClientOnboard = 1,
        /// <summary>
        /// This is the autopilot client.
        /// MAV_STORM32_GIMBAL_MANAGER_CLIENT_AUTOPILOT
        /// </summary>
        MavStorm32GimbalManagerClientAutopilot = 2,
        /// <summary>
        /// This is the GCS client.
        /// MAV_STORM32_GIMBAL_MANAGER_CLIENT_GCS
        /// </summary>
        MavStorm32GimbalManagerClientGcs = 3,
        /// <summary>
        /// This is the camera client.
        /// MAV_STORM32_GIMBAL_MANAGER_CLIENT_CAMERA
        /// </summary>
        MavStorm32GimbalManagerClientCamera = 4,
        /// <summary>
        /// This is the GCS2 client.
        /// MAV_STORM32_GIMBAL_MANAGER_CLIENT_GCS2
        /// </summary>
        MavStorm32GimbalManagerClientGcs2 = 5,
        /// <summary>
        /// This is the camera2 client.
        /// MAV_STORM32_GIMBAL_MANAGER_CLIENT_CAMERA2
        /// </summary>
        MavStorm32GimbalManagerClientCamera2 = 6,
        /// <summary>
        /// This is the custom client.
        /// MAV_STORM32_GIMBAL_MANAGER_CLIENT_CUSTOM
        /// </summary>
        MavStorm32GimbalManagerClientCustom = 7,
        /// <summary>
        /// This is the custom2 client.
        /// MAV_STORM32_GIMBAL_MANAGER_CLIENT_CUSTOM2
        /// </summary>
        MavStorm32GimbalManagerClientCustom2 = 8,
    }

    /// <summary>
    /// Gimbal manager profiles. Only standard profiles are defined. Any implementation can define its own profile(s) in addition, and should use enum values > 16.
    ///  MAV_STORM32_GIMBAL_MANAGER_PROFILE
    /// </summary>
    public enum MavStorm32GimbalManagerProfile:uint
    {
        /// <summary>
        /// Default profile. Implementation specific.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_DEFAULT
        /// </summary>
        MavStorm32GimbalManagerProfileDefault = 0,
        /// <summary>
        /// Not supported/deprecated.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_CUSTOM
        /// </summary>
        MavStorm32GimbalManagerProfileCustom = 1,
        /// <summary>
        /// Profile with cooperative behavior.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_COOPERATIVE
        /// </summary>
        MavStorm32GimbalManagerProfileCooperative = 2,
        /// <summary>
        /// Profile with exclusive behavior.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_EXCLUSIVE
        /// </summary>
        MavStorm32GimbalManagerProfileExclusive = 3,
        /// <summary>
        /// Profile with priority and cooperative behavior for equal priority.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_PRIORITY_COOPERATIVE
        /// </summary>
        MavStorm32GimbalManagerProfilePriorityCooperative = 4,
        /// <summary>
        /// Profile with priority and exclusive behavior for equal priority.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_PRIORITY_EXCLUSIVE
        /// </summary>
        MavStorm32GimbalManagerProfilePriorityExclusive = 5,
    }

    /// <summary>
    /// Enumeration of possible shot modes.
    ///  MAV_QSHOT_MODE
    /// </summary>
    public enum MavQshotMode:uint
    {
        /// <summary>
        /// Undefined shot mode. Can be used to determine if qshots should be used or not.
        /// MAV_QSHOT_MODE_UNDEFINED
        /// </summary>
        MavQshotModeUndefined = 0,
        /// <summary>
        /// Start normal gimbal operation. Is usually used to return back from a shot.
        /// MAV_QSHOT_MODE_DEFAULT
        /// </summary>
        MavQshotModeDefault = 1,
        /// <summary>
        /// Load and keep safe gimbal position and stop stabilization.
        /// MAV_QSHOT_MODE_GIMBAL_RETRACT
        /// </summary>
        MavQshotModeGimbalRetract = 2,
        /// <summary>
        /// Load neutral gimbal position and keep it while stabilizing.
        /// MAV_QSHOT_MODE_GIMBAL_NEUTRAL
        /// </summary>
        MavQshotModeGimbalNeutral = 3,
        /// <summary>
        /// Start mission with gimbal control.
        /// MAV_QSHOT_MODE_GIMBAL_MISSION
        /// </summary>
        MavQshotModeGimbalMission = 4,
        /// <summary>
        /// Start RC gimbal control.
        /// MAV_QSHOT_MODE_GIMBAL_RC_CONTROL
        /// </summary>
        MavQshotModeGimbalRcControl = 5,
        /// <summary>
        /// Start gimbal tracking the point specified by Lat, Lon, Alt.
        /// MAV_QSHOT_MODE_POI_TARGETING
        /// </summary>
        MavQshotModePoiTargeting = 6,
        /// <summary>
        /// Start gimbal tracking the system with specified system ID.
        /// MAV_QSHOT_MODE_SYSID_TARGETING
        /// </summary>
        MavQshotModeSysidTargeting = 7,
        /// <summary>
        /// Start 2-point cable cam quick shot.
        /// MAV_QSHOT_MODE_CABLECAM_2POINT
        /// </summary>
        MavQshotModeCablecam2point = 8,
        /// <summary>
        /// Start gimbal tracking the home location.
        /// MAV_QSHOT_MODE_HOME_TARGETING
        /// </summary>
        MavQshotModeHomeTargeting = 9,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Command to a gimbal manager to control the gimbal tilt and pan angles. It is possible to set combinations of the values below. E.g. an angle as well as a desired angular rate can be used to get to this angle at a certain angular rate, or an angular rate only will result in continuous turning. NaN is to be used to signal unset. A gimbal device is never to react to this command.
        /// Param 1 - Pitch/tilt angle (positive: tilt up). NaN to be ignored.
        /// Param 2 - Yaw/pan angle (positive: pan to the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.
        /// Param 3 - Pitch/tilt rate (positive: tilt up). NaN to be ignored.
        /// Param 4 - Yaw/pan rate (positive: pan to the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.
        /// Param 5 - Gimbal device flags to be applied.
        /// Param 6 - Gimbal manager flags to be applied.
        /// Param 7 - Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals. The client is copied into bits 8-15.
        /// MAV_CMD_STORM32_DO_GIMBAL_MANAGER_CONTROL_PITCHYAW
        /// </summary>
        MavCmdStorm32DoGimbalManagerControlPitchyaw = 60002,
        /// <summary>
        /// Command to configure a gimbal manager. A gimbal device is never to react to this command. The selected profile is reported in the STORM32_GIMBAL_MANAGER_STATUS message.
        /// Param 1 - Gimbal manager profile (0 = default).
        /// Param 7 - Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.
        /// MAV_CMD_STORM32_DO_GIMBAL_MANAGER_SETUP
        /// </summary>
        MavCmdStorm32DoGimbalManagerSetup = 60010,
        /// <summary>
        /// Command to set the shot manager mode.
        /// Param 1 - Set shot mode.
        /// Param 2 - Set shot state or command. The allowed values are specific to the selected shot mode.
        /// MAV_CMD_QSHOT_DO_CONFIGURE
        /// </summary>
        MavCmdQshotDoConfigure = 60020,
    }

    /// <summary>
    /// RADIO_RC_CHANNELS flags (bitmask).
    ///  RADIO_RC_CHANNELS_FLAGS
    /// </summary>
    public enum RadioRcChannelsFlags:uint
    {
        /// <summary>
        /// Failsafe is active.
        /// RADIO_RC_CHANNELS_FLAGS_FAILSAFE
        /// </summary>
        RadioRcChannelsFlagsFailsafe = 1,
        /// <summary>
        /// Indicates that the current frame has not been received. Channel values are frozen.
        /// RADIO_RC_CHANNELS_FLAGS_FRAME_MISSED
        /// </summary>
        RadioRcChannelsFlagsFrameMissed = 2,
    }

    /// <summary>
    /// RADIO_LINK_STATS flags (bitmask).
    ///  RADIO_LINK_STATS_FLAGS
    /// </summary>
    public enum RadioLinkStatsFlags:uint
    {
        /// <summary>
        /// Rssi are in negative dBm. Values 0..254 corresponds to 0..-254 dBm.
        /// RADIO_LINK_STATS_FLAGS_RSSI_DBM
        /// </summary>
        RadioLinkStatsFlagsRssiDbm = 1,
    }


#endregion

#region Messages

    /// <summary>
    /// Information about a gimbal manager. This message should be requested by a ground station using MAV_CMD_REQUEST_MESSAGE. It mirrors some fields of the GIMBAL_DEVICE_INFORMATION message, but not all. If the additional information is desired, also GIMBAL_DEVICE_INFORMATION should be requested.
    ///  STORM32_GIMBAL_MANAGER_INFORMATION
    /// </summary>
    public class Storm32GimbalManagerInformationPacket : MavlinkV2Message<Storm32GimbalManagerInformationPayload>
    {
        public const int MessageId = 60010;
        
        public const byte CrcExtra = 208;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Storm32GimbalManagerInformationPayload Payload { get; } = new();

        public override string Name => "STORM32_GIMBAL_MANAGER_INFORMATION";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "device_cap_flags",
            "Gimbal device capability flags. Same flags as reported by GIMBAL_DEVICE_INFORMATION. The flag is only 16 bit wide, but stored in 32 bit, for backwards compatibility (high word is zero).",
            string.Empty, 
            string.Empty, 
            "bitmask", 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "manager_cap_flags",
            "Gimbal manager capability flags.",
            string.Empty, 
            string.Empty, 
            "bitmask", 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(2,
            "roll_min",
            "Hardware minimum roll angle (positive: roll to the right). NaN if unknown.",
            string.Empty, 
            @"rad", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(3,
            "roll_max",
            "Hardware maximum roll angle (positive: roll to the right). NaN if unknown.",
            string.Empty, 
            @"rad", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(4,
            "pitch_min",
            "Hardware minimum pitch/tilt angle (positive: tilt up). NaN if unknown.",
            string.Empty, 
            @"rad", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(5,
            "pitch_max",
            "Hardware maximum pitch/tilt angle (positive: tilt up). NaN if unknown.",
            string.Empty, 
            @"rad", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(6,
            "yaw_min",
            "Hardware minimum yaw/pan angle (positive: pan to the right, relative to the vehicle/gimbal base). NaN if unknown.",
            string.Empty, 
            @"rad", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(7,
            "yaw_max",
            "Hardware maximum yaw/pan angle (positive: pan to the right, relative to the vehicle/gimbal base). NaN if unknown.",
            string.Empty, 
            @"rad", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(8,
            "gimbal_id",
            "Gimbal ID (component ID or 1-6 for non-MAVLink gimbal) that this gimbal manager is responsible for.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "STORM32_GIMBAL_MANAGER_INFORMATION:"
        + "uint32_t device_cap_flags;"
        + "uint32_t manager_cap_flags;"
        + "float roll_min;"
        + "float roll_max;"
        + "float pitch_min;"
        + "float pitch_max;"
        + "float yaw_min;"
        + "float yaw_max;"
        + "uint8_t gimbal_id;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], (ulong)Payload.DeviceCapFlags);
            writer.Write(StaticFields[1], (ulong)Payload.ManagerCapFlags);
            writer.Write(StaticFields[2], Payload.RollMin);
            writer.Write(StaticFields[3], Payload.RollMax);
            writer.Write(StaticFields[4], Payload.PitchMin);
            writer.Write(StaticFields[5], Payload.PitchMax);
            writer.Write(StaticFields[6], Payload.YawMin);
            writer.Write(StaticFields[7], Payload.YawMax);
            writer.Write(StaticFields[8], Payload.GimbalId);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.DeviceCapFlags = (GimbalDeviceCapFlags)reader.ReadUInt(StaticFields[0]);
            Payload.ManagerCapFlags = (MavStorm32GimbalManagerCapFlags)reader.ReadUInt(StaticFields[1]);
            Payload.RollMin = reader.ReadFloat(StaticFields[2]);
            Payload.RollMax = reader.ReadFloat(StaticFields[3]);
            Payload.PitchMin = reader.ReadFloat(StaticFields[4]);
            Payload.PitchMax = reader.ReadFloat(StaticFields[5]);
            Payload.YawMin = reader.ReadFloat(StaticFields[6]);
            Payload.YawMax = reader.ReadFloat(StaticFields[7]);
            Payload.GimbalId = reader.ReadByte(StaticFields[8]);
        
            
        }
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_INFORMATION
    /// </summary>
    public class Storm32GimbalManagerInformationPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 33; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 33; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 4 // uint32_t device_cap_flags
            + 4 // uint32_t manager_cap_flags
            +4 // float roll_min
            +4 // float roll_max
            +4 // float pitch_min
            +4 // float pitch_max
            +4 // float yaw_min
            +4 // float yaw_max
            +1 // uint8_t gimbal_id
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            DeviceCapFlags = (GimbalDeviceCapFlags)BinSerialize.ReadUInt(ref buffer);
            ManagerCapFlags = (MavStorm32GimbalManagerCapFlags)BinSerialize.ReadUInt(ref buffer);
            RollMin = BinSerialize.ReadFloat(ref buffer);
            RollMax = BinSerialize.ReadFloat(ref buffer);
            PitchMin = BinSerialize.ReadFloat(ref buffer);
            PitchMax = BinSerialize.ReadFloat(ref buffer);
            YawMin = BinSerialize.ReadFloat(ref buffer);
            YawMax = BinSerialize.ReadFloat(ref buffer);
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,(uint)DeviceCapFlags);
            BinSerialize.WriteUInt(ref buffer,(uint)ManagerCapFlags);
            BinSerialize.WriteFloat(ref buffer,RollMin);
            BinSerialize.WriteFloat(ref buffer,RollMax);
            BinSerialize.WriteFloat(ref buffer,PitchMin);
            BinSerialize.WriteFloat(ref buffer,PitchMax);
            BinSerialize.WriteFloat(ref buffer,YawMin);
            BinSerialize.WriteFloat(ref buffer,YawMax);
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);
            /* PayloadByteSize = 33 */;
        }
        
        



        /// <summary>
        /// Gimbal device capability flags. Same flags as reported by GIMBAL_DEVICE_INFORMATION. The flag is only 16 bit wide, but stored in 32 bit, for backwards compatibility (high word is zero).
        /// OriginName: device_cap_flags, Units: , IsExtended: false
        /// </summary>
        public GimbalDeviceCapFlags DeviceCapFlags { get; set; }
        /// <summary>
        /// Gimbal manager capability flags.
        /// OriginName: manager_cap_flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerCapFlags ManagerCapFlags { get; set; }
        /// <summary>
        /// Hardware minimum roll angle (positive: roll to the right). NaN if unknown.
        /// OriginName: roll_min, Units: rad, IsExtended: false
        /// </summary>
        public float RollMin { get; set; }
        /// <summary>
        /// Hardware maximum roll angle (positive: roll to the right). NaN if unknown.
        /// OriginName: roll_max, Units: rad, IsExtended: false
        /// </summary>
        public float RollMax { get; set; }
        /// <summary>
        /// Hardware minimum pitch/tilt angle (positive: tilt up). NaN if unknown.
        /// OriginName: pitch_min, Units: rad, IsExtended: false
        /// </summary>
        public float PitchMin { get; set; }
        /// <summary>
        /// Hardware maximum pitch/tilt angle (positive: tilt up). NaN if unknown.
        /// OriginName: pitch_max, Units: rad, IsExtended: false
        /// </summary>
        public float PitchMax { get; set; }
        /// <summary>
        /// Hardware minimum yaw/pan angle (positive: pan to the right, relative to the vehicle/gimbal base). NaN if unknown.
        /// OriginName: yaw_min, Units: rad, IsExtended: false
        /// </summary>
        public float YawMin { get; set; }
        /// <summary>
        /// Hardware maximum yaw/pan angle (positive: pan to the right, relative to the vehicle/gimbal base). NaN if unknown.
        /// OriginName: yaw_max, Units: rad, IsExtended: false
        /// </summary>
        public float YawMax { get; set; }
        /// <summary>
        /// Gimbal ID (component ID or 1-6 for non-MAVLink gimbal) that this gimbal manager is responsible for.
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public byte GimbalId { get; set; }
    }
    /// <summary>
    /// Message reporting the current status of a gimbal manager. This message should be broadcast at a low regular rate (e.g. 1 Hz, may be increase momentarily to e.g. 5 Hz for a period of 1 sec after a change).
    ///  STORM32_GIMBAL_MANAGER_STATUS
    /// </summary>
    public class Storm32GimbalManagerStatusPacket : MavlinkV2Message<Storm32GimbalManagerStatusPayload>
    {
        public const int MessageId = 60011;
        
        public const byte CrcExtra = 183;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Storm32GimbalManagerStatusPayload Payload { get; } = new();

        public override string Name => "STORM32_GIMBAL_MANAGER_STATUS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "device_flags",
            "Gimbal device flags currently applied. Same flags as reported by GIMBAL_DEVICE_ATTITUDE_STATUS.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "manager_flags",
            "Gimbal manager flags currently applied.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "gimbal_id",
            "Gimbal ID (component ID or 1-6 for non-MAVLink gimbal) that this gimbal manager is responsible for.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(3,
            "supervisor",
            "Client who is currently supervisor (0 = none).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "profile",
            "Profile currently applied (0 = default).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "STORM32_GIMBAL_MANAGER_STATUS:"
        + "uint16_t device_flags;"
        + "uint16_t manager_flags;"
        + "uint8_t gimbal_id;"
        + "uint8_t supervisor;"
        + "uint8_t profile;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], (ulong)Payload.DeviceFlags);
            writer.Write(StaticFields[1], (ulong)Payload.ManagerFlags);
            writer.Write(StaticFields[2], Payload.GimbalId);
            writer.Write(StaticFields[3], (ulong)Payload.Supervisor);
            writer.Write(StaticFields[4], (ulong)Payload.Profile);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.DeviceFlags = (GimbalDeviceFlags)reader.ReadUShort(StaticFields[0]);
            Payload.ManagerFlags = (MavStorm32GimbalManagerFlags)reader.ReadUShort(StaticFields[1]);
            Payload.GimbalId = reader.ReadByte(StaticFields[2]);
            Payload.Supervisor = (MavStorm32GimbalManagerClient)reader.ReadByte(StaticFields[3]);
            Payload.Profile = (MavStorm32GimbalManagerProfile)reader.ReadByte(StaticFields[4]);
        
            
        }
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_STATUS
    /// </summary>
    public class Storm32GimbalManagerStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 7; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 2 // uint16_t device_flags
            + 2 // uint16_t manager_flags
            +1 // uint8_t gimbal_id
            + 1 // uint8_t supervisor
            + 1 // uint8_t profile
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            DeviceFlags = (GimbalDeviceFlags)BinSerialize.ReadUShort(ref buffer);
            ManagerFlags = (MavStorm32GimbalManagerFlags)BinSerialize.ReadUShort(ref buffer);
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);
            Supervisor = (MavStorm32GimbalManagerClient)BinSerialize.ReadByte(ref buffer);
            Profile = (MavStorm32GimbalManagerProfile)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,(ushort)DeviceFlags);
            BinSerialize.WriteUShort(ref buffer,(ushort)ManagerFlags);
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);
            BinSerialize.WriteByte(ref buffer,(byte)Supervisor);
            BinSerialize.WriteByte(ref buffer,(byte)Profile);
            /* PayloadByteSize = 7 */;
        }
        
        



        /// <summary>
        /// Gimbal device flags currently applied. Same flags as reported by GIMBAL_DEVICE_ATTITUDE_STATUS.
        /// OriginName: device_flags, Units: , IsExtended: false
        /// </summary>
        public GimbalDeviceFlags DeviceFlags { get; set; }
        /// <summary>
        /// Gimbal manager flags currently applied.
        /// OriginName: manager_flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerFlags ManagerFlags { get; set; }
        /// <summary>
        /// Gimbal ID (component ID or 1-6 for non-MAVLink gimbal) that this gimbal manager is responsible for.
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public byte GimbalId { get; set; }
        /// <summary>
        /// Client who is currently supervisor (0 = none).
        /// OriginName: supervisor, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerClient Supervisor { get; set; }
        /// <summary>
        /// Profile currently applied (0 = default).
        /// OriginName: profile, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerProfile Profile { get; set; }
    }
    /// <summary>
    /// Message to a gimbal manager to control the gimbal attitude. Angles and rates can be set to NaN according to use case. A gimbal device is never to react to this message.
    ///  STORM32_GIMBAL_MANAGER_CONTROL
    /// </summary>
    public class Storm32GimbalManagerControlPacket : MavlinkV2Message<Storm32GimbalManagerControlPayload>
    {
        public const int MessageId = 60012;
        
        public const byte CrcExtra = 99;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Storm32GimbalManagerControlPayload Payload { get; } = new();

        public override string Name => "STORM32_GIMBAL_MANAGER_CONTROL";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "q",
            "Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation). Set first element to NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"[NaN:]", 
            MessageFieldType.Float32, 
            4, 
            false),
            new(1,
            "angular_velocity_x",
            "X component of angular velocity (positive: roll to the right). NaN to be ignored.",
            string.Empty, 
            @"rad/s", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(2,
            "angular_velocity_y",
            "Y component of angular velocity (positive: tilt up). NaN to be ignored.",
            string.Empty, 
            @"rad/s", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(3,
            "angular_velocity_z",
            "Z component of angular velocity (positive: pan to the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.",
            string.Empty, 
            @"rad/s", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(4,
            "device_flags",
            "Gimbal device flags to be applied (UINT16_MAX to be ignored). Same flags as used in GIMBAL_DEVICE_SET_ATTITUDE.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"UINT16_MAX", 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(5,
            "manager_flags",
            "Gimbal manager flags to be applied (0 to be ignored).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"0", 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(6,
            "target_system",
            "System ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(7,
            "target_component",
            "Component ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(8,
            "gimbal_id",
            "Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(9,
            "client",
            "Client which is contacting the gimbal manager (must be set).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "STORM32_GIMBAL_MANAGER_CONTROL:"
        + "float[4] q;"
        + "float angular_velocity_x;"
        + "float angular_velocity_y;"
        + "float angular_velocity_z;"
        + "uint16_t device_flags;"
        + "uint16_t manager_flags;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t gimbal_id;"
        + "uint8_t client;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Q);
            writer.Write(StaticFields[1], Payload.AngularVelocityX);
            writer.Write(StaticFields[2], Payload.AngularVelocityY);
            writer.Write(StaticFields[3], Payload.AngularVelocityZ);
            writer.Write(StaticFields[4], (ulong)Payload.DeviceFlags);
            writer.Write(StaticFields[5], (ulong)Payload.ManagerFlags);
            writer.Write(StaticFields[6], Payload.TargetSystem);
            writer.Write(StaticFields[7], Payload.TargetComponent);
            writer.Write(StaticFields[8], Payload.GimbalId);
            writer.Write(StaticFields[9], (ulong)Payload.Client);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            reader.ReadFloatArray(StaticFields[0], Payload.Q);
            Payload.AngularVelocityX = reader.ReadFloat(StaticFields[1]);
            Payload.AngularVelocityY = reader.ReadFloat(StaticFields[2]);
            Payload.AngularVelocityZ = reader.ReadFloat(StaticFields[3]);
            Payload.DeviceFlags = (GimbalDeviceFlags)reader.ReadUShort(StaticFields[4]);
            Payload.ManagerFlags = (MavStorm32GimbalManagerFlags)reader.ReadUShort(StaticFields[5]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[6]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[7]);
            Payload.GimbalId = reader.ReadByte(StaticFields[8]);
            Payload.Client = (MavStorm32GimbalManagerClient)reader.ReadByte(StaticFields[9]);
        
            
        }
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_CONTROL
    /// </summary>
    public class Storm32GimbalManagerControlPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 36; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 36; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +Q.Length * 4 // float[4] q
            +4 // float angular_velocity_x
            +4 // float angular_velocity_y
            +4 // float angular_velocity_z
            + 2 // uint16_t device_flags
            + 2 // uint16_t manager_flags
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t gimbal_id
            + 1 // uint8_t client
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/36 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            Q = new float[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Q[i] = BinSerialize.ReadFloat(ref buffer);
            }
            AngularVelocityX = BinSerialize.ReadFloat(ref buffer);
            AngularVelocityY = BinSerialize.ReadFloat(ref buffer);
            AngularVelocityZ = BinSerialize.ReadFloat(ref buffer);
            DeviceFlags = (GimbalDeviceFlags)BinSerialize.ReadUShort(ref buffer);
            ManagerFlags = (MavStorm32GimbalManagerFlags)BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);
            Client = (MavStorm32GimbalManagerClient)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<Q.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,Q[i]);
            }
            BinSerialize.WriteFloat(ref buffer,AngularVelocityX);
            BinSerialize.WriteFloat(ref buffer,AngularVelocityY);
            BinSerialize.WriteFloat(ref buffer,AngularVelocityZ);
            BinSerialize.WriteUShort(ref buffer,(ushort)DeviceFlags);
            BinSerialize.WriteUShort(ref buffer,(ushort)ManagerFlags);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);
            BinSerialize.WriteByte(ref buffer,(byte)Client);
            /* PayloadByteSize = 36 */;
        }
        
        



        /// <summary>
        /// Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation). Set first element to NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.
        /// OriginName: q, Units: , IsExtended: false
        /// </summary>
        public const int QMaxItemsCount = 4;
        public float[] Q { get; set; } = new float[4];
        [Obsolete("This method is deprecated. Use GetQMaxItemsCount instead.")]
        public byte GetQMaxItemsCount() => 4;
        /// <summary>
        /// X component of angular velocity (positive: roll to the right). NaN to be ignored.
        /// OriginName: angular_velocity_x, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityX { get; set; }
        /// <summary>
        /// Y component of angular velocity (positive: tilt up). NaN to be ignored.
        /// OriginName: angular_velocity_y, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityY { get; set; }
        /// <summary>
        /// Z component of angular velocity (positive: pan to the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.
        /// OriginName: angular_velocity_z, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityZ { get; set; }
        /// <summary>
        /// Gimbal device flags to be applied (UINT16_MAX to be ignored). Same flags as used in GIMBAL_DEVICE_SET_ATTITUDE.
        /// OriginName: device_flags, Units: , IsExtended: false
        /// </summary>
        public GimbalDeviceFlags DeviceFlags { get; set; }
        /// <summary>
        /// Gimbal manager flags to be applied (0 to be ignored).
        /// OriginName: manager_flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerFlags ManagerFlags { get; set; }
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
        /// Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public byte GimbalId { get; set; }
        /// <summary>
        /// Client which is contacting the gimbal manager (must be set).
        /// OriginName: client, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerClient Client { get; set; }
    }
    /// <summary>
    /// Message to a gimbal manager to control the gimbal tilt and pan angles. Angles and rates can be set to NaN according to use case. A gimbal device is never to react to this message.
    ///  STORM32_GIMBAL_MANAGER_CONTROL_PITCHYAW
    /// </summary>
    public class Storm32GimbalManagerControlPitchyawPacket : MavlinkV2Message<Storm32GimbalManagerControlPitchyawPayload>
    {
        public const int MessageId = 60013;
        
        public const byte CrcExtra = 129;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Storm32GimbalManagerControlPitchyawPayload Payload { get; } = new();

        public override string Name => "STORM32_GIMBAL_MANAGER_CONTROL_PITCHYAW";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "pitch",
            "Pitch/tilt angle (positive: tilt up). NaN to be ignored.",
            string.Empty, 
            @"rad", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(1,
            "yaw",
            "Yaw/pan angle (positive: pan the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.",
            string.Empty, 
            @"rad", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(2,
            "pitch_rate",
            "Pitch/tilt angular rate (positive: tilt up). NaN to be ignored.",
            string.Empty, 
            @"rad/s", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(3,
            "yaw_rate",
            "Yaw/pan angular rate (positive: pan to the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.",
            string.Empty, 
            @"rad/s", 
            string.Empty, 
            @"NaN", 
            MessageFieldType.Float32, 
            0, 
            false),
            new(4,
            "device_flags",
            "Gimbal device flags to be applied (UINT16_MAX to be ignored). Same flags as used in GIMBAL_DEVICE_SET_ATTITUDE.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"UINT16_MAX", 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(5,
            "manager_flags",
            "Gimbal manager flags to be applied (0 to be ignored).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"0", 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(6,
            "target_system",
            "System ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(7,
            "target_component",
            "Component ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(8,
            "gimbal_id",
            "Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(9,
            "client",
            "Client which is contacting the gimbal manager (must be set).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "STORM32_GIMBAL_MANAGER_CONTROL_PITCHYAW:"
        + "float pitch;"
        + "float yaw;"
        + "float pitch_rate;"
        + "float yaw_rate;"
        + "uint16_t device_flags;"
        + "uint16_t manager_flags;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t gimbal_id;"
        + "uint8_t client;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Pitch);
            writer.Write(StaticFields[1], Payload.Yaw);
            writer.Write(StaticFields[2], Payload.PitchRate);
            writer.Write(StaticFields[3], Payload.YawRate);
            writer.Write(StaticFields[4], (ulong)Payload.DeviceFlags);
            writer.Write(StaticFields[5], (ulong)Payload.ManagerFlags);
            writer.Write(StaticFields[6], Payload.TargetSystem);
            writer.Write(StaticFields[7], Payload.TargetComponent);
            writer.Write(StaticFields[8], Payload.GimbalId);
            writer.Write(StaticFields[9], (ulong)Payload.Client);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Pitch = reader.ReadFloat(StaticFields[0]);
            Payload.Yaw = reader.ReadFloat(StaticFields[1]);
            Payload.PitchRate = reader.ReadFloat(StaticFields[2]);
            Payload.YawRate = reader.ReadFloat(StaticFields[3]);
            Payload.DeviceFlags = (GimbalDeviceFlags)reader.ReadUShort(StaticFields[4]);
            Payload.ManagerFlags = (MavStorm32GimbalManagerFlags)reader.ReadUShort(StaticFields[5]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[6]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[7]);
            Payload.GimbalId = reader.ReadByte(StaticFields[8]);
            Payload.Client = (MavStorm32GimbalManagerClient)reader.ReadByte(StaticFields[9]);
        
            
        }
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_CONTROL_PITCHYAW
    /// </summary>
    public class Storm32GimbalManagerControlPitchyawPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 24; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float pitch
            +4 // float yaw
            +4 // float pitch_rate
            +4 // float yaw_rate
            + 2 // uint16_t device_flags
            + 2 // uint16_t manager_flags
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t gimbal_id
            + 1 // uint8_t client
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            PitchRate = BinSerialize.ReadFloat(ref buffer);
            YawRate = BinSerialize.ReadFloat(ref buffer);
            DeviceFlags = (GimbalDeviceFlags)BinSerialize.ReadUShort(ref buffer);
            ManagerFlags = (MavStorm32GimbalManagerFlags)BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);
            Client = (MavStorm32GimbalManagerClient)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteFloat(ref buffer,PitchRate);
            BinSerialize.WriteFloat(ref buffer,YawRate);
            BinSerialize.WriteUShort(ref buffer,(ushort)DeviceFlags);
            BinSerialize.WriteUShort(ref buffer,(ushort)ManagerFlags);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);
            BinSerialize.WriteByte(ref buffer,(byte)Client);
            /* PayloadByteSize = 24 */;
        }
        
        



        /// <summary>
        /// Pitch/tilt angle (positive: tilt up). NaN to be ignored.
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw/pan angle (positive: pan the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Pitch/tilt angular rate (positive: tilt up). NaN to be ignored.
        /// OriginName: pitch_rate, Units: rad/s, IsExtended: false
        /// </summary>
        public float PitchRate { get; set; }
        /// <summary>
        /// Yaw/pan angular rate (positive: pan to the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.
        /// OriginName: yaw_rate, Units: rad/s, IsExtended: false
        /// </summary>
        public float YawRate { get; set; }
        /// <summary>
        /// Gimbal device flags to be applied (UINT16_MAX to be ignored). Same flags as used in GIMBAL_DEVICE_SET_ATTITUDE.
        /// OriginName: device_flags, Units: , IsExtended: false
        /// </summary>
        public GimbalDeviceFlags DeviceFlags { get; set; }
        /// <summary>
        /// Gimbal manager flags to be applied (0 to be ignored).
        /// OriginName: manager_flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerFlags ManagerFlags { get; set; }
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
        /// Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public byte GimbalId { get; set; }
        /// <summary>
        /// Client which is contacting the gimbal manager (must be set).
        /// OriginName: client, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerClient Client { get; set; }
    }
    /// <summary>
    /// Message to a gimbal manager to correct the gimbal roll angle. This message is typically used to manually correct for a tilted horizon in operation. A gimbal device is never to react to this message.
    ///  STORM32_GIMBAL_MANAGER_CORRECT_ROLL
    /// </summary>
    public class Storm32GimbalManagerCorrectRollPacket : MavlinkV2Message<Storm32GimbalManagerCorrectRollPayload>
    {
        public const int MessageId = 60014;
        
        public const byte CrcExtra = 134;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Storm32GimbalManagerCorrectRollPayload Payload { get; } = new();

        public override string Name => "STORM32_GIMBAL_MANAGER_CORRECT_ROLL";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "roll",
            "Roll angle (positive to roll to the right).",
            string.Empty, 
            @"rad", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(1,
            "target_system",
            "System ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(2,
            "target_component",
            "Component ID",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(3,
            "gimbal_id",
            "Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "client",
            "Client which is contacting the gimbal manager (must be set).",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "STORM32_GIMBAL_MANAGER_CORRECT_ROLL:"
        + "float roll;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t gimbal_id;"
        + "uint8_t client;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Roll);
            writer.Write(StaticFields[1], Payload.TargetSystem);
            writer.Write(StaticFields[2], Payload.TargetComponent);
            writer.Write(StaticFields[3], Payload.GimbalId);
            writer.Write(StaticFields[4], (ulong)Payload.Client);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Roll = reader.ReadFloat(StaticFields[0]);
            Payload.TargetSystem = reader.ReadByte(StaticFields[1]);
            Payload.TargetComponent = reader.ReadByte(StaticFields[2]);
            Payload.GimbalId = reader.ReadByte(StaticFields[3]);
            Payload.Client = (MavStorm32GimbalManagerClient)reader.ReadByte(StaticFields[4]);
        
            
        }
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_CORRECT_ROLL
    /// </summary>
    public class Storm32GimbalManagerCorrectRollPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float roll
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t gimbal_id
            + 1 // uint8_t client
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Roll = BinSerialize.ReadFloat(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);
            Client = (MavStorm32GimbalManagerClient)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);
            BinSerialize.WriteByte(ref buffer,(byte)Client);
            /* PayloadByteSize = 8 */;
        }
        
        



        /// <summary>
        /// Roll angle (positive to roll to the right).
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
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
        /// Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public byte GimbalId { get; set; }
        /// <summary>
        /// Client which is contacting the gimbal manager (must be set).
        /// OriginName: client, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerClient Client { get; set; }
    }
    /// <summary>
    /// Information about the shot operation.
    ///  QSHOT_STATUS
    /// </summary>
    public class QshotStatusPacket : MavlinkV2Message<QshotStatusPayload>
    {
        public const int MessageId = 60020;
        
        public const byte CrcExtra = 202;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override QshotStatusPayload Payload { get; } = new();

        public override string Name => "QSHOT_STATUS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "mode",
            "Current shot mode.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "shot_state",
            "Current state in the shot. States are specific to the selected shot mode.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
        ];
        public const string FormatMessage = "QSHOT_STATUS:"
        + "uint16_t mode;"
        + "uint16_t shot_state;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], (ulong)Payload.Mode);
            writer.Write(StaticFields[1], Payload.ShotState);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Mode = (MavQshotMode)reader.ReadUShort(StaticFields[0]);
            Payload.ShotState = reader.ReadUShort(StaticFields[1]);
        
            
        }
    }

    /// <summary>
    ///  QSHOT_STATUS
    /// </summary>
    public class QshotStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 4; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 4; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 2 // uint16_t mode
            +2 // uint16_t shot_state
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Mode = (MavQshotMode)BinSerialize.ReadUShort(ref buffer);
            ShotState = BinSerialize.ReadUShort(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,(ushort)Mode);
            BinSerialize.WriteUShort(ref buffer,ShotState);
            /* PayloadByteSize = 4 */;
        }
        
        



        /// <summary>
        /// Current shot mode.
        /// OriginName: mode, Units: , IsExtended: false
        /// </summary>
        public MavQshotMode Mode { get; set; }
        /// <summary>
        /// Current state in the shot. States are specific to the selected shot mode.
        /// OriginName: shot_state, Units: , IsExtended: false
        /// </summary>
        public ushort ShotState { get; set; }
    }
    /// <summary>
    /// Radio channels. Supports up to 24 channels. Channel values are in centerd 13 bit format. Range is [-4096,4096], center is 0. Conversion to PWM is x * 5/32 + 1500. Should be emitted only by components with component id MAV_COMP_ID_TELEMETRY_RADIO.
    ///  RADIO_RC_CHANNELS
    /// </summary>
    public class RadioRcChannelsPacket : MavlinkV2Message<RadioRcChannelsPayload>
    {
        public const int MessageId = 60045;
        
        public const byte CrcExtra = 89;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override RadioRcChannelsPayload Payload { get; } = new();

        public override string Name => "RADIO_RC_CHANNELS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "count",
            "Total number of RC channels being received. This can be larger than 24, indicating that more channels are available but not given in this message.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(1,
            "flags",
            "Radio channels status flags.",
            string.Empty, 
            string.Empty, 
            "bitmask", 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(2,
            "channels",
            "RC channels. Channels above count should be set to 0, to benefit from MAVLink's zero padding.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            24, 
            true),
        ];
        public const string FormatMessage = "RADIO_RC_CHANNELS:"
        + "uint8_t count;"
        + "uint8_t flags;"
        + "int16_t[24] channels;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Count);
            writer.Write(StaticFields[1], (ulong)Payload.Flags);
            writer.Write(StaticFields[2], Payload.Channels);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Count = reader.ReadByte(StaticFields[0]);
            Payload.Flags = (RadioRcChannelsFlags)reader.ReadByte(StaticFields[1]);
            reader.ReadShortArray(StaticFields[2], Payload.Channels);
        
            
        }
    }

    /// <summary>
    ///  RADIO_RC_CHANNELS
    /// </summary>
    public class RadioRcChannelsPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 50; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 50; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t count
            + 1 // uint8_t flags
            +Channels.Length * 2 // int16_t[24] channels
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Count = (byte)BinSerialize.ReadByte(ref buffer);
            Flags = (RadioRcChannelsFlags)BinSerialize.ReadByte(ref buffer);
            // extended field 'Channels' can be empty
            if (buffer.IsEmpty) return;
            arraySize = 24;
            for(var i=0;i<arraySize;i++)
            {
                Channels[i] = BinSerialize.ReadShort(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Count);
            BinSerialize.WriteByte(ref buffer,(byte)Flags);
            for(var i=0;i<Channels.Length;i++)
            {
                BinSerialize.WriteShort(ref buffer,Channels[i]);
            }
            /* PayloadByteSize = 50 */;
        }
        
        



        /// <summary>
        /// Total number of RC channels being received. This can be larger than 24, indicating that more channels are available but not given in this message.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public byte Count { get; set; }
        /// <summary>
        /// Radio channels status flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public RadioRcChannelsFlags Flags { get; set; }
        /// <summary>
        /// RC channels. Channels above count should be set to 0, to benefit from MAVLink's zero padding.
        /// OriginName: channels, Units: , IsExtended: true
        /// </summary>
        public const int ChannelsMaxItemsCount = 24;
        public short[] Channels { get; } = new short[24];
    }
    /// <summary>
    /// Radio link statistics. Should be emitted only by components with component id MAV_COMP_ID_TELEMETRY_RADIO. Per default, rssi values are in MAVLink units: 0 represents weakest signal, 254 represents maximum signal; can be changed to dBm with the flag RADIO_LINK_STATS_FLAGS_RSSI_DBM.
    ///  RADIO_LINK_STATS
    /// </summary>
    public class RadioLinkStatsPacket : MavlinkV2Message<RadioLinkStatsPayload>
    {
        public const int MessageId = 60046;
        
        public const byte CrcExtra = 238;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override RadioLinkStatsPayload Payload { get; } = new();

        public override string Name => "RADIO_LINK_STATS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "flags",
            "Radio link statistics flags.",
            string.Empty, 
            string.Empty, 
            "bitmask", 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(1,
            "rx_LQ",
            "Values: 0..100. UINT8_MAX: invalid/unknown.",
            string.Empty, 
            @"c%", 
            string.Empty, 
            @"UINT8_MAX", 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(2,
            "rx_rssi1",
            "Rssi of antenna1. UINT8_MAX: invalid/unknown.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"UINT8_MAX", 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(3,
            "rx_snr1",
            "Noise on antenna1. Radio dependent. INT8_MAX: invalid/unknown.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"INT8_MAX", 
            MessageFieldType.Int8, 
            0, 
            false),
            new(4,
            "rx_rssi2",
            "Rssi of antenna2. UINT8_MAX: ignore/unknown, use rx_rssi1.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"UINT8_MAX", 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(5,
            "rx_snr2",
            "Noise on antenna2. Radio dependent. INT8_MAX: ignore/unknown, use rx_snr1.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"INT8_MAX", 
            MessageFieldType.Int8, 
            0, 
            false),
            new(6,
            "rx_receive_antenna",
            "0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Rx receive diversity, use rx_rssi1, rx_snr1.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"UINT8_MAX", 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(7,
            "rx_transmit_antenna",
            "0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Rx transmit diversity.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"UINT8_MAX", 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(8,
            "tx_LQ",
            "Values: 0..100. UINT8_MAX: invalid/unknown.",
            string.Empty, 
            @"c%", 
            string.Empty, 
            @"UINT8_MAX", 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(9,
            "tx_rssi1",
            "Rssi of antenna1. UINT8_MAX: invalid/unknown.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"UINT8_MAX", 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(10,
            "tx_snr1",
            "Noise on antenna1. Radio dependent. INT8_MAX: invalid/unknown.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"INT8_MAX", 
            MessageFieldType.Int8, 
            0, 
            false),
            new(11,
            "tx_rssi2",
            "Rssi of antenna2. UINT8_MAX: ignore/unknown, use tx_rssi1.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"UINT8_MAX", 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(12,
            "tx_snr2",
            "Noise on antenna2. Radio dependent. INT8_MAX: ignore/unknown, use tx_snr1.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"INT8_MAX", 
            MessageFieldType.Int8, 
            0, 
            false),
            new(13,
            "tx_receive_antenna",
            "0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Tx receive diversity, use tx_rssi1, tx_snr1.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"UINT8_MAX", 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(14,
            "tx_transmit_antenna",
            "0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Tx transmit diversity.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            @"UINT8_MAX", 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "RADIO_LINK_STATS:"
        + "uint8_t flags;"
        + "uint8_t rx_LQ;"
        + "uint8_t rx_rssi1;"
        + "int8_t rx_snr1;"
        + "uint8_t rx_rssi2;"
        + "int8_t rx_snr2;"
        + "uint8_t rx_receive_antenna;"
        + "uint8_t rx_transmit_antenna;"
        + "uint8_t tx_LQ;"
        + "uint8_t tx_rssi1;"
        + "int8_t tx_snr1;"
        + "uint8_t tx_rssi2;"
        + "int8_t tx_snr2;"
        + "uint8_t tx_receive_antenna;"
        + "uint8_t tx_transmit_antenna;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], (ulong)Payload.Flags);
            writer.Write(StaticFields[1], Payload.RxLq);
            writer.Write(StaticFields[2], Payload.RxRssi1);
            writer.Write(StaticFields[3], Payload.RxSnr1);
            writer.Write(StaticFields[4], Payload.RxRssi2);
            writer.Write(StaticFields[5], Payload.RxSnr2);
            writer.Write(StaticFields[6], Payload.RxReceiveAntenna);
            writer.Write(StaticFields[7], Payload.RxTransmitAntenna);
            writer.Write(StaticFields[8], Payload.TxLq);
            writer.Write(StaticFields[9], Payload.TxRssi1);
            writer.Write(StaticFields[10], Payload.TxSnr1);
            writer.Write(StaticFields[11], Payload.TxRssi2);
            writer.Write(StaticFields[12], Payload.TxSnr2);
            writer.Write(StaticFields[13], Payload.TxReceiveAntenna);
            writer.Write(StaticFields[14], Payload.TxTransmitAntenna);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Flags = (RadioLinkStatsFlags)reader.ReadByte(StaticFields[0]);
            Payload.RxLq = reader.ReadByte(StaticFields[1]);
            Payload.RxRssi1 = reader.ReadByte(StaticFields[2]);
            Payload.RxSnr1 = reader.ReadSByte(StaticFields[3]);
            Payload.RxRssi2 = reader.ReadByte(StaticFields[4]);
            Payload.RxSnr2 = reader.ReadSByte(StaticFields[5]);
            Payload.RxReceiveAntenna = reader.ReadByte(StaticFields[6]);
            Payload.RxTransmitAntenna = reader.ReadByte(StaticFields[7]);
            Payload.TxLq = reader.ReadByte(StaticFields[8]);
            Payload.TxRssi1 = reader.ReadByte(StaticFields[9]);
            Payload.TxSnr1 = reader.ReadSByte(StaticFields[10]);
            Payload.TxRssi2 = reader.ReadByte(StaticFields[11]);
            Payload.TxSnr2 = reader.ReadSByte(StaticFields[12]);
            Payload.TxReceiveAntenna = reader.ReadByte(StaticFields[13]);
            Payload.TxTransmitAntenna = reader.ReadByte(StaticFields[14]);
        
            
        }
    }

    /// <summary>
    ///  RADIO_LINK_STATS
    /// </summary>
    public class RadioLinkStatsPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 15; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 15; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 1 // uint8_t flags
            +1 // uint8_t rx_LQ
            +1 // uint8_t rx_rssi1
            +1 // int8_t rx_snr1
            +1 // uint8_t rx_rssi2
            +1 // int8_t rx_snr2
            +1 // uint8_t rx_receive_antenna
            +1 // uint8_t rx_transmit_antenna
            +1 // uint8_t tx_LQ
            +1 // uint8_t tx_rssi1
            +1 // int8_t tx_snr1
            +1 // uint8_t tx_rssi2
            +1 // int8_t tx_snr2
            +1 // uint8_t tx_receive_antenna
            +1 // uint8_t tx_transmit_antenna
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Flags = (RadioLinkStatsFlags)BinSerialize.ReadByte(ref buffer);
            RxLq = (byte)BinSerialize.ReadByte(ref buffer);
            RxRssi1 = (byte)BinSerialize.ReadByte(ref buffer);
            RxSnr1 = (sbyte)BinSerialize.ReadByte(ref buffer);
            RxRssi2 = (byte)BinSerialize.ReadByte(ref buffer);
            RxSnr2 = (sbyte)BinSerialize.ReadByte(ref buffer);
            RxReceiveAntenna = (byte)BinSerialize.ReadByte(ref buffer);
            RxTransmitAntenna = (byte)BinSerialize.ReadByte(ref buffer);
            TxLq = (byte)BinSerialize.ReadByte(ref buffer);
            TxRssi1 = (byte)BinSerialize.ReadByte(ref buffer);
            TxSnr1 = (sbyte)BinSerialize.ReadByte(ref buffer);
            TxRssi2 = (byte)BinSerialize.ReadByte(ref buffer);
            TxSnr2 = (sbyte)BinSerialize.ReadByte(ref buffer);
            TxReceiveAntenna = (byte)BinSerialize.ReadByte(ref buffer);
            TxTransmitAntenna = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Flags);
            BinSerialize.WriteByte(ref buffer,(byte)RxLq);
            BinSerialize.WriteByte(ref buffer,(byte)RxRssi1);
            BinSerialize.WriteByte(ref buffer,(byte)RxSnr1);
            BinSerialize.WriteByte(ref buffer,(byte)RxRssi2);
            BinSerialize.WriteByte(ref buffer,(byte)RxSnr2);
            BinSerialize.WriteByte(ref buffer,(byte)RxReceiveAntenna);
            BinSerialize.WriteByte(ref buffer,(byte)RxTransmitAntenna);
            BinSerialize.WriteByte(ref buffer,(byte)TxLq);
            BinSerialize.WriteByte(ref buffer,(byte)TxRssi1);
            BinSerialize.WriteByte(ref buffer,(byte)TxSnr1);
            BinSerialize.WriteByte(ref buffer,(byte)TxRssi2);
            BinSerialize.WriteByte(ref buffer,(byte)TxSnr2);
            BinSerialize.WriteByte(ref buffer,(byte)TxReceiveAntenna);
            BinSerialize.WriteByte(ref buffer,(byte)TxTransmitAntenna);
            /* PayloadByteSize = 15 */;
        }
        
        



        /// <summary>
        /// Radio link statistics flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public RadioLinkStatsFlags Flags { get; set; }
        /// <summary>
        /// Values: 0..100. UINT8_MAX: invalid/unknown.
        /// OriginName: rx_LQ, Units: c%, IsExtended: false
        /// </summary>
        public byte RxLq { get; set; }
        /// <summary>
        /// Rssi of antenna1. UINT8_MAX: invalid/unknown.
        /// OriginName: rx_rssi1, Units: , IsExtended: false
        /// </summary>
        public byte RxRssi1 { get; set; }
        /// <summary>
        /// Noise on antenna1. Radio dependent. INT8_MAX: invalid/unknown.
        /// OriginName: rx_snr1, Units: , IsExtended: false
        /// </summary>
        public sbyte RxSnr1 { get; set; }
        /// <summary>
        /// Rssi of antenna2. UINT8_MAX: ignore/unknown, use rx_rssi1.
        /// OriginName: rx_rssi2, Units: , IsExtended: false
        /// </summary>
        public byte RxRssi2 { get; set; }
        /// <summary>
        /// Noise on antenna2. Radio dependent. INT8_MAX: ignore/unknown, use rx_snr1.
        /// OriginName: rx_snr2, Units: , IsExtended: false
        /// </summary>
        public sbyte RxSnr2 { get; set; }
        /// <summary>
        /// 0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Rx receive diversity, use rx_rssi1, rx_snr1.
        /// OriginName: rx_receive_antenna, Units: , IsExtended: false
        /// </summary>
        public byte RxReceiveAntenna { get; set; }
        /// <summary>
        /// 0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Rx transmit diversity.
        /// OriginName: rx_transmit_antenna, Units: , IsExtended: false
        /// </summary>
        public byte RxTransmitAntenna { get; set; }
        /// <summary>
        /// Values: 0..100. UINT8_MAX: invalid/unknown.
        /// OriginName: tx_LQ, Units: c%, IsExtended: false
        /// </summary>
        public byte TxLq { get; set; }
        /// <summary>
        /// Rssi of antenna1. UINT8_MAX: invalid/unknown.
        /// OriginName: tx_rssi1, Units: , IsExtended: false
        /// </summary>
        public byte TxRssi1 { get; set; }
        /// <summary>
        /// Noise on antenna1. Radio dependent. INT8_MAX: invalid/unknown.
        /// OriginName: tx_snr1, Units: , IsExtended: false
        /// </summary>
        public sbyte TxSnr1 { get; set; }
        /// <summary>
        /// Rssi of antenna2. UINT8_MAX: ignore/unknown, use tx_rssi1.
        /// OriginName: tx_rssi2, Units: , IsExtended: false
        /// </summary>
        public byte TxRssi2 { get; set; }
        /// <summary>
        /// Noise on antenna2. Radio dependent. INT8_MAX: ignore/unknown, use tx_snr1.
        /// OriginName: tx_snr2, Units: , IsExtended: false
        /// </summary>
        public sbyte TxSnr2 { get; set; }
        /// <summary>
        /// 0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Tx receive diversity, use tx_rssi1, tx_snr1.
        /// OriginName: tx_receive_antenna, Units: , IsExtended: false
        /// </summary>
        public byte TxReceiveAntenna { get; set; }
        /// <summary>
        /// 0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Tx transmit diversity.
        /// OriginName: tx_transmit_antenna, Units: , IsExtended: false
        /// </summary>
        public byte TxTransmitAntenna { get; set; }
    }
    /// <summary>
    /// Frsky SPort passthrough multi packet container.
    ///  FRSKY_PASSTHROUGH_ARRAY
    /// </summary>
    public class FrskyPassthroughArrayPacket : MavlinkV2Message<FrskyPassthroughArrayPayload>
    {
        public const int MessageId = 60040;
        
        public const byte CrcExtra = 156;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override FrskyPassthroughArrayPayload Payload { get; } = new();

        public override string Name => "FRSKY_PASSTHROUGH_ARRAY";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "time_boot_ms",
            "Timestamp (time since system boot).",
            string.Empty, 
            @"ms", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new(1,
            "count",
            "Number of passthrough packets in this message.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(2,
            "packet_buf",
            "Passthrough packet buffer. A packet has 6 bytes: uint16_t id + uint32_t data. The array has space for 40 packets.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            240, 
            false),
        ];
        public const string FormatMessage = "FRSKY_PASSTHROUGH_ARRAY:"
        + "uint32_t time_boot_ms;"
        + "uint8_t count;"
        + "uint8_t[240] packet_buf;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.TimeBootMs);
            writer.Write(StaticFields[1], Payload.Count);
            writer.Write(StaticFields[2], Payload.PacketBuf);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.TimeBootMs = reader.ReadUInt(StaticFields[0]);
            Payload.Count = reader.ReadByte(StaticFields[1]);
            reader.ReadByteArray(StaticFields[2], Payload.PacketBuf);
        
            
        }
    }

    /// <summary>
    ///  FRSKY_PASSTHROUGH_ARRAY
    /// </summary>
    public class FrskyPassthroughArrayPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 245; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 245; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t time_boot_ms
            +1 // uint8_t count
            +PacketBuf.Length // uint8_t[240] packet_buf
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeBootMs = BinSerialize.ReadUInt(ref buffer);
            Count = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/240 - Math.Max(0,((/*PayloadByteSize*/245 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            PacketBuf = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                PacketBuf[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,TimeBootMs);
            BinSerialize.WriteByte(ref buffer,(byte)Count);
            for(var i=0;i<PacketBuf.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)PacketBuf[i]);
            }
            /* PayloadByteSize = 245 */;
        }
        
        



        /// <summary>
        /// Timestamp (time since system boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public uint TimeBootMs { get; set; }
        /// <summary>
        /// Number of passthrough packets in this message.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public byte Count { get; set; }
        /// <summary>
        /// Passthrough packet buffer. A packet has 6 bytes: uint16_t id + uint32_t data. The array has space for 40 packets.
        /// OriginName: packet_buf, Units: , IsExtended: false
        /// </summary>
        public const int PacketBufMaxItemsCount = 240;
        public byte[] PacketBuf { get; set; } = new byte[240];
        [Obsolete("This method is deprecated. Use GetPacketBufMaxItemsCount instead.")]
        public byte GetPacketBufMaxItemsCount() => 240;
    }
    /// <summary>
    /// Parameter multi param value container.
    ///  PARAM_VALUE_ARRAY
    /// </summary>
    public class ParamValueArrayPacket : MavlinkV2Message<ParamValueArrayPayload>
    {
        public const int MessageId = 60041;
        
        public const byte CrcExtra = 191;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override ParamValueArrayPayload Payload { get; } = new();

        public override string Name => "PARAM_VALUE_ARRAY";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "param_count",
            "Total number of onboard parameters.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(1,
            "param_index_first",
            "Index of the first onboard parameter in this array.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(2,
            "flags",
            "Flags.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new(3,
            "param_array_len",
            "Number of onboard parameters in this array.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(4,
            "packet_buf",
            "Parameters buffer. Contains a series of variable length parameter blocks, one per parameter, with format as specifed elsewhere.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            248, 
            false),
        ];
        public const string FormatMessage = "PARAM_VALUE_ARRAY:"
        + "uint16_t param_count;"
        + "uint16_t param_index_first;"
        + "uint16_t flags;"
        + "uint8_t param_array_len;"
        + "uint8_t[248] packet_buf;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.ParamCount);
            writer.Write(StaticFields[1], Payload.ParamIndexFirst);
            writer.Write(StaticFields[2], Payload.Flags);
            writer.Write(StaticFields[3], Payload.ParamArrayLen);
            writer.Write(StaticFields[4], Payload.PacketBuf);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.ParamCount = reader.ReadUShort(StaticFields[0]);
            Payload.ParamIndexFirst = reader.ReadUShort(StaticFields[1]);
            Payload.Flags = reader.ReadUShort(StaticFields[2]);
            Payload.ParamArrayLen = reader.ReadByte(StaticFields[3]);
            reader.ReadByteArray(StaticFields[4], Payload.PacketBuf);
        
            
        }
    }

    /// <summary>
    ///  PARAM_VALUE_ARRAY
    /// </summary>
    public class ParamValueArrayPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 255; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 255; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t param_count
            +2 // uint16_t param_index_first
            +2 // uint16_t flags
            +1 // uint8_t param_array_len
            +PacketBuf.Length // uint8_t[248] packet_buf
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            ParamCount = BinSerialize.ReadUShort(ref buffer);
            ParamIndexFirst = BinSerialize.ReadUShort(ref buffer);
            Flags = BinSerialize.ReadUShort(ref buffer);
            ParamArrayLen = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/248 - Math.Max(0,((/*PayloadByteSize*/255 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            PacketBuf = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                PacketBuf[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,ParamCount);
            BinSerialize.WriteUShort(ref buffer,ParamIndexFirst);
            BinSerialize.WriteUShort(ref buffer,Flags);
            BinSerialize.WriteByte(ref buffer,(byte)ParamArrayLen);
            for(var i=0;i<PacketBuf.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)PacketBuf[i]);
            }
            /* PayloadByteSize = 255 */;
        }
        
        



        /// <summary>
        /// Total number of onboard parameters.
        /// OriginName: param_count, Units: , IsExtended: false
        /// </summary>
        public ushort ParamCount { get; set; }
        /// <summary>
        /// Index of the first onboard parameter in this array.
        /// OriginName: param_index_first, Units: , IsExtended: false
        /// </summary>
        public ushort ParamIndexFirst { get; set; }
        /// <summary>
        /// Flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public ushort Flags { get; set; }
        /// <summary>
        /// Number of onboard parameters in this array.
        /// OriginName: param_array_len, Units: , IsExtended: false
        /// </summary>
        public byte ParamArrayLen { get; set; }
        /// <summary>
        /// Parameters buffer. Contains a series of variable length parameter blocks, one per parameter, with format as specifed elsewhere.
        /// OriginName: packet_buf, Units: , IsExtended: false
        /// </summary>
        public const int PacketBufMaxItemsCount = 248;
        public byte[] PacketBuf { get; set; } = new byte[248];
        [Obsolete("This method is deprecated. Use GetPacketBufMaxItemsCount instead.")]
        public byte GetPacketBufMaxItemsCount() => 248;
    }


#endregion


}

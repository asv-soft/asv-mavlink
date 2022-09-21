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
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.V2.Storm32
{

    public static class Storm32Helper
    {
        public static void RegisterStorm32Dialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new Storm32GimbalDeviceStatusPacket());
            src.Register(()=>new Storm32GimbalDeviceControlPacket());
            src.Register(()=>new Storm32GimbalManagerInformationPacket());
            src.Register(()=>new Storm32GimbalManagerStatusPacket());
            src.Register(()=>new Storm32GimbalManagerControlPacket());
            src.Register(()=>new Storm32GimbalManagerControlPitchyawPacket());
            src.Register(()=>new Storm32GimbalManagerCorrectRollPacket());
            src.Register(()=>new Storm32GimbalManagerProfilePacket());
            src.Register(()=>new QshotStatusPacket());
            src.Register(()=>new ComponentPrearmStatusPacket());
        }
    }

#region Enums

    /// <summary>
    ///  MAV_STORM32_TUNNEL_PAYLOAD_TYPE
    /// </summary>
    public enum MavStorm32TunnelPayloadType:uint
    {
        /// <summary>
        /// Registered for STorM32 gimbal controller.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH1_IN
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch1In = 200,
        /// <summary>
        /// Registered for STorM32 gimbal controller.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH1_OUT
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch1Out = 201,
        /// <summary>
        /// Registered for STorM32 gimbal controller.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH2_IN
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch2In = 202,
        /// <summary>
        /// Registered for STorM32 gimbal controller.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH2_OUT
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch2Out = 203,
        /// <summary>
        /// Registered for STorM32 gimbal controller.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH3_IN
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch3In = 204,
        /// <summary>
        /// Registered for STorM32 gimbal controller.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH3_OUT
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Ch3Out = 205,
        /// <summary>
        /// Registered for STorM32 gimbal controller.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_RESERVED6
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Reserved6 = 206,
        /// <summary>
        /// Registered for STorM32 gimbal controller.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_RESERVED7
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Reserved7 = 207,
        /// <summary>
        /// Registered for STorM32 gimbal controller.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_RESERVED8
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Reserved8 = 208,
        /// <summary>
        /// Registered for STorM32 gimbal controller.
        /// MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_RESERVED9
        /// </summary>
        MavStorm32TunnelPayloadTypeStorm32Reserved9 = 209,
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
        /// ???.
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
    /// Gimbal device capability flags.
    ///  MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS
    /// </summary>
    public enum MavStorm32GimbalDeviceCapFlags:uint
    {
        /// <summary>
        /// Gimbal device supports a retracted position.
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_RETRACT
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasRetract = 1,
        /// <summary>
        /// Gimbal device supports a horizontal, forward looking position, stabilized. Can also be used to reset the gimbal's orientation.
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_NEUTRAL
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasNeutral = 2,
        /// <summary>
        /// Gimbal device supports rotating around roll axis.
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_ROLL_AXIS
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasRollAxis = 4,
        /// <summary>
        /// Gimbal device supports to follow a roll angle relative to the vehicle.
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_ROLL_FOLLOW
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasRollFollow = 8,
        /// <summary>
        /// Gimbal device supports locking to an roll angle (generally that's the default).
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_ROLL_LOCK
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasRollLock = 16,
        /// <summary>
        /// Gimbal device supports rotating around pitch axis.
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_PITCH_AXIS
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasPitchAxis = 32,
        /// <summary>
        /// Gimbal device supports to follow a pitch angle relative to the vehicle.
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_PITCH_FOLLOW
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasPitchFollow = 64,
        /// <summary>
        /// Gimbal device supports locking to an pitch angle (generally that's the default).
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_PITCH_LOCK
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasPitchLock = 128,
        /// <summary>
        /// Gimbal device supports rotating around yaw axis.
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_YAW_AXIS
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasYawAxis = 256,
        /// <summary>
        /// Gimbal device supports to follow a yaw angle relative to the vehicle (generally that's the default).
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_YAW_FOLLOW
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasYawFollow = 512,
        /// <summary>
        /// Gimbal device supports locking to a heading angle.
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_YAW_LOCK
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasYawLock = 1024,
        /// <summary>
        /// Gimbal device supports yawing/panning infinitely (e.g. using a slip ring).
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_INFINITE_YAW
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasInfiniteYaw = 2048,
        /// <summary>
        /// Gimbal device supports absolute yaw angles (this usually requires support by an autopilot, and can be dynamic, i.e., go on and off during runtime).
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_ABSOLUTE_YAW
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasAbsoluteYaw = 65536,
        /// <summary>
        /// Gimbal device supports control via an RC input signal.
        /// MAV_STORM32_GIMBAL_DEVICE_CAP_FLAGS_HAS_RC
        /// </summary>
        MavStorm32GimbalDeviceCapFlagsHasRc = 131072,
    }

    /// <summary>
    /// Flags for gimbal device operation. Used for setting and reporting, unless specified otherwise. Settings which are in violation of the capability flags are ignored by the gimbal device.
    ///  MAV_STORM32_GIMBAL_DEVICE_FLAGS
    /// </summary>
    public enum MavStorm32GimbalDeviceFlags:uint
    {
        /// <summary>
        /// Retracted safe position (no stabilization), takes presedence over NEUTRAL flag. If supported by the gimbal, the angles in the retracted position can be set in addition.
        /// MAV_STORM32_GIMBAL_DEVICE_FLAGS_RETRACT
        /// </summary>
        MavStorm32GimbalDeviceFlagsRetract = 1,
        /// <summary>
        /// Neutral position (horizontal, forward looking, with stabiliziation).
        /// MAV_STORM32_GIMBAL_DEVICE_FLAGS_NEUTRAL
        /// </summary>
        MavStorm32GimbalDeviceFlagsNeutral = 2,
        /// <summary>
        /// Lock roll angle to absolute angle relative to horizon (not relative to drone). This is generally the default.
        /// MAV_STORM32_GIMBAL_DEVICE_FLAGS_ROLL_LOCK
        /// </summary>
        MavStorm32GimbalDeviceFlagsRollLock = 4,
        /// <summary>
        /// Lock pitch angle to absolute angle relative to horizon (not relative to drone). This is generally the default.
        /// MAV_STORM32_GIMBAL_DEVICE_FLAGS_PITCH_LOCK
        /// </summary>
        MavStorm32GimbalDeviceFlagsPitchLock = 8,
        /// <summary>
        /// Lock yaw angle to absolute angle relative to earth (not relative to drone). When the YAW_ABSOLUTE flag is set, the quaternion is in the Earth frame with the x-axis pointing North (yaw absolute), else it is in the Earth frame rotated so that the x-axis is pointing forward (yaw relative to vehicle).
        /// MAV_STORM32_GIMBAL_DEVICE_FLAGS_YAW_LOCK
        /// </summary>
        MavStorm32GimbalDeviceFlagsYawLock = 16,
        /// <summary>
        /// Gimbal device can accept absolute yaw angle input. This flag cannot be set, is only for reporting (attempts to set it are rejected by the gimbal device).
        /// MAV_STORM32_GIMBAL_DEVICE_FLAGS_CAN_ACCEPT_YAW_ABSOLUTE
        /// </summary>
        MavStorm32GimbalDeviceFlagsCanAcceptYawAbsolute = 256,
        /// <summary>
        /// Yaw angle is absolute (is only accepted if CAN_ACCEPT_YAW_ABSOLUTE is set). If this flag is set, the quaternion is in the Earth frame with the x-axis pointing North (yaw absolute), else it is in the Earth frame rotated so that the x-axis is pointing forward (yaw relative to vehicle).
        /// MAV_STORM32_GIMBAL_DEVICE_FLAGS_YAW_ABSOLUTE
        /// </summary>
        MavStorm32GimbalDeviceFlagsYawAbsolute = 512,
        /// <summary>
        /// RC control. The RC input signal fed to the gimbal device exclusively controls the gimbal's orientation. Overrides RC_MIXED flag if that is also set.
        /// MAV_STORM32_GIMBAL_DEVICE_FLAGS_RC_EXCLUSIVE
        /// </summary>
        MavStorm32GimbalDeviceFlagsRcExclusive = 1024,
        /// <summary>
        /// RC control. The RC input signal fed to the gimbal device is mixed into the gimbal's orientation. Is overriden by RC_EXCLUSIVE flag if that is also set.
        /// MAV_STORM32_GIMBAL_DEVICE_FLAGS_RC_MIXED
        /// </summary>
        MavStorm32GimbalDeviceFlagsRcMixed = 2048,
        /// <summary>
        /// UINT16_MAX = ignore.
        /// MAV_STORM32_GIMBAL_DEVICE_FLAGS_NONE
        /// </summary>
        MavStorm32GimbalDeviceFlagsNone = 65535,
    }

    /// <summary>
    /// Gimbal device error and condition flags (0 means no error or other condition).
    ///  MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS
    /// </summary>
    public enum MavStorm32GimbalDeviceErrorFlags:uint
    {
        /// <summary>
        /// Gimbal device is limited by hardware roll limit.
        /// MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS_AT_ROLL_LIMIT
        /// </summary>
        MavStorm32GimbalDeviceErrorFlagsAtRollLimit = 1,
        /// <summary>
        /// Gimbal device is limited by hardware pitch limit.
        /// MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS_AT_PITCH_LIMIT
        /// </summary>
        MavStorm32GimbalDeviceErrorFlagsAtPitchLimit = 2,
        /// <summary>
        /// Gimbal device is limited by hardware yaw limit.
        /// MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS_AT_YAW_LIMIT
        /// </summary>
        MavStorm32GimbalDeviceErrorFlagsAtYawLimit = 4,
        /// <summary>
        /// There is an error with the gimbal device's encoders.
        /// MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS_ENCODER_ERROR
        /// </summary>
        MavStorm32GimbalDeviceErrorFlagsEncoderError = 8,
        /// <summary>
        /// There is an error with the gimbal device's power source.
        /// MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS_POWER_ERROR
        /// </summary>
        MavStorm32GimbalDeviceErrorFlagsPowerError = 16,
        /// <summary>
        /// There is an error with the gimbal device's motors.
        /// MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS_MOTOR_ERROR
        /// </summary>
        MavStorm32GimbalDeviceErrorFlagsMotorError = 32,
        /// <summary>
        /// There is an error with the gimbal device's software.
        /// MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS_SOFTWARE_ERROR
        /// </summary>
        MavStorm32GimbalDeviceErrorFlagsSoftwareError = 64,
        /// <summary>
        /// There is an error with the gimbal device's communication.
        /// MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS_COMMS_ERROR
        /// </summary>
        MavStorm32GimbalDeviceErrorFlagsCommsError = 128,
        /// <summary>
        /// Gimbal device is currently calibrating (not an error).
        /// MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS_CALIBRATION_RUNNING
        /// </summary>
        MavStorm32GimbalDeviceErrorFlagsCalibrationRunning = 256,
        /// <summary>
        /// Gimbal device is not assigned to a gimbal manager (not an error).
        /// MAV_STORM32_GIMBAL_DEVICE_ERROR_FLAGS_NO_MANAGER
        /// </summary>
        MavStorm32GimbalDeviceErrorFlagsNoManager = 32768,
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
        /// <summary>
        /// The gimbal manager supports changing the gimbal manager during run time, i.e. can be enabled/disabled.
        /// MAV_STORM32_GIMBAL_MANAGER_CAP_FLAGS_SUPPORTS_CHANGE
        /// </summary>
        MavStorm32GimbalManagerCapFlagsSupportsChange = 2,
    }

    /// <summary>
    /// Flags for gimbal manager operation. Used for setting and reporting, unless specified otherwise. If a setting is accepted by the gimbal manger, is reported in the STORM32_GIMBAL_MANAGER_STATUS message.
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
    /// Flags for gimbal manager set up. Used for setting and reporting, unless specified otherwise.
    ///  MAV_STORM32_GIMBAL_MANAGER_SETUP_FLAGS
    /// </summary>
    public enum MavStorm32GimbalManagerSetupFlags:uint
    {
        /// <summary>
        /// Enable gimbal manager. This flag is only for setting, is not reported.
        /// MAV_STORM32_GIMBAL_MANAGER_SETUP_FLAGS_ENABLE
        /// </summary>
        MavStorm32GimbalManagerSetupFlagsEnable = 16384,
        /// <summary>
        /// Disable gimbal manager. This flag is only for setting, is not reported.
        /// MAV_STORM32_GIMBAL_MANAGER_SETUP_FLAGS_DISABLE
        /// </summary>
        MavStorm32GimbalManagerSetupFlagsDisable = 32768,
    }

    /// <summary>
    /// Gimbal manager profiles. Only standard profiles are defined. Any implementation can define it's own profile in addition, and should use enum values > 16.
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
        /// Custom profile. Configurable profile according to the STorM32 definition. Is configured with STORM32_GIMBAL_MANAGER_PROFIL.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_CUSTOM
        /// </summary>
        MavStorm32GimbalManagerProfileCustom = 1,
        /// <summary>
        /// Default cooperative profile. Uses STorM32 custom profile with default settings to achieve cooperative behavior.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_COOPERATIVE
        /// </summary>
        MavStorm32GimbalManagerProfileCooperative = 2,
        /// <summary>
        /// Default exclusive profile. Uses STorM32 custom profile with default settings to achieve exclusive behavior.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_EXCLUSIVE
        /// </summary>
        MavStorm32GimbalManagerProfileExclusive = 3,
        /// <summary>
        /// Default priority profile with cooperative behavior for equal priority. Uses STorM32 custom profile with default settings to achieve priority-based behavior.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_PRIORITY_COOPERATIVE
        /// </summary>
        MavStorm32GimbalManagerProfilePriorityCooperative = 4,
        /// <summary>
        /// Default priority profile with exclusive behavior for equal priority. Uses STorM32 custom profile with default settings to achieve priority-based behavior.
        /// MAV_STORM32_GIMBAL_MANAGER_PROFILE_PRIORITY_EXCLUSIVE
        /// </summary>
        MavStorm32GimbalManagerProfilePriorityExclusive = 5,
    }

    /// <summary>
    /// Gimbal actions.
    ///  MAV_STORM32_GIMBAL_ACTION
    /// </summary>
    public enum MavStorm32GimbalAction:uint
    {
        /// <summary>
        /// Trigger the gimbal device to recenter the gimbal.
        /// MAV_STORM32_GIMBAL_ACTION_RECENTER
        /// </summary>
        MavStorm32GimbalActionRecenter = 1,
        /// <summary>
        /// Trigger the gimbal device to run a calibration.
        /// MAV_STORM32_GIMBAL_ACTION_CALIBRATION
        /// </summary>
        MavStorm32GimbalActionCalibration = 2,
        /// <summary>
        /// Trigger gimbal device to (re)discover the gimbal manager during run time.
        /// MAV_STORM32_GIMBAL_ACTION_DISCOVER_MANAGER
        /// </summary>
        MavStorm32GimbalActionDiscoverManager = 3,
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
        /// Start normal gimbal operation. Is usally used to return back from a shot.
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
        /// Param 1 - Pitch/tilt angle (positive: tilt up, NaN to be ignored).
        /// Param 2 - Yaw/pan angle (positive: pan to the right, the frame is determined by the STORM32_GIMBAL_DEVICE_FLAGS_YAW_ABSOLUTE flag, NaN to be ignored).
        /// Param 3 - Pitch/tilt rate (positive: tilt up, NaN to be ignored).
        /// Param 4 - Yaw/pan rate (positive: pan to the right, the frame is determined by the STORM32_GIMBAL_DEVICE_FLAGS_YAW_ABSOLUTE flag, NaN to be ignored).
        /// Param 5 - Gimbal device flags.
        /// Param 6 - Gimbal manager flags.
        /// Param 7 - Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals, send command multiple times for more than one but not all gimbals). The client is copied into bits 8-15.
        /// MAV_CMD_STORM32_DO_GIMBAL_MANAGER_CONTROL_PITCHYAW
        /// </summary>
        MavCmdStorm32DoGimbalManagerControlPitchyaw = 60002,
        /// <summary>
        /// Command to configure a gimbal manager. A gimbal device is never to react to this command. The selected profile is reported in the STORM32_GIMBAL_MANAGER_STATUS message.
        /// Param 1 - Gimbal manager profile (0 = default).
        /// Param 2 - Gimbal manager setup flags (0 = none).
        /// Param 7 - Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.
        /// MAV_CMD_STORM32_DO_GIMBAL_MANAGER_SETUP
        /// </summary>
        MavCmdStorm32DoGimbalManagerSetup = 60010,
        /// <summary>
        /// Command to initiate gimbal actions. Usually performed by the gimbal device, but some can also be done by the gimbal manager. It is hence best to broadcast this command.
        /// Param 1 - Gimbal action to initiate (0 = none).
        /// Param 7 - Gimbal ID of the gimbal to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.
        /// MAV_CMD_STORM32_DO_GIMBAL_ACTION
        /// </summary>
        MavCmdStorm32DoGimbalAction = 60011,
        /// <summary>
        /// Command to set the shot manager mode.
        /// Param 1 - Set shot mode.
        /// Param 2 - Set shot state or command. The allowed values are specific to the selected shot mode.
        /// MAV_CMD_QSHOT_DO_CONFIGURE
        /// </summary>
        MavCmdQshotDoConfigure = 60020,
    }


#endregion

#region Messages

    /// <summary>
    /// Message reporting the current status of a gimbal device. This message should be broadcasted by a gimbal device component at a low regular rate (e.g. 4 Hz). For higher rates it should be emitted with a target.
    ///  STORM32_GIMBAL_DEVICE_STATUS
    /// </summary>
    public class Storm32GimbalDeviceStatusPacket: PacketV2<Storm32GimbalDeviceStatusPayload>
    {
	    public const int PacketMessageId = 60001;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 186;

        public override Storm32GimbalDeviceStatusPayload Payload { get; } = new Storm32GimbalDeviceStatusPayload();

        public override string Name => "STORM32_GIMBAL_DEVICE_STATUS";
    }

    /// <summary>
    ///  STORM32_GIMBAL_DEVICE_STATUS
    /// </summary>
    public class Storm32GimbalDeviceStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 42; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 42; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TimeBootMs = BinSerialize.ReadUInt(ref buffer);index+=4;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/42 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            Q = new float[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Q[i] = BinSerialize.ReadFloat(ref buffer);index+=4;
            }
            AngularVelocityX = BinSerialize.ReadFloat(ref buffer);index+=4;
            AngularVelocityY = BinSerialize.ReadFloat(ref buffer);index+=4;
            AngularVelocityZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            YawAbsolute = BinSerialize.ReadFloat(ref buffer);index+=4;
            Flags = (MavStorm32GimbalDeviceFlags)BinSerialize.ReadUShort(ref buffer);index+=2;
            FailureFlags = (GimbalDeviceErrorFlags)BinSerialize.ReadUShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,TimeBootMs);index+=4;
            for(var i=0;i<Q.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,Q[i]);index+=4;
            }
            BinSerialize.WriteFloat(ref buffer,AngularVelocityX);index+=4;
            BinSerialize.WriteFloat(ref buffer,AngularVelocityY);index+=4;
            BinSerialize.WriteFloat(ref buffer,AngularVelocityZ);index+=4;
            BinSerialize.WriteFloat(ref buffer,YawAbsolute);index+=4;
            BinSerialize.WriteUShort(ref buffer,(ushort)Flags);index+=2;
            BinSerialize.WriteUShort(ref buffer,(ushort)FailureFlags);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            return index; // /*PayloadByteSize*/42;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TimeBootMs = BitConverter.ToUInt32(buffer,index);index+=4;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/42 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            Q = new float[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Q[i] = BitConverter.ToSingle(buffer, index);index+=4;
            }
            AngularVelocityX = BitConverter.ToSingle(buffer, index);index+=4;
            AngularVelocityY = BitConverter.ToSingle(buffer, index);index+=4;
            AngularVelocityZ = BitConverter.ToSingle(buffer, index);index+=4;
            YawAbsolute = BitConverter.ToSingle(buffer, index);index+=4;
            Flags = (MavStorm32GimbalDeviceFlags)BitConverter.ToUInt16(buffer,index);index+=2;
            FailureFlags = (GimbalDeviceErrorFlags)BitConverter.ToUInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TimeBootMs).CopyTo(buffer, index);index+=4;
            for(var i=0;i<Q.Length;i++)
            {
                BitConverter.GetBytes(Q[i]).CopyTo(buffer, index);index+=4;
            }
            BitConverter.GetBytes(AngularVelocityX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AngularVelocityY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AngularVelocityZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(YawAbsolute).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes((ushort)Flags).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes((ushort)FailureFlags).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/42;
        }

        /// <summary>
        /// Timestamp (time since system boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public uint TimeBootMs { get; set; }
        /// <summary>
        /// Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation). The frame depends on the STORM32_GIMBAL_DEVICE_FLAGS_YAW_ABSOLUTE flag.
        /// OriginName: q, Units: , IsExtended: false
        /// </summary>
        public float[] Q { get; set; } = new float[4];
        public byte GetQMaxItemsCount() => 4;
        /// <summary>
        /// X component of angular velocity (NaN if unknown).
        /// OriginName: angular_velocity_x, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityX { get; set; }
        /// <summary>
        /// Y component of angular velocity (NaN if unknown).
        /// OriginName: angular_velocity_y, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityY { get; set; }
        /// <summary>
        /// Z component of angular velocity (the frame depends on the STORM32_GIMBAL_DEVICE_FLAGS_YAW_ABSOLUTE flag, NaN if unknown).
        /// OriginName: angular_velocity_z, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityZ { get; set; }
        /// <summary>
        /// Yaw in absolute frame relative to Earth's North, north is 0 (NaN if unknown).
        /// OriginName: yaw_absolute, Units: deg, IsExtended: false
        /// </summary>
        public float YawAbsolute { get; set; }
        /// <summary>
        /// Gimbal device flags currently applied.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalDeviceFlags Flags { get; set; }
        /// <summary>
        /// Failure flags (0 for no failure).
        /// OriginName: failure_flags, Units: , IsExtended: false
        /// </summary>
        public GimbalDeviceErrorFlags FailureFlags { get; set; }
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
    /// Message to a gimbal device to control its attitude. This message is to be sent from the gimbal manager to the gimbal device. Angles and rates can be set to NaN according to use case.
    ///  STORM32_GIMBAL_DEVICE_CONTROL
    /// </summary>
    public class Storm32GimbalDeviceControlPacket: PacketV2<Storm32GimbalDeviceControlPayload>
    {
	    public const int PacketMessageId = 60002;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 69;

        public override Storm32GimbalDeviceControlPayload Payload { get; } = new Storm32GimbalDeviceControlPayload();

        public override string Name => "STORM32_GIMBAL_DEVICE_CONTROL";
    }

    /// <summary>
    ///  STORM32_GIMBAL_DEVICE_CONTROL
    /// </summary>
    public class Storm32GimbalDeviceControlPayload : IPayload
    {
        public byte GetMaxByteSize() => 32; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 32; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/32 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            Q = new float[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Q[i] = BinSerialize.ReadFloat(ref buffer);index+=4;
            }
            AngularVelocityX = BinSerialize.ReadFloat(ref buffer);index+=4;
            AngularVelocityY = BinSerialize.ReadFloat(ref buffer);index+=4;
            AngularVelocityZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            Flags = (MavStorm32GimbalDeviceFlags)BinSerialize.ReadUShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            for(var i=0;i<Q.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,Q[i]);index+=4;
            }
            BinSerialize.WriteFloat(ref buffer,AngularVelocityX);index+=4;
            BinSerialize.WriteFloat(ref buffer,AngularVelocityY);index+=4;
            BinSerialize.WriteFloat(ref buffer,AngularVelocityZ);index+=4;
            BinSerialize.WriteUShort(ref buffer,(ushort)Flags);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            return index; // /*PayloadByteSize*/32;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/32 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            Q = new float[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Q[i] = BitConverter.ToSingle(buffer, index);index+=4;
            }
            AngularVelocityX = BitConverter.ToSingle(buffer, index);index+=4;
            AngularVelocityY = BitConverter.ToSingle(buffer, index);index+=4;
            AngularVelocityZ = BitConverter.ToSingle(buffer, index);index+=4;
            Flags = (MavStorm32GimbalDeviceFlags)BitConverter.ToUInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            for(var i=0;i<Q.Length;i++)
            {
                BitConverter.GetBytes(Q[i]).CopyTo(buffer, index);index+=4;
            }
            BitConverter.GetBytes(AngularVelocityX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AngularVelocityY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AngularVelocityZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes((ushort)Flags).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/32;
        }

        /// <summary>
        /// Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation, the frame is determined by the STORM32_GIMBAL_DEVICE_FLAGS_YAW_ABSOLUTE flag, set first element to NaN to be ignored).
        /// OriginName: q, Units: , IsExtended: false
        /// </summary>
        public float[] Q { get; set; } = new float[4];
        public byte GetQMaxItemsCount() => 4;
        /// <summary>
        /// X component of angular velocity (positive: roll to the right, NaN to be ignored).
        /// OriginName: angular_velocity_x, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityX { get; set; }
        /// <summary>
        /// Y component of angular velocity (positive: tilt up, NaN to be ignored).
        /// OriginName: angular_velocity_y, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityY { get; set; }
        /// <summary>
        /// Z component of angular velocity (positive: pan to the right, the frame is determined by the STORM32_GIMBAL_DEVICE_FLAGS_YAW_ABSOLUTE flag, NaN to be ignored).
        /// OriginName: angular_velocity_z, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityZ { get; set; }
        /// <summary>
        /// Gimbal device flags (UINT16_MAX to be ignored).
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalDeviceFlags Flags { get; set; }
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
    /// Information about a gimbal manager. This message should be requested by a ground station using MAV_CMD_REQUEST_MESSAGE. It mirrors some fields of the STORM32_GIMBAL_DEVICE_INFORMATION message, but not all. If the additional information is desired, also STORM32_GIMBAL_DEVICE_INFORMATION should be requested.
    ///  STORM32_GIMBAL_MANAGER_INFORMATION
    /// </summary>
    public class Storm32GimbalManagerInformationPacket: PacketV2<Storm32GimbalManagerInformationPayload>
    {
	    public const int PacketMessageId = 60010;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 208;

        public override Storm32GimbalManagerInformationPayload Payload { get; } = new Storm32GimbalManagerInformationPayload();

        public override string Name => "STORM32_GIMBAL_MANAGER_INFORMATION";
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_INFORMATION
    /// </summary>
    public class Storm32GimbalManagerInformationPayload : IPayload
    {
        public byte GetMaxByteSize() => 33; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 33; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            DeviceCapFlags = (MavStorm32GimbalDeviceCapFlags)BinSerialize.ReadUInt(ref buffer);index+=4;
            ManagerCapFlags = (MavStorm32GimbalManagerCapFlags)BinSerialize.ReadUInt(ref buffer);index+=4;
            RollMin = BinSerialize.ReadFloat(ref buffer);index+=4;
            RollMax = BinSerialize.ReadFloat(ref buffer);index+=4;
            PitchMin = BinSerialize.ReadFloat(ref buffer);index+=4;
            PitchMax = BinSerialize.ReadFloat(ref buffer);index+=4;
            YawMin = BinSerialize.ReadFloat(ref buffer);index+=4;
            YawMax = BinSerialize.ReadFloat(ref buffer);index+=4;
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,(uint)DeviceCapFlags);index+=4;
            BinSerialize.WriteUInt(ref buffer,(uint)ManagerCapFlags);index+=4;
            BinSerialize.WriteFloat(ref buffer,RollMin);index+=4;
            BinSerialize.WriteFloat(ref buffer,RollMax);index+=4;
            BinSerialize.WriteFloat(ref buffer,PitchMin);index+=4;
            BinSerialize.WriteFloat(ref buffer,PitchMax);index+=4;
            BinSerialize.WriteFloat(ref buffer,YawMin);index+=4;
            BinSerialize.WriteFloat(ref buffer,YawMax);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);index+=1;
            return index; // /*PayloadByteSize*/33;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            DeviceCapFlags = (MavStorm32GimbalDeviceCapFlags)BitConverter.ToUInt32(buffer,index);index+=4;
            ManagerCapFlags = (MavStorm32GimbalManagerCapFlags)BitConverter.ToUInt32(buffer,index);index+=4;
            RollMin = BitConverter.ToSingle(buffer, index);index+=4;
            RollMax = BitConverter.ToSingle(buffer, index);index+=4;
            PitchMin = BitConverter.ToSingle(buffer, index);index+=4;
            PitchMax = BitConverter.ToSingle(buffer, index);index+=4;
            YawMin = BitConverter.ToSingle(buffer, index);index+=4;
            YawMax = BitConverter.ToSingle(buffer, index);index+=4;
            GimbalId = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes((uint)DeviceCapFlags).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes((uint)ManagerCapFlags).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(RollMin).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(RollMax).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PitchMin).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PitchMax).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(YawMin).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(YawMax).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(GimbalId).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/33;
        }

        /// <summary>
        /// Gimbal device capability flags.
        /// OriginName: device_cap_flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalDeviceCapFlags DeviceCapFlags { get; set; }
        /// <summary>
        /// Gimbal manager capability flags.
        /// OriginName: manager_cap_flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerCapFlags ManagerCapFlags { get; set; }
        /// <summary>
        /// Hardware minimum roll angle (positive: roll to the right, NaN if unknown).
        /// OriginName: roll_min, Units: rad, IsExtended: false
        /// </summary>
        public float RollMin { get; set; }
        /// <summary>
        /// Hardware maximum roll angle (positive: roll to the right, NaN if unknown).
        /// OriginName: roll_max, Units: rad, IsExtended: false
        /// </summary>
        public float RollMax { get; set; }
        /// <summary>
        /// Hardware minimum pitch/tilt angle (positive: tilt up, NaN if unknown).
        /// OriginName: pitch_min, Units: rad, IsExtended: false
        /// </summary>
        public float PitchMin { get; set; }
        /// <summary>
        /// Hardware maximum pitch/tilt angle (positive: tilt up, NaN if unknown).
        /// OriginName: pitch_max, Units: rad, IsExtended: false
        /// </summary>
        public float PitchMax { get; set; }
        /// <summary>
        /// Hardware minimum yaw/pan angle (positive: pan to the right, relative to the vehicle/gimbal base, NaN if unknown).
        /// OriginName: yaw_min, Units: rad, IsExtended: false
        /// </summary>
        public float YawMin { get; set; }
        /// <summary>
        /// Hardware maximum yaw/pan angle (positive: pan to the right, relative to the vehicle/gimbal base, NaN if unknown).
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
    public class Storm32GimbalManagerStatusPacket: PacketV2<Storm32GimbalManagerStatusPayload>
    {
	    public const int PacketMessageId = 60011;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 183;

        public override Storm32GimbalManagerStatusPayload Payload { get; } = new Storm32GimbalManagerStatusPayload();

        public override string Name => "STORM32_GIMBAL_MANAGER_STATUS";
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_STATUS
    /// </summary>
    public class Storm32GimbalManagerStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 7; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            DeviceFlags = (MavStorm32GimbalDeviceFlags)BinSerialize.ReadUShort(ref buffer);index+=2;
            ManagerFlags = (MavStorm32GimbalManagerFlags)BinSerialize.ReadUShort(ref buffer);index+=2;
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Supervisor = (MavStorm32GimbalManagerClient)BinSerialize.ReadByte(ref buffer);index+=1;
            Profile = (MavStorm32GimbalManagerProfile)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUShort(ref buffer,(ushort)DeviceFlags);index+=2;
            BinSerialize.WriteUShort(ref buffer,(ushort)ManagerFlags);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Supervisor);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Profile);index+=1;
            return index; // /*PayloadByteSize*/7;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            DeviceFlags = (MavStorm32GimbalDeviceFlags)BitConverter.ToUInt16(buffer,index);index+=2;
            ManagerFlags = (MavStorm32GimbalManagerFlags)BitConverter.ToUInt16(buffer,index);index+=2;
            GimbalId = (byte)buffer[index++];
            Supervisor = (MavStorm32GimbalManagerClient)buffer[index++];
            Profile = (MavStorm32GimbalManagerProfile)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes((ushort)DeviceFlags).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes((ushort)ManagerFlags).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(GimbalId).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Supervisor;index+=1;
            buffer[index] = (byte)Profile;index+=1;
            return index - start; // /*PayloadByteSize*/7;
        }

        /// <summary>
        /// Gimbal device flags currently applied.
        /// OriginName: device_flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalDeviceFlags DeviceFlags { get; set; }
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
    public class Storm32GimbalManagerControlPacket: PacketV2<Storm32GimbalManagerControlPayload>
    {
	    public const int PacketMessageId = 60012;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 99;

        public override Storm32GimbalManagerControlPayload Payload { get; } = new Storm32GimbalManagerControlPayload();

        public override string Name => "STORM32_GIMBAL_MANAGER_CONTROL";
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_CONTROL
    /// </summary>
    public class Storm32GimbalManagerControlPayload : IPayload
    {
        public byte GetMaxByteSize() => 36; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 36; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/36 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            Q = new float[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Q[i] = BinSerialize.ReadFloat(ref buffer);index+=4;
            }
            AngularVelocityX = BinSerialize.ReadFloat(ref buffer);index+=4;
            AngularVelocityY = BinSerialize.ReadFloat(ref buffer);index+=4;
            AngularVelocityZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            DeviceFlags = (MavStorm32GimbalDeviceFlags)BinSerialize.ReadUShort(ref buffer);index+=2;
            ManagerFlags = (MavStorm32GimbalManagerFlags)BinSerialize.ReadUShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Client = (MavStorm32GimbalManagerClient)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            for(var i=0;i<Q.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,Q[i]);index+=4;
            }
            BinSerialize.WriteFloat(ref buffer,AngularVelocityX);index+=4;
            BinSerialize.WriteFloat(ref buffer,AngularVelocityY);index+=4;
            BinSerialize.WriteFloat(ref buffer,AngularVelocityZ);index+=4;
            BinSerialize.WriteUShort(ref buffer,(ushort)DeviceFlags);index+=2;
            BinSerialize.WriteUShort(ref buffer,(ushort)ManagerFlags);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Client);index+=1;
            return index; // /*PayloadByteSize*/36;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/36 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            Q = new float[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Q[i] = BitConverter.ToSingle(buffer, index);index+=4;
            }
            AngularVelocityX = BitConverter.ToSingle(buffer, index);index+=4;
            AngularVelocityY = BitConverter.ToSingle(buffer, index);index+=4;
            AngularVelocityZ = BitConverter.ToSingle(buffer, index);index+=4;
            DeviceFlags = (MavStorm32GimbalDeviceFlags)BitConverter.ToUInt16(buffer,index);index+=2;
            ManagerFlags = (MavStorm32GimbalManagerFlags)BitConverter.ToUInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            GimbalId = (byte)buffer[index++];
            Client = (MavStorm32GimbalManagerClient)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            for(var i=0;i<Q.Length;i++)
            {
                BitConverter.GetBytes(Q[i]).CopyTo(buffer, index);index+=4;
            }
            BitConverter.GetBytes(AngularVelocityX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AngularVelocityY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AngularVelocityZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes((ushort)DeviceFlags).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes((ushort)ManagerFlags).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(GimbalId).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Client;index+=1;
            return index - start; // /*PayloadByteSize*/36;
        }

        /// <summary>
        /// Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation, the frame is determined by the GIMBAL_MANAGER_FLAGS_ABSOLUTE_YAW flag, set first element to NaN to be ignored).
        /// OriginName: q, Units: , IsExtended: false
        /// </summary>
        public float[] Q { get; set; } = new float[4];
        public byte GetQMaxItemsCount() => 4;
        /// <summary>
        /// X component of angular velocity (positive: roll to the right, NaN to be ignored).
        /// OriginName: angular_velocity_x, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityX { get; set; }
        /// <summary>
        /// Y component of angular velocity (positive: tilt up, NaN to be ignored).
        /// OriginName: angular_velocity_y, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityY { get; set; }
        /// <summary>
        /// Z component of angular velocity (positive: pan to the right, the frame is determined by the STORM32_GIMBAL_DEVICE_FLAGS_YAW_ABSOLUTE flag, NaN to be ignored).
        /// OriginName: angular_velocity_z, Units: rad/s, IsExtended: false
        /// </summary>
        public float AngularVelocityZ { get; set; }
        /// <summary>
        /// Gimbal device flags (UINT16_MAX to be ignored).
        /// OriginName: device_flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalDeviceFlags DeviceFlags { get; set; }
        /// <summary>
        /// Gimbal manager flags (0 to be ignored).
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
        /// Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals, send command multiple times for more than one but not all gimbals).
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
    public class Storm32GimbalManagerControlPitchyawPacket: PacketV2<Storm32GimbalManagerControlPitchyawPayload>
    {
	    public const int PacketMessageId = 60013;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 129;

        public override Storm32GimbalManagerControlPitchyawPayload Payload { get; } = new Storm32GimbalManagerControlPitchyawPayload();

        public override string Name => "STORM32_GIMBAL_MANAGER_CONTROL_PITCHYAW";
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_CONTROL_PITCHYAW
    /// </summary>
    public class Storm32GimbalManagerControlPitchyawPayload : IPayload
    {
        public byte GetMaxByteSize() => 24; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            Pitch = BinSerialize.ReadFloat(ref buffer);index+=4;
            Yaw = BinSerialize.ReadFloat(ref buffer);index+=4;
            PitchRate = BinSerialize.ReadFloat(ref buffer);index+=4;
            YawRate = BinSerialize.ReadFloat(ref buffer);index+=4;
            DeviceFlags = (MavStorm32GimbalDeviceFlags)BinSerialize.ReadUShort(ref buffer);index+=2;
            ManagerFlags = (MavStorm32GimbalManagerFlags)BinSerialize.ReadUShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Client = (MavStorm32GimbalManagerClient)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Pitch);index+=4;
            BinSerialize.WriteFloat(ref buffer,Yaw);index+=4;
            BinSerialize.WriteFloat(ref buffer,PitchRate);index+=4;
            BinSerialize.WriteFloat(ref buffer,YawRate);index+=4;
            BinSerialize.WriteUShort(ref buffer,(ushort)DeviceFlags);index+=2;
            BinSerialize.WriteUShort(ref buffer,(ushort)ManagerFlags);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Client);index+=1;
            return index; // /*PayloadByteSize*/24;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            Pitch = BitConverter.ToSingle(buffer, index);index+=4;
            Yaw = BitConverter.ToSingle(buffer, index);index+=4;
            PitchRate = BitConverter.ToSingle(buffer, index);index+=4;
            YawRate = BitConverter.ToSingle(buffer, index);index+=4;
            DeviceFlags = (MavStorm32GimbalDeviceFlags)BitConverter.ToUInt16(buffer,index);index+=2;
            ManagerFlags = (MavStorm32GimbalManagerFlags)BitConverter.ToUInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            GimbalId = (byte)buffer[index++];
            Client = (MavStorm32GimbalManagerClient)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Pitch).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Yaw).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PitchRate).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(YawRate).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes((ushort)DeviceFlags).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes((ushort)ManagerFlags).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(GimbalId).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Client;index+=1;
            return index - start; // /*PayloadByteSize*/24;
        }

        /// <summary>
        /// Pitch/tilt angle (positive: tilt up, NaN to be ignored).
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw/pan angle (positive: pan the right, the frame is determined by the STORM32_GIMBAL_DEVICE_FLAGS_YAW_ABSOLUTE flag, NaN to be ignored).
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Pitch/tilt angular rate (positive: tilt up, NaN to be ignored).
        /// OriginName: pitch_rate, Units: rad/s, IsExtended: false
        /// </summary>
        public float PitchRate { get; set; }
        /// <summary>
        /// Yaw/pan angular rate (positive: pan to the right, the frame is determined by the STORM32_GIMBAL_DEVICE_FLAGS_YAW_ABSOLUTE flag, NaN to be ignored).
        /// OriginName: yaw_rate, Units: rad/s, IsExtended: false
        /// </summary>
        public float YawRate { get; set; }
        /// <summary>
        /// Gimbal device flags (UINT16_MAX to be ignored).
        /// OriginName: device_flags, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalDeviceFlags DeviceFlags { get; set; }
        /// <summary>
        /// Gimbal manager flags (0 to be ignored).
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
        /// Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals, send command multiple times for more than one but not all gimbals).
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
    public class Storm32GimbalManagerCorrectRollPacket: PacketV2<Storm32GimbalManagerCorrectRollPayload>
    {
	    public const int PacketMessageId = 60014;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 134;

        public override Storm32GimbalManagerCorrectRollPayload Payload { get; } = new Storm32GimbalManagerCorrectRollPayload();

        public override string Name => "STORM32_GIMBAL_MANAGER_CORRECT_ROLL";
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_CORRECT_ROLL
    /// </summary>
    public class Storm32GimbalManagerCorrectRollPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            Roll = BinSerialize.ReadFloat(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Client = (MavStorm32GimbalManagerClient)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Roll);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Client);index+=1;
            return index; // /*PayloadByteSize*/8;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            Roll = BitConverter.ToSingle(buffer, index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            GimbalId = (byte)buffer[index++];
            Client = (MavStorm32GimbalManagerClient)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Roll).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(GimbalId).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Client;index+=1;
            return index - start; // /*PayloadByteSize*/8;
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
        /// Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals, send command multiple times for more than one but not all gimbals).
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
    /// Message to set a gimbal manager profile. A gimbal device is never to react to this command. The selected profile is reported in the STORM32_GIMBAL_MANAGER_STATUS message.
    ///  STORM32_GIMBAL_MANAGER_PROFILE
    /// </summary>
    public class Storm32GimbalManagerProfilePacket: PacketV2<Storm32GimbalManagerProfilePayload>
    {
	    public const int PacketMessageId = 60015;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 78;

        public override Storm32GimbalManagerProfilePayload Payload { get; } = new Storm32GimbalManagerProfilePayload();

        public override string Name => "STORM32_GIMBAL_MANAGER_PROFILE";
    }

    /// <summary>
    ///  STORM32_GIMBAL_MANAGER_PROFILE
    /// </summary>
    public class Storm32GimbalManagerProfilePayload : IPayload
    {
        public byte GetMaxByteSize() => 22; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 22; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            GimbalId = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Profile = (MavStorm32GimbalManagerProfile)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/8 - Math.Max(0,((/*PayloadByteSize*/22 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Priorities = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Priorities[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }
            ProfileFlags = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            RcTimeout = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = 8;
            for(var i=0;i<arraySize;i++)
            {
                Timeouts[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)GimbalId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Profile);index+=1;
            for(var i=0;i<Priorities.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Priorities[i]);index+=1;
            }
            BinSerialize.WriteByte(ref buffer,(byte)ProfileFlags);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)RcTimeout);index+=1;
            for(var i=0;i<Timeouts.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Timeouts[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/22;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            GimbalId = (byte)buffer[index++];
            Profile = (MavStorm32GimbalManagerProfile)buffer[index++];
            arraySize = /*ArrayLength*/8 - Math.Max(0,((/*PayloadByteSize*/22 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Priorities = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Priorities[i] = (byte)buffer[index++];
            }
            ProfileFlags = (byte)buffer[index++];
            RcTimeout = (byte)buffer[index++];
            arraySize = 8;
            for(var i=0;i<arraySize;i++)
            {
                Timeouts[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(GimbalId).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Profile;index+=1;
            for(var i=0;i<Priorities.Length;i++)
            {
                buffer[index] = (byte)Priorities[i];index+=1;
            }
            BitConverter.GetBytes(ProfileFlags).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(RcTimeout).CopyTo(buffer, index);index+=1;
            for(var i=0;i<Timeouts.Length;i++)
            {
                buffer[index] = (byte)Timeouts[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/22;
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
        /// <summary>
        /// Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals, send command multiple times for more than one but not all gimbals).
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public byte GimbalId { get; set; }
        /// <summary>
        /// Profile to be applied (0 = default).
        /// OriginName: profile, Units: , IsExtended: false
        /// </summary>
        public MavStorm32GimbalManagerProfile Profile { get; set; }
        /// <summary>
        /// Priorities for custom profile.
        /// OriginName: priorities, Units: , IsExtended: false
        /// </summary>
        public byte[] Priorities { get; set; } = new byte[8];
        public byte GetPrioritiesMaxItemsCount() => 8;
        /// <summary>
        /// Profile flags for custom profile (0 = default).
        /// OriginName: profile_flags, Units: , IsExtended: false
        /// </summary>
        public byte ProfileFlags { get; set; }
        /// <summary>
        /// Rc timeouts for custom profile (0 = infinite, in uints of 100 ms).
        /// OriginName: rc_timeout, Units: , IsExtended: false
        /// </summary>
        public byte RcTimeout { get; set; }
        /// <summary>
        /// Timeouts for custom profile (0 = infinite, in uints of 100 ms).
        /// OriginName: timeouts, Units: , IsExtended: false
        /// </summary>
        public byte[] Timeouts { get; } = new byte[8];
    }
    /// <summary>
    /// Information about the shot operation.
    ///  QSHOT_STATUS
    /// </summary>
    public class QshotStatusPacket: PacketV2<QshotStatusPayload>
    {
	    public const int PacketMessageId = 60020;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 202;

        public override QshotStatusPayload Payload { get; } = new QshotStatusPayload();

        public override string Name => "QSHOT_STATUS";
    }

    /// <summary>
    ///  QSHOT_STATUS
    /// </summary>
    public class QshotStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 4; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 4; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            Mode = (MavQshotMode)BinSerialize.ReadUShort(ref buffer);index+=2;
            ShotState = BinSerialize.ReadUShort(ref buffer);index+=2;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUShort(ref buffer,(ushort)Mode);index+=2;
            BinSerialize.WriteUShort(ref buffer,ShotState);index+=2;
            return index; // /*PayloadByteSize*/4;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            Mode = (MavQshotMode)BitConverter.ToUInt16(buffer,index);index+=2;
            ShotState = BitConverter.ToUInt16(buffer,index);index+=2;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes((ushort)Mode).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(ShotState).CopyTo(buffer, index);index+=2;
            return index - start; // /*PayloadByteSize*/4;
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
    /// Message reporting the status of the prearm checks. The flags are component specific.
    ///  COMPONENT_PREARM_STATUS
    /// </summary>
    public class ComponentPrearmStatusPacket: PacketV2<ComponentPrearmStatusPayload>
    {
	    public const int PacketMessageId = 60025;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 20;

        public override ComponentPrearmStatusPayload Payload { get; } = new ComponentPrearmStatusPayload();

        public override string Name => "COMPONENT_PREARM_STATUS";
    }

    /// <summary>
    ///  COMPONENT_PREARM_STATUS
    /// </summary>
    public class ComponentPrearmStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 10; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 10; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            EnabledFlags = BinSerialize.ReadUInt(ref buffer);index+=4;
            FailFlags = BinSerialize.ReadUInt(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,EnabledFlags);index+=4;
            BinSerialize.WriteUInt(ref buffer,FailFlags);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            return index; // /*PayloadByteSize*/10;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            EnabledFlags = BitConverter.ToUInt32(buffer,index);index+=4;
            FailFlags = BitConverter.ToUInt32(buffer,index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(EnabledFlags).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(FailFlags).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/10;
        }

        /// <summary>
        /// Currently enabled prearm checks. 0 means no checks are being performed, UINT32_MAX means not known.
        /// OriginName: enabled_flags, Units: , IsExtended: false
        /// </summary>
        public uint EnabledFlags { get; set; }
        /// <summary>
        /// Currently not passed prearm checks. 0 means all checks have been passed.
        /// OriginName: fail_flags, Units: , IsExtended: false
        /// </summary>
        public uint FailFlags { get; set; }
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


#endregion


}

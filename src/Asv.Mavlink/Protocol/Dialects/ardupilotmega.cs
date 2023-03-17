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

namespace Asv.Mavlink.V2.Ardupilotmega
{

    public static class ArdupilotmegaHelper
    {
        public static void RegisterArdupilotmegaDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new SensorOffsetsPacket());
            src.Register(()=>new SetMagOffsetsPacket());
            src.Register(()=>new MeminfoPacket());
            src.Register(()=>new ApAdcPacket());
            src.Register(()=>new DigicamConfigurePacket());
            src.Register(()=>new DigicamControlPacket());
            src.Register(()=>new MountConfigurePacket());
            src.Register(()=>new MountControlPacket());
            src.Register(()=>new MountStatusPacket());
            src.Register(()=>new FencePointPacket());
            src.Register(()=>new FenceFetchPointPacket());
            src.Register(()=>new FenceStatusPacket());
            src.Register(()=>new AhrsPacket());
            src.Register(()=>new SimstatePacket());
            src.Register(()=>new HwstatusPacket());
            src.Register(()=>new RadioPacket());
            src.Register(()=>new LimitsStatusPacket());
            src.Register(()=>new WindPacket());
            src.Register(()=>new Data16Packet());
            src.Register(()=>new Data32Packet());
            src.Register(()=>new Data64Packet());
            src.Register(()=>new Data96Packet());
            src.Register(()=>new RangefinderPacket());
            src.Register(()=>new AirspeedAutocalPacket());
            src.Register(()=>new RallyPointPacket());
            src.Register(()=>new RallyFetchPointPacket());
            src.Register(()=>new CompassmotStatusPacket());
            src.Register(()=>new Ahrs2Packet());
            src.Register(()=>new CameraStatusPacket());
            src.Register(()=>new CameraFeedbackPacket());
            src.Register(()=>new Battery2Packet());
            src.Register(()=>new Ahrs3Packet());
            src.Register(()=>new AutopilotVersionRequestPacket());
            src.Register(()=>new RemoteLogDataBlockPacket());
            src.Register(()=>new RemoteLogBlockStatusPacket());
            src.Register(()=>new LedControlPacket());
            src.Register(()=>new MagCalProgressPacket());
            src.Register(()=>new MagCalReportPacket());
            src.Register(()=>new EkfStatusReportPacket());
            src.Register(()=>new PidTuningPacket());
            src.Register(()=>new DeepstallPacket());
            src.Register(()=>new GimbalReportPacket());
            src.Register(()=>new GimbalControlPacket());
            src.Register(()=>new GimbalTorqueCmdReportPacket());
            src.Register(()=>new GoproHeartbeatPacket());
            src.Register(()=>new GoproGetRequestPacket());
            src.Register(()=>new GoproGetResponsePacket());
            src.Register(()=>new GoproSetRequestPacket());
            src.Register(()=>new GoproSetResponsePacket());
            src.Register(()=>new RpmPacket());
            src.Register(()=>new DeviceOpReadPacket());
            src.Register(()=>new DeviceOpReadReplyPacket());
            src.Register(()=>new DeviceOpWritePacket());
            src.Register(()=>new DeviceOpWriteReplyPacket());
            src.Register(()=>new AdapTuningPacket());
            src.Register(()=>new VisionPositionDeltaPacket());
            src.Register(()=>new AoaSsaPacket());
            src.Register(()=>new EscTelemetry1To4Packet());
            src.Register(()=>new EscTelemetry5To8Packet());
            src.Register(()=>new EscTelemetry9To12Packet());
        }
    }

#region Enums

    /// <summary>
    ///  ACCELCAL_VEHICLE_POS
    /// </summary>
    public enum AccelcalVehiclePos:uint
    {
        /// <summary>
        /// ACCELCAL_VEHICLE_POS_LEVEL
        /// </summary>
        AccelcalVehiclePosLevel = 1,
        /// <summary>
        /// ACCELCAL_VEHICLE_POS_LEFT
        /// </summary>
        AccelcalVehiclePosLeft = 2,
        /// <summary>
        /// ACCELCAL_VEHICLE_POS_RIGHT
        /// </summary>
        AccelcalVehiclePosRight = 3,
        /// <summary>
        /// ACCELCAL_VEHICLE_POS_NOSEDOWN
        /// </summary>
        AccelcalVehiclePosNosedown = 4,
        /// <summary>
        /// ACCELCAL_VEHICLE_POS_NOSEUP
        /// </summary>
        AccelcalVehiclePosNoseup = 5,
        /// <summary>
        /// ACCELCAL_VEHICLE_POS_BACK
        /// </summary>
        AccelcalVehiclePosBack = 6,
        /// <summary>
        /// ACCELCAL_VEHICLE_POS_SUCCESS
        /// </summary>
        AccelcalVehiclePosSuccess = 16777215,
        /// <summary>
        /// ACCELCAL_VEHICLE_POS_FAILED
        /// </summary>
        AccelcalVehiclePosFailed = 16777216,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Mission command to operate EPM gripper.
        /// Param 1 - Gripper number (a number from 1 to max number of grippers on the vehicle).
        /// Param 2 - Gripper action (0=release, 1=grab. See GRIPPER_ACTIONS enum).
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_GRIPPER
        /// </summary>
        MavCmdDoGripper = 211,
        /// <summary>
        /// Enable/disable autotune.
        /// Param 1 - Enable (1: enable, 0:disable).
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_AUTOTUNE_ENABLE
        /// </summary>
        MavCmdDoAutotuneEnable = 212,
        /// <summary>
        /// Mission command to wait for an altitude or downwards vertical speed. This is meant for high altitude balloon launches, allowing the aircraft to be idle until either an altitude is reached or a negative vertical speed is reached (indicating early balloon burst). The wiggle time is how often to wiggle the control surfaces to prevent them seizing up.
        /// Param 1 - Altitude (m).
        /// Param 2 - Descent speed (m/s).
        /// Param 3 - Wiggle Time (s).
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_NAV_ALTITUDE_WAIT
        /// </summary>
        MavCmdNavAltitudeWait = 83,
        /// <summary>
        /// A system wide power-off event has been initiated.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_POWER_OFF_INITIATED
        /// </summary>
        MavCmdPowerOffInitiated = 42000,
        /// <summary>
        /// FLY button has been clicked.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_SOLO_BTN_FLY_CLICK
        /// </summary>
        MavCmdSoloBtnFlyClick = 42001,
        /// <summary>
        /// FLY button has been held for 1.5 seconds.
        /// Param 1 - Takeoff altitude.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_SOLO_BTN_FLY_HOLD
        /// </summary>
        MavCmdSoloBtnFlyHold = 42002,
        /// <summary>
        /// PAUSE button has been clicked.
        /// Param 1 - 1 if Solo is in a shot mode, 0 otherwise.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_SOLO_BTN_PAUSE_CLICK
        /// </summary>
        MavCmdSoloBtnPauseClick = 42003,
        /// <summary>
        /// Magnetometer calibration based on fixed position
        ///         in earth field given by inclination, declination and intensity.
        /// Param 1 - MagDeclinationDegrees.
        /// Param 2 - MagInclinationDegrees.
        /// Param 3 - MagIntensityMilliGauss.
        /// Param 4 - YawDegrees.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_FIXED_MAG_CAL
        /// </summary>
        MavCmdFixedMagCal = 42004,
        /// <summary>
        /// Magnetometer calibration based on fixed expected field values in milliGauss.
        /// Param 1 - FieldX.
        /// Param 2 - FieldY.
        /// Param 3 - FieldZ.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_FIXED_MAG_CAL_FIELD
        /// </summary>
        MavCmdFixedMagCalField = 42005,
        /// <summary>
        /// Initiate a magnetometer calibration.
        /// Param 1 - uint8_t bitmask of magnetometers (0 means all).
        /// Param 2 - Automatically retry on failure (0=no retry, 1=retry).
        /// Param 3 - Save without user input (0=require input, 1=autosave).
        /// Param 4 - Delay (seconds).
        /// Param 5 - Autoreboot (0=user reboot, 1=autoreboot).
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_START_MAG_CAL
        /// </summary>
        MavCmdDoStartMagCal = 42424,
        /// <summary>
        /// Initiate a magnetometer calibration.
        /// Param 1 - uint8_t bitmask of magnetometers (0 means all).
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_ACCEPT_MAG_CAL
        /// </summary>
        MavCmdDoAcceptMagCal = 42425,
        /// <summary>
        /// Cancel a running magnetometer calibration.
        /// Param 1 - uint8_t bitmask of magnetometers (0 means all).
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_CANCEL_MAG_CAL
        /// </summary>
        MavCmdDoCancelMagCal = 42426,
        /// <summary>
        /// Used when doing accelerometer calibration. When sent to the GCS tells it what position to put the vehicle in. When sent to the vehicle says what position the vehicle is in.
        /// Param 1 - Position, one of the ACCELCAL_VEHICLE_POS enum values.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_ACCELCAL_VEHICLE_POS
        /// </summary>
        MavCmdAccelcalVehiclePos = 42429,
        /// <summary>
        /// Reply with the version banner.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_SEND_BANNER
        /// </summary>
        MavCmdDoSendBanner = 42428,
        /// <summary>
        /// Command autopilot to get into factory test/diagnostic mode.
        /// Param 1 - 0 means get out of test mode, 1 means get into test mode.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_SET_FACTORY_TEST_MODE
        /// </summary>
        MavCmdSetFactoryTestMode = 42427,
        /// <summary>
        /// Causes the gimbal to reset and boot as if it was just powered on.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_GIMBAL_RESET
        /// </summary>
        MavCmdGimbalReset = 42501,
        /// <summary>
        /// Reports progress and success or failure of gimbal axis calibration procedure.
        /// Param 1 - Gimbal axis we're reporting calibration progress for.
        /// Param 2 - Current calibration progress for this axis, 0x64=100%.
        /// Param 3 - Status of the calibration.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_GIMBAL_AXIS_CALIBRATION_STATUS
        /// </summary>
        MavCmdGimbalAxisCalibrationStatus = 42502,
        /// <summary>
        /// Starts commutation calibration on the gimbal.
        /// Param 1 - Empty.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_GIMBAL_REQUEST_AXIS_CALIBRATION
        /// </summary>
        MavCmdGimbalRequestAxisCalibration = 42503,
        /// <summary>
        /// Erases gimbal application and parameters.
        /// Param 1 - Magic number.
        /// Param 2 - Magic number.
        /// Param 3 - Magic number.
        /// Param 4 - Magic number.
        /// Param 5 - Magic number.
        /// Param 6 - Magic number.
        /// Param 7 - Magic number.
        /// MAV_CMD_GIMBAL_FULL_RESET
        /// </summary>
        MavCmdGimbalFullReset = 42505,
        /// <summary>
        /// Command to operate winch.
        /// Param 1 - Winch number (0 for the default winch, otherwise a number from 1 to max number of winches on the vehicle).
        /// Param 2 - Action (0=relax, 1=relative length control, 2=rate control. See WINCH_ACTIONS enum.).
        /// Param 3 - Release length (cable distance to unwind in meters, negative numbers to wind in cable).
        /// Param 4 - Release rate (meters/second).
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_WINCH
        /// </summary>
        MavCmdDoWinch = 42600,
        /// <summary>
        /// Update the bootloader
        /// Param 1 - Empty
        /// Param 2 - Empty
        /// Param 3 - Empty
        /// Param 4 - Empty
        /// Param 5 - Magic number - set to 290876 to actually flash
        /// Param 6 - Empty
        /// Param 7 - Empty
        /// MAV_CMD_FLASH_BOOTLOADER
        /// </summary>
        MavCmdFlashBootloader = 42650,
    }

    /// <summary>
    ///  LIMITS_STATE
    /// </summary>
    public enum LimitsState:uint
    {
        /// <summary>
        /// Pre-initialization.
        /// LIMITS_INIT
        /// </summary>
        LimitsInit = 0,
        /// <summary>
        /// Disabled.
        /// LIMITS_DISABLED
        /// </summary>
        LimitsDisabled = 1,
        /// <summary>
        /// Checking limits.
        /// LIMITS_ENABLED
        /// </summary>
        LimitsEnabled = 2,
        /// <summary>
        /// A limit has been breached.
        /// LIMITS_TRIGGERED
        /// </summary>
        LimitsTriggered = 3,
        /// <summary>
        /// Taking action e.g. Return/RTL.
        /// LIMITS_RECOVERING
        /// </summary>
        LimitsRecovering = 4,
        /// <summary>
        /// We're no longer in breach of a limit.
        /// LIMITS_RECOVERED
        /// </summary>
        LimitsRecovered = 5,
    }

    /// <summary>
    ///  LIMIT_MODULE
    /// </summary>
    public enum LimitModule:uint
    {
        /// <summary>
        /// Pre-initialization.
        /// LIMIT_GPSLOCK
        /// </summary>
        LimitGpslock = 1,
        /// <summary>
        /// Disabled.
        /// LIMIT_GEOFENCE
        /// </summary>
        LimitGeofence = 2,
        /// <summary>
        /// Checking limits.
        /// LIMIT_ALTITUDE
        /// </summary>
        LimitAltitude = 4,
    }

    /// <summary>
    /// Flags in RALLY_POINT message.
    ///  RALLY_FLAGS
    /// </summary>
    public enum RallyFlags:uint
    {
        /// <summary>
        /// Flag set when requiring favorable winds for landing.
        /// FAVORABLE_WIND
        /// </summary>
        FavorableWind = 1,
        /// <summary>
        /// Flag set when plane is to immediately descend to break altitude and land without GCS intervention. Flag not set when plane is to loiter at Rally point until commanded to land.
        /// LAND_IMMEDIATELY
        /// </summary>
        LandImmediately = 2,
    }

    /// <summary>
    /// Gripper actions.
    ///  GRIPPER_ACTIONS
    /// </summary>
    public enum GripperActions:uint
    {
        /// <summary>
        /// Gripper release cargo.
        /// GRIPPER_ACTION_RELEASE
        /// </summary>
        GripperActionRelease = 0,
        /// <summary>
        /// Gripper grab onto cargo.
        /// GRIPPER_ACTION_GRAB
        /// </summary>
        GripperActionGrab = 1,
    }

    /// <summary>
    /// Winch actions.
    ///  WINCH_ACTIONS
    /// </summary>
    public enum WinchActions:uint
    {
        /// <summary>
        /// Relax winch.
        /// WINCH_RELAXED
        /// </summary>
        WinchRelaxed = 0,
        /// <summary>
        /// Winch unwinds or winds specified length of cable optionally using specified rate.
        /// WINCH_RELATIVE_LENGTH_CONTROL
        /// </summary>
        WinchRelativeLengthControl = 1,
        /// <summary>
        /// Winch unwinds or winds cable at specified rate in meters/seconds.
        /// WINCH_RATE_CONTROL
        /// </summary>
        WinchRateControl = 2,
    }

    /// <summary>
    ///  CAMERA_STATUS_TYPES
    /// </summary>
    public enum CameraStatusTypes:uint
    {
        /// <summary>
        /// Camera heartbeat, announce camera component ID at 1Hz.
        /// CAMERA_STATUS_TYPE_HEARTBEAT
        /// </summary>
        CameraStatusTypeHeartbeat = 0,
        /// <summary>
        /// Camera image triggered.
        /// CAMERA_STATUS_TYPE_TRIGGER
        /// </summary>
        CameraStatusTypeTrigger = 1,
        /// <summary>
        /// Camera connection lost.
        /// CAMERA_STATUS_TYPE_DISCONNECT
        /// </summary>
        CameraStatusTypeDisconnect = 2,
        /// <summary>
        /// Camera unknown error.
        /// CAMERA_STATUS_TYPE_ERROR
        /// </summary>
        CameraStatusTypeError = 3,
        /// <summary>
        /// Camera battery low. Parameter p1 shows reported voltage.
        /// CAMERA_STATUS_TYPE_LOWBATT
        /// </summary>
        CameraStatusTypeLowbatt = 4,
        /// <summary>
        /// Camera storage low. Parameter p1 shows reported shots remaining.
        /// CAMERA_STATUS_TYPE_LOWSTORE
        /// </summary>
        CameraStatusTypeLowstore = 5,
        /// <summary>
        /// Camera storage low. Parameter p1 shows reported video minutes remaining.
        /// CAMERA_STATUS_TYPE_LOWSTOREV
        /// </summary>
        CameraStatusTypeLowstorev = 6,
    }

    /// <summary>
    ///  CAMERA_FEEDBACK_FLAGS
    /// </summary>
    public enum CameraFeedbackFlags:uint
    {
        /// <summary>
        /// Shooting photos, not video.
        /// CAMERA_FEEDBACK_PHOTO
        /// </summary>
        CameraFeedbackPhoto = 0,
        /// <summary>
        /// Shooting video, not stills.
        /// CAMERA_FEEDBACK_VIDEO
        /// </summary>
        CameraFeedbackVideo = 1,
        /// <summary>
        /// Unable to achieve requested exposure (e.g. shutter speed too low).
        /// CAMERA_FEEDBACK_BADEXPOSURE
        /// </summary>
        CameraFeedbackBadexposure = 2,
        /// <summary>
        /// Closed loop feedback from camera, we know for sure it has successfully taken a picture.
        /// CAMERA_FEEDBACK_CLOSEDLOOP
        /// </summary>
        CameraFeedbackClosedloop = 3,
        /// <summary>
        /// Open loop camera, an image trigger has been requested but we can't know for sure it has successfully taken a picture.
        /// CAMERA_FEEDBACK_OPENLOOP
        /// </summary>
        CameraFeedbackOpenloop = 4,
    }

    /// <summary>
    ///  MAV_MODE_GIMBAL
    /// </summary>
    public enum MavModeGimbal:uint
    {
        /// <summary>
        /// Gimbal is powered on but has not started initializing yet.
        /// MAV_MODE_GIMBAL_UNINITIALIZED
        /// </summary>
        MavModeGimbalUninitialized = 0,
        /// <summary>
        /// Gimbal is currently running calibration on the pitch axis.
        /// MAV_MODE_GIMBAL_CALIBRATING_PITCH
        /// </summary>
        MavModeGimbalCalibratingPitch = 1,
        /// <summary>
        /// Gimbal is currently running calibration on the roll axis.
        /// MAV_MODE_GIMBAL_CALIBRATING_ROLL
        /// </summary>
        MavModeGimbalCalibratingRoll = 2,
        /// <summary>
        /// Gimbal is currently running calibration on the yaw axis.
        /// MAV_MODE_GIMBAL_CALIBRATING_YAW
        /// </summary>
        MavModeGimbalCalibratingYaw = 3,
        /// <summary>
        /// Gimbal has finished calibrating and initializing, but is relaxed pending reception of first rate command from copter.
        /// MAV_MODE_GIMBAL_INITIALIZED
        /// </summary>
        MavModeGimbalInitialized = 4,
        /// <summary>
        /// Gimbal is actively stabilizing.
        /// MAV_MODE_GIMBAL_ACTIVE
        /// </summary>
        MavModeGimbalActive = 5,
        /// <summary>
        /// Gimbal is relaxed because it missed more than 10 expected rate command messages in a row. Gimbal will move back to active mode when it receives a new rate command.
        /// MAV_MODE_GIMBAL_RATE_CMD_TIMEOUT
        /// </summary>
        MavModeGimbalRateCmdTimeout = 6,
    }

    /// <summary>
    ///  GIMBAL_AXIS
    /// </summary>
    public enum GimbalAxis:uint
    {
        /// <summary>
        /// Gimbal yaw axis.
        /// GIMBAL_AXIS_YAW
        /// </summary>
        GimbalAxisYaw = 0,
        /// <summary>
        /// Gimbal pitch axis.
        /// GIMBAL_AXIS_PITCH
        /// </summary>
        GimbalAxisPitch = 1,
        /// <summary>
        /// Gimbal roll axis.
        /// GIMBAL_AXIS_ROLL
        /// </summary>
        GimbalAxisRoll = 2,
    }

    /// <summary>
    ///  GIMBAL_AXIS_CALIBRATION_STATUS
    /// </summary>
    public enum GimbalAxisCalibrationStatus:uint
    {
        /// <summary>
        /// Axis calibration is in progress.
        /// GIMBAL_AXIS_CALIBRATION_STATUS_IN_PROGRESS
        /// </summary>
        GimbalAxisCalibrationStatusInProgress = 0,
        /// <summary>
        /// Axis calibration succeeded.
        /// GIMBAL_AXIS_CALIBRATION_STATUS_SUCCEEDED
        /// </summary>
        GimbalAxisCalibrationStatusSucceeded = 1,
        /// <summary>
        /// Axis calibration failed.
        /// GIMBAL_AXIS_CALIBRATION_STATUS_FAILED
        /// </summary>
        GimbalAxisCalibrationStatusFailed = 2,
    }

    /// <summary>
    ///  GIMBAL_AXIS_CALIBRATION_REQUIRED
    /// </summary>
    public enum GimbalAxisCalibrationRequired:uint
    {
        /// <summary>
        /// Whether or not this axis requires calibration is unknown at this time.
        /// GIMBAL_AXIS_CALIBRATION_REQUIRED_UNKNOWN
        /// </summary>
        GimbalAxisCalibrationRequiredUnknown = 0,
        /// <summary>
        /// This axis requires calibration.
        /// GIMBAL_AXIS_CALIBRATION_REQUIRED_TRUE
        /// </summary>
        GimbalAxisCalibrationRequiredTrue = 1,
        /// <summary>
        /// This axis does not require calibration.
        /// GIMBAL_AXIS_CALIBRATION_REQUIRED_FALSE
        /// </summary>
        GimbalAxisCalibrationRequiredFalse = 2,
    }

    /// <summary>
    ///  GOPRO_HEARTBEAT_STATUS
    /// </summary>
    public enum GoproHeartbeatStatus:uint
    {
        /// <summary>
        /// No GoPro connected.
        /// GOPRO_HEARTBEAT_STATUS_DISCONNECTED
        /// </summary>
        GoproHeartbeatStatusDisconnected = 0,
        /// <summary>
        /// The detected GoPro is not HeroBus compatible.
        /// GOPRO_HEARTBEAT_STATUS_INCOMPATIBLE
        /// </summary>
        GoproHeartbeatStatusIncompatible = 1,
        /// <summary>
        /// A HeroBus compatible GoPro is connected.
        /// GOPRO_HEARTBEAT_STATUS_CONNECTED
        /// </summary>
        GoproHeartbeatStatusConnected = 2,
        /// <summary>
        /// An unrecoverable error was encountered with the connected GoPro, it may require a power cycle.
        /// GOPRO_HEARTBEAT_STATUS_ERROR
        /// </summary>
        GoproHeartbeatStatusError = 3,
    }

    /// <summary>
    ///  GOPRO_HEARTBEAT_FLAGS
    /// </summary>
    public enum GoproHeartbeatFlags:uint
    {
        /// <summary>
        /// GoPro is currently recording.
        /// GOPRO_FLAG_RECORDING
        /// </summary>
        GoproFlagRecording = 1,
    }

    /// <summary>
    ///  GOPRO_REQUEST_STATUS
    /// </summary>
    public enum GoproRequestStatus:uint
    {
        /// <summary>
        /// The write message with ID indicated succeeded.
        /// GOPRO_REQUEST_SUCCESS
        /// </summary>
        GoproRequestSuccess = 0,
        /// <summary>
        /// The write message with ID indicated failed.
        /// GOPRO_REQUEST_FAILED
        /// </summary>
        GoproRequestFailed = 1,
    }

    /// <summary>
    ///  GOPRO_COMMAND
    /// </summary>
    public enum GoproCommand:uint
    {
        /// <summary>
        /// (Get/Set).
        /// GOPRO_COMMAND_POWER
        /// </summary>
        GoproCommandPower = 0,
        /// <summary>
        /// (Get/Set).
        /// GOPRO_COMMAND_CAPTURE_MODE
        /// </summary>
        GoproCommandCaptureMode = 1,
        /// <summary>
        /// (___/Set).
        /// GOPRO_COMMAND_SHUTTER
        /// </summary>
        GoproCommandShutter = 2,
        /// <summary>
        /// (Get/___).
        /// GOPRO_COMMAND_BATTERY
        /// </summary>
        GoproCommandBattery = 3,
        /// <summary>
        /// (Get/___).
        /// GOPRO_COMMAND_MODEL
        /// </summary>
        GoproCommandModel = 4,
        /// <summary>
        /// (Get/Set).
        /// GOPRO_COMMAND_VIDEO_SETTINGS
        /// </summary>
        GoproCommandVideoSettings = 5,
        /// <summary>
        /// (Get/Set).
        /// GOPRO_COMMAND_LOW_LIGHT
        /// </summary>
        GoproCommandLowLight = 6,
        /// <summary>
        /// (Get/Set).
        /// GOPRO_COMMAND_PHOTO_RESOLUTION
        /// </summary>
        GoproCommandPhotoResolution = 7,
        /// <summary>
        /// (Get/Set).
        /// GOPRO_COMMAND_PHOTO_BURST_RATE
        /// </summary>
        GoproCommandPhotoBurstRate = 8,
        /// <summary>
        /// (Get/Set).
        /// GOPRO_COMMAND_PROTUNE
        /// </summary>
        GoproCommandProtune = 9,
        /// <summary>
        /// (Get/Set) Hero 3+ Only.
        /// GOPRO_COMMAND_PROTUNE_WHITE_BALANCE
        /// </summary>
        GoproCommandProtuneWhiteBalance = 10,
        /// <summary>
        /// (Get/Set) Hero 3+ Only.
        /// GOPRO_COMMAND_PROTUNE_COLOUR
        /// </summary>
        GoproCommandProtuneColour = 11,
        /// <summary>
        /// (Get/Set) Hero 3+ Only.
        /// GOPRO_COMMAND_PROTUNE_GAIN
        /// </summary>
        GoproCommandProtuneGain = 12,
        /// <summary>
        /// (Get/Set) Hero 3+ Only.
        /// GOPRO_COMMAND_PROTUNE_SHARPNESS
        /// </summary>
        GoproCommandProtuneSharpness = 13,
        /// <summary>
        /// (Get/Set) Hero 3+ Only.
        /// GOPRO_COMMAND_PROTUNE_EXPOSURE
        /// </summary>
        GoproCommandProtuneExposure = 14,
        /// <summary>
        /// (Get/Set).
        /// GOPRO_COMMAND_TIME
        /// </summary>
        GoproCommandTime = 15,
        /// <summary>
        /// (Get/Set).
        /// GOPRO_COMMAND_CHARGING
        /// </summary>
        GoproCommandCharging = 16,
    }

    /// <summary>
    ///  GOPRO_CAPTURE_MODE
    /// </summary>
    public enum GoproCaptureMode:uint
    {
        /// <summary>
        /// Video mode.
        /// GOPRO_CAPTURE_MODE_VIDEO
        /// </summary>
        GoproCaptureModeVideo = 0,
        /// <summary>
        /// Photo mode.
        /// GOPRO_CAPTURE_MODE_PHOTO
        /// </summary>
        GoproCaptureModePhoto = 1,
        /// <summary>
        /// Burst mode, Hero 3+ only.
        /// GOPRO_CAPTURE_MODE_BURST
        /// </summary>
        GoproCaptureModeBurst = 2,
        /// <summary>
        /// Time lapse mode, Hero 3+ only.
        /// GOPRO_CAPTURE_MODE_TIME_LAPSE
        /// </summary>
        GoproCaptureModeTimeLapse = 3,
        /// <summary>
        /// Multi shot mode, Hero 4 only.
        /// GOPRO_CAPTURE_MODE_MULTI_SHOT
        /// </summary>
        GoproCaptureModeMultiShot = 4,
        /// <summary>
        /// Playback mode, Hero 4 only, silver only except when LCD or HDMI is connected to black.
        /// GOPRO_CAPTURE_MODE_PLAYBACK
        /// </summary>
        GoproCaptureModePlayback = 5,
        /// <summary>
        /// Playback mode, Hero 4 only.
        /// GOPRO_CAPTURE_MODE_SETUP
        /// </summary>
        GoproCaptureModeSetup = 6,
        /// <summary>
        /// Mode not yet known.
        /// GOPRO_CAPTURE_MODE_UNKNOWN
        /// </summary>
        GoproCaptureModeUnknown = 255,
    }

    /// <summary>
    ///  GOPRO_RESOLUTION
    /// </summary>
    public enum GoproResolution:uint
    {
        /// <summary>
        /// 848 x 480 (480p).
        /// GOPRO_RESOLUTION_480p
        /// </summary>
        GoproResolution480p = 0,
        /// <summary>
        /// 1280 x 720 (720p).
        /// GOPRO_RESOLUTION_720p
        /// </summary>
        GoproResolution720p = 1,
        /// <summary>
        /// 1280 x 960 (960p).
        /// GOPRO_RESOLUTION_960p
        /// </summary>
        GoproResolution960p = 2,
        /// <summary>
        /// 1920 x 1080 (1080p).
        /// GOPRO_RESOLUTION_1080p
        /// </summary>
        GoproResolution1080p = 3,
        /// <summary>
        /// 1920 x 1440 (1440p).
        /// GOPRO_RESOLUTION_1440p
        /// </summary>
        GoproResolution1440p = 4,
        /// <summary>
        /// 2704 x 1440 (2.7k-17:9).
        /// GOPRO_RESOLUTION_2_7k_17_9
        /// </summary>
        GoproResolution27k179 = 5,
        /// <summary>
        /// 2704 x 1524 (2.7k-16:9).
        /// GOPRO_RESOLUTION_2_7k_16_9
        /// </summary>
        GoproResolution27k169 = 6,
        /// <summary>
        /// 2704 x 2028 (2.7k-4:3).
        /// GOPRO_RESOLUTION_2_7k_4_3
        /// </summary>
        GoproResolution27k43 = 7,
        /// <summary>
        /// 3840 x 2160 (4k-16:9).
        /// GOPRO_RESOLUTION_4k_16_9
        /// </summary>
        GoproResolution4k169 = 8,
        /// <summary>
        /// 4096 x 2160 (4k-17:9).
        /// GOPRO_RESOLUTION_4k_17_9
        /// </summary>
        GoproResolution4k179 = 9,
        /// <summary>
        /// 1280 x 720 (720p-SuperView).
        /// GOPRO_RESOLUTION_720p_SUPERVIEW
        /// </summary>
        GoproResolution720pSuperview = 10,
        /// <summary>
        /// 1920 x 1080 (1080p-SuperView).
        /// GOPRO_RESOLUTION_1080p_SUPERVIEW
        /// </summary>
        GoproResolution1080pSuperview = 11,
        /// <summary>
        /// 2704 x 1520 (2.7k-SuperView).
        /// GOPRO_RESOLUTION_2_7k_SUPERVIEW
        /// </summary>
        GoproResolution27kSuperview = 12,
        /// <summary>
        /// 3840 x 2160 (4k-SuperView).
        /// GOPRO_RESOLUTION_4k_SUPERVIEW
        /// </summary>
        GoproResolution4kSuperview = 13,
    }

    /// <summary>
    ///  GOPRO_FRAME_RATE
    /// </summary>
    public enum GoproFrameRate:uint
    {
        /// <summary>
        /// 12 FPS.
        /// GOPRO_FRAME_RATE_12
        /// </summary>
        GoproFrameRate12 = 0,
        /// <summary>
        /// 15 FPS.
        /// GOPRO_FRAME_RATE_15
        /// </summary>
        GoproFrameRate15 = 1,
        /// <summary>
        /// 24 FPS.
        /// GOPRO_FRAME_RATE_24
        /// </summary>
        GoproFrameRate24 = 2,
        /// <summary>
        /// 25 FPS.
        /// GOPRO_FRAME_RATE_25
        /// </summary>
        GoproFrameRate25 = 3,
        /// <summary>
        /// 30 FPS.
        /// GOPRO_FRAME_RATE_30
        /// </summary>
        GoproFrameRate30 = 4,
        /// <summary>
        /// 48 FPS.
        /// GOPRO_FRAME_RATE_48
        /// </summary>
        GoproFrameRate48 = 5,
        /// <summary>
        /// 50 FPS.
        /// GOPRO_FRAME_RATE_50
        /// </summary>
        GoproFrameRate50 = 6,
        /// <summary>
        /// 60 FPS.
        /// GOPRO_FRAME_RATE_60
        /// </summary>
        GoproFrameRate60 = 7,
        /// <summary>
        /// 80 FPS.
        /// GOPRO_FRAME_RATE_80
        /// </summary>
        GoproFrameRate80 = 8,
        /// <summary>
        /// 90 FPS.
        /// GOPRO_FRAME_RATE_90
        /// </summary>
        GoproFrameRate90 = 9,
        /// <summary>
        /// 100 FPS.
        /// GOPRO_FRAME_RATE_100
        /// </summary>
        GoproFrameRate100 = 10,
        /// <summary>
        /// 120 FPS.
        /// GOPRO_FRAME_RATE_120
        /// </summary>
        GoproFrameRate120 = 11,
        /// <summary>
        /// 240 FPS.
        /// GOPRO_FRAME_RATE_240
        /// </summary>
        GoproFrameRate240 = 12,
        /// <summary>
        /// 12.5 FPS.
        /// GOPRO_FRAME_RATE_12_5
        /// </summary>
        GoproFrameRate125 = 13,
    }

    /// <summary>
    ///  GOPRO_FIELD_OF_VIEW
    /// </summary>
    public enum GoproFieldOfView:uint
    {
        /// <summary>
        /// 0x00: Wide.
        /// GOPRO_FIELD_OF_VIEW_WIDE
        /// </summary>
        GoproFieldOfViewWide = 0,
        /// <summary>
        /// 0x01: Medium.
        /// GOPRO_FIELD_OF_VIEW_MEDIUM
        /// </summary>
        GoproFieldOfViewMedium = 1,
        /// <summary>
        /// 0x02: Narrow.
        /// GOPRO_FIELD_OF_VIEW_NARROW
        /// </summary>
        GoproFieldOfViewNarrow = 2,
    }

    /// <summary>
    ///  GOPRO_VIDEO_SETTINGS_FLAGS
    /// </summary>
    public enum GoproVideoSettingsFlags:uint
    {
        /// <summary>
        /// 0=NTSC, 1=PAL.
        /// GOPRO_VIDEO_SETTINGS_TV_MODE
        /// </summary>
        GoproVideoSettingsTvMode = 1,
    }

    /// <summary>
    ///  GOPRO_PHOTO_RESOLUTION
    /// </summary>
    public enum GoproPhotoResolution:uint
    {
        /// <summary>
        /// 5MP Medium.
        /// GOPRO_PHOTO_RESOLUTION_5MP_MEDIUM
        /// </summary>
        GoproPhotoResolution5mpMedium = 0,
        /// <summary>
        /// 7MP Medium.
        /// GOPRO_PHOTO_RESOLUTION_7MP_MEDIUM
        /// </summary>
        GoproPhotoResolution7mpMedium = 1,
        /// <summary>
        /// 7MP Wide.
        /// GOPRO_PHOTO_RESOLUTION_7MP_WIDE
        /// </summary>
        GoproPhotoResolution7mpWide = 2,
        /// <summary>
        /// 10MP Wide.
        /// GOPRO_PHOTO_RESOLUTION_10MP_WIDE
        /// </summary>
        GoproPhotoResolution10mpWide = 3,
        /// <summary>
        /// 12MP Wide.
        /// GOPRO_PHOTO_RESOLUTION_12MP_WIDE
        /// </summary>
        GoproPhotoResolution12mpWide = 4,
    }

    /// <summary>
    ///  GOPRO_PROTUNE_WHITE_BALANCE
    /// </summary>
    public enum GoproProtuneWhiteBalance:uint
    {
        /// <summary>
        /// Auto.
        /// GOPRO_PROTUNE_WHITE_BALANCE_AUTO
        /// </summary>
        GoproProtuneWhiteBalanceAuto = 0,
        /// <summary>
        /// 3000K.
        /// GOPRO_PROTUNE_WHITE_BALANCE_3000K
        /// </summary>
        GoproProtuneWhiteBalance3000k = 1,
        /// <summary>
        /// 5500K.
        /// GOPRO_PROTUNE_WHITE_BALANCE_5500K
        /// </summary>
        GoproProtuneWhiteBalance5500k = 2,
        /// <summary>
        /// 6500K.
        /// GOPRO_PROTUNE_WHITE_BALANCE_6500K
        /// </summary>
        GoproProtuneWhiteBalance6500k = 3,
        /// <summary>
        /// Camera Raw.
        /// GOPRO_PROTUNE_WHITE_BALANCE_RAW
        /// </summary>
        GoproProtuneWhiteBalanceRaw = 4,
    }

    /// <summary>
    ///  GOPRO_PROTUNE_COLOUR
    /// </summary>
    public enum GoproProtuneColour:uint
    {
        /// <summary>
        /// Auto.
        /// GOPRO_PROTUNE_COLOUR_STANDARD
        /// </summary>
        GoproProtuneColourStandard = 0,
        /// <summary>
        /// Neutral.
        /// GOPRO_PROTUNE_COLOUR_NEUTRAL
        /// </summary>
        GoproProtuneColourNeutral = 1,
    }

    /// <summary>
    ///  GOPRO_PROTUNE_GAIN
    /// </summary>
    public enum GoproProtuneGain:uint
    {
        /// <summary>
        /// ISO 400.
        /// GOPRO_PROTUNE_GAIN_400
        /// </summary>
        GoproProtuneGain400 = 0,
        /// <summary>
        /// ISO 800 (Only Hero 4).
        /// GOPRO_PROTUNE_GAIN_800
        /// </summary>
        GoproProtuneGain800 = 1,
        /// <summary>
        /// ISO 1600.
        /// GOPRO_PROTUNE_GAIN_1600
        /// </summary>
        GoproProtuneGain1600 = 2,
        /// <summary>
        /// ISO 3200 (Only Hero 4).
        /// GOPRO_PROTUNE_GAIN_3200
        /// </summary>
        GoproProtuneGain3200 = 3,
        /// <summary>
        /// ISO 6400.
        /// GOPRO_PROTUNE_GAIN_6400
        /// </summary>
        GoproProtuneGain6400 = 4,
    }

    /// <summary>
    ///  GOPRO_PROTUNE_SHARPNESS
    /// </summary>
    public enum GoproProtuneSharpness:uint
    {
        /// <summary>
        /// Low Sharpness.
        /// GOPRO_PROTUNE_SHARPNESS_LOW
        /// </summary>
        GoproProtuneSharpnessLow = 0,
        /// <summary>
        /// Medium Sharpness.
        /// GOPRO_PROTUNE_SHARPNESS_MEDIUM
        /// </summary>
        GoproProtuneSharpnessMedium = 1,
        /// <summary>
        /// High Sharpness.
        /// GOPRO_PROTUNE_SHARPNESS_HIGH
        /// </summary>
        GoproProtuneSharpnessHigh = 2,
    }

    /// <summary>
    ///  GOPRO_PROTUNE_EXPOSURE
    /// </summary>
    public enum GoproProtuneExposure:uint
    {
        /// <summary>
        /// -5.0 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_NEG_5_0
        /// </summary>
        GoproProtuneExposureNeg50 = 0,
        /// <summary>
        /// -4.5 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_NEG_4_5
        /// </summary>
        GoproProtuneExposureNeg45 = 1,
        /// <summary>
        /// -4.0 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_NEG_4_0
        /// </summary>
        GoproProtuneExposureNeg40 = 2,
        /// <summary>
        /// -3.5 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_NEG_3_5
        /// </summary>
        GoproProtuneExposureNeg35 = 3,
        /// <summary>
        /// -3.0 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_NEG_3_0
        /// </summary>
        GoproProtuneExposureNeg30 = 4,
        /// <summary>
        /// -2.5 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_NEG_2_5
        /// </summary>
        GoproProtuneExposureNeg25 = 5,
        /// <summary>
        /// -2.0 EV.
        /// GOPRO_PROTUNE_EXPOSURE_NEG_2_0
        /// </summary>
        GoproProtuneExposureNeg20 = 6,
        /// <summary>
        /// -1.5 EV.
        /// GOPRO_PROTUNE_EXPOSURE_NEG_1_5
        /// </summary>
        GoproProtuneExposureNeg15 = 7,
        /// <summary>
        /// -1.0 EV.
        /// GOPRO_PROTUNE_EXPOSURE_NEG_1_0
        /// </summary>
        GoproProtuneExposureNeg10 = 8,
        /// <summary>
        /// -0.5 EV.
        /// GOPRO_PROTUNE_EXPOSURE_NEG_0_5
        /// </summary>
        GoproProtuneExposureNeg05 = 9,
        /// <summary>
        /// 0.0 EV.
        /// GOPRO_PROTUNE_EXPOSURE_ZERO
        /// </summary>
        GoproProtuneExposureZero = 10,
        /// <summary>
        /// +0.5 EV.
        /// GOPRO_PROTUNE_EXPOSURE_POS_0_5
        /// </summary>
        GoproProtuneExposurePos05 = 11,
        /// <summary>
        /// +1.0 EV.
        /// GOPRO_PROTUNE_EXPOSURE_POS_1_0
        /// </summary>
        GoproProtuneExposurePos10 = 12,
        /// <summary>
        /// +1.5 EV.
        /// GOPRO_PROTUNE_EXPOSURE_POS_1_5
        /// </summary>
        GoproProtuneExposurePos15 = 13,
        /// <summary>
        /// +2.0 EV.
        /// GOPRO_PROTUNE_EXPOSURE_POS_2_0
        /// </summary>
        GoproProtuneExposurePos20 = 14,
        /// <summary>
        /// +2.5 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_POS_2_5
        /// </summary>
        GoproProtuneExposurePos25 = 15,
        /// <summary>
        /// +3.0 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_POS_3_0
        /// </summary>
        GoproProtuneExposurePos30 = 16,
        /// <summary>
        /// +3.5 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_POS_3_5
        /// </summary>
        GoproProtuneExposurePos35 = 17,
        /// <summary>
        /// +4.0 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_POS_4_0
        /// </summary>
        GoproProtuneExposurePos40 = 18,
        /// <summary>
        /// +4.5 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_POS_4_5
        /// </summary>
        GoproProtuneExposurePos45 = 19,
        /// <summary>
        /// +5.0 EV (Hero 3+ Only).
        /// GOPRO_PROTUNE_EXPOSURE_POS_5_0
        /// </summary>
        GoproProtuneExposurePos50 = 20,
    }

    /// <summary>
    ///  GOPRO_CHARGING
    /// </summary>
    public enum GoproCharging:uint
    {
        /// <summary>
        /// Charging disabled.
        /// GOPRO_CHARGING_DISABLED
        /// </summary>
        GoproChargingDisabled = 0,
        /// <summary>
        /// Charging enabled.
        /// GOPRO_CHARGING_ENABLED
        /// </summary>
        GoproChargingEnabled = 1,
    }

    /// <summary>
    ///  GOPRO_MODEL
    /// </summary>
    public enum GoproModel:uint
    {
        /// <summary>
        /// Unknown gopro model.
        /// GOPRO_MODEL_UNKNOWN
        /// </summary>
        GoproModelUnknown = 0,
        /// <summary>
        /// Hero 3+ Silver (HeroBus not supported by GoPro).
        /// GOPRO_MODEL_HERO_3_PLUS_SILVER
        /// </summary>
        GoproModelHero3PlusSilver = 1,
        /// <summary>
        /// Hero 3+ Black.
        /// GOPRO_MODEL_HERO_3_PLUS_BLACK
        /// </summary>
        GoproModelHero3PlusBlack = 2,
        /// <summary>
        /// Hero 4 Silver.
        /// GOPRO_MODEL_HERO_4_SILVER
        /// </summary>
        GoproModelHero4Silver = 3,
        /// <summary>
        /// Hero 4 Black.
        /// GOPRO_MODEL_HERO_4_BLACK
        /// </summary>
        GoproModelHero4Black = 4,
    }

    /// <summary>
    ///  GOPRO_BURST_RATE
    /// </summary>
    public enum GoproBurstRate:uint
    {
        /// <summary>
        /// 3 Shots / 1 Second.
        /// GOPRO_BURST_RATE_3_IN_1_SECOND
        /// </summary>
        GoproBurstRate3In1Second = 0,
        /// <summary>
        /// 5 Shots / 1 Second.
        /// GOPRO_BURST_RATE_5_IN_1_SECOND
        /// </summary>
        GoproBurstRate5In1Second = 1,
        /// <summary>
        /// 10 Shots / 1 Second.
        /// GOPRO_BURST_RATE_10_IN_1_SECOND
        /// </summary>
        GoproBurstRate10In1Second = 2,
        /// <summary>
        /// 10 Shots / 2 Second.
        /// GOPRO_BURST_RATE_10_IN_2_SECOND
        /// </summary>
        GoproBurstRate10In2Second = 3,
        /// <summary>
        /// 10 Shots / 3 Second (Hero 4 Only).
        /// GOPRO_BURST_RATE_10_IN_3_SECOND
        /// </summary>
        GoproBurstRate10In3Second = 4,
        /// <summary>
        /// 30 Shots / 1 Second.
        /// GOPRO_BURST_RATE_30_IN_1_SECOND
        /// </summary>
        GoproBurstRate30In1Second = 5,
        /// <summary>
        /// 30 Shots / 2 Second.
        /// GOPRO_BURST_RATE_30_IN_2_SECOND
        /// </summary>
        GoproBurstRate30In2Second = 6,
        /// <summary>
        /// 30 Shots / 3 Second.
        /// GOPRO_BURST_RATE_30_IN_3_SECOND
        /// </summary>
        GoproBurstRate30In3Second = 7,
        /// <summary>
        /// 30 Shots / 6 Second.
        /// GOPRO_BURST_RATE_30_IN_6_SECOND
        /// </summary>
        GoproBurstRate30In6Second = 8,
    }

    /// <summary>
    ///  LED_CONTROL_PATTERN
    /// </summary>
    public enum LedControlPattern:uint
    {
        /// <summary>
        /// LED patterns off (return control to regular vehicle control).
        /// LED_CONTROL_PATTERN_OFF
        /// </summary>
        LedControlPatternOff = 0,
        /// <summary>
        /// LEDs show pattern during firmware update.
        /// LED_CONTROL_PATTERN_FIRMWAREUPDATE
        /// </summary>
        LedControlPatternFirmwareupdate = 1,
        /// <summary>
        /// Custom Pattern using custom bytes fields.
        /// LED_CONTROL_PATTERN_CUSTOM
        /// </summary>
        LedControlPatternCustom = 255,
    }

    /// <summary>
    /// Flags in EKF_STATUS message.
    ///  EKF_STATUS_FLAGS
    /// </summary>
    public enum EkfStatusFlags:uint
    {
        /// <summary>
        /// Set if EKF's attitude estimate is good.
        /// EKF_ATTITUDE
        /// </summary>
        EkfAttitude = 1,
        /// <summary>
        /// Set if EKF's horizontal velocity estimate is good.
        /// EKF_VELOCITY_HORIZ
        /// </summary>
        EkfVelocityHoriz = 2,
        /// <summary>
        /// Set if EKF's vertical velocity estimate is good.
        /// EKF_VELOCITY_VERT
        /// </summary>
        EkfVelocityVert = 4,
        /// <summary>
        /// Set if EKF's horizontal position (relative) estimate is good.
        /// EKF_POS_HORIZ_REL
        /// </summary>
        EkfPosHorizRel = 8,
        /// <summary>
        /// Set if EKF's horizontal position (absolute) estimate is good.
        /// EKF_POS_HORIZ_ABS
        /// </summary>
        EkfPosHorizAbs = 16,
        /// <summary>
        /// Set if EKF's vertical position (absolute) estimate is good.
        /// EKF_POS_VERT_ABS
        /// </summary>
        EkfPosVertAbs = 32,
        /// <summary>
        /// Set if EKF's vertical position (above ground) estimate is good.
        /// EKF_POS_VERT_AGL
        /// </summary>
        EkfPosVertAgl = 64,
        /// <summary>
        /// EKF is in constant position mode and does not know it's absolute or relative position.
        /// EKF_CONST_POS_MODE
        /// </summary>
        EkfConstPosMode = 128,
        /// <summary>
        /// Set if EKF's predicted horizontal position (relative) estimate is good.
        /// EKF_PRED_POS_HORIZ_REL
        /// </summary>
        EkfPredPosHorizRel = 256,
        /// <summary>
        /// Set if EKF's predicted horizontal position (absolute) estimate is good.
        /// EKF_PRED_POS_HORIZ_ABS
        /// </summary>
        EkfPredPosHorizAbs = 512,
    }

    /// <summary>
    ///  PID_TUNING_AXIS
    /// </summary>
    public enum PidTuningAxis:uint
    {
        /// <summary>
        /// PID_TUNING_ROLL
        /// </summary>
        PidTuningRoll = 1,
        /// <summary>
        /// PID_TUNING_PITCH
        /// </summary>
        PidTuningPitch = 2,
        /// <summary>
        /// PID_TUNING_YAW
        /// </summary>
        PidTuningYaw = 3,
        /// <summary>
        /// PID_TUNING_ACCZ
        /// </summary>
        PidTuningAccz = 4,
        /// <summary>
        /// PID_TUNING_STEER
        /// </summary>
        PidTuningSteer = 5,
        /// <summary>
        /// PID_TUNING_LANDING
        /// </summary>
        PidTuningLanding = 6,
    }

    /// <summary>
    ///  MAG_CAL_STATUS
    /// </summary>
    public enum MagCalStatus:uint
    {
        /// <summary>
        /// MAG_CAL_NOT_STARTED
        /// </summary>
        MagCalNotStarted = 0,
        /// <summary>
        /// MAG_CAL_WAITING_TO_START
        /// </summary>
        MagCalWaitingToStart = 1,
        /// <summary>
        /// MAG_CAL_RUNNING_STEP_ONE
        /// </summary>
        MagCalRunningStepOne = 2,
        /// <summary>
        /// MAG_CAL_RUNNING_STEP_TWO
        /// </summary>
        MagCalRunningStepTwo = 3,
        /// <summary>
        /// MAG_CAL_SUCCESS
        /// </summary>
        MagCalSuccess = 4,
        /// <summary>
        /// MAG_CAL_FAILED
        /// </summary>
        MagCalFailed = 5,
        /// <summary>
        /// MAG_CAL_BAD_ORIENTATION
        /// </summary>
        MagCalBadOrientation = 6,
    }

    /// <summary>
    /// Special ACK block numbers control activation of dataflash log streaming.
    ///  MAV_REMOTE_LOG_DATA_BLOCK_COMMANDS
    /// </summary>
    public enum MavRemoteLogDataBlockCommands:uint
    {
        /// <summary>
        /// UAV to stop sending DataFlash blocks.
        /// MAV_REMOTE_LOG_DATA_BLOCK_STOP
        /// </summary>
        MavRemoteLogDataBlockStop = 2147483645,
        /// <summary>
        /// UAV to start sending DataFlash blocks.
        /// MAV_REMOTE_LOG_DATA_BLOCK_START
        /// </summary>
        MavRemoteLogDataBlockStart = 2147483646,
    }

    /// <summary>
    /// Possible remote log data block statuses.
    ///  MAV_REMOTE_LOG_DATA_BLOCK_STATUSES
    /// </summary>
    public enum MavRemoteLogDataBlockStatuses:uint
    {
        /// <summary>
        /// This block has NOT been received.
        /// MAV_REMOTE_LOG_DATA_BLOCK_NACK
        /// </summary>
        MavRemoteLogDataBlockNack = 0,
        /// <summary>
        /// This block has been received.
        /// MAV_REMOTE_LOG_DATA_BLOCK_ACK
        /// </summary>
        MavRemoteLogDataBlockAck = 1,
    }

    /// <summary>
    /// Bus types for device operations.
    ///  DEVICE_OP_BUSTYPE
    /// </summary>
    public enum DeviceOpBustype:uint
    {
        /// <summary>
        /// I2C Device operation.
        /// DEVICE_OP_BUSTYPE_I2C
        /// </summary>
        DeviceOpBustypeI2c = 0,
        /// <summary>
        /// SPI Device operation.
        /// DEVICE_OP_BUSTYPE_SPI
        /// </summary>
        DeviceOpBustypeSpi = 1,
    }

    /// <summary>
    /// Deepstall flight stage.
    ///  DEEPSTALL_STAGE
    /// </summary>
    public enum DeepstallStage:uint
    {
        /// <summary>
        /// Flying to the landing point.
        /// DEEPSTALL_STAGE_FLY_TO_LANDING
        /// </summary>
        DeepstallStageFlyToLanding = 0,
        /// <summary>
        /// Building an estimate of the wind.
        /// DEEPSTALL_STAGE_ESTIMATE_WIND
        /// </summary>
        DeepstallStageEstimateWind = 1,
        /// <summary>
        /// Waiting to breakout of the loiter to fly the approach.
        /// DEEPSTALL_STAGE_WAIT_FOR_BREAKOUT
        /// </summary>
        DeepstallStageWaitForBreakout = 2,
        /// <summary>
        /// Flying to the first arc point to turn around to the landing point.
        /// DEEPSTALL_STAGE_FLY_TO_ARC
        /// </summary>
        DeepstallStageFlyToArc = 3,
        /// <summary>
        /// Turning around back to the deepstall landing point.
        /// DEEPSTALL_STAGE_ARC
        /// </summary>
        DeepstallStageArc = 4,
        /// <summary>
        /// Approaching the landing point.
        /// DEEPSTALL_STAGE_APPROACH
        /// </summary>
        DeepstallStageApproach = 5,
        /// <summary>
        /// Stalling and steering towards the land point.
        /// DEEPSTALL_STAGE_LAND
        /// </summary>
        DeepstallStageLand = 6,
    }

    /// <summary>
    /// A mapping of plane flight modes for custom_mode field of heartbeat.
    ///  PLANE_MODE
    /// </summary>
    public enum PlaneMode:uint
    {
        /// <summary>
        /// PLANE_MODE_MANUAL
        /// </summary>
        PlaneModeManual = 0,
        /// <summary>
        /// PLANE_MODE_CIRCLE
        /// </summary>
        PlaneModeCircle = 1,
        /// <summary>
        /// PLANE_MODE_STABILIZE
        /// </summary>
        PlaneModeStabilize = 2,
        /// <summary>
        /// PLANE_MODE_TRAINING
        /// </summary>
        PlaneModeTraining = 3,
        /// <summary>
        /// PLANE_MODE_ACRO
        /// </summary>
        PlaneModeAcro = 4,
        /// <summary>
        /// PLANE_MODE_FLY_BY_WIRE_A
        /// </summary>
        PlaneModeFlyByWireA = 5,
        /// <summary>
        /// PLANE_MODE_FLY_BY_WIRE_B
        /// </summary>
        PlaneModeFlyByWireB = 6,
        /// <summary>
        /// PLANE_MODE_CRUISE
        /// </summary>
        PlaneModeCruise = 7,
        /// <summary>
        /// PLANE_MODE_AUTOTUNE
        /// </summary>
        PlaneModeAutotune = 8,
        /// <summary>
        /// PLANE_MODE_AUTO
        /// </summary>
        PlaneModeAuto = 10,
        /// <summary>
        /// PLANE_MODE_RTL
        /// </summary>
        PlaneModeRtl = 11,
        /// <summary>
        /// PLANE_MODE_LOITER
        /// </summary>
        PlaneModeLoiter = 12,
        /// <summary>
        /// PLANE_MODE_AVOID_ADSB
        /// </summary>
        PlaneModeAvoidAdsb = 14,
        /// <summary>
        /// PLANE_MODE_GUIDED
        /// </summary>
        PlaneModeGuided = 15,
        /// <summary>
        /// PLANE_MODE_INITIALIZING
        /// </summary>
        PlaneModeInitializing = 16,
        /// <summary>
        /// PLANE_MODE_QSTABILIZE
        /// </summary>
        PlaneModeQstabilize = 17,
        /// <summary>
        /// PLANE_MODE_QHOVER
        /// </summary>
        PlaneModeQhover = 18,
        /// <summary>
        /// PLANE_MODE_QLOITER
        /// </summary>
        PlaneModeQloiter = 19,
        /// <summary>
        /// PLANE_MODE_QLAND
        /// </summary>
        PlaneModeQland = 20,
        /// <summary>
        /// PLANE_MODE_QRTL
        /// </summary>
        PlaneModeQrtl = 21,
        /// <summary>
        /// PLANE_MODE_QAUTOTUNE
        /// </summary>
        PlaneModeQautotune = 22,
    }

    /// <summary>
    /// A mapping of copter flight modes for custom_mode field of heartbeat.
    ///  COPTER_MODE
    /// </summary>
    public enum CopterMode:uint
    {
        /// <summary>
        /// COPTER_MODE_STABILIZE
        /// </summary>
        CopterModeStabilize = 0,
        /// <summary>
        /// COPTER_MODE_ACRO
        /// </summary>
        CopterModeAcro = 1,
        /// <summary>
        /// COPTER_MODE_ALT_HOLD
        /// </summary>
        CopterModeAltHold = 2,
        /// <summary>
        /// COPTER_MODE_AUTO
        /// </summary>
        CopterModeAuto = 3,
        /// <summary>
        /// COPTER_MODE_GUIDED
        /// </summary>
        CopterModeGuided = 4,
        /// <summary>
        /// COPTER_MODE_LOITER
        /// </summary>
        CopterModeLoiter = 5,
        /// <summary>
        /// COPTER_MODE_RTL
        /// </summary>
        CopterModeRtl = 6,
        /// <summary>
        /// COPTER_MODE_CIRCLE
        /// </summary>
        CopterModeCircle = 7,
        /// <summary>
        /// COPTER_MODE_LAND
        /// </summary>
        CopterModeLand = 9,
        /// <summary>
        /// COPTER_MODE_DRIFT
        /// </summary>
        CopterModeDrift = 11,
        /// <summary>
        /// COPTER_MODE_SPORT
        /// </summary>
        CopterModeSport = 13,
        /// <summary>
        /// COPTER_MODE_FLIP
        /// </summary>
        CopterModeFlip = 14,
        /// <summary>
        /// COPTER_MODE_AUTOTUNE
        /// </summary>
        CopterModeAutotune = 15,
        /// <summary>
        /// COPTER_MODE_POSHOLD
        /// </summary>
        CopterModePoshold = 16,
        /// <summary>
        /// COPTER_MODE_BRAKE
        /// </summary>
        CopterModeBrake = 17,
        /// <summary>
        /// COPTER_MODE_THROW
        /// </summary>
        CopterModeThrow = 18,
        /// <summary>
        /// COPTER_MODE_AVOID_ADSB
        /// </summary>
        CopterModeAvoidAdsb = 19,
        /// <summary>
        /// COPTER_MODE_GUIDED_NOGPS
        /// </summary>
        CopterModeGuidedNogps = 20,
        /// <summary>
        /// COPTER_MODE_SMART_RTL
        /// </summary>
        CopterModeSmartRtl = 21,
    }

    /// <summary>
    /// A mapping of sub flight modes for custom_mode field of heartbeat.
    ///  SUB_MODE
    /// </summary>
    public enum SubMode:uint
    {
        /// <summary>
        /// SUB_MODE_STABILIZE
        /// </summary>
        SubModeStabilize = 0,
        /// <summary>
        /// SUB_MODE_ACRO
        /// </summary>
        SubModeAcro = 1,
        /// <summary>
        /// SUB_MODE_ALT_HOLD
        /// </summary>
        SubModeAltHold = 2,
        /// <summary>
        /// SUB_MODE_AUTO
        /// </summary>
        SubModeAuto = 3,
        /// <summary>
        /// SUB_MODE_GUIDED
        /// </summary>
        SubModeGuided = 4,
        /// <summary>
        /// SUB_MODE_CIRCLE
        /// </summary>
        SubModeCircle = 7,
        /// <summary>
        /// SUB_MODE_SURFACE
        /// </summary>
        SubModeSurface = 9,
        /// <summary>
        /// SUB_MODE_POSHOLD
        /// </summary>
        SubModePoshold = 16,
        /// <summary>
        /// SUB_MODE_MANUAL
        /// </summary>
        SubModeManual = 19,
    }

    /// <summary>
    /// A mapping of rover flight modes for custom_mode field of heartbeat.
    ///  ROVER_MODE
    /// </summary>
    public enum RoverMode:uint
    {
        /// <summary>
        /// ROVER_MODE_MANUAL
        /// </summary>
        RoverModeManual = 0,
        /// <summary>
        /// ROVER_MODE_ACRO
        /// </summary>
        RoverModeAcro = 1,
        /// <summary>
        /// ROVER_MODE_STEERING
        /// </summary>
        RoverModeSteering = 3,
        /// <summary>
        /// ROVER_MODE_HOLD
        /// </summary>
        RoverModeHold = 4,
        /// <summary>
        /// ROVER_MODE_LOITER
        /// </summary>
        RoverModeLoiter = 5,
        /// <summary>
        /// ROVER_MODE_AUTO
        /// </summary>
        RoverModeAuto = 10,
        /// <summary>
        /// ROVER_MODE_RTL
        /// </summary>
        RoverModeRtl = 11,
        /// <summary>
        /// ROVER_MODE_SMART_RTL
        /// </summary>
        RoverModeSmartRtl = 12,
        /// <summary>
        /// ROVER_MODE_GUIDED
        /// </summary>
        RoverModeGuided = 15,
        /// <summary>
        /// ROVER_MODE_INITIALIZING
        /// </summary>
        RoverModeInitializing = 16,
    }

    /// <summary>
    /// A mapping of antenna tracker flight modes for custom_mode field of heartbeat.
    ///  TRACKER_MODE
    /// </summary>
    public enum TrackerMode:uint
    {
        /// <summary>
        /// TRACKER_MODE_MANUAL
        /// </summary>
        TrackerModeManual = 0,
        /// <summary>
        /// TRACKER_MODE_STOP
        /// </summary>
        TrackerModeStop = 1,
        /// <summary>
        /// TRACKER_MODE_SCAN
        /// </summary>
        TrackerModeScan = 2,
        /// <summary>
        /// TRACKER_MODE_SERVO_TEST
        /// </summary>
        TrackerModeServoTest = 3,
        /// <summary>
        /// TRACKER_MODE_AUTO
        /// </summary>
        TrackerModeAuto = 10,
        /// <summary>
        /// TRACKER_MODE_INITIALIZING
        /// </summary>
        TrackerModeInitializing = 16,
    }


#endregion

#region Messages

    /// <summary>
    /// Offsets and calibrations values for hardware sensors. This makes it easier to debug the calibration process.
    ///  SENSOR_OFFSETS
    /// </summary>
    public class SensorOffsetsPacket: PacketV2<SensorOffsetsPayload>
    {
	    public const int PacketMessageId = 150;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 134;

        public override SensorOffsetsPayload Payload { get; } = new SensorOffsetsPayload();

        public override string Name => "SENSOR_OFFSETS";
    }

    /// <summary>
    ///  SENSOR_OFFSETS
    /// </summary>
    public class SensorOffsetsPayload : IPayload
    {
        public byte GetMaxByteSize() => 42; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 42; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            MagDeclination = BinSerialize.ReadFloat(ref buffer);index+=4;
            RawPress = BinSerialize.ReadInt(ref buffer);index+=4;
            RawTemp = BinSerialize.ReadInt(ref buffer);index+=4;
            GyroCalX = BinSerialize.ReadFloat(ref buffer);index+=4;
            GyroCalY = BinSerialize.ReadFloat(ref buffer);index+=4;
            GyroCalZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            AccelCalX = BinSerialize.ReadFloat(ref buffer);index+=4;
            AccelCalY = BinSerialize.ReadFloat(ref buffer);index+=4;
            AccelCalZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            MagOfsX = BinSerialize.ReadShort(ref buffer);index+=2;
            MagOfsY = BinSerialize.ReadShort(ref buffer);index+=2;
            MagOfsZ = BinSerialize.ReadShort(ref buffer);index+=2;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,MagDeclination);index+=4;
            BinSerialize.WriteInt(ref buffer,RawPress);index+=4;
            BinSerialize.WriteInt(ref buffer,RawTemp);index+=4;
            BinSerialize.WriteFloat(ref buffer,GyroCalX);index+=4;
            BinSerialize.WriteFloat(ref buffer,GyroCalY);index+=4;
            BinSerialize.WriteFloat(ref buffer,GyroCalZ);index+=4;
            BinSerialize.WriteFloat(ref buffer,AccelCalX);index+=4;
            BinSerialize.WriteFloat(ref buffer,AccelCalY);index+=4;
            BinSerialize.WriteFloat(ref buffer,AccelCalZ);index+=4;
            BinSerialize.WriteShort(ref buffer,MagOfsX);index+=2;
            BinSerialize.WriteShort(ref buffer,MagOfsY);index+=2;
            BinSerialize.WriteShort(ref buffer,MagOfsZ);index+=2;
            return index; // /*PayloadByteSize*/42;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            MagDeclination = BitConverter.ToSingle(buffer, index);index+=4;
            RawPress = BitConverter.ToInt32(buffer,index);index+=4;
            RawTemp = BitConverter.ToInt32(buffer,index);index+=4;
            GyroCalX = BitConverter.ToSingle(buffer, index);index+=4;
            GyroCalY = BitConverter.ToSingle(buffer, index);index+=4;
            GyroCalZ = BitConverter.ToSingle(buffer, index);index+=4;
            AccelCalX = BitConverter.ToSingle(buffer, index);index+=4;
            AccelCalY = BitConverter.ToSingle(buffer, index);index+=4;
            AccelCalZ = BitConverter.ToSingle(buffer, index);index+=4;
            MagOfsX = BitConverter.ToInt16(buffer,index);index+=2;
            MagOfsY = BitConverter.ToInt16(buffer,index);index+=2;
            MagOfsZ = BitConverter.ToInt16(buffer,index);index+=2;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(MagDeclination).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(RawPress).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(RawTemp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(GyroCalX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(GyroCalY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(GyroCalZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AccelCalX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AccelCalY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AccelCalZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(MagOfsX).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(MagOfsY).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(MagOfsZ).CopyTo(buffer, index);index+=2;
            return index - start; // /*PayloadByteSize*/42;
        }

        /// <summary>
        /// Magnetic declination.
        /// OriginName: mag_declination, Units: rad, IsExtended: false
        /// </summary>
        public float MagDeclination { get; set; }
        /// <summary>
        /// Raw pressure from barometer.
        /// OriginName: raw_press, Units: , IsExtended: false
        /// </summary>
        public int RawPress { get; set; }
        /// <summary>
        /// Raw temperature from barometer.
        /// OriginName: raw_temp, Units: , IsExtended: false
        /// </summary>
        public int RawTemp { get; set; }
        /// <summary>
        /// Gyro X calibration.
        /// OriginName: gyro_cal_x, Units: , IsExtended: false
        /// </summary>
        public float GyroCalX { get; set; }
        /// <summary>
        /// Gyro Y calibration.
        /// OriginName: gyro_cal_y, Units: , IsExtended: false
        /// </summary>
        public float GyroCalY { get; set; }
        /// <summary>
        /// Gyro Z calibration.
        /// OriginName: gyro_cal_z, Units: , IsExtended: false
        /// </summary>
        public float GyroCalZ { get; set; }
        /// <summary>
        /// Accel X calibration.
        /// OriginName: accel_cal_x, Units: , IsExtended: false
        /// </summary>
        public float AccelCalX { get; set; }
        /// <summary>
        /// Accel Y calibration.
        /// OriginName: accel_cal_y, Units: , IsExtended: false
        /// </summary>
        public float AccelCalY { get; set; }
        /// <summary>
        /// Accel Z calibration.
        /// OriginName: accel_cal_z, Units: , IsExtended: false
        /// </summary>
        public float AccelCalZ { get; set; }
        /// <summary>
        /// Magnetometer X offset.
        /// OriginName: mag_ofs_x, Units: , IsExtended: false
        /// </summary>
        public short MagOfsX { get; set; }
        /// <summary>
        /// Magnetometer Y offset.
        /// OriginName: mag_ofs_y, Units: , IsExtended: false
        /// </summary>
        public short MagOfsY { get; set; }
        /// <summary>
        /// Magnetometer Z offset.
        /// OriginName: mag_ofs_z, Units: , IsExtended: false
        /// </summary>
        public short MagOfsZ { get; set; }
    }
    /// <summary>
    /// Set the magnetometer offsets
    ///  SET_MAG_OFFSETS
    /// </summary>
    public class SetMagOffsetsPacket: PacketV2<SetMagOffsetsPayload>
    {
	    public const int PacketMessageId = 151;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 219;

        public override SetMagOffsetsPayload Payload { get; } = new SetMagOffsetsPayload();

        public override string Name => "SET_MAG_OFFSETS";
    }

    /// <summary>
    ///  SET_MAG_OFFSETS
    /// </summary>
    public class SetMagOffsetsPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            MagOfsX = BinSerialize.ReadShort(ref buffer);index+=2;
            MagOfsY = BinSerialize.ReadShort(ref buffer);index+=2;
            MagOfsZ = BinSerialize.ReadShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteShort(ref buffer,MagOfsX);index+=2;
            BinSerialize.WriteShort(ref buffer,MagOfsY);index+=2;
            BinSerialize.WriteShort(ref buffer,MagOfsZ);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            return index; // /*PayloadByteSize*/8;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            MagOfsX = BitConverter.ToInt16(buffer,index);index+=2;
            MagOfsY = BitConverter.ToInt16(buffer,index);index+=2;
            MagOfsZ = BitConverter.ToInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(MagOfsX).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(MagOfsY).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(MagOfsZ).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/8;
        }

        /// <summary>
        /// Magnetometer X offset.
        /// OriginName: mag_ofs_x, Units: , IsExtended: false
        /// </summary>
        public short MagOfsX { get; set; }
        /// <summary>
        /// Magnetometer Y offset.
        /// OriginName: mag_ofs_y, Units: , IsExtended: false
        /// </summary>
        public short MagOfsY { get; set; }
        /// <summary>
        /// Magnetometer Z offset.
        /// OriginName: mag_ofs_z, Units: , IsExtended: false
        /// </summary>
        public short MagOfsZ { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
    }
    /// <summary>
    /// State of APM memory.
    ///  MEMINFO
    /// </summary>
    public class MeminfoPacket: PacketV2<MeminfoPayload>
    {
	    public const int PacketMessageId = 152;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 208;

        public override MeminfoPayload Payload { get; } = new MeminfoPayload();

        public override string Name => "MEMINFO";
    }

    /// <summary>
    ///  MEMINFO
    /// </summary>
    public class MeminfoPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Brkval = BinSerialize.ReadUShort(ref buffer);index+=2;
            Freemem = BinSerialize.ReadUShort(ref buffer);index+=2;
            // extended field 'Freemem32' can be empty
            if (index >= endIndex) return;
            Freemem32 = BinSerialize.ReadUInt(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUShort(ref buffer,Brkval);index+=2;
            BinSerialize.WriteUShort(ref buffer,Freemem);index+=2;
            BinSerialize.WriteUInt(ref buffer,Freemem32);index+=4;
            return index; // /*PayloadByteSize*/8;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Brkval = BitConverter.ToUInt16(buffer,index);index+=2;
            Freemem = BitConverter.ToUInt16(buffer,index);index+=2;
            // extended field 'Freemem32' can be empty
            if (index >= endIndex) return;
            Freemem32 = BitConverter.ToUInt32(buffer,index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Brkval).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Freemem).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Freemem32).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/8;
        }

        /// <summary>
        /// Heap top.
        /// OriginName: brkval, Units: , IsExtended: false
        /// </summary>
        public ushort Brkval { get; set; }
        /// <summary>
        /// Free memory.
        /// OriginName: freemem, Units: bytes, IsExtended: false
        /// </summary>
        public ushort Freemem { get; set; }
        /// <summary>
        /// Free memory (32 bit).
        /// OriginName: freemem32, Units: bytes, IsExtended: true
        /// </summary>
        public uint Freemem32 { get; set; }
    }
    /// <summary>
    /// Raw ADC output.
    ///  AP_ADC
    /// </summary>
    public class ApAdcPacket: PacketV2<ApAdcPayload>
    {
	    public const int PacketMessageId = 153;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 188;

        public override ApAdcPayload Payload { get; } = new ApAdcPayload();

        public override string Name => "AP_ADC";
    }

    /// <summary>
    ///  AP_ADC
    /// </summary>
    public class ApAdcPayload : IPayload
    {
        public byte GetMaxByteSize() => 12; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 12; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Adc1 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Adc2 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Adc3 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Adc4 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Adc5 = BinSerialize.ReadUShort(ref buffer);index+=2;
            Adc6 = BinSerialize.ReadUShort(ref buffer);index+=2;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUShort(ref buffer,Adc1);index+=2;
            BinSerialize.WriteUShort(ref buffer,Adc2);index+=2;
            BinSerialize.WriteUShort(ref buffer,Adc3);index+=2;
            BinSerialize.WriteUShort(ref buffer,Adc4);index+=2;
            BinSerialize.WriteUShort(ref buffer,Adc5);index+=2;
            BinSerialize.WriteUShort(ref buffer,Adc6);index+=2;
            return index; // /*PayloadByteSize*/12;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Adc1 = BitConverter.ToUInt16(buffer,index);index+=2;
            Adc2 = BitConverter.ToUInt16(buffer,index);index+=2;
            Adc3 = BitConverter.ToUInt16(buffer,index);index+=2;
            Adc4 = BitConverter.ToUInt16(buffer,index);index+=2;
            Adc5 = BitConverter.ToUInt16(buffer,index);index+=2;
            Adc6 = BitConverter.ToUInt16(buffer,index);index+=2;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Adc1).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Adc2).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Adc3).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Adc4).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Adc5).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Adc6).CopyTo(buffer, index);index+=2;
            return index - start; // /*PayloadByteSize*/12;
        }

        /// <summary>
        /// ADC output 1.
        /// OriginName: adc1, Units: , IsExtended: false
        /// </summary>
        public ushort Adc1 { get; set; }
        /// <summary>
        /// ADC output 2.
        /// OriginName: adc2, Units: , IsExtended: false
        /// </summary>
        public ushort Adc2 { get; set; }
        /// <summary>
        /// ADC output 3.
        /// OriginName: adc3, Units: , IsExtended: false
        /// </summary>
        public ushort Adc3 { get; set; }
        /// <summary>
        /// ADC output 4.
        /// OriginName: adc4, Units: , IsExtended: false
        /// </summary>
        public ushort Adc4 { get; set; }
        /// <summary>
        /// ADC output 5.
        /// OriginName: adc5, Units: , IsExtended: false
        /// </summary>
        public ushort Adc5 { get; set; }
        /// <summary>
        /// ADC output 6.
        /// OriginName: adc6, Units: , IsExtended: false
        /// </summary>
        public ushort Adc6 { get; set; }
    }
    /// <summary>
    /// Configure on-board Camera Control System.
    ///  DIGICAM_CONFIGURE
    /// </summary>
    public class DigicamConfigurePacket: PacketV2<DigicamConfigurePayload>
    {
	    public const int PacketMessageId = 154;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 84;

        public override DigicamConfigurePayload Payload { get; } = new DigicamConfigurePayload();

        public override string Name => "DIGICAM_CONFIGURE";
    }

    /// <summary>
    ///  DIGICAM_CONFIGURE
    /// </summary>
    public class DigicamConfigurePayload : IPayload
    {
        public byte GetMaxByteSize() => 15; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 15; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            ExtraValue = BinSerialize.ReadFloat(ref buffer);index+=4;
            ShutterSpeed = BinSerialize.ReadUShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Mode = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Aperture = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Iso = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            ExposureType = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CommandId = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            EngineCutOff = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            ExtraParam = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,ExtraValue);index+=4;
            BinSerialize.WriteUShort(ref buffer,ShutterSpeed);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Mode);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Aperture);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Iso);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)ExposureType);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CommandId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)EngineCutOff);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)ExtraParam);index+=1;
            return index; // /*PayloadByteSize*/15;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            ExtraValue = BitConverter.ToSingle(buffer, index);index+=4;
            ShutterSpeed = BitConverter.ToUInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Mode = (byte)buffer[index++];
            Aperture = (byte)buffer[index++];
            Iso = (byte)buffer[index++];
            ExposureType = (byte)buffer[index++];
            CommandId = (byte)buffer[index++];
            EngineCutOff = (byte)buffer[index++];
            ExtraParam = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(ExtraValue).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(ShutterSpeed).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Mode).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Aperture).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Iso).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(ExposureType).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(CommandId).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(EngineCutOff).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(ExtraParam).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/15;
        }

        /// <summary>
        /// Correspondent value to given extra_param.
        /// OriginName: extra_value, Units: , IsExtended: false
        /// </summary>
        public float ExtraValue { get; set; }
        /// <summary>
        /// Divisor number //e.g. 1000 means 1/1000 (0 means ignore).
        /// OriginName: shutter_speed, Units: , IsExtended: false
        /// </summary>
        public ushort ShutterSpeed { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Mode enumeration from 1 to N //P, TV, AV, M, etc. (0 means ignore).
        /// OriginName: mode, Units: , IsExtended: false
        /// </summary>
        public byte Mode { get; set; }
        /// <summary>
        /// F stop number x 10 //e.g. 28 means 2.8 (0 means ignore).
        /// OriginName: aperture, Units: , IsExtended: false
        /// </summary>
        public byte Aperture { get; set; }
        /// <summary>
        /// ISO enumeration from 1 to N //e.g. 80, 100, 200, Etc (0 means ignore).
        /// OriginName: iso, Units: , IsExtended: false
        /// </summary>
        public byte Iso { get; set; }
        /// <summary>
        /// Exposure type enumeration from 1 to N (0 means ignore).
        /// OriginName: exposure_type, Units: , IsExtended: false
        /// </summary>
        public byte ExposureType { get; set; }
        /// <summary>
        /// Command Identity (incremental loop: 0 to 255). //A command sent multiple times will be executed or pooled just once.
        /// OriginName: command_id, Units: , IsExtended: false
        /// </summary>
        public byte CommandId { get; set; }
        /// <summary>
        /// Main engine cut-off time before camera trigger (0 means no cut-off).
        /// OriginName: engine_cut_off, Units: ds, IsExtended: false
        /// </summary>
        public byte EngineCutOff { get; set; }
        /// <summary>
        /// Extra parameters enumeration (0 means ignore).
        /// OriginName: extra_param, Units: , IsExtended: false
        /// </summary>
        public byte ExtraParam { get; set; }
    }
    /// <summary>
    /// Control on-board Camera Control System to take shots.
    ///  DIGICAM_CONTROL
    /// </summary>
    public class DigicamControlPacket: PacketV2<DigicamControlPayload>
    {
	    public const int PacketMessageId = 155;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 22;

        public override DigicamControlPayload Payload { get; } = new DigicamControlPayload();

        public override string Name => "DIGICAM_CONTROL";
    }

    /// <summary>
    ///  DIGICAM_CONTROL
    /// </summary>
    public class DigicamControlPayload : IPayload
    {
        public byte GetMaxByteSize() => 13; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 13; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            ExtraValue = BinSerialize.ReadFloat(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Session = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            ZoomPos = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            ZoomStep = (sbyte)BinSerialize.ReadByte(ref buffer);index+=1;
            FocusLock = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Shot = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CommandId = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            ExtraParam = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,ExtraValue);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Session);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)ZoomPos);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)ZoomStep);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)FocusLock);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Shot);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CommandId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)ExtraParam);index+=1;
            return index; // /*PayloadByteSize*/13;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            ExtraValue = BitConverter.ToSingle(buffer, index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Session = (byte)buffer[index++];
            ZoomPos = (byte)buffer[index++];
            ZoomStep = (sbyte)buffer[index++];
            FocusLock = (byte)buffer[index++];
            Shot = (byte)buffer[index++];
            CommandId = (byte)buffer[index++];
            ExtraParam = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(ExtraValue).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Session).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(ZoomPos).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(ZoomStep).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(FocusLock).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Shot).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(CommandId).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(ExtraParam).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/13;
        }

        /// <summary>
        /// Correspondent value to given extra_param.
        /// OriginName: extra_value, Units: , IsExtended: false
        /// </summary>
        public float ExtraValue { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// 0: stop, 1: start or keep it up //Session control e.g. show/hide lens.
        /// OriginName: session, Units: , IsExtended: false
        /// </summary>
        public byte Session { get; set; }
        /// <summary>
        /// 1 to N //Zoom's absolute position (0 means ignore).
        /// OriginName: zoom_pos, Units: , IsExtended: false
        /// </summary>
        public byte ZoomPos { get; set; }
        /// <summary>
        /// -100 to 100 //Zooming step value to offset zoom from the current position.
        /// OriginName: zoom_step, Units: , IsExtended: false
        /// </summary>
        public sbyte ZoomStep { get; set; }
        /// <summary>
        /// 0: unlock focus or keep unlocked, 1: lock focus or keep locked, 3: re-lock focus.
        /// OriginName: focus_lock, Units: , IsExtended: false
        /// </summary>
        public byte FocusLock { get; set; }
        /// <summary>
        /// 0: ignore, 1: shot or start filming.
        /// OriginName: shot, Units: , IsExtended: false
        /// </summary>
        public byte Shot { get; set; }
        /// <summary>
        /// Command Identity (incremental loop: 0 to 255)//A command sent multiple times will be executed or pooled just once.
        /// OriginName: command_id, Units: , IsExtended: false
        /// </summary>
        public byte CommandId { get; set; }
        /// <summary>
        /// Extra parameters enumeration (0 means ignore).
        /// OriginName: extra_param, Units: , IsExtended: false
        /// </summary>
        public byte ExtraParam { get; set; }
    }
    /// <summary>
    /// Message to configure a camera mount, directional antenna, etc.
    ///  MOUNT_CONFIGURE
    /// </summary>
    public class MountConfigurePacket: PacketV2<MountConfigurePayload>
    {
	    public const int PacketMessageId = 156;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 19;

        public override MountConfigurePayload Payload { get; } = new MountConfigurePayload();

        public override string Name => "MOUNT_CONFIGURE";
    }

    /// <summary>
    ///  MOUNT_CONFIGURE
    /// </summary>
    public class MountConfigurePayload : IPayload
    {
        public byte GetMaxByteSize() => 6; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            MountMode = (MavMountMode)BinSerialize.ReadByte(ref buffer);index+=1;
            StabRoll = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            StabPitch = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            StabYaw = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)MountMode);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)StabRoll);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)StabPitch);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)StabYaw);index+=1;
            return index; // /*PayloadByteSize*/6;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            MountMode = (MavMountMode)buffer[index++];
            StabRoll = (byte)buffer[index++];
            StabPitch = (byte)buffer[index++];
            StabYaw = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)MountMode;index+=1;
            BitConverter.GetBytes(StabRoll).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(StabPitch).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(StabYaw).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/6;
        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Mount operating mode.
        /// OriginName: mount_mode, Units: , IsExtended: false
        /// </summary>
        public MavMountMode MountMode { get; set; }
        /// <summary>
        /// (1 = yes, 0 = no).
        /// OriginName: stab_roll, Units: , IsExtended: false
        /// </summary>
        public byte StabRoll { get; set; }
        /// <summary>
        /// (1 = yes, 0 = no).
        /// OriginName: stab_pitch, Units: , IsExtended: false
        /// </summary>
        public byte StabPitch { get; set; }
        /// <summary>
        /// (1 = yes, 0 = no).
        /// OriginName: stab_yaw, Units: , IsExtended: false
        /// </summary>
        public byte StabYaw { get; set; }
    }
    /// <summary>
    /// Message to control a camera mount, directional antenna, etc.
    ///  MOUNT_CONTROL
    /// </summary>
    public class MountControlPacket: PacketV2<MountControlPayload>
    {
	    public const int PacketMessageId = 157;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 21;

        public override MountControlPayload Payload { get; } = new MountControlPayload();

        public override string Name => "MOUNT_CONTROL";
    }

    /// <summary>
    ///  MOUNT_CONTROL
    /// </summary>
    public class MountControlPayload : IPayload
    {
        public byte GetMaxByteSize() => 15; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 15; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            InputA = BinSerialize.ReadInt(ref buffer);index+=4;
            InputB = BinSerialize.ReadInt(ref buffer);index+=4;
            InputC = BinSerialize.ReadInt(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            SavePosition = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteInt(ref buffer,InputA);index+=4;
            BinSerialize.WriteInt(ref buffer,InputB);index+=4;
            BinSerialize.WriteInt(ref buffer,InputC);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)SavePosition);index+=1;
            return index; // /*PayloadByteSize*/15;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            InputA = BitConverter.ToInt32(buffer,index);index+=4;
            InputB = BitConverter.ToInt32(buffer,index);index+=4;
            InputC = BitConverter.ToInt32(buffer,index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            SavePosition = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(InputA).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(InputB).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(InputC).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(SavePosition).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/15;
        }

        /// <summary>
        /// Pitch (centi-degrees) or lat (degE7), depending on mount mode.
        /// OriginName: input_a, Units: , IsExtended: false
        /// </summary>
        public int InputA { get; set; }
        /// <summary>
        /// Roll (centi-degrees) or lon (degE7) depending on mount mode.
        /// OriginName: input_b, Units: , IsExtended: false
        /// </summary>
        public int InputB { get; set; }
        /// <summary>
        /// Yaw (centi-degrees) or alt (cm) depending on mount mode.
        /// OriginName: input_c, Units: , IsExtended: false
        /// </summary>
        public int InputC { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// If "1" it will save current trimmed position on EEPROM (just valid for NEUTRAL and LANDING).
        /// OriginName: save_position, Units: , IsExtended: false
        /// </summary>
        public byte SavePosition { get; set; }
    }
    /// <summary>
    /// Message with some status from APM to GCS about camera or antenna mount.
    ///  MOUNT_STATUS
    /// </summary>
    public class MountStatusPacket: PacketV2<MountStatusPayload>
    {
	    public const int PacketMessageId = 158;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 134;

        public override MountStatusPayload Payload { get; } = new MountStatusPayload();

        public override string Name => "MOUNT_STATUS";
    }

    /// <summary>
    ///  MOUNT_STATUS
    /// </summary>
    public class MountStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 14; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 14; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            PointingA = BinSerialize.ReadInt(ref buffer);index+=4;
            PointingB = BinSerialize.ReadInt(ref buffer);index+=4;
            PointingC = BinSerialize.ReadInt(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteInt(ref buffer,PointingA);index+=4;
            BinSerialize.WriteInt(ref buffer,PointingB);index+=4;
            BinSerialize.WriteInt(ref buffer,PointingC);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            return index; // /*PayloadByteSize*/14;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            PointingA = BitConverter.ToInt32(buffer,index);index+=4;
            PointingB = BitConverter.ToInt32(buffer,index);index+=4;
            PointingC = BitConverter.ToInt32(buffer,index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(PointingA).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PointingB).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PointingC).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/14;
        }

        /// <summary>
        /// Pitch.
        /// OriginName: pointing_a, Units: cdeg, IsExtended: false
        /// </summary>
        public int PointingA { get; set; }
        /// <summary>
        /// Roll.
        /// OriginName: pointing_b, Units: cdeg, IsExtended: false
        /// </summary>
        public int PointingB { get; set; }
        /// <summary>
        /// Yaw.
        /// OriginName: pointing_c, Units: cdeg, IsExtended: false
        /// </summary>
        public int PointingC { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
    }
    /// <summary>
    /// A fence point. Used to set a point when from GCS -> MAV. Also used to return a point from MAV -> GCS.
    ///  FENCE_POINT
    /// </summary>
    public class FencePointPacket: PacketV2<FencePointPayload>
    {
	    public const int PacketMessageId = 160;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 78;

        public override FencePointPayload Payload { get; } = new FencePointPayload();

        public override string Name => "FENCE_POINT";
    }

    /// <summary>
    ///  FENCE_POINT
    /// </summary>
    public class FencePointPayload : IPayload
    {
        public byte GetMaxByteSize() => 12; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 12; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Lat = BinSerialize.ReadFloat(ref buffer);index+=4;
            Lng = BinSerialize.ReadFloat(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Idx = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Count = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Lat);index+=4;
            BinSerialize.WriteFloat(ref buffer,Lng);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Idx);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Count);index+=1;
            return index; // /*PayloadByteSize*/12;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Lat = BitConverter.ToSingle(buffer, index);index+=4;
            Lng = BitConverter.ToSingle(buffer, index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Idx = (byte)buffer[index++];
            Count = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Lat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Lng).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Idx).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Count).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/12;
        }

        /// <summary>
        /// Latitude of point.
        /// OriginName: lat, Units: deg, IsExtended: false
        /// </summary>
        public float Lat { get; set; }
        /// <summary>
        /// Longitude of point.
        /// OriginName: lng, Units: deg, IsExtended: false
        /// </summary>
        public float Lng { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Point index (first point is 1, 0 is for return point).
        /// OriginName: idx, Units: , IsExtended: false
        /// </summary>
        public byte Idx { get; set; }
        /// <summary>
        /// Total number of points (for sanity checking).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public byte Count { get; set; }
    }
    /// <summary>
    /// Request a current fence point from MAV.
    ///  FENCE_FETCH_POINT
    /// </summary>
    public class FenceFetchPointPacket: PacketV2<FenceFetchPointPayload>
    {
	    public const int PacketMessageId = 161;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 68;

        public override FenceFetchPointPayload Payload { get; } = new FenceFetchPointPayload();

        public override string Name => "FENCE_FETCH_POINT";
    }

    /// <summary>
    ///  FENCE_FETCH_POINT
    /// </summary>
    public class FenceFetchPointPayload : IPayload
    {
        public byte GetMaxByteSize() => 3; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Idx = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Idx);index+=1;
            return index; // /*PayloadByteSize*/3;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Idx = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Idx).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/3;
        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Point index (first point is 1, 0 is for return point).
        /// OriginName: idx, Units: , IsExtended: false
        /// </summary>
        public byte Idx { get; set; }
    }
    /// <summary>
    /// Status of geo-fencing. Sent in extended status stream when fencing enabled.
    ///  FENCE_STATUS
    /// </summary>
    public class FenceStatusPacket: PacketV2<FenceStatusPayload>
    {
	    public const int PacketMessageId = 162;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 189;

        public override FenceStatusPayload Payload { get; } = new FenceStatusPayload();

        public override string Name => "FENCE_STATUS";
    }

    /// <summary>
    ///  FENCE_STATUS
    /// </summary>
    public class FenceStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            BreachTime = BinSerialize.ReadUInt(ref buffer);index+=4;
            BreachCount = BinSerialize.ReadUShort(ref buffer);index+=2;
            BreachStatus = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            BreachType = (FenceBreach)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,BreachTime);index+=4;
            BinSerialize.WriteUShort(ref buffer,BreachCount);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)BreachStatus);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)BreachType);index+=1;
            return index; // /*PayloadByteSize*/8;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            BreachTime = BitConverter.ToUInt32(buffer,index);index+=4;
            BreachCount = BitConverter.ToUInt16(buffer,index);index+=2;
            BreachStatus = (byte)buffer[index++];
            BreachType = (FenceBreach)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(BreachTime).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(BreachCount).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(BreachStatus).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)BreachType;index+=1;
            return index - start; // /*PayloadByteSize*/8;
        }

        /// <summary>
        /// Time (since boot) of last breach.
        /// OriginName: breach_time, Units: ms, IsExtended: false
        /// </summary>
        public uint BreachTime { get; set; }
        /// <summary>
        /// Number of fence breaches.
        /// OriginName: breach_count, Units: , IsExtended: false
        /// </summary>
        public ushort BreachCount { get; set; }
        /// <summary>
        /// Breach status (0 if currently inside fence, 1 if outside).
        /// OriginName: breach_status, Units: , IsExtended: false
        /// </summary>
        public byte BreachStatus { get; set; }
        /// <summary>
        /// Last breach type.
        /// OriginName: breach_type, Units: , IsExtended: false
        /// </summary>
        public FenceBreach BreachType { get; set; }
    }
    /// <summary>
    /// Status of DCM attitude estimator.
    ///  AHRS
    /// </summary>
    public class AhrsPacket: PacketV2<AhrsPayload>
    {
	    public const int PacketMessageId = 163;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 127;

        public override AhrsPayload Payload { get; } = new AhrsPayload();

        public override string Name => "AHRS";
    }

    /// <summary>
    ///  AHRS
    /// </summary>
    public class AhrsPayload : IPayload
    {
        public byte GetMaxByteSize() => 28; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 28; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Omegaix = BinSerialize.ReadFloat(ref buffer);index+=4;
            Omegaiy = BinSerialize.ReadFloat(ref buffer);index+=4;
            Omegaiz = BinSerialize.ReadFloat(ref buffer);index+=4;
            AccelWeight = BinSerialize.ReadFloat(ref buffer);index+=4;
            RenormVal = BinSerialize.ReadFloat(ref buffer);index+=4;
            ErrorRp = BinSerialize.ReadFloat(ref buffer);index+=4;
            ErrorYaw = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Omegaix);index+=4;
            BinSerialize.WriteFloat(ref buffer,Omegaiy);index+=4;
            BinSerialize.WriteFloat(ref buffer,Omegaiz);index+=4;
            BinSerialize.WriteFloat(ref buffer,AccelWeight);index+=4;
            BinSerialize.WriteFloat(ref buffer,RenormVal);index+=4;
            BinSerialize.WriteFloat(ref buffer,ErrorRp);index+=4;
            BinSerialize.WriteFloat(ref buffer,ErrorYaw);index+=4;
            return index; // /*PayloadByteSize*/28;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Omegaix = BitConverter.ToSingle(buffer, index);index+=4;
            Omegaiy = BitConverter.ToSingle(buffer, index);index+=4;
            Omegaiz = BitConverter.ToSingle(buffer, index);index+=4;
            AccelWeight = BitConverter.ToSingle(buffer, index);index+=4;
            RenormVal = BitConverter.ToSingle(buffer, index);index+=4;
            ErrorRp = BitConverter.ToSingle(buffer, index);index+=4;
            ErrorYaw = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Omegaix).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Omegaiy).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Omegaiz).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AccelWeight).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(RenormVal).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(ErrorRp).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(ErrorYaw).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/28;
        }

        /// <summary>
        /// X gyro drift estimate.
        /// OriginName: omegaIx, Units: rad/s, IsExtended: false
        /// </summary>
        public float Omegaix { get; set; }
        /// <summary>
        /// Y gyro drift estimate.
        /// OriginName: omegaIy, Units: rad/s, IsExtended: false
        /// </summary>
        public float Omegaiy { get; set; }
        /// <summary>
        /// Z gyro drift estimate.
        /// OriginName: omegaIz, Units: rad/s, IsExtended: false
        /// </summary>
        public float Omegaiz { get; set; }
        /// <summary>
        /// Average accel_weight.
        /// OriginName: accel_weight, Units: , IsExtended: false
        /// </summary>
        public float AccelWeight { get; set; }
        /// <summary>
        /// Average renormalisation value.
        /// OriginName: renorm_val, Units: , IsExtended: false
        /// </summary>
        public float RenormVal { get; set; }
        /// <summary>
        /// Average error_roll_pitch value.
        /// OriginName: error_rp, Units: , IsExtended: false
        /// </summary>
        public float ErrorRp { get; set; }
        /// <summary>
        /// Average error_yaw value.
        /// OriginName: error_yaw, Units: , IsExtended: false
        /// </summary>
        public float ErrorYaw { get; set; }
    }
    /// <summary>
    /// Status of simulation environment, if used.
    ///  SIMSTATE
    /// </summary>
    public class SimstatePacket: PacketV2<SimstatePayload>
    {
	    public const int PacketMessageId = 164;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 154;

        public override SimstatePayload Payload { get; } = new SimstatePayload();

        public override string Name => "SIMSTATE";
    }

    /// <summary>
    ///  SIMSTATE
    /// </summary>
    public class SimstatePayload : IPayload
    {
        public byte GetMaxByteSize() => 44; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Roll = BinSerialize.ReadFloat(ref buffer);index+=4;
            Pitch = BinSerialize.ReadFloat(ref buffer);index+=4;
            Yaw = BinSerialize.ReadFloat(ref buffer);index+=4;
            Xacc = BinSerialize.ReadFloat(ref buffer);index+=4;
            Yacc = BinSerialize.ReadFloat(ref buffer);index+=4;
            Zacc = BinSerialize.ReadFloat(ref buffer);index+=4;
            Xgyro = BinSerialize.ReadFloat(ref buffer);index+=4;
            Ygyro = BinSerialize.ReadFloat(ref buffer);index+=4;
            Zgyro = BinSerialize.ReadFloat(ref buffer);index+=4;
            Lat = BinSerialize.ReadInt(ref buffer);index+=4;
            Lng = BinSerialize.ReadInt(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Roll);index+=4;
            BinSerialize.WriteFloat(ref buffer,Pitch);index+=4;
            BinSerialize.WriteFloat(ref buffer,Yaw);index+=4;
            BinSerialize.WriteFloat(ref buffer,Xacc);index+=4;
            BinSerialize.WriteFloat(ref buffer,Yacc);index+=4;
            BinSerialize.WriteFloat(ref buffer,Zacc);index+=4;
            BinSerialize.WriteFloat(ref buffer,Xgyro);index+=4;
            BinSerialize.WriteFloat(ref buffer,Ygyro);index+=4;
            BinSerialize.WriteFloat(ref buffer,Zgyro);index+=4;
            BinSerialize.WriteInt(ref buffer,Lat);index+=4;
            BinSerialize.WriteInt(ref buffer,Lng);index+=4;
            return index; // /*PayloadByteSize*/44;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Roll = BitConverter.ToSingle(buffer, index);index+=4;
            Pitch = BitConverter.ToSingle(buffer, index);index+=4;
            Yaw = BitConverter.ToSingle(buffer, index);index+=4;
            Xacc = BitConverter.ToSingle(buffer, index);index+=4;
            Yacc = BitConverter.ToSingle(buffer, index);index+=4;
            Zacc = BitConverter.ToSingle(buffer, index);index+=4;
            Xgyro = BitConverter.ToSingle(buffer, index);index+=4;
            Ygyro = BitConverter.ToSingle(buffer, index);index+=4;
            Zgyro = BitConverter.ToSingle(buffer, index);index+=4;
            Lat = BitConverter.ToInt32(buffer,index);index+=4;
            Lng = BitConverter.ToInt32(buffer,index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Roll).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Pitch).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Yaw).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Xacc).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Yacc).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Zacc).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Xgyro).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Ygyro).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Zgyro).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Lat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Lng).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/44;
        }

        /// <summary>
        /// Roll angle.
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
        /// <summary>
        /// Pitch angle.
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw angle.
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// X acceleration.
        /// OriginName: xacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public float Xacc { get; set; }
        /// <summary>
        /// Y acceleration.
        /// OriginName: yacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public float Yacc { get; set; }
        /// <summary>
        /// Z acceleration.
        /// OriginName: zacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public float Zacc { get; set; }
        /// <summary>
        /// Angular speed around X axis.
        /// OriginName: xgyro, Units: rad/s, IsExtended: false
        /// </summary>
        public float Xgyro { get; set; }
        /// <summary>
        /// Angular speed around Y axis.
        /// OriginName: ygyro, Units: rad/s, IsExtended: false
        /// </summary>
        public float Ygyro { get; set; }
        /// <summary>
        /// Angular speed around Z axis.
        /// OriginName: zgyro, Units: rad/s, IsExtended: false
        /// </summary>
        public float Zgyro { get; set; }
        /// <summary>
        /// Latitude.
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Longitude.
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public int Lng { get; set; }
    }
    /// <summary>
    /// Status of key hardware.
    ///  HWSTATUS
    /// </summary>
    public class HwstatusPacket: PacketV2<HwstatusPayload>
    {
	    public const int PacketMessageId = 165;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 21;

        public override HwstatusPayload Payload { get; } = new HwstatusPayload();

        public override string Name => "HWSTATUS";
    }

    /// <summary>
    ///  HWSTATUS
    /// </summary>
    public class HwstatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 3; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Vcc = BinSerialize.ReadUShort(ref buffer);index+=2;
            I2cerr = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUShort(ref buffer,Vcc);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)I2cerr);index+=1;
            return index; // /*PayloadByteSize*/3;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Vcc = BitConverter.ToUInt16(buffer,index);index+=2;
            I2cerr = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Vcc).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(I2cerr).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/3;
        }

        /// <summary>
        /// Board voltage.
        /// OriginName: Vcc, Units: mV, IsExtended: false
        /// </summary>
        public ushort Vcc { get; set; }
        /// <summary>
        /// I2C error count.
        /// OriginName: I2Cerr, Units: , IsExtended: false
        /// </summary>
        public byte I2cerr { get; set; }
    }
    /// <summary>
    /// Status generated by radio.
    ///  RADIO
    /// </summary>
    public class RadioPacket: PacketV2<RadioPayload>
    {
	    public const int PacketMessageId = 166;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 21;

        public override RadioPayload Payload { get; } = new RadioPayload();

        public override string Name => "RADIO";
    }

    /// <summary>
    ///  RADIO
    /// </summary>
    public class RadioPayload : IPayload
    {
        public byte GetMaxByteSize() => 9; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 9; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Rxerrors = BinSerialize.ReadUShort(ref buffer);index+=2;
            Fixed = BinSerialize.ReadUShort(ref buffer);index+=2;
            Rssi = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Remrssi = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Txbuf = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Noise = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Remnoise = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUShort(ref buffer,Rxerrors);index+=2;
            BinSerialize.WriteUShort(ref buffer,Fixed);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)Rssi);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Remrssi);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Txbuf);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Noise);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Remnoise);index+=1;
            return index; // /*PayloadByteSize*/9;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Rxerrors = BitConverter.ToUInt16(buffer,index);index+=2;
            Fixed = BitConverter.ToUInt16(buffer,index);index+=2;
            Rssi = (byte)buffer[index++];
            Remrssi = (byte)buffer[index++];
            Txbuf = (byte)buffer[index++];
            Noise = (byte)buffer[index++];
            Remnoise = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Rxerrors).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Fixed).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Rssi).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Remrssi).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Txbuf).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Noise).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Remnoise).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/9;
        }

        /// <summary>
        /// Receive errors.
        /// OriginName: rxerrors, Units: , IsExtended: false
        /// </summary>
        public ushort Rxerrors { get; set; }
        /// <summary>
        /// Count of error corrected packets.
        /// OriginName: fixed, Units: , IsExtended: false
        /// </summary>
        public ushort Fixed { get; set; }
        /// <summary>
        /// Local signal strength.
        /// OriginName: rssi, Units: , IsExtended: false
        /// </summary>
        public byte Rssi { get; set; }
        /// <summary>
        /// Remote signal strength.
        /// OriginName: remrssi, Units: , IsExtended: false
        /// </summary>
        public byte Remrssi { get; set; }
        /// <summary>
        /// How full the tx buffer is.
        /// OriginName: txbuf, Units: %, IsExtended: false
        /// </summary>
        public byte Txbuf { get; set; }
        /// <summary>
        /// Background noise level.
        /// OriginName: noise, Units: , IsExtended: false
        /// </summary>
        public byte Noise { get; set; }
        /// <summary>
        /// Remote background noise level.
        /// OriginName: remnoise, Units: , IsExtended: false
        /// </summary>
        public byte Remnoise { get; set; }
    }
    /// <summary>
    /// Status of AP_Limits. Sent in extended status stream when AP_Limits is enabled.
    ///  LIMITS_STATUS
    /// </summary>
    public class LimitsStatusPacket: PacketV2<LimitsStatusPayload>
    {
	    public const int PacketMessageId = 167;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 144;

        public override LimitsStatusPayload Payload { get; } = new LimitsStatusPayload();

        public override string Name => "LIMITS_STATUS";
    }

    /// <summary>
    ///  LIMITS_STATUS
    /// </summary>
    public class LimitsStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 22; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 22; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            LastTrigger = BinSerialize.ReadUInt(ref buffer);index+=4;
            LastAction = BinSerialize.ReadUInt(ref buffer);index+=4;
            LastRecovery = BinSerialize.ReadUInt(ref buffer);index+=4;
            LastClear = BinSerialize.ReadUInt(ref buffer);index+=4;
            BreachCount = BinSerialize.ReadUShort(ref buffer);index+=2;
            LimitsState = (LimitsState)BinSerialize.ReadByte(ref buffer);index+=1;
            ModsEnabled = (LimitModule)BinSerialize.ReadByte(ref buffer);index+=1;
            ModsRequired = (LimitModule)BinSerialize.ReadByte(ref buffer);index+=1;
            ModsTriggered = (LimitModule)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,LastTrigger);index+=4;
            BinSerialize.WriteUInt(ref buffer,LastAction);index+=4;
            BinSerialize.WriteUInt(ref buffer,LastRecovery);index+=4;
            BinSerialize.WriteUInt(ref buffer,LastClear);index+=4;
            BinSerialize.WriteUShort(ref buffer,BreachCount);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)LimitsState);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)ModsEnabled);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)ModsRequired);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)ModsTriggered);index+=1;
            return index; // /*PayloadByteSize*/22;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            LastTrigger = BitConverter.ToUInt32(buffer,index);index+=4;
            LastAction = BitConverter.ToUInt32(buffer,index);index+=4;
            LastRecovery = BitConverter.ToUInt32(buffer,index);index+=4;
            LastClear = BitConverter.ToUInt32(buffer,index);index+=4;
            BreachCount = BitConverter.ToUInt16(buffer,index);index+=2;
            LimitsState = (LimitsState)buffer[index++];
            ModsEnabled = (LimitModule)buffer[index++];
            ModsRequired = (LimitModule)buffer[index++];
            ModsTriggered = (LimitModule)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(LastTrigger).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(LastAction).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(LastRecovery).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(LastClear).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(BreachCount).CopyTo(buffer, index);index+=2;
            buffer[index] = (byte)LimitsState;index+=1;
            buffer[index] = (byte)ModsEnabled;index+=1;
            buffer[index] = (byte)ModsRequired;index+=1;
            buffer[index] = (byte)ModsTriggered;index+=1;
            return index - start; // /*PayloadByteSize*/22;
        }

        /// <summary>
        /// Time (since boot) of last breach.
        /// OriginName: last_trigger, Units: ms, IsExtended: false
        /// </summary>
        public uint LastTrigger { get; set; }
        /// <summary>
        /// Time (since boot) of last recovery action.
        /// OriginName: last_action, Units: ms, IsExtended: false
        /// </summary>
        public uint LastAction { get; set; }
        /// <summary>
        /// Time (since boot) of last successful recovery.
        /// OriginName: last_recovery, Units: ms, IsExtended: false
        /// </summary>
        public uint LastRecovery { get; set; }
        /// <summary>
        /// Time (since boot) of last all-clear.
        /// OriginName: last_clear, Units: ms, IsExtended: false
        /// </summary>
        public uint LastClear { get; set; }
        /// <summary>
        /// Number of fence breaches.
        /// OriginName: breach_count, Units: , IsExtended: false
        /// </summary>
        public ushort BreachCount { get; set; }
        /// <summary>
        /// State of AP_Limits.
        /// OriginName: limits_state, Units: , IsExtended: false
        /// </summary>
        public LimitsState LimitsState { get; set; }
        /// <summary>
        /// AP_Limit_Module bitfield of enabled modules.
        /// OriginName: mods_enabled, Units: , IsExtended: false
        /// </summary>
        public LimitModule ModsEnabled { get; set; }
        /// <summary>
        /// AP_Limit_Module bitfield of required modules.
        /// OriginName: mods_required, Units: , IsExtended: false
        /// </summary>
        public LimitModule ModsRequired { get; set; }
        /// <summary>
        /// AP_Limit_Module bitfield of triggered modules.
        /// OriginName: mods_triggered, Units: , IsExtended: false
        /// </summary>
        public LimitModule ModsTriggered { get; set; }
    }
    /// <summary>
    /// Wind estimation.
    ///  WIND
    /// </summary>
    public class WindPacket: PacketV2<WindPayload>
    {
	    public const int PacketMessageId = 168;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 1;

        public override WindPayload Payload { get; } = new WindPayload();

        public override string Name => "WIND";
    }

    /// <summary>
    ///  WIND
    /// </summary>
    public class WindPayload : IPayload
    {
        public byte GetMaxByteSize() => 12; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 12; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Direction = BinSerialize.ReadFloat(ref buffer);index+=4;
            Speed = BinSerialize.ReadFloat(ref buffer);index+=4;
            SpeedZ = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Direction);index+=4;
            BinSerialize.WriteFloat(ref buffer,Speed);index+=4;
            BinSerialize.WriteFloat(ref buffer,SpeedZ);index+=4;
            return index; // /*PayloadByteSize*/12;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Direction = BitConverter.ToSingle(buffer, index);index+=4;
            Speed = BitConverter.ToSingle(buffer, index);index+=4;
            SpeedZ = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Direction).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Speed).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(SpeedZ).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/12;
        }

        /// <summary>
        /// Wind direction (that wind is coming from).
        /// OriginName: direction, Units: deg, IsExtended: false
        /// </summary>
        public float Direction { get; set; }
        /// <summary>
        /// Wind speed in ground plane.
        /// OriginName: speed, Units: m/s, IsExtended: false
        /// </summary>
        public float Speed { get; set; }
        /// <summary>
        /// Vertical wind speed.
        /// OriginName: speed_z, Units: m/s, IsExtended: false
        /// </summary>
        public float SpeedZ { get; set; }
    }
    /// <summary>
    /// Data packet, size 16.
    ///  DATA16
    /// </summary>
    public class Data16Packet: PacketV2<Data16Payload>
    {
	    public const int PacketMessageId = 169;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 234;

        public override Data16Payload Payload { get; } = new Data16Payload();

        public override string Name => "DATA16";
    }

    /// <summary>
    ///  DATA16
    /// </summary>
    public class Data16Payload : IPayload
    {
        public byte GetMaxByteSize() => 18; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 18; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Type = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Len = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/18 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)Type);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Len);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/18;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Type = (byte)buffer[index++];
            Len = (byte)buffer[index++];
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/18 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Type).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Len).CopyTo(buffer, index);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                buffer[index] = (byte)Data[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/18;
        }

        /// <summary>
        /// Data type.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// Data length.
        /// OriginName: len, Units: bytes, IsExtended: false
        /// </summary>
        public byte Len { get; set; }
        /// <summary>
        /// Raw data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public byte[] Data { get; set; } = new byte[16];
        public byte GetDataMaxItemsCount() => 16;
    }
    /// <summary>
    /// Data packet, size 32.
    ///  DATA32
    /// </summary>
    public class Data32Packet: PacketV2<Data32Payload>
    {
	    public const int PacketMessageId = 170;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 73;

        public override Data32Payload Payload { get; } = new Data32Payload();

        public override string Name => "DATA32";
    }

    /// <summary>
    ///  DATA32
    /// </summary>
    public class Data32Payload : IPayload
    {
        public byte GetMaxByteSize() => 34; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 34; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Type = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Len = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/34 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)Type);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Len);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/34;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Type = (byte)buffer[index++];
            Len = (byte)buffer[index++];
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/34 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Type).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Len).CopyTo(buffer, index);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                buffer[index] = (byte)Data[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/34;
        }

        /// <summary>
        /// Data type.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// Data length.
        /// OriginName: len, Units: bytes, IsExtended: false
        /// </summary>
        public byte Len { get; set; }
        /// <summary>
        /// Raw data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public byte[] Data { get; set; } = new byte[32];
        public byte GetDataMaxItemsCount() => 32;
    }
    /// <summary>
    /// Data packet, size 64.
    ///  DATA64
    /// </summary>
    public class Data64Packet: PacketV2<Data64Payload>
    {
	    public const int PacketMessageId = 171;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 181;

        public override Data64Payload Payload { get; } = new Data64Payload();

        public override string Name => "DATA64";
    }

    /// <summary>
    ///  DATA64
    /// </summary>
    public class Data64Payload : IPayload
    {
        public byte GetMaxByteSize() => 66; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 66; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Type = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Len = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/64 - Math.Max(0,((/*PayloadByteSize*/66 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)Type);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Len);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/66;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Type = (byte)buffer[index++];
            Len = (byte)buffer[index++];
            arraySize = /*ArrayLength*/64 - Math.Max(0,((/*PayloadByteSize*/66 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Type).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Len).CopyTo(buffer, index);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                buffer[index] = (byte)Data[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/66;
        }

        /// <summary>
        /// Data type.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// Data length.
        /// OriginName: len, Units: bytes, IsExtended: false
        /// </summary>
        public byte Len { get; set; }
        /// <summary>
        /// Raw data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public byte[] Data { get; set; } = new byte[64];
        public byte GetDataMaxItemsCount() => 64;
    }
    /// <summary>
    /// Data packet, size 96.
    ///  DATA96
    /// </summary>
    public class Data96Packet: PacketV2<Data96Payload>
    {
	    public const int PacketMessageId = 172;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 22;

        public override Data96Payload Payload { get; } = new Data96Payload();

        public override string Name => "DATA96";
    }

    /// <summary>
    ///  DATA96
    /// </summary>
    public class Data96Payload : IPayload
    {
        public byte GetMaxByteSize() => 98; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 98; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Type = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Len = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/96 - Math.Max(0,((/*PayloadByteSize*/98 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)Type);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Len);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/98;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Type = (byte)buffer[index++];
            Len = (byte)buffer[index++];
            arraySize = /*ArrayLength*/96 - Math.Max(0,((/*PayloadByteSize*/98 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Type).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Len).CopyTo(buffer, index);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                buffer[index] = (byte)Data[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/98;
        }

        /// <summary>
        /// Data type.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// Data length.
        /// OriginName: len, Units: bytes, IsExtended: false
        /// </summary>
        public byte Len { get; set; }
        /// <summary>
        /// Raw data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public byte[] Data { get; set; } = new byte[96];
        public byte GetDataMaxItemsCount() => 96;
    }
    /// <summary>
    /// Rangefinder reporting.
    ///  RANGEFINDER
    /// </summary>
    public class RangefinderPacket: PacketV2<RangefinderPayload>
    {
	    public const int PacketMessageId = 173;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 83;

        public override RangefinderPayload Payload { get; } = new RangefinderPayload();

        public override string Name => "RANGEFINDER";
    }

    /// <summary>
    ///  RANGEFINDER
    /// </summary>
    public class RangefinderPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Distance = BinSerialize.ReadFloat(ref buffer);index+=4;
            Voltage = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Distance);index+=4;
            BinSerialize.WriteFloat(ref buffer,Voltage);index+=4;
            return index; // /*PayloadByteSize*/8;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Distance = BitConverter.ToSingle(buffer, index);index+=4;
            Voltage = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Distance).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Voltage).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/8;
        }

        /// <summary>
        /// Distance.
        /// OriginName: distance, Units: m, IsExtended: false
        /// </summary>
        public float Distance { get; set; }
        /// <summary>
        /// Raw voltage if available, zero otherwise.
        /// OriginName: voltage, Units: V, IsExtended: false
        /// </summary>
        public float Voltage { get; set; }
    }
    /// <summary>
    /// Airspeed auto-calibration.
    ///  AIRSPEED_AUTOCAL
    /// </summary>
    public class AirspeedAutocalPacket: PacketV2<AirspeedAutocalPayload>
    {
	    public const int PacketMessageId = 174;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 167;

        public override AirspeedAutocalPayload Payload { get; } = new AirspeedAutocalPayload();

        public override string Name => "AIRSPEED_AUTOCAL";
    }

    /// <summary>
    ///  AIRSPEED_AUTOCAL
    /// </summary>
    public class AirspeedAutocalPayload : IPayload
    {
        public byte GetMaxByteSize() => 48; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 48; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Vx = BinSerialize.ReadFloat(ref buffer);index+=4;
            Vy = BinSerialize.ReadFloat(ref buffer);index+=4;
            Vz = BinSerialize.ReadFloat(ref buffer);index+=4;
            DiffPressure = BinSerialize.ReadFloat(ref buffer);index+=4;
            Eas2tas = BinSerialize.ReadFloat(ref buffer);index+=4;
            Ratio = BinSerialize.ReadFloat(ref buffer);index+=4;
            StateX = BinSerialize.ReadFloat(ref buffer);index+=4;
            StateY = BinSerialize.ReadFloat(ref buffer);index+=4;
            StateZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            Pax = BinSerialize.ReadFloat(ref buffer);index+=4;
            Pby = BinSerialize.ReadFloat(ref buffer);index+=4;
            Pcz = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Vx);index+=4;
            BinSerialize.WriteFloat(ref buffer,Vy);index+=4;
            BinSerialize.WriteFloat(ref buffer,Vz);index+=4;
            BinSerialize.WriteFloat(ref buffer,DiffPressure);index+=4;
            BinSerialize.WriteFloat(ref buffer,Eas2tas);index+=4;
            BinSerialize.WriteFloat(ref buffer,Ratio);index+=4;
            BinSerialize.WriteFloat(ref buffer,StateX);index+=4;
            BinSerialize.WriteFloat(ref buffer,StateY);index+=4;
            BinSerialize.WriteFloat(ref buffer,StateZ);index+=4;
            BinSerialize.WriteFloat(ref buffer,Pax);index+=4;
            BinSerialize.WriteFloat(ref buffer,Pby);index+=4;
            BinSerialize.WriteFloat(ref buffer,Pcz);index+=4;
            return index; // /*PayloadByteSize*/48;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Vx = BitConverter.ToSingle(buffer, index);index+=4;
            Vy = BitConverter.ToSingle(buffer, index);index+=4;
            Vz = BitConverter.ToSingle(buffer, index);index+=4;
            DiffPressure = BitConverter.ToSingle(buffer, index);index+=4;
            Eas2tas = BitConverter.ToSingle(buffer, index);index+=4;
            Ratio = BitConverter.ToSingle(buffer, index);index+=4;
            StateX = BitConverter.ToSingle(buffer, index);index+=4;
            StateY = BitConverter.ToSingle(buffer, index);index+=4;
            StateZ = BitConverter.ToSingle(buffer, index);index+=4;
            Pax = BitConverter.ToSingle(buffer, index);index+=4;
            Pby = BitConverter.ToSingle(buffer, index);index+=4;
            Pcz = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Vx).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Vy).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Vz).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DiffPressure).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Eas2tas).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Ratio).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(StateX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(StateY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(StateZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Pax).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Pby).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Pcz).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/48;
        }

        /// <summary>
        /// GPS velocity north.
        /// OriginName: vx, Units: m/s, IsExtended: false
        /// </summary>
        public float Vx { get; set; }
        /// <summary>
        /// GPS velocity east.
        /// OriginName: vy, Units: m/s, IsExtended: false
        /// </summary>
        public float Vy { get; set; }
        /// <summary>
        /// GPS velocity down.
        /// OriginName: vz, Units: m/s, IsExtended: false
        /// </summary>
        public float Vz { get; set; }
        /// <summary>
        /// Differential pressure.
        /// OriginName: diff_pressure, Units: Pa, IsExtended: false
        /// </summary>
        public float DiffPressure { get; set; }
        /// <summary>
        /// Estimated to true airspeed ratio.
        /// OriginName: EAS2TAS, Units: , IsExtended: false
        /// </summary>
        public float Eas2tas { get; set; }
        /// <summary>
        /// Airspeed ratio.
        /// OriginName: ratio, Units: , IsExtended: false
        /// </summary>
        public float Ratio { get; set; }
        /// <summary>
        /// EKF state x.
        /// OriginName: state_x, Units: , IsExtended: false
        /// </summary>
        public float StateX { get; set; }
        /// <summary>
        /// EKF state y.
        /// OriginName: state_y, Units: , IsExtended: false
        /// </summary>
        public float StateY { get; set; }
        /// <summary>
        /// EKF state z.
        /// OriginName: state_z, Units: , IsExtended: false
        /// </summary>
        public float StateZ { get; set; }
        /// <summary>
        /// EKF Pax.
        /// OriginName: Pax, Units: , IsExtended: false
        /// </summary>
        public float Pax { get; set; }
        /// <summary>
        /// EKF Pby.
        /// OriginName: Pby, Units: , IsExtended: false
        /// </summary>
        public float Pby { get; set; }
        /// <summary>
        /// EKF Pcz.
        /// OriginName: Pcz, Units: , IsExtended: false
        /// </summary>
        public float Pcz { get; set; }
    }
    /// <summary>
    /// A rally point. Used to set a point when from GCS -> MAV. Also used to return a point from MAV -> GCS.
    ///  RALLY_POINT
    /// </summary>
    public class RallyPointPacket: PacketV2<RallyPointPayload>
    {
	    public const int PacketMessageId = 175;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 138;

        public override RallyPointPayload Payload { get; } = new RallyPointPayload();

        public override string Name => "RALLY_POINT";
    }

    /// <summary>
    ///  RALLY_POINT
    /// </summary>
    public class RallyPointPayload : IPayload
    {
        public byte GetMaxByteSize() => 19; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 19; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Lat = BinSerialize.ReadInt(ref buffer);index+=4;
            Lng = BinSerialize.ReadInt(ref buffer);index+=4;
            Alt = BinSerialize.ReadShort(ref buffer);index+=2;
            BreakAlt = BinSerialize.ReadShort(ref buffer);index+=2;
            LandDir = BinSerialize.ReadUShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Idx = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Count = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Flags = (RallyFlags)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteInt(ref buffer,Lat);index+=4;
            BinSerialize.WriteInt(ref buffer,Lng);index+=4;
            BinSerialize.WriteShort(ref buffer,Alt);index+=2;
            BinSerialize.WriteShort(ref buffer,BreakAlt);index+=2;
            BinSerialize.WriteUShort(ref buffer,LandDir);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Idx);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Count);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Flags);index+=1;
            return index; // /*PayloadByteSize*/19;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Lat = BitConverter.ToInt32(buffer,index);index+=4;
            Lng = BitConverter.ToInt32(buffer,index);index+=4;
            Alt = BitConverter.ToInt16(buffer,index);index+=2;
            BreakAlt = BitConverter.ToInt16(buffer,index);index+=2;
            LandDir = BitConverter.ToUInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Idx = (byte)buffer[index++];
            Count = (byte)buffer[index++];
            Flags = (RallyFlags)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Lat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Lng).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Alt).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(BreakAlt).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(LandDir).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Idx).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Count).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Flags;index+=1;
            return index - start; // /*PayloadByteSize*/19;
        }

        /// <summary>
        /// Latitude of point.
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Longitude of point.
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public int Lng { get; set; }
        /// <summary>
        /// Transit / loiter altitude relative to home.
        /// OriginName: alt, Units: m, IsExtended: false
        /// </summary>
        public short Alt { get; set; }
        /// <summary>
        /// Break altitude relative to home.
        /// OriginName: break_alt, Units: m, IsExtended: false
        /// </summary>
        public short BreakAlt { get; set; }
        /// <summary>
        /// Heading to aim for when landing.
        /// OriginName: land_dir, Units: cdeg, IsExtended: false
        /// </summary>
        public ushort LandDir { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Point index (first point is 0).
        /// OriginName: idx, Units: , IsExtended: false
        /// </summary>
        public byte Idx { get; set; }
        /// <summary>
        /// Total number of points (for sanity checking).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public byte Count { get; set; }
        /// <summary>
        /// Configuration flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public RallyFlags Flags { get; set; }
    }
    /// <summary>
    /// Request a current rally point from MAV. MAV should respond with a RALLY_POINT message. MAV should not respond if the request is invalid.
    ///  RALLY_FETCH_POINT
    /// </summary>
    public class RallyFetchPointPacket: PacketV2<RallyFetchPointPayload>
    {
	    public const int PacketMessageId = 176;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 234;

        public override RallyFetchPointPayload Payload { get; } = new RallyFetchPointPayload();

        public override string Name => "RALLY_FETCH_POINT";
    }

    /// <summary>
    ///  RALLY_FETCH_POINT
    /// </summary>
    public class RallyFetchPointPayload : IPayload
    {
        public byte GetMaxByteSize() => 3; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Idx = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Idx);index+=1;
            return index; // /*PayloadByteSize*/3;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Idx = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Idx).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/3;
        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Point index (first point is 0).
        /// OriginName: idx, Units: , IsExtended: false
        /// </summary>
        public byte Idx { get; set; }
    }
    /// <summary>
    /// Status of compassmot calibration.
    ///  COMPASSMOT_STATUS
    /// </summary>
    public class CompassmotStatusPacket: PacketV2<CompassmotStatusPayload>
    {
	    public const int PacketMessageId = 177;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 240;

        public override CompassmotStatusPayload Payload { get; } = new CompassmotStatusPayload();

        public override string Name => "COMPASSMOT_STATUS";
    }

    /// <summary>
    ///  COMPASSMOT_STATUS
    /// </summary>
    public class CompassmotStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 20; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Current = BinSerialize.ReadFloat(ref buffer);index+=4;
            Compensationx = BinSerialize.ReadFloat(ref buffer);index+=4;
            Compensationy = BinSerialize.ReadFloat(ref buffer);index+=4;
            Compensationz = BinSerialize.ReadFloat(ref buffer);index+=4;
            Throttle = BinSerialize.ReadUShort(ref buffer);index+=2;
            Interference = BinSerialize.ReadUShort(ref buffer);index+=2;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Current);index+=4;
            BinSerialize.WriteFloat(ref buffer,Compensationx);index+=4;
            BinSerialize.WriteFloat(ref buffer,Compensationy);index+=4;
            BinSerialize.WriteFloat(ref buffer,Compensationz);index+=4;
            BinSerialize.WriteUShort(ref buffer,Throttle);index+=2;
            BinSerialize.WriteUShort(ref buffer,Interference);index+=2;
            return index; // /*PayloadByteSize*/20;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Current = BitConverter.ToSingle(buffer, index);index+=4;
            Compensationx = BitConverter.ToSingle(buffer, index);index+=4;
            Compensationy = BitConverter.ToSingle(buffer, index);index+=4;
            Compensationz = BitConverter.ToSingle(buffer, index);index+=4;
            Throttle = BitConverter.ToUInt16(buffer,index);index+=2;
            Interference = BitConverter.ToUInt16(buffer,index);index+=2;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Current).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Compensationx).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Compensationy).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Compensationz).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Throttle).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(Interference).CopyTo(buffer, index);index+=2;
            return index - start; // /*PayloadByteSize*/20;
        }

        /// <summary>
        /// Current.
        /// OriginName: current, Units: A, IsExtended: false
        /// </summary>
        public float Current { get; set; }
        /// <summary>
        /// Motor Compensation X.
        /// OriginName: CompensationX, Units: , IsExtended: false
        /// </summary>
        public float Compensationx { get; set; }
        /// <summary>
        /// Motor Compensation Y.
        /// OriginName: CompensationY, Units: , IsExtended: false
        /// </summary>
        public float Compensationy { get; set; }
        /// <summary>
        /// Motor Compensation Z.
        /// OriginName: CompensationZ, Units: , IsExtended: false
        /// </summary>
        public float Compensationz { get; set; }
        /// <summary>
        /// Throttle.
        /// OriginName: throttle, Units: d%, IsExtended: false
        /// </summary>
        public ushort Throttle { get; set; }
        /// <summary>
        /// Interference.
        /// OriginName: interference, Units: %, IsExtended: false
        /// </summary>
        public ushort Interference { get; set; }
    }
    /// <summary>
    /// Status of secondary AHRS filter if available.
    ///  AHRS2
    /// </summary>
    public class Ahrs2Packet: PacketV2<Ahrs2Payload>
    {
	    public const int PacketMessageId = 178;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 47;

        public override Ahrs2Payload Payload { get; } = new Ahrs2Payload();

        public override string Name => "AHRS2";
    }

    /// <summary>
    ///  AHRS2
    /// </summary>
    public class Ahrs2Payload : IPayload
    {
        public byte GetMaxByteSize() => 24; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Roll = BinSerialize.ReadFloat(ref buffer);index+=4;
            Pitch = BinSerialize.ReadFloat(ref buffer);index+=4;
            Yaw = BinSerialize.ReadFloat(ref buffer);index+=4;
            Altitude = BinSerialize.ReadFloat(ref buffer);index+=4;
            Lat = BinSerialize.ReadInt(ref buffer);index+=4;
            Lng = BinSerialize.ReadInt(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Roll);index+=4;
            BinSerialize.WriteFloat(ref buffer,Pitch);index+=4;
            BinSerialize.WriteFloat(ref buffer,Yaw);index+=4;
            BinSerialize.WriteFloat(ref buffer,Altitude);index+=4;
            BinSerialize.WriteInt(ref buffer,Lat);index+=4;
            BinSerialize.WriteInt(ref buffer,Lng);index+=4;
            return index; // /*PayloadByteSize*/24;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Roll = BitConverter.ToSingle(buffer, index);index+=4;
            Pitch = BitConverter.ToSingle(buffer, index);index+=4;
            Yaw = BitConverter.ToSingle(buffer, index);index+=4;
            Altitude = BitConverter.ToSingle(buffer, index);index+=4;
            Lat = BitConverter.ToInt32(buffer,index);index+=4;
            Lng = BitConverter.ToInt32(buffer,index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Roll).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Pitch).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Yaw).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Altitude).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Lat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Lng).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/24;
        }

        /// <summary>
        /// Roll angle.
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
        /// <summary>
        /// Pitch angle.
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw angle.
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Altitude (MSL).
        /// OriginName: altitude, Units: m, IsExtended: false
        /// </summary>
        public float Altitude { get; set; }
        /// <summary>
        /// Latitude.
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Longitude.
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public int Lng { get; set; }
    }
    /// <summary>
    /// Camera Event.
    ///  CAMERA_STATUS
    /// </summary>
    public class CameraStatusPacket: PacketV2<CameraStatusPayload>
    {
	    public const int PacketMessageId = 179;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 189;

        public override CameraStatusPayload Payload { get; } = new CameraStatusPayload();

        public override string Name => "CAMERA_STATUS";
    }

    /// <summary>
    ///  CAMERA_STATUS
    /// </summary>
    public class CameraStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 29; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 29; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TimeUsec = BinSerialize.ReadULong(ref buffer);index+=8;
            P1 = BinSerialize.ReadFloat(ref buffer);index+=4;
            P2 = BinSerialize.ReadFloat(ref buffer);index+=4;
            P3 = BinSerialize.ReadFloat(ref buffer);index+=4;
            P4 = BinSerialize.ReadFloat(ref buffer);index+=4;
            ImgIdx = BinSerialize.ReadUShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CamIdx = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            EventId = (CameraStatusTypes)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,TimeUsec);index+=8;
            BinSerialize.WriteFloat(ref buffer,P1);index+=4;
            BinSerialize.WriteFloat(ref buffer,P2);index+=4;
            BinSerialize.WriteFloat(ref buffer,P3);index+=4;
            BinSerialize.WriteFloat(ref buffer,P4);index+=4;
            BinSerialize.WriteUShort(ref buffer,ImgIdx);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CamIdx);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)EventId);index+=1;
            return index; // /*PayloadByteSize*/29;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TimeUsec = BitConverter.ToUInt64(buffer,index);index+=8;
            P1 = BitConverter.ToSingle(buffer, index);index+=4;
            P2 = BitConverter.ToSingle(buffer, index);index+=4;
            P3 = BitConverter.ToSingle(buffer, index);index+=4;
            P4 = BitConverter.ToSingle(buffer, index);index+=4;
            ImgIdx = BitConverter.ToUInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            CamIdx = (byte)buffer[index++];
            EventId = (CameraStatusTypes)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TimeUsec).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(P1).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(P2).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(P3).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(P4).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(ImgIdx).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(CamIdx).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)EventId;index+=1;
            return index - start; // /*PayloadByteSize*/29;
        }

        /// <summary>
        /// Image timestamp (since UNIX epoch, according to camera clock).
        /// OriginName: time_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUsec { get; set; }
        /// <summary>
        /// Parameter 1 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).
        /// OriginName: p1, Units: , IsExtended: false
        /// </summary>
        public float P1 { get; set; }
        /// <summary>
        /// Parameter 2 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).
        /// OriginName: p2, Units: , IsExtended: false
        /// </summary>
        public float P2 { get; set; }
        /// <summary>
        /// Parameter 3 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).
        /// OriginName: p3, Units: , IsExtended: false
        /// </summary>
        public float P3 { get; set; }
        /// <summary>
        /// Parameter 4 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).
        /// OriginName: p4, Units: , IsExtended: false
        /// </summary>
        public float P4 { get; set; }
        /// <summary>
        /// Image index.
        /// OriginName: img_idx, Units: , IsExtended: false
        /// </summary>
        public ushort ImgIdx { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Camera ID.
        /// OriginName: cam_idx, Units: , IsExtended: false
        /// </summary>
        public byte CamIdx { get; set; }
        /// <summary>
        /// Event type.
        /// OriginName: event_id, Units: , IsExtended: false
        /// </summary>
        public CameraStatusTypes EventId { get; set; }
    }
    /// <summary>
    /// Camera Capture Feedback.
    ///  CAMERA_FEEDBACK
    /// </summary>
    public class CameraFeedbackPacket: PacketV2<CameraFeedbackPayload>
    {
	    public const int PacketMessageId = 180;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 52;

        public override CameraFeedbackPayload Payload { get; } = new CameraFeedbackPayload();

        public override string Name => "CAMERA_FEEDBACK";
    }

    /// <summary>
    ///  CAMERA_FEEDBACK
    /// </summary>
    public class CameraFeedbackPayload : IPayload
    {
        public byte GetMaxByteSize() => 47; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 47; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TimeUsec = BinSerialize.ReadULong(ref buffer);index+=8;
            Lat = BinSerialize.ReadInt(ref buffer);index+=4;
            Lng = BinSerialize.ReadInt(ref buffer);index+=4;
            AltMsl = BinSerialize.ReadFloat(ref buffer);index+=4;
            AltRel = BinSerialize.ReadFloat(ref buffer);index+=4;
            Roll = BinSerialize.ReadFloat(ref buffer);index+=4;
            Pitch = BinSerialize.ReadFloat(ref buffer);index+=4;
            Yaw = BinSerialize.ReadFloat(ref buffer);index+=4;
            FocLen = BinSerialize.ReadFloat(ref buffer);index+=4;
            ImgIdx = BinSerialize.ReadUShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CamIdx = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Flags = (CameraFeedbackFlags)BinSerialize.ReadByte(ref buffer);index+=1;
            // extended field 'CompletedCaptures' can be empty
            if (index >= endIndex) return;
            CompletedCaptures = BinSerialize.ReadUShort(ref buffer);index+=2;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,TimeUsec);index+=8;
            BinSerialize.WriteInt(ref buffer,Lat);index+=4;
            BinSerialize.WriteInt(ref buffer,Lng);index+=4;
            BinSerialize.WriteFloat(ref buffer,AltMsl);index+=4;
            BinSerialize.WriteFloat(ref buffer,AltRel);index+=4;
            BinSerialize.WriteFloat(ref buffer,Roll);index+=4;
            BinSerialize.WriteFloat(ref buffer,Pitch);index+=4;
            BinSerialize.WriteFloat(ref buffer,Yaw);index+=4;
            BinSerialize.WriteFloat(ref buffer,FocLen);index+=4;
            BinSerialize.WriteUShort(ref buffer,ImgIdx);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CamIdx);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Flags);index+=1;
            BinSerialize.WriteUShort(ref buffer,CompletedCaptures);index+=2;
            return index; // /*PayloadByteSize*/47;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TimeUsec = BitConverter.ToUInt64(buffer,index);index+=8;
            Lat = BitConverter.ToInt32(buffer,index);index+=4;
            Lng = BitConverter.ToInt32(buffer,index);index+=4;
            AltMsl = BitConverter.ToSingle(buffer, index);index+=4;
            AltRel = BitConverter.ToSingle(buffer, index);index+=4;
            Roll = BitConverter.ToSingle(buffer, index);index+=4;
            Pitch = BitConverter.ToSingle(buffer, index);index+=4;
            Yaw = BitConverter.ToSingle(buffer, index);index+=4;
            FocLen = BitConverter.ToSingle(buffer, index);index+=4;
            ImgIdx = BitConverter.ToUInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            CamIdx = (byte)buffer[index++];
            Flags = (CameraFeedbackFlags)buffer[index++];
            // extended field 'CompletedCaptures' can be empty
            if (index >= endIndex) return;
            CompletedCaptures = BitConverter.ToUInt16(buffer,index);index+=2;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TimeUsec).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(Lat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Lng).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AltMsl).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(AltRel).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Roll).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Pitch).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Yaw).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(FocLen).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(ImgIdx).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(CamIdx).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Flags;index+=1;
            BitConverter.GetBytes(CompletedCaptures).CopyTo(buffer, index);index+=2;
            return index - start; // /*PayloadByteSize*/47;
        }

        /// <summary>
        /// Image timestamp (since UNIX epoch), as passed in by CAMERA_STATUS message (or autopilot if no CCB).
        /// OriginName: time_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUsec { get; set; }
        /// <summary>
        /// Latitude.
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Longitude.
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public int Lng { get; set; }
        /// <summary>
        /// Altitude (MSL).
        /// OriginName: alt_msl, Units: m, IsExtended: false
        /// </summary>
        public float AltMsl { get; set; }
        /// <summary>
        /// Altitude (Relative to HOME location).
        /// OriginName: alt_rel, Units: m, IsExtended: false
        /// </summary>
        public float AltRel { get; set; }
        /// <summary>
        /// Camera Roll angle (earth frame, +-180).
        /// OriginName: roll, Units: deg, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
        /// <summary>
        /// Camera Pitch angle (earth frame, +-180).
        /// OriginName: pitch, Units: deg, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Camera Yaw (earth frame, 0-360, true).
        /// OriginName: yaw, Units: deg, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Focal Length.
        /// OriginName: foc_len, Units: mm, IsExtended: false
        /// </summary>
        public float FocLen { get; set; }
        /// <summary>
        /// Image index.
        /// OriginName: img_idx, Units: , IsExtended: false
        /// </summary>
        public ushort ImgIdx { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Camera ID.
        /// OriginName: cam_idx, Units: , IsExtended: false
        /// </summary>
        public byte CamIdx { get; set; }
        /// <summary>
        /// Feedback flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public CameraFeedbackFlags Flags { get; set; }
        /// <summary>
        /// Completed image captures.
        /// OriginName: completed_captures, Units: , IsExtended: true
        /// </summary>
        public ushort CompletedCaptures { get; set; }
    }
    /// <summary>
    /// 2nd Battery status
    ///  BATTERY2
    /// </summary>
    public class Battery2Packet: PacketV2<Battery2Payload>
    {
	    public const int PacketMessageId = 181;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 174;

        public override Battery2Payload Payload { get; } = new Battery2Payload();

        public override string Name => "BATTERY2";
    }

    /// <summary>
    ///  BATTERY2
    /// </summary>
    public class Battery2Payload : IPayload
    {
        public byte GetMaxByteSize() => 4; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 4; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Voltage = BinSerialize.ReadUShort(ref buffer);index+=2;
            CurrentBattery = BinSerialize.ReadShort(ref buffer);index+=2;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUShort(ref buffer,Voltage);index+=2;
            BinSerialize.WriteShort(ref buffer,CurrentBattery);index+=2;
            return index; // /*PayloadByteSize*/4;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Voltage = BitConverter.ToUInt16(buffer,index);index+=2;
            CurrentBattery = BitConverter.ToInt16(buffer,index);index+=2;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Voltage).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(CurrentBattery).CopyTo(buffer, index);index+=2;
            return index - start; // /*PayloadByteSize*/4;
        }

        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: mV, IsExtended: false
        /// </summary>
        public ushort Voltage { get; set; }
        /// <summary>
        /// Battery current, -1: autopilot does not measure the current.
        /// OriginName: current_battery, Units: cA, IsExtended: false
        /// </summary>
        public short CurrentBattery { get; set; }
    }
    /// <summary>
    /// Status of third AHRS filter if available. This is for ANU research group (Ali and Sean).
    ///  AHRS3
    /// </summary>
    public class Ahrs3Packet: PacketV2<Ahrs3Payload>
    {
	    public const int PacketMessageId = 182;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 229;

        public override Ahrs3Payload Payload { get; } = new Ahrs3Payload();

        public override string Name => "AHRS3";
    }

    /// <summary>
    ///  AHRS3
    /// </summary>
    public class Ahrs3Payload : IPayload
    {
        public byte GetMaxByteSize() => 40; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 40; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Roll = BinSerialize.ReadFloat(ref buffer);index+=4;
            Pitch = BinSerialize.ReadFloat(ref buffer);index+=4;
            Yaw = BinSerialize.ReadFloat(ref buffer);index+=4;
            Altitude = BinSerialize.ReadFloat(ref buffer);index+=4;
            Lat = BinSerialize.ReadInt(ref buffer);index+=4;
            Lng = BinSerialize.ReadInt(ref buffer);index+=4;
            V1 = BinSerialize.ReadFloat(ref buffer);index+=4;
            V2 = BinSerialize.ReadFloat(ref buffer);index+=4;
            V3 = BinSerialize.ReadFloat(ref buffer);index+=4;
            V4 = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Roll);index+=4;
            BinSerialize.WriteFloat(ref buffer,Pitch);index+=4;
            BinSerialize.WriteFloat(ref buffer,Yaw);index+=4;
            BinSerialize.WriteFloat(ref buffer,Altitude);index+=4;
            BinSerialize.WriteInt(ref buffer,Lat);index+=4;
            BinSerialize.WriteInt(ref buffer,Lng);index+=4;
            BinSerialize.WriteFloat(ref buffer,V1);index+=4;
            BinSerialize.WriteFloat(ref buffer,V2);index+=4;
            BinSerialize.WriteFloat(ref buffer,V3);index+=4;
            BinSerialize.WriteFloat(ref buffer,V4);index+=4;
            return index; // /*PayloadByteSize*/40;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Roll = BitConverter.ToSingle(buffer, index);index+=4;
            Pitch = BitConverter.ToSingle(buffer, index);index+=4;
            Yaw = BitConverter.ToSingle(buffer, index);index+=4;
            Altitude = BitConverter.ToSingle(buffer, index);index+=4;
            Lat = BitConverter.ToInt32(buffer,index);index+=4;
            Lng = BitConverter.ToInt32(buffer,index);index+=4;
            V1 = BitConverter.ToSingle(buffer, index);index+=4;
            V2 = BitConverter.ToSingle(buffer, index);index+=4;
            V3 = BitConverter.ToSingle(buffer, index);index+=4;
            V4 = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Roll).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Pitch).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Yaw).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Altitude).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Lat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Lng).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(V1).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(V2).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(V3).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(V4).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/40;
        }

        /// <summary>
        /// Roll angle.
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
        /// <summary>
        /// Pitch angle.
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw angle.
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Altitude (MSL).
        /// OriginName: altitude, Units: m, IsExtended: false
        /// </summary>
        public float Altitude { get; set; }
        /// <summary>
        /// Latitude.
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Longitude.
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public int Lng { get; set; }
        /// <summary>
        /// Test variable1.
        /// OriginName: v1, Units: , IsExtended: false
        /// </summary>
        public float V1 { get; set; }
        /// <summary>
        /// Test variable2.
        /// OriginName: v2, Units: , IsExtended: false
        /// </summary>
        public float V2 { get; set; }
        /// <summary>
        /// Test variable3.
        /// OriginName: v3, Units: , IsExtended: false
        /// </summary>
        public float V3 { get; set; }
        /// <summary>
        /// Test variable4.
        /// OriginName: v4, Units: , IsExtended: false
        /// </summary>
        public float V4 { get; set; }
    }
    /// <summary>
    /// Request the autopilot version from the system/component.
    ///  AUTOPILOT_VERSION_REQUEST
    /// </summary>
    public class AutopilotVersionRequestPacket: PacketV2<AutopilotVersionRequestPayload>
    {
	    public const int PacketMessageId = 183;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 85;

        public override AutopilotVersionRequestPayload Payload { get; } = new AutopilotVersionRequestPayload();

        public override string Name => "AUTOPILOT_VERSION_REQUEST";
    }

    /// <summary>
    ///  AUTOPILOT_VERSION_REQUEST
    /// </summary>
    public class AutopilotVersionRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 2; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 2; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            return index; // /*PayloadByteSize*/2;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/2;
        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
    }
    /// <summary>
    /// Send a block of log data to remote location.
    ///  REMOTE_LOG_DATA_BLOCK
    /// </summary>
    public class RemoteLogDataBlockPacket: PacketV2<RemoteLogDataBlockPayload>
    {
	    public const int PacketMessageId = 184;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 159;

        public override RemoteLogDataBlockPayload Payload { get; } = new RemoteLogDataBlockPayload();

        public override string Name => "REMOTE_LOG_DATA_BLOCK";
    }

    /// <summary>
    ///  REMOTE_LOG_DATA_BLOCK
    /// </summary>
    public class RemoteLogDataBlockPayload : IPayload
    {
        public byte GetMaxByteSize() => 206; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 206; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Seqno = (MavRemoteLogDataBlockCommands)BinSerialize.ReadUInt(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/200 - Math.Max(0,((/*PayloadByteSize*/206 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,(uint)Seqno);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/206;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Seqno = (MavRemoteLogDataBlockCommands)BitConverter.ToUInt32(buffer,index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            arraySize = /*ArrayLength*/200 - Math.Max(0,((/*PayloadByteSize*/206 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes((uint)Seqno).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                buffer[index] = (byte)Data[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/206;
        }

        /// <summary>
        /// Log data block sequence number.
        /// OriginName: seqno, Units: , IsExtended: false
        /// </summary>
        public MavRemoteLogDataBlockCommands Seqno { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Log data block.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public byte[] Data { get; set; } = new byte[200];
        public byte GetDataMaxItemsCount() => 200;
    }
    /// <summary>
    /// Send Status of each log block that autopilot board might have sent.
    ///  REMOTE_LOG_BLOCK_STATUS
    /// </summary>
    public class RemoteLogBlockStatusPacket: PacketV2<RemoteLogBlockStatusPayload>
    {
	    public const int PacketMessageId = 185;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 186;

        public override RemoteLogBlockStatusPayload Payload { get; } = new RemoteLogBlockStatusPayload();

        public override string Name => "REMOTE_LOG_BLOCK_STATUS";
    }

    /// <summary>
    ///  REMOTE_LOG_BLOCK_STATUS
    /// </summary>
    public class RemoteLogBlockStatusPayload : IPayload
    {
        public byte GetMaxByteSize() => 7; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Seqno = BinSerialize.ReadUInt(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Status = (MavRemoteLogDataBlockStatuses)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,Seqno);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Status);index+=1;
            return index; // /*PayloadByteSize*/7;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Seqno = BitConverter.ToUInt32(buffer,index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Status = (MavRemoteLogDataBlockStatuses)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Seqno).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Status;index+=1;
            return index - start; // /*PayloadByteSize*/7;
        }

        /// <summary>
        /// Log data block sequence number.
        /// OriginName: seqno, Units: , IsExtended: false
        /// </summary>
        public uint Seqno { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Log data block status.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public MavRemoteLogDataBlockStatuses Status { get; set; }
    }
    /// <summary>
    /// Control vehicle LEDs.
    ///  LED_CONTROL
    /// </summary>
    public class LedControlPacket: PacketV2<LedControlPayload>
    {
	    public const int PacketMessageId = 186;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 72;

        public override LedControlPayload Payload { get; } = new LedControlPayload();

        public override string Name => "LED_CONTROL";
    }

    /// <summary>
    ///  LED_CONTROL
    /// </summary>
    public class LedControlPayload : IPayload
    {
        public byte GetMaxByteSize() => 29; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 29; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Instance = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Pattern = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CustomLen = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/24 - Math.Max(0,((/*PayloadByteSize*/29 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            CustomBytes = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                CustomBytes[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Instance);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Pattern);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CustomLen);index+=1;
            for(var i=0;i<CustomBytes.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)CustomBytes[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/29;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Instance = (byte)buffer[index++];
            Pattern = (byte)buffer[index++];
            CustomLen = (byte)buffer[index++];
            arraySize = /*ArrayLength*/24 - Math.Max(0,((/*PayloadByteSize*/29 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            CustomBytes = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                CustomBytes[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Instance).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Pattern).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(CustomLen).CopyTo(buffer, index);index+=1;
            for(var i=0;i<CustomBytes.Length;i++)
            {
                buffer[index] = (byte)CustomBytes[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/29;
        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Instance (LED instance to control or 255 for all LEDs).
        /// OriginName: instance, Units: , IsExtended: false
        /// </summary>
        public byte Instance { get; set; }
        /// <summary>
        /// Pattern (see LED_PATTERN_ENUM).
        /// OriginName: pattern, Units: , IsExtended: false
        /// </summary>
        public byte Pattern { get; set; }
        /// <summary>
        /// Custom Byte Length.
        /// OriginName: custom_len, Units: , IsExtended: false
        /// </summary>
        public byte CustomLen { get; set; }
        /// <summary>
        /// Custom Bytes.
        /// OriginName: custom_bytes, Units: , IsExtended: false
        /// </summary>
        public byte[] CustomBytes { get; set; } = new byte[24];
        public byte GetCustomBytesMaxItemsCount() => 24;
    }
    /// <summary>
    /// Reports progress of compass calibration.
    ///  MAG_CAL_PROGRESS
    /// </summary>
    public class MagCalProgressPacket: PacketV2<MagCalProgressPayload>
    {
	    public const int PacketMessageId = 191;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 92;

        public override MagCalProgressPayload Payload { get; } = new MagCalProgressPayload();

        public override string Name => "MAG_CAL_PROGRESS";
    }

    /// <summary>
    ///  MAG_CAL_PROGRESS
    /// </summary>
    public class MagCalProgressPayload : IPayload
    {
        public byte GetMaxByteSize() => 27; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 27; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            DirectionX = BinSerialize.ReadFloat(ref buffer);index+=4;
            DirectionY = BinSerialize.ReadFloat(ref buffer);index+=4;
            DirectionZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            CompassId = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CalMask = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CalStatus = (MagCalStatus)BinSerialize.ReadByte(ref buffer);index+=1;
            Attempt = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CompletionPct = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/10 - Math.Max(0,((/*PayloadByteSize*/27 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            CompletionMask = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                CompletionMask[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,DirectionX);index+=4;
            BinSerialize.WriteFloat(ref buffer,DirectionY);index+=4;
            BinSerialize.WriteFloat(ref buffer,DirectionZ);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)CompassId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CalMask);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CalStatus);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Attempt);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CompletionPct);index+=1;
            for(var i=0;i<CompletionMask.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)CompletionMask[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/27;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            DirectionX = BitConverter.ToSingle(buffer, index);index+=4;
            DirectionY = BitConverter.ToSingle(buffer, index);index+=4;
            DirectionZ = BitConverter.ToSingle(buffer, index);index+=4;
            CompassId = (byte)buffer[index++];
            CalMask = (byte)buffer[index++];
            CalStatus = (MagCalStatus)buffer[index++];
            Attempt = (byte)buffer[index++];
            CompletionPct = (byte)buffer[index++];
            arraySize = /*ArrayLength*/10 - Math.Max(0,((/*PayloadByteSize*/27 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            CompletionMask = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                CompletionMask[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(DirectionX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DirectionY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DirectionZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(CompassId).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(CalMask).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)CalStatus;index+=1;
            BitConverter.GetBytes(Attempt).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(CompletionPct).CopyTo(buffer, index);index+=1;
            for(var i=0;i<CompletionMask.Length;i++)
            {
                buffer[index] = (byte)CompletionMask[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/27;
        }

        /// <summary>
        /// Body frame direction vector for display.
        /// OriginName: direction_x, Units: , IsExtended: false
        /// </summary>
        public float DirectionX { get; set; }
        /// <summary>
        /// Body frame direction vector for display.
        /// OriginName: direction_y, Units: , IsExtended: false
        /// </summary>
        public float DirectionY { get; set; }
        /// <summary>
        /// Body frame direction vector for display.
        /// OriginName: direction_z, Units: , IsExtended: false
        /// </summary>
        public float DirectionZ { get; set; }
        /// <summary>
        /// Compass being calibrated.
        /// OriginName: compass_id, Units: , IsExtended: false
        /// </summary>
        public byte CompassId { get; set; }
        /// <summary>
        /// Bitmask of compasses being calibrated.
        /// OriginName: cal_mask, Units: , IsExtended: false
        /// </summary>
        public byte CalMask { get; set; }
        /// <summary>
        /// Calibration Status.
        /// OriginName: cal_status, Units: , IsExtended: false
        /// </summary>
        public MagCalStatus CalStatus { get; set; }
        /// <summary>
        /// Attempt number.
        /// OriginName: attempt, Units: , IsExtended: false
        /// </summary>
        public byte Attempt { get; set; }
        /// <summary>
        /// Completion percentage.
        /// OriginName: completion_pct, Units: %, IsExtended: false
        /// </summary>
        public byte CompletionPct { get; set; }
        /// <summary>
        /// Bitmask of sphere sections (see http://en.wikipedia.org/wiki/Geodesic_grid).
        /// OriginName: completion_mask, Units: , IsExtended: false
        /// </summary>
        public byte[] CompletionMask { get; set; } = new byte[10];
        public byte GetCompletionMaskMaxItemsCount() => 10;
    }
    /// <summary>
    /// Reports results of completed compass calibration. Sent until MAG_CAL_ACK received.
    ///  MAG_CAL_REPORT
    /// </summary>
    public class MagCalReportPacket: PacketV2<MagCalReportPayload>
    {
	    public const int PacketMessageId = 192;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 36;

        public override MagCalReportPayload Payload { get; } = new MagCalReportPayload();

        public override string Name => "MAG_CAL_REPORT";
    }

    /// <summary>
    ///  MAG_CAL_REPORT
    /// </summary>
    public class MagCalReportPayload : IPayload
    {
        public byte GetMaxByteSize() => 50; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 50; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Fitness = BinSerialize.ReadFloat(ref buffer);index+=4;
            OfsX = BinSerialize.ReadFloat(ref buffer);index+=4;
            OfsY = BinSerialize.ReadFloat(ref buffer);index+=4;
            OfsZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            DiagX = BinSerialize.ReadFloat(ref buffer);index+=4;
            DiagY = BinSerialize.ReadFloat(ref buffer);index+=4;
            DiagZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            OffdiagX = BinSerialize.ReadFloat(ref buffer);index+=4;
            OffdiagY = BinSerialize.ReadFloat(ref buffer);index+=4;
            OffdiagZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            CompassId = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CalMask = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CalStatus = (MagCalStatus)BinSerialize.ReadByte(ref buffer);index+=1;
            Autosaved = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            // extended field 'OrientationConfidence' can be empty
            if (index >= endIndex) return;
            OrientationConfidence = BinSerialize.ReadFloat(ref buffer);index+=4;
            // extended field 'OldOrientation' can be empty
            if (index >= endIndex) return;
            OldOrientation = (MavSensorOrientation)BinSerialize.ReadByte(ref buffer);index+=1;
            // extended field 'NewOrientation' can be empty
            if (index >= endIndex) return;
            NewOrientation = (MavSensorOrientation)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Fitness);index+=4;
            BinSerialize.WriteFloat(ref buffer,OfsX);index+=4;
            BinSerialize.WriteFloat(ref buffer,OfsY);index+=4;
            BinSerialize.WriteFloat(ref buffer,OfsZ);index+=4;
            BinSerialize.WriteFloat(ref buffer,DiagX);index+=4;
            BinSerialize.WriteFloat(ref buffer,DiagY);index+=4;
            BinSerialize.WriteFloat(ref buffer,DiagZ);index+=4;
            BinSerialize.WriteFloat(ref buffer,OffdiagX);index+=4;
            BinSerialize.WriteFloat(ref buffer,OffdiagY);index+=4;
            BinSerialize.WriteFloat(ref buffer,OffdiagZ);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)CompassId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CalMask);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CalStatus);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Autosaved);index+=1;
            BinSerialize.WriteFloat(ref buffer,OrientationConfidence);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)OldOrientation);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)NewOrientation);index+=1;
            return index; // /*PayloadByteSize*/50;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Fitness = BitConverter.ToSingle(buffer, index);index+=4;
            OfsX = BitConverter.ToSingle(buffer, index);index+=4;
            OfsY = BitConverter.ToSingle(buffer, index);index+=4;
            OfsZ = BitConverter.ToSingle(buffer, index);index+=4;
            DiagX = BitConverter.ToSingle(buffer, index);index+=4;
            DiagY = BitConverter.ToSingle(buffer, index);index+=4;
            DiagZ = BitConverter.ToSingle(buffer, index);index+=4;
            OffdiagX = BitConverter.ToSingle(buffer, index);index+=4;
            OffdiagY = BitConverter.ToSingle(buffer, index);index+=4;
            OffdiagZ = BitConverter.ToSingle(buffer, index);index+=4;
            CompassId = (byte)buffer[index++];
            CalMask = (byte)buffer[index++];
            CalStatus = (MagCalStatus)buffer[index++];
            Autosaved = (byte)buffer[index++];
            // extended field 'OrientationConfidence' can be empty
            if (index >= endIndex) return;
            OrientationConfidence = BitConverter.ToSingle(buffer, index);index+=4;
            // extended field 'OldOrientation' can be empty
            if (index >= endIndex) return;
            OldOrientation = (MavSensorOrientation)buffer[index++];
            // extended field 'NewOrientation' can be empty
            if (index >= endIndex) return;
            NewOrientation = (MavSensorOrientation)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Fitness).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(OfsX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(OfsY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(OfsZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DiagX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DiagY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DiagZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(OffdiagX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(OffdiagY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(OffdiagZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(CompassId).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(CalMask).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)CalStatus;index+=1;
            BitConverter.GetBytes(Autosaved).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(OrientationConfidence).CopyTo(buffer, index);index+=4;
            buffer[index] = (byte)OldOrientation;index+=1;
            buffer[index] = (byte)NewOrientation;index+=1;
            return index - start; // /*PayloadByteSize*/50;
        }

        /// <summary>
        /// RMS milligauss residuals.
        /// OriginName: fitness, Units: mgauss, IsExtended: false
        /// </summary>
        public float Fitness { get; set; }
        /// <summary>
        /// X offset.
        /// OriginName: ofs_x, Units: , IsExtended: false
        /// </summary>
        public float OfsX { get; set; }
        /// <summary>
        /// Y offset.
        /// OriginName: ofs_y, Units: , IsExtended: false
        /// </summary>
        public float OfsY { get; set; }
        /// <summary>
        /// Z offset.
        /// OriginName: ofs_z, Units: , IsExtended: false
        /// </summary>
        public float OfsZ { get; set; }
        /// <summary>
        /// X diagonal (matrix 11).
        /// OriginName: diag_x, Units: , IsExtended: false
        /// </summary>
        public float DiagX { get; set; }
        /// <summary>
        /// Y diagonal (matrix 22).
        /// OriginName: diag_y, Units: , IsExtended: false
        /// </summary>
        public float DiagY { get; set; }
        /// <summary>
        /// Z diagonal (matrix 33).
        /// OriginName: diag_z, Units: , IsExtended: false
        /// </summary>
        public float DiagZ { get; set; }
        /// <summary>
        /// X off-diagonal (matrix 12 and 21).
        /// OriginName: offdiag_x, Units: , IsExtended: false
        /// </summary>
        public float OffdiagX { get; set; }
        /// <summary>
        /// Y off-diagonal (matrix 13 and 31).
        /// OriginName: offdiag_y, Units: , IsExtended: false
        /// </summary>
        public float OffdiagY { get; set; }
        /// <summary>
        /// Z off-diagonal (matrix 32 and 23).
        /// OriginName: offdiag_z, Units: , IsExtended: false
        /// </summary>
        public float OffdiagZ { get; set; }
        /// <summary>
        /// Compass being calibrated.
        /// OriginName: compass_id, Units: , IsExtended: false
        /// </summary>
        public byte CompassId { get; set; }
        /// <summary>
        /// Bitmask of compasses being calibrated.
        /// OriginName: cal_mask, Units: , IsExtended: false
        /// </summary>
        public byte CalMask { get; set; }
        /// <summary>
        /// Calibration Status.
        /// OriginName: cal_status, Units: , IsExtended: false
        /// </summary>
        public MagCalStatus CalStatus { get; set; }
        /// <summary>
        /// 0=requires a MAV_CMD_DO_ACCEPT_MAG_CAL, 1=saved to parameters.
        /// OriginName: autosaved, Units: , IsExtended: false
        /// </summary>
        public byte Autosaved { get; set; }
        /// <summary>
        /// Confidence in orientation (higher is better).
        /// OriginName: orientation_confidence, Units: , IsExtended: true
        /// </summary>
        public float OrientationConfidence { get; set; }
        /// <summary>
        /// orientation before calibration.
        /// OriginName: old_orientation, Units: , IsExtended: true
        /// </summary>
        public MavSensorOrientation OldOrientation { get; set; }
        /// <summary>
        /// orientation after calibration.
        /// OriginName: new_orientation, Units: , IsExtended: true
        /// </summary>
        public MavSensorOrientation NewOrientation { get; set; }
    }
    /// <summary>
    /// EKF Status message including flags and variances.
    ///  EKF_STATUS_REPORT
    /// </summary>
    public class EkfStatusReportPacket: PacketV2<EkfStatusReportPayload>
    {
	    public const int PacketMessageId = 193;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 71;

        public override EkfStatusReportPayload Payload { get; } = new EkfStatusReportPayload();

        public override string Name => "EKF_STATUS_REPORT";
    }

    /// <summary>
    ///  EKF_STATUS_REPORT
    /// </summary>
    public class EkfStatusReportPayload : IPayload
    {
        public byte GetMaxByteSize() => 26; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 26; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            VelocityVariance = BinSerialize.ReadFloat(ref buffer);index+=4;
            PosHorizVariance = BinSerialize.ReadFloat(ref buffer);index+=4;
            PosVertVariance = BinSerialize.ReadFloat(ref buffer);index+=4;
            CompassVariance = BinSerialize.ReadFloat(ref buffer);index+=4;
            TerrainAltVariance = BinSerialize.ReadFloat(ref buffer);index+=4;
            Flags = (EkfStatusFlags)BinSerialize.ReadUShort(ref buffer);index+=2;
            // extended field 'AirspeedVariance' can be empty
            if (index >= endIndex) return;
            AirspeedVariance = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,VelocityVariance);index+=4;
            BinSerialize.WriteFloat(ref buffer,PosHorizVariance);index+=4;
            BinSerialize.WriteFloat(ref buffer,PosVertVariance);index+=4;
            BinSerialize.WriteFloat(ref buffer,CompassVariance);index+=4;
            BinSerialize.WriteFloat(ref buffer,TerrainAltVariance);index+=4;
            BinSerialize.WriteUShort(ref buffer,(ushort)Flags);index+=2;
            BinSerialize.WriteFloat(ref buffer,AirspeedVariance);index+=4;
            return index; // /*PayloadByteSize*/26;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            VelocityVariance = BitConverter.ToSingle(buffer, index);index+=4;
            PosHorizVariance = BitConverter.ToSingle(buffer, index);index+=4;
            PosVertVariance = BitConverter.ToSingle(buffer, index);index+=4;
            CompassVariance = BitConverter.ToSingle(buffer, index);index+=4;
            TerrainAltVariance = BitConverter.ToSingle(buffer, index);index+=4;
            Flags = (EkfStatusFlags)BitConverter.ToUInt16(buffer,index);index+=2;
            // extended field 'AirspeedVariance' can be empty
            if (index >= endIndex) return;
            AirspeedVariance = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(VelocityVariance).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PosHorizVariance).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PosVertVariance).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(CompassVariance).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TerrainAltVariance).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes((ushort)Flags).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(AirspeedVariance).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/26;
        }

        /// <summary>
        /// Velocity variance.
        /// OriginName: velocity_variance, Units: , IsExtended: false
        /// </summary>
        public float VelocityVariance { get; set; }
        /// <summary>
        /// Horizontal Position variance.
        /// OriginName: pos_horiz_variance, Units: , IsExtended: false
        /// </summary>
        public float PosHorizVariance { get; set; }
        /// <summary>
        /// Vertical Position variance.
        /// OriginName: pos_vert_variance, Units: , IsExtended: false
        /// </summary>
        public float PosVertVariance { get; set; }
        /// <summary>
        /// Compass variance.
        /// OriginName: compass_variance, Units: , IsExtended: false
        /// </summary>
        public float CompassVariance { get; set; }
        /// <summary>
        /// Terrain Altitude variance.
        /// OriginName: terrain_alt_variance, Units: , IsExtended: false
        /// </summary>
        public float TerrainAltVariance { get; set; }
        /// <summary>
        /// Flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public EkfStatusFlags Flags { get; set; }
        /// <summary>
        /// Airspeed variance.
        /// OriginName: airspeed_variance, Units: , IsExtended: true
        /// </summary>
        public float AirspeedVariance { get; set; }
    }
    /// <summary>
    /// PID tuning information.
    ///  PID_TUNING
    /// </summary>
    public class PidTuningPacket: PacketV2<PidTuningPayload>
    {
	    public const int PacketMessageId = 194;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 98;

        public override PidTuningPayload Payload { get; } = new PidTuningPayload();

        public override string Name => "PID_TUNING";
    }

    /// <summary>
    ///  PID_TUNING
    /// </summary>
    public class PidTuningPayload : IPayload
    {
        public byte GetMaxByteSize() => 25; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 25; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Desired = BinSerialize.ReadFloat(ref buffer);index+=4;
            Achieved = BinSerialize.ReadFloat(ref buffer);index+=4;
            Ff = BinSerialize.ReadFloat(ref buffer);index+=4;
            P = BinSerialize.ReadFloat(ref buffer);index+=4;
            I = BinSerialize.ReadFloat(ref buffer);index+=4;
            D = BinSerialize.ReadFloat(ref buffer);index+=4;
            Axis = (PidTuningAxis)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Desired);index+=4;
            BinSerialize.WriteFloat(ref buffer,Achieved);index+=4;
            BinSerialize.WriteFloat(ref buffer,Ff);index+=4;
            BinSerialize.WriteFloat(ref buffer,P);index+=4;
            BinSerialize.WriteFloat(ref buffer,I);index+=4;
            BinSerialize.WriteFloat(ref buffer,D);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)Axis);index+=1;
            return index; // /*PayloadByteSize*/25;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Desired = BitConverter.ToSingle(buffer, index);index+=4;
            Achieved = BitConverter.ToSingle(buffer, index);index+=4;
            Ff = BitConverter.ToSingle(buffer, index);index+=4;
            P = BitConverter.ToSingle(buffer, index);index+=4;
            I = BitConverter.ToSingle(buffer, index);index+=4;
            D = BitConverter.ToSingle(buffer, index);index+=4;
            Axis = (PidTuningAxis)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Desired).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Achieved).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Ff).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(P).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(I).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(D).CopyTo(buffer, index);index+=4;
            buffer[index] = (byte)Axis;index+=1;
            return index - start; // /*PayloadByteSize*/25;
        }

        /// <summary>
        /// Desired rate.
        /// OriginName: desired, Units: deg/s, IsExtended: false
        /// </summary>
        public float Desired { get; set; }
        /// <summary>
        /// Achieved rate.
        /// OriginName: achieved, Units: deg/s, IsExtended: false
        /// </summary>
        public float Achieved { get; set; }
        /// <summary>
        /// FF component.
        /// OriginName: FF, Units: , IsExtended: false
        /// </summary>
        public float Ff { get; set; }
        /// <summary>
        /// P component.
        /// OriginName: P, Units: , IsExtended: false
        /// </summary>
        public float P { get; set; }
        /// <summary>
        /// I component.
        /// OriginName: I, Units: , IsExtended: false
        /// </summary>
        public float I { get; set; }
        /// <summary>
        /// D component.
        /// OriginName: D, Units: , IsExtended: false
        /// </summary>
        public float D { get; set; }
        /// <summary>
        /// Axis.
        /// OriginName: axis, Units: , IsExtended: false
        /// </summary>
        public PidTuningAxis Axis { get; set; }
    }
    /// <summary>
    /// Deepstall path planning.
    ///  DEEPSTALL
    /// </summary>
    public class DeepstallPacket: PacketV2<DeepstallPayload>
    {
	    public const int PacketMessageId = 195;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 120;

        public override DeepstallPayload Payload { get; } = new DeepstallPayload();

        public override string Name => "DEEPSTALL";
    }

    /// <summary>
    ///  DEEPSTALL
    /// </summary>
    public class DeepstallPayload : IPayload
    {
        public byte GetMaxByteSize() => 37; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 37; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            LandingLat = BinSerialize.ReadInt(ref buffer);index+=4;
            LandingLon = BinSerialize.ReadInt(ref buffer);index+=4;
            PathLat = BinSerialize.ReadInt(ref buffer);index+=4;
            PathLon = BinSerialize.ReadInt(ref buffer);index+=4;
            ArcEntryLat = BinSerialize.ReadInt(ref buffer);index+=4;
            ArcEntryLon = BinSerialize.ReadInt(ref buffer);index+=4;
            Altitude = BinSerialize.ReadFloat(ref buffer);index+=4;
            ExpectedTravelDistance = BinSerialize.ReadFloat(ref buffer);index+=4;
            CrossTrackError = BinSerialize.ReadFloat(ref buffer);index+=4;
            Stage = (DeepstallStage)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteInt(ref buffer,LandingLat);index+=4;
            BinSerialize.WriteInt(ref buffer,LandingLon);index+=4;
            BinSerialize.WriteInt(ref buffer,PathLat);index+=4;
            BinSerialize.WriteInt(ref buffer,PathLon);index+=4;
            BinSerialize.WriteInt(ref buffer,ArcEntryLat);index+=4;
            BinSerialize.WriteInt(ref buffer,ArcEntryLon);index+=4;
            BinSerialize.WriteFloat(ref buffer,Altitude);index+=4;
            BinSerialize.WriteFloat(ref buffer,ExpectedTravelDistance);index+=4;
            BinSerialize.WriteFloat(ref buffer,CrossTrackError);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)Stage);index+=1;
            return index; // /*PayloadByteSize*/37;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            LandingLat = BitConverter.ToInt32(buffer,index);index+=4;
            LandingLon = BitConverter.ToInt32(buffer,index);index+=4;
            PathLat = BitConverter.ToInt32(buffer,index);index+=4;
            PathLon = BitConverter.ToInt32(buffer,index);index+=4;
            ArcEntryLat = BitConverter.ToInt32(buffer,index);index+=4;
            ArcEntryLon = BitConverter.ToInt32(buffer,index);index+=4;
            Altitude = BitConverter.ToSingle(buffer, index);index+=4;
            ExpectedTravelDistance = BitConverter.ToSingle(buffer, index);index+=4;
            CrossTrackError = BitConverter.ToSingle(buffer, index);index+=4;
            Stage = (DeepstallStage)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(LandingLat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(LandingLon).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PathLat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(PathLon).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(ArcEntryLat).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(ArcEntryLon).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Altitude).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(ExpectedTravelDistance).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(CrossTrackError).CopyTo(buffer, index);index+=4;
            buffer[index] = (byte)Stage;index+=1;
            return index - start; // /*PayloadByteSize*/37;
        }

        /// <summary>
        /// Landing latitude.
        /// OriginName: landing_lat, Units: degE7, IsExtended: false
        /// </summary>
        public int LandingLat { get; set; }
        /// <summary>
        /// Landing longitude.
        /// OriginName: landing_lon, Units: degE7, IsExtended: false
        /// </summary>
        public int LandingLon { get; set; }
        /// <summary>
        /// Final heading start point, latitude.
        /// OriginName: path_lat, Units: degE7, IsExtended: false
        /// </summary>
        public int PathLat { get; set; }
        /// <summary>
        /// Final heading start point, longitude.
        /// OriginName: path_lon, Units: degE7, IsExtended: false
        /// </summary>
        public int PathLon { get; set; }
        /// <summary>
        /// Arc entry point, latitude.
        /// OriginName: arc_entry_lat, Units: degE7, IsExtended: false
        /// </summary>
        public int ArcEntryLat { get; set; }
        /// <summary>
        /// Arc entry point, longitude.
        /// OriginName: arc_entry_lon, Units: degE7, IsExtended: false
        /// </summary>
        public int ArcEntryLon { get; set; }
        /// <summary>
        /// Altitude.
        /// OriginName: altitude, Units: m, IsExtended: false
        /// </summary>
        public float Altitude { get; set; }
        /// <summary>
        /// Distance the aircraft expects to travel during the deepstall.
        /// OriginName: expected_travel_distance, Units: m, IsExtended: false
        /// </summary>
        public float ExpectedTravelDistance { get; set; }
        /// <summary>
        /// Deepstall cross track error (only valid when in DEEPSTALL_STAGE_LAND).
        /// OriginName: cross_track_error, Units: m, IsExtended: false
        /// </summary>
        public float CrossTrackError { get; set; }
        /// <summary>
        /// Deepstall stage.
        /// OriginName: stage, Units: , IsExtended: false
        /// </summary>
        public DeepstallStage Stage { get; set; }
    }
    /// <summary>
    /// 3 axis gimbal measurements.
    ///  GIMBAL_REPORT
    /// </summary>
    public class GimbalReportPacket: PacketV2<GimbalReportPayload>
    {
	    public const int PacketMessageId = 200;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 134;

        public override GimbalReportPayload Payload { get; } = new GimbalReportPayload();

        public override string Name => "GIMBAL_REPORT";
    }

    /// <summary>
    ///  GIMBAL_REPORT
    /// </summary>
    public class GimbalReportPayload : IPayload
    {
        public byte GetMaxByteSize() => 42; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 42; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            DeltaTime = BinSerialize.ReadFloat(ref buffer);index+=4;
            DeltaAngleX = BinSerialize.ReadFloat(ref buffer);index+=4;
            DeltaAngleY = BinSerialize.ReadFloat(ref buffer);index+=4;
            DeltaAngleZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            DeltaVelocityX = BinSerialize.ReadFloat(ref buffer);index+=4;
            DeltaVelocityY = BinSerialize.ReadFloat(ref buffer);index+=4;
            DeltaVelocityZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            JointRoll = BinSerialize.ReadFloat(ref buffer);index+=4;
            JointEl = BinSerialize.ReadFloat(ref buffer);index+=4;
            JointAz = BinSerialize.ReadFloat(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,DeltaTime);index+=4;
            BinSerialize.WriteFloat(ref buffer,DeltaAngleX);index+=4;
            BinSerialize.WriteFloat(ref buffer,DeltaAngleY);index+=4;
            BinSerialize.WriteFloat(ref buffer,DeltaAngleZ);index+=4;
            BinSerialize.WriteFloat(ref buffer,DeltaVelocityX);index+=4;
            BinSerialize.WriteFloat(ref buffer,DeltaVelocityY);index+=4;
            BinSerialize.WriteFloat(ref buffer,DeltaVelocityZ);index+=4;
            BinSerialize.WriteFloat(ref buffer,JointRoll);index+=4;
            BinSerialize.WriteFloat(ref buffer,JointEl);index+=4;
            BinSerialize.WriteFloat(ref buffer,JointAz);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            return index; // /*PayloadByteSize*/42;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            DeltaTime = BitConverter.ToSingle(buffer, index);index+=4;
            DeltaAngleX = BitConverter.ToSingle(buffer, index);index+=4;
            DeltaAngleY = BitConverter.ToSingle(buffer, index);index+=4;
            DeltaAngleZ = BitConverter.ToSingle(buffer, index);index+=4;
            DeltaVelocityX = BitConverter.ToSingle(buffer, index);index+=4;
            DeltaVelocityY = BitConverter.ToSingle(buffer, index);index+=4;
            DeltaVelocityZ = BitConverter.ToSingle(buffer, index);index+=4;
            JointRoll = BitConverter.ToSingle(buffer, index);index+=4;
            JointEl = BitConverter.ToSingle(buffer, index);index+=4;
            JointAz = BitConverter.ToSingle(buffer, index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(DeltaTime).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DeltaAngleX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DeltaAngleY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DeltaAngleZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DeltaVelocityX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DeltaVelocityY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DeltaVelocityZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(JointRoll).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(JointEl).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(JointAz).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/42;
        }

        /// <summary>
        /// Time since last update.
        /// OriginName: delta_time, Units: s, IsExtended: false
        /// </summary>
        public float DeltaTime { get; set; }
        /// <summary>
        /// Delta angle X.
        /// OriginName: delta_angle_x, Units: rad, IsExtended: false
        /// </summary>
        public float DeltaAngleX { get; set; }
        /// <summary>
        /// Delta angle Y.
        /// OriginName: delta_angle_y, Units: rad, IsExtended: false
        /// </summary>
        public float DeltaAngleY { get; set; }
        /// <summary>
        /// Delta angle X.
        /// OriginName: delta_angle_z, Units: rad, IsExtended: false
        /// </summary>
        public float DeltaAngleZ { get; set; }
        /// <summary>
        /// Delta velocity X.
        /// OriginName: delta_velocity_x, Units: m/s, IsExtended: false
        /// </summary>
        public float DeltaVelocityX { get; set; }
        /// <summary>
        /// Delta velocity Y.
        /// OriginName: delta_velocity_y, Units: m/s, IsExtended: false
        /// </summary>
        public float DeltaVelocityY { get; set; }
        /// <summary>
        /// Delta velocity Z.
        /// OriginName: delta_velocity_z, Units: m/s, IsExtended: false
        /// </summary>
        public float DeltaVelocityZ { get; set; }
        /// <summary>
        /// Joint ROLL.
        /// OriginName: joint_roll, Units: rad, IsExtended: false
        /// </summary>
        public float JointRoll { get; set; }
        /// <summary>
        /// Joint EL.
        /// OriginName: joint_el, Units: rad, IsExtended: false
        /// </summary>
        public float JointEl { get; set; }
        /// <summary>
        /// Joint AZ.
        /// OriginName: joint_az, Units: rad, IsExtended: false
        /// </summary>
        public float JointAz { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
    }
    /// <summary>
    /// Control message for rate gimbal.
    ///  GIMBAL_CONTROL
    /// </summary>
    public class GimbalControlPacket: PacketV2<GimbalControlPayload>
    {
	    public const int PacketMessageId = 201;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 205;

        public override GimbalControlPayload Payload { get; } = new GimbalControlPayload();

        public override string Name => "GIMBAL_CONTROL";
    }

    /// <summary>
    ///  GIMBAL_CONTROL
    /// </summary>
    public class GimbalControlPayload : IPayload
    {
        public byte GetMaxByteSize() => 14; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 14; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            DemandedRateX = BinSerialize.ReadFloat(ref buffer);index+=4;
            DemandedRateY = BinSerialize.ReadFloat(ref buffer);index+=4;
            DemandedRateZ = BinSerialize.ReadFloat(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,DemandedRateX);index+=4;
            BinSerialize.WriteFloat(ref buffer,DemandedRateY);index+=4;
            BinSerialize.WriteFloat(ref buffer,DemandedRateZ);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            return index; // /*PayloadByteSize*/14;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            DemandedRateX = BitConverter.ToSingle(buffer, index);index+=4;
            DemandedRateY = BitConverter.ToSingle(buffer, index);index+=4;
            DemandedRateZ = BitConverter.ToSingle(buffer, index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(DemandedRateX).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DemandedRateY).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(DemandedRateZ).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/14;
        }

        /// <summary>
        /// Demanded angular rate X.
        /// OriginName: demanded_rate_x, Units: rad/s, IsExtended: false
        /// </summary>
        public float DemandedRateX { get; set; }
        /// <summary>
        /// Demanded angular rate Y.
        /// OriginName: demanded_rate_y, Units: rad/s, IsExtended: false
        /// </summary>
        public float DemandedRateY { get; set; }
        /// <summary>
        /// Demanded angular rate Z.
        /// OriginName: demanded_rate_z, Units: rad/s, IsExtended: false
        /// </summary>
        public float DemandedRateZ { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
    }
    /// <summary>
    /// 100 Hz gimbal torque command telemetry.
    ///  GIMBAL_TORQUE_CMD_REPORT
    /// </summary>
    public class GimbalTorqueCmdReportPacket: PacketV2<GimbalTorqueCmdReportPayload>
    {
	    public const int PacketMessageId = 214;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 69;

        public override GimbalTorqueCmdReportPayload Payload { get; } = new GimbalTorqueCmdReportPayload();

        public override string Name => "GIMBAL_TORQUE_CMD_REPORT";
    }

    /// <summary>
    ///  GIMBAL_TORQUE_CMD_REPORT
    /// </summary>
    public class GimbalTorqueCmdReportPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            RlTorqueCmd = BinSerialize.ReadShort(ref buffer);index+=2;
            ElTorqueCmd = BinSerialize.ReadShort(ref buffer);index+=2;
            AzTorqueCmd = BinSerialize.ReadShort(ref buffer);index+=2;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteShort(ref buffer,RlTorqueCmd);index+=2;
            BinSerialize.WriteShort(ref buffer,ElTorqueCmd);index+=2;
            BinSerialize.WriteShort(ref buffer,AzTorqueCmd);index+=2;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            return index; // /*PayloadByteSize*/8;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            RlTorqueCmd = BitConverter.ToInt16(buffer,index);index+=2;
            ElTorqueCmd = BitConverter.ToInt16(buffer,index);index+=2;
            AzTorqueCmd = BitConverter.ToInt16(buffer,index);index+=2;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(RlTorqueCmd).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(ElTorqueCmd).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(AzTorqueCmd).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/8;
        }

        /// <summary>
        /// Roll Torque Command.
        /// OriginName: rl_torque_cmd, Units: , IsExtended: false
        /// </summary>
        public short RlTorqueCmd { get; set; }
        /// <summary>
        /// Elevation Torque Command.
        /// OriginName: el_torque_cmd, Units: , IsExtended: false
        /// </summary>
        public short ElTorqueCmd { get; set; }
        /// <summary>
        /// Azimuth Torque Command.
        /// OriginName: az_torque_cmd, Units: , IsExtended: false
        /// </summary>
        public short AzTorqueCmd { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
    }
    /// <summary>
    /// Heartbeat from a HeroBus attached GoPro.
    ///  GOPRO_HEARTBEAT
    /// </summary>
    public class GoproHeartbeatPacket: PacketV2<GoproHeartbeatPayload>
    {
	    public const int PacketMessageId = 215;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 101;

        public override GoproHeartbeatPayload Payload { get; } = new GoproHeartbeatPayload();

        public override string Name => "GOPRO_HEARTBEAT";
    }

    /// <summary>
    ///  GOPRO_HEARTBEAT
    /// </summary>
    public class GoproHeartbeatPayload : IPayload
    {
        public byte GetMaxByteSize() => 3; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Status = (GoproHeartbeatStatus)BinSerialize.ReadByte(ref buffer);index+=1;
            CaptureMode = (GoproCaptureMode)BinSerialize.ReadByte(ref buffer);index+=1;
            Flags = (GoproHeartbeatFlags)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)Status);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CaptureMode);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Flags);index+=1;
            return index; // /*PayloadByteSize*/3;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Status = (GoproHeartbeatStatus)buffer[index++];
            CaptureMode = (GoproCaptureMode)buffer[index++];
            Flags = (GoproHeartbeatFlags)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            buffer[index] = (byte)Status;index+=1;
            buffer[index] = (byte)CaptureMode;index+=1;
            buffer[index] = (byte)Flags;index+=1;
            return index - start; // /*PayloadByteSize*/3;
        }

        /// <summary>
        /// Status.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public GoproHeartbeatStatus Status { get; set; }
        /// <summary>
        /// Current capture mode.
        /// OriginName: capture_mode, Units: , IsExtended: false
        /// </summary>
        public GoproCaptureMode CaptureMode { get; set; }
        /// <summary>
        /// Additional status bits.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public GoproHeartbeatFlags Flags { get; set; }
    }
    /// <summary>
    /// Request a GOPRO_COMMAND response from the GoPro.
    ///  GOPRO_GET_REQUEST
    /// </summary>
    public class GoproGetRequestPacket: PacketV2<GoproGetRequestPayload>
    {
	    public const int PacketMessageId = 216;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 50;

        public override GoproGetRequestPayload Payload { get; } = new GoproGetRequestPayload();

        public override string Name => "GOPRO_GET_REQUEST";
    }

    /// <summary>
    ///  GOPRO_GET_REQUEST
    /// </summary>
    public class GoproGetRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 3; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CmdId = (GoproCommand)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CmdId);index+=1;
            return index; // /*PayloadByteSize*/3;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            CmdId = (GoproCommand)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)CmdId;index+=1;
            return index - start; // /*PayloadByteSize*/3;
        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Command ID.
        /// OriginName: cmd_id, Units: , IsExtended: false
        /// </summary>
        public GoproCommand CmdId { get; set; }
    }
    /// <summary>
    /// Response from a GOPRO_COMMAND get request.
    ///  GOPRO_GET_RESPONSE
    /// </summary>
    public class GoproGetResponsePacket: PacketV2<GoproGetResponsePayload>
    {
	    public const int PacketMessageId = 217;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 202;

        public override GoproGetResponsePayload Payload { get; } = new GoproGetResponsePayload();

        public override string Name => "GOPRO_GET_RESPONSE";
    }

    /// <summary>
    ///  GOPRO_GET_RESPONSE
    /// </summary>
    public class GoproGetResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 6; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            CmdId = (GoproCommand)BinSerialize.ReadByte(ref buffer);index+=1;
            Status = (GoproRequestStatus)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/6 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Value = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Value[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)CmdId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Status);index+=1;
            for(var i=0;i<Value.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Value[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/6;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            CmdId = (GoproCommand)buffer[index++];
            Status = (GoproRequestStatus)buffer[index++];
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/6 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Value = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Value[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            buffer[index] = (byte)CmdId;index+=1;
            buffer[index] = (byte)Status;index+=1;
            for(var i=0;i<Value.Length;i++)
            {
                buffer[index] = (byte)Value[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/6;
        }

        /// <summary>
        /// Command ID.
        /// OriginName: cmd_id, Units: , IsExtended: false
        /// </summary>
        public GoproCommand CmdId { get; set; }
        /// <summary>
        /// Status.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public GoproRequestStatus Status { get; set; }
        /// <summary>
        /// Value.
        /// OriginName: value, Units: , IsExtended: false
        /// </summary>
        public byte[] Value { get; set; } = new byte[4];
        public byte GetValueMaxItemsCount() => 4;
    }
    /// <summary>
    /// Request to set a GOPRO_COMMAND with a desired.
    ///  GOPRO_SET_REQUEST
    /// </summary>
    public class GoproSetRequestPacket: PacketV2<GoproSetRequestPayload>
    {
	    public const int PacketMessageId = 218;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 17;

        public override GoproSetRequestPayload Payload { get; } = new GoproSetRequestPayload();

        public override string Name => "GOPRO_SET_REQUEST";
    }

    /// <summary>
    ///  GOPRO_SET_REQUEST
    /// </summary>
    public class GoproSetRequestPayload : IPayload
    {
        public byte GetMaxByteSize() => 7; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            CmdId = (GoproCommand)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/7 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Value = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Value[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)CmdId);index+=1;
            for(var i=0;i<Value.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Value[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/7;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            CmdId = (GoproCommand)buffer[index++];
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/7 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Value = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Value[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)CmdId;index+=1;
            for(var i=0;i<Value.Length;i++)
            {
                buffer[index] = (byte)Value[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/7;
        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// Command ID.
        /// OriginName: cmd_id, Units: , IsExtended: false
        /// </summary>
        public GoproCommand CmdId { get; set; }
        /// <summary>
        /// Value.
        /// OriginName: value, Units: , IsExtended: false
        /// </summary>
        public byte[] Value { get; set; } = new byte[4];
        public byte GetValueMaxItemsCount() => 4;
    }
    /// <summary>
    /// Response from a GOPRO_COMMAND set request.
    ///  GOPRO_SET_RESPONSE
    /// </summary>
    public class GoproSetResponsePacket: PacketV2<GoproSetResponsePayload>
    {
	    public const int PacketMessageId = 219;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 162;

        public override GoproSetResponsePayload Payload { get; } = new GoproSetResponsePayload();

        public override string Name => "GOPRO_SET_RESPONSE";
    }

    /// <summary>
    ///  GOPRO_SET_RESPONSE
    /// </summary>
    public class GoproSetResponsePayload : IPayload
    {
        public byte GetMaxByteSize() => 2; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 2; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            CmdId = (GoproCommand)BinSerialize.ReadByte(ref buffer);index+=1;
            Status = (GoproRequestStatus)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteByte(ref buffer,(byte)CmdId);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Status);index+=1;
            return index; // /*PayloadByteSize*/2;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            CmdId = (GoproCommand)buffer[index++];
            Status = (GoproRequestStatus)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            buffer[index] = (byte)CmdId;index+=1;
            buffer[index] = (byte)Status;index+=1;
            return index - start; // /*PayloadByteSize*/2;
        }

        /// <summary>
        /// Command ID.
        /// OriginName: cmd_id, Units: , IsExtended: false
        /// </summary>
        public GoproCommand CmdId { get; set; }
        /// <summary>
        /// Status.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public GoproRequestStatus Status { get; set; }
    }
    /// <summary>
    /// RPM sensor output.
    ///  RPM
    /// </summary>
    public class RpmPacket: PacketV2<RpmPayload>
    {
	    public const int PacketMessageId = 226;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 207;

        public override RpmPayload Payload { get; } = new RpmPayload();

        public override string Name => "RPM";
    }

    /// <summary>
    ///  RPM
    /// </summary>
    public class RpmPayload : IPayload
    {
        public byte GetMaxByteSize() => 8; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Rpm1 = BinSerialize.ReadFloat(ref buffer);index+=4;
            Rpm2 = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Rpm1);index+=4;
            BinSerialize.WriteFloat(ref buffer,Rpm2);index+=4;
            return index; // /*PayloadByteSize*/8;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Rpm1 = BitConverter.ToSingle(buffer, index);index+=4;
            Rpm2 = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Rpm1).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Rpm2).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/8;
        }

        /// <summary>
        /// RPM Sensor1.
        /// OriginName: rpm1, Units: , IsExtended: false
        /// </summary>
        public float Rpm1 { get; set; }
        /// <summary>
        /// RPM Sensor2.
        /// OriginName: rpm2, Units: , IsExtended: false
        /// </summary>
        public float Rpm2 { get; set; }
    }
    /// <summary>
    /// Read registers for a device.
    ///  DEVICE_OP_READ
    /// </summary>
    public class DeviceOpReadPacket: PacketV2<DeviceOpReadPayload>
    {
	    public const int PacketMessageId = 11000;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 134;

        public override DeviceOpReadPayload Payload { get; } = new DeviceOpReadPayload();

        public override string Name => "DEVICE_OP_READ";
    }

    /// <summary>
    ///  DEVICE_OP_READ
    /// </summary>
    public class DeviceOpReadPayload : IPayload
    {
        public byte GetMaxByteSize() => 51; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 51; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            RequestId = BinSerialize.ReadUInt(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Bustype = (DeviceOpBustype)BinSerialize.ReadByte(ref buffer);index+=1;
            Bus = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Address = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/40 - Math.Max(0,((/*PayloadByteSize*/51 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Busname = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Busname)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Busname.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
            index+=arraySize;
            Regstart = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Count = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,RequestId);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Bustype);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Bus);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Address);index+=1;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Busname)
                {
                    Encoding.ASCII.GetBytes(charPointer, Busname.Length, bytePointer, Busname.Length);
                }
            }
            buffer = buffer.Slice(Busname.Length);
            index+=Busname.Length;
            BinSerialize.WriteByte(ref buffer,(byte)Regstart);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Count);index+=1;
            return index; // /*PayloadByteSize*/51;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            RequestId = BitConverter.ToUInt32(buffer,index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Bustype = (DeviceOpBustype)buffer[index++];
            Bus = (byte)buffer[index++];
            Address = (byte)buffer[index++];
            arraySize = /*ArrayLength*/40 - Math.Max(0,((/*PayloadByteSize*/51 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Busname = new char[arraySize];
            Encoding.ASCII.GetChars(buffer, index,arraySize,Busname,0);
            index+=arraySize;
            Regstart = (byte)buffer[index++];
            Count = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(RequestId).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Bustype;index+=1;
            BitConverter.GetBytes(Bus).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Address).CopyTo(buffer, index);index+=1;
            index+=Encoding.ASCII.GetBytes(Busname,0,Busname.Length,buffer,index);
            BitConverter.GetBytes(Regstart).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Count).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/51;
        }

        /// <summary>
        /// Request ID - copied to reply.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public uint RequestId { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// The bus type.
        /// OriginName: bustype, Units: , IsExtended: false
        /// </summary>
        public DeviceOpBustype Bustype { get; set; }
        /// <summary>
        /// Bus number.
        /// OriginName: bus, Units: , IsExtended: false
        /// </summary>
        public byte Bus { get; set; }
        /// <summary>
        /// Bus address.
        /// OriginName: address, Units: , IsExtended: false
        /// </summary>
        public byte Address { get; set; }
        /// <summary>
        /// Name of device on bus (for SPI).
        /// OriginName: busname, Units: , IsExtended: false
        /// </summary>
        public char[] Busname { get; set; } = new char[40];
        public byte GetBusnameMaxItemsCount() => 40;
        /// <summary>
        /// First register to read.
        /// OriginName: regstart, Units: , IsExtended: false
        /// </summary>
        public byte Regstart { get; set; }
        /// <summary>
        /// Count of registers to read.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public byte Count { get; set; }
    }
    /// <summary>
    /// Read registers reply.
    ///  DEVICE_OP_READ_REPLY
    /// </summary>
    public class DeviceOpReadReplyPacket: PacketV2<DeviceOpReadReplyPayload>
    {
	    public const int PacketMessageId = 11001;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 15;

        public override DeviceOpReadReplyPayload Payload { get; } = new DeviceOpReadReplyPayload();

        public override string Name => "DEVICE_OP_READ_REPLY";
    }

    /// <summary>
    ///  DEVICE_OP_READ_REPLY
    /// </summary>
    public class DeviceOpReadReplyPayload : IPayload
    {
        public byte GetMaxByteSize() => 135; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 135; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            RequestId = BinSerialize.ReadUInt(ref buffer);index+=4;
            Result = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Regstart = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Count = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/128 - Math.Max(0,((/*PayloadByteSize*/135 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,RequestId);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)Result);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Regstart);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Count);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/135;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            RequestId = BitConverter.ToUInt32(buffer,index);index+=4;
            Result = (byte)buffer[index++];
            Regstart = (byte)buffer[index++];
            Count = (byte)buffer[index++];
            arraySize = /*ArrayLength*/128 - Math.Max(0,((/*PayloadByteSize*/135 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(RequestId).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Result).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Regstart).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Count).CopyTo(buffer, index);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                buffer[index] = (byte)Data[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/135;
        }

        /// <summary>
        /// Request ID - copied from request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public uint RequestId { get; set; }
        /// <summary>
        /// 0 for success, anything else is failure code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public byte Result { get; set; }
        /// <summary>
        /// Starting register.
        /// OriginName: regstart, Units: , IsExtended: false
        /// </summary>
        public byte Regstart { get; set; }
        /// <summary>
        /// Count of bytes read.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public byte Count { get; set; }
        /// <summary>
        /// Reply data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public byte[] Data { get; set; } = new byte[128];
        public byte GetDataMaxItemsCount() => 128;
    }
    /// <summary>
    /// Write registers for a device.
    ///  DEVICE_OP_WRITE
    /// </summary>
    public class DeviceOpWritePacket: PacketV2<DeviceOpWritePayload>
    {
	    public const int PacketMessageId = 11002;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 234;

        public override DeviceOpWritePayload Payload { get; } = new DeviceOpWritePayload();

        public override string Name => "DEVICE_OP_WRITE";
    }

    /// <summary>
    ///  DEVICE_OP_WRITE
    /// </summary>
    public class DeviceOpWritePayload : IPayload
    {
        public byte GetMaxByteSize() => 179; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 179; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            RequestId = BinSerialize.ReadUInt(ref buffer);index+=4;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Bustype = (DeviceOpBustype)BinSerialize.ReadByte(ref buffer);index+=1;
            Bus = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Address = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = 40;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Busname)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Busname.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
            index+=arraySize;
            Regstart = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            Count = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = /*ArrayLength*/128 - Math.Max(0,((/*PayloadByteSize*/179 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,RequestId);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Bustype);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Bus);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Address);index+=1;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Busname)
                {
                    Encoding.ASCII.GetBytes(charPointer, Busname.Length, bytePointer, Busname.Length);
                }
            }
            buffer = buffer.Slice(Busname.Length);
            index+=Busname.Length;
            BinSerialize.WriteByte(ref buffer,(byte)Regstart);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)Count);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/179;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            RequestId = BitConverter.ToUInt32(buffer,index);index+=4;
            TargetSystem = (byte)buffer[index++];
            TargetComponent = (byte)buffer[index++];
            Bustype = (DeviceOpBustype)buffer[index++];
            Bus = (byte)buffer[index++];
            Address = (byte)buffer[index++];
            arraySize = 40;
            Encoding.ASCII.GetChars(buffer, index,arraySize,Busname,0);
            index+=arraySize;
            Regstart = (byte)buffer[index++];
            Count = (byte)buffer[index++];
            arraySize = /*ArrayLength*/128 - Math.Max(0,((/*PayloadByteSize*/179 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(RequestId).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(TargetSystem).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(TargetComponent).CopyTo(buffer, index);index+=1;
            buffer[index] = (byte)Bustype;index+=1;
            BitConverter.GetBytes(Bus).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Address).CopyTo(buffer, index);index+=1;
            index+=Encoding.ASCII.GetBytes(Busname,0,Busname.Length,buffer,index);
            BitConverter.GetBytes(Regstart).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(Count).CopyTo(buffer, index);index+=1;
            for(var i=0;i<Data.Length;i++)
            {
                buffer[index] = (byte)Data[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/179;
        }

        /// <summary>
        /// Request ID - copied to reply.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public uint RequestId { get; set; }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public byte TargetSystem { get; set; }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public byte TargetComponent { get; set; }
        /// <summary>
        /// The bus type.
        /// OriginName: bustype, Units: , IsExtended: false
        /// </summary>
        public DeviceOpBustype Bustype { get; set; }
        /// <summary>
        /// Bus number.
        /// OriginName: bus, Units: , IsExtended: false
        /// </summary>
        public byte Bus { get; set; }
        /// <summary>
        /// Bus address.
        /// OriginName: address, Units: , IsExtended: false
        /// </summary>
        public byte Address { get; set; }
        /// <summary>
        /// Name of device on bus (for SPI).
        /// OriginName: busname, Units: , IsExtended: false
        /// </summary>
        public char[] Busname { get; } = new char[40];
        /// <summary>
        /// First register to write.
        /// OriginName: regstart, Units: , IsExtended: false
        /// </summary>
        public byte Regstart { get; set; }
        /// <summary>
        /// Count of registers to write.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public byte Count { get; set; }
        /// <summary>
        /// Write data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public byte[] Data { get; set; } = new byte[128];
        public byte GetDataMaxItemsCount() => 128;
    }
    /// <summary>
    /// Write registers reply.
    ///  DEVICE_OP_WRITE_REPLY
    /// </summary>
    public class DeviceOpWriteReplyPacket: PacketV2<DeviceOpWriteReplyPayload>
    {
	    public const int PacketMessageId = 11003;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 64;

        public override DeviceOpWriteReplyPayload Payload { get; } = new DeviceOpWriteReplyPayload();

        public override string Name => "DEVICE_OP_WRITE_REPLY";
    }

    /// <summary>
    ///  DEVICE_OP_WRITE_REPLY
    /// </summary>
    public class DeviceOpWriteReplyPayload : IPayload
    {
        public byte GetMaxByteSize() => 5; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 5; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            RequestId = BinSerialize.ReadUInt(ref buffer);index+=4;
            Result = (byte)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteUInt(ref buffer,RequestId);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)Result);index+=1;
            return index; // /*PayloadByteSize*/5;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            RequestId = BitConverter.ToUInt32(buffer,index);index+=4;
            Result = (byte)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(RequestId).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Result).CopyTo(buffer, index);index+=1;
            return index - start; // /*PayloadByteSize*/5;
        }

        /// <summary>
        /// Request ID - copied from request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public uint RequestId { get; set; }
        /// <summary>
        /// 0 for success, anything else is failure code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public byte Result { get; set; }
    }
    /// <summary>
    /// Adaptive Controller tuning information.
    ///  ADAP_TUNING
    /// </summary>
    public class AdapTuningPacket: PacketV2<AdapTuningPayload>
    {
	    public const int PacketMessageId = 11010;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 46;

        public override AdapTuningPayload Payload { get; } = new AdapTuningPayload();

        public override string Name => "ADAP_TUNING";
    }

    /// <summary>
    ///  ADAP_TUNING
    /// </summary>
    public class AdapTuningPayload : IPayload
    {
        public byte GetMaxByteSize() => 49; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 49; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            Desired = BinSerialize.ReadFloat(ref buffer);index+=4;
            Achieved = BinSerialize.ReadFloat(ref buffer);index+=4;
            Error = BinSerialize.ReadFloat(ref buffer);index+=4;
            Theta = BinSerialize.ReadFloat(ref buffer);index+=4;
            Omega = BinSerialize.ReadFloat(ref buffer);index+=4;
            Sigma = BinSerialize.ReadFloat(ref buffer);index+=4;
            ThetaDot = BinSerialize.ReadFloat(ref buffer);index+=4;
            OmegaDot = BinSerialize.ReadFloat(ref buffer);index+=4;
            SigmaDot = BinSerialize.ReadFloat(ref buffer);index+=4;
            F = BinSerialize.ReadFloat(ref buffer);index+=4;
            FDot = BinSerialize.ReadFloat(ref buffer);index+=4;
            U = BinSerialize.ReadFloat(ref buffer);index+=4;
            Axis = (PidTuningAxis)BinSerialize.ReadByte(ref buffer);index+=1;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteFloat(ref buffer,Desired);index+=4;
            BinSerialize.WriteFloat(ref buffer,Achieved);index+=4;
            BinSerialize.WriteFloat(ref buffer,Error);index+=4;
            BinSerialize.WriteFloat(ref buffer,Theta);index+=4;
            BinSerialize.WriteFloat(ref buffer,Omega);index+=4;
            BinSerialize.WriteFloat(ref buffer,Sigma);index+=4;
            BinSerialize.WriteFloat(ref buffer,ThetaDot);index+=4;
            BinSerialize.WriteFloat(ref buffer,OmegaDot);index+=4;
            BinSerialize.WriteFloat(ref buffer,SigmaDot);index+=4;
            BinSerialize.WriteFloat(ref buffer,F);index+=4;
            BinSerialize.WriteFloat(ref buffer,FDot);index+=4;
            BinSerialize.WriteFloat(ref buffer,U);index+=4;
            BinSerialize.WriteByte(ref buffer,(byte)Axis);index+=1;
            return index; // /*PayloadByteSize*/49;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            Desired = BitConverter.ToSingle(buffer, index);index+=4;
            Achieved = BitConverter.ToSingle(buffer, index);index+=4;
            Error = BitConverter.ToSingle(buffer, index);index+=4;
            Theta = BitConverter.ToSingle(buffer, index);index+=4;
            Omega = BitConverter.ToSingle(buffer, index);index+=4;
            Sigma = BitConverter.ToSingle(buffer, index);index+=4;
            ThetaDot = BitConverter.ToSingle(buffer, index);index+=4;
            OmegaDot = BitConverter.ToSingle(buffer, index);index+=4;
            SigmaDot = BitConverter.ToSingle(buffer, index);index+=4;
            F = BitConverter.ToSingle(buffer, index);index+=4;
            FDot = BitConverter.ToSingle(buffer, index);index+=4;
            U = BitConverter.ToSingle(buffer, index);index+=4;
            Axis = (PidTuningAxis)buffer[index++];
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(Desired).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Achieved).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Error).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Theta).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Omega).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Sigma).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(ThetaDot).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(OmegaDot).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(SigmaDot).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(F).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(FDot).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(U).CopyTo(buffer, index);index+=4;
            buffer[index] = (byte)Axis;index+=1;
            return index - start; // /*PayloadByteSize*/49;
        }

        /// <summary>
        /// Desired rate.
        /// OriginName: desired, Units: deg/s, IsExtended: false
        /// </summary>
        public float Desired { get; set; }
        /// <summary>
        /// Achieved rate.
        /// OriginName: achieved, Units: deg/s, IsExtended: false
        /// </summary>
        public float Achieved { get; set; }
        /// <summary>
        /// Error between model and vehicle.
        /// OriginName: error, Units: , IsExtended: false
        /// </summary>
        public float Error { get; set; }
        /// <summary>
        /// Theta estimated state predictor.
        /// OriginName: theta, Units: , IsExtended: false
        /// </summary>
        public float Theta { get; set; }
        /// <summary>
        /// Omega estimated state predictor.
        /// OriginName: omega, Units: , IsExtended: false
        /// </summary>
        public float Omega { get; set; }
        /// <summary>
        /// Sigma estimated state predictor.
        /// OriginName: sigma, Units: , IsExtended: false
        /// </summary>
        public float Sigma { get; set; }
        /// <summary>
        /// Theta derivative.
        /// OriginName: theta_dot, Units: , IsExtended: false
        /// </summary>
        public float ThetaDot { get; set; }
        /// <summary>
        /// Omega derivative.
        /// OriginName: omega_dot, Units: , IsExtended: false
        /// </summary>
        public float OmegaDot { get; set; }
        /// <summary>
        /// Sigma derivative.
        /// OriginName: sigma_dot, Units: , IsExtended: false
        /// </summary>
        public float SigmaDot { get; set; }
        /// <summary>
        /// Projection operator value.
        /// OriginName: f, Units: , IsExtended: false
        /// </summary>
        public float F { get; set; }
        /// <summary>
        /// Projection operator derivative.
        /// OriginName: f_dot, Units: , IsExtended: false
        /// </summary>
        public float FDot { get; set; }
        /// <summary>
        /// u adaptive controlled output command.
        /// OriginName: u, Units: , IsExtended: false
        /// </summary>
        public float U { get; set; }
        /// <summary>
        /// Axis.
        /// OriginName: axis, Units: , IsExtended: false
        /// </summary>
        public PidTuningAxis Axis { get; set; }
    }
    /// <summary>
    /// Camera vision based attitude and position deltas.
    ///  VISION_POSITION_DELTA
    /// </summary>
    public class VisionPositionDeltaPacket: PacketV2<VisionPositionDeltaPayload>
    {
	    public const int PacketMessageId = 11011;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 106;

        public override VisionPositionDeltaPayload Payload { get; } = new VisionPositionDeltaPayload();

        public override string Name => "VISION_POSITION_DELTA";
    }

    /// <summary>
    ///  VISION_POSITION_DELTA
    /// </summary>
    public class VisionPositionDeltaPayload : IPayload
    {
        public byte GetMaxByteSize() => 44; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TimeUsec = BinSerialize.ReadULong(ref buffer);index+=8;
            TimeDeltaUsec = BinSerialize.ReadULong(ref buffer);index+=8;
            arraySize = /*ArrayLength*/3 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            AngleDelta = new float[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                AngleDelta[i] = BinSerialize.ReadFloat(ref buffer);index+=4;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                PositionDelta[i] = BinSerialize.ReadFloat(ref buffer);index+=4;
            }
            Confidence = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,TimeUsec);index+=8;
            BinSerialize.WriteULong(ref buffer,TimeDeltaUsec);index+=8;
            for(var i=0;i<AngleDelta.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,AngleDelta[i]);index+=4;
            }
            for(var i=0;i<PositionDelta.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,PositionDelta[i]);index+=4;
            }
            BinSerialize.WriteFloat(ref buffer,Confidence);index+=4;
            return index; // /*PayloadByteSize*/44;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TimeUsec = BitConverter.ToUInt64(buffer,index);index+=8;
            TimeDeltaUsec = BitConverter.ToUInt64(buffer,index);index+=8;
            arraySize = /*ArrayLength*/3 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            AngleDelta = new float[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                AngleDelta[i] = BitConverter.ToSingle(buffer, index);index+=4;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                PositionDelta[i] = BitConverter.ToSingle(buffer, index);index+=4;
            }
            Confidence = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TimeUsec).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(TimeDeltaUsec).CopyTo(buffer, index);index+=8;
            for(var i=0;i<AngleDelta.Length;i++)
            {
                BitConverter.GetBytes(AngleDelta[i]).CopyTo(buffer, index);index+=4;
            }
            for(var i=0;i<PositionDelta.Length;i++)
            {
                BitConverter.GetBytes(PositionDelta[i]).CopyTo(buffer, index);index+=4;
            }
            BitConverter.GetBytes(Confidence).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/44;
        }

        /// <summary>
        /// Timestamp (synced to UNIX time or since system boot).
        /// OriginName: time_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUsec { get; set; }
        /// <summary>
        /// Time since the last reported camera frame.
        /// OriginName: time_delta_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeDeltaUsec { get; set; }
        /// <summary>
        /// Defines a rotation vector in body frame that rotates the vehicle from the previous to the current orientation.
        /// OriginName: angle_delta, Units: , IsExtended: false
        /// </summary>
        public float[] AngleDelta { get; set; } = new float[3];
        public byte GetAngleDeltaMaxItemsCount() => 3;
        /// <summary>
        /// Change in position from previous to current frame rotated into body frame (0=forward, 1=right, 2=down).
        /// OriginName: position_delta, Units: m, IsExtended: false
        /// </summary>
        public float[] PositionDelta { get; } = new float[3];
        /// <summary>
        /// Normalised confidence value from 0 to 100.
        /// OriginName: confidence, Units: %, IsExtended: false
        /// </summary>
        public float Confidence { get; set; }
    }
    /// <summary>
    /// Angle of Attack and Side Slip Angle.
    ///  AOA_SSA
    /// </summary>
    public class AoaSsaPacket: PacketV2<AoaSsaPayload>
    {
	    public const int PacketMessageId = 11020;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 205;

        public override AoaSsaPayload Payload { get; } = new AoaSsaPayload();

        public override string Name => "AOA_SSA";
    }

    /// <summary>
    ///  AOA_SSA
    /// </summary>
    public class AoaSsaPayload : IPayload
    {
        public byte GetMaxByteSize() => 16; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            TimeUsec = BinSerialize.ReadULong(ref buffer);index+=8;
            Aoa = BinSerialize.ReadFloat(ref buffer);index+=4;
            Ssa = BinSerialize.ReadFloat(ref buffer);index+=4;

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,TimeUsec);index+=8;
            BinSerialize.WriteFloat(ref buffer,Aoa);index+=4;
            BinSerialize.WriteFloat(ref buffer,Ssa);index+=4;
            return index; // /*PayloadByteSize*/16;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            TimeUsec = BitConverter.ToUInt64(buffer,index);index+=8;
            Aoa = BitConverter.ToSingle(buffer, index);index+=4;
            Ssa = BitConverter.ToSingle(buffer, index);index+=4;
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(TimeUsec).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(Aoa).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(Ssa).CopyTo(buffer, index);index+=4;
            return index - start; // /*PayloadByteSize*/16;
        }

        /// <summary>
        /// Timestamp (since boot or Unix epoch).
        /// OriginName: time_usec, Units: us, IsExtended: false
        /// </summary>
        public ulong TimeUsec { get; set; }
        /// <summary>
        /// Angle of Attack.
        /// OriginName: AOA, Units: deg, IsExtended: false
        /// </summary>
        public float Aoa { get; set; }
        /// <summary>
        /// Side Slip Angle.
        /// OriginName: SSA, Units: deg, IsExtended: false
        /// </summary>
        public float Ssa { get; set; }
    }
    /// <summary>
    /// ESC Telemetry Data for ESCs 1 to 4, matching data sent by BLHeli ESCs.
    ///  ESC_TELEMETRY_1_TO_4
    /// </summary>
    public class EscTelemetry1To4Packet: PacketV2<EscTelemetry1To4Payload>
    {
	    public const int PacketMessageId = 11030;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 144;

        public override EscTelemetry1To4Payload Payload { get; } = new EscTelemetry1To4Payload();

        public override string Name => "ESC_TELEMETRY_1_TO_4";
    }

    /// <summary>
    ///  ESC_TELEMETRY_1_TO_4
    /// </summary>
    public class EscTelemetry1To4Payload : IPayload
    {
        public byte GetMaxByteSize() => 44; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            Voltage = new ushort[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Voltage[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Current[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Totalcurrent[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Rpm[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Count[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Temperature[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            for(var i=0;i<Voltage.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Voltage[i]);index+=2;
            }
            for(var i=0;i<Current.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Current[i]);index+=2;
            }
            for(var i=0;i<Totalcurrent.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Totalcurrent[i]);index+=2;
            }
            for(var i=0;i<Rpm.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Rpm[i]);index+=2;
            }
            for(var i=0;i<Count.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Count[i]);index+=2;
            }
            for(var i=0;i<Temperature.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Temperature[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/44;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            Voltage = new ushort[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Voltage[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Current[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Totalcurrent[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Rpm[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Count[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Temperature[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            for(var i=0;i<Voltage.Length;i++)
            {
                BitConverter.GetBytes(Voltage[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Current.Length;i++)
            {
                BitConverter.GetBytes(Current[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Totalcurrent.Length;i++)
            {
                BitConverter.GetBytes(Totalcurrent[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Rpm.Length;i++)
            {
                BitConverter.GetBytes(Rpm[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Count.Length;i++)
            {
                BitConverter.GetBytes(Count[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Temperature.Length;i++)
            {
                buffer[index] = (byte)Temperature[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/44;
        }

        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: cV, IsExtended: false
        /// </summary>
        public ushort[] Voltage { get; set; } = new ushort[4];
        public byte GetVoltageMaxItemsCount() => 4;
        /// <summary>
        /// Current.
        /// OriginName: current, Units: cA, IsExtended: false
        /// </summary>
        public ushort[] Current { get; } = new ushort[4];
        /// <summary>
        /// Total current.
        /// OriginName: totalcurrent, Units: mAh, IsExtended: false
        /// </summary>
        public ushort[] Totalcurrent { get; } = new ushort[4];
        /// <summary>
        /// RPM (eRPM).
        /// OriginName: rpm, Units: rpm, IsExtended: false
        /// </summary>
        public ushort[] Rpm { get; } = new ushort[4];
        /// <summary>
        /// count of telemetry packets received (wraps at 65535).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public ushort[] Count { get; } = new ushort[4];
        /// <summary>
        /// Temperature.
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
        public byte[] Temperature { get; } = new byte[4];
    }
    /// <summary>
    /// ESC Telemetry Data for ESCs 5 to 8, matching data sent by BLHeli ESCs.
    ///  ESC_TELEMETRY_5_TO_8
    /// </summary>
    public class EscTelemetry5To8Packet: PacketV2<EscTelemetry5To8Payload>
    {
	    public const int PacketMessageId = 11031;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 133;

        public override EscTelemetry5To8Payload Payload { get; } = new EscTelemetry5To8Payload();

        public override string Name => "ESC_TELEMETRY_5_TO_8";
    }

    /// <summary>
    ///  ESC_TELEMETRY_5_TO_8
    /// </summary>
    public class EscTelemetry5To8Payload : IPayload
    {
        public byte GetMaxByteSize() => 44; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            Voltage = new ushort[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Voltage[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Current[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Totalcurrent[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Rpm[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Count[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Temperature[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            for(var i=0;i<Voltage.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Voltage[i]);index+=2;
            }
            for(var i=0;i<Current.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Current[i]);index+=2;
            }
            for(var i=0;i<Totalcurrent.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Totalcurrent[i]);index+=2;
            }
            for(var i=0;i<Rpm.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Rpm[i]);index+=2;
            }
            for(var i=0;i<Count.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Count[i]);index+=2;
            }
            for(var i=0;i<Temperature.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Temperature[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/44;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            Voltage = new ushort[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Voltage[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Current[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Totalcurrent[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Rpm[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Count[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Temperature[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            for(var i=0;i<Voltage.Length;i++)
            {
                BitConverter.GetBytes(Voltage[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Current.Length;i++)
            {
                BitConverter.GetBytes(Current[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Totalcurrent.Length;i++)
            {
                BitConverter.GetBytes(Totalcurrent[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Rpm.Length;i++)
            {
                BitConverter.GetBytes(Rpm[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Count.Length;i++)
            {
                BitConverter.GetBytes(Count[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Temperature.Length;i++)
            {
                buffer[index] = (byte)Temperature[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/44;
        }

        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: cV, IsExtended: false
        /// </summary>
        public ushort[] Voltage { get; set; } = new ushort[4];
        public byte GetVoltageMaxItemsCount() => 4;
        /// <summary>
        /// Current.
        /// OriginName: current, Units: cA, IsExtended: false
        /// </summary>
        public ushort[] Current { get; } = new ushort[4];
        /// <summary>
        /// Total current.
        /// OriginName: totalcurrent, Units: mAh, IsExtended: false
        /// </summary>
        public ushort[] Totalcurrent { get; } = new ushort[4];
        /// <summary>
        /// RPM (eRPM).
        /// OriginName: rpm, Units: rpm, IsExtended: false
        /// </summary>
        public ushort[] Rpm { get; } = new ushort[4];
        /// <summary>
        /// count of telemetry packets received (wraps at 65535).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public ushort[] Count { get; } = new ushort[4];
        /// <summary>
        /// Temperature.
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
        public byte[] Temperature { get; } = new byte[4];
    }
    /// <summary>
    /// ESC Telemetry Data for ESCs 9 to 12, matching data sent by BLHeli ESCs.
    ///  ESC_TELEMETRY_9_TO_12
    /// </summary>
    public class EscTelemetry9To12Packet: PacketV2<EscTelemetry9To12Payload>
    {
	    public const int PacketMessageId = 11032;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 85;

        public override EscTelemetry9To12Payload Payload { get; } = new EscTelemetry9To12Payload();

        public override string Name => "ESC_TELEMETRY_9_TO_12";
    }

    /// <summary>
    ///  ESC_TELEMETRY_9_TO_12
    /// </summary>
    public class EscTelemetry9To12Payload : IPayload
    {
        public byte GetMaxByteSize() => 44; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            Voltage = new ushort[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Voltage[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Current[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Totalcurrent[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Rpm[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Count[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Temperature[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            for(var i=0;i<Voltage.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Voltage[i]);index+=2;
            }
            for(var i=0;i<Current.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Current[i]);index+=2;
            }
            for(var i=0;i<Totalcurrent.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Totalcurrent[i]);index+=2;
            }
            for(var i=0;i<Rpm.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Rpm[i]);index+=2;
            }
            for(var i=0;i<Count.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Count[i]);index+=2;
            }
            for(var i=0;i<Temperature.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Temperature[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/44;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            Voltage = new ushort[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Voltage[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Current[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Totalcurrent[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Rpm[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Count[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Temperature[i] = (byte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            for(var i=0;i<Voltage.Length;i++)
            {
                BitConverter.GetBytes(Voltage[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Current.Length;i++)
            {
                BitConverter.GetBytes(Current[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Totalcurrent.Length;i++)
            {
                BitConverter.GetBytes(Totalcurrent[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Rpm.Length;i++)
            {
                BitConverter.GetBytes(Rpm[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Count.Length;i++)
            {
                BitConverter.GetBytes(Count[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<Temperature.Length;i++)
            {
                buffer[index] = (byte)Temperature[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/44;
        }

        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: cV, IsExtended: false
        /// </summary>
        public ushort[] Voltage { get; set; } = new ushort[4];
        public byte GetVoltageMaxItemsCount() => 4;
        /// <summary>
        /// Current.
        /// OriginName: current, Units: cA, IsExtended: false
        /// </summary>
        public ushort[] Current { get; } = new ushort[4];
        /// <summary>
        /// Total current.
        /// OriginName: totalcurrent, Units: mAh, IsExtended: false
        /// </summary>
        public ushort[] Totalcurrent { get; } = new ushort[4];
        /// <summary>
        /// RPM (eRPM).
        /// OriginName: rpm, Units: rpm, IsExtended: false
        /// </summary>
        public ushort[] Rpm { get; } = new ushort[4];
        /// <summary>
        /// count of telemetry packets received (wraps at 65535).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public ushort[] Count { get; } = new ushort[4];
        /// <summary>
        /// Temperature.
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
        public byte[] Temperature { get; } = new byte[4];
    }


#endregion


}

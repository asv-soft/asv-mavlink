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

namespace Asv.Mavlink.Ardupilotmega
{

    public static class ArdupilotmegaHelper
    {
        public static void RegisterArdupilotmegaDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(SensorOffsetsPacket.MessageId, ()=>new SensorOffsetsPacket());
            src.Add(SetMagOffsetsPacket.MessageId, ()=>new SetMagOffsetsPacket());
            src.Add(MeminfoPacket.MessageId, ()=>new MeminfoPacket());
            src.Add(ApAdcPacket.MessageId, ()=>new ApAdcPacket());
            src.Add(DigicamConfigurePacket.MessageId, ()=>new DigicamConfigurePacket());
            src.Add(DigicamControlPacket.MessageId, ()=>new DigicamControlPacket());
            src.Add(MountConfigurePacket.MessageId, ()=>new MountConfigurePacket());
            src.Add(MountControlPacket.MessageId, ()=>new MountControlPacket());
            src.Add(MountStatusPacket.MessageId, ()=>new MountStatusPacket());
            src.Add(FencePointPacket.MessageId, ()=>new FencePointPacket());
            src.Add(FenceFetchPointPacket.MessageId, ()=>new FenceFetchPointPacket());
            src.Add(AhrsPacket.MessageId, ()=>new AhrsPacket());
            src.Add(SimstatePacket.MessageId, ()=>new SimstatePacket());
            src.Add(HwstatusPacket.MessageId, ()=>new HwstatusPacket());
            src.Add(RadioPacket.MessageId, ()=>new RadioPacket());
            src.Add(LimitsStatusPacket.MessageId, ()=>new LimitsStatusPacket());
            src.Add(WindPacket.MessageId, ()=>new WindPacket());
            src.Add(Data16Packet.MessageId, ()=>new Data16Packet());
            src.Add(Data32Packet.MessageId, ()=>new Data32Packet());
            src.Add(Data64Packet.MessageId, ()=>new Data64Packet());
            src.Add(Data96Packet.MessageId, ()=>new Data96Packet());
            src.Add(RangefinderPacket.MessageId, ()=>new RangefinderPacket());
            src.Add(AirspeedAutocalPacket.MessageId, ()=>new AirspeedAutocalPacket());
            src.Add(RallyPointPacket.MessageId, ()=>new RallyPointPacket());
            src.Add(RallyFetchPointPacket.MessageId, ()=>new RallyFetchPointPacket());
            src.Add(CompassmotStatusPacket.MessageId, ()=>new CompassmotStatusPacket());
            src.Add(Ahrs2Packet.MessageId, ()=>new Ahrs2Packet());
            src.Add(CameraStatusPacket.MessageId, ()=>new CameraStatusPacket());
            src.Add(CameraFeedbackPacket.MessageId, ()=>new CameraFeedbackPacket());
            src.Add(Battery2Packet.MessageId, ()=>new Battery2Packet());
            src.Add(Ahrs3Packet.MessageId, ()=>new Ahrs3Packet());
            src.Add(AutopilotVersionRequestPacket.MessageId, ()=>new AutopilotVersionRequestPacket());
            src.Add(RemoteLogDataBlockPacket.MessageId, ()=>new RemoteLogDataBlockPacket());
            src.Add(RemoteLogBlockStatusPacket.MessageId, ()=>new RemoteLogBlockStatusPacket());
            src.Add(LedControlPacket.MessageId, ()=>new LedControlPacket());
            src.Add(MagCalProgressPacket.MessageId, ()=>new MagCalProgressPacket());
            src.Add(EkfStatusReportPacket.MessageId, ()=>new EkfStatusReportPacket());
            src.Add(PidTuningPacket.MessageId, ()=>new PidTuningPacket());
            src.Add(DeepstallPacket.MessageId, ()=>new DeepstallPacket());
            src.Add(GimbalReportPacket.MessageId, ()=>new GimbalReportPacket());
            src.Add(GimbalControlPacket.MessageId, ()=>new GimbalControlPacket());
            src.Add(GimbalTorqueCmdReportPacket.MessageId, ()=>new GimbalTorqueCmdReportPacket());
            src.Add(GoproHeartbeatPacket.MessageId, ()=>new GoproHeartbeatPacket());
            src.Add(GoproGetRequestPacket.MessageId, ()=>new GoproGetRequestPacket());
            src.Add(GoproGetResponsePacket.MessageId, ()=>new GoproGetResponsePacket());
            src.Add(GoproSetRequestPacket.MessageId, ()=>new GoproSetRequestPacket());
            src.Add(GoproSetResponsePacket.MessageId, ()=>new GoproSetResponsePacket());
            src.Add(RpmPacket.MessageId, ()=>new RpmPacket());
            src.Add(DeviceOpReadPacket.MessageId, ()=>new DeviceOpReadPacket());
            src.Add(DeviceOpReadReplyPacket.MessageId, ()=>new DeviceOpReadReplyPacket());
            src.Add(DeviceOpWritePacket.MessageId, ()=>new DeviceOpWritePacket());
            src.Add(DeviceOpWriteReplyPacket.MessageId, ()=>new DeviceOpWriteReplyPacket());
            src.Add(AdapTuningPacket.MessageId, ()=>new AdapTuningPacket());
            src.Add(VisionPositionDeltaPacket.MessageId, ()=>new VisionPositionDeltaPacket());
            src.Add(AoaSsaPacket.MessageId, ()=>new AoaSsaPacket());
            src.Add(EscTelemetry1To4Packet.MessageId, ()=>new EscTelemetry1To4Packet());
            src.Add(EscTelemetry5To8Packet.MessageId, ()=>new EscTelemetry5To8Packet());
            src.Add(EscTelemetry9To12Packet.MessageId, ()=>new EscTelemetry9To12Packet());
            src.Add(OsdParamConfigPacket.MessageId, ()=>new OsdParamConfigPacket());
            src.Add(OsdParamConfigReplyPacket.MessageId, ()=>new OsdParamConfigReplyPacket());
            src.Add(OsdParamShowConfigPacket.MessageId, ()=>new OsdParamShowConfigPacket());
            src.Add(OsdParamShowConfigReplyPacket.MessageId, ()=>new OsdParamShowConfigReplyPacket());
            src.Add(ObstacleDistance3dPacket.MessageId, ()=>new ObstacleDistance3dPacket());
            src.Add(WaterDepthPacket.MessageId, ()=>new WaterDepthPacket());
            src.Add(McuStatusPacket.MessageId, ()=>new McuStatusPacket());
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
    ///  HEADING_TYPE
    /// </summary>
    public enum HeadingType:uint
    {
        /// <summary>
        /// HEADING_TYPE_COURSE_OVER_GROUND
        /// </summary>
        HeadingTypeCourseOverGround = 0,
        /// <summary>
        /// HEADING_TYPE_HEADING
        /// </summary>
        HeadingTypeHeading = 1,
    }

    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd:uint
    {
        /// <summary>
        /// Set the distance to be repeated on mission resume
        /// Param 1 - Distance.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_SET_RESUME_REPEAT_DIST
        /// </summary>
        MavCmdDoSetResumeRepeatDist = 215,
        /// <summary>
        /// Control attached liquid sprayer
        /// Param 1 - 0: disable sprayer. 1: enable sprayer.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_SPRAYER
        /// </summary>
        MavCmdDoSprayer = 216,
        /// <summary>
        /// Pass instructions onto scripting, a script should be checking for a new command
        /// Param 1 - uint16 ID value to be passed to scripting
        /// Param 2 - float value to be passed to scripting
        /// Param 3 - float value to be passed to scripting
        /// Param 4 - float value to be passed to scripting
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_SEND_SCRIPT_MESSAGE
        /// </summary>
        MavCmdDoSendScriptMessage = 217,
        /// <summary>
        /// Execute auxiliary function
        /// Param 1 - Auxiliary Function.
        /// Param 2 - Switch Level.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_AUX_FUNCTION
        /// </summary>
        MavCmdDoAuxFunction = 218,
        /// <summary>
        /// Mission command to wait for an altitude or downwards vertical speed. This is meant for high altitude balloon launches, allowing the aircraft to be idle until either an altitude is reached or a negative vertical speed is reached (indicating early balloon burst). The wiggle time is how often to wiggle the control surfaces to prevent them seizing up.
        /// Param 1 - Altitude.
        /// Param 2 - Descent speed.
        /// Param 3 - How long to wiggle the control surfaces to prevent them seizing up.
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
        /// Param 1 - Magnetic declination.
        /// Param 2 - Magnetic inclination.
        /// Param 3 - Magnetic intensity.
        /// Param 4 - Yaw.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_FIXED_MAG_CAL
        /// </summary>
        MavCmdFixedMagCal = 42004,
        /// <summary>
        /// Magnetometer calibration based on fixed expected field values.
        /// Param 1 - Field strength X.
        /// Param 2 - Field strength Y.
        /// Param 3 - Field strength Z.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_FIXED_MAG_CAL_FIELD
        /// </summary>
        MavCmdFixedMagCalField = 42005,
        /// <summary>
        /// Set EKF sensor source set.
        /// Param 1 - Source Set Id.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_SET_EKF_SOURCE_SET
        /// </summary>
        MavCmdSetEkfSourceSet = 42007,
        /// <summary>
        /// Initiate a magnetometer calibration.
        /// Param 1 - Bitmask of magnetometers to calibrate. Use 0 to calibrate all sensors that can be started (sensors may not start if disabled, unhealthy, etc.). The command will NACK if calibration does not start for a sensor explicitly specified by the bitmask.
        /// Param 2 - Automatically retry on failure (0=no retry, 1=retry).
        /// Param 3 - Save without user input (0=require input, 1=autosave).
        /// Param 4 - Delay.
        /// Param 5 - Autoreboot (0=user reboot, 1=autoreboot).
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DO_START_MAG_CAL
        /// </summary>
        MavCmdDoStartMagCal = 42424,
        /// <summary>
        /// Accept a magnetometer calibration.
        /// Param 1 - Bitmask of magnetometers that calibration is accepted (0 means all).
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
        /// Param 1 - Bitmask of magnetometers to cancel a running calibration (0 means all).
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
        /// Param 1 - Position.
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
        /// Param 1 - 0: activate test mode, 1: exit test mode.
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
        /// Param 2 - Current calibration progress for this axis.
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
        /// <summary>
        /// Reset battery capacity for batteries that accumulate consumed battery via integration.
        /// Param 1 - Bitmask of batteries to reset. Least significant bit is for the first battery.
        /// Param 2 - Battery percentage remaining to set.
        /// MAV_CMD_BATTERY_RESET
        /// </summary>
        MavCmdBatteryReset = 42651,
        /// <summary>
        /// Issue a trap signal to the autopilot process, presumably to enter the debugger.
        /// Param 1 - Magic number - set to 32451 to actually trap.
        /// Param 2 - Empty.
        /// Param 3 - Empty.
        /// Param 4 - Empty.
        /// Param 5 - Empty.
        /// Param 6 - Empty.
        /// Param 7 - Empty.
        /// MAV_CMD_DEBUG_TRAP
        /// </summary>
        MavCmdDebugTrap = 42700,
        /// <summary>
        /// Control onboard scripting.
        /// Param 1 - Scripting command to execute
        /// MAV_CMD_SCRIPTING
        /// </summary>
        MavCmdScripting = 42701,
        /// <summary>
        /// Scripting command as NAV command with wait for completion.
        /// Param 1 - integer command number (0 to 255)
        /// Param 2 - timeout for operation in seconds. Zero means no timeout (0 to 255)
        /// Param 3 - argument1.
        /// Param 4 - argument2.
        /// Param 5 - Empty
        /// Param 6 - Empty
        /// Param 7 - Empty
        /// MAV_CMD_NAV_SCRIPT_TIME
        /// </summary>
        MavCmdNavScriptTime = 42702,
        /// <summary>
        /// Maintain an attitude for a specified time.
        /// Param 1 - Time to maintain specified attitude and climb rate
        /// Param 2 - Roll angle in degrees (positive is lean right, negative is lean left)
        /// Param 3 - Pitch angle in degrees (positive is lean back, negative is lean forward)
        /// Param 4 - Yaw angle
        /// Param 5 - Climb rate
        /// Param 6 - Empty
        /// Param 7 - Empty
        /// MAV_CMD_NAV_ATTITUDE_TIME
        /// </summary>
        MavCmdNavAttitudeTime = 42703,
        /// <summary>
        /// Change flight speed at a given rate. This slews the vehicle at a controllable rate between it's previous speed and the new one. (affects GUIDED only. Outside GUIDED, aircraft ignores these commands. Designed for onboard companion-computer command-and-control, not normally operator/GCS control.)
        /// Param 1 - Airspeed or groundspeed.
        /// Param 2 - Target Speed
        /// Param 3 - Acceleration rate, 0 to take effect instantly
        /// Param 4 - Empty
        /// Param 5 - Empty
        /// Param 6 - Empty
        /// Param 7 - Empty
        /// MAV_CMD_GUIDED_CHANGE_SPEED
        /// </summary>
        MavCmdGuidedChangeSpeed = 43000,
        /// <summary>
        /// Change target altitude at a given rate. This slews the vehicle at a controllable rate between it's previous altitude and the new one. (affects GUIDED only. Outside GUIDED, aircraft ignores these commands. Designed for onboard companion-computer command-and-control, not normally operator/GCS control.)
        /// Param 1 - Empty
        /// Param 2 - Empty
        /// Param 3 - Rate of change, toward new altitude. 0 for maximum rate change. Positive numbers only, as negative numbers will not converge on the new target alt.
        /// Param 4 - Empty
        /// Param 5 - Empty
        /// Param 6 - Empty
        /// Param 7 - Target Altitude
        /// MAV_CMD_GUIDED_CHANGE_ALTITUDE
        /// </summary>
        MavCmdGuidedChangeAltitude = 43001,
        /// <summary>
        /// Change to target heading at a given rate, overriding previous heading/s. This slews the vehicle at a controllable rate between it's previous heading and the new one. (affects GUIDED only. Exiting GUIDED returns aircraft to normal behaviour defined elsewhere. Designed for onboard companion-computer command-and-control, not normally operator/GCS control.)
        /// Param 1 - course-over-ground or raw vehicle heading.
        /// Param 2 - Target heading.
        /// Param 3 - Maximum centripetal accelearation, ie rate of change,  toward new heading.
        /// Param 4 - Empty
        /// Param 5 - Empty
        /// Param 6 - Empty
        /// Param 7 - Empty
        /// MAV_CMD_GUIDED_CHANGE_HEADING
        /// </summary>
        MavCmdGuidedChangeHeading = 43002,
        /// <summary>
        /// Provide an external position estimate for use when dead-reckoning. This is meant to be used for occasional position resets that may be provided by a external system such as a remote pilot using landmarks over a video link.
        /// Param 1 - Timestamp that this message was sent as a time in the transmitters time domain. The sender should wrap this time back to zero based on required timing accuracy for the application and the limitations of a 32 bit float. For example, wrapping at 10 hours would give approximately 1ms accuracy. Recipient must handle time wrap in any timing jitter correction applied to this field. Wrap rollover time should not be at not more than 250 seconds, which would give approximately 10 microsecond accuracy.
        /// Param 2 - The time spent in processing the sensor data that is the basis for this position. The recipient can use this to improve time alignment of the data. Set to zero if not known.
        /// Param 3 - estimated one standard deviation accuracy of the measurement. Set to NaN if not known.
        /// Param 4 - Empty
        /// Param 5 - Latitude
        /// Param 6 - Longitude
        /// Param 7 - Altitude, not used. Should be sent as NaN. May be supported in a future version of this message.
        /// MAV_CMD_EXTERNAL_POSITION_ESTIMATE
        /// </summary>
        MavCmdExternalPositionEstimate = 43003,
    }

    /// <summary>
    ///  SCRIPTING_CMD
    /// </summary>
    public enum ScriptingCmd:uint
    {
        /// <summary>
        /// Start a REPL session.
        /// SCRIPTING_CMD_REPL_START
        /// </summary>
        ScriptingCmdReplStart = 0,
        /// <summary>
        /// End a REPL session.
        /// SCRIPTING_CMD_REPL_STOP
        /// </summary>
        ScriptingCmdReplStop = 1,
        /// <summary>
        /// Stop execution of scripts.
        /// SCRIPTING_CMD_STOP
        /// </summary>
        ScriptingCmdStop = 2,
        /// <summary>
        /// Stop execution of scripts and restart.
        /// SCRIPTING_CMD_STOP_AND_RESTART
        /// </summary>
        ScriptingCmdStopAndRestart = 3,
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
    ///  MAV_CMD_DO_AUX_FUNCTION_SWITCH_LEVEL
    /// </summary>
    public enum MavCmdDoAuxFunctionSwitchLevel:uint
    {
        /// <summary>
        /// Switch Low.
        /// MAV_CMD_DO_AUX_FUNCTION_SWITCH_LEVEL_LOW
        /// </summary>
        MavCmdDoAuxFunctionSwitchLevelLow = 0,
        /// <summary>
        /// Switch Middle.
        /// MAV_CMD_DO_AUX_FUNCTION_SWITCH_LEVEL_MIDDLE
        /// </summary>
        MavCmdDoAuxFunctionSwitchLevelMiddle = 1,
        /// <summary>
        /// Switch High.
        /// MAV_CMD_DO_AUX_FUNCTION_SWITCH_LEVEL_HIGH
        /// </summary>
        MavCmdDoAuxFunctionSwitchLevelHigh = 2,
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
        /// <summary>
        /// Set if EKF has never been healthy.
        /// EKF_UNINITIALIZED
        /// </summary>
        EkfUninitialized = 1024,
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
        /// PLANE_MODE_TAKEOFF
        /// </summary>
        PlaneModeTakeoff = 13,
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
        /// <summary>
        /// PLANE_MODE_QACRO
        /// </summary>
        PlaneModeQacro = 23,
        /// <summary>
        /// PLANE_MODE_THERMAL
        /// </summary>
        PlaneModeThermal = 24,
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
        /// <summary>
        /// COPTER_MODE_FLOWHOLD
        /// </summary>
        CopterModeFlowhold = 22,
        /// <summary>
        /// COPTER_MODE_FOLLOW
        /// </summary>
        CopterModeFollow = 23,
        /// <summary>
        /// COPTER_MODE_ZIGZAG
        /// </summary>
        CopterModeZigzag = 24,
        /// <summary>
        /// COPTER_MODE_SYSTEMID
        /// </summary>
        CopterModeSystemid = 25,
        /// <summary>
        /// COPTER_MODE_AUTOROTATE
        /// </summary>
        CopterModeAutorotate = 26,
        /// <summary>
        /// COPTER_MODE_AUTO_RTL
        /// </summary>
        CopterModeAutoRtl = 27,
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
        /// ROVER_MODE_FOLLOW
        /// </summary>
        RoverModeFollow = 6,
        /// <summary>
        /// ROVER_MODE_SIMPLE
        /// </summary>
        RoverModeSimple = 7,
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

    /// <summary>
    /// The type of parameter for the OSD parameter editor.
    ///  OSD_PARAM_CONFIG_TYPE
    /// </summary>
    public enum OsdParamConfigType:uint
    {
        /// <summary>
        /// OSD_PARAM_NONE
        /// </summary>
        OsdParamNone = 0,
        /// <summary>
        /// OSD_PARAM_SERIAL_PROTOCOL
        /// </summary>
        OsdParamSerialProtocol = 1,
        /// <summary>
        /// OSD_PARAM_SERVO_FUNCTION
        /// </summary>
        OsdParamServoFunction = 2,
        /// <summary>
        /// OSD_PARAM_AUX_FUNCTION
        /// </summary>
        OsdParamAuxFunction = 3,
        /// <summary>
        /// OSD_PARAM_FLIGHT_MODE
        /// </summary>
        OsdParamFlightMode = 4,
        /// <summary>
        /// OSD_PARAM_FAILSAFE_ACTION
        /// </summary>
        OsdParamFailsafeAction = 5,
        /// <summary>
        /// OSD_PARAM_FAILSAFE_ACTION_1
        /// </summary>
        OsdParamFailsafeAction1 = 6,
        /// <summary>
        /// OSD_PARAM_FAILSAFE_ACTION_2
        /// </summary>
        OsdParamFailsafeAction2 = 7,
        /// <summary>
        /// OSD_PARAM_NUM_TYPES
        /// </summary>
        OsdParamNumTypes = 8,
    }

    /// <summary>
    /// The error type for the OSD parameter editor.
    ///  OSD_PARAM_CONFIG_ERROR
    /// </summary>
    public enum OsdParamConfigError:uint
    {
        /// <summary>
        /// OSD_PARAM_SUCCESS
        /// </summary>
        OsdParamSuccess = 0,
        /// <summary>
        /// OSD_PARAM_INVALID_SCREEN
        /// </summary>
        OsdParamInvalidScreen = 1,
        /// <summary>
        /// OSD_PARAM_INVALID_PARAMETER_INDEX
        /// </summary>
        OsdParamInvalidParameterIndex = 2,
        /// <summary>
        /// OSD_PARAM_INVALID_PARAMETER
        /// </summary>
        OsdParamInvalidParameter = 3,
    }


#endregion

#region Messages

    /// <summary>
    /// Offsets and calibrations values for hardware sensors. This makes it easier to debug the calibration process.
    ///  SENSOR_OFFSETS
    /// </summary>
    public class SensorOffsetsPacket : MavlinkV2Message<SensorOffsetsPayload>
    {
        public const int MessageId = 150;
        
        public const byte CrcExtra = 134;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SensorOffsetsPayload Payload { get; } = new();

        public override string Name => "SENSOR_OFFSETS";
    }

    /// <summary>
    ///  SENSOR_OFFSETS
    /// </summary>
    public class SensorOffsetsPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 42; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 42; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float mag_declination
            +4 // int32_t raw_press
            +4 // int32_t raw_temp
            +4 // float gyro_cal_x
            +4 // float gyro_cal_y
            +4 // float gyro_cal_z
            +4 // float accel_cal_x
            +4 // float accel_cal_y
            +4 // float accel_cal_z
            +2 // int16_t mag_ofs_x
            +2 // int16_t mag_ofs_y
            +2 // int16_t mag_ofs_z
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            MagDeclination = BinSerialize.ReadFloat(ref buffer);
            RawPress = BinSerialize.ReadInt(ref buffer);
            RawTemp = BinSerialize.ReadInt(ref buffer);
            GyroCalX = BinSerialize.ReadFloat(ref buffer);
            GyroCalY = BinSerialize.ReadFloat(ref buffer);
            GyroCalZ = BinSerialize.ReadFloat(ref buffer);
            AccelCalX = BinSerialize.ReadFloat(ref buffer);
            AccelCalY = BinSerialize.ReadFloat(ref buffer);
            AccelCalZ = BinSerialize.ReadFloat(ref buffer);
            MagOfsX = BinSerialize.ReadShort(ref buffer);
            MagOfsY = BinSerialize.ReadShort(ref buffer);
            MagOfsZ = BinSerialize.ReadShort(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,MagDeclination);
            BinSerialize.WriteInt(ref buffer,RawPress);
            BinSerialize.WriteInt(ref buffer,RawTemp);
            BinSerialize.WriteFloat(ref buffer,GyroCalX);
            BinSerialize.WriteFloat(ref buffer,GyroCalY);
            BinSerialize.WriteFloat(ref buffer,GyroCalZ);
            BinSerialize.WriteFloat(ref buffer,AccelCalX);
            BinSerialize.WriteFloat(ref buffer,AccelCalY);
            BinSerialize.WriteFloat(ref buffer,AccelCalZ);
            BinSerialize.WriteShort(ref buffer,MagOfsX);
            BinSerialize.WriteShort(ref buffer,MagOfsY);
            BinSerialize.WriteShort(ref buffer,MagOfsZ);
            /* PayloadByteSize = 42 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,MagDeclinationField, ref _MagDeclination);    
            Int32Type.Accept(visitor,RawPressField, ref _RawPress);    
            Int32Type.Accept(visitor,RawTempField, ref _RawTemp);    
            FloatType.Accept(visitor,GyroCalXField, ref _GyroCalX);    
            FloatType.Accept(visitor,GyroCalYField, ref _GyroCalY);    
            FloatType.Accept(visitor,GyroCalZField, ref _GyroCalZ);    
            FloatType.Accept(visitor,AccelCalXField, ref _AccelCalX);    
            FloatType.Accept(visitor,AccelCalYField, ref _AccelCalY);    
            FloatType.Accept(visitor,AccelCalZField, ref _AccelCalZ);    
            Int16Type.Accept(visitor,MagOfsXField, ref _MagOfsX);
            Int16Type.Accept(visitor,MagOfsYField, ref _MagOfsY);
            Int16Type.Accept(visitor,MagOfsZField, ref _MagOfsZ);

        }

        /// <summary>
        /// Magnetic declination.
        /// OriginName: mag_declination, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field MagDeclinationField = new Field.Builder()
            .Name(nameof(MagDeclination))
            .Title("mag_declination")
            .Description("Magnetic declination.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _MagDeclination;
        public float MagDeclination { get => _MagDeclination; set { _MagDeclination = value; } }
        /// <summary>
        /// Raw pressure from barometer.
        /// OriginName: raw_press, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RawPressField = new Field.Builder()
            .Name(nameof(RawPress))
            .Title("raw_press")
            .Description("Raw pressure from barometer.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int32Type.Default)

            .Build();
        private int _RawPress;
        public int RawPress { get => _RawPress; set { _RawPress = value; } }
        /// <summary>
        /// Raw temperature from barometer.
        /// OriginName: raw_temp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RawTempField = new Field.Builder()
            .Name(nameof(RawTemp))
            .Title("raw_temp")
            .Description("Raw temperature from barometer.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int32Type.Default)

            .Build();
        private int _RawTemp;
        public int RawTemp { get => _RawTemp; set { _RawTemp = value; } }
        /// <summary>
        /// Gyro X calibration.
        /// OriginName: gyro_cal_x, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GyroCalXField = new Field.Builder()
            .Name(nameof(GyroCalX))
            .Title("gyro_cal_x")
            .Description("Gyro X calibration.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _GyroCalX;
        public float GyroCalX { get => _GyroCalX; set { _GyroCalX = value; } }
        /// <summary>
        /// Gyro Y calibration.
        /// OriginName: gyro_cal_y, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GyroCalYField = new Field.Builder()
            .Name(nameof(GyroCalY))
            .Title("gyro_cal_y")
            .Description("Gyro Y calibration.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _GyroCalY;
        public float GyroCalY { get => _GyroCalY; set { _GyroCalY = value; } }
        /// <summary>
        /// Gyro Z calibration.
        /// OriginName: gyro_cal_z, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GyroCalZField = new Field.Builder()
            .Name(nameof(GyroCalZ))
            .Title("gyro_cal_z")
            .Description("Gyro Z calibration.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _GyroCalZ;
        public float GyroCalZ { get => _GyroCalZ; set { _GyroCalZ = value; } }
        /// <summary>
        /// Accel X calibration.
        /// OriginName: accel_cal_x, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AccelCalXField = new Field.Builder()
            .Name(nameof(AccelCalX))
            .Title("accel_cal_x")
            .Description("Accel X calibration.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _AccelCalX;
        public float AccelCalX { get => _AccelCalX; set { _AccelCalX = value; } }
        /// <summary>
        /// Accel Y calibration.
        /// OriginName: accel_cal_y, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AccelCalYField = new Field.Builder()
            .Name(nameof(AccelCalY))
            .Title("accel_cal_y")
            .Description("Accel Y calibration.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _AccelCalY;
        public float AccelCalY { get => _AccelCalY; set { _AccelCalY = value; } }
        /// <summary>
        /// Accel Z calibration.
        /// OriginName: accel_cal_z, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AccelCalZField = new Field.Builder()
            .Name(nameof(AccelCalZ))
            .Title("accel_cal_z")
            .Description("Accel Z calibration.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _AccelCalZ;
        public float AccelCalZ { get => _AccelCalZ; set { _AccelCalZ = value; } }
        /// <summary>
        /// Magnetometer X offset.
        /// OriginName: mag_ofs_x, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MagOfsXField = new Field.Builder()
            .Name(nameof(MagOfsX))
            .Title("mag_ofs_x")
            .Description("Magnetometer X offset.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _MagOfsX;
        public short MagOfsX { get => _MagOfsX; set { _MagOfsX = value; } }
        /// <summary>
        /// Magnetometer Y offset.
        /// OriginName: mag_ofs_y, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MagOfsYField = new Field.Builder()
            .Name(nameof(MagOfsY))
            .Title("mag_ofs_y")
            .Description("Magnetometer Y offset.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _MagOfsY;
        public short MagOfsY { get => _MagOfsY; set { _MagOfsY = value; } }
        /// <summary>
        /// Magnetometer Z offset.
        /// OriginName: mag_ofs_z, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MagOfsZField = new Field.Builder()
            .Name(nameof(MagOfsZ))
            .Title("mag_ofs_z")
            .Description("Magnetometer Z offset.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _MagOfsZ;
        public short MagOfsZ { get => _MagOfsZ; set { _MagOfsZ = value; } }
    }
    /// <summary>
    /// Set the magnetometer offsets
    ///  SET_MAG_OFFSETS
    /// </summary>
    public class SetMagOffsetsPacket : MavlinkV2Message<SetMagOffsetsPayload>
    {
        public const int MessageId = 151;
        
        public const byte CrcExtra = 219;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SetMagOffsetsPayload Payload { get; } = new();

        public override string Name => "SET_MAG_OFFSETS";
    }

    /// <summary>
    ///  SET_MAG_OFFSETS
    /// </summary>
    public class SetMagOffsetsPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // int16_t mag_ofs_x
            +2 // int16_t mag_ofs_y
            +2 // int16_t mag_ofs_z
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            MagOfsX = BinSerialize.ReadShort(ref buffer);
            MagOfsY = BinSerialize.ReadShort(ref buffer);
            MagOfsZ = BinSerialize.ReadShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteShort(ref buffer,MagOfsX);
            BinSerialize.WriteShort(ref buffer,MagOfsY);
            BinSerialize.WriteShort(ref buffer,MagOfsZ);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 8 */;
        }

        public void Visit(IVisitor visitor)
        {
            Int16Type.Accept(visitor,MagOfsXField, ref _MagOfsX);
            Int16Type.Accept(visitor,MagOfsYField, ref _MagOfsY);
            Int16Type.Accept(visitor,MagOfsZField, ref _MagOfsZ);
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// Magnetometer X offset.
        /// OriginName: mag_ofs_x, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MagOfsXField = new Field.Builder()
            .Name(nameof(MagOfsX))
            .Title("mag_ofs_x")
            .Description("Magnetometer X offset.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _MagOfsX;
        public short MagOfsX { get => _MagOfsX; set { _MagOfsX = value; } }
        /// <summary>
        /// Magnetometer Y offset.
        /// OriginName: mag_ofs_y, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MagOfsYField = new Field.Builder()
            .Name(nameof(MagOfsY))
            .Title("mag_ofs_y")
            .Description("Magnetometer Y offset.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _MagOfsY;
        public short MagOfsY { get => _MagOfsY; set { _MagOfsY = value; } }
        /// <summary>
        /// Magnetometer Z offset.
        /// OriginName: mag_ofs_z, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MagOfsZField = new Field.Builder()
            .Name(nameof(MagOfsZ))
            .Title("mag_ofs_z")
            .Description("Magnetometer Z offset.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _MagOfsZ;
        public short MagOfsZ { get => _MagOfsZ; set { _MagOfsZ = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
    }
    /// <summary>
    /// State of autopilot RAM.
    ///  MEMINFO
    /// </summary>
    public class MeminfoPacket : MavlinkV2Message<MeminfoPayload>
    {
        public const int MessageId = 152;
        
        public const byte CrcExtra = 208;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override MeminfoPayload Payload { get; } = new();

        public override string Name => "MEMINFO";
    }

    /// <summary>
    ///  MEMINFO
    /// </summary>
    public class MeminfoPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t brkval
            +2 // uint16_t freemem
            +4 // uint32_t freemem32
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Brkval = BinSerialize.ReadUShort(ref buffer);
            Freemem = BinSerialize.ReadUShort(ref buffer);
            // extended field 'Freemem32' can be empty
            if (buffer.IsEmpty) return;
            Freemem32 = BinSerialize.ReadUInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,Brkval);
            BinSerialize.WriteUShort(ref buffer,Freemem);
            BinSerialize.WriteUInt(ref buffer,Freemem32);
            /* PayloadByteSize = 8 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,BrkvalField, ref _Brkval);    
            UInt16Type.Accept(visitor,FreememField, ref _Freemem);    
            UInt32Type.Accept(visitor,Freemem32Field, ref _Freemem32);    

        }

        /// <summary>
        /// Heap top.
        /// OriginName: brkval, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BrkvalField = new Field.Builder()
            .Name(nameof(Brkval))
            .Title("brkval")
            .Description("Heap top.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Brkval;
        public ushort Brkval { get => _Brkval; set { _Brkval = value; } }
        /// <summary>
        /// Free memory.
        /// OriginName: freemem, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field FreememField = new Field.Builder()
            .Name(nameof(Freemem))
            .Title("freemem")
            .Description("Free memory.")
            .FormatString(string.Empty)
            .Units(@"bytes")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Freemem;
        public ushort Freemem { get => _Freemem; set { _Freemem = value; } }
        /// <summary>
        /// Free memory (32 bit).
        /// OriginName: freemem32, Units: bytes, IsExtended: true
        /// </summary>
        public static readonly Field Freemem32Field = new Field.Builder()
            .Name(nameof(Freemem32))
            .Title("freemem32")
            .Description("Free memory (32 bit).")
            .FormatString(string.Empty)
            .Units(@"bytes")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Freemem32;
        public uint Freemem32 { get => _Freemem32; set { _Freemem32 = value; } }
    }
    /// <summary>
    /// Raw ADC output.
    ///  AP_ADC
    /// </summary>
    public class ApAdcPacket : MavlinkV2Message<ApAdcPayload>
    {
        public const int MessageId = 153;
        
        public const byte CrcExtra = 188;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override ApAdcPayload Payload { get; } = new();

        public override string Name => "AP_ADC";
    }

    /// <summary>
    ///  AP_ADC
    /// </summary>
    public class ApAdcPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 12; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 12; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t adc1
            +2 // uint16_t adc2
            +2 // uint16_t adc3
            +2 // uint16_t adc4
            +2 // uint16_t adc5
            +2 // uint16_t adc6
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Adc1 = BinSerialize.ReadUShort(ref buffer);
            Adc2 = BinSerialize.ReadUShort(ref buffer);
            Adc3 = BinSerialize.ReadUShort(ref buffer);
            Adc4 = BinSerialize.ReadUShort(ref buffer);
            Adc5 = BinSerialize.ReadUShort(ref buffer);
            Adc6 = BinSerialize.ReadUShort(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,Adc1);
            BinSerialize.WriteUShort(ref buffer,Adc2);
            BinSerialize.WriteUShort(ref buffer,Adc3);
            BinSerialize.WriteUShort(ref buffer,Adc4);
            BinSerialize.WriteUShort(ref buffer,Adc5);
            BinSerialize.WriteUShort(ref buffer,Adc6);
            /* PayloadByteSize = 12 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,Adc1Field, ref _Adc1);    
            UInt16Type.Accept(visitor,Adc2Field, ref _Adc2);    
            UInt16Type.Accept(visitor,Adc3Field, ref _Adc3);    
            UInt16Type.Accept(visitor,Adc4Field, ref _Adc4);    
            UInt16Type.Accept(visitor,Adc5Field, ref _Adc5);    
            UInt16Type.Accept(visitor,Adc6Field, ref _Adc6);    

        }

        /// <summary>
        /// ADC output 1.
        /// OriginName: adc1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Adc1Field = new Field.Builder()
            .Name(nameof(Adc1))
            .Title("adc1")
            .Description("ADC output 1.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Adc1;
        public ushort Adc1 { get => _Adc1; set { _Adc1 = value; } }
        /// <summary>
        /// ADC output 2.
        /// OriginName: adc2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Adc2Field = new Field.Builder()
            .Name(nameof(Adc2))
            .Title("adc2")
            .Description("ADC output 2.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Adc2;
        public ushort Adc2 { get => _Adc2; set { _Adc2 = value; } }
        /// <summary>
        /// ADC output 3.
        /// OriginName: adc3, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Adc3Field = new Field.Builder()
            .Name(nameof(Adc3))
            .Title("adc3")
            .Description("ADC output 3.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Adc3;
        public ushort Adc3 { get => _Adc3; set { _Adc3 = value; } }
        /// <summary>
        /// ADC output 4.
        /// OriginName: adc4, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Adc4Field = new Field.Builder()
            .Name(nameof(Adc4))
            .Title("adc4")
            .Description("ADC output 4.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Adc4;
        public ushort Adc4 { get => _Adc4; set { _Adc4 = value; } }
        /// <summary>
        /// ADC output 5.
        /// OriginName: adc5, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Adc5Field = new Field.Builder()
            .Name(nameof(Adc5))
            .Title("adc5")
            .Description("ADC output 5.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Adc5;
        public ushort Adc5 { get => _Adc5; set { _Adc5 = value; } }
        /// <summary>
        /// ADC output 6.
        /// OriginName: adc6, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Adc6Field = new Field.Builder()
            .Name(nameof(Adc6))
            .Title("adc6")
            .Description("ADC output 6.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Adc6;
        public ushort Adc6 { get => _Adc6; set { _Adc6 = value; } }
    }
    /// <summary>
    /// Configure on-board Camera Control System.
    ///  DIGICAM_CONFIGURE
    /// </summary>
    public class DigicamConfigurePacket : MavlinkV2Message<DigicamConfigurePayload>
    {
        public const int MessageId = 154;
        
        public const byte CrcExtra = 84;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override DigicamConfigurePayload Payload { get; } = new();

        public override string Name => "DIGICAM_CONFIGURE";
    }

    /// <summary>
    ///  DIGICAM_CONFIGURE
    /// </summary>
    public class DigicamConfigurePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 15; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 15; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float extra_value
            +2 // uint16_t shutter_speed
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t mode
            +1 // uint8_t aperture
            +1 // uint8_t iso
            +1 // uint8_t exposure_type
            +1 // uint8_t command_id
            +1 // uint8_t engine_cut_off
            +1 // uint8_t extra_param
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            ExtraValue = BinSerialize.ReadFloat(ref buffer);
            ShutterSpeed = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Mode = (byte)BinSerialize.ReadByte(ref buffer);
            Aperture = (byte)BinSerialize.ReadByte(ref buffer);
            Iso = (byte)BinSerialize.ReadByte(ref buffer);
            ExposureType = (byte)BinSerialize.ReadByte(ref buffer);
            CommandId = (byte)BinSerialize.ReadByte(ref buffer);
            EngineCutOff = (byte)BinSerialize.ReadByte(ref buffer);
            ExtraParam = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,ExtraValue);
            BinSerialize.WriteUShort(ref buffer,ShutterSpeed);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Mode);
            BinSerialize.WriteByte(ref buffer,(byte)Aperture);
            BinSerialize.WriteByte(ref buffer,(byte)Iso);
            BinSerialize.WriteByte(ref buffer,(byte)ExposureType);
            BinSerialize.WriteByte(ref buffer,(byte)CommandId);
            BinSerialize.WriteByte(ref buffer,(byte)EngineCutOff);
            BinSerialize.WriteByte(ref buffer,(byte)ExtraParam);
            /* PayloadByteSize = 15 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,ExtraValueField, ref _ExtraValue);    
            UInt16Type.Accept(visitor,ShutterSpeedField, ref _ShutterSpeed);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,ModeField, ref _Mode);    
            UInt8Type.Accept(visitor,ApertureField, ref _Aperture);    
            UInt8Type.Accept(visitor,IsoField, ref _Iso);    
            UInt8Type.Accept(visitor,ExposureTypeField, ref _ExposureType);    
            UInt8Type.Accept(visitor,CommandIdField, ref _CommandId);    
            UInt8Type.Accept(visitor,EngineCutOffField, ref _EngineCutOff);    
            UInt8Type.Accept(visitor,ExtraParamField, ref _ExtraParam);    

        }

        /// <summary>
        /// Correspondent value to given extra_param.
        /// OriginName: extra_value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ExtraValueField = new Field.Builder()
            .Name(nameof(ExtraValue))
            .Title("extra_value")
            .Description("Correspondent value to given extra_param.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _ExtraValue;
        public float ExtraValue { get => _ExtraValue; set { _ExtraValue = value; } }
        /// <summary>
        /// Divisor number //e.g. 1000 means 1/1000 (0 means ignore).
        /// OriginName: shutter_speed, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ShutterSpeedField = new Field.Builder()
            .Name(nameof(ShutterSpeed))
            .Title("shutter_speed")
            .Description("Divisor number //e.g. 1000 means 1/1000 (0 means ignore).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ShutterSpeed;
        public ushort ShutterSpeed { get => _ShutterSpeed; set { _ShutterSpeed = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Mode enumeration from 1 to N //P, TV, AV, M, etc. (0 means ignore).
        /// OriginName: mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ModeField = new Field.Builder()
            .Name(nameof(Mode))
            .Title("mode")
            .Description("Mode enumeration from 1 to N //P, TV, AV, M, etc. (0 means ignore).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Mode;
        public byte Mode { get => _Mode; set { _Mode = value; } }
        /// <summary>
        /// F stop number x 10 //e.g. 28 means 2.8 (0 means ignore).
        /// OriginName: aperture, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ApertureField = new Field.Builder()
            .Name(nameof(Aperture))
            .Title("aperture")
            .Description("F stop number x 10 //e.g. 28 means 2.8 (0 means ignore).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Aperture;
        public byte Aperture { get => _Aperture; set { _Aperture = value; } }
        /// <summary>
        /// ISO enumeration from 1 to N //e.g. 80, 100, 200, Etc (0 means ignore).
        /// OriginName: iso, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IsoField = new Field.Builder()
            .Name(nameof(Iso))
            .Title("iso")
            .Description("ISO enumeration from 1 to N //e.g. 80, 100, 200, Etc (0 means ignore).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Iso;
        public byte Iso { get => _Iso; set { _Iso = value; } }
        /// <summary>
        /// Exposure type enumeration from 1 to N (0 means ignore).
        /// OriginName: exposure_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ExposureTypeField = new Field.Builder()
            .Name(nameof(ExposureType))
            .Title("exposure_type")
            .Description("Exposure type enumeration from 1 to N (0 means ignore).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _ExposureType;
        public byte ExposureType { get => _ExposureType; set { _ExposureType = value; } }
        /// <summary>
        /// Command Identity (incremental loop: 0 to 255). //A command sent multiple times will be executed or pooled just once.
        /// OriginName: command_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CommandIdField = new Field.Builder()
            .Name(nameof(CommandId))
            .Title("command_id")
            .Description("Command Identity (incremental loop: 0 to 255). //A command sent multiple times will be executed or pooled just once.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _CommandId;
        public byte CommandId { get => _CommandId; set { _CommandId = value; } }
        /// <summary>
        /// Main engine cut-off time before camera trigger (0 means no cut-off).
        /// OriginName: engine_cut_off, Units: ds, IsExtended: false
        /// </summary>
        public static readonly Field EngineCutOffField = new Field.Builder()
            .Name(nameof(EngineCutOff))
            .Title("engine_cut_off")
            .Description("Main engine cut-off time before camera trigger (0 means no cut-off).")
            .FormatString(string.Empty)
            .Units(@"ds")
            .DataType(UInt8Type.Default)

            .Build();
        private byte _EngineCutOff;
        public byte EngineCutOff { get => _EngineCutOff; set { _EngineCutOff = value; } }
        /// <summary>
        /// Extra parameters enumeration (0 means ignore).
        /// OriginName: extra_param, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ExtraParamField = new Field.Builder()
            .Name(nameof(ExtraParam))
            .Title("extra_param")
            .Description("Extra parameters enumeration (0 means ignore).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _ExtraParam;
        public byte ExtraParam { get => _ExtraParam; set { _ExtraParam = value; } }
    }
    /// <summary>
    /// Control on-board Camera Control System to take shots.
    ///  DIGICAM_CONTROL
    /// </summary>
    public class DigicamControlPacket : MavlinkV2Message<DigicamControlPayload>
    {
        public const int MessageId = 155;
        
        public const byte CrcExtra = 22;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override DigicamControlPayload Payload { get; } = new();

        public override string Name => "DIGICAM_CONTROL";
    }

    /// <summary>
    ///  DIGICAM_CONTROL
    /// </summary>
    public class DigicamControlPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 13; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 13; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float extra_value
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t session
            +1 // uint8_t zoom_pos
            +1 // int8_t zoom_step
            +1 // uint8_t focus_lock
            +1 // uint8_t shot
            +1 // uint8_t command_id
            +1 // uint8_t extra_param
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            ExtraValue = BinSerialize.ReadFloat(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Session = (byte)BinSerialize.ReadByte(ref buffer);
            ZoomPos = (byte)BinSerialize.ReadByte(ref buffer);
            ZoomStep = (sbyte)BinSerialize.ReadByte(ref buffer);
            FocusLock = (byte)BinSerialize.ReadByte(ref buffer);
            Shot = (byte)BinSerialize.ReadByte(ref buffer);
            CommandId = (byte)BinSerialize.ReadByte(ref buffer);
            ExtraParam = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,ExtraValue);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Session);
            BinSerialize.WriteByte(ref buffer,(byte)ZoomPos);
            BinSerialize.WriteByte(ref buffer,(byte)ZoomStep);
            BinSerialize.WriteByte(ref buffer,(byte)FocusLock);
            BinSerialize.WriteByte(ref buffer,(byte)Shot);
            BinSerialize.WriteByte(ref buffer,(byte)CommandId);
            BinSerialize.WriteByte(ref buffer,(byte)ExtraParam);
            /* PayloadByteSize = 13 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,ExtraValueField, ref _ExtraValue);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,SessionField, ref _Session);    
            UInt8Type.Accept(visitor,ZoomPosField, ref _ZoomPos);    
            Int8Type.Accept(visitor,ZoomStepField, ref _ZoomStep);                
            UInt8Type.Accept(visitor,FocusLockField, ref _FocusLock);    
            UInt8Type.Accept(visitor,ShotField, ref _Shot);    
            UInt8Type.Accept(visitor,CommandIdField, ref _CommandId);    
            UInt8Type.Accept(visitor,ExtraParamField, ref _ExtraParam);    

        }

        /// <summary>
        /// Correspondent value to given extra_param.
        /// OriginName: extra_value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ExtraValueField = new Field.Builder()
            .Name(nameof(ExtraValue))
            .Title("extra_value")
            .Description("Correspondent value to given extra_param.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _ExtraValue;
        public float ExtraValue { get => _ExtraValue; set { _ExtraValue = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// 0: stop, 1: start or keep it up //Session control e.g. show/hide lens.
        /// OriginName: session, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SessionField = new Field.Builder()
            .Name(nameof(Session))
            .Title("session")
            .Description("0: stop, 1: start or keep it up //Session control e.g. show/hide lens.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Session;
        public byte Session { get => _Session; set { _Session = value; } }
        /// <summary>
        /// 1 to N //Zoom's absolute position (0 means ignore).
        /// OriginName: zoom_pos, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ZoomPosField = new Field.Builder()
            .Name(nameof(ZoomPos))
            .Title("zoom_pos")
            .Description("1 to N //Zoom's absolute position (0 means ignore).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _ZoomPos;
        public byte ZoomPos { get => _ZoomPos; set { _ZoomPos = value; } }
        /// <summary>
        /// -100 to 100 //Zooming step value to offset zoom from the current position.
        /// OriginName: zoom_step, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ZoomStepField = new Field.Builder()
            .Name(nameof(ZoomStep))
            .Title("zoom_step")
            .Description("-100 to 100 //Zooming step value to offset zoom from the current position.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int8Type.Default)

            .Build();
        private sbyte _ZoomStep;
        public sbyte ZoomStep { get => _ZoomStep; set { _ZoomStep = value; } }
        /// <summary>
        /// 0: unlock focus or keep unlocked, 1: lock focus or keep locked, 3: re-lock focus.
        /// OriginName: focus_lock, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FocusLockField = new Field.Builder()
            .Name(nameof(FocusLock))
            .Title("focus_lock")
            .Description("0: unlock focus or keep unlocked, 1: lock focus or keep locked, 3: re-lock focus.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _FocusLock;
        public byte FocusLock { get => _FocusLock; set { _FocusLock = value; } }
        /// <summary>
        /// 0: ignore, 1: shot or start filming.
        /// OriginName: shot, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ShotField = new Field.Builder()
            .Name(nameof(Shot))
            .Title("shot")
            .Description("0: ignore, 1: shot or start filming.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Shot;
        public byte Shot { get => _Shot; set { _Shot = value; } }
        /// <summary>
        /// Command Identity (incremental loop: 0 to 255)//A command sent multiple times will be executed or pooled just once.
        /// OriginName: command_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CommandIdField = new Field.Builder()
            .Name(nameof(CommandId))
            .Title("command_id")
            .Description("Command Identity (incremental loop: 0 to 255)//A command sent multiple times will be executed or pooled just once.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _CommandId;
        public byte CommandId { get => _CommandId; set { _CommandId = value; } }
        /// <summary>
        /// Extra parameters enumeration (0 means ignore).
        /// OriginName: extra_param, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ExtraParamField = new Field.Builder()
            .Name(nameof(ExtraParam))
            .Title("extra_param")
            .Description("Extra parameters enumeration (0 means ignore).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _ExtraParam;
        public byte ExtraParam { get => _ExtraParam; set { _ExtraParam = value; } }
    }
    /// <summary>
    /// Message to configure a camera mount, directional antenna, etc.
    ///  MOUNT_CONFIGURE
    /// </summary>
    public class MountConfigurePacket : MavlinkV2Message<MountConfigurePayload>
    {
        public const int MessageId = 156;
        
        public const byte CrcExtra = 19;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override MountConfigurePayload Payload { get; } = new();

        public override string Name => "MOUNT_CONFIGURE";
    }

    /// <summary>
    ///  MOUNT_CONFIGURE
    /// </summary>
    public class MountConfigurePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 6; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            + 1 // uint8_t mount_mode
            +1 // uint8_t stab_roll
            +1 // uint8_t stab_pitch
            +1 // uint8_t stab_yaw
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            MountMode = (MavMountMode)BinSerialize.ReadByte(ref buffer);
            StabRoll = (byte)BinSerialize.ReadByte(ref buffer);
            StabPitch = (byte)BinSerialize.ReadByte(ref buffer);
            StabYaw = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)MountMode);
            BinSerialize.WriteByte(ref buffer,(byte)StabRoll);
            BinSerialize.WriteByte(ref buffer,(byte)StabPitch);
            BinSerialize.WriteByte(ref buffer,(byte)StabYaw);
            /* PayloadByteSize = 6 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            var tmpMountMode = (byte)MountMode;
            UInt8Type.Accept(visitor,MountModeField, ref tmpMountMode);
            MountMode = (MavMountMode)tmpMountMode;
            UInt8Type.Accept(visitor,StabRollField, ref _StabRoll);    
            UInt8Type.Accept(visitor,StabPitchField, ref _StabPitch);    
            UInt8Type.Accept(visitor,StabYawField, ref _StabYaw);    

        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Mount operating mode.
        /// OriginName: mount_mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MountModeField = new Field.Builder()
            .Name(nameof(MountMode))
            .Title("mount_mode")
            .Description("Mount operating mode.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public MavMountMode _MountMode;
        public MavMountMode MountMode { get => _MountMode; set => _MountMode = value; } 
        /// <summary>
        /// (1 = yes, 0 = no).
        /// OriginName: stab_roll, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StabRollField = new Field.Builder()
            .Name(nameof(StabRoll))
            .Title("stab_roll")
            .Description("(1 = yes, 0 = no).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _StabRoll;
        public byte StabRoll { get => _StabRoll; set { _StabRoll = value; } }
        /// <summary>
        /// (1 = yes, 0 = no).
        /// OriginName: stab_pitch, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StabPitchField = new Field.Builder()
            .Name(nameof(StabPitch))
            .Title("stab_pitch")
            .Description("(1 = yes, 0 = no).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _StabPitch;
        public byte StabPitch { get => _StabPitch; set { _StabPitch = value; } }
        /// <summary>
        /// (1 = yes, 0 = no).
        /// OriginName: stab_yaw, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StabYawField = new Field.Builder()
            .Name(nameof(StabYaw))
            .Title("stab_yaw")
            .Description("(1 = yes, 0 = no).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _StabYaw;
        public byte StabYaw { get => _StabYaw; set { _StabYaw = value; } }
    }
    /// <summary>
    /// Message to control a camera mount, directional antenna, etc.
    ///  MOUNT_CONTROL
    /// </summary>
    public class MountControlPacket : MavlinkV2Message<MountControlPayload>
    {
        public const int MessageId = 157;
        
        public const byte CrcExtra = 21;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override MountControlPayload Payload { get; } = new();

        public override string Name => "MOUNT_CONTROL";
    }

    /// <summary>
    ///  MOUNT_CONTROL
    /// </summary>
    public class MountControlPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 15; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 15; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // int32_t input_a
            +4 // int32_t input_b
            +4 // int32_t input_c
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t save_position
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            InputA = BinSerialize.ReadInt(ref buffer);
            InputB = BinSerialize.ReadInt(ref buffer);
            InputC = BinSerialize.ReadInt(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            SavePosition = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteInt(ref buffer,InputA);
            BinSerialize.WriteInt(ref buffer,InputB);
            BinSerialize.WriteInt(ref buffer,InputC);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)SavePosition);
            /* PayloadByteSize = 15 */;
        }

        public void Visit(IVisitor visitor)
        {
            Int32Type.Accept(visitor,InputAField, ref _InputA);    
            Int32Type.Accept(visitor,InputBField, ref _InputB);    
            Int32Type.Accept(visitor,InputCField, ref _InputC);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,SavePositionField, ref _SavePosition);    

        }

        /// <summary>
        /// Pitch (centi-degrees) or lat (degE7), depending on mount mode.
        /// OriginName: input_a, Units: , IsExtended: false
        /// </summary>
        public static readonly Field InputAField = new Field.Builder()
            .Name(nameof(InputA))
            .Title("input_a")
            .Description("Pitch (centi-degrees) or lat (degE7), depending on mount mode.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int32Type.Default)

            .Build();
        private int _InputA;
        public int InputA { get => _InputA; set { _InputA = value; } }
        /// <summary>
        /// Roll (centi-degrees) or lon (degE7) depending on mount mode.
        /// OriginName: input_b, Units: , IsExtended: false
        /// </summary>
        public static readonly Field InputBField = new Field.Builder()
            .Name(nameof(InputB))
            .Title("input_b")
            .Description("Roll (centi-degrees) or lon (degE7) depending on mount mode.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int32Type.Default)

            .Build();
        private int _InputB;
        public int InputB { get => _InputB; set { _InputB = value; } }
        /// <summary>
        /// Yaw (centi-degrees) or alt (cm) depending on mount mode.
        /// OriginName: input_c, Units: , IsExtended: false
        /// </summary>
        public static readonly Field InputCField = new Field.Builder()
            .Name(nameof(InputC))
            .Title("input_c")
            .Description("Yaw (centi-degrees) or alt (cm) depending on mount mode.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int32Type.Default)

            .Build();
        private int _InputC;
        public int InputC { get => _InputC; set { _InputC = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// If "1" it will save current trimmed position on EEPROM (just valid for NEUTRAL and LANDING).
        /// OriginName: save_position, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SavePositionField = new Field.Builder()
            .Name(nameof(SavePosition))
            .Title("save_position")
            .Description("If \"1\" it will save current trimmed position on EEPROM (just valid for NEUTRAL and LANDING).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _SavePosition;
        public byte SavePosition { get => _SavePosition; set { _SavePosition = value; } }
    }
    /// <summary>
    /// Message with some status from autopilot to GCS about camera or antenna mount.
    ///  MOUNT_STATUS
    /// </summary>
    public class MountStatusPacket : MavlinkV2Message<MountStatusPayload>
    {
        public const int MessageId = 158;
        
        public const byte CrcExtra = 134;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override MountStatusPayload Payload { get; } = new();

        public override string Name => "MOUNT_STATUS";
    }

    /// <summary>
    ///  MOUNT_STATUS
    /// </summary>
    public class MountStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 15; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 15; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // int32_t pointing_a
            +4 // int32_t pointing_b
            +4 // int32_t pointing_c
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            + 1 // uint8_t mount_mode
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            PointingA = BinSerialize.ReadInt(ref buffer);
            PointingB = BinSerialize.ReadInt(ref buffer);
            PointingC = BinSerialize.ReadInt(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            // extended field 'MountMode' can be empty
            if (buffer.IsEmpty) return;
            MountMode = (MavMountMode)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteInt(ref buffer,PointingA);
            BinSerialize.WriteInt(ref buffer,PointingB);
            BinSerialize.WriteInt(ref buffer,PointingC);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)MountMode);
            /* PayloadByteSize = 15 */;
        }

        public void Visit(IVisitor visitor)
        {
            Int32Type.Accept(visitor,PointingAField, ref _PointingA);    
            Int32Type.Accept(visitor,PointingBField, ref _PointingB);    
            Int32Type.Accept(visitor,PointingCField, ref _PointingC);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            var tmpMountMode = (byte)MountMode;
            UInt8Type.Accept(visitor,MountModeField, ref tmpMountMode);
            MountMode = (MavMountMode)tmpMountMode;

        }

        /// <summary>
        /// Pitch.
        /// OriginName: pointing_a, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field PointingAField = new Field.Builder()
            .Name(nameof(PointingA))
            .Title("pointing_a")
            .Description("Pitch.")
            .FormatString(string.Empty)
            .Units(@"cdeg")
            .DataType(Int32Type.Default)

            .Build();
        private int _PointingA;
        public int PointingA { get => _PointingA; set { _PointingA = value; } }
        /// <summary>
        /// Roll.
        /// OriginName: pointing_b, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field PointingBField = new Field.Builder()
            .Name(nameof(PointingB))
            .Title("pointing_b")
            .Description("Roll.")
            .FormatString(string.Empty)
            .Units(@"cdeg")
            .DataType(Int32Type.Default)

            .Build();
        private int _PointingB;
        public int PointingB { get => _PointingB; set { _PointingB = value; } }
        /// <summary>
        /// Yaw.
        /// OriginName: pointing_c, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field PointingCField = new Field.Builder()
            .Name(nameof(PointingC))
            .Title("pointing_c")
            .Description("Yaw.")
            .FormatString(string.Empty)
            .Units(@"cdeg")
            .DataType(Int32Type.Default)

            .Build();
        private int _PointingC;
        public int PointingC { get => _PointingC; set { _PointingC = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Mount operating mode.
        /// OriginName: mount_mode, Units: , IsExtended: true
        /// </summary>
        public static readonly Field MountModeField = new Field.Builder()
            .Name(nameof(MountMode))
            .Title("mount_mode")
            .Description("Mount operating mode.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public MavMountMode _MountMode;
        public MavMountMode MountMode { get => _MountMode; set => _MountMode = value; } 
    }
    /// <summary>
    /// A fence point. Used to set a point when from GCS -> MAV. Also used to return a point from MAV -> GCS.
    ///  FENCE_POINT
    /// </summary>
    public class FencePointPacket : MavlinkV2Message<FencePointPayload>
    {
        public const int MessageId = 160;
        
        public const byte CrcExtra = 78;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override FencePointPayload Payload { get; } = new();

        public override string Name => "FENCE_POINT";
    }

    /// <summary>
    ///  FENCE_POINT
    /// </summary>
    public class FencePointPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 12; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 12; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float lat
            +4 // float lng
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t idx
            +1 // uint8_t count
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Lat = BinSerialize.ReadFloat(ref buffer);
            Lng = BinSerialize.ReadFloat(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Idx = (byte)BinSerialize.ReadByte(ref buffer);
            Count = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Lat);
            BinSerialize.WriteFloat(ref buffer,Lng);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Idx);
            BinSerialize.WriteByte(ref buffer,(byte)Count);
            /* PayloadByteSize = 12 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,LatField, ref _Lat);    
            FloatType.Accept(visitor,LngField, ref _Lng);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,IdxField, ref _Idx);    
            UInt8Type.Accept(visitor,CountField, ref _Count);    

        }

        /// <summary>
        /// Latitude of point.
        /// OriginName: lat, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Latitude of point.")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Lat;
        public float Lat { get => _Lat; set { _Lat = value; } }
        /// <summary>
        /// Longitude of point.
        /// OriginName: lng, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field LngField = new Field.Builder()
            .Name(nameof(Lng))
            .Title("lng")
            .Description("Longitude of point.")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Lng;
        public float Lng { get => _Lng; set { _Lng = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Point index (first point is 1, 0 is for return point).
        /// OriginName: idx, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IdxField = new Field.Builder()
            .Name(nameof(Idx))
            .Title("idx")
            .Description("Point index (first point is 1, 0 is for return point).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Idx;
        public byte Idx { get => _Idx; set { _Idx = value; } }
        /// <summary>
        /// Total number of points (for sanity checking).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Total number of points (for sanity checking).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Count;
        public byte Count { get => _Count; set { _Count = value; } }
    }
    /// <summary>
    /// Request a current fence point from MAV.
    ///  FENCE_FETCH_POINT
    /// </summary>
    public class FenceFetchPointPacket : MavlinkV2Message<FenceFetchPointPayload>
    {
        public const int MessageId = 161;
        
        public const byte CrcExtra = 68;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override FenceFetchPointPayload Payload { get; } = new();

        public override string Name => "FENCE_FETCH_POINT";
    }

    /// <summary>
    ///  FENCE_FETCH_POINT
    /// </summary>
    public class FenceFetchPointPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t idx
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Idx = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Idx);
            /* PayloadByteSize = 3 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,IdxField, ref _Idx);    

        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Point index (first point is 1, 0 is for return point).
        /// OriginName: idx, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IdxField = new Field.Builder()
            .Name(nameof(Idx))
            .Title("idx")
            .Description("Point index (first point is 1, 0 is for return point).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Idx;
        public byte Idx { get => _Idx; set { _Idx = value; } }
    }
    /// <summary>
    /// Status of DCM attitude estimator.
    ///  AHRS
    /// </summary>
    public class AhrsPacket : MavlinkV2Message<AhrsPayload>
    {
        public const int MessageId = 163;
        
        public const byte CrcExtra = 127;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AhrsPayload Payload { get; } = new();

        public override string Name => "AHRS";
    }

    /// <summary>
    ///  AHRS
    /// </summary>
    public class AhrsPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 28; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 28; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float omegaIx
            +4 // float omegaIy
            +4 // float omegaIz
            +4 // float accel_weight
            +4 // float renorm_val
            +4 // float error_rp
            +4 // float error_yaw
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Omegaix = BinSerialize.ReadFloat(ref buffer);
            Omegaiy = BinSerialize.ReadFloat(ref buffer);
            Omegaiz = BinSerialize.ReadFloat(ref buffer);
            AccelWeight = BinSerialize.ReadFloat(ref buffer);
            RenormVal = BinSerialize.ReadFloat(ref buffer);
            ErrorRp = BinSerialize.ReadFloat(ref buffer);
            ErrorYaw = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Omegaix);
            BinSerialize.WriteFloat(ref buffer,Omegaiy);
            BinSerialize.WriteFloat(ref buffer,Omegaiz);
            BinSerialize.WriteFloat(ref buffer,AccelWeight);
            BinSerialize.WriteFloat(ref buffer,RenormVal);
            BinSerialize.WriteFloat(ref buffer,ErrorRp);
            BinSerialize.WriteFloat(ref buffer,ErrorYaw);
            /* PayloadByteSize = 28 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,OmegaixField, ref _Omegaix);    
            FloatType.Accept(visitor,OmegaiyField, ref _Omegaiy);    
            FloatType.Accept(visitor,OmegaizField, ref _Omegaiz);    
            FloatType.Accept(visitor,AccelWeightField, ref _AccelWeight);    
            FloatType.Accept(visitor,RenormValField, ref _RenormVal);    
            FloatType.Accept(visitor,ErrorRpField, ref _ErrorRp);    
            FloatType.Accept(visitor,ErrorYawField, ref _ErrorYaw);    

        }

        /// <summary>
        /// X gyro drift estimate.
        /// OriginName: omegaIx, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field OmegaixField = new Field.Builder()
            .Name(nameof(Omegaix))
            .Title("omegaIx")
            .Description("X gyro drift estimate.")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Omegaix;
        public float Omegaix { get => _Omegaix; set { _Omegaix = value; } }
        /// <summary>
        /// Y gyro drift estimate.
        /// OriginName: omegaIy, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field OmegaiyField = new Field.Builder()
            .Name(nameof(Omegaiy))
            .Title("omegaIy")
            .Description("Y gyro drift estimate.")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Omegaiy;
        public float Omegaiy { get => _Omegaiy; set { _Omegaiy = value; } }
        /// <summary>
        /// Z gyro drift estimate.
        /// OriginName: omegaIz, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field OmegaizField = new Field.Builder()
            .Name(nameof(Omegaiz))
            .Title("omegaIz")
            .Description("Z gyro drift estimate.")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Omegaiz;
        public float Omegaiz { get => _Omegaiz; set { _Omegaiz = value; } }
        /// <summary>
        /// Average accel_weight.
        /// OriginName: accel_weight, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AccelWeightField = new Field.Builder()
            .Name(nameof(AccelWeight))
            .Title("accel_weight")
            .Description("Average accel_weight.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _AccelWeight;
        public float AccelWeight { get => _AccelWeight; set { _AccelWeight = value; } }
        /// <summary>
        /// Average renormalisation value.
        /// OriginName: renorm_val, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RenormValField = new Field.Builder()
            .Name(nameof(RenormVal))
            .Title("renorm_val")
            .Description("Average renormalisation value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _RenormVal;
        public float RenormVal { get => _RenormVal; set { _RenormVal = value; } }
        /// <summary>
        /// Average error_roll_pitch value.
        /// OriginName: error_rp, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ErrorRpField = new Field.Builder()
            .Name(nameof(ErrorRp))
            .Title("error_rp")
            .Description("Average error_roll_pitch value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _ErrorRp;
        public float ErrorRp { get => _ErrorRp; set { _ErrorRp = value; } }
        /// <summary>
        /// Average error_yaw value.
        /// OriginName: error_yaw, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ErrorYawField = new Field.Builder()
            .Name(nameof(ErrorYaw))
            .Title("error_yaw")
            .Description("Average error_yaw value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _ErrorYaw;
        public float ErrorYaw { get => _ErrorYaw; set { _ErrorYaw = value; } }
    }
    /// <summary>
    /// Status of simulation environment, if used.
    ///  SIMSTATE
    /// </summary>
    public class SimstatePacket : MavlinkV2Message<SimstatePayload>
    {
        public const int MessageId = 164;
        
        public const byte CrcExtra = 154;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override SimstatePayload Payload { get; } = new();

        public override string Name => "SIMSTATE";
    }

    /// <summary>
    ///  SIMSTATE
    /// </summary>
    public class SimstatePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 44; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float roll
            +4 // float pitch
            +4 // float yaw
            +4 // float xacc
            +4 // float yacc
            +4 // float zacc
            +4 // float xgyro
            +4 // float ygyro
            +4 // float zgyro
            +4 // int32_t lat
            +4 // int32_t lng
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Roll = BinSerialize.ReadFloat(ref buffer);
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            Xacc = BinSerialize.ReadFloat(ref buffer);
            Yacc = BinSerialize.ReadFloat(ref buffer);
            Zacc = BinSerialize.ReadFloat(ref buffer);
            Xgyro = BinSerialize.ReadFloat(ref buffer);
            Ygyro = BinSerialize.ReadFloat(ref buffer);
            Zgyro = BinSerialize.ReadFloat(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lng = BinSerialize.ReadInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteFloat(ref buffer,Xacc);
            BinSerialize.WriteFloat(ref buffer,Yacc);
            BinSerialize.WriteFloat(ref buffer,Zacc);
            BinSerialize.WriteFloat(ref buffer,Xgyro);
            BinSerialize.WriteFloat(ref buffer,Ygyro);
            BinSerialize.WriteFloat(ref buffer,Zgyro);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lng);
            /* PayloadByteSize = 44 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,RollField, ref _Roll);    
            FloatType.Accept(visitor,PitchField, ref _Pitch);    
            FloatType.Accept(visitor,YawField, ref _Yaw);    
            FloatType.Accept(visitor,XaccField, ref _Xacc);    
            FloatType.Accept(visitor,YaccField, ref _Yacc);    
            FloatType.Accept(visitor,ZaccField, ref _Zacc);    
            FloatType.Accept(visitor,XgyroField, ref _Xgyro);    
            FloatType.Accept(visitor,YgyroField, ref _Ygyro);    
            FloatType.Accept(visitor,ZgyroField, ref _Zgyro);    
            Int32Type.Accept(visitor,LatField, ref _Lat);    
            Int32Type.Accept(visitor,LngField, ref _Lng);    

        }

        /// <summary>
        /// Roll angle.
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Roll;
        public float Roll { get => _Roll; set { _Roll = value; } }
        /// <summary>
        /// Pitch angle.
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch angle.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Pitch;
        public float Pitch { get => _Pitch; set { _Pitch = value; } }
        /// <summary>
        /// Yaw angle.
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw angle.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Yaw;
        public float Yaw { get => _Yaw; set { _Yaw = value; } }
        /// <summary>
        /// X acceleration.
        /// OriginName: xacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public static readonly Field XaccField = new Field.Builder()
            .Name(nameof(Xacc))
            .Title("xacc")
            .Description("X acceleration.")
            .FormatString(string.Empty)
            .Units(@"m/s/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Xacc;
        public float Xacc { get => _Xacc; set { _Xacc = value; } }
        /// <summary>
        /// Y acceleration.
        /// OriginName: yacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public static readonly Field YaccField = new Field.Builder()
            .Name(nameof(Yacc))
            .Title("yacc")
            .Description("Y acceleration.")
            .FormatString(string.Empty)
            .Units(@"m/s/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Yacc;
        public float Yacc { get => _Yacc; set { _Yacc = value; } }
        /// <summary>
        /// Z acceleration.
        /// OriginName: zacc, Units: m/s/s, IsExtended: false
        /// </summary>
        public static readonly Field ZaccField = new Field.Builder()
            .Name(nameof(Zacc))
            .Title("zacc")
            .Description("Z acceleration.")
            .FormatString(string.Empty)
            .Units(@"m/s/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Zacc;
        public float Zacc { get => _Zacc; set { _Zacc = value; } }
        /// <summary>
        /// Angular speed around X axis.
        /// OriginName: xgyro, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field XgyroField = new Field.Builder()
            .Name(nameof(Xgyro))
            .Title("xgyro")
            .Description("Angular speed around X axis.")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Xgyro;
        public float Xgyro { get => _Xgyro; set { _Xgyro = value; } }
        /// <summary>
        /// Angular speed around Y axis.
        /// OriginName: ygyro, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field YgyroField = new Field.Builder()
            .Name(nameof(Ygyro))
            .Title("ygyro")
            .Description("Angular speed around Y axis.")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Ygyro;
        public float Ygyro { get => _Ygyro; set { _Ygyro = value; } }
        /// <summary>
        /// Angular speed around Z axis.
        /// OriginName: zgyro, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field ZgyroField = new Field.Builder()
            .Name(nameof(Zgyro))
            .Title("zgyro")
            .Description("Angular speed around Z axis.")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Zgyro;
        public float Zgyro { get => _Zgyro; set { _Zgyro = value; } }
        /// <summary>
        /// Latitude.
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Latitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lat;
        public int Lat { get => _Lat; set { _Lat = value; } }
        /// <summary>
        /// Longitude.
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LngField = new Field.Builder()
            .Name(nameof(Lng))
            .Title("lng")
            .Description("Longitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lng;
        public int Lng { get => _Lng; set { _Lng = value; } }
    }
    /// <summary>
    /// Status of key hardware.
    ///  HWSTATUS
    /// </summary>
    public class HwstatusPacket : MavlinkV2Message<HwstatusPayload>
    {
        public const int MessageId = 165;
        
        public const byte CrcExtra = 21;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override HwstatusPayload Payload { get; } = new();

        public override string Name => "HWSTATUS";
    }

    /// <summary>
    ///  HWSTATUS
    /// </summary>
    public class HwstatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t Vcc
            +1 // uint8_t I2Cerr
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Vcc = BinSerialize.ReadUShort(ref buffer);
            I2cerr = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,Vcc);
            BinSerialize.WriteByte(ref buffer,(byte)I2cerr);
            /* PayloadByteSize = 3 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,VccField, ref _Vcc);    
            UInt8Type.Accept(visitor,I2cerrField, ref _I2cerr);    

        }

        /// <summary>
        /// Board voltage.
        /// OriginName: Vcc, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field VccField = new Field.Builder()
            .Name(nameof(Vcc))
            .Title("Vcc")
            .Description("Board voltage.")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Vcc;
        public ushort Vcc { get => _Vcc; set { _Vcc = value; } }
        /// <summary>
        /// I2C error count.
        /// OriginName: I2Cerr, Units: , IsExtended: false
        /// </summary>
        public static readonly Field I2cerrField = new Field.Builder()
            .Name(nameof(I2cerr))
            .Title("I2Cerr")
            .Description("I2C error count.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _I2cerr;
        public byte I2cerr { get => _I2cerr; set { _I2cerr = value; } }
    }
    /// <summary>
    /// Status generated by radio.
    ///  RADIO
    /// </summary>
    public class RadioPacket : MavlinkV2Message<RadioPayload>
    {
        public const int MessageId = 166;
        
        public const byte CrcExtra = 21;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override RadioPayload Payload { get; } = new();

        public override string Name => "RADIO";
    }

    /// <summary>
    ///  RADIO
    /// </summary>
    public class RadioPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 9; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 9; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t rxerrors
            +2 // uint16_t fixed
            +1 // uint8_t rssi
            +1 // uint8_t remrssi
            +1 // uint8_t txbuf
            +1 // uint8_t noise
            +1 // uint8_t remnoise
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Rxerrors = BinSerialize.ReadUShort(ref buffer);
            Fixed = BinSerialize.ReadUShort(ref buffer);
            Rssi = (byte)BinSerialize.ReadByte(ref buffer);
            Remrssi = (byte)BinSerialize.ReadByte(ref buffer);
            Txbuf = (byte)BinSerialize.ReadByte(ref buffer);
            Noise = (byte)BinSerialize.ReadByte(ref buffer);
            Remnoise = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,Rxerrors);
            BinSerialize.WriteUShort(ref buffer,Fixed);
            BinSerialize.WriteByte(ref buffer,(byte)Rssi);
            BinSerialize.WriteByte(ref buffer,(byte)Remrssi);
            BinSerialize.WriteByte(ref buffer,(byte)Txbuf);
            BinSerialize.WriteByte(ref buffer,(byte)Noise);
            BinSerialize.WriteByte(ref buffer,(byte)Remnoise);
            /* PayloadByteSize = 9 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,RxerrorsField, ref _Rxerrors);    
            UInt16Type.Accept(visitor,FixedField, ref _Fixed);    
            UInt8Type.Accept(visitor,RssiField, ref _Rssi);    
            UInt8Type.Accept(visitor,RemrssiField, ref _Remrssi);    
            UInt8Type.Accept(visitor,TxbufField, ref _Txbuf);    
            UInt8Type.Accept(visitor,NoiseField, ref _Noise);    
            UInt8Type.Accept(visitor,RemnoiseField, ref _Remnoise);    

        }

        /// <summary>
        /// Receive errors.
        /// OriginName: rxerrors, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RxerrorsField = new Field.Builder()
            .Name(nameof(Rxerrors))
            .Title("rxerrors")
            .Description("Receive errors.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Rxerrors;
        public ushort Rxerrors { get => _Rxerrors; set { _Rxerrors = value; } }
        /// <summary>
        /// Count of error corrected packets.
        /// OriginName: fixed, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FixedField = new Field.Builder()
            .Name(nameof(Fixed))
            .Title("fixed")
            .Description("Count of error corrected packets.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Fixed;
        public ushort Fixed { get => _Fixed; set { _Fixed = value; } }
        /// <summary>
        /// Local signal strength.
        /// OriginName: rssi, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RssiField = new Field.Builder()
            .Name(nameof(Rssi))
            .Title("rssi")
            .Description("Local signal strength.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Rssi;
        public byte Rssi { get => _Rssi; set { _Rssi = value; } }
        /// <summary>
        /// Remote signal strength.
        /// OriginName: remrssi, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RemrssiField = new Field.Builder()
            .Name(nameof(Remrssi))
            .Title("remrssi")
            .Description("Remote signal strength.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Remrssi;
        public byte Remrssi { get => _Remrssi; set { _Remrssi = value; } }
        /// <summary>
        /// How full the tx buffer is.
        /// OriginName: txbuf, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field TxbufField = new Field.Builder()
            .Name(nameof(Txbuf))
            .Title("txbuf")
            .Description("How full the tx buffer is.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Txbuf;
        public byte Txbuf { get => _Txbuf; set { _Txbuf = value; } }
        /// <summary>
        /// Background noise level.
        /// OriginName: noise, Units: , IsExtended: false
        /// </summary>
        public static readonly Field NoiseField = new Field.Builder()
            .Name(nameof(Noise))
            .Title("noise")
            .Description("Background noise level.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Noise;
        public byte Noise { get => _Noise; set { _Noise = value; } }
        /// <summary>
        /// Remote background noise level.
        /// OriginName: remnoise, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RemnoiseField = new Field.Builder()
            .Name(nameof(Remnoise))
            .Title("remnoise")
            .Description("Remote background noise level.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Remnoise;
        public byte Remnoise { get => _Remnoise; set { _Remnoise = value; } }
    }
    /// <summary>
    /// Status of AP_Limits. Sent in extended status stream when AP_Limits is enabled.
    ///  LIMITS_STATUS
    /// </summary>
    public class LimitsStatusPacket : MavlinkV2Message<LimitsStatusPayload>
    {
        public const int MessageId = 167;
        
        public const byte CrcExtra = 144;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override LimitsStatusPayload Payload { get; } = new();

        public override string Name => "LIMITS_STATUS";
    }

    /// <summary>
    ///  LIMITS_STATUS
    /// </summary>
    public class LimitsStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 22; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 22; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t last_trigger
            +4 // uint32_t last_action
            +4 // uint32_t last_recovery
            +4 // uint32_t last_clear
            +2 // uint16_t breach_count
            + 1 // uint8_t limits_state
            + 1 // uint8_t mods_enabled
            + 1 // uint8_t mods_required
            + 1 // uint8_t mods_triggered
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            LastTrigger = BinSerialize.ReadUInt(ref buffer);
            LastAction = BinSerialize.ReadUInt(ref buffer);
            LastRecovery = BinSerialize.ReadUInt(ref buffer);
            LastClear = BinSerialize.ReadUInt(ref buffer);
            BreachCount = BinSerialize.ReadUShort(ref buffer);
            LimitsState = (LimitsState)BinSerialize.ReadByte(ref buffer);
            ModsEnabled = (LimitModule)BinSerialize.ReadByte(ref buffer);
            ModsRequired = (LimitModule)BinSerialize.ReadByte(ref buffer);
            ModsTriggered = (LimitModule)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,LastTrigger);
            BinSerialize.WriteUInt(ref buffer,LastAction);
            BinSerialize.WriteUInt(ref buffer,LastRecovery);
            BinSerialize.WriteUInt(ref buffer,LastClear);
            BinSerialize.WriteUShort(ref buffer,BreachCount);
            BinSerialize.WriteByte(ref buffer,(byte)LimitsState);
            BinSerialize.WriteByte(ref buffer,(byte)ModsEnabled);
            BinSerialize.WriteByte(ref buffer,(byte)ModsRequired);
            BinSerialize.WriteByte(ref buffer,(byte)ModsTriggered);
            /* PayloadByteSize = 22 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,LastTriggerField, ref _LastTrigger);    
            UInt32Type.Accept(visitor,LastActionField, ref _LastAction);    
            UInt32Type.Accept(visitor,LastRecoveryField, ref _LastRecovery);    
            UInt32Type.Accept(visitor,LastClearField, ref _LastClear);    
            UInt16Type.Accept(visitor,BreachCountField, ref _BreachCount);    
            var tmpLimitsState = (byte)LimitsState;
            UInt8Type.Accept(visitor,LimitsStateField, ref tmpLimitsState);
            LimitsState = (LimitsState)tmpLimitsState;
            var tmpModsEnabled = (byte)ModsEnabled;
            UInt8Type.Accept(visitor,ModsEnabledField, ref tmpModsEnabled);
            ModsEnabled = (LimitModule)tmpModsEnabled;
            var tmpModsRequired = (byte)ModsRequired;
            UInt8Type.Accept(visitor,ModsRequiredField, ref tmpModsRequired);
            ModsRequired = (LimitModule)tmpModsRequired;
            var tmpModsTriggered = (byte)ModsTriggered;
            UInt8Type.Accept(visitor,ModsTriggeredField, ref tmpModsTriggered);
            ModsTriggered = (LimitModule)tmpModsTriggered;

        }

        /// <summary>
        /// Time (since boot) of last breach.
        /// OriginName: last_trigger, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field LastTriggerField = new Field.Builder()
            .Name(nameof(LastTrigger))
            .Title("last_trigger")
            .Description("Time (since boot) of last breach.")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _LastTrigger;
        public uint LastTrigger { get => _LastTrigger; set { _LastTrigger = value; } }
        /// <summary>
        /// Time (since boot) of last recovery action.
        /// OriginName: last_action, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field LastActionField = new Field.Builder()
            .Name(nameof(LastAction))
            .Title("last_action")
            .Description("Time (since boot) of last recovery action.")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _LastAction;
        public uint LastAction { get => _LastAction; set { _LastAction = value; } }
        /// <summary>
        /// Time (since boot) of last successful recovery.
        /// OriginName: last_recovery, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field LastRecoveryField = new Field.Builder()
            .Name(nameof(LastRecovery))
            .Title("last_recovery")
            .Description("Time (since boot) of last successful recovery.")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _LastRecovery;
        public uint LastRecovery { get => _LastRecovery; set { _LastRecovery = value; } }
        /// <summary>
        /// Time (since boot) of last all-clear.
        /// OriginName: last_clear, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field LastClearField = new Field.Builder()
            .Name(nameof(LastClear))
            .Title("last_clear")
            .Description("Time (since boot) of last all-clear.")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _LastClear;
        public uint LastClear { get => _LastClear; set { _LastClear = value; } }
        /// <summary>
        /// Number of fence breaches.
        /// OriginName: breach_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BreachCountField = new Field.Builder()
            .Name(nameof(BreachCount))
            .Title("breach_count")
            .Description("Number of fence breaches.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _BreachCount;
        public ushort BreachCount { get => _BreachCount; set { _BreachCount = value; } }
        /// <summary>
        /// State of AP_Limits.
        /// OriginName: limits_state, Units: , IsExtended: false
        /// </summary>
        public static readonly Field LimitsStateField = new Field.Builder()
            .Name(nameof(LimitsState))
            .Title("limits_state")
            .Description("State of AP_Limits.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public LimitsState _LimitsState;
        public LimitsState LimitsState { get => _LimitsState; set => _LimitsState = value; } 
        /// <summary>
        /// AP_Limit_Module bitfield of enabled modules.
        /// OriginName: mods_enabled, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ModsEnabledField = new Field.Builder()
            .Name(nameof(ModsEnabled))
            .Title("bitmask")
            .Description("AP_Limit_Module bitfield of enabled modules.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public LimitModule _ModsEnabled;
        public LimitModule ModsEnabled { get => _ModsEnabled; set => _ModsEnabled = value; } 
        /// <summary>
        /// AP_Limit_Module bitfield of required modules.
        /// OriginName: mods_required, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ModsRequiredField = new Field.Builder()
            .Name(nameof(ModsRequired))
            .Title("bitmask")
            .Description("AP_Limit_Module bitfield of required modules.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public LimitModule _ModsRequired;
        public LimitModule ModsRequired { get => _ModsRequired; set => _ModsRequired = value; } 
        /// <summary>
        /// AP_Limit_Module bitfield of triggered modules.
        /// OriginName: mods_triggered, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ModsTriggeredField = new Field.Builder()
            .Name(nameof(ModsTriggered))
            .Title("bitmask")
            .Description("AP_Limit_Module bitfield of triggered modules.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public LimitModule _ModsTriggered;
        public LimitModule ModsTriggered { get => _ModsTriggered; set => _ModsTriggered = value; } 
    }
    /// <summary>
    /// Wind estimation.
    ///  WIND
    /// </summary>
    public class WindPacket : MavlinkV2Message<WindPayload>
    {
        public const int MessageId = 168;
        
        public const byte CrcExtra = 1;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override WindPayload Payload { get; } = new();

        public override string Name => "WIND";
    }

    /// <summary>
    ///  WIND
    /// </summary>
    public class WindPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 12; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 12; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float direction
            +4 // float speed
            +4 // float speed_z
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Direction = BinSerialize.ReadFloat(ref buffer);
            Speed = BinSerialize.ReadFloat(ref buffer);
            SpeedZ = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Direction);
            BinSerialize.WriteFloat(ref buffer,Speed);
            BinSerialize.WriteFloat(ref buffer,SpeedZ);
            /* PayloadByteSize = 12 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,DirectionField, ref _Direction);    
            FloatType.Accept(visitor,SpeedField, ref _Speed);    
            FloatType.Accept(visitor,SpeedZField, ref _SpeedZ);    

        }

        /// <summary>
        /// Wind direction (that wind is coming from).
        /// OriginName: direction, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field DirectionField = new Field.Builder()
            .Name(nameof(Direction))
            .Title("direction")
            .Description("Wind direction (that wind is coming from).")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Direction;
        public float Direction { get => _Direction; set { _Direction = value; } }
        /// <summary>
        /// Wind speed in ground plane.
        /// OriginName: speed, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field SpeedField = new Field.Builder()
            .Name(nameof(Speed))
            .Title("speed")
            .Description("Wind speed in ground plane.")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Speed;
        public float Speed { get => _Speed; set { _Speed = value; } }
        /// <summary>
        /// Vertical wind speed.
        /// OriginName: speed_z, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field SpeedZField = new Field.Builder()
            .Name(nameof(SpeedZ))
            .Title("speed_z")
            .Description("Vertical wind speed.")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _SpeedZ;
        public float SpeedZ { get => _SpeedZ; set { _SpeedZ = value; } }
    }
    /// <summary>
    /// Data packet, size 16.
    ///  DATA16
    /// </summary>
    public class Data16Packet : MavlinkV2Message<Data16Payload>
    {
        public const int MessageId = 169;
        
        public const byte CrcExtra = 234;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Data16Payload Payload { get; } = new();

        public override string Name => "DATA16";
    }

    /// <summary>
    ///  DATA16
    /// </summary>
    public class Data16Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 18; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 18; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t type
            +1 // uint8_t len
            +Data.Length // uint8_t[16] data
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Type = (byte)BinSerialize.ReadByte(ref buffer);
            Len = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/18 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Type);
            BinSerialize.WriteByte(ref buffer,(byte)Len);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            /* PayloadByteSize = 18 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TypeField, ref _Type);    
            UInt8Type.Accept(visitor,LenField, ref _Len);    
            ArrayType.Accept(visitor,DataField, 16,
                (index,v) => UInt8Type.Accept(v, DataField, ref Data[index]));    

        }

        /// <summary>
        /// Data type.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TypeField = new Field.Builder()
            .Name(nameof(Type))
            .Title("type")
            .Description("Data type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Type;
        public byte Type { get => _Type; set { _Type = value; } }
        /// <summary>
        /// Data length.
        /// OriginName: len, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field LenField = new Field.Builder()
            .Name(nameof(Len))
            .Title("len")
            .Description("Data length.")
            .FormatString(string.Empty)
            .Units(@"bytes")
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Len;
        public byte Len { get => _Len; set { _Len = value; } }
        /// <summary>
        /// Raw data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Raw data.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,16))

            .Build();
        public const int DataMaxItemsCount = 16;
        public byte[] Data { get; } = new byte[16];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 16;
    }
    /// <summary>
    /// Data packet, size 32.
    ///  DATA32
    /// </summary>
    public class Data32Packet : MavlinkV2Message<Data32Payload>
    {
        public const int MessageId = 170;
        
        public const byte CrcExtra = 73;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Data32Payload Payload { get; } = new();

        public override string Name => "DATA32";
    }

    /// <summary>
    ///  DATA32
    /// </summary>
    public class Data32Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 34; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 34; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t type
            +1 // uint8_t len
            +Data.Length // uint8_t[32] data
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Type = (byte)BinSerialize.ReadByte(ref buffer);
            Len = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/34 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Type);
            BinSerialize.WriteByte(ref buffer,(byte)Len);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            /* PayloadByteSize = 34 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TypeField, ref _Type);    
            UInt8Type.Accept(visitor,LenField, ref _Len);    
            ArrayType.Accept(visitor,DataField, 32,
                (index,v) => UInt8Type.Accept(v, DataField, ref Data[index]));    

        }

        /// <summary>
        /// Data type.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TypeField = new Field.Builder()
            .Name(nameof(Type))
            .Title("type")
            .Description("Data type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Type;
        public byte Type { get => _Type; set { _Type = value; } }
        /// <summary>
        /// Data length.
        /// OriginName: len, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field LenField = new Field.Builder()
            .Name(nameof(Len))
            .Title("len")
            .Description("Data length.")
            .FormatString(string.Empty)
            .Units(@"bytes")
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Len;
        public byte Len { get => _Len; set { _Len = value; } }
        /// <summary>
        /// Raw data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Raw data.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,32))

            .Build();
        public const int DataMaxItemsCount = 32;
        public byte[] Data { get; } = new byte[32];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 32;
    }
    /// <summary>
    /// Data packet, size 64.
    ///  DATA64
    /// </summary>
    public class Data64Packet : MavlinkV2Message<Data64Payload>
    {
        public const int MessageId = 171;
        
        public const byte CrcExtra = 181;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Data64Payload Payload { get; } = new();

        public override string Name => "DATA64";
    }

    /// <summary>
    ///  DATA64
    /// </summary>
    public class Data64Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 66; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 66; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t type
            +1 // uint8_t len
            +Data.Length // uint8_t[64] data
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Type = (byte)BinSerialize.ReadByte(ref buffer);
            Len = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/64 - Math.Max(0,((/*PayloadByteSize*/66 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Type);
            BinSerialize.WriteByte(ref buffer,(byte)Len);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            /* PayloadByteSize = 66 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TypeField, ref _Type);    
            UInt8Type.Accept(visitor,LenField, ref _Len);    
            ArrayType.Accept(visitor,DataField, 64,
                (index,v) => UInt8Type.Accept(v, DataField, ref Data[index]));    

        }

        /// <summary>
        /// Data type.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TypeField = new Field.Builder()
            .Name(nameof(Type))
            .Title("type")
            .Description("Data type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Type;
        public byte Type { get => _Type; set { _Type = value; } }
        /// <summary>
        /// Data length.
        /// OriginName: len, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field LenField = new Field.Builder()
            .Name(nameof(Len))
            .Title("len")
            .Description("Data length.")
            .FormatString(string.Empty)
            .Units(@"bytes")
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Len;
        public byte Len { get => _Len; set { _Len = value; } }
        /// <summary>
        /// Raw data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Raw data.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,64))

            .Build();
        public const int DataMaxItemsCount = 64;
        public byte[] Data { get; } = new byte[64];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 64;
    }
    /// <summary>
    /// Data packet, size 96.
    ///  DATA96
    /// </summary>
    public class Data96Packet : MavlinkV2Message<Data96Payload>
    {
        public const int MessageId = 172;
        
        public const byte CrcExtra = 22;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Data96Payload Payload { get; } = new();

        public override string Name => "DATA96";
    }

    /// <summary>
    ///  DATA96
    /// </summary>
    public class Data96Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 98; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 98; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t type
            +1 // uint8_t len
            +Data.Length // uint8_t[96] data
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Type = (byte)BinSerialize.ReadByte(ref buffer);
            Len = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/96 - Math.Max(0,((/*PayloadByteSize*/98 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Type);
            BinSerialize.WriteByte(ref buffer,(byte)Len);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            /* PayloadByteSize = 98 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TypeField, ref _Type);    
            UInt8Type.Accept(visitor,LenField, ref _Len);    
            ArrayType.Accept(visitor,DataField, 96,
                (index,v) => UInt8Type.Accept(v, DataField, ref Data[index]));    

        }

        /// <summary>
        /// Data type.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TypeField = new Field.Builder()
            .Name(nameof(Type))
            .Title("type")
            .Description("Data type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Type;
        public byte Type { get => _Type; set { _Type = value; } }
        /// <summary>
        /// Data length.
        /// OriginName: len, Units: bytes, IsExtended: false
        /// </summary>
        public static readonly Field LenField = new Field.Builder()
            .Name(nameof(Len))
            .Title("len")
            .Description("Data length.")
            .FormatString(string.Empty)
            .Units(@"bytes")
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Len;
        public byte Len { get => _Len; set { _Len = value; } }
        /// <summary>
        /// Raw data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Raw data.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,96))

            .Build();
        public const int DataMaxItemsCount = 96;
        public byte[] Data { get; } = new byte[96];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 96;
    }
    /// <summary>
    /// Rangefinder reporting.
    ///  RANGEFINDER
    /// </summary>
    public class RangefinderPacket : MavlinkV2Message<RangefinderPayload>
    {
        public const int MessageId = 173;
        
        public const byte CrcExtra = 83;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override RangefinderPayload Payload { get; } = new();

        public override string Name => "RANGEFINDER";
    }

    /// <summary>
    ///  RANGEFINDER
    /// </summary>
    public class RangefinderPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float distance
            +4 // float voltage
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Distance = BinSerialize.ReadFloat(ref buffer);
            Voltage = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Distance);
            BinSerialize.WriteFloat(ref buffer,Voltage);
            /* PayloadByteSize = 8 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,DistanceField, ref _Distance);    
            FloatType.Accept(visitor,VoltageField, ref _Voltage);    

        }

        /// <summary>
        /// Distance.
        /// OriginName: distance, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field DistanceField = new Field.Builder()
            .Name(nameof(Distance))
            .Title("distance")
            .Description("Distance.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Distance;
        public float Distance { get => _Distance; set { _Distance = value; } }
        /// <summary>
        /// Raw voltage if available, zero otherwise.
        /// OriginName: voltage, Units: V, IsExtended: false
        /// </summary>
        public static readonly Field VoltageField = new Field.Builder()
            .Name(nameof(Voltage))
            .Title("voltage")
            .Description("Raw voltage if available, zero otherwise.")
            .FormatString(string.Empty)
            .Units(@"V")
            .DataType(FloatType.Default)

            .Build();
        private float _Voltage;
        public float Voltage { get => _Voltage; set { _Voltage = value; } }
    }
    /// <summary>
    /// Airspeed auto-calibration.
    ///  AIRSPEED_AUTOCAL
    /// </summary>
    public class AirspeedAutocalPacket : MavlinkV2Message<AirspeedAutocalPayload>
    {
        public const int MessageId = 174;
        
        public const byte CrcExtra = 167;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AirspeedAutocalPayload Payload { get; } = new();

        public override string Name => "AIRSPEED_AUTOCAL";
    }

    /// <summary>
    ///  AIRSPEED_AUTOCAL
    /// </summary>
    public class AirspeedAutocalPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 48; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 48; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float vx
            +4 // float vy
            +4 // float vz
            +4 // float diff_pressure
            +4 // float EAS2TAS
            +4 // float ratio
            +4 // float state_x
            +4 // float state_y
            +4 // float state_z
            +4 // float Pax
            +4 // float Pby
            +4 // float Pcz
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Vx = BinSerialize.ReadFloat(ref buffer);
            Vy = BinSerialize.ReadFloat(ref buffer);
            Vz = BinSerialize.ReadFloat(ref buffer);
            DiffPressure = BinSerialize.ReadFloat(ref buffer);
            Eas2tas = BinSerialize.ReadFloat(ref buffer);
            Ratio = BinSerialize.ReadFloat(ref buffer);
            StateX = BinSerialize.ReadFloat(ref buffer);
            StateY = BinSerialize.ReadFloat(ref buffer);
            StateZ = BinSerialize.ReadFloat(ref buffer);
            Pax = BinSerialize.ReadFloat(ref buffer);
            Pby = BinSerialize.ReadFloat(ref buffer);
            Pcz = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Vx);
            BinSerialize.WriteFloat(ref buffer,Vy);
            BinSerialize.WriteFloat(ref buffer,Vz);
            BinSerialize.WriteFloat(ref buffer,DiffPressure);
            BinSerialize.WriteFloat(ref buffer,Eas2tas);
            BinSerialize.WriteFloat(ref buffer,Ratio);
            BinSerialize.WriteFloat(ref buffer,StateX);
            BinSerialize.WriteFloat(ref buffer,StateY);
            BinSerialize.WriteFloat(ref buffer,StateZ);
            BinSerialize.WriteFloat(ref buffer,Pax);
            BinSerialize.WriteFloat(ref buffer,Pby);
            BinSerialize.WriteFloat(ref buffer,Pcz);
            /* PayloadByteSize = 48 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,VxField, ref _Vx);    
            FloatType.Accept(visitor,VyField, ref _Vy);    
            FloatType.Accept(visitor,VzField, ref _Vz);    
            FloatType.Accept(visitor,DiffPressureField, ref _DiffPressure);    
            FloatType.Accept(visitor,Eas2tasField, ref _Eas2tas);    
            FloatType.Accept(visitor,RatioField, ref _Ratio);    
            FloatType.Accept(visitor,StateXField, ref _StateX);    
            FloatType.Accept(visitor,StateYField, ref _StateY);    
            FloatType.Accept(visitor,StateZField, ref _StateZ);    
            FloatType.Accept(visitor,PaxField, ref _Pax);    
            FloatType.Accept(visitor,PbyField, ref _Pby);    
            FloatType.Accept(visitor,PczField, ref _Pcz);    

        }

        /// <summary>
        /// GPS velocity north.
        /// OriginName: vx, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field VxField = new Field.Builder()
            .Name(nameof(Vx))
            .Title("vx")
            .Description("GPS velocity north.")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Vx;
        public float Vx { get => _Vx; set { _Vx = value; } }
        /// <summary>
        /// GPS velocity east.
        /// OriginName: vy, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field VyField = new Field.Builder()
            .Name(nameof(Vy))
            .Title("vy")
            .Description("GPS velocity east.")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Vy;
        public float Vy { get => _Vy; set { _Vy = value; } }
        /// <summary>
        /// GPS velocity down.
        /// OriginName: vz, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field VzField = new Field.Builder()
            .Name(nameof(Vz))
            .Title("vz")
            .Description("GPS velocity down.")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Vz;
        public float Vz { get => _Vz; set { _Vz = value; } }
        /// <summary>
        /// Differential pressure.
        /// OriginName: diff_pressure, Units: Pa, IsExtended: false
        /// </summary>
        public static readonly Field DiffPressureField = new Field.Builder()
            .Name(nameof(DiffPressure))
            .Title("diff_pressure")
            .Description("Differential pressure.")
            .FormatString(string.Empty)
            .Units(@"Pa")
            .DataType(FloatType.Default)

            .Build();
        private float _DiffPressure;
        public float DiffPressure { get => _DiffPressure; set { _DiffPressure = value; } }
        /// <summary>
        /// Estimated to true airspeed ratio.
        /// OriginName: EAS2TAS, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Eas2tasField = new Field.Builder()
            .Name(nameof(Eas2tas))
            .Title("EAS2TAS")
            .Description("Estimated to true airspeed ratio.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Eas2tas;
        public float Eas2tas { get => _Eas2tas; set { _Eas2tas = value; } }
        /// <summary>
        /// Airspeed ratio.
        /// OriginName: ratio, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RatioField = new Field.Builder()
            .Name(nameof(Ratio))
            .Title("ratio")
            .Description("Airspeed ratio.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Ratio;
        public float Ratio { get => _Ratio; set { _Ratio = value; } }
        /// <summary>
        /// EKF state x.
        /// OriginName: state_x, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StateXField = new Field.Builder()
            .Name(nameof(StateX))
            .Title("state_x")
            .Description("EKF state x.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _StateX;
        public float StateX { get => _StateX; set { _StateX = value; } }
        /// <summary>
        /// EKF state y.
        /// OriginName: state_y, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StateYField = new Field.Builder()
            .Name(nameof(StateY))
            .Title("state_y")
            .Description("EKF state y.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _StateY;
        public float StateY { get => _StateY; set { _StateY = value; } }
        /// <summary>
        /// EKF state z.
        /// OriginName: state_z, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StateZField = new Field.Builder()
            .Name(nameof(StateZ))
            .Title("state_z")
            .Description("EKF state z.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _StateZ;
        public float StateZ { get => _StateZ; set { _StateZ = value; } }
        /// <summary>
        /// EKF Pax.
        /// OriginName: Pax, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PaxField = new Field.Builder()
            .Name(nameof(Pax))
            .Title("Pax")
            .Description("EKF Pax.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Pax;
        public float Pax { get => _Pax; set { _Pax = value; } }
        /// <summary>
        /// EKF Pby.
        /// OriginName: Pby, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PbyField = new Field.Builder()
            .Name(nameof(Pby))
            .Title("Pby")
            .Description("EKF Pby.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Pby;
        public float Pby { get => _Pby; set { _Pby = value; } }
        /// <summary>
        /// EKF Pcz.
        /// OriginName: Pcz, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PczField = new Field.Builder()
            .Name(nameof(Pcz))
            .Title("Pcz")
            .Description("EKF Pcz.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Pcz;
        public float Pcz { get => _Pcz; set { _Pcz = value; } }
    }
    /// <summary>
    /// A rally point. Used to set a point when from GCS -> MAV. Also used to return a point from MAV -> GCS.
    ///  RALLY_POINT
    /// </summary>
    public class RallyPointPacket : MavlinkV2Message<RallyPointPayload>
    {
        public const int MessageId = 175;
        
        public const byte CrcExtra = 138;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override RallyPointPayload Payload { get; } = new();

        public override string Name => "RALLY_POINT";
    }

    /// <summary>
    ///  RALLY_POINT
    /// </summary>
    public class RallyPointPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 19; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 19; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // int32_t lat
            +4 // int32_t lng
            +2 // int16_t alt
            +2 // int16_t break_alt
            +2 // uint16_t land_dir
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t idx
            +1 // uint8_t count
            + 1 // uint8_t flags
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Lat = BinSerialize.ReadInt(ref buffer);
            Lng = BinSerialize.ReadInt(ref buffer);
            Alt = BinSerialize.ReadShort(ref buffer);
            BreakAlt = BinSerialize.ReadShort(ref buffer);
            LandDir = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Idx = (byte)BinSerialize.ReadByte(ref buffer);
            Count = (byte)BinSerialize.ReadByte(ref buffer);
            Flags = (RallyFlags)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lng);
            BinSerialize.WriteShort(ref buffer,Alt);
            BinSerialize.WriteShort(ref buffer,BreakAlt);
            BinSerialize.WriteUShort(ref buffer,LandDir);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Idx);
            BinSerialize.WriteByte(ref buffer,(byte)Count);
            BinSerialize.WriteByte(ref buffer,(byte)Flags);
            /* PayloadByteSize = 19 */;
        }

        public void Visit(IVisitor visitor)
        {
            Int32Type.Accept(visitor,LatField, ref _Lat);    
            Int32Type.Accept(visitor,LngField, ref _Lng);    
            Int16Type.Accept(visitor,AltField, ref _Alt);
            Int16Type.Accept(visitor,BreakAltField, ref _BreakAlt);
            UInt16Type.Accept(visitor,LandDirField, ref _LandDir);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,IdxField, ref _Idx);    
            UInt8Type.Accept(visitor,CountField, ref _Count);    
            var tmpFlags = (byte)Flags;
            UInt8Type.Accept(visitor,FlagsField, ref tmpFlags);
            Flags = (RallyFlags)tmpFlags;

        }

        /// <summary>
        /// Latitude of point.
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Latitude of point.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lat;
        public int Lat { get => _Lat; set { _Lat = value; } }
        /// <summary>
        /// Longitude of point.
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LngField = new Field.Builder()
            .Name(nameof(Lng))
            .Title("lng")
            .Description("Longitude of point.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lng;
        public int Lng { get => _Lng; set { _Lng = value; } }
        /// <summary>
        /// Transit / loiter altitude relative to home.
        /// OriginName: alt, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field AltField = new Field.Builder()
            .Name(nameof(Alt))
            .Title("alt")
            .Description("Transit / loiter altitude relative to home.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(Int16Type.Default)

            .Build();
        private short _Alt;
        public short Alt { get => _Alt; set { _Alt = value; } }
        /// <summary>
        /// Break altitude relative to home.
        /// OriginName: break_alt, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field BreakAltField = new Field.Builder()
            .Name(nameof(BreakAlt))
            .Title("break_alt")
            .Description("Break altitude relative to home.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(Int16Type.Default)

            .Build();
        private short _BreakAlt;
        public short BreakAlt { get => _BreakAlt; set { _BreakAlt = value; } }
        /// <summary>
        /// Heading to aim for when landing.
        /// OriginName: land_dir, Units: cdeg, IsExtended: false
        /// </summary>
        public static readonly Field LandDirField = new Field.Builder()
            .Name(nameof(LandDir))
            .Title("land_dir")
            .Description("Heading to aim for when landing.")
            .FormatString(string.Empty)
            .Units(@"cdeg")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _LandDir;
        public ushort LandDir { get => _LandDir; set { _LandDir = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Point index (first point is 0).
        /// OriginName: idx, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IdxField = new Field.Builder()
            .Name(nameof(Idx))
            .Title("idx")
            .Description("Point index (first point is 0).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Idx;
        public byte Idx { get => _Idx; set { _Idx = value; } }
        /// <summary>
        /// Total number of points (for sanity checking).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Total number of points (for sanity checking).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Count;
        public byte Count { get => _Count; set { _Count = value; } }
        /// <summary>
        /// Configuration flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("bitmask")
            .Description("Configuration flags.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public RallyFlags _Flags;
        public RallyFlags Flags { get => _Flags; set => _Flags = value; } 
    }
    /// <summary>
    /// Request a current rally point from MAV. MAV should respond with a RALLY_POINT message. MAV should not respond if the request is invalid.
    ///  RALLY_FETCH_POINT
    /// </summary>
    public class RallyFetchPointPacket : MavlinkV2Message<RallyFetchPointPayload>
    {
        public const int MessageId = 176;
        
        public const byte CrcExtra = 234;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override RallyFetchPointPayload Payload { get; } = new();

        public override string Name => "RALLY_FETCH_POINT";
    }

    /// <summary>
    ///  RALLY_FETCH_POINT
    /// </summary>
    public class RallyFetchPointPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t idx
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Idx = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Idx);
            /* PayloadByteSize = 3 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,IdxField, ref _Idx);    

        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Point index (first point is 0).
        /// OriginName: idx, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IdxField = new Field.Builder()
            .Name(nameof(Idx))
            .Title("idx")
            .Description("Point index (first point is 0).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Idx;
        public byte Idx { get => _Idx; set { _Idx = value; } }
    }
    /// <summary>
    /// Status of compassmot calibration.
    ///  COMPASSMOT_STATUS
    /// </summary>
    public class CompassmotStatusPacket : MavlinkV2Message<CompassmotStatusPayload>
    {
        public const int MessageId = 177;
        
        public const byte CrcExtra = 240;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override CompassmotStatusPayload Payload { get; } = new();

        public override string Name => "COMPASSMOT_STATUS";
    }

    /// <summary>
    ///  COMPASSMOT_STATUS
    /// </summary>
    public class CompassmotStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 20; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 20; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float current
            +4 // float CompensationX
            +4 // float CompensationY
            +4 // float CompensationZ
            +2 // uint16_t throttle
            +2 // uint16_t interference
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Current = BinSerialize.ReadFloat(ref buffer);
            Compensationx = BinSerialize.ReadFloat(ref buffer);
            Compensationy = BinSerialize.ReadFloat(ref buffer);
            Compensationz = BinSerialize.ReadFloat(ref buffer);
            Throttle = BinSerialize.ReadUShort(ref buffer);
            Interference = BinSerialize.ReadUShort(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Current);
            BinSerialize.WriteFloat(ref buffer,Compensationx);
            BinSerialize.WriteFloat(ref buffer,Compensationy);
            BinSerialize.WriteFloat(ref buffer,Compensationz);
            BinSerialize.WriteUShort(ref buffer,Throttle);
            BinSerialize.WriteUShort(ref buffer,Interference);
            /* PayloadByteSize = 20 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,CurrentField, ref _Current);    
            FloatType.Accept(visitor,CompensationxField, ref _Compensationx);    
            FloatType.Accept(visitor,CompensationyField, ref _Compensationy);    
            FloatType.Accept(visitor,CompensationzField, ref _Compensationz);    
            UInt16Type.Accept(visitor,ThrottleField, ref _Throttle);    
            UInt16Type.Accept(visitor,InterferenceField, ref _Interference);    

        }

        /// <summary>
        /// Current.
        /// OriginName: current, Units: A, IsExtended: false
        /// </summary>
        public static readonly Field CurrentField = new Field.Builder()
            .Name(nameof(Current))
            .Title("current")
            .Description("Current.")
            .FormatString(string.Empty)
            .Units(@"A")
            .DataType(FloatType.Default)

            .Build();
        private float _Current;
        public float Current { get => _Current; set { _Current = value; } }
        /// <summary>
        /// Motor Compensation X.
        /// OriginName: CompensationX, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CompensationxField = new Field.Builder()
            .Name(nameof(Compensationx))
            .Title("CompensationX")
            .Description("Motor Compensation X.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Compensationx;
        public float Compensationx { get => _Compensationx; set { _Compensationx = value; } }
        /// <summary>
        /// Motor Compensation Y.
        /// OriginName: CompensationY, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CompensationyField = new Field.Builder()
            .Name(nameof(Compensationy))
            .Title("CompensationY")
            .Description("Motor Compensation Y.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Compensationy;
        public float Compensationy { get => _Compensationy; set { _Compensationy = value; } }
        /// <summary>
        /// Motor Compensation Z.
        /// OriginName: CompensationZ, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CompensationzField = new Field.Builder()
            .Name(nameof(Compensationz))
            .Title("CompensationZ")
            .Description("Motor Compensation Z.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Compensationz;
        public float Compensationz { get => _Compensationz; set { _Compensationz = value; } }
        /// <summary>
        /// Throttle.
        /// OriginName: throttle, Units: d%, IsExtended: false
        /// </summary>
        public static readonly Field ThrottleField = new Field.Builder()
            .Name(nameof(Throttle))
            .Title("throttle")
            .Description("Throttle.")
            .FormatString(string.Empty)
            .Units(@"d%")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Throttle;
        public ushort Throttle { get => _Throttle; set { _Throttle = value; } }
        /// <summary>
        /// Interference.
        /// OriginName: interference, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field InterferenceField = new Field.Builder()
            .Name(nameof(Interference))
            .Title("interference")
            .Description("Interference.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Interference;
        public ushort Interference { get => _Interference; set { _Interference = value; } }
    }
    /// <summary>
    /// Status of secondary AHRS filter if available.
    ///  AHRS2
    /// </summary>
    public class Ahrs2Packet : MavlinkV2Message<Ahrs2Payload>
    {
        public const int MessageId = 178;
        
        public const byte CrcExtra = 47;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Ahrs2Payload Payload { get; } = new();

        public override string Name => "AHRS2";
    }

    /// <summary>
    ///  AHRS2
    /// </summary>
    public class Ahrs2Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 24; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float roll
            +4 // float pitch
            +4 // float yaw
            +4 // float altitude
            +4 // int32_t lat
            +4 // int32_t lng
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Roll = BinSerialize.ReadFloat(ref buffer);
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            Altitude = BinSerialize.ReadFloat(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lng = BinSerialize.ReadInt(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteFloat(ref buffer,Altitude);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lng);
            /* PayloadByteSize = 24 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,RollField, ref _Roll);    
            FloatType.Accept(visitor,PitchField, ref _Pitch);    
            FloatType.Accept(visitor,YawField, ref _Yaw);    
            FloatType.Accept(visitor,AltitudeField, ref _Altitude);    
            Int32Type.Accept(visitor,LatField, ref _Lat);    
            Int32Type.Accept(visitor,LngField, ref _Lng);    

        }

        /// <summary>
        /// Roll angle.
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Roll;
        public float Roll { get => _Roll; set { _Roll = value; } }
        /// <summary>
        /// Pitch angle.
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch angle.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Pitch;
        public float Pitch { get => _Pitch; set { _Pitch = value; } }
        /// <summary>
        /// Yaw angle.
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw angle.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Yaw;
        public float Yaw { get => _Yaw; set { _Yaw = value; } }
        /// <summary>
        /// Altitude (MSL).
        /// OriginName: altitude, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field AltitudeField = new Field.Builder()
            .Name(nameof(Altitude))
            .Title("altitude")
            .Description("Altitude (MSL).")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Altitude;
        public float Altitude { get => _Altitude; set { _Altitude = value; } }
        /// <summary>
        /// Latitude.
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Latitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lat;
        public int Lat { get => _Lat; set { _Lat = value; } }
        /// <summary>
        /// Longitude.
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LngField = new Field.Builder()
            .Name(nameof(Lng))
            .Title("lng")
            .Description("Longitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lng;
        public int Lng { get => _Lng; set { _Lng = value; } }
    }
    /// <summary>
    /// Camera Event.
    ///  CAMERA_STATUS
    /// </summary>
    public class CameraStatusPacket : MavlinkV2Message<CameraStatusPayload>
    {
        public const int MessageId = 179;
        
        public const byte CrcExtra = 189;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override CameraStatusPayload Payload { get; } = new();

        public override string Name => "CAMERA_STATUS";
    }

    /// <summary>
    ///  CAMERA_STATUS
    /// </summary>
    public class CameraStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 29; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 29; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_usec
            +4 // float p1
            +4 // float p2
            +4 // float p3
            +4 // float p4
            +2 // uint16_t img_idx
            +1 // uint8_t target_system
            +1 // uint8_t cam_idx
            + 1 // uint8_t event_id
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUsec = BinSerialize.ReadULong(ref buffer);
            P1 = BinSerialize.ReadFloat(ref buffer);
            P2 = BinSerialize.ReadFloat(ref buffer);
            P3 = BinSerialize.ReadFloat(ref buffer);
            P4 = BinSerialize.ReadFloat(ref buffer);
            ImgIdx = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            CamIdx = (byte)BinSerialize.ReadByte(ref buffer);
            EventId = (CameraStatusTypes)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUsec);
            BinSerialize.WriteFloat(ref buffer,P1);
            BinSerialize.WriteFloat(ref buffer,P2);
            BinSerialize.WriteFloat(ref buffer,P3);
            BinSerialize.WriteFloat(ref buffer,P4);
            BinSerialize.WriteUShort(ref buffer,ImgIdx);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)CamIdx);
            BinSerialize.WriteByte(ref buffer,(byte)EventId);
            /* PayloadByteSize = 29 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUsecField, ref _TimeUsec);    
            FloatType.Accept(visitor,P1Field, ref _P1);    
            FloatType.Accept(visitor,P2Field, ref _P2);    
            FloatType.Accept(visitor,P3Field, ref _P3);    
            FloatType.Accept(visitor,P4Field, ref _P4);    
            UInt16Type.Accept(visitor,ImgIdxField, ref _ImgIdx);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,CamIdxField, ref _CamIdx);    
            var tmpEventId = (byte)EventId;
            UInt8Type.Accept(visitor,EventIdField, ref tmpEventId);
            EventId = (CameraStatusTypes)tmpEventId;

        }

        /// <summary>
        /// Image timestamp (since UNIX epoch, according to camera clock).
        /// OriginName: time_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUsecField = new Field.Builder()
            .Name(nameof(TimeUsec))
            .Title("time_usec")
            .Description("Image timestamp (since UNIX epoch, according to camera clock).")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TimeUsec;
        public ulong TimeUsec { get => _TimeUsec; set { _TimeUsec = value; } }
        /// <summary>
        /// Parameter 1 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).
        /// OriginName: p1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field P1Field = new Field.Builder()
            .Name(nameof(P1))
            .Title("p1")
            .Description("Parameter 1 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _P1;
        public float P1 { get => _P1; set { _P1 = value; } }
        /// <summary>
        /// Parameter 2 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).
        /// OriginName: p2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field P2Field = new Field.Builder()
            .Name(nameof(P2))
            .Title("p2")
            .Description("Parameter 2 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _P2;
        public float P2 { get => _P2; set { _P2 = value; } }
        /// <summary>
        /// Parameter 3 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).
        /// OriginName: p3, Units: , IsExtended: false
        /// </summary>
        public static readonly Field P3Field = new Field.Builder()
            .Name(nameof(P3))
            .Title("p3")
            .Description("Parameter 3 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _P3;
        public float P3 { get => _P3; set { _P3 = value; } }
        /// <summary>
        /// Parameter 4 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).
        /// OriginName: p4, Units: , IsExtended: false
        /// </summary>
        public static readonly Field P4Field = new Field.Builder()
            .Name(nameof(P4))
            .Title("p4")
            .Description("Parameter 4 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _P4;
        public float P4 { get => _P4; set { _P4 = value; } }
        /// <summary>
        /// Image index.
        /// OriginName: img_idx, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ImgIdxField = new Field.Builder()
            .Name(nameof(ImgIdx))
            .Title("img_idx")
            .Description("Image index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ImgIdx;
        public ushort ImgIdx { get => _ImgIdx; set { _ImgIdx = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Camera ID.
        /// OriginName: cam_idx, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CamIdxField = new Field.Builder()
            .Name(nameof(CamIdx))
            .Title("cam_idx")
            .Description("Camera ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _CamIdx;
        public byte CamIdx { get => _CamIdx; set { _CamIdx = value; } }
        /// <summary>
        /// Event type.
        /// OriginName: event_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field EventIdField = new Field.Builder()
            .Name(nameof(EventId))
            .Title("event_id")
            .Description("Event type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public CameraStatusTypes _EventId;
        public CameraStatusTypes EventId { get => _EventId; set => _EventId = value; } 
    }
    /// <summary>
    /// Camera Capture Feedback.
    ///  CAMERA_FEEDBACK
    /// </summary>
    public class CameraFeedbackPacket : MavlinkV2Message<CameraFeedbackPayload>
    {
        public const int MessageId = 180;
        
        public const byte CrcExtra = 52;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override CameraFeedbackPayload Payload { get; } = new();

        public override string Name => "CAMERA_FEEDBACK";
    }

    /// <summary>
    ///  CAMERA_FEEDBACK
    /// </summary>
    public class CameraFeedbackPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 47; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 47; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_usec
            +4 // int32_t lat
            +4 // int32_t lng
            +4 // float alt_msl
            +4 // float alt_rel
            +4 // float roll
            +4 // float pitch
            +4 // float yaw
            +4 // float foc_len
            +2 // uint16_t img_idx
            +1 // uint8_t target_system
            +1 // uint8_t cam_idx
            + 1 // uint8_t flags
            +2 // uint16_t completed_captures
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUsec = BinSerialize.ReadULong(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lng = BinSerialize.ReadInt(ref buffer);
            AltMsl = BinSerialize.ReadFloat(ref buffer);
            AltRel = BinSerialize.ReadFloat(ref buffer);
            Roll = BinSerialize.ReadFloat(ref buffer);
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            FocLen = BinSerialize.ReadFloat(ref buffer);
            ImgIdx = BinSerialize.ReadUShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            CamIdx = (byte)BinSerialize.ReadByte(ref buffer);
            Flags = (CameraFeedbackFlags)BinSerialize.ReadByte(ref buffer);
            // extended field 'CompletedCaptures' can be empty
            if (buffer.IsEmpty) return;
            CompletedCaptures = BinSerialize.ReadUShort(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUsec);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lng);
            BinSerialize.WriteFloat(ref buffer,AltMsl);
            BinSerialize.WriteFloat(ref buffer,AltRel);
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteFloat(ref buffer,FocLen);
            BinSerialize.WriteUShort(ref buffer,ImgIdx);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)CamIdx);
            BinSerialize.WriteByte(ref buffer,(byte)Flags);
            BinSerialize.WriteUShort(ref buffer,CompletedCaptures);
            /* PayloadByteSize = 47 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUsecField, ref _TimeUsec);    
            Int32Type.Accept(visitor,LatField, ref _Lat);    
            Int32Type.Accept(visitor,LngField, ref _Lng);    
            FloatType.Accept(visitor,AltMslField, ref _AltMsl);    
            FloatType.Accept(visitor,AltRelField, ref _AltRel);    
            FloatType.Accept(visitor,RollField, ref _Roll);    
            FloatType.Accept(visitor,PitchField, ref _Pitch);    
            FloatType.Accept(visitor,YawField, ref _Yaw);    
            FloatType.Accept(visitor,FocLenField, ref _FocLen);    
            UInt16Type.Accept(visitor,ImgIdxField, ref _ImgIdx);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,CamIdxField, ref _CamIdx);    
            var tmpFlags = (byte)Flags;
            UInt8Type.Accept(visitor,FlagsField, ref tmpFlags);
            Flags = (CameraFeedbackFlags)tmpFlags;
            UInt16Type.Accept(visitor,CompletedCapturesField, ref _CompletedCaptures);    

        }

        /// <summary>
        /// Image timestamp (since UNIX epoch), as passed in by CAMERA_STATUS message (or autopilot if no CCB).
        /// OriginName: time_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUsecField = new Field.Builder()
            .Name(nameof(TimeUsec))
            .Title("time_usec")
            .Description("Image timestamp (since UNIX epoch), as passed in by CAMERA_STATUS message (or autopilot if no CCB).")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TimeUsec;
        public ulong TimeUsec { get => _TimeUsec; set { _TimeUsec = value; } }
        /// <summary>
        /// Latitude.
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Latitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lat;
        public int Lat { get => _Lat; set { _Lat = value; } }
        /// <summary>
        /// Longitude.
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LngField = new Field.Builder()
            .Name(nameof(Lng))
            .Title("lng")
            .Description("Longitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lng;
        public int Lng { get => _Lng; set { _Lng = value; } }
        /// <summary>
        /// Altitude (MSL).
        /// OriginName: alt_msl, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field AltMslField = new Field.Builder()
            .Name(nameof(AltMsl))
            .Title("alt_msl")
            .Description("Altitude (MSL).")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _AltMsl;
        public float AltMsl { get => _AltMsl; set { _AltMsl = value; } }
        /// <summary>
        /// Altitude (Relative to HOME location).
        /// OriginName: alt_rel, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field AltRelField = new Field.Builder()
            .Name(nameof(AltRel))
            .Title("alt_rel")
            .Description("Altitude (Relative to HOME location).")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _AltRel;
        public float AltRel { get => _AltRel; set { _AltRel = value; } }
        /// <summary>
        /// Camera Roll angle (earth frame, +-180).
        /// OriginName: roll, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Camera Roll angle (earth frame, +-180).")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Roll;
        public float Roll { get => _Roll; set { _Roll = value; } }
        /// <summary>
        /// Camera Pitch angle (earth frame, +-180).
        /// OriginName: pitch, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Camera Pitch angle (earth frame, +-180).")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Pitch;
        public float Pitch { get => _Pitch; set { _Pitch = value; } }
        /// <summary>
        /// Camera Yaw (earth frame, 0-360, true).
        /// OriginName: yaw, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Camera Yaw (earth frame, 0-360, true).")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Yaw;
        public float Yaw { get => _Yaw; set { _Yaw = value; } }
        /// <summary>
        /// Focal Length.
        /// OriginName: foc_len, Units: mm, IsExtended: false
        /// </summary>
        public static readonly Field FocLenField = new Field.Builder()
            .Name(nameof(FocLen))
            .Title("foc_len")
            .Description("Focal Length.")
            .FormatString(string.Empty)
            .Units(@"mm")
            .DataType(FloatType.Default)

            .Build();
        private float _FocLen;
        public float FocLen { get => _FocLen; set { _FocLen = value; } }
        /// <summary>
        /// Image index.
        /// OriginName: img_idx, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ImgIdxField = new Field.Builder()
            .Name(nameof(ImgIdx))
            .Title("img_idx")
            .Description("Image index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ImgIdx;
        public ushort ImgIdx { get => _ImgIdx; set { _ImgIdx = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Camera ID.
        /// OriginName: cam_idx, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CamIdxField = new Field.Builder()
            .Name(nameof(CamIdx))
            .Title("cam_idx")
            .Description("Camera ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _CamIdx;
        public byte CamIdx { get => _CamIdx; set { _CamIdx = value; } }
        /// <summary>
        /// Feedback flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Feedback flags.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public CameraFeedbackFlags _Flags;
        public CameraFeedbackFlags Flags { get => _Flags; set => _Flags = value; } 
        /// <summary>
        /// Completed image captures.
        /// OriginName: completed_captures, Units: , IsExtended: true
        /// </summary>
        public static readonly Field CompletedCapturesField = new Field.Builder()
            .Name(nameof(CompletedCaptures))
            .Title("completed_captures")
            .Description("Completed image captures.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _CompletedCaptures;
        public ushort CompletedCaptures { get => _CompletedCaptures; set { _CompletedCaptures = value; } }
    }
    /// <summary>
    /// 2nd Battery status
    ///  BATTERY2
    /// </summary>
    public class Battery2Packet : MavlinkV2Message<Battery2Payload>
    {
        public const int MessageId = 181;
        
        public const byte CrcExtra = 174;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Battery2Payload Payload { get; } = new();

        public override string Name => "BATTERY2";
    }

    /// <summary>
    ///  BATTERY2
    /// </summary>
    public class Battery2Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 4; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 4; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t voltage
            +2 // int16_t current_battery
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Voltage = BinSerialize.ReadUShort(ref buffer);
            CurrentBattery = BinSerialize.ReadShort(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,Voltage);
            BinSerialize.WriteShort(ref buffer,CurrentBattery);
            /* PayloadByteSize = 4 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,VoltageField, ref _Voltage);    
            Int16Type.Accept(visitor,CurrentBatteryField, ref _CurrentBattery);

        }

        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field VoltageField = new Field.Builder()
            .Name(nameof(Voltage))
            .Title("voltage")
            .Description("Voltage.")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _Voltage;
        public ushort Voltage { get => _Voltage; set { _Voltage = value; } }
        /// <summary>
        /// Battery current, -1: autopilot does not measure the current.
        /// OriginName: current_battery, Units: cA, IsExtended: false
        /// </summary>
        public static readonly Field CurrentBatteryField = new Field.Builder()
            .Name(nameof(CurrentBattery))
            .Title("current_battery")
            .Description("Battery current, -1: autopilot does not measure the current.")
            .FormatString(string.Empty)
            .Units(@"cA")
            .DataType(Int16Type.Default)

            .Build();
        private short _CurrentBattery;
        public short CurrentBattery { get => _CurrentBattery; set { _CurrentBattery = value; } }
    }
    /// <summary>
    /// Status of third AHRS filter if available. This is for ANU research group (Ali and Sean).
    ///  AHRS3
    /// </summary>
    public class Ahrs3Packet : MavlinkV2Message<Ahrs3Payload>
    {
        public const int MessageId = 182;
        
        public const byte CrcExtra = 229;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override Ahrs3Payload Payload { get; } = new();

        public override string Name => "AHRS3";
    }

    /// <summary>
    ///  AHRS3
    /// </summary>
    public class Ahrs3Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 40; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 40; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float roll
            +4 // float pitch
            +4 // float yaw
            +4 // float altitude
            +4 // int32_t lat
            +4 // int32_t lng
            +4 // float v1
            +4 // float v2
            +4 // float v3
            +4 // float v4
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Roll = BinSerialize.ReadFloat(ref buffer);
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            Altitude = BinSerialize.ReadFloat(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lng = BinSerialize.ReadInt(ref buffer);
            V1 = BinSerialize.ReadFloat(ref buffer);
            V2 = BinSerialize.ReadFloat(ref buffer);
            V3 = BinSerialize.ReadFloat(ref buffer);
            V4 = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteFloat(ref buffer,Altitude);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lng);
            BinSerialize.WriteFloat(ref buffer,V1);
            BinSerialize.WriteFloat(ref buffer,V2);
            BinSerialize.WriteFloat(ref buffer,V3);
            BinSerialize.WriteFloat(ref buffer,V4);
            /* PayloadByteSize = 40 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,RollField, ref _Roll);    
            FloatType.Accept(visitor,PitchField, ref _Pitch);    
            FloatType.Accept(visitor,YawField, ref _Yaw);    
            FloatType.Accept(visitor,AltitudeField, ref _Altitude);    
            Int32Type.Accept(visitor,LatField, ref _Lat);    
            Int32Type.Accept(visitor,LngField, ref _Lng);    
            FloatType.Accept(visitor,V1Field, ref _V1);    
            FloatType.Accept(visitor,V2Field, ref _V2);    
            FloatType.Accept(visitor,V3Field, ref _V3);    
            FloatType.Accept(visitor,V4Field, ref _V4);    

        }

        /// <summary>
        /// Roll angle.
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Roll;
        public float Roll { get => _Roll; set { _Roll = value; } }
        /// <summary>
        /// Pitch angle.
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch angle.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Pitch;
        public float Pitch { get => _Pitch; set { _Pitch = value; } }
        /// <summary>
        /// Yaw angle.
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw angle.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Yaw;
        public float Yaw { get => _Yaw; set { _Yaw = value; } }
        /// <summary>
        /// Altitude (MSL).
        /// OriginName: altitude, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field AltitudeField = new Field.Builder()
            .Name(nameof(Altitude))
            .Title("altitude")
            .Description("Altitude (MSL).")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Altitude;
        public float Altitude { get => _Altitude; set { _Altitude = value; } }
        /// <summary>
        /// Latitude.
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Latitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lat;
        public int Lat { get => _Lat; set { _Lat = value; } }
        /// <summary>
        /// Longitude.
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LngField = new Field.Builder()
            .Name(nameof(Lng))
            .Title("lng")
            .Description("Longitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lng;
        public int Lng { get => _Lng; set { _Lng = value; } }
        /// <summary>
        /// Test variable1.
        /// OriginName: v1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field V1Field = new Field.Builder()
            .Name(nameof(V1))
            .Title("v1")
            .Description("Test variable1.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _V1;
        public float V1 { get => _V1; set { _V1 = value; } }
        /// <summary>
        /// Test variable2.
        /// OriginName: v2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field V2Field = new Field.Builder()
            .Name(nameof(V2))
            .Title("v2")
            .Description("Test variable2.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _V2;
        public float V2 { get => _V2; set { _V2 = value; } }
        /// <summary>
        /// Test variable3.
        /// OriginName: v3, Units: , IsExtended: false
        /// </summary>
        public static readonly Field V3Field = new Field.Builder()
            .Name(nameof(V3))
            .Title("v3")
            .Description("Test variable3.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _V3;
        public float V3 { get => _V3; set { _V3 = value; } }
        /// <summary>
        /// Test variable4.
        /// OriginName: v4, Units: , IsExtended: false
        /// </summary>
        public static readonly Field V4Field = new Field.Builder()
            .Name(nameof(V4))
            .Title("v4")
            .Description("Test variable4.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _V4;
        public float V4 { get => _V4; set { _V4 = value; } }
    }
    /// <summary>
    /// Request the autopilot version from the system/component.
    ///  AUTOPILOT_VERSION_REQUEST
    /// </summary>
    public class AutopilotVersionRequestPacket : MavlinkV2Message<AutopilotVersionRequestPayload>
    {
        public const int MessageId = 183;
        
        public const byte CrcExtra = 85;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AutopilotVersionRequestPayload Payload { get; } = new();

        public override string Name => "AUTOPILOT_VERSION_REQUEST";
    }

    /// <summary>
    ///  AUTOPILOT_VERSION_REQUEST
    /// </summary>
    public class AutopilotVersionRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 2; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 2; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 2 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
    }
    /// <summary>
    /// Send a block of log data to remote location.
    ///  REMOTE_LOG_DATA_BLOCK
    /// </summary>
    public class RemoteLogDataBlockPacket : MavlinkV2Message<RemoteLogDataBlockPayload>
    {
        public const int MessageId = 184;
        
        public const byte CrcExtra = 159;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override RemoteLogDataBlockPayload Payload { get; } = new();

        public override string Name => "REMOTE_LOG_DATA_BLOCK";
    }

    /// <summary>
    ///  REMOTE_LOG_DATA_BLOCK
    /// </summary>
    public class RemoteLogDataBlockPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 206; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 206; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 4 // uint32_t seqno
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +Data.Length // uint8_t[200] data
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Seqno = (MavRemoteLogDataBlockCommands)BinSerialize.ReadUInt(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/200 - Math.Max(0,((/*PayloadByteSize*/206 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,(uint)Seqno);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            /* PayloadByteSize = 206 */;
        }

        public void Visit(IVisitor visitor)
        {
            var tmpSeqno = (uint)Seqno;
            UInt32Type.Accept(visitor,SeqnoField, ref tmpSeqno);
            Seqno = (MavRemoteLogDataBlockCommands)tmpSeqno;
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            ArrayType.Accept(visitor,DataField, 200,
                (index,v) => UInt8Type.Accept(v, DataField, ref Data[index]));    

        }

        /// <summary>
        /// LoggerFactory data block sequence number.
        /// OriginName: seqno, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SeqnoField = new Field.Builder()
            .Name(nameof(Seqno))
            .Title("seqno")
            .Description("LoggerFactory data block sequence number.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        public MavRemoteLogDataBlockCommands _Seqno;
        public MavRemoteLogDataBlockCommands Seqno { get => _Seqno; set => _Seqno = value; } 
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// LoggerFactory data block.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("LoggerFactory data block.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,200))

            .Build();
        public const int DataMaxItemsCount = 200;
        public byte[] Data { get; } = new byte[200];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 200;
    }
    /// <summary>
    /// Send Status of each log block that autopilot board might have sent.
    ///  REMOTE_LOG_BLOCK_STATUS
    /// </summary>
    public class RemoteLogBlockStatusPacket : MavlinkV2Message<RemoteLogBlockStatusPayload>
    {
        public const int MessageId = 185;
        
        public const byte CrcExtra = 186;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override RemoteLogBlockStatusPayload Payload { get; } = new();

        public override string Name => "REMOTE_LOG_BLOCK_STATUS";
    }

    /// <summary>
    ///  REMOTE_LOG_BLOCK_STATUS
    /// </summary>
    public class RemoteLogBlockStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 7; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t seqno
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            + 1 // uint8_t status
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Seqno = BinSerialize.ReadUInt(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Status = (MavRemoteLogDataBlockStatuses)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,Seqno);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Status);
            /* PayloadByteSize = 7 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,SeqnoField, ref _Seqno);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            var tmpStatus = (byte)Status;
            UInt8Type.Accept(visitor,StatusField, ref tmpStatus);
            Status = (MavRemoteLogDataBlockStatuses)tmpStatus;

        }

        /// <summary>
        /// LoggerFactory data block sequence number.
        /// OriginName: seqno, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SeqnoField = new Field.Builder()
            .Name(nameof(Seqno))
            .Title("seqno")
            .Description("LoggerFactory data block sequence number.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _Seqno;
        public uint Seqno { get => _Seqno; set { _Seqno = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// LoggerFactory data block status.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StatusField = new Field.Builder()
            .Name(nameof(Status))
            .Title("status")
            .Description("LoggerFactory data block status.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public MavRemoteLogDataBlockStatuses _Status;
        public MavRemoteLogDataBlockStatuses Status { get => _Status; set => _Status = value; } 
    }
    /// <summary>
    /// Control vehicle LEDs.
    ///  LED_CONTROL
    /// </summary>
    public class LedControlPacket : MavlinkV2Message<LedControlPayload>
    {
        public const int MessageId = 186;
        
        public const byte CrcExtra = 72;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override LedControlPayload Payload { get; } = new();

        public override string Name => "LED_CONTROL";
    }

    /// <summary>
    ///  LED_CONTROL
    /// </summary>
    public class LedControlPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 29; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 29; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t instance
            +1 // uint8_t pattern
            +1 // uint8_t custom_len
            +CustomBytes.Length // uint8_t[24] custom_bytes
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Instance = (byte)BinSerialize.ReadByte(ref buffer);
            Pattern = (byte)BinSerialize.ReadByte(ref buffer);
            CustomLen = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/24 - Math.Max(0,((/*PayloadByteSize*/29 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                CustomBytes[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Instance);
            BinSerialize.WriteByte(ref buffer,(byte)Pattern);
            BinSerialize.WriteByte(ref buffer,(byte)CustomLen);
            for(var i=0;i<CustomBytes.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)CustomBytes[i]);
            }
            /* PayloadByteSize = 29 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,InstanceField, ref _Instance);    
            UInt8Type.Accept(visitor,PatternField, ref _Pattern);    
            UInt8Type.Accept(visitor,CustomLenField, ref _CustomLen);    
            ArrayType.Accept(visitor,CustomBytesField, 24,
                (index,v) => UInt8Type.Accept(v, CustomBytesField, ref CustomBytes[index]));    

        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Instance (LED instance to control or 255 for all LEDs).
        /// OriginName: instance, Units: , IsExtended: false
        /// </summary>
        public static readonly Field InstanceField = new Field.Builder()
            .Name(nameof(Instance))
            .Title("instance")
            .Description("Instance (LED instance to control or 255 for all LEDs).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Instance;
        public byte Instance { get => _Instance; set { _Instance = value; } }
        /// <summary>
        /// Pattern (see LED_PATTERN_ENUM).
        /// OriginName: pattern, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PatternField = new Field.Builder()
            .Name(nameof(Pattern))
            .Title("pattern")
            .Description("Pattern (see LED_PATTERN_ENUM).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Pattern;
        public byte Pattern { get => _Pattern; set { _Pattern = value; } }
        /// <summary>
        /// Custom Byte Length.
        /// OriginName: custom_len, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CustomLenField = new Field.Builder()
            .Name(nameof(CustomLen))
            .Title("custom_len")
            .Description("Custom Byte Length.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _CustomLen;
        public byte CustomLen { get => _CustomLen; set { _CustomLen = value; } }
        /// <summary>
        /// Custom Bytes.
        /// OriginName: custom_bytes, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CustomBytesField = new Field.Builder()
            .Name(nameof(CustomBytes))
            .Title("custom_bytes")
            .Description("Custom Bytes.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,24))

            .Build();
        public const int CustomBytesMaxItemsCount = 24;
        public byte[] CustomBytes { get; } = new byte[24];
        [Obsolete("This method is deprecated. Use GetCustomBytesMaxItemsCount instead.")]
        public byte GetCustomBytesMaxItemsCount() => 24;
    }
    /// <summary>
    /// Reports progress of compass calibration.
    ///  MAG_CAL_PROGRESS
    /// </summary>
    public class MagCalProgressPacket : MavlinkV2Message<MagCalProgressPayload>
    {
        public const int MessageId = 191;
        
        public const byte CrcExtra = 92;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override MagCalProgressPayload Payload { get; } = new();

        public override string Name => "MAG_CAL_PROGRESS";
    }

    /// <summary>
    ///  MAG_CAL_PROGRESS
    /// </summary>
    public class MagCalProgressPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 27; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 27; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float direction_x
            +4 // float direction_y
            +4 // float direction_z
            +1 // uint8_t compass_id
            +1 // uint8_t cal_mask
            + 1 // uint8_t cal_status
            +1 // uint8_t attempt
            +1 // uint8_t completion_pct
            +CompletionMask.Length // uint8_t[10] completion_mask
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            DirectionX = BinSerialize.ReadFloat(ref buffer);
            DirectionY = BinSerialize.ReadFloat(ref buffer);
            DirectionZ = BinSerialize.ReadFloat(ref buffer);
            CompassId = (byte)BinSerialize.ReadByte(ref buffer);
            CalMask = (byte)BinSerialize.ReadByte(ref buffer);
            CalStatus = (MagCalStatus)BinSerialize.ReadByte(ref buffer);
            Attempt = (byte)BinSerialize.ReadByte(ref buffer);
            CompletionPct = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/10 - Math.Max(0,((/*PayloadByteSize*/27 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                CompletionMask[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,DirectionX);
            BinSerialize.WriteFloat(ref buffer,DirectionY);
            BinSerialize.WriteFloat(ref buffer,DirectionZ);
            BinSerialize.WriteByte(ref buffer,(byte)CompassId);
            BinSerialize.WriteByte(ref buffer,(byte)CalMask);
            BinSerialize.WriteByte(ref buffer,(byte)CalStatus);
            BinSerialize.WriteByte(ref buffer,(byte)Attempt);
            BinSerialize.WriteByte(ref buffer,(byte)CompletionPct);
            for(var i=0;i<CompletionMask.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)CompletionMask[i]);
            }
            /* PayloadByteSize = 27 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,DirectionXField, ref _DirectionX);    
            FloatType.Accept(visitor,DirectionYField, ref _DirectionY);    
            FloatType.Accept(visitor,DirectionZField, ref _DirectionZ);    
            UInt8Type.Accept(visitor,CompassIdField, ref _CompassId);    
            UInt8Type.Accept(visitor,CalMaskField, ref _CalMask);    
            var tmpCalStatus = (byte)CalStatus;
            UInt8Type.Accept(visitor,CalStatusField, ref tmpCalStatus);
            CalStatus = (MagCalStatus)tmpCalStatus;
            UInt8Type.Accept(visitor,AttemptField, ref _Attempt);    
            UInt8Type.Accept(visitor,CompletionPctField, ref _CompletionPct);    
            ArrayType.Accept(visitor,CompletionMaskField, 10,
                (index,v) => UInt8Type.Accept(v, CompletionMaskField, ref CompletionMask[index]));    

        }

        /// <summary>
        /// Body frame direction vector for display.
        /// OriginName: direction_x, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DirectionXField = new Field.Builder()
            .Name(nameof(DirectionX))
            .Title("direction_x")
            .Description("Body frame direction vector for display.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _DirectionX;
        public float DirectionX { get => _DirectionX; set { _DirectionX = value; } }
        /// <summary>
        /// Body frame direction vector for display.
        /// OriginName: direction_y, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DirectionYField = new Field.Builder()
            .Name(nameof(DirectionY))
            .Title("direction_y")
            .Description("Body frame direction vector for display.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _DirectionY;
        public float DirectionY { get => _DirectionY; set { _DirectionY = value; } }
        /// <summary>
        /// Body frame direction vector for display.
        /// OriginName: direction_z, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DirectionZField = new Field.Builder()
            .Name(nameof(DirectionZ))
            .Title("direction_z")
            .Description("Body frame direction vector for display.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _DirectionZ;
        public float DirectionZ { get => _DirectionZ; set { _DirectionZ = value; } }
        /// <summary>
        /// Compass being calibrated.
        /// OriginName: compass_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CompassIdField = new Field.Builder()
            .Name(nameof(CompassId))
            .Title("compass_id")
            .Description("Compass being calibrated.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _CompassId;
        public byte CompassId { get => _CompassId; set { _CompassId = value; } }
        /// <summary>
        /// Bitmask of compasses being calibrated.
        /// OriginName: cal_mask, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CalMaskField = new Field.Builder()
            .Name(nameof(CalMask))
            .Title("bitmask")
            .Description("Bitmask of compasses being calibrated.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _CalMask;
        public byte CalMask { get => _CalMask; set { _CalMask = value; } }
        /// <summary>
        /// Calibration Status.
        /// OriginName: cal_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CalStatusField = new Field.Builder()
            .Name(nameof(CalStatus))
            .Title("cal_status")
            .Description("Calibration Status.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public MagCalStatus _CalStatus;
        public MagCalStatus CalStatus { get => _CalStatus; set => _CalStatus = value; } 
        /// <summary>
        /// Attempt number.
        /// OriginName: attempt, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AttemptField = new Field.Builder()
            .Name(nameof(Attempt))
            .Title("attempt")
            .Description("Attempt number.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Attempt;
        public byte Attempt { get => _Attempt; set { _Attempt = value; } }
        /// <summary>
        /// Completion percentage.
        /// OriginName: completion_pct, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field CompletionPctField = new Field.Builder()
            .Name(nameof(CompletionPct))
            .Title("completion_pct")
            .Description("Completion percentage.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(UInt8Type.Default)

            .Build();
        private byte _CompletionPct;
        public byte CompletionPct { get => _CompletionPct; set { _CompletionPct = value; } }
        /// <summary>
        /// Bitmask of sphere sections (see http://en.wikipedia.org/wiki/Geodesic_grid).
        /// OriginName: completion_mask, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CompletionMaskField = new Field.Builder()
            .Name(nameof(CompletionMask))
            .Title("completion_mask")
            .Description("Bitmask of sphere sections (see http://en.wikipedia.org/wiki/Geodesic_grid).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,10))

            .Build();
        public const int CompletionMaskMaxItemsCount = 10;
        public byte[] CompletionMask { get; } = new byte[10];
        [Obsolete("This method is deprecated. Use GetCompletionMaskMaxItemsCount instead.")]
        public byte GetCompletionMaskMaxItemsCount() => 10;
    }
    /// <summary>
    /// EKF Status message including flags and variances.
    ///  EKF_STATUS_REPORT
    /// </summary>
    public class EkfStatusReportPacket : MavlinkV2Message<EkfStatusReportPayload>
    {
        public const int MessageId = 193;
        
        public const byte CrcExtra = 71;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override EkfStatusReportPayload Payload { get; } = new();

        public override string Name => "EKF_STATUS_REPORT";
    }

    /// <summary>
    ///  EKF_STATUS_REPORT
    /// </summary>
    public class EkfStatusReportPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 26; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 26; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float velocity_variance
            +4 // float pos_horiz_variance
            +4 // float pos_vert_variance
            +4 // float compass_variance
            +4 // float terrain_alt_variance
            + 2 // uint16_t flags
            +4 // float airspeed_variance
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            VelocityVariance = BinSerialize.ReadFloat(ref buffer);
            PosHorizVariance = BinSerialize.ReadFloat(ref buffer);
            PosVertVariance = BinSerialize.ReadFloat(ref buffer);
            CompassVariance = BinSerialize.ReadFloat(ref buffer);
            TerrainAltVariance = BinSerialize.ReadFloat(ref buffer);
            Flags = (EkfStatusFlags)BinSerialize.ReadUShort(ref buffer);
            // extended field 'AirspeedVariance' can be empty
            if (buffer.IsEmpty) return;
            AirspeedVariance = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,VelocityVariance);
            BinSerialize.WriteFloat(ref buffer,PosHorizVariance);
            BinSerialize.WriteFloat(ref buffer,PosVertVariance);
            BinSerialize.WriteFloat(ref buffer,CompassVariance);
            BinSerialize.WriteFloat(ref buffer,TerrainAltVariance);
            BinSerialize.WriteUShort(ref buffer,(ushort)Flags);
            BinSerialize.WriteFloat(ref buffer,AirspeedVariance);
            /* PayloadByteSize = 26 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,VelocityVarianceField, ref _VelocityVariance);    
            FloatType.Accept(visitor,PosHorizVarianceField, ref _PosHorizVariance);    
            FloatType.Accept(visitor,PosVertVarianceField, ref _PosVertVariance);    
            FloatType.Accept(visitor,CompassVarianceField, ref _CompassVariance);    
            FloatType.Accept(visitor,TerrainAltVarianceField, ref _TerrainAltVariance);    
            var tmpFlags = (ushort)Flags;
            UInt16Type.Accept(visitor,FlagsField, ref tmpFlags);
            Flags = (EkfStatusFlags)tmpFlags;
            FloatType.Accept(visitor,AirspeedVarianceField, ref _AirspeedVariance);    

        }

        /// <summary>
        /// Velocity variance.
        /// OriginName: velocity_variance, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VelocityVarianceField = new Field.Builder()
            .Name(nameof(VelocityVariance))
            .Title("velocity_variance")
            .Description("Velocity variance.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _VelocityVariance;
        public float VelocityVariance { get => _VelocityVariance; set { _VelocityVariance = value; } }
        /// <summary>
        /// Horizontal Position variance.
        /// OriginName: pos_horiz_variance, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PosHorizVarianceField = new Field.Builder()
            .Name(nameof(PosHorizVariance))
            .Title("pos_horiz_variance")
            .Description("Horizontal Position variance.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _PosHorizVariance;
        public float PosHorizVariance { get => _PosHorizVariance; set { _PosHorizVariance = value; } }
        /// <summary>
        /// Vertical Position variance.
        /// OriginName: pos_vert_variance, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PosVertVarianceField = new Field.Builder()
            .Name(nameof(PosVertVariance))
            .Title("pos_vert_variance")
            .Description("Vertical Position variance.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _PosVertVariance;
        public float PosVertVariance { get => _PosVertVariance; set { _PosVertVariance = value; } }
        /// <summary>
        /// Compass variance.
        /// OriginName: compass_variance, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CompassVarianceField = new Field.Builder()
            .Name(nameof(CompassVariance))
            .Title("compass_variance")
            .Description("Compass variance.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _CompassVariance;
        public float CompassVariance { get => _CompassVariance; set { _CompassVariance = value; } }
        /// <summary>
        /// Terrain Altitude variance.
        /// OriginName: terrain_alt_variance, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TerrainAltVarianceField = new Field.Builder()
            .Name(nameof(TerrainAltVariance))
            .Title("terrain_alt_variance")
            .Description("Terrain Altitude variance.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _TerrainAltVariance;
        public float TerrainAltVariance { get => _TerrainAltVariance; set { _TerrainAltVariance = value; } }
        /// <summary>
        /// Flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("bitmask")
            .Description("Flags.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        public EkfStatusFlags _Flags;
        public EkfStatusFlags Flags { get => _Flags; set => _Flags = value; } 
        /// <summary>
        /// Airspeed variance.
        /// OriginName: airspeed_variance, Units: , IsExtended: true
        /// </summary>
        public static readonly Field AirspeedVarianceField = new Field.Builder()
            .Name(nameof(AirspeedVariance))
            .Title("airspeed_variance")
            .Description("Airspeed variance.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _AirspeedVariance;
        public float AirspeedVariance { get => _AirspeedVariance; set { _AirspeedVariance = value; } }
    }
    /// <summary>
    /// PID tuning information.
    ///  PID_TUNING
    /// </summary>
    public class PidTuningPacket : MavlinkV2Message<PidTuningPayload>
    {
        public const int MessageId = 194;
        
        public const byte CrcExtra = 98;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override PidTuningPayload Payload { get; } = new();

        public override string Name => "PID_TUNING";
    }

    /// <summary>
    ///  PID_TUNING
    /// </summary>
    public class PidTuningPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 33; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 33; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float desired
            +4 // float achieved
            +4 // float FF
            +4 // float P
            +4 // float I
            +4 // float D
            + 1 // uint8_t axis
            +4 // float SRate
            +4 // float PDmod
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Desired = BinSerialize.ReadFloat(ref buffer);
            Achieved = BinSerialize.ReadFloat(ref buffer);
            Ff = BinSerialize.ReadFloat(ref buffer);
            P = BinSerialize.ReadFloat(ref buffer);
            I = BinSerialize.ReadFloat(ref buffer);
            D = BinSerialize.ReadFloat(ref buffer);
            Axis = (PidTuningAxis)BinSerialize.ReadByte(ref buffer);
            // extended field 'Srate' can be empty
            if (buffer.IsEmpty) return;
            Srate = BinSerialize.ReadFloat(ref buffer);
            // extended field 'Pdmod' can be empty
            if (buffer.IsEmpty) return;
            Pdmod = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Desired);
            BinSerialize.WriteFloat(ref buffer,Achieved);
            BinSerialize.WriteFloat(ref buffer,Ff);
            BinSerialize.WriteFloat(ref buffer,P);
            BinSerialize.WriteFloat(ref buffer,I);
            BinSerialize.WriteFloat(ref buffer,D);
            BinSerialize.WriteByte(ref buffer,(byte)Axis);
            BinSerialize.WriteFloat(ref buffer,Srate);
            BinSerialize.WriteFloat(ref buffer,Pdmod);
            /* PayloadByteSize = 33 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,DesiredField, ref _Desired);    
            FloatType.Accept(visitor,AchievedField, ref _Achieved);    
            FloatType.Accept(visitor,FfField, ref _Ff);    
            FloatType.Accept(visitor,PField, ref _P);    
            FloatType.Accept(visitor,IField, ref _I);    
            FloatType.Accept(visitor,DField, ref _D);    
            var tmpAxis = (byte)Axis;
            UInt8Type.Accept(visitor,AxisField, ref tmpAxis);
            Axis = (PidTuningAxis)tmpAxis;
            FloatType.Accept(visitor,SrateField, ref _Srate);    
            FloatType.Accept(visitor,PdmodField, ref _Pdmod);    

        }

        /// <summary>
        /// Desired rate.
        /// OriginName: desired, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DesiredField = new Field.Builder()
            .Name(nameof(Desired))
            .Title("desired")
            .Description("Desired rate.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Desired;
        public float Desired { get => _Desired; set { _Desired = value; } }
        /// <summary>
        /// Achieved rate.
        /// OriginName: achieved, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AchievedField = new Field.Builder()
            .Name(nameof(Achieved))
            .Title("achieved")
            .Description("Achieved rate.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Achieved;
        public float Achieved { get => _Achieved; set { _Achieved = value; } }
        /// <summary>
        /// FF component.
        /// OriginName: FF, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FfField = new Field.Builder()
            .Name(nameof(Ff))
            .Title("FF")
            .Description("FF component.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Ff;
        public float Ff { get => _Ff; set { _Ff = value; } }
        /// <summary>
        /// P component.
        /// OriginName: P, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PField = new Field.Builder()
            .Name(nameof(P))
            .Title("P")
            .Description("P component.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _P;
        public float P { get => _P; set { _P = value; } }
        /// <summary>
        /// I component.
        /// OriginName: I, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IField = new Field.Builder()
            .Name(nameof(I))
            .Title("I")
            .Description("I component.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _I;
        public float I { get => _I; set { _I = value; } }
        /// <summary>
        /// D component.
        /// OriginName: D, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DField = new Field.Builder()
            .Name(nameof(D))
            .Title("D")
            .Description("D component.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _D;
        public float D { get => _D; set { _D = value; } }
        /// <summary>
        /// Axis.
        /// OriginName: axis, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxisField = new Field.Builder()
            .Name(nameof(Axis))
            .Title("axis")
            .Description("Axis.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public PidTuningAxis _Axis;
        public PidTuningAxis Axis { get => _Axis; set => _Axis = value; } 
        /// <summary>
        /// Slew rate.
        /// OriginName: SRate, Units: , IsExtended: true
        /// </summary>
        public static readonly Field SrateField = new Field.Builder()
            .Name(nameof(Srate))
            .Title("SRate")
            .Description("Slew rate.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Srate;
        public float Srate { get => _Srate; set { _Srate = value; } }
        /// <summary>
        /// P/D oscillation modifier.
        /// OriginName: PDmod, Units: , IsExtended: true
        /// </summary>
        public static readonly Field PdmodField = new Field.Builder()
            .Name(nameof(Pdmod))
            .Title("PDmod")
            .Description("P/D oscillation modifier.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Pdmod;
        public float Pdmod { get => _Pdmod; set { _Pdmod = value; } }
    }
    /// <summary>
    /// Deepstall path planning.
    ///  DEEPSTALL
    /// </summary>
    public class DeepstallPacket : MavlinkV2Message<DeepstallPayload>
    {
        public const int MessageId = 195;
        
        public const byte CrcExtra = 120;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override DeepstallPayload Payload { get; } = new();

        public override string Name => "DEEPSTALL";
    }

    /// <summary>
    ///  DEEPSTALL
    /// </summary>
    public class DeepstallPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 37; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 37; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // int32_t landing_lat
            +4 // int32_t landing_lon
            +4 // int32_t path_lat
            +4 // int32_t path_lon
            +4 // int32_t arc_entry_lat
            +4 // int32_t arc_entry_lon
            +4 // float altitude
            +4 // float expected_travel_distance
            +4 // float cross_track_error
            + 1 // uint8_t stage
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            LandingLat = BinSerialize.ReadInt(ref buffer);
            LandingLon = BinSerialize.ReadInt(ref buffer);
            PathLat = BinSerialize.ReadInt(ref buffer);
            PathLon = BinSerialize.ReadInt(ref buffer);
            ArcEntryLat = BinSerialize.ReadInt(ref buffer);
            ArcEntryLon = BinSerialize.ReadInt(ref buffer);
            Altitude = BinSerialize.ReadFloat(ref buffer);
            ExpectedTravelDistance = BinSerialize.ReadFloat(ref buffer);
            CrossTrackError = BinSerialize.ReadFloat(ref buffer);
            Stage = (DeepstallStage)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteInt(ref buffer,LandingLat);
            BinSerialize.WriteInt(ref buffer,LandingLon);
            BinSerialize.WriteInt(ref buffer,PathLat);
            BinSerialize.WriteInt(ref buffer,PathLon);
            BinSerialize.WriteInt(ref buffer,ArcEntryLat);
            BinSerialize.WriteInt(ref buffer,ArcEntryLon);
            BinSerialize.WriteFloat(ref buffer,Altitude);
            BinSerialize.WriteFloat(ref buffer,ExpectedTravelDistance);
            BinSerialize.WriteFloat(ref buffer,CrossTrackError);
            BinSerialize.WriteByte(ref buffer,(byte)Stage);
            /* PayloadByteSize = 37 */;
        }

        public void Visit(IVisitor visitor)
        {
            Int32Type.Accept(visitor,LandingLatField, ref _LandingLat);    
            Int32Type.Accept(visitor,LandingLonField, ref _LandingLon);    
            Int32Type.Accept(visitor,PathLatField, ref _PathLat);    
            Int32Type.Accept(visitor,PathLonField, ref _PathLon);    
            Int32Type.Accept(visitor,ArcEntryLatField, ref _ArcEntryLat);    
            Int32Type.Accept(visitor,ArcEntryLonField, ref _ArcEntryLon);    
            FloatType.Accept(visitor,AltitudeField, ref _Altitude);    
            FloatType.Accept(visitor,ExpectedTravelDistanceField, ref _ExpectedTravelDistance);    
            FloatType.Accept(visitor,CrossTrackErrorField, ref _CrossTrackError);    
            var tmpStage = (byte)Stage;
            UInt8Type.Accept(visitor,StageField, ref tmpStage);
            Stage = (DeepstallStage)tmpStage;

        }

        /// <summary>
        /// Landing latitude.
        /// OriginName: landing_lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LandingLatField = new Field.Builder()
            .Name(nameof(LandingLat))
            .Title("landing_lat")
            .Description("Landing latitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _LandingLat;
        public int LandingLat { get => _LandingLat; set { _LandingLat = value; } }
        /// <summary>
        /// Landing longitude.
        /// OriginName: landing_lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LandingLonField = new Field.Builder()
            .Name(nameof(LandingLon))
            .Title("landing_lon")
            .Description("Landing longitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _LandingLon;
        public int LandingLon { get => _LandingLon; set { _LandingLon = value; } }
        /// <summary>
        /// Final heading start point, latitude.
        /// OriginName: path_lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field PathLatField = new Field.Builder()
            .Name(nameof(PathLat))
            .Title("path_lat")
            .Description("Final heading start point, latitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _PathLat;
        public int PathLat { get => _PathLat; set { _PathLat = value; } }
        /// <summary>
        /// Final heading start point, longitude.
        /// OriginName: path_lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field PathLonField = new Field.Builder()
            .Name(nameof(PathLon))
            .Title("path_lon")
            .Description("Final heading start point, longitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _PathLon;
        public int PathLon { get => _PathLon; set { _PathLon = value; } }
        /// <summary>
        /// Arc entry point, latitude.
        /// OriginName: arc_entry_lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field ArcEntryLatField = new Field.Builder()
            .Name(nameof(ArcEntryLat))
            .Title("arc_entry_lat")
            .Description("Arc entry point, latitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _ArcEntryLat;
        public int ArcEntryLat { get => _ArcEntryLat; set { _ArcEntryLat = value; } }
        /// <summary>
        /// Arc entry point, longitude.
        /// OriginName: arc_entry_lon, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field ArcEntryLonField = new Field.Builder()
            .Name(nameof(ArcEntryLon))
            .Title("arc_entry_lon")
            .Description("Arc entry point, longitude.")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _ArcEntryLon;
        public int ArcEntryLon { get => _ArcEntryLon; set { _ArcEntryLon = value; } }
        /// <summary>
        /// Altitude.
        /// OriginName: altitude, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field AltitudeField = new Field.Builder()
            .Name(nameof(Altitude))
            .Title("altitude")
            .Description("Altitude.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Altitude;
        public float Altitude { get => _Altitude; set { _Altitude = value; } }
        /// <summary>
        /// Distance the aircraft expects to travel during the deepstall.
        /// OriginName: expected_travel_distance, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field ExpectedTravelDistanceField = new Field.Builder()
            .Name(nameof(ExpectedTravelDistance))
            .Title("expected_travel_distance")
            .Description("Distance the aircraft expects to travel during the deepstall.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _ExpectedTravelDistance;
        public float ExpectedTravelDistance { get => _ExpectedTravelDistance; set { _ExpectedTravelDistance = value; } }
        /// <summary>
        /// Deepstall cross track error (only valid when in DEEPSTALL_STAGE_LAND).
        /// OriginName: cross_track_error, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field CrossTrackErrorField = new Field.Builder()
            .Name(nameof(CrossTrackError))
            .Title("cross_track_error")
            .Description("Deepstall cross track error (only valid when in DEEPSTALL_STAGE_LAND).")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _CrossTrackError;
        public float CrossTrackError { get => _CrossTrackError; set { _CrossTrackError = value; } }
        /// <summary>
        /// Deepstall stage.
        /// OriginName: stage, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StageField = new Field.Builder()
            .Name(nameof(Stage))
            .Title("stage")
            .Description("Deepstall stage.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public DeepstallStage _Stage;
        public DeepstallStage Stage { get => _Stage; set => _Stage = value; } 
    }
    /// <summary>
    /// 3 axis gimbal measurements.
    ///  GIMBAL_REPORT
    /// </summary>
    public class GimbalReportPacket : MavlinkV2Message<GimbalReportPayload>
    {
        public const int MessageId = 200;
        
        public const byte CrcExtra = 134;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override GimbalReportPayload Payload { get; } = new();

        public override string Name => "GIMBAL_REPORT";
    }

    /// <summary>
    ///  GIMBAL_REPORT
    /// </summary>
    public class GimbalReportPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 42; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 42; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float delta_time
            +4 // float delta_angle_x
            +4 // float delta_angle_y
            +4 // float delta_angle_z
            +4 // float delta_velocity_x
            +4 // float delta_velocity_y
            +4 // float delta_velocity_z
            +4 // float joint_roll
            +4 // float joint_el
            +4 // float joint_az
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            DeltaTime = BinSerialize.ReadFloat(ref buffer);
            DeltaAngleX = BinSerialize.ReadFloat(ref buffer);
            DeltaAngleY = BinSerialize.ReadFloat(ref buffer);
            DeltaAngleZ = BinSerialize.ReadFloat(ref buffer);
            DeltaVelocityX = BinSerialize.ReadFloat(ref buffer);
            DeltaVelocityY = BinSerialize.ReadFloat(ref buffer);
            DeltaVelocityZ = BinSerialize.ReadFloat(ref buffer);
            JointRoll = BinSerialize.ReadFloat(ref buffer);
            JointEl = BinSerialize.ReadFloat(ref buffer);
            JointAz = BinSerialize.ReadFloat(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,DeltaTime);
            BinSerialize.WriteFloat(ref buffer,DeltaAngleX);
            BinSerialize.WriteFloat(ref buffer,DeltaAngleY);
            BinSerialize.WriteFloat(ref buffer,DeltaAngleZ);
            BinSerialize.WriteFloat(ref buffer,DeltaVelocityX);
            BinSerialize.WriteFloat(ref buffer,DeltaVelocityY);
            BinSerialize.WriteFloat(ref buffer,DeltaVelocityZ);
            BinSerialize.WriteFloat(ref buffer,JointRoll);
            BinSerialize.WriteFloat(ref buffer,JointEl);
            BinSerialize.WriteFloat(ref buffer,JointAz);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 42 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,DeltaTimeField, ref _DeltaTime);    
            FloatType.Accept(visitor,DeltaAngleXField, ref _DeltaAngleX);    
            FloatType.Accept(visitor,DeltaAngleYField, ref _DeltaAngleY);    
            FloatType.Accept(visitor,DeltaAngleZField, ref _DeltaAngleZ);    
            FloatType.Accept(visitor,DeltaVelocityXField, ref _DeltaVelocityX);    
            FloatType.Accept(visitor,DeltaVelocityYField, ref _DeltaVelocityY);    
            FloatType.Accept(visitor,DeltaVelocityZField, ref _DeltaVelocityZ);    
            FloatType.Accept(visitor,JointRollField, ref _JointRoll);    
            FloatType.Accept(visitor,JointElField, ref _JointEl);    
            FloatType.Accept(visitor,JointAzField, ref _JointAz);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// Time since last update.
        /// OriginName: delta_time, Units: s, IsExtended: false
        /// </summary>
        public static readonly Field DeltaTimeField = new Field.Builder()
            .Name(nameof(DeltaTime))
            .Title("delta_time")
            .Description("Time since last update.")
            .FormatString(string.Empty)
            .Units(@"s")
            .DataType(FloatType.Default)

            .Build();
        private float _DeltaTime;
        public float DeltaTime { get => _DeltaTime; set { _DeltaTime = value; } }
        /// <summary>
        /// Delta angle X.
        /// OriginName: delta_angle_x, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field DeltaAngleXField = new Field.Builder()
            .Name(nameof(DeltaAngleX))
            .Title("delta_angle_x")
            .Description("Delta angle X.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _DeltaAngleX;
        public float DeltaAngleX { get => _DeltaAngleX; set { _DeltaAngleX = value; } }
        /// <summary>
        /// Delta angle Y.
        /// OriginName: delta_angle_y, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field DeltaAngleYField = new Field.Builder()
            .Name(nameof(DeltaAngleY))
            .Title("delta_angle_y")
            .Description("Delta angle Y.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _DeltaAngleY;
        public float DeltaAngleY { get => _DeltaAngleY; set { _DeltaAngleY = value; } }
        /// <summary>
        /// Delta angle X.
        /// OriginName: delta_angle_z, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field DeltaAngleZField = new Field.Builder()
            .Name(nameof(DeltaAngleZ))
            .Title("delta_angle_z")
            .Description("Delta angle X.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _DeltaAngleZ;
        public float DeltaAngleZ { get => _DeltaAngleZ; set { _DeltaAngleZ = value; } }
        /// <summary>
        /// Delta velocity X.
        /// OriginName: delta_velocity_x, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field DeltaVelocityXField = new Field.Builder()
            .Name(nameof(DeltaVelocityX))
            .Title("delta_velocity_x")
            .Description("Delta velocity X.")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _DeltaVelocityX;
        public float DeltaVelocityX { get => _DeltaVelocityX; set { _DeltaVelocityX = value; } }
        /// <summary>
        /// Delta velocity Y.
        /// OriginName: delta_velocity_y, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field DeltaVelocityYField = new Field.Builder()
            .Name(nameof(DeltaVelocityY))
            .Title("delta_velocity_y")
            .Description("Delta velocity Y.")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _DeltaVelocityY;
        public float DeltaVelocityY { get => _DeltaVelocityY; set { _DeltaVelocityY = value; } }
        /// <summary>
        /// Delta velocity Z.
        /// OriginName: delta_velocity_z, Units: m/s, IsExtended: false
        /// </summary>
        public static readonly Field DeltaVelocityZField = new Field.Builder()
            .Name(nameof(DeltaVelocityZ))
            .Title("delta_velocity_z")
            .Description("Delta velocity Z.")
            .FormatString(string.Empty)
            .Units(@"m/s")
            .DataType(FloatType.Default)

            .Build();
        private float _DeltaVelocityZ;
        public float DeltaVelocityZ { get => _DeltaVelocityZ; set { _DeltaVelocityZ = value; } }
        /// <summary>
        /// Joint ROLL.
        /// OriginName: joint_roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field JointRollField = new Field.Builder()
            .Name(nameof(JointRoll))
            .Title("joint_roll")
            .Description("Joint ROLL.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _JointRoll;
        public float JointRoll { get => _JointRoll; set { _JointRoll = value; } }
        /// <summary>
        /// Joint EL.
        /// OriginName: joint_el, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field JointElField = new Field.Builder()
            .Name(nameof(JointEl))
            .Title("joint_el")
            .Description("Joint EL.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _JointEl;
        public float JointEl { get => _JointEl; set { _JointEl = value; } }
        /// <summary>
        /// Joint AZ.
        /// OriginName: joint_az, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field JointAzField = new Field.Builder()
            .Name(nameof(JointAz))
            .Title("joint_az")
            .Description("Joint AZ.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _JointAz;
        public float JointAz { get => _JointAz; set { _JointAz = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
    }
    /// <summary>
    /// Control message for rate gimbal.
    ///  GIMBAL_CONTROL
    /// </summary>
    public class GimbalControlPacket : MavlinkV2Message<GimbalControlPayload>
    {
        public const int MessageId = 201;
        
        public const byte CrcExtra = 205;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override GimbalControlPayload Payload { get; } = new();

        public override string Name => "GIMBAL_CONTROL";
    }

    /// <summary>
    ///  GIMBAL_CONTROL
    /// </summary>
    public class GimbalControlPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 14; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 14; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float demanded_rate_x
            +4 // float demanded_rate_y
            +4 // float demanded_rate_z
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            DemandedRateX = BinSerialize.ReadFloat(ref buffer);
            DemandedRateY = BinSerialize.ReadFloat(ref buffer);
            DemandedRateZ = BinSerialize.ReadFloat(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,DemandedRateX);
            BinSerialize.WriteFloat(ref buffer,DemandedRateY);
            BinSerialize.WriteFloat(ref buffer,DemandedRateZ);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 14 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,DemandedRateXField, ref _DemandedRateX);    
            FloatType.Accept(visitor,DemandedRateYField, ref _DemandedRateY);    
            FloatType.Accept(visitor,DemandedRateZField, ref _DemandedRateZ);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// Demanded angular rate X.
        /// OriginName: demanded_rate_x, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field DemandedRateXField = new Field.Builder()
            .Name(nameof(DemandedRateX))
            .Title("demanded_rate_x")
            .Description("Demanded angular rate X.")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _DemandedRateX;
        public float DemandedRateX { get => _DemandedRateX; set { _DemandedRateX = value; } }
        /// <summary>
        /// Demanded angular rate Y.
        /// OriginName: demanded_rate_y, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field DemandedRateYField = new Field.Builder()
            .Name(nameof(DemandedRateY))
            .Title("demanded_rate_y")
            .Description("Demanded angular rate Y.")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _DemandedRateY;
        public float DemandedRateY { get => _DemandedRateY; set { _DemandedRateY = value; } }
        /// <summary>
        /// Demanded angular rate Z.
        /// OriginName: demanded_rate_z, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field DemandedRateZField = new Field.Builder()
            .Name(nameof(DemandedRateZ))
            .Title("demanded_rate_z")
            .Description("Demanded angular rate Z.")
            .FormatString(string.Empty)
            .Units(@"rad/s")
            .DataType(FloatType.Default)

            .Build();
        private float _DemandedRateZ;
        public float DemandedRateZ { get => _DemandedRateZ; set { _DemandedRateZ = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
    }
    /// <summary>
    /// 100 Hz gimbal torque command telemetry.
    ///  GIMBAL_TORQUE_CMD_REPORT
    /// </summary>
    public class GimbalTorqueCmdReportPacket : MavlinkV2Message<GimbalTorqueCmdReportPayload>
    {
        public const int MessageId = 214;
        
        public const byte CrcExtra = 69;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override GimbalTorqueCmdReportPayload Payload { get; } = new();

        public override string Name => "GIMBAL_TORQUE_CMD_REPORT";
    }

    /// <summary>
    ///  GIMBAL_TORQUE_CMD_REPORT
    /// </summary>
    public class GimbalTorqueCmdReportPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // int16_t rl_torque_cmd
            +2 // int16_t el_torque_cmd
            +2 // int16_t az_torque_cmd
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RlTorqueCmd = BinSerialize.ReadShort(ref buffer);
            ElTorqueCmd = BinSerialize.ReadShort(ref buffer);
            AzTorqueCmd = BinSerialize.ReadShort(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteShort(ref buffer,RlTorqueCmd);
            BinSerialize.WriteShort(ref buffer,ElTorqueCmd);
            BinSerialize.WriteShort(ref buffer,AzTorqueCmd);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            /* PayloadByteSize = 8 */;
        }

        public void Visit(IVisitor visitor)
        {
            Int16Type.Accept(visitor,RlTorqueCmdField, ref _RlTorqueCmd);
            Int16Type.Accept(visitor,ElTorqueCmdField, ref _ElTorqueCmd);
            Int16Type.Accept(visitor,AzTorqueCmdField, ref _AzTorqueCmd);
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    

        }

        /// <summary>
        /// Roll Torque Command.
        /// OriginName: rl_torque_cmd, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RlTorqueCmdField = new Field.Builder()
            .Name(nameof(RlTorqueCmd))
            .Title("rl_torque_cmd")
            .Description("Roll Torque Command.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _RlTorqueCmd;
        public short RlTorqueCmd { get => _RlTorqueCmd; set { _RlTorqueCmd = value; } }
        /// <summary>
        /// Elevation Torque Command.
        /// OriginName: el_torque_cmd, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ElTorqueCmdField = new Field.Builder()
            .Name(nameof(ElTorqueCmd))
            .Title("el_torque_cmd")
            .Description("Elevation Torque Command.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _ElTorqueCmd;
        public short ElTorqueCmd { get => _ElTorqueCmd; set { _ElTorqueCmd = value; } }
        /// <summary>
        /// Azimuth Torque Command.
        /// OriginName: az_torque_cmd, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AzTorqueCmdField = new Field.Builder()
            .Name(nameof(AzTorqueCmd))
            .Title("az_torque_cmd")
            .Description("Azimuth Torque Command.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int16Type.Default)

            .Build();
        private short _AzTorqueCmd;
        public short AzTorqueCmd { get => _AzTorqueCmd; set { _AzTorqueCmd = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
    }
    /// <summary>
    /// Heartbeat from a HeroBus attached GoPro.
    ///  GOPRO_HEARTBEAT
    /// </summary>
    public class GoproHeartbeatPacket : MavlinkV2Message<GoproHeartbeatPayload>
    {
        public const int MessageId = 215;
        
        public const byte CrcExtra = 101;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override GoproHeartbeatPayload Payload { get; } = new();

        public override string Name => "GOPRO_HEARTBEAT";
    }

    /// <summary>
    ///  GOPRO_HEARTBEAT
    /// </summary>
    public class GoproHeartbeatPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 1 // uint8_t status
            + 1 // uint8_t capture_mode
            + 1 // uint8_t flags
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Status = (GoproHeartbeatStatus)BinSerialize.ReadByte(ref buffer);
            CaptureMode = (GoproCaptureMode)BinSerialize.ReadByte(ref buffer);
            Flags = (GoproHeartbeatFlags)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Status);
            BinSerialize.WriteByte(ref buffer,(byte)CaptureMode);
            BinSerialize.WriteByte(ref buffer,(byte)Flags);
            /* PayloadByteSize = 3 */;
        }

        public void Visit(IVisitor visitor)
        {
            var tmpStatus = (byte)Status;
            UInt8Type.Accept(visitor,StatusField, ref tmpStatus);
            Status = (GoproHeartbeatStatus)tmpStatus;
            var tmpCaptureMode = (byte)CaptureMode;
            UInt8Type.Accept(visitor,CaptureModeField, ref tmpCaptureMode);
            CaptureMode = (GoproCaptureMode)tmpCaptureMode;
            var tmpFlags = (byte)Flags;
            UInt8Type.Accept(visitor,FlagsField, ref tmpFlags);
            Flags = (GoproHeartbeatFlags)tmpFlags;

        }

        /// <summary>
        /// Status.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StatusField = new Field.Builder()
            .Name(nameof(Status))
            .Title("status")
            .Description("Status.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GoproHeartbeatStatus _Status;
        public GoproHeartbeatStatus Status { get => _Status; set => _Status = value; } 
        /// <summary>
        /// Current capture mode.
        /// OriginName: capture_mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CaptureModeField = new Field.Builder()
            .Name(nameof(CaptureMode))
            .Title("capture_mode")
            .Description("Current capture mode.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GoproCaptureMode _CaptureMode;
        public GoproCaptureMode CaptureMode { get => _CaptureMode; set => _CaptureMode = value; } 
        /// <summary>
        /// Additional status bits.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("bitmask")
            .Description("Additional status bits.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GoproHeartbeatFlags _Flags;
        public GoproHeartbeatFlags Flags { get => _Flags; set => _Flags = value; } 
    }
    /// <summary>
    /// Request a GOPRO_COMMAND response from the GoPro.
    ///  GOPRO_GET_REQUEST
    /// </summary>
    public class GoproGetRequestPacket : MavlinkV2Message<GoproGetRequestPayload>
    {
        public const int MessageId = 216;
        
        public const byte CrcExtra = 50;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override GoproGetRequestPayload Payload { get; } = new();

        public override string Name => "GOPRO_GET_REQUEST";
    }

    /// <summary>
    ///  GOPRO_GET_REQUEST
    /// </summary>
    public class GoproGetRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            + 1 // uint8_t cmd_id
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            CmdId = (GoproCommand)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)CmdId);
            /* PayloadByteSize = 3 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            var tmpCmdId = (byte)CmdId;
            UInt8Type.Accept(visitor,CmdIdField, ref tmpCmdId);
            CmdId = (GoproCommand)tmpCmdId;

        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Command ID.
        /// OriginName: cmd_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CmdIdField = new Field.Builder()
            .Name(nameof(CmdId))
            .Title("cmd_id")
            .Description("Command ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GoproCommand _CmdId;
        public GoproCommand CmdId { get => _CmdId; set => _CmdId = value; } 
    }
    /// <summary>
    /// Response from a GOPRO_COMMAND get request.
    ///  GOPRO_GET_RESPONSE
    /// </summary>
    public class GoproGetResponsePacket : MavlinkV2Message<GoproGetResponsePayload>
    {
        public const int MessageId = 217;
        
        public const byte CrcExtra = 202;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override GoproGetResponsePayload Payload { get; } = new();

        public override string Name => "GOPRO_GET_RESPONSE";
    }

    /// <summary>
    ///  GOPRO_GET_RESPONSE
    /// </summary>
    public class GoproGetResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 6; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 6; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 1 // uint8_t cmd_id
            + 1 // uint8_t status
            +Value.Length // uint8_t[4] value
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            CmdId = (GoproCommand)BinSerialize.ReadByte(ref buffer);
            Status = (GoproRequestStatus)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/6 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Value[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)CmdId);
            BinSerialize.WriteByte(ref buffer,(byte)Status);
            for(var i=0;i<Value.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Value[i]);
            }
            /* PayloadByteSize = 6 */;
        }

        public void Visit(IVisitor visitor)
        {
            var tmpCmdId = (byte)CmdId;
            UInt8Type.Accept(visitor,CmdIdField, ref tmpCmdId);
            CmdId = (GoproCommand)tmpCmdId;
            var tmpStatus = (byte)Status;
            UInt8Type.Accept(visitor,StatusField, ref tmpStatus);
            Status = (GoproRequestStatus)tmpStatus;
            ArrayType.Accept(visitor,ValueField, 4,
                (index,v) => UInt8Type.Accept(v, ValueField, ref Value[index]));    

        }

        /// <summary>
        /// Command ID.
        /// OriginName: cmd_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CmdIdField = new Field.Builder()
            .Name(nameof(CmdId))
            .Title("cmd_id")
            .Description("Command ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GoproCommand _CmdId;
        public GoproCommand CmdId { get => _CmdId; set => _CmdId = value; } 
        /// <summary>
        /// Status.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StatusField = new Field.Builder()
            .Name(nameof(Status))
            .Title("status")
            .Description("Status.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GoproRequestStatus _Status;
        public GoproRequestStatus Status { get => _Status; set => _Status = value; } 
        /// <summary>
        /// Value.
        /// OriginName: value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ValueField = new Field.Builder()
            .Name(nameof(Value))
            .Title("value")
            .Description("Value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,4))

            .Build();
        public const int ValueMaxItemsCount = 4;
        public byte[] Value { get; } = new byte[4];
        [Obsolete("This method is deprecated. Use GetValueMaxItemsCount instead.")]
        public byte GetValueMaxItemsCount() => 4;
    }
    /// <summary>
    /// Request to set a GOPRO_COMMAND with a desired.
    ///  GOPRO_SET_REQUEST
    /// </summary>
    public class GoproSetRequestPacket : MavlinkV2Message<GoproSetRequestPayload>
    {
        public const int MessageId = 218;
        
        public const byte CrcExtra = 17;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override GoproSetRequestPayload Payload { get; } = new();

        public override string Name => "GOPRO_SET_REQUEST";
    }

    /// <summary>
    ///  GOPRO_SET_REQUEST
    /// </summary>
    public class GoproSetRequestPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 7; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 7; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            + 1 // uint8_t cmd_id
            +Value.Length // uint8_t[4] value
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            CmdId = (GoproCommand)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/7 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Value[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)CmdId);
            for(var i=0;i<Value.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Value[i]);
            }
            /* PayloadByteSize = 7 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            var tmpCmdId = (byte)CmdId;
            UInt8Type.Accept(visitor,CmdIdField, ref tmpCmdId);
            CmdId = (GoproCommand)tmpCmdId;
            ArrayType.Accept(visitor,ValueField, 4,
                (index,v) => UInt8Type.Accept(v, ValueField, ref Value[index]));    

        }

        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// Command ID.
        /// OriginName: cmd_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CmdIdField = new Field.Builder()
            .Name(nameof(CmdId))
            .Title("cmd_id")
            .Description("Command ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GoproCommand _CmdId;
        public GoproCommand CmdId { get => _CmdId; set => _CmdId = value; } 
        /// <summary>
        /// Value.
        /// OriginName: value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ValueField = new Field.Builder()
            .Name(nameof(Value))
            .Title("value")
            .Description("Value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,4))

            .Build();
        public const int ValueMaxItemsCount = 4;
        public byte[] Value { get; } = new byte[4];
        [Obsolete("This method is deprecated. Use GetValueMaxItemsCount instead.")]
        public byte GetValueMaxItemsCount() => 4;
    }
    /// <summary>
    /// Response from a GOPRO_COMMAND set request.
    ///  GOPRO_SET_RESPONSE
    /// </summary>
    public class GoproSetResponsePacket : MavlinkV2Message<GoproSetResponsePayload>
    {
        public const int MessageId = 219;
        
        public const byte CrcExtra = 162;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override GoproSetResponsePayload Payload { get; } = new();

        public override string Name => "GOPRO_SET_RESPONSE";
    }

    /// <summary>
    ///  GOPRO_SET_RESPONSE
    /// </summary>
    public class GoproSetResponsePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 2; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 2; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 1 // uint8_t cmd_id
            + 1 // uint8_t status
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            CmdId = (GoproCommand)BinSerialize.ReadByte(ref buffer);
            Status = (GoproRequestStatus)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)CmdId);
            BinSerialize.WriteByte(ref buffer,(byte)Status);
            /* PayloadByteSize = 2 */;
        }

        public void Visit(IVisitor visitor)
        {
            var tmpCmdId = (byte)CmdId;
            UInt8Type.Accept(visitor,CmdIdField, ref tmpCmdId);
            CmdId = (GoproCommand)tmpCmdId;
            var tmpStatus = (byte)Status;
            UInt8Type.Accept(visitor,StatusField, ref tmpStatus);
            Status = (GoproRequestStatus)tmpStatus;

        }

        /// <summary>
        /// Command ID.
        /// OriginName: cmd_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CmdIdField = new Field.Builder()
            .Name(nameof(CmdId))
            .Title("cmd_id")
            .Description("Command ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GoproCommand _CmdId;
        public GoproCommand CmdId { get => _CmdId; set => _CmdId = value; } 
        /// <summary>
        /// Status.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StatusField = new Field.Builder()
            .Name(nameof(Status))
            .Title("status")
            .Description("Status.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public GoproRequestStatus _Status;
        public GoproRequestStatus Status { get => _Status; set => _Status = value; } 
    }
    /// <summary>
    /// RPM sensor output.
    ///  RPM
    /// </summary>
    public class RpmPacket : MavlinkV2Message<RpmPayload>
    {
        public const int MessageId = 226;
        
        public const byte CrcExtra = 207;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override RpmPayload Payload { get; } = new();

        public override string Name => "RPM";
    }

    /// <summary>
    ///  RPM
    /// </summary>
    public class RpmPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float rpm1
            +4 // float rpm2
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Rpm1 = BinSerialize.ReadFloat(ref buffer);
            Rpm2 = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Rpm1);
            BinSerialize.WriteFloat(ref buffer,Rpm2);
            /* PayloadByteSize = 8 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,Rpm1Field, ref _Rpm1);    
            FloatType.Accept(visitor,Rpm2Field, ref _Rpm2);    

        }

        /// <summary>
        /// RPM Sensor1.
        /// OriginName: rpm1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Rpm1Field = new Field.Builder()
            .Name(nameof(Rpm1))
            .Title("rpm1")
            .Description("RPM Sensor1.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Rpm1;
        public float Rpm1 { get => _Rpm1; set { _Rpm1 = value; } }
        /// <summary>
        /// RPM Sensor2.
        /// OriginName: rpm2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Rpm2Field = new Field.Builder()
            .Name(nameof(Rpm2))
            .Title("rpm2")
            .Description("RPM Sensor2.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Rpm2;
        public float Rpm2 { get => _Rpm2; set { _Rpm2 = value; } }
    }
    /// <summary>
    /// Read registers for a device.
    ///  DEVICE_OP_READ
    /// </summary>
    public class DeviceOpReadPacket : MavlinkV2Message<DeviceOpReadPayload>
    {
        public const int MessageId = 11000;
        
        public const byte CrcExtra = 134;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override DeviceOpReadPayload Payload { get; } = new();

        public override string Name => "DEVICE_OP_READ";
    }

    /// <summary>
    ///  DEVICE_OP_READ
    /// </summary>
    public class DeviceOpReadPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 52; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 52; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t request_id
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            + 1 // uint8_t bustype
            +1 // uint8_t bus
            +1 // uint8_t address
            +Busname.Length // char[40] busname
            +1 // uint8_t regstart
            +1 // uint8_t count
            +1 // uint8_t bank
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUInt(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Bustype = (DeviceOpBustype)BinSerialize.ReadByte(ref buffer);
            Bus = (byte)BinSerialize.ReadByte(ref buffer);
            Address = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/40 - Math.Max(0,((/*PayloadByteSize*/52 - payloadSize - /*ExtendedFieldsLength*/1)/1 /*FieldTypeByteSize*/));
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Busname)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Busname.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            Regstart = (byte)BinSerialize.ReadByte(ref buffer);
            Count = (byte)BinSerialize.ReadByte(ref buffer);
            // extended field 'Bank' can be empty
            if (buffer.IsEmpty) return;
            Bank = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Bustype);
            BinSerialize.WriteByte(ref buffer,(byte)Bus);
            BinSerialize.WriteByte(ref buffer,(byte)Address);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Busname)
                {
                    Encoding.ASCII.GetBytes(charPointer, Busname.Length, bytePointer, Busname.Length);
                }
            }
            buffer = buffer.Slice(Busname.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)Regstart);
            BinSerialize.WriteByte(ref buffer,(byte)Count);
            BinSerialize.WriteByte(ref buffer,(byte)Bank);
            /* PayloadByteSize = 52 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            var tmpBustype = (byte)Bustype;
            UInt8Type.Accept(visitor,BustypeField, ref tmpBustype);
            Bustype = (DeviceOpBustype)tmpBustype;
            UInt8Type.Accept(visitor,BusField, ref _Bus);    
            UInt8Type.Accept(visitor,AddressField, ref _Address);    
            ArrayType.Accept(visitor,BusnameField, 40, (index,v) =>
            {
                var tmp = (byte)Busname[index];
                UInt8Type.Accept(v,BusnameField, ref tmp);
                Busname[index] = (char)tmp;
            });
            UInt8Type.Accept(visitor,RegstartField, ref _Regstart);    
            UInt8Type.Accept(visitor,CountField, ref _Count);    
            UInt8Type.Accept(visitor,BankField, ref _Bank);    

        }

        /// <summary>
        /// Request ID - copied to reply.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Request ID - copied to reply.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _RequestId;
        public uint RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// The bus type.
        /// OriginName: bustype, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BustypeField = new Field.Builder()
            .Name(nameof(Bustype))
            .Title("bustype")
            .Description("The bus type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public DeviceOpBustype _Bustype;
        public DeviceOpBustype Bustype { get => _Bustype; set => _Bustype = value; } 
        /// <summary>
        /// Bus number.
        /// OriginName: bus, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BusField = new Field.Builder()
            .Name(nameof(Bus))
            .Title("bus")
            .Description("Bus number.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Bus;
        public byte Bus { get => _Bus; set { _Bus = value; } }
        /// <summary>
        /// Bus address.
        /// OriginName: address, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AddressField = new Field.Builder()
            .Name(nameof(Address))
            .Title("address")
            .Description("Bus address.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Address;
        public byte Address { get => _Address; set { _Address = value; } }
        /// <summary>
        /// Name of device on bus (for SPI).
        /// OriginName: busname, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BusnameField = new Field.Builder()
            .Name(nameof(Busname))
            .Title("busname")
            .Description("Name of device on bus (for SPI).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,40))

            .Build();
        public const int BusnameMaxItemsCount = 40;
        public char[] Busname { get; } = new char[40];
        [Obsolete("This method is deprecated. Use GetBusnameMaxItemsCount instead.")]
        public byte GetBusnameMaxItemsCount() => 40;
        /// <summary>
        /// First register to read.
        /// OriginName: regstart, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RegstartField = new Field.Builder()
            .Name(nameof(Regstart))
            .Title("regstart")
            .Description("First register to read.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Regstart;
        public byte Regstart { get => _Regstart; set { _Regstart = value; } }
        /// <summary>
        /// Count of registers to read.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Count of registers to read.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Count;
        public byte Count { get => _Count; set { _Count = value; } }
        /// <summary>
        /// Bank number.
        /// OriginName: bank, Units: , IsExtended: true
        /// </summary>
        public static readonly Field BankField = new Field.Builder()
            .Name(nameof(Bank))
            .Title("bank")
            .Description("Bank number.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Bank;
        public byte Bank { get => _Bank; set { _Bank = value; } }
    }
    /// <summary>
    /// Read registers reply.
    ///  DEVICE_OP_READ_REPLY
    /// </summary>
    public class DeviceOpReadReplyPacket : MavlinkV2Message<DeviceOpReadReplyPayload>
    {
        public const int MessageId = 11001;
        
        public const byte CrcExtra = 15;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override DeviceOpReadReplyPayload Payload { get; } = new();

        public override string Name => "DEVICE_OP_READ_REPLY";
    }

    /// <summary>
    ///  DEVICE_OP_READ_REPLY
    /// </summary>
    public class DeviceOpReadReplyPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 136; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 136; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t request_id
            +1 // uint8_t result
            +1 // uint8_t regstart
            +1 // uint8_t count
            +Data.Length // uint8_t[128] data
            +1 // uint8_t bank
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUInt(ref buffer);
            Result = (byte)BinSerialize.ReadByte(ref buffer);
            Regstart = (byte)BinSerialize.ReadByte(ref buffer);
            Count = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/128 - Math.Max(0,((/*PayloadByteSize*/136 - payloadSize - /*ExtendedFieldsLength*/1)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            // extended field 'Bank' can be empty
            if (buffer.IsEmpty) return;
            Bank = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            BinSerialize.WriteByte(ref buffer,(byte)Regstart);
            BinSerialize.WriteByte(ref buffer,(byte)Count);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)Bank);
            /* PayloadByteSize = 136 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt8Type.Accept(visitor,ResultField, ref _Result);    
            UInt8Type.Accept(visitor,RegstartField, ref _Regstart);    
            UInt8Type.Accept(visitor,CountField, ref _Count);    
            ArrayType.Accept(visitor,DataField, 128,
                (index,v) => UInt8Type.Accept(v, DataField, ref Data[index]));    
            UInt8Type.Accept(visitor,BankField, ref _Bank);    

        }

        /// <summary>
        /// Request ID - copied from request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Request ID - copied from request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _RequestId;
        public uint RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// 0 for success, anything else is failure code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("0 for success, anything else is failure code.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Result;
        public byte Result { get => _Result; set { _Result = value; } }
        /// <summary>
        /// Starting register.
        /// OriginName: regstart, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RegstartField = new Field.Builder()
            .Name(nameof(Regstart))
            .Title("regstart")
            .Description("Starting register.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Regstart;
        public byte Regstart { get => _Regstart; set { _Regstart = value; } }
        /// <summary>
        /// Count of bytes read.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Count of bytes read.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Count;
        public byte Count { get => _Count; set { _Count = value; } }
        /// <summary>
        /// Reply data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Reply data.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,128))

            .Build();
        public const int DataMaxItemsCount = 128;
        public byte[] Data { get; } = new byte[128];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 128;
        /// <summary>
        /// Bank number.
        /// OriginName: bank, Units: , IsExtended: true
        /// </summary>
        public static readonly Field BankField = new Field.Builder()
            .Name(nameof(Bank))
            .Title("bank")
            .Description("Bank number.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Bank;
        public byte Bank { get => _Bank; set { _Bank = value; } }
    }
    /// <summary>
    /// Write registers for a device.
    ///  DEVICE_OP_WRITE
    /// </summary>
    public class DeviceOpWritePacket : MavlinkV2Message<DeviceOpWritePayload>
    {
        public const int MessageId = 11002;
        
        public const byte CrcExtra = 234;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override DeviceOpWritePayload Payload { get; } = new();

        public override string Name => "DEVICE_OP_WRITE";
    }

    /// <summary>
    ///  DEVICE_OP_WRITE
    /// </summary>
    public class DeviceOpWritePayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 180; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 180; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t request_id
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            + 1 // uint8_t bustype
            +1 // uint8_t bus
            +1 // uint8_t address
            +Busname.Length // char[40] busname
            +1 // uint8_t regstart
            +1 // uint8_t count
            +Data.Length // uint8_t[128] data
            +1 // uint8_t bank
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUInt(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            Bustype = (DeviceOpBustype)BinSerialize.ReadByte(ref buffer);
            Bus = (byte)BinSerialize.ReadByte(ref buffer);
            Address = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = 40;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Busname)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, Busname.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            Regstart = (byte)BinSerialize.ReadByte(ref buffer);
            Count = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/128 - Math.Max(0,((/*PayloadByteSize*/180 - payloadSize - /*ExtendedFieldsLength*/1)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Data[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            // extended field 'Bank' can be empty
            if (buffer.IsEmpty) return;
            Bank = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)Bustype);
            BinSerialize.WriteByte(ref buffer,(byte)Bus);
            BinSerialize.WriteByte(ref buffer,(byte)Address);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = Busname)
                {
                    Encoding.ASCII.GetBytes(charPointer, Busname.Length, bytePointer, Busname.Length);
                }
            }
            buffer = buffer.Slice(Busname.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)Regstart);
            BinSerialize.WriteByte(ref buffer,(byte)Count);
            for(var i=0;i<Data.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Data[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)Bank);
            /* PayloadByteSize = 180 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            var tmpBustype = (byte)Bustype;
            UInt8Type.Accept(visitor,BustypeField, ref tmpBustype);
            Bustype = (DeviceOpBustype)tmpBustype;
            UInt8Type.Accept(visitor,BusField, ref _Bus);    
            UInt8Type.Accept(visitor,AddressField, ref _Address);    
            ArrayType.Accept(visitor,BusnameField, 40, (index,v) =>
            {
                var tmp = (byte)Busname[index];
                UInt8Type.Accept(v,BusnameField, ref tmp);
                Busname[index] = (char)tmp;
            });
            UInt8Type.Accept(visitor,RegstartField, ref _Regstart);    
            UInt8Type.Accept(visitor,CountField, ref _Count);    
            ArrayType.Accept(visitor,DataField, 128,
                (index,v) => UInt8Type.Accept(v, DataField, ref Data[index]));    
            UInt8Type.Accept(visitor,BankField, ref _Bank);    

        }

        /// <summary>
        /// Request ID - copied to reply.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Request ID - copied to reply.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _RequestId;
        public uint RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// The bus type.
        /// OriginName: bustype, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BustypeField = new Field.Builder()
            .Name(nameof(Bustype))
            .Title("bustype")
            .Description("The bus type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public DeviceOpBustype _Bustype;
        public DeviceOpBustype Bustype { get => _Bustype; set => _Bustype = value; } 
        /// <summary>
        /// Bus number.
        /// OriginName: bus, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BusField = new Field.Builder()
            .Name(nameof(Bus))
            .Title("bus")
            .Description("Bus number.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Bus;
        public byte Bus { get => _Bus; set { _Bus = value; } }
        /// <summary>
        /// Bus address.
        /// OriginName: address, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AddressField = new Field.Builder()
            .Name(nameof(Address))
            .Title("address")
            .Description("Bus address.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Address;
        public byte Address { get => _Address; set { _Address = value; } }
        /// <summary>
        /// Name of device on bus (for SPI).
        /// OriginName: busname, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BusnameField = new Field.Builder()
            .Name(nameof(Busname))
            .Title("busname")
            .Description("Name of device on bus (for SPI).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,40))

            .Build();
        public const int BusnameMaxItemsCount = 40;
        public char[] Busname { get; } = new char[40];
        /// <summary>
        /// First register to write.
        /// OriginName: regstart, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RegstartField = new Field.Builder()
            .Name(nameof(Regstart))
            .Title("regstart")
            .Description("First register to write.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Regstart;
        public byte Regstart { get => _Regstart; set { _Regstart = value; } }
        /// <summary>
        /// Count of registers to write.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Count of registers to write.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Count;
        public byte Count { get => _Count; set { _Count = value; } }
        /// <summary>
        /// Write data.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DataField = new Field.Builder()
            .Name(nameof(Data))
            .Title("data")
            .Description("Write data.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,128))

            .Build();
        public const int DataMaxItemsCount = 128;
        public byte[] Data { get; } = new byte[128];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 128;
        /// <summary>
        /// Bank number.
        /// OriginName: bank, Units: , IsExtended: true
        /// </summary>
        public static readonly Field BankField = new Field.Builder()
            .Name(nameof(Bank))
            .Title("bank")
            .Description("Bank number.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Bank;
        public byte Bank { get => _Bank; set { _Bank = value; } }
    }
    /// <summary>
    /// Write registers reply.
    ///  DEVICE_OP_WRITE_REPLY
    /// </summary>
    public class DeviceOpWriteReplyPacket : MavlinkV2Message<DeviceOpWriteReplyPayload>
    {
        public const int MessageId = 11003;
        
        public const byte CrcExtra = 64;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override DeviceOpWriteReplyPayload Payload { get; } = new();

        public override string Name => "DEVICE_OP_WRITE_REPLY";
    }

    /// <summary>
    ///  DEVICE_OP_WRITE_REPLY
    /// </summary>
    public class DeviceOpWriteReplyPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 5; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 5; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t request_id
            +1 // uint8_t result
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUInt(ref buffer);
            Result = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 5 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt8Type.Accept(visitor,ResultField, ref _Result);    

        }

        /// <summary>
        /// Request ID - copied from request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Request ID - copied from request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _RequestId;
        public uint RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// 0 for success, anything else is failure code.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("0 for success, anything else is failure code.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Result;
        public byte Result { get => _Result; set { _Result = value; } }
    }
    /// <summary>
    /// Adaptive Controller tuning information.
    ///  ADAP_TUNING
    /// </summary>
    public class AdapTuningPacket : MavlinkV2Message<AdapTuningPayload>
    {
        public const int MessageId = 11010;
        
        public const byte CrcExtra = 46;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AdapTuningPayload Payload { get; } = new();

        public override string Name => "ADAP_TUNING";
    }

    /// <summary>
    ///  ADAP_TUNING
    /// </summary>
    public class AdapTuningPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 49; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 49; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float desired
            +4 // float achieved
            +4 // float error
            +4 // float theta
            +4 // float omega
            +4 // float sigma
            +4 // float theta_dot
            +4 // float omega_dot
            +4 // float sigma_dot
            +4 // float f
            +4 // float f_dot
            +4 // float u
            + 1 // uint8_t axis
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Desired = BinSerialize.ReadFloat(ref buffer);
            Achieved = BinSerialize.ReadFloat(ref buffer);
            Error = BinSerialize.ReadFloat(ref buffer);
            Theta = BinSerialize.ReadFloat(ref buffer);
            Omega = BinSerialize.ReadFloat(ref buffer);
            Sigma = BinSerialize.ReadFloat(ref buffer);
            ThetaDot = BinSerialize.ReadFloat(ref buffer);
            OmegaDot = BinSerialize.ReadFloat(ref buffer);
            SigmaDot = BinSerialize.ReadFloat(ref buffer);
            F = BinSerialize.ReadFloat(ref buffer);
            FDot = BinSerialize.ReadFloat(ref buffer);
            U = BinSerialize.ReadFloat(ref buffer);
            Axis = (PidTuningAxis)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Desired);
            BinSerialize.WriteFloat(ref buffer,Achieved);
            BinSerialize.WriteFloat(ref buffer,Error);
            BinSerialize.WriteFloat(ref buffer,Theta);
            BinSerialize.WriteFloat(ref buffer,Omega);
            BinSerialize.WriteFloat(ref buffer,Sigma);
            BinSerialize.WriteFloat(ref buffer,ThetaDot);
            BinSerialize.WriteFloat(ref buffer,OmegaDot);
            BinSerialize.WriteFloat(ref buffer,SigmaDot);
            BinSerialize.WriteFloat(ref buffer,F);
            BinSerialize.WriteFloat(ref buffer,FDot);
            BinSerialize.WriteFloat(ref buffer,U);
            BinSerialize.WriteByte(ref buffer,(byte)Axis);
            /* PayloadByteSize = 49 */;
        }

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,DesiredField, ref _Desired);    
            FloatType.Accept(visitor,AchievedField, ref _Achieved);    
            FloatType.Accept(visitor,ErrorField, ref _Error);    
            FloatType.Accept(visitor,ThetaField, ref _Theta);    
            FloatType.Accept(visitor,OmegaField, ref _Omega);    
            FloatType.Accept(visitor,SigmaField, ref _Sigma);    
            FloatType.Accept(visitor,ThetaDotField, ref _ThetaDot);    
            FloatType.Accept(visitor,OmegaDotField, ref _OmegaDot);    
            FloatType.Accept(visitor,SigmaDotField, ref _SigmaDot);    
            FloatType.Accept(visitor,FField, ref _F);    
            FloatType.Accept(visitor,FDotField, ref _FDot);    
            FloatType.Accept(visitor,UField, ref _U);    
            var tmpAxis = (byte)Axis;
            UInt8Type.Accept(visitor,AxisField, ref tmpAxis);
            Axis = (PidTuningAxis)tmpAxis;

        }

        /// <summary>
        /// Desired rate.
        /// OriginName: desired, Units: deg/s, IsExtended: false
        /// </summary>
        public static readonly Field DesiredField = new Field.Builder()
            .Name(nameof(Desired))
            .Title("desired")
            .Description("Desired rate.")
            .FormatString(string.Empty)
            .Units(@"deg/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Desired;
        public float Desired { get => _Desired; set { _Desired = value; } }
        /// <summary>
        /// Achieved rate.
        /// OriginName: achieved, Units: deg/s, IsExtended: false
        /// </summary>
        public static readonly Field AchievedField = new Field.Builder()
            .Name(nameof(Achieved))
            .Title("achieved")
            .Description("Achieved rate.")
            .FormatString(string.Empty)
            .Units(@"deg/s")
            .DataType(FloatType.Default)

            .Build();
        private float _Achieved;
        public float Achieved { get => _Achieved; set { _Achieved = value; } }
        /// <summary>
        /// Error between model and vehicle.
        /// OriginName: error, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ErrorField = new Field.Builder()
            .Name(nameof(Error))
            .Title("error")
            .Description("Error between model and vehicle.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Error;
        public float Error { get => _Error; set { _Error = value; } }
        /// <summary>
        /// Theta estimated state predictor.
        /// OriginName: theta, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ThetaField = new Field.Builder()
            .Name(nameof(Theta))
            .Title("theta")
            .Description("Theta estimated state predictor.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Theta;
        public float Theta { get => _Theta; set { _Theta = value; } }
        /// <summary>
        /// Omega estimated state predictor.
        /// OriginName: omega, Units: , IsExtended: false
        /// </summary>
        public static readonly Field OmegaField = new Field.Builder()
            .Name(nameof(Omega))
            .Title("omega")
            .Description("Omega estimated state predictor.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Omega;
        public float Omega { get => _Omega; set { _Omega = value; } }
        /// <summary>
        /// Sigma estimated state predictor.
        /// OriginName: sigma, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SigmaField = new Field.Builder()
            .Name(nameof(Sigma))
            .Title("sigma")
            .Description("Sigma estimated state predictor.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Sigma;
        public float Sigma { get => _Sigma; set { _Sigma = value; } }
        /// <summary>
        /// Theta derivative.
        /// OriginName: theta_dot, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ThetaDotField = new Field.Builder()
            .Name(nameof(ThetaDot))
            .Title("theta_dot")
            .Description("Theta derivative.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _ThetaDot;
        public float ThetaDot { get => _ThetaDot; set { _ThetaDot = value; } }
        /// <summary>
        /// Omega derivative.
        /// OriginName: omega_dot, Units: , IsExtended: false
        /// </summary>
        public static readonly Field OmegaDotField = new Field.Builder()
            .Name(nameof(OmegaDot))
            .Title("omega_dot")
            .Description("Omega derivative.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _OmegaDot;
        public float OmegaDot { get => _OmegaDot; set { _OmegaDot = value; } }
        /// <summary>
        /// Sigma derivative.
        /// OriginName: sigma_dot, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SigmaDotField = new Field.Builder()
            .Name(nameof(SigmaDot))
            .Title("sigma_dot")
            .Description("Sigma derivative.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _SigmaDot;
        public float SigmaDot { get => _SigmaDot; set { _SigmaDot = value; } }
        /// <summary>
        /// Projection operator value.
        /// OriginName: f, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FField = new Field.Builder()
            .Name(nameof(F))
            .Title("f")
            .Description("Projection operator value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _F;
        public float F { get => _F; set { _F = value; } }
        /// <summary>
        /// Projection operator derivative.
        /// OriginName: f_dot, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FDotField = new Field.Builder()
            .Name(nameof(FDot))
            .Title("f_dot")
            .Description("Projection operator derivative.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _FDot;
        public float FDot { get => _FDot; set { _FDot = value; } }
        /// <summary>
        /// u adaptive controlled output command.
        /// OriginName: u, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UField = new Field.Builder()
            .Name(nameof(U))
            .Title("u")
            .Description("u adaptive controlled output command.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _U;
        public float U { get => _U; set { _U = value; } }
        /// <summary>
        /// Axis.
        /// OriginName: axis, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AxisField = new Field.Builder()
            .Name(nameof(Axis))
            .Title("axis")
            .Description("Axis.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public PidTuningAxis _Axis;
        public PidTuningAxis Axis { get => _Axis; set => _Axis = value; } 
    }
    /// <summary>
    /// Camera vision based attitude and position deltas.
    ///  VISION_POSITION_DELTA
    /// </summary>
    public class VisionPositionDeltaPacket : MavlinkV2Message<VisionPositionDeltaPayload>
    {
        public const int MessageId = 11011;
        
        public const byte CrcExtra = 106;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override VisionPositionDeltaPayload Payload { get; } = new();

        public override string Name => "VISION_POSITION_DELTA";
    }

    /// <summary>
    ///  VISION_POSITION_DELTA
    /// </summary>
    public class VisionPositionDeltaPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 44; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_usec
            +8 // uint64_t time_delta_usec
            +AngleDelta.Length * 4 // float[3] angle_delta
            +PositionDelta.Length * 4 // float[3] position_delta
            +4 // float confidence
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUsec = BinSerialize.ReadULong(ref buffer);
            TimeDeltaUsec = BinSerialize.ReadULong(ref buffer);
            arraySize = /*ArrayLength*/3 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                AngleDelta[i] = BinSerialize.ReadFloat(ref buffer);
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                PositionDelta[i] = BinSerialize.ReadFloat(ref buffer);
            }
            Confidence = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUsec);
            BinSerialize.WriteULong(ref buffer,TimeDeltaUsec);
            for(var i=0;i<AngleDelta.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,AngleDelta[i]);
            }
            for(var i=0;i<PositionDelta.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,PositionDelta[i]);
            }
            BinSerialize.WriteFloat(ref buffer,Confidence);
            /* PayloadByteSize = 44 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUsecField, ref _TimeUsec);    
            UInt64Type.Accept(visitor,TimeDeltaUsecField, ref _TimeDeltaUsec);    
            ArrayType.Accept(visitor,AngleDeltaField, 3,
                (index,v) => FloatType.Accept(v, AngleDeltaField, ref AngleDelta[index]));
            ArrayType.Accept(visitor,PositionDeltaField, 3,
                (index,v) => FloatType.Accept(v, PositionDeltaField, ref PositionDelta[index]));
            FloatType.Accept(visitor,ConfidenceField, ref _Confidence);    

        }

        /// <summary>
        /// Timestamp (synced to UNIX time or since system boot).
        /// OriginName: time_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUsecField = new Field.Builder()
            .Name(nameof(TimeUsec))
            .Title("time_usec")
            .Description("Timestamp (synced to UNIX time or since system boot).")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TimeUsec;
        public ulong TimeUsec { get => _TimeUsec; set { _TimeUsec = value; } }
        /// <summary>
        /// Time since the last reported camera frame.
        /// OriginName: time_delta_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeDeltaUsecField = new Field.Builder()
            .Name(nameof(TimeDeltaUsec))
            .Title("time_delta_usec")
            .Description("Time since the last reported camera frame.")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TimeDeltaUsec;
        public ulong TimeDeltaUsec { get => _TimeDeltaUsec; set { _TimeDeltaUsec = value; } }
        /// <summary>
        /// Defines a rotation vector [roll, pitch, yaw] to the current MAV_FRAME_BODY_FRD from the previous MAV_FRAME_BODY_FRD.
        /// OriginName: angle_delta, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field AngleDeltaField = new Field.Builder()
            .Name(nameof(AngleDelta))
            .Title("angle_delta")
            .Description("Defines a rotation vector [roll, pitch, yaw] to the current MAV_FRAME_BODY_FRD from the previous MAV_FRAME_BODY_FRD.")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(new ArrayType(FloatType.Default,3))        

            .Build();
        public const int AngleDeltaMaxItemsCount = 3;
        public float[] AngleDelta { get; } = new float[3];
        [Obsolete("This method is deprecated. Use GetAngleDeltaMaxItemsCount instead.")]
        public byte GetAngleDeltaMaxItemsCount() => 3;
        /// <summary>
        /// Change in position to the current MAV_FRAME_BODY_FRD from the previous FRAME_BODY_FRD rotated to the current MAV_FRAME_BODY_FRD.
        /// OriginName: position_delta, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field PositionDeltaField = new Field.Builder()
            .Name(nameof(PositionDelta))
            .Title("position_delta")
            .Description("Change in position to the current MAV_FRAME_BODY_FRD from the previous FRAME_BODY_FRD rotated to the current MAV_FRAME_BODY_FRD.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(new ArrayType(FloatType.Default,3))        

            .Build();
        public const int PositionDeltaMaxItemsCount = 3;
        public float[] PositionDelta { get; } = new float[3];
        /// <summary>
        /// Normalised confidence value from 0 to 100.
        /// OriginName: confidence, Units: %, IsExtended: false
        /// </summary>
        public static readonly Field ConfidenceField = new Field.Builder()
            .Name(nameof(Confidence))
            .Title("confidence")
            .Description("Normalised confidence value from 0 to 100.")
            .FormatString(string.Empty)
            .Units(@"%")
            .DataType(FloatType.Default)

            .Build();
        private float _Confidence;
        public float Confidence { get => _Confidence; set { _Confidence = value; } }
    }
    /// <summary>
    /// Angle of Attack and Side Slip Angle.
    ///  AOA_SSA
    /// </summary>
    public class AoaSsaPacket : MavlinkV2Message<AoaSsaPayload>
    {
        public const int MessageId = 11020;
        
        public const byte CrcExtra = 205;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override AoaSsaPayload Payload { get; } = new();

        public override string Name => "AOA_SSA";
    }

    /// <summary>
    ///  AOA_SSA
    /// </summary>
    public class AoaSsaPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 16; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t time_usec
            +4 // float AOA
            +4 // float SSA
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeUsec = BinSerialize.ReadULong(ref buffer);
            Aoa = BinSerialize.ReadFloat(ref buffer);
            Ssa = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,TimeUsec);
            BinSerialize.WriteFloat(ref buffer,Aoa);
            BinSerialize.WriteFloat(ref buffer,Ssa);
            /* PayloadByteSize = 16 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,TimeUsecField, ref _TimeUsec);    
            FloatType.Accept(visitor,AoaField, ref _Aoa);    
            FloatType.Accept(visitor,SsaField, ref _Ssa);    

        }

        /// <summary>
        /// Timestamp (since boot or Unix epoch).
        /// OriginName: time_usec, Units: us, IsExtended: false
        /// </summary>
        public static readonly Field TimeUsecField = new Field.Builder()
            .Name(nameof(TimeUsec))
            .Title("time_usec")
            .Description("Timestamp (since boot or Unix epoch).")
            .FormatString(string.Empty)
            .Units(@"us")
            .DataType(UInt64Type.Default)

            .Build();
        private ulong _TimeUsec;
        public ulong TimeUsec { get => _TimeUsec; set { _TimeUsec = value; } }
        /// <summary>
        /// Angle of Attack.
        /// OriginName: AOA, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field AoaField = new Field.Builder()
            .Name(nameof(Aoa))
            .Title("AOA")
            .Description("Angle of Attack.")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Aoa;
        public float Aoa { get => _Aoa; set { _Aoa = value; } }
        /// <summary>
        /// Side Slip Angle.
        /// OriginName: SSA, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field SsaField = new Field.Builder()
            .Name(nameof(Ssa))
            .Title("SSA")
            .Description("Side Slip Angle.")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Ssa;
        public float Ssa { get => _Ssa; set { _Ssa = value; } }
    }
    /// <summary>
    /// ESC Telemetry Data for ESCs 1 to 4, matching data sent by BLHeli ESCs.
    ///  ESC_TELEMETRY_1_TO_4
    /// </summary>
    public class EscTelemetry1To4Packet : MavlinkV2Message<EscTelemetry1To4Payload>
    {
        public const int MessageId = 11030;
        
        public const byte CrcExtra = 144;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override EscTelemetry1To4Payload Payload { get; } = new();

        public override string Name => "ESC_TELEMETRY_1_TO_4";
    }

    /// <summary>
    ///  ESC_TELEMETRY_1_TO_4
    /// </summary>
    public class EscTelemetry1To4Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 44; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +Voltage.Length * 2 // uint16_t[4] voltage
            +Current.Length * 2 // uint16_t[4] current
            +Totalcurrent.Length * 2 // uint16_t[4] totalcurrent
            +Rpm.Length * 2 // uint16_t[4] rpm
            +Count.Length * 2 // uint16_t[4] count
            +Temperature.Length // uint8_t[4] temperature
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Voltage[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Current[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Totalcurrent[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Rpm[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Count[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Temperature[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<Voltage.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Voltage[i]);
            }
            for(var i=0;i<Current.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Current[i]);
            }
            for(var i=0;i<Totalcurrent.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Totalcurrent[i]);
            }
            for(var i=0;i<Rpm.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Rpm[i]);
            }
            for(var i=0;i<Count.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Count[i]);
            }
            for(var i=0;i<Temperature.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Temperature[i]);
            }
            /* PayloadByteSize = 44 */;
        }

        public void Visit(IVisitor visitor)
        {
            ArrayType.Accept(visitor,VoltageField, 4,
                (index,v) => UInt16Type.Accept(v, VoltageField, ref Voltage[index]));    
            ArrayType.Accept(visitor,CurrentField, 4,
                (index,v) => UInt16Type.Accept(v, CurrentField, ref Current[index]));    
            ArrayType.Accept(visitor,TotalcurrentField, 4,
                (index,v) => UInt16Type.Accept(v, TotalcurrentField, ref Totalcurrent[index]));    
            ArrayType.Accept(visitor,RpmField, 4,
                (index,v) => UInt16Type.Accept(v, RpmField, ref Rpm[index]));    
            ArrayType.Accept(visitor,CountField, 4,
                (index,v) => UInt16Type.Accept(v, CountField, ref Count[index]));    
            ArrayType.Accept(visitor,TemperatureField, 4,
                (index,v) => UInt8Type.Accept(v, TemperatureField, ref Temperature[index]));    

        }

        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: cV, IsExtended: false
        /// </summary>
        public static readonly Field VoltageField = new Field.Builder()
            .Name(nameof(Voltage))
            .Title("voltage")
            .Description("Voltage.")
            .FormatString(string.Empty)
            .Units(@"cV")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int VoltageMaxItemsCount = 4;
        public ushort[] Voltage { get; } = new ushort[4];
        [Obsolete("This method is deprecated. Use GetVoltageMaxItemsCount instead.")]
        public byte GetVoltageMaxItemsCount() => 4;
        /// <summary>
        /// Current.
        /// OriginName: current, Units: cA, IsExtended: false
        /// </summary>
        public static readonly Field CurrentField = new Field.Builder()
            .Name(nameof(Current))
            .Title("current")
            .Description("Current.")
            .FormatString(string.Empty)
            .Units(@"cA")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int CurrentMaxItemsCount = 4;
        public ushort[] Current { get; } = new ushort[4];
        /// <summary>
        /// Total current.
        /// OriginName: totalcurrent, Units: mAh, IsExtended: false
        /// </summary>
        public static readonly Field TotalcurrentField = new Field.Builder()
            .Name(nameof(Totalcurrent))
            .Title("totalcurrent")
            .Description("Total current.")
            .FormatString(string.Empty)
            .Units(@"mAh")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int TotalcurrentMaxItemsCount = 4;
        public ushort[] Totalcurrent { get; } = new ushort[4];
        /// <summary>
        /// RPM (eRPM).
        /// OriginName: rpm, Units: rpm, IsExtended: false
        /// </summary>
        public static readonly Field RpmField = new Field.Builder()
            .Name(nameof(Rpm))
            .Title("rpm")
            .Description("RPM (eRPM).")
            .FormatString(string.Empty)
            .Units(@"rpm")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int RpmMaxItemsCount = 4;
        public ushort[] Rpm { get; } = new ushort[4];
        /// <summary>
        /// count of telemetry packets received (wraps at 65535).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("count of telemetry packets received (wraps at 65535).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int CountMaxItemsCount = 4;
        public ushort[] Count { get; } = new ushort[4];
        /// <summary>
        /// Temperature.
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
        public static readonly Field TemperatureField = new Field.Builder()
            .Name(nameof(Temperature))
            .Title("temperature")
            .Description("Temperature.")
            .FormatString(string.Empty)
            .Units(@"degC")
            .DataType(new ArrayType(UInt8Type.Default,4))

            .Build();
        public const int TemperatureMaxItemsCount = 4;
        public byte[] Temperature { get; } = new byte[4];
    }
    /// <summary>
    /// ESC Telemetry Data for ESCs 5 to 8, matching data sent by BLHeli ESCs.
    ///  ESC_TELEMETRY_5_TO_8
    /// </summary>
    public class EscTelemetry5To8Packet : MavlinkV2Message<EscTelemetry5To8Payload>
    {
        public const int MessageId = 11031;
        
        public const byte CrcExtra = 133;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override EscTelemetry5To8Payload Payload { get; } = new();

        public override string Name => "ESC_TELEMETRY_5_TO_8";
    }

    /// <summary>
    ///  ESC_TELEMETRY_5_TO_8
    /// </summary>
    public class EscTelemetry5To8Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 44; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +Voltage.Length * 2 // uint16_t[4] voltage
            +Current.Length * 2 // uint16_t[4] current
            +Totalcurrent.Length * 2 // uint16_t[4] totalcurrent
            +Rpm.Length * 2 // uint16_t[4] rpm
            +Count.Length * 2 // uint16_t[4] count
            +Temperature.Length // uint8_t[4] temperature
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Voltage[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Current[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Totalcurrent[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Rpm[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Count[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Temperature[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<Voltage.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Voltage[i]);
            }
            for(var i=0;i<Current.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Current[i]);
            }
            for(var i=0;i<Totalcurrent.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Totalcurrent[i]);
            }
            for(var i=0;i<Rpm.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Rpm[i]);
            }
            for(var i=0;i<Count.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Count[i]);
            }
            for(var i=0;i<Temperature.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Temperature[i]);
            }
            /* PayloadByteSize = 44 */;
        }

        public void Visit(IVisitor visitor)
        {
            ArrayType.Accept(visitor,VoltageField, 4,
                (index,v) => UInt16Type.Accept(v, VoltageField, ref Voltage[index]));    
            ArrayType.Accept(visitor,CurrentField, 4,
                (index,v) => UInt16Type.Accept(v, CurrentField, ref Current[index]));    
            ArrayType.Accept(visitor,TotalcurrentField, 4,
                (index,v) => UInt16Type.Accept(v, TotalcurrentField, ref Totalcurrent[index]));    
            ArrayType.Accept(visitor,RpmField, 4,
                (index,v) => UInt16Type.Accept(v, RpmField, ref Rpm[index]));    
            ArrayType.Accept(visitor,CountField, 4,
                (index,v) => UInt16Type.Accept(v, CountField, ref Count[index]));    
            ArrayType.Accept(visitor,TemperatureField, 4,
                (index,v) => UInt8Type.Accept(v, TemperatureField, ref Temperature[index]));    

        }

        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: cV, IsExtended: false
        /// </summary>
        public static readonly Field VoltageField = new Field.Builder()
            .Name(nameof(Voltage))
            .Title("voltage")
            .Description("Voltage.")
            .FormatString(string.Empty)
            .Units(@"cV")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int VoltageMaxItemsCount = 4;
        public ushort[] Voltage { get; } = new ushort[4];
        [Obsolete("This method is deprecated. Use GetVoltageMaxItemsCount instead.")]
        public byte GetVoltageMaxItemsCount() => 4;
        /// <summary>
        /// Current.
        /// OriginName: current, Units: cA, IsExtended: false
        /// </summary>
        public static readonly Field CurrentField = new Field.Builder()
            .Name(nameof(Current))
            .Title("current")
            .Description("Current.")
            .FormatString(string.Empty)
            .Units(@"cA")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int CurrentMaxItemsCount = 4;
        public ushort[] Current { get; } = new ushort[4];
        /// <summary>
        /// Total current.
        /// OriginName: totalcurrent, Units: mAh, IsExtended: false
        /// </summary>
        public static readonly Field TotalcurrentField = new Field.Builder()
            .Name(nameof(Totalcurrent))
            .Title("totalcurrent")
            .Description("Total current.")
            .FormatString(string.Empty)
            .Units(@"mAh")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int TotalcurrentMaxItemsCount = 4;
        public ushort[] Totalcurrent { get; } = new ushort[4];
        /// <summary>
        /// RPM (eRPM).
        /// OriginName: rpm, Units: rpm, IsExtended: false
        /// </summary>
        public static readonly Field RpmField = new Field.Builder()
            .Name(nameof(Rpm))
            .Title("rpm")
            .Description("RPM (eRPM).")
            .FormatString(string.Empty)
            .Units(@"rpm")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int RpmMaxItemsCount = 4;
        public ushort[] Rpm { get; } = new ushort[4];
        /// <summary>
        /// count of telemetry packets received (wraps at 65535).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("count of telemetry packets received (wraps at 65535).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int CountMaxItemsCount = 4;
        public ushort[] Count { get; } = new ushort[4];
        /// <summary>
        /// Temperature.
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
        public static readonly Field TemperatureField = new Field.Builder()
            .Name(nameof(Temperature))
            .Title("temperature")
            .Description("Temperature.")
            .FormatString(string.Empty)
            .Units(@"degC")
            .DataType(new ArrayType(UInt8Type.Default,4))

            .Build();
        public const int TemperatureMaxItemsCount = 4;
        public byte[] Temperature { get; } = new byte[4];
    }
    /// <summary>
    /// ESC Telemetry Data for ESCs 9 to 12, matching data sent by BLHeli ESCs.
    ///  ESC_TELEMETRY_9_TO_12
    /// </summary>
    public class EscTelemetry9To12Packet : MavlinkV2Message<EscTelemetry9To12Payload>
    {
        public const int MessageId = 11032;
        
        public const byte CrcExtra = 85;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override EscTelemetry9To12Payload Payload { get; } = new();

        public override string Name => "ESC_TELEMETRY_9_TO_12";
    }

    /// <summary>
    ///  ESC_TELEMETRY_9_TO_12
    /// </summary>
    public class EscTelemetry9To12Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 44; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 44; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +Voltage.Length * 2 // uint16_t[4] voltage
            +Current.Length * 2 // uint16_t[4] current
            +Totalcurrent.Length * 2 // uint16_t[4] totalcurrent
            +Rpm.Length * 2 // uint16_t[4] rpm
            +Count.Length * 2 // uint16_t[4] count
            +Temperature.Length // uint8_t[4] temperature
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                Voltage[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Current[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Totalcurrent[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Rpm[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Count[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                Temperature[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<Voltage.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Voltage[i]);
            }
            for(var i=0;i<Current.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Current[i]);
            }
            for(var i=0;i<Totalcurrent.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Totalcurrent[i]);
            }
            for(var i=0;i<Rpm.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Rpm[i]);
            }
            for(var i=0;i<Count.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Count[i]);
            }
            for(var i=0;i<Temperature.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)Temperature[i]);
            }
            /* PayloadByteSize = 44 */;
        }

        public void Visit(IVisitor visitor)
        {
            ArrayType.Accept(visitor,VoltageField, 4,
                (index,v) => UInt16Type.Accept(v, VoltageField, ref Voltage[index]));    
            ArrayType.Accept(visitor,CurrentField, 4,
                (index,v) => UInt16Type.Accept(v, CurrentField, ref Current[index]));    
            ArrayType.Accept(visitor,TotalcurrentField, 4,
                (index,v) => UInt16Type.Accept(v, TotalcurrentField, ref Totalcurrent[index]));    
            ArrayType.Accept(visitor,RpmField, 4,
                (index,v) => UInt16Type.Accept(v, RpmField, ref Rpm[index]));    
            ArrayType.Accept(visitor,CountField, 4,
                (index,v) => UInt16Type.Accept(v, CountField, ref Count[index]));    
            ArrayType.Accept(visitor,TemperatureField, 4,
                (index,v) => UInt8Type.Accept(v, TemperatureField, ref Temperature[index]));    

        }

        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: cV, IsExtended: false
        /// </summary>
        public static readonly Field VoltageField = new Field.Builder()
            .Name(nameof(Voltage))
            .Title("voltage")
            .Description("Voltage.")
            .FormatString(string.Empty)
            .Units(@"cV")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int VoltageMaxItemsCount = 4;
        public ushort[] Voltage { get; } = new ushort[4];
        [Obsolete("This method is deprecated. Use GetVoltageMaxItemsCount instead.")]
        public byte GetVoltageMaxItemsCount() => 4;
        /// <summary>
        /// Current.
        /// OriginName: current, Units: cA, IsExtended: false
        /// </summary>
        public static readonly Field CurrentField = new Field.Builder()
            .Name(nameof(Current))
            .Title("current")
            .Description("Current.")
            .FormatString(string.Empty)
            .Units(@"cA")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int CurrentMaxItemsCount = 4;
        public ushort[] Current { get; } = new ushort[4];
        /// <summary>
        /// Total current.
        /// OriginName: totalcurrent, Units: mAh, IsExtended: false
        /// </summary>
        public static readonly Field TotalcurrentField = new Field.Builder()
            .Name(nameof(Totalcurrent))
            .Title("totalcurrent")
            .Description("Total current.")
            .FormatString(string.Empty)
            .Units(@"mAh")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int TotalcurrentMaxItemsCount = 4;
        public ushort[] Totalcurrent { get; } = new ushort[4];
        /// <summary>
        /// RPM (eRPM).
        /// OriginName: rpm, Units: rpm, IsExtended: false
        /// </summary>
        public static readonly Field RpmField = new Field.Builder()
            .Name(nameof(Rpm))
            .Title("rpm")
            .Description("RPM (eRPM).")
            .FormatString(string.Empty)
            .Units(@"rpm")
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int RpmMaxItemsCount = 4;
        public ushort[] Rpm { get; } = new ushort[4];
        /// <summary>
        /// count of telemetry packets received (wraps at 65535).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("count of telemetry packets received (wraps at 65535).")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt16Type.Default,4))

            .Build();
        public const int CountMaxItemsCount = 4;
        public ushort[] Count { get; } = new ushort[4];
        /// <summary>
        /// Temperature.
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
        public static readonly Field TemperatureField = new Field.Builder()
            .Name(nameof(Temperature))
            .Title("temperature")
            .Description("Temperature.")
            .FormatString(string.Empty)
            .Units(@"degC")
            .DataType(new ArrayType(UInt8Type.Default,4))

            .Build();
        public const int TemperatureMaxItemsCount = 4;
        public byte[] Temperature { get; } = new byte[4];
    }
    /// <summary>
    /// Configure an OSD parameter slot.
    ///  OSD_PARAM_CONFIG
    /// </summary>
    public class OsdParamConfigPacket : MavlinkV2Message<OsdParamConfigPayload>
    {
        public const int MessageId = 11033;
        
        public const byte CrcExtra = 195;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override OsdParamConfigPayload Payload { get; } = new();

        public override string Name => "OSD_PARAM_CONFIG";
    }

    /// <summary>
    ///  OSD_PARAM_CONFIG
    /// </summary>
    public class OsdParamConfigPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 37; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 37; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t request_id
            +4 // float min_value
            +4 // float max_value
            +4 // float increment
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t osd_screen
            +1 // uint8_t osd_index
            +ParamId.Length // char[16] param_id
            + 1 // uint8_t config_type
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUInt(ref buffer);
            MinValue = BinSerialize.ReadFloat(ref buffer);
            MaxValue = BinSerialize.ReadFloat(ref buffer);
            Increment = BinSerialize.ReadFloat(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            OsdScreen = (byte)BinSerialize.ReadByte(ref buffer);
            OsdIndex = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/37 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ParamId)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, ParamId.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            ConfigType = (OsdParamConfigType)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,RequestId);
            BinSerialize.WriteFloat(ref buffer,MinValue);
            BinSerialize.WriteFloat(ref buffer,MaxValue);
            BinSerialize.WriteFloat(ref buffer,Increment);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)OsdScreen);
            BinSerialize.WriteByte(ref buffer,(byte)OsdIndex);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ParamId)
                {
                    Encoding.ASCII.GetBytes(charPointer, ParamId.Length, bytePointer, ParamId.Length);
                }
            }
            buffer = buffer.Slice(ParamId.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)ConfigType);
            /* PayloadByteSize = 37 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,RequestIdField, ref _RequestId);    
            FloatType.Accept(visitor,MinValueField, ref _MinValue);    
            FloatType.Accept(visitor,MaxValueField, ref _MaxValue);    
            FloatType.Accept(visitor,IncrementField, ref _Increment);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,OsdScreenField, ref _OsdScreen);    
            UInt8Type.Accept(visitor,OsdIndexField, ref _OsdIndex);    
            ArrayType.Accept(visitor,ParamIdField, 16, (index,v) =>
            {
                var tmp = (byte)ParamId[index];
                UInt8Type.Accept(v,ParamIdField, ref tmp);
                ParamId[index] = (char)tmp;
            });
            var tmpConfigType = (byte)ConfigType;
            UInt8Type.Accept(visitor,ConfigTypeField, ref tmpConfigType);
            ConfigType = (OsdParamConfigType)tmpConfigType;

        }

        /// <summary>
        /// Request ID - copied to reply.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Request ID - copied to reply.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _RequestId;
        public uint RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// OSD parameter minimum value.
        /// OriginName: min_value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MinValueField = new Field.Builder()
            .Name(nameof(MinValue))
            .Title("min_value")
            .Description("OSD parameter minimum value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _MinValue;
        public float MinValue { get => _MinValue; set { _MinValue = value; } }
        /// <summary>
        /// OSD parameter maximum value.
        /// OriginName: max_value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MaxValueField = new Field.Builder()
            .Name(nameof(MaxValue))
            .Title("max_value")
            .Description("OSD parameter maximum value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _MaxValue;
        public float MaxValue { get => _MaxValue; set { _MaxValue = value; } }
        /// <summary>
        /// OSD parameter increment.
        /// OriginName: increment, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IncrementField = new Field.Builder()
            .Name(nameof(Increment))
            .Title("increment")
            .Description("OSD parameter increment.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Increment;
        public float Increment { get => _Increment; set { _Increment = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// OSD parameter screen index.
        /// OriginName: osd_screen, Units: , IsExtended: false
        /// </summary>
        public static readonly Field OsdScreenField = new Field.Builder()
            .Name(nameof(OsdScreen))
            .Title("osd_screen")
            .Description("OSD parameter screen index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _OsdScreen;
        public byte OsdScreen { get => _OsdScreen; set { _OsdScreen = value; } }
        /// <summary>
        /// OSD parameter display index.
        /// OriginName: osd_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field OsdIndexField = new Field.Builder()
            .Name(nameof(OsdIndex))
            .Title("osd_index")
            .Description("OSD parameter display index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _OsdIndex;
        public byte OsdIndex { get => _OsdIndex; set { _OsdIndex = value; } }
        /// <summary>
        /// Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
        /// OriginName: param_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ParamIdField = new Field.Builder()
            .Name(nameof(ParamId))
            .Title("param_id")
            .Description("Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,16))

            .Build();
        public const int ParamIdMaxItemsCount = 16;
        public char[] ParamId { get; } = new char[16];
        [Obsolete("This method is deprecated. Use GetParamIdMaxItemsCount instead.")]
        public byte GetParamIdMaxItemsCount() => 16;
        /// <summary>
        /// Config type.
        /// OriginName: config_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ConfigTypeField = new Field.Builder()
            .Name(nameof(ConfigType))
            .Title("config_type")
            .Description("Config type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public OsdParamConfigType _ConfigType;
        public OsdParamConfigType ConfigType { get => _ConfigType; set => _ConfigType = value; } 
    }
    /// <summary>
    /// Configure OSD parameter reply.
    ///  OSD_PARAM_CONFIG_REPLY
    /// </summary>
    public class OsdParamConfigReplyPacket : MavlinkV2Message<OsdParamConfigReplyPayload>
    {
        public const int MessageId = 11034;
        
        public const byte CrcExtra = 79;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override OsdParamConfigReplyPayload Payload { get; } = new();

        public override string Name => "OSD_PARAM_CONFIG_REPLY";
    }

    /// <summary>
    ///  OSD_PARAM_CONFIG_REPLY
    /// </summary>
    public class OsdParamConfigReplyPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 5; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 5; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t request_id
            + 1 // uint8_t result
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUInt(ref buffer);
            Result = (OsdParamConfigError)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            /* PayloadByteSize = 5 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,RequestIdField, ref _RequestId);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ref tmpResult);
            Result = (OsdParamConfigError)tmpResult;

        }

        /// <summary>
        /// Request ID - copied from request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Request ID - copied from request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _RequestId;
        public uint RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// Config error type.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Config error type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public OsdParamConfigError _Result;
        public OsdParamConfigError Result { get => _Result; set => _Result = value; } 
    }
    /// <summary>
    /// Read a configured an OSD parameter slot.
    ///  OSD_PARAM_SHOW_CONFIG
    /// </summary>
    public class OsdParamShowConfigPacket : MavlinkV2Message<OsdParamShowConfigPayload>
    {
        public const int MessageId = 11035;
        
        public const byte CrcExtra = 128;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override OsdParamShowConfigPayload Payload { get; } = new();

        public override string Name => "OSD_PARAM_SHOW_CONFIG";
    }

    /// <summary>
    ///  OSD_PARAM_SHOW_CONFIG
    /// </summary>
    public class OsdParamShowConfigPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 8; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 8; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t request_id
            +1 // uint8_t target_system
            +1 // uint8_t target_component
            +1 // uint8_t osd_screen
            +1 // uint8_t osd_index
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            RequestId = BinSerialize.ReadUInt(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            OsdScreen = (byte)BinSerialize.ReadByte(ref buffer);
            OsdIndex = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,RequestId);
            BinSerialize.WriteByte(ref buffer,(byte)TargetSystem);
            BinSerialize.WriteByte(ref buffer,(byte)TargetComponent);
            BinSerialize.WriteByte(ref buffer,(byte)OsdScreen);
            BinSerialize.WriteByte(ref buffer,(byte)OsdIndex);
            /* PayloadByteSize = 8 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,RequestIdField, ref _RequestId);    
            UInt8Type.Accept(visitor,TargetSystemField, ref _TargetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, ref _TargetComponent);    
            UInt8Type.Accept(visitor,OsdScreenField, ref _OsdScreen);    
            UInt8Type.Accept(visitor,OsdIndexField, ref _OsdIndex);    

        }

        /// <summary>
        /// Request ID - copied to reply.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Request ID - copied to reply.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _RequestId;
        public uint RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// System ID.
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetSystem;
        public byte TargetSystem { get => _TargetSystem; set { _TargetSystem = value; } }
        /// <summary>
        /// Component ID.
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _TargetComponent;
        public byte TargetComponent { get => _TargetComponent; set { _TargetComponent = value; } }
        /// <summary>
        /// OSD parameter screen index.
        /// OriginName: osd_screen, Units: , IsExtended: false
        /// </summary>
        public static readonly Field OsdScreenField = new Field.Builder()
            .Name(nameof(OsdScreen))
            .Title("osd_screen")
            .Description("OSD parameter screen index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _OsdScreen;
        public byte OsdScreen { get => _OsdScreen; set { _OsdScreen = value; } }
        /// <summary>
        /// OSD parameter display index.
        /// OriginName: osd_index, Units: , IsExtended: false
        /// </summary>
        public static readonly Field OsdIndexField = new Field.Builder()
            .Name(nameof(OsdIndex))
            .Title("osd_index")
            .Description("OSD parameter display index.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _OsdIndex;
        public byte OsdIndex { get => _OsdIndex; set { _OsdIndex = value; } }
    }
    /// <summary>
    /// Read configured OSD parameter reply.
    ///  OSD_PARAM_SHOW_CONFIG_REPLY
    /// </summary>
    public class OsdParamShowConfigReplyPacket : MavlinkV2Message<OsdParamShowConfigReplyPayload>
    {
        public const int MessageId = 11036;
        
        public const byte CrcExtra = 177;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override OsdParamShowConfigReplyPayload Payload { get; } = new();

        public override string Name => "OSD_PARAM_SHOW_CONFIG_REPLY";
    }

    /// <summary>
    ///  OSD_PARAM_SHOW_CONFIG_REPLY
    /// </summary>
    public class OsdParamShowConfigReplyPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 34; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 34; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t request_id
            +4 // float min_value
            +4 // float max_value
            +4 // float increment
            + 1 // uint8_t result
            +ParamId.Length // char[16] param_id
            + 1 // uint8_t config_type
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            RequestId = BinSerialize.ReadUInt(ref buffer);
            MinValue = BinSerialize.ReadFloat(ref buffer);
            MaxValue = BinSerialize.ReadFloat(ref buffer);
            Increment = BinSerialize.ReadFloat(ref buffer);
            Result = (OsdParamConfigError)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/34 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ParamId)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, ParamId.Length);
                }
            }
            buffer = buffer[arraySize..];
           
            ConfigType = (OsdParamConfigType)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,RequestId);
            BinSerialize.WriteFloat(ref buffer,MinValue);
            BinSerialize.WriteFloat(ref buffer,MaxValue);
            BinSerialize.WriteFloat(ref buffer,Increment);
            BinSerialize.WriteByte(ref buffer,(byte)Result);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ParamId)
                {
                    Encoding.ASCII.GetBytes(charPointer, ParamId.Length, bytePointer, ParamId.Length);
                }
            }
            buffer = buffer.Slice(ParamId.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)ConfigType);
            /* PayloadByteSize = 34 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,RequestIdField, ref _RequestId);    
            FloatType.Accept(visitor,MinValueField, ref _MinValue);    
            FloatType.Accept(visitor,MaxValueField, ref _MaxValue);    
            FloatType.Accept(visitor,IncrementField, ref _Increment);    
            var tmpResult = (byte)Result;
            UInt8Type.Accept(visitor,ResultField, ref tmpResult);
            Result = (OsdParamConfigError)tmpResult;
            ArrayType.Accept(visitor,ParamIdField, 16, (index,v) =>
            {
                var tmp = (byte)ParamId[index];
                UInt8Type.Accept(v,ParamIdField, ref tmp);
                ParamId[index] = (char)tmp;
            });
            var tmpConfigType = (byte)ConfigType;
            UInt8Type.Accept(visitor,ConfigTypeField, ref tmpConfigType);
            ConfigType = (OsdParamConfigType)tmpConfigType;

        }

        /// <summary>
        /// Request ID - copied from request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RequestIdField = new Field.Builder()
            .Name(nameof(RequestId))
            .Title("request_id")
            .Description("Request ID - copied from request.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt32Type.Default)

            .Build();
        private uint _RequestId;
        public uint RequestId { get => _RequestId; set { _RequestId = value; } }
        /// <summary>
        /// OSD parameter minimum value.
        /// OriginName: min_value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MinValueField = new Field.Builder()
            .Name(nameof(MinValue))
            .Title("min_value")
            .Description("OSD parameter minimum value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _MinValue;
        public float MinValue { get => _MinValue; set { _MinValue = value; } }
        /// <summary>
        /// OSD parameter maximum value.
        /// OriginName: max_value, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MaxValueField = new Field.Builder()
            .Name(nameof(MaxValue))
            .Title("max_value")
            .Description("OSD parameter maximum value.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _MaxValue;
        public float MaxValue { get => _MaxValue; set { _MaxValue = value; } }
        /// <summary>
        /// OSD parameter increment.
        /// OriginName: increment, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IncrementField = new Field.Builder()
            .Name(nameof(Increment))
            .Title("increment")
            .Description("OSD parameter increment.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(FloatType.Default)

            .Build();
        private float _Increment;
        public float Increment { get => _Increment; set { _Increment = value; } }
        /// <summary>
        /// Config error type.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ResultField = new Field.Builder()
            .Name(nameof(Result))
            .Title("result")
            .Description("Config error type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public OsdParamConfigError _Result;
        public OsdParamConfigError Result { get => _Result; set => _Result = value; } 
        /// <summary>
        /// Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
        /// OriginName: param_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ParamIdField = new Field.Builder()
            .Name(nameof(ParamId))
            .Title("param_id")
            .Description("Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(new ArrayType(UInt8Type.Default,16))

            .Build();
        public const int ParamIdMaxItemsCount = 16;
        public char[] ParamId { get; } = new char[16];
        [Obsolete("This method is deprecated. Use GetParamIdMaxItemsCount instead.")]
        public byte GetParamIdMaxItemsCount() => 16;
        /// <summary>
        /// Config type.
        /// OriginName: config_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ConfigTypeField = new Field.Builder()
            .Name(nameof(ConfigType))
            .Title("config_type")
            .Description("Config type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public OsdParamConfigType _ConfigType;
        public OsdParamConfigType ConfigType { get => _ConfigType; set => _ConfigType = value; } 
    }
    /// <summary>
    /// Obstacle located as a 3D vector.
    ///  OBSTACLE_DISTANCE_3D
    /// </summary>
    public class ObstacleDistance3dPacket : MavlinkV2Message<ObstacleDistance3dPayload>
    {
        public const int MessageId = 11037;
        
        public const byte CrcExtra = 130;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override ObstacleDistance3dPayload Payload { get; } = new();

        public override string Name => "OBSTACLE_DISTANCE_3D";
    }

    /// <summary>
    ///  OBSTACLE_DISTANCE_3D
    /// </summary>
    public class ObstacleDistance3dPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 28; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 28; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t time_boot_ms
            +4 // float x
            +4 // float y
            +4 // float z
            +4 // float min_distance
            +4 // float max_distance
            +2 // uint16_t obstacle_id
            + 1 // uint8_t sensor_type
            + 1 // uint8_t frame
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeBootMs = BinSerialize.ReadUInt(ref buffer);
            X = BinSerialize.ReadFloat(ref buffer);
            Y = BinSerialize.ReadFloat(ref buffer);
            Z = BinSerialize.ReadFloat(ref buffer);
            MinDistance = BinSerialize.ReadFloat(ref buffer);
            MaxDistance = BinSerialize.ReadFloat(ref buffer);
            ObstacleId = BinSerialize.ReadUShort(ref buffer);
            SensorType = (MavDistanceSensor)BinSerialize.ReadByte(ref buffer);
            Frame = (MavFrame)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,TimeBootMs);
            BinSerialize.WriteFloat(ref buffer,X);
            BinSerialize.WriteFloat(ref buffer,Y);
            BinSerialize.WriteFloat(ref buffer,Z);
            BinSerialize.WriteFloat(ref buffer,MinDistance);
            BinSerialize.WriteFloat(ref buffer,MaxDistance);
            BinSerialize.WriteUShort(ref buffer,ObstacleId);
            BinSerialize.WriteByte(ref buffer,(byte)SensorType);
            BinSerialize.WriteByte(ref buffer,(byte)Frame);
            /* PayloadByteSize = 28 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,TimeBootMsField, ref _TimeBootMs);    
            FloatType.Accept(visitor,XField, ref _X);    
            FloatType.Accept(visitor,YField, ref _Y);    
            FloatType.Accept(visitor,ZField, ref _Z);    
            FloatType.Accept(visitor,MinDistanceField, ref _MinDistance);    
            FloatType.Accept(visitor,MaxDistanceField, ref _MaxDistance);    
            UInt16Type.Accept(visitor,ObstacleIdField, ref _ObstacleId);    
            var tmpSensorType = (byte)SensorType;
            UInt8Type.Accept(visitor,SensorTypeField, ref tmpSensorType);
            SensorType = (MavDistanceSensor)tmpSensorType;
            var tmpFrame = (byte)Frame;
            UInt8Type.Accept(visitor,FrameField, ref tmpFrame);
            Frame = (MavFrame)tmpFrame;

        }

        /// <summary>
        /// Timestamp (time since system boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field TimeBootMsField = new Field.Builder()
            .Name(nameof(TimeBootMs))
            .Title("time_boot_ms")
            .Description("Timestamp (time since system boot).")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _TimeBootMs;
        public uint TimeBootMs { get => _TimeBootMs; set { _TimeBootMs = value; } }
        /// <summary>
        ///  X position of the obstacle.
        /// OriginName: x, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field XField = new Field.Builder()
            .Name(nameof(X))
            .Title("x")
            .Description(" X position of the obstacle.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _X;
        public float X { get => _X; set { _X = value; } }
        /// <summary>
        ///  Y position of the obstacle.
        /// OriginName: y, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field YField = new Field.Builder()
            .Name(nameof(Y))
            .Title("y")
            .Description(" Y position of the obstacle.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Y;
        public float Y { get => _Y; set { _Y = value; } }
        /// <summary>
        ///  Z position of the obstacle.
        /// OriginName: z, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field ZField = new Field.Builder()
            .Name(nameof(Z))
            .Title("z")
            .Description(" Z position of the obstacle.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Z;
        public float Z { get => _Z; set { _Z = value; } }
        /// <summary>
        /// Minimum distance the sensor can measure.
        /// OriginName: min_distance, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field MinDistanceField = new Field.Builder()
            .Name(nameof(MinDistance))
            .Title("min_distance")
            .Description("Minimum distance the sensor can measure.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _MinDistance;
        public float MinDistance { get => _MinDistance; set { _MinDistance = value; } }
        /// <summary>
        /// Maximum distance the sensor can measure.
        /// OriginName: max_distance, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field MaxDistanceField = new Field.Builder()
            .Name(nameof(MaxDistance))
            .Title("max_distance")
            .Description("Maximum distance the sensor can measure.")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _MaxDistance;
        public float MaxDistance { get => _MaxDistance; set { _MaxDistance = value; } }
        /// <summary>
        ///  Unique ID given to each obstacle so that its movement can be tracked. Use UINT16_MAX if object ID is unknown or cannot be determined.
        /// OriginName: obstacle_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ObstacleIdField = new Field.Builder()
            .Name(nameof(ObstacleId))
            .Title("obstacle_id")
            .Description(" Unique ID given to each obstacle so that its movement can be tracked. Use UINT16_MAX if object ID is unknown or cannot be determined.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _ObstacleId;
        public ushort ObstacleId { get => _ObstacleId; set { _ObstacleId = value; } }
        /// <summary>
        /// Class id of the distance sensor type.
        /// OriginName: sensor_type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SensorTypeField = new Field.Builder()
            .Name(nameof(SensorType))
            .Title("sensor_type")
            .Description("Class id of the distance sensor type.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public MavDistanceSensor _SensorType;
        public MavDistanceSensor SensorType { get => _SensorType; set => _SensorType = value; } 
        /// <summary>
        /// Coordinate frame of reference.
        /// OriginName: frame, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FrameField = new Field.Builder()
            .Name(nameof(Frame))
            .Title("frame")
            .Description("Coordinate frame of reference.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public MavFrame _Frame;
        public MavFrame Frame { get => _Frame; set => _Frame = value; } 
    }
    /// <summary>
    /// Water depth
    ///  WATER_DEPTH
    /// </summary>
    public class WaterDepthPacket : MavlinkV2Message<WaterDepthPayload>
    {
        public const int MessageId = 11038;
        
        public const byte CrcExtra = 47;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override WaterDepthPayload Payload { get; } = new();

        public override string Name => "WATER_DEPTH";
    }

    /// <summary>
    ///  WATER_DEPTH
    /// </summary>
    public class WaterDepthPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 38; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 38; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t time_boot_ms
            +4 // int32_t lat
            +4 // int32_t lng
            +4 // float alt
            +4 // float roll
            +4 // float pitch
            +4 // float yaw
            +4 // float distance
            +4 // float temperature
            +1 // uint8_t id
            +1 // uint8_t healthy
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TimeBootMs = BinSerialize.ReadUInt(ref buffer);
            Lat = BinSerialize.ReadInt(ref buffer);
            Lng = BinSerialize.ReadInt(ref buffer);
            Alt = BinSerialize.ReadFloat(ref buffer);
            Roll = BinSerialize.ReadFloat(ref buffer);
            Pitch = BinSerialize.ReadFloat(ref buffer);
            Yaw = BinSerialize.ReadFloat(ref buffer);
            Distance = BinSerialize.ReadFloat(ref buffer);
            Temperature = BinSerialize.ReadFloat(ref buffer);
            Id = (byte)BinSerialize.ReadByte(ref buffer);
            Healthy = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,TimeBootMs);
            BinSerialize.WriteInt(ref buffer,Lat);
            BinSerialize.WriteInt(ref buffer,Lng);
            BinSerialize.WriteFloat(ref buffer,Alt);
            BinSerialize.WriteFloat(ref buffer,Roll);
            BinSerialize.WriteFloat(ref buffer,Pitch);
            BinSerialize.WriteFloat(ref buffer,Yaw);
            BinSerialize.WriteFloat(ref buffer,Distance);
            BinSerialize.WriteFloat(ref buffer,Temperature);
            BinSerialize.WriteByte(ref buffer,(byte)Id);
            BinSerialize.WriteByte(ref buffer,(byte)Healthy);
            /* PayloadByteSize = 38 */;
        }

        public void Visit(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,TimeBootMsField, ref _TimeBootMs);    
            Int32Type.Accept(visitor,LatField, ref _Lat);    
            Int32Type.Accept(visitor,LngField, ref _Lng);    
            FloatType.Accept(visitor,AltField, ref _Alt);    
            FloatType.Accept(visitor,RollField, ref _Roll);    
            FloatType.Accept(visitor,PitchField, ref _Pitch);    
            FloatType.Accept(visitor,YawField, ref _Yaw);    
            FloatType.Accept(visitor,DistanceField, ref _Distance);    
            FloatType.Accept(visitor,TemperatureField, ref _Temperature);    
            UInt8Type.Accept(visitor,IdField, ref _Id);    
            UInt8Type.Accept(visitor,HealthyField, ref _Healthy);    

        }

        /// <summary>
        /// Timestamp (time since system boot)
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field TimeBootMsField = new Field.Builder()
            .Name(nameof(TimeBootMs))
            .Title("time_boot_ms")
            .Description("Timestamp (time since system boot)")
            .FormatString(string.Empty)
            .Units(@"ms")
            .DataType(UInt32Type.Default)

            .Build();
        private uint _TimeBootMs;
        public uint TimeBootMs { get => _TimeBootMs; set { _TimeBootMs = value; } }
        /// <summary>
        /// Latitude
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LatField = new Field.Builder()
            .Name(nameof(Lat))
            .Title("lat")
            .Description("Latitude")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lat;
        public int Lat { get => _Lat; set { _Lat = value; } }
        /// <summary>
        /// Longitude
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public static readonly Field LngField = new Field.Builder()
            .Name(nameof(Lng))
            .Title("lng")
            .Description("Longitude")
            .FormatString(string.Empty)
            .Units(@"degE7")
            .DataType(Int32Type.Default)

            .Build();
        private int _Lng;
        public int Lng { get => _Lng; set { _Lng = value; } }
        /// <summary>
        /// Altitude (MSL) of vehicle
        /// OriginName: alt, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field AltField = new Field.Builder()
            .Name(nameof(Alt))
            .Title("alt")
            .Description("Altitude (MSL) of vehicle")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Alt;
        public float Alt { get => _Alt; set { _Alt = value; } }
        /// <summary>
        /// Roll angle
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Roll;
        public float Roll { get => _Roll; set { _Roll = value; } }
        /// <summary>
        /// Pitch angle
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch angle")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Pitch;
        public float Pitch { get => _Pitch; set { _Pitch = value; } }
        /// <summary>
        /// Yaw angle
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw angle")
            .FormatString(string.Empty)
            .Units(@"rad")
            .DataType(FloatType.Default)

            .Build();
        private float _Yaw;
        public float Yaw { get => _Yaw; set { _Yaw = value; } }
        /// <summary>
        /// Distance (uncorrected)
        /// OriginName: distance, Units: m, IsExtended: false
        /// </summary>
        public static readonly Field DistanceField = new Field.Builder()
            .Name(nameof(Distance))
            .Title("distance")
            .Description("Distance (uncorrected)")
            .FormatString(string.Empty)
            .Units(@"m")
            .DataType(FloatType.Default)

            .Build();
        private float _Distance;
        public float Distance { get => _Distance; set { _Distance = value; } }
        /// <summary>
        /// Water temperature
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
        public static readonly Field TemperatureField = new Field.Builder()
            .Name(nameof(Temperature))
            .Title("temperature")
            .Description("Water temperature")
            .FormatString(string.Empty)
            .Units(@"degC")
            .DataType(FloatType.Default)

            .Build();
        private float _Temperature;
        public float Temperature { get => _Temperature; set { _Temperature = value; } }
        /// <summary>
        /// Onboard ID of the sensor
        /// OriginName: id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IdField = new Field.Builder()
            .Name(nameof(Id))
            .Title("id")
            .Description("Onboard ID of the sensor")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Id;
        public byte Id { get => _Id; set { _Id = value; } }
        /// <summary>
        /// Sensor data healthy (0=unhealthy, 1=healthy)
        /// OriginName: healthy, Units: , IsExtended: false
        /// </summary>
        public static readonly Field HealthyField = new Field.Builder()
            .Name(nameof(Healthy))
            .Title("healthy")
            .Description("Sensor data healthy (0=unhealthy, 1=healthy)")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Healthy;
        public byte Healthy { get => _Healthy; set { _Healthy = value; } }
    }
    /// <summary>
    /// The MCU status, giving MCU temperature and voltage. The min and max voltages are to allow for detecting power supply instability.
    ///  MCU_STATUS
    /// </summary>
    public class McuStatusPacket : MavlinkV2Message<McuStatusPayload>
    {
        public const int MessageId = 11039;
        
        public const byte CrcExtra = 142;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override McuStatusPayload Payload { get; } = new();

        public override string Name => "MCU_STATUS";
    }

    /// <summary>
    ///  MCU_STATUS
    /// </summary>
    public class McuStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 9; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 9; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // int16_t MCU_temperature
            +2 // uint16_t MCU_voltage
            +2 // uint16_t MCU_voltage_min
            +2 // uint16_t MCU_voltage_max
            +1 // uint8_t id
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            McuTemperature = BinSerialize.ReadShort(ref buffer);
            McuVoltage = BinSerialize.ReadUShort(ref buffer);
            McuVoltageMin = BinSerialize.ReadUShort(ref buffer);
            McuVoltageMax = BinSerialize.ReadUShort(ref buffer);
            Id = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteShort(ref buffer,McuTemperature);
            BinSerialize.WriteUShort(ref buffer,McuVoltage);
            BinSerialize.WriteUShort(ref buffer,McuVoltageMin);
            BinSerialize.WriteUShort(ref buffer,McuVoltageMax);
            BinSerialize.WriteByte(ref buffer,(byte)Id);
            /* PayloadByteSize = 9 */;
        }

        public void Visit(IVisitor visitor)
        {
            Int16Type.Accept(visitor,McuTemperatureField, ref _McuTemperature);
            UInt16Type.Accept(visitor,McuVoltageField, ref _McuVoltage);    
            UInt16Type.Accept(visitor,McuVoltageMinField, ref _McuVoltageMin);    
            UInt16Type.Accept(visitor,McuVoltageMaxField, ref _McuVoltageMax);    
            UInt8Type.Accept(visitor,IdField, ref _Id);    

        }

        /// <summary>
        /// MCU Internal temperature
        /// OriginName: MCU_temperature, Units: cdegC, IsExtended: false
        /// </summary>
        public static readonly Field McuTemperatureField = new Field.Builder()
            .Name(nameof(McuTemperature))
            .Title("MCU_temperature")
            .Description("MCU Internal temperature")
            .FormatString(string.Empty)
            .Units(@"cdegC")
            .DataType(Int16Type.Default)

            .Build();
        private short _McuTemperature;
        public short McuTemperature { get => _McuTemperature; set { _McuTemperature = value; } }
        /// <summary>
        /// MCU voltage
        /// OriginName: MCU_voltage, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field McuVoltageField = new Field.Builder()
            .Name(nameof(McuVoltage))
            .Title("MCU_voltage")
            .Description("MCU voltage")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _McuVoltage;
        public ushort McuVoltage { get => _McuVoltage; set { _McuVoltage = value; } }
        /// <summary>
        /// MCU voltage minimum
        /// OriginName: MCU_voltage_min, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field McuVoltageMinField = new Field.Builder()
            .Name(nameof(McuVoltageMin))
            .Title("MCU_voltage_min")
            .Description("MCU voltage minimum")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _McuVoltageMin;
        public ushort McuVoltageMin { get => _McuVoltageMin; set { _McuVoltageMin = value; } }
        /// <summary>
        /// MCU voltage maximum
        /// OriginName: MCU_voltage_max, Units: mV, IsExtended: false
        /// </summary>
        public static readonly Field McuVoltageMaxField = new Field.Builder()
            .Name(nameof(McuVoltageMax))
            .Title("MCU_voltage_max")
            .Description("MCU voltage maximum")
            .FormatString(string.Empty)
            .Units(@"mV")
            .DataType(UInt16Type.Default)

            .Build();
        private ushort _McuVoltageMax;
        public ushort McuVoltageMax { get => _McuVoltageMax; set { _McuVoltageMax = value; } }
        /// <summary>
        /// MCU instance
        /// OriginName: id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field IdField = new Field.Builder()
            .Name(nameof(Id))
            .Title("id")
            .Description("MCU instance")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        private byte _Id;
        public byte Id { get => _Id; set { _Id = value; } }
    }




        


#endregion


}

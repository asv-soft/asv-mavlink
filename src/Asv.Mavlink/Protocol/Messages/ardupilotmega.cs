// MIT License
//
// Copyright (c) 2024 asv-soft (https://github.com/asv-soft)
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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.11+22841a669900eb4c494a7e77e2d4b5fee4e474db

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("mag_declination",
"Magnetic declination.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("raw_press",
"Raw pressure from barometer.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("raw_temp",
"Raw temperature from barometer.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("gyro_cal_x",
"Gyro X calibration.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("gyro_cal_y",
"Gyro Y calibration.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("gyro_cal_z",
"Gyro Z calibration.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("accel_cal_x",
"Accel X calibration.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("accel_cal_y",
"Accel Y calibration.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("accel_cal_z",
"Accel Z calibration.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("mag_ofs_x",
"Magnetometer X offset.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("mag_ofs_y",
"Magnetometer Y offset.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("mag_ofs_z",
"Magnetometer Z offset.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
        ];
        public const string FormatMessage = "SENSOR_OFFSETS:"
        + "float mag_declination;"
        + "int32_t raw_press;"
        + "int32_t raw_temp;"
        + "float gyro_cal_x;"
        + "float gyro_cal_y;"
        + "float gyro_cal_z;"
        + "float accel_cal_x;"
        + "float accel_cal_y;"
        + "float accel_cal_z;"
        + "int16_t mag_ofs_x;"
        + "int16_t mag_ofs_y;"
        + "int16_t mag_ofs_z;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //MagDeclination
            sum+=4; //RawPress
            sum+=4; //RawTemp
            sum+=4; //GyroCalX
            sum+=4; //GyroCalY
            sum+=4; //GyroCalZ
            sum+=4; //AccelCalX
            sum+=4; //AccelCalY
            sum+=4; //AccelCalZ
            sum+=2; //MagOfsX
            sum+=2; //MagOfsY
            sum+=2; //MagOfsZ
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("mag_ofs_x",
"Magnetometer X offset.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("mag_ofs_y",
"Magnetometer Y offset.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("mag_ofs_z",
"Magnetometer Z offset.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "SET_MAG_OFFSETS:"
        + "int16_t mag_ofs_x;"
        + "int16_t mag_ofs_y;"
        + "int16_t mag_ofs_z;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //MagOfsX
            sum+=2; //MagOfsY
            sum+=2; //MagOfsZ
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("brkval",
"Heap top.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("freemem",
"Free memory.",
string.Empty, 
@"bytes", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("freemem32",
"Free memory (32 bit).",
string.Empty, 
@"bytes", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
true),
        ];
        public const string FormatMessage = "MEMINFO:"
        + "uint16_t brkval;"
        + "uint16_t freemem;"
        + "uint32_t freemem32;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //Brkval
            sum+=2; //Freemem
            sum+=4; //Freemem32
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("adc1",
"ADC output 1.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("adc2",
"ADC output 2.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("adc3",
"ADC output 3.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("adc4",
"ADC output 4.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("adc5",
"ADC output 5.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("adc6",
"ADC output 6.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
        ];
        public const string FormatMessage = "AP_ADC:"
        + "uint16_t adc1;"
        + "uint16_t adc2;"
        + "uint16_t adc3;"
        + "uint16_t adc4;"
        + "uint16_t adc5;"
        + "uint16_t adc6;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //Adc1
            sum+=2; //Adc2
            sum+=2; //Adc3
            sum+=2; //Adc4
            sum+=2; //Adc5
            sum+=2; //Adc6
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("extra_value",
"Correspondent value to given extra_param.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("shutter_speed",
"Divisor number //e.g. 1000 means 1/1000 (0 means ignore).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("mode",
"Mode enumeration from 1 to N //P, TV, AV, M, etc. (0 means ignore).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("aperture",
"F stop number x 10 //e.g. 28 means 2.8 (0 means ignore).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("iso",
"ISO enumeration from 1 to N //e.g. 80, 100, 200, Etc (0 means ignore).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("exposure_type",
"Exposure type enumeration from 1 to N (0 means ignore).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("command_id",
"Command Identity (incremental loop: 0 to 255). //A command sent multiple times will be executed or pooled just once.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("engine_cut_off",
"Main engine cut-off time before camera trigger (0 means no cut-off).",
string.Empty, 
@"ds", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("extra_param",
"Extra parameters enumeration (0 means ignore).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "DIGICAM_CONFIGURE:"
        + "float extra_value;"
        + "uint16_t shutter_speed;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t mode;"
        + "uint8_t aperture;"
        + "uint8_t iso;"
        + "uint8_t exposure_type;"
        + "uint8_t command_id;"
        + "uint8_t engine_cut_off;"
        + "uint8_t extra_param;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //ExtraValue
            sum+=2; //ShutterSpeed
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //Mode
            sum+=1; //Aperture
            sum+=1; //Iso
            sum+=1; //ExposureType
            sum+=1; //CommandId
            sum+=1; //EngineCutOff
            sum+=1; //ExtraParam
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("extra_value",
"Correspondent value to given extra_param.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("session",
"0: stop, 1: start or keep it up //Session control e.g. show/hide lens.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("zoom_pos",
"1 to N //Zoom's absolute position (0 means ignore).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("zoom_step",
"-100 to 100 //Zooming step value to offset zoom from the current position.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int8, 
            0, 
false),
            new("focus_lock",
"0: unlock focus or keep unlocked, 1: lock focus or keep locked, 3: re-lock focus.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("shot",
"0: ignore, 1: shot or start filming.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("command_id",
"Command Identity (incremental loop: 0 to 255)//A command sent multiple times will be executed or pooled just once.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("extra_param",
"Extra parameters enumeration (0 means ignore).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "DIGICAM_CONTROL:"
        + "float extra_value;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t session;"
        + "uint8_t zoom_pos;"
        + "int8_t zoom_step;"
        + "uint8_t focus_lock;"
        + "uint8_t shot;"
        + "uint8_t command_id;"
        + "uint8_t extra_param;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //ExtraValue
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //Session
            sum+=1; //ZoomPos
            sum+=1; //ZoomStep
            sum+=1; //FocusLock
            sum+=1; //Shot
            sum+=1; //CommandId
            sum+=1; //ExtraParam
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("mount_mode",
"Mount operating mode.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("stab_roll",
"(1 = yes, 0 = no).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("stab_pitch",
"(1 = yes, 0 = no).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("stab_yaw",
"(1 = yes, 0 = no).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "MOUNT_CONFIGURE:"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t mount_mode;"
        + "uint8_t stab_roll;"
        + "uint8_t stab_pitch;"
        + "uint8_t stab_yaw;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+= 1; // MountMode
            sum+=1; //StabRoll
            sum+=1; //StabPitch
            sum+=1; //StabYaw
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("input_a",
"Pitch (centi-degrees) or lat (degE7), depending on mount mode.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("input_b",
"Roll (centi-degrees) or lon (degE7) depending on mount mode.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("input_c",
"Yaw (centi-degrees) or alt (cm) depending on mount mode.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("save_position",
"If \"1\" it will save current trimmed position on EEPROM (just valid for NEUTRAL and LANDING).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "MOUNT_CONTROL:"
        + "int32_t input_a;"
        + "int32_t input_b;"
        + "int32_t input_c;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t save_position;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //InputA
            sum+=4; //InputB
            sum+=4; //InputC
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //SavePosition
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("pointing_a",
"Pitch.",
string.Empty, 
@"cdeg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("pointing_b",
"Roll.",
string.Empty, 
@"cdeg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("pointing_c",
"Yaw.",
string.Empty, 
@"cdeg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("mount_mode",
"Mount operating mode.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
true),
        ];
        public const string FormatMessage = "MOUNT_STATUS:"
        + "int32_t pointing_a;"
        + "int32_t pointing_b;"
        + "int32_t pointing_c;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t mount_mode;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //PointingA
            sum+=4; //PointingB
            sum+=4; //PointingC
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+= 1; // MountMode
            return (byte)sum;
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
        /// <summary>
        /// Mount operating mode.
        /// OriginName: mount_mode, Units: , IsExtended: true
        /// </summary>
        public MavMountMode MountMode { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("lat",
"Latitude of point.",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("lng",
"Longitude of point.",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("idx",
"Point index (first point is 1, 0 is for return point).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("count",
"Total number of points (for sanity checking).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "FENCE_POINT:"
        + "float lat;"
        + "float lng;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t idx;"
        + "uint8_t count;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Lat
            sum+=4; //Lng
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //Idx
            sum+=1; //Count
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("idx",
"Point index (first point is 1, 0 is for return point).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "FENCE_FETCH_POINT:"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t idx;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //Idx
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("omegaIx",
"X gyro drift estimate.",
string.Empty, 
@"rad/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("omegaIy",
"Y gyro drift estimate.",
string.Empty, 
@"rad/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("omegaIz",
"Z gyro drift estimate.",
string.Empty, 
@"rad/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("accel_weight",
"Average accel_weight.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("renorm_val",
"Average renormalisation value.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("error_rp",
"Average error_roll_pitch value.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("error_yaw",
"Average error_yaw value.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
        ];
        public const string FormatMessage = "AHRS:"
        + "float omegaIx;"
        + "float omegaIy;"
        + "float omegaIz;"
        + "float accel_weight;"
        + "float renorm_val;"
        + "float error_rp;"
        + "float error_yaw;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Omegaix
            sum+=4; //Omegaiy
            sum+=4; //Omegaiz
            sum+=4; //AccelWeight
            sum+=4; //RenormVal
            sum+=4; //ErrorRp
            sum+=4; //ErrorYaw
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("roll",
"Roll angle.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("pitch",
"Pitch angle.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("yaw",
"Yaw angle.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("xacc",
"X acceleration.",
string.Empty, 
@"m/s/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("yacc",
"Y acceleration.",
string.Empty, 
@"m/s/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("zacc",
"Z acceleration.",
string.Empty, 
@"m/s/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("xgyro",
"Angular speed around X axis.",
string.Empty, 
@"rad/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("ygyro",
"Angular speed around Y axis.",
string.Empty, 
@"rad/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("zgyro",
"Angular speed around Z axis.",
string.Empty, 
@"rad/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("lat",
"Latitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("lng",
"Longitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
        ];
        public const string FormatMessage = "SIMSTATE:"
        + "float roll;"
        + "float pitch;"
        + "float yaw;"
        + "float xacc;"
        + "float yacc;"
        + "float zacc;"
        + "float xgyro;"
        + "float ygyro;"
        + "float zgyro;"
        + "int32_t lat;"
        + "int32_t lng;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Roll
            sum+=4; //Pitch
            sum+=4; //Yaw
            sum+=4; //Xacc
            sum+=4; //Yacc
            sum+=4; //Zacc
            sum+=4; //Xgyro
            sum+=4; //Ygyro
            sum+=4; //Zgyro
            sum+=4; //Lat
            sum+=4; //Lng
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("Vcc",
"Board voltage.",
string.Empty, 
@"mV", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("I2Cerr",
"I2C error count.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "HWSTATUS:"
        + "uint16_t Vcc;"
        + "uint8_t I2Cerr;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //Vcc
            sum+=1; //I2cerr
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("rxerrors",
"Receive errors.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("fixed",
"Count of error corrected packets.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("rssi",
"Local signal strength.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("remrssi",
"Remote signal strength.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("txbuf",
"How full the tx buffer is.",
string.Empty, 
@"%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("noise",
"Background noise level.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("remnoise",
"Remote background noise level.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "RADIO:"
        + "uint16_t rxerrors;"
        + "uint16_t fixed;"
        + "uint8_t rssi;"
        + "uint8_t remrssi;"
        + "uint8_t txbuf;"
        + "uint8_t noise;"
        + "uint8_t remnoise;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //Rxerrors
            sum+=2; //Fixed
            sum+=1; //Rssi
            sum+=1; //Remrssi
            sum+=1; //Txbuf
            sum+=1; //Noise
            sum+=1; //Remnoise
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("last_trigger",
"Time (since boot) of last breach.",
string.Empty, 
@"ms", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("last_action",
"Time (since boot) of last recovery action.",
string.Empty, 
@"ms", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("last_recovery",
"Time (since boot) of last successful recovery.",
string.Empty, 
@"ms", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("last_clear",
"Time (since boot) of last all-clear.",
string.Empty, 
@"ms", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("breach_count",
"Number of fence breaches.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("limits_state",
"State of AP_Limits.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("mods_enabled",
"AP_Limit_Module bitfield of enabled modules.",
string.Empty, 
string.Empty, 
"bitmask", 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("mods_required",
"AP_Limit_Module bitfield of required modules.",
string.Empty, 
string.Empty, 
"bitmask", 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("mods_triggered",
"AP_Limit_Module bitfield of triggered modules.",
string.Empty, 
string.Empty, 
"bitmask", 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "LIMITS_STATUS:"
        + "uint32_t last_trigger;"
        + "uint32_t last_action;"
        + "uint32_t last_recovery;"
        + "uint32_t last_clear;"
        + "uint16_t breach_count;"
        + "uint8_t limits_state;"
        + "uint8_t mods_enabled;"
        + "uint8_t mods_required;"
        + "uint8_t mods_triggered;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //LastTrigger
            sum+=4; //LastAction
            sum+=4; //LastRecovery
            sum+=4; //LastClear
            sum+=2; //BreachCount
            sum+= 1; // LimitsState
            sum+= 1; // ModsEnabled
            sum+= 1; // ModsRequired
            sum+= 1; // ModsTriggered
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("direction",
"Wind direction (that wind is coming from).",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("speed",
"Wind speed in ground plane.",
string.Empty, 
@"m/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("speed_z",
"Vertical wind speed.",
string.Empty, 
@"m/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
        ];
        public const string FormatMessage = "WIND:"
        + "float direction;"
        + "float speed;"
        + "float speed_z;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Direction
            sum+=4; //Speed
            sum+=4; //SpeedZ
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("type",
"Data type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("len",
"Data length.",
string.Empty, 
@"bytes", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("data",
"Raw data.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            16, 
false),
        ];
        public const string FormatMessage = "DATA16:"
        + "uint8_t type;"
        + "uint8_t len;"
        + "uint8_t[16] data;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //Type
            sum+=1; //Len
            sum+=Data.Length; //Data
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Type = (byte)BinSerialize.ReadByte(ref buffer);
            Len = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/16 - Math.Max(0,((/*PayloadByteSize*/18 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
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
        public const int DataMaxItemsCount = 16;
        public byte[] Data { get; set; } = new byte[16];
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("type",
"Data type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("len",
"Data length.",
string.Empty, 
@"bytes", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("data",
"Raw data.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            32, 
false),
        ];
        public const string FormatMessage = "DATA32:"
        + "uint8_t type;"
        + "uint8_t len;"
        + "uint8_t[32] data;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //Type
            sum+=1; //Len
            sum+=Data.Length; //Data
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Type = (byte)BinSerialize.ReadByte(ref buffer);
            Len = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/34 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
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
        public const int DataMaxItemsCount = 32;
        public byte[] Data { get; set; } = new byte[32];
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("type",
"Data type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("len",
"Data length.",
string.Empty, 
@"bytes", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("data",
"Raw data.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            64, 
false),
        ];
        public const string FormatMessage = "DATA64:"
        + "uint8_t type;"
        + "uint8_t len;"
        + "uint8_t[64] data;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //Type
            sum+=1; //Len
            sum+=Data.Length; //Data
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Type = (byte)BinSerialize.ReadByte(ref buffer);
            Len = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/64 - Math.Max(0,((/*PayloadByteSize*/66 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
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
        public const int DataMaxItemsCount = 64;
        public byte[] Data { get; set; } = new byte[64];
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("type",
"Data type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("len",
"Data length.",
string.Empty, 
@"bytes", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("data",
"Raw data.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            96, 
false),
        ];
        public const string FormatMessage = "DATA96:"
        + "uint8_t type;"
        + "uint8_t len;"
        + "uint8_t[96] data;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //Type
            sum+=1; //Len
            sum+=Data.Length; //Data
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Type = (byte)BinSerialize.ReadByte(ref buffer);
            Len = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/96 - Math.Max(0,((/*PayloadByteSize*/98 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
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
        public const int DataMaxItemsCount = 96;
        public byte[] Data { get; set; } = new byte[96];
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("distance",
"Distance.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("voltage",
"Raw voltage if available, zero otherwise.",
string.Empty, 
@"V", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
        ];
        public const string FormatMessage = "RANGEFINDER:"
        + "float distance;"
        + "float voltage;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Distance
            sum+=4; //Voltage
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("vx",
"GPS velocity north.",
string.Empty, 
@"m/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("vy",
"GPS velocity east.",
string.Empty, 
@"m/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("vz",
"GPS velocity down.",
string.Empty, 
@"m/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("diff_pressure",
"Differential pressure.",
string.Empty, 
@"Pa", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("EAS2TAS",
"Estimated to true airspeed ratio.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("ratio",
"Airspeed ratio.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("state_x",
"EKF state x.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("state_y",
"EKF state y.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("state_z",
"EKF state z.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("Pax",
"EKF Pax.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("Pby",
"EKF Pby.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("Pcz",
"EKF Pcz.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
        ];
        public const string FormatMessage = "AIRSPEED_AUTOCAL:"
        + "float vx;"
        + "float vy;"
        + "float vz;"
        + "float diff_pressure;"
        + "float EAS2TAS;"
        + "float ratio;"
        + "float state_x;"
        + "float state_y;"
        + "float state_z;"
        + "float Pax;"
        + "float Pby;"
        + "float Pcz;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Vx
            sum+=4; //Vy
            sum+=4; //Vz
            sum+=4; //DiffPressure
            sum+=4; //Eas2tas
            sum+=4; //Ratio
            sum+=4; //StateX
            sum+=4; //StateY
            sum+=4; //StateZ
            sum+=4; //Pax
            sum+=4; //Pby
            sum+=4; //Pcz
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("lat",
"Latitude of point.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("lng",
"Longitude of point.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("alt",
"Transit / loiter altitude relative to home.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("break_alt",
"Break altitude relative to home.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("land_dir",
"Heading to aim for when landing.",
string.Empty, 
@"cdeg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("idx",
"Point index (first point is 0).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("count",
"Total number of points (for sanity checking).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("flags",
"Configuration flags.",
string.Empty, 
string.Empty, 
"bitmask", 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "RALLY_POINT:"
        + "int32_t lat;"
        + "int32_t lng;"
        + "int16_t alt;"
        + "int16_t break_alt;"
        + "uint16_t land_dir;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t idx;"
        + "uint8_t count;"
        + "uint8_t flags;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Lat
            sum+=4; //Lng
            sum+=2; //Alt
            sum+=2; //BreakAlt
            sum+=2; //LandDir
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //Idx
            sum+=1; //Count
            sum+= 1; // Flags
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("idx",
"Point index (first point is 0).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "RALLY_FETCH_POINT:"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t idx;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //Idx
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("current",
"Current.",
string.Empty, 
@"A", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("CompensationX",
"Motor Compensation X.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("CompensationY",
"Motor Compensation Y.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("CompensationZ",
"Motor Compensation Z.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("throttle",
"Throttle.",
string.Empty, 
@"d%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("interference",
"Interference.",
string.Empty, 
@"%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
        ];
        public const string FormatMessage = "COMPASSMOT_STATUS:"
        + "float current;"
        + "float CompensationX;"
        + "float CompensationY;"
        + "float CompensationZ;"
        + "uint16_t throttle;"
        + "uint16_t interference;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Current
            sum+=4; //Compensationx
            sum+=4; //Compensationy
            sum+=4; //Compensationz
            sum+=2; //Throttle
            sum+=2; //Interference
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("roll",
"Roll angle.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("pitch",
"Pitch angle.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("yaw",
"Yaw angle.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("altitude",
"Altitude (MSL).",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("lat",
"Latitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("lng",
"Longitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
        ];
        public const string FormatMessage = "AHRS2:"
        + "float roll;"
        + "float pitch;"
        + "float yaw;"
        + "float altitude;"
        + "int32_t lat;"
        + "int32_t lng;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Roll
            sum+=4; //Pitch
            sum+=4; //Yaw
            sum+=4; //Altitude
            sum+=4; //Lat
            sum+=4; //Lng
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_usec",
"Image timestamp (since UNIX epoch, according to camera clock).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("p1",
"Parameter 1 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("p2",
"Parameter 2 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("p3",
"Parameter 3 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("p4",
"Parameter 4 (meaning depends on event_id, see CAMERA_STATUS_TYPES enum).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("img_idx",
"Image index.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("cam_idx",
"Camera ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("event_id",
"Event type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "CAMERA_STATUS:"
        + "uint64_t time_usec;"
        + "float p1;"
        + "float p2;"
        + "float p3;"
        + "float p4;"
        + "uint16_t img_idx;"
        + "uint8_t target_system;"
        + "uint8_t cam_idx;"
        + "uint8_t event_id;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUsec
            sum+=4; //P1
            sum+=4; //P2
            sum+=4; //P3
            sum+=4; //P4
            sum+=2; //ImgIdx
            sum+=1; //TargetSystem
            sum+=1; //CamIdx
            sum+= 1; // EventId
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_usec",
"Image timestamp (since UNIX epoch), as passed in by CAMERA_STATUS message (or autopilot if no CCB).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("lat",
"Latitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("lng",
"Longitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("alt_msl",
"Altitude (MSL).",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("alt_rel",
"Altitude (Relative to HOME location).",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("roll",
"Camera Roll angle (earth frame, +-180).",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("pitch",
"Camera Pitch angle (earth frame, +-180).",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("yaw",
"Camera Yaw (earth frame, 0-360, true).",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("foc_len",
"Focal Length.",
string.Empty, 
@"mm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("img_idx",
"Image index.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("cam_idx",
"Camera ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("flags",
"Feedback flags.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("completed_captures",
"Completed image captures.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
true),
        ];
        public const string FormatMessage = "CAMERA_FEEDBACK:"
        + "uint64_t time_usec;"
        + "int32_t lat;"
        + "int32_t lng;"
        + "float alt_msl;"
        + "float alt_rel;"
        + "float roll;"
        + "float pitch;"
        + "float yaw;"
        + "float foc_len;"
        + "uint16_t img_idx;"
        + "uint8_t target_system;"
        + "uint8_t cam_idx;"
        + "uint8_t flags;"
        + "uint16_t completed_captures;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUsec
            sum+=4; //Lat
            sum+=4; //Lng
            sum+=4; //AltMsl
            sum+=4; //AltRel
            sum+=4; //Roll
            sum+=4; //Pitch
            sum+=4; //Yaw
            sum+=4; //FocLen
            sum+=2; //ImgIdx
            sum+=1; //TargetSystem
            sum+=1; //CamIdx
            sum+= 1; // Flags
            sum+=2; //CompletedCaptures
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("voltage",
"Voltage.",
string.Empty, 
@"mV", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("current_battery",
"Battery current, -1: autopilot does not measure the current.",
string.Empty, 
@"cA", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
        ];
        public const string FormatMessage = "BATTERY2:"
        + "uint16_t voltage;"
        + "int16_t current_battery;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //Voltage
            sum+=2; //CurrentBattery
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("roll",
"Roll angle.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("pitch",
"Pitch angle.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("yaw",
"Yaw angle.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("altitude",
"Altitude (MSL).",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("lat",
"Latitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("lng",
"Longitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("v1",
"Test variable1.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("v2",
"Test variable2.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("v3",
"Test variable3.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("v4",
"Test variable4.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
        ];
        public const string FormatMessage = "AHRS3:"
        + "float roll;"
        + "float pitch;"
        + "float yaw;"
        + "float altitude;"
        + "int32_t lat;"
        + "int32_t lng;"
        + "float v1;"
        + "float v2;"
        + "float v3;"
        + "float v4;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Roll
            sum+=4; //Pitch
            sum+=4; //Yaw
            sum+=4; //Altitude
            sum+=4; //Lat
            sum+=4; //Lng
            sum+=4; //V1
            sum+=4; //V2
            sum+=4; //V3
            sum+=4; //V4
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "AUTOPILOT_VERSION_REQUEST:"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("seqno",
"LoggerFactory data block sequence number.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("data",
"LoggerFactory data block.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            200, 
false),
        ];
        public const string FormatMessage = "REMOTE_LOG_DATA_BLOCK:"
        + "uint32_t seqno;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t[200] data;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 4; // Seqno
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=Data.Length; //Data
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Seqno = (MavRemoteLogDataBlockCommands)BinSerialize.ReadUInt(ref buffer);
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/200 - Math.Max(0,((/*PayloadByteSize*/206 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
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
        
        



        /// <summary>
        /// LoggerFactory data block sequence number.
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
        /// LoggerFactory data block.
        /// OriginName: data, Units: , IsExtended: false
        /// </summary>
        public const int DataMaxItemsCount = 200;
        public byte[] Data { get; set; } = new byte[200];
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("seqno",
"LoggerFactory data block sequence number.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("status",
"LoggerFactory data block status.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "REMOTE_LOG_BLOCK_STATUS:"
        + "uint32_t seqno;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t status;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Seqno
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+= 1; // Status
            return (byte)sum;
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
        
        



        /// <summary>
        /// LoggerFactory data block sequence number.
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
        /// LoggerFactory data block status.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public MavRemoteLogDataBlockStatuses Status { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("instance",
"Instance (LED instance to control or 255 for all LEDs).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("pattern",
"Pattern (see LED_PATTERN_ENUM).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("custom_len",
"Custom Byte Length.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("custom_bytes",
"Custom Bytes.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            24, 
false),
        ];
        public const string FormatMessage = "LED_CONTROL:"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t instance;"
        + "uint8_t pattern;"
        + "uint8_t custom_len;"
        + "uint8_t[24] custom_bytes;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //Instance
            sum+=1; //Pattern
            sum+=1; //CustomLen
            sum+=CustomBytes.Length; //CustomBytes
            return (byte)sum;
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
            CustomBytes = new byte[arraySize];
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
        public const int CustomBytesMaxItemsCount = 24;
        public byte[] CustomBytes { get; set; } = new byte[24];
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("direction_x",
"Body frame direction vector for display.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("direction_y",
"Body frame direction vector for display.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("direction_z",
"Body frame direction vector for display.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("compass_id",
"Compass being calibrated.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("cal_mask",
"Bitmask of compasses being calibrated.",
string.Empty, 
string.Empty, 
"bitmask", 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("cal_status",
"Calibration Status.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("attempt",
"Attempt number.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("completion_pct",
"Completion percentage.",
string.Empty, 
@"%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("completion_mask",
"Bitmask of sphere sections (see http://en.wikipedia.org/wiki/Geodesic_grid).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            10, 
false),
        ];
        public const string FormatMessage = "MAG_CAL_PROGRESS:"
        + "float direction_x;"
        + "float direction_y;"
        + "float direction_z;"
        + "uint8_t compass_id;"
        + "uint8_t cal_mask;"
        + "uint8_t cal_status;"
        + "uint8_t attempt;"
        + "uint8_t completion_pct;"
        + "uint8_t[10] completion_mask;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //DirectionX
            sum+=4; //DirectionY
            sum+=4; //DirectionZ
            sum+=1; //CompassId
            sum+=1; //CalMask
            sum+= 1; // CalStatus
            sum+=1; //Attempt
            sum+=1; //CompletionPct
            sum+=CompletionMask.Length; //CompletionMask
            return (byte)sum;
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
            CompletionMask = new byte[arraySize];
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
        public const int CompletionMaskMaxItemsCount = 10;
        public byte[] CompletionMask { get; set; } = new byte[10];
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("velocity_variance",
"Velocity variance.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("pos_horiz_variance",
"Horizontal Position variance.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("pos_vert_variance",
"Vertical Position variance.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("compass_variance",
"Compass variance.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("terrain_alt_variance",
"Terrain Altitude variance.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("flags",
"Flags.",
string.Empty, 
string.Empty, 
"bitmask", 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("airspeed_variance",
"Airspeed variance.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
true),
        ];
        public const string FormatMessage = "EKF_STATUS_REPORT:"
        + "float velocity_variance;"
        + "float pos_horiz_variance;"
        + "float pos_vert_variance;"
        + "float compass_variance;"
        + "float terrain_alt_variance;"
        + "uint16_t flags;"
        + "float airspeed_variance;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //VelocityVariance
            sum+=4; //PosHorizVariance
            sum+=4; //PosVertVariance
            sum+=4; //CompassVariance
            sum+=4; //TerrainAltVariance
            sum+= 2; // Flags
            sum+=4; //AirspeedVariance
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("desired",
"Desired rate.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("achieved",
"Achieved rate.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("FF",
"FF component.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("P",
"P component.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("I",
"I component.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("D",
"D component.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("axis",
"Axis.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("SRate",
"Slew rate.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
true),
            new("PDmod",
"P/D oscillation modifier.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
true),
        ];
        public const string FormatMessage = "PID_TUNING:"
        + "float desired;"
        + "float achieved;"
        + "float FF;"
        + "float P;"
        + "float I;"
        + "float D;"
        + "uint8_t axis;"
        + "float SRate;"
        + "float PDmod;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Desired
            sum+=4; //Achieved
            sum+=4; //Ff
            sum+=4; //P
            sum+=4; //I
            sum+=4; //D
            sum+= 1; // Axis
            sum+=4; //Srate
            sum+=4; //Pdmod
            return (byte)sum;
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
        
        



        /// <summary>
        /// Desired rate.
        /// OriginName: desired, Units: , IsExtended: false
        /// </summary>
        public float Desired { get; set; }
        /// <summary>
        /// Achieved rate.
        /// OriginName: achieved, Units: , IsExtended: false
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
        /// <summary>
        /// Slew rate.
        /// OriginName: SRate, Units: , IsExtended: true
        /// </summary>
        public float Srate { get; set; }
        /// <summary>
        /// P/D oscillation modifier.
        /// OriginName: PDmod, Units: , IsExtended: true
        /// </summary>
        public float Pdmod { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("landing_lat",
"Landing latitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("landing_lon",
"Landing longitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("path_lat",
"Final heading start point, latitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("path_lon",
"Final heading start point, longitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("arc_entry_lat",
"Arc entry point, latitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("arc_entry_lon",
"Arc entry point, longitude.",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("altitude",
"Altitude.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("expected_travel_distance",
"Distance the aircraft expects to travel during the deepstall.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("cross_track_error",
"Deepstall cross track error (only valid when in DEEPSTALL_STAGE_LAND).",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("stage",
"Deepstall stage.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "DEEPSTALL:"
        + "int32_t landing_lat;"
        + "int32_t landing_lon;"
        + "int32_t path_lat;"
        + "int32_t path_lon;"
        + "int32_t arc_entry_lat;"
        + "int32_t arc_entry_lon;"
        + "float altitude;"
        + "float expected_travel_distance;"
        + "float cross_track_error;"
        + "uint8_t stage;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //LandingLat
            sum+=4; //LandingLon
            sum+=4; //PathLat
            sum+=4; //PathLon
            sum+=4; //ArcEntryLat
            sum+=4; //ArcEntryLon
            sum+=4; //Altitude
            sum+=4; //ExpectedTravelDistance
            sum+=4; //CrossTrackError
            sum+= 1; // Stage
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("delta_time",
"Time since last update.",
string.Empty, 
@"s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("delta_angle_x",
"Delta angle X.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("delta_angle_y",
"Delta angle Y.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("delta_angle_z",
"Delta angle X.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("delta_velocity_x",
"Delta velocity X.",
string.Empty, 
@"m/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("delta_velocity_y",
"Delta velocity Y.",
string.Empty, 
@"m/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("delta_velocity_z",
"Delta velocity Z.",
string.Empty, 
@"m/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("joint_roll",
"Joint ROLL.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("joint_el",
"Joint EL.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("joint_az",
"Joint AZ.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "GIMBAL_REPORT:"
        + "float delta_time;"
        + "float delta_angle_x;"
        + "float delta_angle_y;"
        + "float delta_angle_z;"
        + "float delta_velocity_x;"
        + "float delta_velocity_y;"
        + "float delta_velocity_z;"
        + "float joint_roll;"
        + "float joint_el;"
        + "float joint_az;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //DeltaTime
            sum+=4; //DeltaAngleX
            sum+=4; //DeltaAngleY
            sum+=4; //DeltaAngleZ
            sum+=4; //DeltaVelocityX
            sum+=4; //DeltaVelocityY
            sum+=4; //DeltaVelocityZ
            sum+=4; //JointRoll
            sum+=4; //JointEl
            sum+=4; //JointAz
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("demanded_rate_x",
"Demanded angular rate X.",
string.Empty, 
@"rad/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("demanded_rate_y",
"Demanded angular rate Y.",
string.Empty, 
@"rad/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("demanded_rate_z",
"Demanded angular rate Z.",
string.Empty, 
@"rad/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "GIMBAL_CONTROL:"
        + "float demanded_rate_x;"
        + "float demanded_rate_y;"
        + "float demanded_rate_z;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //DemandedRateX
            sum+=4; //DemandedRateY
            sum+=4; //DemandedRateZ
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("rl_torque_cmd",
"Roll Torque Command.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("el_torque_cmd",
"Elevation Torque Command.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("az_torque_cmd",
"Azimuth Torque Command.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "GIMBAL_TORQUE_CMD_REPORT:"
        + "int16_t rl_torque_cmd;"
        + "int16_t el_torque_cmd;"
        + "int16_t az_torque_cmd;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //RlTorqueCmd
            sum+=2; //ElTorqueCmd
            sum+=2; //AzTorqueCmd
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("status",
"Status.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("capture_mode",
"Current capture mode.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("flags",
"Additional status bits.",
string.Empty, 
string.Empty, 
"bitmask", 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "GOPRO_HEARTBEAT:"
        + "uint8_t status;"
        + "uint8_t capture_mode;"
        + "uint8_t flags;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 1; // Status
            sum+= 1; // CaptureMode
            sum+= 1; // Flags
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("cmd_id",
"Command ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "GOPRO_GET_REQUEST:"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t cmd_id;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+= 1; // CmdId
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("cmd_id",
"Command ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("status",
"Status.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("value",
"Value.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            4, 
false),
        ];
        public const string FormatMessage = "GOPRO_GET_RESPONSE:"
        + "uint8_t cmd_id;"
        + "uint8_t status;"
        + "uint8_t[4] value;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 1; // CmdId
            sum+= 1; // Status
            sum+=Value.Length; //Value
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            CmdId = (GoproCommand)BinSerialize.ReadByte(ref buffer);
            Status = (GoproRequestStatus)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/6 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Value = new byte[arraySize];
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
        public const int ValueMaxItemsCount = 4;
        public byte[] Value { get; set; } = new byte[4];
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("cmd_id",
"Command ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("value",
"Value.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            4, 
false),
        ];
        public const string FormatMessage = "GOPRO_SET_REQUEST:"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t cmd_id;"
        + "uint8_t[4] value;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+= 1; // CmdId
            sum+=Value.Length; //Value
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TargetSystem = (byte)BinSerialize.ReadByte(ref buffer);
            TargetComponent = (byte)BinSerialize.ReadByte(ref buffer);
            CmdId = (GoproCommand)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/7 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            Value = new byte[arraySize];
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
        public const int ValueMaxItemsCount = 4;
        public byte[] Value { get; set; } = new byte[4];
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("cmd_id",
"Command ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("status",
"Status.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "GOPRO_SET_RESPONSE:"
        + "uint8_t cmd_id;"
        + "uint8_t status;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 1; // CmdId
            sum+= 1; // Status
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("rpm1",
"RPM Sensor1.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("rpm2",
"RPM Sensor2.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
        ];
        public const string FormatMessage = "RPM:"
        + "float rpm1;"
        + "float rpm2;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Rpm1
            sum+=4; //Rpm2
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("request_id",
"Request ID - copied to reply.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("bustype",
"The bus type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("bus",
"Bus number.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("address",
"Bus address.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("busname",
"Name of device on bus (for SPI).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Char, 
            40, 
false),
            new("regstart",
"First register to read.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("count",
"Count of registers to read.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("bank",
"Bank number.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
true),
        ];
        public const string FormatMessage = "DEVICE_OP_READ:"
        + "uint32_t request_id;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t bustype;"
        + "uint8_t bus;"
        + "uint8_t address;"
        + "char[40] busname;"
        + "uint8_t regstart;"
        + "uint8_t count;"
        + "uint8_t bank;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //RequestId
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+= 1; // Bustype
            sum+=1; //Bus
            sum+=1; //Address
            sum+=Busname.Length; //Busname
            sum+=1; //Regstart
            sum+=1; //Count
            sum+=1; //Bank
            return (byte)sum;
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
        public const int BusnameMaxItemsCount = 40;
        public char[] Busname { get; set; } = new char[40];
        [Obsolete("This method is deprecated. Use GetBusnameMaxItemsCount instead.")]
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
        /// <summary>
        /// Bank number.
        /// OriginName: bank, Units: , IsExtended: true
        /// </summary>
        public byte Bank { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("request_id",
"Request ID - copied from request.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("result",
"0 for success, anything else is failure code.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("regstart",
"Starting register.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("count",
"Count of bytes read.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("data",
"Reply data.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            128, 
false),
            new("bank",
"Bank number.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
true),
        ];
        public const string FormatMessage = "DEVICE_OP_READ_REPLY:"
        + "uint32_t request_id;"
        + "uint8_t result;"
        + "uint8_t regstart;"
        + "uint8_t count;"
        + "uint8_t[128] data;"
        + "uint8_t bank;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //RequestId
            sum+=1; //Result
            sum+=1; //Regstart
            sum+=1; //Count
            sum+=Data.Length; //Data
            sum+=1; //Bank
            return (byte)sum;
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
            Data = new byte[arraySize];
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
        public const int DataMaxItemsCount = 128;
        public byte[] Data { get; set; } = new byte[128];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 128;
        /// <summary>
        /// Bank number.
        /// OriginName: bank, Units: , IsExtended: true
        /// </summary>
        public byte Bank { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("request_id",
"Request ID - copied to reply.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("bustype",
"The bus type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("bus",
"Bus number.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("address",
"Bus address.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("busname",
"Name of device on bus (for SPI).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Char, 
            40, 
false),
            new("regstart",
"First register to write.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("count",
"Count of registers to write.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("data",
"Write data.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            128, 
false),
            new("bank",
"Bank number.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
true),
        ];
        public const string FormatMessage = "DEVICE_OP_WRITE:"
        + "uint32_t request_id;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t bustype;"
        + "uint8_t bus;"
        + "uint8_t address;"
        + "char[40] busname;"
        + "uint8_t regstart;"
        + "uint8_t count;"
        + "uint8_t[128] data;"
        + "uint8_t bank;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //RequestId
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+= 1; // Bustype
            sum+=1; //Bus
            sum+=1; //Address
            sum+=Busname.Length; //Busname
            sum+=1; //Regstart
            sum+=1; //Count
            sum+=Data.Length; //Data
            sum+=1; //Bank
            return (byte)sum;
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
            buffer = buffer.Slice(arraySize);
           
            Regstart = (byte)BinSerialize.ReadByte(ref buffer);
            Count = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = /*ArrayLength*/128 - Math.Max(0,((/*PayloadByteSize*/180 - payloadSize - /*ExtendedFieldsLength*/1)/1 /*FieldTypeByteSize*/));
            Data = new byte[arraySize];
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
        public const int BusnameMaxItemsCount = 40;
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
        public const int DataMaxItemsCount = 128;
        public byte[] Data { get; set; } = new byte[128];
        [Obsolete("This method is deprecated. Use GetDataMaxItemsCount instead.")]
        public byte GetDataMaxItemsCount() => 128;
        /// <summary>
        /// Bank number.
        /// OriginName: bank, Units: , IsExtended: true
        /// </summary>
        public byte Bank { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("request_id",
"Request ID - copied from request.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("result",
"0 for success, anything else is failure code.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "DEVICE_OP_WRITE_REPLY:"
        + "uint32_t request_id;"
        + "uint8_t result;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //RequestId
            sum+=1; //Result
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("desired",
"Desired rate.",
string.Empty, 
@"deg/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("achieved",
"Achieved rate.",
string.Empty, 
@"deg/s", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("error",
"Error between model and vehicle.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("theta",
"Theta estimated state predictor.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("omega",
"Omega estimated state predictor.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("sigma",
"Sigma estimated state predictor.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("theta_dot",
"Theta derivative.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("omega_dot",
"Omega derivative.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("sigma_dot",
"Sigma derivative.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("f",
"Projection operator value.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("f_dot",
"Projection operator derivative.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("u",
"u adaptive controlled output command.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("axis",
"Axis.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "ADAP_TUNING:"
        + "float desired;"
        + "float achieved;"
        + "float error;"
        + "float theta;"
        + "float omega;"
        + "float sigma;"
        + "float theta_dot;"
        + "float omega_dot;"
        + "float sigma_dot;"
        + "float f;"
        + "float f_dot;"
        + "float u;"
        + "uint8_t axis;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Desired
            sum+=4; //Achieved
            sum+=4; //Error
            sum+=4; //Theta
            sum+=4; //Omega
            sum+=4; //Sigma
            sum+=4; //ThetaDot
            sum+=4; //OmegaDot
            sum+=4; //SigmaDot
            sum+=4; //F
            sum+=4; //FDot
            sum+=4; //U
            sum+= 1; // Axis
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_usec",
"Timestamp (synced to UNIX time or since system boot).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("time_delta_usec",
"Time since the last reported camera frame.",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("angle_delta",
"Defines a rotation vector [roll, pitch, yaw] to the current MAV_FRAME_BODY_FRD from the previous MAV_FRAME_BODY_FRD.",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            3, 
false),
            new("position_delta",
"Change in position to the current MAV_FRAME_BODY_FRD from the previous FRAME_BODY_FRD rotated to the current MAV_FRAME_BODY_FRD.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            3, 
false),
            new("confidence",
"Normalised confidence value from 0 to 100.",
string.Empty, 
@"%", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
        ];
        public const string FormatMessage = "VISION_POSITION_DELTA:"
        + "uint64_t time_usec;"
        + "uint64_t time_delta_usec;"
        + "float[3] angle_delta;"
        + "float[3] position_delta;"
        + "float confidence;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUsec
            sum+=8; //TimeDeltaUsec
            sum+=AngleDelta.Length * 4; //AngleDelta
            sum+=PositionDelta.Length * 4; //PositionDelta
            sum+=4; //Confidence
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            TimeUsec = BinSerialize.ReadULong(ref buffer);
            TimeDeltaUsec = BinSerialize.ReadULong(ref buffer);
            arraySize = /*ArrayLength*/3 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            AngleDelta = new float[arraySize];
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
        /// Defines a rotation vector [roll, pitch, yaw] to the current MAV_FRAME_BODY_FRD from the previous MAV_FRAME_BODY_FRD.
        /// OriginName: angle_delta, Units: rad, IsExtended: false
        /// </summary>
        public const int AngleDeltaMaxItemsCount = 3;
        public float[] AngleDelta { get; set; } = new float[3];
        [Obsolete("This method is deprecated. Use GetAngleDeltaMaxItemsCount instead.")]
        public byte GetAngleDeltaMaxItemsCount() => 3;
        /// <summary>
        /// Change in position to the current MAV_FRAME_BODY_FRD from the previous FRAME_BODY_FRD rotated to the current MAV_FRAME_BODY_FRD.
        /// OriginName: position_delta, Units: m, IsExtended: false
        /// </summary>
        public const int PositionDeltaMaxItemsCount = 3;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_usec",
"Timestamp (since boot or Unix epoch).",
string.Empty, 
@"us", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint64, 
            0, 
false),
            new("AOA",
"Angle of Attack.",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("SSA",
"Side Slip Angle.",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
        ];
        public const string FormatMessage = "AOA_SSA:"
        + "uint64_t time_usec;"
        + "float AOA;"
        + "float SSA;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //TimeUsec
            sum+=4; //Aoa
            sum+=4; //Ssa
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("voltage",
"Voltage.",
string.Empty, 
@"cV", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("current",
"Current.",
string.Empty, 
@"cA", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("totalcurrent",
"Total current.",
string.Empty, 
@"mAh", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("rpm",
"RPM (eRPM).",
string.Empty, 
@"rpm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("count",
"count of telemetry packets received (wraps at 65535).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("temperature",
"Temperature.",
string.Empty, 
@"degC", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            4, 
false),
        ];
        public const string FormatMessage = "ESC_TELEMETRY_1_TO_4:"
        + "uint16_t[4] voltage;"
        + "uint16_t[4] current;"
        + "uint16_t[4] totalcurrent;"
        + "uint16_t[4] rpm;"
        + "uint16_t[4] count;"
        + "uint8_t[4] temperature;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=Voltage.Length * 2; //Voltage
            sum+=Current.Length * 2; //Current
            sum+=Totalcurrent.Length * 2; //Totalcurrent
            sum+=Rpm.Length * 2; //Rpm
            sum+=Count.Length * 2; //Count
            sum+=Temperature.Length; //Temperature
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            Voltage = new ushort[arraySize];
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
        
        



        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: cV, IsExtended: false
        /// </summary>
        public const int VoltageMaxItemsCount = 4;
        public ushort[] Voltage { get; set; } = new ushort[4];
        [Obsolete("This method is deprecated. Use GetVoltageMaxItemsCount instead.")]
        public byte GetVoltageMaxItemsCount() => 4;
        /// <summary>
        /// Current.
        /// OriginName: current, Units: cA, IsExtended: false
        /// </summary>
        public const int CurrentMaxItemsCount = 4;
        public ushort[] Current { get; } = new ushort[4];
        /// <summary>
        /// Total current.
        /// OriginName: totalcurrent, Units: mAh, IsExtended: false
        /// </summary>
        public const int TotalcurrentMaxItemsCount = 4;
        public ushort[] Totalcurrent { get; } = new ushort[4];
        /// <summary>
        /// RPM (eRPM).
        /// OriginName: rpm, Units: rpm, IsExtended: false
        /// </summary>
        public const int RpmMaxItemsCount = 4;
        public ushort[] Rpm { get; } = new ushort[4];
        /// <summary>
        /// count of telemetry packets received (wraps at 65535).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public const int CountMaxItemsCount = 4;
        public ushort[] Count { get; } = new ushort[4];
        /// <summary>
        /// Temperature.
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("voltage",
"Voltage.",
string.Empty, 
@"cV", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("current",
"Current.",
string.Empty, 
@"cA", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("totalcurrent",
"Total current.",
string.Empty, 
@"mAh", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("rpm",
"RPM (eRPM).",
string.Empty, 
@"rpm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("count",
"count of telemetry packets received (wraps at 65535).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("temperature",
"Temperature.",
string.Empty, 
@"degC", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            4, 
false),
        ];
        public const string FormatMessage = "ESC_TELEMETRY_5_TO_8:"
        + "uint16_t[4] voltage;"
        + "uint16_t[4] current;"
        + "uint16_t[4] totalcurrent;"
        + "uint16_t[4] rpm;"
        + "uint16_t[4] count;"
        + "uint8_t[4] temperature;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=Voltage.Length * 2; //Voltage
            sum+=Current.Length * 2; //Current
            sum+=Totalcurrent.Length * 2; //Totalcurrent
            sum+=Rpm.Length * 2; //Rpm
            sum+=Count.Length * 2; //Count
            sum+=Temperature.Length; //Temperature
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            Voltage = new ushort[arraySize];
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
        
        



        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: cV, IsExtended: false
        /// </summary>
        public const int VoltageMaxItemsCount = 4;
        public ushort[] Voltage { get; set; } = new ushort[4];
        [Obsolete("This method is deprecated. Use GetVoltageMaxItemsCount instead.")]
        public byte GetVoltageMaxItemsCount() => 4;
        /// <summary>
        /// Current.
        /// OriginName: current, Units: cA, IsExtended: false
        /// </summary>
        public const int CurrentMaxItemsCount = 4;
        public ushort[] Current { get; } = new ushort[4];
        /// <summary>
        /// Total current.
        /// OriginName: totalcurrent, Units: mAh, IsExtended: false
        /// </summary>
        public const int TotalcurrentMaxItemsCount = 4;
        public ushort[] Totalcurrent { get; } = new ushort[4];
        /// <summary>
        /// RPM (eRPM).
        /// OriginName: rpm, Units: rpm, IsExtended: false
        /// </summary>
        public const int RpmMaxItemsCount = 4;
        public ushort[] Rpm { get; } = new ushort[4];
        /// <summary>
        /// count of telemetry packets received (wraps at 65535).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public const int CountMaxItemsCount = 4;
        public ushort[] Count { get; } = new ushort[4];
        /// <summary>
        /// Temperature.
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("voltage",
"Voltage.",
string.Empty, 
@"cV", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("current",
"Current.",
string.Empty, 
@"cA", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("totalcurrent",
"Total current.",
string.Empty, 
@"mAh", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("rpm",
"RPM (eRPM).",
string.Empty, 
@"rpm", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("count",
"count of telemetry packets received (wraps at 65535).",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            4, 
false),
            new("temperature",
"Temperature.",
string.Empty, 
@"degC", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            4, 
false),
        ];
        public const string FormatMessage = "ESC_TELEMETRY_9_TO_12:"
        + "uint16_t[4] voltage;"
        + "uint16_t[4] current;"
        + "uint16_t[4] totalcurrent;"
        + "uint16_t[4] rpm;"
        + "uint16_t[4] count;"
        + "uint8_t[4] temperature;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=Voltage.Length * 2; //Voltage
            sum+=Current.Length * 2; //Current
            sum+=Totalcurrent.Length * 2; //Totalcurrent
            sum+=Rpm.Length * 2; //Rpm
            sum+=Count.Length * 2; //Count
            sum+=Temperature.Length; //Temperature
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/44 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            Voltage = new ushort[arraySize];
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
        
        



        /// <summary>
        /// Voltage.
        /// OriginName: voltage, Units: cV, IsExtended: false
        /// </summary>
        public const int VoltageMaxItemsCount = 4;
        public ushort[] Voltage { get; set; } = new ushort[4];
        [Obsolete("This method is deprecated. Use GetVoltageMaxItemsCount instead.")]
        public byte GetVoltageMaxItemsCount() => 4;
        /// <summary>
        /// Current.
        /// OriginName: current, Units: cA, IsExtended: false
        /// </summary>
        public const int CurrentMaxItemsCount = 4;
        public ushort[] Current { get; } = new ushort[4];
        /// <summary>
        /// Total current.
        /// OriginName: totalcurrent, Units: mAh, IsExtended: false
        /// </summary>
        public const int TotalcurrentMaxItemsCount = 4;
        public ushort[] Totalcurrent { get; } = new ushort[4];
        /// <summary>
        /// RPM (eRPM).
        /// OriginName: rpm, Units: rpm, IsExtended: false
        /// </summary>
        public const int RpmMaxItemsCount = 4;
        public ushort[] Rpm { get; } = new ushort[4];
        /// <summary>
        /// count of telemetry packets received (wraps at 65535).
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public const int CountMaxItemsCount = 4;
        public ushort[] Count { get; } = new ushort[4];
        /// <summary>
        /// Temperature.
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("request_id",
"Request ID - copied to reply.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("min_value",
"OSD parameter minimum value.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("max_value",
"OSD parameter maximum value.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("increment",
"OSD parameter increment.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("osd_screen",
"OSD parameter screen index.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("osd_index",
"OSD parameter display index.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("param_id",
"Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Char, 
            16, 
false),
            new("config_type",
"Config type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "OSD_PARAM_CONFIG:"
        + "uint32_t request_id;"
        + "float min_value;"
        + "float max_value;"
        + "float increment;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t osd_screen;"
        + "uint8_t osd_index;"
        + "char[16] param_id;"
        + "uint8_t config_type;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //RequestId
            sum+=4; //MinValue
            sum+=4; //MaxValue
            sum+=4; //Increment
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //OsdScreen
            sum+=1; //OsdIndex
            sum+=ParamId.Length; //ParamId
            sum+= 1; // ConfigType
            return (byte)sum;
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
            ParamId = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ParamId)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, ParamId.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
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
        
        



        /// <summary>
        /// Request ID - copied to reply.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public uint RequestId { get; set; }
        /// <summary>
        /// OSD parameter minimum value.
        /// OriginName: min_value, Units: , IsExtended: false
        /// </summary>
        public float MinValue { get; set; }
        /// <summary>
        /// OSD parameter maximum value.
        /// OriginName: max_value, Units: , IsExtended: false
        /// </summary>
        public float MaxValue { get; set; }
        /// <summary>
        /// OSD parameter increment.
        /// OriginName: increment, Units: , IsExtended: false
        /// </summary>
        public float Increment { get; set; }
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
        /// OSD parameter screen index.
        /// OriginName: osd_screen, Units: , IsExtended: false
        /// </summary>
        public byte OsdScreen { get; set; }
        /// <summary>
        /// OSD parameter display index.
        /// OriginName: osd_index, Units: , IsExtended: false
        /// </summary>
        public byte OsdIndex { get; set; }
        /// <summary>
        /// Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
        /// OriginName: param_id, Units: , IsExtended: false
        /// </summary>
        public const int ParamIdMaxItemsCount = 16;
        public char[] ParamId { get; set; } = new char[16];
        [Obsolete("This method is deprecated. Use GetParamIdMaxItemsCount instead.")]
        public byte GetParamIdMaxItemsCount() => 16;
        /// <summary>
        /// Config type.
        /// OriginName: config_type, Units: , IsExtended: false
        /// </summary>
        public OsdParamConfigType ConfigType { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("request_id",
"Request ID - copied from request.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("result",
"Config error type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "OSD_PARAM_CONFIG_REPLY:"
        + "uint32_t request_id;"
        + "uint8_t result;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //RequestId
            sum+= 1; // Result
            return (byte)sum;
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
        
        



        /// <summary>
        /// Request ID - copied from request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public uint RequestId { get; set; }
        /// <summary>
        /// Config error type.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public OsdParamConfigError Result { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("request_id",
"Request ID - copied to reply.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("target_system",
"System ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("target_component",
"Component ID.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("osd_screen",
"OSD parameter screen index.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("osd_index",
"OSD parameter display index.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "OSD_PARAM_SHOW_CONFIG:"
        + "uint32_t request_id;"
        + "uint8_t target_system;"
        + "uint8_t target_component;"
        + "uint8_t osd_screen;"
        + "uint8_t osd_index;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //RequestId
            sum+=1; //TargetSystem
            sum+=1; //TargetComponent
            sum+=1; //OsdScreen
            sum+=1; //OsdIndex
            return (byte)sum;
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
        /// OSD parameter screen index.
        /// OriginName: osd_screen, Units: , IsExtended: false
        /// </summary>
        public byte OsdScreen { get; set; }
        /// <summary>
        /// OSD parameter display index.
        /// OriginName: osd_index, Units: , IsExtended: false
        /// </summary>
        public byte OsdIndex { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("request_id",
"Request ID - copied from request.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("min_value",
"OSD parameter minimum value.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("max_value",
"OSD parameter maximum value.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("increment",
"OSD parameter increment.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("result",
"Config error type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("param_id",
"Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Char, 
            16, 
false),
            new("config_type",
"Config type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "OSD_PARAM_SHOW_CONFIG_REPLY:"
        + "uint32_t request_id;"
        + "float min_value;"
        + "float max_value;"
        + "float increment;"
        + "uint8_t result;"
        + "char[16] param_id;"
        + "uint8_t config_type;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //RequestId
            sum+=4; //MinValue
            sum+=4; //MaxValue
            sum+=4; //Increment
            sum+= 1; // Result
            sum+=ParamId.Length; //ParamId
            sum+= 1; // ConfigType
            return (byte)sum;
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
            ParamId = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ParamId)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, ParamId.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
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
        
        



        /// <summary>
        /// Request ID - copied from request.
        /// OriginName: request_id, Units: , IsExtended: false
        /// </summary>
        public uint RequestId { get; set; }
        /// <summary>
        /// OSD parameter minimum value.
        /// OriginName: min_value, Units: , IsExtended: false
        /// </summary>
        public float MinValue { get; set; }
        /// <summary>
        /// OSD parameter maximum value.
        /// OriginName: max_value, Units: , IsExtended: false
        /// </summary>
        public float MaxValue { get; set; }
        /// <summary>
        /// OSD parameter increment.
        /// OriginName: increment, Units: , IsExtended: false
        /// </summary>
        public float Increment { get; set; }
        /// <summary>
        /// Config error type.
        /// OriginName: result, Units: , IsExtended: false
        /// </summary>
        public OsdParamConfigError Result { get; set; }
        /// <summary>
        /// Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
        /// OriginName: param_id, Units: , IsExtended: false
        /// </summary>
        public const int ParamIdMaxItemsCount = 16;
        public char[] ParamId { get; set; } = new char[16];
        [Obsolete("This method is deprecated. Use GetParamIdMaxItemsCount instead.")]
        public byte GetParamIdMaxItemsCount() => 16;
        /// <summary>
        /// Config type.
        /// OriginName: config_type, Units: , IsExtended: false
        /// </summary>
        public OsdParamConfigType ConfigType { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_boot_ms",
"Timestamp (time since system boot).",
string.Empty, 
@"ms", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("x",
" X position of the obstacle.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("y",
" Y position of the obstacle.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("z",
" Z position of the obstacle.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("min_distance",
"Minimum distance the sensor can measure.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("max_distance",
"Maximum distance the sensor can measure.",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("obstacle_id",
" Unique ID given to each obstacle so that its movement can be tracked. Use UINT16_MAX if object ID is unknown or cannot be determined.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("sensor_type",
"Class id of the distance sensor type.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("frame",
"Coordinate frame of reference.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "OBSTACLE_DISTANCE_3D:"
        + "uint32_t time_boot_ms;"
        + "float x;"
        + "float y;"
        + "float z;"
        + "float min_distance;"
        + "float max_distance;"
        + "uint16_t obstacle_id;"
        + "uint8_t sensor_type;"
        + "uint8_t frame;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //TimeBootMs
            sum+=4; //X
            sum+=4; //Y
            sum+=4; //Z
            sum+=4; //MinDistance
            sum+=4; //MaxDistance
            sum+=2; //ObstacleId
            sum+= 1; // SensorType
            sum+= 1; // Frame
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (time since system boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public uint TimeBootMs { get; set; }
        /// <summary>
        ///  X position of the obstacle.
        /// OriginName: x, Units: m, IsExtended: false
        /// </summary>
        public float X { get; set; }
        /// <summary>
        ///  Y position of the obstacle.
        /// OriginName: y, Units: m, IsExtended: false
        /// </summary>
        public float Y { get; set; }
        /// <summary>
        ///  Z position of the obstacle.
        /// OriginName: z, Units: m, IsExtended: false
        /// </summary>
        public float Z { get; set; }
        /// <summary>
        /// Minimum distance the sensor can measure.
        /// OriginName: min_distance, Units: m, IsExtended: false
        /// </summary>
        public float MinDistance { get; set; }
        /// <summary>
        /// Maximum distance the sensor can measure.
        /// OriginName: max_distance, Units: m, IsExtended: false
        /// </summary>
        public float MaxDistance { get; set; }
        /// <summary>
        ///  Unique ID given to each obstacle so that its movement can be tracked. Use UINT16_MAX if object ID is unknown or cannot be determined.
        /// OriginName: obstacle_id, Units: , IsExtended: false
        /// </summary>
        public ushort ObstacleId { get; set; }
        /// <summary>
        /// Class id of the distance sensor type.
        /// OriginName: sensor_type, Units: , IsExtended: false
        /// </summary>
        public MavDistanceSensor SensorType { get; set; }
        /// <summary>
        /// Coordinate frame of reference.
        /// OriginName: frame, Units: , IsExtended: false
        /// </summary>
        public MavFrame Frame { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("time_boot_ms",
"Timestamp (time since system boot)",
string.Empty, 
@"ms", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint32, 
            0, 
false),
            new("lat",
"Latitude",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("lng",
"Longitude",
string.Empty, 
@"degE7", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int32, 
            0, 
false),
            new("alt",
"Altitude (MSL) of vehicle",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("roll",
"Roll angle",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("pitch",
"Pitch angle",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("yaw",
"Yaw angle",
string.Empty, 
@"rad", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("distance",
"Distance (uncorrected)",
string.Empty, 
@"m", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("temperature",
"Water temperature",
string.Empty, 
@"degC", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("id",
"Onboard ID of the sensor",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("healthy",
"Sensor data healthy (0=unhealthy, 1=healthy)",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "WATER_DEPTH:"
        + "uint32_t time_boot_ms;"
        + "int32_t lat;"
        + "int32_t lng;"
        + "float alt;"
        + "float roll;"
        + "float pitch;"
        + "float yaw;"
        + "float distance;"
        + "float temperature;"
        + "uint8_t id;"
        + "uint8_t healthy;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //TimeBootMs
            sum+=4; //Lat
            sum+=4; //Lng
            sum+=4; //Alt
            sum+=4; //Roll
            sum+=4; //Pitch
            sum+=4; //Yaw
            sum+=4; //Distance
            sum+=4; //Temperature
            sum+=1; //Id
            sum+=1; //Healthy
            return (byte)sum;
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
        
        



        /// <summary>
        /// Timestamp (time since system boot)
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public uint TimeBootMs { get; set; }
        /// <summary>
        /// Latitude
        /// OriginName: lat, Units: degE7, IsExtended: false
        /// </summary>
        public int Lat { get; set; }
        /// <summary>
        /// Longitude
        /// OriginName: lng, Units: degE7, IsExtended: false
        /// </summary>
        public int Lng { get; set; }
        /// <summary>
        /// Altitude (MSL) of vehicle
        /// OriginName: alt, Units: m, IsExtended: false
        /// </summary>
        public float Alt { get; set; }
        /// <summary>
        /// Roll angle
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public float Roll { get; set; }
        /// <summary>
        /// Pitch angle
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public float Pitch { get; set; }
        /// <summary>
        /// Yaw angle
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public float Yaw { get; set; }
        /// <summary>
        /// Distance (uncorrected)
        /// OriginName: distance, Units: m, IsExtended: false
        /// </summary>
        public float Distance { get; set; }
        /// <summary>
        /// Water temperature
        /// OriginName: temperature, Units: degC, IsExtended: false
        /// </summary>
        public float Temperature { get; set; }
        /// <summary>
        /// Onboard ID of the sensor
        /// OriginName: id, Units: , IsExtended: false
        /// </summary>
        public byte Id { get; set; }
        /// <summary>
        /// Sensor data healthy (0=unhealthy, 1=healthy)
        /// OriginName: healthy, Units: , IsExtended: false
        /// </summary>
        public byte Healthy { get; set; }
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("MCU_temperature",
"MCU Internal temperature",
string.Empty, 
@"cdegC", 
string.Empty, 
string.Empty, 
            MessageFieldType.Int16, 
            0, 
false),
            new("MCU_voltage",
"MCU voltage",
string.Empty, 
@"mV", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("MCU_voltage_min",
"MCU voltage minimum",
string.Empty, 
@"mV", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("MCU_voltage_max",
"MCU voltage maximum",
string.Empty, 
@"mV", 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint16, 
            0, 
false),
            new("id",
"MCU instance",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
        ];
        public const string FormatMessage = "MCU_STATUS:"
        + "int16_t MCU_temperature;"
        + "uint16_t MCU_voltage;"
        + "uint16_t MCU_voltage_min;"
        + "uint16_t MCU_voltage_max;"
        + "uint8_t id;"
        ;
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //McuTemperature
            sum+=2; //McuVoltage
            sum+=2; //McuVoltageMin
            sum+=2; //McuVoltageMax
            sum+=1; //Id
            return (byte)sum;
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
        
        



        /// <summary>
        /// MCU Internal temperature
        /// OriginName: MCU_temperature, Units: cdegC, IsExtended: false
        /// </summary>
        public short McuTemperature { get; set; }
        /// <summary>
        /// MCU voltage
        /// OriginName: MCU_voltage, Units: mV, IsExtended: false
        /// </summary>
        public ushort McuVoltage { get; set; }
        /// <summary>
        /// MCU voltage minimum
        /// OriginName: MCU_voltage_min, Units: mV, IsExtended: false
        /// </summary>
        public ushort McuVoltageMin { get; set; }
        /// <summary>
        /// MCU voltage maximum
        /// OriginName: MCU_voltage_max, Units: mV, IsExtended: false
        /// </summary>
        public ushort McuVoltageMax { get; set; }
        /// <summary>
        /// MCU instance
        /// OriginName: id, Units: , IsExtended: false
        /// </summary>
        public byte Id { get; set; }
    }


#endregion


}

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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.8+aedf0e45cecf4e3648d310da2728457ab10b401a 25-07-02.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.AsvAudio;
using System.Linq;
using System.Collections.Generic;
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
    public enum MavStorm32TunnelPayloadType : ulong
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
    public static class MavStorm32TunnelPayloadTypeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(200);
            yield return converter(201);
            yield return converter(202);
            yield return converter(203);
            yield return converter(204);
            yield return converter(205);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(200),"MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH1_IN");
            yield return new EnumValue<T>(converter(201),"MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH1_OUT");
            yield return new EnumValue<T>(converter(202),"MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH2_IN");
            yield return new EnumValue<T>(converter(203),"MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH2_OUT");
            yield return new EnumValue<T>(converter(204),"MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH3_IN");
            yield return new EnumValue<T>(converter(205),"MAV_STORM32_TUNNEL_PAYLOAD_TYPE_STORM32_CH3_OUT");
        }
    }
    /// <summary>
    /// STorM32 gimbal prearm check flags.
    ///  MAV_STORM32_GIMBAL_PREARM_FLAGS
    /// </summary>
    public enum MavStorm32GimbalPrearmFlags : ulong
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
    public static class MavStorm32GimbalPrearmFlagsHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(4);
            yield return converter(8);
            yield return converter(16);
            yield return converter(32);
            yield return converter(64);
            yield return converter(128);
            yield return converter(256);
            yield return converter(512);
            yield return converter(1024);
            yield return converter(2048);
            yield return converter(4096);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"MAV_STORM32_GIMBAL_PREARM_FLAGS_IS_NORMAL");
            yield return new EnumValue<T>(converter(2),"MAV_STORM32_GIMBAL_PREARM_FLAGS_IMUS_WORKING");
            yield return new EnumValue<T>(converter(4),"MAV_STORM32_GIMBAL_PREARM_FLAGS_MOTORS_WORKING");
            yield return new EnumValue<T>(converter(8),"MAV_STORM32_GIMBAL_PREARM_FLAGS_ENCODERS_WORKING");
            yield return new EnumValue<T>(converter(16),"MAV_STORM32_GIMBAL_PREARM_FLAGS_VOLTAGE_OK");
            yield return new EnumValue<T>(converter(32),"MAV_STORM32_GIMBAL_PREARM_FLAGS_VIRTUALCHANNELS_RECEIVING");
            yield return new EnumValue<T>(converter(64),"MAV_STORM32_GIMBAL_PREARM_FLAGS_MAVLINK_RECEIVING");
            yield return new EnumValue<T>(converter(128),"MAV_STORM32_GIMBAL_PREARM_FLAGS_STORM32LINK_QFIX");
            yield return new EnumValue<T>(converter(256),"MAV_STORM32_GIMBAL_PREARM_FLAGS_STORM32LINK_WORKING");
            yield return new EnumValue<T>(converter(512),"MAV_STORM32_GIMBAL_PREARM_FLAGS_CAMERA_CONNECTED");
            yield return new EnumValue<T>(converter(1024),"MAV_STORM32_GIMBAL_PREARM_FLAGS_AUX0_LOW");
            yield return new EnumValue<T>(converter(2048),"MAV_STORM32_GIMBAL_PREARM_FLAGS_AUX1_LOW");
            yield return new EnumValue<T>(converter(4096),"MAV_STORM32_GIMBAL_PREARM_FLAGS_NTLOGGER_WORKING");
        }
    }
    /// <summary>
    /// STorM32 camera prearm check flags.
    ///  MAV_STORM32_CAMERA_PREARM_FLAGS
    /// </summary>
    public enum MavStorm32CameraPrearmFlags : ulong
    {
        /// <summary>
        /// The camera has been found and is connected.
        /// MAV_STORM32_CAMERA_PREARM_FLAGS_CONNECTED
        /// </summary>
        MavStorm32CameraPrearmFlagsConnected = 1,
    }
    public static class MavStorm32CameraPrearmFlagsHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"MAV_STORM32_CAMERA_PREARM_FLAGS_CONNECTED");
        }
    }
    /// <summary>
    /// Gimbal manager capability flags.
    ///  MAV_STORM32_GIMBAL_MANAGER_CAP_FLAGS
    /// </summary>
    public enum MavStorm32GimbalManagerCapFlags : ulong
    {
        /// <summary>
        /// The gimbal manager supports several profiles.
        /// MAV_STORM32_GIMBAL_MANAGER_CAP_FLAGS_HAS_PROFILES
        /// </summary>
        MavStorm32GimbalManagerCapFlagsHasProfiles = 1,
    }
    public static class MavStorm32GimbalManagerCapFlagsHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"MAV_STORM32_GIMBAL_MANAGER_CAP_FLAGS_HAS_PROFILES");
        }
    }
    /// <summary>
    /// Flags for gimbal manager operation. Used for setting and reporting, unless specified otherwise. If a setting has been accepted by the gimbal manager is reported in the STORM32_GIMBAL_MANAGER_STATUS message.
    ///  MAV_STORM32_GIMBAL_MANAGER_FLAGS
    /// </summary>
    public enum MavStorm32GimbalManagerFlags : ulong
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
    public static class MavStorm32GimbalManagerFlagsHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
            yield return converter(4);
            yield return converter(8);
            yield return converter(16);
            yield return converter(32);
            yield return converter(64);
            yield return converter(128);
            yield return converter(256);
            yield return converter(512);
            yield return converter(1024);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_NONE");
            yield return new EnumValue<T>(converter(1),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_RC_ACTIVE");
            yield return new EnumValue<T>(converter(2),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_ONBOARD_ACTIVE");
            yield return new EnumValue<T>(converter(4),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_AUTOPILOT_ACTIVE");
            yield return new EnumValue<T>(converter(8),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_GCS_ACTIVE");
            yield return new EnumValue<T>(converter(16),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_CAMERA_ACTIVE");
            yield return new EnumValue<T>(converter(32),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_GCS2_ACTIVE");
            yield return new EnumValue<T>(converter(64),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_CAMERA2_ACTIVE");
            yield return new EnumValue<T>(converter(128),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_CUSTOM_ACTIVE");
            yield return new EnumValue<T>(converter(256),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_CLIENT_CUSTOM2_ACTIVE");
            yield return new EnumValue<T>(converter(512),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_SET_SUPERVISON");
            yield return new EnumValue<T>(converter(1024),"MAV_STORM32_GIMBAL_MANAGER_FLAGS_SET_RELEASE");
        }
    }
    /// <summary>
    /// Gimbal manager client ID. In a prioritizing profile, the priorities are determined by the implementation; they could e.g. be custom1 > onboard > GCS > autopilot/camera > GCS2 > custom2.
    ///  MAV_STORM32_GIMBAL_MANAGER_CLIENT
    /// </summary>
    public enum MavStorm32GimbalManagerClient : ulong
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
    public static class MavStorm32GimbalManagerClientHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
            yield return converter(4);
            yield return converter(5);
            yield return converter(6);
            yield return converter(7);
            yield return converter(8);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"MAV_STORM32_GIMBAL_MANAGER_CLIENT_NONE");
            yield return new EnumValue<T>(converter(1),"MAV_STORM32_GIMBAL_MANAGER_CLIENT_ONBOARD");
            yield return new EnumValue<T>(converter(2),"MAV_STORM32_GIMBAL_MANAGER_CLIENT_AUTOPILOT");
            yield return new EnumValue<T>(converter(3),"MAV_STORM32_GIMBAL_MANAGER_CLIENT_GCS");
            yield return new EnumValue<T>(converter(4),"MAV_STORM32_GIMBAL_MANAGER_CLIENT_CAMERA");
            yield return new EnumValue<T>(converter(5),"MAV_STORM32_GIMBAL_MANAGER_CLIENT_GCS2");
            yield return new EnumValue<T>(converter(6),"MAV_STORM32_GIMBAL_MANAGER_CLIENT_CAMERA2");
            yield return new EnumValue<T>(converter(7),"MAV_STORM32_GIMBAL_MANAGER_CLIENT_CUSTOM");
            yield return new EnumValue<T>(converter(8),"MAV_STORM32_GIMBAL_MANAGER_CLIENT_CUSTOM2");
        }
    }
    /// <summary>
    /// Gimbal manager profiles. Only standard profiles are defined. Any implementation can define its own profile(s) in addition, and should use enum values > 16.
    ///  MAV_STORM32_GIMBAL_MANAGER_PROFILE
    /// </summary>
    public enum MavStorm32GimbalManagerProfile : ulong
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
    public static class MavStorm32GimbalManagerProfileHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
            yield return converter(4);
            yield return converter(5);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"MAV_STORM32_GIMBAL_MANAGER_PROFILE_DEFAULT");
            yield return new EnumValue<T>(converter(1),"MAV_STORM32_GIMBAL_MANAGER_PROFILE_CUSTOM");
            yield return new EnumValue<T>(converter(2),"MAV_STORM32_GIMBAL_MANAGER_PROFILE_COOPERATIVE");
            yield return new EnumValue<T>(converter(3),"MAV_STORM32_GIMBAL_MANAGER_PROFILE_EXCLUSIVE");
            yield return new EnumValue<T>(converter(4),"MAV_STORM32_GIMBAL_MANAGER_PROFILE_PRIORITY_COOPERATIVE");
            yield return new EnumValue<T>(converter(5),"MAV_STORM32_GIMBAL_MANAGER_PROFILE_PRIORITY_EXCLUSIVE");
        }
    }
    /// <summary>
    /// Enumeration of possible shot modes.
    ///  MAV_QSHOT_MODE
    /// </summary>
    public enum MavQshotMode : ulong
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
    public static class MavQshotModeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
            yield return converter(4);
            yield return converter(5);
            yield return converter(6);
            yield return converter(7);
            yield return converter(8);
            yield return converter(9);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"MAV_QSHOT_MODE_UNDEFINED");
            yield return new EnumValue<T>(converter(1),"MAV_QSHOT_MODE_DEFAULT");
            yield return new EnumValue<T>(converter(2),"MAV_QSHOT_MODE_GIMBAL_RETRACT");
            yield return new EnumValue<T>(converter(3),"MAV_QSHOT_MODE_GIMBAL_NEUTRAL");
            yield return new EnumValue<T>(converter(4),"MAV_QSHOT_MODE_GIMBAL_MISSION");
            yield return new EnumValue<T>(converter(5),"MAV_QSHOT_MODE_GIMBAL_RC_CONTROL");
            yield return new EnumValue<T>(converter(6),"MAV_QSHOT_MODE_POI_TARGETING");
            yield return new EnumValue<T>(converter(7),"MAV_QSHOT_MODE_SYSID_TARGETING");
            yield return new EnumValue<T>(converter(8),"MAV_QSHOT_MODE_CABLECAM_2POINT");
            yield return new EnumValue<T>(converter(9),"MAV_QSHOT_MODE_HOME_TARGETING");
        }
    }
    /// <summary>
    ///  MAV_CMD
    /// </summary>
    public enum MavCmd : ulong
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
    public static class MavCmdHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(60002);
            yield return converter(60010);
            yield return converter(60020);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(60002),"MAV_CMD_STORM32_DO_GIMBAL_MANAGER_CONTROL_PITCHYAW");
            yield return new EnumValue<T>(converter(60010),"MAV_CMD_STORM32_DO_GIMBAL_MANAGER_SETUP");
            yield return new EnumValue<T>(converter(60020),"MAV_CMD_QSHOT_DO_CONFIGURE");
        }
    }
    /// <summary>
    /// RADIO_RC_CHANNELS flags (bitmask).
    ///  RADIO_RC_CHANNELS_FLAGS
    /// </summary>
    public enum RadioRcChannelsFlags : ulong
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
    public static class RadioRcChannelsFlagsHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"RADIO_RC_CHANNELS_FLAGS_FAILSAFE");
            yield return new EnumValue<T>(converter(2),"RADIO_RC_CHANNELS_FLAGS_FRAME_MISSED");
        }
    }
    /// <summary>
    /// RADIO_LINK_STATS flags (bitmask).
    ///  RADIO_LINK_STATS_FLAGS
    /// </summary>
    public enum RadioLinkStatsFlags : ulong
    {
        /// <summary>
        /// Rssi are in negative dBm. Values 0..254 corresponds to 0..-254 dBm.
        /// RADIO_LINK_STATS_FLAGS_RSSI_DBM
        /// </summary>
        RadioLinkStatsFlagsRssiDbm = 1,
    }
    public static class RadioLinkStatsFlagsHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"RADIO_LINK_STATS_FLAGS_RSSI_DBM");
        }
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

        public void Accept(IVisitor visitor)
        {
            var tmpDeviceCapFlags = (uint)DeviceCapFlags;
            UInt32Type.Accept(visitor,DeviceCapFlagsField, DeviceCapFlagsField.DataType, ref tmpDeviceCapFlags);
            DeviceCapFlags = (GimbalDeviceCapFlags)tmpDeviceCapFlags;
            var tmpManagerCapFlags = (uint)ManagerCapFlags;
            UInt32Type.Accept(visitor,ManagerCapFlagsField, ManagerCapFlagsField.DataType, ref tmpManagerCapFlags);
            ManagerCapFlags = (MavStorm32GimbalManagerCapFlags)tmpManagerCapFlags;
            FloatType.Accept(visitor,RollMinField, RollMinField.DataType, ref _rollMin);    
            FloatType.Accept(visitor,RollMaxField, RollMaxField.DataType, ref _rollMax);    
            FloatType.Accept(visitor,PitchMinField, PitchMinField.DataType, ref _pitchMin);    
            FloatType.Accept(visitor,PitchMaxField, PitchMaxField.DataType, ref _pitchMax);    
            FloatType.Accept(visitor,YawMinField, YawMinField.DataType, ref _yawMin);    
            FloatType.Accept(visitor,YawMaxField, YawMaxField.DataType, ref _yawMax);    
            UInt8Type.Accept(visitor,GimbalIdField, GimbalIdField.DataType, ref _gimbalId);    

        }

        /// <summary>
        /// Gimbal device capability flags. Same flags as reported by GIMBAL_DEVICE_INFORMATION. The flag is only 16 bit wide, but stored in 32 bit, for backwards compatibility (high word is zero).
        /// OriginName: device_cap_flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DeviceCapFlagsField = new Field.Builder()
            .Name(nameof(DeviceCapFlags))
            .Title("bitmask")
            .Description("Gimbal device capability flags. Same flags as reported by GIMBAL_DEVICE_INFORMATION. The flag is only 16 bit wide, but stored in 32 bit, for backwards compatibility (high word is zero).")
            .DataType(new UInt32Type(GimbalDeviceCapFlagsHelper.GetValues(x=>(uint)x).Min(),GimbalDeviceCapFlagsHelper.GetValues(x=>(uint)x).Max()))
            .Enum(GimbalDeviceCapFlagsHelper.GetEnumValues(x=>(uint)x))
            .Build();
        private GimbalDeviceCapFlags _deviceCapFlags;
        public GimbalDeviceCapFlags DeviceCapFlags { get => _deviceCapFlags; set => _deviceCapFlags = value; } 
        /// <summary>
        /// Gimbal manager capability flags.
        /// OriginName: manager_cap_flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ManagerCapFlagsField = new Field.Builder()
            .Name(nameof(ManagerCapFlags))
            .Title("bitmask")
            .Description("Gimbal manager capability flags.")
            .DataType(new UInt32Type(MavStorm32GimbalManagerCapFlagsHelper.GetValues(x=>(uint)x).Min(),MavStorm32GimbalManagerCapFlagsHelper.GetValues(x=>(uint)x).Max()))
            .Enum(MavStorm32GimbalManagerCapFlagsHelper.GetEnumValues(x=>(uint)x))
            .Build();
        private MavStorm32GimbalManagerCapFlags _managerCapFlags;
        public MavStorm32GimbalManagerCapFlags ManagerCapFlags { get => _managerCapFlags; set => _managerCapFlags = value; } 
        /// <summary>
        /// Hardware minimum roll angle (positive: roll to the right). NaN if unknown.
        /// OriginName: roll_min, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollMinField = new Field.Builder()
            .Name(nameof(RollMin))
            .Title("roll_min")
            .Description("Hardware minimum roll angle (positive: roll to the right). NaN if unknown.")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _rollMin;
        public float RollMin { get => _rollMin; set => _rollMin = value; }
        /// <summary>
        /// Hardware maximum roll angle (positive: roll to the right). NaN if unknown.
        /// OriginName: roll_max, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollMaxField = new Field.Builder()
            .Name(nameof(RollMax))
            .Title("roll_max")
            .Description("Hardware maximum roll angle (positive: roll to the right). NaN if unknown.")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _rollMax;
        public float RollMax { get => _rollMax; set => _rollMax = value; }
        /// <summary>
        /// Hardware minimum pitch/tilt angle (positive: tilt up). NaN if unknown.
        /// OriginName: pitch_min, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchMinField = new Field.Builder()
            .Name(nameof(PitchMin))
            .Title("pitch_min")
            .Description("Hardware minimum pitch/tilt angle (positive: tilt up). NaN if unknown.")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _pitchMin;
        public float PitchMin { get => _pitchMin; set => _pitchMin = value; }
        /// <summary>
        /// Hardware maximum pitch/tilt angle (positive: tilt up). NaN if unknown.
        /// OriginName: pitch_max, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchMaxField = new Field.Builder()
            .Name(nameof(PitchMax))
            .Title("pitch_max")
            .Description("Hardware maximum pitch/tilt angle (positive: tilt up). NaN if unknown.")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _pitchMax;
        public float PitchMax { get => _pitchMax; set => _pitchMax = value; }
        /// <summary>
        /// Hardware minimum yaw/pan angle (positive: pan to the right, relative to the vehicle/gimbal base). NaN if unknown.
        /// OriginName: yaw_min, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawMinField = new Field.Builder()
            .Name(nameof(YawMin))
            .Title("yaw_min")
            .Description("Hardware minimum yaw/pan angle (positive: pan to the right, relative to the vehicle/gimbal base). NaN if unknown.")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _yawMin;
        public float YawMin { get => _yawMin; set => _yawMin = value; }
        /// <summary>
        /// Hardware maximum yaw/pan angle (positive: pan to the right, relative to the vehicle/gimbal base). NaN if unknown.
        /// OriginName: yaw_max, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawMaxField = new Field.Builder()
            .Name(nameof(YawMax))
            .Title("yaw_max")
            .Description("Hardware maximum yaw/pan angle (positive: pan to the right, relative to the vehicle/gimbal base). NaN if unknown.")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _yawMax;
        public float YawMax { get => _yawMax; set => _yawMax = value; }
        /// <summary>
        /// Gimbal ID (component ID or 1-6 for non-MAVLink gimbal) that this gimbal manager is responsible for.
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GimbalIdField = new Field.Builder()
            .Name(nameof(GimbalId))
            .Title("gimbal_id")
            .Description("Gimbal ID (component ID or 1-6 for non-MAVLink gimbal) that this gimbal manager is responsible for.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _gimbalId;
        public byte GimbalId { get => _gimbalId; set => _gimbalId = value; }
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

        public void Accept(IVisitor visitor)
        {
            var tmpDeviceFlags = (ushort)DeviceFlags;
            UInt16Type.Accept(visitor,DeviceFlagsField, DeviceFlagsField.DataType, ref tmpDeviceFlags);
            DeviceFlags = (GimbalDeviceFlags)tmpDeviceFlags;
            var tmpManagerFlags = (ushort)ManagerFlags;
            UInt16Type.Accept(visitor,ManagerFlagsField, ManagerFlagsField.DataType, ref tmpManagerFlags);
            ManagerFlags = (MavStorm32GimbalManagerFlags)tmpManagerFlags;
            UInt8Type.Accept(visitor,GimbalIdField, GimbalIdField.DataType, ref _gimbalId);    
            var tmpSupervisor = (byte)Supervisor;
            UInt8Type.Accept(visitor,SupervisorField, SupervisorField.DataType, ref tmpSupervisor);
            Supervisor = (MavStorm32GimbalManagerClient)tmpSupervisor;
            var tmpProfile = (byte)Profile;
            UInt8Type.Accept(visitor,ProfileField, ProfileField.DataType, ref tmpProfile);
            Profile = (MavStorm32GimbalManagerProfile)tmpProfile;

        }

        /// <summary>
        /// Gimbal device flags currently applied. Same flags as reported by GIMBAL_DEVICE_ATTITUDE_STATUS.
        /// OriginName: device_flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DeviceFlagsField = new Field.Builder()
            .Name(nameof(DeviceFlags))
            .Title("device_flags")
            .Description("Gimbal device flags currently applied. Same flags as reported by GIMBAL_DEVICE_ATTITUDE_STATUS.")
            .DataType(new UInt16Type(GimbalDeviceFlagsHelper.GetValues(x=>(ushort)x).Min(),GimbalDeviceFlagsHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(GimbalDeviceFlagsHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private GimbalDeviceFlags _deviceFlags;
        public GimbalDeviceFlags DeviceFlags { get => _deviceFlags; set => _deviceFlags = value; } 
        /// <summary>
        /// Gimbal manager flags currently applied.
        /// OriginName: manager_flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ManagerFlagsField = new Field.Builder()
            .Name(nameof(ManagerFlags))
            .Title("manager_flags")
            .Description("Gimbal manager flags currently applied.")
            .DataType(new UInt16Type(MavStorm32GimbalManagerFlagsHelper.GetValues(x=>(ushort)x).Min(),MavStorm32GimbalManagerFlagsHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(MavStorm32GimbalManagerFlagsHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private MavStorm32GimbalManagerFlags _managerFlags;
        public MavStorm32GimbalManagerFlags ManagerFlags { get => _managerFlags; set => _managerFlags = value; } 
        /// <summary>
        /// Gimbal ID (component ID or 1-6 for non-MAVLink gimbal) that this gimbal manager is responsible for.
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GimbalIdField = new Field.Builder()
            .Name(nameof(GimbalId))
            .Title("gimbal_id")
            .Description("Gimbal ID (component ID or 1-6 for non-MAVLink gimbal) that this gimbal manager is responsible for.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _gimbalId;
        public byte GimbalId { get => _gimbalId; set => _gimbalId = value; }
        /// <summary>
        /// Client who is currently supervisor (0 = none).
        /// OriginName: supervisor, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SupervisorField = new Field.Builder()
            .Name(nameof(Supervisor))
            .Title("supervisor")
            .Description("Client who is currently supervisor (0 = none).")
            .DataType(new UInt8Type(MavStorm32GimbalManagerClientHelper.GetValues(x=>(byte)x).Min(),MavStorm32GimbalManagerClientHelper.GetValues(x=>(byte)x).Max()))
            .Enum(MavStorm32GimbalManagerClientHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private MavStorm32GimbalManagerClient _supervisor;
        public MavStorm32GimbalManagerClient Supervisor { get => _supervisor; set => _supervisor = value; } 
        /// <summary>
        /// Profile currently applied (0 = default).
        /// OriginName: profile, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ProfileField = new Field.Builder()
            .Name(nameof(Profile))
            .Title("profile")
            .Description("Profile currently applied (0 = default).")
            .DataType(new UInt8Type(MavStorm32GimbalManagerProfileHelper.GetValues(x=>(byte)x).Min(),MavStorm32GimbalManagerProfileHelper.GetValues(x=>(byte)x).Max()))
            .Enum(MavStorm32GimbalManagerProfileHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private MavStorm32GimbalManagerProfile _profile;
        public MavStorm32GimbalManagerProfile Profile { get => _profile; set => _profile = value; } 
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

        public void Accept(IVisitor visitor)
        {
            ArrayType.Accept(visitor,QField, QField.DataType, 4,
                (index, v, f, t) => FloatType.Accept(v, f, t, ref Q[index]));
            FloatType.Accept(visitor,AngularVelocityXField, AngularVelocityXField.DataType, ref _angularVelocityX);    
            FloatType.Accept(visitor,AngularVelocityYField, AngularVelocityYField.DataType, ref _angularVelocityY);    
            FloatType.Accept(visitor,AngularVelocityZField, AngularVelocityZField.DataType, ref _angularVelocityZ);    
            var tmpDeviceFlags = (ushort)DeviceFlags;
            UInt16Type.Accept(visitor,DeviceFlagsField, DeviceFlagsField.DataType, ref tmpDeviceFlags);
            DeviceFlags = (GimbalDeviceFlags)tmpDeviceFlags;
            var tmpManagerFlags = (ushort)ManagerFlags;
            UInt16Type.Accept(visitor,ManagerFlagsField, ManagerFlagsField.DataType, ref tmpManagerFlags);
            ManagerFlags = (MavStorm32GimbalManagerFlags)tmpManagerFlags;
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    
            UInt8Type.Accept(visitor,GimbalIdField, GimbalIdField.DataType, ref _gimbalId);    
            var tmpClient = (byte)Client;
            UInt8Type.Accept(visitor,ClientField, ClientField.DataType, ref tmpClient);
            Client = (MavStorm32GimbalManagerClient)tmpClient;

        }

        /// <summary>
        /// Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation). Set first element to NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.
        /// OriginName: q, Units: , IsExtended: false
        /// </summary>
        public static readonly Field QField = new Field.Builder()
            .Name(nameof(Q))
            .Title("q")
            .Description("Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation). Set first element to NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.")

            .DataType(new ArrayType(FloatType.Default,4))        
        .Build();
        public const int QMaxItemsCount = 4;
        public float[] Q { get; } = new float[4];
        [Obsolete("This method is deprecated. Use GetQMaxItemsCount instead.")]
        public byte GetQMaxItemsCount() => 4;
        /// <summary>
        /// X component of angular velocity (positive: roll to the right). NaN to be ignored.
        /// OriginName: angular_velocity_x, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field AngularVelocityXField = new Field.Builder()
            .Name(nameof(AngularVelocityX))
            .Title("angular_velocity_x")
            .Description("X component of angular velocity (positive: roll to the right). NaN to be ignored.")
.Units(@"rad/s")
            .DataType(FloatType.Default)
        .Build();
        private float _angularVelocityX;
        public float AngularVelocityX { get => _angularVelocityX; set => _angularVelocityX = value; }
        /// <summary>
        /// Y component of angular velocity (positive: tilt up). NaN to be ignored.
        /// OriginName: angular_velocity_y, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field AngularVelocityYField = new Field.Builder()
            .Name(nameof(AngularVelocityY))
            .Title("angular_velocity_y")
            .Description("Y component of angular velocity (positive: tilt up). NaN to be ignored.")
.Units(@"rad/s")
            .DataType(FloatType.Default)
        .Build();
        private float _angularVelocityY;
        public float AngularVelocityY { get => _angularVelocityY; set => _angularVelocityY = value; }
        /// <summary>
        /// Z component of angular velocity (positive: pan to the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.
        /// OriginName: angular_velocity_z, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field AngularVelocityZField = new Field.Builder()
            .Name(nameof(AngularVelocityZ))
            .Title("angular_velocity_z")
            .Description("Z component of angular velocity (positive: pan to the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.")
.Units(@"rad/s")
            .DataType(FloatType.Default)
        .Build();
        private float _angularVelocityZ;
        public float AngularVelocityZ { get => _angularVelocityZ; set => _angularVelocityZ = value; }
        /// <summary>
        /// Gimbal device flags to be applied (UINT16_MAX to be ignored). Same flags as used in GIMBAL_DEVICE_SET_ATTITUDE.
        /// OriginName: device_flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DeviceFlagsField = new Field.Builder()
            .Name(nameof(DeviceFlags))
            .Title("device_flags")
            .Description("Gimbal device flags to be applied (UINT16_MAX to be ignored). Same flags as used in GIMBAL_DEVICE_SET_ATTITUDE.")
            .DataType(new UInt16Type(GimbalDeviceFlagsHelper.GetValues(x=>(ushort)x).Min(),GimbalDeviceFlagsHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(GimbalDeviceFlagsHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private GimbalDeviceFlags _deviceFlags;
        public GimbalDeviceFlags DeviceFlags { get => _deviceFlags; set => _deviceFlags = value; } 
        /// <summary>
        /// Gimbal manager flags to be applied (0 to be ignored).
        /// OriginName: manager_flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ManagerFlagsField = new Field.Builder()
            .Name(nameof(ManagerFlags))
            .Title("manager_flags")
            .Description("Gimbal manager flags to be applied (0 to be ignored).")
            .DataType(new UInt16Type(MavStorm32GimbalManagerFlagsHelper.GetValues(x=>(ushort)x).Min(),MavStorm32GimbalManagerFlagsHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(MavStorm32GimbalManagerFlagsHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private MavStorm32GimbalManagerFlags _managerFlags;
        public MavStorm32GimbalManagerFlags ManagerFlags { get => _managerFlags; set => _managerFlags = value; } 
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetSystem;
        public byte TargetSystem { get => _targetSystem; set => _targetSystem = value; }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetComponent;
        public byte TargetComponent { get => _targetComponent; set => _targetComponent = value; }
        /// <summary>
        /// Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GimbalIdField = new Field.Builder()
            .Name(nameof(GimbalId))
            .Title("gimbal_id")
            .Description("Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _gimbalId;
        public byte GimbalId { get => _gimbalId; set => _gimbalId = value; }
        /// <summary>
        /// Client which is contacting the gimbal manager (must be set).
        /// OriginName: client, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ClientField = new Field.Builder()
            .Name(nameof(Client))
            .Title("client")
            .Description("Client which is contacting the gimbal manager (must be set).")
            .DataType(new UInt8Type(MavStorm32GimbalManagerClientHelper.GetValues(x=>(byte)x).Min(),MavStorm32GimbalManagerClientHelper.GetValues(x=>(byte)x).Max()))
            .Enum(MavStorm32GimbalManagerClientHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private MavStorm32GimbalManagerClient _client;
        public MavStorm32GimbalManagerClient Client { get => _client; set => _client = value; } 
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

        public void Accept(IVisitor visitor)
        {
            FloatType.Accept(visitor,PitchField, PitchField.DataType, ref _pitch);    
            FloatType.Accept(visitor,YawField, YawField.DataType, ref _yaw);    
            FloatType.Accept(visitor,PitchRateField, PitchRateField.DataType, ref _pitchRate);    
            FloatType.Accept(visitor,YawRateField, YawRateField.DataType, ref _yawRate);    
            var tmpDeviceFlags = (ushort)DeviceFlags;
            UInt16Type.Accept(visitor,DeviceFlagsField, DeviceFlagsField.DataType, ref tmpDeviceFlags);
            DeviceFlags = (GimbalDeviceFlags)tmpDeviceFlags;
            var tmpManagerFlags = (ushort)ManagerFlags;
            UInt16Type.Accept(visitor,ManagerFlagsField, ManagerFlagsField.DataType, ref tmpManagerFlags);
            ManagerFlags = (MavStorm32GimbalManagerFlags)tmpManagerFlags;
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    
            UInt8Type.Accept(visitor,GimbalIdField, GimbalIdField.DataType, ref _gimbalId);    
            var tmpClient = (byte)Client;
            UInt8Type.Accept(visitor,ClientField, ClientField.DataType, ref tmpClient);
            Client = (MavStorm32GimbalManagerClient)tmpClient;

        }

        /// <summary>
        /// Pitch/tilt angle (positive: tilt up). NaN to be ignored.
        /// OriginName: pitch, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch/tilt angle (positive: tilt up). NaN to be ignored.")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _pitch;
        public float Pitch { get => _pitch; set => _pitch = value; }
        /// <summary>
        /// Yaw/pan angle (positive: pan the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.
        /// OriginName: yaw, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field YawField = new Field.Builder()
            .Name(nameof(Yaw))
            .Title("yaw")
            .Description("Yaw/pan angle (positive: pan the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _yaw;
        public float Yaw { get => _yaw; set => _yaw = value; }
        /// <summary>
        /// Pitch/tilt angular rate (positive: tilt up). NaN to be ignored.
        /// OriginName: pitch_rate, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field PitchRateField = new Field.Builder()
            .Name(nameof(PitchRate))
            .Title("pitch_rate")
            .Description("Pitch/tilt angular rate (positive: tilt up). NaN to be ignored.")
.Units(@"rad/s")
            .DataType(FloatType.Default)
        .Build();
        private float _pitchRate;
        public float PitchRate { get => _pitchRate; set => _pitchRate = value; }
        /// <summary>
        /// Yaw/pan angular rate (positive: pan to the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.
        /// OriginName: yaw_rate, Units: rad/s, IsExtended: false
        /// </summary>
        public static readonly Field YawRateField = new Field.Builder()
            .Name(nameof(YawRate))
            .Title("yaw_rate")
            .Description("Yaw/pan angular rate (positive: pan to the right). NaN to be ignored. The frame is determined by the GIMBAL_DEVICE_FLAGS_YAW_IN_xxx_FRAME flags.")
.Units(@"rad/s")
            .DataType(FloatType.Default)
        .Build();
        private float _yawRate;
        public float YawRate { get => _yawRate; set => _yawRate = value; }
        /// <summary>
        /// Gimbal device flags to be applied (UINT16_MAX to be ignored). Same flags as used in GIMBAL_DEVICE_SET_ATTITUDE.
        /// OriginName: device_flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DeviceFlagsField = new Field.Builder()
            .Name(nameof(DeviceFlags))
            .Title("device_flags")
            .Description("Gimbal device flags to be applied (UINT16_MAX to be ignored). Same flags as used in GIMBAL_DEVICE_SET_ATTITUDE.")
            .DataType(new UInt16Type(GimbalDeviceFlagsHelper.GetValues(x=>(ushort)x).Min(),GimbalDeviceFlagsHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(GimbalDeviceFlagsHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private GimbalDeviceFlags _deviceFlags;
        public GimbalDeviceFlags DeviceFlags { get => _deviceFlags; set => _deviceFlags = value; } 
        /// <summary>
        /// Gimbal manager flags to be applied (0 to be ignored).
        /// OriginName: manager_flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ManagerFlagsField = new Field.Builder()
            .Name(nameof(ManagerFlags))
            .Title("manager_flags")
            .Description("Gimbal manager flags to be applied (0 to be ignored).")
            .DataType(new UInt16Type(MavStorm32GimbalManagerFlagsHelper.GetValues(x=>(ushort)x).Min(),MavStorm32GimbalManagerFlagsHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(MavStorm32GimbalManagerFlagsHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private MavStorm32GimbalManagerFlags _managerFlags;
        public MavStorm32GimbalManagerFlags ManagerFlags { get => _managerFlags; set => _managerFlags = value; } 
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetSystem;
        public byte TargetSystem { get => _targetSystem; set => _targetSystem = value; }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetComponent;
        public byte TargetComponent { get => _targetComponent; set => _targetComponent = value; }
        /// <summary>
        /// Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GimbalIdField = new Field.Builder()
            .Name(nameof(GimbalId))
            .Title("gimbal_id")
            .Description("Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _gimbalId;
        public byte GimbalId { get => _gimbalId; set => _gimbalId = value; }
        /// <summary>
        /// Client which is contacting the gimbal manager (must be set).
        /// OriginName: client, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ClientField = new Field.Builder()
            .Name(nameof(Client))
            .Title("client")
            .Description("Client which is contacting the gimbal manager (must be set).")
            .DataType(new UInt8Type(MavStorm32GimbalManagerClientHelper.GetValues(x=>(byte)x).Min(),MavStorm32GimbalManagerClientHelper.GetValues(x=>(byte)x).Max()))
            .Enum(MavStorm32GimbalManagerClientHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private MavStorm32GimbalManagerClient _client;
        public MavStorm32GimbalManagerClient Client { get => _client; set => _client = value; } 
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

        public void Accept(IVisitor visitor)
        {
            FloatType.Accept(visitor,RollField, RollField.DataType, ref _roll);    
            UInt8Type.Accept(visitor,TargetSystemField, TargetSystemField.DataType, ref _targetSystem);    
            UInt8Type.Accept(visitor,TargetComponentField, TargetComponentField.DataType, ref _targetComponent);    
            UInt8Type.Accept(visitor,GimbalIdField, GimbalIdField.DataType, ref _gimbalId);    
            var tmpClient = (byte)Client;
            UInt8Type.Accept(visitor,ClientField, ClientField.DataType, ref tmpClient);
            Client = (MavStorm32GimbalManagerClient)tmpClient;

        }

        /// <summary>
        /// Roll angle (positive to roll to the right).
        /// OriginName: roll, Units: rad, IsExtended: false
        /// </summary>
        public static readonly Field RollField = new Field.Builder()
            .Name(nameof(Roll))
            .Title("roll")
            .Description("Roll angle (positive to roll to the right).")
.Units(@"rad")
            .DataType(FloatType.Default)
        .Build();
        private float _roll;
        public float Roll { get => _roll; set => _roll = value; }
        /// <summary>
        /// System ID
        /// OriginName: target_system, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetSystemField = new Field.Builder()
            .Name(nameof(TargetSystem))
            .Title("target_system")
            .Description("System ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetSystem;
        public byte TargetSystem { get => _targetSystem; set => _targetSystem = value; }
        /// <summary>
        /// Component ID
        /// OriginName: target_component, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TargetComponentField = new Field.Builder()
            .Name(nameof(TargetComponent))
            .Title("target_component")
            .Description("Component ID")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _targetComponent;
        public byte TargetComponent { get => _targetComponent; set => _targetComponent = value; }
        /// <summary>
        /// Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.
        /// OriginName: gimbal_id, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GimbalIdField = new Field.Builder()
            .Name(nameof(GimbalId))
            .Title("gimbal_id")
            .Description("Gimbal ID of the gimbal manager to address (component ID or 1-6 for non-MAVLink gimbal, 0 for all gimbals). Send command multiple times for more than one but not all gimbals.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _gimbalId;
        public byte GimbalId { get => _gimbalId; set => _gimbalId = value; }
        /// <summary>
        /// Client which is contacting the gimbal manager (must be set).
        /// OriginName: client, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ClientField = new Field.Builder()
            .Name(nameof(Client))
            .Title("client")
            .Description("Client which is contacting the gimbal manager (must be set).")
            .DataType(new UInt8Type(MavStorm32GimbalManagerClientHelper.GetValues(x=>(byte)x).Min(),MavStorm32GimbalManagerClientHelper.GetValues(x=>(byte)x).Max()))
            .Enum(MavStorm32GimbalManagerClientHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private MavStorm32GimbalManagerClient _client;
        public MavStorm32GimbalManagerClient Client { get => _client; set => _client = value; } 
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

        public void Accept(IVisitor visitor)
        {
            var tmpMode = (ushort)Mode;
            UInt16Type.Accept(visitor,ModeField, ModeField.DataType, ref tmpMode);
            Mode = (MavQshotMode)tmpMode;
            UInt16Type.Accept(visitor,ShotStateField, ShotStateField.DataType, ref _shotState);    

        }

        /// <summary>
        /// Current shot mode.
        /// OriginName: mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ModeField = new Field.Builder()
            .Name(nameof(Mode))
            .Title("mode")
            .Description("Current shot mode.")
            .DataType(new UInt16Type(MavQshotModeHelper.GetValues(x=>(ushort)x).Min(),MavQshotModeHelper.GetValues(x=>(ushort)x).Max()))
            .Enum(MavQshotModeHelper.GetEnumValues(x=>(ushort)x))
            .Build();
        private MavQshotMode _mode;
        public MavQshotMode Mode { get => _mode; set => _mode = value; } 
        /// <summary>
        /// Current state in the shot. States are specific to the selected shot mode.
        /// OriginName: shot_state, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ShotStateField = new Field.Builder()
            .Name(nameof(ShotState))
            .Title("shot_state")
            .Description("Current state in the shot. States are specific to the selected shot mode.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _shotState;
        public ushort ShotState { get => _shotState; set => _shotState = value; }
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

        public void Accept(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,CountField, CountField.DataType, ref _count);    
            var tmpFlags = (byte)Flags;
            UInt8Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (RadioRcChannelsFlags)tmpFlags;
            ArrayType.Accept(visitor,ChannelsField, ChannelsField.DataType, 24,
                (index, v, f, t) => Int16Type.Accept(v, f, t, ref Channels[index]));    

        }

        /// <summary>
        /// Total number of RC channels being received. This can be larger than 24, indicating that more channels are available but not given in this message.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Total number of RC channels being received. This can be larger than 24, indicating that more channels are available but not given in this message.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _count;
        public byte Count { get => _count; set => _count = value; }
        /// <summary>
        /// Radio channels status flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("bitmask")
            .Description("Radio channels status flags.")
            .DataType(new UInt8Type(RadioRcChannelsFlagsHelper.GetValues(x=>(byte)x).Min(),RadioRcChannelsFlagsHelper.GetValues(x=>(byte)x).Max()))
            .Enum(RadioRcChannelsFlagsHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private RadioRcChannelsFlags _flags;
        public RadioRcChannelsFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// RC channels. Channels above count should be set to 0, to benefit from MAVLink's zero padding.
        /// OriginName: channels, Units: , IsExtended: true
        /// </summary>
        public static readonly Field ChannelsField = new Field.Builder()
            .Name(nameof(Channels))
            .Title("channels")
            .Description("RC channels. Channels above count should be set to 0, to benefit from MAVLink's zero padding.")

            .DataType(new ArrayType(Int16Type.Default,24))
        .Build();
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

        public void Accept(IVisitor visitor)
        {
            var tmpFlags = (byte)Flags;
            UInt8Type.Accept(visitor,FlagsField, FlagsField.DataType, ref tmpFlags);
            Flags = (RadioLinkStatsFlags)tmpFlags;
            UInt8Type.Accept(visitor,RxLqField, RxLqField.DataType, ref _rxLq);    
            UInt8Type.Accept(visitor,RxRssi1Field, RxRssi1Field.DataType, ref _rxRssi1);    
            Int8Type.Accept(visitor,RxSnr1Field, RxSnr1Field.DataType, ref _rxSnr1);                
            UInt8Type.Accept(visitor,RxRssi2Field, RxRssi2Field.DataType, ref _rxRssi2);    
            Int8Type.Accept(visitor,RxSnr2Field, RxSnr2Field.DataType, ref _rxSnr2);                
            UInt8Type.Accept(visitor,RxReceiveAntennaField, RxReceiveAntennaField.DataType, ref _rxReceiveAntenna);    
            UInt8Type.Accept(visitor,RxTransmitAntennaField, RxTransmitAntennaField.DataType, ref _rxTransmitAntenna);    
            UInt8Type.Accept(visitor,TxLqField, TxLqField.DataType, ref _txLq);    
            UInt8Type.Accept(visitor,TxRssi1Field, TxRssi1Field.DataType, ref _txRssi1);    
            Int8Type.Accept(visitor,TxSnr1Field, TxSnr1Field.DataType, ref _txSnr1);                
            UInt8Type.Accept(visitor,TxRssi2Field, TxRssi2Field.DataType, ref _txRssi2);    
            Int8Type.Accept(visitor,TxSnr2Field, TxSnr2Field.DataType, ref _txSnr2);                
            UInt8Type.Accept(visitor,TxReceiveAntennaField, TxReceiveAntennaField.DataType, ref _txReceiveAntenna);    
            UInt8Type.Accept(visitor,TxTransmitAntennaField, TxTransmitAntennaField.DataType, ref _txTransmitAntenna);    

        }

        /// <summary>
        /// Radio link statistics flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("bitmask")
            .Description("Radio link statistics flags.")
            .DataType(new UInt8Type(RadioLinkStatsFlagsHelper.GetValues(x=>(byte)x).Min(),RadioLinkStatsFlagsHelper.GetValues(x=>(byte)x).Max()))
            .Enum(RadioLinkStatsFlagsHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private RadioLinkStatsFlags _flags;
        public RadioLinkStatsFlags Flags { get => _flags; set => _flags = value; } 
        /// <summary>
        /// Values: 0..100. UINT8_MAX: invalid/unknown.
        /// OriginName: rx_LQ, Units: c%, IsExtended: false
        /// </summary>
        public static readonly Field RxLqField = new Field.Builder()
            .Name(nameof(RxLq))
            .Title("rx_LQ")
            .Description("Values: 0..100. UINT8_MAX: invalid/unknown.")
.Units(@"c%")
            .DataType(UInt8Type.Default)
        .Build();
        private byte _rxLq;
        public byte RxLq { get => _rxLq; set => _rxLq = value; }
        /// <summary>
        /// Rssi of antenna1. UINT8_MAX: invalid/unknown.
        /// OriginName: rx_rssi1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RxRssi1Field = new Field.Builder()
            .Name(nameof(RxRssi1))
            .Title("rx_rssi1")
            .Description("Rssi of antenna1. UINT8_MAX: invalid/unknown.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _rxRssi1;
        public byte RxRssi1 { get => _rxRssi1; set => _rxRssi1 = value; }
        /// <summary>
        /// Noise on antenna1. Radio dependent. INT8_MAX: invalid/unknown.
        /// OriginName: rx_snr1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RxSnr1Field = new Field.Builder()
            .Name(nameof(RxSnr1))
            .Title("rx_snr1")
            .Description("Noise on antenna1. Radio dependent. INT8_MAX: invalid/unknown.")

            .DataType(Int8Type.Default)
        .Build();
        private sbyte _rxSnr1;
        public sbyte RxSnr1 { get => _rxSnr1; set => _rxSnr1 = value; }
        /// <summary>
        /// Rssi of antenna2. UINT8_MAX: ignore/unknown, use rx_rssi1.
        /// OriginName: rx_rssi2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RxRssi2Field = new Field.Builder()
            .Name(nameof(RxRssi2))
            .Title("rx_rssi2")
            .Description("Rssi of antenna2. UINT8_MAX: ignore/unknown, use rx_rssi1.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _rxRssi2;
        public byte RxRssi2 { get => _rxRssi2; set => _rxRssi2 = value; }
        /// <summary>
        /// Noise on antenna2. Radio dependent. INT8_MAX: ignore/unknown, use rx_snr1.
        /// OriginName: rx_snr2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RxSnr2Field = new Field.Builder()
            .Name(nameof(RxSnr2))
            .Title("rx_snr2")
            .Description("Noise on antenna2. Radio dependent. INT8_MAX: ignore/unknown, use rx_snr1.")

            .DataType(Int8Type.Default)
        .Build();
        private sbyte _rxSnr2;
        public sbyte RxSnr2 { get => _rxSnr2; set => _rxSnr2 = value; }
        /// <summary>
        /// 0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Rx receive diversity, use rx_rssi1, rx_snr1.
        /// OriginName: rx_receive_antenna, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RxReceiveAntennaField = new Field.Builder()
            .Name(nameof(RxReceiveAntenna))
            .Title("rx_receive_antenna")
            .Description("0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Rx receive diversity, use rx_rssi1, rx_snr1.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _rxReceiveAntenna;
        public byte RxReceiveAntenna { get => _rxReceiveAntenna; set => _rxReceiveAntenna = value; }
        /// <summary>
        /// 0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Rx transmit diversity.
        /// OriginName: rx_transmit_antenna, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RxTransmitAntennaField = new Field.Builder()
            .Name(nameof(RxTransmitAntenna))
            .Title("rx_transmit_antenna")
            .Description("0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Rx transmit diversity.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _rxTransmitAntenna;
        public byte RxTransmitAntenna { get => _rxTransmitAntenna; set => _rxTransmitAntenna = value; }
        /// <summary>
        /// Values: 0..100. UINT8_MAX: invalid/unknown.
        /// OriginName: tx_LQ, Units: c%, IsExtended: false
        /// </summary>
        public static readonly Field TxLqField = new Field.Builder()
            .Name(nameof(TxLq))
            .Title("tx_LQ")
            .Description("Values: 0..100. UINT8_MAX: invalid/unknown.")
.Units(@"c%")
            .DataType(UInt8Type.Default)
        .Build();
        private byte _txLq;
        public byte TxLq { get => _txLq; set => _txLq = value; }
        /// <summary>
        /// Rssi of antenna1. UINT8_MAX: invalid/unknown.
        /// OriginName: tx_rssi1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TxRssi1Field = new Field.Builder()
            .Name(nameof(TxRssi1))
            .Title("tx_rssi1")
            .Description("Rssi of antenna1. UINT8_MAX: invalid/unknown.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _txRssi1;
        public byte TxRssi1 { get => _txRssi1; set => _txRssi1 = value; }
        /// <summary>
        /// Noise on antenna1. Radio dependent. INT8_MAX: invalid/unknown.
        /// OriginName: tx_snr1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TxSnr1Field = new Field.Builder()
            .Name(nameof(TxSnr1))
            .Title("tx_snr1")
            .Description("Noise on antenna1. Radio dependent. INT8_MAX: invalid/unknown.")

            .DataType(Int8Type.Default)
        .Build();
        private sbyte _txSnr1;
        public sbyte TxSnr1 { get => _txSnr1; set => _txSnr1 = value; }
        /// <summary>
        /// Rssi of antenna2. UINT8_MAX: ignore/unknown, use tx_rssi1.
        /// OriginName: tx_rssi2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TxRssi2Field = new Field.Builder()
            .Name(nameof(TxRssi2))
            .Title("tx_rssi2")
            .Description("Rssi of antenna2. UINT8_MAX: ignore/unknown, use tx_rssi1.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _txRssi2;
        public byte TxRssi2 { get => _txRssi2; set => _txRssi2 = value; }
        /// <summary>
        /// Noise on antenna2. Radio dependent. INT8_MAX: ignore/unknown, use tx_snr1.
        /// OriginName: tx_snr2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TxSnr2Field = new Field.Builder()
            .Name(nameof(TxSnr2))
            .Title("tx_snr2")
            .Description("Noise on antenna2. Radio dependent. INT8_MAX: ignore/unknown, use tx_snr1.")

            .DataType(Int8Type.Default)
        .Build();
        private sbyte _txSnr2;
        public sbyte TxSnr2 { get => _txSnr2; set => _txSnr2 = value; }
        /// <summary>
        /// 0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Tx receive diversity, use tx_rssi1, tx_snr1.
        /// OriginName: tx_receive_antenna, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TxReceiveAntennaField = new Field.Builder()
            .Name(nameof(TxReceiveAntenna))
            .Title("tx_receive_antenna")
            .Description("0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Tx receive diversity, use tx_rssi1, tx_snr1.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _txReceiveAntenna;
        public byte TxReceiveAntenna { get => _txReceiveAntenna; set => _txReceiveAntenna = value; }
        /// <summary>
        /// 0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Tx transmit diversity.
        /// OriginName: tx_transmit_antenna, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TxTransmitAntennaField = new Field.Builder()
            .Name(nameof(TxTransmitAntenna))
            .Title("tx_transmit_antenna")
            .Description("0: antenna1, 1: antenna2, UINT8_MAX: ignore, no Tx transmit diversity.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _txTransmitAntenna;
        public byte TxTransmitAntenna { get => _txTransmitAntenna; set => _txTransmitAntenna = value; }
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

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,TimeBootMsField, TimeBootMsField.DataType, ref _timeBootMs);    
            UInt8Type.Accept(visitor,CountField, CountField.DataType, ref _count);    
            ArrayType.Accept(visitor,PacketBufField, PacketBufField.DataType, 240,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref PacketBuf[index]));    

        }

        /// <summary>
        /// Timestamp (time since system boot).
        /// OriginName: time_boot_ms, Units: ms, IsExtended: false
        /// </summary>
        public static readonly Field TimeBootMsField = new Field.Builder()
            .Name(nameof(TimeBootMs))
            .Title("time_boot_ms")
            .Description("Timestamp (time since system boot).")
.Units(@"ms")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _timeBootMs;
        public uint TimeBootMs { get => _timeBootMs; set => _timeBootMs = value; }
        /// <summary>
        /// Number of passthrough packets in this message.
        /// OriginName: count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CountField = new Field.Builder()
            .Name(nameof(Count))
            .Title("count")
            .Description("Number of passthrough packets in this message.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _count;
        public byte Count { get => _count; set => _count = value; }
        /// <summary>
        /// Passthrough packet buffer. A packet has 6 bytes: uint16_t id + uint32_t data. The array has space for 40 packets.
        /// OriginName: packet_buf, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PacketBufField = new Field.Builder()
            .Name(nameof(PacketBuf))
            .Title("packet_buf")
            .Description("Passthrough packet buffer. A packet has 6 bytes: uint16_t id + uint32_t data. The array has space for 40 packets.")

            .DataType(new ArrayType(UInt8Type.Default,240))
        .Build();
        public const int PacketBufMaxItemsCount = 240;
        public byte[] PacketBuf { get; } = new byte[240];
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

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,ParamCountField, ParamCountField.DataType, ref _paramCount);    
            UInt16Type.Accept(visitor,ParamIndexFirstField, ParamIndexFirstField.DataType, ref _paramIndexFirst);    
            UInt16Type.Accept(visitor,FlagsField, FlagsField.DataType, ref _flags);    
            UInt8Type.Accept(visitor,ParamArrayLenField, ParamArrayLenField.DataType, ref _paramArrayLen);    
            ArrayType.Accept(visitor,PacketBufField, PacketBufField.DataType, 248,
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref PacketBuf[index]));    

        }

        /// <summary>
        /// Total number of onboard parameters.
        /// OriginName: param_count, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ParamCountField = new Field.Builder()
            .Name(nameof(ParamCount))
            .Title("param_count")
            .Description("Total number of onboard parameters.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _paramCount;
        public ushort ParamCount { get => _paramCount; set => _paramCount = value; }
        /// <summary>
        /// Index of the first onboard parameter in this array.
        /// OriginName: param_index_first, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ParamIndexFirstField = new Field.Builder()
            .Name(nameof(ParamIndexFirst))
            .Title("param_index_first")
            .Description("Index of the first onboard parameter in this array.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _paramIndexFirst;
        public ushort ParamIndexFirst { get => _paramIndexFirst; set => _paramIndexFirst = value; }
        /// <summary>
        /// Flags.
        /// OriginName: flags, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FlagsField = new Field.Builder()
            .Name(nameof(Flags))
            .Title("flags")
            .Description("Flags.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _flags;
        public ushort Flags { get => _flags; set => _flags = value; }
        /// <summary>
        /// Number of onboard parameters in this array.
        /// OriginName: param_array_len, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ParamArrayLenField = new Field.Builder()
            .Name(nameof(ParamArrayLen))
            .Title("param_array_len")
            .Description("Number of onboard parameters in this array.")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _paramArrayLen;
        public byte ParamArrayLen { get => _paramArrayLen; set => _paramArrayLen = value; }
        /// <summary>
        /// Parameters buffer. Contains a series of variable length parameter blocks, one per parameter, with format as specifed elsewhere.
        /// OriginName: packet_buf, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PacketBufField = new Field.Builder()
            .Name(nameof(PacketBuf))
            .Title("packet_buf")
            .Description("Parameters buffer. Contains a series of variable length parameter blocks, one per parameter, with format as specifed elsewhere.")

            .DataType(new ArrayType(UInt8Type.Default,248))
        .Build();
        public const int PacketBufMaxItemsCount = 248;
        public byte[] PacketBuf { get; } = new byte[248];
        [Obsolete("This method is deprecated. Use GetPacketBufMaxItemsCount instead.")]
        public byte GetPacketBufMaxItemsCount() => 248;
    }




        


#endregion


}

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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.17-dev.8+356100e330ee3351d1c0a76be38f09294117ae6a 25-09-26.

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

namespace Asv.Mavlink.Minimal
{

    public static class MinimalHelper
    {
        public static void RegisterMinimalDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(HeartbeatPacket.MessageId, ()=>new HeartbeatPacket());
            src.Add(ProtocolVersionPacket.MessageId, ()=>new ProtocolVersionPacket());
        }
 
    }

#region Enums

    /// <summary>
    /// Micro air vehicle / autopilot classes. This identifies the individual model.
    ///  MAV_AUTOPILOT
    /// </summary>
    public enum MavAutopilot : ulong
    {
        /// <summary>
        /// Generic autopilot, full support for everything
        /// MAV_AUTOPILOT_GENERIC
        /// </summary>
        MavAutopilotGeneric = 0,
        /// <summary>
        /// Reserved for future use.
        /// MAV_AUTOPILOT_RESERVED
        /// </summary>
        MavAutopilotReserved = 1,
        /// <summary>
        /// SLUGS autopilot, http://slugsuav.soe.ucsc.edu
        /// MAV_AUTOPILOT_SLUGS
        /// </summary>
        MavAutopilotSlugs = 2,
        /// <summary>
        /// ArduPilot - Plane/Copter/Rover/Sub/Tracker, https://ardupilot.org
        /// MAV_AUTOPILOT_ARDUPILOTMEGA
        /// </summary>
        MavAutopilotArdupilotmega = 3,
        /// <summary>
        /// OpenPilot, http://openpilot.org
        /// MAV_AUTOPILOT_OPENPILOT
        /// </summary>
        MavAutopilotOpenpilot = 4,
        /// <summary>
        /// Generic autopilot only supporting simple waypoints
        /// MAV_AUTOPILOT_GENERIC_WAYPOINTS_ONLY
        /// </summary>
        MavAutopilotGenericWaypointsOnly = 5,
        /// <summary>
        /// Generic autopilot supporting waypoints and other simple navigation commands
        /// MAV_AUTOPILOT_GENERIC_WAYPOINTS_AND_SIMPLE_NAVIGATION_ONLY
        /// </summary>
        MavAutopilotGenericWaypointsAndSimpleNavigationOnly = 6,
        /// <summary>
        /// Generic autopilot supporting the full mission command set
        /// MAV_AUTOPILOT_GENERIC_MISSION_FULL
        /// </summary>
        MavAutopilotGenericMissionFull = 7,
        /// <summary>
        /// No valid autopilot, e.g. a GCS or other MAVLink component
        /// MAV_AUTOPILOT_INVALID
        /// </summary>
        MavAutopilotInvalid = 8,
        /// <summary>
        /// PPZ UAV - http://nongnu.org/paparazzi
        /// MAV_AUTOPILOT_PPZ
        /// </summary>
        MavAutopilotPpz = 9,
        /// <summary>
        /// UAV Dev Board
        /// MAV_AUTOPILOT_UDB
        /// </summary>
        MavAutopilotUdb = 10,
        /// <summary>
        /// FlexiPilot
        /// MAV_AUTOPILOT_FP
        /// </summary>
        MavAutopilotFp = 11,
        /// <summary>
        /// PX4 Autopilot - http://px4.io/
        /// MAV_AUTOPILOT_PX4
        /// </summary>
        MavAutopilotPx4 = 12,
        /// <summary>
        /// SMACCMPilot - http://smaccmpilot.org
        /// MAV_AUTOPILOT_SMACCMPILOT
        /// </summary>
        MavAutopilotSmaccmpilot = 13,
        /// <summary>
        /// AutoQuad -- http://autoquad.org
        /// MAV_AUTOPILOT_AUTOQUAD
        /// </summary>
        MavAutopilotAutoquad = 14,
        /// <summary>
        /// Armazila -- http://armazila.com
        /// MAV_AUTOPILOT_ARMAZILA
        /// </summary>
        MavAutopilotArmazila = 15,
        /// <summary>
        /// Aerob -- http://aerob.ru
        /// MAV_AUTOPILOT_AEROB
        /// </summary>
        MavAutopilotAerob = 16,
        /// <summary>
        /// ASLUAV autopilot -- http://www.asl.ethz.ch
        /// MAV_AUTOPILOT_ASLUAV
        /// </summary>
        MavAutopilotAsluav = 17,
        /// <summary>
        /// SmartAP Autopilot - http://sky-drones.com
        /// MAV_AUTOPILOT_SMARTAP
        /// </summary>
        MavAutopilotSmartap = 18,
        /// <summary>
        /// AirRails - http://uaventure.com
        /// MAV_AUTOPILOT_AIRRAILS
        /// </summary>
        MavAutopilotAirrails = 19,
        /// <summary>
        /// Fusion Reflex - https://fusion.engineering
        /// MAV_AUTOPILOT_REFLEX
        /// </summary>
        MavAutopilotReflex = 20,
    }
    public static class MavAutopilotHelper
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
            yield return converter(10);
            yield return converter(11);
            yield return converter(12);
            yield return converter(13);
            yield return converter(14);
            yield return converter(15);
            yield return converter(16);
            yield return converter(17);
            yield return converter(18);
            yield return converter(19);
            yield return converter(20);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"MAV_AUTOPILOT_GENERIC");
            yield return new EnumValue<T>(converter(1),"MAV_AUTOPILOT_RESERVED");
            yield return new EnumValue<T>(converter(2),"MAV_AUTOPILOT_SLUGS");
            yield return new EnumValue<T>(converter(3),"MAV_AUTOPILOT_ARDUPILOTMEGA");
            yield return new EnumValue<T>(converter(4),"MAV_AUTOPILOT_OPENPILOT");
            yield return new EnumValue<T>(converter(5),"MAV_AUTOPILOT_GENERIC_WAYPOINTS_ONLY");
            yield return new EnumValue<T>(converter(6),"MAV_AUTOPILOT_GENERIC_WAYPOINTS_AND_SIMPLE_NAVIGATION_ONLY");
            yield return new EnumValue<T>(converter(7),"MAV_AUTOPILOT_GENERIC_MISSION_FULL");
            yield return new EnumValue<T>(converter(8),"MAV_AUTOPILOT_INVALID");
            yield return new EnumValue<T>(converter(9),"MAV_AUTOPILOT_PPZ");
            yield return new EnumValue<T>(converter(10),"MAV_AUTOPILOT_UDB");
            yield return new EnumValue<T>(converter(11),"MAV_AUTOPILOT_FP");
            yield return new EnumValue<T>(converter(12),"MAV_AUTOPILOT_PX4");
            yield return new EnumValue<T>(converter(13),"MAV_AUTOPILOT_SMACCMPILOT");
            yield return new EnumValue<T>(converter(14),"MAV_AUTOPILOT_AUTOQUAD");
            yield return new EnumValue<T>(converter(15),"MAV_AUTOPILOT_ARMAZILA");
            yield return new EnumValue<T>(converter(16),"MAV_AUTOPILOT_AEROB");
            yield return new EnumValue<T>(converter(17),"MAV_AUTOPILOT_ASLUAV");
            yield return new EnumValue<T>(converter(18),"MAV_AUTOPILOT_SMARTAP");
            yield return new EnumValue<T>(converter(19),"MAV_AUTOPILOT_AIRRAILS");
            yield return new EnumValue<T>(converter(20),"MAV_AUTOPILOT_REFLEX");
        }
    }
    /// <summary>
    /// MAVLINK component type reported in HEARTBEAT message. Flight controllers must report the type of the vehicle on which they are mounted (e.g. MAV_TYPE_OCTOROTOR). All other components must report a value appropriate for their type (e.g. a camera must use MAV_TYPE_CAMERA).
    ///  MAV_TYPE
    /// </summary>
    public enum MavType : ulong
    {
        /// <summary>
        /// Generic micro air vehicle
        /// MAV_TYPE_GENERIC
        /// </summary>
        MavTypeGeneric = 0,
        /// <summary>
        /// Fixed wing aircraft.
        /// MAV_TYPE_FIXED_WING
        /// </summary>
        MavTypeFixedWing = 1,
        /// <summary>
        /// Quadrotor
        /// MAV_TYPE_QUADROTOR
        /// </summary>
        MavTypeQuadrotor = 2,
        /// <summary>
        /// Coaxial helicopter
        /// MAV_TYPE_COAXIAL
        /// </summary>
        MavTypeCoaxial = 3,
        /// <summary>
        /// Normal helicopter with tail rotor.
        /// MAV_TYPE_HELICOPTER
        /// </summary>
        MavTypeHelicopter = 4,
        /// <summary>
        /// Ground installation
        /// MAV_TYPE_ANTENNA_TRACKER
        /// </summary>
        MavTypeAntennaTracker = 5,
        /// <summary>
        /// Operator control unit / ground control station
        /// MAV_TYPE_GCS
        /// </summary>
        MavTypeGcs = 6,
        /// <summary>
        /// Airship, controlled
        /// MAV_TYPE_AIRSHIP
        /// </summary>
        MavTypeAirship = 7,
        /// <summary>
        /// Free balloon, uncontrolled
        /// MAV_TYPE_FREE_BALLOON
        /// </summary>
        MavTypeFreeBalloon = 8,
        /// <summary>
        /// Rocket
        /// MAV_TYPE_ROCKET
        /// </summary>
        MavTypeRocket = 9,
        /// <summary>
        /// Ground rover
        /// MAV_TYPE_GROUND_ROVER
        /// </summary>
        MavTypeGroundRover = 10,
        /// <summary>
        /// Surface vessel, boat, ship
        /// MAV_TYPE_SURFACE_BOAT
        /// </summary>
        MavTypeSurfaceBoat = 11,
        /// <summary>
        /// Submarine
        /// MAV_TYPE_SUBMARINE
        /// </summary>
        MavTypeSubmarine = 12,
        /// <summary>
        /// Hexarotor
        /// MAV_TYPE_HEXAROTOR
        /// </summary>
        MavTypeHexarotor = 13,
        /// <summary>
        /// Octorotor
        /// MAV_TYPE_OCTOROTOR
        /// </summary>
        MavTypeOctorotor = 14,
        /// <summary>
        /// Tricopter
        /// MAV_TYPE_TRICOPTER
        /// </summary>
        MavTypeTricopter = 15,
        /// <summary>
        /// Flapping wing
        /// MAV_TYPE_FLAPPING_WING
        /// </summary>
        MavTypeFlappingWing = 16,
        /// <summary>
        /// Kite
        /// MAV_TYPE_KITE
        /// </summary>
        MavTypeKite = 17,
        /// <summary>
        /// Onboard companion controller
        /// MAV_TYPE_ONBOARD_CONTROLLER
        /// </summary>
        MavTypeOnboardController = 18,
        /// <summary>
        /// Two-rotor Tailsitter VTOL that additionally uses control surfaces in vertical operation. Note, value previously named MAV_TYPE_VTOL_DUOROTOR.
        /// MAV_TYPE_VTOL_TAILSITTER_DUOROTOR
        /// </summary>
        MavTypeVtolTailsitterDuorotor = 19,
        /// <summary>
        /// Quad-rotor Tailsitter VTOL using a V-shaped quad config in vertical operation. Note: value previously named MAV_TYPE_VTOL_QUADROTOR.
        /// MAV_TYPE_VTOL_TAILSITTER_QUADROTOR
        /// </summary>
        MavTypeVtolTailsitterQuadrotor = 20,
        /// <summary>
        /// Tiltrotor VTOL. Fuselage and wings stay (nominally) horizontal in all flight phases. It able to tilt (some) rotors to provide thrust in cruise flight.
        /// MAV_TYPE_VTOL_TILTROTOR
        /// </summary>
        MavTypeVtolTiltrotor = 21,
        /// <summary>
        /// VTOL with separate fixed rotors for hover and cruise flight. Fuselage and wings stay (nominally) horizontal in all flight phases.
        /// MAV_TYPE_VTOL_FIXEDROTOR
        /// </summary>
        MavTypeVtolFixedrotor = 22,
        /// <summary>
        /// Tailsitter VTOL. Fuselage and wings orientation changes depending on flight phase: vertical for hover, horizontal for cruise. Use more specific VTOL MAV_TYPE_VTOL_DUOROTOR or MAV_TYPE_VTOL_QUADROTOR if appropriate.
        /// MAV_TYPE_VTOL_TAILSITTER
        /// </summary>
        MavTypeVtolTailsitter = 23,
        /// <summary>
        /// Tiltwing VTOL. Fuselage stays horizontal in all flight phases. The whole wing, along with any attached engine, can tilt between vertical and horizontal mode.
        /// MAV_TYPE_VTOL_TILTWING
        /// </summary>
        MavTypeVtolTiltwing = 24,
        /// <summary>
        /// VTOL reserved 5
        /// MAV_TYPE_VTOL_RESERVED5
        /// </summary>
        MavTypeVtolReserved5 = 25,
        /// <summary>
        /// Gimbal
        /// MAV_TYPE_GIMBAL
        /// </summary>
        MavTypeGimbal = 26,
        /// <summary>
        /// ADSB system
        /// MAV_TYPE_ADSB
        /// </summary>
        MavTypeAdsb = 27,
        /// <summary>
        /// Steerable, nonrigid airfoil
        /// MAV_TYPE_PARAFOIL
        /// </summary>
        MavTypeParafoil = 28,
        /// <summary>
        /// Dodecarotor
        /// MAV_TYPE_DODECAROTOR
        /// </summary>
        MavTypeDodecarotor = 29,
        /// <summary>
        /// Camera
        /// MAV_TYPE_CAMERA
        /// </summary>
        MavTypeCamera = 30,
        /// <summary>
        /// Charging station
        /// MAV_TYPE_CHARGING_STATION
        /// </summary>
        MavTypeChargingStation = 31,
        /// <summary>
        /// FLARM collision avoidance system
        /// MAV_TYPE_FLARM
        /// </summary>
        MavTypeFlarm = 32,
        /// <summary>
        /// Servo
        /// MAV_TYPE_SERVO
        /// </summary>
        MavTypeServo = 33,
        /// <summary>
        /// Open Drone ID. See https://mavlink.io/en/services/opendroneid.html.
        /// MAV_TYPE_ODID
        /// </summary>
        MavTypeOdid = 34,
        /// <summary>
        /// Decarotor
        /// MAV_TYPE_DECAROTOR
        /// </summary>
        MavTypeDecarotor = 35,
        /// <summary>
        /// Battery
        /// MAV_TYPE_BATTERY
        /// </summary>
        MavTypeBattery = 36,
        /// <summary>
        /// Parachute
        /// MAV_TYPE_PARACHUTE
        /// </summary>
        MavTypeParachute = 37,
        /// <summary>
        /// LoggerFactory
        /// MAV_TYPE_LOG
        /// </summary>
        MavTypeLog = 38,
        /// <summary>
        /// OSD
        /// MAV_TYPE_OSD
        /// </summary>
        MavTypeOsd = 39,
        /// <summary>
        /// IMU
        /// MAV_TYPE_IMU
        /// </summary>
        MavTypeImu = 40,
        /// <summary>
        /// GPS
        /// MAV_TYPE_GPS
        /// </summary>
        MavTypeGps = 41,
        /// <summary>
        /// Winch
        /// MAV_TYPE_WINCH
        /// </summary>
        MavTypeWinch = 42,
        /// <summary>
        /// Generic multirotor that does not fit into a specific type or whose type is unknown
        /// MAV_TYPE_GENERIC_MULTIROTOR
        /// </summary>
        MavTypeGenericMultirotor = 43,
    }
    public static class MavTypeHelper
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
            yield return converter(10);
            yield return converter(11);
            yield return converter(12);
            yield return converter(13);
            yield return converter(14);
            yield return converter(15);
            yield return converter(16);
            yield return converter(17);
            yield return converter(18);
            yield return converter(19);
            yield return converter(20);
            yield return converter(21);
            yield return converter(22);
            yield return converter(23);
            yield return converter(24);
            yield return converter(25);
            yield return converter(26);
            yield return converter(27);
            yield return converter(28);
            yield return converter(29);
            yield return converter(30);
            yield return converter(31);
            yield return converter(32);
            yield return converter(33);
            yield return converter(34);
            yield return converter(35);
            yield return converter(36);
            yield return converter(37);
            yield return converter(38);
            yield return converter(39);
            yield return converter(40);
            yield return converter(41);
            yield return converter(42);
            yield return converter(43);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"MAV_TYPE_GENERIC");
            yield return new EnumValue<T>(converter(1),"MAV_TYPE_FIXED_WING");
            yield return new EnumValue<T>(converter(2),"MAV_TYPE_QUADROTOR");
            yield return new EnumValue<T>(converter(3),"MAV_TYPE_COAXIAL");
            yield return new EnumValue<T>(converter(4),"MAV_TYPE_HELICOPTER");
            yield return new EnumValue<T>(converter(5),"MAV_TYPE_ANTENNA_TRACKER");
            yield return new EnumValue<T>(converter(6),"MAV_TYPE_GCS");
            yield return new EnumValue<T>(converter(7),"MAV_TYPE_AIRSHIP");
            yield return new EnumValue<T>(converter(8),"MAV_TYPE_FREE_BALLOON");
            yield return new EnumValue<T>(converter(9),"MAV_TYPE_ROCKET");
            yield return new EnumValue<T>(converter(10),"MAV_TYPE_GROUND_ROVER");
            yield return new EnumValue<T>(converter(11),"MAV_TYPE_SURFACE_BOAT");
            yield return new EnumValue<T>(converter(12),"MAV_TYPE_SUBMARINE");
            yield return new EnumValue<T>(converter(13),"MAV_TYPE_HEXAROTOR");
            yield return new EnumValue<T>(converter(14),"MAV_TYPE_OCTOROTOR");
            yield return new EnumValue<T>(converter(15),"MAV_TYPE_TRICOPTER");
            yield return new EnumValue<T>(converter(16),"MAV_TYPE_FLAPPING_WING");
            yield return new EnumValue<T>(converter(17),"MAV_TYPE_KITE");
            yield return new EnumValue<T>(converter(18),"MAV_TYPE_ONBOARD_CONTROLLER");
            yield return new EnumValue<T>(converter(19),"MAV_TYPE_VTOL_TAILSITTER_DUOROTOR");
            yield return new EnumValue<T>(converter(20),"MAV_TYPE_VTOL_TAILSITTER_QUADROTOR");
            yield return new EnumValue<T>(converter(21),"MAV_TYPE_VTOL_TILTROTOR");
            yield return new EnumValue<T>(converter(22),"MAV_TYPE_VTOL_FIXEDROTOR");
            yield return new EnumValue<T>(converter(23),"MAV_TYPE_VTOL_TAILSITTER");
            yield return new EnumValue<T>(converter(24),"MAV_TYPE_VTOL_TILTWING");
            yield return new EnumValue<T>(converter(25),"MAV_TYPE_VTOL_RESERVED5");
            yield return new EnumValue<T>(converter(26),"MAV_TYPE_GIMBAL");
            yield return new EnumValue<T>(converter(27),"MAV_TYPE_ADSB");
            yield return new EnumValue<T>(converter(28),"MAV_TYPE_PARAFOIL");
            yield return new EnumValue<T>(converter(29),"MAV_TYPE_DODECAROTOR");
            yield return new EnumValue<T>(converter(30),"MAV_TYPE_CAMERA");
            yield return new EnumValue<T>(converter(31),"MAV_TYPE_CHARGING_STATION");
            yield return new EnumValue<T>(converter(32),"MAV_TYPE_FLARM");
            yield return new EnumValue<T>(converter(33),"MAV_TYPE_SERVO");
            yield return new EnumValue<T>(converter(34),"MAV_TYPE_ODID");
            yield return new EnumValue<T>(converter(35),"MAV_TYPE_DECAROTOR");
            yield return new EnumValue<T>(converter(36),"MAV_TYPE_BATTERY");
            yield return new EnumValue<T>(converter(37),"MAV_TYPE_PARACHUTE");
            yield return new EnumValue<T>(converter(38),"MAV_TYPE_LOG");
            yield return new EnumValue<T>(converter(39),"MAV_TYPE_OSD");
            yield return new EnumValue<T>(converter(40),"MAV_TYPE_IMU");
            yield return new EnumValue<T>(converter(41),"MAV_TYPE_GPS");
            yield return new EnumValue<T>(converter(42),"MAV_TYPE_WINCH");
            yield return new EnumValue<T>(converter(43),"MAV_TYPE_GENERIC_MULTIROTOR");
        }
    }
    /// <summary>
    /// These flags encode the MAV mode.
    ///  MAV_MODE_FLAG
    /// </summary>
    public enum MavModeFlag : ulong
    {
        /// <summary>
        /// 0b10000000 MAV safety set to armed. Motors are enabled / running / can start. Ready to fly. Additional note: this flag is to be ignore when sent in the command MAV_CMD_DO_SET_MODE and MAV_CMD_COMPONENT_ARM_DISARM shall be used instead. The flag can still be used to report the armed state.
        /// MAV_MODE_FLAG_SAFETY_ARMED
        /// </summary>
        MavModeFlagSafetyArmed = 128,
        /// <summary>
        /// 0b01000000 remote control input is enabled.
        /// MAV_MODE_FLAG_MANUAL_INPUT_ENABLED
        /// </summary>
        MavModeFlagManualInputEnabled = 64,
        /// <summary>
        /// 0b00100000 hardware in the loop simulation. All motors / actuators are blocked, but internal software is full operational.
        /// MAV_MODE_FLAG_HIL_ENABLED
        /// </summary>
        MavModeFlagHilEnabled = 32,
        /// <summary>
        /// 0b00010000 system stabilizes electronically its attitude (and optionally position). It needs however further control inputs to move around.
        /// MAV_MODE_FLAG_STABILIZE_ENABLED
        /// </summary>
        MavModeFlagStabilizeEnabled = 16,
        /// <summary>
        /// 0b00001000 guided mode enabled, system flies waypoints / mission items.
        /// MAV_MODE_FLAG_GUIDED_ENABLED
        /// </summary>
        MavModeFlagGuidedEnabled = 8,
        /// <summary>
        /// 0b00000100 autonomous mode enabled, system finds its own goal positions. Guided flag can be set or not, depends on the actual implementation.
        /// MAV_MODE_FLAG_AUTO_ENABLED
        /// </summary>
        MavModeFlagAutoEnabled = 4,
        /// <summary>
        /// 0b00000010 system has a test mode enabled. This flag is intended for temporary system tests and should not be used for stable implementations.
        /// MAV_MODE_FLAG_TEST_ENABLED
        /// </summary>
        MavModeFlagTestEnabled = 2,
        /// <summary>
        /// 0b00000001 Reserved for future use.
        /// MAV_MODE_FLAG_CUSTOM_MODE_ENABLED
        /// </summary>
        MavModeFlagCustomModeEnabled = 1,
    }
    public static class MavModeFlagHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(128);
            yield return converter(64);
            yield return converter(32);
            yield return converter(16);
            yield return converter(8);
            yield return converter(4);
            yield return converter(2);
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(128),"MAV_MODE_FLAG_SAFETY_ARMED");
            yield return new EnumValue<T>(converter(64),"MAV_MODE_FLAG_MANUAL_INPUT_ENABLED");
            yield return new EnumValue<T>(converter(32),"MAV_MODE_FLAG_HIL_ENABLED");
            yield return new EnumValue<T>(converter(16),"MAV_MODE_FLAG_STABILIZE_ENABLED");
            yield return new EnumValue<T>(converter(8),"MAV_MODE_FLAG_GUIDED_ENABLED");
            yield return new EnumValue<T>(converter(4),"MAV_MODE_FLAG_AUTO_ENABLED");
            yield return new EnumValue<T>(converter(2),"MAV_MODE_FLAG_TEST_ENABLED");
            yield return new EnumValue<T>(converter(1),"MAV_MODE_FLAG_CUSTOM_MODE_ENABLED");
        }
    }
    /// <summary>
    /// These values encode the bit positions of the decode position. These values can be used to read the value of a flag bit by combining the base_mode variable with AND with the flag position value. The result will be either 0 or 1, depending on if the flag is set or not.
    ///  MAV_MODE_FLAG_DECODE_POSITION
    /// </summary>
    public enum MavModeFlagDecodePosition : ulong
    {
        /// <summary>
        /// First bit:  10000000
        /// MAV_MODE_FLAG_DECODE_POSITION_SAFETY
        /// </summary>
        MavModeFlagDecodePositionSafety = 128,
        /// <summary>
        /// Second bit: 01000000
        /// MAV_MODE_FLAG_DECODE_POSITION_MANUAL
        /// </summary>
        MavModeFlagDecodePositionManual = 64,
        /// <summary>
        /// Third bit:  00100000
        /// MAV_MODE_FLAG_DECODE_POSITION_HIL
        /// </summary>
        MavModeFlagDecodePositionHil = 32,
        /// <summary>
        /// Fourth bit: 00010000
        /// MAV_MODE_FLAG_DECODE_POSITION_STABILIZE
        /// </summary>
        MavModeFlagDecodePositionStabilize = 16,
        /// <summary>
        /// Fifth bit:  00001000
        /// MAV_MODE_FLAG_DECODE_POSITION_GUIDED
        /// </summary>
        MavModeFlagDecodePositionGuided = 8,
        /// <summary>
        /// Sixth bit:   00000100
        /// MAV_MODE_FLAG_DECODE_POSITION_AUTO
        /// </summary>
        MavModeFlagDecodePositionAuto = 4,
        /// <summary>
        /// Seventh bit: 00000010
        /// MAV_MODE_FLAG_DECODE_POSITION_TEST
        /// </summary>
        MavModeFlagDecodePositionTest = 2,
        /// <summary>
        /// Eighth bit: 00000001
        /// MAV_MODE_FLAG_DECODE_POSITION_CUSTOM_MODE
        /// </summary>
        MavModeFlagDecodePositionCustomMode = 1,
    }
    public static class MavModeFlagDecodePositionHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(128);
            yield return converter(64);
            yield return converter(32);
            yield return converter(16);
            yield return converter(8);
            yield return converter(4);
            yield return converter(2);
            yield return converter(1);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(128),"MAV_MODE_FLAG_DECODE_POSITION_SAFETY");
            yield return new EnumValue<T>(converter(64),"MAV_MODE_FLAG_DECODE_POSITION_MANUAL");
            yield return new EnumValue<T>(converter(32),"MAV_MODE_FLAG_DECODE_POSITION_HIL");
            yield return new EnumValue<T>(converter(16),"MAV_MODE_FLAG_DECODE_POSITION_STABILIZE");
            yield return new EnumValue<T>(converter(8),"MAV_MODE_FLAG_DECODE_POSITION_GUIDED");
            yield return new EnumValue<T>(converter(4),"MAV_MODE_FLAG_DECODE_POSITION_AUTO");
            yield return new EnumValue<T>(converter(2),"MAV_MODE_FLAG_DECODE_POSITION_TEST");
            yield return new EnumValue<T>(converter(1),"MAV_MODE_FLAG_DECODE_POSITION_CUSTOM_MODE");
        }
    }
    /// <summary>
    ///  MAV_STATE
    /// </summary>
    public enum MavState : ulong
    {
        /// <summary>
        /// Uninitialized system, state is unknown.
        /// MAV_STATE_UNINIT
        /// </summary>
        MavStateUninit = 0,
        /// <summary>
        /// System is booting up.
        /// MAV_STATE_BOOT
        /// </summary>
        MavStateBoot = 1,
        /// <summary>
        /// System is calibrating and not flight-ready.
        /// MAV_STATE_CALIBRATING
        /// </summary>
        MavStateCalibrating = 2,
        /// <summary>
        /// System is grounded and on standby. It can be launched any time.
        /// MAV_STATE_STANDBY
        /// </summary>
        MavStateStandby = 3,
        /// <summary>
        /// System is active and might be already airborne. Motors are engaged.
        /// MAV_STATE_ACTIVE
        /// </summary>
        MavStateActive = 4,
        /// <summary>
        /// System is in a non-normal flight mode (failsafe). It can however still navigate.
        /// MAV_STATE_CRITICAL
        /// </summary>
        MavStateCritical = 5,
        /// <summary>
        /// System is in a non-normal flight mode (failsafe). It lost control over parts or over the whole airframe. It is in mayday and going down.
        /// MAV_STATE_EMERGENCY
        /// </summary>
        MavStateEmergency = 6,
        /// <summary>
        /// System just initialized its power-down sequence, will shut down now.
        /// MAV_STATE_POWEROFF
        /// </summary>
        MavStatePoweroff = 7,
        /// <summary>
        /// System is terminating itself (failsafe or commanded).
        /// MAV_STATE_FLIGHT_TERMINATION
        /// </summary>
        MavStateFlightTermination = 8,
    }
    public static class MavStateHelper
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
            yield return new EnumValue<T>(converter(0),"MAV_STATE_UNINIT");
            yield return new EnumValue<T>(converter(1),"MAV_STATE_BOOT");
            yield return new EnumValue<T>(converter(2),"MAV_STATE_CALIBRATING");
            yield return new EnumValue<T>(converter(3),"MAV_STATE_STANDBY");
            yield return new EnumValue<T>(converter(4),"MAV_STATE_ACTIVE");
            yield return new EnumValue<T>(converter(5),"MAV_STATE_CRITICAL");
            yield return new EnumValue<T>(converter(6),"MAV_STATE_EMERGENCY");
            yield return new EnumValue<T>(converter(7),"MAV_STATE_POWEROFF");
            yield return new EnumValue<T>(converter(8),"MAV_STATE_FLIGHT_TERMINATION");
        }
    }
    /// <summary>
    /// Component ids (values) for the different types and instances of onboard hardware/software that might make up a MAVLink system (autopilot, cameras, servos, GPS systems, avoidance systems etc.).
    ///       Components must use the appropriate ID in their source address when sending messages. Components can also use IDs to determine if they are the intended recipient of an incoming message. The MAV_COMP_ID_ALL value is used to indicate messages that must be processed by all components.
    ///       When creating new entries, components that can have multiple instances (e.g. cameras, servos etc.) should be allocated sequential values. An appropriate number of values should be left free after these components to allow the number of instances to be expanded.
    ///  MAV_COMPONENT
    /// </summary>
    public enum MavComponent : ulong
    {
        /// <summary>
        /// Target id (target_component) used to broadcast messages to all components of the receiving system. Components should attempt to process messages with this component ID and forward to components on any other interfaces. Note: This is not a valid *source* component id for a message.
        /// MAV_COMP_ID_ALL
        /// </summary>
        MavCompIdAll = 0,
        /// <summary>
        /// System flight controller component ("autopilot"). Only one autopilot is expected in a particular system.
        /// MAV_COMP_ID_AUTOPILOT1
        /// </summary>
        MavCompIdAutopilot1 = 1,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER1
        /// </summary>
        MavCompIdUser1 = 25,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER2
        /// </summary>
        MavCompIdUser2 = 26,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER3
        /// </summary>
        MavCompIdUser3 = 27,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER4
        /// </summary>
        MavCompIdUser4 = 28,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER5
        /// </summary>
        MavCompIdUser5 = 29,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER6
        /// </summary>
        MavCompIdUser6 = 30,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER7
        /// </summary>
        MavCompIdUser7 = 31,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER8
        /// </summary>
        MavCompIdUser8 = 32,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER9
        /// </summary>
        MavCompIdUser9 = 33,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER10
        /// </summary>
        MavCompIdUser10 = 34,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER11
        /// </summary>
        MavCompIdUser11 = 35,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER12
        /// </summary>
        MavCompIdUser12 = 36,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER13
        /// </summary>
        MavCompIdUser13 = 37,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER14
        /// </summary>
        MavCompIdUser14 = 38,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER15
        /// </summary>
        MavCompIdUser15 = 39,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER16
        /// </summary>
        MavCompIdUser16 = 40,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER17
        /// </summary>
        MavCompIdUser17 = 41,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER18
        /// </summary>
        MavCompIdUser18 = 42,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER19
        /// </summary>
        MavCompIdUser19 = 43,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER20
        /// </summary>
        MavCompIdUser20 = 44,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER21
        /// </summary>
        MavCompIdUser21 = 45,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER22
        /// </summary>
        MavCompIdUser22 = 46,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER23
        /// </summary>
        MavCompIdUser23 = 47,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER24
        /// </summary>
        MavCompIdUser24 = 48,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER25
        /// </summary>
        MavCompIdUser25 = 49,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER26
        /// </summary>
        MavCompIdUser26 = 50,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER27
        /// </summary>
        MavCompIdUser27 = 51,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER28
        /// </summary>
        MavCompIdUser28 = 52,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER29
        /// </summary>
        MavCompIdUser29 = 53,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER30
        /// </summary>
        MavCompIdUser30 = 54,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER31
        /// </summary>
        MavCompIdUser31 = 55,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER32
        /// </summary>
        MavCompIdUser32 = 56,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER33
        /// </summary>
        MavCompIdUser33 = 57,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER34
        /// </summary>
        MavCompIdUser34 = 58,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER35
        /// </summary>
        MavCompIdUser35 = 59,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER36
        /// </summary>
        MavCompIdUser36 = 60,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER37
        /// </summary>
        MavCompIdUser37 = 61,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER38
        /// </summary>
        MavCompIdUser38 = 62,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER39
        /// </summary>
        MavCompIdUser39 = 63,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER40
        /// </summary>
        MavCompIdUser40 = 64,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER41
        /// </summary>
        MavCompIdUser41 = 65,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER42
        /// </summary>
        MavCompIdUser42 = 66,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER43
        /// </summary>
        MavCompIdUser43 = 67,
        /// <summary>
        /// Telemetry radio (e.g. SiK radio, or other component that emits RADIO_STATUS messages).
        /// MAV_COMP_ID_TELEMETRY_RADIO
        /// </summary>
        MavCompIdTelemetryRadio = 68,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER45
        /// </summary>
        MavCompIdUser45 = 69,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER46
        /// </summary>
        MavCompIdUser46 = 70,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER47
        /// </summary>
        MavCompIdUser47 = 71,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER48
        /// </summary>
        MavCompIdUser48 = 72,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER49
        /// </summary>
        MavCompIdUser49 = 73,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER50
        /// </summary>
        MavCompIdUser50 = 74,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER51
        /// </summary>
        MavCompIdUser51 = 75,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER52
        /// </summary>
        MavCompIdUser52 = 76,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER53
        /// </summary>
        MavCompIdUser53 = 77,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER54
        /// </summary>
        MavCompIdUser54 = 78,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER55
        /// </summary>
        MavCompIdUser55 = 79,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER56
        /// </summary>
        MavCompIdUser56 = 80,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER57
        /// </summary>
        MavCompIdUser57 = 81,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER58
        /// </summary>
        MavCompIdUser58 = 82,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER59
        /// </summary>
        MavCompIdUser59 = 83,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER60
        /// </summary>
        MavCompIdUser60 = 84,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER61
        /// </summary>
        MavCompIdUser61 = 85,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER62
        /// </summary>
        MavCompIdUser62 = 86,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER63
        /// </summary>
        MavCompIdUser63 = 87,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER64
        /// </summary>
        MavCompIdUser64 = 88,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER65
        /// </summary>
        MavCompIdUser65 = 89,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER66
        /// </summary>
        MavCompIdUser66 = 90,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER67
        /// </summary>
        MavCompIdUser67 = 91,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER68
        /// </summary>
        MavCompIdUser68 = 92,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER69
        /// </summary>
        MavCompIdUser69 = 93,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER70
        /// </summary>
        MavCompIdUser70 = 94,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER71
        /// </summary>
        MavCompIdUser71 = 95,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER72
        /// </summary>
        MavCompIdUser72 = 96,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER73
        /// </summary>
        MavCompIdUser73 = 97,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER74
        /// </summary>
        MavCompIdUser74 = 98,
        /// <summary>
        /// Id for a component on privately managed MAVLink network. Can be used for any purpose but may not be published by components outside of the private network.
        /// MAV_COMP_ID_USER75
        /// </summary>
        MavCompIdUser75 = 99,
        /// <summary>
        /// Camera #1.
        /// MAV_COMP_ID_CAMERA
        /// </summary>
        MavCompIdCamera = 100,
        /// <summary>
        /// Camera #2.
        /// MAV_COMP_ID_CAMERA2
        /// </summary>
        MavCompIdCamera2 = 101,
        /// <summary>
        /// Camera #3.
        /// MAV_COMP_ID_CAMERA3
        /// </summary>
        MavCompIdCamera3 = 102,
        /// <summary>
        /// Camera #4.
        /// MAV_COMP_ID_CAMERA4
        /// </summary>
        MavCompIdCamera4 = 103,
        /// <summary>
        /// Camera #5.
        /// MAV_COMP_ID_CAMERA5
        /// </summary>
        MavCompIdCamera5 = 104,
        /// <summary>
        /// Camera #6.
        /// MAV_COMP_ID_CAMERA6
        /// </summary>
        MavCompIdCamera6 = 105,
        /// <summary>
        /// Servo #1.
        /// MAV_COMP_ID_SERVO1
        /// </summary>
        MavCompIdServo1 = 140,
        /// <summary>
        /// Servo #2.
        /// MAV_COMP_ID_SERVO2
        /// </summary>
        MavCompIdServo2 = 141,
        /// <summary>
        /// Servo #3.
        /// MAV_COMP_ID_SERVO3
        /// </summary>
        MavCompIdServo3 = 142,
        /// <summary>
        /// Servo #4.
        /// MAV_COMP_ID_SERVO4
        /// </summary>
        MavCompIdServo4 = 143,
        /// <summary>
        /// Servo #5.
        /// MAV_COMP_ID_SERVO5
        /// </summary>
        MavCompIdServo5 = 144,
        /// <summary>
        /// Servo #6.
        /// MAV_COMP_ID_SERVO6
        /// </summary>
        MavCompIdServo6 = 145,
        /// <summary>
        /// Servo #7.
        /// MAV_COMP_ID_SERVO7
        /// </summary>
        MavCompIdServo7 = 146,
        /// <summary>
        /// Servo #8.
        /// MAV_COMP_ID_SERVO8
        /// </summary>
        MavCompIdServo8 = 147,
        /// <summary>
        /// Servo #9.
        /// MAV_COMP_ID_SERVO9
        /// </summary>
        MavCompIdServo9 = 148,
        /// <summary>
        /// Servo #10.
        /// MAV_COMP_ID_SERVO10
        /// </summary>
        MavCompIdServo10 = 149,
        /// <summary>
        /// Servo #11.
        /// MAV_COMP_ID_SERVO11
        /// </summary>
        MavCompIdServo11 = 150,
        /// <summary>
        /// Servo #12.
        /// MAV_COMP_ID_SERVO12
        /// </summary>
        MavCompIdServo12 = 151,
        /// <summary>
        /// Servo #13.
        /// MAV_COMP_ID_SERVO13
        /// </summary>
        MavCompIdServo13 = 152,
        /// <summary>
        /// Servo #14.
        /// MAV_COMP_ID_SERVO14
        /// </summary>
        MavCompIdServo14 = 153,
        /// <summary>
        /// Gimbal #1.
        /// MAV_COMP_ID_GIMBAL
        /// </summary>
        MavCompIdGimbal = 154,
        /// <summary>
        /// Logging component.
        /// MAV_COMP_ID_LOG
        /// </summary>
        MavCompIdLog = 155,
        /// <summary>
        /// Automatic Dependent Surveillance-Broadcast (ADS-B) component.
        /// MAV_COMP_ID_ADSB
        /// </summary>
        MavCompIdAdsb = 156,
        /// <summary>
        /// On Screen Display (OSD) devices for video links.
        /// MAV_COMP_ID_OSD
        /// </summary>
        MavCompIdOsd = 157,
        /// <summary>
        /// Generic autopilot peripheral component ID. Meant for devices that do not implement the parameter microservice.
        /// MAV_COMP_ID_PERIPHERAL
        /// </summary>
        MavCompIdPeripheral = 158,
        /// <summary>
        /// Gimbal ID for QX1.
        /// MAV_COMP_ID_QX1_GIMBAL
        /// </summary>
        MavCompIdQx1Gimbal = 159,
        /// <summary>
        /// FLARM collision alert component.
        /// MAV_COMP_ID_FLARM
        /// </summary>
        MavCompIdFlarm = 160,
        /// <summary>
        /// Parachute component.
        /// MAV_COMP_ID_PARACHUTE
        /// </summary>
        MavCompIdParachute = 161,
        /// <summary>
        /// Winch component.
        /// MAV_COMP_ID_WINCH
        /// </summary>
        MavCompIdWinch = 169,
        /// <summary>
        /// Gimbal #2.
        /// MAV_COMP_ID_GIMBAL2
        /// </summary>
        MavCompIdGimbal2 = 171,
        /// <summary>
        /// Gimbal #3.
        /// MAV_COMP_ID_GIMBAL3
        /// </summary>
        MavCompIdGimbal3 = 172,
        /// <summary>
        /// Gimbal #4
        /// MAV_COMP_ID_GIMBAL4
        /// </summary>
        MavCompIdGimbal4 = 173,
        /// <summary>
        /// Gimbal #5.
        /// MAV_COMP_ID_GIMBAL5
        /// </summary>
        MavCompIdGimbal5 = 174,
        /// <summary>
        /// Gimbal #6.
        /// MAV_COMP_ID_GIMBAL6
        /// </summary>
        MavCompIdGimbal6 = 175,
        /// <summary>
        /// Battery #1.
        /// MAV_COMP_ID_BATTERY
        /// </summary>
        MavCompIdBattery = 180,
        /// <summary>
        /// Battery #2.
        /// MAV_COMP_ID_BATTERY2
        /// </summary>
        MavCompIdBattery2 = 181,
        /// <summary>
        /// CAN over MAVLink client.
        /// MAV_COMP_ID_MAVCAN
        /// </summary>
        MavCompIdMavcan = 189,
        /// <summary>
        /// Component that can generate/supply a mission flight plan (e.g. GCS or developer API).
        /// MAV_COMP_ID_MISSIONPLANNER
        /// </summary>
        MavCompIdMissionplanner = 190,
        /// <summary>
        /// Component that lives on the onboard computer (companion computer) and has some generic functionalities, such as settings system parameters and monitoring the status of some processes that don't directly speak mavlink and so on.
        /// MAV_COMP_ID_ONBOARD_COMPUTER
        /// </summary>
        MavCompIdOnboardComputer = 191,
        /// <summary>
        /// Component that lives on the onboard computer (companion computer) and has some generic functionalities, such as settings system parameters and monitoring the status of some processes that don't directly speak mavlink and so on.
        /// MAV_COMP_ID_ONBOARD_COMPUTER2
        /// </summary>
        MavCompIdOnboardComputer2 = 192,
        /// <summary>
        /// Component that lives on the onboard computer (companion computer) and has some generic functionalities, such as settings system parameters and monitoring the status of some processes that don't directly speak mavlink and so on.
        /// MAV_COMP_ID_ONBOARD_COMPUTER3
        /// </summary>
        MavCompIdOnboardComputer3 = 193,
        /// <summary>
        /// Component that lives on the onboard computer (companion computer) and has some generic functionalities, such as settings system parameters and monitoring the status of some processes that don't directly speak mavlink and so on.
        /// MAV_COMP_ID_ONBOARD_COMPUTER4
        /// </summary>
        MavCompIdOnboardComputer4 = 194,
        /// <summary>
        /// Component that finds an optimal path between points based on a certain constraint (e.g. minimum snap, shortest path, cost, etc.).
        /// MAV_COMP_ID_PATHPLANNER
        /// </summary>
        MavCompIdPathplanner = 195,
        /// <summary>
        /// Component that plans a collision free path between two points.
        /// MAV_COMP_ID_OBSTACLE_AVOIDANCE
        /// </summary>
        MavCompIdObstacleAvoidance = 196,
        /// <summary>
        /// Component that provides position estimates using VIO techniques.
        /// MAV_COMP_ID_VISUAL_INERTIAL_ODOMETRY
        /// </summary>
        MavCompIdVisualInertialOdometry = 197,
        /// <summary>
        /// Component that manages pairing of vehicle and GCS.
        /// MAV_COMP_ID_PAIRING_MANAGER
        /// </summary>
        MavCompIdPairingManager = 198,
        /// <summary>
        /// Inertial Measurement Unit (IMU) #1.
        /// MAV_COMP_ID_IMU
        /// </summary>
        MavCompIdImu = 200,
        /// <summary>
        /// Inertial Measurement Unit (IMU) #2.
        /// MAV_COMP_ID_IMU_2
        /// </summary>
        MavCompIdImu2 = 201,
        /// <summary>
        /// Inertial Measurement Unit (IMU) #3.
        /// MAV_COMP_ID_IMU_3
        /// </summary>
        MavCompIdImu3 = 202,
        /// <summary>
        /// GPS #1.
        /// MAV_COMP_ID_GPS
        /// </summary>
        MavCompIdGps = 220,
        /// <summary>
        /// GPS #2.
        /// MAV_COMP_ID_GPS2
        /// </summary>
        MavCompIdGps2 = 221,
        /// <summary>
        /// Open Drone ID transmitter/receiver (Bluetooth/WiFi/Internet).
        /// MAV_COMP_ID_ODID_TXRX_1
        /// </summary>
        MavCompIdOdidTxrx1 = 236,
        /// <summary>
        /// Open Drone ID transmitter/receiver (Bluetooth/WiFi/Internet).
        /// MAV_COMP_ID_ODID_TXRX_2
        /// </summary>
        MavCompIdOdidTxrx2 = 237,
        /// <summary>
        /// Open Drone ID transmitter/receiver (Bluetooth/WiFi/Internet).
        /// MAV_COMP_ID_ODID_TXRX_3
        /// </summary>
        MavCompIdOdidTxrx3 = 238,
        /// <summary>
        /// Component to bridge MAVLink to UDP (i.e. from a UART).
        /// MAV_COMP_ID_UDP_BRIDGE
        /// </summary>
        MavCompIdUdpBridge = 240,
        /// <summary>
        /// Component to bridge to UART (i.e. from UDP).
        /// MAV_COMP_ID_UART_BRIDGE
        /// </summary>
        MavCompIdUartBridge = 241,
        /// <summary>
        /// Component handling TUNNEL messages (e.g. vendor specific GUI of a component).
        /// MAV_COMP_ID_TUNNEL_NODE
        /// </summary>
        MavCompIdTunnelNode = 242,
        /// <summary>
        /// Deprecated, don't use. Component for handling system messages (e.g. to ARM, takeoff, etc.).
        /// MAV_COMP_ID_SYSTEM_CONTROL
        /// </summary>
        MavCompIdSystemControl = 250,
    }
    public static class MavComponentHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(25);
            yield return converter(26);
            yield return converter(27);
            yield return converter(28);
            yield return converter(29);
            yield return converter(30);
            yield return converter(31);
            yield return converter(32);
            yield return converter(33);
            yield return converter(34);
            yield return converter(35);
            yield return converter(36);
            yield return converter(37);
            yield return converter(38);
            yield return converter(39);
            yield return converter(40);
            yield return converter(41);
            yield return converter(42);
            yield return converter(43);
            yield return converter(44);
            yield return converter(45);
            yield return converter(46);
            yield return converter(47);
            yield return converter(48);
            yield return converter(49);
            yield return converter(50);
            yield return converter(51);
            yield return converter(52);
            yield return converter(53);
            yield return converter(54);
            yield return converter(55);
            yield return converter(56);
            yield return converter(57);
            yield return converter(58);
            yield return converter(59);
            yield return converter(60);
            yield return converter(61);
            yield return converter(62);
            yield return converter(63);
            yield return converter(64);
            yield return converter(65);
            yield return converter(66);
            yield return converter(67);
            yield return converter(68);
            yield return converter(69);
            yield return converter(70);
            yield return converter(71);
            yield return converter(72);
            yield return converter(73);
            yield return converter(74);
            yield return converter(75);
            yield return converter(76);
            yield return converter(77);
            yield return converter(78);
            yield return converter(79);
            yield return converter(80);
            yield return converter(81);
            yield return converter(82);
            yield return converter(83);
            yield return converter(84);
            yield return converter(85);
            yield return converter(86);
            yield return converter(87);
            yield return converter(88);
            yield return converter(89);
            yield return converter(90);
            yield return converter(91);
            yield return converter(92);
            yield return converter(93);
            yield return converter(94);
            yield return converter(95);
            yield return converter(96);
            yield return converter(97);
            yield return converter(98);
            yield return converter(99);
            yield return converter(100);
            yield return converter(101);
            yield return converter(102);
            yield return converter(103);
            yield return converter(104);
            yield return converter(105);
            yield return converter(140);
            yield return converter(141);
            yield return converter(142);
            yield return converter(143);
            yield return converter(144);
            yield return converter(145);
            yield return converter(146);
            yield return converter(147);
            yield return converter(148);
            yield return converter(149);
            yield return converter(150);
            yield return converter(151);
            yield return converter(152);
            yield return converter(153);
            yield return converter(154);
            yield return converter(155);
            yield return converter(156);
            yield return converter(157);
            yield return converter(158);
            yield return converter(159);
            yield return converter(160);
            yield return converter(161);
            yield return converter(169);
            yield return converter(171);
            yield return converter(172);
            yield return converter(173);
            yield return converter(174);
            yield return converter(175);
            yield return converter(180);
            yield return converter(181);
            yield return converter(189);
            yield return converter(190);
            yield return converter(191);
            yield return converter(192);
            yield return converter(193);
            yield return converter(194);
            yield return converter(195);
            yield return converter(196);
            yield return converter(197);
            yield return converter(198);
            yield return converter(200);
            yield return converter(201);
            yield return converter(202);
            yield return converter(220);
            yield return converter(221);
            yield return converter(236);
            yield return converter(237);
            yield return converter(238);
            yield return converter(240);
            yield return converter(241);
            yield return converter(242);
            yield return converter(250);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"MAV_COMP_ID_ALL");
            yield return new EnumValue<T>(converter(1),"MAV_COMP_ID_AUTOPILOT1");
            yield return new EnumValue<T>(converter(25),"MAV_COMP_ID_USER1");
            yield return new EnumValue<T>(converter(26),"MAV_COMP_ID_USER2");
            yield return new EnumValue<T>(converter(27),"MAV_COMP_ID_USER3");
            yield return new EnumValue<T>(converter(28),"MAV_COMP_ID_USER4");
            yield return new EnumValue<T>(converter(29),"MAV_COMP_ID_USER5");
            yield return new EnumValue<T>(converter(30),"MAV_COMP_ID_USER6");
            yield return new EnumValue<T>(converter(31),"MAV_COMP_ID_USER7");
            yield return new EnumValue<T>(converter(32),"MAV_COMP_ID_USER8");
            yield return new EnumValue<T>(converter(33),"MAV_COMP_ID_USER9");
            yield return new EnumValue<T>(converter(34),"MAV_COMP_ID_USER10");
            yield return new EnumValue<T>(converter(35),"MAV_COMP_ID_USER11");
            yield return new EnumValue<T>(converter(36),"MAV_COMP_ID_USER12");
            yield return new EnumValue<T>(converter(37),"MAV_COMP_ID_USER13");
            yield return new EnumValue<T>(converter(38),"MAV_COMP_ID_USER14");
            yield return new EnumValue<T>(converter(39),"MAV_COMP_ID_USER15");
            yield return new EnumValue<T>(converter(40),"MAV_COMP_ID_USER16");
            yield return new EnumValue<T>(converter(41),"MAV_COMP_ID_USER17");
            yield return new EnumValue<T>(converter(42),"MAV_COMP_ID_USER18");
            yield return new EnumValue<T>(converter(43),"MAV_COMP_ID_USER19");
            yield return new EnumValue<T>(converter(44),"MAV_COMP_ID_USER20");
            yield return new EnumValue<T>(converter(45),"MAV_COMP_ID_USER21");
            yield return new EnumValue<T>(converter(46),"MAV_COMP_ID_USER22");
            yield return new EnumValue<T>(converter(47),"MAV_COMP_ID_USER23");
            yield return new EnumValue<T>(converter(48),"MAV_COMP_ID_USER24");
            yield return new EnumValue<T>(converter(49),"MAV_COMP_ID_USER25");
            yield return new EnumValue<T>(converter(50),"MAV_COMP_ID_USER26");
            yield return new EnumValue<T>(converter(51),"MAV_COMP_ID_USER27");
            yield return new EnumValue<T>(converter(52),"MAV_COMP_ID_USER28");
            yield return new EnumValue<T>(converter(53),"MAV_COMP_ID_USER29");
            yield return new EnumValue<T>(converter(54),"MAV_COMP_ID_USER30");
            yield return new EnumValue<T>(converter(55),"MAV_COMP_ID_USER31");
            yield return new EnumValue<T>(converter(56),"MAV_COMP_ID_USER32");
            yield return new EnumValue<T>(converter(57),"MAV_COMP_ID_USER33");
            yield return new EnumValue<T>(converter(58),"MAV_COMP_ID_USER34");
            yield return new EnumValue<T>(converter(59),"MAV_COMP_ID_USER35");
            yield return new EnumValue<T>(converter(60),"MAV_COMP_ID_USER36");
            yield return new EnumValue<T>(converter(61),"MAV_COMP_ID_USER37");
            yield return new EnumValue<T>(converter(62),"MAV_COMP_ID_USER38");
            yield return new EnumValue<T>(converter(63),"MAV_COMP_ID_USER39");
            yield return new EnumValue<T>(converter(64),"MAV_COMP_ID_USER40");
            yield return new EnumValue<T>(converter(65),"MAV_COMP_ID_USER41");
            yield return new EnumValue<T>(converter(66),"MAV_COMP_ID_USER42");
            yield return new EnumValue<T>(converter(67),"MAV_COMP_ID_USER43");
            yield return new EnumValue<T>(converter(68),"MAV_COMP_ID_TELEMETRY_RADIO");
            yield return new EnumValue<T>(converter(69),"MAV_COMP_ID_USER45");
            yield return new EnumValue<T>(converter(70),"MAV_COMP_ID_USER46");
            yield return new EnumValue<T>(converter(71),"MAV_COMP_ID_USER47");
            yield return new EnumValue<T>(converter(72),"MAV_COMP_ID_USER48");
            yield return new EnumValue<T>(converter(73),"MAV_COMP_ID_USER49");
            yield return new EnumValue<T>(converter(74),"MAV_COMP_ID_USER50");
            yield return new EnumValue<T>(converter(75),"MAV_COMP_ID_USER51");
            yield return new EnumValue<T>(converter(76),"MAV_COMP_ID_USER52");
            yield return new EnumValue<T>(converter(77),"MAV_COMP_ID_USER53");
            yield return new EnumValue<T>(converter(78),"MAV_COMP_ID_USER54");
            yield return new EnumValue<T>(converter(79),"MAV_COMP_ID_USER55");
            yield return new EnumValue<T>(converter(80),"MAV_COMP_ID_USER56");
            yield return new EnumValue<T>(converter(81),"MAV_COMP_ID_USER57");
            yield return new EnumValue<T>(converter(82),"MAV_COMP_ID_USER58");
            yield return new EnumValue<T>(converter(83),"MAV_COMP_ID_USER59");
            yield return new EnumValue<T>(converter(84),"MAV_COMP_ID_USER60");
            yield return new EnumValue<T>(converter(85),"MAV_COMP_ID_USER61");
            yield return new EnumValue<T>(converter(86),"MAV_COMP_ID_USER62");
            yield return new EnumValue<T>(converter(87),"MAV_COMP_ID_USER63");
            yield return new EnumValue<T>(converter(88),"MAV_COMP_ID_USER64");
            yield return new EnumValue<T>(converter(89),"MAV_COMP_ID_USER65");
            yield return new EnumValue<T>(converter(90),"MAV_COMP_ID_USER66");
            yield return new EnumValue<T>(converter(91),"MAV_COMP_ID_USER67");
            yield return new EnumValue<T>(converter(92),"MAV_COMP_ID_USER68");
            yield return new EnumValue<T>(converter(93),"MAV_COMP_ID_USER69");
            yield return new EnumValue<T>(converter(94),"MAV_COMP_ID_USER70");
            yield return new EnumValue<T>(converter(95),"MAV_COMP_ID_USER71");
            yield return new EnumValue<T>(converter(96),"MAV_COMP_ID_USER72");
            yield return new EnumValue<T>(converter(97),"MAV_COMP_ID_USER73");
            yield return new EnumValue<T>(converter(98),"MAV_COMP_ID_USER74");
            yield return new EnumValue<T>(converter(99),"MAV_COMP_ID_USER75");
            yield return new EnumValue<T>(converter(100),"MAV_COMP_ID_CAMERA");
            yield return new EnumValue<T>(converter(101),"MAV_COMP_ID_CAMERA2");
            yield return new EnumValue<T>(converter(102),"MAV_COMP_ID_CAMERA3");
            yield return new EnumValue<T>(converter(103),"MAV_COMP_ID_CAMERA4");
            yield return new EnumValue<T>(converter(104),"MAV_COMP_ID_CAMERA5");
            yield return new EnumValue<T>(converter(105),"MAV_COMP_ID_CAMERA6");
            yield return new EnumValue<T>(converter(140),"MAV_COMP_ID_SERVO1");
            yield return new EnumValue<T>(converter(141),"MAV_COMP_ID_SERVO2");
            yield return new EnumValue<T>(converter(142),"MAV_COMP_ID_SERVO3");
            yield return new EnumValue<T>(converter(143),"MAV_COMP_ID_SERVO4");
            yield return new EnumValue<T>(converter(144),"MAV_COMP_ID_SERVO5");
            yield return new EnumValue<T>(converter(145),"MAV_COMP_ID_SERVO6");
            yield return new EnumValue<T>(converter(146),"MAV_COMP_ID_SERVO7");
            yield return new EnumValue<T>(converter(147),"MAV_COMP_ID_SERVO8");
            yield return new EnumValue<T>(converter(148),"MAV_COMP_ID_SERVO9");
            yield return new EnumValue<T>(converter(149),"MAV_COMP_ID_SERVO10");
            yield return new EnumValue<T>(converter(150),"MAV_COMP_ID_SERVO11");
            yield return new EnumValue<T>(converter(151),"MAV_COMP_ID_SERVO12");
            yield return new EnumValue<T>(converter(152),"MAV_COMP_ID_SERVO13");
            yield return new EnumValue<T>(converter(153),"MAV_COMP_ID_SERVO14");
            yield return new EnumValue<T>(converter(154),"MAV_COMP_ID_GIMBAL");
            yield return new EnumValue<T>(converter(155),"MAV_COMP_ID_LOG");
            yield return new EnumValue<T>(converter(156),"MAV_COMP_ID_ADSB");
            yield return new EnumValue<T>(converter(157),"MAV_COMP_ID_OSD");
            yield return new EnumValue<T>(converter(158),"MAV_COMP_ID_PERIPHERAL");
            yield return new EnumValue<T>(converter(159),"MAV_COMP_ID_QX1_GIMBAL");
            yield return new EnumValue<T>(converter(160),"MAV_COMP_ID_FLARM");
            yield return new EnumValue<T>(converter(161),"MAV_COMP_ID_PARACHUTE");
            yield return new EnumValue<T>(converter(169),"MAV_COMP_ID_WINCH");
            yield return new EnumValue<T>(converter(171),"MAV_COMP_ID_GIMBAL2");
            yield return new EnumValue<T>(converter(172),"MAV_COMP_ID_GIMBAL3");
            yield return new EnumValue<T>(converter(173),"MAV_COMP_ID_GIMBAL4");
            yield return new EnumValue<T>(converter(174),"MAV_COMP_ID_GIMBAL5");
            yield return new EnumValue<T>(converter(175),"MAV_COMP_ID_GIMBAL6");
            yield return new EnumValue<T>(converter(180),"MAV_COMP_ID_BATTERY");
            yield return new EnumValue<T>(converter(181),"MAV_COMP_ID_BATTERY2");
            yield return new EnumValue<T>(converter(189),"MAV_COMP_ID_MAVCAN");
            yield return new EnumValue<T>(converter(190),"MAV_COMP_ID_MISSIONPLANNER");
            yield return new EnumValue<T>(converter(191),"MAV_COMP_ID_ONBOARD_COMPUTER");
            yield return new EnumValue<T>(converter(192),"MAV_COMP_ID_ONBOARD_COMPUTER2");
            yield return new EnumValue<T>(converter(193),"MAV_COMP_ID_ONBOARD_COMPUTER3");
            yield return new EnumValue<T>(converter(194),"MAV_COMP_ID_ONBOARD_COMPUTER4");
            yield return new EnumValue<T>(converter(195),"MAV_COMP_ID_PATHPLANNER");
            yield return new EnumValue<T>(converter(196),"MAV_COMP_ID_OBSTACLE_AVOIDANCE");
            yield return new EnumValue<T>(converter(197),"MAV_COMP_ID_VISUAL_INERTIAL_ODOMETRY");
            yield return new EnumValue<T>(converter(198),"MAV_COMP_ID_PAIRING_MANAGER");
            yield return new EnumValue<T>(converter(200),"MAV_COMP_ID_IMU");
            yield return new EnumValue<T>(converter(201),"MAV_COMP_ID_IMU_2");
            yield return new EnumValue<T>(converter(202),"MAV_COMP_ID_IMU_3");
            yield return new EnumValue<T>(converter(220),"MAV_COMP_ID_GPS");
            yield return new EnumValue<T>(converter(221),"MAV_COMP_ID_GPS2");
            yield return new EnumValue<T>(converter(236),"MAV_COMP_ID_ODID_TXRX_1");
            yield return new EnumValue<T>(converter(237),"MAV_COMP_ID_ODID_TXRX_2");
            yield return new EnumValue<T>(converter(238),"MAV_COMP_ID_ODID_TXRX_3");
            yield return new EnumValue<T>(converter(240),"MAV_COMP_ID_UDP_BRIDGE");
            yield return new EnumValue<T>(converter(241),"MAV_COMP_ID_UART_BRIDGE");
            yield return new EnumValue<T>(converter(242),"MAV_COMP_ID_TUNNEL_NODE");
            yield return new EnumValue<T>(converter(250),"MAV_COMP_ID_SYSTEM_CONTROL");
        }
    }

#endregion

#region Messages

    /// <summary>
    /// The heartbeat message shows that a system or component is present and responding. The type and autopilot fields (along with the message component id), allow the receiving system to treat further messages from this system appropriately (e.g. by laying out the user interface based on the autopilot). This microservice is documented at https://mavlink.io/en/services/heartbeat.html
    ///  HEARTBEAT
    /// </summary>
    public class HeartbeatPacket : MavlinkV2Message<HeartbeatPayload>
    {
        public const int MessageId = 0;
        
        public const byte CrcExtra = 50;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override HeartbeatPayload Payload { get; } = new();

        public override string Name => "HEARTBEAT";

    }

    /// <summary>
    ///  HEARTBEAT
    /// </summary>
    public class HeartbeatPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 9; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 9; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // uint32_t custom_mode
            + 1 // uint8_t type
            + 1 // uint8_t autopilot
            + 1 // uint8_t base_mode
            + 1 // uint8_t system_status
            +1 // uint8_t mavlink_version
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            CustomMode = BinSerialize.ReadUInt(ref buffer);
            Type = (MavType)BinSerialize.ReadByte(ref buffer);
            Autopilot = (MavAutopilot)BinSerialize.ReadByte(ref buffer);
            BaseMode = (MavModeFlag)BinSerialize.ReadByte(ref buffer);
            SystemStatus = (MavState)BinSerialize.ReadByte(ref buffer);
            MavlinkVersion = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,CustomMode);
            BinSerialize.WriteByte(ref buffer,(byte)Type);
            BinSerialize.WriteByte(ref buffer,(byte)Autopilot);
            BinSerialize.WriteByte(ref buffer,(byte)BaseMode);
            BinSerialize.WriteByte(ref buffer,(byte)SystemStatus);
            BinSerialize.WriteByte(ref buffer,(byte)MavlinkVersion);
            /* PayloadByteSize = 9 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt32Type.Accept(visitor,CustomModeField, ref _customMode);    
            var tmpType = (byte)Type;
            UInt8Type.Accept(visitor,TypeField, ref tmpType);
            Type = (MavType)tmpType;
            var tmpAutopilot = (byte)Autopilot;
            UInt8Type.Accept(visitor,AutopilotField, ref tmpAutopilot);
            Autopilot = (MavAutopilot)tmpAutopilot;
            var tmpBaseMode = (byte)BaseMode;
            UInt8Type.Accept(visitor,BaseModeField, ref tmpBaseMode);
            BaseMode = (MavModeFlag)tmpBaseMode;
            var tmpSystemStatus = (byte)SystemStatus;
            UInt8Type.Accept(visitor,SystemStatusField, ref tmpSystemStatus);
            SystemStatus = (MavState)tmpSystemStatus;
            UInt8Type.Accept(visitor,MavlinkVersionField, ref _mavlinkVersion);    

        }

        /// <summary>
        /// A bitfield for use for autopilot-specific flags
        /// OriginName: custom_mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CustomModeField = new Field.Builder()
            .Name(nameof(CustomMode))
            .Title("custom_mode")
            .Description("A bitfield for use for autopilot-specific flags")

            .DataType(UInt32Type.Default)
        .Build();
        private uint _customMode;
        public uint CustomMode { get => _customMode; set => _customMode = value; }
        /// <summary>
        /// Vehicle or component type. For a flight controller component the vehicle type (quadrotor, helicopter, etc.). For other components the component type (e.g. camera, gimbal, etc.). This should be used in preference to component id for identifying the component type.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        public static readonly Field TypeField = new Field.Builder()
            .Name(nameof(Type))
            .Title("type")
            .Description("Vehicle or component type. For a flight controller component the vehicle type (quadrotor, helicopter, etc.). For other components the component type (e.g. camera, gimbal, etc.). This should be used in preference to component id for identifying the component type.")
            .DataType(new UInt8Type(MavTypeHelper.GetValues(x=>(byte)x).Min(),MavTypeHelper.GetValues(x=>(byte)x).Max()))
            .Enum(MavTypeHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private MavType _type;
        public MavType Type { get => _type; set => _type = value; } 
        /// <summary>
        /// Autopilot type / class. Use MAV_AUTOPILOT_INVALID for components that are not flight controllers.
        /// OriginName: autopilot, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AutopilotField = new Field.Builder()
            .Name(nameof(Autopilot))
            .Title("autopilot")
            .Description("Autopilot type / class. Use MAV_AUTOPILOT_INVALID for components that are not flight controllers.")
            .DataType(new UInt8Type(MavAutopilotHelper.GetValues(x=>(byte)x).Min(),MavAutopilotHelper.GetValues(x=>(byte)x).Max()))
            .Enum(MavAutopilotHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private MavAutopilot _autopilot;
        public MavAutopilot Autopilot { get => _autopilot; set => _autopilot = value; } 
        /// <summary>
        /// System mode bitmap.
        /// OriginName: base_mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field BaseModeField = new Field.Builder()
            .Name(nameof(BaseMode))
            .Title("bitmask")
            .Description("System mode bitmap.")
            .DataType(new UInt8Type(MavModeFlagHelper.GetValues(x=>(byte)x).Min(),MavModeFlagHelper.GetValues(x=>(byte)x).Max()))
            .Enum(MavModeFlagHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private MavModeFlag _baseMode;
        public MavModeFlag BaseMode { get => _baseMode; set => _baseMode = value; } 
        /// <summary>
        /// System status flag.
        /// OriginName: system_status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SystemStatusField = new Field.Builder()
            .Name(nameof(SystemStatus))
            .Title("system_status")
            .Description("System status flag.")
            .DataType(new UInt8Type(MavStateHelper.GetValues(x=>(byte)x).Min(),MavStateHelper.GetValues(x=>(byte)x).Max()))
            .Enum(MavStateHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private MavState _systemStatus;
        public MavState SystemStatus { get => _systemStatus; set => _systemStatus = value; } 
        /// <summary>
        /// MAVLink version, not writable by user, gets added by protocol because of magic data type: uint8_t_mavlink_version
        /// OriginName: mavlink_version, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MavlinkVersionField = new Field.Builder()
            .Name(nameof(MavlinkVersion))
            .Title("mavlink_version")
            .Description("MAVLink version, not writable by user, gets added by protocol because of magic data type: uint8_t_mavlink_version")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _mavlinkVersion;
        public byte MavlinkVersion { get => _mavlinkVersion; set => _mavlinkVersion = value; }
    }
    /// <summary>
    /// Version and capability of protocol version. This message can be requested with MAV_CMD_REQUEST_MESSAGE and is used as part of the handshaking to establish which MAVLink version should be used on the network. Every node should respond to a request for PROTOCOL_VERSION to enable the handshaking. Library implementers should consider adding this into the default decoding state machine to allow the protocol core to respond directly.
    ///  PROTOCOL_VERSION
    /// </summary>
    public class ProtocolVersionPacket : MavlinkV2Message<ProtocolVersionPayload>
    {
        public const int MessageId = 300;
        
        public const byte CrcExtra = 217;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override ProtocolVersionPayload Payload { get; } = new();

        public override string Name => "PROTOCOL_VERSION";

    }

    /// <summary>
    ///  PROTOCOL_VERSION
    /// </summary>
    public class ProtocolVersionPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 22; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 22; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +2 // uint16_t version
            +2 // uint16_t min_version
            +2 // uint16_t max_version
            +SpecVersionHash.Length // uint8_t[8] spec_version_hash
            +LibraryVersionHash.Length // uint8_t[8] library_version_hash
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Version = BinSerialize.ReadUShort(ref buffer);
            MinVersion = BinSerialize.ReadUShort(ref buffer);
            MaxVersion = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/8 - Math.Max(0,((/*PayloadByteSize*/22 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            
            for(var i=0;i<arraySize;i++)
            {
                SpecVersionHash[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 8;
            for(var i=0;i<arraySize;i++)
            {
                LibraryVersionHash[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,Version);
            BinSerialize.WriteUShort(ref buffer,MinVersion);
            BinSerialize.WriteUShort(ref buffer,MaxVersion);
            for(var i=0;i<SpecVersionHash.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)SpecVersionHash[i]);
            }
            for(var i=0;i<LibraryVersionHash.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)LibraryVersionHash[i]);
            }
            /* PayloadByteSize = 22 */;
        }

        public void Accept(IVisitor visitor)
        {
            UInt16Type.Accept(visitor,VersionField, ref _version);    
            UInt16Type.Accept(visitor,MinVersionField, ref _minVersion);    
            UInt16Type.Accept(visitor,MaxVersionField, ref _maxVersion);    
            ArrayType.Accept(visitor,SpecVersionHashField, 
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref SpecVersionHash[index]));    
            ArrayType.Accept(visitor,LibraryVersionHashField, 
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref LibraryVersionHash[index]));    

        }

        /// <summary>
        /// Currently active MAVLink version number * 100: v1.0 is 100, v2.0 is 200, etc.
        /// OriginName: version, Units: , IsExtended: false
        /// </summary>
        public static readonly Field VersionField = new Field.Builder()
            .Name(nameof(Version))
            .Title("version")
            .Description("Currently active MAVLink version number * 100: v1.0 is 100, v2.0 is 200, etc.")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _version;
        public ushort Version { get => _version; set => _version = value; }
        /// <summary>
        /// Minimum MAVLink version supported
        /// OriginName: min_version, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MinVersionField = new Field.Builder()
            .Name(nameof(MinVersion))
            .Title("min_version")
            .Description("Minimum MAVLink version supported")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _minVersion;
        public ushort MinVersion { get => _minVersion; set => _minVersion = value; }
        /// <summary>
        /// Maximum MAVLink version supported (set to the same value as version by default)
        /// OriginName: max_version, Units: , IsExtended: false
        /// </summary>
        public static readonly Field MaxVersionField = new Field.Builder()
            .Name(nameof(MaxVersion))
            .Title("max_version")
            .Description("Maximum MAVLink version supported (set to the same value as version by default)")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _maxVersion;
        public ushort MaxVersion { get => _maxVersion; set => _maxVersion = value; }
        /// <summary>
        /// The first 8 bytes (not characters printed in hex!) of the git hash.
        /// OriginName: spec_version_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SpecVersionHashField = new Field.Builder()
            .Name(nameof(SpecVersionHash))
            .Title("spec_version_hash")
            .Description("The first 8 bytes (not characters printed in hex!) of the git hash.")

            .DataType(new ArrayType(UInt8Type.Default,8))
        .Build();
        public const int SpecVersionHashMaxItemsCount = 8;
        public byte[] SpecVersionHash { get; } = new byte[8];
        [Obsolete("This method is deprecated. Use GetSpecVersionHashMaxItemsCount instead.")]
        public byte GetSpecVersionHashMaxItemsCount() => 8;
        /// <summary>
        /// The first 8 bytes (not characters printed in hex!) of the git hash.
        /// OriginName: library_version_hash, Units: , IsExtended: false
        /// </summary>
        public static readonly Field LibraryVersionHashField = new Field.Builder()
            .Name(nameof(LibraryVersionHash))
            .Title("library_version_hash")
            .Description("The first 8 bytes (not characters printed in hex!) of the git hash.")

            .DataType(new ArrayType(UInt8Type.Default,8))
        .Build();
        public const int LibraryVersionHashMaxItemsCount = 8;
        public byte[] LibraryVersionHash { get; } = new byte[8];
    }




        


#endregion


}

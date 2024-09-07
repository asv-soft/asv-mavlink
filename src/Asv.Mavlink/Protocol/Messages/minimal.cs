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

// This code was generate by tool Asv.Mavlink.Shell version 3.7.1+98c5c7a392002d9bb54507cd50df001a14c44120

using System;
using Asv.IO;

namespace Asv.Mavlink.V2.Minimal
{

    public static class MinimalHelper
    {
        public static void RegisterMinimalDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new HeartbeatPacket());
            src.Register(()=>new ProtocolVersionPacket());
        }
    }

#region Enums

    /// <summary>
    /// Micro air vehicle / autopilot classes. This identifies the individual model.
    ///  MAV_AUTOPILOT
    /// </summary>
    public enum MavAutopilot:uint
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

    /// <summary>
    /// MAVLINK component type reported in HEARTBEAT message. Flight controllers must report the type of the vehicle on which they are mounted (e.g. MAV_TYPE_OCTOROTOR). All other components must report a value appropriate for their type (e.g. a camera must use MAV_TYPE_CAMERA).
    ///  MAV_TYPE
    /// </summary>
    public enum MavType:uint
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
        /// Log
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

    /// <summary>
    /// These flags encode the MAV mode.
    ///  MAV_MODE_FLAG
    /// </summary>
    public enum MavModeFlag:uint
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

    /// <summary>
    /// These values encode the bit positions of the decode position. These values can be used to read the value of a flag bit by combining the base_mode variable with AND with the flag position value. The result will be either 0 or 1, depending on if the flag is set or not.
    ///  MAV_MODE_FLAG_DECODE_POSITION
    /// </summary>
    public enum MavModeFlagDecodePosition:uint
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

    /// <summary>
    ///  MAV_STATE
    /// </summary>
    public enum MavState:uint
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

    /// <summary>
    /// Component ids (values) for the different types and instances of onboard hardware/software that might make up a MAVLink system (autopilot, cameras, servos, GPS systems, avoidance systems etc.).
    ///       Components must use the appropriate ID in their source address when sending messages. Components can also use IDs to determine if they are the intended recipient of an incoming message. The MAV_COMP_ID_ALL value is used to indicate messages that must be processed by all components.
    ///       When creating new entries, components that can have multiple instances (e.g. cameras, servos etc.) should be allocated sequential values. An appropriate number of values should be left free after these components to allow the number of instances to be expanded.
    ///  MAV_COMPONENT
    /// </summary>
    public enum MavComponent:uint
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


#endregion

#region Messages

    /// <summary>
    /// The heartbeat message shows that a system or component is present and responding. The type and autopilot fields (along with the message component id), allow the receiving system to treat further messages from this system appropriately (e.g. by laying out the user interface based on the autopilot). This microservice is documented at https://mavlink.io/en/services/heartbeat.html
    ///  HEARTBEAT
    /// </summary>
    public class HeartbeatPacket: PacketV2<HeartbeatPayload>
    {
	    public const int PacketMessageId = 0;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 50;
        public override bool WrapToV2Extension => false;

        public override HeartbeatPayload Payload { get; } = new HeartbeatPayload();

        public override string Name => "HEARTBEAT";
    }

    /// <summary>
    ///  HEARTBEAT
    /// </summary>
    public class HeartbeatPayload : IPayload
    {
        public byte GetMaxByteSize() => 9; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 9; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //CustomMode
            sum+= 1; // Type
            sum+= 1; // Autopilot
            sum+= 1; // BaseMode
            sum+= 1; // SystemStatus
            sum+=1; //MavlinkVersion
            return (byte)sum;
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
        
        



        /// <summary>
        /// A bitfield for use for autopilot-specific flags
        /// OriginName: custom_mode, Units: , IsExtended: false
        /// </summary>
        public uint CustomMode { get; set; }
        /// <summary>
        /// Vehicle or component type. For a flight controller component the vehicle type (quadrotor, helicopter, etc.). For other components the component type (e.g. camera, gimbal, etc.). This should be used in preference to component id for identifying the component type.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        public MavType Type { get; set; }
        /// <summary>
        /// Autopilot type / class. Use MAV_AUTOPILOT_INVALID for components that are not flight controllers.
        /// OriginName: autopilot, Units: , IsExtended: false
        /// </summary>
        public MavAutopilot Autopilot { get; set; }
        /// <summary>
        /// System mode bitmap.
        /// OriginName: base_mode, Units: , IsExtended: false
        /// </summary>
        public MavModeFlag BaseMode { get; set; }
        /// <summary>
        /// System status flag.
        /// OriginName: system_status, Units: , IsExtended: false
        /// </summary>
        public MavState SystemStatus { get; set; }
        /// <summary>
        /// MAVLink version, not writable by user, gets added by protocol because of magic data type: uint8_t_mavlink_version
        /// OriginName: mavlink_version, Units: , IsExtended: false
        /// </summary>
        public byte MavlinkVersion { get; set; }
    }
    /// <summary>
    /// Version and capability of protocol version. This message can be requested with MAV_CMD_REQUEST_MESSAGE and is used as part of the handshaking to establish which MAVLink version should be used on the network. Every node should respond to a request for PROTOCOL_VERSION to enable the handshaking. Library implementers should consider adding this into the default decoding state machine to allow the protocol core to respond directly.
    ///  PROTOCOL_VERSION
    /// </summary>
    public class ProtocolVersionPacket: PacketV2<ProtocolVersionPayload>
    {
	    public const int PacketMessageId = 300;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 217;
        public override bool WrapToV2Extension => false;

        public override ProtocolVersionPayload Payload { get; } = new ProtocolVersionPayload();

        public override string Name => "PROTOCOL_VERSION";
    }

    /// <summary>
    ///  PROTOCOL_VERSION
    /// </summary>
    public class ProtocolVersionPayload : IPayload
    {
        public byte GetMaxByteSize() => 22; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 22; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=2; //Version
            sum+=2; //MinVersion
            sum+=2; //MaxVersion
            sum+=SpecVersionHash.Length; //SpecVersionHash
            sum+=LibraryVersionHash.Length; //LibraryVersionHash
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            Version = BinSerialize.ReadUShort(ref buffer);
            MinVersion = BinSerialize.ReadUShort(ref buffer);
            MaxVersion = BinSerialize.ReadUShort(ref buffer);
            arraySize = /*ArrayLength*/8 - Math.Max(0,((/*PayloadByteSize*/22 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            SpecVersionHash = new byte[arraySize];
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
        
        



        /// <summary>
        /// Currently active MAVLink version number * 100: v1.0 is 100, v2.0 is 200, etc.
        /// OriginName: version, Units: , IsExtended: false
        /// </summary>
        public ushort Version { get; set; }
        /// <summary>
        /// Minimum MAVLink version supported
        /// OriginName: min_version, Units: , IsExtended: false
        /// </summary>
        public ushort MinVersion { get; set; }
        /// <summary>
        /// Maximum MAVLink version supported (set to the same value as version by default)
        /// OriginName: max_version, Units: , IsExtended: false
        /// </summary>
        public ushort MaxVersion { get; set; }
        /// <summary>
        /// The first 8 bytes (not characters printed in hex!) of the git hash.
        /// OriginName: spec_version_hash, Units: , IsExtended: false
        /// </summary>
        public const int SpecVersionHashMaxItemsCount = 8;
        public byte[] SpecVersionHash { get; set; } = new byte[8];
        [Obsolete("This method is deprecated. Use GetSpecVersionHashMaxItemsCount instead.")]
        public byte GetSpecVersionHashMaxItemsCount() => 8;
        /// <summary>
        /// The first 8 bytes (not characters printed in hex!) of the git hash.
        /// OriginName: library_version_hash, Units: , IsExtended: false
        /// </summary>
        public const int LibraryVersionHashMaxItemsCount = 8;
        public byte[] LibraryVersionHash { get; } = new byte[8];
    }


#endregion


}

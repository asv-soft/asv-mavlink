using System;
using Asv.IO;
using Asv.Mavlink.V2.Minimal;
using IDisposable = System.IDisposable;

namespace Asv.Mavlink
{
    public interface IMavlinkDeviceInfo
    {
        /// <summary>
        /// ID of message sender system/aircraft
        /// </summary>
        int SystemId { get; }
        /// <summary>
        /// ID of the message sender component
        /// </summary>
        int ComponentId { get; }
        /// <summary>
        /// ComponentId | SystemId << 8 as ushort
        /// </summary>
        ushort FullId { get; }
        /// <summary>
        /// A bitfield for use for autopilot-specific flags
        /// OriginName: custom_mode, Units: , IsExtended: false
        /// </summary>
        uint CustomMode { get; }
        /// <summary>
        /// Type of the system (quadrotor, helicopter, etc.). Components use the same type as their associated system.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        MavType Type  { get; }
        /// <summary>
        /// Autopilot type / class.
        /// OriginName: autopilot, Units: , IsExtended: false
        /// </summary>
        MavAutopilot Autopilot { get; }
        /// <summary>
        /// System mode bitmap.
        /// OriginName: base_mode, Units: , IsExtended: false
        /// </summary>
        MavModeFlag BaseMode { get; }
        /// <summary>
        /// System status flag.
        /// OriginName: system_status, Units: , IsExtended: false
        /// </summary>
        MavState SystemStatus { get; }
        /// <summary>
        /// MAVLink version, not writable by user, gets added by protocol because of magic data type: uint8_t_mavlink_version
        /// OriginName: mavlink_version, Units: , IsExtended: false
        /// </summary>
        byte MavlinkVersion { get; }
    }

    

    public interface IGroundControlStation:IDisposable
    {
        GroundControlStationIdentity Identity { get; }
        IPortManager Ports { get; }
        IMavlinkV2Connection MavlinkV2 { get; }
        IObservable<IMavlinkDeviceInfo> OnFoundNewDevice { get; }
        IObservable<IMavlinkDeviceInfo> OnLostDevice { get; }
        IMavlinkDeviceInfo[] Devices { get; }
    }
}

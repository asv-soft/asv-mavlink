using System;
using System.Reactive;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink
{
    public interface IMavlinkDevice
    {
        /// <summary>
        /// ID of message sender system/aircraft
        /// </summary>
        byte SystemId { get; }
        /// <summary>
        /// ID of the message sender component
        /// </summary>
        byte ComponentId { get; }
        /// <summary>
        /// ComponentId | SystemId << 8 as ushort
        /// </summary>
        ushort FullId { get; }
        /// <summary>
        /// Type of the system (quadrotor, helicopter, etc.). Components use the same type as their associated system.
        /// OriginName: type, Units: , IsExtended: false
        /// </summary>
        MavType Type { get; }
        /// <summary>
        /// Autopilot type / class.
        /// OriginName: autopilot, Units: , IsExtended: false
        /// </summary>
        MavAutopilot Autopilot { get; }
        /// <summary>
        /// MAVLink version, not writable by user, gets added by protocol because of magic data type: uint8_t_mavlink_version
        /// OriginName: mavlink_version, Units: , IsExtended: false
        /// </summary>
        byte MavlinkVersion { get; }
        /// <summary>
        /// System mode bitmap.
        /// OriginName: base_mode, Units: , IsExtended: false
        /// </summary>
        IRxValue<MavModeFlag> BaseMode { get; }
        /// <summary>
        /// A bitfield for use for autopilot-specific flags
        /// OriginName: custom_mode, Units: , IsExtended: false
        /// </summary>
        IRxValue<uint> CustomMode { get; }
        /// <summary>
        /// System status flag.
        /// OriginName: system_status, Units: , IsExtended: false
        /// </summary>
        IRxValue<MavState> SystemStatus { get; }
        /// <summary>
        /// Ping sequence 
        /// </summary>
        IObservable<Unit> Ping { get; }
    }


    /// <summary>
    /// Device browser in mavlink network
    /// </summary>
    public interface IMavlinkDeviceBrowser
    {
        IObservable<IChangeSet<IMavlinkDevice,ushort>> Devices { get; }
        /// <summary>
        /// Current device timeout
        /// </summary>
        IRxEditableValue<TimeSpan> DeviceTimeout { get; }
    }



    
}
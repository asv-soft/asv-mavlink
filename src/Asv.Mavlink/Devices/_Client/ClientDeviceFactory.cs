using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public static class ClientDeviceFactory
{
    public static IClientDevice Create(HeartbeatPacket packet, ICoreServices core)
    {
        switch (packet.Payload.Autopilot)
        {
            case MavAutopilot.MavAutopilotGeneric:
            case MavAutopilot.MavAutopilotReserved:
            case MavAutopilot.MavAutopilotSlugs:
            case MavAutopilot.MavAutopilotArdupilotmega:
            case MavAutopilot.MavAutopilotOpenpilot:
            case MavAutopilot.MavAutopilotGenericWaypointsOnly:
            case MavAutopilot.MavAutopilotGenericWaypointsAndSimpleNavigationOnly:
            case MavAutopilot.MavAutopilotGenericMissionFull:
            case MavAutopilot.MavAutopilotInvalid:
            case MavAutopilot.MavAutopilotPpz:
            case MavAutopilot.MavAutopilotUdb:
            case MavAutopilot.MavAutopilotFp:
            case MavAutopilot.MavAutopilotPx4:
            case MavAutopilot.MavAutopilotSmaccmpilot:
            case MavAutopilot.MavAutopilotAutoquad:
            case MavAutopilot.MavAutopilotArmazila:
            case MavAutopilot.MavAutopilotAerob:
            case MavAutopilot.MavAutopilotAsluav:
            case MavAutopilot.MavAutopilotSmartap:
            case MavAutopilot.MavAutopilotAirrails:
            case MavAutopilot.MavAutopilotReflex:
            default:
                return new GenericDevice();
        }
    }
}

/// Represents the class of a device.
/// /
public enum DeviceClass
{
    Unknown,
    /// <summary>
    /// Represents a device class category of Plane.
    /// </summary>
    Plane,
    /// <summary>
    /// Represents the Copter device class.
    /// </summary>
    Copter,
    /// <summary>
    /// Represents a device of GBS RTK class.
    /// </summary>
    GbsRtk,
    /// <summary>
    /// Represents the class of a device.
    /// </summary>
    SdrPayload,
    /// <summary>
    /// Radio transmitter device class.
    /// </summary>
    Radio,
    /// <summary>
    /// Represents the device class for ADS-B devices.
    /// </summary>
    Adsb,
    /// <summary>
    /// Represents the device class for RF signal analyzer devices.
    /// </summary>
    Rfsa,
    /// <summary>
    /// Represents the device class for Radio Signal Generator and Analyzer devices.
    /// </summary>
    Rsga,
}
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class ArduPlaneClientDeviceProvider(VehicleClientDeviceConfig deviceConfig) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet)
    {
        return packet.Payload is
        {
            Type: MavType.MavTypeFixedWing, 
            Autopilot: MavAutopilot.MavAutopilotArdupilotmega
        };
    }

    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new ArduPlaneClientDevice(identity, deviceConfig, core);
    }
}
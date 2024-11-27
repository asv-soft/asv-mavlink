using Asv.Mavlink.Minimal;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class Px4PlaneClientDeviceProvider(VehicleClientDeviceConfig deviceConfig) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet)
    {
        return packet.Payload is
        {
            Type: MavType.MavTypeFixedWing,
            Autopilot: MavAutopilot.MavAutopilotPx4
        };
    }

    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new Px4PlaneClientDevice(identity, deviceConfig, core);
    }
}
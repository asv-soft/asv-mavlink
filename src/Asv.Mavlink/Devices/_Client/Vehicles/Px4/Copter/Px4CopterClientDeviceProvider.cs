using System;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class Px4CopterClientDeviceProvider(VehicleClientDeviceConfig deviceConfig) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet)
    {
        return packet.Payload is
        {
            Type: MavType.MavTypeQuadrotor or MavType.MavTypeTricopter or MavType.MavTypeHexarotor or MavType.MavTypeOctorotor,
            Autopilot: MavAutopilot.MavAutopilotPx4
        };
    }

    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new Px4VehicleClientDevice(identity, deviceConfig, core, DeviceClass.Copter);
    }
}
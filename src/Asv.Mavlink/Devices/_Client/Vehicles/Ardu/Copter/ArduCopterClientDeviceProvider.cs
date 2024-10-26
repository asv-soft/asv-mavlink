using System;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class ArduCopterClientDeviceProvider(VehicleClientDeviceConfig deviceConfig) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet)
    {
        return packet.Payload is
        {
            Type: MavType.MavTypeQuadrotor or MavType.MavTypeTricopter or MavType.MavTypeHexarotor or MavType.MavTypeOctorotor, 
            Autopilot: MavAutopilot.MavAutopilotArdupilotmega
        };
    }

    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new ArduCopterClientDeviceV2(identity, deviceConfig, core);
    }
}
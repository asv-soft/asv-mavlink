using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class Px4CopterClientDeviceFactory(MavlinkIdentity selfId, IPacketSequenceCalculator seq,VehicleClientDeviceConfig config)
    : MavlinkClientDeviceFactory<Px4VehicleClientDevice>(selfId,seq)
{
    public override int Order => ClientDeviceFactory.MinimumOrder;
    public override string DeviceClass => Vehicles.CopterDeviceClass;

    protected override Px4VehicleClientDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId, ImmutableArray<IClientDeviceExtender> extenders,
        IMavlinkContext context)
    {
        return new Px4VehicleClientDevice(clientDeviceId,config,extenders,context);
    }

    protected override bool CheckDevice(HeartbeatPacket msg)
    {
        return msg.Payload is
        {
            Type: MavType.MavTypeQuadrotor or MavType.MavTypeTricopter or MavType.MavTypeHexarotor or MavType.MavTypeOctorotor,
            Autopilot: MavAutopilot.MavAutopilotPx4
        };
    }

}
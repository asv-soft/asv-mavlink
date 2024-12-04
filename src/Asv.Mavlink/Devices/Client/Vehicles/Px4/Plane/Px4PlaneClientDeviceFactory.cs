using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class Px4PlaneClientDeviceFactory(MavlinkIdentity selfId, IPacketSequenceCalculator seq, VehicleClientDeviceConfig deviceConfig) 
    : MavlinkClientDeviceFactory<Px4PlaneClientDevice>(selfId,seq)
{
    public override int Order => ClientDeviceFactory.DefaultOrder;
    public override string DeviceClass => Vehicles.PlaneDeviceClass;

    protected override Px4PlaneClientDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId, ImmutableArray<IClientDeviceExtender> extenders,
        ICoreServices context)
    {
        return new Px4PlaneClientDevice(clientDeviceId, deviceConfig, extenders, context);
    }

    protected override bool CheckDevice(HeartbeatPacket msg)
    {
        return msg.Payload is
        {
            Type: MavType.MavTypeFixedWing,
            Autopilot: MavAutopilot.MavAutopilotPx4
        };
    }
}
using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;

namespace Asv.Mavlink;

public class ArduPlaneClientDeviceFactory(MavlinkIdentity selfId, IPacketSequenceCalculator seq,VehicleClientDeviceConfig config)
    : MavlinkClientDeviceFactory<ArduPlaneClientDevice>(selfId,seq)
{
    public override int Order => ClientDeviceFactory.MinimumOrder;
    public override string DeviceClass => Vehicles.PlaneDeviceClass;

    protected override ArduPlaneClientDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId, ImmutableArray<IClientDeviceExtender> extenders,
        IMavlinkContext context)
    {
        return new ArduPlaneClientDevice(clientDeviceId,config,extenders,context);
    }

    protected override bool CheckDevice(HeartbeatPacket msg)
    {
        return msg.Payload is
        {
            Type: MavType.MavTypeFixedWing, 
            Autopilot: MavAutopilot.MavAutopilotArdupilotmega
        };
    }

}
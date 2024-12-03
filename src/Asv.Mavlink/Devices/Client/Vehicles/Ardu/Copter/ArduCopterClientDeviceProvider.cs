using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class ArduCopterClientDeviceProvider(MavlinkIdentity selfId, IPacketSequenceCalculator seq, VehicleClientDeviceConfig config) 
    : MavlinkClientDeviceFactory<ArduCopterClientDeviceV2>(selfId,seq)
{
    public override int Order => ClientDeviceFactory.DefaultOrder;
    public override string DeviceClass => Vehicles.CopterDeviceClass;

    protected override ArduCopterClientDeviceV2 InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId, ImmutableArray<IClientDeviceExtender> extenders,
        ICoreServices context)
    {
        return new ArduCopterClientDeviceV2(clientDeviceId,config,extenders,context);
    }

    protected override bool CheckDevice(HeartbeatPacket msg)
    {
        return msg.Payload is
        {
            Type: MavType.MavTypeQuadrotor or MavType.MavTypeTricopter or MavType.MavTypeHexarotor or MavType.MavTypeOctorotor, 
            Autopilot: MavAutopilot.MavAutopilotArdupilotmega
        };
    }

}
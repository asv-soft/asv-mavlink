using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class GbsClientDeviceProvider(MavlinkIdentity selfId, IPacketSequenceCalculator seq, GbsClientDeviceConfig config) 
    : MavlinkClientDeviceFactory<GbsClientDevice>(selfId,seq)
{
    public override int Order => ClientDeviceFactory.DefaultOrder;
    public override string DeviceClass => GbsClientDevice.DeviceClass;

    protected override GbsClientDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId,
        ImmutableArray<IClientDeviceExtender> extenders, ICoreServices context)
    {
        return new GbsClientDevice(clientDeviceId,config, extenders, context);
    }

    protected override bool CheckDevice(HeartbeatPacket msg)
    {
        return msg.Payload.Type == (MavType)AsvGbs.MavType.MavTypeAsvGbs;
    }

}
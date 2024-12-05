using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class SdrClientDeviceFactory(MavlinkIdentity selfId, IPacketSequenceCalculator seq, SdrClientDeviceConfig config) 
    : MavlinkClientDeviceFactory<SdrClientDevice>(selfId,seq)
{
    public override int Order => ClientDeviceFactory.DefaultOrder;
    public override string DeviceClass => SdrClientDevice.DeviceClass;

    protected override SdrClientDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId, ImmutableArray<IClientDeviceExtender> extenders,
        IMavlinkContext context)
    {
        return new SdrClientDevice(clientDeviceId,config,extenders,context);
    }

    protected override bool CheckDevice(HeartbeatPacket msg)
    {
        return msg.Payload.Type == (MavType)AsvSdr.MavType.MavTypeAsvSdrPayload;
    }
   
}
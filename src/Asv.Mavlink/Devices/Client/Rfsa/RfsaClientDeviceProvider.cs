using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class RfsaClientDeviceProvider(MavlinkIdentity selfId, IPacketSequenceCalculator seq, RfsaClientDeviceConfig config) 
    : MavlinkClientDeviceFactory<RfsaClientDevice>(selfId,seq)
{
    public override int Order => ClientDeviceFactory.DefaultOrder;
    public override string DeviceClass => RfsaClientDevice.DeviceClass;

    protected override RfsaClientDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId, ImmutableArray<IClientDeviceExtender> extenders,
        ICoreServices context)
    {
        return new RfsaClientDevice(clientDeviceId,config,extenders,context);
    }

    protected override bool CheckDevice(HeartbeatPacket msg)
    {
        return msg.Payload.Type == (MavType)AsvRfsa.MavType.MavTypeAsvRfsa;
    }

}
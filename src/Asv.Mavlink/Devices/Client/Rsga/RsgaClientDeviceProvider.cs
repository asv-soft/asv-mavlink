using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class RsgaClientDeviceFactory(MavlinkIdentity selfId, IPacketSequenceCalculator seq,RsgaClientDeviceConfig config) 
    : MavlinkClientDeviceFactory<RsgaClientDevice>(selfId,seq)
{
    public override int Order => ClientDeviceFactory.MinimumOrder;
    public override string DeviceClass => RsgaClientDevice.DeviceClass;
    protected override RsgaClientDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId,
        ImmutableArray<IClientDeviceExtender> extenders, ICoreServices context)
    {
        return new RsgaClientDevice(clientDeviceId,config,extenders,context);
    }

    protected override bool CheckDevice(HeartbeatPacket msg)
    {
        return msg.Payload.Type == (MavType)AsvRsga.MavType.MavTypeAsvRsga;
    }
}
using System.Collections.Generic;
using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class AdsbClientDeviceProvider(MavlinkIdentity selfId, IPacketSequenceCalculator seq, AdsbClientDeviceConfig config,IEnumerable<ParamDescription> paramDescriptions) 
    : MavlinkClientDeviceFactory<AdsbClientDevice>(selfId,seq)
{
    public override int Order => ClientDeviceFactory.DefaultOrder;
    public override string DeviceClass => AdsbClientDevice.DeviceClass;

    protected override AdsbClientDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId,
        ImmutableArray<IClientDeviceExtender> extenders, ICoreServices context)
    {
        return new AdsbClientDevice(clientDeviceId, config, extenders, context,paramDescriptions);
    }

    protected override bool CheckDevice(HeartbeatPacket msg)
    {
        return msg.Payload.Type == MavType.MavTypeAdsb;
    }
}
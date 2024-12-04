using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class GenericClientDeviceFactory(MavlinkIdentity selfId, IPacketSequenceCalculator seq, GenericDeviceConfig config) 
    : MavlinkClientDeviceFactory<GenericDevice>(selfId,seq)
{
    public override int Order => ClientDeviceFactory.MinimumOrder;
    public override string DeviceClass => GenericDevice.DeviceClass;

    protected override GenericDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId,
        ImmutableArray<IClientDeviceExtender> extenders, ICoreServices context)
    {
        return new GenericDevice(clientDeviceId,config,extenders,context);
    }

    protected override bool CheckDevice(HeartbeatPacket msg) => true;

    public bool CanCreateDevice(HeartbeatPacket packet) => true;
   
}
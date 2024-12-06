using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class RadioClientDeviceFactory(MavlinkIdentity selfId, IPacketSequenceCalculator seq, RadioClientDeviceConfig config) 
    : MavlinkClientDeviceFactory<RadioClientDevice>(selfId, seq)
{
    public override int Order => ClientDeviceFactory.DefaultOrder;
    public override string DeviceClass => RadioClientDevice.DeviceClass;

    protected override RadioClientDevice InternalCreateDevice(HeartbeatPacket msg, MavlinkClientDeviceId clientDeviceId, ImmutableArray<IClientDeviceExtender> extenders,
        IMavlinkContext context)
    {
        return new RadioClientDevice(clientDeviceId, config, extenders, context);
    }

    protected override bool CheckDevice(HeartbeatPacket msg) => msg.Payload.Type == (MavType)AsvRadio.MavType.MavTypeAsvRadio;

    public bool CanCreateDevice(HeartbeatPacket packet) => packet.Payload.Type == (MavType)Mavlink.AsvRadio.MavType.MavTypeAsvRadio;
   
}
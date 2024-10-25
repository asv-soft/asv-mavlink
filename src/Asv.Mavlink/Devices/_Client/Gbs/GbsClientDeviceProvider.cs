using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class GbsClientDeviceProvider(GbsClientDeviceConfig config) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet)
    {
        return packet.Payload.Type == (MavType)Mavlink.V2.AsvGbs.MavType.MavTypeAsvGbs; 
    }

    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new GbsClientDevice(identity,config,core);
    }
}
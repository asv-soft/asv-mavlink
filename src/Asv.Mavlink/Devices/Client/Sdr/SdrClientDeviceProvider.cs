using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class SdrClientDeviceProvider(SdrClientDeviceConfig config) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet) => packet.Payload.Type == (MavType)Mavlink.V2.AsvSdr.MavType.MavTypeAsvSdrPayload;
    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new SdrClientDevice(identity,config,core);
    }
}
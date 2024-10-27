using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class RfsaClientDeviceProvider(RfsaClientDeviceConfig config) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet) => packet.Payload.Type == (MavType)Mavlink.V2.AsvRfsa.MavType.MavTypeAsvRfsa;
    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new RfsaClientDevice(identity,config,core);
    }
}
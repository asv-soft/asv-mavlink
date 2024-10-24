using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class RfsaClientDeviceProvider(RfsaClientDeviceConfig config) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet) => true;
    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new RfsaClientDevice(identity,config,core);
    }
}
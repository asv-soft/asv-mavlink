using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class GenericDeviceProvider(GenericDeviceConfig config) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.MinimumOrder;
    public bool CanCreateDevice(HeartbeatPacket packet) => true;
    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new GenericDevice(identity,config,core);
    }
}
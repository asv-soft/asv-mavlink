using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class AdsbClientDeviceProvider(AdsbClientDeviceConfig config) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet) => packet.Payload.Type == MavType.MavTypeAdsb;
    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkIdentity self, ICoreServices core)
    {
        return new AdsbClientDevice(new MavlinkClientIdentity(self.SystemId,self.ComponentId,packet.SystemId,packet.ComponentId),config,core);
    }
}
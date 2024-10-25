using System.Collections.Generic;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class AdsbClientDeviceProvider(AdsbClientDeviceConfig config,IEnumerable<ParamDescription> paramDescriptions) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet) => packet.Payload.Type == MavType.MavTypeAdsb;
    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new AdsbClientDevice(identity,config,core, paramDescriptions);
    }

}
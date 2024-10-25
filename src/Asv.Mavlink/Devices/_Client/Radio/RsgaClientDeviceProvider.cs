using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class RadioClientDeviceProvider(RadioClientDeviceConfig config) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet) => packet.Payload.Type == (MavType)Mavlink.V2.AsvRadio.MavType.MavTypeAsvRadio;
    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new RadioClientDevice(identity,config,core);
    }
}
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class RsgaClientDeviceProvider(RsgaClientDeviceConfig config) : IClientDeviceProvider
{
    public int Order => ClientDeviceFactory.DefaultOrder;
    public bool CanCreateDevice(HeartbeatPacket packet) => packet.Payload.Type == (MavType)Mavlink.AsvRsga.MavType.MavTypeAsvRsga;
    public IClientDevice CreateDevice(HeartbeatPacket packet, MavlinkClientIdentity identity, ICoreServices core)
    {
        return new RsgaClientDevice(identity,config,core);
    }
}
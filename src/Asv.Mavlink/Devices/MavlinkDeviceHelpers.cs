using Asv.Cfg;
using Asv.IO;

namespace Asv.Mavlink;


public static class MavlinkDeviceHelpers
{
    public static void RegisterDefaultDevices(this IClientDeviceFactoryBuilder builder, MavlinkIdentity selfId, IPacketSequenceCalculator seq, IConfiguration config, IProtocolMessageFactory<MavlinkMessage, int> msgFactory)
    {
        builder.Register(new Px4PlaneClientDeviceFactory(selfId,seq, config.Get<VehicleClientDeviceConfig>(), msgFactory));
        builder.Register(new Px4CopterClientDeviceFactory(selfId,seq, config.Get<VehicleClientDeviceConfig>(), msgFactory));
        
        builder.Register(new ArduPlaneClientDeviceFactory(selfId,seq, config.Get<VehicleClientDeviceConfig>(), msgFactory));
        builder.Register(new ArduCopterClientDeviceFactory(selfId,seq, config.Get<VehicleClientDeviceConfig>(), msgFactory));
        
        builder.Register(new SdrClientDeviceFactory(selfId,seq, config.Get<SdrClientDeviceConfig>(), msgFactory));
        builder.Register(new RsgaClientDeviceFactory(selfId,seq, config.Get<RsgaClientDeviceConfig>(), msgFactory));
        builder.Register(new RfsaClientDeviceFactory(selfId,seq, config.Get<RfsaClientDeviceConfig>(), msgFactory));
        builder.Register(new RadioClientDeviceFactory(selfId,seq, config.Get<RadioClientDeviceConfig>(), msgFactory));
        builder.Register(new GenericClientDeviceFactory(selfId,seq, config.Get<GenericDeviceConfig>(), msgFactory));
        builder.Register(new GbsClientDeviceFactory(selfId,seq, config.Get<GbsClientDeviceConfig>(), msgFactory));
        builder.Register(new AdsbClientDeviceFactory(selfId,seq, config.Get<AdsbClientDeviceConfig>(), msgFactory));
    }
    
    public static IAdsbVehicleServer CreateAdsbServer(MavlinkIdentity identity, 
        IConfiguration config, IMavlinkContext context)
    {
        return new AdsbVehicleServer(identity, context);
    }

    public static IHeartbeatServer CreateHeartbeatServer(
        MavlinkIdentity identity, 
        IConfiguration config, IMavlinkContext context)
    {
        return new HeartbeatServer(identity, config.Get<MavlinkHeartbeatServerConfig>(), context);
    }
    
    
}
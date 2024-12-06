using Asv.Cfg;
using Asv.IO;

namespace Asv.Mavlink;


public static class MavlinkDeviceHelpers
{
    public static void RegisterDefaultDevices(this IClientDeviceFactoryBuilder builder, MavlinkIdentity selfId, IPacketSequenceCalculator seq, IConfiguration config)
    {
        builder.Register(new Px4PlaneClientDeviceFactory(selfId,seq, config.Get<VehicleClientDeviceConfig>()));
        builder.Register(new Px4CopterClientDeviceFactory(selfId,seq, config.Get<VehicleClientDeviceConfig>()));
        
        builder.Register(new ArduPlaneClientDeviceFactory(selfId,seq, config.Get<VehicleClientDeviceConfig>()));
        builder.Register(new ArduCopterClientDeviceFactory(selfId,seq, config.Get<VehicleClientDeviceConfig>()));
        
        builder.Register(new SdrClientDeviceFactory(selfId,seq, config.Get<SdrClientDeviceConfig>()));
        builder.Register(new RsgaClientDeviceFactory(selfId,seq, config.Get<RsgaClientDeviceConfig>()));
        builder.Register(new RfsaClientDeviceFactory(selfId,seq, config.Get<RfsaClientDeviceConfig>()));
        builder.Register(new RadioClientDeviceFactory(selfId,seq, config.Get<RadioClientDeviceConfig>()));
        builder.Register(new GenericClientDeviceFactory(selfId,seq, config.Get<GenericDeviceConfig>()));
        builder.Register(new GbsClientDeviceFactory(selfId,seq, config.Get<GbsClientDeviceConfig>()));
        builder.Register(new AdsbClientDeviceFactory(selfId,seq, config.Get<AdsbClientDeviceConfig>()));
    }
}
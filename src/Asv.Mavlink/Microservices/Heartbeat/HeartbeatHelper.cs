using System.IO.Abstractions;
using Asv.Cfg;

namespace Asv.Mavlink;

public static class HeartbeatHelper
{
    public const string MicroserviceName = "HEARTBEAT";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterHeartbeat(this IServerDeviceBuilder builder)
    {
        builder.Register<IHeartbeatServer>((identity, context,config) => new HeartbeatServer(identity, config.Get<MavlinkHeartbeatServerConfig>(), context));
        return builder;
    }
   
    public static IServerDeviceBuilder RegisterHeartbeat(this IServerDeviceBuilder builder, MavlinkHeartbeatServerConfig config)
    {
        builder
            .Register<IHeartbeatServer>((identity, context,_) =>  new HeartbeatServer(identity,config,context));
        return builder;
    }

    public static IHeartbeatServer GetHeartbeat(this IServerDevice factory) 
        => factory.Get<IHeartbeatServer>();

    #endregion
}
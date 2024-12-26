using Asv.Cfg;
using Asv.Mavlink.Common;

namespace Asv.Mavlink;

public static class AsvGbsHelper
{
    public const string MicroserviceName = "GBS";
    public const string MicroserviceExName = $"{MicroserviceName}Ex";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterGbs(this IServerDeviceBuilder builder)
    {
        builder.Register<IAsvGbsServer>((identity, context,config) => new AsvGbsServer(identity,config.Get<AsvGbsServerConfig>(), context));
        return builder;
    }
    public static IServerDeviceBuilder RegisterGbs(this IServerDeviceBuilder builder, AsvGbsServerConfig config)
    {
        builder.Register<IAsvGbsServer>((identity, context,_) => new AsvGbsServer(identity,config, context));
        return builder;
    }

    public static IServerDeviceBuilder RegisterGbsEx(this IServerDeviceBuilder builder)
    {
        builder.Register<IAsvGbsServerEx, IAsvGbsServer, IHeartbeatServer, ICommandServerEx<CommandLongPacket>>(
            (_, _, _, @base, hb, cmd) => new AsvGbsExServer(@base, hb, cmd));
        return builder;
    }

    public static IAsvChartServer GetGbs(this IServerDevice factory)
    {
        return factory.Get<IAsvChartServer>();
    }
    public static IAsvGbsServerEx GetGbsEx(this IServerDevice factory)
    {
        return factory.Get<IAsvGbsServerEx>();
    }
    
    #endregion
}
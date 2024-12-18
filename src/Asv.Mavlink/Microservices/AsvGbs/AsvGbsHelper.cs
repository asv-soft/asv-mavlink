using Asv.Cfg;
using Asv.Mavlink.Common;

namespace Asv.Mavlink;

public static class AsvGbsHelper
{
    public const string MicroserviceName = "GBS";
    public const string MicroserviceExName = $"{MicroserviceName}Ex";
    
    #region ServerFactory

    public static IMavlinkServerMicroserviceBuilder RegisterGbs(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder.Register<IAsvGbsServer>((identity, context,config) => new AsvGbsServer(identity,config.Get<AsvGbsServerConfig>(), context));
        return builder;
    }
    public static IMavlinkServerMicroserviceBuilder RegisterGbs(this IMavlinkServerMicroserviceBuilder builder, AsvGbsServerConfig config)
    {
        builder.Register<IAsvGbsServer>((identity, context,_) => new AsvGbsServer(identity,config, context));
        return builder;
    }

    public static IMavlinkServerMicroserviceBuilder RegisterGbsEx(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder.Register<IAsvGbsServerEx, IAsvGbsServer, IHeartbeatServer, ICommandServerEx<CommandLongPacket>>(
            (_, _, _, @base, hb, cmd) => new AsvGbsExServer(@base, hb, cmd));
        return builder;
    }

    public static IAsvChartServer GetGbs(this IMavlinkServerMicroserviceFactory factory)
    {
        return factory.Get<IAsvChartServer>();
    }
    public static IAsvGbsServerEx GetGbsEx(this IMavlinkServerMicroserviceFactory factory)
    {
        return factory.Get<IAsvGbsServerEx>();
    }
    
    #endregion
}
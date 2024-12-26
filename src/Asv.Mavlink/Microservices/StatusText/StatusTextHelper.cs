using Asv.Cfg;

namespace Asv.Mavlink;

public static class StatusTextHelper
{
    public const string MicroserviceName = "STATUS";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterStatus(this IServerDeviceBuilder builder)
    {
        builder
            .Register<IStatusTextServer>((identity, context, config) => new StatusTextServer(identity, config.Get<StatusTextLoggerConfig>(), context));
        return builder;
    }
    
    public static IServerDeviceBuilder RegisterStatus(this IServerDeviceBuilder builder, StatusTextLoggerConfig config)
    {
        builder
            .Register<IStatusTextServer>((identity, context, _) => new StatusTextServer(identity, config, context));
        return builder;
    }
   

    public static IStatusTextServer GetStatus(this IServerDevice factory) 
        => factory.Get<IStatusTextServer>();
    
   

    #endregion
}
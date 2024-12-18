using Asv.Cfg;

namespace Asv.Mavlink;

public static class StatusTextHelper
{
    public const string MicroserviceName = "STATUS";
    
    #region ServerFactory

    public static IMavlinkServerMicroserviceBuilder RegisterStatus(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder
            .Register<IStatusTextServer>((identity, context, config) => new StatusTextServer(identity, config.Get<StatusTextLoggerConfig>(), context));
        return builder;
    }
    
    public static IMavlinkServerMicroserviceBuilder RegisterStatus(this IMavlinkServerMicroserviceBuilder builder, StatusTextLoggerConfig config)
    {
        builder
            .Register<IStatusTextServer>((identity, context, _) => new StatusTextServer(identity, config, context));
        return builder;
    }
   

    public static IStatusTextServer GetStatus(this IMavlinkServerMicroserviceFactory factory) 
        => factory.Get<IStatusTextServer>();
    
   

    #endregion
}
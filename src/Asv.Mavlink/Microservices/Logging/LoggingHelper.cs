namespace Asv.Mavlink;

public static class LoggingHelper
{
    public const string MicroserviceName = "LOG";
    
    #region ServerFactory

    public static IMavlinkServerMicroserviceBuilder RegisterLog(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder
            .Register<ILoggingServer>((identity, context,_) =>  new LoggingServer(identity,context));
        return builder;
    }

    public static ILoggingServer GetLog(this IMavlinkServerMicroserviceFactory factory) 
        => factory.Get<ILoggingServer>();

    #endregion
}
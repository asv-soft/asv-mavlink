namespace Asv.Mavlink;

public static class LoggingHelper
{
    public const string MicroserviceName = "LOG";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterLog(this IServerDeviceBuilder builder)
    {
        builder
            .Register<ILoggingServer>((identity, context,_) =>  new LoggingServer(identity,context));
        return builder;
    }

    public static ILoggingServer GetLog(this IServerDevice factory) 
        => factory.Get<ILoggingServer>();

    #endregion
}
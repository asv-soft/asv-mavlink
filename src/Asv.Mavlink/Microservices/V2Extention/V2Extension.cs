namespace Asv.Mavlink;

public static class V2Extension
{
    public const string MicroserviceTypeName = "V2EXT";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterV2Ext(this IServerDeviceBuilder builder)
    {
        builder
            .Register<IV2ExtensionServer>((identity, context, _) => new V2ExtensionServer(identity, context));
        return builder;
    }

    public static IV2ExtensionServer GetV2Ext(this IServerDevice factory) 
        => factory.Get<IV2ExtensionServer>();
    
   

    #endregion
}
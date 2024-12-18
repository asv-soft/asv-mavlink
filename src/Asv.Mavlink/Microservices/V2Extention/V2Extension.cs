namespace Asv.Mavlink;

public static class V2Extension
{
    public const string MicroserviceTypeName = "V2EXT";
    
    #region ServerFactory

    public static IMavlinkServerMicroserviceBuilder RegisterV2Ext(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder
            .Register<IV2ExtensionServer>((identity, context, _) => new V2ExtensionServer(identity, context));
        return builder;
    }

    public static IV2ExtensionServer GetV2Ext(this IMavlinkServerMicroserviceFactory factory) 
        => factory.Get<IV2ExtensionServer>();
    
   

    #endregion
}
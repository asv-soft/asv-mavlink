using System.Collections.Generic;
using Asv.Cfg;

namespace Asv.Mavlink;

public static class ParamsHelper
{
    public const string MicroserviceName = "PARAM";
    public const string MicroserviceExName = $"{MicroserviceName}EX";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterParams(this IServerDeviceBuilder builder)
    {
        builder.Register<IParamsServer>((identity, context, _) => new ParamsServer(identity, context));
        return builder;
    }

    public static IServerDeviceBuilder RegisterParamsEx(this IServerDeviceBuilder builder,
        IEnumerable<IMavParamTypeMetadata> paramDescriptions, IMavParamEncoding encoding)
    {
        builder.Register<IParamsServerEx, IParamsServer, IStatusTextServer>((_, _, config, @base, status) =>
            new ParamsServerEx(@base, status, paramDescriptions, encoding, config, config.Get<ParamsServerExConfig>()));
        return builder;
    }
    
    public static IServerDeviceBuilder RegisterParamsEx(this IServerDeviceBuilder builder,
        IEnumerable<IMavParamTypeMetadata> paramDescriptions, IMavParamEncoding encoding, ParamsServerExConfig config)
    {
        builder.Register<IParamsServerEx, IParamsServer, IStatusTextServer>((_, _, cfg, @base, status) =>
            new ParamsServerEx(@base, status, paramDescriptions, encoding, cfg, config));
        return builder;
    }

    public static IParamsServer GetParams(this IServerDevice factory) 
        => factory.Get<IParamsServer>();
    
    public static IParamsServerEx GetParamsEx(this IServerDevice factory) 
        => factory.Get<IParamsServerEx>();

    #endregion
}
using System.Collections.Generic;
using Asv.Cfg;

namespace Asv.Mavlink;

public static class ParamsExtHelper
{
    public const string MicroserviceName = "PARAMSEXT";
    public const string MicroserviceExName = $"{MicroserviceName}EX";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterParamsExt(this IServerDeviceBuilder builder)
    {
        builder.Register<IParamsExtServer>((identity, context, _) => new ParamsExtServer(identity, context));
        return builder;
    }

    public static IServerDeviceBuilder RegisterParamsExtEx(this IServerDeviceBuilder builder,
        IEnumerable<IMavParamExtTypeMetadata> paramDescriptions)
    {
        builder.Register<IParamsExtServerEx, IParamsExtServer, IStatusTextServer>((_, _, config, @base, status) =>
            new ParamsExtServerEx(@base, status, paramDescriptions, config, config.Get<ParamsExtServerExConfig>()));
        return builder;
    }
    
    public static IServerDeviceBuilder RegisterParamsExtEx(this IServerDeviceBuilder builder,
        IEnumerable<IMavParamExtTypeMetadata> paramDescriptions, ParamsExtServerExConfig config)
    {
        builder.Register<IParamsExtServerEx, IParamsExtServer, IStatusTextServer>((_, _, cfg, @base, status) =>
            new ParamsExtServerEx(@base, status, paramDescriptions, cfg, config));
        return builder;
    }

    public static IParamsServer GetParamsExt(this IServerDevice factory) 
        => factory.Get<IParamsServer>();
    
    public static IParamsServerEx GetParamsExtEx(this IServerDevice factory) 
        => factory.Get<IParamsServerEx>();

    #endregion
}
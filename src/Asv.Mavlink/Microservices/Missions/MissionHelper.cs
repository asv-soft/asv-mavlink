namespace Asv.Mavlink;

public static class MissionHelper
{
    public const string MicroserviceName = "MISSION";
    public const string MicroserviceExName = $"{MicroserviceName}EX";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterMission(this IServerDeviceBuilder builder)
    {
        builder
            .Register<IMissionServer>((identity, context, _) => new MissionServer(identity, context));
        return builder;
    }
    
    public static IServerDeviceBuilder RegisterMissionEx(this IServerDeviceBuilder builder)
    {
        builder
            .Register<IMissionServerEx, IMissionServer, IStatusTextServer>((_, _, _, mis, status) =>
                new MissionServerEx(mis, status));
        return builder;
    }

    public static IMissionServer GetMission(this IServerDevice factory) 
        => factory.Get<IMissionServer>();
    
    public static IMissionServerEx GetMissionEx(this IServerDevice factory) 
        => factory.Get<IMissionServerEx>();

    #endregion
}
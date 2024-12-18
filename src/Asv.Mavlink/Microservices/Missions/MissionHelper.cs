namespace Asv.Mavlink;

public static class MissionHelper
{
    public const string MicroserviceName = "MISSION";
    public const string MicroserviceExName = $"{MicroserviceName}EX";
    
    #region ServerFactory

    public static IMavlinkServerMicroserviceBuilder RegisterMission(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder
            .Register<IMissionServer>((identity, context, _) => new MissionServer(identity, context));
        return builder;
    }
    
    public static IMavlinkServerMicroserviceBuilder RegisterMissionEx(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder
            .Register<IMissionServerEx, IMissionServer, IStatusTextServer>((_, _, _, mis, status) =>
                new MissionServerEx(mis, status));
        return builder;
    }

    public static IMissionServer GetMission(this IMavlinkServerMicroserviceFactory factory) 
        => factory.Get<IMissionServer>();
    
    public static IMissionServerEx GetMissionEx(this IMavlinkServerMicroserviceFactory factory) 
        => factory.Get<IMissionServerEx>();

    #endregion
}
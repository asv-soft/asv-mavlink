namespace Asv.Mavlink;

public static class AdsbHelper
{
    public const string MicroserviceName = "ADSB";

    public static IServerDeviceBuilder RegisterAdsb(this IServerDeviceBuilder builder)
    {
        builder.Register<IAdsbVehicleServer>((identity, context,_) => new AdsbVehicleServer(identity, context));
        return builder;
    }

    public static IAdsbVehicleServer GetAdsb(this IServerDevice factory)
    {
        return factory.Get<IAdsbVehicleServer>();
    }
}
namespace Asv.Mavlink;

public static class AdsbHelper
{
    public const string MicroserviceName = "ADSB";

    public static IMavlinkServerMicroserviceBuilder RegisterAdsb(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder.Register<IAdsbVehicleServer>((identity, context) => new AdsbVehicleServer(identity, context));
        return builder;
    }

    public static IAdsbVehicleServer GetAdsb(this IMavlinkServerMicroserviceFactory factory)
    {
        return factory.Get<IAdsbVehicleServer>();
    }
}
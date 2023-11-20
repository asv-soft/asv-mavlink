namespace Asv.Mavlink;

public interface IAdsbServerDevice : IServerDevice
{
    IAdsbVehicleServer Adsb { get; }
}
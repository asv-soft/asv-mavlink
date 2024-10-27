namespace Asv.Mavlink;

/// <summary>
/// Represents a server device that supports ADS-B (Automatic Dependent Surveillance-Broadcast) functionality.
/// </summary>
public interface IAdsbServerDevice : IServerDevice
{
    IAdsbVehicleServer Adsb { get; }
    IParamsServerEx Params { get; }
    
}
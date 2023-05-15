using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IAdsbServerDevice : IServerDevice
{
    IAdsbVehicleServer Adsb { get; }
}
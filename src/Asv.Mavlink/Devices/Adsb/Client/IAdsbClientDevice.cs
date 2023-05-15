using System.Windows.Input;

namespace Asv.Mavlink;

public interface IAdsbClientDevice : IClientDevice
{
    IAdsbVehicleClient Adsb { get; }
}
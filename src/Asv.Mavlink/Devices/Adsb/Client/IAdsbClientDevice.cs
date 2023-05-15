using System.Windows.Input;

namespace Asv.Mavlink;

public interface IAdsbClientDevice : IClientDevice
{
    IAdsbVehicleClientEx AdsbClient { get; }
    ICommandClient Command { get; }
}
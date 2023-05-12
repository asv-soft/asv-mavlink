using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IAdsbServerDevice : IServerDevice
{
    ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    IAdsbVehicleServer Adsb { get; }
}
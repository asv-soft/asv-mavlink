using Asv.Mavlink.Client;

namespace Asv.Mavlink;

public interface IVehicleClient:IClientDevice
{
    ICommandClient Commands { get; }
    IParamsClientEx Params { get; }
    IMissionClientEx Missions { get; }
    IGnssClientEx Gnss { get; }
    ITelemetryClientEx Rtt { get; }
    IPositionClientEx Position { get; }
}



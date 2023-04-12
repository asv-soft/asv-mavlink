using Asv.Common;
using Asv.Mavlink.Client;

namespace Asv.Mavlink;

public interface IVehicleClient:IClientDevice
{
    ICommandClient Commands { get; }
    IParamsClient Params { get; }
    IMissionClientEx Missions { get; }
    IGnssClientEx Gnss { get; }
    ITelemetryClientEx Rtt { get; }
    IPositionClientEx Position { get; }
}



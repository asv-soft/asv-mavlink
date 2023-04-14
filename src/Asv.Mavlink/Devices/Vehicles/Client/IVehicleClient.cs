using Asv.Mavlink.Client;
using Asv.Mavlink.Client.Ftp;

namespace Asv.Mavlink;

public interface IVehicleClient:IClientDevice
{
    ICommandClient Commands { get; }
    IDebugClient Debug { get; }
    IDgpsClient Dgps { get; }
    IFtpClient Ftp { get; }
    IGnssClientEx Gnss { get; }
    IHeartbeatClient Heartbeat { get; }
    ILoggingClient Logging { get; }
    IMissionClientEx Missions { get; }
    IOffboardClient Offboard { get; }
    IParamsClientEx Params { get; }
    IPositionClientEx Position { get; }
    IStatusTextClient StatusText { get; }
    ITelemetryClientEx Rtt { get; }
    IV2ExtensionClient V2Extension { get; }
    
}



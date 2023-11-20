#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

public interface IVehicleClient:IClientDevice
{
    ICommandClient Commands { get; }
    IDebugClient Debug { get; }
    IDgpsClient Dgps { get; }
    IFtpClient Ftp { get; }
    IGnssClientEx Gnss { get; }
    ILoggingClient Logging { get; }
    IMissionClientEx Missions { get; }
    IOffboardClient Offboard { get; }
    IParamsClientEx Params { get; }
    IPositionClientEx Position { get; }
    ITelemetryClientEx Rtt { get; }
    IV2ExtensionClient V2Extension { get; }
    Task EnsureInGuidedMode(CancellationToken cancel);
    Task<bool> CheckGuidedMode(CancellationToken cancel);
    Task GoTo(GeoPoint point, CancellationToken cancel = default);
    Task DoLand(CancellationToken cancel = default);
    Task DoRtl(CancellationToken cancel = default);
    Task SetAutoMode(CancellationToken cancel = default);
    Task TakeOff(double altInMeters, CancellationToken cancel = default);
    IEnumerable<IVehicleMode> AvailableModes { get; }
    IRxValue<IVehicleMode> CurrentMode { get; }
    Task SetVehicleMode(IVehicleMode mode, CancellationToken cancel = default);
}
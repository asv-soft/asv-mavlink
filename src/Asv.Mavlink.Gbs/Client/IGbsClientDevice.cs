using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IGbsClientDevice
{
    IRxValue<AsvGbsCustomMode> CustomMode { get; }
    IRxValue<GeoPoint> Position { get; }
    IRxValue<byte> VehicleCount { get; }
    IRxValue<double> AccuracyMeter { get; }
    IRxValue<ushort> ObservationSec { get; }
    IRxValue<ushort> DgpsRate { get; }
    IRxValue<byte> AllSatellites { get; }
    IRxValue<byte> GalSatellites { get; }
    IRxValue<byte> BeidouSatellites { get; }
    IRxValue<byte> GlonassSatellites { get; }
    IRxValue<byte> GpsSatellites { get; }
    IRxValue<byte> QzssSatellites { get; }
    IRxValue<byte> SbasSatellites { get; }
    IRxValue<byte> ImesSatellites { get; }
    Task<MavResult> StartAutoMode(float duration, float accuracy, CancellationToken cancel);
    Task<MavResult> StartFixedMode(GeoPoint geoPoint, float accuracy, CancellationToken cancel);
    Task<MavResult> StartIdleMode(CancellationToken cancel);
}
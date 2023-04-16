using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public delegate Task<MavResult> StartAutoModeDelegate(float duration, float accuracy, CancellationToken cancel);
public delegate Task<MavResult> StartFixedModeDelegate(GeoPoint geoPoint, float accuracy, CancellationToken cancel);
public delegate Task<MavResult> StartIdleModeDelegate(CancellationToken cancel);

public interface IAsvGbsServerEx
{
    IAsvGbsServer Base { get; }
    IRxEditableValue<AsvGbsCustomMode> CustomMode { get; }
    IRxEditableValue<GeoPoint> Position { get; }
    IRxEditableValue<double> AccuracyMeter { get; }
    IRxEditableValue<ushort> ObservationSec { get; }
    IRxEditableValue<ushort> DgpsRate { get; }
    IRxEditableValue<byte> AllSatellites { get; }
    IRxEditableValue<byte> GalSatellites { get; }
    IRxEditableValue<byte> BeidouSatellites { get; }
    IRxEditableValue<byte> GlonassSatellites { get; }
    IRxEditableValue<byte> GpsSatellites { get; }
    IRxEditableValue<byte> QzssSatellites { get; }
    IRxEditableValue<byte> SbasSatellites { get; }
    IRxEditableValue<byte> ImesSatellites { get; }
    StartAutoModeDelegate StartAutoMode { set; }
    StartFixedModeDelegate StartFixedMode { set; }
    StartIdleModeDelegate StartIdleMode { set; }
    
    Task SendRtcmData(byte[] data, int length, CancellationToken cancel);
}
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public abstract class GbsClientDeviceBase : DisposableOnceWithCancel, IGbsClientDevice
{
    protected readonly RxValue<AsvGbsCustomMode> InternalCustomMode;
    protected readonly RxValue<GeoPoint> InternalPosition;
    protected readonly RxValue<byte> InternalVehicleCount;
    protected readonly RxValue<double> InternalAccuracyMeter;
    protected readonly RxValue<ushort> InternalObservationSec;
    protected readonly RxValue<ushort> InternalDgpsRate;
    protected readonly RxValue<byte> InternalAllSatellites;
    protected readonly RxValue<byte> InternalGalSatellites;
    protected readonly RxValue<byte> InternalBeidouSatellites;
    protected readonly RxValue<byte> InternalGlonassSatellites;
    protected readonly RxValue<byte> InternalGpsSatellites;
    protected readonly RxValue<byte> InternalQzssSatellites;
    protected readonly RxValue<byte> InternalSbasSatellites;
    protected readonly RxValue<byte> InternalImesSatellites;

    protected GbsClientDeviceBase()
    {
        InternalCustomMode = new RxValue<AsvGbsCustomMode>(AsvGbsCustomMode.AsvGbsCustomModeLoading).DisposeItWith(Disposable);
        InternalPosition = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
        InternalVehicleCount = new RxValue<byte>(0).DisposeItWith(Disposable);
        InternalAccuracyMeter = new RxValue<double>(0).DisposeItWith(Disposable);
        InternalObservationSec = new RxValue<ushort>(0).DisposeItWith(Disposable);
        InternalDgpsRate = new RxValue<ushort>(0).DisposeItWith(Disposable);
        InternalAllSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        InternalGalSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        InternalBeidouSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        InternalGlonassSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        InternalGpsSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        InternalQzssSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        InternalSbasSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        InternalImesSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
    }
    public IRxValue<AsvGbsCustomMode> CustomMode => InternalCustomMode;
    public IRxValue<GeoPoint> Position => InternalPosition;
    public IRxValue<byte> VehicleCount => InternalVehicleCount;
    public IRxValue<double> AccuracyMeter => InternalAccuracyMeter;
    public IRxValue<ushort> ObservationSec => InternalObservationSec;
    public IRxValue<ushort> DgpsRate => InternalDgpsRate;
    public IRxValue<byte> AllSatellites => InternalAllSatellites;
    public IRxValue<byte> GalSatellites => InternalGalSatellites;
    public IRxValue<byte> BeidouSatellites => InternalBeidouSatellites;
    public IRxValue<byte> GlonassSatellites => InternalGlonassSatellites;
    public IRxValue<byte> GpsSatellites => InternalGpsSatellites;
    public IRxValue<byte> QzssSatellites => InternalQzssSatellites;
    public IRxValue<byte> SbasSatellites => InternalSbasSatellites;
    public IRxValue<byte> ImesSatellites => InternalImesSatellites;
    public abstract Task<MavResult> StartAutoMode(float duration, float accuracy, CancellationToken cancel);
    public abstract Task<MavResult> StartFixedMode(GeoPoint geoPoint, float accuracy, CancellationToken cancel);
    public abstract Task<MavResult> StartIdleMode(CancellationToken cancel);

}
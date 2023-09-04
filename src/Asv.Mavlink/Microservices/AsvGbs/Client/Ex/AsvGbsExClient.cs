using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class AsvGbsExClient: DisposableOnceWithCancel, IAsvGbsExClient
{
    private readonly ICommandClient _command;
    private readonly RxValue<AsvGbsCustomMode> _internalCustomMode;
    private readonly RxValue<GeoPoint> _internalPosition;
    private readonly RxValue<double> _internalAccuracyMeter;
    private readonly RxValue<ushort> _internalObservationSec;
    private readonly RxValue<ushort> _internalDgpsRate;
    private readonly RxValue<byte> _internalAllSatellites;
    private readonly RxValue<byte> _internalGalSatellites;
    private readonly RxValue<byte> _internalBeidouSatellites;
    private readonly RxValue<byte> _internalGlonassSatellites;
    private readonly RxValue<byte> _internalGpsSatellites;
    private readonly RxValue<byte> _internalQzssSatellites;
    private readonly RxValue<byte> _internalSbasSatellites;
    private readonly RxValue<byte> _internalImesSatellites;

    public AsvGbsExClient(IAsvGbsClient asvGbs, IHeartbeatClient heartbeat, ICommandClient command)
    {
        if (heartbeat == null) throw new ArgumentNullException(nameof(heartbeat));
        _command = command ?? throw new ArgumentNullException(nameof(command));
        Base = asvGbs ?? throw new ArgumentNullException(nameof(asvGbs));
        _internalCustomMode = new RxValue<AsvGbsCustomMode>(AsvGbsCustomMode.AsvGbsCustomModeLoading).DisposeItWith(Disposable);
        _internalPosition = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
        _internalAccuracyMeter = new RxValue<double>(0).DisposeItWith(Disposable);
        _internalObservationSec = new RxValue<ushort>(0).DisposeItWith(Disposable);
        _internalDgpsRate = new RxValue<ushort>(0).DisposeItWith(Disposable);
        _internalAllSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _internalGalSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _internalBeidouSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _internalGlonassSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _internalGpsSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _internalQzssSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _internalSbasSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _internalImesSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        Base.RawStatus.Select(ConvertLocation).Subscribe(_internalPosition).DisposeItWith(Disposable);
        Base.RawStatus.Select(_=>Math.Round(_.Accuracy/100.0,2)).Subscribe(_internalAccuracyMeter).DisposeItWith(Disposable);
        Base.RawStatus.Select(_=>_.Observation).Subscribe(_internalObservationSec).DisposeItWith(Disposable);
        Base.RawStatus.Select(_=>_.DgpsRate).Subscribe(_internalDgpsRate).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatAll).Subscribe(_internalAllSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatGal).Subscribe(_internalGalSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatBdu).Subscribe(_internalBeidouSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatGlo).Subscribe(_internalGlonassSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatGps).Subscribe(_internalGpsSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatQzs).Subscribe(_internalQzssSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatSbs).Subscribe(_internalSbasSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatIme).Subscribe(_internalImesSatellites).DisposeItWith(Disposable);
        heartbeat.RawHeartbeat
            .Select(_ => (AsvGbsCustomMode)_.CustomMode)
            .Subscribe(_internalCustomMode)
            .DisposeItWith(Disposable);
    }

    private static GeoPoint ConvertLocation(AsvGbsOutStatusPayload payload)
    {
        return new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(payload.Lat), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(payload.Lng), MavlinkTypesHelper.AltFromMmToDoubleMeter(payload.Alt));
    }

    public IAsvGbsClient Base { get; }

    public async Task<MavResult> StartAutoMode(float duration, float accuracy, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var ack = await _command.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode,
            duration,
            accuracy,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            cs.Token).ConfigureAwait(false);
        return ack.Result;
    }

    public async Task<MavResult> StartFixedMode(GeoPoint geoPoint, float accuracy, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var ack = await _command.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunFixedMode,
            BitConverter.ToSingle(BitConverter.GetBytes(geoPoint.Latitude), 0),
            BitConverter.ToSingle(BitConverter.GetBytes(geoPoint.Longitude), 0),
            BitConverter.ToSingle(BitConverter.GetBytes(geoPoint.Altitude), 0),
            accuracy,
            Single.NaN,
            Single.NaN,
            Single.NaN,
             cs.Token).ConfigureAwait(false);
        return ack.Result;
    }

    public async Task<MavResult> StartIdleMode(CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var ack = await _command.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunIdleMode,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
             cs.Token).ConfigureAwait(false);
        return ack.Result;
    }
    
    public IRxValue<AsvGbsCustomMode> CustomMode => _internalCustomMode;
    public IRxValue<GeoPoint> Position => _internalPosition;
    public IRxValue<double> AccuracyMeter => _internalAccuracyMeter;
    public IRxValue<ushort> ObservationSec => _internalObservationSec;
    public IRxValue<ushort> DgpsRate => _internalDgpsRate;
    public IRxValue<byte> AllSatellites => _internalAllSatellites;
    public IRxValue<byte> GalSatellites => _internalGalSatellites;
    public IRxValue<byte> BeidouSatellites => _internalBeidouSatellites;
    public IRxValue<byte> GlonassSatellites => _internalGlonassSatellites;
    public IRxValue<byte> GpsSatellites => _internalGpsSatellites;
    public IRxValue<byte> QzssSatellites => _internalQzssSatellites;
    public IRxValue<byte> SbasSatellites => _internalSbasSatellites;
    public IRxValue<byte> ImesSatellites => _internalImesSatellites;
}
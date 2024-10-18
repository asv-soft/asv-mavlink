using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class AsvGbsExClient: DisposableOnceWithCancel, IAsvGbsExClient
{
    private readonly ILogger _logger;
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
    

    public AsvGbsExClient(
        IAsvGbsClient asvGbs, 
        IHeartbeatClient heartbeat, 
        ICommandClient command,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? logFactory = null)
    {
        logFactory??=NullLoggerFactory.Instance;
        _logger = logFactory.CreateLogger<AsvGbsExClient>();
        ArgumentNullException.ThrowIfNull(heartbeat);
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
        Base.RawStatus.Select(p=>Math.Round(p.Accuracy/100.0,2)).Subscribe(_internalAccuracyMeter).DisposeItWith(Disposable);
        Base.RawStatus.Select(p=>p.Observation).Subscribe(_internalObservationSec).DisposeItWith(Disposable);
        Base.RawStatus.Select(p=>p.DgpsRate).Subscribe(_internalDgpsRate).DisposeItWith(Disposable);
        Base.RawStatus.Select(p => p.SatAll).Subscribe(_internalAllSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(p => p.SatGal).Subscribe(_internalGalSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(p => p.SatBdu).Subscribe(_internalBeidouSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(p => p.SatGlo).Subscribe(_internalGlonassSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(p => p.SatGps).Subscribe(_internalGpsSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(p => p.SatQzs).Subscribe(_internalQzssSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(p => p.SatSbs).Subscribe(_internalSbasSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(p => p.SatIme).Subscribe(_internalImesSatellites).DisposeItWith(Disposable);
        heartbeat.RawHeartbeat
            .Select(p => (AsvGbsCustomMode)p.CustomMode)
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
            BitConverter.ToSingle(BitConverter.GetBytes((Int32)(geoPoint.Latitude * 10000000)), 0),
            BitConverter.ToSingle(BitConverter.GetBytes((Int32)(geoPoint.Longitude * 10000000)), 0),
            BitConverter.ToSingle(BitConverter.GetBytes((Int32)(geoPoint.Altitude * 1000)), 0),
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
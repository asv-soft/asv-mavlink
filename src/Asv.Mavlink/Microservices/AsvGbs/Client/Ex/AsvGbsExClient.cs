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
using R3;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class AsvGbsExClient: IAsvGbsExClient, IDisposable
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
    private readonly IDisposable _disposeIt;
    private readonly CancellationTokenSource _disposedCancel;


    public AsvGbsExClient(
        IAsvGbsClient client, 
        IHeartbeatClient heartbeat, 
        ICommandClient command)
    {
        _logger = client.Core.Log.CreateLogger<AsvGbsExClient>();
        ArgumentNullException.ThrowIfNull(heartbeat);
        _command = command ?? throw new ArgumentNullException(nameof(command));
        Base = client ?? throw new ArgumentNullException(nameof(client));
        var builder = Disposable.CreateBuilder();
        _disposedCancel = new CancellationTokenSource().AddTo(ref builder);
        _internalCustomMode = new RxValue<AsvGbsCustomMode>(AsvGbsCustomMode.AsvGbsCustomModeLoading).AddTo(ref builder);
        _internalPosition = new RxValue<GeoPoint>(GeoPoint.Zero).AddTo(ref builder);
        _internalAccuracyMeter = new RxValue<double>(0).AddTo(ref builder);
        _internalObservationSec = new RxValue<ushort>(0).AddTo(ref builder);
        _internalDgpsRate = new RxValue<ushort>(0).AddTo(ref builder);
        _internalAllSatellites = new RxValue<byte>(0).AddTo(ref builder);
        _internalGalSatellites = new RxValue<byte>(0).AddTo(ref builder);
        _internalBeidouSatellites = new RxValue<byte>(0).AddTo(ref builder);
        _internalGlonassSatellites = new RxValue<byte>(0).AddTo(ref builder);
        _internalGpsSatellites = new RxValue<byte>(0).AddTo(ref builder);
        _internalQzssSatellites = new RxValue<byte>(0).AddTo(ref builder);
        _internalSbasSatellites = new RxValue<byte>(0).AddTo(ref builder);
        _internalImesSatellites = new RxValue<byte>(0).AddTo(ref builder);
        Base.RawStatus.Select(ConvertLocation).Subscribe(_internalPosition).AddTo(ref builder);
        Base.RawStatus.Select(p=>Math.Round(p.Accuracy/100.0,2)).Subscribe(_internalAccuracyMeter).AddTo(ref builder);
        Base.RawStatus.Select(p=>p.Observation).Subscribe(_internalObservationSec).AddTo(ref builder);
        Base.RawStatus.Select(p=>p.DgpsRate).Subscribe(_internalDgpsRate).AddTo(ref builder);
        Base.RawStatus.Select(p => p.SatAll).Subscribe(_internalAllSatellites).AddTo(ref builder);
        Base.RawStatus.Select(p => p.SatGal).Subscribe(_internalGalSatellites).AddTo(ref builder);
        Base.RawStatus.Select(p => p.SatBdu).Subscribe(_internalBeidouSatellites).AddTo(ref builder);
        Base.RawStatus.Select(p => p.SatGlo).Subscribe(_internalGlonassSatellites).AddTo(ref builder);
        Base.RawStatus.Select(p => p.SatGps).Subscribe(_internalGpsSatellites).AddTo(ref builder);
        Base.RawStatus.Select(p => p.SatQzs).Subscribe(_internalQzssSatellites).AddTo(ref builder);
        Base.RawStatus.Select(p => p.SatSbs).Subscribe(_internalSbasSatellites).AddTo(ref builder);
        Base.RawStatus.Select(p => p.SatIme).Subscribe(_internalImesSatellites).AddTo(ref builder);
        heartbeat.RawHeartbeat
            .Select(p => (AsvGbsCustomMode)p.CustomMode)
            .Subscribe(_internalCustomMode)
            .AddTo(ref builder);
        
        _disposeIt = builder.Build();
    }
    public string Name => $"{Base.Name}Ex";
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

    private CancellationToken DisposeCancel => _disposedCancel.Token;

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

    public void Dispose()
    {
        _disposedCancel.Cancel();
        _disposeIt.Dispose();
    }

    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
}
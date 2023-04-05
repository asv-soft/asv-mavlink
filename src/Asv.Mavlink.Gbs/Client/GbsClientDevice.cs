using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class GbsClientDevice : DisposableOnceWithCancel, IGbsClientDevice
{
    private readonly IMavlinkClient _client;
    private readonly RxValue<AsvGbsCustomMode> _customMode;
    private readonly RxValue<GeoPoint> _position;
    private readonly RxValue<byte> _vehicleCount;
    private readonly RxValue<double> _accuracyMeter;
    private readonly RxValue<ushort> _observationSec;
    private readonly RxValue<ushort> _dgpsRate;
    private readonly RxValue<byte> _allSatellites;
    private readonly RxValue<byte> _galSatellites;
    private readonly RxValue<byte> _beidouSatellites;
    private readonly RxValue<byte> _glonassSatellites;
    private readonly RxValue<byte> _gpsSatellites;
    private readonly RxValue<byte> _qzssSatellites;
    private readonly RxValue<byte> _sbasSatellites;
    private readonly RxValue<byte> _imesSatellites;
    private const int DefaultAttemptCount = 3;

    public GbsClientDevice(IMavlinkClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        
        _position = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(ConvertLocation).Subscribe(_position).DisposeItWith(Disposable);
        
        _vehicleCount = new RxValue<byte>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_=>_.VehicleCount).Subscribe(_vehicleCount).DisposeItWith(Disposable);
        
        _accuracyMeter = new RxValue<double>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_=>Math.Round(_.Accuracy/100.0,2)).Subscribe(_accuracyMeter).DisposeItWith(Disposable);
        
        _observationSec = new RxValue<ushort>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_=>_.Observation).Subscribe(_observationSec).DisposeItWith(Disposable);
        
        _dgpsRate = new RxValue<ushort>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_=>_.DgpsRate).Subscribe(_dgpsRate).DisposeItWith(Disposable);
        
        _allSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_ => _.SatAll).Subscribe(_allSatellites).DisposeItWith(Disposable);
        
        _galSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_ => _.SatGal).Subscribe(_galSatellites).DisposeItWith(Disposable);
        
        _beidouSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_ => _.SatBdu).Subscribe(_beidouSatellites).DisposeItWith(Disposable);
        
        _glonassSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_ => _.SatGlo).Subscribe(_glonassSatellites).DisposeItWith(Disposable);
        
        _gpsSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_ => _.SatGps).Subscribe(_gpsSatellites).DisposeItWith(Disposable);
        
        _qzssSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_ => _.SatQzs).Subscribe(_qzssSatellites).DisposeItWith(Disposable);
        
        _sbasSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_ => _.SatSbs).Subscribe(_sbasSatellites).DisposeItWith(Disposable);
        
        _imesSatellites = new RxValue<byte>(0).DisposeItWith(Disposable);
        _client.Gbs.Status.Select(_ => _.SatIme).Subscribe(_imesSatellites).DisposeItWith(Disposable);

        _customMode = new RxValue<AsvGbsCustomMode>();
        _client.Heartbeat.RawHeartbeat.Select(_ => (AsvGbsCustomMode)_.CustomMode).Subscribe(_customMode)
            .DisposeItWith(Disposable);
        
        
    }

    private static GeoPoint ConvertLocation(AsvGbsOutStatusPayload payload)
    {
        return new GeoPoint(payload.Lat / 10000000D, payload.Lng / 10000000D, payload.Alt / 1000D);
    }

    public IRxValue<AsvGbsCustomMode> CustomMode => _customMode;
    public IRxValue<GeoPoint> Position => _position;
    public IRxValue<byte> VehicleCount => _vehicleCount;
    public IRxValue<double> AccuracyMeter => _accuracyMeter;
    public IRxValue<ushort> ObservationSec => _observationSec;
    public IRxValue<ushort> DgpsRate => _dgpsRate;
    public IRxValue<byte> AllSatellites => _allSatellites;
    public IRxValue<byte> GalSatellites => _galSatellites;
    public IRxValue<byte> BeidouSatellites => _beidouSatellites;
    public IRxValue<byte> GlonassSatellites => _glonassSatellites;
    public IRxValue<byte> GpsSatellites => _gpsSatellites;
    public IRxValue<byte> QzssSatellites => _qzssSatellites;
    public IRxValue<byte> SbasSatellites => _sbasSatellites;
    public IRxValue<byte> ImesSatellites => _imesSatellites;

    public async Task<MavResult> StartAutoMode(float duration, float accuracy, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _client.Commands.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode,
            duration,
            accuracy,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            DefaultAttemptCount, cs.Token);
        return result.Result;
    }

    public async Task<MavResult> StartFixedMode(GeoPoint geoPoint, float accuracy, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _client.Commands.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunFixedMode,
            BitConverter.ToSingle(BitConverter.GetBytes(geoPoint.Latitude), 0),
            BitConverter.ToSingle(BitConverter.GetBytes(geoPoint.Longitude), 0),
            BitConverter.ToSingle(BitConverter.GetBytes(geoPoint.Altitude), 0),
            (float)accuracy,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            DefaultAttemptCount, cs.Token);
        return result.Result;
    }

    public async Task<MavResult> StartIdleMode(CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _client.Commands.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunIdleMode,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            Single.NaN,
            DefaultAttemptCount, cs.Token);
        return result.Result;
    }
}
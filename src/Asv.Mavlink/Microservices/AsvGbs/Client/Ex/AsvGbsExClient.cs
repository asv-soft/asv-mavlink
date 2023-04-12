using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class AsvGbsExClientConfig
{
    public int DefaultAttemptCount { get; set; } = 5;
}

public class AsvGbsExClient: AsvGbsExBase, IAsvGbsExClient
{
    private readonly ICommandClient _command;
    private readonly AsvGbsExClientConfig _config;

    public AsvGbsExClient(IAsvGbsClient asvGbs, IHeartbeatClient heartbeat, ICommandClient command,AsvGbsExClientConfig config)
    {
        if (heartbeat == null) throw new ArgumentNullException(nameof(heartbeat));
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        Base = asvGbs ?? throw new ArgumentNullException(nameof(asvGbs));
        Base.RawStatus.Select(ConvertLocation).Subscribe(InternalPosition).DisposeItWith(Disposable);
        Base.RawStatus.Select(_=>_.VehicleCount).Subscribe(InternalVehicleCount).DisposeItWith(Disposable);
        Base.RawStatus.Select(_=>Math.Round(_.Accuracy/100.0,2)).Subscribe(InternalAccuracyMeter).DisposeItWith(Disposable);
        Base.RawStatus.Select(_=>_.Observation).Subscribe(InternalObservationSec).DisposeItWith(Disposable);
        Base.RawStatus.Select(_=>_.DgpsRate).Subscribe(InternalDgpsRate).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatAll).Subscribe(InternalAllSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatGal).Subscribe(InternalGalSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatBdu).Subscribe(InternalBeidouSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatGlo).Subscribe(InternalGlonassSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatGps).Subscribe(InternalGpsSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatQzs).Subscribe(InternalQzssSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatSbs).Subscribe(InternalSbasSatellites).DisposeItWith(Disposable);
        Base.RawStatus.Select(_ => _.SatIme).Subscribe(InternalImesSatellites).DisposeItWith(Disposable);
        heartbeat.RawHeartbeat
            .Select(_ => (AsvGbsCustomMode)_.CustomMode)
            .Subscribe(InternalCustomMode)
            .DisposeItWith(Disposable);
    }

    private static GeoPoint ConvertLocation(AsvGbsOutStatusPayload payload)
    {
        return new GeoPoint(payload.Lat / 10000000D, payload.Lng / 10000000D, payload.Alt / 1000D);
    }

    public IAsvGbsClient Base { get; }

    public override async Task<MavResult> StartAutoMode(float duration, float accuracy, CancellationToken cancel)
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
            _config.DefaultAttemptCount, cs.Token).ConfigureAwait(false);
        return ack.Result;
    }

    public override async Task<MavResult> StartFixedMode(GeoPoint geoPoint, float accuracy, CancellationToken cancel)
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
            _config.DefaultAttemptCount, cs.Token).ConfigureAwait(false);
        return ack.Result;
    }

    public override async Task<MavResult> StartIdleMode(CancellationToken cancel)
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
            _config.DefaultAttemptCount, cs.Token).ConfigureAwait(false);
        return ack.Result;
    }
}
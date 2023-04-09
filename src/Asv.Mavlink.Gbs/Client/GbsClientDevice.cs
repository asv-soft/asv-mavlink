using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class GbsClientDevice : GbsClientDeviceBase, IGbsClientDevice
{
    private readonly IMavlinkClient _client;
    private const int DefaultAttemptCount = 3;

    public GbsClientDevice(IMavlinkClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        
        _client.Gbs.RawStatus.Select(ConvertLocation).Subscribe(InternalPosition).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_=>_.VehicleCount).Subscribe(InternalVehicleCount).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_=>Math.Round(_.Accuracy/100.0,2)).Subscribe(InternalAccuracyMeter).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_=>_.Observation).Subscribe(InternalObservationSec).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_=>_.DgpsRate).Subscribe(InternalDgpsRate).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_ => _.SatAll).Subscribe(InternalAllSatellites).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_ => _.SatGal).Subscribe(InternalGalSatellites).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_ => _.SatBdu).Subscribe(InternalBeidouSatellites).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_ => _.SatGlo).Subscribe(InternalGlonassSatellites).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_ => _.SatGps).Subscribe(InternalGpsSatellites).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_ => _.SatQzs).Subscribe(InternalQzssSatellites).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_ => _.SatSbs).Subscribe(InternalSbasSatellites).DisposeItWith(Disposable);
        _client.Gbs.RawStatus.Select(_ => _.SatIme).Subscribe(InternalImesSatellites).DisposeItWith(Disposable);
        _client.Heartbeat.RawHeartbeat
            .Select(_ => (AsvGbsCustomMode)_.CustomMode)
            .Subscribe(InternalCustomMode)
            .DisposeItWith(Disposable);
    }

    private static GeoPoint ConvertLocation(AsvGbsOutStatusPayload payload)
    {
        return new GeoPoint(payload.Lat / 10000000D, payload.Lng / 10000000D, payload.Alt / 1000D);
    }

    public override async Task<MavResult> StartAutoMode(float duration, float accuracy, CancellationToken cancel)
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

    public override async Task<MavResult> StartFixedMode(GeoPoint geoPoint, float accuracy, CancellationToken cancel)
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

    public override async Task<MavResult> StartIdleMode(CancellationToken cancel)
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
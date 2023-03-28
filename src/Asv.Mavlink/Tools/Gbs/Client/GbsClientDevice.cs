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
    private const int DefaultAttemptCount = 3;

    public GbsClientDevice(IMavlinkClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _position = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
        _customMode = new RxValue<AsvGbsCustomMode>();
        _client.Heartbeat.RawHeartbeat.Select(_ => (AsvGbsCustomMode)_.CustomMode).Subscribe(_customMode)
            .DisposeItWith(Disposable);
        _client.Gbs.Status.Select(Convert).Subscribe(_position).DisposeItWith(Disposable);
    }

    private static GeoPoint Convert(AsvGbsOutStatusPayload payload)
    {
        return new GeoPoint(payload.Lat / 10000000D, payload.Lng / 10000000D, payload.Alt / 1000D);
    }

    public IRxValue<AsvGbsCustomMode> CustomMode => _customMode;
    public IRxValue<GeoPoint> Position => _position;

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
        var result = await _client.Commands.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode,
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
        var result = await _client.Commands.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode,
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
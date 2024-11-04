using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using R3;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class AsvGbsExClient: IAsvGbsExClient, IDisposable, IAsyncDisposable
{
    private readonly ILogger _logger;
    private readonly ICommandClient _command;
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
        _disposedCancel = new CancellationTokenSource();
        CustomMode = heartbeat.RawHeartbeat
            .Select(p =>
            {
                if (p != null) return (AsvGbsCustomMode)(p.CustomMode);
                return AsvGbsCustomMode.AsvGbsCustomModeLoading;
            }).ToReadOnlyReactiveProperty();
        Position = Base.RawStatus.Select(ConvertLocation).ToReadOnlyReactiveProperty();
        AccuracyMeter =
            Base.RawStatus.Select(p => Math.Round((p?.Accuracy ?? 0) / 100.0, 2)).ToReadOnlyReactiveProperty();
        ObservationSec = Base.RawStatus.Select(p=>p?.Observation ?? 0).ToReadOnlyReactiveProperty();
        DgpsRate = Base.RawStatus.Select(p=>p?.DgpsRate ?? 0).ToReadOnlyReactiveProperty();
        AllSatellites = Base.RawStatus.Select(p => p?.SatAll?? 0).ToReadOnlyReactiveProperty();
        GalSatellites = Base.RawStatus.Select(p => p?.SatGal?? 0).ToReadOnlyReactiveProperty();
        BeidouSatellites = Base.RawStatus.Select(p => p?.SatBdu?? 0).ToReadOnlyReactiveProperty();
        GlonassSatellites = Base.RawStatus.Select(p => p?.SatGlo?? 0).ToReadOnlyReactiveProperty();
        GpsSatellites = Base.RawStatus.Select(p => p?.SatGps?? 0).ToReadOnlyReactiveProperty();
        QzssSatellites = Base.RawStatus.Select(p => p?.SatQzs?? 0).ToReadOnlyReactiveProperty();
        SbasSatellites = Base.RawStatus.Select(p => p?.SatSbs?? 0).ToReadOnlyReactiveProperty();
        ImesSatellites = Base.RawStatus.Select(p => p?.SatIme?? 0).ToReadOnlyReactiveProperty();
       
    }
    public IAsvGbsClient Base { get; }
    public string Name => $"{Base.Name}Ex";
    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
    private static GeoPoint ConvertLocation(AsvGbsOutStatusPayload? payload)
    {
        if (payload == null) return GeoPoint.NaN;
        return new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(payload.Lat), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(payload.Lng), MavlinkTypesHelper.AltFromMmToDoubleMeter(payload.Alt));
    }
    public async Task<MavResult> StartAutoMode(float duration, float accuracy, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var ack = await _command.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode,
            duration,
            accuracy,
            float.NaN,
            float.NaN,
            float.NaN,
            float.NaN,
            float.NaN,
            cs.Token).ConfigureAwait(false);
        return ack.Result;
    }

    public async Task<MavResult> StartFixedMode(GeoPoint geoPoint, float accuracy, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var ack = await _command.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunFixedMode,
            BitConverter.ToSingle(BitConverter.GetBytes((int)(geoPoint.Latitude * 10000000)), 0),
            BitConverter.ToSingle(BitConverter.GetBytes((int)(geoPoint.Longitude * 10000000)), 0),
            BitConverter.ToSingle(BitConverter.GetBytes((int)(geoPoint.Altitude * 1000)), 0),
            accuracy,
            float.NaN,
            float.NaN,
            float.NaN,
             cs.Token).ConfigureAwait(false);
        return ack.Result;
    }

    public async Task<MavResult> StartIdleMode(CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var ack = await _command.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunIdleMode,
            float.NaN,
            float.NaN,
            float.NaN,
            float.NaN,
            float.NaN,
            float.NaN,
            float.NaN,
             cs.Token).ConfigureAwait(false);
        return ack.Result;
    }

    private CancellationToken DisposeCancel => _disposedCancel.Token;

    public ReadOnlyReactiveProperty<AsvGbsCustomMode> CustomMode { get; }
    public ReadOnlyReactiveProperty<GeoPoint> Position { get; }
    public ReadOnlyReactiveProperty<double> AccuracyMeter { get; }
    public ReadOnlyReactiveProperty<ushort> ObservationSec { get; }
    public ReadOnlyReactiveProperty<ushort> DgpsRate { get; }
    public ReadOnlyReactiveProperty<byte> AllSatellites{ get; }
    public ReadOnlyReactiveProperty<byte> GalSatellites { get; }
    public ReadOnlyReactiveProperty<byte> BeidouSatellites { get; }
    public ReadOnlyReactiveProperty<byte> GlonassSatellites { get; }
    public ReadOnlyReactiveProperty<byte> GpsSatellites { get; }
    public ReadOnlyReactiveProperty<byte> QzssSatellites { get; }
    public ReadOnlyReactiveProperty<byte> SbasSatellites { get; }
    public ReadOnlyReactiveProperty<byte> ImesSatellites { get; }


    #region Dispose

    public void Dispose()
    {
        _disposedCancel.Dispose();
        CustomMode.Dispose();
        Position.Dispose();
        AccuracyMeter.Dispose();
        ObservationSec.Dispose();
        DgpsRate.Dispose();
        AllSatellites.Dispose();
        GalSatellites.Dispose();
        BeidouSatellites.Dispose();
        GlonassSatellites.Dispose();
        GpsSatellites.Dispose();
        QzssSatellites.Dispose();
        SbasSatellites.Dispose();
        ImesSatellites.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_disposedCancel).ConfigureAwait(false);
        await CastAndDispose(CustomMode).ConfigureAwait(false);
        await CastAndDispose(Position).ConfigureAwait(false);
        await CastAndDispose(AccuracyMeter).ConfigureAwait(false);
        await CastAndDispose(ObservationSec).ConfigureAwait(false);
        await CastAndDispose(DgpsRate).ConfigureAwait(false);
        await CastAndDispose(AllSatellites).ConfigureAwait(false);
        await CastAndDispose(GalSatellites).ConfigureAwait(false);
        await CastAndDispose(BeidouSatellites).ConfigureAwait(false);
        await CastAndDispose(GlonassSatellites).ConfigureAwait(false);
        await CastAndDispose(GpsSatellites).ConfigureAwait(false);
        await CastAndDispose(QzssSatellites).ConfigureAwait(false);
        await CastAndDispose(SbasSatellites).ConfigureAwait(false);
        await CastAndDispose(ImesSatellites).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}
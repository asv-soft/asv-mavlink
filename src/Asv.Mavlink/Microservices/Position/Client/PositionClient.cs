using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class PositionClient : MavlinkMicroserviceClient, IPositionClient
{
    

    private readonly ILogger _logger;
    private readonly ReadOnlyReactiveProperty<PositionTargetGlobalIntPayload?> _target;
    private readonly ReadOnlyReactiveProperty<HomePositionPayload?> _home;
    private readonly ReadOnlyReactiveProperty<GlobalPositionIntPayload?> _globalPosition;
    private readonly ReadOnlyReactiveProperty<AltitudePayload?> _altitude;
    private readonly ReadOnlyReactiveProperty<VfrHudPayload?> _vfrHud;
    private readonly ReadOnlyReactiveProperty<HighresImuPayload?> _imu;
    private readonly ReadOnlyReactiveProperty<AttitudePayload?> _attitude;

    public PositionClient(MavlinkClientIdentity identity, ICoreServices core)
        : base("CTRL", identity, core)
    {
        _logger = core.LoggerFactory.CreateLogger<PositionClient>();
        _target = InternalFilter<PositionTargetGlobalIntPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        _home = InternalFilter<HomePositionPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        _globalPosition = InternalFilter<GlobalPositionIntPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        _altitude = InternalFilter<AltitudePacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        _vfrHud = InternalFilter<VfrHudPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        _imu = InternalFilter<HighresImuPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        _attitude = InternalFilter<AttitudePacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
    }
    public ReadOnlyReactiveProperty<GlobalPositionIntPayload?> GlobalPosition => _globalPosition;
    public ReadOnlyReactiveProperty<HomePositionPayload?> Home => _home;
    public ReadOnlyReactiveProperty<PositionTargetGlobalIntPayload?> Target => _target;
    public ReadOnlyReactiveProperty<AltitudePayload?> Altitude => _altitude;
    public ReadOnlyReactiveProperty<VfrHudPayload?> VfrHud => _vfrHud;
    public ReadOnlyReactiveProperty<HighresImuPayload?> Imu => _imu;
    public ReadOnlyReactiveProperty<AttitudePayload?> Attitude => _attitude;

    public ValueTask SetTargetGlobalInt(uint timeBootMs, MavFrame coordinateFrame, int latInt, int lonInt, float alt,
        float vx, float vy, float vz, float afx, float afy, float afz, float yaw,
        float yawRate, PositionTargetTypemask typeMask, CancellationToken cancel)
    {
        _logger.ZLogDebug($"{Id} {nameof(SetTargetGlobalInt)} ");
        return InternalSend<SetPositionTargetGlobalIntPacket>(p =>
        {
            p.Payload.TimeBootMs = timeBootMs;
            p.Payload.TargetSystem = Identity.Target.SystemId;
            p.Payload.TargetComponent = Identity.Target.ComponentId;
            p.Payload.CoordinateFrame = coordinateFrame;
            p.Payload.LatInt = latInt;
            p.Payload.LonInt = lonInt;
            p.Payload.Alt = alt;
            p.Payload.Vx = vx;
            p.Payload.Vy = vy;
            p.Payload.Vz = vz;
            p.Payload.Afx = afx;
            p.Payload.Afy = afy;
            p.Payload.Afz = afz;
            p.Payload.Yaw = yaw;
            p.Payload.YawRate = yawRate;
            p.Payload.TypeMask = typeMask;
        }, cancel);
    }

    public ValueTask SetPositionTargetLocalNed(uint timeBootMs, MavFrame coordinateFrame, PositionTargetTypemask typeMask, float x,
        float y, float z, float vx, float vy, float vz, float afx, float afy, float afz, float yaw, float yawRate,
        CancellationToken cancel)
    {
        return InternalSend<SetPositionTargetLocalNedPacket>(p =>
        {
            p.Payload.TimeBootMs = timeBootMs;
            p.Payload.TargetComponent = Identity.Target.ComponentId;
            p.Payload.TargetSystem = Identity.Target.SystemId;
            p.Payload.CoordinateFrame = coordinateFrame;
            p.Payload.TypeMask = typeMask;
            p.Payload.X = x;
            p.Payload.Y = y;
            p.Payload.Z = z;
            p.Payload.Vx = vx;
            p.Payload.Vy = vy;
            p.Payload.Vz = vz;
            p.Payload.Afx = afx;
            p.Payload.Afy = afy;
            p.Payload.Afz = afz;
            p.Payload.Yaw = yaw;
            p.Payload.YawRate = yawRate;
        }, cancel);
    }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _target.Dispose();
            _home.Dispose();
            _globalPosition.Dispose();
            _altitude.Dispose();
            _vfrHud.Dispose();
            _imu.Dispose();
            _attitude.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_target).ConfigureAwait(false);
        await CastAndDispose(_home).ConfigureAwait(false);
        await CastAndDispose(_globalPosition).ConfigureAwait(false);
        await CastAndDispose(_altitude).ConfigureAwait(false);
        await CastAndDispose(_vfrHud).ConfigureAwait(false);
        await CastAndDispose(_imu).ConfigureAwait(false);
        await CastAndDispose(_attitude).ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

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

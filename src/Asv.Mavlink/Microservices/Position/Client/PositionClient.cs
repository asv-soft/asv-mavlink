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

    public PositionClient(MavlinkClientIdentity identity, IMavlinkContext core)
        : base("CTRL", identity, core)
    {
        _logger = core.LoggerFactory.CreateLogger<PositionClient>();
        Target = InternalFilter<PositionTargetGlobalIntPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        Home = InternalFilter<HomePositionPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        GlobalPosition = InternalFilter<GlobalPositionIntPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        Altitude = InternalFilter<AltitudePacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        VfrHud = InternalFilter<VfrHudPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        Imu = InternalFilter<HighresImuPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        Attitude = InternalFilter<AttitudePacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
    }

    public ReadOnlyReactiveProperty<GlobalPositionIntPayload?> GlobalPosition { get; }
    public ReadOnlyReactiveProperty<HomePositionPayload?> Home { get; }
    public ReadOnlyReactiveProperty<PositionTargetGlobalIntPayload?> Target { get; }
    public ReadOnlyReactiveProperty<AltitudePayload?> Altitude { get; }
    public ReadOnlyReactiveProperty<VfrHudPayload?> VfrHud { get; }
    public ReadOnlyReactiveProperty<HighresImuPayload?> Imu { get; }
    public ReadOnlyReactiveProperty<AttitudePayload?> Attitude { get; }

    public ValueTask SetTargetGlobalInt(
        uint timeBootMs, 
        MavFrame coordinateFrame, 
        int latInt, 
        int lonInt, 
        float alt,
        float vx, 
        float vy, 
        float vz, 
        float afx, 
        float afy, 
        float afz, 
        float yaw,
        float yawRate, 
        PositionTargetTypemask typeMask, 
        CancellationToken cancel
    )
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

    public ValueTask SetPositionTargetLocalNed(
        uint timeBootMs, 
        MavFrame coordinateFrame, 
        PositionTargetTypemask typeMask, 
        float x,
        float y, 
        float z, 
        float vx, 
        float vy, 
        float vz, 
        float afx,
        float afy,
        float afz, 
        float yaw,
        float yawRate,
        CancellationToken cancel
    )
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
            Target.Dispose();
            Home.Dispose();
            GlobalPosition.Dispose();
            Altitude.Dispose();
            VfrHud.Dispose();
            Imu.Dispose();
            Attitude.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(Target).ConfigureAwait(false);
        await CastAndDispose(Home).ConfigureAwait(false);
        await CastAndDispose(GlobalPosition).ConfigureAwait(false);
        await CastAndDispose(Attitude).ConfigureAwait(false);
        await CastAndDispose(VfrHud).ConfigureAwait(false);
        await CastAndDispose(Imu).ConfigureAwait(false);
        await CastAndDispose(Attitude).ConfigureAwait(false);

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

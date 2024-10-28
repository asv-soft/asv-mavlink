using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        _logger = core.Log.CreateLogger<PositionClient>();
        _target = InternalFilter<PositionTargetGlobalIntPacket>()
            .Select(p => p.Payload)
            .ToReadOnlyReactiveProperty();
        _home = InternalFilter<HomePositionPacket>()
            .Select(p => p.Payload)
            .ToReadOnlyReactiveProperty();
        _globalPosition = InternalFilter<GlobalPositionIntPacket>()
            .Select(p => p.Payload)
            .ToReadOnlyReactiveProperty();
        _altitude = InternalFilter<AltitudePacket>()
            .Select(p => p.Payload)
            .ToReadOnlyReactiveProperty();
        _vfrHud = InternalFilter<VfrHudPacket>()
            .Select(p => p.Payload)
            .ToReadOnlyReactiveProperty();
        _imu = InternalFilter<HighresImuPacket>()
            .Select(p => p.Payload)
            .ToReadOnlyReactiveProperty();
        _attitude = InternalFilter<AttitudePacket>()
            .Select(p => p.Payload)
            .ToReadOnlyReactiveProperty();
    }
    public ReadOnlyReactiveProperty<GlobalPositionIntPayload?> GlobalPosition => _globalPosition;
    public ReadOnlyReactiveProperty<HomePositionPayload?> Home => _home;
    public ReadOnlyReactiveProperty<PositionTargetGlobalIntPayload?> Target => _target;
    public ReadOnlyReactiveProperty<AltitudePayload?> Altitude => _altitude;
    public ReadOnlyReactiveProperty<VfrHudPayload?> VfrHud => _vfrHud;
    public ReadOnlyReactiveProperty<HighresImuPayload?> Imu => _imu;
    public ReadOnlyReactiveProperty<AttitudePayload?> Attitude => _attitude;

    public Task SetTargetGlobalInt(uint timeBootMs, MavFrame coordinateFrame, int latInt, int lonInt, float alt,
        float vx, float vy, float vz, float afx, float afy, float afz, float yaw,
        float yawRate, PositionTargetTypemask typeMask, CancellationToken cancel)
    {
        _logger.ZLogDebug($"{LogSend} {nameof(SetTargetGlobalInt)} ");
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

    
}

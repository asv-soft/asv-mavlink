using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class PositionClient : MavlinkMicroserviceClient, IPositionClient
{
    private readonly ILogger _logger;
    private readonly RxValueBehaviour<GlobalPositionIntPayload?> _globalPosition;
    private readonly RxValueBehaviour<HomePositionPayload?> _home;
    private readonly RxValueBehaviour<PositionTargetGlobalIntPayload?> _target;
    private readonly RxValueBehaviour<AltitudePayload?> _altitude;
    private readonly RxValueBehaviour<VfrHudPayload?> _vfrHud;
    private readonly RxValueBehaviour<HighresImuPayload?> _imu;
    private readonly RxValueBehaviour<AttitudePayload?> _attitude;

    public PositionClient(MavlinkClientIdentity identity, ICoreServices core)
        : base("CTRL", identity, core)
    {
        _logger = core.Log.CreateLogger<PositionClient>();
        _target = new RxValueBehaviour<PositionTargetGlobalIntPayload?>(default);
        InternalFilter<PositionTargetGlobalIntPacket>()
            .Select(p => p.Payload).Subscribe(_target);
        _home = new RxValueBehaviour<HomePositionPayload?>(default);
        InternalFilter<HomePositionPacket>()
            .Select(p => p.Payload).Subscribe(_home);
        _globalPosition = new RxValueBehaviour<GlobalPositionIntPayload?>(default);
        InternalFilter<GlobalPositionIntPacket>()
            .Select(p => p.Payload).Subscribe(_globalPosition);
        _altitude = new RxValueBehaviour<AltitudePayload?>(default);
        InternalFilter<AltitudePacket>()
            .Select(p => p.Payload).Subscribe(_altitude);
        _vfrHud = new RxValueBehaviour<VfrHudPayload?>(default);
        InternalFilter<VfrHudPacket>()
            .Select(p => p.Payload).Subscribe(_vfrHud);
        _imu = new RxValueBehaviour<HighresImuPayload?>(default);
        InternalFilter<HighresImuPacket>()
            .Select(p => p.Payload).Subscribe(_imu);
        _attitude = new RxValueBehaviour<AttitudePayload?>(default);
        InternalFilter<AttitudePacket>()
            .Select(p => p.Payload).Subscribe(_attitude);
    }
    public IRxValue<GlobalPositionIntPayload?> GlobalPosition => _globalPosition;
    public IRxValue<HomePositionPayload?> Home => _home;
    public IRxValue<PositionTargetGlobalIntPayload?> Target => _target;
    public IRxValue<AltitudePayload?> Altitude => _altitude;
    public IRxValue<VfrHudPayload?> VfrHud => _vfrHud;
    public IRxValue<HighresImuPayload?> Imu => _imu;
    public IRxValue<AttitudePayload?> Attitude => _attitude;

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

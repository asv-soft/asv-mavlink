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
    private readonly RxValue<GlobalPositionIntPayload> _globalPosition;
    private readonly RxValue<HomePositionPayload> _home;
    private readonly RxValue<PositionTargetGlobalIntPayload> _target;
    private readonly RxValue<AltitudePayload> _altitude;
    private readonly RxValue<VfrHudPayload> _vfrHud;
    private readonly RxValue<HighresImuPayload> _imu;
    private readonly RxValue<AttitudePayload> _attitude;


    public PositionClient(
        IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq,
        IScheduler? scheduler = null,
        ILogger? logger = null)
        : base("CTRL", connection, identity, seq,scheduler,logger)
    {
        _logger = logger ?? NullLogger.Instance;
        _target = new RxValue<PositionTargetGlobalIntPayload>().DisposeItWith(Disposable);
        InternalFilter<PositionTargetGlobalIntPacket>()
            .Select(p => p.Payload).Subscribe(_target).DisposeItWith(Disposable);
        _home = new RxValue<HomePositionPayload>().DisposeItWith(Disposable);
        InternalFilter<HomePositionPacket>()
            .Select(p => p.Payload).Subscribe(_home).DisposeItWith(Disposable);
        _globalPosition = new RxValue<GlobalPositionIntPayload>().DisposeItWith(Disposable);
        InternalFilter<GlobalPositionIntPacket>()
            .Select(p => p.Payload).Subscribe(_globalPosition).DisposeItWith(Disposable);
        _altitude = new RxValue<AltitudePayload>().DisposeItWith(Disposable);
        InternalFilter<AltitudePacket>()
            .Select(p => p.Payload).Subscribe(_altitude).DisposeItWith(Disposable);
        _vfrHud = new RxValue<VfrHudPayload>().DisposeItWith(Disposable);
        InternalFilter<VfrHudPacket>()
            .Select(p => p.Payload).Subscribe(_vfrHud).DisposeItWith(Disposable);
        _imu = new RxValue<HighresImuPayload>().DisposeItWith(Disposable);
        InternalFilter<HighresImuPacket>()
            .Select(p => p.Payload).Subscribe(_imu).DisposeItWith(Disposable);
        _attitude = new RxValue<AttitudePayload>().DisposeItWith(Disposable);
        InternalFilter<AttitudePacket>()
            .Select(p => p.Payload).Subscribe(_attitude).DisposeItWith(Disposable);
    }
    public IRxValue<GlobalPositionIntPayload> GlobalPosition => _globalPosition;
    public IRxValue<HomePositionPayload> Home => _home;
    public IRxValue<PositionTargetGlobalIntPayload> Target => _target;
    public IRxValue<AltitudePayload> Altitude => _altitude;
    public IRxValue<VfrHudPayload> VfrHud => _vfrHud;
    public IRxValue<HighresImuPayload> Imu => _imu;
    public IRxValue<AttitudePayload> Attitude => _attitude;

    public Task SetTargetGlobalInt(uint timeBootMs, MavFrame coordinateFrame, int latInt, int lonInt, float alt,
        float vx, float vy, float vz, float afx, float afy, float afz, float yaw,
        float yawRate, PositionTargetTypemask typeMask, CancellationToken cancel)
    {
        _logger.ZLogDebug($"{LogSend} {nameof(SetTargetGlobalInt)} ");
        return InternalSend<SetPositionTargetGlobalIntPacket>(p =>
        {
            p.Payload.TimeBootMs = timeBootMs;
            p.Payload.TargetSystem = Identity.TargetSystemId;
            p.Payload.TargetComponent = Identity.TargetComponentId;
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

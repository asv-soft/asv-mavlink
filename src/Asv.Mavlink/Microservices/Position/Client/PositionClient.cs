using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public class PositionClient : MavlinkMicroserviceClient, IPositionClient
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly RxValue<GlobalPositionIntPayload> _globalPosition;
    private readonly RxValue<HomePositionPayload> _home;
    private readonly RxValue<PositionTargetGlobalIntPayload> _target;
    private readonly RxValue<AltitudePayload> _altitude;
    private readonly RxValue<VfrHudPayload> _vfrHud;
    private readonly RxValue<HighresImuPayload> _imu;
    private readonly RxValue<AttitudePayload> _attitude;


    public PositionClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq)
        : base("CTRL", connection, identity, seq)
    {
        _target = new RxValue<PositionTargetGlobalIntPayload>().DisposeItWith(Disposable);
        InternalFilter<PositionTargetGlobalIntPacket>()
            .Select(_ => _.Payload).Subscribe(_target).DisposeItWith(Disposable);
        _home = new RxValue<HomePositionPayload>().DisposeItWith(Disposable);
        InternalFilter<HomePositionPacket>()
            .Select(_ => _.Payload).Subscribe(_home).DisposeItWith(Disposable);
        _globalPosition = new RxValue<GlobalPositionIntPayload>().DisposeItWith(Disposable);
        InternalFilter<GlobalPositionIntPacket>()
            .Select(_ => _.Payload).Subscribe(_globalPosition).DisposeItWith(Disposable);
        _altitude = new RxValue<AltitudePayload>().DisposeItWith(Disposable);
        InternalFilter<AltitudePacket>()
            .Select(_ => _.Payload).Subscribe(_altitude).DisposeItWith(Disposable);
        _vfrHud = new RxValue<VfrHudPayload>().DisposeItWith(Disposable);
        InternalFilter<VfrHudPacket>()
            .Select(_ => _.Payload).Subscribe(_vfrHud).DisposeItWith(Disposable);
        _imu = new RxValue<HighresImuPayload>().DisposeItWith(Disposable);
        InternalFilter<HighresImuPacket>()
            .Select(_ => _.Payload).Subscribe(_imu).DisposeItWith(Disposable);
        _attitude = new RxValue<AttitudePayload>().DisposeItWith(Disposable);
        InternalFilter<AttitudePacket>()
            .Select(_ => _.Payload).Subscribe(_attitude).DisposeItWith(Disposable);
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
        Logger.Debug($"{LogSend} {nameof(SetTargetGlobalInt)} ");
        return InternalSend<SetPositionTargetGlobalIntPacket>(_ =>
        {
            _.Payload.TimeBootMs = timeBootMs;
            _.Payload.TargetSystem = Identity.TargetSystemId;
            _.Payload.TargetComponent = Identity.TargetComponentId;
            _.Payload.CoordinateFrame = coordinateFrame;
            _.Payload.LatInt = latInt;
            _.Payload.LonInt = lonInt;
            _.Payload.Alt = alt;
            _.Payload.Vx = vx;
            _.Payload.Vy = vy;
            _.Payload.Vz = vz;
            _.Payload.Afx = afx;
            _.Payload.Afy = afy;
            _.Payload.Afz = afz;
            _.Payload.Yaw = yaw;
            _.Payload.YawRate = yawRate;
            _.Payload.TypeMask = typeMask;
        }, cancel);
    }

}

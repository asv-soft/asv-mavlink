#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.Vehicle;
using NLog;

namespace Asv.Mavlink;

public class PositionClientEx : DisposableOnceWithCancel, IPositionClientEx
{
    public static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly ICommandClient _commandClient;
    private readonly RxValue<GeoPoint?> _target;
    private readonly RxValue<GeoPoint?> _home;
    private readonly RxValue<GeoPoint> _current;
    private readonly RxValue<double> _homeDistance;
    private readonly RxValue<double> _targetDistance;
    private readonly RxValue<bool> _isArmed;
    private readonly RxValue<TimeSpan> _armedTime;
    private readonly RxValue<GeoPoint?> _roi;
    private long _lastArmedTime;
    private readonly RxValue<double> _altitudeAboveHome;
    private readonly RxValue<double> _pitch;
    private readonly RxValue<double> _pitchSpeed;
    private readonly RxValue<double> _roll;
    private readonly RxValue<double> _rollSpeed;
    private readonly RxValue<double> _yaw;
    private readonly RxValue<double> _yawSpeed;


    public PositionClientEx(IPositionClient client, IHeartbeatClient heartbeatClient, ICommandClient commandClient, IScheduler? scheduler = null)
    {
        _commandClient = commandClient;
        Base = client;
        
        _pitch = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(_ => GeoMath.RadiansToDegrees(_.Pitch)).Subscribe(_pitch).DisposeItWith(Disposable);
        _pitchSpeed = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(_ => (double)_.Pitchspeed).Subscribe(_pitchSpeed).DisposeItWith(Disposable);
        
        _roll = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(_ => GeoMath.RadiansToDegrees(_.Roll)).Subscribe(_roll).DisposeItWith(Disposable);
        _rollSpeed = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(_ => (double)_.Rollspeed).Subscribe(_rollSpeed).DisposeItWith(Disposable);
        
        _yaw = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(_ => GeoMath.RadiansToDegrees(_.Yaw)).Subscribe(_yaw).DisposeItWith(Disposable);
        _yawSpeed = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(_ => (double)_.Yawspeed).Subscribe(_yawSpeed).DisposeItWith(Disposable);
        
        _target = new RxValue<GeoPoint?>(null).DisposeItWith(Disposable);
        client.Target.Where(_ => _.CoordinateFrame == MavFrame.MavFrameGlobal)
            .Select(_ =>(GeoPoint?) new GeoPoint(_.LatInt / 10000000.0, _.LonInt / 10000000.0, _.Alt))
            .Subscribe(_target).DisposeItWith(Disposable);
        
        _home = new RxValue<GeoPoint?>(null).DisposeItWith(Disposable);
        client.Home.Select(_ => (GeoPoint?)new GeoPoint(_.Latitude / 10000000D, _.Longitude / 10000000D, _.Altitude / 1000D))
            .Subscribe(_home).DisposeItWith(Disposable);
        
        _current = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
        client.GlobalPosition.Select(_=>new GeoPoint(_.Lat / 10000000D, _.Lon / 10000000D, _.Alt / 1000D))
            .Subscribe(_current).DisposeItWith(Disposable);
        
        _homeDistance = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        _current.CombineLatest(_home)
            // ReSharper disable once PossibleInvalidOperationException
            .Select(_ => GeoMath.Distance(_.First, _.Second))
            .Subscribe(_homeDistance).DisposeItWith(Disposable);
        
        _targetDistance = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        _current.CombineLatest(_target)
            // ReSharper disable once PossibleInvalidOperationException
            .Select(_ => GeoMath.Distance(_.First, _.Second))
            .Subscribe(_targetDistance).DisposeItWith(Disposable);

        _isArmed = new RxValue<bool>(false).DisposeItWith(Disposable);
        _armedTime = new RxValue<TimeSpan>(TimeSpan.Zero).DisposeItWith(Disposable);
        heartbeatClient.RawHeartbeat.Select(_ => _.BaseMode.HasFlag(MavModeFlag.MavModeFlagSafetyArmed)).Subscribe(_isArmed).DisposeItWith(Disposable);
        var timer = scheduler == null ? Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)): Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1),scheduler);
        timer.Where(_=>IsArmed.Value).Subscribe(_ =>
        {
            var lastBin = Interlocked.Read(ref _lastArmedTime);
            if (lastBin == 0)
            {
                _armedTime.OnNext(TimeSpan.Zero);
                return;
            }
            var last = DateTime.FromBinary(lastBin);
            var now = DateTime.Now;
            var delay = (now - last);
            _armedTime.OnNext(delay);
        }).DisposeItWith(Disposable);
        _isArmed.DistinctUntilChanged().Where(_ => _isArmed.Value)
            .Subscribe(_ => Interlocked.Exchange(ref _lastArmedTime,DateTime.Now.ToBinary())).DisposeItWith(Disposable);
        
        _roi = new RxValue<GeoPoint?>(null).DisposeItWith(Disposable);
        
        _altitudeAboveHome = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.GlobalPosition.Select(_=>_.RelativeAlt/1000D).Subscribe(_altitudeAboveHome).DisposeItWith(Disposable);
    }

    public IPositionClient Base { get; }

    public IRxValue<double> Pitch => _pitch;
    public IRxValue<double> PitchSpeed => _pitchSpeed;
    public IRxValue<double> Roll => _roll;
    public IRxValue<double> RollSpeed => _rollSpeed;
    public IRxValue<double> Yaw => _yaw;
    public IRxValue<double> YawSpeed => _yawSpeed;

    public IRxValue<GeoPoint> Current => _current;
    public IRxValue<GeoPoint?> Target => _target;
    public IRxValue<GeoPoint?> Home => _home;
    public IRxValue<double> AltitudeAboveHome => _altitudeAboveHome;
    public IRxValue<double> HomeDistance => _homeDistance;
    public IRxValue<double> TargetDistance => _targetDistance;
    public IRxValue<bool> IsArmed => _isArmed;
    public IRxValue<TimeSpan> ArmedTime => _armedTime;
    public IRxValue<GeoPoint?> Roi => _roi;
    
    public async Task ArmDisarm(bool isArm, CancellationToken cancel)
    {
        if (_isArmed.Value == isArm) return;
        await _commandClient.CommandLongAndCheckResult(MavCmd.MavCmdComponentArmDisarm, isArm ? 1 : 0, float.NaN, float.NaN,
            float.NaN, float.NaN, float.NaN, float.NaN, cancel).ConfigureAwait(false);
        
    }

    public async Task SetRoi(GeoPoint location, CancellationToken cancel)
    {
        await _commandClient.CommandLongAndCheckResult(MavCmd.MavCmdDoSetRoi, (int)MavRoi.MavRoiLocation, 0, 0, 0, (float)location.Latitude, (float)location.Longitude, (float)location.Altitude,  CancellationToken.None).ConfigureAwait(false);
        _roi.OnNext(location);
    }

    public async Task ClearRoi(CancellationToken cancel)
    {
        await _commandClient.CommandLongAndCheckResult(MavCmd.MavCmdDoSetRoiNone, (int)MavRoi.MavRoiLocation, 0, 0, 0, 0, 0, 0, CancellationToken.None).ConfigureAwait(false);
        _roi.OnNext(null);
    }

    public Task SetTarget(GeoPoint point, CancellationToken cancel)
    {
        return Base.SetTargetGlobalInt(0, MavFrame.MavFrameGlobalInt, cancel, (int)(point.Latitude * 10000000),
            (int)(point.Longitude * 10000000), (float?)point.Altitude);
    }

    public Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        return _commandClient.CommandLongAndCheckResult(MavCmd.MavCmdNavTakeoff, 0, 0, 0, 0, 0, 0, (float)altInMeters, cancel);
    }

    public Task GetHomePosition(CancellationToken cancel)
    {
        return _commandClient.CommandLong(MavCmd.MavCmdGetHomePosition, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
            float.NaN, float.NaN, cancel);
    }
}
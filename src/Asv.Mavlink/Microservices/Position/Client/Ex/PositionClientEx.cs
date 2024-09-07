#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Asv.Mavlink.Vehicle;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class PositionClientEx : DisposableOnceWithCancel, IPositionClientEx
{
    public readonly ILogger _logger;
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
        client.Attitude.Select(p => GeoMath.RadiansToDegrees(p.Pitch)).Subscribe(_pitch).DisposeItWith(Disposable);
        _pitchSpeed = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(p => (double)p.Pitchspeed).Subscribe(_pitchSpeed).DisposeItWith(Disposable);
        
        _roll = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(p => GeoMath.RadiansToDegrees(p.Roll)).Subscribe(_roll).DisposeItWith(Disposable);
        _rollSpeed = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(p => (double)p.Rollspeed).Subscribe(_rollSpeed).DisposeItWith(Disposable);
        
        _yaw = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(p => GeoMath.RadiansToDegrees(p.Yaw)).Subscribe(_yaw).DisposeItWith(Disposable);
        _yawSpeed = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        client.Attitude.Select(p => (double)p.Yawspeed).Subscribe(_yawSpeed).DisposeItWith(Disposable);
        
        _target = new RxValue<GeoPoint?>(null).DisposeItWith(Disposable);
        client.Target.Where(p => p.CoordinateFrame == MavFrame.MavFrameGlobal)
            .Select(p =>(GeoPoint?) new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.LatInt)  , MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.LonInt), p.Alt))
            .Subscribe(_target).DisposeItWith(Disposable);
        
        _home = new RxValue<GeoPoint?>(null).DisposeItWith(Disposable);
        client.Home.Select(p => (GeoPoint?)new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Latitude), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Longitude), MavlinkTypesHelper.AltFromMmToDoubleMeter(p.Altitude)))
            .Subscribe(_home).DisposeItWith(Disposable);
        
        _current = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
        client.GlobalPosition.Select(p=>new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lat), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lon), MavlinkTypesHelper.AltFromMmToDoubleMeter(p.Alt)))
            .Subscribe(_current).DisposeItWith(Disposable);
        
        _homeDistance = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        _current.CombineLatest(_home)
            // ReSharper disable once PossibleInvalidOperationException
            .Select(t => GeoMath.Distance(t.First, t.Second))
            .Subscribe(_homeDistance).DisposeItWith(Disposable);
        
        _targetDistance = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        _current.CombineLatest(_target)
            // ReSharper disable once PossibleInvalidOperationException
            .Select(t => GeoMath.Distance(t.First, t.Second))
            .Subscribe(_targetDistance).DisposeItWith(Disposable);

        _isArmed = new RxValue<bool>(false).DisposeItWith(Disposable);
        _armedTime = new RxValue<TimeSpan>(TimeSpan.Zero).DisposeItWith(Disposable);
        heartbeatClient.RawHeartbeat.Select(p => p.BaseMode.HasFlag(MavModeFlag.MavModeFlagSafetyArmed)).Subscribe(_isArmed).DisposeItWith(Disposable);
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
        client.GlobalPosition.Select(p=>p.RelativeAlt/1000D).Subscribe(_altitudeAboveHome).DisposeItWith(Disposable);
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
        await _commandClient.CommandLongAndCheckResult(MavCmd.MavCmdComponentArmDisarm, isArm ? 1 : 0, 0, 0,
            0, 0, 0, 0, cancel).ConfigureAwait(false);
        
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
        return Base.SetTargetGlobalInt(0, MavFrame.MavFrameGlobalInt, cancel, MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(point.Latitude),
            MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(point.Longitude), (float?)point.Altitude);
    }

    public Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        return _commandClient.CommandLongAndCheckResult(MavCmd.MavCmdNavTakeoff, 0, 0, 0, 0, 0, 0, (float)altInMeters, cancel);
    }

    public Task QTakeOff(double altInMeters, CancellationToken cancel = default)
    {
        return _commandClient.CommandLongAndCheckResult(MavCmd.MavCmdNavVtolTakeoff, 0, 0, 0, 0, 0, 0, (float)altInMeters, cancel);
    }

    public Task GetHomePosition(CancellationToken cancel)
    {
        return _commandClient.CommandLong(MavCmd.MavCmdGetHomePosition, 0, 0, 0, 0, 0,
            0, 0, cancel);
    }

    public Task QLand(NavVtolLandOptions landOptions, double approachAlt, CancellationToken cancel)
    {
        return _commandClient.CommandLong(MavCmd.MavCmdNavVtolLand, (float)landOptions, 0, (float)approachAlt, 0, 0, 0, 0, cancel);
    }
}
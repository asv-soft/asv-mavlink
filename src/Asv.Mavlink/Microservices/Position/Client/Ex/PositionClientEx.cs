#nullable enable
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Asv.Mavlink.Vehicle;
using R3;

namespace Asv.Mavlink;

public class PositionClientEx : IPositionClientEx,IDisposable
{
    private static readonly TimeSpan ArmedTimeCheckInterval = TimeSpan.FromSeconds(1);
    private readonly ICommandClient _commandClient;
    private readonly RxValueBehaviour<GeoPoint?> _target;
    private readonly RxValueBehaviour<GeoPoint?> _home;
    private readonly RxValueBehaviour<GeoPoint> _current;
    private readonly RxValueBehaviour<double> _homeDistance;
    private readonly RxValueBehaviour<double> _targetDistance;
    private readonly RxValueBehaviour<bool> _isArmed;
    private readonly RxValueBehaviour<TimeSpan> _armedTime;
    private readonly RxValueBehaviour<GeoPoint?> _roi;
    private long _lastArmedTime;
    private readonly RxValueBehaviour<double> _altitudeAboveHome;
    private readonly RxValueBehaviour<double> _pitch;
    private readonly RxValueBehaviour<double> _pitchSpeed;
    private readonly RxValueBehaviour<double> _roll;
    private readonly RxValueBehaviour<double> _rollSpeed;
    private readonly RxValueBehaviour<double> _yaw;
    private readonly RxValueBehaviour<double> _yawSpeed;
    private readonly IDisposable _disposeIt;


    public PositionClientEx(
        IPositionClient client, 
        IHeartbeatClient heartbeatClient, 
        ICommandClient commandClient)
    {
        _commandClient = commandClient;
        Base = client;
        var builder = Disposable.CreateBuilder();
        _pitch = new RxValueBehaviour<double>(double.NaN).AddTo(ref builder);
        client.Attitude.Select(p => GeoMath.RadiansToDegrees(p.Pitch)).Subscribe(_pitch).AddTo(ref builder);
        _pitchSpeed = new RxValueBehaviour<double>(double.NaN).AddTo(ref builder);
        client.Attitude.Select(p => (double)p.Pitchspeed).Subscribe(_pitchSpeed).AddTo(ref builder);
        
        _roll = new RxValueBehaviour<double>(double.NaN).AddTo(ref builder);
        client.Attitude.Select(p => GeoMath.RadiansToDegrees(p.Roll)).Subscribe(_roll).AddTo(ref builder);
        _rollSpeed = new RxValueBehaviour<double>(double.NaN).AddTo(ref builder);
        client.Attitude.Select(p => (double)p.Rollspeed).Subscribe(_rollSpeed).AddTo(ref builder);
        
        _yaw = new RxValueBehaviour<double>(double.NaN).AddTo(ref builder);
        client.Attitude.Select(p => GeoMath.RadiansToDegrees(p.Yaw)).Subscribe(_yaw).AddTo(ref builder);
        _yawSpeed = new RxValueBehaviour<double>(double.NaN).AddTo(ref builder);
        client.Attitude.Select(p => (double)p.Yawspeed).Subscribe(_yawSpeed).AddTo(ref builder);
        
        _target = new RxValueBehaviour<GeoPoint?>(null).AddTo(ref builder);
        client.Target.Where(p => p.CoordinateFrame == MavFrame.MavFrameGlobal)
            .Select(p =>(GeoPoint?) new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.LatInt)  , MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.LonInt), p.Alt))
            .Subscribe(_target).AddTo(ref builder);
        
        _home = new RxValueBehaviour<GeoPoint?>(null).AddTo(ref builder);
        client.Home.Select(p => (GeoPoint?)new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Latitude), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Longitude), MavlinkTypesHelper.AltFromMmToDoubleMeter(p.Altitude)))
            .Subscribe(_home).AddTo(ref builder);
        
        _current = new RxValueBehaviour<GeoPoint>(GeoPoint.Zero).AddTo(ref builder);
        client.GlobalPosition.Select(p=>new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lat), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lon), MavlinkTypesHelper.AltFromMmToDoubleMeter(p.Alt)))
            .Subscribe(_current).AddTo(ref builder);
        
        _homeDistance = new RxValueBehaviour<double>(double.NaN).AddTo(ref builder);
        _current.CombineLatest(_home)
            // ReSharper disable once PossibleInvalidOperationException
            .Select(t => GeoMath.Distance(t.First, t.Second))
            .Subscribe(_homeDistance).AddTo(ref builder);
        
        _targetDistance = new RxValueBehaviour<double>(double.NaN).AddTo(ref builder);
        _current.CombineLatest(_target)
            // ReSharper disable once PossibleInvalidOperationException
            .Select(t => GeoMath.Distance(t.First, t.Second))
            .Subscribe(_targetDistance).AddTo(ref builder);

        _isArmed = new RxValueBehaviour<bool>(false).AddTo(ref builder);
        _armedTime = new RxValueBehaviour<TimeSpan>(TimeSpan.Zero).AddTo(ref builder);
        heartbeatClient.RawHeartbeat.Select(p => p.BaseMode.HasFlag(MavModeFlag.MavModeFlagSafetyArmed)).Subscribe(_isArmed).AddTo(ref builder);
        
        client.Core.TimeProvider.CreateTimer(CheckArmedTime,null,ArmedTimeCheckInterval, ArmedTimeCheckInterval).AddTo(ref builder);
        _isArmed.DistinctUntilChanged().Where(_ => _isArmed.Value)
            .Subscribe(_ => Interlocked.Exchange(ref _lastArmedTime,client.Core.TimeProvider.GetTimestamp())).AddTo(ref builder);
        
        _roi = new RxValueBehaviour<GeoPoint?>(null).AddTo(ref builder);
        
        _altitudeAboveHome = new RxValueBehaviour<double>(double.NaN).AddTo(ref builder);
        client.GlobalPosition.Select(p=>p.RelativeAlt/1000D).Subscribe(_altitudeAboveHome).AddTo(ref builder);
        _disposeIt = builder.Build();
    }

    public string Name => $"{Base.Name}Ex";
    
    private void CheckArmedTime(object? state)
    {
        var lastBin = Interlocked.Read(ref _lastArmedTime);
        if (lastBin == 0)
        {
            _armedTime.OnNext(TimeSpan.Zero);
            return;
        }
        _armedTime.OnNext(Base.Core.TimeProvider.GetElapsedTime(lastBin));
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

    public void Dispose()
    {
        _disposeIt.Dispose();
    }

    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
}
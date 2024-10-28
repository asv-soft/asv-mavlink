#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Asv.Mavlink.Vehicle;
using R3;

namespace Asv.Mavlink;

public sealed class PositionClientEx : IPositionClientEx,IDisposable, IAsyncDisposable
{
    private static readonly TimeSpan ArmedTimeCheckInterval = TimeSpan.FromSeconds(1);
    private readonly ICommandClient _commandClient;
    private long _lastArmedTime;
    private readonly IReadOnlyBindableReactiveProperty<double> _pitch;
    private readonly IReadOnlyBindableReactiveProperty<double> _pitchSpeed;
    private readonly IReadOnlyBindableReactiveProperty<double> _roll;
    private readonly IReadOnlyBindableReactiveProperty<double> _rollSpeed;
    private readonly IReadOnlyBindableReactiveProperty<double> _yaw;
    private readonly IReadOnlyBindableReactiveProperty<double> _yawSpeed;
    private readonly IReadOnlyBindableReactiveProperty<GeoPoint?> _target;
    private readonly IReadOnlyBindableReactiveProperty<GeoPoint?> _home;
    private readonly IReadOnlyBindableReactiveProperty<GeoPoint> _current;
    private readonly IReadOnlyBindableReactiveProperty<double> _homeDistance;
    private readonly IReadOnlyBindableReactiveProperty<double> _targetDistance;
    private readonly IReadOnlyBindableReactiveProperty<bool> _isArmed;
    private readonly BindableReactiveProperty<TimeSpan> _armedTime;
    private readonly ITimer _armedTimer;
    private readonly BindableReactiveProperty<GeoPoint?> _roi;
    private readonly IReadOnlyBindableReactiveProperty<double> _altitudeAboveHome;


    public PositionClientEx(
        IPositionClient client, 
        IHeartbeatClient heartbeatClient, 
        ICommandClient commandClient)
    {
        _commandClient = commandClient;
        Base = client;
        _pitch = client.Attitude.Select(p => GeoMath.RadiansToDegrees(p.Pitch))
            .ToReadOnlyBindableReactiveProperty(double.NaN);
        
        _pitchSpeed = client.Attitude
            .Select(p => (double)p.Pitchspeed)
            .ToReadOnlyBindableReactiveProperty(double.NaN);
        
        _roll = client.Attitude
            .Select(p => GeoMath.RadiansToDegrees(p.Roll))
            .ToReadOnlyBindableReactiveProperty(double.NaN);
        
        _rollSpeed = client.Attitude
            .Select(p => (double)p.Rollspeed)
            .ToReadOnlyBindableReactiveProperty(double.NaN);
        
        _yaw = client.Attitude
            .Select(p => GeoMath.RadiansToDegrees(p.Yaw))
            .ToReadOnlyBindableReactiveProperty(double.NaN);
        
        _yawSpeed = client.Attitude
            .Select(p => (double)p.Yawspeed)
            .ToReadOnlyBindableReactiveProperty(double.NaN);
        
        _target = client.Target
            .Where(p => p.CoordinateFrame == MavFrame.MavFrameGlobal)
            .Select(p =>(GeoPoint?) new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.LatInt)  , MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.LonInt), p.Alt))
            .ToReadOnlyBindableReactiveProperty();
        
        _home = client.Home
            .Select(p => (GeoPoint?)new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Latitude), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Longitude), MavlinkTypesHelper.AltFromMmToDoubleMeter(p.Altitude)))
            .ToReadOnlyBindableReactiveProperty();
        
        _current = client.GlobalPosition
            .Select(p=>new GeoPoint(MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lat), MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p.Lon), MavlinkTypesHelper.AltFromMmToDoubleMeter(p.Alt)))
            .ToReadOnlyBindableReactiveProperty(null);
        
        _homeDistance = _current.AsObservable()
            .CombineLatest(_home.AsObservable(), (c, h) => GeoMath.Distance(c, h))
            .ToReadOnlyBindableReactiveProperty(double.NaN);

        _targetDistance = _current.AsObservable()
            .CombineLatest(_target.AsObservable(), (c, h) => GeoMath.Distance(c, h))
            .ToReadOnlyBindableReactiveProperty();

        _isArmed = heartbeatClient.RawHeartbeat
            .Select(p => p.BaseMode.HasFlag(MavModeFlag.MavModeFlagSafetyArmed))
            .ToReadOnlyBindableReactiveProperty();
        
        _armedTime = new BindableReactiveProperty<TimeSpan>(TimeSpan.Zero);

        _armedTimer =
            client.Core.TimeProvider.CreateTimer(CheckArmedTime, null, ArmedTimeCheckInterval, ArmedTimeCheckInterval);
        _isArmed.AsObservable().Where(_ => _)
            .Subscribe(_ => Interlocked.Exchange(ref _lastArmedTime,client.Core.TimeProvider.GetTimestamp()));

        _roi = new BindableReactiveProperty<GeoPoint?>(null);
        
        _altitudeAboveHome = client.GlobalPosition
            .Select(p=>p.RelativeAlt/1000D)
            .ToReadOnlyBindableReactiveProperty(double.NaN);
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

    public IReadOnlyBindableReactiveProperty<double> Pitch => _pitch;
    public IReadOnlyBindableReactiveProperty<double> PitchSpeed => _pitchSpeed;
    public IReadOnlyBindableReactiveProperty<double> Roll => _roll;
    public IReadOnlyBindableReactiveProperty<double> RollSpeed => _rollSpeed;
    public IReadOnlyBindableReactiveProperty<double> Yaw => _yaw;
    public IReadOnlyBindableReactiveProperty<double> YawSpeed => _yawSpeed;

    public IReadOnlyBindableReactiveProperty<GeoPoint> Current => _current;
    public IReadOnlyBindableReactiveProperty<GeoPoint?> Target => _target;
    public IReadOnlyBindableReactiveProperty<GeoPoint?> Home => _home;
    public IReadOnlyBindableReactiveProperty<double> AltitudeAboveHome => _altitudeAboveHome;
    public IReadOnlyBindableReactiveProperty<double> HomeDistance => _homeDistance;
    public IReadOnlyBindableReactiveProperty<double> TargetDistance => _targetDistance;
    public IReadOnlyBindableReactiveProperty<bool> IsArmed => _isArmed;
    public IReadOnlyBindableReactiveProperty<TimeSpan> ArmedTime => _armedTime;
    public IReadOnlyBindableReactiveProperty<GeoPoint?> Roi => _roi;
    
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

    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }

    #region Disposable

    public void Dispose()
    {
        _pitch.Dispose();
        _pitchSpeed.Dispose();
        _roll.Dispose();
        _rollSpeed.Dispose();
        _yaw.Dispose();
        _yawSpeed.Dispose();
        _target.Dispose();
        _home.Dispose();
        _current.Dispose();
        _homeDistance.Dispose();
        _targetDistance.Dispose();
        _isArmed.Dispose();
        _armedTime.Dispose();
        _armedTimer.Dispose();
        _roi.Dispose();
        _altitudeAboveHome.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_pitch).ConfigureAwait(false);
        await CastAndDispose(_pitchSpeed).ConfigureAwait(false);
        await CastAndDispose(_roll).ConfigureAwait(false);
        await CastAndDispose(_rollSpeed).ConfigureAwait(false);
        await CastAndDispose(_yaw).ConfigureAwait(false);
        await CastAndDispose(_yawSpeed).ConfigureAwait(false);
        await CastAndDispose(_target).ConfigureAwait(false);
        await CastAndDispose(_home).ConfigureAwait(false);
        await CastAndDispose(_current).ConfigureAwait(false);
        await CastAndDispose(_homeDistance).ConfigureAwait(false);
        await CastAndDispose(_targetDistance).ConfigureAwait(false);
        await CastAndDispose(_isArmed).ConfigureAwait(false);
        await CastAndDispose(_armedTime).ConfigureAwait(false);
        await _armedTimer.DisposeAsync().ConfigureAwait(false);
        await CastAndDispose(_roi).ConfigureAwait(false);
        await CastAndDispose(_altitudeAboveHome).ConfigureAwait(false);

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
using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using R3;

namespace Asv.Mavlink;

public sealed class PositionClientEx : MavlinkMicroserviceClient, IPositionClientEx
{
    private static readonly TimeSpan ArmedTimeCheckInterval = TimeSpan.FromSeconds(1);
    private readonly ICommandClient _commandClient;
    private long _lastArmedTime;
    private readonly BindableReactiveProperty<TimeSpan> _armedTime;
    private readonly ITimer _armedTimer;
    private readonly BindableReactiveProperty<GeoPoint?> _roi;


    public PositionClientEx(
        IPositionClient client, 
        IHeartbeatClient heartbeatClient, 
        ICommandClient commandClient)
        :base(PositionHelper.MicroserviceName, client.Identity, client.Core)
    {
        _commandClient = commandClient;
        Base = client;
        Pitch = client.Attitude.Select(p => GeoMath.RadiansToDegrees(p?.Pitch ?? 0.0))
            .ToReadOnlyReactiveProperty(double.NaN);
        
        PitchSpeed = client.Attitude
            .Select(p => (double)(p?.Pitchspeed ?? 0.0))
            .ToReadOnlyReactiveProperty(double.NaN);
        
        Roll = client.Attitude
            .Select(p => GeoMath.RadiansToDegrees(p?.Roll ?? 0.0))
            .ToReadOnlyReactiveProperty(double.NaN);
        
        RollSpeed = client.Attitude
            .Select(p => (double)(p?.Rollspeed ?? 0.0))
            .ToReadOnlyReactiveProperty(double.NaN);
        
        Yaw = client.Attitude
            .Select(p => GeoMath.RadiansToDegrees(p?.Yaw ?? 0))
            .ToReadOnlyReactiveProperty();
        
        YawSpeed = client.Attitude
            .Select(p => (double)(p?.Yawspeed ?? 0.0))
            .ToReadOnlyReactiveProperty();
        
        Target = client.Target
            .Where(p => p?.CoordinateFrame == MavFrame.MavFrameGlobal)
            .Select(p => (GeoPoint?)
                new GeoPoint(
                    MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p?.LatInt ?? 0),
                    MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p?.LonInt ?? 0),
                    p?.Alt ?? 0
                )
            ).ToReadOnlyReactiveProperty();
        
        Home = client.Home
            .Select(p => (GeoPoint?)
                new GeoPoint(
                    MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p?.Latitude ?? 0), 
                    MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p?.Longitude?? 0), 
                    MavlinkTypesHelper.AltFromMmToDoubleMeter(p?.Altitude ?? 0)
                )
            ).ToReadOnlyReactiveProperty();
        
        Current = client.GlobalPosition
            .Select(p=>new GeoPoint(
                MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p?.Lat ?? 0), 
                MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(p?.Lon ?? 0), 
                MavlinkTypesHelper.AltFromMmToDoubleMeter(p?.Alt ?? 0)
                )
            )
            .ToReadOnlyReactiveProperty(null);
        
        HomeDistance = Current.AsObservable()
            .CombineLatest(
                Home.AsObservable(), 
                (c, h) => GeoMath.Distance(c, h)
            ).ToReadOnlyReactiveProperty(double.NaN);

        TargetDistance = Current.AsObservable()
            .CombineLatest(
                Target.AsObservable(), 
                (c, h) => GeoMath.Distance(c, h)
            ).ToReadOnlyReactiveProperty();

        IsArmed = heartbeatClient.RawHeartbeat
            .Select(p =>
                p?.BaseMode.HasFlag(MavModeFlag.MavModeFlagSafetyArmed) ?? false
            )
            .ToReadOnlyReactiveProperty();
        
        _armedTime = new BindableReactiveProperty<TimeSpan>(TimeSpan.Zero);

        _armedTimer =
            client.Core.TimeProvider.CreateTimer(
                CheckArmedTime, 
                null,
                ArmedTimeCheckInterval, 
                ArmedTimeCheckInterval
            );
        IsArmed.AsObservable().Where(_ => _)
            .Subscribe(_ => 
                Interlocked.Exchange(
                    ref _lastArmedTime,
                    client.Core.TimeProvider.GetTimestamp()
                )
            );

        _roi = new BindableReactiveProperty<GeoPoint?>(null);
        
        AltitudeAboveHome = client.GlobalPosition
            .Select(p=>(p?.RelativeAlt ?? 0)/1000D)
            .ToReadOnlyReactiveProperty(double.NaN);
        }

    public string TypeName => $"{Base.TypeName}Ex";
    
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

    public ReadOnlyReactiveProperty<double> Pitch { get; }

    public ReadOnlyReactiveProperty<double> PitchSpeed { get; }

    public ReadOnlyReactiveProperty<double> Roll { get; }

    public ReadOnlyReactiveProperty<double> RollSpeed { get; }

    public ReadOnlyReactiveProperty<double> Yaw { get; }

    public ReadOnlyReactiveProperty<double> YawSpeed { get; }

    public ReadOnlyReactiveProperty<GeoPoint> Current { get; }

    public ReadOnlyReactiveProperty<GeoPoint?> Target { get; }

    public ReadOnlyReactiveProperty<GeoPoint?> Home { get; }

    public ReadOnlyReactiveProperty<double> AltitudeAboveHome { get; }

    public ReadOnlyReactiveProperty<double> HomeDistance { get; }

    public ReadOnlyReactiveProperty<double> TargetDistance { get; }

    public ReadOnlyReactiveProperty<bool> IsArmed { get; }

    public ReadOnlyReactiveProperty<TimeSpan> ArmedTime => _armedTime;
    public ReadOnlyReactiveProperty<GeoPoint?> Roi => _roi;
    
    public async Task ArmDisarm(bool isArm, CancellationToken cancel)
    {
        if (IsArmed.CurrentValue == isArm)
        {
            return;
        }
        
        await _commandClient.CommandLongAndCheckResult(
            MavCmd.MavCmdComponentArmDisarm, 
            isArm ? 1 : 0, 
            0, 
            0,
            0, 
            0, 
            0, 
            0, 
            cancel)
            .ConfigureAwait(false);
    }

    public async Task SetRoi(GeoPoint location, CancellationToken cancel)
    {
        await _commandClient.CommandLongAndCheckResult(
            MavCmd.MavCmdDoSetRoi, 
            (int)MavRoi.MavRoiLocation, 
            0, 
            0, 
            0, 
            (float)location.Latitude, 
            (float)location.Longitude,
            (float)location.Altitude,  
            cancel).ConfigureAwait(false);
        _roi.OnNext(location);
    }

    public async Task ClearRoi(CancellationToken cancel)
    {
        await _commandClient.CommandLongAndCheckResult(
            MavCmd.MavCmdDoSetRoiNone, 
            (int)MavRoi.MavRoiLocation, 
            0, 
            0, 
            0, 
            0, 
            0, 
            0, 
            cancel).ConfigureAwait(false);
        _roi.OnNext(null);
    }

    public ValueTask SetTarget(GeoPoint point, CancellationToken cancel)
    {
        return Base.SetTargetGlobalInt(
            0,
            MavFrame.MavFrameGlobalInt, 
            cancel,
            MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(point.Latitude),
            MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(point.Longitude), 
            (float?)point.Altitude
        );
    }

    public Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        return _commandClient.CommandLongAndCheckResult(
            MavCmd.MavCmdNavTakeoff, 
            0, 
            0, 
            0, 
            0, 
            0, 
            0, 
            (float)altInMeters, 
            cancel
        );
    }

    public Task QTakeOff(double altInMeters, CancellationToken cancel = default)
    {
        return _commandClient.CommandLongAndCheckResult(
            MavCmd.MavCmdNavVtolTakeoff, 
            0, 
            0, 
            0, 
            0, 
            0, 
            0, 
            (float)altInMeters, 
            cancel
        );
    }

    public Task GetHomePosition(CancellationToken cancel)
    {
        return _commandClient.CommandLongAndCheckResult(
            MavCmd.MavCmdGetHomePosition, 
            0, 
            0, 
            0, 
            0, 
            0,
            0, 
            0,
            cancel
        );
    }

    public Task QLand(NavVtolLandOptions landOptions, double approachAlt, CancellationToken cancel)
    {
        return _commandClient.CommandLongAndCheckResult(
            MavCmd.MavCmdNavVtolLand, 
            (float)landOptions, 
            0, 
            (float)approachAlt, 
            0, 
            0, 
            0, 
            0, 
            cancel
        );
    }

    #region Disposable

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Pitch.Dispose();
            PitchSpeed.Dispose();
            Roll.Dispose();
            RollSpeed.Dispose();
            Yaw.Dispose();
            YawSpeed.Dispose();
            Target.Dispose();
            Home.Dispose();
            Current.Dispose();
            HomeDistance.Dispose();
            TargetDistance.Dispose();
            IsArmed.Dispose();
            _armedTime.Dispose();
            _armedTimer.Dispose();
            _roi.Dispose();
            AltitudeAboveHome.Dispose();
        }
        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(Pitch).ConfigureAwait(false);
        await CastAndDispose(PitchSpeed).ConfigureAwait(false);
        await CastAndDispose(Roll).ConfigureAwait(false);
        await CastAndDispose(RollSpeed).ConfigureAwait(false);
        await CastAndDispose(Yaw).ConfigureAwait(false);
        await CastAndDispose(YawSpeed).ConfigureAwait(false);
        await CastAndDispose(Target).ConfigureAwait(false);
        await CastAndDispose(Home).ConfigureAwait(false);
        await CastAndDispose(Current).ConfigureAwait(false);
        await CastAndDispose(HomeDistance).ConfigureAwait(false);
        await CastAndDispose(TargetDistance).ConfigureAwait(false);
        await CastAndDispose(IsArmed).ConfigureAwait(false);
        await CastAndDispose(_armedTime).ConfigureAwait(false);
        await _armedTimer.DisposeAsync().ConfigureAwait(false);
        await CastAndDispose(_roi).ConfigureAwait(false);
        await CastAndDispose(AltitudeAboveHome).ConfigureAwait(false);
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
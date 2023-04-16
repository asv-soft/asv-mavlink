using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

public interface IPositionClientEx
{
    
    IPositionClient Base { get; }
    
    IRxValue<double> Pitch { get; }
    IRxValue<double> PitchSpeed { get; }
    IRxValue<double> Roll { get; }
    IRxValue<double> RollSpeed { get; }
    IRxValue<double> Yaw { get; }
    IRxValue<double> YawSpeed { get; }
    IRxValue<GeoPoint> Current { get; }
    IRxValue<GeoPoint?> Target { get; }
    IRxValue<GeoPoint?> Home { get; }
    IRxValue<double> AltitudeAboveHome { get; }
    Task GetHomePosition(CancellationToken cancel = default);
    IRxValue<double> HomeDistance { get; }
    IRxValue<double> TargetDistance { get; }
    IRxValue<bool> IsArmed { get; }
    IRxValue<TimeSpan> ArmedTime { get; }
    Task ArmDisarm(bool isArm, CancellationToken cancel = default);
    IRxValue<GeoPoint?> Roi { get; }
    Task SetRoi(GeoPoint location, CancellationToken cancel = default);
    Task ClearRoi(CancellationToken cancel = default);
    Task SetTarget(GeoPoint point, CancellationToken none);
    Task TakeOff(double altInMeters, CancellationToken cancel=default);
}
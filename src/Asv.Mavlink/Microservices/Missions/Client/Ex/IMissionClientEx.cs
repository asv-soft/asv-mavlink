using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface IMissionClientEx
{
    IMissionClient Base { get; }
    Task<MissionItem[]> Download(CancellationToken cancel, Action<double> progress = null);
    Task Upload(CancellationToken cancel = default, Action<double> progress = null);
    MissionItem Create();
    void Remove(ushort index);
    Task ClearRemote(CancellationToken cancel = default);
    void ClearLocal();
    IObservable<IChangeSet<MissionItem, ushort>> MissionItems { get; }
    IRxValue<bool> IsSynced { get; }
    Task SetCurrent(ushort index, CancellationToken cancel = default);
    IRxValue<ushort> Current { get; }
    IRxValue<ushort> Reached { get; }
    IRxValue<double> AllMissionsDistance { get; }
}

public static class MissionClientExHelper
{
    public static MissionItem AddSplineMissionItem(this IMissionClientEx vehicle, GeoPoint point)
    {
        var item = vehicle.Create();
        item.Location.OnNext(point);
        item.AutoContinue.OnNext(true);
        item.Command.OnNext(MavCmd.MavCmdNavSplineWaypoint);
        item.Current.OnNext(false);
        item.Frame.OnNext(MavFrame.MavFrameGlobalInt);
        item.MissionType.OnNext(MavMissionType.MavMissionTypeMission);
        item.Param1.OnNext(0);
        item.Param2.OnNext(0);
        item.Param3.OnNext(0);
        item.Param4.OnNext(0);
        return item;
    }
    
    public static MissionItem AddNavMissionItem(this IMissionClientEx vehicle, GeoPoint point)
    {
        var item = vehicle.Create();
        item.Location.OnNext(point);
        item.AutoContinue.OnNext(true);
        item.Command.OnNext(MavCmd.MavCmdNavWaypoint);
        item.Current.OnNext(false);
        item.Frame.OnNext(MavFrame.MavFrameGlobalInt);
        item.MissionType.OnNext(MavMissionType.MavMissionTypeMission);
        item.Param1.OnNext(0);
        item.Param2.OnNext(0);
        item.Param3.OnNext(0);
        item.Param4.OnNext(0);
        return item;
    }
    
    public static MissionItem AddRoiMissionItem(this IMissionClientEx vehicle, GeoPoint point)
    {
        var item = vehicle.Create();
        item.Location.OnNext(point);
        item.AutoContinue.OnNext(true);
        item.Command.OnNext(MavCmd.MavCmdDoSetRoiLocation);
        item.Current.OnNext(false);
        item.Frame.OnNext(MavFrame.MavFrameGlobalInt);
        item.MissionType.OnNext(MavMissionType.MavMissionTypeMission);
        item.Param1.OnNext(0);
        item.Param2.OnNext(0);
        item.Param3.OnNext(0);
        item.Param4.OnNext(0);
        return item;
    }

    /// <summary>
    /// Change speed and/or throttle set points. The value persists until it is overridden or there is a mode change
    /// </summary>
    /// <param name="speed">Speed (-1 indicates no change, -2 indicates return to default vehicle speed)</param>
    /// <param name="speedType">Speed type of value set in param2 (such as airspeed, ground speed, and so on)</param>
    /// <param name="throttle">Throttle (-1 indicates no change, -2 indicates return to default vehicle throttle value)</param>
    /// <returns></returns>
    public static MissionItem SetVehicleSpeed(this IMissionClientEx vehicle, float speed, float speedType = 1, float throttle = -1)
    {
        var item = vehicle.Create();
        item.AutoContinue.OnNext(true);
        item.Command.OnNext(MavCmd.MavCmdDoChangeSpeed);
        item.Current.OnNext(false);
        item.Frame.OnNext(MavFrame.MavFrameGlobalInt);
        item.MissionType.OnNext(MavMissionType.MavMissionTypeMission);
        item.Param1.OnNext(speedType);
        item.Param2.OnNext(speed);
        item.Param3.OnNext(throttle);
        return item;
    }
}
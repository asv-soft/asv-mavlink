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
}
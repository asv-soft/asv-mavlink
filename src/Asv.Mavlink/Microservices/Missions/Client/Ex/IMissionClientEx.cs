using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Common;

using ObservableCollections;
using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents an extended interface for interacting with mission clients.
/// </summary>
public interface IMissionClientEx:IMavlinkMicroserviceClient
{
    /// <summary>
    /// Gets the base mission client.
    /// </summary>
    /// <value>
    /// The base mission client.
    /// </value>
    IMissionClient Base { get; }

    /// <summary>
    /// Starts the mission (send MAV_CMD_MISSION_START)
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="stopIndex"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task StartMission(ushort startIndex, ushort stopIndex, CancellationToken cancel = default);
    /// <summary>
    /// Downloads mission items asynchronously.
    /// </summary>
    /// <param name="cancel">The cancellation token to cancel the download.</param>
    /// <param name="progress">Optional. The callback to report progress during the download.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an array of downloaded mission items.</returns>
    /// <remarks>
    /// The <paramref name="cancel"/> parameter is used to cancel the download operation. If cancellation is requested, the task will be cancelled and an <see cref="OperationCanceledException"/> will be thrown.
    /// The <paramref name="progress"/> parameter can be used to track progress during the download. The callback will be called with a value between 0.0 and 1.0 indicating the progress
    /// percentage.
    /// </remarks>
    Task<MissionItem[]> Download(CancellationToken cancel, Action<double>? progress = null);

    /// <summary>
    /// Uploads a file to the server.
    /// </summary>
    /// <param name="cancel">A cancellation token that can be used to cancel the upload operation.</param>
    /// <param name="progress">An optional callback to track the progress of the upload. The callback receives a value between 0 and 1 representing the progress percentage.</param>
    /// <returns>A task representing the asynchronous upload operation.</returns>
    Task Upload(CancellationToken cancel = default, Action<double>? progress = null);

    /// <summary>
    /// Creates a new MissionItem object.
    /// </summary>
    /// <returns>A new instance of the MissionItem class.</returns>
    MissionItem Create();

    /// <summary>
    /// Removes an element at the specified index from the collection.
    /// </summary>
    /// <param name="index">The index of the element to remove.</param>
    void Remove(ushort index);

    /// <summary>
    /// Clears the remote system.
    /// </summary>
    /// <param name="cancel">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task ClearRemote(CancellationToken cancel = default);

    /// <summary>
    /// Clears the local data. </summary> <remarks>
    /// This method is used to clear any local data that has been stored on the client. </remarks>
    /// /
    void ClearLocal();

    /// <summary>
    /// This property represents an observable collection of MissionItems.
    /// </summary>
    /// <remarks>
    /// The MissionItems property is used to provide a stream of changes to the collection of MissionItems.
    /// Each change is represented as an IChangeSet, containing the updated MissionItem and an index.
    /// </remarks>
    /// <returns>
    /// An Observable<IChangeSet<MissionItem, ushort>> representing the stream of changes to the MissionItems collection.
    /// </returns>
    IReadOnlyObservableList<MissionItem> MissionItems { get; }

    /// <summary>
    /// Gets the interface for observing the synchronization state.
    /// </summary>
    /// <value>
    /// The interface for observing the synchronization state.
    /// </value>
    ReadOnlyReactiveProperty<bool> IsSynced { get; }

    /// <summary>
    /// Sets the current position to the specified index.
    /// </summary>
    /// <param name="index">The index position to set as the current position.</param>
    /// <param name="cancel">The cancellation token to be used for cancelling the operation. Optional.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method sets the current position to the specified index.
    /// If a cancellation token is provided and cancellation is requested, the operation will be cancelled.
    /// </remarks>
    Task SetCurrent(ushort index, CancellationToken cancel = default);

    /// Gets the current value of the property.
    /// @return The current value of the property as an ReadOnlyReactiveProperty of type ushort.
    /// /
    ReadOnlyReactiveProperty<ushort> Current { get; }

    /// <summary>
    /// Gets the value indicating if a particular condition has been reached.
    /// </summary>
    /// <value>
    /// The <see cref="ReadOnlyReactiveProperty{T}"/> representing the condition being reached. The value will be
    /// updated whenever the condition is reached.
    /// </value>
    ReadOnlyReactiveProperty<ushort> Reached { get; }

    /// <summary>
    /// Gets the total distance of all missions.
    /// </summary>
    /// <returns>An <see cref="ReadOnlyReactiveProperty"/> object representing the total distance.</returns>
    ReadOnlyReactiveProperty<double> AllMissionsDistance { get; }
}

/// <summary>
/// Helper class for creating different types of mission items.
/// </summary>
public static class MissionClientExHelper
{
    /// <summary>
    /// Adds a spline mission item to a vehicle's mission.
    /// </summary>
    /// <param name="vehicle">The vehicle's mission client</param>
    /// <param name="point">The geographic point for the mission item</param>
    /// <returns>The newly added spline mission item</returns>
    public static MissionItem AddSplineMissionItem(this IMissionClientEx vehicle, GeoPoint point)
    {
        var item = vehicle.Create();
        item.Location.Value =point;
        item.AutoContinue.Value =true;
        item.Command.Value =MavCmd.MavCmdNavSplineWaypoint;
        item.Current.Value =false;
        item.Frame.Value =MavFrame.MavFrameGlobalInt;
        item.MissionType.Value =MavMissionType.MavMissionTypeMission;
        item.Param1.Value =0;
        item.Param2.Value =0;
        item.Param3.Value =0;
        item.Param4.Value =0;
        return item;
    }

    /// <summary>
    /// Change speed and/or throttle set points. The value persists until it is overridden or there is a mode change
    /// </summary>
    /// <param name="vehicle">The vehicle's mission client</param>
    /// <param name="speed">Speed (-1 indicates no change, -2 indicates return to default vehicle speed)</param>
    /// <param name="speedType">Speed type of value set in param2 (such as airspeed, ground speed, and so on)</param>
    /// <param name="throttle">Throttle (-1 indicates no change, -2 indicates return to default vehicle throttle value)</param>
    /// <returns>MissionItem object</returns>
    public static MissionItem SetVehicleSpeed(
        this IMissionClientEx vehicle, 
        float speed, 
        float speedType = 1,
        float throttle = -1
    )
    {
        var item = vehicle.Create();
        item.AutoContinue.Value =true;
        item.Command.Value =MavCmd.MavCmdDoChangeSpeed;
        item.Current.Value =false;
        item.Frame.Value =MavFrame.MavFrameGlobalInt;
        item.MissionType.Value =MavMissionType.MavMissionTypeMission;
        item.Param1.Value =speedType;
        item.Param2.Value =speed;
        item.Param3.Value =throttle;
        return item;
    }

    //TODO: MissionItem is not working properly yet and needs to be finalized
    // public static MissionItem DoChangeAltitude(this IMissionClientEx vehicle, float altitude, MavFrame frame = MavFrame.MavFrameGlobalTerrainAlt)
    //  {
    //      var item = vehicle.Create();
    //      item.AutoContinue.OnNext(true);
    //      item.Command.OnNext(MavCmd.MavCmdDoChangeAltitude);
    //      item.Current.OnNext(false);
    //      item.Frame.OnNext(frame);
    //      item.MissionType.OnNext(MavMissionType.MavMissionTypeMission);
    //      item.Param1.OnNext(altitude);
    //      item.Param2.OnNext((float)frame);
    //      return item;
    //  }

    /// and creating the documentation based on those comments yourself.
    public static MissionItem AddNavMissionItem(
        this IMissionClientEx vehicle, 
        GeoPoint point,
        float holdTime = 0, 
        float acceptRadius = 0, 
        float passRadius = 0, 
        float yawAngle = float.NaN
    )
    {
        var missionItem = vehicle.Create();
        missionItem.Location.Value =point;
        missionItem.AutoContinue.Value =true;
        missionItem.Command.Value =MavCmd.MavCmdNavWaypoint;
        missionItem.Current.Value =false;
        missionItem.Frame.Value =MavFrame.MavFrameGlobalInt;
        missionItem.MissionType.Value =MavMissionType.MavMissionTypeMission;
        missionItem.Param1.Value =holdTime;
        missionItem.Param2.Value =acceptRadius;
        missionItem.Param3.Value =passRadius;
        missionItem.Param4.Value =yawAngle;
        return missionItem;
    }

    /// <summary>
    /// Adds a takeoff mission item to the mission. </summary> <param name="vehicle">The mission client.</param> <param name="point">The coordinates of the takeoff point.</param> <param name="pitch">The pitch angle (optional, default = 0).</param> <param name="yawAngle">The yaw angle (optional, default = NaN).</param> <returns>A mission item representing the takeoff mission.</returns>
    /// /
    public static MissionItem AddTakeOffMissionItem(
        this IMissionClientEx vehicle, 
        GeoPoint point, 
        float pitch = 0, 
        float yawAngle = float.NaN
    )
    {
        var missionItem = vehicle.Create();
        missionItem.Location.Value =point;
        missionItem.AutoContinue.Value =true;
        missionItem.Command.Value =MavCmd.MavCmdNavTakeoff;
        missionItem.Current.Value =false;
        missionItem.Frame.Value =MavFrame.MavFrameGlobalInt;
        missionItem.MissionType.Value =MavMissionType.MavMissionTypeMission;
        missionItem.Param1.Value =pitch;
        missionItem.Param2.Value =0.0f;
        missionItem.Param3.Value =0.0f;
        missionItem.Param4.Value =yawAngle;
        return missionItem;
    }

    /// <summary>
    /// Adds a land mission item to the mission list.
    /// </summary>
    /// <param name="vehicle">The vehicle that the mission item will be added to.</param>
    /// <param name="point">The geographical point of the land mission item.</param>
    /// <param name="abortAltitude">The altitude at which the land mission item can be aborted, default is 0.</param>
    /// <param name="landMode">The mode of precision landing, default is PrecisionLandModeDisabled.</param>
    /// <param name="yawAngle">The yaw angle for the land mission item, default is NaN.</param>
    /// <returns>The created land mission item.</returns>
    public static MissionItem AddLandMissionItem(
        this IMissionClientEx vehicle, 
        GeoPoint point,
        float abortAltitude = 0,
        PrecisionLandMode landMode = PrecisionLandMode.PrecisionLandModeDisabled, 
        float yawAngle = float.NaN
    )
    {
        var missionItem = vehicle.Create();
        missionItem.Location.Value =point;
        missionItem.AutoContinue.Value =true;
        missionItem.Command.Value =MavCmd.MavCmdNavLand;
        missionItem.Current.Value =false;
        missionItem.Frame.Value =MavFrame.MavFrameGlobalInt;
        missionItem.MissionType.Value =MavMissionType.MavMissionTypeMission;
        missionItem.Param1.Value =abortAltitude;
        missionItem.Param2.Value =(float)landMode;
        missionItem.Param3.Value =0.0f;
        missionItem.Param4.Value =yawAngle;
        return missionItem;
    }

    /// <summary>
    /// Adds a Region of Interest (ROI) mission item to the mission.
    /// </summary>
    /// <param name="vehicle">The mission client extension.</param>
    /// <param name="point">The geographical point target for the ROI.</param>
    /// <param name="roiMode">The mode for the ROI (optional, defaults to MavRoi.MavRoiLocation).</param>
    /// <param name="wpIndex">The waypoint index of the mission item (optional, defaults to 0).</param>
    /// <param name="roiIndex">The ROI index (optional, defaults to 0).</param>
    /// <returns>The newly created ROI mission item.</returns>
    public static MissionItem AddRoiMissionItem(
        this IMissionClientEx vehicle, 
        GeoPoint point, 
        MavRoi roiMode = MavRoi.MavRoiLocation, 
        float wpIndex = 0, 
        float roiIndex = 0
    )
    {
        var missionItem = vehicle.Create();
        missionItem.Location.Value =point;
        missionItem.AutoContinue.Value =true;
        missionItem.Command.Value =MavCmd.MavCmdDoSetRoi;
        missionItem.Current.Value =false;
        missionItem.Frame.Value =MavFrame.MavFrameGlobalInt;
        missionItem.MissionType.Value =MavMissionType.MavMissionTypeMission;
        missionItem.Param1.Value =(float)roiMode;
        missionItem.Param2.Value =wpIndex;
        missionItem.Param3.Value =roiIndex;
        missionItem.Param4.Value =0.0f;
        return missionItem;
    }
}
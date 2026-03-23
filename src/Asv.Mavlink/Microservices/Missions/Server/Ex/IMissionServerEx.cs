using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public enum MissionServerState
{
    /// <summary>
    /// No mission execution in progress.
    /// </summary>
    Idle,
    
    /// <summary>
    /// Mission execution is running.
    /// </summary>
    Running,
    
    /// <summary>
    /// Mission execution completed successfully.
    /// </summary>
    CompleteSuccess,
    
    /// <summary>
    /// Mission execution ended with error.
    /// </summary>
    CompleteError,
    
    /// <summary>
    /// Mission execution was canceled.
    /// </summary>
    Canceled
}

public interface IMissionServerEx : IMavlinkMicroserviceServer
{
    /// <summary>
    /// Underlying low-level mission server.
    /// </summary>
    IMissionServer Base { get; }

    /// <summary>
    /// Current mission item index.
    /// </summary>
    ReadOnlyReactiveProperty<ushort> Current { get; }
    
    /// <summary>
    /// Last reached mission item index.
    /// </summary>
    ReadOnlyReactiveProperty<ushort> Reached { get; }
    
    /// <summary>
    /// Mission storage.
    /// </summary>
    IReadOnlyObservableList<ServerMissionItem> Items { get; }
    
    /// <summary>
    /// Current mission execution state.
    /// </summary>
    ReadOnlyReactiveProperty<MissionServerState> State { get; }
    
    /// <summary>
    /// Commands that currently have registered handlers.
    /// </summary>
    IEnumerable<MavCmd> SupportedCommands { get; }
    
    /// <summary>
    /// Register/remove mission command handler.
    /// </summary>
    MissionTaskDelegate? this[MavCmd mavCmd] { set; }
    
    /// <summary>
    /// Returns items snapshot.
    /// </summary>
    /// <returns>Snapshot copy of mission items.</returns>
    ImmutableArray<ServerMissionItem> GetItemsSnapshot();
    
    /// <summary>
    /// Adds items to mission storage.
    /// </summary>
    /// <param name="items">Items to append.</param>
    void AddItems(IEnumerable<ServerMissionItem> items);
    
    /// <summary>
    /// Removes items from mission storage
    /// </summary>
    /// <param name="items">Items to remove.</param>
    void RemoveItems(IEnumerable<ServerMissionItem> items);
    
    /// <summary>
    /// Clears mission storage.
    /// </summary>
    void ClearItems();
   
    /// <summary>
    /// Changes current mission item index.
    /// </summary>
    /// <param name="index"> Mission item index.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    ValueTask ChangeCurrentMissionItem(ushort index);
    
    /// <summary>
    /// Starts mission execution.
    /// </summary>
    /// <param name="missionIndex">Start index (`0` by default).</param>
    void StartMission(ushort missionIndex = 0);
    
    /// <summary>
    /// Stops mission execution.
    /// </summary>
    /// <param name="cancel">Cancel token argument.</param>
    void StopMission(CancellationToken cancel);
}

/// <param name="item">Mission item to execute.</param>
/// <param name="cancel">Cancel token argument.</param>
public delegate Task MissionTaskDelegate(ServerMissionItem item, CancellationToken cancel);

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
    Idle,
    Running,
    CompleteSuccess,
    CompleteError,
    Canceled
}

public interface IMissionServerEx : IMavlinkMicroserviceServer
{
   
   IMissionServer Base { get; }
   ReadOnlyReactiveProperty<ushort> Current { get; }
   ReadOnlyReactiveProperty<ushort> Reached { get; }
   IReadOnlyObservableList<ServerMissionItem> Items { get; }
   void AddItems(IEnumerable<ServerMissionItem> items);
   void RemoveItems(IEnumerable<ServerMissionItem> items);
   void ClearItems();
   ImmutableArray<ServerMissionItem> GetItemsSnapshot();
   ReadOnlyReactiveProperty<MissionServerState> State { get; }
   ValueTask ChangeCurrentMissionItem(ushort index);
   void StartMission(ushort missionIndex = 0);
   void StopMission(CancellationToken cancel);
   MissionTaskDelegate? this[MavCmd mavCmd] { set; }
   IEnumerable<MavCmd> SupportedCommands { get; }
}

public delegate Task MissionTaskDelegate(ServerMissionItem item, CancellationToken cancel);

using System.Collections.Generic;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public static class Pv2MissionInterface
    {
        public const string InterfaceName = "MISSION";
        public const ushort InterfaceId = 4;

        #region Methods

        
        public static MethodInfo<SpanVoidType,Pv2MissionInfo> GetInfo = new(nameof(GetInfo), 0, InterfaceName, InterfaceId);
        public static MethodInfo<SpanVoidType, SpanVoidType> ClearAll = new(nameof(ClearAll), 1, InterfaceName, InterfaceId);
        public static MethodInfo<SpanPacketUnsignedIntegerType, Pv2MissionItem> ReadMissionItem = new(nameof(ReadMissionItem), 2, InterfaceName, InterfaceId);
        public static MethodInfo<Pv2MissionItem, Pv2MissionItem> WriteMissionItem = new(nameof(WriteMissionItem), 3, InterfaceName, InterfaceId);
        public static MethodInfo<SpanPacketUnsignedIntegerType, SpanVoidType> Start = new(nameof(Start), 4, InterfaceName, InterfaceId);
        public static MethodInfo<SpanVoidType, SpanVoidType> Stop = new(nameof(Stop), 5, InterfaceName, InterfaceId);

        #endregion

        #region Params

        

        public static IEnumerable<Pv2ParamType> Params
        {
            get { yield break; }
        }

        #endregion

        #region Factory

        public static Pv2MissionTrigger CreateTrigger(Pv2MissionTriggerType trigger)
        {
            return trigger switch
            {
                Pv2MissionTriggerType.UavWayPointReached => new Pv2UavWayPointReachedTrigger(),
                _ => Pv2UnknownMissionTrigger.Default
            };
        }

        public static Pv2MissionAction CreateAction(Pv2MissionActionType action)
        {
            return action switch
            {
                Pv2MissionActionType.StartRecord => new Pv2StartRecordAction(),
                Pv2MissionActionType.StopRecord => new Pv2StopRecordAction(),
                _ => Pv2UnknownAction.Default
            };
        }

        #endregion

    }
}

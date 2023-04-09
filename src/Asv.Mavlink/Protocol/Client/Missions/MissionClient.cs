using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.Common;
using Newtonsoft.Json;
using NLog;

namespace Asv.Mavlink
{

    public class MissionClientConfig
    {
        public int CommandTimeoutMs { get; set; } = 1000;
    }

    public class MissionClient : MavlinkMicroserviceClient, IMissionClient
    {
        private readonly MissionClientConfig _config;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly RxValue<ushort> _missionCurrent;
        private readonly RxValue<ushort> _missionReached;

        public MissionClient(IMavlinkV2Connection mavlink, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, MissionClientConfig config, IScheduler scheduler) : base(mavlink, identity, seq, "MISSION",scheduler)
        {
            _config = config;
            _missionCurrent = new RxValue<ushort>().DisposeItWith(Disposable);
            _missionReached = new RxValue<ushort>().DisposeItWith(Disposable);
            InternalFilter<MissionCurrentPacket>().Select(_ => _.Payload.Seq).Subscribe(_missionCurrent)
                .DisposeItWith(Disposable);
            InternalFilter<MissionItemReachedPacket>().Select(_ => _.Payload.Seq).Subscribe(_missionReached)
                .DisposeItWith(Disposable);
        }

        public async Task MissionSetCurrent(ushort missionItemsIndex, int attemptCount, CancellationToken cancel)
        {
            Logger.Debug($"{LogSend} Set current mission index to '{missionItemsIndex}' with {attemptCount} attempts");
            var result = await InternalCall<int, MissionSetCurrentPacket, MissionCurrentPacket>(_ =>
            {
                _.Payload.Seq = missionItemsIndex;
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
            }, null, _ => _.Payload.Seq, attemptCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            // Debug.Assert(result == missionItemsIndex);
        }

        public async Task<int> MissionRequestCount(int attemptCount, CancellationToken cancel)
        {
            Logger.Debug($"{LogSend} Begin request items count with {attemptCount} attempts");
            var result = await InternalCall<int, MissionRequestListPacket, MissionCountPacket>(_ =>
            {
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
            }, null, _ => _.Payload.Count, attemptCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            Logger.Info($"{LogRecv} Mission item count: {result} items");
            return result;
        }

        

        public IRxValue<ushort> MissionCurrent => _missionCurrent;

        public IRxValue<ushort> MissionReached => _missionReached;

        public IObservable<MissionRequestPayload> OnMissionRequest => InternalFilter<MissionRequestPacket>(_=>true).Select(_=>_.Payload);
        public IObservable<MissionAckPayload> OnMissionAck => InternalFilter<MissionAckPacket>(_ => true).Select(_ => _.Payload);

        public async Task<MissionItemIntPayload> MissionRequestItem(ushort index, int attemptCount, CancellationToken cancel)
        {
            // MISSION_REQUEST_INT
            Logger.Debug($"{LogSend} Begin request mission item {index} with {attemptCount} attempts");
            var result = await InternalCall<MissionItemIntPayload, MissionRequestIntPacket, MissionItemIntPacket>(_ =>
            {
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
                _.Payload.Seq = index;
                _.Payload.MissionType = MavMissionType.MavMissionTypeMission;
            }, null, _ => _.Payload, attemptCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            Logger.Info($"{LogRecv} Mission item {index} recieved: {JsonConvert.SerializeObject(result)}");
            return result;
        }



        public Task WriteMissionItem(ushort seq, MavFrame frame, MavCmd cmd, bool current, bool autoContinue, float param1, float param2, float param3,
            float param4, float x, float y, float z, MavMissionType missionType, int attemptCount, CancellationToken cancel)
        {
            Logger.Info($"{LogSend} Write mission item");

            // Ardupilot has custom implementation see =>  https://mavlink.io/en/services/mission.html#flight-plan-missions

            return InternalSend<MissionItemPacket>(_ =>
            {
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
                _.Payload.Seq = seq;
                _.Payload.Frame = frame;
                _.Payload.Command = cmd;
                _.Payload.Current = (byte)(current ? 1 : 0);
                _.Payload.Autocontinue = (byte)(autoContinue ? 1 : 0);
                _.Payload.Param1 = param1;
                _.Payload.Param2 = param2;
                _.Payload.Param3 = param3;
                _.Payload.Param4 = param4;
                _.Payload.X = x;
                _.Payload.Y = y;
                _.Payload.Z = z;
                _.Payload.MissionType = missionType;

            }, cancel);

        }

        public async Task ClearAll(int attemptCount, CancellationToken cancel)
        {
            Logger.Info($"{LogSend} Clear all mission items");
            var result = await InternalCall<MavMissionResult, MissionClearAllPacket, MissionAckPacket>(_ =>
            {
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
            }, null, _ => _.Payload.Type, attemptCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            CheckResult(result, "MissionClearAll");
        }

        public Task MissionSetCount(ushort count, CancellationToken cancel)
        {
            Logger.Debug($"{LogSend} Begin set items count '{count}'");
            return InternalSend<MissionCountPacket>(_ =>
            {
                _.Payload.Count = count;
                _.Payload.MissionType = MavMissionType.MavMissionTypeMission;
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
            },  cancel: cancel);
        }

        public Task WriteMissionItem(MissionItem missionItem,  CancellationToken cancel)
        {
            Logger.Info($"{LogSend} Write mission item {missionItem.Index}");

            // Ardupilot has custom implementation see =>  https://mavlink.io/en/services/mission.html#flight-plan-missions
            return InternalSend<MissionItemIntPacket>(_ =>
            {
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
                _.Payload.Seq = missionItem.Payload.Seq;
                _.Payload.Frame = missionItem.Payload.Frame;
                _.Payload.Command = missionItem.Payload.Command;
                _.Payload.Current = missionItem.Payload.Current;
                _.Payload.Autocontinue = missionItem.Payload.Autocontinue;
                _.Payload.Param1 = missionItem.Payload.Param1;
                _.Payload.Param2 = missionItem.Payload.Param2;
                _.Payload.Param3 = missionItem.Payload.Param3;
                _.Payload.Param4 = missionItem.Payload.Param4;
                _.Payload.X = missionItem.Payload.X;
                _.Payload.Y = missionItem.Payload.Y;
                _.Payload.Z = missionItem.Payload.Z;
                _.Payload.MissionType = missionItem.Payload.MissionType;
            }, cancel);
        }

        public void CheckResult(MavMissionResult result, string actionName)
        {
            if (result == MavMissionResult.MavMissionAccepted) return;
            throw new MavlinkException($"{LogSend} Error to {actionName}:{result:G}");

        }
    }
}

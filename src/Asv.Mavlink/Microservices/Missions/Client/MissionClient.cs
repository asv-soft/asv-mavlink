using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using ZLogger;

namespace Asv.Mavlink
{

    public class MissionClientConfig
    {
        public int CommandTimeoutMs { get; set; } = 1000;
        public int AttemptToCallCount { get; set; } = 3;
    }

    public class MissionClient : MavlinkMicroserviceClient, IMissionClient
    {
        private readonly MissionClientConfig _config;
        private readonly ILogger _logger;
        private readonly RxValue<ushort> _missionCurrent;
        private readonly RxValue<ushort> _missionReached;

        public MissionClient(IMavlinkV2Connection mavlink, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, MissionClientConfig config,
            IScheduler? scheduler = null,
            ILogger? logger = null) : base("MISSION",mavlink, identity, seq,scheduler,logger)
        {
            _logger = logger ?? NullLogger.Instance;
            _config = config;
            _missionCurrent = new RxValue<ushort>().DisposeItWith(Disposable);
            _missionReached = new RxValue<ushort>().DisposeItWith(Disposable);
            InternalFilter<MissionCurrentPacket>().Select(p => p.Payload.Seq).Subscribe(_missionCurrent)
                .DisposeItWith(Disposable);
            InternalFilter<MissionItemReachedPacket>().Select(p => p.Payload.Seq).Subscribe(_missionReached)
                .DisposeItWith(Disposable);
        }

        public async Task MissionSetCurrent(ushort missionItemsIndex, CancellationToken cancel)
        {
            _logger.ZLogDebug($"{LogSend} Set current mission index to '{missionItemsIndex}' with {_config.AttemptToCallCount} attempts");
            var result = await InternalCall<int, MissionSetCurrentPacket, MissionCurrentPacket>(p =>
            {
                p.Payload.Seq = missionItemsIndex;
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
            }, null, p => p.Payload.Seq, _config.AttemptToCallCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            // Debug.Assert(result == missionItemsIndex);
        }

        public async Task<int> MissionRequestCount(CancellationToken cancel)
        {
            _logger.ZLogDebug($"{LogSend} Begin request items count with {_config.AttemptToCallCount} attempts");
            var result = await InternalCall<int, MissionRequestListPacket, MissionCountPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
            }, null, p => p.Payload.Count, _config.AttemptToCallCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            _logger.ZLogInformation($"{LogRecv} Mission item count: {result} items");
            return result;
        }

        public new MavlinkClientIdentity Identity => base.Identity;
        public IRxValue<ushort> MissionCurrent => _missionCurrent;
        public IRxValue<ushort> MissionReached => _missionReached;
        public IObservable<MissionRequestPayload> OnMissionRequest => InternalFilter<MissionRequestPacket>(_=>true).Select(p=>p.Payload);
        public IObservable<MissionAckPayload> OnMissionAck => InternalFilter<MissionAckPacket>(_ => true).Select(p => p.Payload);
        public async Task<MissionItemIntPayload> MissionRequestItem(ushort index, CancellationToken cancel)
        {
            // MISSION_REQUEST_INT
            _logger.ZLogDebug($"{LogSend} Begin request mission item {index} with {_config.AttemptToCallCount} attempts");
            var result = await InternalCall<MissionItemIntPayload, MissionRequestIntPacket, MissionItemIntPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
                p.Payload.Seq = index;
                p.Payload.MissionType = MavMissionType.MavMissionTypeMission;
            }, null, p => p.Payload, _config.AttemptToCallCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            _logger.ZLogInformation($"{LogRecv} Mission item {index} recieved: {JsonConvert.SerializeObject(result)}");
            return result;
        }
        public Task WriteMissionItem(ushort seq, MavFrame frame, MavCmd cmd, bool current, bool autoContinue, float param1, float param2, float param3,
            float param4, float x, float y, float z, MavMissionType missionType, CancellationToken cancel)
        {
            _logger.ZLogInformation($"{LogSend} Write mission item");

            // Ardupilot has custom implementation see =>  https://mavlink.io/en/services/mission.html#flight-plan-missions

            return InternalSend<MissionItemPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
                p.Payload.Seq = seq;
                p.Payload.Frame = frame;
                p.Payload.Command = cmd;
                p.Payload.Current = (byte)(current ? 1 : 0);
                p.Payload.Autocontinue = (byte)(autoContinue ? 1 : 0);
                p.Payload.Param1 = param1;
                p.Payload.Param2 = param2;
                p.Payload.Param3 = param3;
                p.Payload.Param4 = param4;
                p.Payload.X = x;
                p.Payload.Y = y;
                p.Payload.Z = z;
                p.Payload.MissionType = missionType;

            }, cancel);

        }
        
        public async Task ClearAll(MavMissionType type = MavMissionType.MavMissionTypeAll,
            CancellationToken cancel = default)
        {
            _logger.ZLogInformation($"{LogSend} Clear all mission items");
            var result = await InternalCall<MavMissionResult, MissionClearAllPacket, MissionAckPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
                p.Payload.MissionType = type;
            }, null, p => p.Payload.Type, _config.AttemptToCallCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            CheckResult(result, "MissionClearAll");
        }

        public Task MissionSetCount(ushort count, CancellationToken cancel)
        {
            _logger.ZLogDebug($"{LogSend} Begin set items count '{count}'");
            return InternalSend<MissionCountPacket>(p =>
            {
                p.Payload.Count = count;
                p.Payload.MissionType = MavMissionType.MavMissionTypeMission;
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
            },  cancel: cancel);
        }

        public Task WriteMissionItem(MissionItem missionItem,  CancellationToken cancel)
        {
            _logger.ZLogInformation($"{LogSend} Write mission item {missionItem.Index}");

            // Ardupilot has custom implementation see =>  https://mavlink.io/en/services/mission.html#flight-plan-missions
            return InternalSend<MissionItemIntPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
                p.Payload.Seq = missionItem.Payload.Seq;
                p.Payload.Frame = missionItem.Payload.Frame;
                p.Payload.Command = missionItem.Payload.Command;
                p.Payload.Current = missionItem.Payload.Current;
                p.Payload.Autocontinue = missionItem.Payload.Autocontinue;
                p.Payload.Param1 = missionItem.Payload.Param1;
                p.Payload.Param2 = missionItem.Payload.Param2;
                p.Payload.Param3 = missionItem.Payload.Param3;
                p.Payload.Param4 = missionItem.Payload.Param4;
                p.Payload.X = missionItem.Payload.X;
                p.Payload.Y = missionItem.Payload.Y;
                p.Payload.Z = missionItem.Payload.Z;
                p.Payload.MissionType = missionItem.Payload.MissionType;
            }, cancel);
        }
        public Task WriteMissionIntItem(Action<MissionItemIntPayload> fillCallback, CancellationToken cancel = default)
        {
            _logger.ZLogInformation($"{LogSend} Write mission item");
            return InternalSend<MissionItemIntPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
                fillCallback(p.Payload);
            }, cancel);
        }

        private void CheckResult(MavMissionResult result, string actionName)
        {
            if (result == MavMissionResult.MavMissionAccepted) return;
            throw new MavlinkException($"{LogSend} Error to {actionName}:{result:G}");
        }
    }
}

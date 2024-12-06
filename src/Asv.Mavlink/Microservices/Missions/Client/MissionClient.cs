using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using R3;
using ZLogger;

namespace Asv.Mavlink
{
    public class MissionClientConfig
    {
        private int _commandTimeoutMs = 1000;
        private int _attemptToCallCount = 3;

        public int CommandTimeoutMs
        {
            get => _commandTimeoutMs;
            set => _commandTimeoutMs = value >= 0 
                ? value 
                : throw new ArgumentOutOfRangeException(nameof(CommandTimeoutMs));
        }
        public int AttemptToCallCount 
        { 
            get => _attemptToCallCount;
            set => _attemptToCallCount = value >= 0
                ? value 
                : throw new ArgumentOutOfRangeException(nameof(AttemptToCallCount));
        }
    }

    public sealed class MissionClient : MavlinkMicroserviceClient, IMissionClient
    {
        private readonly MissionClientConfig _config;
        private readonly ILogger _logger;

        public MissionClient(MavlinkClientIdentity identity, MissionClientConfig config, IMavlinkContext core) 
            : base(MissionClientHelper.MicroserviceName,identity, core)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = core.LoggerFactory.CreateLogger<MissionClient>();
            MissionCurrent = InternalFilter<MissionCurrentPacket>().Select(p => p.Payload.Seq)
                .ToReadOnlyReactiveProperty();
            MissionReached = InternalFilter<MissionItemReachedPacket>().Select(p => p.Payload.Seq)
                .ToReadOnlyReactiveProperty();
            OnMissionRequest = InternalFilter<MissionRequestPacket>(_=>true).Select(p=>p.Payload);
            OnMissionAck = InternalFilter<MissionAckPacket>(_ => true).Select(p => p.Payload);
        }

        public Task MissionSetCurrent(ushort missionItemsIndex, CancellationToken cancel)
        {
            _logger.ZLogDebug($"{Id} Set current mission index to '{missionItemsIndex}' with {_config.AttemptToCallCount} attempts");
            return InternalCall<int, MissionSetCurrentPacket, MissionCurrentPacket>(p =>
            {
                p.Payload.Seq = missionItemsIndex;
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
            }, null, p => p.Payload.Seq, _config.AttemptToCallCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel);
        }

        public async Task<int> MissionRequestCount(CancellationToken cancel)
        {
            _logger.ZLogDebug($"{Id} Begin request items count with {_config.AttemptToCallCount} attempts");
            var result = await InternalCall<int, MissionRequestListPacket, MissionCountPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
            }, null, p => p.Payload.Count, _config.AttemptToCallCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            _logger.ZLogInformation($"{Id} Mission item count: {result} items");
            return result;
        }

        public new MavlinkClientIdentity Identity => base.Identity;
        public ReadOnlyReactiveProperty<ushort> MissionCurrent { get; }
        public ReadOnlyReactiveProperty<ushort> MissionReached { get; }
        public Observable<MissionRequestPayload> OnMissionRequest { get; }
        public Observable<MissionAckPayload> OnMissionAck { get; }
        public async Task<MissionItemIntPayload> MissionRequestItem(ushort index, CancellationToken cancel)
        {
            // MISSION_REQUEST_INT
            _logger.ZLogDebug($"{Id} Begin request mission item {index} with {_config.AttemptToCallCount} attempts");
            var result = await InternalCall<MissionItemIntPayload, MissionRequestIntPacket, MissionItemIntPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
                p.Payload.Seq = index;
                p.Payload.MissionType = MavMissionType.MavMissionTypeMission;
            }, null, p => p.Payload, _config.AttemptToCallCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            _logger.ZLogInformation($"{Id} Mission item {index} recieved: {JsonConvert.SerializeObject(result)}");
            return result;
        }
        public ValueTask WriteMissionItem(ushort seq, MavFrame frame, MavCmd cmd, bool current, bool autoContinue, float param1, float param2, float param3,
            float param4, float x, float y, float z, MavMissionType missionType, CancellationToken cancel)
        {
            _logger.ZLogInformation($"{Id} Write mission item");

            // Ardupilot has custom implementation see =>  https://mavlink.io/en/services/mission.html#flight-plan-missions

            return InternalSend<MissionItemPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
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
            _logger.ZLogInformation($"{Id} Clear all mission items");
            var result = await InternalCall<MavMissionResult, MissionClearAllPacket, MissionAckPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
                p.Payload.MissionType = type;
            }, null, p => p.Payload.Type, _config.AttemptToCallCount, timeoutMs: _config.CommandTimeoutMs, cancel: cancel).ConfigureAwait(false);
            CheckResult(result, "MissionClearAll");
        }

        public ValueTask MissionSetCount(ushort count, CancellationToken cancel)
        {
            _logger.ZLogDebug($"{Id} Begin set items count '{count}'");
            return InternalSend<MissionCountPacket>(p =>
            {
                p.Payload.Count = count;
                p.Payload.MissionType = MavMissionType.MavMissionTypeMission;
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
            },  cancel: cancel);
        }

        public ValueTask WriteMissionItem(MissionItem missionItem,  CancellationToken cancel)
        {
            _logger.ZLogInformation($"{Id} Write mission item {missionItem.Index}");

            // Ardupilot has custom implementation see =>  https://mavlink.io/en/services/mission.html#flight-plan-missions
            return InternalSend<MissionItemIntPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
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
        public ValueTask WriteMissionIntItem(Action<MissionItemIntPayload> fillCallback, CancellationToken cancel = default)
        {
            _logger.ZLogInformation($"{Id} Write mission item");
            return InternalSend<MissionItemIntPacket>(p =>
            {
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
                fillCallback(p.Payload);
            }, cancel);
        }
        
        public ValueTask SendMissionAck
        (
            MavMissionResult result, 
            byte targetSystemId = 0, 
            byte targetComponentId = 0,
            MavMissionType? type = null,
            CancellationToken cancel = default
        )
        {
            return InternalSend<MissionAckPacket>(x =>
            {
                x.Payload.TargetSystem = targetSystemId;
                x.Payload.TargetComponent = targetComponentId;
                x.Payload.Type = result;
                if (type.HasValue)
                {
                    x.Payload.MissionType = type.Value;
                }
            }, cancel);
        }

        private void CheckResult(MavMissionResult result, string actionName)
        {
            if (result == MavMissionResult.MavMissionAccepted) return;
            throw new MavlinkException($"{Id} Error to {actionName}:{result:G}");
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                MissionCurrent.Dispose();
                MissionReached.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            await CastAndDispose(MissionCurrent).ConfigureAwait(false);
            await CastAndDispose(MissionReached).ConfigureAwait(false);

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
}

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink
{
    public interface ITelemetryClient
    {
        IRxValue<RadioStatusPayload> Radio { get; }
        IRxValue<SysStatusPayload> SystemStatus { get; }
        IRxValue<ExtendedSysStatePayload> ExtendedSystemState { get; }
        IRxValue<BatteryStatusPayload> Battery { get; }
        /// <summary>
        /// Request a data stream.
        /// DEPRECATED: Replaced by SET_MESSAGE_INTERVAL (2015-08).
        /// 
        /// </summary>
        /// <param name="streamId"></param>
        /// <param name="rateHz"></param>
        /// <param name="startStop"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task RequestDataStream(byte streamId, ushort rateHz, bool startStop, CancellationToken cancel = default);
        
    }

    public class TelemetryClient : MavlinkMicroserviceClient, ITelemetryClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly RxValue<RadioStatusPayload> _radio;
        private readonly RxValue<SysStatusPayload> _systemStatus;
        private readonly RxValue<ExtendedSysStatePayload> _extendedSystemState;
        private readonly RxValue<BatteryStatusPayload> _battery;

        public TelemetryClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, IScheduler scheduler)
            : base(connection, identity, seq, "RTT", scheduler)
        {
            _radio = new RxValue<RadioStatusPayload>().DisposeItWith(Disposable);
            InternalFilter<RadioStatusPacket>().Select(_=>_.Payload).Subscribe(_radio).DisposeItWith(Disposable);
            _systemStatus = new RxValue<SysStatusPayload>().DisposeItWith(Disposable);
            InternalFilter<SysStatusPacket>().Select(_ => _.Payload).Subscribe(_systemStatus).DisposeItWith(Disposable);
            _extendedSystemState = new RxValue<ExtendedSysStatePayload>().DisposeItWith(Disposable);
            InternalFilter<ExtendedSysStatePacket>().Select(_ => _.Payload).Subscribe(_extendedSystemState).DisposeItWith(Disposable);
            _battery = new RxValue<BatteryStatusPayload>().DisposeItWith(Disposable);
            InternalFilter<BatteryStatusPacket>().Select(_ => _.Payload).Subscribe(_battery).DisposeItWith(Disposable);
        }
        public IRxValue<RadioStatusPayload> Radio => _radio;
        public IRxValue<SysStatusPayload> SystemStatus => _systemStatus;
        public IRxValue<ExtendedSysStatePayload> ExtendedSystemState => _extendedSystemState;
        public IRxValue<BatteryStatusPayload> Battery => _battery;
        public Task RequestDataStream(byte streamId, ushort rateHz, bool startStop, CancellationToken cancel = default)
        {
            Logger.Debug($"{LogSend} {( startStop ? "Enable stream":"DisableStream")} with ID '{streamId}' and rate {rateHz} Hz");
            return InternalSend<RequestDataStreamPacket>(_ =>
            {
                _.Payload.TargetSystem = Identity.TargetSystemId;
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.ReqMessageRate = rateHz;
                _.Payload.StartStop = (byte)(startStop ? 1 : 0);
                _.Payload.ReqStreamId = streamId;
            }, cancel);
        }
    }
}

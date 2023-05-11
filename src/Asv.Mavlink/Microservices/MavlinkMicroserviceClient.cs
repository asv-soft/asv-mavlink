using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using NLog;

namespace Asv.Mavlink
{
    public class MavlinkClientIdentity
    {
        public byte SystemId { get; set; } = 254;
        public byte ComponentId { get; set; } = 254;
        public byte TargetSystemId { get; set; } = 13;
        public byte TargetComponentId { get; set; } = 13;

        public override string ToString()
        {
            return $"[Client:{SystemId}.{ComponentId}]=>[Server:{TargetSystemId}.{TargetComponentId}]";
        }
    }
    
    
    public abstract class MavlinkMicroserviceClient:DisposableOnceWithCancel
    {
        private readonly string _ifcLogName;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private string _locTargetName;
        private string _logLocalName;
        private string _logSend;
        private string _logRecv;

        protected MavlinkMicroserviceClient(string ifcLogName, IMavlinkV2Connection connection,
            MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, IScheduler scheduler)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            Sequence = seq ?? throw new ArgumentNullException(nameof(seq));
            _ifcLogName = ifcLogName;
            Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        protected string LogTargetName => _locTargetName ??= $"{Identity.TargetSystemId}:{Identity.TargetSystemId}";
        protected string LogLocalName => _logLocalName ??= $"{Identity.SystemId}:{Identity.ComponentId}";
        protected string LogSend => _logSend ??= $"[{LogLocalName}]=>[{LogTargetName}][{_ifcLogName}]:";
        protected string LogRecv => _logRecv ??= $"[{LogLocalName}]<=[{LogTargetName}][{_ifcLogName}]:";

        protected IObservable<TPacket> InternalFilter<TPacket>()
            where TPacket : IPacketV2<IPayload>, new()
        {
            var id = new TPacket().MessageId;
            return InternalFilteredVehiclePackets.Where(_=>_.MessageId == id).Cast<TPacket>();
        }

        protected IObservable<TPacket> InternalFilterFirstAsync<TPacket>(Func<TPacket, bool> filter)
            where TPacket : IPacketV2<IPayload>, new()
        {
            var id = new TPacket().MessageId;
            return InternalFilteredVehiclePackets.Where(_ => _.MessageId == id).Cast<TPacket>().FirstAsync(filter);
        }

        protected IObservable<TPacket> InternalFilter<TPacket>(Func<TPacket, bool> filter)
            where TPacket : IPacketV2<IPayload>, new()
        {
            var id = new TPacket().MessageId;
            return InternalFilteredVehiclePackets.Where(_ => _.MessageId == id).Cast<TPacket>().Where(filter);
        }

        protected IObservable<IPacketV2<IPayload>> InternalFilteredVehiclePackets => Connection.Where(FilterVehicle).ObserveOn(Scheduler).Publish().RefCount();
        private bool FilterVehicle(IPacketV2<IPayload> packetV2)
        {
            if (Identity.TargetSystemId != packetV2.SystemId) return false;
            if (Identity.TargetComponentId != packetV2.ComponentId) return false;
            return true;
        }
        protected MavlinkClientIdentity Identity { get; }

        protected IPacketSequenceCalculator Sequence { get; }

        protected IMavlinkV2Connection Connection { get; }

        protected IScheduler Scheduler { get; }


        protected TPacket InternalGeneratePacket<TPacket>()
            where TPacket : IPacketV2<IPayload>, new()
        {
            return new TPacket
            {
                ComponentId = Identity.ComponentId,
                SystemId = Identity.SystemId,
                Sequence = Sequence.GetNextSequenceNumber(),
            };
        }


        protected Task InternalSend<TPacketSend>(Action<TPacketSend> fillPacket, CancellationToken cancel = default)
            where TPacketSend : IPacketV2<IPayload>, new()
        {
            var packet = InternalGeneratePacket<TPacketSend>();
            fillPacket(packet);
            Logger.Trace($"{LogSend} send {packet.Name}");
            return Connection.Send(packet, cancel);
        }

        protected async Task<TAnswerPacket> InternalSendAndWaitAnswer<TAnswerPacket>(IPacketV2<IPayload> packet,
            CancellationToken cancel, Func<TAnswerPacket, bool> filter = null, int timeoutMs = 1000)
            where TAnswerPacket : IPacketV2<IPayload>, new()
        {
            var p = new TAnswerPacket();
            Logger.Trace($"{LogSend} call {p.Name}");
            using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
            linkedCancel.CancelAfter(timeoutMs);
            var tcs = new TaskCompletionSource<TAnswerPacket>();
            await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled());

            filter ??= (_ => true);
            using var subscribe = InternalFilterFirstAsync(filter).Subscribe(_=>tcs.TrySetResult(_));
            await Connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
            var result = await tcs.Task.ConfigureAwait(false);
            Logger.Trace($"{LogRecv} ok {packet.Name}<=={p.Name}");
            return result;
        }

        protected async Task<TResult> InternalCall<TResult,TPacketSend,TPacketRecv>(
            Action<TPacketSend> fillPacket, Func<TPacketRecv,bool> filter, Func<TPacketRecv,TResult> resultGetter, int attemptCount = 5,
            Action<TPacketSend,int> fillOnConfirmation = null, int timeoutMs = 1000,  CancellationToken cancel = default)
            where TPacketSend : IPacketV2<IPayload>, new()
            where TPacketRecv : IPacketV2<IPayload>, new()
        {
            var packet = InternalGeneratePacket<TPacketSend>();
            fillPacket(packet);
            byte currentAttempt = 0;
            TPacketRecv result = default;
            var name = packet.Name;
            while (currentAttempt < attemptCount)
            {
                if (currentAttempt != 0)
                {
                    // we need new packet sequence number for each attempt
                    packet.Sequence = Sequence.GetNextSequenceNumber();
                    fillOnConfirmation?.Invoke(packet, currentAttempt);
                    Logger.Warn($"{LogSend} replay {currentAttempt} {name}");
                }
                ++currentAttempt;
                try
                {
                    result = await InternalSendAndWaitAnswer(packet, cancel, filter, timeoutMs).ConfigureAwait(false);
                    break;
                }
                catch (OperationCanceledException)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        throw;
                    }
                }
            }

            if (result != null) return resultGetter(result);
            var msg = $"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'";
            Logger.Error(msg);
            throw new TimeoutException(msg);
        }
    }
}

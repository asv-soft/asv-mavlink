using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using NLog;

namespace Asv.Mavlink
{
    public abstract class MavlinkMicroserviceClient:DisposableOnceWithCancel
    {
        private readonly IMavlinkV2Connection _connection;
        private readonly MavlinkClientIdentity _identity;
        private readonly IPacketSequenceCalculator _seq;
        private readonly string _ifcLogName;
        private readonly IScheduler _scheduler;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private string _locTargetName;
        private string _logLocalName;
        private string _logSend;
        private string _logRecv;

        protected MavlinkMicroserviceClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, string ifcLogName, IScheduler scheduler)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _identity = identity ?? throw new ArgumentNullException(nameof(identity));
            _seq = seq ?? throw new ArgumentNullException(nameof(seq));
            _ifcLogName = ifcLogName;
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        protected string LogTargetName => _locTargetName ??= $"{Identity.TargetSystemId}:{Identity.TargetSystemId}";
        protected string LogLocalName => _logLocalName ??= $"{Identity.SystemId}:{Identity.ComponentId}";
        protected string LogSend => _logSend ??= $"[{LogLocalName}]=>[{LogTargetName}][{_ifcLogName}]:";
        protected string LogRecv => _logRecv ??= $"[{LogLocalName}]<=[{LogTargetName}][{_ifcLogName}]:";

        protected IObservable<TPacket> Filter<TPacket>()
            where TPacket : IPacketV2<IPayload>, new()
        {
            var id = new TPacket().MessageId;
            return FilterVehiclePackets.Where(_=>_.MessageId == id).Cast<TPacket>();
        }

        protected IObservable<TPacket> FilterFirstAsync<TPacket>(Func<TPacket, bool> filter)
            where TPacket : IPacketV2<IPayload>, new()
        {
            var id = new TPacket().MessageId;
            return FilterVehiclePackets.Where(_ => _.MessageId == id).Cast<TPacket>().FirstAsync(filter);
        }

        protected IObservable<TPacket> Filter<TPacket>(Func<TPacket, bool> filter)
            where TPacket : IPacketV2<IPayload>, new()
        {
            var id = new TPacket().MessageId;
            return FilterVehiclePackets.Where(_ => _.MessageId == id).Cast<TPacket>().Where(filter);
        }

        protected IObservable<IPacketV2<IPayload>> FilterVehiclePackets => _connection.Where(FilterVehicle).ObserveOn(_scheduler).Publish().RefCount();

        protected MavlinkClientIdentity Identity => _identity;
        protected IPacketSequenceCalculator Sequence => _seq;
        protected IMavlinkV2Connection MavlinkConnection => _connection;

        private bool FilterVehicle(IPacketV2<IPayload> packetV2)
        {
            if (_identity.TargetSystemId != packetV2.SystemId) return false;
            if (_identity.TargetComponentId != packetV2.ComponenId) return false;
            return true;
        }

        protected TPacket GeneratePacket<TPacket>()
            where TPacket : IPacketV2<IPayload>, new()
        {
            return new TPacket
            {
                ComponenId = _identity.ComponentId,
                SystemId = _identity.SystemId,
                Sequence = _seq.GetNextSequenceNumber(),
            };
        }


        protected Task InternalSend<TPacketSend>(Action<TPacketSend> fillPacket, CancellationToken cancel = default)
            where TPacketSend : IPacketV2<IPayload>, new()
        {
            var packet = GeneratePacket<TPacketSend>();
            fillPacket(packet);
            Logger.Trace($"{LogSend} send {packet.Name}");
            return _connection.Send(packet, cancel);
        }

        public async Task<TAnswerPacket> SendAndWaitAnswer<TAnswerPacket>(IPacketV2<IPayload> packet,
            CancellationToken cancel, Func<TAnswerPacket, bool> filter = null, int timeoutMs = 1000)
            where TAnswerPacket : IPacketV2<IPayload>, new()
        {
            var p = new TAnswerPacket();
            Logger.Trace($"{LogSend} call {p.Name}");
            using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
            linkedCancel.CancelAfter(timeoutMs);
            var tcs = new TaskCompletionSource<TAnswerPacket>();
            using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled());

            filter ??= (_ => true);
            using var subscribe = FilterFirstAsync(filter).Subscribe(_=>tcs.TrySetResult(_));
            await _connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
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
            var packet = GeneratePacket<TPacketSend>();
            fillPacket(packet);
            byte currentAttempt = 0;
            TPacketRecv result = default;
            var name = packet.Name;
            while (currentAttempt < attemptCount)
            {
                if (currentAttempt != 0)
                {
                    fillOnConfirmation?.Invoke(packet, currentAttempt);
                    Logger.Warn($"{LogSend} replay {currentAttempt} {name}");
                }
                ++currentAttempt;
                try
                {
                    result = await SendAndWaitAnswer(packet, cancel, filter, timeoutMs).ConfigureAwait(false);
                    break;
                }
                catch (TaskCanceledException)
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

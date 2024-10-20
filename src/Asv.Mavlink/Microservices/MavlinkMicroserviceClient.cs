#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink
{
    public class MavlinkClientIdentity
    {
        public MavlinkClientIdentity()
        {
            
        }

        public MavlinkClientIdentity(byte systemId, byte componentId, byte targetSystemId, byte targetComponentId)
        {
            SystemId = systemId;
            ComponentId = componentId;
            TargetSystemId = targetSystemId;
            TargetComponentId = targetComponentId;
        }
        
        public byte SystemId { get; set; } = 254;
        public byte ComponentId { get; set; } = 254;
        public byte TargetSystemId { get; set; } = 13;
        public byte TargetComponentId { get; set; } = 13;

        public override string ToString()
        {
            return $"[Client:{SystemId}.{ComponentId}]=>[Server:{TargetSystemId}.{TargetComponentId}]";
        }
    }
    
    
    public delegate bool FilterDelegate<TResult>(IPacketV2<IPayload> inputPacket, out TResult result);
    
    public abstract class MavlinkMicroserviceClient:DisposableOnceWithCancel
    {
        private readonly string _ifcLogName;
        private readonly ILogger _loggerBase;
        private string? _locTargetName;
        private string? _logLocalName;
        private string? _logSend;
        private string? _logRecv;

        protected MavlinkMicroserviceClient(string ifcLogName, IMavlinkV2Connection connection,
            MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq,
            TimeProvider? timeProvider = null,
            IScheduler? scheduler = null,
            ILoggerFactory? logFactory = null)
        {
            Scheduler = scheduler ?? System.Reactive.Concurrency.Scheduler.Default;
            logFactory??=NullLoggerFactory.Instance;
            _loggerBase = logFactory.CreateLogger<MavlinkMicroserviceClient>();
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            Sequence = seq ?? throw new ArgumentNullException(nameof(seq));
            TimeProvider = timeProvider ?? TimeProvider.System;
            _ifcLogName = ifcLogName;
            InternalFilteredVehiclePackets = Connection.Where(FilterVehicle).Publish().RefCount();
        }
        
        protected IScheduler Scheduler { get; } 
        protected IPacketSequenceCalculator Sequence { get; }
        protected TimeProvider TimeProvider { get; }
        protected IMavlinkV2Connection Connection { get; }

        protected string LogTargetName => _locTargetName ??= $"{Identity.TargetSystemId}:{Identity.TargetComponentId}";
        protected string LogLocalName => _logLocalName ??= $"{Identity.SystemId}:{Identity.ComponentId}";
        protected string LogSend => _logSend ??= $"[{LogLocalName}]=>[{LogTargetName}][{_ifcLogName}]:";
        protected string LogRecv => _logRecv ??= $"[{LogLocalName}]<=[{LogTargetName}][{_ifcLogName}]:";

        protected IObservable<TPacket> InternalFilter<TPacket>()
            where TPacket : IPacketV2<IPayload>, new()
        {
            var id = new TPacket().MessageId;
            return InternalFilteredVehiclePackets.Where(v=>v.MessageId == id).Cast<TPacket>();
        }

        protected IObservable<TPacket> InternalFilterFirstAsync<TPacket>(Func<TPacket, bool> filter)
            where TPacket : IPacketV2<IPayload>, new()
        {
            var id = new TPacket().MessageId;
            return InternalFilteredVehiclePackets.Where(v => v.MessageId == id).Cast<TPacket>().FirstAsync(filter);
        }

        protected IObservable<TPacket> InternalFilter<TPacket>(Func<TPacket, bool> filter)
            where TPacket : IPacketV2<IPayload>, new()
        {
            var id = new TPacket().MessageId;
            return InternalFilteredVehiclePackets.Where(v => v.MessageId == id).Cast<TPacket>().Where(filter);
        }

        protected IObservable<IPacketV2<IPayload>> InternalFilteredVehiclePackets { get; }
        private bool FilterVehicle(IPacketV2<IPayload> packetV2)
        {
            if (Identity.TargetSystemId != packetV2.SystemId) return false;
            if (Identity.TargetComponentId != packetV2.ComponentId) return false;
            return true;
        }
        public MavlinkClientIdentity Identity { get; }

        protected Task InternalSend<TPacketSend>(Action<TPacketSend> fillPacket, CancellationToken cancel = default)
            where TPacketSend : IPacketV2<IPayload>, new()
        {
            var packet = new TPacketSend();
            fillPacket(packet);
            _loggerBase.ZLogTrace($"{LogSend} send {packet.Name}");
            packet.Sequence = Sequence.GetNextSequenceNumber();
            packet.ComponentId = Identity.ComponentId;
            packet.SystemId = Identity.SystemId;
            return Connection.Send(packet, cancel);
        }

        protected async Task<TResult> InternalSendAndWaitAnswer<TResult>(IPacketV2<IPayload> packet,
            CancellationToken cancel, FilterDelegate<TResult> filterAndResultGetter, int timeoutMs = 1000)
        {
            ArgumentNullException.ThrowIfNull(filterAndResultGetter);
            _loggerBase.ZLogTrace($"{LogSend} call {packet.Name}");
            using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
            linkedCancel.CancelAfter(TimeSpan.FromMilliseconds(timeoutMs), TimeProvider);
            var tcs = new TaskCompletionSource<TResult>();
            await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);

            using var subscribe = InternalFilteredVehiclePackets.Subscribe(x=>
            {
                if (filterAndResultGetter(x, out var result) == true)
                {
                    tcs.TrySetResult(result);
                }
            });
            packet.Sequence = Sequence.GetNextSequenceNumber();
            packet.ComponentId = Identity.ComponentId;
            packet.SystemId = Identity.SystemId;
            await Connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
            var result = await tcs.Task.ConfigureAwait(false);
            _loggerBase.ZLogTrace($"{LogRecv} ok {packet.Name}<=={result}");
            return result;
        }
        
        protected async Task<TResult> InternalCall<TResult,TPacketSend>(
            Action<TPacketSend> fillPacket, FilterDelegate<TResult> filterAndResultGetter, int attemptCount = 5,
            Action<TPacketSend,int>? fillOnConfirmation = null, int timeoutMs = 1000,  CancellationToken cancel = default)
            where TPacketSend : IPacketV2<IPayload>, new()
        {
            var packet = new TPacketSend();
            fillPacket(packet);
            byte currentAttempt = 0;
            var name = packet.Name;
            while (currentAttempt < attemptCount)
            {
                if (currentAttempt != 0)
                {
                    fillOnConfirmation?.Invoke(packet, currentAttempt);
                    _loggerBase.ZLogWarning($"{LogSend} replay {currentAttempt} {name}");
                }
                ++currentAttempt;
                try
                {
                    return await InternalSendAndWaitAnswer(packet, cancel, filterAndResultGetter, timeoutMs).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        throw;
                    }
                }
            }
            _loggerBase.ZLogError($"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'");
            throw new TimeoutException($"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'");
        }
        
        
        protected async Task<TAnswerPacket> InternalSendAndWaitAnswer<TAnswerPacket>(IPacketV2<IPayload> packet,
            CancellationToken cancel, Func<TAnswerPacket, bool>? filter = null, int timeoutMs = 1000)
            where TAnswerPacket : IPacketV2<IPayload>, new()
        {
            var p = new TAnswerPacket();
            _loggerBase.ZLogTrace($"{LogSend} call {p.Name}");
            using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
            linkedCancel.CancelAfter(timeoutMs, TimeProvider);
            var tcs = new TaskCompletionSource<TAnswerPacket>();
            await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);

            filter ??= (_ => true);
            using var subscribe = InternalFilterFirstAsync(filter).Subscribe(v=>tcs.TrySetResult(v));
            packet.Sequence = Sequence.GetNextSequenceNumber();
            packet.ComponentId = Identity.ComponentId;
            packet.SystemId = Identity.SystemId;
            await Connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
            var result = await tcs.Task.ConfigureAwait(false);
            _loggerBase.ZLogTrace($"{LogRecv} ok {packet.Name}<=={p.Name}");
            return result;
        }

        protected async Task<TResult> InternalCall<TResult,TPacketSend,TPacketRecv>(
            Action<TPacketSend> fillPacket, Func<TPacketRecv,bool> filter, Func<TPacketRecv,TResult> resultGetter, int attemptCount = 5,
            Action<TPacketSend,int>? fillOnConfirmation = null, int timeoutMs = 500, CancellationToken cancel = default)
            where TPacketSend : IPacketV2<IPayload>, new()
            where TPacketRecv : IPacketV2<IPayload>, new()
        {
            var packet = new TPacketSend();
            fillPacket(packet);
            byte currentAttempt = 0;
            TPacketRecv? result = default;
            var name = packet.Name;
            while (currentAttempt < attemptCount)
            {
                if (currentAttempt != 0)
                {
                    fillOnConfirmation?.Invoke(packet, currentAttempt);
                    _loggerBase.ZLogWarning($"{LogSend} replay {currentAttempt} {name}");
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
            _loggerBase.ZLogError($"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'");
            throw new TimeoutException($"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'");
        }
    }
}

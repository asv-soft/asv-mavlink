#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink
{
    public interface IMavlinkMicroserviceClient
    {
        string Name { get; }
        MavlinkClientIdentity Identity { get; }
        ICoreServices Core { get; }
        Task Init(CancellationToken cancel = default);
    }
    
    public delegate bool FilterDelegate<TResult>(MavlinkMessage inputPacket, out TResult result);
    
    public abstract class MavlinkMicroserviceClient: AsyncDisposableWithCancel, IMavlinkMicroserviceClient,IDisposable, IAsyncDisposable
    {
        private readonly string _ifcLogName;
        private readonly ILogger _loggerBase;
        private string? _logSend;
        private string? _logRecv;
        private readonly Subject<MavlinkMessage> _internalFilteredVehiclePackets = new();
        private readonly IDisposable _sub1;

        protected MavlinkMicroserviceClient(string ifcLogName, MavlinkClientIdentity identity,
            ICoreServices core) 
        {
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            Core = core ?? throw new ArgumentNullException(nameof(core));
            _loggerBase = Core.Log.CreateLogger<MavlinkMicroserviceClient>();
            _ifcLogName = ifcLogName;
            _sub1 = core.Connection.OnRxMessage.Where(x =>
                {
                    if (x.Protocol.Id != MavlinkV2Protocol.Info.Id) return false;
                    return true;
                })
                .Cast<IProtocolMessage, MavlinkMessage>()
                .Where(x =>
                {
                    if (Identity.Target.SystemId != x.SystemId) return false;
                    if (Identity.Target.ComponentId != x.ComponentId) return false;
                    return true;
                }).Subscribe(_internalFilteredVehiclePackets.AsObserver());
        }

        public MavlinkClientIdentity Identity { get; }
        public ICoreServices Core { get; }
        public virtual Task Init(CancellationToken cancel = default)
        {
            return Task.CompletedTask;
        }

        public string Name => _ifcLogName;
        protected string LogSend => _logSend ??= $"{Identity.Self}=>{Identity.Target}[{_ifcLogName}]:";
        protected string LogRecv => _logRecv ??= $"{Identity.Self}=>{Identity.Target}[{_ifcLogName}]:";

        protected Observable<TMessage> InternalFilter<TMessage>()
            where TMessage : MavlinkMessage, new()
        {
            var id = new TMessage().Id;
            return InternalFilteredVehiclePackets.Where(id, (v,i)=>v.Id == i).Cast<MavlinkMessage,TMessage>();
        }
        protected Observable<TPacket> InternalFilter<TPacket>(Func<TPacket, bool> filter)
            where TPacket : MavlinkMessage, new()
        {
            return InternalFilter<TPacket>().Where(filter);
        }
        protected Observable<TMessage> InternalFilterFirstAsync<TMessage>(Func<TMessage, bool> filter)
            where TMessage : MavlinkMessage, new()
        {
            return InternalFilter(filter).Take(1);
        }

        protected Observable<MavlinkMessage> InternalFilteredVehiclePackets => _internalFilteredVehiclePackets;

        protected ValueTask InternalSend<TMessage>(Action<TMessage> fillPacket, CancellationToken cancel = default)
            where TMessage : MavlinkMessage, new()
        {
            ArgumentNullException.ThrowIfNull(fillPacket);
            cancel.ThrowIfCancellationRequested();
            var packet = new TMessage();
            fillPacket(packet);
            _loggerBase.ZLogTrace($"{LogSend} send {packet.Name}");
            packet.Sequence = Core.Sequence.GetNextSequenceNumber();
            packet.ComponentId = Identity.Self.ComponentId;
            packet.SystemId = Identity.Self.SystemId;
            return Core.Connection.Send(packet, cancel);
        }

        protected async Task<TResult> InternalSendAndWaitAnswer<TResult>(MavlinkMessage packet,
            FilterDelegate<TResult> filterAndResultGetter, int timeoutMs = 1000,
            CancellationToken cancel = default)
        {
            cancel.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(filterAndResultGetter);
            _loggerBase.ZLogTrace($"{LogSend} call {packet.Name}");
            using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
            linkedCancel.CancelAfter(TimeSpan.FromMilliseconds(timeoutMs), Core.TimeProvider);
            var tcs = new TaskCompletionSource<TResult>();
            await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);

            using var subscribe = InternalFilteredVehiclePackets.Subscribe(x=>
            {
                if (filterAndResultGetter(x, out var result) == true)
                {
                    tcs.TrySetResult(result);
                }
            });
            packet.Sequence = Core.Sequence.GetNextSequenceNumber();
            packet.ComponentId = Identity.Self.ComponentId;
            packet.SystemId = Identity.Self.SystemId;
            await Core.Connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
            var result = await tcs.Task.ConfigureAwait(false);
            _loggerBase.ZLogTrace($"{LogRecv} ok {packet.Name}<=={result}");
            return result;
        }
        
        protected async Task<TResult> InternalCall<TResult,TPacketSend>(
            Action<TPacketSend> fillPacket, FilterDelegate<TResult> filterAndResultGetter, int attemptCount = 5,
            Action<TPacketSend,int>? fillOnConfirmation = null, int timeoutMs = 1000,  CancellationToken cancel = default)
            where TPacketSend : MavlinkMessage, new()
        {
            cancel.ThrowIfCancellationRequested();
            var packet = new TPacketSend();
            fillPacket(packet);
            byte currentAttempt = 0;
            var name = packet.Name;
            bool IsRetryCondition() => currentAttempt < attemptCount;
            while (IsRetryCondition())
            {
                if (currentAttempt != 0)
                {
                    fillOnConfirmation?.Invoke(packet, currentAttempt);
                    _loggerBase.ZLogWarning($"{LogSend} replay {currentAttempt} {name}");
                }
                ++currentAttempt;
                try
                {
                    return await InternalSendAndWaitAnswer(packet, filterAndResultGetter, timeoutMs, cancel).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    if (IsRetryCondition())
                    {
                        continue;
                    }

                    cancel.ThrowIfCancellationRequested();
                }
            }
            _loggerBase.ZLogError($"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'");
            throw new TimeoutException($"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'");
        }
        
        
        protected async Task<TAnswerPacket> InternalSendAndWaitAnswer<TAnswerPacket>(MavlinkMessage packet,
            CancellationToken cancel, Func<TAnswerPacket, bool>? filter = null, int timeoutMs = 1000)
            where TAnswerPacket : MavlinkMessage, new()
        {
            cancel.ThrowIfCancellationRequested();
            var p = new TAnswerPacket();
            _loggerBase.ZLogTrace($"{LogSend} call {p.Name}");
            using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
            linkedCancel.CancelAfter(timeoutMs, Core.TimeProvider);
            var tcs = new TaskCompletionSource<TAnswerPacket>();
            await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);

            filter ??= (_ => true);
            using var subscribe = InternalFilterFirstAsync(filter).Subscribe(v=>tcs.TrySetResult(v));
            packet.Sequence = Core.Sequence.GetNextSequenceNumber();
            packet.ComponentId = Identity.Self.ComponentId;
            packet.SystemId = Identity.Self.SystemId;
            await Core.Connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
            var result = await tcs.Task.ConfigureAwait(false);
            _loggerBase.ZLogTrace($"{LogRecv} ok {packet.Name}<=={p.Name}");
            return result;
        }

        protected async Task<TResult> InternalCall<TResult,TPacketSend,TPacketRecv>(
            Action<TPacketSend> fillPacket, Func<TPacketRecv,bool>? filter, Func<TPacketRecv,TResult> resultGetter, int attemptCount = 5,
            Action<TPacketSend,int>? fillOnConfirmation = null, int timeoutMs = 1000, CancellationToken cancel = default)
            where TPacketSend : MavlinkMessage, new()
            where TPacketRecv : MavlinkMessage, new()
        {
            cancel.ThrowIfCancellationRequested();
            var packet = new TPacketSend();
            fillPacket(packet);
            byte currentAttempt = 0;
            TPacketRecv? result = default;
            var name = packet.Name;
            bool IsRetryCondition() => currentAttempt < attemptCount;
            while (IsRetryCondition())
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
                    if (IsRetryCondition())
                    {
                        continue;
                    }
                    
                    cancel.ThrowIfCancellationRequested();
                }
            }

            if (result != null) return resultGetter(result);
            _loggerBase.ZLogError($"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'");
            throw new TimeoutException($"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _internalFilteredVehiclePackets.Dispose();
                _sub1.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            await CastAndDispose(_internalFilteredVehiclePackets).ConfigureAwait(false);
            await CastAndDispose(_sub1).ConfigureAwait(false);

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
    }
}

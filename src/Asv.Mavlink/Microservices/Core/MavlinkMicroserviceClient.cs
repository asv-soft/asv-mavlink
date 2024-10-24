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
    public interface IMavlinkMicroserviceClient
    {
        MavlinkClientIdentity Identity { get; }
        ICoreServices Core { get; }
        Task Init(CancellationToken cancel = default);
    }
    
    public delegate bool FilterDelegate<TResult>(IPacketV2<IPayload> inputPacket, out TResult result);
    
    public abstract class MavlinkMicroserviceClient:IMavlinkMicroserviceClient,IDisposable
    {
        private readonly string _ifcLogName;
        private readonly ILogger _loggerBase;
        private string? _locTargetName;
        private string? _logLocalName;
        private string? _logSend;
        private string? _logRecv;
        private readonly CancellationTokenSource _disposeCancel = new();
        
        protected MavlinkMicroserviceClient(string ifcLogName, MavlinkClientIdentity identity,
            ICoreServices core) 
        {
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            Core = core ?? throw new ArgumentNullException(nameof(core));
            _loggerBase = Core.Log.CreateLogger<MavlinkMicroserviceClient>();
            _ifcLogName = ifcLogName;
            InternalFilteredVehiclePackets = core.Connection.Where(FilterVehicle).Publish().RefCount();
        }

        public MavlinkClientIdentity Identity { get; }
        public ICoreServices Core { get; }
        public virtual Task Init(CancellationToken cancel = default)
        {
            return Task.CompletedTask;
        }

        protected CancellationToken DisposeCancel => _disposeCancel.Token;

        protected string LogTargetName => _locTargetName ??= $"{Identity.Target.SystemId}:{Identity.Target.ComponentId}";
        protected string LogLocalName => _logLocalName ??= $"{Identity.Self.SystemId}:{Identity.Self.ComponentId}";
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
            if (Identity.Target.SystemId != packetV2.SystemId) return false;
            if (Identity.Target.ComponentId != packetV2.ComponentId) return false;
            return true;
        }
        protected Task InternalSend<TPacketSend>(Action<TPacketSend> fillPacket, CancellationToken cancel = default)
            where TPacketSend : IPacketV2<IPayload>, new()
        {
            var packet = new TPacketSend();
            fillPacket(packet);
            _loggerBase.ZLogTrace($"{LogSend} send {packet.Name}");
            packet.Sequence = Core.Sequence.GetNextSequenceNumber();
            packet.ComponentId = Identity.Self.ComponentId;
            packet.SystemId = Identity.Self.SystemId;
            return Core.Connection.Send(packet, cancel);
        }

        protected async Task<TResult> InternalSendAndWaitAnswer<TResult>(IPacketV2<IPayload> packet,
            CancellationToken cancel, FilterDelegate<TResult> filterAndResultGetter, int timeoutMs = 1000)
        {
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
            Action<TPacketSend> fillPacket, Func<TPacketRecv,bool> filter, Func<TPacketRecv,TResult> resultGetter, int attemptCount = 5,
            Action<TPacketSend,int>? fillOnConfirmation = null, int timeoutMs = 1000, CancellationToken cancel = default)
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

        public virtual void Dispose()
        {
            _disposeCancel.Cancel();
            _disposeCancel.Dispose();
        }
    }
}

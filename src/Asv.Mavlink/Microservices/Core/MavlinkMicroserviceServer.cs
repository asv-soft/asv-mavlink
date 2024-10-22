using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public interface IMavlinkMicroserviceServer
{
    ICoreServices Core { get; }

    MavlinkIdentity Identity { get; }
}

public abstract class MavlinkMicroserviceServer(string ifcLogName, MavlinkIdentity identity, ICoreServices core)
    : IMavlinkMicroserviceServer, IDisposable
{
    private readonly ILogger _loggerBase = core.Log.CreateLogger<MavlinkMicroserviceServer>();
    private string? _logLocalName;
    private string? _logSend;
    private string? _logRecv;
    private readonly CancellationTokenSource _disposeCancel = new();

    protected string LogLocalName => _logLocalName ??= $"{Identity.SystemId}:{Identity.ComponentId}";
    protected string LogSend => _logSend ??= $"[{LogLocalName}]=>[{ifcLogName}]:";
    protected string LogRecv => _logRecv ??= $"[{LogLocalName}]<=[{ifcLogName}]:";

    public ICoreServices Core { get; } = core;

    public MavlinkIdentity Identity { get; } = identity;

    protected CancellationToken DisposeCancel => _disposeCancel.Token;


    protected IObservable<TPacket> InternalFilter<TPacket>(Func<TPacket, byte> targetSystemGetter,
        Func<TPacket, byte> targetComponentGetter)
        where TPacket : IPacketV2<IPayload>, new()
    {
        return Core.Connection.Filter<TPacket>().Where(v =>
            Identity.SystemId == targetSystemGetter(v) && Identity.ComponentId == targetComponentGetter(v));
    }

    protected IObservable<TPacket> InternalFilterFirstAsync<TPacket>(Func<TPacket, byte> targetSystemGetter,
        Func<TPacket, byte> targetComponentGetter, Func<TPacket, bool> filter)
        where TPacket : IPacketV2<IPayload>, new()
    {
        return InternalFilter(targetSystemGetter, targetComponentGetter).FirstAsync(filter);
    }
    protected Task InternalSend(int messageId, Action<IPacketV2<IPayload>> fillPacket, CancellationToken cancel = default)
    {
        var pkt = Core.Connection.CreatePacketByMessageId(messageId);
        fillPacket(pkt);
        pkt.ComponentId = Identity.ComponentId;
        pkt.SystemId = Identity.SystemId;
        pkt.Sequence = Core.Sequence.GetNextSequenceNumber();
        return Core.Connection.Send(pkt, cancel);
    }
    
    protected Task InternalSend<TPacketSend>(Action<TPacketSend> fillPacket, CancellationToken cancel = default)
        where TPacketSend : IPacketV2<IPayload>, new()
    {
        var packet = new TPacketSend();
        fillPacket(packet);
        packet.ComponentId = Identity.ComponentId;
        packet.SystemId = Identity.SystemId;
        packet.Sequence = Core.Sequence.GetNextSequenceNumber();
        return Core.Connection.Send(packet, cancel);
    }

    protected async Task<TAnswerPacket> InternalSendAndWaitAnswer<TAnswerPacket>(IPacketV2<IPayload> packet,
        CancellationToken cancel, 
        Func<TAnswerPacket, byte> targetSystemGetter,
        Func<TAnswerPacket, byte> targetComponentGetter, 
        Func<TAnswerPacket, bool> filter = null,
        int timeoutMs = 1000)
        where TAnswerPacket : IPacketV2<IPayload>, new()
    {
        var p = new TAnswerPacket();
        _loggerBase.ZLogTrace($"{LogSend} call {p.Name}");
        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, _disposeCancel.Token);
        linkedCancel.CancelAfter(timeoutMs);
        var tcs = new TaskCompletionSource<TAnswerPacket>();
        using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);

        filter ??= (_ => true);
        using var subscribe = InternalFilterFirstAsync(targetSystemGetter,targetComponentGetter,filter).Subscribe(v => tcs.TrySetResult(v));
        packet.ComponentId = Identity.ComponentId;
        packet.SystemId = Identity.SystemId;
        packet.Sequence = Core.Sequence.GetNextSequenceNumber();
        await Core.Connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
        var result = await tcs.Task.ConfigureAwait(false);
        _loggerBase.ZLogTrace($"{LogRecv} ok {packet.Name}<=={p.Name}");
        return result;
    }

    protected async Task<TResult> InternalCall<TResult, TPacketSend, TPacketRecv>(
        Action<TPacketSend> fillPacket, 
        Func<TPacketRecv, byte> targetSystemGetter,
        Func<TPacketRecv, byte> targetComponentGetter,
        Func<TPacketRecv, bool> filter, 
        Func<TPacketRecv, TResult> resultGetter,
        int attemptCount = 5,
        Action<TPacketSend, int> fillOnConfirmation = null, int timeoutMs = 1000, CancellationToken cancel = default)
        where TPacketSend : IPacketV2<IPayload>, new()
        where TPacketRecv : IPacketV2<IPayload>, new()
    {
        var packet = new TPacketSend();
        fillPacket(packet);
        byte currentAttempt = 0;
        TPacketRecv result = default;
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
                result = await InternalSendAndWaitAnswer(packet, cancel,targetSystemGetter, targetComponentGetter,filter, timeoutMs).ConfigureAwait(false);
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
        _loggerBase.ZLogError($"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'");
        throw new TimeoutException($"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'");
    }

    public virtual void Dispose()
    {
        _disposeCancel.Cancel(false);
        _disposeCancel.Dispose();
    }
}
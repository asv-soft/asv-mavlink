using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public interface IMavlinkMicroserviceServer
{
    ICoreServices Core { get; }

    MavlinkIdentity Identity { get; }
}

public abstract class MavlinkMicroserviceServer : IMavlinkMicroserviceServer, IDisposable, IAsyncDisposable
{
    private readonly ILogger _loggerBase;
    private string? _logLocalName;
    private string? _logSend;
    private string? _logRecv;
    private readonly CancellationTokenSource _disposeCancel = new();
    private readonly string _ifcLogName;

    protected MavlinkMicroserviceServer(string ifcLogName, MavlinkIdentity identity, ICoreServices core)
    {
        ArgumentNullException.ThrowIfNull(identity);
        ArgumentNullException.ThrowIfNull(core);
        ArgumentException.ThrowIfNullOrWhiteSpace(ifcLogName);
        _ifcLogName = ifcLogName;
        _loggerBase = core.Log.CreateLogger<MavlinkMicroserviceServer>();
        Core = core;
        Identity = identity;
    }

    protected string LogLocalName => _logLocalName ??= $"{Identity.SystemId}:{Identity.ComponentId}";
    protected string LogSend => _logSend ??= $"[{LogLocalName}]=>[{_ifcLogName}]:";
    protected string LogRecv => _logRecv ??= $"[{LogLocalName}]<=[{_ifcLogName}]:";

    public ICoreServices Core { get; }

    public MavlinkIdentity Identity { get; }

    protected CancellationToken DisposeCancel => _disposeCancel.Token;

    protected Observable<TPacket> InternalFilter<TPacket>(Func<TPacket, byte> targetSystemGetter,
        Func<TPacket, byte> targetComponentGetter)
        where TPacket : MavlinkMessage, new()
    {
        return Core.Connection.RxFilter<TPacket, ushort>().Where((targetSystemGetter,targetComponentGetter),(v, f) =>
        {
            var sys = f.targetSystemGetter(v);
            var com = f.targetComponentGetter(v);
            return (Identity.SystemId == sys || sys == 0) && (Identity.ComponentId == com || com == 0);
        });
    }

    protected Observable<TPacket> InternalFilterFirstAsync<TPacket>(Func<TPacket, byte> targetSystemGetter,
        Func<TPacket, byte> targetComponentGetter, Func<TPacket, bool> filter)
        where TPacket : MavlinkMessage, new()
    {
        return InternalFilter(targetSystemGetter, targetComponentGetter).Where(filter).Take(1);
    }
    protected ValueTask InternalSend(int messageId, Action<MavlinkV2Message<IPayload>> fillPacket, CancellationToken cancel = default)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        var pkt = (MavlinkV2Message<IPayload>)MavlinkV2MessageFactory.Instance.Create((ushort)messageId);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        fillPacket(pkt ?? throw new InvalidOperationException($"Packet {messageId} not found"));
        pkt.ComponentId = Identity.ComponentId;
        pkt.SystemId = Identity.SystemId;
        pkt.Sequence = Core.Sequence.GetNextSequenceNumber();
        return Core.Connection.Send(pkt, cancel);
    }
    
    protected ValueTask InternalSend<TPacketSend>(Action<TPacketSend> fillPacket, CancellationToken cancel = default)
        where TPacketSend : MavlinkMessage, new()
    {
        var packet = new TPacketSend();
        fillPacket(packet);
        packet.ComponentId = Identity.ComponentId;
        packet.SystemId = Identity.SystemId;
        packet.Sequence = Core.Sequence.GetNextSequenceNumber();
        return Core.Connection.Send(packet, cancel);
    }

    protected async Task<TAnswerPacket> InternalSendAndWaitAnswer<TAnswerPacket>(MavlinkMessage packet,
        CancellationToken cancel, 
        Func<TAnswerPacket, byte> targetSystemGetter,
        Func<TAnswerPacket, byte> targetComponentGetter, 
        Func<TAnswerPacket, bool>? filter = null,
        int timeoutMs = 1000)
        where TAnswerPacket : MavlinkMessage, new()
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
        Action<TPacketSend, int>? fillOnConfirmation = null, int timeoutMs = 1000, CancellationToken cancel = default)
        where TPacketSend : MavlinkMessage, new()
        where TPacketRecv : MavlinkMessage, new()
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

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _disposeCancel.Cancel(false);
            _disposeCancel.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual ValueTask DisposeAsyncCore()
    {
        _disposeCancel.Cancel(false);
        _disposeCancel.Dispose();
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }
}
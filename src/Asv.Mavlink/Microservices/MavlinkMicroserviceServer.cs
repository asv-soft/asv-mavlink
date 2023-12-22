using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using NLog;

namespace Asv.Mavlink;


public abstract class MavlinkMicroserviceServer : DisposableOnceWithCancel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly string _ifcLogName;
    private string _logLocalName;
    private string _logSend;
    private string _logRecv;

    protected MavlinkMicroserviceServer(string ifcLogName, IMavlinkV2Connection connection,
        MavlinkIdentity identity,
        IPacketSequenceCalculator seq, IScheduler rxScheduler)
    {
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        Identity = identity;
        PacketSequence = seq ?? throw new ArgumentNullException(nameof(seq));
        _ifcLogName = ifcLogName;
        Scheduler = rxScheduler ?? throw new ArgumentNullException(nameof(rxScheduler));
    }

    protected string LogLocalName => _logLocalName ??= $"{Identity.SystemId}:{Identity.ComponentId}";
    protected string LogSend => _logSend ??= $"[{LogLocalName}]=>[{_ifcLogName}]:";
    protected string LogRecv => _logRecv ??= $"[{LogLocalName}]<=[{_ifcLogName}]:";

    protected IMavlinkV2Connection Connection { get; }

    protected MavlinkIdentity Identity { get; }

    protected IPacketSequenceCalculator PacketSequence { get; }

    protected IScheduler Scheduler { get; }

    protected IObservable<TPacket> InternalFilter<TPacket>(Func<TPacket, byte> targetSystemGetter,
        Func<TPacket, byte> targetComponentGetter)
        where TPacket : IPacketV2<IPayload>, new()
    {
        return Connection.Filter<TPacket>().Where(_ =>
            Identity.SystemId == targetSystemGetter(_) && Identity.ComponentId == targetComponentGetter(_));
    }

    protected IObservable<TPacket> InternalFilterFirstAsync<TPacket>(Func<TPacket, byte> targetSystemGetter,
        Func<TPacket, byte> targetComponentGetter, Func<TPacket, bool> filter)
        where TPacket : IPacketV2<IPayload>, new()
    {
        return InternalFilter(targetSystemGetter, targetComponentGetter).FirstAsync(filter);
    }

    /*protected TPacket InternalGeneratePacket<TPacket>()
        where TPacket : IPacketV2<IPayload>, new()
    {
        return new TPacket
        {
            ComponentId = Identity.ComponentId,
            SystemId = Identity.SystemId,
        };
    }
    
    protected IPacketV2<IPayload> InternalGeneratePacket(int messageId)
    {
        var pkt = Connection.CreatePacketByMessageId(messageId);
        pkt.ComponentId = Identity.ComponentId;
        pkt.SystemId = Identity.SystemId;
        return pkt;
    }*/

    
    protected Task InternalSend(int messageId, Action<IPacketV2<IPayload>> fillPacket, CancellationToken cancel = default)
    {
        var pkt = Connection.CreatePacketByMessageId(messageId);
        fillPacket(pkt);
        pkt.ComponentId = Identity.ComponentId;
        pkt.SystemId = Identity.SystemId;
        pkt.Sequence = PacketSequence.GetNextSequenceNumber();
        return Connection.Send(pkt, cancel);
    }
    
    protected Task InternalSend<TPacketSend>(Action<TPacketSend> fillPacket, CancellationToken cancel = default)
        where TPacketSend : IPacketV2<IPayload>, new()
    {
        var packet = new TPacketSend();
        fillPacket(packet);
        packet.ComponentId = Identity.ComponentId;
        packet.SystemId = Identity.SystemId;
        packet.Sequence = PacketSequence.GetNextSequenceNumber();
        return Connection.Send(packet, cancel);
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
        Logger.Trace($"{LogSend} call {p.Name}");
        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        linkedCancel.CancelAfter(timeoutMs);
        var tcs = new TaskCompletionSource<TAnswerPacket>();
        using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled());

        filter ??= (_ => true);
        using var subscribe = InternalFilterFirstAsync(targetSystemGetter,targetComponentGetter,filter).Subscribe(_ => tcs.TrySetResult(_));
        packet.ComponentId = Identity.ComponentId;
        packet.SystemId = Identity.SystemId;
        packet.Sequence = PacketSequence.GetNextSequenceNumber();
        await Connection.Send(packet, linkedCancel.Token).ConfigureAwait(false);
        var result = await tcs.Task.ConfigureAwait(false);
        Logger.Trace($"{LogRecv} ok {packet.Name}<=={p.Name}");
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
                Logger.Warn($"{LogSend} replay {currentAttempt} {name}");
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
        var msg = $"{LogSend} Timeout to execute '{name}' with {attemptCount} x {timeoutMs} ms'";
        Logger.Error(msg);
        throw new TimeoutException(msg);
    }

   
}
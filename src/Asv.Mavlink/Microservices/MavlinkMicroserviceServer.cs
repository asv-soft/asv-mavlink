using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public interface IMavlinkMicroserviceServer : IMicroserviceServer
{
    IMavlinkContext Core { get; }

    MavlinkIdentity Identity { get; }
}

public abstract class MavlinkMicroserviceServer : MicroserviceServer<MavlinkMessage>, IMavlinkMicroserviceServer
{
    protected MavlinkMicroserviceServer(string microserviceTypeName, MavlinkIdentity identity,
        IMavlinkContext core) : base(core, $"{identity}.{microserviceTypeName}")
    {
        ArgumentNullException.ThrowIfNull(microserviceTypeName);
        ArgumentNullException.ThrowIfNull(identity);
        ArgumentNullException.ThrowIfNull(core);
        Core = core;
        Identity = identity;
        TypeName = microserviceTypeName;
    }

    public IMavlinkContext Core { get; }
    public MavlinkIdentity Identity { get; }
    public override string TypeName { get; }

    protected override bool FilterDeviceMessages(MavlinkMessage arg)
    {
        // we cant filter messages by device id, cause not all messages have target device id
        return true;
    }

    protected override void FillMessageBeforeSent(MavlinkMessage message)
    {
        message.ComponentId = Identity.ComponentId;
        message.SystemId = Identity.SystemId;
        message.Sequence = Core.Sequence.GetNextSequenceNumber();
    }

    protected ValueTask InternalSend(int messageId, Action<MavlinkV2Message<IPayload>> fillPacket, CancellationToken cancel = default)
    {
        cancel.ThrowIfCancellationRequested();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        var pkt = (MavlinkV2Message<IPayload>)MavlinkV2MessageFactory.Instance.Create((ushort)messageId);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        fillPacket(pkt ?? throw new InvalidOperationException($"Packet {messageId} not found"));
        FillMessageBeforeSent(pkt);
        return Core.Connection.Send(pkt, cancel);
    }
    
    protected Observable<TMessage> InternalFilter<TMessage>(Func<TMessage, byte> targetSystemGetter,
        Func<TMessage, byte> targetComponentGetter)
        where TMessage : MavlinkMessage, new()
    {
        return base.InternalFilter<TMessage>(m =>
        {
            var sys = targetSystemGetter(m);
            var com = targetComponentGetter(m);
            return (Identity.SystemId == sys || sys == 0) && (Identity.ComponentId == com || com == 0);
        });
    }

    protected Observable<TPacket> InternalFilterFirstAsync<TPacket>(Func<TPacket, byte> targetSystemGetter,
        Func<TPacket, byte> targetComponentGetter, Func<TPacket, bool> filter)
        where TPacket : MavlinkMessage, new()
    {
        return InternalFilter(targetSystemGetter, targetComponentGetter).Where(filter).Take(1);
    }

    protected Task<TAnswerPacket> InternalSendAndWaitAnswer<TAnswerPacket>(MavlinkMessage packet,
        CancellationToken cancel,
        Func<TAnswerPacket, byte> targetSystemGetter,
        Func<TAnswerPacket, byte> targetComponentGetter,
        Func<TAnswerPacket, bool>? filter = null,
        int timeoutMs = 1000)
        where TAnswerPacket : MavlinkMessage, new()
    {
        return base.InternalSendAndWaitAnswer<TAnswerPacket>(packet,cancel, m =>
        {
            var sys = targetSystemGetter(m);
            var com = targetComponentGetter(m);
            if ((Identity.SystemId == sys || sys == 0) && (Identity.ComponentId == com || com == 0))
            {
                return filter?.Invoke(m) ?? true;
            }

            return false;
        }, timeoutMs);
    }

    protected Task<TResult> InternalCall<TResult, TSend, TReceive>(
        Action<TSend> fillPacket,
        Func<TReceive, byte> targetSystemGetter,
        Func<TReceive, byte> targetComponentGetter,
        Func<TReceive, bool> filter,
        Func<TReceive, TResult> resultGetter,
        int attemptCount = 5,
        Action<TSend, int>? fillOnConfirmation = null, int timeoutMs = 1000, CancellationToken cancel = default)
        where TSend : MavlinkMessage, new()
        where TReceive : MavlinkMessage, new()
    {
        return base.InternalCall(fillPacket, m =>
        {
            var sys = targetSystemGetter(m);
            var com = targetComponentGetter(m);
            if ((Identity.SystemId == sys || sys == 0) && (Identity.ComponentId == com || com == 0))
            {
                return filter?.Invoke(m) ?? true;
            }
            return false;
        }, resultGetter, attemptCount, fillOnConfirmation, timeoutMs, cancel);
    }


}
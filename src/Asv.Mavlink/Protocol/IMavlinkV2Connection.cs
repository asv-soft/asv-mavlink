using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink
{
    
    /// <summary>
    /// Connection for Mavlink V2
    /// </summary>
    public interface IMavlinkV2Connection:IObservable<IPacketV2<IPayload>>, IDisposable
    {
        long RxPackets { get; }
        long TxPackets { get; }
        long SkipPackets { get; }
        /// <summary>
        /// Event for deserialize package errors
        /// </summary>
        IObservable<DeserializePackageException> DeserializePackageErrors { get; }
        /// <summary>
        /// Event for transfer packet
        /// </summary>
        IObservable<IPacketV2<IPayload>> OnSendPacket { get; }
        /// <summary>
        /// Send packet
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task Send(IPacketV2<IPayload> packet, CancellationToken cancel);
        /// <summary>
        /// Base data stream
        /// </summary>
        IDataStream DataStream { get; }
    }

    public static class MavlinkV2Helper
    {

        /// <summary>
        /// Subscribe to connection packet pipe for waiting answer packet and then send request
        /// </summary>
        /// <typeparam name="TAnswerPacket"></typeparam>
        /// <typeparam name="TAnswerPayload"></typeparam>
        /// <param name="src"></param>
        /// <param name="packet"></param>
        /// <param name="targetSystem"></param>
        /// <param name="targetComponent"></param>
        /// <param name="cancel"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static async Task<TAnswerPacket> SendAndWaitAnswer<TAnswerPacket, TAnswerPayload>(this IMavlinkV2Connection src, IPacketV2<IPayload> packet, int targetSystem, int targetComponent, CancellationToken cancel, Func<TAnswerPacket,bool> filter = null)
        where TAnswerPacket : IPacketV2<TAnswerPayload>, new() where TAnswerPayload : IPayload
        {
            var p = new TAnswerPacket();
            var tcs = new TaskCompletionSource<TAnswerPacket>();
            using var c1 = cancel.Register(()=>tcs.TrySetCanceled());
            filter ??= (_ => true);
            using var subscribe = src.Where(_ => _.ComponenId == targetComponent && _.SystemId == targetSystem && _.MessageId == p.MessageId)
                .Cast<TAnswerPacket>()
                .FirstAsync(filter)
                .Subscribe(_=>tcs.TrySetResult(_));
            await src.Send(packet, cancel).ConfigureAwait(false);
            return await tcs.Task.ConfigureAwait(false);
        }

        public static Task Send<TAnswerPacket, TAnswerPayload>(this IMavlinkV2Connection src, Action<TAnswerPayload> setValueCallback, byte systemId,
            byte componentId, IPacketSequenceCalculator seq, CancellationToken cancel)
            where TAnswerPacket : IPacketV2<TAnswerPayload>, new() where TAnswerPayload : IPayload
        {
            var packet = new TAnswerPacket
            {
                ComponenId = componentId,
                SystemId = systemId,
                Sequence = seq.GetNextSequenceNumber(),
                CompatFlags = 0,
                IncompatFlags = 0,
            };
            setValueCallback(packet.Payload);
            return src.Send((IPacketV2<IPayload>)packet, cancel);
        }
        
    }
}

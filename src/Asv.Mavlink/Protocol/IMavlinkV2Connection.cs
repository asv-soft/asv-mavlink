using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using R3;

namespace Asv.Mavlink
{
    
    /// <summary>
    /// Connection for Mavlink V2
    /// </summary>
    public interface IMavlinkV2Connection: IProtocolConnection
    {
        /// <summary>
        /// Devices in network could not understand unknown messages (cause https://github.com/mavlink/mavlink/issues/1166) and will not forwarding it.
        /// This flag indicates that the message is not standard and we need to wrap this message into a V2_EXTENSION message for the routers to successfully transmit over the network.
        /// </summary>
        bool WrapToV2ExtensionEnabled { get; set; }
        long RxPackets { get; }
        long TxPackets { get; }
        long SkipPackets { get; }
        /// <summary>
        /// Event for deserialize package errors
        /// </summary>
        Observable<DeserializePackageException> DeserializePackageErrors { get; }
        /// <summary>
        /// Event for transfer packet
        /// </summary>
        Observable<IPacketV2<IPayload>> TxPipe { get; }
        Observable<IPacketV2<IPayload>> RxPipe { get; }
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
        /// <summary>
        /// Create packet by message id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        IPacketV2<IPayload>? CreatePacketByMessageId(int messageId);
    }

    /// <summary>
    /// Provides helper methods for MAVLink v2 communication.
    /// </summary>
    public static class MavlinkV2Helper
    {
        
        public static Observable<TPacket> Filter<TPacket>(this IMavlinkV2Connection src) 
            where TPacket : IPacketV2<IPayload>, new()
        {
            var origin = new TPacket();
            return src.RxPipe.Where(origin,(p,o) => o.MessageId == p.MessageId).Cast<IPacketV2<IPayload>,TPacket>();
        }
        /// <summary>
        /// This method sends a packet over an IMavlinkV2Connection and waits for an answer packet. </summary> <param name="src">The IMavlinkV2Connection instance to send the packet over.</param> <param name="packet">The packet to send.</param> <param name="targetSystem">The target system ID.</param> <param name="targetComponent">The target component ID.</param> <param name="cancel">The cancellation token to cancel the operation.</param> <param name="filter">An optional filter function to filter the answer packet.</param> <typeparam name="TAnswerPacket">The type of the answer packet.</typeparam> <typeparam name="TAnswerPayload">The type of the payload in the answer packet.</typeparam> <returns>The answer packet received.</returns>
        /// /
        public static async Task<TAnswerPacket> SendAndWaitAnswer<TAnswerPacket, TAnswerPayload>(
            this IMavlinkV2Connection src, IPacketV2<IPayload> packet, int targetSystem, int targetComponent,
            CancellationToken cancel, Func<TAnswerPacket, bool>? filter = null)
            where TAnswerPacket : IPacketV2<TAnswerPayload>, new() where TAnswerPayload : IPayload
        {
            var origin = new TAnswerPacket();
            var tcs = new TaskCompletionSource<TAnswerPacket>();
            await using var c1 = cancel.Register(()=>tcs.TrySetCanceled());
            filter ??= (_ => true);
            using var subscribe = src.RxPipe
                .Where(origin,(v,o) => v.ComponentId == targetComponent && v.SystemId == targetSystem && v.MessageId == o.MessageId)
                .Cast<IPacketV2<IPayload>,TAnswerPacket>()
                .Where(filter)
                .TakeLast(1)
                .Subscribe(tcs,(p,x)=>x.TrySetResult(p));
            await src.Send(packet, cancel).ConfigureAwait(false);
            return await tcs.Task.ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a packet to the specified connection, with the specified values, using the MAVLink v2 protocol.
        /// </summary>
        /// <typeparam name="TAnswerPacket">The type of the packet being sent.</typeparam>
        /// <typeparam name="TAnswerPayload">The type of the payload in the packet.</typeparam>
        /// <param name="src">The IMavlinkV2Connection instance.</param>
        /// <param name="setValueCallback">The callback action to set the values of the payload in the packet.</param>
        /// <param name="systemId">The system ID of the sender.</param>
        /// <param name="componentId">The component ID of the sender.</param>
        /// <param name="seq">The packet sequence calculator.</param>
        /// <param name="cancel">The cancellation token.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static Task Send<TAnswerPacket, TAnswerPayload>(this IMavlinkV2Connection src,
            Action<TAnswerPayload> setValueCallback, byte systemId,
            byte componentId, IPacketSequenceCalculator seq, CancellationToken cancel)
            where TAnswerPacket : IPacketV2<TAnswerPayload>, new() where TAnswerPayload : IPayload
        {
            var packet = new TAnswerPacket
            {
                ComponentId = componentId,
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

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink
{
    public abstract class MavlinkMicroserviceBase:DisposableOnceWithCancel, IDisposable
    {
        private readonly IMavlinkV2Connection _connection;
        private readonly IPacketSequenceCalculator _seq;
        private readonly MavlinkServerIdentity _identity;

        protected MavlinkMicroserviceBase(IMavlinkV2Connection connection, IPacketSequenceCalculator seq, MavlinkServerIdentity identity)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _seq = seq ?? throw new ArgumentNullException(nameof(seq));
            _identity = identity ?? throw new ArgumentNullException(nameof(identity));
        }

        protected Task Send<TPacket,TPayload>(Action<TPayload> fillPayload)
            where TPacket : IPacketV2<TPayload>, new() where TPayload : IPayload
        {
            var packet = new TPacket
            {
                ComponenId = _identity.ComponentId,
                SystemId = _identity.SystemId,
                CompatFlags = 0,
                IncompatFlags = 0,
                Sequence = _seq.GetNextSequenceNumber(),
            };
            fillPayload(packet.Payload);
            return _connection.Send((IPacketV2<IPayload>)packet, DisposeCancel);
        }

        protected void Subscribe<TPacket,TPayload>(Action<TPacket> callback)
            where TPacket : IPacketV2<TPayload>, new() where TPayload : IPayload
        {
            var id = new TPacket().MessageId;
            _connection
                .Where(_ => _.MessageId == id)
                .Cast<TPacket>()
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(callback).DisposeItWith(Disposable);
        }

       
    }
}

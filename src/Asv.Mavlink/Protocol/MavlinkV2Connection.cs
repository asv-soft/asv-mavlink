using System;
using System.Buffers;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Decoder;

namespace Asv.Mavlink
{
    public class MavlinkV2Connection : DisposableOnceWithCancel, IMavlinkV2Connection
    {
        private readonly PacketV2Decoder _decoder;
        private long _txPackets;
        private long _rxPackets;
        private long _skipPackets;
        private readonly Subject<IPacketV2<IPayload>> _sendPacketSubject;

        public MavlinkV2Connection(string connectionString, Action<IPacketDecoder<IPacketV2<IPayload>>> register):this(ConnectionStringConvert(connectionString),register,true)
        {
        }

        private static IDataStream ConnectionStringConvert(string connString)
        {
            var p = PortFactory.Create(connString);
            p.Enable();
            return p;
        }

        public MavlinkV2Connection(IDataStream dataStream, Action<IPacketDecoder<IPacketV2<IPayload>>> register, bool disposeDataStream = false)
        {
            DataStream = dataStream;
            if (disposeDataStream && DataStream is IDisposable disposableStrm)
            {
                disposableStrm.DisposeItWith(Disposable);
            }
            _decoder = new PacketV2Decoder().DisposeItWith(Disposable);
            register(_decoder);
            _decoder.DisposeItWith(Disposable);
            DataStream.SelectMany(_=>_).Subscribe(_=> _decoder.OnData(_)).DisposeItWith(Disposable); 
            _decoder.Subscribe(_ => Interlocked.Increment(ref _rxPackets)).DisposeItWith(Disposable);
            _sendPacketSubject = new Subject<IPacketV2<IPayload>>().DisposeItWith(Disposable);

            _sendPacketSubject.Subscribe(_ => Interlocked.Increment(ref _txPackets)).DisposeItWith(Disposable);
            _decoder.OutError.Subscribe(_ => Interlocked.Increment(ref _skipPackets)).DisposeItWith(Disposable);
        }

        public long RxPackets => Interlocked.Read(ref _rxPackets);
        public long TxPackets => Interlocked.Read(ref _txPackets);
        public long SkipPackets => Interlocked.Read(ref _skipPackets);
        public IObservable<DeserializePackageException> DeserializePackageErrors => _decoder.OutError;
        public IObservable<IPacketV2<IPayload>> OnSendPacket => _sendPacketSubject;
        public IDataStream DataStream { get; }

        public Task Send(IPacketV2<IPayload> packet, CancellationToken cancel)
        {
            if (IsDisposed) return Task.CompletedTask;
            _sendPacketSubject.OnNext(packet);
            return Task.Run(() =>
            {
                var data = ArrayPool<byte>.Shared.Rent(packet.GetMaxByteSize());
                try
                {
                    var span = new Span<byte>(data);
                    var size = span.Length;
                    packet.Serialize(ref span);
                    size -= span.Length;
                    DataStream.Send(data,size,cancel).Wait(cancel);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(data);
                }
                
            },cancel);
        }

        public IDisposable Subscribe(IObserver<IPacketV2<IPayload>> observer)
        {
            return _decoder.Subscribe(observer);
        }
    }
}

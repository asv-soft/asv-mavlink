#nullable enable
using System;
using System.Buffers;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Icarous;
using Asv.Mavlink.V2.Uavionix;

namespace Asv.Mavlink
{
    public class MavlinkV2Connection : DisposableOnceWithCancel, IMavlinkV2Connection
    {
        private readonly IScheduler? _publishScheduler;

        #region Static

        public static IMavlinkV2Connection Create(IDataStream dataStream, bool disposeDataStream = false,IScheduler? publishScheduler = null)
        {
            return new MavlinkV2Connection(dataStream, RegisterDefaultDialects,disposeDataStream,publishScheduler);
        }
        
        public static IMavlinkV2Connection Create(IDataStream dataStream,Action<IPacketDecoder<IPacketV2<IPayload>>> register, bool disposeDataStream = false,IScheduler? publishScheduler = null)
        {
            return new MavlinkV2Connection(dataStream, register,disposeDataStream,publishScheduler);
        }
        
        public static IMavlinkV2Connection Create(string connectionString)
        {
            return new MavlinkV2Connection(connectionString, RegisterDefaultDialects);
        }

        public static void RegisterDefaultDialects(IPacketDecoder<IPacketV2<IPayload>> decoder)
        {
            decoder.RegisterCommonDialect();
            decoder.RegisterArdupilotmegaDialect();
            decoder.RegisterIcarousDialect();
            decoder.RegisterUavionixDialect();
            decoder.RegisterAsvGbsDialect();
            decoder.RegisterAsvSdrDialect();
        }

        #endregion
        
        private readonly PacketV2Decoder _decoder;
        private long _txPackets;
        private long _rxPackets;
        private long _skipPackets;
        private readonly Subject<IPacketV2<IPayload>> _sendPacketSubject;
        private readonly Subject<IPacketV2<IPayload>> _recvPacketsSubject;

        public MavlinkV2Connection(string connectionString, Action<IPacketDecoder<IPacketV2<IPayload>>> register):this(ConnectionStringConvert(connectionString),register,true)
        {
        }

        private static IDataStream ConnectionStringConvert(string connString)
        {
            var p = PortFactory.Create(connString);
            p.Enable();
            return p;
        }

        public MavlinkV2Connection(IDataStream dataStream, Action<IPacketDecoder<IPacketV2<IPayload>>> register, bool disposeDataStream = false, IScheduler? publishScheduler = null)
        {
            _publishScheduler = publishScheduler;
            DataStream = dataStream;
            if (disposeDataStream && DataStream is IDisposable disposableStrm)
            {
                disposableStrm.DisposeItWith(Disposable);
            }
            _decoder = new PacketV2Decoder().DisposeItWith(Disposable);
            register(_decoder);
            _decoder.DisposeItWith(Disposable);
            DataStream.Subscribe(_=> _decoder.OnData(_)).DisposeItWith(Disposable); 
            _sendPacketSubject = new Subject<IPacketV2<IPayload>>().DisposeItWith(Disposable);
            _recvPacketsSubject = new Subject<IPacketV2<IPayload>>().DisposeItWith(Disposable);
            _decoder.Where(TryDecodeV2ExtensionPackets).Subscribe(_recvPacketsSubject).DisposeItWith(Disposable); // we need only one subscription to decoder
            _recvPacketsSubject.Subscribe(_ => Interlocked.Increment(ref _rxPackets)).DisposeItWith(Disposable);
            _sendPacketSubject.Subscribe(_ => Interlocked.Increment(ref _txPackets)).DisposeItWith(Disposable);
            _decoder.OutError.Subscribe(_ => Interlocked.Increment(ref _skipPackets)).DisposeItWith(Disposable);
        }

        public bool WrapToV2ExtensionEnabled { get; set; } = false;
        public long RxPackets => Interlocked.Read(ref _rxPackets);
        public long TxPackets => Interlocked.Read(ref _txPackets);
        public long SkipPackets => Interlocked.Read(ref _skipPackets);
        public IObservable<DeserializePackageException> DeserializePackageErrors => _decoder.OutError;
        public IObservable<IPacketV2<IPayload>> OnSendPacket => _sendPacketSubject;
        public IDataStream DataStream { get; }
        public IPacketV2<IPayload> CreatePacketByMessageId(int messageId)
        {
            return _decoder.Create(messageId);
        }

        public Task Send(IPacketV2<IPayload> packet, CancellationToken cancel)
        {
            if (IsDisposed) return Task.CompletedTask;
            _sendPacketSubject.OnNext(packet);
            return Task.Run(() =>
            {
                if (packet.WrapToV2Extension && WrapToV2ExtensionEnabled)
                {
                    var wrappedPacket = new V2ExtensionPacket
                    {
                        Tag = packet.Tag,
                        IncompatFlags = packet.IncompatFlags,
                        CompatFlags = packet.CompatFlags,
                        Sequence = packet.Sequence,
                        SystemId = packet.SystemId,
                        ComponentId = packet.ComponentId,
                        Payload =
                        {
                            // broadcast
                            TargetComponent = 0,
                            TargetSystem = 0,
                            TargetNetwork = 0,
                            MessageType = V2ExtensionFeature.V2ExtensionMessageId
                        }
                    };
                    var span = new Span<byte>(wrappedPacket.Payload.Payload);
                    var size = span.Length;
                    packet.Serialize(ref span);
                    size -= span.Length;
                    var arr = wrappedPacket.Payload.Payload;
                    Array.Resize(ref arr, size);
                    wrappedPacket.Payload.Payload = arr;
                    packet = wrappedPacket;
                }
                
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
            return _publishScheduler != null ? _recvPacketsSubject.ObserveOn(_publishScheduler).Subscribe(observer) : _recvPacketsSubject.Subscribe(observer);
        }

        private bool TryDecodeV2ExtensionPackets(IPacketV2<IPayload> arg)
        {
            if (WrapToV2ExtensionEnabled == false) return true;
            if (arg is not V2ExtensionPacket wrapped) return true;
            Task.Factory.StartNew(()=>_decoder.OnData(wrapped.Payload.Payload));
            return false;

        }
    }
}


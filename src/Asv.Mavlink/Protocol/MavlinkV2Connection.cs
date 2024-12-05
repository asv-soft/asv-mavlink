/*
#nullable enable
using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;



using Asv.Mavlink.AsvGbs;
using Asv.Mavlink.AsvRadio;
using Asv.Mavlink.AsvRfsa;
using Asv.Mavlink.AsvRsga;

using Asv.Mavlink.Avssuas;

using Asv.Mavlink.Csairlink;
using Asv.Mavlink.Cubepilot;
using Asv.Mavlink.Icarous;

using Asv.Mavlink.Storm32;
using Asv.Mavlink.Ualberta;
using Asv.Mavlink.Uavionix;
using R3;

namespace Asv.Mavlink
{
    public sealed class MavlinkV2Connection : IProtocolConnection
    {
        #region Static

        public static IProtocolConnection Create(IDataStream dataStream, bool disposeDataStream = false)
        {
            return new MavlinkV2Connection(dataStream, RegisterDefaultDialects,disposeDataStream);
        }
        
        public static IProtocolConnection Create(IDataStream dataStream,Action<IPacketDecoder<MavlinkMessage>> register, bool disposeDataStream = false)
        {
            return new MavlinkV2Connection(dataStream, register,disposeDataStream);
        }
        
        public static IProtocolConnection Create(string connectionString)
        {
            return new MavlinkV2Connection(connectionString, RegisterDefaultDialects);
        }

        public static void RegisterDefaultDialects(IPacketDecoder<MavlinkMessage> decoder)
        {
            decoder.RegisterMinimalDialect();
            decoder.RegisterCommonDialect();
            decoder.RegisterArdupilotmegaDialect();
            decoder.RegisterIcarousDialect();
            decoder.RegisterUalbertaDialect();
            decoder.RegisterStorm32Dialect();
            decoder.RegisterAvssuasDialect();
            decoder.RegisterUavionixDialect();
            decoder.RegisterCubepilotDialect();
            decoder.RegisterCsairlinkDialect();
            decoder.RegisterAsvGbsDialect();
            decoder.RegisterAsvSdrDialect();
            decoder.RegisterAsvAudioDialect();
            decoder.RegisterAsvRadioDialect();
            decoder.RegisterAsvRfsaDialect();
            decoder.RegisterAsvChartDialect();
            decoder.RegisterAsvRsgaDialect();
        }

        #endregion
        
        private readonly PacketV2Decoder _decoder;
        private long _txPackets;
        private long _rxPackets;
        private long _skipPackets;
        private readonly Subject<MavlinkMessage> _sendPacketSubject;
        private readonly IDisposable _sub1;
        private readonly IDisposable _sub2;

        public MavlinkV2Connection(string connectionString, Action<IPacketDecoder<MavlinkMessage>> register):this(ConnectionStringConvert(connectionString),register,true)
        {
        }

        private static IDataStream ConnectionStringConvert(string connString)
        {
            var p = PortFactory.Create(connString);
            p.Enable();
            return p;
        }

        public MavlinkV2Connection(IDataStream dataStream, Action<IPacketDecoder<MavlinkMessage>> register, bool disposeDataStream = false)
        {
            DataStream = dataStream;
            _decoder = new PacketV2Decoder();
            register(_decoder);
            _sub1 = DataStream.OnReceive.Subscribe(b=> _decoder.OnData(b)); 
            _sendPacketSubject = new Subject<MavlinkMessage>();
            RxPipe = _decoder.OnPacket
                .Do(x => Interlocked.Increment(ref _rxPackets))
                .Where(TryDecodeV2ExtensionPackets).Publish().RefCount();
            // ReSharper disable once HeapView.CanAvoidClosure
            _sub2 = _decoder.OutError.Subscribe(_ => Interlocked.Increment(ref _skipPackets));
        }

        public bool WrapToV2ExtensionEnabled { get; set; } = false;
        public long RxPackets => Interlocked.Read(ref _rxPackets);
        public long TxPackets => Interlocked.Read(ref _txPackets);
        public long SkipPackets => Interlocked.Read(ref _skipPackets);
        public Observable<DeserializePackageException> DeserializePackageErrors => _decoder.OutError;
        public Observable<MavlinkMessage> TxPipe => _sendPacketSubject;
        public Observable<MavlinkMessage> RxPipe { get; }

        public IDataStream DataStream { get; }
        public MavlinkMessage? CreatePacketByMessageId(int messageId)
        {
            return _decoder.Create(messageId);
        }

        public async Task Send(MavlinkMessage packet, CancellationToken cancel)
        {
            if (_sendPacketSubject.IsDisposed) return;
            Interlocked.Increment(ref _txPackets);
            _sendPacketSubject.OnNext(packet);
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
                
                var size = packet.Serialize(wrappedPacket.Payload.Payload);
                var arr = wrappedPacket.Payload.Payload;
                Array.Resize(ref arr, size);
                wrappedPacket.Payload.Payload = arr;
                packet = wrappedPacket;
            }
                
            var data = ArrayPool<byte>.Shared.Rent(packet.GetMaxByteSize());
            try
            {
                
                var size = packet.Serialize(data);
                await DataStream.Send(data,size,cancel).ConfigureAwait(false);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(data);
            }
        }


        private bool TryDecodeV2ExtensionPackets(MavlinkMessage arg)
        {
            if (WrapToV2ExtensionEnabled == false) return true;
            if (arg is not V2ExtensionPacket wrapped) return true;
            Task.Factory.StartNew(()=>_decoder.OnData(wrapped.Payload.Payload));
            return false;

        }

        #region Dispose

        public void Dispose()
        {
            _decoder.Dispose();
            _sendPacketSubject.Dispose();
            _sub1.Dispose();
            _sub2.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _decoder.DisposeAsync().ConfigureAwait(false);
            await CastAndDispose(_sendPacketSubject).ConfigureAwait(false);
            await CastAndDispose(_sub1).ConfigureAwait(false);
            await CastAndDispose(_sub2).ConfigureAwait(false);

            return;

            static async ValueTask CastAndDispose(IDisposable resource)
            {
                if (resource is IAsyncDisposable resourceAsyncDisposable)
                    await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
                else
                    resource.Dispose();
            }
        }

        #endregion
    }
}
*/


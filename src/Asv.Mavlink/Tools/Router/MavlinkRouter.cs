#nullable enable
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;
using R3;

namespace Asv.Mavlink
{
    public class MavlinkRouter: IMavlinkRouter, IDataStream
    {
        public const string DefaultName = "MavlinkRouter";
        public static MavlinkRouter CreateDefault(ILoggerFactory? logFactory = null)
        {
            return new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects,DefaultName,logFactory);
        }
       
        private readonly Action<IPacketDecoder<IPacketV2<IPayload>>> _register;
        private readonly ReaderWriterLockSlim _portCollectionSync;
        private readonly Subject<IPacketV2<IPayload>> _inputPackets;
        private readonly Subject<byte[]> _rawData;
        private readonly List<MavlinkPort> _ports = new(8);
        private long _rxBytes;
        private long _txBytes;
        private readonly Subject<IPacketV2<IPayload>> _onSendPacketSubject;
        private readonly Subject<DeserializePackageException> _deserializeErrors;
        private long _txPackets;
        private long _rxPackets;
        private readonly ILogger _logger;
        private readonly Subject<Guid> _onAddSubject;
        private readonly Subject<Guid> _onRemoveSubject;
        private readonly Subject<Guid> _onConfigChanged;
        private readonly PacketV2Decoder _decoder;

        public MavlinkRouter(Action<IPacketDecoder<IPacketV2<IPayload>>> register,string? name=DefaultName, ILoggerFactory? logFactory = null)
        {
            logFactory??=NullLoggerFactory.Instance;
            _logger = logFactory.CreateLogger<MavlinkRouter>();
            Name = name ?? DefaultName;
            _register = register;
            _decoder = new PacketV2Decoder();
            register(_decoder);
            _inputPackets = new Subject<IPacketV2<IPayload>>();
            _decoder.OnPacket.Subscribe(_inputPackets.AsObserver());
            _rawData = new Subject<byte[]>();
            _onSendPacketSubject = new Subject<IPacketV2<IPayload>>();
            _deserializeErrors = new Subject<DeserializePackageException>();
            _onAddSubject = new Subject<Guid>();
            _onConfigChanged = new Subject<Guid>();
            _onRemoveSubject = new Subject<Guid>();
            _portCollectionSync = new(LockRecursionPolicy.SupportsRecursion);
        }

        public Guid AddPort(MavlinkPortConfig port)
        {
            Guid id;
            try
            {
                _portCollectionSync.EnterWriteLock();
                var portObject = new MavlinkPort(port, _register, Guid.NewGuid());
                portObject.Connection.DeserializePackageErrors.Do(p=>p.SourceName = port.Name).Subscribe(_deserializeErrors.AsObserver());
                portObject.Port.Subscribe(x=>_rawData.OnNext(x));

                if (port.PacketLossChance <= 0)
                {
                    portObject.Connection.RxPipe.Do(v=>v.Tag = portObject).Subscribe(OnRecvPacket);
                }
                else
                {
                    portObject.Connection.RxPipe.Do(v=>v.Tag = portObject).Subscribe(port,(packet,ctx) =>
                    {
                        var chance = Random.Shared.Next(100);
                        if (chance > ctx.PacketLossChance)
                        {
                            OnRecvPacket(packet);
                        }
                    });
                }
                _ports.Add(portObject);
                id = portObject.Id;
            }
            finally
            {
                _portCollectionSync.ExitWriteLock();
            }
            _onConfigChanged.OnNext(id);
            _onAddSubject.OnNext(id);
            return id;
        }

        public Observable<Guid> OnAddPort => _onAddSubject;

        public bool RemovePort(Guid id)
        {
            bool result;
            try
            {
                _portCollectionSync.EnterWriteLock();
                var portToRemove = _ports.FirstOrDefault(p => p.Id == id);
                if (portToRemove == null) return false;
                portToRemove.Dispose();
                result = _ports.Remove(portToRemove);
            }
            finally
            {
                _portCollectionSync.ExitWriteLock();
            }

            if (result)
            {
                _onConfigChanged.OnNext(id);
                _onRemoveSubject.OnNext(id);
            }

            return result;
        }

        public Observable<Guid> OnRemovePort => _onRemoveSubject;

        public bool SetEnabled(Guid id, bool enabled)
        {
            try
            {
                _portCollectionSync.EnterReadLock();
                var portToRemove = _ports.FirstOrDefault(p => p.Id == id);
                if (portToRemove == null) return false;
                if (enabled)
                {
                    portToRemove.Port.Enable();
                }
                else
                {
                    portToRemove.Port.Disable();
                }
                _onConfigChanged.OnNext(id);
                return true;
            }
            finally
            {
                _portCollectionSync.ExitReadLock();
            }

        }

        public Guid[] GetPorts()
        {
            try
            {
                _portCollectionSync.EnterReadLock();
                return _ports.Select(p => p.Id).ToArray();
            }
            finally
            {
                _portCollectionSync.ExitReadLock();
            }
        }

        public MavlinkPortInfo? GetInfo(Guid id)
        {
            try
            {
                _portCollectionSync.EnterReadLock();
                var portToRemove = _ports.FirstOrDefault(p => p.Id == id);
                return portToRemove?.GetInfo();
            }
            finally
            {
                _portCollectionSync.ExitReadLock();
            }
        }

        public MavlinkPortConfig? GetConfig(Guid id)
        {
            try
            {
                _portCollectionSync.EnterReadLock();
                var portToRemove = _ports.FirstOrDefault(p => p.Id == id);
                return portToRemove?.GetConfig();
            }
            finally
            {
                _portCollectionSync.ExitReadLock();
            }
        }

        public MavlinkPortConfig[] GetConfig()
        {
            try
            {
                _portCollectionSync.EnterReadLock();
                return _ports.Select(p => p.GetConfig()).ToArray();
            }
            finally
            {
                _portCollectionSync.ExitReadLock();
            }
        }

        public Observable<Guid> OnConfigChanged => _onConfigChanged;
        
        private void OnRecvPacket(IPacketV2<IPayload> packet)
        {
            Interlocked.Increment(ref _rxPackets);
            var size = packet.GetMaxByteSize();
            var data = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                if (_onSendPacketSubject.IsDisposed) return;
                _portCollectionSync.EnterReadLock();
                // for optimization we serialize packet once and then send it as byte array
                var span = new Span<byte>(data, 0, size);
                packet.Serialize(ref span);
                var packetSize = size - span.Length;
                Interlocked.Add(ref _rxBytes,packetSize);
                _onSendPacketSubject.OnNext(packet);
                // we need to send the packet to all other ports except the receiving one
                var portToSend = _ports.Where(p => p.Port.IsEnabled.Value && p.Port.State.Value == PortState.Connected && packet.Tag != p);
                Task.WaitAll(portToSend.Select(p => (Task)p.InternalSendSerializedPacket(new ReadOnlyMemory<byte>(data,0,size), default)).ToArray());
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(data);
                if (_onSendPacketSubject.IsDisposed == false)
                {
                    _portCollectionSync.ExitReadLock();    
                }
            }

            packet.Tag = null;
            try
            {
                if (WrapToV2ExtensionEnabled && packet is V2ExtensionPacket wrapped)
                {
                    // decoder will send unwrapped message to _inputPackets
                    _decoder.OnData(wrapped.Payload.Payload);
                }
                else
                {
                    _inputPackets.OnNext(packet);
                }
            }
            catch (Exception e)
            {
                _logger.ZLogError(e,$"Error to publish packet:{packet.Name}");
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
            

        }
        public Observable<DeserializePackageException> DeserializePackageErrors => _deserializeErrors;
        public Observable<IPacketV2<IPayload>> TxPipe => _onSendPacketSubject;
        public Observable<IPacketV2<IPayload>> RxPipe => _inputPackets;

        public async Task Send(IPacketV2<IPayload> packet, CancellationToken cancel)
        {
            if (_onSendPacketSubject.IsDisposed) return;
            _onSendPacketSubject.OnNext(packet);
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
                await Send(data, size, cancel).ConfigureAwait(false);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(data);
            }
        }

        public IDataStream DataStream => this;
        public IPacketV2<IPayload>? CreatePacketByMessageId(int messageId)
        {
            return _decoder.Create(messageId);
        }


        public async Task<bool> Send(byte[] data, int count, CancellationToken cancel)
        {
            Interlocked.Add(ref _txBytes, count);
            Interlocked.Increment(ref _txPackets);
            Task<bool>[] tasks;
            try
            {
                _portCollectionSync.EnterReadLock();
                tasks =  _ports.Where(p => p.Port.IsEnabled.Value && p.Port.State.Value == PortState.Connected)
                    .Select(p => p.InternalSendSerializedPacket(data, count, cancel)).ToArray();
            }
            finally
            {
                _portCollectionSync.ExitReadLock();
            }
            var result = await Task.WhenAll(tasks).ConfigureAwait(false);
            return result.All(b => b);
        }

        public async Task<bool> Send(ReadOnlyMemory<byte> data, CancellationToken cancel)
        {
            Interlocked.Add(ref _txBytes, data.Length);
            Interlocked.Increment(ref _txPackets);
            Task<bool>[] tasks;
            try
            {
                _portCollectionSync.EnterReadLock();
                tasks =  _ports.Where(p => p.Port.IsEnabled.Value && p.Port.State.Value == PortState.Connected)
                    .Select(p => p.InternalSendSerializedPacket(data, cancel)).ToArray();
            }
            finally
            {
                _portCollectionSync.ExitReadLock();
            }
            var result = await Task.WhenAll(tasks).ConfigureAwait(false);
            return result.All(b => b);
        }

        public string Name { get; }

        public bool WrapToV2ExtensionEnabled { get; set; } = false;
        public long RxPackets => Interlocked.Read(ref _rxPackets);
        public long TxPackets => Interlocked.Read(ref _txPackets);
        public long SkipPackets
        {
            get
            {
                try
                {
                    _portCollectionSync.EnterReadLock();
                    return _ports.Sum(p => p.Connection.SkipPackets);
                }
                finally
                {
                    _portCollectionSync.ExitReadLock();
                }
            }
        }
        public long RxBytes => Interlocked.Read(ref _rxBytes);
        public long TxBytes => Interlocked.Read(ref _txBytes);

        public IDisposable Subscribe(IObserver<byte[]> observer)
        {
            return _rawData.Subscribe(observer.ToObserver());
        }

        #region Dispose

        public void Dispose()
        {
            _inputPackets.Dispose();
            _rawData.Dispose();
            _onSendPacketSubject.Dispose();
            _deserializeErrors.Dispose();
            _onAddSubject.Dispose();
            _onRemoveSubject.Dispose();
            _onConfigChanged.Dispose();
            _decoder.Dispose();
            
            _portCollectionSync.EnterWriteLock();
            try
            {
                foreach (var mavlinkPort in _ports)
                {
                    mavlinkPort.Dispose();
                }
                _ports.Clear();
            }
            catch (Exception e)
            {
                _logger.ZLogError(e,$"Error to dispose mavlink ports:{e.Message}");
            }
            finally
            {
                _portCollectionSync.ExitWriteLock();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await CastAndDispose(_portCollectionSync).ConfigureAwait(false);
            await CastAndDispose(_inputPackets).ConfigureAwait(false);
            await CastAndDispose(_rawData).ConfigureAwait(false);
            await CastAndDispose(_onSendPacketSubject).ConfigureAwait(false);
            await CastAndDispose(_deserializeErrors).ConfigureAwait(false);
            await CastAndDispose(_onAddSubject).ConfigureAwait(false);
            await CastAndDispose(_onRemoveSubject).ConfigureAwait(false);
            await CastAndDispose(_onConfigChanged).ConfigureAwait(false);
            await _decoder.DisposeAsync().ConfigureAwait(false);

            _portCollectionSync.EnterWriteLock();
            try
            {
                foreach (var mavlinkPort in _ports)
                {
                    await CastAndDispose(mavlinkPort).ConfigureAwait(false);
                }
                _ports.Clear();
            }
            catch (Exception e)
            {
                _logger.ZLogError(e,$"Error to dispose mavlink ports:{e.Message}");
            }
            finally
            {
                _portCollectionSync.ExitWriteLock();
            }
            
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
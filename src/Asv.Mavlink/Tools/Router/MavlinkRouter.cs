#nullable enable
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using NLog;

namespace Asv.Mavlink
{

    public class MavlinkPort : DisposableOnceWithCancel
    {
        private readonly MavlinkPortConfig _config;
        private int _deserializeError;
        private long _txPackets;
        public Guid Id { get; }

        public MavlinkPort(MavlinkPortConfig config, Action<IPacketDecoder<IPacketV2<IPayload>>> register, Guid id)
        {
            _config = config;
            Id = id;
            Port = PortFactory.Create(config.ConnectionString, config.IsEnabled).DisposeItWith(Disposable);
            Connection = new MavlinkV2Connection(Port, register).DisposeItWith(Disposable);
            Connection.DeserializePackageErrors.Subscribe(_ => Interlocked.Increment(ref _deserializeError)).DisposeItWith(Disposable);
        }

        public IMavlinkV2Connection Connection { get; }
        public IPort Port { get; }

       
        internal Task<bool> InternalSendSerializedPacket(byte[] data, int count, CancellationToken cancel)
        {
            // we have to manually increment the packet counter because we serialize the packet once into an array of bytes and send
            Interlocked.Increment(ref _txPackets);
            return Port.Send(data, count, cancel);
        }

        public MavlinkPortInfo GetInfo()
        {
            return new MavlinkPortInfo
            {
                Id = Id,
                Name = _config.Name,
                ConnectionString = _config.ConnectionString,
                State = Port.State.Value,
                RxBytes = Port.RxBytes,
                TxBytes = Port.TxBytes,
                RxPackets = Connection.RxPackets,
                // we have to manually increment the packet counter because we serialize the packet once into an array of bytes and send
                TxPackets = Interlocked.Read(ref _txPackets),
                SkipPackets = Connection.SkipPackets,
                DeserializationErrors = _deserializeError,
                Description = Port.ToString(),
                LastException = Port.Error.Value,
                Type = Port.PortType,
                IsEnabled = Port?.IsEnabled.Value,
            };
        }

        public MavlinkPortConfig GetConfig()
        {
            return new MavlinkPortConfig
            {
                ConnectionString = _config.ConnectionString,
                Name = _config.Name,
                IsEnabled = Port.IsEnabled.Value
            };
        }
    }

    public class MavlinkRouter:DisposableOnceWithCancel, IMavlinkRouter, IDataStream
    {
        public static MavlinkRouter CreateDefault()
        {
            return new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects);
        }
        
        private readonly Action<IPacketDecoder<IPacketV2<IPayload>>> _register;
        private readonly ReaderWriterLockSlim _portCollectionSync = new(LockRecursionPolicy.SupportsRecursion);
        private readonly Subject<IPacketV2<IPayload>> _inputPackets;
        private readonly Subject<byte[]> _rawData;
        private readonly List<MavlinkPort> _ports = new(8);
        private long _rxBytes;
        private long _txBytes;
        private readonly Subject<IPacketV2<IPayload>> _onSendPacketSubject;
        private readonly Subject<DeserializePackageException> _deserializeErrors;
        private long _txPackets;
        private long _rxPackets;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Subject<Guid> _onAddSubject;
        private readonly Subject<Guid> _onRemoveSubject;
        private readonly Subject<Guid> _onConfigChanged = new();

        public MavlinkRouter(Action<IPacketDecoder<IPacketV2<IPayload>>> register,string name="MavlinkRouter")
        {
            Name = name;
            _register = register;
            _inputPackets = new Subject<IPacketV2<IPayload>>().DisposeItWith(Disposable);
            _rawData = new Subject<byte[]>().DisposeItWith(Disposable);
            _onSendPacketSubject = new Subject<IPacketV2<IPayload>>().DisposeItWith(Disposable);
            _deserializeErrors = new Subject<DeserializePackageException>().DisposeItWith(Disposable);
            _onAddSubject = new Subject<Guid>().DisposeItWith(Disposable);
            _onRemoveSubject = new Subject<Guid>().DisposeItWith(Disposable);
        }

        public Guid AddPort(MavlinkPortConfig port)
        {
            Guid id;
            try
            {
                _portCollectionSync.EnterWriteLock();
                var portObject = new MavlinkPort(port, _register, Guid.NewGuid());
                portObject.Connection.DeserializePackageErrors.Do(_=>_.SourceName = port.Name).Subscribe(_deserializeErrors);
                portObject.Port.Subscribe(_rawData);
                portObject.Connection.Do(_=>_.Tag = portObject).Subscribe(OnRecvPacket);
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

        public IObservable<Guid> OnAddPort => _onAddSubject;

        public bool RemovePort(Guid id)
        {
            bool result;
            try
            {
                _portCollectionSync.EnterWriteLock();
                var portToRemove = _ports.FirstOrDefault(_ => _.Id == id);
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

        public IObservable<Guid> OnRemovePort => _onRemoveSubject;

        public bool SetEnabled(Guid id, bool enabled)
        {
            try
            {
                _portCollectionSync.EnterReadLock();
                var portToRemove = _ports.FirstOrDefault(_ => _.Id == id);
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
                return _ports.Select(_ => _.Id).ToArray();
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
                var portToRemove = _ports.FirstOrDefault(_ => _.Id == id);
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
                var portToRemove = _ports.FirstOrDefault(_ => _.Id == id);
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
                return _ports.Select(_ => _.GetConfig()).ToArray();
            }
            finally
            {
                _portCollectionSync.ExitReadLock();
            }
        }

        public IObservable<Guid> OnConfigChanged => _onConfigChanged;

        private void OnRecvPacket(IPacketV2<IPayload> packet)
        {
            Interlocked.Increment(ref _rxPackets);
            var size = packet.GetMaxByteSize();
            var data = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                
                _portCollectionSync.EnterReadLock();
                // for optimization we serialize packet once and then send it as byte array
                var span = new Span<byte>(data, 0, size);
                packet.Serialize(ref span);
                var packetSize = size - span.Length;
                Interlocked.Add(ref _rxBytes,packetSize);
                _onSendPacketSubject.OnNext(packet);
                // we need to send the packet to all other ports except the receiving one
                var portToSend = _ports.Where(_ => _.Port.IsEnabled.Value && _.Port.State.Value == PortState.Connected && packet.Tag != _);
                Task.WaitAll(portToSend.Select(_ => (Task)_.InternalSendSerializedPacket(data, packetSize, DisposeCancel)).ToArray());
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(data);
                _portCollectionSync.ExitReadLock();
            }

            packet.Tag = null;
            try
            {
                _inputPackets.OnNext(packet);
            }
            catch (Exception e)
            {
                Logger.Error(e,$"Error to publish packet:{packet.Name}");
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
            

        }

        public IDisposable Subscribe(IObserver<IPacketV2<IPayload>> observer)
        {
            return _inputPackets.Subscribe(observer);
        }

        

        public IObservable<DeserializePackageException> DeserializePackageErrors => _deserializeErrors;

        public IObservable<IPacketV2<IPayload>> OnSendPacket => _onSendPacketSubject;

        public Task Send(IPacketV2<IPayload> packet, CancellationToken cancel)
        {
            Interlocked.Increment(ref _txPackets);
            var size = packet.GetMaxByteSize();
            var data = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                var span = new Span<byte>(data, 0, size);
                packet.Serialize(ref span);
                var packetSize = size - span.Length;
                _onSendPacketSubject.OnNext(packet);
                return Send(data, packetSize, cancel);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(data);
            }
            
        }

        public IDataStream DataStream => this;

        public IDisposable Subscribe(IObserver<byte[]> observer)
        {
            return _rawData.Subscribe(observer);
        }

        public async Task<bool> Send(byte[] data, int count, CancellationToken cancel)
        {
            Interlocked.Add(ref _txBytes, count);
            Interlocked.Increment(ref _txPackets);
            Task<bool>[] tasks;
            try
            {
                _portCollectionSync.EnterReadLock();
                tasks =  _ports.Where(_ => _.Port.IsEnabled.Value && _.Port.State.Value == PortState.Connected)
                    .Select(_ => _.InternalSendSerializedPacket(data, count, cancel)).ToArray();
            }
            finally
            {
                _portCollectionSync.ExitReadLock();
            }
            var result = await Task.WhenAll(tasks).ConfigureAwait(false);
            return result.All(_ => _);
        }

        public string Name { get; }
        public long RxPackets => Interlocked.Read(ref _rxPackets);
        public long TxPackets => Interlocked.Read(ref _txPackets);
        public long SkipPackets
        {
            get
            {
                try
                {
                    _portCollectionSync.EnterReadLock();
                    return _ports.Sum(_ => _.Connection.SkipPackets);
                }
                finally
                {
                    _portCollectionSync.ExitReadLock();
                }
            }
        }
        public long RxBytes => Interlocked.Read(ref _rxBytes);
        public long TxBytes => Interlocked.Read(ref _txBytes);

        protected override void InternalDisposeOnce()
        {
            base.InternalDisposeOnce();
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
                Logger.Error(e,$"Error to dispose mavlink ports:{e.Message}");
            }
            finally
            {
                _portCollectionSync.ExitWriteLock();
            }
            
            _portCollectionSync.Dispose();
        }
    }
}
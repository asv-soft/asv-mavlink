﻿#nullable enable
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

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
        internal Task<bool> InternalSendSerializedPacket(ReadOnlyMemory<byte> data, CancellationToken cancel)
        {
            // we have to manually increment the packet counter because we serialize the packet once into an array of bytes and send
            Interlocked.Increment(ref _txPackets);
            return Port.Send(data, cancel);
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
                PacketLossChance = _config.PacketLossChance
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
        public const string DefaultName = "MavlinkRouter";
        public static MavlinkRouter CreateDefault(IScheduler? scheduler = null,ILoggerFactory? logFactory = null)
        {
            return new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects,DefaultName,scheduler,logFactory);
        }
       
        private readonly Action<IPacketDecoder<IPacketV2<IPayload>>> _register;
        private readonly IScheduler? _publishScheduler;
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
        private readonly ILogger _logger;
        private readonly Subject<Guid> _onAddSubject;
        private readonly Subject<Guid> _onRemoveSubject;
        private readonly Subject<Guid> _onConfigChanged = new();
        private readonly PacketV2Decoder _decoder;

        public MavlinkRouter(Action<IPacketDecoder<IPacketV2<IPayload>>> register,string? name=DefaultName, IScheduler? publishScheduler = null, ILoggerFactory? logFactory = null)
        {
            logFactory??=NullLoggerFactory.Instance;
            _logger = logFactory.CreateLogger<MavlinkRouter>();
            Name = name ?? DefaultName;
            _register = register;
            _publishScheduler = publishScheduler;
            _decoder = new PacketV2Decoder();
            register(_decoder);
            _inputPackets = new Subject<IPacketV2<IPayload>>().DisposeItWith(Disposable);
            _decoder.Subscribe(_inputPackets).DisposeItWith(Disposable);
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
                portObject.Connection.DeserializePackageErrors.Do(p=>p.SourceName = port.Name).Subscribe(_deserializeErrors);
                portObject.Port.Subscribe(_rawData);

                if (port.PacketLossChance <= 0)
                {
                    portObject.Connection.Do(v=>v.Tag = portObject).Subscribe(OnRecvPacket);
                }
                else
                {
                    portObject.Connection.Do(v=>v.Tag = portObject).Subscribe(packet =>
                    {
                        var chance = Random.Shared.Next(100);
                        if (chance > port.PacketLossChance)
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

        public IObservable<Guid> OnAddPort => _onAddSubject;

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

        public IObservable<Guid> OnRemovePort => _onRemoveSubject;

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

        public IObservable<Guid> OnConfigChanged => _onConfigChanged;
        
        private void OnRecvPacket(IPacketV2<IPayload> packet)
        {
            Interlocked.Increment(ref _rxPackets);
            var size = packet.GetMaxByteSize();
            var data = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                if (IsDisposed) return;
                _portCollectionSync.EnterReadLock();
                // for optimization we serialize packet once and then send it as byte array
                var span = new Span<byte>(data, 0, size);
                packet.Serialize(ref span);
                var packetSize = size - span.Length;
                Interlocked.Add(ref _rxBytes,packetSize);
                _onSendPacketSubject.OnNext(packet);
                // we need to send the packet to all other ports except the receiving one
                var portToSend = _ports.Where(p => p.Port.IsEnabled.Value && p.Port.State.Value == PortState.Connected && packet.Tag != p);
                Task.WaitAll(portToSend.Select(p => (Task)p.InternalSendSerializedPacket(new ReadOnlyMemory<byte>(data,0,size), DisposeCancel)).ToArray());
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(data);
                if (IsDisposed == false)
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

        public IDisposable Subscribe(IObserver<IPacketV2<IPayload>> observer)
        {
            return _publishScheduler != null ? _inputPackets.ObserveOn(_publishScheduler).Subscribe(observer) : _inputPackets.Subscribe(observer);
        }
        
        public IObservable<DeserializePackageException> DeserializePackageErrors => _deserializeErrors;

        public IObservable<IPacketV2<IPayload>> OnSendPacket => _onSendPacketSubject;

        public Task Send(IPacketV2<IPayload> packet, CancellationToken cancel)
        {
            if (IsDisposed) return Task.CompletedTask;
            _onSendPacketSubject.OnNext(packet);
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
                    
                    Send(data, size, cancel).Wait(cancel);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(data);
                }
            }, cancel);
        }

        public IDataStream DataStream => this;
        public IPacketV2<IPayload> CreatePacketByMessageId(int messageId)
        {
            return _decoder.Create(messageId);
        }

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

        protected override void InternalDisposeOnce()
        {
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
            _portCollectionSync.Dispose();
             base.InternalDisposeOnce();
        }
    }
}
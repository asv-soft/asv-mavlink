using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Icarous;
using Asv.Mavlink.V2.Minimal;
using Asv.Mavlink.V2.Uavionix;
using Newtonsoft.Json;
using NLog;

namespace Asv.Mavlink
{
    public class GroundControlStationIdentity
    {
        public byte SystemId { get; set; } = 254;
        public byte ComponentId { get; set; } = 254;
    }

    public class GroundControlStation : DisposableOnceWithCancel, IGroundControlStation
    {
        private readonly GroundControlStationIdentity _config;
#if DEBUG
        private readonly TimeSpan _linkTimeout = TimeSpan.FromSeconds(120);
#else
        private readonly TimeSpan _linkTimeout = TimeSpan.FromSeconds(10);
#endif

        private readonly List<MavlinkDevice> _info = new();
        private readonly ReaderWriterLockSlim _deviceListLock = new();
        private readonly Subject<IMavlinkDeviceInfo> _foundDeviceSubject = new();
        private readonly Subject<IMavlinkDeviceInfo> _lostDeviceSubject = new();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPacketSequenceCalculator _seq;
        private readonly MavlinkPacketTransponder<HeartbeatPacket, HeartbeatPayload> _transponder;
        

        public class MavlinkDeviceInfo : IMavlinkDeviceInfo
        {
            private readonly HeartbeatPacket _packet;

            public MavlinkDeviceInfo(HeartbeatPacket packet)
            {
                _packet = packet;
            }

            public int SystemId => _packet.SystemId;
            public int ComponentId => _packet.ComponenId;
            public ushort FullId => (ushort)(ComponentId | SystemId << 8);
            public uint CustomMode => _packet.Payload.CustomMode;
            public MavType Type => _packet.Payload.Type;
            public MavAutopilot Autopilot => _packet.Payload.Autopilot;
            public MavModeFlag BaseMode => _packet.Payload.BaseMode;
            public MavState SystemStatus => _packet.Payload.SystemStatus;
            public byte MavlinkVersion => _packet.Payload.MavlinkVersion;
        }

        class MavlinkDevice
        {
            private long _lastHit;
            public HeartbeatPacket Packet { get; }

            public MavlinkDevice(HeartbeatPacket packet)
            {
                Packet = packet;
                Touch();
            }

            public DateTime GetLastHit()
            {
                var lastHit = Interlocked.CompareExchange(ref _lastHit, 0, 0);
                return DateTime.FromBinary(lastHit);
            }

            public void Touch()
            {
                Interlocked.Exchange(ref _lastHit, DateTime.Now.ToBinary());
            }

            public IMavlinkDeviceInfo GetInfo()
            {
                return new MavlinkDeviceInfo(Packet);
            }
        }

        public GroundControlStation(GroundControlStationIdentity config, IPacketSequenceCalculator sequenceCalculator = null, int sendHeartBeatMs = 0)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _seq = sequenceCalculator ?? new PacketSequenceCalculator();
            _config = config;
            
            MavlinkV2 = new MavlinkV2Connection(Ports, _ =>
            {
                _.RegisterMinimalDialect();
                _.RegisterCommonDialect();
                _.RegisterArdupilotmegaDialect();
                _.RegisterIcarousDialect();
                _.RegisterUavionixDialect();
            }).DisposeItWith(Disposable);
            Ports.DisposeItWith(Disposable);

            _foundDeviceSubject.DisposeItWith(Disposable);
            _lostDeviceSubject.DisposeItWith(Disposable);
            MavlinkV2.Where(_ => _.MessageId == HeartbeatPacket.PacketMessageId).Cast<HeartbeatPacket>().Subscribe(DeviceFounder).DisposeItWith(Disposable);
            Observable.Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3)).Subscribe(_ => RemoveOldDevice()).DisposeItWith(Disposable); ;

            if (sendHeartBeatMs!=0)
            {
                _transponder = new MavlinkPacketTransponder<HeartbeatPacket, HeartbeatPayload>(MavlinkV2, new MavlinkServerIdentity { ComponentId = _config.ComponentId, SystemId = _config.SystemId }, _seq)
                    .DisposeItWith(Disposable);
                _transponder.Set(_=>
                {
                    _.Autopilot = MavAutopilot.MavAutopilotInvalid;
                    _.BaseMode = 0;
                    _.CustomMode = 0;
                    _.MavlinkVersion = 3;
                    _.SystemStatus = MavState.MavStateActive;
                    _.Type = MavType.MavTypeGcs;
                }).Wait();
                _transponder.Start(TimeSpan.FromMilliseconds(sendHeartBeatMs));
            }

            
        }

       
        

        private void RemoveOldDevice()
        {
            _deviceListLock.EnterUpgradeableReadLock();
            var now = DateTime.Now;
            var deviceToRemove = _info.Where(_ => (now - _.GetLastHit()) > _linkTimeout).ToArray();
            if (deviceToRemove.Length != 0)
            {
                _deviceListLock.EnterWriteLock();
                foreach (var device in deviceToRemove)
                {
                    _info.Remove(device);
                    Logger.Info($"Delete device {JsonConvert.SerializeObject(device.GetInfo())}");
                }
                _deviceListLock.ExitWriteLock();
            }
            _deviceListLock.ExitUpgradeableReadLock();
            foreach (var dev in deviceToRemove)
            {
                _lostDeviceSubject.OnNext(dev.GetInfo());
            }

        }

        private void DeviceFounder(HeartbeatPacket packet)
        {
            MavlinkDevice newItem = null;
            _deviceListLock.EnterUpgradeableReadLock();
            var founded = _info.Find(_ => _.Packet.SystemId == packet.SystemId && _.Packet.ComponenId == packet.ComponenId);
            if (founded != null)
            {
                founded.Touch();
            }
            else
            {
                _deviceListLock.EnterWriteLock();
                newItem = new MavlinkDevice(packet);
                _info.Add(newItem);
                _deviceListLock.ExitWriteLock();
                Logger.Info($"Found new device {JsonConvert.SerializeObject(newItem.GetInfo())}");
            }
            _deviceListLock.ExitUpgradeableReadLock();

            if (newItem != null) _foundDeviceSubject.OnNext(newItem.GetInfo());
        }

        public GroundControlStationIdentity Identity => _config;
        public IPortManager Ports { get; } = new PortManager();
        public IMavlinkV2Connection MavlinkV2 { get; }
        public IObservable<IMavlinkDeviceInfo> OnFoundNewDevice => _foundDeviceSubject;
        public IObservable<IMavlinkDeviceInfo> OnLostDevice => _lostDeviceSubject;

        public IMavlinkDeviceInfo[] Devices
        {
            get
            {
                _deviceListLock.EnterReadLock();
                var items = _info.Select(_ => _.GetInfo()).ToArray();
                _deviceListLock.ExitReadLock();
                return items;
            }
        }

        
    }
}

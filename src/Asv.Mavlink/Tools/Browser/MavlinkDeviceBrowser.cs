using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Newtonsoft.Json;
using NLog;

namespace Asv.Mavlink
{
    public class MavlinkDevice : DisposableOnceWithCancel, IMavlinkDevice
    {
        private long _lastHit;
        private readonly RxValue<MavModeFlag> _baseMode;
        private readonly RxValue<uint> _customMode;
        private readonly RxValue<MavState> _status;
        private readonly Subject<Unit> _ping = new();

        public MavlinkDevice(HeartbeatPacket packet)
        {
            SystemId = packet.SystemId;
            ComponentId = packet.ComponenId;
            MavlinkVersion = packet.Payload.MavlinkVersion;
            Autopilot = packet.Payload.Autopilot;
            Type = packet.Payload.Type;
            _baseMode = new RxValue<MavModeFlag>(packet.Payload.BaseMode).DisposeItWith(Disposable);
            _customMode = new RxValue<uint>(packet.Payload.CustomMode).DisposeItWith(Disposable);
            _status = new RxValue<MavState>(packet.Payload.SystemStatus).DisposeItWith(Disposable);
            Update(packet.Payload);
        }

        public void Update(HeartbeatPayload packetPayload)
        {
            Touch();

            if (_baseMode.Value.Equals(packetPayload.BaseMode) == false)
            {
                _baseMode.OnNext(packetPayload.BaseMode);
            }

            if (_customMode.Value.Equals(packetPayload.CustomMode) == false)
            {
                _customMode.OnNext(packetPayload.CustomMode);
            }

            if (_status.Value.Equals(packetPayload.SystemStatus) == false)
            {
                _status.OnNext(packetPayload.SystemStatus);
            }
        }

        public DateTime GetLastHit()
        {
            var lastHit = Interlocked.CompareExchange(ref _lastHit, 0, 0);
            return DateTime.FromBinary(lastHit);
        }

        private void Touch()
        {
            Interlocked.Exchange(ref _lastHit, DateTime.Now.ToBinary());
            _ping.OnNext(Unit.Default);
        }

        public byte SystemId { get; }
        public byte ComponentId { get; }
        public ushort FullId => (ushort)(ComponentId | SystemId << 8);
        public MavType Type { get; }
        public MavAutopilot Autopilot { get; }
        public byte MavlinkVersion { get; }
        public IRxValue<MavModeFlag> BaseMode => _baseMode;
        public IRxValue<uint> CustomMode => _customMode;
        public IRxValue<MavState> SystemStatus => _status;

        public IObservable<Unit> Ping => _ping;

        public override string ToString()
        {
            return $"{Type:G}.{Autopilot:G}[{SystemId}:{ComponentId}]";
        }
    }

    public class MavlinkDeviceBrowser : DisposableOnceWithCancel, IMavlinkDeviceBrowser
    {
        private readonly ReaderWriterLockSlim _deviceListLock = new();
        private readonly List<MavlinkDevice> _info = new();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Subject<IMavlinkDevice> _foundDeviceSubject;
        private readonly Subject<IMavlinkDevice> _lostDeviceSubject;
        private readonly RxValue<TimeSpan> _deviceTimeout;

        public MavlinkDeviceBrowser(IMavlinkV2Connection connection, TimeSpan deviceTimeout)
        {
            connection
                .Filter<HeartbeatPacket>()
                .Subscribe(UpdateDevice)
                .DisposeItWith(Disposable);
            Observable
                .Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3))
                .Subscribe(RemoveOldDevice)
                .DisposeItWith(Disposable);
            _foundDeviceSubject = new Subject<IMavlinkDevice>().DisposeItWith(Disposable);
            _lostDeviceSubject = new Subject<IMavlinkDevice>().DisposeItWith(Disposable);
            _deviceTimeout = new RxValue<TimeSpan>(deviceTimeout).DisposeItWith(Disposable);
        }

        private void RemoveOldDevice(long l)
        {
            _deviceListLock.EnterUpgradeableReadLock();
            var now = DateTime.Now;
            var deviceToRemove = _info.Where(_ => (now - _.GetLastHit()) > _deviceTimeout.Value).ToArray();
            if (deviceToRemove.Length != 0)
            {
                _deviceListLock.EnterWriteLock();
                foreach (var device in deviceToRemove)
                {
                    _info.Remove(device);
                    Logger.Info($"Delete device {device}");
                }
                _deviceListLock.ExitWriteLock();
            }
            _deviceListLock.ExitUpgradeableReadLock();
            foreach (var dev in deviceToRemove)
            {
                try
                {
                    _lostDeviceSubject.OnNext(dev);
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Error to publish remove device event '{dev}':{e.Message}");
                    if (Debugger.IsAttached) Debugger.Break();
                }
                
            }
        }

        private void UpdateDevice(HeartbeatPacket packet)
        {
            MavlinkDevice newItem = null;
            _deviceListLock.EnterUpgradeableReadLock();
            var founded = _info.Find(_ => 
                _.SystemId == packet.SystemId && 
                _.ComponentId == packet.ComponenId && 
                _.Type == packet.Payload.Type && 
                _.MavlinkVersion == packet.Payload.MavlinkVersion &&
                _.Autopilot == packet.Payload.Autopilot);
            if (founded != null)
            {
                founded.Update(packet.Payload);
            }
            else
            {
                _deviceListLock.EnterWriteLock();
                newItem = new MavlinkDevice(packet);
                _info.Add(newItem);
                _deviceListLock.ExitWriteLock();
                Logger.Info($"Found new device {JsonConvert.SerializeObject(newItem.ToString())}");
            }
            _deviceListLock.ExitUpgradeableReadLock();
            try
            {
                if (newItem != null) _foundDeviceSubject.OnNext(newItem);
            }
            catch (Exception e)
            {
                Logger.Error(e,$"Error to publish new device event '{newItem}':{e.Message}");
                if (Debugger.IsAttached) Debugger.Break();
            }
            
        }

        public IObservable<IMavlinkDevice> OnFoundDevice => _foundDeviceSubject;

        public IObservable<IMavlinkDevice> OnLostDevice => _lostDeviceSubject;

        public IRxEditableValue<TimeSpan> DeviceTimeout => _deviceTimeout;

        public IMavlinkDevice[] Devices
        {
            get
            {
                _deviceListLock.EnterReadLock();
                var items = _info.Cast<IMavlinkDevice>().ToArray();
                _deviceListLock.ExitReadLock();
                return items;
            }
        }
    }
}
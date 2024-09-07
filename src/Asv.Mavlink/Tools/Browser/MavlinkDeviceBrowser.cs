#nullable enable
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using ZLogger;

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
            ComponentId = packet.ComponentId;
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
        public ushort FullId => MavlinkHelper.ConvertToFullId(ComponentId , SystemId);
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
        private readonly ILogger _logger;
        private readonly RxValue<TimeSpan> _deviceTimeout;
        private readonly SourceCache<MavlinkDevice,ushort> _deviceCache;

        public MavlinkDeviceBrowser(IMavlinkV2Connection connection, TimeSpan deviceTimeout, IScheduler? scheduler = null, ILogger? logger = null)
        {
            _logger = logger ?? NullLogger.Instance;
            connection
                .Filter<HeartbeatPacket>()
                .Subscribe(UpdateDevice)
                .DisposeItWith(Disposable);
            Observable
                .Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3))
                .Subscribe(RemoveOldDevice)
                .DisposeItWith(Disposable);
            _deviceCache = new SourceCache<MavlinkDevice, ushort>(x => x.FullId).DisposeItWith(Disposable);
            _deviceTimeout = new RxValue<TimeSpan>(deviceTimeout).DisposeItWith(Disposable);
            
            if (scheduler != null)
            {
                Devices = _deviceCache.Connect().ObserveOn(scheduler).Transform(d => (IMavlinkDevice)d).RefCount();    
            }
            else
            {
                Devices = _deviceCache.Connect().Transform(d => (IMavlinkDevice)d).RefCount();
            }
            
        }

        private void RemoveOldDevice(long l)
        {
            _deviceCache.Edit(update =>
            {
                var now = DateTime.Now;
                var itemsToDelete = update.Items.Where(device => (now - device.GetLastHit()) > _deviceTimeout.Value).ToList();
                foreach (var item in itemsToDelete)
                {
                    item.Dispose();
                }
                update.RemoveKeys(itemsToDelete.Select(device=>device.FullId));
            });
        }

        private void UpdateDevice(HeartbeatPacket packet)
        {
            _deviceCache.Edit(update =>
            {
                var item = update.Lookup(packet.FullId);
                if (item.HasValue)
                {
                    item.Value.Update(packet.Payload);
                }
                else
                {
                    var newItem = new MavlinkDevice(packet);
                    update.AddOrUpdate(newItem);
                    _logger.ZLogInformation($"Found new device {JsonConvert.SerializeObject(newItem.ToString())}");
                }
            });
        }
        public IObservable<IChangeSet<IMavlinkDevice, ushort>> Devices { get; }
        public IRxEditableValue<TimeSpan> DeviceTimeout => _deviceTimeout;

       
    }
}
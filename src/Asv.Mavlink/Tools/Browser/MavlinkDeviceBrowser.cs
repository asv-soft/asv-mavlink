#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using ZLogger;
using R3;

namespace Asv.Mavlink
{
    public sealed class MavlinkDevice : IMavlinkDevice, IDisposable, IAsyncDisposable
    {
        private long _lastHit;
        private readonly ReactiveProperty<MavModeFlag> _baseMode;
        private readonly ReactiveProperty<uint> _customMode;
        private readonly ReactiveProperty<MavState> _status;
        private readonly Subject<Unit> _ping = new();

        public MavlinkDevice(HeartbeatPacket packet)
        {
            SystemId = packet.SystemId;
            ComponentId = packet.ComponentId;
            MavlinkVersion = packet.Payload.MavlinkVersion;
            Autopilot = packet.Payload.Autopilot;
            Type = packet.Payload.Type;
            _baseMode = new ReactiveProperty<MavModeFlag>(packet.Payload.BaseMode);
            _customMode = new ReactiveProperty<uint>(packet.Payload.CustomMode);
            _status = new ReactiveProperty<MavState>(packet.Payload.SystemStatus);
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
        public MavlinkIdentity FullId => new(SystemId,ComponentId);
        public MavType Type { get; }
        public MavAutopilot Autopilot { get; }
        public byte MavlinkVersion { get; }
        public ReactiveProperty<MavModeFlag> BaseMode => _baseMode;
        public ReactiveProperty<uint> CustomMode => _customMode;
        public ReactiveProperty<MavState> SystemStatus => _status;

        public Observable<Unit> Ping => _ping;

        public override string ToString()
        {
            return $"{Type:G}.{Autopilot:G}[{SystemId}:{ComponentId}]";
        }

        public void Dispose()
        {
            _baseMode.Dispose();
            _customMode.Dispose();
            _status.Dispose();
            _ping.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await CastAndDispose(_baseMode).ConfigureAwait(false);
            await CastAndDispose(_customMode).ConfigureAwait(false);
            await CastAndDispose(_status).ConfigureAwait(false);
            await CastAndDispose(_ping).ConfigureAwait(false);

            return;

            static async ValueTask CastAndDispose(IDisposable resource)
            {
                if (resource is IAsyncDisposable resourceAsyncDisposable)
                    await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
                else
                    resource.Dispose();
            }
        }
    }

    public sealed class MavlinkDeviceBrowser : IMavlinkDeviceBrowser, IDisposable,IAsyncDisposable
    {
        private readonly ILogger _logger;
        private readonly ReactiveProperty<TimeSpan> _deviceTimeout;
        private readonly SourceCache<MavlinkDevice,MavlinkIdentity> _deviceCache;
        private readonly ITimer _timer;
        private readonly IDisposable _sub1;

        public MavlinkDeviceBrowser(IMavlinkV2Connection connection, TimeSpan deviceTimeout, TimeProvider? timeProvider = null, ILoggerFactory? logFactory = null)
        {
            logFactory??=NullLoggerFactory.Instance;
            _logger = logFactory.CreateLogger<MavlinkDeviceBrowser>();
            _sub1 = connection
                .Filter<HeartbeatPacket>()
                .Subscribe(UpdateDevice);
            timeProvider ??= TimeProvider.System;
            _timer = timeProvider.CreateTimer(RemoveOldDevice,null,TimeSpan.FromSeconds(3),TimeSpan.FromSeconds(3));
           
            _deviceCache = new SourceCache<MavlinkDevice, MavlinkIdentity>(x => x.FullId);
            _deviceTimeout = new ReactiveProperty<TimeSpan>(deviceTimeout);
            Devices = _deviceCache.Connect().DisposeMany().Transform(d => (IMavlinkDevice)d).RefCount();
        }

        private void RemoveOldDevice(object? state)
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
        public IObservable<IChangeSet<IMavlinkDevice, MavlinkIdentity>> Devices { get; }
        public ReactiveProperty<TimeSpan> DeviceTimeout => _deviceTimeout;


        public void Dispose()
        {
            _deviceCache.Clear();
            _deviceTimeout.Dispose();
            _deviceCache.Dispose();
            _timer.Dispose();
            _sub1.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            _deviceCache.Clear();
            await CastAndDispose(_deviceTimeout).ConfigureAwait(false);
            await CastAndDispose(_deviceCache).ConfigureAwait(false);
            await _timer.DisposeAsync().ConfigureAwait(false);
            await CastAndDispose(_sub1).ConfigureAwait(false);

            return;

            static async ValueTask CastAndDispose(IDisposable resource)
            {
                if (resource is IAsyncDisposable resourceAsyncDisposable)
                    await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
                else
                    resource.Dispose();
            }
        }
    }
}
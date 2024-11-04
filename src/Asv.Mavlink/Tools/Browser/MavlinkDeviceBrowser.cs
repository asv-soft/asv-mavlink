#nullable enable
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ObservableCollections;
using ZLogger;
using R3;

namespace Asv.Mavlink
{
    public sealed class MavlinkDevice : IDisposable, IAsyncDisposable
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
        }

        internal void Update(HeartbeatPayload packetPayload,long time)
        {
            _baseMode.OnNext(packetPayload.BaseMode);
            _customMode.OnNext(packetPayload.CustomMode);
            _status.OnNext(packetPayload.SystemStatus);
            Interlocked.Exchange(ref _lastHit, time);
            _ping.OnNext(Unit.Default);
        }

        internal long LastHit => Interlocked.Read(ref _lastHit);
        public byte SystemId { get; }
        public byte ComponentId { get; }
        public MavlinkIdentity FullId => new(SystemId,ComponentId);
        public MavType Type { get; }
        public MavAutopilot Autopilot { get; }
        public byte MavlinkVersion { get; }
        public ReadOnlyReactiveProperty<MavModeFlag> BaseMode => _baseMode;
        public ReadOnlyReactiveProperty<uint> CustomMode => _customMode;
        public ReadOnlyReactiveProperty<MavState> SystemStatus => _status;
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

    public class MavlinkDeviceBrowserConfig
    {
        public int DeviceTimeoutMs { get; set; } = 30_000;
        public int DeviceCheckOldTimeoutMs { get; set; } = 3_000;
    }
    
    public sealed class MavlinkDeviceBrowser : IMavlinkDeviceBrowser, IDisposable,IAsyncDisposable
    {
        private readonly TimeProvider _timeProvider;
        private readonly ILogger _logger;
        private readonly ReactiveProperty<TimeSpan> _deviceTimeout;
        private readonly ObservableDictionary<MavlinkIdentity,MavlinkDevice> _deviceCache;
        private readonly ITimer _timer;
        private readonly IDisposable _sub1;

        public MavlinkDeviceBrowser(IMavlinkV2Connection connection, MavlinkDeviceBrowserConfig config, TimeProvider? timeProvider = null, ILoggerFactory? logFactory = null)
        {
            _timeProvider = timeProvider ?? TimeProvider.System;
            ArgumentNullException.ThrowIfNull(connection);
            Config = config ?? throw new ArgumentNullException(nameof(config));
            logFactory??=NullLoggerFactory.Instance;
            _logger = logFactory.CreateLogger<MavlinkDeviceBrowser>();
            _sub1 = connection
                .Filter<HeartbeatPacket>()
                .Subscribe(UpdateDevice);
            _timer = _timeProvider.CreateTimer(RemoveOldDevice,null,TimeSpan.FromMilliseconds(config.DeviceCheckOldTimeoutMs),TimeSpan.FromMilliseconds(config.DeviceCheckOldTimeoutMs));
           
            _deviceCache = new ObservableDictionary<MavlinkIdentity,MavlinkDevice>();
            _deviceTimeout = new ReactiveProperty<TimeSpan>(TimeSpan.FromMilliseconds(config.DeviceTimeoutMs));
            _sub1 = _deviceTimeout.Subscribe(x=>Config.DeviceTimeoutMs = (int)x.TotalMilliseconds);
        }

        public MavlinkDeviceBrowserConfig Config { get; }

        private void RemoveOldDevice(object? state)
        {
            var itemsToDelete = _deviceCache
                .Where(device => _timeProvider.GetElapsedTime(device.Value.LastHit) > _deviceTimeout.Value)
                .ToImmutableArray();
            foreach (var item in itemsToDelete)
            {
                item.Value.Dispose();
                _deviceCache.Remove(item.Key);
            }
        }

        private void UpdateDevice(HeartbeatPacket packet)
        {
            if (_deviceCache.TryGetValue(packet.FullId, out var device) == false)
            {
                device = new MavlinkDevice(packet);
                _logger.ZLogInformation($"Found new device {device}");
            }
            device.Update(packet.Payload,_timeProvider.GetTimestamp());
        }

        public IReadOnlyObservableDictionary<MavlinkIdentity, MavlinkDevice> Devices => _deviceCache;
        public ReactiveProperty<TimeSpan> DeviceTimeout => _deviceTimeout;

        public void Dispose()
        {
            foreach (var item in _deviceCache)
            {
                item.Value.Dispose();
            }
            _deviceCache.Clear();
            _deviceTimeout.Dispose();
            _timer.Dispose();
            _sub1.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var item in _deviceCache)
            {
                await item.Value.DisposeAsync().ConfigureAwait(false);
            }
            _deviceCache.Clear();
            await CastAndDispose(_deviceTimeout).ConfigureAwait(false);
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
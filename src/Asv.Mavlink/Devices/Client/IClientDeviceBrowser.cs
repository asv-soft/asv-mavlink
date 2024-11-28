using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Minimal;

using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public interface IClientDeviceBrowser
{
    IReadOnlyObservableDictionary<MavlinkIdentity, IClientDevice> Devices { get; }
    ReactiveProperty<TimeSpan> DeviceTimeout { get; }
}

public class DeviceBrowserConfig
{
    public int DeviceTimeoutMs { get; set; } = 30_000;
    public int DeviceCheckIntervalMs { get; set; } = 1000;
}

public sealed class ClientDeviceBrowser : IClientDeviceBrowser, IDisposable,IAsyncDisposable
{
    private readonly IClientDeviceFactory _factory;
    private readonly ICoreServices _core;
    private readonly ObservableDictionary<MavlinkIdentity,IClientDevice> _deviceCache;
    private readonly ConcurrentDictionary<MavlinkIdentity,long> _lastUpdateTime = new ();
    
    private readonly IDisposable _subscribe1;
    private readonly ILogger _logger;
    private readonly ReactiveProperty<TimeSpan> _deviceTimeout;
    private readonly ITimer _timer;

    public ClientDeviceBrowser(IClientDeviceFactory factory, DeviceBrowserConfig config,ICoreServices core)
    {
        _factory = factory;
        _core = core;
        _logger = core.Log.CreateLogger<ClientDeviceBrowser>();
        _deviceCache = new ObservableDictionary<MavlinkIdentity,IClientDevice>();
        _deviceTimeout = new ReactiveProperty<TimeSpan>(TimeSpan.FromMilliseconds(config.DeviceTimeoutMs));
        _subscribe1 = core.Connection
            .RxFilterByType<HeartbeatPacket>()
            .Subscribe(UpdateDevice);
        _timer = core.TimeProvider.CreateTimer(RemoveOldDevices, null, TimeSpan.FromMilliseconds(config.DeviceCheckIntervalMs), TimeSpan.FromMilliseconds(config.DeviceCheckIntervalMs));
    }

    private void RemoveOldDevices(object? state)
    {
        var itemsToDelete = _lastUpdateTime
            .Where(x => _core.TimeProvider.GetElapsedTime(x.Value) > _deviceTimeout.Value).ToImmutableArray();
        if (itemsToDelete.Length == 0) return;
        foreach (var item in itemsToDelete)
        {
            if (_deviceCache.TryGetValue(item.Key, out var device))
            {
                ((ClientDevice)device).Dispose();
            }
            _deviceCache.Remove(item.Key);
            _lastUpdateTime.TryRemove(item.Key, out _);
            _logger.ZLogInformation($"Remove device {item.Key}");
        }
    }
    public ReactiveProperty<TimeSpan> DeviceTimeout => _deviceTimeout;
    private void UpdateDevice(HeartbeatPacket packet)
    {
        _lastUpdateTime.AddOrUpdate(packet.FullId, _core.TimeProvider.GetTimestamp(),
            (_, _) => _core.TimeProvider.GetTimestamp());
        
        if (_deviceCache.TryGetValue(packet.FullId, out var item) == true) return;
        var device = _factory.Create(packet, _core);
        if (device == null)
        {
            _logger.ZLogWarning($"Device provider for {packet.FullId} not found");
            return;
        }
        _deviceCache[packet.FullId] = device;
        _logger.ZLogInformation($"Found new device {packet.FullId}");
    }

    public IReadOnlyObservableDictionary<MavlinkIdentity,IClientDevice> Devices => _deviceCache;

    public void Dispose()
    {
        _subscribe1.Dispose();
        _deviceTimeout.Dispose();
        _timer.Dispose();
        var items = _deviceCache.Select(x => (ClientDevice)x.Value).ToImmutableArray();
        _deviceCache.Clear();
        _lastUpdateTime.Clear();
        foreach (var item in items)
        {
            item.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_subscribe1).ConfigureAwait(false);
        await CastAndDispose(_deviceTimeout).ConfigureAwait(false);
        await _timer.DisposeAsync().ConfigureAwait(false);

        var items = _deviceCache.Select(x => (ClientDevice)x.Value).ToImmutableArray();
        _deviceCache.Clear();
        _lastUpdateTime.Clear();
        foreach (var item in items)
        {
            await item.DisposeAsync().ConfigureAwait(false);
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
}
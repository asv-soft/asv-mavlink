using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;
using ZLogger;

namespace Asv.Mavlink.Diagnostic.Client;

public class DiagnosticClientConfig
{
    public int DeleteProbesTimeoutMs { get; set; } = 60_000;
    public int CheckProbesDelayMs { get; set; } = 10_000;
    public int MaxCollectionSize { get; set; } = 100;
}


internal class ClientNamedProbe<T> : INamedProbe<T>
{
    private readonly ReactiveProperty<(TimeSpan,T)> _value;
    private long _lastUpdate;


    public ClientNamedProbe(string name, T value, uint payloadTimeBootMs, long updateLocalTimestamp)
    {
        Name = name;
        _value = new ReactiveProperty<(TimeSpan,T)>(new (TimeSpan.FromMilliseconds(payloadTimeBootMs),value));
        _lastUpdate = updateLocalTimestamp;
    }

    public string Name { get; }
    public ReadOnlyReactiveProperty<(TimeSpan,T)> Value => _value;
    
    public long LastUpdateTimestamp => _lastUpdate;
    public void Dispose()
    {
        _value.Dispose();
    }

    public void Update(T payloadValue, uint payloadTimeBootMs, long updateLocalTimestamp)
    {
        _value.OnNext(new (TimeSpan.FromMilliseconds(payloadTimeBootMs), payloadValue));
        _lastUpdate = updateLocalTimestamp;
    }
}

public class DiagnosticClient:MavlinkMicroserviceClient,IDiagnosticClient
{
    private readonly DiagnosticClientConfig _config;
    private readonly ObservableDictionary<string,INamedProbe<float>> _floatProbes;
    private readonly ObservableDictionary<string,INamedProbe<int>> _intProbes;
    private int _deleteProbesLock;
    private readonly ILogger _logger;
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly ITimer _timer;

    public DiagnosticClient(MavlinkClientIdentity identity,DiagnosticClientConfig config,ICoreServices core) 
        : base("DIAG", identity, core)
    {
        _logger = core.Log.CreateLogger<DiagnosticClient>();
        _config = config;
        _floatProbes = new ObservableDictionary<string,INamedProbe<float>>();
        _intProbes = new ObservableDictionary<string,INamedProbe<int>>();
        _sub1 = InternalFilter<NamedValueFloatPacket>()
            .Subscribe(OnRecvFloat);
        _sub2 = InternalFilter<NamedValueIntPacket>()
            .Subscribe(OnRecvInt);
        
        var time = TimeSpan.FromMilliseconds(config.DeleteProbesTimeoutMs);
        DebugFloatArray = InternalFilter<DebugFloatArrayPacket>().Select(x => x.Payload);
        MemoryVector = InternalFilter<MemoryVectPacket>().Select(x => x.Payload);
        _timer = core.TimeProvider.CreateTimer(RemoveOldItems, null, time, time);
    }

    private void RemoveOldItems(object? state)
    {
        if (Interlocked.Exchange(ref _deleteProbesLock, 1) == 1) return;
        try
        {
            if (_floatProbes.Count != 0)
            {
                var itemsToDelete = _floatProbes.Select(x=>x.Value)
                    .Cast<ClientNamedProbe<float>>()
                    .Where(y => Core.TimeProvider.GetElapsedTime(y.LastUpdateTimestamp).TotalMilliseconds > _config.DeleteProbesTimeoutMs)
                    .ToImmutableArray();
                if (itemsToDelete.Length != 0)
                {
                    _logger.ZLogTrace($"Remove {string.Join(',',itemsToDelete.Select(p=>p.Name))} float probes by timeout {_config.DeleteProbesTimeoutMs} ms");
                }

                foreach (var key in itemsToDelete.Select(x=>x.Name))
                {
                    _floatProbes.Remove(key);
                }
            }
            if (_intProbes.Count != 0)
            {
                var itemsToDelete = _intProbes.Select(x=>x.Value)
                    .Cast<ClientNamedProbe<int>>()
                    .Where(y => Core.TimeProvider.GetElapsedTime(y.LastUpdateTimestamp).TotalMilliseconds > _config.DeleteProbesTimeoutMs)
                    .ToImmutableArray();
                if (itemsToDelete.Length != 0)
                {
                    _logger.ZLogTrace($"Remove {string.Join(',',itemsToDelete.Select(p=>p.Name))} int probes by timeout {_config.DeleteProbesTimeoutMs} ms");
                }
                foreach (var key in itemsToDelete.Select(x=>x.Name))
                {
                    _intProbes.Remove(key);
                }
            }
        }
        finally
        {
            Interlocked.Exchange(ref _deleteProbesLock, 0);
        }
    }

    private void OnRecvInt(NamedValueIntPacket namedValueFloatPacket)
    {
        var name = MavlinkTypesHelper.GetString(namedValueFloatPacket.Payload.Name);
        if (_intProbes.TryGetValue(name, out var found) == true)
        {
            var probe = (ClientNamedProbe<int>)found;
            probe.Update(namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs, Core.TimeProvider.GetTimestamp());
        }
        else
        {
            _intProbes.Add(name, new ClientNamedProbe<int>(name, namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs,Core.TimeProvider.GetTimestamp()));
        }
        if (_config.MaxCollectionSize > 0 && _intProbes.Count > _config.MaxCollectionSize)
        {
            var itemsToDelete = _intProbes
                .Select(x=>x.Value)
                .Cast<ClientNamedProbe<int>>()
                .OrderBy(y => y.LastUpdateTimestamp)
                .Take(_config.MaxCollectionSize - _intProbes.Count).ToImmutableArray();
            foreach (var key in itemsToDelete.Select(x=>x.Name))
            {
                _intProbes.Remove(key);
            }
        }
    }
    private void OnRecvFloat(NamedValueFloatPacket namedValueFloatPacket)
    {
        var name = MavlinkTypesHelper.GetString(namedValueFloatPacket.Payload.Name);
        if (_floatProbes.TryGetValue(name, out var found) == true)
        {
            var probe = (ClientNamedProbe<float>)found;
            probe.Update(namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs, Core.TimeProvider.GetTimestamp());
        }
        else
        {
            _floatProbes.Add(name, new ClientNamedProbe<float>(name, namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs,Core.TimeProvider.GetTimestamp()));
        }
        if (_config.MaxCollectionSize > 0 && _floatProbes.Count > _config.MaxCollectionSize)
        {
            var itemsToDelete = _floatProbes
                .Select(x=>x.Value)
                .Cast<ClientNamedProbe<int>>()
                .OrderBy(y => y.LastUpdateTimestamp)
                .Take(_config.MaxCollectionSize - _floatProbes.Count).ToImmutableArray();
            foreach (var key in itemsToDelete.Select(x=>x.Name))
            {
                _floatProbes.Remove(key);
            }
        }
    }

    public IReadOnlyObservableDictionary<string, INamedProbe<float>> FloatProbes => _floatProbes;
    public IReadOnlyObservableDictionary<string, INamedProbe<int>> IntProbes => _intProbes;
    public Observable<DebugFloatArrayPayload> DebugFloatArray { get; }
    public Observable<MemoryVectPayload> MemoryVector { get; }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _sub1.Dispose();
            _sub2.Dispose();
            _timer.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await _timer.DisposeAsync().ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

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
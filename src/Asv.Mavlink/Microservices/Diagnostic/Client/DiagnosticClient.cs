using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
    private readonly RxValue<(TimeSpan,T)> _value;
    private long _lastUpdate;


    public ClientNamedProbe(string name, T value, uint payloadTimeBootMs, long updateLocalTimestamp)
    {
        Name = name;
        _value = new RxValue<(TimeSpan,T)>(new (TimeSpan.FromMilliseconds(payloadTimeBootMs),value));
        _lastUpdate = updateLocalTimestamp;
    }

    public string Name { get; }
    public IRxValue<(TimeSpan,T)> Value => _value;
    
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
    private readonly SourceCache<INamedProbe<float>,string> _floatProbes;
    private readonly SourceCache<INamedProbe<int>,string> _intProbes;
    private int _deleteProbesLock;
    private readonly ILogger _logger;
    private readonly IDisposable _disposeIt;

    public DiagnosticClient(MavlinkClientIdentity identity,DiagnosticClientConfig config,ICoreServices core) 
        : base("DIAG", identity, core)
    {
        _logger = core.Log.CreateLogger<DiagnosticClient>();
        _config = config;
        _floatProbes = new SourceCache<INamedProbe<float>, string>(x => x.Name);
        _intProbes = new SourceCache<INamedProbe<int>, string>(x => x.Name);
        FloatProbes = _floatProbes.Connect().RefCount();
        IntProbes = _intProbes.Connect().RefCount();
        var d1 = InternalFilter<NamedValueFloatPacket>()
            .Subscribe(OnRecvFloat);
        var d2 = InternalFilter<NamedValueIntPacket>()
            .Subscribe(OnRecvInt);
        if (config.DeleteProbesTimeoutMs > 0)
        {
            var time = TimeSpan.FromMilliseconds(config.DeleteProbesTimeoutMs);
            var timer = core.TimeProvider.CreateTimer(RemoveOldItems, null, time, time);
            _disposeIt = Disposable.Combine(_floatProbes,_intProbes,d1,d2,timer);
        }
        else
        {
            _disposeIt = Disposable.Combine(_floatProbes,_intProbes,d1,d2);
        }
    }

    private void RemoveOldItems(object? state)
    {
        if (Interlocked.Exchange(ref _deleteProbesLock, 1) == 1) return;
        try
        {
            if (_floatProbes.Count != 0)
            {
                _floatProbes.Edit(x =>
                {
                    var itemsToDelete = x.Items
                        .Cast<ClientNamedProbe<float>>()
                        .Where(y => Core.TimeProvider.GetElapsedTime(y.LastUpdateTimestamp).TotalMilliseconds > _config.DeleteProbesTimeoutMs)
                        .ToImmutableArray();
                    if (itemsToDelete.Length != 0)
                    {
                        _logger.ZLogTrace($"Remove {string.Join(',',itemsToDelete.Select(p=>p.Name))} float probes by timeout {_config.DeleteProbesTimeoutMs} ms");
                    }
                    x.Remove(itemsToDelete);
                });    
            }
            
            _intProbes.Edit(x =>
            {
                var itemsToDelete = x.Items
                    .Cast<ClientNamedProbe<int>>()
                    .Where(y => Core.TimeProvider.GetElapsedTime(y.LastUpdateTimestamp).TotalMilliseconds > _config.DeleteProbesTimeoutMs)
                    .ToImmutableArray();
                if (itemsToDelete.Length != 0)
                {
                    _logger.ZLogTrace($"Remove {string.Join(',',itemsToDelete.Select(p=>p.Name))} int probes by timeout {_config.DeleteProbesTimeoutMs} ms");
                }
                x.Remove(itemsToDelete);
            });
        }
        finally
        {
            Interlocked.Exchange(ref _deleteProbesLock, 0);
        }
    }

    private void OnRecvInt(NamedValueIntPacket namedValueFloatPacket)
    {
        _intProbes.Edit(x =>
        {
            var name = MavlinkTypesHelper.GetString(namedValueFloatPacket.Payload.Name);
            var found = x.Lookup(name);
            if (found.HasValue)
            {
                var probe = (ClientNamedProbe<int>)found.Value;
                probe.Update(namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs, Core.TimeProvider.GetTimestamp());
            }
            else
            {
                x.AddOrUpdate(new ClientNamedProbe<int>(name, namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs,Core.TimeProvider.GetTimestamp()));
                if (_config.MaxCollectionSize > 0 && x.Count > _config.MaxCollectionSize)
                {
                    var itemsToDelete = x.Items.Cast<ClientNamedProbe<int>>().OrderBy(y => y.LastUpdateTimestamp)
                        .Take(_config.MaxCollectionSize - x.Count).ToImmutableArray();
                    x.Remove(itemsToDelete);
                }
            }
        });
    }
    private void OnRecvFloat(NamedValueFloatPacket namedValueFloatPacket)
    {
        _floatProbes.Edit(x =>
        {
            var name = MavlinkTypesHelper.GetString(namedValueFloatPacket.Payload.Name);
            var found = x.Lookup(name);
            if (found.HasValue)
            {
                var probe = (ClientNamedProbe<float>)found.Value;
                probe.Update(namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs,Core.TimeProvider.GetTimestamp());
            }
            else
            {
                x.AddOrUpdate(new ClientNamedProbe<float>(name, namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs,Core.TimeProvider.GetTimestamp()));
                if (_config.MaxCollectionSize > 0 && x.Count > _config.MaxCollectionSize)
                {
                    var itemsToDelete = x.Items.Cast<ClientNamedProbe<float>>().OrderBy(y => y.LastUpdateTimestamp)
                        .Take(_config.MaxCollectionSize - x.Count).ToImmutableArray();
                    x.Remove(itemsToDelete);
                }
            }
        });
    }

    public IObservable<IChangeSet<INamedProbe<float>, string>> FloatProbes { get; }
    public IObservable<IChangeSet<INamedProbe<int>, string>> IntProbes { get; }

    public IObservable<DebugFloatArrayPayload> DebugFloatArray =>
        InternalFilter<DebugFloatArrayPacket>().Select(x => x.Payload);
    public IObservable<MemoryVectPayload> MemoryVector => 
        InternalFilter<MemoryVectPacket>().Select(x => x.Payload);

    public override void Dispose()
    {
        _disposeIt.Dispose();
        base.Dispose();
    }
}
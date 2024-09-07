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

namespace Asv.Mavlink.Diagnostic.Client;

public class DiagnosticClientConfig
{
    public int DeleteProbesTimeoutMs { get; set; } = 60_000;
    public int MaxCollectionSize { get; set; } = 100;
}


internal class ClientNamedProbe<T> : INamedProbe<T>
{
    private readonly RxValue<(TimeSpan,T)> _value;
    private DateTime _lastUpdate;


    public ClientNamedProbe(string name, T value, uint payloadTimeBootMs)
    {
        Name = name;
        _value = new RxValue<(TimeSpan,T)>(new (TimeSpan.FromMilliseconds(payloadTimeBootMs),value));
        _lastUpdate = DateTime.Now;
    }

    public string Name { get; }
    public IRxValue<(TimeSpan,T)> Value => _value;
    
    public DateTime LastUpdate => _lastUpdate;
    public void Dispose()
    {
        _value.Dispose();
    }

    public void Update(T payloadValue, uint payloadTimeBootMs)
    {
        _value.OnNext(new (TimeSpan.FromMilliseconds(payloadTimeBootMs), payloadValue));
        _lastUpdate = DateTime.Now;
    }
}

public class DiagnosticClient:MavlinkMicroserviceClient,IDiagnosticClient
{
    private readonly DiagnosticClientConfig _config;
    private readonly SourceCache<INamedProbe<float>,string> _floatProbes;
    private readonly SourceCache<INamedProbe<int>,string> _intProbes;
    private int _deleteProbesLock;
    private readonly ILogger _logger;

    public DiagnosticClient(
        DiagnosticClientConfig config, 
        IMavlinkV2Connection connection, 
        MavlinkClientIdentity identity, 
        IPacketSequenceCalculator seq, 
        IScheduler? scheduler,
        ILogger? logger = null) 
        : base("DIAG", connection, identity, seq,scheduler,logger)
    {
        _logger = logger ?? NullLogger.Instance;
        _config = config;
        _floatProbes = new SourceCache<INamedProbe<float>, string>(x => x.Name).DisposeItWith(Disposable);
        _intProbes = new SourceCache<INamedProbe<int>, string>(x => x.Name).DisposeItWith(Disposable);
        FloatProbes = _floatProbes.Connect().RefCount();
        IntProbes = _intProbes.Connect().RefCount();
        InternalFilter<NamedValueFloatPacket>()
            .Subscribe(OnRecvFloat).DisposeItWith(Disposable);
        InternalFilter<NamedValueIntPacket>()
            .Subscribe(OnRecvInt).DisposeItWith(Disposable);
        if (config.DeleteProbesTimeoutMs > 0)
        {
            Observable.Timer(TimeSpan.FromMilliseconds(config.DeleteProbesTimeoutMs),TimeSpan.FromMilliseconds(config.DeleteProbesTimeoutMs))
                .ObserveOn(Scheduler)
                .Subscribe(RemoveOldItems).DisposeItWith(Disposable);    
        }
    }

    private void RemoveOldItems(long l)
    {
        if (Interlocked.Exchange(ref _deleteProbesLock, 1) == 1) return;
        try
        {
            _floatProbes.Edit(x =>
            {
                var itemsToDelete = x.Items.Cast<ClientNamedProbe<float>>().Where(y => y.LastUpdate.Add(TimeSpan.FromMilliseconds(_config.DeleteProbesTimeoutMs)) < DateTime.Now)
                    .ToImmutableArray();
                x.Remove(itemsToDelete);
            });
            _intProbes.Edit(x =>
            {
                var itemsToDelete = x.Items.Cast<ClientNamedProbe<int>>().Where(y => y.LastUpdate.Add(TimeSpan.FromMilliseconds(_config.DeleteProbesTimeoutMs)) < DateTime.Now)
                    .ToImmutableArray();
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
                probe.Update(namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs);
            }
            else
            {
                x.AddOrUpdate(new ClientNamedProbe<int>(name, namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs));
                if (_config.MaxCollectionSize > 0 && x.Count > _config.MaxCollectionSize)
                {
                    var itemsToDelete = x.Items.Cast<ClientNamedProbe<int>>().OrderBy(y => y.LastUpdate)
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
                probe.Update(namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs);
            }
            else
            {
                x.AddOrUpdate(new ClientNamedProbe<float>(name, namedValueFloatPacket.Payload.Value, namedValueFloatPacket.Payload.TimeBootMs));
                if (_config.MaxCollectionSize > 0 && x.Count > _config.MaxCollectionSize)
                {
                    var itemsToDelete = x.Items.Cast<ClientNamedProbe<float>>().OrderBy(y => y.LastUpdate)
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
}
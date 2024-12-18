using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.Common;
using Asv.Mavlink.Diagnostic.Server;
using DotNext;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public delegate IMavlinkMicroserviceServer RegisterServerMicroserviceDelegate(MavlinkIdentity identity, IMavlinkContext context, IConfiguration config);

public delegate TMicroservice RegisterServerMicroserviceDelegate<out TMicroservice>(MavlinkIdentity identity, IMavlinkContext context, IConfiguration config)
    where TMicroservice : IMavlinkMicroserviceServer;
public delegate TMicroservice RegisterServerMicroserviceDelegate<out TMicroservice, in TArg>(MavlinkIdentity identity, IMavlinkContext context, IConfiguration config, TArg arg)
    where TMicroservice : IMavlinkMicroserviceServer;

public delegate TMicroservice RegisterServerMicroserviceDelegate<out TMicroservice, in TArg1, in TArg2>(MavlinkIdentity identity, IMavlinkContext context, IConfiguration config, TArg1 arg1, TArg2 arg2)
    where TMicroservice : IMavlinkMicroserviceServer;

public delegate TMicroservice RegisterServerMicroserviceDelegate<out TMicroservice, in TArg1, in TArg2, in TArg3>(MavlinkIdentity identity, IMavlinkContext context, IConfiguration config, TArg1 arg1, TArg2 arg2, TArg3 arg3)
    where TMicroservice : IMavlinkMicroserviceServer;

public delegate TMicroservice RegisterServerMicroserviceDelegate<out TMicroservice, in TArg1, in TArg2, in TArg3, in TArg4>(MavlinkIdentity identity, IMavlinkContext context, IConfiguration config, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
    where TMicroservice : IMavlinkMicroserviceServer;


public interface IMavlinkServerMicroserviceBuilder
{
    void SetPacketSequence(IPacketSequenceCalculator calculator);
    void SetLog(ILoggerFactory loggerFactory);
    void SetTimeProvider(TimeProvider timeProvider);
    void SetMeterFactory(IMeterFactory meterFactory);
    void SetConfiguration(IConfiguration configuration);
    void Register<TMicroservice>(RegisterServerMicroserviceDelegate<TMicroservice> factory)
        where TMicroservice: IMavlinkMicroserviceServer;
    void Register<TMicroservice, TDependency>(RegisterServerMicroserviceDelegate<TMicroservice,TDependency> factory)
        where TMicroservice: IMavlinkMicroserviceServer;
    void Register<TMicroservice, TDependency1, TDependency2>(RegisterServerMicroserviceDelegate<TMicroservice,TDependency1,TDependency2> factory)
        where TMicroservice: IMavlinkMicroserviceServer;
    void Register<TMicroservice, TDependency1, TDependency2, TDependency3>(RegisterServerMicroserviceDelegate<TMicroservice,TDependency1,TDependency2, TDependency3> factory)
        where TMicroservice: IMavlinkMicroserviceServer;
    void Register<TMicroservice, TDependency1, TDependency2, TDependency3, TDependency4>(RegisterServerMicroserviceDelegate<TMicroservice,TDependency1,TDependency2, TDependency3, TDependency4> factory)
        where TMicroservice: IMavlinkMicroserviceServer;
    
   
}

public class MavlinkServerMicroserviceBuilder(MavlinkIdentity identity, IProtocolConnection connection) 
    : IMavlinkServerMicroserviceBuilder
{
    private IPacketSequenceCalculator? _seq;
    private ILoggerFactory? _logFactory;
    private TimeProvider? _timeProvider;
    private IMeterFactory? _meterFactory;
    private IConfiguration? _configuration;
    
    private readonly Dictionary<Type,RegisterServerMicroserviceDelegate> _buildFactory = new();
    private readonly Dictionary<Type, IMavlinkMicroserviceServer> _services = new();
    private readonly Dictionary<Type,Type[]> _deps = new();

    public void SetPacketSequence(IPacketSequenceCalculator calculator)
    {
        ArgumentNullException.ThrowIfNull(calculator);
        _seq = calculator;
    }

    public void SetLog(ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        _logFactory = loggerFactory;
    }

    public void SetTimeProvider(TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        _timeProvider = timeProvider;
    }
    
    public void SetMeterFactory(IMeterFactory meterFactory)
    {
        ArgumentNullException.ThrowIfNull(meterFactory);
        _meterFactory = meterFactory;
    }

    public void SetConfiguration(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;
    }

    public void Register<TMicroservice>(RegisterServerMicroserviceDelegate<TMicroservice> factory)
        where TMicroservice : IMavlinkMicroserviceServer
    {
        ArgumentNullException.ThrowIfNull(factory);
        _deps.Add(typeof(TMicroservice), []);
        _buildFactory.Add(typeof(TMicroservice), (id, context, cfg) => factory(id, context, cfg));
    }

    public void Register<TMicroservice, TDependency>(RegisterServerMicroserviceDelegate<TMicroservice, TDependency> factory)
        where TMicroservice : IMavlinkMicroserviceServer
    {
        _deps.Add(typeof(TMicroservice), [typeof(TDependency)]);
        _buildFactory.Add(typeof(TMicroservice), (id, context, cfg) => factory(id, context, cfg,
            (TDependency)_services[typeof(TDependency)]));
    }

    public void Register<TMicroservice, TDependency1, TDependency2>(RegisterServerMicroserviceDelegate<TMicroservice, TDependency1, TDependency2> factory)
        where TMicroservice : IMavlinkMicroserviceServer
    {
        _deps.Add(typeof(TMicroservice), [typeof(TDependency1),typeof(TDependency2)]);
        _buildFactory.Add(typeof(TMicroservice), (id, context, cfg) => factory(id, context, cfg,
            (TDependency1)_services[typeof(TDependency1)],
            (TDependency2)_services[typeof(TDependency2)]));
    }

    public void Register<TMicroservice, TDependency1, TDependency2, TDependency3>(
        RegisterServerMicroserviceDelegate<TMicroservice, TDependency1, TDependency2, TDependency3> factory)
        where TMicroservice : IMavlinkMicroserviceServer
    {
        _deps.Add(typeof(TMicroservice), [typeof(TDependency1), typeof(TDependency2), typeof(TDependency3)]);
        _buildFactory.Add(typeof(TMicroservice),
            (id, context, cfg) => factory(id, context, cfg,
                (TDependency1)_services[typeof(TDependency1)],
                (TDependency2)_services[typeof(TDependency2)],
                (TDependency3)_services[typeof(TDependency3)]));
    }

    public void Register<TMicroservice, TDependency1, TDependency2, TDependency3, TDependency4>(
        RegisterServerMicroserviceDelegate<TMicroservice, TDependency1, TDependency2, TDependency3, TDependency4>
            factory) where TMicroservice : IMavlinkMicroserviceServer
    {
        _deps.Add(typeof(TMicroservice),
            [typeof(TDependency1), typeof(TDependency2), typeof(TDependency3), typeof(TDependency4)]);
        _buildFactory.Add(typeof(TMicroservice),
            (id, context, cfg) => factory(id, context, cfg, (TDependency1)_services[typeof(TDependency1)],
                (TDependency2)_services[typeof(TDependency2)],
                (TDependency3)_services[typeof(TDependency3)],
                (TDependency4)_services[typeof(TDependency4)]));
    }

    public IMavlinkServerMicroserviceFactory Build()
    {
        var core = new CoreServices(
            connection,
            _seq ?? new PacketSequenceCalculator(),
            _logFactory ?? NullLoggerFactory.Instance,
            _timeProvider ?? TimeProvider.System,
            _meterFactory ?? new DefaultMeterFactory());
        var config = _configuration ?? new InMemoryConfiguration();
        
        foreach (var type in DepthFirstSearch.Sort(_deps))
        {
            var factory = _buildFactory[type];
            _services[type] = factory(identity, core, config);
        }

        return new MavlinkServerMicroserviceFactory(
            identity,
            core,
            config,
            _services.ToImmutableDictionary());
    }
}

public interface IMavlinkServerMicroserviceFactory
{
    MavlinkIdentity Identity { get; }
    IMavlinkContext Context { get; }
    IConfiguration Configuration { get; }
    TMicroservice Get<TMicroservice>() 
        where TMicroservice:IMavlinkMicroserviceServer;
}

public class MavlinkServerMicroserviceFactory : AsyncDisposableOnce, IMavlinkServerMicroserviceFactory
{
    #region Static

    public static IMavlinkServerMicroserviceFactory Create(MavlinkIdentity identity, IProtocolConnection connection, Action<IMavlinkServerMicroserviceBuilder> builder)
    {
        var b = new MavlinkServerMicroserviceBuilder(identity,connection);
        builder(b);
        return b.Build();
    }

    #endregion
    
    private ImmutableDictionary<Type,IMavlinkMicroserviceServer> _services;
    
    internal MavlinkServerMicroserviceFactory(
        MavlinkIdentity identity, 
        IMavlinkContext context, 
        IConfiguration config,
        ImmutableDictionary<Type,IMavlinkMicroserviceServer> services)  
    {
        ArgumentNullException.ThrowIfNull(identity);
        ArgumentNullException.ThrowIfNull(context);
        Identity = identity;
        Context = context;
        Configuration = config;
        _services = services;
    }

    public MavlinkIdentity Identity { get; }
    public IMavlinkContext Context { get; }
    public IConfiguration Configuration { get; }
    public TMicroservice Get<TMicroservice>() where TMicroservice : IMavlinkMicroserviceServer
    {
        if (_services.TryGetValue(typeof(TMicroservice), out var svc) == false)
        {
            throw new ArgumentOutOfRangeException($"Microservice not found {nameof(TMicroservice)}");
        }
        return (TMicroservice)_services[typeof(TMicroservice)];
    }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var svc in _services) 
            {
                svc.Value.Dispose();
            }
            _services = _services.Clear();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await base.DisposeAsyncCore().ConfigureAwait(false);
        foreach (var svc in _services)
        {
            await svc.Value.DisposeAsync().ConfigureAwait(false);
        }
        _services = _services.Clear();
    }

    #endregion
}
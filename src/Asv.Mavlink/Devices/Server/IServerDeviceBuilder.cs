using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;
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


public interface IServerDeviceBuilder
{
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

public class ServerDeviceBuilder(MavlinkIdentity identity, IMavlinkContext context) 
    : IServerDeviceBuilder
{
    private IConfiguration? _configuration;
    private readonly Dictionary<Type,RegisterServerMicroserviceDelegate> _buildFactory = new();
    private readonly Dictionary<Type, IMavlinkMicroserviceServer> _services = new();
    private readonly Dictionary<Type,Type[]> _deps = new();

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

    public IServerDevice Build()
    {
        var config = _configuration ?? new InMemoryConfiguration();
        
        foreach (var type in DepthFirstSearch.Sort(_deps))
        {
            var factory = _buildFactory[type];
            _services[type] = factory(identity, context, config);
        }

        return new ServerDevice(
            identity,
            context,
            config,
            _services.ToImmutableDictionary());
    }
}


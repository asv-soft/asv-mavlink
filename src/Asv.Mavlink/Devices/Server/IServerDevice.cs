using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink;

public interface IServerDevice: IDisposable, IAsyncDisposable
{
    MavlinkIdentity Identity { get; }
    IMavlinkContext Context { get; }
    IConfiguration Configuration { get; }
    TMicroservice Get<TMicroservice>() 
        where TMicroservice:IMavlinkMicroserviceServer;

    void Start();
}

public sealed class ServerDevice : AsyncDisposableOnce, IServerDevice
{
    #region Static

    public static IServerDevice Create(MavlinkIdentity identity, IProtocolConnection connection, Action<IServerDeviceBuilder> builder)
    {
        ArgumentNullException.ThrowIfNull(identity);
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(builder);
        var b = new ServerDeviceBuilder(identity,new CoreServices(connection));
        builder(b);
        return b.Build();
    }
    public static IServerDevice Create(MavlinkIdentity identity, IMavlinkContext context, Action<IServerDeviceBuilder> builder)
    {
        ArgumentNullException.ThrowIfNull(identity);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(builder);
        var b = new ServerDeviceBuilder(identity,context);
        builder(b);
        return b.Build();
    }

    #endregion
    
    private ImmutableDictionary<Type,IMavlinkMicroserviceServer> _services;
    
    internal ServerDevice(
        MavlinkIdentity identity, 
        IMavlinkContext context, 
        IConfiguration config,
        ImmutableDictionary<Type,IMavlinkMicroserviceServer> services)  
    {
        ArgumentNullException.ThrowIfNull(identity);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(config);
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

    public void Start()
    {
        _services.Values.ForEach(x => x.Start());
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
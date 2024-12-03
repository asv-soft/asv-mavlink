using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class ServerDeviceConfig
{
    public MavlinkHeartbeatServerConfig Heartbeat { get; set; } = new();
    public StatusTextLoggerConfig StatusText { get; set; } = new();
}

public class ServerDevice : IServerDevice, IDisposable, IAsyncDisposable
{
    private readonly HeartbeatServer _heartbeat;
    private readonly StatusTextServer _statusText;
    private readonly ILogger<ServerDevice> _logger;

    public ServerDevice(MavlinkIdentity identity, ServerDeviceConfig config, ICoreServices core)
    {
        ArgumentNullException.ThrowIfNull(core);
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(identity);
        _logger = core.LoggerFactory.CreateLogger<ServerDevice>();
        _heartbeat = new(identity, config.Heartbeat, core);
        _statusText = new(identity, config.StatusText,core);
        Core = core;
        Identity = identity;
    }

    public ICoreServices Core { get; }
    public MavlinkIdentity Identity { get; }

    public IStatusTextServer StatusText => _statusText;

    public IHeartbeatServer Heartbeat => _heartbeat;

    public virtual void Start()
    {
        
        Heartbeat.Start();
    }


    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _heartbeat.Dispose();
            _statusText.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_heartbeat).ConfigureAwait(false);
        await CastAndDispose(_statusText).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }
}
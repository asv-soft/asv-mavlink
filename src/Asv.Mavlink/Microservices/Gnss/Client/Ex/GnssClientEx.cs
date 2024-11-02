using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

/// <summary>
/// The GNSS client extension class.
/// </summary>
public class GnssClientEx : IGnssClientEx,IDisposable,IAsyncDisposable
{
    private readonly GnssStatusClient _main;
    private readonly GnssStatusClient _additional;

    public GnssClientEx(IGnssClient client)
    {
        Base = client;
        _main = new GnssStatusClient(client.Main);
        _additional = new GnssStatusClient(client.Additional);
    }
    public string Name => $"{Base.Name}Ex";
    /// <summary>
    /// Gets the base GNSS client.
    /// </summary>
    /// <value>
    /// The base GNSS client.
    /// </value>
    public IGnssClient Base { get; }

    /// <summary>
    /// Gets the main instance of the GNSS status client.
    /// </summary>
    /// <value>
    /// The main instance of the GNSS status client.
    /// </value>
    public IGnssStatusClient Main => _main;

    /// <summary>
    /// Gets the additional GNSS status client.
    /// </summary>
    /// <value>
    /// The additional GNSS status client.
    /// </value>
    public IGnssStatusClient Additional => _additional;

    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }

    #region Dispose

    public void Dispose()
    {
        _main.Dispose();
        _additional.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_main).ConfigureAwait(false);
        await CastAndDispose(_additional).ConfigureAwait(false);

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
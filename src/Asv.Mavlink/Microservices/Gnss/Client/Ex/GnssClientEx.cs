using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

/// <summary>
/// The GNSS client extension class.
/// </summary>
public class GnssClientEx : DisposableOnceWithCancel, IGnssClientEx
{
    public GnssClientEx(IGnssClient client)
    {
        Base = client;
        Main = new GnssStatusClient(client.Main).DisposeItWith(Disposable);
        Additional = new GnssStatusClient(client.Additional).DisposeItWith(Disposable);
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
    public IGnssStatusClient Main { get; }

    /// <summary>
    /// Gets the additional GNSS status client.
    /// </summary>
    /// <value>
    /// The additional GNSS status client.
    /// </value>
    public IGnssStatusClient Additional { get; }

    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
}
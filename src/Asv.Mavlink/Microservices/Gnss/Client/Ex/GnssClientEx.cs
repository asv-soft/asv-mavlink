using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

/// <summary>
/// The GNSS client extension class.
/// </summary>
public class GnssClientEx : MavlinkMicroserviceClient, IGnssClientEx
{
    

    private readonly GnssStatusClient _main;
    private readonly GnssStatusClient _additional;

    public GnssClientEx(IGnssClient client) 
        : base(GnssHelper.MicroserviceExName,client.Identity, client.Core)
    {
        Base = client;
        _main = new GnssStatusClient(client.Main);
        _additional = new GnssStatusClient(client.Additional);
    }
    
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

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _main.Dispose();
            _additional.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await _main.DisposeAsync().ConfigureAwait(false);
        await _additional.DisposeAsync().ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);
    }
    
    #endregion
}
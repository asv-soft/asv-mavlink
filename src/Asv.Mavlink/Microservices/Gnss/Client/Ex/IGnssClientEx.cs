using System.Reactive.Concurrency;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

/// <summary>
/// Represents a GNSS client with extended functionalities.
/// </summary>
public interface IGnssClientEx
{
    /// <summary>
    /// Gets the main object implementing the IGnssStatusClient interface.
    /// </summary>
    /// <remarks>
    /// The IGnssStatusClient interface provides methods and properties
    /// to interact with the GNSS (Global Navigation Satellite System) status.
    /// It allows the user to retrieve information about the status of
    /// satellite signals, including the number of satellites visible,
    /// their signal strength, and their signal-to-noise ratio.
    /// </remarks>
    /// <value>
    /// The main object implementing the IGnssStatusClient interface.
    /// </value>
    IGnssStatusClient Main { get; }

    /// <summary>
    /// Gets the additional GNSS status client.
    /// </summary>
    /// <value>
    /// The additional GNSS status client.
    /// </value>
    IGnssStatusClient Additional { get; }
}

/// <summary>
/// The GNSS client extension class.
/// </summary>
public class GnssClientEx : DisposableOnceWithCancel, IGnssClientEx
{
    private readonly IScheduler _scheduler;
    private readonly ILogger _logger;

    public GnssClientEx(IGnssClient client, IScheduler? scheduler = null, ILogger? logger = null)
    {
        _scheduler = scheduler ?? Scheduler.Default;
        _logger = logger ?? NullLogger.Instance;
        Base = client;
        Main = new GnssStatusClient(client.Main).DisposeItWith(Disposable);
        Additional = new GnssStatusClient(client.Additional).DisposeItWith(Disposable);
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
    public IGnssStatusClient Main { get; }

    /// <summary>
    /// Gets the additional GNSS status client.
    /// </summary>
    /// <value>
    /// The additional GNSS status client.
    /// </value>
    public IGnssStatusClient Additional { get; }
}
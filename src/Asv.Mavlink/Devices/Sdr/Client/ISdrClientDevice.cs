namespace Asv.Mavlink;

/// <summary>
/// Represents an SDR (Software-Defined Radio) client device.
/// </summary>
public interface ISdrClientDevice:IClientDevice
{
    /// <summary>
    /// Gets the SDR client instance.
    /// </summary>
    /// <value>
    /// The SDR client instance.
    /// </value>
    IAsvSdrClientEx Sdr { get; }

    /// <summary>
    /// Gets the Command property.
    /// </summary>
    /// <value>
    /// The ICommandClient object.
    /// </value>
    ICommandClient Command { get; }

    /// <summary>
    /// Gets the client for accessing missions.
    /// </summary>
    /// <remarks>
    /// The mission client allows you to perform operations related to missions, such as creating, updating, deleting, and retrieving missions.
    /// </remarks>
    IMissionClientEx Missions { get; }

    /// <summary>
    /// Gets the interface for accessing the Params property.
    /// </summary>
    /// <value>
    /// The Params property interface.
    /// </value>
    IParamsClientEx Params { get; }
}
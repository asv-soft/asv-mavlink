namespace Asv.Mavlink;

/// <summary>
/// Represents a client device interface for a GBS client.
/// </summary>
public interface IGbsClientDevice:IClientDevice
{
    /// <summary>
    /// Gets the Params object from the ParamsClientEx instance.
    /// </summary>
    /// <remarks>
    /// The Params object provides access to the parameters for the client.
    /// </remarks>
    /// <value>
    /// The Params object from the ParamsClientEx instance.
    /// </value>
    IParamsClientEx Params { get; }

    /// <summary>
    /// Gets the ICommandClient command property.
    /// </summary>
    /// <returns>The ICommandClient command property.</returns>
    ICommandClient Command { get; }

    /// <summary>
    /// Gets the Gbs property.
    /// </summary>
    /// <value>
    /// The instance of the IAsvGbsExClient.
    /// </value>
    IAsvGbsExClient Gbs { get; }
}
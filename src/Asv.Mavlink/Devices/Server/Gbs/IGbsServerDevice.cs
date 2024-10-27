using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents a Gbs server device that extends the functionality of an IServerDevice.
/// </summary>
public interface IGbsServerDevice:IServerDevice
{
    /// <summary>
    /// Gets the extended command long server.
    /// </summary>
    /// <value>
    /// The extended command long server.
    /// </value>
    ICommandServerEx<CommandLongPacket> CommandLongEx { get; }

    /// <summary>
    /// Gets the Gbs server.
    /// </summary>
    /// <remarks>
    /// This property provides access to the Gbs server.
    /// </remarks>
    /// <value>
    /// An instance of <see cref="IAsvGbsServerEx"/> representing the Gbs server.
    /// </value>
    IAsvGbsServerEx Gbs { get; }

    /// <summary>
    /// Gets the Parameters for the server.
    /// </summary>
    /// <returns>
    /// An object of type IParamsServerEx representing the server parameters.
    /// </returns>
    /// <remarks>
    /// This property allows access to the Parameters for the server. The returned object
    /// provides methods and properties to interact with and manage the server parameters.
    /// </remarks>
    IParamsServerEx Params { get; }
    
}
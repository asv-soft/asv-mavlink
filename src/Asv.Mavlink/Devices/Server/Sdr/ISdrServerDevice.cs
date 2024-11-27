using Asv.Mavlink.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents a server device for SDR communication.
/// </summary>
public interface ISdrServerDevice:IServerDevice
{
    /// <summary>
    /// Gets the ICommandServerEx of type CommandLongPacket for extended long commands.
    /// </summary>
    /// <value>
    /// The ICommandServerEx of type CommandLongPacket for extended long commands.
    /// </value>
    ICommandServerEx<CommandLongPacket> CommandLongEx { get; }

    /// <summary>
    /// Gets the Params property.
    /// </summary>
    /// <value>
    /// The Params property.
    /// </value>
    IParamsServerEx Params { get; }

    /// <summary>
    /// Gets the extended interface of the ASV SDR server.
    /// </summary>
    /// <remarks>
    /// This property provides access to the extended capabilities of the ASV SDR server.
    /// </remarks>
    /// <value>
    /// An instance of the <see cref="IAsvSdrServerEx"/> interface that represents the extended ASV SDR server.
    /// </value>
    IAsvSdrServerEx SdrEx { get; }

    /// <summary>
    /// Gets the mission server interface for additional functionality.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="IMissionServerEx"/> interface.
    /// </returns>
    IMissionServerEx Missions { get; }
}
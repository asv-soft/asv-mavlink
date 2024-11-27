using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

/// <summary>
/// Helper class containing extension methods for ICommandServer interface.
/// </summary>
public static class CommandServerHelper
{
    /// <summary>
    /// Sends command acknowledge accepted to the command server.
    /// </summary>
    /// <param name="server">The command server.</param>
    /// <param name="req">The command request packet.</param>
    /// <param name="result">The mav result.</param>
    /// <param name="cancel">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static ValueTask SendCommandAckAccepted(this ICommandServer server, CommandIntPacket req, MavResult result, CancellationToken cancel = default)
    {
        return server.SendCommandAck(req.Payload.Command, new DeviceIdentity(req.SystemId,req.ComponentId), CommandResult.FromResult(result), cancel);
    }

    /// <summary>
    /// Sends a command acknowledgment indicating that the command has been accepted.
    /// </summary>
    /// <param name="server">The command server.</param>
    /// <param name="req">The command long packet.</param>
    /// <param name="result">The result of the command.</param>
    /// <param name="cancel">The cancellation token (optional).</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    public static ValueTask SendCommandAckAccepted(this ICommandServer server, CommandLongPacket req, MavResult result, CancellationToken cancel = default)
    {
        return server.SendCommandAck(req.Payload.Command, new DeviceIdentity(req.SystemId,req.ComponentId), CommandResult.FromResult(result), cancel);
    }
}
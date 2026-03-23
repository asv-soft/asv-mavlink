using System;
using Asv.Mavlink.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents an exception that is thrown when a MAVLink command execution
/// results in a non-success acknowledgment. Contains the <see cref="CommandAckPayload"/>
/// detailing the result of the command.
/// </summary>
[Serializable]
public class CommandException : MavlinkException
{
    /// <summary>
    /// Gets the command acknowledgment payload returned by the device.
    /// </summary>
    public CommandAckPayload Result { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandException"/> class
    /// with the acknowledgment payload returned by the remote device.
    /// </summary>
    /// <param name="result">The acknowledgment payload.</param>
    public CommandException(CommandAckPayload result): base(GetMessage(result))
    {
        Result = result;
    }

    /// <summary>
    /// Builds a human-readable error message from a <see cref="CommandAckPayload"/>.
    /// </summary>
    /// <param name="result">The acknowledgment payload to format.</param>
    /// <returns>A formatted error message describing the command failure.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="result"/> contains a successful or unrecognized
    /// <see cref="MavResult"/> value, indicating incorrect use of this exception.
    /// </exception>
    private static string GetMessage(CommandAckPayload result)
    {
        return result.Result switch
        {
            MavResult.MavResultTemporarilyRejected => string.Format(
                "Command '{0:G}' temporarily rejected by [SYS:{1};COM:{2}]. Result code: '{3:G}' Result param: {4:G}",
                result.Command, result.TargetSystem, result.TargetComponent, result.Result, result.ResultParam2),
            MavResult.MavResultDenied => string.Format(
                "Command '{0:G}' denied by [SYS:{1};COM:{2}]. Result code: '{3:G}' Result param: {4:G}",
                result.Command, result.TargetSystem, result.TargetComponent, result.Result, result.ResultParam2),
            MavResult.MavResultUnsupported => string.Format(
                "Command '{0:G}' not supported by [SYS:{1};COM:{2}]. Result code: '{3:G}' Result param: {4:G}",
                result.Command, result.TargetSystem, result.TargetComponent, result.Result, result.ResultParam2),
            MavResult.MavResultFailed => string.Format(
                "Command '{0:G}' failed by [SYS:{1};COM:{2}]. Result code: '{3:G}' Result param: {4:G}",
                result.Command, result.TargetSystem, result.TargetComponent, result.Result, result.ResultParam2),
            _ => throw new ArgumentOutOfRangeException(nameof(result.Result), "Wrong use exception")
        };
    }
}
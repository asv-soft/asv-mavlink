using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

namespace Asv.Mavlink;

public static class CommandClientHelper
{
    /// <summary>
    /// Sends a command with long parameter to the vehicle and validates the result.
    /// </summary>
    /// <param name="client">The command client.</param>
    /// <param name="edit">Delegate to populate the <see cref="CommandLongPayload"/> fields.</param>
    /// <param name="cancel">Optional cancel token argument.</param>
    /// <exception cref="CommandException">
    /// Thrown when the command is
    /// <see cref="MavResult.MavResultTemporarilyRejected"/>,
    /// <see cref="MavResult.MavResultDenied"/>,
    /// <see cref="MavResult.MavResultUnsupported"/>, or
    /// <see cref="MavResult.MavResultFailed"/>.
    /// </exception>
    public static async Task CommandLongAndCheckResult(
        this ICommandClient client, 
        Action<CommandLongPayload> edit,
        CancellationToken cancel = default
    )
    {
        var result = await client.CommandLong(edit, cancel).ConfigureAwait(false);
        switch (result.Result)
        {
            case MavResult.MavResultTemporarilyRejected:
            case MavResult.MavResultDenied:
            case MavResult.MavResultUnsupported:
            case MavResult.MavResultFailed:
                throw new CommandException(result);
        }
    }
    
    /// <summary>
    /// Executes a long-duration command with specified parameters and validates the result.
    /// </summary>
    /// <param name="client">The command client.</param>
    /// <param name="command">The MAVLink command.</param>
    /// <param name="param1">Command parameter 1.</param>
    /// <param name="param2">Command parameter 2.</param>
    /// <param name="param3">Command parameter 3.</param>
    /// <param name="param4">Command parameter 4.</param>
    /// <param name="param5">Command parameter 5.</param>
    /// <param name="param6">Command parameter 6.</param>
    /// <param name="param7">Command parameter 7.</param>
    /// <param name="cancel">Optional cancel token argument.</param>
    /// <exception cref="CommandException">
    /// Thrown when the command is
    /// <see cref="MavResult.MavResultTemporarilyRejected"/>,
    /// <see cref="MavResult.MavResultDenied"/>,
    /// <see cref="MavResult.MavResultUnsupported"/>, or
    /// <see cref="MavResult.MavResultFailed"/>.
    /// </exception>
    public static async Task CommandLongAndCheckResult(
        this ICommandClient client, 
        MavCmd command, 
        float param1, 
        float param2, 
        float param3,
        float param4, 
        float param5, 
        float param6, 
        float param7, 
        CancellationToken cancel
    )
    {
        var result = await client.CommandLong(
            command, 
            param1, 
            param2, 
            param3, 
            param4, 
            param5, 
            param6, 
            param7, 
            cancel
        ).ConfigureAwait(false);
        
        switch (result.Result)
        {
            case MavResult.MavResultTemporarilyRejected:
            case MavResult.MavResultDenied:
            case MavResult.MavResultUnsupported:
            case MavResult.MavResultFailed:
                throw new CommandException(result);
        }
    }

    /// <summary>
    /// Set the interval between messages for a particular MAVLink message ID. This interface replaces REQUEST_DATA_STREAM.
    /// </summary>
    /// <param name="client">The command client.</param>
    /// <param name="messageId">The MAVLink message ID to configure.</param>
    /// <param name="intervalUs">Interval in microseconds between messages.</param>
    /// <param name="cancel">Optional cancel token argument.</param>
    /// <exception cref="CommandException">Thrown when the command result is unsuccessful.</exception>
    public static Task SetMessageInterval(
        this ICommandClient client,
        int messageId,
        int intervalUs, 
        CancellationToken cancel = default
    )
    {
        return client.CommandLongAndCheckResult(
            MavCmd.MavCmdSetMessageInterval, 
            messageId, 
            intervalUs, 
            0, 
            0,
            0, 
            0, 
            0, 
            cancel
        );
    }
    
    /// <summary>
    /// Request the target system(s) emit a single instance of a specified message (i.e. a "one-shot" version of MAV_CMD_SET_MESSAGE_INTERVAL).
    /// </summary>
    /// <param name="client">The MAVLink command client.</param>
    /// <param name="messageId">The MAVLink message ID to request.</param>
    /// <param name="cancel">Optional cancel token argument.</param>
    /// <exception cref="CommandException">Thrown when the command result is unsuccessful.</exception>
    public static Task RequestMessageOnce(
        this ICommandClient client,
        int messageId, 
        CancellationToken cancel = default
    )
    {
        return client.CommandLongAndCheckResult(
            MavCmd.MavCmdRequestMessage, 
            messageId, 
            0, 
            0, 
            0,
            0, 
            0, 
            0, 
            cancel
        );
    }
    
    /// <summary>
    /// Set the interval between messages for a particular MAVLink message ID. This interface replaces REQUEST_DATA_STREAM.
    /// </summary>
    /// <typeparam name="TPacket">The MAVLink packet type. Must have a parameterless constructor.</typeparam>
    /// <param name="src">The command client.</param>
    /// <param name="intervalUs">Interval in microseconds between messages.</param>
    /// <param name="cancel">Optional cancel token argument.</param>
    /// <exception cref="CommandException">Thrown when the command result is unsuccessful.</exception>
    public static Task SetMessageInterval<TPacket>(
        this ICommandClient src, 
        int intervalUs,
        CancellationToken cancel = default
    ) where TPacket : MavlinkMessage, new()
    {
        var pkt = new TPacket();
        return src.SetMessageInterval(pkt.Id, intervalUs, cancel);
    }
    
    /// <summary>
    /// Request the target system(s) emit a single instance of a specified message (i.e. a "one-shot" version of MAV_CMD_SET_MESSAGE_INTERVAL).
    /// </summary>
    /// <typeparam name="TPacket">The expected MAVLink packet type. Must have a parameterless constructor.</typeparam>
    /// <param name="src">The command client.</param>
    /// <param name="cancel">Optional cancel token argument.</param>
    /// <returns>The received <typeparamref name="TPacket"/> instance.</returns>
    /// <exception cref="CommandException">Thrown when the command result is unsuccessful.</exception>
    public static Task<TPacket> RequestMessageOnce<TPacket>(
        this ICommandClient src, 
        CancellationToken cancel = default
    ) where TPacket : MavlinkMessage, new()
    {
        var pkt = new TPacket();
        return src.CommandLongAndWaitPacket<TPacket>(
            MavCmd.MavCmdRequestMessage, 
            pkt.Id, 
            0, 
            0, 
            0,
            0, 
            0, 
            0, 
            cancel
        );
    }
    
    /// <summary>
    /// Requests the autopilot version from the target system
    /// by sending a one-shot <see cref="AutopilotVersionPacket"/> request.
    /// </summary>
    /// <param name="src">The command client.</param>
    /// <param name="cancel">Optional cancel token argument.</param>
    /// <returns>The <see cref="AutopilotVersionPayload"/> received from the target system.</returns>
    /// <exception cref="CommandException">Thrown when the command result is unsuccessful.</exception>
    public static async Task<AutopilotVersionPayload> GetAutopilotVersion(
        this ICommandClient src,
        CancellationToken cancel = default
    )
    {
        var result = await src
            .RequestMessageOnce<AutopilotVersionPacket>(cancel)
            .ConfigureAwait(false);
        
        return result.Payload;
    }

    /// <summary>
    /// Sets the flight mode of the target system using <see cref="MavCmd.MavCmdDoSetMode"/>.
    /// </summary>
    /// <param name="src">The command client.</param>
    /// <param name="mode">The base mode bitmask.</param>
    /// <param name="customMode">Autopilot-specific custom mode.</param>
    /// <param name="customSubMode">Autopilot-specific custom sub-mode.</param>
    /// <param name="cancel">Optional cancel token argument.</param>
    /// <exception cref="CommandException">Thrown when the command result is unsuccessful.</exception>
    public static Task DoSetMode(
        this ICommandClient src,
        uint mode, 
        uint customMode, 
        uint customSubMode, 
        CancellationToken cancel = default
    )
    {
        return src.CommandLongAndCheckResult(
            MavCmd.MavCmdDoSetMode, 
            mode, 
            customMode, 
            customSubMode, 
            0, 
            0, 
            0, 
            0,
            cancel
        );
    }

    /// <summary>
    /// Request the reboot or shutdown of system components using <see cref="MavCmd.MavCmdPreflightRebootShutdown"/>.
    /// </summary>
    /// <param name="src">The command client.</param>
    /// <param name="autopilot">Reboot/shutdown action for the autopilot.</param>
    /// <param name="companion">Reboot/shutdown action for the companion computer.</param>
    /// <param name="cancel">Optional cancel token argument.</param>
    /// <exception cref="CommandException">Thrown when the command result is unsuccessful.</exception>
    public static Task PreflightRebootShutdown(
       this ICommandClient src, 
       AutopilotRebootShutdown autopilot, 
       CompanionRebootShutdown companion, 
       CancellationToken cancel = default
    )
    {
        return src.CommandLongAndCheckResult(
            MavCmd.MavCmdPreflightRebootShutdown, 
            (float)autopilot, 
            (float)companion, 
            0, 
            0, 
            0, 
            0, 
            0, 
            cancel
        );
    }

    /// <summary>
    /// Returns a human-readable description for an <see cref="AutopilotRebootShutdown"/> action.
    /// </summary>
    /// <param name="src">The autopilot reboot/shutdown action.</param>
    /// <returns>A localized description string.</returns>
   public static string GetAutopilotRebootShutdownDescription(this AutopilotRebootShutdown src)
   {
       return src switch
       {
           AutopilotRebootShutdown.DoNothingForAutopilot => RS.CommandClientHelper_AutopilotRebootShutdown_DoNothingForAutopilot_Description,
           AutopilotRebootShutdown.RebootAutopilot => RS.CommandClientHelper_AutopilotRebootShutdown_RebootAutopilot_Description,
           AutopilotRebootShutdown.ShutdownAutopilot => RS.CommandClientHelper_AutopilotRebootShutdown_ShutdownAutopilot_Description,
           AutopilotRebootShutdown.RebootAutopilotAndKeepItInTheBootloaderUntilUpgraded => RS.CommandClientHelper_AutopilotRebootShutdown_RebootAutopilotAndKeepItInTheBootloaderUntilUpgraded_Description,
           _ => RS.CommandClientHelper_RebootShutdown_UnknownCommand_Description
       };
   }
   
    /// <summary>
    /// Returns a human-readable description for a <see cref="CompanionRebootShutdown"/> action.
    /// </summary>
    /// <param name="src">The companion computer reboot/shutdown action.</param>
    /// <returns>A localized description string.</returns>
   public static string GetCompanionRebootShutdownDescription(this CompanionRebootShutdown src)
   {
       return src switch
       {
           CompanionRebootShutdown.DoNothingForOnboardComputer => RS.CommandClientHelper_CompanionRebootShutdown_DoNothingForOnboardComputer_Description,
           CompanionRebootShutdown.RebootOnboardComputer => RS.CommandClientHelper_CompanionRebootShutdown_RebootOnboardComputer_Description,
           CompanionRebootShutdown.ShutdownOnboardComputer => RS.CommandClientHelper_CompanionRebootShutdown_ShutdownOnboardComputer_Description,
           CompanionRebootShutdown.RebootOnboardComputerAndKeepItInTheBootloaderUntilUpgraded => RS.CommandClientHelper_CompanionRebootShutdown_RebootOnboardComputerAndKeepItInTheBootloaderUntilUpgraded_Description,
           _ => RS.CommandClientHelper_RebootShutdown_UnknownCommand_Description
       };
   }
}

public enum AutopilotRebootShutdown
{
    DoNothingForAutopilot = 0,
    RebootAutopilot = 1,
    ShutdownAutopilot = 2,
    RebootAutopilotAndKeepItInTheBootloaderUntilUpgraded = 3,
}

public enum CompanionRebootShutdown
{
    DoNothingForOnboardComputer = 0,
    RebootOnboardComputer = 1,
    ShutdownOnboardComputer = 2,
    RebootOnboardComputerAndKeepItInTheBootloaderUntilUpgraded = 3,
}
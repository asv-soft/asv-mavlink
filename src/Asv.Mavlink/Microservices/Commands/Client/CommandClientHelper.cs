using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public static class CommandClientHelper
{
    public static async Task CommandLongAndCheckResult(this ICommandClient client, MavCmd command, float param1, float param2, float param3, float param4, float param5, float param6, float param7, CancellationToken cancel)
    {
        var result = await client.CommandLong(command, param1, param2, param3, param4, param5, param6, param7, cancel).ConfigureAwait(false);
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
    public static Task SetMessageInterval(this ICommandClient client,int messageId,int intervalUs, CancellationToken cancel = default)
    {
        return client.CommandLongAndCheckResult(MavCmd.MavCmdSetMessageInterval, messageId, intervalUs, 0, float.NaN,
            float.NaN, float.NaN, float.NaN, cancel);
    }
    /// <summary>
    /// Request the target system(s) emit a single instance of a specified message (i.e. a "one-shot" version of MAV_CMD_SET_MESSAGE_INTERVAL).
    /// </summary>
    public static Task RequestMessageOnce(this ICommandClient client,int messageId, CancellationToken cancel = default)
    {
        return client.CommandLongAndCheckResult(MavCmd.MavCmdRequestMessage, messageId, float.NaN, float.NaN, float.NaN,
            float.NaN, float.NaN, 0, cancel);
    }
    /// <summary>
    /// Set the interval between messages for a particular MAVLink message ID. This interface replaces REQUEST_DATA_STREAM.
    /// </summary>
    public static Task SetMessageInterval<TPacket>(this ICommandClient src, int intervalUs, CancellationToken cancel = default)
        where TPacket : IPacketV2<IPayload>, new()
    {
        var pkt = new TPacket();
        return src.SetMessageInterval(pkt.MessageId, intervalUs, cancel);
    }
    /// <summary>
    /// Request the target system(s) emit a single instance of a specified message (i.e. a "one-shot" version of MAV_CMD_SET_MESSAGE_INTERVAL).
    /// </summary>
    public static Task<TPacket> RequestMessageOnce<TPacket>(this ICommandClient src, CancellationToken cancel = default)
        where TPacket : IPacketV2<IPayload>, new()
    {
        var pkt = new TPacket();
        return src.CommandLongAndWaitPacket<TPacket>(MavCmd.MavCmdRequestMessage, pkt.MessageId, float.NaN, float.NaN, float.NaN,
            float.NaN, float.NaN, 0, cancel);
    }
    
    public static async Task<AutopilotVersionPayload> GetAutopilotVersion(this ICommandClient src,CancellationToken cancel = default)
    {
        var result = await src.RequestMessageOnce<AutopilotVersionPacket>(cancel).ConfigureAwait(false);
        return result.Payload;
    }
}
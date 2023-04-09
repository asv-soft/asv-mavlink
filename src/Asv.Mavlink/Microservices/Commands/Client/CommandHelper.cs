using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public static class CommandHelper
    {
        public static Task GetHomePosition(this ICommandClient src, CancellationToken cancel)
        {
            return src.CommandLong(MavCmd.MavCmdGetHomePosition, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
                float.NaN, float.NaN, 1, cancel);
        }

        // public static Task<AutopilotVersionPayload> GetAutopilotVersion(CancellationToken cancel)
        // {
        //     var packet = InternalGeneratePacket<CommandLongPacket>();
        //     packet.Payload.Command = MavCmd.MavCmdRequestAutopilotCapabilities;
        //     packet.Payload.TargetComponent = Identity.TargetComponentId;
        //     packet.Payload.TargetSystem = Identity.TargetSystemId;
        //     packet.Payload.Confirmation = 0;
        //     packet.Payload.Param1 = 1;
        //     var result = await InternalSendAndWaitAnswer<AutopilotVersionPacket>(packet, cancel).ConfigureAwait(false);
        //     return result.Payload;
        // }
       
    }

   
}
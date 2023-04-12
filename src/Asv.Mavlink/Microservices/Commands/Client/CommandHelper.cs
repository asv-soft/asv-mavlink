using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public static class CommandHelper
    {
        

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
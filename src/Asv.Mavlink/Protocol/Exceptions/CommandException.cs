using System;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    [Serializable]
    public class CommandException : MavlinkException
    {
        public CommandAckPayload Result { get; }

        public CommandException(CommandAckPayload result):base(GetMessage(result))
        {
            Result = result;
        }

        private static string GetMessage(CommandAckPayload result)
        {
            switch (result.Result)
            {
                case MavResult.MavResultTemporarilyRejected:
                    return string.Format("Command '{0:G}' temporarily rejected by vehicle [SYS:{1};COM:{2}]. Result code: '{3:G}' Result param: {4:G}", result.Command, result.TargetSystem, result.TargetComponent, result.Result, result.ResultParam2);
                case MavResult.MavResultDenied:
                    return string.Format("Command '{0:G}' denied by vehicle [SYS:{1};COM:{2}]. Result code: '{3:G}' Result param: {4:G}", result.Command, result.TargetSystem, result.TargetComponent, result.Result, result.ResultParam2);
                case MavResult.MavResultUnsupported:
                    return string.Format("Command '{0:G}' not supported by vehicle [SYS:{1};COM:{2}]. Result code: '{3:G}' Result param: {4:G}", result.Command, result.TargetSystem, result.TargetComponent, result.Result, result.ResultParam2);
                case MavResult.MavResultFailed:
                    return string.Format("Command '{0:G}' failed by vehicle [SYS:{1};COM:{2}]. Result code: '{3:G}' Result param: {4:G}", result.Command, result.TargetSystem, result.TargetComponent, result.Result, result.ResultParam2);
                default:
                    throw new ArgumentOutOfRangeException(nameof(result.Result), "Wrong use exception");
            }
        }
    }
}
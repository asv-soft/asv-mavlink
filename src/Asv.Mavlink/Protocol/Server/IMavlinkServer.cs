using System;

namespace Asv.Mavlink.Server
{
    public interface IMavlinkServer:IDisposable
    {
        IMavlinkV2Connection MavlinkV2Connection { get; }
        MavlinkServerIdentity Identity { get; }
        IHeartbeatServer Heartbeat { get; }
        IStatusTextServer StatusText { get; }
        IDebugServer Debug { get; }
        ICommandServer Command { get; }
        ILoggingServer Logging { get; }
        IV2ExtensionServer V2Extension { get; }
        IParamsServer Params { get; }
        IAsvGbsServer Gbs { get; }
        IAsvSdrServer Sdr { get; }
    }
}

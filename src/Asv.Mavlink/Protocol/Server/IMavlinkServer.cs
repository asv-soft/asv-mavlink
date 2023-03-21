using System;

namespace Asv.Mavlink.Server
{
    public interface IMavlinkServer:IDisposable
    {
        MavlinkServerIdentity Identity { get; }
        IMavlinkHeartbeatServer Heartbeat { get; }
        IStatusTextServer StatusText { get; }
        IDebugServer Debug { get; }
        ICommandLongServer CommandLong { get; }
        ILoggingServer Logging { get; }
        IV2ExtensionServer V2Extension { get; }
        IMavlinkV2Connection MavlinkV2Connection { get; }
        IMavlinkParamsServer Params { get; }
        IAsvGbsServer Gbs { get; }
    }
}

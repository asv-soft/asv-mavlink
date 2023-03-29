using System;
using Asv.Mavlink.Client;

namespace Asv.Mavlink
{
    /// <summary>
    /// 
    /// https://mavlink.io/en/services/
    /// </summary>
    public interface IMavlinkClient:IDisposable
    {
        MavlinkClientIdentity Identity { get; }
        IHeartbeatClient Heartbeat { get; }
        IMavlinkTelemetry Rtt { get; }
        IMavlinkParameterClient Params { get; }
        IMavlinkCommandClient Commands { get; }
        IMissionClient Mission { get; }
        IMavlinkOffboardMode Offboard { get; }
        IMavlinkCommon Common { get; }
        IDebugClient Debug { get; }
        ILoggingClient Logging { get; }
        IV2ExtensionClient V2Extension { get; }
        IDgpsClient Rtk { get; }
        IMavlinkV2Connection MavlinkV2Connection { get; }
        IAsvGbsClient Gbs { get; }
        IAsvSdrClient Sdr { get; }
    }
}

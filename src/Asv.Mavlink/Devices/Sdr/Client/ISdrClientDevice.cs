using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface ISdrClientDevice
{
    IAsvSdrClientEx Sdr { get; }
    IHeartbeatClient Heartbeat { get; }
    ICommandClient Command { get; }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRsga;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface IAsvRsgaClientEx:IMavlinkMicroserviceClient
{
    IAsvRsgaClient Base { get; }
    IObservable<IChangeSet<AsvRsgaCustomMode>> AvailableModes { get; }
    Task RefreshInfo(CancellationToken cancel = default);
    Task<MavResult> SetMode(AsvRsgaCustomMode mode, CancellationToken cancel = default);
}
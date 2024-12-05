using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;
using ObservableCollections;

namespace Asv.Mavlink;

public interface IAsvRsgaClientEx:IMavlinkMicroserviceClient
{
    IAsvRsgaClient Base { get; }
    IReadOnlyObservableList<AsvRsgaCustomMode> AvailableModes { get; }
    Task RefreshInfo(CancellationToken cancel = default);
    Task<MavResult> SetMode(AsvRsgaCustomMode mode, CancellationToken cancel = default);
}
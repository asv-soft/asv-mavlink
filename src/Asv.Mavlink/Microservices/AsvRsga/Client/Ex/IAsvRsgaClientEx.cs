using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public interface IAsvRsgaClientEx:IMavlinkMicroserviceClient
{
    IAsvRsgaClient Base { get; }
    ReadOnlyReactiveProperty<AsvRsgaCustomMode> CurrentMode { get; }
    ReadOnlyReactiveProperty<AsvRsgaCustomSubMode> CurrentSubMode { get; }
    IReadOnlyObservableList<AsvRsgaCustomMode> AvailableModes { get; }
    Task RefreshInfo(CancellationToken cancel = default);
    Task<MavResult> SetMode(AsvRsgaCustomMode mode, CancellationToken cancel = default);
}
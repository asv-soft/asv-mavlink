using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Asv.Mavlink;

public interface IModeServer : IMavlinkMicroserviceServer
{
    ReadOnlyReactiveProperty<bool> IsBusy { get; }
    ImmutableArray<ICustomMode> AvailableModes { get; }
    ReadOnlyReactiveProperty<IWorkModeHandler> CurrentMode { get; }
    Task SetMode(ICustomMode mode, CancellationToken cancel = default);
}
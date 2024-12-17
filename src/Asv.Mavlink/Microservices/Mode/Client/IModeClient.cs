using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Asv.Mavlink;



public interface IModeClient: IMavlinkMicroserviceClient
{
    ImmutableArray<ICustomMode> AvailableModes { get; }
    ReadOnlyReactiveProperty<ICustomMode> CurrentMode { get; }
    Task SetMode(ICustomMode mode, CancellationToken cancel = default);

}
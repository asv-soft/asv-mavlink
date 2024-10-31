using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Asv.Mavlink;

public interface IModeClient: IMavlinkMicroserviceClient
{
    IEnumerable<OpMode> AvailableModes { get; }
    ReadOnlyReactiveProperty<OpMode> CurrentMode { get; }
    Task SetMode(OpMode mode, CancellationToken cancel = default);

}
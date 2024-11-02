using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public static class ControlClientHelper
{
    public static async Task EnsureAutoMode(this IControlClient client, CancellationToken cancel = default)
    {
        if (await client.IsAutoMode(cancel).ConfigureAwait(false))
        {
            await client.SetAutoMode(cancel).ConfigureAwait(false);
        }
    }

    public static async Task EnsureGuidedMode(this IControlClient client, CancellationToken cancel = default)
    {
        if (await client.IsGuidedMode(cancel).ConfigureAwait(false))
        {
            await client.SetGuidedMode(cancel).ConfigureAwait(false);
        }
    }
}
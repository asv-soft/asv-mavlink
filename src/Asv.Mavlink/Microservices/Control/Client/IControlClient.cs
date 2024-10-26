using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

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

public interface IControlClient:IMavlinkMicroserviceClient
{
    ValueTask<bool> IsAutoMode(CancellationToken cancel = default);
    Task SetAutoMode(CancellationToken cancel = default);
    public ValueTask<bool> IsGuidedMode(CancellationToken cancel = default);
    Task SetGuidedMode(CancellationToken cancel = default);
    /// <summary>
    /// Navigates to the specified GeoPoint. </summary> <param name="point">The GeoPoint to navigate to.</param> <param name="cancel">The cancellation token to cancel the navigation (optional).</param> <returns>
    /// A Task representing the asynchronous operation.
    /// The task will complete when the navigation to the specified GeoPoint is finished. </returns>
    /// /
    Task GoTo(GeoPoint point, CancellationToken cancel = default);

    /// <summary>
    /// Land method.
    /// </summary>
    /// <param name="cancel">An optional token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method is used to initiate the landing process.
    /// The cancel token can be used to cancel the landing operation.
    /// </remarks>
    Task DoLand(CancellationToken cancel = default);

    /// <summary>
    /// Performs a right-to-left operation asynchronously.
    /// </summary>
    /// <param name="cancel">An optional cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DoRtl(CancellationToken cancel = default);

    /// <summary>
    /// Initiates the takeoff process and ascends to the specified altitude in meters.
    /// </summary>
    /// <param name="altInMeters">The target altitude in meters to ascend to.</param>
    /// <param name="cancel">The cancellation token (optional) to cancel the takeoff.</param>
    /// <returns>A task representing the asynchronous takeoff operation.</returns>
    Task TakeOff(double altInMeters, CancellationToken cancel = default);
}
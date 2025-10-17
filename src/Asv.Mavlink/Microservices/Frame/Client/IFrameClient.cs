using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public interface IFrameClient
{
    /// <summary>
    /// Available motor frames. Populated by <see cref="RefreshAvailableFrames"/>.
    /// </summary>
    IReadOnlyObservableDictionary<string, IMotorFrame> MotorFrames { get; }
    
    /// <summary>
    /// Currently selected motor frame.
    /// </summary>
    /// <remarks>
    /// This value is <see langword="null"/> if:
    /// <list type="bullet">
    /// <item>The current frame configuration cannot be matched to any of the available frames on this device, or</item>
    /// <item><see cref="RefreshCurrentFrame"/> has not been called yet.</item>
    /// </list>
    /// </remarks>
    ReadOnlyReactiveProperty<IMotorFrame?> CurrentMotorFrame { get; }

    /// <summary>
    /// Refreshes available frame types from the device and updates <see cref="MotorFrames"/> collection.
    /// </summary>
    /// <param name="cancel">An optional token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    ValueTask RefreshAvailableFrames(CancellationToken cancel = default);

    /// <summary>
    /// Updates the frame type for the current device.
    /// </summary>
    /// <param name="motorFrameToSet">Frame type to use.</param>
    /// <param name="cancel">An optional token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// Only use frames from the <see cref="MotorFrames"/> collection when calling this method.
    /// </remarks>
    Task SetFrame(IMotorFrame motorFrameToSet, CancellationToken cancel = default);

    /// <summary>
    /// Refreshes the current motor frame configuration from the device
    /// and starts reactively updating <see cref="CurrentMotorFrame"/> when the frame parameters change.
    /// </summary>
    /// <param name="cancel">An optional token to cancel the operation.</param>
    Task RefreshCurrentFrame(CancellationToken cancel = default);
}

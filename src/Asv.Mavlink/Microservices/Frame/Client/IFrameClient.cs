using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;

namespace Asv.Mavlink;

public interface IFrameClient
{
    /// <summary>
    /// Available motor frames. Populated by <see cref="LoadAvailableFrames"/>.
    /// </summary>
    IReadOnlyObservableDictionary<string, IMotorFrame> MotorFrames { get; }

    /// <summary>
    /// Loads available frame types from the device and updates <see cref="MotorFrames"/> collection.
    /// </summary>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    ValueTask LoadAvailableFrames(CancellationToken cancel = default);

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
    /// Gets the frame type currently selected on the device.
    /// </summary>
    /// <param name="cancel">An optional token to cancel the operation.</param>
    /// <returns>A current frame type.</returns>
    /// <remarks>
    /// Returns null if the current frame value cannot be represented as one of the available frames of this device.
    /// Remember to call the <see cref="LoadAvailableFrames"/> before using this method.
    /// </remarks>
    Task<IMotorFrame?> GetCurrentFrame(CancellationToken cancel = default);
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents a server that handles parameter operations.
/// </summary>
public interface IParamsServer:IMavlinkMicroserviceServer
{
    /// <summary>
    /// Sends a parameter value to the server.
    /// </summary>
    /// <param name="changeCallback">The callback method to be executed when the parameter value changes.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    ValueTask SendParamValue(Action<ParamValuePayload> changeCallback, CancellationToken cancel = default);

    /// <summary>
    /// Gets the observable sequence for handling param request read packets.
    /// </summary>
    /// <value>
    /// The observable sequence for handling param request read packets.
    /// </value>
    Observable<ParamRequestReadPacket> OnParamRequestRead { get; }

    /// <summary>
    /// Event fired when a param request list packet is received.
    /// </summary>
    /// <remarks>
    /// This event provides an Observable<ParamRequestListPacket> that can be subscribed to receive param request list packets.
    /// </remarks>
    Observable<ParamRequestListPacket> OnParamRequestList { get; }

    /// <summary>
    /// Represents an event that occurs when a parameter set packet is received. </summary>
    /// <remarks>
    /// This event provides a stream of ParamSetPacket objects. </remarks>
    /// <value>
    /// An Observable of ParamSetPacket representing the event stream
    /// of parameter set packets being received. </value>
    /// /
    Observable<ParamSetPacket> OnParamSet { get; }
}
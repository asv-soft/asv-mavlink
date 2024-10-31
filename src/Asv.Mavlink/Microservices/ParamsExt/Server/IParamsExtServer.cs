using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink;

public interface IParamsExtServer:IMavlinkMicroserviceServer
{
    /// <summary>
    /// Sends a ParamExtAckPacket with the specified changeCallback and cancellation token.
    /// </summary>
    /// <param name="changeCallback">The action to be performed on the ParamExtAckPayload.</param>
    /// <param name="cancel">The cancellation token.</param>
    /// <returns>Sends ancknowledgement packet.</returns>
    Task SendParamExtAck(Action<ParamExtAckPayload> changeCallback, CancellationToken cancel = default);

    /// <summary>
    /// Sends a ParamExtValuePacket with the specified changeCallback and cancellation token.
    /// </summary>
    /// <param name="changeCallback">The action to be performed on the ParamExtValuePayload.</param>
    /// <param name="cancel">The cancellation token.</param>
    /// <returns>Sends value packet.</returns>
    Task SendParamExtValue(Action<ParamExtValuePayload> changeCallback, CancellationToken cancel = default);

    /// <summary>
    /// Gets the observable sequence of ParamExtSetPacket objects.
    /// </summary>
    Observable<ParamExtSetPacket> OnParamExtSet { get; }

    /// <summary>
    /// Gets the observable sequence of ParamExtRequestListPacket objects.
    /// </summary>
    Observable<ParamExtRequestListPacket> OnParamExtRequestList { get; }

    /// <summary>
    /// Gets the observable sequence of ParamExtRequestReadPacket objects.
    /// </summary>
    Observable<ParamExtRequestReadPacket> OnParamExtRequestRead { get; }
}
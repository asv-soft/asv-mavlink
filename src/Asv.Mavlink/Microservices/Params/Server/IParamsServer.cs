using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IParamsServer
{
    Task SendParamValue(Action<ParamValuePayload> changeCallback, CancellationToken cancel = default);
    IObservable<ParamRequestReadPacket> OnParamRequestRead { get; }
    IObservable<ParamRequestListPacket> OnParamRequestList { get; }
    IObservable<ParamSetPacket> OnParamSet { get; }
}
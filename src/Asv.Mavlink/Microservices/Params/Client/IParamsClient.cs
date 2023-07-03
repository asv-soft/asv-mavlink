using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IParamsClient
{
    IObservable<ParamValuePayload> OnParamValue { get; }
    Task SendRequestList(CancellationToken cancel = default);
    Task<ParamValuePayload> Read(string name, CancellationToken cancel = default);
    Task<ParamValuePayload> Read(ushort index, CancellationToken cancel = default);
    Task<ParamValuePayload> Write(string name, MavParamType type, float value, CancellationToken cancel = default);
}
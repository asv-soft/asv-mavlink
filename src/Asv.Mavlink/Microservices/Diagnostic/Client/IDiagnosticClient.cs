using System;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink.Diagnostic.Client;

public interface INamedProbe<T>:IDisposable
{
    public string Name { get; }
    public IRxValue<(TimeSpan, T)> Value { get; }
}



public interface IDiagnosticClient: IMavlinkMicroserviceClient
{
    IObservable<IChangeSet<INamedProbe<float>,string>> FloatProbes { get; }
    IObservable<IChangeSet<INamedProbe<int>,string>> IntProbes { get; }
    
    IObservable<DebugFloatArrayPayload> DebugFloatArray { get; }
    IObservable<MemoryVectPayload> MemoryVector { get; }
}
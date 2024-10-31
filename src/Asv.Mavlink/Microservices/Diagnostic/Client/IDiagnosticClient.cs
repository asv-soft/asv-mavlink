using System;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink.Diagnostic.Client;

public interface INamedProbe<T>:IDisposable
{
    public string Name { get; }
    public ReadOnlyReactiveProperty<(TimeSpan, T)> Value { get; }
}



public interface IDiagnosticClient: IMavlinkMicroserviceClient
{
    IObservable<IChangeSet<INamedProbe<float>,string>> FloatProbes { get; }
    IObservable<IChangeSet<INamedProbe<int>,string>> IntProbes { get; }
    
    IObservable<DebugFloatArrayPayload> DebugFloatArray { get; }
    IObservable<MemoryVectPayload> MemoryVector { get; }
}
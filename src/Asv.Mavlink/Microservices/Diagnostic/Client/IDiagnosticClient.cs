using System;
using Asv.Mavlink.Common;

using ObservableCollections;
using R3;

namespace Asv.Mavlink.Diagnostic.Client;

public interface INamedProbe<T>:IDisposable
{
    public string Name { get; }
    public ReadOnlyReactiveProperty<(TimeSpan, T)> Value { get; }
}



public interface IDiagnosticClient: IMavlinkMicroserviceClient
{
    IReadOnlyObservableDictionary<string,INamedProbe<float>> FloatProbes { get; }
    IReadOnlyObservableDictionary<string,INamedProbe<int>> IntProbes { get; }
    Observable<NamedValueIntPayload> OnIntProbe { get; }
    Observable<NamedValueFloatPayload> OnFloatProbe { get; }
    Observable<DebugFloatArrayPayload> DebugFloatArray { get; }
    Observable<MemoryVectPayload> MemoryVector { get; }
}
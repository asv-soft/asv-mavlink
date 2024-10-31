using System;
using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public interface IParamsExtClientEx : IMavlinkMicroserviceClient
{
    IParamsExtClient Base { get; }
    IReadOnlyBindableReactiveProperty<bool> IsSynced { get; }
    IReadOnlyBindableReactiveProperty<int> LocalCount { get; }
    IReadOnlyBindableReactiveProperty<int> RemoteCount { get; }
    Observable<MavParamExtValue> Filter(string name);
    IReadOnlyObservableDictionary<string, ParamExtItem> Items { get; }
    Observable<(string, MavParamExtValue)> OnValueChanged { get; }
    Task<MavParamExtValue> ReadOnce(string name, CancellationToken cancel = default);
    Task ReadAll(IProgress<double>? progress = null, CancellationToken cancel = default);
    Task<MavParamExtValue> WriteOnce(string name, MavParamExtValue value, CancellationToken cancel = default);
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using DynamicData;
using R3;

namespace Asv.Mavlink;

public interface IParamsExtClientEx
{
    bool IsInit { get; set; }

    IParamsExtClient Base { get; }

    ReadOnlyReactiveProperty<bool> IsSynced { get; }

    ReadOnlyReactiveProperty<ushort> LocalCount { get; }

    ReadOnlyReactiveProperty<ushort?> RemoteCount { get; }

    IObservable<MavParamExtValue> Filter(string name);
    
    IObservable<IChangeSet<IParamExtItem, string>> Items { get; }

    IObservable<(string, MavParamExtValue)> OnValueChanged { get; }
    
    Task<MavParamExtValue> ReadOnce(string name, CancellationToken cancel = default);

    Task ReadAll(IProgress<double> progress = null, CancellationToken cancel = default);

    Task<MavParamExtValue> WriteOnce(string name, MavParamExtValue value, CancellationToken cancel = default);
}
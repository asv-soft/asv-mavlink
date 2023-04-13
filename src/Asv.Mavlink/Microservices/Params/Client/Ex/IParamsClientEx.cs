using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface IParamsClientEx
{
    bool IsInit { get; set; }
    IRxValue<bool> IsSynced { get; }
    IObservable<IChangeSet<IParamItem, string>> Items { get; }
    Task ReadAll(IProgress<double> progress = null, CancellationToken cancel = default);
    IRxValue<ushort?> RemoteCount { get; }
    IRxValue<ushort> LocalCount { get; }
    Task<decimal> ReadOnce(string name, CancellationToken cancel = default);
}


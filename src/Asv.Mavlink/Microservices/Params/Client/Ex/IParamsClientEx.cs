using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface IParamsClientEx
{
    IParamsClient Base { get; }
    IObservable<(string, MavParamValue)> OnValueChanged { get; }

    
    bool IsInit { get; set; }
    /// <summary>
    /// True if params synced with remote device and local cache
    /// </summary>
    IRxValue<bool> IsSynced { get; }
    /// <summary>
    /// Collection of params items
    /// </summary>
    IObservable<IChangeSet<IParamItem, string>> Items { get; }
    /// <summary>
    /// Send request to remote device for read all params and populate local cache (Items)
    /// </summary>
    /// <param name="progress"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task ReadAll(IProgress<double> progress = null, CancellationToken cancel = default);
    /// <summary>
    /// Count of params on remote device
    /// </summary>
    IRxValue<ushort?> RemoteCount { get; }
    /// <summary>
    /// Count of params in local cache
    /// </summary>
    IRxValue<ushort> LocalCount { get; }
    /// <summary>
    /// Read params once from remote device, update local value and return result
    /// </summary>
    /// <param name="name">Name of param</param>
    /// <param name="cancel">Cancellation token</param>
    /// <returns></returns>
    Task<MavParamValue> ReadOnce(string name, CancellationToken cancel = default);
    /// <summary>
    /// Write params once to remote device, update local value and return result
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task<MavParamValue> WriteOnce(string name, MavParamValue value, CancellationToken cancel = default);
    
    public IObservable<MavParamValue> Filter(string name)
    {
        MavParamHelper.CheckParamName(name);
        return OnValueChanged.Where(x=>x.Item1.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Select(x=>x.Item2);
    }
}


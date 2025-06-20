using System;
 
 using System.Threading;
 using System.Threading.Tasks;
 using ObservableCollections;
 using R3;

 namespace Asv.Mavlink
 {
     /// <summary>
     /// Exposes members to interact with parameters.
     /// </summary>
     public interface IParamsClientEx:IMavlinkMicroserviceClient
     {
         /// <summary>
         /// Base param client interface from which this extension interface is derived.
         /// </summary>
         IParamsClient Base { get; }

         /// <summary>
         /// Gets an observable sequence that represents the OnValueChanged event.
         /// The observable sequence emits tuples containing a string and MavParamValue objects.
         /// The string represents the name of the changed value, and MavParamValue represents the new value.
         /// </summary>
         /// <remarks>
         /// This event is raised whenever a value has changed and provides a way to observe those changes.
         /// Subscribing to the observable sequence returned by this property allows users to receive notifications
         /// whenever a value is changed.
         /// </remarks>
         Observable<(string, MavParamValue)> OnValueChanged { get; }

         /// <summary>
         /// True if params synced with remote device and local cache
         /// </summary>
         ReadOnlyReactiveProperty<bool> IsSynced { get; }

         /// <summary>
         /// Collection of parameters items from the remote device.
         /// </summary>
         IReadOnlyObservableDictionary<string, ParamItem> Items { get; }

         /// <summary>
         /// Send request to remote device for read all parameters and populate local cache (Items)
         /// </summary>
         /// <param name="progress">IProgress to track the progress of operation.</param>
         /// <param name="readMissedOneByOne">If true, finally read one by one skipped params</param>
         /// <param name="cancel">CancellationToken to cancel the operation.</param>
         /// <returns>A Task representing the asynchronous operation.</returns>
         Task ReadAll(IProgress<double>? progress = null,bool readMissedOneByOne = true,  CancellationToken cancel = default);
         /// <summary>
         /// Count of params on remote device
         /// </summary>
         ReadOnlyReactiveProperty<int> RemoteCount { get; }

         /// <summary>
         /// Count of params in local cache
         /// </summary>
         ReadOnlyReactiveProperty<int> LocalCount { get; }

         /// <summary>
         /// Read params once from remote device, update local value and return result
         /// </summary>
         /// <param name="name">Name of the parameter</param>
         /// <param name="cancel">CancellationToken to cancel the operation.</param>
         /// <returns>A Task representing the asynchronous operation.</returns>
         Task<MavParamValue> ReadOnce(string name, CancellationToken cancel = default);

         /// <summary>
         /// Write params once to remote device, update local value and return result
         /// </summary>
         /// <param name="name">Name of the parameter</param>
         /// <param name="value">Value of the parameter to be written.</param>
         /// <param name="cancel">CancellationToken to cancel the operation.</param>
         /// <returns>A Task representing the asynchronous operation.</returns>
         Task<MavParamValue> WriteOnce(string name, MavParamValue value, CancellationToken cancel = default);

         /// <summary>
         /// Filter parameters based on their name.
         /// </summary>
         /// <param name="name">Name of the parameter</param>
         /// <returns>An observable sequence of MavParamValue when the value of the parameter changes.</returns>
         public Observable<MavParamValue> Filter(string name)
         {
             MavParamHelper.CheckParamName(name);
             return OnValueChanged.Where(x => x.Item1.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                 .Select(x => x.Item2);
         }

         /// <summary>
         /// Try to get parameter from local cache or read it once from remote device.
         /// </summary>
         /// <param name="name"></param>
         /// <param name="cancel"></param>
         /// <returns></returns>
         public ValueTask<MavParamValue> GetFromCacheOrReadOnce(string name, CancellationToken cancel = default)
         {
             return Items.TryGetValue(name, out var item) ? new ValueTask<MavParamValue>(item.Value.Value) : new ValueTask<MavParamValue>(ReadOnce(name, cancel));
         }
     }
 }


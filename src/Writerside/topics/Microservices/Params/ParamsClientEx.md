# Params ex-client

You can use the higher-level [IParamsClientEx](#iparamsclientex), which provides convenient caching, synchronization, and simplified parameter access.

```C#
var paramsEx = device.GetMicroservice<IParamsClientEx>() 
    ?? throw new Exception("ParamsClientEx not found");
```

After that, you can read all parameters and cache them locally:

```C#
// Read all parameters from the remote device
await paramsEx.ReadAll(progress: new Progress<double>(p => 
{
    Console.WriteLine($"Progress: {p:P0}");
}));

// Now you can access parameters from the local cache
foreach (var param in paramsEx.Items)
{
    Console.WriteLine($"{param.Key}: {param.Value.Value}");
}
```

You can read or write individual parameters:

```C#
// Read a parameter (updates cache)
var value = await paramsEx.ReadOnce("BARO_PRIMARY");
Console.WriteLine($"Value: {value}");

// Write a parameter (updates cache)
var newValue = await paramsEx.WriteOnce("BARO_PRIMARY", new MavParamValue((byte)1));
Console.WriteLine($"New value: {newValue}");
```

Subscribe to received parameter values:

```C#
// Subscribe to all received parameter values
using var subscription = paramsEx.OnValueChanged.Subscribe(change =>
{
    Console.WriteLine($"Received {change.Item1}: {change.Item2}");
});

// Or filter for a specific parameter
using var specificParam = paramsEx.Filter("BARO_PRIMARY").Subscribe(value =>
{
    Console.WriteLine($"Received BARO_PRIMARY: {value}");
});
```

Check synchronization status:

```C#
using var syncStatus = paramsEx.IsSynced.Subscribe(isSynced =>
{
    Console.WriteLine($"Parameters synced: {isSynced}");
});
```

>Don't forget to dispose subscriptions when they are no longer needed.
{style="warning"}

## [ParamsClientExConfig](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Params/Client/Ex/ParamsClientEx.cs#L16)

Extends `ParameterClientConfig` with cache update batching.

| Property              | Type  | Default | Description                                                                           |
|-----------------------|-------|---------|---------------------------------------------------------------------------------------|
| `ReadAttemptCount`    | `int` | `6`     | Total number of request attempts, including the first attempt.                        |
| `ReadTimeouMs`        | `int` | `1000`  | Timeout for each request attempt, in milliseconds.                                    |
| `ChunkUpdateBufferMs` | `int` | `100`   | Cache update batching interval in milliseconds. Non-positive values disable batching. |

## [IParamsClientEx](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Params/Client/Ex/IParamsClientEx.cs)

Exposes members to interact with parameters.

| Property         | Type                                               | Description                                                                     |
|------------------|----------------------------------------------------|---------------------------------------------------------------------------------|
| `Base`           | `IParamsClient`                                    | Base parameter client interface from which this extension interface is derived. |
| `OnValueChanged` | `Observable<(string, MavParamValue)>`              | Emits each processed incoming parameter value, even if the value is unchanged.  |
| `IsSynced`       | `ReadOnlyReactiveProperty<bool>`                   | Indicates if parameters are synchronized with remote device.                    |
| `Items`          | `IReadOnlyObservableDictionary<string, ParamItem>` | Collection of cached [parameters](ParamItem.md) from the remote device.         |
| `RemoteCount`    | `ReadOnlyReactiveProperty<int>`                    | Number of parameters reported by the remote device; initially `-1`.             |
| `LocalCount`     | `ReadOnlyReactiveProperty<int>`                    | Number of parameters in the local cache.                                        |

| Method                                                                                                            | Return Type                 | Description                                                   |
|-------------------------------------------------------------------------------------------------------------------|-----------------------------|---------------------------------------------------------------|
| `ReadAll(IProgress<double>? progress = null, bool readMissedOneByOne = true, CancellationToken cancel = default)` | `Task`                      | Reads all parameters from remote device and populates cache.  |
| `ReadOnce(string name, CancellationToken cancel = default)`                                                       | `Task<MavParamValue>`       | Reads a parameter from remote device and updates local cache. |
| `WriteOnce(string name, MavParamValue value, CancellationToken cancel = default)`                                 | `Task<MavParamValue>`       | Writes a parameter to remote device and updates local cache.  |
| `Filter(string name)`                                                                                             | `Observable<MavParamValue>` | Filters parameter changes for a specific parameter name.      |
| `GetFromCacheOrReadOnce(string name, CancellationToken cancel = default)`                                         | `ValueTask<MavParamValue>`  | Gets parameter from cache or reads it from remote device.     |

### `IParamsClientEx.ReadAll`
| Parameter             | Type                  | Description                                                      |
|-----------------------|-----------------------|------------------------------------------------------------------|
| `progress`            | `IProgress<double>?`  | Optional progress tracker (reports 0.0 to 1.0).                  |
| `readMissedOneByOne`  | `bool`                | If true, reads missed parameters individually after bulk read.   |
| `cancel`              | `CancellationToken`   | Optional cancellation token.                                     |

### `IParamsClientEx.ReadOnce`
| Parameter | Type                | Description                        |
|-----------|---------------------|------------------------------------|
| `name`    | `string`            | Parameter name.                    |
| `cancel`  | `CancellationToken` | (Optional) The cancellation token. |

### `IParamsClientEx.WriteOnce`
| Parameter | Type                | Description                        |
|-----------|---------------------|------------------------------------|
| `name`    | `string`            | Parameter name (max 16 chars).     |
| `value`   | `MavParamValue`     | Parameter value to write.          |
| `cancel`  | `CancellationToken` | (Optional) The cancellation token. |

### `IParamsClientEx.Filter`
| Parameter | Type     | Description                      |
|-----------|----------|----------------------------------|
| `name`    | `string` | Parameter name to filter for.    |

### `IParamsClientEx.GetFromCacheOrReadOnce`
| Parameter | Type                | Description                        |
|-----------|---------------------|------------------------------------|
| `name`    | `string`            | Parameter name.                    |
| `cancel`  | `CancellationToken` | (Optional) The cancellation token. |

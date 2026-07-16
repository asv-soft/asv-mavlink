# Params client

To work with parameters from a client, request the [IParamsClient](#iparamsclient) from the device instance.

```C#
var paramsClient = device.GetMicroservice<IParamsClient>() 
    ?? throw new Exception("Params client not found");
```

After that, you can read parameters from the remote device:

```C#
// Read parameter by name
var param = await paramsClient.Read("BARO_PRIMARY");
Console.WriteLine($"Parameter value: {param.ParamValue}");
```

You can also write parameters:

```C#
// Write parameter value
var result = await paramsClient.Write(
    "BARO_PRIMARY", 
    MavParamType.MavParamTypeUint8, 
    1.0f);
Console.WriteLine($"New value: {result.ParamValue}");
```

To receive all parameters from a device:

```C#
// Subscribe to incoming parameter values
using var subscription = paramsClient.OnParamValue.Subscribe(param =>
{
    var name = MavlinkTypesHelper.GetString(param.ParamId);
    Console.WriteLine($"{name}: {param.ParamValue}");
});

// Request all parameters from device
await paramsClient.SendRequestList();
```

> Don't forget to dispose subscriptions when they are no longer needed.
{style="warning"}

> For easier parameter management with caching and synchronization, consider using [ParamsClientEx](ParamsClientEx.md).
{style="tip"}

## [ParameterClientConfig](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Params/Client/ParamsClient.cs#L12)

Configures request retries and timeouts for `IParamsClient`.

| Property           | Type  | Default | Description                                                    |
|--------------------|-------|---------|----------------------------------------------------------------|
| `ReadAttemptCount` | `int` | `6`     | Total number of request attempts, including the first attempt. |
| `ReadTimeouMs`     | `int` | `1000`  | Timeout for each request attempt, in milliseconds.             |

## [IParamsClient](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Params/Client/IParamsClient.cs)

Represents a client for handling parameters.

| Property       | Type                            | Description                                                                     |
|----------------|---------------------------------|---------------------------------------------------------------------------------|
| `OnParamValue` | `Observable<ParamValuePayload>` | Property that represents an observable sequence of `ParamValuePayload` objects. |

| Method                                                                                   | Return Type               | Description                                            |
|------------------------------------------------------------------------------------------|---------------------------|--------------------------------------------------------|
| `SendRequestList(CancellationToken cancel = default)`                                    | `ValueTask`               | Sends a request list asynchronously.                   |
| `Read(string name, CancellationToken cancel = default)`                                  | `Task<ParamValuePayload>` | Reads the value of a specified name.                   |
| `Read(ushort index, CancellationToken cancel = default)`                                 | `Task<ParamValuePayload>` | Reads the value of a parameter at the specified index. |
| `Write(string name, MavParamType type, float value, CancellationToken cancel = default)` | `Task<ParamValuePayload>` | Writes a value to a parameter.                         |

### `IParamsClient.SendRequestList`
| Parameter | Type                | Description                        |
|-----------|---------------------|------------------------------------|
| `cancel`  | `CancellationToken` | (Optional) The cancellation token. |

### `IParamsClient.Read(string name)`
| Parameter | Type                | Description                        |
|-----------|---------------------|------------------------------------|
| `name`    | `string`            | The name of the value to read.     |
| `cancel`  | `CancellationToken` | (Optional) The cancellation token. |

### `IParamsClient.Read(ushort index)`
| Parameter | Type                | Description                         |
|-----------|---------------------|-------------------------------------|
| `index`   | `ushort`            | The index of the parameter to read. |
| `cancel`  | `CancellationToken` | (Optional) The cancellation token.  |

### `IParamsClient.Write`
| Parameter | Type                | Description                        |
|-----------|---------------------|------------------------------------|
| `name`    | `string`            | The name of the parameter.         |
| `type`    | `MavParamType`      | The type of the parameter.         |
| `value`   | `float`             | The value to write.                |
| `cancel`  | `CancellationToken` | (Optional) The cancellation token. |

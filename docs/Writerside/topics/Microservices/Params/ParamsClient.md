# Params client

To work with parameters from a client, request the [IParamsClient](#iparamsclient-source) from the device instance.

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

To read all parameters from a device:

```C#
// Request all parameters from device
await paramsClient.SendRequestList();

// Subscribe to incoming parameter values
using var subscription = paramsClient.OnParamValue.Subscribe(param =>
{
    Console.WriteLine($"{param.ParamId}: {param.ParamValue}");
});
```

> Don't forget to dispose subscriptions when they are no longer needed.
{style="warning"}

> For easier parameter management with caching and synchronization, consider using [ParamsClientEx](ParamsClientEx.md).
{style="tip"}

## IParamsClient ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Params/Client/IParamsClient.cs))

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
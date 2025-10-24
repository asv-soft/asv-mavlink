# ParamItem
[Source code](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Params/Client/Ex/ParamItem.cs)

Represents a single parameter on the MAVLink device.  
Provides metadata, current and remote values, synchronization status, and methods for reading/writing the parameter.

| Property   | Type                                      | Description                                                                                  |
|------------|-------------------------------------------|----------------------------------------------------------------------------------------------|
| `Info`     | `ParamDescription`                        | Contains metadata and information about the parameter, such as name, description, and range. |
| `Name`     | `string`                                  | The parameter name.                                                                          |
| `Type`     | `MavParamType`                            | MAVLink type of the parameter.                                                               |
| `Index`    | `ushort`                                  | The parameter index.                                                                         |
| `IsSynced` | `IReadOnlyBindableReactiveProperty<bool>` | Reactive property indicating whether the value synced with remote one.                       |
| `Value`    | `BindableReactiveProperty<MavParamValue>` | Reactive property representing the current value of the parameter.                           |

| Method                                           | Return Type | Description                                                                    |
|--------------------------------------------------|-------------|--------------------------------------------------------------------------------|
| `Task Read(CancellationToken cancel)`            | `Task`      | Reads the parameter value from the remote device and updates the local cache.  |
| `Task Write(CancellationToken cancel)`           | `Task`      | Writes the local parameter value to the remote device.                         |

## `ParamItem.Read`
| Parameter | Type                | Description                        |
|-----------|---------------------|------------------------------------|
| `cancel`  | `CancellationToken` | (Optional) The cancellation token. |

## `ParamItem.Write`
| Parameter | Type                | Description                        |
|-----------|---------------------|------------------------------------|
| `cancel`  | `CancellationToken` | (Optional) The cancellation token. |
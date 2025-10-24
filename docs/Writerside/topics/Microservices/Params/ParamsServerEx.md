# Params ex-server

If you implement a server (autopilot or simulator), you can use [IParamsServerEx](#iparamsserverex-source) for high-level parameter management with metadata, type-safe handling, and reactive updates.

First, register the params service during server device build:

```C#
var serverDevice = ServerDevice.Create(
    new MavlinkIdentity(config.SystemId, config.ComponentId),
    core,
    builder =>
    {
        // register other services...
        
        // IStatusTextServer has to be registered as well
        config.RegisterStatus();
        
        // Register base params service first
        builder.RegisterParams();
        
        // Then register ex-version with parameter descriptions
        var paramDescriptions = new List<IMavParamTypeMetadata>
        {
            new MavParamTypeMetadata("BARO_PRIMARY", MavParamType.MavParamTypeUint8)
            {
                DefaultValue = new MavParamValue((byte)0),
                MinValue = new MavParamValue((byte)0),
                MaxValue = new MavParamValue((byte)2),
                ShortDesc = "Primary barometer selection"
            },
            new MavParamTypeMetadata("WPNAV_SPEED", MavParamType.MavParamTypeInt32)
            {
                DefaultValue = new MavParamValue(500),
                MinValue = new MavParamValue(10),
                MaxValue = new MavParamValue(2000),
            }
        };
        
        builder.RegisterParamsEx(paramDescriptions, new MavParamCStyleEncoding());
    });
```

As you can see, some parameter metadata must be provided during server registration.
You can read more about metadata [here](ParamsMetadata.md).

After building, you can get the service and work with parameters:

```C#
var paramsEx = serverDevice.GetParamsEx();

// Get parameter value
var value = paramsEx["BARO_PRIMARY"];
Console.WriteLine($"Value: {value}");

// Set parameter value
paramsEx["BARO_PRIMARY"] = new MavParamValue((byte)1);

// Subscribe to all parameter changes
using var allChanges = paramsEx.OnUpdated.Subscribe(evt =>
{
    Console.WriteLine($"Parameter {evt.Metadata.Name} changed from {evt.OldValue} to {evt.NewValue}");
});

// Subscribe to specific parameter changes
using var specificChange = paramsEx.OnChange("BARO_PRIMARY").Subscribe(evt =>
{
    Console.WriteLine($"BARO_PRIMARY changed to {evt.NewValue}");
});
```

You can also set up reactive callbacks for specific parameter types:

```C#
// React to float parameter changes
await paramsEx.OnR32(
    myFloatParam, 
    disposeCancel, 
    logger, 
    async (value, firstChange, cancel) =>
    {
        if (firstChange)
        {
            Console.WriteLine($"Initial value: {value}");
        }
        else
        {
            Console.WriteLine($"Parameter changed to: {value}");
            // Apply the new value to your system...
        }
    });

// React to uint8 parameter as boolean
await paramsEx.OnU8Bool(
    myBoolParam,
    disposeCancel,
    logger,
    async (value, firstChange, cancel) =>
    {
        Console.WriteLine($"Boolean parameter: {value}");
        // Apply the boolean value...
    });

// Command-style parameter (triggers action when changed from 0 to non-zero)
paramsEx.OnU8Command(
    myCommandParam,
    disposeCancel,
    logger,
    async (cancel) =>
    {
        Console.WriteLine("Command triggered!");
        // Execute command action...
    });
```

## IParamsServerEx ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Params/Server/Ex/IParamsServerEx.cs))

Represents a high-level parameter server with metadata support, type-safe handling, and reactive updates.

| Property        | Type                                                         | Description                                                                                        |
|-----------------|--------------------------------------------------------------|----------------------------------------------------------------------------------------------------|
| `OnError`       | `Observable<Exception>`                                      | Gets the observable sequence of exceptions that occur in the source sequence.                      |
| `OnUpdated`     | `Observable<ParamChangedEvent>`                              | Gets an observable sequence of `ParamChangedEvent` events that represents updates to the property. |
| `AllParamsList` | `IReadOnlyList<IMavParamTypeMetadata>`                       | List of all parameter metadata.                                                                    |
| `AllParamsDict` | `IReadOnlyDictionary<string,(ushort,IMavParamTypeMetadata)>` | Dictionary of parameter metadata by name.                                                          |

| Indexer                               | Type            | Description                                    |
|---------------------------------------|-----------------|------------------------------------------------|
| `this[string name]`                   | `MavParamValue` | Gets or sets parameter value by name.          |
| `this[IMavParamTypeMetadata param]`   | `MavParamValue` | Gets or sets parameter value by metadata.      |

| Method                                                                                                                                 | Return Type                     | Description                                                                                    |
|----------------------------------------------------------------------------------------------------------------------------------------|---------------------------------|------------------------------------------------------------------------------------------------|
| `OnChange(string name)`                                                                                                                | `Observable<ParamChangedEvent>` | Filters parameter changes for a specific parameter.                                            |
| `OnChange(IMavParamTypeMetadata param)`                                                                                                | `Observable<ParamChangedEvent>` | Filters parameter changes for a specific parameter.                                            |
| `OnRemoteChange(string name)`                                                                                                          | `Observable<ParamChangedEvent>` | Filters remote parameter changes for a specific parameter.                                     |
| `OnRemoteChange(IMavParamTypeMetadata param)`                                                                                          | `Observable<ParamChangedEvent>` | Filters remote parameter changes for a specific parameter.                                     |
| `OnLocalChange(string name)`                                                                                                           | `Observable<ParamChangedEvent>` | Filters local parameter changes for a specific parameter.                                      |
| `OnLocalChange(IMavParamTypeMetadata param)`                                                                                           | `Observable<ParamChangedEvent>` | Filters local parameter changes for a specific parameter.                                      |
| `SetOnChangeReal32(IMavParamTypeMetadata param, Action<float> setCallback)`                                                            | `IDisposable`                   | Sets callback for `float` parameter changes (invokes with initial value then on each change).  |
| `SetOnChangeInt32(IMavParamTypeMetadata param, Action<int> setCallback)`                                                               | `IDisposable`                   | Sets callback for `int32` parameter changes (invokes with initial value then on each change).  |
| `SetOnChangeUint32(IMavParamTypeMetadata param, Action<uint> setCallback)`                                                             | `IDisposable`                   | Sets callback for `uint32` parameter changes (invokes with initial value then on each change). |
| `SetOnChangeUint16(IMavParamTypeMetadata param, Action<ushort> setCallback)`                                                           | `IDisposable`                   | Sets callback for `uint16` parameter changes (invokes with initial value then on each change). |
| `SetOnChangeInt16(IMavParamTypeMetadata param, Action<short> setCallback)`                                                             | `IDisposable`                   | Sets callback for `int16` parameter changes (invokes with initial value then on each change).  |
| `SetOnChangeUint8(IMavParamTypeMetadata param, Action<byte> setCallback)`                                                              | `IDisposable`                   | Sets callback for `uint8` parameter changes (invokes with initial value then on each change).  |
| `SetOnChangeInt8(IMavParamTypeMetadata param, Action<sbyte> setCallback)`                                                              | `IDisposable`                   | Sets callback for `int8` parameter changes (invokes with initial value then on each change).   |
| `OnR32(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, ParamValueCallback<float> setCallback)`           | `Task`                          | Async callback for `float` parameter changes.                                                  |
| `OnU8(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, ParamValueCallback<byte> setCallback)`             | `Task`                          | Async callback for `uint8` parameter changes.                                                  |
| `OnS8(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, ParamValueCallback<sbyte> setCallback)`            | `Task`                          | Async callback for `int8` parameter changes.                                                   |
| `OnU16(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, ParamValueCallback<ushort> setCallback)`          | `Task`                          | Async callback for `uint16` parameter changes.                                                 |
| `OnS16(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, ParamValueCallback<short> setCallback)`           | `Task`                          | Async callback for `int16` parameter changes.                                                  |
| `OnU32(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, ParamValueCallback<uint> setCallback)`            | `Task`                          | Async callback for `uint32` parameter changes.                                                 |
| `OnS32(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, ParamValueCallback<int> setCallback)`             | `Task`                          | Async callback for `int32` parameter changes.                                                  |
| `OnU8Bool(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, ParamValueCallback<bool> setCallback)`         | `Task`                          | Async callback for `uint8` parameter as boolean.                                               |
| `OnU8Enum<TEnum>(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, ParamValueCallback<TEnum> setCallback)` | `Task`                          | Async callback for `uint8` parameter as enum.                                                  |
| `OnU8Command(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, Func<CancellationToken, Task> onEvent)`     | `void`                          | Command trigger on 0 to non-zero transition.                                                   |
| `OnInt32Command(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, Func<CancellationToken, Task> onEvent)`  | `void`                          | Command trigger on 0 to non-zero transition.                                                   |

### `ParamValueCallback<T>` Delegate

A callback delegate used for parameter value change handlers.

```C#
public delegate Task ParamValueCallback<in T>(T value, bool firstChange, CancellationToken cancel);
```

| Parameter     | Type                | Description                                                      |
|---------------|---------------------|------------------------------------------------------------------|
| `value`       | `T`                 | The parameter value.                                             |
| `firstChange` | `bool`              | True if this is the initial value, false for subsequent changes. |
| `cancel`      | `CancellationToken` | Cancellation token.                                              |

### OnChange* Methods

With name:

| Parameter | Type                    | Description                           |
|-----------|-------------------------|---------------------------------------|
| `name`    | `string`                | Parameter name to filter for.         |

With param:

| Parameter | Type                    | Description                           |
|-----------|-------------------------|---------------------------------------|
| `param`   | `IMavParamTypeMetadata` | Parameter metadata to filter for.     |

### SetOnChange* Methods
| Parameter     | Type                    | Description                                           |
|---------------|-------------------------|-------------------------------------------------------|
| `param`       | `IMavParamTypeMetadata` | Parameter metadata.                                   |
| `setCallback` | `Action<T>`             | Callback invoked with parameter value on each change. |

### On* Async Methods
| Parameter       | Type                    | Description                                                        |
|-----------------|-------------------------|--------------------------------------------------------------------|
| `param`         | `IMavParamTypeMetadata` | Parameter metadata.                                                |
| `disposeCancel` | `CancellationToken`     | The cancellation token.                                            |
| `logger`        | `ILogger`               | Logger for error reporting.                                        |
| `setCallback`   | `ParamValueCallback<T>` | Async callback invoked on parameter changes (initial and updates). |

### On*Command Methods
| Parameter       | Type                            | Description                                             |
|-----------------|---------------------------------|---------------------------------------------------------|
| `param`         | `IMavParamTypeMetadata`         | Parameter metadata.                                     |
| `disposeCancel` | `CancellationToken`             | The cancellation token.                                 |
| `logger`        | `ILogger`                       | Logger for error reporting.                             |
| `onEvent`       | `Func<CancellationToken, Task>` | Async handler triggered on 0→non-zero parameter change. |

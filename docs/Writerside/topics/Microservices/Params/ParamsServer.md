# Params server

If you implement a server device or simulator, you can use [IParamsServer](#iparamsserver-source) to handle parameter requests.

First, you need to register the params service when building server device:

```C#
var serverDevice = ServerDevice.Create(
    new MavlinkIdentity(config.SystemId, config.ComponentId), 
    core, 
    builder =>
{
    // register some other services...
    
    builder.RegisterParams();
});
```

After building the device, you can get the service and handle parameter requests:

```C#
var paramsServer = serverDevice.GetParams();

// Subscribe to parameter read requests
using var readSubscription = paramsServer.OnParamRequestRead.Subscribe(request =>
{
    Console.WriteLine($"Parameter read requested: {request.Payload.ParamId}");
    // Handle the request...
});

// Subscribe to parameter list requests
using var listSubscription = paramsServer.OnParamRequestList.Subscribe(request =>
{
    Console.WriteLine("Parameter list requested");
    // Send all parameters...
});

// Subscribe to parameter set requests
using var setSubscription = paramsServer.OnParamSet.Subscribe(packet =>
{
    Console.WriteLine($"Parameter set: {packet.Payload.ParamId} = {packet.Payload.ParamValue}");
    // Update parameter and send response...
});
```

| Method                                                                                         | Return Type | Description                         |
|------------------------------------------------------------------------------------------------|-------------|-------------------------------------|
| `SendParamValue(Action<ParamValuePayload> changeCallback, CancellationToken cancel = default)` | `ValueTask` | Sends a parameter value to clients. |

You can send parameter values to clients:

```C#
await paramsServer.SendParamValue(param =>
{
    param.ParamId = "BARO_PRIMARY";
    param.ParamValue = 1.0f;
    param.ParamType = MavParamType.MavParamTypeUint8;
    param.ParamCount = 100;
    param.ParamIndex = 0;
});
```

> For easier parameter management with metadata and type-safe handling, consider using [ParamsServerEx](ParamsServerEx.md).
{style="tip"}

## IParamsServer ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Params/Server/IParamsServer.cs))

Represents a server that handles low-level parameter operations.

| Property             | Type                                 | Description                                                              |
|----------------------|--------------------------------------|--------------------------------------------------------------------------|
| `OnParamRequestRead` | `Observable<ParamRequestReadPacket>` | Gets the observable sequence for handling param request read packets.    |
| `OnParamRequestList` | `Observable<ParamRequestListPacket>` | Event fired when a param request list packet is received.                |
| `OnParamSet`         | `Observable<ParamSetPacket>`         | Represents an event that occurs when a parameter set packet is received. |

| Method                                                                                         | Return Type | Description                         |
|------------------------------------------------------------------------------------------------|-------------|-------------------------------------|
| `SendParamValue(Action<ParamValuePayload> changeCallback, CancellationToken cancel = default)` | `ValueTask` | Sends a parameter value to clients. |

### `IParamsServer.SendParamValue`
| Parameter        | Type                        | Description                                       |
|------------------|-----------------------------|---------------------------------------------------|
| `changeCallback` | `Action<ParamValuePayload>` | Callback to populate the parameter value payload. |
| `cancel`         | `CancellationToken`         | (Optional) The cancellation token.                |
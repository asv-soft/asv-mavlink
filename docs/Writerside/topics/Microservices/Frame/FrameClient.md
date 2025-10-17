# Frame client

To work with frame configuration from the client side, request the [IFrameClient](#iframeclient-source) microservice from a device instance.

```C#
var frameClient = device.GetMicroservice<IFrameClient>()
    ?? throw new Exception("No frame client found");
```

## Usage

First, load the list of available frame configurations for the connected device:

```C#
// Load available frames from the device
await frameClient.LoadAvailableFrames();

// Access the available frames through MotorFrames property
foreach (var frame in frameClient.MotorFrames.Values)
{
    Console.WriteLine($"Frame: {frame.Id}");
}
```

Get the current frame configuration:

```C#
// Load available frames first
await frameClient.LoadAvailableFrames();

var currentFrame = await frameClient.GetCurrentFrame();

if (currentFrame != null)
{
    Console.WriteLine($"Current frame: {currentFrame}");
}
```

Update the frame configuration:

```C#
// Load available frames first
await frameClient.LoadAvailableFrames();

// Select a frame from MotorFrames collection
var selectedFrame = frameClient.MotorFrames.Values.First();

// Apply the new frame configuration
await frameClient.SetFrame(selectedFrame);
```

> Always use frame configurations from the `MotorFrames` collection. Don't create custom frame instances — they may not be supported by the device.
{style="warning"}

## IFrameClient ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Frame/Client/IFrameClient.cs))

| Property      | Type                                                 | Description                                                   |
|---------------|------------------------------------------------------|---------------------------------------------------------------|
| `MotorFrames` | `IReadOnlyObservableDictionary<string, IMotorFrame>` | Available motor frames. Populated by `LoadAvailableFrames()`. |

| Method                                                                      | Return Type          | Description                                                            |
|-----------------------------------------------------------------------------|----------------------|------------------------------------------------------------------------|
| `LoadAvailableFrames(CancellationToken cancel = default)`                   | `ValueTask`          | Loads available frame types from the device and updates `MotorFrames`. |
| `SetFrame(IMotorFrame motorFrameToSet, CancellationToken cancel = default)` | `Task`               | Updates the frame type for the current device.                         |
| `GetCurrentFrame(CancellationToken cancel = default)`                       | `Task<IMotorFrame?>` | Gets the frame type currently selected on the device.                  |

### `IFrameClient.LoadAvailableFrames`

| Parameter | Type                | Description                  |
|-----------|---------------------|------------------------------|
| `cancel`  | `CancellationToken` | Optional cancellation token. |

### `IFrameClient.SetFrame`

| Parameter         | Type                | Description                  |
|-------------------|---------------------|------------------------------|
| `motorFrameToSet` | `IMotorFrame`       | Frame type to set.           |
| `cancel`          | `CancellationToken` | Optional cancellation token. |

### `IFrameClient.GetCurrentFrame`

| Parameter | Type                | Description                  |
|-----------|---------------------|------------------------------|
| `cancel`  | `CancellationToken` | Optional cancellation token. |

## Implementations

Different vehicle types have their own implementations of [`IFrameClient`](#iframeclient-source):

- [ArduCopterFrameClient](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Copter/ArduCopterFrameClient.cs) — uses `FRAME_CLASS` and `FRAME_TYPE` parameters.
- [ArduQuadPlaneFrameClient](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Devices/Client/Vehicles/Ardu/Plane/Quad/ArduQuadPlaneFrameClient.cs) — uses `Q_FRAME_CLASS` and `Q_FRAME_TYPE` parameters.
# Frame client

To work with the frame configuration from the client side, request the [IFrameClient](#iframeclient-source) microservice from a device instance.

```C#
var frameClient = device.GetMicroservice<IFrameClient>()
    ?? throw new Exception("No frame client found");
```

## Usage

First, load the list of available frame configurations from the connected device:

```C#
// Load available frames from the device
await frameClient.LoadAvailableFrames();

// Access the available frames through the Frames property
foreach (var frame in frameClient.Frames.Values)
{
    Console.WriteLine($"Frame: {frame.Id}");
}
```

Get the current frame configuration:

```C#
// Load available frames first
await frameClient.LoadAvailableFrames();

await frameClient.LoadCurrentFrame();

var subscription = frameClient.CurrentFrame.Subscribe(currentFrame =>
{
    if (currentFrame is null) 
    {
        return;
    }
    
    Console.WriteLine($"Current frame: {currentFrame}");
});
```

> Do not forget to dispose subscriptions when they are no longer needed.
{style="warning"}

Update the frame configuration:

```C#
// Load available frames first
await frameClient.LoadAvailableFrames();

// Select a frame from the Frames collection
var selectedFrame = frameClient.Frames.Values.First();

// Apply the new frame configuration
await frameClient.SetFrame(selectedFrame);
```

> Always use frames from the `Frames` collection. 
> Do not create custom `IDroneFrame` instances — they may not be supported by the device.
{style="warning"}

## IFrameClient ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Frame/Client/IFrameClient.cs))

| Property | Type                                                 | Description                                                   |
|----------|------------------------------------------------------|---------------------------------------------------------------|
| `Frames` | `IReadOnlyObservableDictionary<string, IDroneFrame>` | Available drone frames. Populated by `LoadAvailableFrames()`. |

| Method                                                                      | Return Type | Description                                                                                                                           |
|-----------------------------------------------------------------------------|-------------|---------------------------------------------------------------------------------------------------------------------------------------|
| `LoadAvailableFrames(CancellationToken cancel = default)`                   | `ValueTask` | Loads available frame types from the device and updates the `Frames` collection.                                                      |
| `SetFrame(IDroneFrame droneFrameToSet, CancellationToken cancel = default)` | `Task`      | Updates the frame type for the current device.                                                                                        |
| `LoadCurrentFrame(CancellationToken cancel = default)`                      | `Task`      | Loads the current frame configuration from the device and starts reactively updating `CurrentFrame` when the frame parameters change. |

### `IFrameClient.LoadAvailableFrames`

| Parameter | Type                | Description                                |
|-----------|---------------------|--------------------------------------------|
| `cancel`  | `CancellationToken` | An optional token to cancel the operation. |

### `IFrameClient.SetFrame`

| Parameter         | Type                | Description                                |
|-------------------|---------------------|--------------------------------------------|
| `droneFrameToSet` | `IDroneFrame`       | Frame type to use.                         |
| `cancel`          | `CancellationToken` | An optional token to cancel the operation. |

### `IFrameClient.LoadCurrentFrame`

| Parameter | Type                | Description                                |
|-----------|---------------------|--------------------------------------------|
| `cancel`  | `CancellationToken` | An optional token to cancel the operation. |

## Implementations

Different vehicle types provide their own implementations of [`IFrameClient`](#iframeclient-source):

- [Ardu devices](ArduFrameClient.md)

Each device has its own set of available frame configurations based on ArduPilot compatibility tables.

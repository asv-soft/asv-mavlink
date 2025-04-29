# With Ardu SITL

## [Ardu SITL](https://ardupilot.org/dev/docs/sitl-simulator-software-in-the-loop.html) Installation

You have two options of the SITL installation:
1. Through [docker](https://www.docker.com)
2. Through **.exe** *(note: windows only)*

### With Docker { #with-docker }
*Note: we assume that you already have [docker](https://docs.docker.com/engine/install/) installed.*

1. Open your console and write.
   (note: you may also need to write sudo before every command on [Unix](https://en.wikipedia.org/wiki/Unix) systems)
```bash
docker pull flurps1/asv-sitl
```

2. Run container with port 5760 (note: you can select another free port)
```bash
docker run -p 5760:5760 flurps1/asv-sitl
```

### With .exe

*Note: only for windows!*
*If you have any problems with this SITL it is better to use [docker](#with-docker)*

1. Download files from [here](https://firmware.ardupilot.org/Tools/MissionPlanner/sitl/CopterStable/) and put them in any folder.
2. Change all ```.elf``` to ```.exe```
3. Create a ```.bat``` file in the folder with name ```run_copter.bat```.
4. Edit this file with a notepad and add code:
```text
ArduCopter.exe -M+ -O55.2905802,61.6063891,191.303759999999,0 -s10 --uartA tcp:0 
rm logs
```
5. Run .bat file.
6. Now SITL is online.
![image](bat-sitl-windows.jpg)

## How to connect to SITL with Asv.Mavlink library

A lot is similar to the [virtual connection](With-Virtual-Connection.md) approach, but there are some key differences.

Repeat up to this [step](With-Virtual-Connection.md#setup-task-completion-source-and-cancellation-token)

### Setup task completion source and cancellation token

1. Create a task completion source.
   You'll need it to wait for a response from the server.
```c#
var tcs = new TaskCompletionSource<HeartbeatPayload>();
```

2. Create a cancellation token source with a timeout.
   The cancellation token will trigger after 20 seconds. You can change this by setting a different time span.
```c#
using var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(20), TimeProvider.System);
```

3. Register a delegate that will be called when the cancellation token is canceled.
   This will stop tcs from running in a loop when the token fails.
```c#
cancel.Token.Register(() => tcs.TrySetCanceled());
```

### Create router

1. Create protocol factory
```c#
var protocol = Protocol.Create(builder =>
{
    builder.RegisterMavlinkV2Protocol();
    builder.Features.RegisterBroadcastFeature<MavlinkMessage>();
    builder.Formatters.RegisterSimpleFormatter();
});
```

2. Create router
```c#
await using var router = protocol.CreateRouter("ROUTER");
```

3. Add port with the drone connection data
```c#
router.AddTcpClientPort(p =>
{
    p.Host = "127.0.0.1";
    p.Port = 5760;
});
```

### Device explorer creation

We need a device explorer to search for the drone

1. Create packet sequence calculator
```c#
var seq = new PacketSequenceCalculator();
```

2. Create id of the system that will search for the drone
```c#
var seq = new PacketSequenceCalculator();
```

3. Create device explorer
```c#
await using var deviceExplorer = DeviceExplorer.Create(router, builder =>
{
    builder.SetConfig(new ClientDeviceBrowserConfig()
    {
        DeviceTimeoutMs = 1000,
        DeviceCheckIntervalMs = 30_000,
    });
    builder.Factories.RegisterDefaultDevices(
        new MavlinkIdentity(identity.SystemId, identity.ComponentId),
        seq,
        new InMemoryConfiguration());
});
```

### Find the device

1. Create variable for the client device
```c#
IClientDevice? drone = null;
```

2. Subscribe to search for the drone
```c#
using var sub = deviceExplorer.Devices
    .ObserveAdd()
    .Take(1)
    .Subscribe(kvp => 
    { 
        drone = kvp.Value.Value; 
        tcs.TrySetResult(); 
    });
```

3. Wait until the subscription locates a drone
```c#
await tcs.Task;
```

4. Check if the drone was successfully found
```c#
if (drone is null)
{
    throw new Exception("Drone not found");
}
```

### Initialize the drone

1. Create new task completion source
```c#
tcs = new TaskCompletionSource();
```

2. Subscribe to stop completion source when the client is initialized
```c#
using var sub2 = drone.State
    .Subscribe(x => 
    { 
        if (x != ClientDeviceState.Complete) 
        { 
            return; 
        }
        
        tcs.TrySetResult(); 
    });
```

3. Wait until the drone is initialized
```c#
await tcs.Task;
```

### Search for the heartbeat client

1. Get heartbeat microservice
```c#
await using var heartbeat = drone?.GetMicroservice<IHeartbeatClient>(); 
```

2. Check if the heartbeat client was successfully found
```c#
if (heartbeat is null)
{
    throw new Exception("No control client found");
}
```

### Test the connection

1. Create new task completion source
```c#
tcs = new TaskCompletionSource();
```

2. Create counter to limit application's lifetime
```c#
var count = 0;
```

3. Subscribe to the RawHeartbeat pipe to show heartbeat packages
   This code catches first 20 heartbeat packages
```c#
using var sub3 = heartbeat.RawHeartbeat
    .ThrottleLast(TimeSpan.FromMilliseconds(100))
    .Subscribe(p => 
    { 
        if (p is null) 
        { 
            return; 
        }
        
        Console.WriteLine($"Heartbeat type: {p.Type}, Heartbeat baseMode: {p.BaseMode}");
        
        if (count >= 19) 
        { 
            tcs.TrySetResult(); 
            return;
        }
        
        count++; 
    });
```

4. Wait until the stop is called manually in the subscription
```c#
await tcs.Task;
```

### Proper Output Example

```c#
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81

Process finished with exit code 0.
```

## Full code

```c#
using Asv.Cfg;
using Asv.IO;
using Asv.Mavlink;
using ObservableCollections;
using R3;

// Setup
var tcs = new TaskCompletionSource();
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20), TimeProvider.System);
await using var s = cts.Token.Register(() => tcs.TrySetCanceled());
//

// Router creation
var protocol = Protocol.Create(builder =>
{
    builder.RegisterMavlinkV2Protocol();
    builder.Features.RegisterBroadcastFeature<MavlinkMessage>();
    builder.Formatters.RegisterSimpleFormatter();
});

await using var router = protocol.CreateRouter("ROUTER");
router.AddTcpClientPort(p =>
{
    p.Host = "127.0.0.1";
    p.Port = 5760;
});
//

// Device explorer creation
var seq = new PacketSequenceCalculator();
var identity = new MavlinkIdentity(255, 255);
await using var deviceExplorer = DeviceExplorer.Create(router, builder =>
{
    builder.SetConfig(new ClientDeviceBrowserConfig()
    {
        DeviceTimeoutMs = 1000,
        DeviceCheckIntervalMs = 30_000,
    });
    builder.Factories.RegisterDefaultDevices(
        new MavlinkIdentity(identity.SystemId, identity.ComponentId),
        seq,
        new InMemoryConfiguration());
});
//

// Device search
IClientDevice? drone = null;
using var sub = deviceExplorer.Devices
    .ObserveAdd()
    .Take(1)
    .Subscribe(kvp => 
    { 
        drone = kvp.Value.Value; 
        tcs.TrySetResult(); 
    });

await tcs.Task;

if (drone is null)
{
    throw new Exception("Drone not found");
}
//

// Drone init
tcs = new TaskCompletionSource();

using var sub2 = drone.State
    .Subscribe(x => 
    { 
        if (x != ClientDeviceState.Complete) 
        { 
            return; 
        }
        
        tcs.TrySetResult(); 
    });

await tcs.Task;
//

// Heartbeat client search
await using var heartbeat = drone?.GetMicroservice<IHeartbeatClient>(); 

if (heartbeat is null)
{
    throw new Exception("No control client found");
}
//

// Test
tcs = new TaskCompletionSource();

var count = 0;
using var sub3 = heartbeat.RawHeartbeat
    .ThrottleLast(TimeSpan.FromMilliseconds(100))
    .Subscribe(p => 
    { 
        if (p is null) 
        { 
            return; 
        }
        
        Console.WriteLine($"Heartbeat type: {p.Type}, Heartbeat baseMode: {p.BaseMode}");
        
        if (count >= 19) 
        { 
            tcs.TrySetResult(); 
            return;
        }
        
        count++; 
    });

await tcs.Task;
//
```
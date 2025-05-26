# With Virtual Connection

## Create console application

*Note: [Rider](https://www.jetbrains.com/rider/) will be used in this guide, you are free to choose any other IDE for C# that you prefer.*

1. Open your IDE and select the Project Type: Console
   ![image](step1-console-type.png){ style="block" }

2. Leave everything as default. You may choose a different project name if you wish.
3. Press **Create** to create a project.

4. You should now see the default project page.
   ![image](step1-final.png){ style="block" }{ thumbnail = true }

## Install Required NuGet Packages

1. Open the NuGet Package Manager and search for the [Asv.Mavlink](https://www.nuget.org/packages/Asv.Mavlink/) packet.
2. Install the latest version (this guide is relevant to version 4.0.0 and higher)
   ![image](step2-mavlink-package.png){ style="block" }

The [DeepEqual](https://github.com/jamesfoster/DeepEqual) package is also used in this guide for testing purposes.
![image](step2-deep-equal.png){ style="block" }

## Setup task completion source and cancellation token

1. Create a task completion source.
   You'll need it to wait for a client initialization.
```c#
var tcsInitialize = new TaskCompletionSource();
```

2. Create a cancellation token source with a timeout.
   The cancellation token will trigger after 20 seconds. You can change this by setting a different time span.
```c#
using var cancelInitialize = new CancellationTokenSource(TimeSpan.FromSeconds(20), TimeProvider.System);
```

3. Register a delegate that will be called when the cancellation token is canceled.
   This will stop tcs from running in a loop when the token fails.
```c#
cancelInitialize.Token.Register(() => tcsInitialize.TrySetCanceled());
```

## Create virtual mavlink connection

*Note: Virtual MAVLink connection is used for testing purposes only.
You'll need to create a router for a real MAVLink connection.
Refer to [](With-Ardu-SITL.md) for more information.*

1. Create a virtual connection.
```c#
var protocol = Protocol.Create(builder =>
{
    builder.SetTimeProvider(TimeProvider.System);
    builder.RegisterMavlinkV2Protocol();
    builder.Features.RegisterBroadcastFeature<MavlinkMessage>();
    builder.Formatters.RegisterSimpleFormatter();
});
        
var link = protocol.CreateVirtualConnection();
```

2. Create a MAVLink client identity.
   Assign SystemId, ComponentId, TargetSystemId, and TargetComponentId.
   You can read more about it in the official [mavlink guide](https://mavlink.io/en/services/mavlink_id_assignment.html#system-id-assignment).
```c#
var identity = new MavlinkClientIdentity(1,2,3,4);
```

## Device creation

1. Create a packet sequence calculator.
```c#
// Class for calculating the next sequence number of a packet
var clientSeq = new PacketSequenceCalculator();
```

2. Provide core services with a log factory to create a logger and a different time provider.
   This class provides everything that your device or MAVLink microservice will use.
```c#
var clientCore = new CoreServices(
    link.Client, 
    clientSeq, 
    logFactory: null, 
    timeProvider: TimeProvider.System, 
    new DefaultMeterFactory()
);
```

3. Create a default device id.
```c#
var deviceId = new MavlinkClientDeviceId("COPTER", identity);
```

4. Create a configuration for the client device.
```c#
MavlinkClientDeviceConfig config = new()
{
    Heartbeat =
    {
        HeartbeatTimeoutMs = 1000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 10,
    }
};
```

5. Finally, create a MavlinkClient client device.
```c#
var client = new MavlinkClientDevice(
    deviceId, 
    config, 
    [], 
    clientCore
);
```

## Create mavlink server

1. Create a packet sequence calculator and core services for the server.
```c#
var serverSeq = new PacketSequenceCalculator();
var serverCore = new CoreServices(
    link.Server,
    serverSeq, 
    logFactory: null, 
    timeProvider: TimeProvider.System, 
    new DefaultMeterFactory()
);
```

2. Create heartbeat server.
```c#
var server = new HeartbeatServer(
    identity.Target, 
    new MavlinkHeartbeatServerConfig(), 
    serverCore
);
```

3. Start the server (note: some servers may not have this method).
```c#
server.Start();
```

4. Create a variable to save the payload sent from the server.
```c#
HeartbeatPayload? payload = null;
```

5. Set the payload for the MAVLink package.
   This operation triggers the process that sends data to the client.
```c#
server.Set(p =>
{
p.Type = MavType.MavTypeBattery;
p.BaseMode = MavModeFlag.MavModeFlagGuidedEnabled;
Console.WriteLine($"From Server Type: {p.Type}, From server baseMode: {p.BaseMode}");
payload = p;
});
```

## Heartbeat client initialization

1. Call client initialization
``` c#
client.Initialize();
```

2. Wait until the client is initialized
``` c#
var sub2 = client.State
.Subscribe(x =>
{ 
    if (x != ClientDeviceState.Complete) 
    { 
        return; 
    }
    
    tcsInitialize.TrySetResult(); 
});

await tcsInitialize.Task;
```
3. Dispose the subscription, because we don't need it anymore
``` c#
sub2.Dispose();
```

## Setup for Heartbeat test

Setup new tcs and cancellation token for the heartbeat test
``` c#
var tcsHeartbeat = new TaskCompletionSource();
using var cancelHeartbeat = new CancellationTokenSource(TimeSpan.FromSeconds(20), TimeProvider.System);
cancelHeartbeat.Token.Register(() => tcsHeartbeat.TrySetCanceled());
```

## Check for the Heartbeat client
```c#
var heartbeat = client.GetMicroservice<IHeartbeatClient>();

if (heartbeat is null)
{
    throw new Exception("Heartbeat client not found");
}
```

## Send data from server to client

1. Asv.Mavlink uses the R3 library, which allows you to use a reactive programming approach.
   To catch data received from the server, subscribe to its source.
```c#
HeartbeatPayload? result = null;
var sub = heartbeat.RawHeartbeat
    .Subscribe(p => 
    { 
        if (p is null) 
        { 
            return; 
        }
        
        Console.WriteLine($"Heartbeat type: {p.Type}, Heartbeat baseMode: {p.BaseMode}");
        
        result = p;
        tcsHeartbeat.TrySetResult(); 
    });
```

2. Wait for the result and remove subscription
```c#
await tcsHeartbeat.Task;
sub.Dispose();
```

## Compare data

```c#
Console.WriteLine(
    !payload.IsDeepEqual(result) 
        ? "Result is not equal to payload from server" 
        : "Payload from server is equal to result"
);
```

## Code's output

Proper console output should look something like that:
```bash
From Server Type: MavTypeBattery, From server baseMode: MavModeFlagGuidedEnabled
Heartbeat type: MavTypeBattery, Heartbeat baseMode: MavModeFlagGuidedEnabled
Payload from server is equal to result

Process finished with exit code 0.
```

## Clear the resources
```c#
await Task.Delay(TimeSpan.FromMilliseconds(500)); // Wait till the server gets unlocked

client.Dispose();
server.Dispose();
heartbeat.Dispose();
```

## Complete code { #virtual-complete-code }

```c#
using Asv.Common;
using Asv.IO;
using Asv.Mavlink;
using Asv.Mavlink.Minimal;
using DeepEqual.Syntax;
using R3;

// Setup for device initialization
var tcsInitialize = new TaskCompletionSource();
using var cancelInitialize = new CancellationTokenSource(TimeSpan.FromSeconds(20), TimeProvider.System);
cancelInitialize.Token.Register(() => tcsInitialize.TrySetCanceled());

// Virtual mavlink connection
var protocol = Protocol.Create(builder =>
{
    builder.SetTimeProvider(TimeProvider.System);
    builder.RegisterMavlinkV2Protocol();
    builder.Features.RegisterBroadcastFeature<MavlinkMessage>();
    builder.Formatters.RegisterSimpleFormatter();
});
        
var link = protocol.CreateVirtualConnection();
//

// Client Device
var identity = new MavlinkClientIdentity(1,2,3,4);

var clientSeq = new PacketSequenceCalculator();
var clientCore = new CoreServices(
    link.Client, 
    clientSeq, 
    logFactory: null, 
    timeProvider: TimeProvider.System, 
    new DefaultMeterFactory()
);

var deviceId = new MavlinkClientDeviceId("COPTER", identity);
MavlinkClientDeviceConfig config = new()
{
    Heartbeat =
    {
        HeartbeatTimeoutMs = 1000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 10,
    }
};

var client = new MavlinkClientDevice(
    deviceId, 
    config, 
    [], 
    clientCore
);
//

// Heartbeat server
var serverSeq = new PacketSequenceCalculator();
var serverCore = new CoreServices(
    link.Server,
    serverSeq, 
    logFactory: null, 
    timeProvider: TimeProvider.System, 
    new DefaultMeterFactory()
);

var server = new HeartbeatServer(
    identity.Target, 
    new MavlinkHeartbeatServerConfig(), 
    serverCore
);

server.Start();

HeartbeatPayload? payload = null;
server.Set(p =>
{
    p.Type = MavType.MavTypeBattery;
    p.BaseMode = MavModeFlag.MavModeFlagGuidedEnabled;
    Console.WriteLine($"From Server Type: {p.Type}, From server baseMode: {p.BaseMode}");
    payload = p;
});
//

// Heartbeat client initialization
client.Initialize();
var sub2 = client.State
    .Subscribe(x => 
    {
        if (x != ClientDeviceState.Complete) 
        { 
            return; 
        }
        
        tcsInitialize.TrySetResult();
    });

await tcsInitialize.Task;
sub2.Dispose();
//

// Setup for Heartbeat test
var tcsHeartbeat = new TaskCompletionSource();
using var cancelHeartbeat = new CancellationTokenSource(TimeSpan.FromSeconds(20), TimeProvider.System);
cancelHeartbeat.Token.Register(() => tcsHeartbeat.TrySetCanceled());
//

// Test
var heartbeat = client.GetMicroservice<IHeartbeatClient>();

if (heartbeat is null)
{
    throw new Exception("Heartbeat client not found");
}

HeartbeatPayload? result = null;
var sub = heartbeat.RawHeartbeat
    .Subscribe(p => 
    { 
        if (p is null) 
        { 
            return; 
        }
        
        Console.WriteLine($"Heartbeat type: {p.Type}, Heartbeat baseMode: {p.BaseMode}");
        
        result = p;
        tcsHeartbeat.TrySetResult(); 
    });

await tcsHeartbeat.Task;
sub.Dispose();

Console.WriteLine(
    !payload.IsDeepEqual(result) 
        ? "Result is not equal to payload from server" 
        : "Payload from server is equal to result"
);
//

await Task.Delay(TimeSpan.FromMilliseconds(500)); // Wait till the server gets unlocked

// Dispose
client.Dispose();
server.Dispose();
heartbeat.Dispose();
//
```
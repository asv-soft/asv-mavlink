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
   You'll need it to wait for a response from the server.
```c#
var tcs = new TaskCompletionSource<HeartbeatPayload>();
```

2. Create a cancellation token source with a timeout.
   The cancellation token will trigger after 10 seconds. You can change this by setting a different time span.
```c#
var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10), TimeProvider.System);
```

3. Register a delegate that will be called when the cancellation token is canceled.
   This will stop tcs from running in a loop when the token fails.
```c#
cancel.Token.Register(() => tcs.TrySetCanceled());
```

## Create virtual mavlink connection

*Note: Virtual MAVLink connection is used for testing purposes only.
You'll need to create a router for a real MAVLink connection.
Refer to [](With-Ardu-SITL.md) for more information.*

1. Create a virtual connection.
```c#
var protocol = Protocol.Create(builder =>
{
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
    timeProvider: null, 
    new DefaultMeterFactory()
);
```

3. Create a default device id.
```c#
var deviceId = new MavlinkClientDeviceId("copter", identity);
```

4. Create a configuration for the vehicle client device.
```c#
var config = new VehicleClientDeviceConfig();
```

5. Finally, create an ArduCopter client device.
```c#
var arduCopter = new ArduCopterClientDevice(
    deviceId, 
    config, 
    ImmutableArray<IClientDeviceExtender>.Empty, 
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
    timeProvider: null, 
    new DefaultMeterFactory()
);
```

2. Create a heartbeat server.
```c#
var mavlinkServer = new HeartbeatServer(
    identity.Target, 
    new MavlinkHeartbeatServerConfig(), 
    serverCore
);
```

## Send data from server to client

1. Asv.Mavlink uses the R3 library, which allows you to use a reactive programming approach.
   To catch data received from the server, subscribe to its source.
```c#
arduCopter.Heartbeat.RawHeartbeat.Subscribe(p =>
{
    if (p is null) // if we have no data we skip it
    {
        return;
    }
    
    // We use console to see that everything works fine
    Console.WriteLine($"Heartbeat type: {p.Type}, Heartbeat baseMode: {p.BaseMode}");
    tcs.TrySetResult(p); // here we get the result
});
```

2. tart the server (note: some servers may not have this method).
```c#
mavlinkServer.Start();
```

3. Create a variable to save the payload sent from the server.
```c#
HeartbeatPayload? payload = null;
```

4. Set the payload for the MAVLink package.
   This operation triggers the process that sends data to the client.
```c#
mavlinkServer.Set(p =>
{
p.Type = MavType.MavTypeBattery;
p.BaseMode = MavModeFlag.MavModeFlagGuidedEnabled;
Console.WriteLine($"From Server Type: {p.Type}, From server baseMode: {p.BaseMode}");
payload = p;
});
```

5. Get the result
```c#
var result = await tcs.Task;
```

## Compare data

```c#
if (!payload.IsDeepEqual(result))
{
    Console.WriteLine("Result is not equal to payload from server");
}

Console.WriteLine("Payload from server is equal to result");
```

## Code's output

Proper console output should look something like that:
```bash
From Server Type: MavTypeBattery, From server baseMode: MavModeFlagGuidedEnabled
Heartbeat type: MavTypeBattery, Heartbeat baseMode: MavModeFlagGuidedEnabled
Payload from server is equal to result

Process finished with exit code 0.
```

## Complete code { #virtual-complete-code }

```c#
using System.Collections.Immutable;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink;
using Asv.Mavlink.Minimal;
using DeepEqual.Syntax;
using R3;

// Setup
var tcs = new TaskCompletionSource<HeartbeatPayload>();
var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10), TimeProvider.System);
cancel.Token.Register(() => tcs.TrySetCanceled());

// Virtual mavlink connection
var protocol = Protocol.Create(builder =>
{
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
    timeProvider: null, 
    new DefaultMeterFactory()
);

var deviceId = new MavlinkClientDeviceId("copter", identity);
var config = new VehicleClientDeviceConfig();

var arduCopter = new ArduCopterClientDevice(
    deviceId, 
    config, 
    ImmutableArray<IClientDeviceExtender>.Empty, 
    clientCore
);
//

// Heartbeat server
var serverSeq = new PacketSequenceCalculator();
var serverCore = new CoreServices(
    link.Server,
    serverSeq, 
    logFactory: null, 
    timeProvider: null, 
    new DefaultMeterFactory()
);

var mavlinkServer = new HeartbeatServer(
    identity.Target, 
    new MavlinkHeartbeatServerConfig(), 
    serverCore
);
//

arduCopter.Heartbeat.RawHeartbeat.Subscribe(p =>
{
    if (p is null)
    {
        return;
    }
    
    Console.WriteLine($"Heartbeat type: {p.Type}, Heartbeat baseMode: {p.BaseMode}");
    tcs.TrySetResult(p);
});

mavlinkServer.Start();

HeartbeatPayload? payload = null;
mavlinkServer.Set(p =>
{
    p.Type = MavType.MavTypeBattery;
    p.BaseMode = MavModeFlag.MavModeFlagGuidedEnabled;
    Console.WriteLine($"From Server Type: {p.Type}, From server baseMode: {p.BaseMode}");
    payload = p;
});

var result = await tcs.Task;

if (!payload.IsDeepEqual(result))
{
    Console.WriteLine("Result is not equal to payload from server");
}

Console.WriteLine("Payload from server is equal to result");
```
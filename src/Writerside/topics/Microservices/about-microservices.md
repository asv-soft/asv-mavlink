# Microservices

The MAVLink protocol defines microservices as:

> *The MAVLink microservices define higher-level protocols that MAVLink systems can adopt in order to better inter-operate.*

> You can read more about microservices on the [official MAVLink documentation page](https://mavlink.io/en/services/).

We also use microservices from MAVLink.
You can usually access them from an `IClientDevice` like this:
```c#
// Heartbeat client search
var heartbeat = drone.GetMicroservice<IHeartbeatClient>(); 
```
`GetMicroservice` returns `null` if it fails to find the requested microservice.

>Do not dispose microservices manually.
>Even though they implement IDisposable, their lifecycle is managed by the IClientDevice.
>This rule generally applies to all MAVLink objects:
>if you didn’t create it yourself, you are usually not responsible for managing its resources.
{style="warning"}

Microservices are designed to provide user-friendly abstractions for different parts of the MAVLink protocol.
They are fully compatible with other MAVLink devices — for example, you can connect your own FTP client to the autopilot's FTP server and vice versa.
We also provide several custom MAVLink services, which are typically built by combining multiple standard microservices.

Available microservices:
* <a href="Adsb.md" id="adsb">Adsb</a>
* <a href="AsvAudio.md" id="asv-audio"/>
* <a href="AsvChart.md" id="asv-chart"/>
* <a href="AsvGbs.md" id="asv-gbs"/>
* <a href="AsvRadio.md" id="asv-radio"/>
* <a href="AsvRsga.md" id="asv-rsga"/>
* <a href="AsvSdr.md" id="asv-sdr"/>
* <a href="Commands.md" id="commands"/>
* <a href="Control.md" id="control"/>
* <a href="Dgps.md" id="dgps"/>
* <a href="Diagnostic.md" id="diagnostic"/>
* <a href="Frame.md" id="frame"/>
* <a href="Ftp.md" id="ftp"/>
* <a href="Gnss.md" id="gnss"/>
* <a href="Heartbeat.md" id="heartbeat"/>
* <a href="Logging.md" id="logging"/>
* <a href="Missions.md" id="missions"/>
* <a href="Mode.md" id="mode"/>
* <a href="MotorTest.md" id="motor-test"/>
* <a href="Params.md" id="params"/>
* <a href="ParamsExt.md" id="params-ext"/>
* <a href="Position.md" id="position">Position (Offboard Control Interface)</a>
* <a href="StatusText.md" id="status-text"/>
* <a href="Telemetry.md" id="telemetry"/>
* <a href="V2Extention.md" id="v2extention"/>

## Note about the reactive approach (R3 library)

This package was originally developed for our [asv-drones](https://github.com/asv-soft/asv-drones) desktop application.
We chose a **reactive approach** because it is more efficient and easier to use compared to classic event-based patterns.
We initially used Microsoft's Rx (System.Reactive) but later switched to [R3](https://github.com/Cysharp/R3).

Reactivity is very similar to events — in fact, it uses events under the hood.
Here’s a short introduction to make it easier for you to get started:
Use our [](get-started.md) guide to experiment with microservices and reactivity.

Most of our services expose data *pipes*.
Clients and servers from [Asv.Common](https://github.com/asv-soft/asv-common) have Rx and Tx pipes to receive and send data — similar to microcontrollers with UART interfaces.
[Asv.Mavlink](https://github.com/asv-soft/asv-mavlink) microservices follow the same principle.

Let’s take a closer look at the heartbeat client:

1. Get the microservice:
```c#
var heartbeat = device.GetMicroservice<IHeartbeatClient>() ?? throw new Exception("Heartbeat client not found");
```

2. Subscribe to RawHeartbeat and print the MavType:
```c#
var sub = heartbeat.RawHeartbeat.Subscribe(p =>
{ 
    Console.WriteLine($"MavType = {p.Type}");
});
```

3. Dispose of the subscription when it’s no longer needed:
```c#
sub.Dispose();
```

In the example above, we use `Subscribe()` from the [R3](https://github.com/Cysharp/R3) library to print all heartbeat packets to the console.
The code inside the `Action` runs every time the `ReadOnlyReactiveProperty<HeartbeatPayload?>` changes.
You can also use `SubscribeAwait()` to run code asynchronously.
Each subscription creates unmanaged resources (`IDisposable`), 
so remember to dispose them manually with `.Dispose()` — otherwise you may end up with a memory leak.

We hope this guide helps you get started.
Feel free to experiment — try subscribing to other reactive properties of the heartbeat client to explore more data.





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

Almost everything is similar to the [virtual connection](With-Virtual-Connection.md) approach, but there are some key differences.

1. Copy all the code from [here](With-Virtual-Connection.md#virtual-complete-code)

2. Change
```c#
var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(10), TimeProvider.System);
```
to
```c#
var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
```

3. Change
```c#
var link = protocol.CreateVirtualConnection();
```
to
```c#
var router = protocol.CreateRouter("router");
```

4. After ```var router = protocol.CreateRouter("router");``` add
```c#
router.AddTcpClientPort(p =>
{
    p.Host = "127.0.0.1";
    p.Port = 5760; // change 
});
```

5. Change
```c#
var identity = new MavlinkClientIdentity(1,2,3,4);
```
to
```c#
var identity = new MavlinkClientIdentity(2,3,1,1);
```
*Note: TargetSystemId and TargetComponentId for Ardu SITL by default is 1*

6. Delete everything after ArduCopter creation
7. Add code:
```c#
var called = 0;
arduCopter.Heartbeat.RawHeartbeat.Subscribe(p =>
{
    if (p is null)
    {
        return;
    }
    
    Console.WriteLine($"Heartbeat type: {p.Type}, Heartbeat baseMode: {p.BaseMode}");
    
    if (called != 20)
    {
        called++;
        return;
    }
    
    tcs.TrySetResult(p);
});

await tcs.Task;
```
This code catches first 20 heartbeat packages

### Proper Output

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
Heartbeat type: MavTypeQuadrotor, Heartbeat baseMode: 81

Process finished with exit code 0.
```

### Full code
```c#
using System.Collections.Immutable;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink;
using Asv.Mavlink.Minimal;
using R3;

var tcs = new TaskCompletionSource<HeartbeatPayload>();
var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
cancel.Token.Register(() => tcs.TrySetCanceled());

var protocol = Protocol.Create(builder =>
{
    builder.RegisterMavlinkV2Protocol();
    builder.Features.RegisterBroadcastFeature<MavlinkMessage>();
    builder.Formatters.RegisterSimpleFormatter();
});
        
var router = protocol.CreateRouter("router");

router.AddTcpClientPort(p =>
{
    p.Host = "127.0.0.1";
    p.Port = 5760;
});
        
var identity = new MavlinkClientIdentity(2,3,1,1);

var clientSeq = new PacketSequenceCalculator();
var clientCore = new CoreServices(
    router, 
    clientSeq, 
    null, 
    null, 
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

var called = 0;
arduCopter.Heartbeat.RawHeartbeat.Subscribe(p =>
{
    if (p is null)
    {
        return;
    }
    
    Console.WriteLine($"Heartbeat type: {p.Type}, Heartbeat baseMode: {p.BaseMode}");
    
    if (called != 20)
    {
        called++;
        return;
    }
    
    tcs.TrySetResult(p);
});

await tcs.Task;
```
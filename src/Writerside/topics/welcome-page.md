# 👋 Welcome to Asv.Mavlink

## Introduction

The [`asv-mavlink`](https://github.com/asv-soft/asv-mavlink) library provides a robust interface for communication with MAVLink compatible vehicles and payloads. This library is designed to facilitate the interaction with drones and other devices using the MAVLink protocol, enabling users to send commands, receive telemetry data, and perform various operations.

Additionally, the library includes an CLI utility [Asv.Mavlink.Shell](https://github.com/asv-soft/asv-mavlink/tree/main/src/Asv.Mavlink.Shell) for simulating, testing and code generation.

## Installation

To install the [`asv-mavlink`](https://github.com/asv-soft/asv-mavlink) library, you can use the following command:

```
dotnet add package Asv.Mavlink --version <Version>
```

## Device addresses and source attribution

MAVLink client factories expose a common `DeviceAddress` for every MAVLink message,
including telemetry that does not participate in discovery. Its family is `mavlink`
and its value is the invariant `SystemId:ComponentId` pair from the message header,
for example `new DeviceAddress("mavlink", "1:1")`. A `MavlinkClientDeviceId` produces
the same address from `Id.Target`; its local `Id.Self`, device class, protocol
version, port and endpoint do not affect the address.

Use the `IDeviceExplorer` address methods to extract addresses before discovery,
for example when inspecting a capture:

```c#
if (explorer.TryGetSourceAddress(message, out var address))
{
    Console.WriteLine($"{address.Family}: {address.Value}");
}
```

`TryGetDeviceAddress(deviceId, out address)` also works without a live device.
Both extraction methods remain available after the explorer is disposed.
`TryIdentify(message, out deviceId)` resolves only a currently discovered device:
it returns `false` and `null` for unknown, unsupported or removed senders. It does
not create devices or refresh their timeout. Discovery remains heartbeat based.
The same remote system/component pair received over several channels belongs to
one live device. These methods attribute the **source** of a message; payload
destination fields such as `TargetSystem` and `TargetComponent` are not used to
resolve its sender.


![linkedin](https://github.com/user-attachments/assets/4fa5221e-7ae5-4b6b-98a8-1c1e39b49afb)

# 🧊 Asv.Mavlink

## Introduction

The [`asv-mavlink`](https://github.com/asv-soft/asv-mavlink) library provides a robust interface for communication with MAVLink compatible vehicles and payloads. This library is designed to facilitate the interaction with drones and other devices using the MAVLink protocol, enabling users to send commands, receive telemetry data, and perform various operations.

Additionally, the library includes a CLI utility [Asv.Mavlink.Shell](https://github.com/asv-soft/asv-mavlink/tree/main/src/Asv.Mavlink.Shell) for simulating, testing and code generation.

This library is part of the open-source cross-platform application for drones [Asv Drones](https://github.com/asv-soft/asv-drones).

## Installation

To install the [`asv-mavlink`](https://github.com/asv-soft/asv-mavlink) library, you can use the following command:

```
dotnet add package Asv.Mavlink --version <Version>
```

## Documentation

Documentation can be found [here](https://docs.asv.me/libraries/asv-mavlink)

## Example: Emulate ADSB reciever

This command starts a virtual ADS-B receiver that sends [ADSB\_VEHICLE](https://mavlink.io/en/messages/common.html#ADSB\_VEHICLE) packets at a specified rate for every vehicle defined in the configuration file.
```bash
Asv.Mavlink.Shell.exe adsb --cfg=adsb.json
```

![image](https://github.com/user-attachments/assets/6bbaa4b6-5e8e-4f11-b6c2-2a9e36944339)

Here's an example of ADSB utility being used with [Asv.Drones](https://github.com/asv-soft/asv-drones).

![image](https://github.com/user-attachments/assets/2cdfd705-9ccf-47c4-93c4-531f9b4a44b7)

Here's an example of ADSB utility being used with [Mission Planner](https://ardupilot.org/planner/)

![image](https://github.com/user-attachments/assets/334b2422-1b25-41d5-852d-0b8b76cab6d8)


## Example: Packet code generation

Generate C# code for packet serialization\deserialization

```bash
Asv.Mavlink.Shell.exe gen -t=[mavlink-xml-file] -i=[mavlink-xml-folder] -o=[output-folder] -e=cs [path-to-liquid-template]/csharp.tpl
```
```cs
   /// <summary>
    ///  HEARTBEAT
    /// </summary>
    public class HeartbeatPayload : IPayload
    {
        public byte GetMaxByteSize() => 9; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 9; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //CustomMode
            sum+= 1; // Type
            sum+= 1; // Autopilot
            sum+= 1; // BaseMode
            sum+= 1; // SystemStatus
            sum+=1; //MavlinkVersion
            return (byte)sum;
        }
        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            CustomMode = BinSerialize.ReadUInt(ref buffer);
            Type = (MavType)BinSerialize.ReadByte(ref buffer);
            Autopilot = (MavAutopilot)BinSerialize.ReadByte(ref buffer);
            BaseMode = (MavModeFlag)BinSerialize.ReadByte(ref buffer);
            SystemStatus = (MavState)BinSerialize.ReadByte(ref buffer);
            MavlinkVersion = (byte)BinSerialize.ReadByte(ref buffer);

        }
        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,CustomMode);
            BinSerialize.WriteByte(ref buffer,(byte)Type);
            BinSerialize.WriteByte(ref buffer,(byte)Autopilot);
            BinSerialize.WriteByte(ref buffer,(byte)BaseMode);
            BinSerialize.WriteByte(ref buffer,(byte)SystemStatus);
            BinSerialize.WriteByte(ref buffer,(byte)MavlinkVersion);
            /* PayloadByteSize = 9 */;
        }
        ...
    }
```

## CLI: Devices info
This command shows info about the mavlink device and all other mavlink devices that are connected to it.

```bash
Asv.Mavlink.Shell.exe devices-info -cs "tcp://127.0.0.1:7341"
```

![image](https://github.com/asv-soft/asv-drones-docs/blob/main/.gitbook/assets/asv-drones-mavlink-devices-info-command.png?raw=true)


You may also use some parameters in the command to customise the output
```bash
Usage: devices-info [options...] [-h|--help] [--version]

Command that shows info about devices in the mavlink network

Options:
-cs|--connection-string <string>    The address of the connection to the mavlink device (Required)
-i|--iterations <uint?>             States how many iterations should the program work through (Default: null)
-dt|--devices-timeout <uint>        (in seconds) States the lifetime of a mavlink device that shows no Heartbeat (Default: 10)
-r|--refresh-rate <uint>            (in ms) States how fast should the console be refreshed (Default: 3000)
```

Full possible command with all the parameters
```bash
Asv.Mavlink.Shell.exe devices-info -cs "tcp://127.0.0.1:7341" -i 400 -dt 20 -r 1000
```

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


## CLI: Ftp tree

This command provides a tree representation of all available files and directories on the drone's FTP server. It allows users to see the entire file structure in a hierarchical format, making it easy to browse and understand the file layout without navigating through individual folders.

```bash
Asv.Mavlink.Shell.exe ftp-tree -cs tcp://127.0.0.1:5760
```

### Features:
- Display the full directory structure of the drone's file system in a tree format.
- Automatically refreshes and loads the / and @SYS directories.
- Displays directories and files with visual guides for better clarity.

You may also use some parameters in the command.
```bash
Usage: ftp-tree [options...] [-h|--help] [--version]

Tree representation of all available files and directories on the drones FTP server

Options:
-cs|--connection <string>    The address of the connection to the mavlink device (Required)
````

![image](https://github.com/asv-soft/asv-drones-docs/blob/main/.gitbook/assets/asv-drones-mavlink-ftp-treecommand.png?raw=true)


## CLI: Ftp browser

This command is a file manager for interacting with a drone's file system via FTP. It allows users to browse directories, view files, and perform various file operations (e.g., download, rename, remove, etc.) in an interactive console environment. The tool is designed for MAVLink-based systems and provides an intuitive way to manage the drone’s files and directories.

```bash
Asv.Mavlink.Shell.exe ftp-browser -cs tcp://127.0.0.1:5760
```
### Features:
- FTP Connection: The command connects to a drone via TCP using a specified connection string, establishing an FTP client for file interactions.
- Tree Navigation: The file system is presented in a hierarchical structure using a tree model. The user can browse through directories interactively.
- File and Directory Operations: The user can:
    - Open directories.
    - Remove, rename, or create directories.
    - Perform file operations such as downloading, removing, truncating, renaming, and calculating CRC32.

```bash
Usage: ftp-browser [options...] [-h|--help] [--version]

File manager for interacting with a drones file system via FTP

Options:
  -cs|--connection <string>    The address of the connection to the mavlink device (Required)
````

![image](https://github.com/asv-soft/asv-drones-docs/blob/main/.gitbook/assets/asv-drones-mavlink-ftp-browser-command.png?raw=true)


## CLI: Export sdr data

This command extracts SDR (Software Defined Radio) data from a binary file and exports it into a CSV format. The SDR data is deserialized using the AsvSdrRecordDataLlzPayload class, and each record is written as a row in the CSV file with specific data fields such as altitude, signal strength, and power levels.

### Features:

- Reads binary SDR data from an input file.
- Exports the data to a CSV file for further analysis or storage.
- Provides a simple and automated way to convert SDR logs into human-readable tabular data.
```bash
Asv.Mavlink.Shell.exe export-sdr
```

You may also use some parameters in the command.
```bash
Usage: export-sdr [options...] [-h|--help] [--version]

Export sdt data to csv format

Options:
-i|--input-file <string>     Input file (Required)
-o|--output-file <string>    Output file (Default: @"out.csv")
```


## CLI: Mavlink

This command listens to incoming MAVLink packets and displays statistics on the received messages. It allows monitoring of the communication between a ground station and an unmanned vehicle, showing information like the frequency of each type of message and the last few received packets.

### Features:

- Connects to a MAVLink stream via the provided connection string.
- Displays statistics such as message ID, message frequency, and the last received packets
- Continually updates the display with real-time data and allows the user to stop the process by pressing 'Q'.

```bash
Asv.Mavlink.Shell.exe mavlink
```

You may also use some parameters in the command.
```bash
Usage: mavlink [options...] [-h|--help] [--version]

Listen MAVLink packages and print statistic

Options:
  -cs|--connection <string>    Connection string. Default "tcp://127.0.0.1:5760" (Default: null)
```

![image](https://github.com/asv-soft/asv-drones-docs/blob/main/.gitbook/assets/asv-drones-mavlink-mavlink-command.png?raw=true)



## CLI: Proxy

This command is used to connect a vehicle with multiple ground stations, creating a hub that routes MAVLink messages between them. It provides flexible filtering options to log specific MAVLink messages, and can output the filtered data to a file. It supports multiple connections (UDP or serial) and can operate in silent mode (without printing to the console).
### Features:

- Connects to multiple MAVLink streams, allowing you to route messages between different systems (e.g., vehicle and multiple ground stations).
- Supports filtering by system ID, message ID, message name (using regex), and message content (JSON text).
- Can log filtered MAVLink messages to a file.
- Allows disabling console output for silent operation.
- Automatically propagates MAVLink messages between the connected links.

```bash
Asv.Mavlink.Shell.exe proxy -l tcp://127.0.0.1:5762 -l tcp://127.0.0.1:7341 -o out.txt
```

You may also use some parameters in the command.
```bash
Usage: proxy [options...] [-h|--help] [--version]

Used for connecting vehicle and several ground station
     Example: proxy -l udp://192.168.0.140:14560 -l udp://192.168.0.140:14550 -o out.txt

Options:
  -l|--links <string[]>            Add connection to hub. Can be used multiple times. Example: udp://192.168.0.140:45560 or serial://COM5?br=57600 (Required)
  -o|--output-file <string>        Write filtered message to file (Default: null)
  -silent|--silent                 Disable print filtered message to screen (Optional)
  -sys|--sys-ids <int[]>           Filter for logging: system id field (Example: -sys 1 -sys 255) (Default: null)
  -id|--msg-ids <int[]>            Filter for logging: message id field (Example: -id 1 -mid 255) (Default: null)
  -name|--name-pattern <string>    Filter for logging: regex message name filter (Example: -name MAV_CMD_D) (Default: null)
  -txt|--text-pattern <string>     Filter for logging: regex json text filter (Example: -txt MAV_CMD_D) (Default: null)
  -from|--directions <int[]>       Filter for packet direction: select only input packets from the specified direction (Default: null)
```

## CLI: Benchmark-serialization

This command benchmarks the serialization and deserialization performance of MAVLink packets. It uses BenchmarkDotNet to measure the efficiency of the serialization process, focusing on how MAVLink packets are serialized and deserialized using spans.### Features:

- Connects to multiple MAVLink streams, allowing you to route messages between different systems (e.g., vehicle and multiple ground stations).
- Supports filtering by system ID, message ID, message name (using regex), and message content (JSON text).
- Can log filtered MAVLink messages to a file.
- Allows disabling console output for silent operation.
- Automatically propagates MAVLink messages between the connected links.

```bash
Asv.Mavlink.Shell.exe benchmark-serialization
```

![image](https://github.com/asv-soft/asv-drones-docs/blob/main/.gitbook/assets/asv-drones-mavlink-benchmark-serialization-command.png?raw=true)

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
## CLI: Packet Viewer
```bash
Asv.Mavlink.Shell.exe packetviewer --connection tcp://127.0.0.1:5762
```
This command starts the console implementation of packet viewer.

![image](https://github.com/asv-soft/asv-drones-docs/blob/main/.gitbook/assets/asv-drones-mavlink-packets.png?raw=true)

Packet Viewer sets up the Mavlink router and waits for a connection using parameters provided in the command line.
Launch a real drone or simulator to connect and start receiving packets from it. Once the connection is established, the packets will be displayed in the "Packets" section below.

It provides the following actions:
 - Search for the packet you need;
 - Adjust the size of the output;
 - Pause the output;
 - Safely terminate the execution;

By default, the viewer has no filters enabled and displays all received packets.

## CLI: Generate fake diagnostic data
This command generates fake diagnostic with customizable frequency.

```bash
Asv.Mavlink.Shell.exe generate-diagnostics
```

![image](https://github.com/asv-soft/asv-drones-docs/blob/main/.gitbook/assets/screenshot-generate-diag.png?raw=true)

The program generates a default configuration file by default, but you can provide a custom configuration. 
Simply pass the path to your configuration file as a command-line parameter.

*Note: config is a json file.*

```bash
Asv.Mavlink.Shell.exe generate-diagnostics -cfg "path/to/your/cfg.json"
```

All the possible parameters for the command:
```bash
Usage: generate-diagnostics [options...] [-h|--help] [--version]

Command creates fake diagnostics data from file and opens a mavlink connection.

Options:
  -cfg|--cfg-path <string?>    location of the config file for the generator (Default: null)
  -r|--refresh-rate <uint>     (in ms) States how fast should the console be refreshed (Default: 2000)
```

Full command with all the parameters
```bash
Asv.Mavlink.Shell.exe generate-diagnostics -cfg "path/to/your/cfg.json" -r 3000
```

## CLI: Test diagnostic data
This command creates Diagnostic client and prints all diagnostics that the client retrieves.

```bash
Asv.Mavlink.Shell.exe test-diagnostics -cs tcp://127.0.0.1:7342?srv=true -tsid 1 -tcid 241 -r 3000
```
![image](https://github.com/asv-soft/asv-drones-docs/blob/main/.gitbook/assets/screenshot-test-diag.png?raw=true)

All the possible parameters for the command:
```bash
Command creates diagnostic client that retrieves diagnostic data.

Options:
  -cs|--connection-string <string>      The address of the connection to the mavlink diagnostic server (Required)
  -tsid|--target-system-id <byte>       Server's system id (Required)
  -tcid|--target-component-id <byte>    Server's component id (Required)
  -r|--refresh-rate <uint>              (in ms) States how fast should the console be refreshed (Default: 1000)

```

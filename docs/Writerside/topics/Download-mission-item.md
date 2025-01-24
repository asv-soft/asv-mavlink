# Download mission item

This command is used to download mission items from an unmanned aerial vehicle (UAV). It provides options to cancel the operation and handles exceptions that may occur during the download process.

## Features:

- Downloads mission items from a UAV.
- Supports operation cancellation using a `CancellationToken`.
- Handles exceptions that may occur during the download process.

```bash
Asv.Mavlink.Shell.exe show-mission -cs "tcp://127.0.0.1:7341"
```

![image](asv-drones-mavlink-download-mission-item.png)

You may also use some parameters in the command.

```bash
Usage: devices-info [options...] [-h|--help] [--version]

Command that shows info about devices in the mavlink network

Options:
-cs|--connection-string <string>    The address of the connection to the mavlink device (Required)
-i|--iterations <uint?>             States how many iterations should the program work through (Default: null)
-r|--refresh-rate <uint>            (in ms) States how fast should the console be refreshed (Default: 9000)
```

Exceptions:
- `OperationCanceledException`: Thrown if the operation was canceled.
- `Exception`: Thrown in case of an error during the mission items download.

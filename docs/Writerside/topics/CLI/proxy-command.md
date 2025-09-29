# Proxy

This command is used to connect a vehicle with multiple ground stations, creating a hub that routes MAVLink messages between them. It provides flexible filtering options to log specific MAVLink messages, and can output the filtered data to a file. It supports multiple connections (UDP or serial) and can operate in silent mode (without printing to the console).
## Features:

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
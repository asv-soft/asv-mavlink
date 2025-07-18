# Proxy

This command is used to connect a vehicle with multiple ground stations, creating a hub that routes MAVLink messages between them. 
It provides flexible filtering options to log specific MAVLink messages, and can output the filtered data to a file. 
It supports multiple connections (UDP or serial) and can operate in silent mode (without printing to the console).
## Features:

- Connects to multiple MAVLink streams, allowing you to route messages between different systems (e.g., vehicle and multiple ground stations).
- Supports filtering by system ID, message ID, message name (using regex), and message content (JSON text).
- Can log filtered MAVLink messages to a file.
- Allows disabling console output for silent operation.
- Automatically propagates MAVLink messages between the connected links.

```bash
Asv.Mavlink.Shell.exe proxy -l tcp://127.0.0.1:5762,tcp://127.0.0.1:7341 -o out.txt
```

You may also use some parameters in the command.
```bash
Usage: proxy [options...] [-h|--help] [--version]

Used for connecting vehicle and several ground stations
     Example: proxy -l udp://192.168.0.140:14560,udp://192.168.0.140:14550 -o out.txt

Options:
  -l|--links <string[]>            Add connection to hub. Can be used multiple times. Example: udp://192.168.0.140:45560 or serial://COM5?br=57600 (Required)
  -o|--output <string?>            Write filtered messages to file (Default: null)
  -s|--silent                      Disable printing filtered messages to the screen (Optional)
  -s-ids|--sys-ids <int[]?>        Filter for logging: system id field (Example: -s-ids 1 -s-ids 255) (Default: null)
  -m-ids|--msg-ids <int[]?>        Filter for logging: message id field (Example: -m-ids 1 -m-ids 255) (Default: null)
  -n-p|--name-pattern <string?>    Filter for logging: regex message name filter (Example: -n-p MAV_CMD_D) (Default: null)
  -t-p|--text-pattern <string?>    Filter for logging: regex json text filter (Example: -t-p MAV_CMD_D) (Default: null)

```
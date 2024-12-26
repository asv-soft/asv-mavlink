# Devices info
This command shows info about the mavlink device and all other mavlink devices that are connected to it.

```bash
Asv.Mavlink.Shell.exe devices-info -cs "tcp://127.0.0.1:7341"
```

![image](asv-drones-mavlink-devices-info-command.png)


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
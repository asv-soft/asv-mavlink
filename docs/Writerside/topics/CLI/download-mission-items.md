# Download mission items

This command is used to download mission items from an unmanned aerial vehicle (UAV). 
It provides options to cancel the operation and handles exceptions that may occur during the download process.

```bash
Asv.Mavlink.Shell.exe show-mission -cs tcp://127.0.0.1:7341
```

![image](asv-drones-mavlink-download-mission-items.png)

You may also use some parameters in the command.

```bash
Usage: show-mission [options...] [-h|--help] [--version]

Command that allows you to select a device and watch its mission items

Options:
  -cs|--connection-string <string>    The address of the connection to the mavlink device (Required)
  -i|--iterations <uint?>             States how many iterations should the program work through (Default: null)
  -r|--refresh-rate <uint>            (in ms) States how fast should the console be refreshed (Default: 3000)
```

Full possible command with all the parameters
```bash
Asv.Mavlink.Shell.exe show-mission -cs  tcp://127.0.0.1:5760 -r 1000 -i 20
```
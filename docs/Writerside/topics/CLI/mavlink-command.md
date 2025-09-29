# Mavlink

This command listens to incoming MAVLink packets and displays statistics on the received messages. It allows monitoring of the communication between a ground station and an unmanned vehicle, showing information like the frequency of each type of message and the last few received packets.

## Features:

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

![image](asv-drones-mavlink-mavlink-command.png)

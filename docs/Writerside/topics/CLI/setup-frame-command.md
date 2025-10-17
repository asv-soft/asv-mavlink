# Setup frame

This command allows you to view and change the frame type on supported MAVLink devices (ArduCopter and ArduPlane - QuadPlane).

The command displays available frame configurations for the connected device and allows you to select a different frame type interactively.

```bash
Asv.Mavlink.Shell.exe setup-frame -cs tcp://127.0.0.1:7341
```

## Usage

```bash
Usage: setup-frame [options...] [-h|--help] [--version]

Select device frame type

Options:
  -cs|--connection <string>    The address of the connection to the mavlink device, e.g. tcp://127.0.0.1:5760 (Required)
```

> The command will show only frame configurations that are compatible with the connected device.
{style="note"}
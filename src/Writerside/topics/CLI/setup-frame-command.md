# Setup frame

This command lets you view and change the motor frame type on supported MAVLink devices (ArduCopter and ArduPlane — QuadPlane). It uses the Frame client (`IFrameClient`) to list supported frames and apply your selection.

```bash
Asv.Mavlink.Shell.exe setup-frame -cs tcp://127.0.0.1:7341
```

## Usage

```bash
Usage: setup-frame [options...] [-h|--help] [--version]

Select the device frame type

Options:
  -cs|--connection <string>    The address of the connection to the MAVLink device, e.g., tcp://127.0.0.1:5760 (Required)
```

> The command shows only frame configurations that are compatible with the connected device.
{style="note"}

## Examples

- TCP:
```bash
Asv.Mavlink.Shell.exe setup-frame -cs tcp://127.0.0.1:5760
```

- UDP:
```bash
Asv.Mavlink.Shell.exe setup-frame -cs udp://127.0.0.1:14550
```

## Notes

- On some ArduPilot setups, applying a new frame may require a reboot for the change to take full effect. Consult your autopilot documentation.